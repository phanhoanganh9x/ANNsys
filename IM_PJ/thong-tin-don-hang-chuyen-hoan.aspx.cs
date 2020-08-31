using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Utils;
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
    public partial class thong_tin_don_hang_chuyen_hoan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);
                    if (acc != null)
                    {
                        
                        // check user role
                        if (acc.RoleID == 0)
                        {

                        }
                        else if (acc.RoleID == 2)
                        {
                            hdfUsername.Value = acc.Username;
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
                if(order == null)
                {
                    PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy đơn hàng " + ID, "e", true, "/danh-sach-don-hang-chuyen-hoan", Page);
                }
                else
                {
                    hdfOrderID.Value = order.ID.ToString();
                    string username = HttpContext.Current.Request.Cookies["usernameLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);

                    // check order condition
                    if(acc.RoleID != 0)
                    {
                        if (order.ExcuteStatus == 4)
                        {
                            PJUtils.ShowMessageBoxSwAlertError("Đơn hàng này đã chuyển hoàn", "w", false, "", Page);
                        }
                        else
                        {
                            if (order.CreatedBy != acc.Username)
                            {
                                PJUtils.ShowMessageBoxSwAlertError("Đơn hàng này không phải của bạn", "e", true, "/danh-sach-don-hang-chuyen-hoan", Page);
                            }

                            if (order.ExcuteStatus == 1)
                            {
                                PJUtils.ShowMessageBoxSwAlertError("Đơn hàng này chưa hoàn tất nên không thể chuyển hoàn", "e", true, "/danh-sach-don-hang-chuyen-hoan", Page);
                            }

                            if (order.ExcuteStatus == 3)
                            {
                                PJUtils.ShowMessageBoxSwAlertError("Đơn hàng này đã hủy nên không thể chuyển hoàn", "e", true, "/danh-sach-don-hang-chuyen-hoan", Page);
                            }

                            if (order.ShippingType == 1)
                            {
                                PJUtils.ShowMessageBoxSwAlertError("Đơn hàng này lấy trực tiếp nên không thể chuyển hoàn", "e", true, "/danh-sach-don-hang-chuyen-hoan", Page);
                            }
                        }
                    }
                    else
                    {
                        if (order.ExcuteStatus == 4)
                        {
                            PJUtils.ShowMessageBoxSwAlertError("Đơn hàng này đã chuyển hoàn", "w", false, "", Page);
                        }
                        else
                        {
                            if (order.ExcuteStatus == 1)
                            {
                                PJUtils.ShowMessageBoxSwAlertError("Đơn hàng này chưa hoàn tất nên không thể chuyển hoàn", "e", true, "/danh-sach-don-hang-chuyen-hoan", Page);
                            }

                            if (order.ExcuteStatus == 3)
                            {
                                PJUtils.ShowMessageBoxSwAlertError("Đơn hàng này đã hủy nên không thể chuyển hoàn", "e", true, "/danh-sach-don-hang-chuyen-hoan", Page);
                            }

                            if (order.ShippingType == 1)
                            {
                                PJUtils.ShowMessageBoxSwAlertError("Đơn hàng này lấy trực tiếp nên không thể chuyển hoàn", "e", true, "/danh-sach-don-hang-chuyen-hoan", Page);
                            }
                        }
                        
                    }

                    ViewState["ID"] = ID;

                    Response.Cookies["odid"].Value = ID.ToString();

                    int AgentID = Convert.ToInt32(order.AgentID);
                    txtPhone.Text = order.CustomerPhone;
                    txtFullname.Text = order.CustomerName;
                    txtAddress.Text = order.CustomerAddress;
                    var cus = CustomerController.GetByID(order.CustomerID.Value);
                    if (cus != null)
                    {
                        txtNick.Text = cus.Nick;
                        
                        txtZalo.Text = cus.Zalo;

                        txtFacebook.Text = cus.Facebook;
                        if (!string.IsNullOrEmpty(cus.Facebook))
                        {
                            ltrFb.Text += "<a href =\"" + cus.Facebook + "\" class=\"btn primary-btn fw-btn not-fullwidth\" target=\"_blank\">Xem</a>";
                        }
                    }
                    int customerID = Convert.ToInt32(order.CustomerID);
                    ltrViewDetail.Text = "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth\" onclick=\"viewCustomerDetail('" + customerID + "')\"><i class=\"fa fa-address-card-o\" aria-hidden=\"true\"></i> Xem chi tiết</a>";
                    ltrViewDetail.Text += "<a href=\"chi-tiet-khach-hang?id=" + customerID + "\" class=\"btn primary-btn fw-btn not-fullwidth edit-customer-btn\" target=\"_blank\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Chỉnh sửa</a>";

                    var d = DiscountCustomerController.getbyCustID(customerID);
                    if (d.Count > 0)
                    {
                        var da = d[0].DiscountAmount;
                        hdfIsDiscount.Value = "1";
                        hdfDiscountAmount.Value = da.ToString();
                        ltrDiscountInfo.Text = "<strong>Khách hàng được chiết khấu: " + string.Format("{0:N0}", Convert.ToDouble(da)) + " vnđ/sản phẩm.</strong>";
                    }
                    else
                    {
                        hdfIsDiscount.Value = "0";
                        hdfDiscountAmount.Value = "0";
                    }

                    int customerType = Convert.ToInt32(order.OrderType);
                    ltrCustomerType.Text = "";
                    ltrCustomerType.Text += "<select disabled class=\"form-control customer-type\" onchange=\"getProductPrice($(this))\">";
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
                    string html = "";
                    string Print = "";
                    if (orderdetails.Count > 0)
                    {
                        int t = 0;
                        int orderitem = 0;
                        foreach (var item in orderdetails)
                        {
                            ProductQuantity += Convert.ToDouble(item.Quantity);

                            int ProductType = Convert.ToInt32(item.ProductType);
                            int ProductID = Convert.ToInt32(item.ProductID);
                            int ProductVariableID = Convert.ToInt32(item.ProductVariableID);
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
                            int PID = 0;
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
                                PID = ProductID;
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

                                    var img = ProductImageController.GetFirstByProductID(product.ID);
                                    if (!string.IsNullOrEmpty(product.ProductImage))
                                    {
                                        ProductImage = "<img src='" + Thumbnail.getURL(product.ProductImage, Thumbnail.Size.Small) + "'>";
                                        ProductImageOrigin = Thumbnail.getURL(product.ProductImage, Thumbnail.Size.Source);
                                    }
                                    else if (img != null)
                                    {
                                        ProductImage = "<img src='" + Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Small) + "'>";
                                        ProductImageOrigin = Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Source);
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
                                PID = ProductVariableID;
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

                                    if (!string.IsNullOrEmpty(productvariable.Image))
                                    {
                                        ProductImage = "<img src='" + Thumbnail.getURL(productvariable.Image, Thumbnail.Size.Small) + "'>";
                                        ProductImageOrigin = Thumbnail.getURL(productvariable.Image, Thumbnail.Size.Source);
                                    }
                                    else if (_product != null && !string.IsNullOrEmpty(_product.ProductImage))
                                    {
                                        ProductImage = "<img src='" + Thumbnail.getURL(_product.ProductImage, Thumbnail.Size.Small) + "'>";
                                        ProductImageOrigin = Thumbnail.getURL(_product.ProductImage, Thumbnail.Size.Source);
                                    }

                                    ProductVariable = variable;

                                    if (_product != null)
                                        ProductName = _product.ProductTitle;

                                    QuantityMainInstock = mainstock;
                                    QuantityMainInstockString = string.Format("{0:N0}", mainstock);
                                    ProductVariableSave = item.ProductVariableDescrition;

                                    ProductVariableName = variablename;
                                    ProductVariableValue = variablevalue;
                                }
                            }
                            orderitem++;
                            html += "<tr class=\"product-result\" data-orderdetailid=\"" + item.ID + "\" data-giabansi=\"" + Giabansi + "\" data-giabanle=\"" + Giabanle + "\" " +
                                                "data-quantityinstock=\"" + QuantityInstock + "\" data-productimageorigin=\"" + ProductImageOrigin + "\" " +
                                                "data-productvariable=\"" + ProductVariable + "\" data-productname=\"" + ProductName + "\" " +
                                                "data-sku=\"" + SKU + "\" data-producttype=\"" + ProductType + "\" data-id=\"" + PID + "\" " +
                                                "data-productnariablename=\"" + ProductVariableName + "\" " +
                                                "data-productvariablevalue =\"" + ProductVariableValue + "\" " +
                                                "data-productvariablesave =\"" + ProductVariableSave + "\" " +
                                                "data-quantitymaininstock=\"" + QuantityMainInstock + "\">";
                            html += "   <td class=\"order-item\">" + orderitem + "";
                            html += "   <td class=\"image-item\">" + ProductImage + "";
                            html += "   <td class=\"name-item\">" + ProductName + "</td>";
                            html += "   <td class=\"sku-item\">" + SKU + "</td>";
                            html += "   <td class=\"variable-item\">" + ProductVariable + "</td>";
                            html += "   <td class=\"price-item gia-san-pham\" data-price=\"" + ItemPrice + "\">" + string.Format("{0:N0}", ItemPrice) + "</td>";
                            html += "   <td class=\"quantity-item soluong\">" + QuantityInstockString + "</td>";
                            html += "   <td class=\"quantity-item\"><input disabled data-quantity=\"" + item.Quantity + "\" value=\"" + item.Quantity + "\" type=\"text\" class=\"form-control in-quanlity\" value=\"1\" onblur=\"checkQuantiy($(this))\" onkeypress='return event.charCode >= 48 && event.charCode <= 57'/></td>";
                            int k = Convert.ToInt32(ItemPrice) * Convert.ToInt32(item.Quantity);
                            html += "<td class=\"total-item totalprice-view\">" + string.Format("{0:N0}", k) + "</td>";
                            html += "   <td class=\"trash-item\"><a href=\"javascript:;\" class=\"link-btn\"><i class=\"fa fa-trash\"></i></a>    </td>";

                            html += "</tr>";


                            Print += " <tr>";
                            t++;
                            Print += "<td>" + t + "</td>";
                            Print += "<td>" + SKU + " - " + ProductName + " - " + ProductVariableSave.Replace("|", ", ") + "</td> ";
                            Print += "<td>" + item.Quantity + "</td>";
                            Print += "<td>" + string.Format("{0:N0}", ItemPrice) + "</td>";

                            Print += "<td> " + string.Format("{0:N0}", k) + "</td>";

                            Print += "</tr>";

                        }
                        ltrProducts.Text = html;

                    }
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

                    ltrOtherFeeName.Text = order.OtherFeeName;
                    txtOtherFeeName.Text = order.OtherFeeName;
                    pOtherFee.Value = Convert.ToDouble(order.OtherFeeValue);

                    ltrTotalAfterCK.Text = string.Format("{0:N0}", (Convert.ToDouble(order.TotalPriceNotDiscount) - Convert.ToDouble(order.TotalDiscount)));
                    ltrOrderID.Text = ID.ToString();
                    ltrCreateBy.Text = order.CreatedBy;
                    ltrCreateDate.Text = order.CreatedDate.ToString();
                    ltrDateDone.Text = "Chưa hoàn tất";
                    if (order.DateDone != null)
                    {
                        ltrDateDone.Text = order.DateDone.ToString();
                    }
                    ltrOrderNote.Text = order.OrderNote;
                    ltrOrderQuantity.Text = ProductQuantity.ToString();
                    ltrOrderTotalPrice.Text = string.Format("{0:N0}", Convert.ToDouble(order.TotalPrice));
                    ltrOrderStatus.Text = PJUtils.OrderExcuteStatus(Convert.ToInt32(order.ExcuteStatus));

                    ltrOrderType.Text = PJUtils.OrderType(Convert.ToInt32(order.OrderType));
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
                    ListItem listitem = new ListItem(p.CompanyName, p.ID.ToString());
                    ddlTransportCompanyID.Items.Add(listitem);
                }
                ddlTransportCompanyID.DataBind();
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
                        ListItem listitem = new ListItem(p.ShipTo, p.SubID.ToString());
                        ddlTransportCompanySubID.Items.Add(listitem);
                    }
                }
                ddlTransportCompanySubID.DataBind();
            }
        }

        protected void ddlTransportCompanyID_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTransportCompanySubID(ddlTransportCompanyID.SelectedValue.ToInt(0));
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

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Request.Cookies["usernameLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);

            if (acc != null)
            {
                if (acc.RoleID == 0 || acc.RoleID == 2)
                {
                    int OrderID = ViewState["ID"].ToString().ToInt(0);
                    if (OrderID > 0)
                    {
                        var order = OrderController.GetByID(OrderID);

                        if (order != null)
                        {
                            int ExcuteStatusOld = Convert.ToInt32(order.ExcuteStatus);

                            string OrderNote = txtOrderNote.Text;

                            // Xử lý nhập kho khi chuyển hoàn
                            if (ExcuteStatusOld != 4)
                            {
                                var orderDetails = OrderDetailController.GetByOrderID(order.ID);

                                foreach (tbl_OrderDetail product in orderDetails)
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
                                        new tbl_StockManager
                                        {
                                            AgentID = product.AgentID,
                                            ProductID = product.ProductID,
                                            ProductVariableID = product.ProductVariableID,
                                            Quantity = product.Quantity,
                                            QuantityCurrent = 0,
                                            Type = 1,
                                            NoteID = "Nhập kho do chuyển hoàn đơn " + product.OrderID,
                                            OrderID = product.OrderID,
                                            Status = 13,
                                            SKU = product.SKU,
                                            CreatedDate = currentDate,
                                            CreatedBy = product.CreatedBy,
                                            ModifiedDate = currentDate,
                                            ModifiedBy = product.CreatedBy,
                                            MoveProID = 0,
                                            ParentID = parentID
                                        });
                                }
                            }

                            bool updateOrder = OrderController.UpdateExcuteStatus4(order.ID, username, OrderNote);

                            if(updateOrder == true)
                            {
                                Response.Redirect("/thong-tin-don-hang-chuyen-hoan?id=" + order.ID);
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlertCallFunction("Đã xảy ra lỗi", "s", true, "payAllClicked()", Page);
                            }
                        }
                    }
                }
            }
        }

    }
}