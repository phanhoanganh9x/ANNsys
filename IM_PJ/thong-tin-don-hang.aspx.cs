using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
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
                if (Request.Cookies["userLoginSystem"] != null)
                {
                    string username = Request.Cookies["userLoginSystem"].Value;
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
                        }
                        else if (acc.RoleID == 2)
                        {
                            hdfUsername.Value = acc.Username;
                            hdfUsernameCurrent.Value = acc.Username;
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
        public void LoadData()
        {
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
                    // Init drop down list ddlFeeType
                    var feeTypes = FeeTypeController.getDropDownList();
                    feeTypes[0].Text = "Loại Phí";
                    ddlFeeType.Items.Clear();
                    ddlFeeType.Items.AddRange(feeTypes.ToArray());
                    ddlFeeType.DataBind();
                    ddlFeeType.SelectedIndex = 0;

                    // Init drop down list Price Type
                    ddlPriceType.Items.Clear();
                    ddlPriceType.Items.Add(new ListItem("Trừ", "0"));
                    ddlPriceType.Items.Add(new ListItem("Cộng", "1"));
                    ddlPriceType.DataBind();
                    ddlPriceType.SelectedIndex = 1;

                    // chuyển sang giao diện xem đơn chuyển hoàn nếu trạng thái xử lý đã chuyển hoàn
                    if (order.ExcuteStatus == 4)
                    {
                        Response.Redirect("/thong-tin-don-hang-chuyen-hoan?id="+ID);
                    }


                    string username = HttpContext.Current.Request.Cookies["userLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);
                    if(acc.RoleID != 0)
                    {
                        if (order.CreatedBy != acc.Username)
                        {
                            Response.Redirect("/danh-sach-don-hang");
                        }
                    }

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
                    hdOrderInfoID.Value = ID.ToString();

                    int AgentID = Convert.ToInt32(order.AgentID);
                    txtPhone.Text = order.CustomerPhone;
                    txtFullname.Text = order.CustomerName;

                    txtAddress.Text = order.CustomerAddress;
                    var cus = CustomerController.GetByID(order.CustomerID.Value);
                    if (cus != null)
                    {
                        txtNick.Text = cus.Nick;
                        if (string.IsNullOrEmpty(cus.Nick))
                        {
                            txtNick.Enabled = true;
                        }
                        
                        txtZalo.Text = cus.Zalo;
                        if (string.IsNullOrEmpty(cus.Zalo))
                        {
                            txtZalo.Enabled = true;
                        }

                        txtFacebook.Text = cus.Facebook;
                        if (!string.IsNullOrEmpty(cus.Facebook))
                        {
                            ltrFb.Text += "<a href =\"" + cus.Facebook + "\" class=\"btn primary-btn fw-btn not-fullwidth\" target=\"_blank\">Xem</a>";
                        }
                        else
                        {
                            txtFacebook.Enabled = true;
                        }
                    }

                    // Title
                    this.Title = String.Format("{0} - Đơn hàng", cus.Nick != "" ? cus.Nick.ToTitleCase() : cus.CustomerName.ToTitleCase());
                    ltrHeading.Text = "Đơn hàng #" + ID.ToString() + " - " + (cus.Nick != "" ? cus.Nick.ToTitleCase() : cus.CustomerName.ToTitleCase());

                    int customerID = Convert.ToInt32(order.CustomerID);
                    ltrViewDetail.Text = "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth\" onclick=\"viewCustomerDetail('" + customerID + "')\"><i class=\"fa fa-address-card-o\" aria-hidden=\"true\"></i> Chi tiết</a>";
                    ltrViewDetail.Text += "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth edit-customer-btn\" onclick=\"refreshCustomerInfo('" + customerID + "')\"><i class=\"fa fa-refresh\" aria-hidden=\"true\"></i> Làm mới</a>";
                    ltrViewDetail.Text += "<a href=\"chi-tiet-khach-hang?id=" + customerID + "\" class=\"btn primary-btn fw-btn not-fullwidth edit-customer-btn\" target=\"_blank\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Chỉnh sửa</a>";
                    ltrViewDetail.Text += "<a href=\"danh-sach-don-hang?textsearch=" + order.CustomerPhone + "\" class=\"btn primary-btn fw-btn not-fullwidth edit-customer-btn\" target=\"_blank\"><i class=\"fa fa-history\" aria-hidden=\"true\"></i> Lịch sử</a>";
                    ltrViewDetail.Text += "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth clear-btn\" onclick=\"clearCustomerDetail()\"><i class=\"fa fa-times\" aria-hidden=\"true\"></i> Bỏ qua</a>";
                    var d = DiscountCustomerController.getbyCustID(customerID);
                    if (d.Count > 0)
                    {
                        var da = d[0].DiscountAmount;
                        hdfIsDiscount.Value = "1";
                        hdfDiscountAmount.Value = da.ToString();
                        ltrDiscountInfo.Text = "<strong>* Chiết khấu của khách: " + string.Format("{0:N0}", Convert.ToDouble(da)) + " đ/cái.</strong>";
                    }
                    else
                    {
                        hdfIsDiscount.Value = "0";
                        hdfDiscountAmount.Value = "0";
                    }
                    int customerType = Convert.ToInt32(order.OrderType);
                    ltrCustomerType.Text = "";
                    ltrCustomerType.Text += "<select class=\"form-control customer-type\" onchange=\"getProductPrice($(this))\">";
                    if (customerType == 1)
                    {
                        ltrCustomerType.Text += "<option value=\"2\">Khách mua sỉ</option>";
                        ltrCustomerType.Text += "<option value=\"1\" selected>Khách mua lẻ</option>";

                    }
                    else
                    {
                        ltrCustomerType.Text += "<option value=\"2\" selected>Khách mua sỉ</option>";
                        ltrCustomerType.Text += "<option value=\"1\">Khách mua lẻ</option>";

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

                        ltrTotalPriceRefund.Text = string.Format("{0:N0}", totalrefund);
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

                    #region Lấy danh sách sản phẩm

                    var orderdetails = OrderDetailController.GetByOrderID(ID);
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
                                        Giabansi = Convert.ToDouble(product.Regular_Price);
                                        Giabanle = ItemPrice;
                                    }
                                    else
                                    {
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

                                    if (!string.IsNullOrEmpty(product.ProductImage))
                                    {
                                        ProductImage = "<img src=\"" + product.ProductImage + "\" />";
                                        ProductImageOrigin = product.ProductImage;
                                    }
                                    else
                                    {
                                        ProductImage = "<img src=\"/App_Themes/Ann/image/placeholder.png\" />";
                                        ProductImageOrigin = "";
                                    }
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
                                    }
                                        

                                    if (!string.IsNullOrEmpty(productvariable.Image))
                                    {
                                        ProductImage = "<img src=\"" + productvariable.Image + "\" />";
                                        ProductImageOrigin = productvariable.Image;
                                    }
                                    else if (_product != null && !string.IsNullOrEmpty(_product.ProductImage))
                                    {
                                        ProductImage = "<img src=\"" + _product.ProductImage + "\" />";
                                        ProductImageOrigin = _product.ProductImage;
                                    }
                                    else
                                    {
                                        ProductImage = "<img src=\"/App_Themes/Ann/image/placeholder.png\" />";
                                        ProductImageOrigin = "";
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
                            html.AppendLine(String.Format("    <td class='order-item'>{0}</td>", orderitem));
                            html.AppendLine(String.Format("    <td class='image-item'>{0}</td>", "<a href='/xem-san-pham?id=" + item.ProductID + "&variableid=" + item.ProductVariableID + "' target='_blank'>" + ProductImage + "</a>"));
                            html.AppendLine(String.Format("    <td class='name-item'>{0}</td>", "<a href='/xem-san-pham?id=" + item.ProductID + "&variableid=" + item.ProductVariableID + "' target='_blank'>" + ProductName + "</a>"));
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
                    
                    hdfOrderType.Value = customerType.ToString();
                    hdfTotalPrice.Value = totalPrice.ToString();
                    hdfTotalPriceNotDiscount.Value = totalPriceNotDiscount.ToString();
                    hdfPaymentStatus.Value = paymentStatus.ToString();
                    hdfExcuteStatus.Value = excuteStatus.ToString();
                    hdftotal.Value = ProductQuantity.ToString();
                    hdfRoleID.Value = acc.RoleID.ToString();

                    #endregion
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

                    ltrProductQuantity.Text = string.Format("{0:N0}", ProductQuantity) + " sản phẩm";
                    ltrTotalNotDiscount.Text = string.Format("{0:N0}", Convert.ToDouble(order.TotalPriceNotDiscount));
                    ltrTotalprice.Text = string.Format("{0:N0}", Convert.ToDouble(order.TotalPrice));
                    pDiscount.Value = order.DiscountPerProduct;
                    pFeeShip.Value = Convert.ToDouble(order.FeeShipping);

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
                    ltrPrint.Text = "<a href=\"javascript:;\" onclick=\"warningPrintInvoice(" + ID + ")\" class=\"btn primary-btn fw-btn not-fullwidth\"><i class=\"fa fa-print\" aria-hidden=\"true\"></i> In hóa đơn</a>";
                    ltrPrint.Text += "<a href=\"/print-invoice?id=" + ID + "&merge=1\" target=\"_blank\" class=\"btn primary-btn fw-btn not-fullwidth print-invoice-merged\"><i class=\"fa fa-print\" aria-hidden=\"true\"></i> In hóa đơn gộp</a>";
                    ltrPrint.Text += "<a href=\"javascript:;\" onclick=\"warningGetOrderImage(" + ID + ", 0)\" class=\"btn primary-btn fw-btn not-fullwidth print-invoice-merged\"><i class=\"fa fa-picture-o\" aria-hidden=\"true\"></i> Lấy ảnh đơn hàng</a>";
                    ltrPrint.Text += "<a href=\"javascript:;\" onclick=\"warningGetOrderImage(" + ID + ", 1)\" class=\"btn primary-btn fw-btn not-fullwidth print-invoice-merged\"><i class=\"fa fa-picture-o\" aria-hidden=\"true\"></i> Lấy ảnh đơn hàng gộp</a>";
                    ltrPrint.Text += "<a href=\"javascript:;\" onclick=\"warningShippingNote(" + ID + ")\" class=\"btn primary-btn fw-btn not-fullwidth print-invoice-merged\"><i class=\"fa fa-file-text-o\" aria-hidden=\"true\"></i> In phiếu gửi hàng</a>";
                    if(order.ShippingType == 3 && !string.IsNullOrEmpty(order.ShippingCode))
                    {
                        ltrPrint.Text += "<a href=\"https://proship.vn/quan-ly-van-don/?isInvoiceFilter=1&generalInfo=" + order.ShippingCode + "\" target=\"_blank\" class=\"btn primary-btn fw-btn not-fullwidth print-invoice-merged\"><i class=\"fa fa-file-text-o\" aria-hidden=\"true\"></i> Xem đơn dịch vụ ship</a>";
                    }
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
        public static string findReturnOrder(string order, string remove)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (remove.ToInt() == 0)
            {
                var or = RefundGoodController.GetOrderByID(order.ToInt());
                if (or != null)
                {
                    return serializer.Serialize(or);
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

        [WebMethod]
        public static string UpdateStatus(int OrderID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string username = HttpContext.Current.Request.Cookies["userLoginSystem"].Value;
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
            string username = HttpContext.Current.Request.Cookies["userLoginSystem"].Value;
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
            string username = Request.Cookies["userLoginSystem"].Value;
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
                            int AgentID = Convert.ToInt32(acc.AgentID);
                            int OrderType = hdfOrderType.Value.ToInt();
                            string AdditionFee = "0";
                            string DisCount = "0";
                            int CustomerID = 0;

                            string CustomerPhone = txtPhone.Text.Trim().Replace(" ", "");
                            string CustomerName = txtFullname.Text.Trim();
                            string Nick = txtNick.Text.Trim();
                            string CustomerAddress = txtAddress.Text.Trim();
                            string Zalo = txtZalo.Text.Trim();
                            string Facebook = txtFacebook.Text.Trim();

                            int PaymentType = ddlPaymentType.SelectedValue.ToInt(0);
                            int ShippingType = ddlShippingType.SelectedValue.ToInt(0);
                            int TransportCompanyID = ddlTransportCompanyID.SelectedValue.ToInt(0);
                            int TransportCompanySubID = ddlTransportCompanySubID.SelectedValue.ToInt(0);
                            int PostalDeliveryType = ddlPostalDeliveryType.SelectedValue.ToInt();

                            var Customer = CustomerController.GetByPhone(CustomerPhone);
                            if (Customer != null)
                            {
                                CustomerID = Customer.ID;
                                string kq = CustomerController.Update(CustomerID, CustomerName, Customer.CustomerPhone, CustomerAddress, "", Convert.ToInt32(Customer.CustomerLevelID), Convert.ToInt32(Customer.Status), Customer.CreatedBy, currentDate, username, false, Zalo, Facebook, Customer.Note, Customer.ProvinceID.ToString(), Nick, Customer.Avatar, ShippingType, PaymentType, TransportCompanyID, TransportCompanySubID, Customer.CustomerPhone2);
                            }
                            else
                            {
                                string kq = CustomerController.Insert(CustomerName, CustomerPhone, CustomerAddress, "", 0, 0, currentDate, username, false, Zalo, Facebook, "", "", Nick, "", ShippingType, PaymentType, TransportCompanyID, TransportCompanySubID);
                                if (kq.ToInt(0) > 0)
                                {
                                    CustomerID = kq.ToInt(0);
                                }
                            }

                            string totalPrice = hdfTotalPrice.Value.ToString();

                            string totalPriceNotDiscount = hdfTotalPriceNotDiscount.Value;
                            int PaymentStatusOld = Convert.ToInt32(order.PaymentStatus);
                            int ExcuteStatusOld = Convert.ToInt32(order.ExcuteStatus);
                            int PaymentStatus = ddlPaymentStatus.SelectedValue.ToInt(0);
                            int ExcuteStatus = ddlExcuteStatus.SelectedValue.ToInt(0);

                            string ShippingCode = txtShippingCode.Text.Trim().Replace("#", "").Replace(" ", "");
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

                            string datedone = "";
                            if (order.DateDone != null)
                            {
                                datedone = order.DateDone.ToString();
                            }

                            if (order.ExcuteStatus != 2)
                            {
                                if (ExcuteStatus == 2)
                                {
                                    if (string.IsNullOrEmpty(order.DateDone.ToString()))
                                    {
                                        datedone = DateTime.Now.ToString();
                                    }
                                }
                            }
                            else
                            {
                                if (ExcuteStatus == 2)
                                {
                                    DateTime old = Convert.ToDateTime(order.DateDone).Date;
                                    if (old == DateTime.Now.Date)
                                    {
                                        datedone = DateTime.Now.ToString();
                                    }
                                }
                            }

                            string ret = OrderController.UpdateOnSystem(OrderID, OrderType, AdditionFee, DisCount, CustomerID, CustomerName, CustomerPhone,
                                CustomerAddress, "", totalPrice, totalPriceNotDiscount, PaymentStatus, ExcuteStatus, currentDate, username,
                                Convert.ToDouble(pDiscount.Value), TotalDiscount, FeeShipping, Convert.ToDouble(order.GuestPaid), Convert.ToDouble(order.GuestChange), PaymentType, ShippingType, OrderNote, datedone, 0, ShippingCode, TransportCompanyID, TransportCompanySubID, String.Empty, 0, PostalDeliveryType);

                            // Insert Other Fee
                            if (!String.IsNullOrEmpty(hdfOtherFees.Value))
                            {
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                var fees = serializer.Deserialize<List<Fee>>(hdfOtherFees.Value);
                                if (fees != null)
                                {
                                    foreach (var fee in fees)
                                    {
                                        fee.OrderID = OrderID;
                                        fee.CreatedBy = acc.ID;
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

                            // Xử lý hủy đơn hàng
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
                                            MoveProID = 0,
                                            ParentID = parentID
                                        });
                                }

                                PJUtils.ShowMessageBoxSwAlertCallFunction("Hủy đơn hàng thành công", "s", true, "", Page);

                                Response.Redirect("/danh-sach-don-hang");
                                return;
                            }

                            if (OrderID > 0)
                            {
                                string list = hdfListProduct.Value;
                                string[] items = list.Split(';');
                                if (items.Length - 1 > 0)
                                {
                                    for (int i = 0; i < items.Length - 1; i++)
                                    {
                                        var item = items[i];
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
                                        
                                        // Xử lý với trạng thái của đơn hàng đã hủy
                                        if (ExcuteStatusOld == 3)
                                        {
                                            var orderDetail = OrderDetailController.GetByID(OrderDetailID);

                                            if(orderDetail != null)
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
                                                    IsCount = true
                                                });
                                            }

                                            StockManagerController.Insert(
                                                new tbl_StockManager {
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
                                                    MoveProID = 0,
                                                    ParentID = parentID,
                                                });

                                            continue;
                                        }

                                        // kiểm tra sản phẩm này đã có trong đơn chưa?
                                        
                                        var od = OrderDetailController.GetByID(OrderDetailID);
                                        
                                        if (od != null) // nếu sản phẩm này có trong đơn có rồi thì chỉnh sửa
                                        {
                                            if (od.IsCount == true)
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
                                                            MoveProID = 0,
                                                            ParentID = parentID,
                                                        });
                                                }
                                            }
                                            else
                                            {
                                                StockManagerController.Insert(
                                                    new tbl_StockManager
                                                    {
                                                        AgentID = AgentID,
                                                        ProductID = ProductID,
                                                        ProductVariableID = ProductVariableID,
                                                        Quantity = Quantity,
                                                        QuantityCurrent = 0,
                                                        Type = 2,
                                                        NoteID = "Xuất kho thêm mới sản phẩm khi sửa đơn",
                                                        OrderID = OrderID,
                                                        Status = 3,
                                                        SKU = SKU,
                                                        CreatedDate = currentDate,
                                                        CreatedBy = username,
                                                        MoveProID = 0,
                                                        ParentID = parentID,
                                                    });
                                                OrderDetailController.UpdateIsCount(OrderDetailID, true);
                                            }

                                            // cập nhật số lượng sản phẩm trong đơn hàng
                                            OrderDetailController.UpdateQuantity(OrderDetailID, Quantity, Price, currentDate, username);
                                        }
                                        // nếu sản phẩm này chưa có trong đơn thì thêm vào
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
                                                    MoveProID = 0,
                                                    ParentID = parentID,
                                                });
                                        }
                                    }
                                }

                                // thêm đơn hàng đổi trả
                                string refund = hdSession.Value;
                                // case click "bo qua"
                                if (refund == "0")
                                {
                                    if (hdfcheckR.Value != "")
                                    {
                                        string[] b = hdfcheckR.Value.Split(',');
                                        var update_returnorder = RefundGoodController.UpdateStatus(b[0].ToInt(), acc.Username, 1, 0);
                                        var update_order = OrderController.UpdateRefund(OrderID, null, acc.Username);
                                    }
                                }
                                else if (refund != "1")
                                {
                                    string[] RefundID = refund.Split('|');
                                    var update_returnorder = RefundGoodController.UpdateStatus(RefundID[0].ToInt(), acc.Username, 2, OrderID);
                                    var update_order = OrderController.UpdateRefund(OrderID, RefundID[0].ToInt(), acc.Username);

                                    if(hdfcheckR.Value != "")
                                    {
                                        string[] k = hdfcheckR.Value.Split(',');
                                        if (k[0] != RefundID[0])
                                        {
                                            var update_oldreturnorder = RefundGoodController.UpdateStatus(k[0].ToInt(), acc.Username, 1, 0);
                                        }
                                    }
                                }

                                PJUtils.ShowMessageBoxSwAlertCallFunction("Cập nhật đơn hàng thành công", "s", true, "", Page);
                            }
                        }
                    }
                }
            }
        }
    }
}