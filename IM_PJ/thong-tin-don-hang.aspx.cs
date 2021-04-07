using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Utils;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static IM_PJ.Controllers.DiscountCustomerController;
using static IM_PJ.pos;

namespace IM_PJ
{
    public partial class thong_tin_don_hang : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 600;

            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);
                    if (acc != null)
                    {

                        hdSession.Value = "1";

                        var agent = acc.AgentID;
                        if (agent == 1)
                        {
                            hdfIsMain.Value = "1";
                        }
                        else
                        {
                            hdfIsMain.Value = "0";
                        }

                        if (acc.RoleID == 0)
                        {
                            hdfUsernameCurrent.Value = acc.Username;
                            LoadCreatedBy();
                        }
                        else if (acc.RoleID == 2)
                        {
                            hdfUsername.Value = acc.Username;
                            hdfUsernameCurrent.Value = acc.Username;
                            LoadCreatedBy(acc);
                        }
                        else
                        {
                            Response.Redirect("/trang-chu");
                        }
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
                LoadTransportCompany();
                LoadData();
            }
        }

        #region Private
        /// <summary>
        /// Cài đặt thông tin địa chỉ nhận hàng
        /// </summary>
        /// <param name="deliveryAddressId"></param>
        private void _initDeliveryAddress(long? deliveryAddressId)
        {
            if (deliveryAddressId.HasValue)
                hdfDeliveryAddressId.Value = deliveryAddressId.Value.ToString();
        }
        #endregion

        public void LoadCreatedBy(tbl_Account acc = null)
        {
            if (acc != null)
            {
                ddlCreatedBy.Items.Clear();
                ddlCreatedBy.Items.Insert(0, new ListItem(acc.Username, acc.Username));
            }
            else
            {
                var CreateBy = AccountController.GetAllNotSearch();
                ddlCreatedBy.Items.Clear();
                if (CreateBy.Count > 0)
                {
                    foreach (var p in CreateBy)
                    {
                        if (p.RoleID == 2 || p.RoleID == 0)
                        {
                            ListItem listitem = new ListItem(p.Username, p.Username);
                            ddlCreatedBy.Items.Add(listitem);
                        }
                    }
                    ddlCreatedBy.DataBind();
                }
            }
        }
        public void LoadData()
        {
            int n;
            if (String.IsNullOrEmpty(Request.QueryString["id"]) || !int.TryParse(Request.QueryString["id"], out n))
            {
                PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy đơn hàng", "e", true, "/danh-sach-don-hang", Page);
            }

            int ID = Request.QueryString["id"].ToInt(0);
            if (ID > 0)
            {
                var order = OrderController.GetByID(ID);
                if (order == null)
                {
                    PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy đơn hàng " + ID, "e", true, "/danh-sach-don-hang", Page);
                }
                else
                {
                    // chuyển sang giao diện xem đơn chuyển hoàn nếu trạng thái xử lý đã chuyển hoàn
                    if (order.ExcuteStatus == 4)
                    {
                        Response.Redirect("/thong-tin-don-hang-chuyen-hoan?id=" + ID);
                    }
                    hdfUsername.Value = order.CreatedBy;
                    ddlCreatedBy.SelectedValue = order.CreatedBy.ToString();

                    // Init drop down list ddlFeeType
                    var feeTypes = FeeTypeController.getDropDownList();
                    feeTypes[0].Text = "Loại Phí";
                    ddlFeeType.Items.Clear();
                    ddlFeeType.Items.AddRange(feeTypes.ToArray());
                    ddlFeeType.DataBind();
                    ddlFeeType.SelectedIndex = 0;

                    // Init drop down list Bank
                    var banks = BankController.getDropDownList();
                    banks[0].Text = "Chọn ngân hàng";

                    ddlBank.Items.Clear();
                    ddlBank.Items.AddRange(banks.ToArray());
                    ddlBank.DataBind();
                    int bankOfOrder = BankTransferController.getBankOfOrder(ID);
                    if(bankOfOrder != 0)
                    {
                        ddlBank.SelectedValue = bankOfOrder.ToString();
                    }

                    // Init Price Type List
                    hdfFeeType.Value = FeeTypeController.getFeeTypeJSON();

                    // Init shipping type
                    hdfShippingType.Value = order.ShippingType.ToString();

                    // hidden TransportCompanySubID
                    hdfTransportCompanySubID.Value = order.TransportCompanySubID.ToString();

                    string username = HttpContext.Current.Request.Cookies["usernameLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);
                    if(acc.RoleID != 0)
                    {
                        if (order.CreatedBy != acc.Username)
                        {
                            Response.Redirect("/danh-sach-don-hang");
                        }
                    }

                    // get default discount list
                    var dc = DiscountController.GetAll();
                    if (dc != null)
                    {
                        string list = "";
                        foreach (var item in dc)
                        {
                            list += item.Quantity + "-" + item.DiscountPerProduct + "|";
                        }
                        hdfChietKhau.Value = list;
                    }

                    // hidden OrderID
                    hdOrderInfoID.Value = ID.ToString();



                    int AgentID = Convert.ToInt32(order.AgentID);
                    txtPhone.Text = order.CustomerPhone;
                    txtFullname.Text = order.CustomerName.ToLower().ToTitleCase();
                    txtAddress.Text = order.CustomerAddress.ToTitleCase();
                    var cus = CustomerController.GetByID(order.CustomerID.Value);
                    if (cus != null)
                    {
                        txtNick.Text = cus.Nick;
                        if (string.IsNullOrEmpty(cus.Nick))
                        {
                            txtNick.Enabled = true;
                        }

                        txtFacebook.Text = cus.Facebook;
                        if (!string.IsNullOrEmpty(cus.Facebook))
                        {
                            ltrFb.Text += "<a href='" + cus.Facebook + "' class='btn primary-btn fw-btn not-fullwidth' target='_blank'>Xem</a>";
                        }
                        else
                        {
                            txtFacebook.Enabled = true;
                        }
                        hdfProvinceID.Value = cus.ProvinceID.ToString();
                        hdfDistrictID.Value = cus.DistrictId.ToString();
                        hdfWardID.Value = cus.WardId.ToString();
                    }

                    // Title
                    this.Title = String.Format("{0} - Đơn hàng", cus.Nick != "" ? cus.Nick.ToTitleCase() : cus.CustomerName.ToTitleCase());
                    ltrHeading.Text = "Đơn hàng " + ID.ToString() + " - " + (cus.Nick != "" ? cus.Nick.ToTitleCase() : cus.CustomerName.ToLower().ToTitleCase()) + (!String.IsNullOrEmpty(order.UserHelp) ? " (được tạo giúp bởi " + order.UserHelp + ")" : "");
                    ltrOrderID.Text = ID.ToString();
                    int customerID = Convert.ToInt32(order.CustomerID);
                    ltrViewDetail.Text = "<a href='javascript:;' class='btn primary-btn fw-btn not-fullwidth' onclick='viewCustomerDetail(`" + customerID + "`)'><i class='fa fa-address-card-o' aria-hidden='true'></i> Xem</a>";
                    ltrViewDetail.Text += "<a href='javascript:;' class='btn primary-btn fw-btn not-fullwidth edit-customer-btn' onclick='refreshCustomerInfo(`" + customerID + "`)'><i class='fa fa-refresh' aria-hidden='true'></i> Làm mới</a>";
                    ltrViewDetail.Text += "<a href='chi-tiet-khach-hang?id=" + customerID + "' class='btn primary-btn fw-btn not-fullwidth edit-customer-btn' target='_blank'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Sửa</a>";
                    ltrViewDetail.Text += "<a href='danh-sach-don-hang?searchtype=1&textsearch=" + order.CustomerPhone + "' class='btn primary-btn fw-btn not-fullwidth edit-customer-btn' target='_blank'><i class='fa fa-history' aria-hidden='true'></i> Lịch sử</a>";
                    ltrViewDetail.Text += "<a href='javascript:;' class='btn primary-btn fw-btn not-fullwidth clear-btn' onclick='clearCustomerDetail()'><i class='fa fa-times' aria-hidden='true'></i> Bỏ</a>";
                    if (UserController.checkExists(customerID))
                    {
                        ltrViewDetail.Text += "    <br><br><strong class='font-green'>Đã đăng ký App</strong>";
                    }
                    else
                    {
                        ltrViewDetail.Text += "    <br><br><strong class='font-red'>Chưa đăng ký App</strong>";
                    }
                    var d = DiscountCustomerController.getbyCustID(customerID);
                    if (d.Count > 0)
                    {
                        var da = d[0].DiscountAmount;
                        hdfIsDiscount.Value = "1";
                        hdfDiscountAmount.Value = da.ToString();
                        hdfQuantityRequirement.Value = d[0].QuantityProduct.ToString();
                        ltrDiscountInfo.Text = "<strong>* Chiết khấu của khách: " + string.Format("{0:N0}", Convert.ToDouble(da)) + "/cái (đơn từ " + d[0].QuantityProduct.ToString() + " cái).</strong>";
                    }
                    else
                    {
                        hdfIsDiscount.Value = "0";
                        hdfDiscountAmount.Value = "0";
                        hdfQuantityRequirement.Value = "0";
                    }
                    int customerType = Convert.ToInt32(order.OrderType);
                    ltrCustomerType.Text = "";
                    ltrCustomerType.Text += "<select class='form-control customer-type' onchange='getProductPrice($(this))'>";
                    if (customerType == 1)
                    {
                        ltrCustomerType.Text += "<option value='2'>Khách mua sỉ</option>";
                        ltrCustomerType.Text += "<option value='1' selected>Khách mua lẻ</option>";

                    }
                    else
                    {
                        ltrCustomerType.Text += "<option value='2' selected>Khách mua sỉ</option>";
                        ltrCustomerType.Text += "<option value='1'>Khách mua lẻ</option>";

                    }
                    ltrCustomerType.Text += "</select>";


                    double ProductQuantity = 0;
                    double totalPrice = Convert.ToDouble(order.TotalPrice);
                    double totalPriceNotDiscount = Convert.ToDouble(order.TotalPriceNotDiscount);

                    hdfcheckR.Value = "";

                    int totalrefund = 0;
                    if (order.RefundsGoodsID > 0)
                    {
                        var re = RefundGoodController.GetByID(order.RefundsGoodsID.Value);
                        if (re != null)
                        {
                            totalrefund = Convert.ToInt32(re.TotalPrice);
                            hdfcheckR.Value = order.RefundsGoodsID.ToString() + "," + re.TotalPrice;
                        }

                        ltrtotalpricedetail.Text = string.Format("{0:N0}", totalPrice - totalrefund);

                        ltrTotalPriceRefund.Text = string.Format("-{0:N0}", totalrefund);
                    }

                    hdfDiscountInOrder.Value = "";

                    if (order.DiscountPerProduct > 0)
                    {
                        hdfDiscountInOrder.Value = order.DiscountPerProduct.ToString();
                    }

                    int paymentStatus = Convert.ToInt32(order.PaymentStatus);
                    int excuteStatus = Convert.ToInt32(order.ExcuteStatus);
                    int shipping = Convert.ToInt32(order.ShippingType);
                    int TransportCompanyID = Convert.ToInt32(order.TransportCompanyID);
                    int TransportCompanySubID = Convert.ToInt32(order.TransportCompanySubID);
                    int PostalDeliveryType = Convert.ToInt32(order.PostalDeliveryType);
                    int paymenttype = Convert.ToInt32(order.PaymentType);

                    #region Lấy ghi chú đơn hàng cũ

                    var oldOrders = OrderController.GetAllNoteByCustomerID(customerID, ID);
                    if (oldOrders.Count() > 1)
                    {
                        StringBuilder notestring = new StringBuilder();
                        foreach (var item in oldOrders)
                        {
                            notestring.AppendLine(String.Format("<li>Đơn <strong><a href='/thong-tin-don-hang?id={0}' target='_blank'>{0}</a></strong> (<em>{1}</em>): {2}</li>", item.ID, item.DateDone, item.OrderNote));
                        }

                        StringBuilder notehtml = new StringBuilder();
                        notehtml.AppendLine("<div id='old-order-note' class='row'>");
                        notehtml.AppendLine("    <div class='col-md-12'>");
                        notehtml.AppendLine("        <div class='panel panelborderheading'>");
                        notehtml.AppendLine("            <div class='panel-heading clear'>");
                        notehtml.AppendLine("                <h3 class='page-title left not-margin-bot'>" + oldOrders.Count() + " đơn hàng cũ gần nhất có ghi chú:</h3>");
                        notehtml.AppendLine("            </div>");
                        notehtml.AppendLine("            <div class='panel-body'>");
                        notehtml.AppendLine("                <div class='row'>");
                        notehtml.AppendLine("                    <div class='col-sm-12'>");
                        notehtml.AppendLine("                        <ul>");
                        notehtml.AppendLine(String.Format("{0}", notestring));
                        notehtml.AppendLine("                        </ul>");
                        notehtml.AppendLine("                    </div>");
                        notehtml.AppendLine("                </div>");
                        notehtml.AppendLine("            </div>");
                        notehtml.AppendLine("        </div>");
                        notehtml.AppendLine("    </div>");
                        notehtml.AppendLine("</div>");

                        ltrOldOrderNote.Text = notehtml.ToString();
                    }

                    #endregion

                    #region Lấy danh sách sản phẩm

                    var orderdetails = OrderDetailController.GetByOrderID(ID);

                    if (excuteStatus == (int)ExcuteStatus.Doing)
                        orderdetails.Reverse();

                    StringBuilder html = new StringBuilder();
                    if (orderdetails.Count > 0)
                    {
                        int orderitem = 0;
                        foreach (var item in orderdetails)
                        {
                            ProductQuantity += Convert.ToDouble(item.Quantity);

                            int ProductType = Convert.ToInt32(item.ProductType);
                            double ItemPrice = Convert.ToDouble(item.Price);
                            string SKU = item.SKU;
                            double Giacu = 0;
                            double Giabansi = 0;
                            double Giabanle = 0;
                            string stringGiabansi = "";
                            string stringGiabanle = "";
                            double QuantityInstock = 0;
                            string ProductImageOrigin = "";
                            string ProductVariable = "";
                            string ProductName = "";
                            string ProductVariableName = "";
                            string ProductVariableValue = "";
                            string ProductVariableSave = "";
                            double QuantityMainInstock = 0;
                            string ProductImage = "";
                            string QuantityMainInstockString = "";
                            string QuantityInstockString = "";
                            string productVariableDescription = item.ProductVariableDescrition;

                            if (ProductType == 1)
                            {
                                var product = ProductController.GetBySKU(SKU);
                                if (product != null)
                                {
                                    double mainstock = PJUtils.TotalProductQuantityInstock(1, SKU);

                                    if (customerType == 1)
                                    {
                                        Giacu = 0;
                                        Giabansi = Convert.ToDouble(product.Regular_Price);
                                        Giabanle = ItemPrice;
                                    }
                                    else
                                    {
                                        Giacu = Convert.ToDouble(product.Old_Price);
                                        Giabansi = ItemPrice;
                                        Giabanle = Convert.ToDouble(product.Retail_Price);
                                    }
                                    stringGiabansi = string.Format("{0:N0}", Giabansi);
                                    stringGiabanle = string.Format("{0:N0}", Giabanle);
                                    string variablename = "";
                                    string variablevalue = "";
                                    string variable = "";

                                    QuantityInstock = mainstock;
                                    QuantityInstockString = string.Format("{0:N0}", mainstock);
                                    ProductImage = "<img onclick='openImage($(this))' src='" + Thumbnail.getURL(product.ProductImage, Thumbnail.Size.Small) + "'>";
                                    ProductImageOrigin = Thumbnail.getURL(product.ProductImage, Thumbnail.Size.Source);
                                    ProductVariable = variable;
                                    ProductName = product.ProductTitle;
                                    QuantityMainInstock = mainstock;
                                    QuantityMainInstockString = string.Format("{0:N0}", mainstock);
                                    ProductVariableSave = item.ProductVariableDescrition;

                                    ProductVariableName = variablename;
                                    ProductVariableValue = variablevalue;
                                }
                            }
                            else
                            {
                                var productvariable = ProductVariableController.GetBySKU(SKU);
                                if (productvariable != null)
                                {
                                    SKU = productvariable.SKU.Trim().ToUpper();

                                    double mainstock = PJUtils.TotalProductQuantityInstock(1, SKU);

                                    if (customerType == 1)
                                    {
                                        Giabansi = Convert.ToDouble(productvariable.Regular_Price);
                                        Giabanle = ItemPrice;
                                    }
                                    else
                                    {
                                        Giabansi = ItemPrice;
                                        Giabanle = Convert.ToDouble(productvariable.RetailPrice);
                                    }
                                    stringGiabansi = string.Format("{0:N0}", Giabansi);
                                    stringGiabanle = string.Format("{0:N0}", Giabanle);


                                    string variablename = "";
                                    string variablevalue = "";
                                    string variable = "";

                                    string[] vs = productVariableDescription.Split('|');
                                    if (vs.Length - 1 > 0)
                                    {
                                        for (int i = 0; i < vs.Length - 1; i++)
                                        {
                                            string[] items = vs[i].Split(':');
                                            variable += items[0] + ":" + items[1] + "<br/>";
                                            variablename += items[0] + "|";
                                            variablevalue += items[1] + "|";
                                        }
                                    }

                                    QuantityInstock = mainstock;
                                    QuantityInstockString = string.Format("{0:N0}", mainstock);

                                    var _product = ProductController.GetByID(Convert.ToInt32(productvariable.ProductID));

                                    if (_product != null)
                                    {
                                        ProductName = _product.ProductTitle;

                                        if (customerType == 1)
                                        {
                                            Giacu = 0;
                                        }
                                        else
                                        {
                                            Giacu = Convert.ToDouble(_product.Old_Price);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(productvariable.Image))
                                    {
                                        ProductImage = "<img onclick='openImage($(this))' src='" + Thumbnail.getURL(productvariable.Image, Thumbnail.Size.Small) + "'>";
                                        ProductImageOrigin = Thumbnail.getURL(productvariable.Image, Thumbnail.Size.Source);
                                    }
                                    else if (_product != null && !string.IsNullOrEmpty(_product.ProductImage))
                                    {
                                        ProductImage = "<img onclick='openImage($(this))' src='" + Thumbnail.getURL(_product.ProductImage, Thumbnail.Size.Small) + "'>";
                                        ProductImageOrigin = Thumbnail.getURL(_product.ProductImage, Thumbnail.Size.Source);
                                    }

                                    ProductVariable = variable;

                                    QuantityMainInstock = mainstock;
                                    QuantityMainInstockString = string.Format("{0:N0}", mainstock);
                                    ProductVariableSave = item.ProductVariableDescrition;

                                    ProductVariableName = variablename;
                                    ProductVariableValue = variablevalue;
                                }
                            }

                            orderitem++;
                            int k = Convert.ToInt32(ItemPrice) * Convert.ToInt32(item.Quantity);

                            html.AppendLine(String.Format("<tr ondblclick='clickrow($(this))' class='product-result'"));
                            html.AppendLine(String.Format("        data-orderdetailid='{0}'", item.ID));
                            html.AppendLine(String.Format("        data-giabansi='{0}'", Giabansi));
                            html.AppendLine(String.Format("        data-giabanle='{0}'", Giabanle));
                            html.AppendLine(String.Format("        data-quantityinstock='{0}'", QuantityInstock));
                            html.AppendLine(String.Format("        data-productimageorigin='{0}'", ProductImageOrigin));
                            html.AppendLine(String.Format("        data-productvariable='{0}'", ProductVariable));
                            html.AppendLine(String.Format("        data-productname='{0}'", ProductName));
                            html.AppendLine(String.Format("        data-sku='{0}'", SKU));
                            html.AppendLine(String.Format("        data-producttype='{0}'", ProductType));
                            html.AppendLine(String.Format("        data-productid='{0}'", item.ProductID));
                            html.AppendLine(String.Format("        data-productvariableid='{0}'", item.ProductVariableID));
                            html.AppendLine(String.Format("        data-productvariablename='{0}'", ProductVariableName));
                            html.AppendLine(String.Format("        data-productvariablevalue ='{0}'", ProductVariableValue));
                            html.AppendLine(String.Format("        data-productvariablesave ='{0}'", ProductVariableSave));
                            html.AppendLine(String.Format("        data-quantitymaininstock='{0}'>", QuantityMainInstock));
                            html.AppendLine(String.Format("    <td class='order-item'>{0}</td>", excuteStatus == (int)ExcuteStatus.Doing ? (orderdetails.Count - orderitem + 1) : orderitem));
                            html.AppendLine(String.Format("    <td class='image-item'>{0}</td>", ProductImage));
                            html.AppendLine(String.Format("    <td class='name-item'>{0}</td>", "<a href='/xem-san-pham?id=" + item.ProductID + "&variableid=" + item.ProductVariableID + "' target='_blank'>" + (Giacu > 0 ? "<span class='sale-icon'>SALE</span> " : "") + ProductName + "</a>"));
                            html.AppendLine(String.Format("    <td class='sku-item'>{0}</td>", SKU));
                            html.AppendLine(String.Format("    <td class='variable-item'>{0}</td>", ProductVariable));
                            html.AppendLine(String.Format("    <td class='price-item gia-san-pham' data-price='{0}'>{0:N0}</td>", ItemPrice));
                            html.AppendLine(String.Format("    <td class='quantity-item soluong'>{0}</td>", QuantityInstockString));
                            html.AppendLine(String.Format("    <td class='quantity-item'>"));
                            html.AppendLine(String.Format("        <input type='text' class='form-control in-quantity'"));
                            html.AppendLine("                              pattern='[0-9]{1,3}'");
                            html.AppendLine(String.Format("                onblur='checkQuantiy($(this))'"));
                            html.AppendLine(String.Format("                onkeyup='pressKeyQuantity($(this))'"));
                            html.AppendLine(String.Format("                onkeypress='return event.charCode >= 48 && event.charCode <= 57'"));
                            html.AppendLine(String.Format("                value='{0}'/>", item.Quantity));
                            html.AppendLine(String.Format("    </td>"));
                            html.AppendLine(String.Format("    <td class='total-item totalprice-view'>{0:N0}</td>", k));
                            html.AppendLine(String.Format("   <td class='trash-item'><a href='javascript:;' class='link-btn' onclick='deleteRow($(this))'><i class='fa fa-trash'></i></a></td>"));
                            html.AppendLine(String.Format("</tr>"));
                        }

                        ltrProducts.Text = html.ToString();

                    }
                    #endregion
                    #region HiddenField

                    hdfCustomerID.Value = order.CustomerID.ToString();
                    hdfOrderType.Value = customerType.ToString();
                    hdfTotalPrice.Value = totalPrice.ToString();
                    hdfTotalPriceNotDiscount.Value = totalPriceNotDiscount.ToString();
                    hdfPaymentStatus.Value = paymentStatus.ToString();
                    hdfExcuteStatus.Value = excuteStatus.ToString();
                    hdftotal.Value = ProductQuantity.ToString();
                    hdfTotalQuantity.Value = ProductQuantity.ToString();
                    hdfRoleID.Value = acc.RoleID.ToString();

                    #endregion
                    if (excuteStatus == 2 && paymentStatus == 4)
                    {
                        ListItem listitem = new ListItem("Đã duyệt", "4");
                        ddlPaymentStatus.Items.Insert(3, listitem);
                    }
                    ddlPaymentStatus.SelectedValue = paymentStatus.ToString();
                    ddlExcuteStatus.SelectedValue = excuteStatus.ToString();
                    ddlPaymentType.SelectedValue = paymenttype.ToString();
                    ddlShippingType.SelectedValue = shipping.ToString();
                    ddlPostalDeliveryType.SelectedValue = PostalDeliveryType.ToString();

                    LoadTransportCompanySubID(TransportCompanyID);
                    ddlTransportCompanyID.SelectedValue = TransportCompanyID.ToString();
                    ddlTransportCompanySubID.SelectedValue = TransportCompanySubID.ToString();

                    txtShippingCode.Text = order.ShippingCode;
                    txtOrderNote.Text = order.OrderNote;

                    ltrProductQuantity.Text = string.Format("{0:N0}", ProductQuantity) + " cái";
                    ltrTotalNotDiscount.Text = string.Format("{0:N0}", Convert.ToDouble(order.TotalPriceNotDiscount));
                    ltrTotalprice.Text = string.Format("{0:N0}", Convert.ToDouble(order.TotalPrice));
                    pDiscount.Value = order.DiscountPerProduct;
                    pFeeShip.Value = Convert.ToDouble(order.FeeShipping);
                    txtWeight.Value = order.Weight.HasValue ? order.Weight.Value : 0;
                    // Get fee info
                    hdfOtherFees.Value = FeeController.getFeesJSON(ID);

                    ltrTotalAfterCK.Text = string.Format("{0:N0}", (Convert.ToDouble(order.TotalPriceNotDiscount) - Convert.ToDouble(order.TotalDiscount)));

                    ltrCreateBy.Text = order.CreatedBy;
                    ltrCreateDate.Text = order.CreatedDate.ToString();
                    ltrDateDone.Text = "Chưa hoàn tất";
                    if (order.DateDone != null && order.ExcuteStatus == 2)
                    {
                        ltrDateDone.Text = order.DateDone.ToString();
                    }
                    ltrOrderNote.Text = order.OrderNote;
                    ltrOrderQuantity.Text = ProductQuantity.ToString();

                    // Thông tin giảm mã giảm giá
                    hdfCouponID.Value = order.CouponID.ToString();
                    hdfCouponValue.Value = order.CouponValue.ToString();
                    hdfCouponIDOld.Value = order.CouponID.ToString();
                    if (order.CouponID.HasValue && order.CouponID > 0)
                    {
                        var coupon = CouponController.getCoupon(order.CouponID.Value);

                        if (coupon != null)
                        {
                            hdfCouponProductNumber.Value = coupon.ProductNumber.ToString();
                            hdfCouponPriceMin.Value = coupon.PriceMin.ToString();
                            hdfCouponCodeOld.Value = coupon.Code.Trim().ToUpper();
                            hdfCouponProductNumberOld.Value = coupon.ProductNumber.ToString();
                            hdfCouponPriceMinOld.Value = coupon.PriceMin.ToString();
                        }
                    }
                    hdfCouponValueOld.Value = order.CouponValue.ToString();

                    ltrOrderTotalPrice.Text = string.Format("{0:N0}", Convert.ToDouble(order.TotalPrice));

                    if(order.ExcuteStatus == 1)
                    {
                        ltrOrderStatus.Text = "Đang xử lý";
                    }
                    else if(order.ExcuteStatus == 2)
                    {
                        ltrOrderStatus.Text = "Đã hoàn tất";
                    }
                    else
                    {
                        ltrOrderStatus.Text = "Đã hủy";
                    }

                    ltrOrderType.Text = PJUtils.OrderType(Convert.ToInt32(order.OrderType));
                    ltrPrint.Text = "<a href='javascript:;' onclick='warningPrintInvoice(" + ID + ")' class='btn primary-btn fw-btn not-fullwidth'><i class='fa fa-print' aria-hidden='true'></i> Hóa đơn</a>";
                    ltrPrint.Text += "<a href='/print-invoice?id=" + ID + "&merge=1' target='_blank' class='btn primary-btn btn-black fw-btn not-fullwidth print-invoice-merged'><i class='fa fa-print' aria-hidden='true'></i> Hóa đơn gộp</a>";
                    ltrPrint.Text += "<a href='javascript:;' onclick='warningGetOrderImage(" + ID + ", 0)' class='btn primary-btn btn-blue fw-btn not-fullwidth print-invoice-merged'><i class='fa fa-picture-o' aria-hidden='true'></i> Ảnh đơn hàng</a>";
                    ltrPrint.Text += "<a href='javascript:;' onclick='warningGetOrderImage(" + ID + ", 1)' class='btn primary-btn btn-green fw-btn not-fullwidth print-invoice-merged'><i class='fa fa-picture-o' aria-hidden='true'></i> Ảnh đơn hàng gộp</a>";
                    ltrPrint.Text += "<a href='javascript:;' onclick='warningShippingNote(" + ID + ")' class='btn primary-btn btn-red fw-btn not-fullwidth print-invoice-merged'><i class='fa fa-file-text-o' aria-hidden='true'></i> Phiếu gửi hàng</a>";
                    ltrPrint.Text += "<a href='javascript:;' onclick='copyInvoiceURL(" + ID + ", " + order.CustomerID + ")' class='btn primary-btn btn-violet fw-btn not-fullwidth print-invoice-merged'><i class='fa fa-files-o' aria-hidden='true'></i> Link hóa đơn</a>";
                    if (String.IsNullOrEmpty(order.ShippingCode) && order.ExcuteStatus == 2 && order.ShippingType == 6)
                    {
                        ltrPrint.Text += "<a target='_blank' href='/dang-ky-ghtk?orderID=" + ID + "' class='btn primary-btn btn-ghtk fw-btn not-fullwidth print-invoice-merged'><i class='fa fa-upload' aria-hidden='true'></i> Đẩy đơn GHTK</a>";
                    }
                    if (order.ShippingType == 6 && !String.IsNullOrEmpty(order.ShippingCode))
                    {
                        ltrPrint.Text += "<a target='_blank' href='https://khachhang.giaohangtietkiem.vn/khachhang?code=" + order.ShippingCode + "' class='btn primary-btn btn-ghtk fw-btn not-fullwidth print-invoice-merged'><i class='fa fa-upload' aria-hidden='true'></i> Xem đơn GHTK</a>";
                    }
                    if (order.ShippingType == 3 && !String.IsNullOrEmpty(order.ShippingCode))
                    {
                        ltrPrint.Text += "<a target='_blank' href='https://proship.vn/quan-ly-van-don/?isInvoiceFilter=1&generalInfo=" + order.ShippingCode + "' class='btn primary-btn btn-proship fw-btn not-fullwidth print-invoice-merged'><i class='fa fa-file-text-o' aria-hidden='true'></i> Xem đơn Proship</a>";
                    }

                    // Cài đặt thông tin địa chỉ giao hàng
                    _initDeliveryAddress(order.DeliveryAddressId);
                }
            }
        }

        public void LoadTransportCompany()
        {
            var TransportCompany = TransportCompanyController.GetTransportCompany();
            ddlTransportCompanyID.Items.Clear();
            ddlTransportCompanyID.Items.Insert(0, new ListItem("Chọn chành xe", "0"));
            if (TransportCompany.Count > 0)
            {
                foreach (var p in TransportCompany)
                {
                    ListItem listitem = new ListItem(p.CompanyName.ToTitleCase(), p.ID.ToString());
                    ddlTransportCompanyID.Items.Add(listitem);
                }
            }
        }

        public void LoadTransportCompanySubID(int ID)
        {
            ddlTransportCompanySubID.Items.Clear();
            ddlTransportCompanySubID.Items.Insert(0, new ListItem("Chọn nơi nhận", "0"));
            if (ID > 0)
            {
                var ShipTo = TransportCompanyController.GetReceivePlace(ID); ;

                if (ShipTo.Count > 0)
                {
                    foreach (var p in ShipTo)
                    {
                        ListItem listitem = new ListItem(p.ShipTo.ToTitleCase(), p.SubID.ToString());
                        ddlTransportCompanySubID.Items.Add(listitem);
                    }
                }
                ddlTransportCompanySubID.DataBind();
            }
        }

        [WebMethod]
        public static string GetTransportSub(int transComID)
        {
            if (transComID > 0)
            {
                var data = new List<object>();
                data.Add(new
                {
                    key = "0",
                    value = "Chọn nơi nhận"
                });

                var ShipTo = TransportCompanyController.GetReceivePlace(transComID);

                foreach (var p in ShipTo)
                {
                    data.Add(new
                    {
                        key = p.SubID.ToString(),
                        value = p.ShipTo.ToTitleCase()
                    });
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                return serializer.Serialize(data);
            }

            return String.Empty;
        }

        [WebMethod]
        public static string getOrderReturn(int customerID)
        {
            return RefundGoodController.getOrderReturnJSON(customerID);
        }

        [WebMethod]
        public static string UpdateStatus(int OrderID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string username = HttpContext.Current.Request.Cookies["usernameLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            var order = OrderController.GetByID(OrderID);
            if (order != null)
            {
                int i = OrderController.UpdateExcuteStatus(order.ID, acc.Username);
                if (i > 0)
                {
                    return serializer.Serialize(i);
                }
                else
                {
                    return serializer.Serialize(null);
                }
            }
            else
            {
                return serializer.Serialize(null);
            }
        }

        public class ProductGetOut
        {
            public int ID { get; set; }
            public string ProductName { get; set; }
            public string ProductVariable { get; set; }
            public string ProductVariableSave { get; set; }
            public string ProductVariableName { get; set; }
            public string ProductVariableValue { get; set; }
            public int ProductType { get; set; }
            public string ProductImage { get; set; }
            public string ProductImageOrigin { get; set; }
            public string QuantityMainInstockString { get; set; }
            public double QuantityMainInstock { get; set; }
            public string QuantityInstockString { get; set; }
            public double QuantityInstock { get; set; }
            public string SKU { get; set; }
            public double Giabanle { get; set; }
            public string stringGiabanle { get; set; }
            public double Giabansi { get; set; }
            public string stringGiabansi { get; set; }
        }

        [WebMethod]
        public static void Delete(List<tbl_OrderDetail> listOrderDetail)
        {
            DateTime currentDate = DateTime.Now;
            string username = HttpContext.Current.Request.Cookies["usernameLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (listOrderDetail != null && listOrderDetail.Count > 0)
            {
                foreach (var orderDetail in listOrderDetail)
                {
                    int AgentID = Convert.ToInt32(acc.AgentID);
                    int OrderID = orderDetail.ID;
                    string OrderSKU = orderDetail.SKU;
                    int ProductID = orderDetail.ProductID.Value;
                    double quantitynew = orderDetail.Quantity.Value;
                    string ord = OrderDetailController.Delete(OrderID, OrderSKU);

                    var parent = ProductVariableController.GetBySKU(OrderSKU);

                    if (parent != null)
                    {
                        StockManagerController.Insert(
                            new tbl_StockManager
                            {
                                AgentID = AgentID,
                                ProductID = 0,
                                ProductVariableID = ProductID,
                                Quantity = quantitynew,
                                QuantityCurrent = 0,
                                Type = 1,
                                NoteID = "Nhập kho khi xóa sản phẩm trong sửa đơn",
                                OrderID = OrderID,
                                Status = 10,
                                SKU = OrderSKU,
                                CreatedDate = currentDate,
                                CreatedBy = username,
                                ModifiedDate = currentDate,
                                ModifiedBy = username,
                                MoveProID = 0,
                                ParentID = parent.ProductID
                            });
                    }
                    else
                    {
                        StockManagerController.Insert(
                            new tbl_StockManager
                            {
                                AgentID = AgentID,
                                ProductID = ProductID,
                                ProductVariableID = 0,
                                Quantity = quantitynew,
                                QuantityCurrent = 0,
                                Type = 1,
                                NoteID = "Nhập kho khi xóa sản phẩm trong sửa đơn",
                                OrderID = OrderID,
                                Status = 10,
                                SKU = OrderSKU,
                                CreatedDate = currentDate,
                                CreatedBy = username,
                                ModifiedDate = currentDate,
                                ModifiedBy = username,
                                MoveProID = 0,
                                ParentID = ProductID
                            });
                    }
                }
            }
        }

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Request.Cookies["usernameLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);

            if (acc != null)
            {
                if (acc.RoleID == 0 || acc.RoleID == 2)
                {
                    int OrderID = hdOrderInfoID.Value.ToInt(0);
                    if (OrderID > 0)
                    {
                        var order = OrderController.GetByID(OrderID);
                        if (order != null)
                        {
                            #region Xử lý Order
                            #region Lấy thông để cập nhật Order
                            username = order.CreatedBy;
                            int userID = AccountController.GetByUsername(username).ID;

                            if (ddlCreatedBy.SelectedValue != order.CreatedBy)
                            {
                                username = ddlCreatedBy.SelectedValue;
                            }
                            string message = "";
                            int AgentID = Convert.ToInt32(acc.AgentID);
                            int OrderType = hdfOrderType.Value.ToInt();
                            string AdditionFee = "0";
                            string DisCount = "0";
                            int CustomerID = 0;

                            string CustomerPhone = Regex.Replace(txtPhone.Text.Trim(), @"[^\d]", "");
                            string CustomerName = txtFullname.Text.Trim().ToTitleCase();
                            string Nick = txtNick.Text.Trim();
                            string CustomerAddress = txtAddress.Text.Trim().ToTitleCase();
                            string Zalo = "";
                            string Facebook = txtFacebook.Text.Trim();

                            int ProvinceID = hdfProvinceID.Value.ToInt(0);
                            int DistrictID = hdfDistrictID.Value.ToInt(0);
                            int WardID = hdfWardID.Value.ToInt(0);

                            int PaymentType = ddlPaymentType.SelectedValue.ToInt(0);
                            int ShippingType = ddlShippingType.SelectedValue.ToInt(0);
                            int TransportCompanyID = ddlTransportCompanyID.SelectedValue.ToInt(0);
                            int TransportCompanySubID = hdfTransportCompanySubID.Value.ToInt(0);
                            int PostalDeliveryType = ddlPostalDeliveryType.SelectedValue.ToInt();

                            #region Câp nhật thông tin khách hàng
                            var Customer = CustomerController.GetByPhone(CustomerPhone);
                            if (Customer != null)
                            {
                                CustomerID = Customer.ID;
                                string kq = CustomerController.Update(CustomerID, CustomerName, Customer.CustomerPhone, CustomerAddress, "", Customer.CustomerLevelID.Value, Customer.Status.Value, username, currentDate, acc.Username, false, Zalo, Facebook, Customer.Note, Nick, Customer.Avatar, ShippingType, PaymentType, TransportCompanyID, TransportCompanySubID, Customer.CustomerPhone2, ProvinceID, DistrictID, WardID);
                            }
                            else
                            {
                                string kq = CustomerController.Insert(CustomerName, CustomerPhone, CustomerAddress, "", 0, 0, currentDate, username, false, Zalo, Facebook, "", Nick, "", ShippingType, PaymentType, TransportCompanyID, TransportCompanySubID, "", ProvinceID, DistrictID, WardID);
                                if (kq.ToInt(0) > 0)
                                {
                                    CustomerID = kq.ToInt(0);
                                }
                            }
                            #endregion

                            string totalPrice = hdfTotalPrice.Value.ToString();

                            string totalPriceNotDiscount = hdfTotalPriceNotDiscount.Value;
                            int PaymentStatusOld = order.PaymentStatus.Value;
                            int ExcuteStatusOld = order.ExcuteStatus.Value;
                            int PaymentStatus = ddlPaymentStatus.SelectedValue.ToInt(0);
                            int ExcuteStatus = ddlExcuteStatus.SelectedValue.ToInt(0);

                            string ShippingCode = txtShippingCode.Text.Trim().Replace("#", "").Replace(" ", "");
                            //if (ShippingType == 6)
                            //{
                            //    string[] barcode = ShippingCode.Split('.');
                            //    ShippingCode = barcode[barcode.Length - 1];
                            //}
                            string OrderNote = txtOrderNote.Text;

                            double DiscountPerProduct = 0;
                            if (!string.IsNullOrEmpty(hdfDiscountAmount.Value))
                            {
                                DiscountPerProduct = Convert.ToDouble(hdfDiscountAmount.Value);
                            }

                            string sl = hdftotal.Value;
                            if (!string.IsNullOrEmpty(hdfTotalQuantity.Value))
                            {
                                sl = hdfTotalQuantity.Value;
                            }
                            double TotalDiscount = Convert.ToDouble(pDiscount.Value) * Convert.ToDouble(sl);
                            string FeeShipping = pFeeShip.Value.ToString();
                            double Weight = Convert.ToDouble(txtWeight.Value);

                            #region Tính ngày kết thúc theo ExcuteStatus và PaymentType
                            string datedone = "";
                            if (order.DateDone != null)
                            {
                                datedone = order.DateDone.ToString();
                            }

                            if (order.ExcuteStatus != 2)
                            {
                                if (ExcuteStatus == 2)
                                {
                                    // Nếu chưa có ngày hoàn tất
                                    if (string.IsNullOrEmpty(order.DateDone.ToString()))
                                    {
                                        // Lấy ngày hiện tại làm ngày hoàn tất
                                        datedone = DateTime.Now.ToString();
                                    }
                                    else
                                    {
                                        // Nếu đã có ngày hoàn tất nhưng phương thức thanh toán thu hộ thì lấy ngày hiện tại
                                        if(PaymentType == 3)
                                        {
                                            datedone = DateTime.Now.ToString();
                                            message = "<br>Lưu ý: do đơn hàng này thu hộ nên ngày hoàn tất cập nhật ngày hôm nay!";
                                        }
                                    }
                                }
                            }
                            else if (order.ExcuteStatus == 2)
                            {
                                if (ExcuteStatus == 2)
                                {
                                    DateTime oldDateDone = Convert.ToDateTime(order.DateDone).Date;
                                    if (oldDateDone == DateTime.Now.Date)
                                    {
                                        datedone = DateTime.Now.ToString();
                                    }
                                }
                            }
                            #endregion

                            // Update coupon
                            var couponID = hdfCouponID.Value.ToInt(0);
                            var couponIDOld = order.CouponID.HasValue ? order.CouponID.Value : 0;
                            var couponValue = hdfCouponValue.Value.ToDecimal(0);

                            // Cập nhật địa chỉ giao hàng
                            var deliveryAddressId = Convert.ToInt64(hdfDeliveryAddressId.Value);

                            #region Tạo dữ liệu cập nhật Order
                            var orderNew = new tbl_Order()
                            {
                                ID = OrderID,
                                OrderType = OrderType,
                                AdditionFee = AdditionFee,
                                DisCount = DisCount,
                                CustomerID = CustomerID,
                                CustomerName = CustomerName,
                                CustomerPhone = CustomerPhone,
                                CustomerAddress = CustomerAddress,
                                CustomerEmail = String.Empty,
                                TotalPrice = totalPrice,
                                TotalPriceNotDiscount = totalPriceNotDiscount,
                                PaymentStatus = PaymentStatus,
                                ExcuteStatus = ExcuteStatus,
                                ModifiedDate = currentDate,
                                CreatedBy = username,
                                ModifiedBy = acc.Username,
                                DiscountPerProduct = Convert.ToDouble(pDiscount.Value),
                                TotalDiscount = TotalDiscount,
                                FeeShipping = FeeShipping,
                                GuestPaid = order.GuestPaid.Value,
                                GuestChange = order.GuestChange.Value,
                                PaymentType = PaymentType,
                                ShippingType = ShippingType,
                                OrderNote = OrderNote,
                                RefundsGoodsID = 0,
                                ShippingCode = ShippingCode,
                                TransportCompanyID = TransportCompanyID,
                                TransportCompanySubID = TransportCompanySubID,
                                OtherFeeName = String.Empty,
                                OtherFeeValue = 0,
                                PostalDeliveryType = PostalDeliveryType,
                                CouponID = couponID,
                                CouponValue = couponValue,
                                Weight = Weight,
                                DeliveryAddressId = deliveryAddressId
                            };
                            #endregion

                            // Update date done
                            if (!String.IsNullOrEmpty(datedone))
                                orderNew.DateDone = Convert.ToDateTime(datedone);

                            OrderController.UpdateOnSystem(orderNew);
                            #endregion

                            #region Xử lý Other Fee
                            if (!String.IsNullOrEmpty(hdfOtherFees.Value))
                            {
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                var fees = serializer.Deserialize<List<Fee>>(hdfOtherFees.Value);
                                if (fees != null)
                                {
                                    foreach (var fee in fees)
                                    {
                                        fee.OrderID = OrderID;
                                        fee.CreatedBy = userID;
                                        fee.CreatedDate = DateTime.Now;
                                        fee.ModifiedBy = acc.ID;
                                        fee.ModifiedDate = DateTime.Now;
                                    }

                                    FeeController.Update(OrderID, fees);
                                }
                            }
                            else
                            {
                                // Remove all fee
                                FeeController.deleteAll(OrderID);
                            }
                            #endregion

                            #region Xử lý Coupon
                            if (couponID == 0)
                            {
                                if (couponIDOld > 0)
                                {
                                    CouponController.updateStatusCouponCustomer(CustomerID, couponIDOld, true);
                                }
                            }
                            else
                            {
                                if (couponIDOld != couponID)
                                {
                                    if (couponIDOld > 0)
                                        CouponController.updateStatusCouponCustomer(CustomerID, couponIDOld, true);
                                    if (couponID > 0)
                                        CouponController.updateStatusCouponCustomer(CustomerID, couponID, false);
                                }
                            }
                            #endregion

                            #region Xử lý Transfer Bank
                            var bankID = ddlBank.SelectedValue.ToInt(0);
                            if (bankID != 0)
                            {
                                int bankOfOrder = BankTransferController.getBankOfOrder(order.ID);
                                if (bankOfOrder != 0)
                                {
                                    BankTransferController.updateBankOfOrder(order.ID, bankID);
                                }
                                else
                                {
                                    BankTransferController.Create(order, bankID, acc);
                                }
                            }
                            #endregion

                            #region Xử lý hủy đơn hàng
                            if (ExcuteStatus == 3)
                            {
                                var productRefund = OrderDetailController.GetByOrderID(order.ID);

                                foreach(tbl_OrderDetail product in productRefund)
                                {
                                    int parentID = 0;

                                    if (product.ProductID != 0)
                                    {
                                        parentID = product.ProductID.Value;
                                    }
                                    else
                                    {
                                        parentID = ProductVariableController.GetByID(product.ProductVariableID.Value).ProductID.Value;
                                    }

                                    StockManagerController.Insert(
                                        new tbl_StockManager {
                                            AgentID = product.AgentID,
                                            ProductID = product.ProductID,
                                            ProductVariableID = product.ProductVariableID,
                                            Quantity = product.Quantity,
                                            QuantityCurrent = 0,
                                            Type = 1,
                                            NoteID = "Nhập kho do hủy đơn hàng",
                                            OrderID = product.OrderID,
                                            Status = 4,
                                            SKU = product.SKU,
                                            CreatedDate = currentDate,
                                            CreatedBy = product.CreatedBy,
                                            ModifiedDate = currentDate,
                                            ModifiedBy = product.CreatedBy,
                                            MoveProID = 0,
                                            ParentID = parentID
                                        });
                                }

                                PJUtils.ShowMessageBoxSwAlertCallFunction("Hủy đơn hàng thành công", "s", true, "", Page);

                                Response.Redirect("/danh-sach-don-hang");
                                return;
                            }
                            #endregion
                            #endregion

                            if (OrderID > 0)
                            {
                                #region Xử lý OrderDetail
                                #region Cập nhật từng dòng chi tiết đơn hàng
                                var orderAvatar = String.Empty;
                                var list = hdfListProduct.Value;
                                var items = list.Split(';').Where(x => !String.IsNullOrEmpty(x)).ToList();

                                if (items.Count > 0 && ExcuteStatus == 1)
                                    items.Reverse();

                                for (var i = 0; i < items.Count; i++)
                                {
                                    #region Lấy thông tin chi tiết của đơn hàng
                                    var item = items.ElementAt(i);
                                    string[] itemValue = item.Split(',');

                                    int ProductID = itemValue[0].ToInt();
                                    int ProductVariableID = itemValue[12].ToInt();
                                    string SKU = itemValue[1].ToString();
                                    int ProductType = itemValue[2].ToInt();

                                    // Tìm parentID
                                    int parentID = ProductID;
                                    var variable = ProductVariableController.GetByID(ProductVariableID);
                                    if (variable != null)
                                    {
                                        parentID = Convert.ToInt32(variable.ProductID);
                                    }

                                    string ProductVariableName = itemValue[3];
                                    string ProductVariableValue = itemValue[4];
                                    double Quantity = Convert.ToDouble(itemValue[5]);
                                    string ProductName = itemValue[6];
                                    string ProductImageOrigin = itemValue[7];
                                    string ProductVariable = itemValue[8];
                                    double Price = Convert.ToDouble(itemValue[9]);
                                    string ProductVariableSave = itemValue[10];
                                    int OrderDetailID = itemValue[11].ToInt(0);
                                    #endregion

                                    #region Cập nhật avatar cho đơn hàng
                                    if (String.IsNullOrEmpty(orderAvatar))
                                        orderAvatar = AnnImage.extractImage(ProductImageOrigin);
                                    #endregion

                                    #region Xử lý với trạng thái của đơn hàng đã hủy
                                    if (ExcuteStatusOld == 3)
                                    {
                                        var orderDetail = OrderDetailController.GetByID(OrderDetailID);

                                        if (orderDetail != null)
                                        {
                                            OrderDetailController.UpdateQuantity(OrderDetailID, Quantity, Price, currentDate, username);
                                        }
                                        else
                                        {
                                            OrderDetailController.Insert(new tbl_OrderDetail()
                                            {
                                                AgentID = AgentID,
                                                OrderID = OrderID,
                                                SKU = SKU,
                                                ProductID = ProductID,
                                                ProductVariableID = ProductVariableID,
                                                ProductVariableDescrition = ProductVariableSave,
                                                Quantity = Quantity,
                                                Price = Price,
                                                Status = 1,
                                                DiscountPrice = 0,
                                                ProductType = 2,
                                                CreatedDate = currentDate,
                                                CreatedBy = username,
                                                ModifiedDate = currentDate,
                                                ModifiedBy = username,
                                                IsCount = true
                                            });
                                        }

                                        StockManagerController.Insert(
                                            new tbl_StockManager
                                            {
                                                AgentID = AgentID,
                                                ProductID = ProductID,
                                                ProductVariableID = ProductVariableID,
                                                Quantity = Quantity,
                                                QuantityCurrent = 0,
                                                Type = 2,
                                                NoteID = "Xuất kho khi chuyển trạng từ trạng thái hủy đơn hàng sang trạng thái khác",
                                                OrderID = OrderID,
                                                Status = 4,
                                                SKU = SKU,
                                                CreatedDate = currentDate,
                                                CreatedBy = username,
                                                ModifiedDate = currentDate,
                                                ModifiedBy = username,
                                                MoveProID = 0,
                                                ParentID = parentID,
                                            });

                                        continue;
                                    }
                                    #endregion

                                    #region kiểm tra sản phẩm này đã có trong đơn chưa?
                                    var od = OrderDetailController.GetByID(OrderDetailID);

                                    #region nếu sản phẩm này có trong đơn có rồi thì chỉnh sửa
                                    if (od != null)
                                    {
                                        double quantityOld = Convert.ToDouble(od.Quantity);

                                        if (quantityOld > Quantity)
                                        {
                                            //cộng vô kho
                                            double quantitynew = quantityOld - Quantity;
                                            StockManagerController.Insert(
                                                new tbl_StockManager
                                                {
                                                    AgentID = AgentID,
                                                    ProductID = ProductID,
                                                    ProductVariableID = ProductVariableID,
                                                    Quantity = quantitynew,
                                                    QuantityCurrent = 0,
                                                    Type = 1,
                                                    NoteID = "Nhập kho khi giảm số lượng trong sửa đơn",
                                                    OrderID = OrderID,
                                                    Status = 4,
                                                    SKU = SKU,
                                                    CreatedDate = currentDate,
                                                    CreatedBy = username,
                                                    ModifiedDate = currentDate,
                                                    ModifiedBy = username,
                                                    MoveProID = 0,
                                                    ParentID = parentID,
                                                });
                                        }
                                        else if (quantityOld < Quantity)
                                        {
                                            // tính số lượng kho cần xuất thêm
                                            double quantitynew = Quantity - quantityOld;

                                            //trừ tiếp trong kho
                                            StockManagerController.Insert(
                                                new tbl_StockManager
                                                {
                                                    AgentID = AgentID,
                                                    ProductID = ProductID,
                                                    ProductVariableID = ProductVariableID,
                                                    Quantity = quantitynew,
                                                    QuantityCurrent = 0,
                                                    Type = 2,
                                                    NoteID = "Xuất kho khi tăng số lượng trong sửa đơn",
                                                    OrderID = OrderID,
                                                    Status = 3,
                                                    SKU = SKU,
                                                    CreatedDate = currentDate,
                                                    CreatedBy = username,
                                                    ModifiedDate = currentDate,
                                                    ModifiedBy = username,
                                                    MoveProID = 0,
                                                    ParentID = parentID,
                                                });
                                        }

                                        // cập nhật số lượng sản phẩm trong đơn hàng
                                        OrderDetailController.UpdateQuantity(OrderDetailID, Quantity, Price, currentDate, username);
                                    }
                                    #endregion
                                    #region nếu sản phẩm này chưa có trong đơn thì thêm vào
                                    else
                                    {
                                        OrderDetailController.Insert(AgentID, OrderID, SKU, ProductID, ProductVariableID, ProductVariableSave, Quantity, Price, 1, 0, ProductType, currentDate, username, true);

                                        StockManagerController.Insert(
                                            new tbl_StockManager
                                            {
                                                AgentID = AgentID,
                                                ProductID = ProductID,
                                                ProductVariableID = ProductVariableID,
                                                Quantity = Quantity,
                                                QuantityCurrent = 0,
                                                Type = 2,
                                                NoteID = "Xuất kho khi thêm sản phẩm mới trong sửa đơn",
                                                OrderID = OrderID,
                                                Status = 3,
                                                SKU = SKU,
                                                CreatedDate = currentDate,
                                                CreatedBy = username,
                                                ModifiedDate = currentDate,
                                                ModifiedBy = username,
                                                MoveProID = 0,
                                                ParentID = parentID,
                                            });
                                    }
                                    #endregion
                                    #endregion
                                }
                                #endregion

                                OrderController.updateAvatarQuantityCOGS(OrderID, orderAvatar);
                                #endregion

                                // update stockmanager createdby nếu đổi nhân viên phụ trách
                                if (ddlCreatedBy.SelectedValue != order.CreatedBy)
                                    StockManagerController.updateCreatedByOrderID(OrderID, username);

                                #region Thêm đơn hàng đổi trả
                                string refund = hdSession.Value;
                                // case click "bo qua"
                                if (refund == "0")
                                {
                                    if (hdfcheckR.Value != "")
                                    {
                                        string[] b = hdfcheckR.Value.Split(',');
                                        var update_returnorder = RefundGoodController.UpdateStatus(b[0].ToInt(), username, 1, 0);
                                        var update_order = OrderController.UpdateRefund(OrderID, null, acc.Username);
                                    }
                                }
                                else if (refund != "1")
                                {
                                    string[] RefundID = refund.Split('|');
                                    var update_returnorder = RefundGoodController.UpdateStatus(RefundID[0].ToInt(), username, 2, OrderID);
                                    var update_order = OrderController.UpdateRefund(OrderID, RefundID[0].ToInt(), acc.Username);

                                    if(hdfcheckR.Value != "")
                                    {
                                        string[] k = hdfcheckR.Value.Split(',');
                                        if (k[0] != RefundID[0])
                                        {
                                            var update_oldreturnorder = RefundGoodController.UpdateStatus(k[0].ToInt(), username, 1, 0);
                                        }
                                    }
                                }
                                #endregion

                                #region push notify telegram
                                if (ExcuteStatusOld == 1 && ExcuteStatus == 2)
                                {
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                                    var pushOrder = OrderController.GetByID(order.ID);
                                    var msg = "<b>" + pushOrder.ID + "</b> - " + DateTime.Now.ToString("dd/MM HH:mm");
                                    msg += "\r\n- <b>TỔNG</b>: " + string.Format("{0:N0}", Convert.ToDouble(pushOrder.TotalPrice));
                                    msg += "\r\n- <b>Số lượng</b>: " + pushOrder.TotalQuantity + " cái";
                                    
                                    msg += "\r\n- <b>Thanh toán</b>: " + ddlPaymentType.SelectedItem.Text;
                                    msg += "\r\n- <b>Giao hàng</b>: " + ddlShippingType.SelectedItem.Text;
                                    if (pushOrder.DiscountPerProduct > 0)
                                    {
                                        msg += "\r\n- <b>Chiết khấu</b>: -" + string.Format("{0:N0}", Convert.ToDouble(pushOrder.DiscountPerProduct)) + "/cái";
                                    }
                                    if (Convert.ToDouble(pushOrder.FeeShipping) > 0)
                                    {
                                        msg += "\r\n- <b>Phí ship</b>: " + string.Format("{0:N0}", Convert.ToDouble(pushOrder.FeeShipping));
                                    }
                                    if (Convert.ToDouble(pushOrder.CouponValue) > 0)
                                    {
                                        msg += "\r\n- <b>Mã giảm giá</b>: -" + string.Format("{0:N0}", Convert.ToDouble(pushOrder.CouponValue));
                                    }

                                    double totalProfit = Convert.ToDouble(pushOrder.TotalPriceNotDiscount) - Convert.ToDouble(pushOrder.TotalDiscount) - Convert.ToDouble(pushOrder.CouponValue) - Convert.ToDouble(pushOrder.TotalCostOfGood);
                                    double totalRefundProfit = 0;
                                    if (pushOrder.RefundsGoodsID != 0 && pushOrder.RefundsGoodsID != null)
                                    {
                                        var refundOrder = RefundGoodController.GetByID(pushOrder.RefundsGoodsID.Value);
                                        if (refundOrder != null)
                                        {
                                            msg += "\r\n- <b>Trừ hàng trả</b>: -" + string.Format("{0:N0}", Convert.ToDouble(refundOrder.TotalPrice));
                                            totalRefundProfit = Convert.ToDouble(refundOrder.TotalPrice) - Convert.ToDouble(refundOrder.TotalCostOfGood);
                                        }
                                    }
                                    msg += "\r\n- <b>Lợi nhuận</b>: " + string.Format("{0:N0}", totalProfit - totalRefundProfit);
                                    msg += "\r\n-------------------------";
                                    msg += "\r\n- <b>Khách hàng</b>: " + pushOrder.CustomerName;
                                    if (!string.IsNullOrEmpty(txtNick.Text.Trim()))
                                    {
                                        msg += "\r\n- <b>Nick</b>: " + txtNick.Text.Trim();
                                    }
                                    msg += "\r\n- <b>Điện thoại</b>: " + pushOrder.CustomerPhone;
                                    msg += "\r\n- <b>Nhân viên</b>: " + pushOrder.CreatedBy;

                                    var chatID = "-1001229080769";
                                    var token = "bot1714400602:AAHlWZhq4IZZ18wCQxVVGA4kuZJQPkb50z0";
                                    var api = "https://api.telegram.org/" + token + "/sendMessage?parse_mode=html&chat_id=" + chatID + "&text=" + HttpUtility.UrlEncode(msg);

                                    WebRequest request = WebRequest.Create(api);
                                    Stream rs = request.GetResponse().GetResponseStream();
                                }
                                #endregion

                                PJUtils.ShowMessageBoxSwAlertCallFunction("Cập nhật đơn hàng thành công" + message, "s", true, "", Page);
                            }
                        }
                    }
                }
            }
        }

        [WebMethod]
        public static string getCoupon(int customerID, string code, int productNumber, decimal price)
        {
            return CouponController.getCoupon(customerID, code, productNumber, price);
        }
    }
}