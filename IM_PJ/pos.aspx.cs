using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Utils;
using MB.Extensions;
using Newtonsoft.Json;
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

namespace IM_PJ
{
    public partial class pos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 600;

            if (!IsPostBack)
            {

                if (Request.Cookies["usernameLoginSystem"] != null)
                {
                    hdStatusPage.Value = "Create";
                    string username = Request.Cookies["usernameLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);
                    if (acc != null)
                    {
                        hdfUsernameCurrent.Value = acc.Username;

                        if (acc.RoleID == 0)
                        {
                            hdfRoleID.Value = acc.RoleID.ToString();
                        }
                        else if (acc.RoleID == 2)
                        {
                            hdfRoleID.Value = acc.RoleID.ToString();
                            hdfUsername.Value = acc.Username;
                        }
                        else
                        {
                            Response.Redirect("/trang-chu");
                        }

                        hdSession.Value = "1";

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

                        // get user list (only role 2) to change user when create order
                        var CreateBy = AccountController.GetAllByRoleID(2);
                        if (CreateBy != null)
                        {
                            string listUser = "";
                            foreach (var item in CreateBy)
                            {
                                if (item.Username != acc.Username)
                                {
                                    listUser += item.Username + "|";
                                }
                            }
                            hdfListUser.Value = listUser;
                        }
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
                LoadPage();
            }
        }

        public void LoadPage()
        {
            // Fix bug, case setting value for pDiscount on HTML but don't change value
            pDiscount.Value = 1;
            pFeeShip.Value = 1;

            // Init drop down list ddlFeeType
            var feeTypes = FeeTypeController.getDropDownList();
            feeTypes[0].Text = "Loại phí";
            ddlFeeType.Items.Clear();
            ddlFeeType.Items.AddRange(feeTypes.ToArray());
            ddlFeeType.DataBind();
            ddlFeeType.SelectedIndex = 0;

            // Init Price Type List
            hdfFeeType.Value = FeeTypeController.getFeeTypeJSON();
        }
        [WebMethod]
        public static string searchCustomerByPhone(string phone)
        {
            var customer = CustomerController.GetByPhone(phone);
            if (customer != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(customer);
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        public static string searchCustomerByText(string textsearch, string createdby = "")
        {
            string search = Regex.Replace(textsearch.Trim(), @"[^0-9a-zA-Z\s_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+", "");
            var customer = CustomerController.Find(search, createdby);
            if (customer.Count > 0)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(new { listCustomer = customer, employee = 0 });
            }
            else
            {
                var customer_other = CustomerController.Find(search);
                if (customer_other.Count > 0)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    return serializer.Serialize(new { listCustomer = customer_other, employee = 1 });
                }
                else
                {
                    return null;
                }
            }
        }

        [WebMethod]
        public static string getOrderReturn(int customerID)
        {
            return RefundGoodController.getOrderReturnJSON(customerID);
        }

        [WebMethod]
        public static string getCustomerDetail(int ID)
        {
            var customer = CustomerController.GetByID(ID);
            if (customer != null)
            {
                var ci = new CustomerInfoWithDiscount();
                ci.Customer = customer;
                ci.CreatedDate = customer.CreatedDate.ToString();
                List<DiscountCustomerGet> dc = new List<DiscountCustomerGet>();
                var d = DiscountCustomerController.getbyCustID(ID);
                if (d.Count > 0)
                {
                    dc = d;
                }
                ci.AllDiscount = dc;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(ci);
            }
            else
            {
                return "null";
            }
        }
        [WebMethod]
        public static string getCustomerDiscount(int ID)
        {
            var now = DateTime.Now;
            var discount = DiscountCustomerController.getbyCustID(ID).FirstOrDefault();
            var config = ConfigController.GetByTop1();
            // Khởi tao object json
            bool isDiscount;
            var isUserApp = UserController.checkExists(userID: ID);
            var ci = new CustomerGroup();
            var promotion = RefundGoodController.getPromotion(customerID: ID);


            if (discount != null)
            {
                // Phí đổi trả
                var feeRefund = discount.FeeRefund;
                // Tính số lượng hàng trã có phí
                var refundQuantityFee = Convert.ToInt32(discount.NumOfProductCanChange - discount.RefundQuantityNoFee);
                // Tính số lượng hàng đổi trả trong 30 ngày
                var fromDate = now.AddDays(-discount.NumOfDateToChangeProduct).Date;
                var toDate = now.Date;
                var customer = CustomerController.getRefundQuantity(ID, fromDate, toDate).FirstOrDefault();
                var quantityNoFree = discount.RefundQuantityNoFee - (customer != null ? Convert.ToInt32(customer.refundNoFeeQuantity) : 0);
                var quantityFree = refundQuantityFee - (customer != null ? Convert.ToInt32(customer.refundFeeQuantity) : 0);

                isDiscount = discount.DiscountAmount > 0;
                quantityNoFree = quantityNoFree > 0 ? quantityNoFree : 0;
                quantityFree = quantityFree > 0 ? quantityFree : 0;

                if (promotion.IsPromotion)
                {
                    isDiscount = true;
                    feeRefund = feeRefund - promotion.DecreasePrice;

                    if (feeRefund <= 0)
                    {
                        feeRefund = 0;
                        quantityNoFree = quantityFree + quantityNoFree;
                        quantityFree = 0;
                    }
                }

                ci.IsDiscount = isDiscount;
                ci.Discount = discount.DiscountAmount.ToString();
                ci.QuantityProduct = discount.QuantityProduct;
                ci.FeeRefund = feeRefund.ToString();
                ci.DaysExchange = Convert.ToInt32(discount.NumOfDateToChangeProduct);
                ci.RefundQuantityNoFee = quantityNoFree;
                ci.RefundQuantityFee = quantityFree;
                ci.IsUserApp = isUserApp;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(ci);
            }
            else if (config != null)
            {
                // Phí đổi trả
                var feeRefund = config.FeeChangeProduct;
                // Tính số lượng hàng trã có phí
                var refundQuantityFee = config.NumOfProductCanChange.HasValue ? Convert.ToInt32(config.NumOfProductCanChange.Value) : 0;
                // Tính số lượng hàng đổi trả trong 30 ngày
                var days = config.NumOfDateToChangeProduct.HasValue ? config.NumOfDateToChangeProduct.Value : 0;
                var fromDate = now.AddDays(-days).Date;
                var toDate = now.Date;
                var customer = CustomerController.getRefundQuantity(ID, fromDate, toDate).FirstOrDefault();
                var quantityFree = refundQuantityFee - (customer != null ? Convert.ToInt32(customer.refundNoFeeQuantity) : 0) - (customer != null ? Convert.ToInt32(customer.refundFeeQuantity) : 0);

                isDiscount = config.FeeDiscountPerProduct > 0;

                if (promotion.IsPromotion)
                {
                    isDiscount = true;
                    feeRefund = (feeRefund - promotion.DecreasePrice) < 0 ? 0 : feeRefund - promotion.DecreasePrice;
                }

                ci.IsDiscount = isDiscount;
                ci.Discount = config.FeeDiscountPerProduct.ToString();
                ci.QuantityProduct = 0;
                ci.FeeRefund = feeRefund.ToString();
                ci.DaysExchange = Convert.ToInt32(config.NumOfDateToChangeProduct);
                ci.RefundQuantityNoFee = 0;
                ci.RefundQuantityFee = quantityFree > 0 ? quantityFree : 0;
                ci.IsUserApp = isUserApp;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(ci);
            }
            else
            {

                return "null";
            }

        }

        public class ProductGetOut
        {
            public int ProductID { get; set; }
            public int ProductVariableID { get; set; }
            public string ProductName { get; set; }
            public string ProductVariable { get; set; }
            public string ProductVariableSave { get; set; }
            public string ProductVariableName { get; set; }
            public string ProductVariableValue { get; set; }
            public int ProductType { get; set; }
            public string ProductImage { get; set; }
            public string ProductImageOrigin { get; set; }
            public string QuantityInstockString { get; set; }
            public double QuantityInstock { get; set; }
            public string SKU { get; set; }
            public double Giabanle { get; set; }
            public string stringGiabanle { get; set; }
            public double Giabansi { get; set; }
            public string stringGiabansi { get; set; }
        }

        public class ProductPOS
        {
            public List<ProductGetOut> productPOS { get; set; }
        }

        public class CustomerGroup
        {
            public bool IsDiscount { get; set; }
            public string Discount { get; set; }
            public int QuantityProduct { get; set; }
            public string FeeRefund { get; set; }
            public int DaysExchange { get; set; }
            public int RefundQuantityNoFee { get; set; }
            public int RefundQuantityFee { get; set; }
            public bool IsUserApp { get; set; }
        }

        public class CustomerInfoWithDiscount
        {
            public tbl_Customer Customer { get; set; }
            public string CreatedDate { get; set; }
            public string ProvinceName { get; set; }
            public List<DiscountCustomerGet> AllDiscount { get; set; }
        }

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                string username = Request.Cookies["usernameLoginSystem"].Value;
                var acc = AccountController.GetByUsername(username);
                if (acc != null)
                {
                    if (acc.RoleID == 0 || acc.RoleID == 2)
                    {
                        #region Lấy thông tin khởi tạo Order
                        // Change user
                        string UserHelp = "";
                        if (username != hdfUsernameCurrent.Value)
                        {
                            UserHelp = username;
                            username = hdfUsernameCurrent.Value;
                        }

                        int AgentID = Convert.ToInt32(acc.AgentID);
                        int OrderType = hdfOrderType.Value.ToInt();
                        string AdditionFee = "0";
                        string DisCount = "0";
                        int CustomerID = 0;

                        string CustomerPhone = Regex.Replace(txtPhone.Text.Trim(), @"[^\d]", "");
                        string CustomerName = txtFullname.Text.Trim().ToLower().ToTitleCase();
                        string CustomerEmail = "";
                        string CustomerAddress = txtAddress.Text.Trim().ToTitleCase();

                        int ProvinceID = hdfProvinceID.Value.ToInt(0);
                        int DistrictID = hdfDistrictID.Value.ToInt(0);
                        int WardID = hdfWardID.Value.ToInt(0);

                        var checkCustomer = CustomerController.GetByPhone(CustomerPhone);

                        string kq = "";

                        #region Cập nhật thông tin khách hàng
                        if (checkCustomer != null)
                        {
                            CustomerID = checkCustomer.ID;
                            kq = CustomerController.Update(CustomerID, CustomerName, checkCustomer.CustomerPhone, CustomerAddress, "", checkCustomer.CustomerLevelID.Value, checkCustomer.Status.Value, checkCustomer.CreatedBy, currentDate, username, false, checkCustomer.Zalo, checkCustomer.Facebook, checkCustomer.Note, checkCustomer.Nick, checkCustomer.Avatar, checkCustomer.ShippingType.Value, checkCustomer.PaymentType.Value, checkCustomer.TransportCompanyID.Value, checkCustomer.TransportCompanySubID.Value, checkCustomer.CustomerPhone2, ProvinceID, DistrictID, WardID);
                        }
                        else
                        {
                            kq = CustomerController.Insert(CustomerName, CustomerPhone, CustomerAddress, CustomerEmail, 0, 0, currentDate, username, false, "", "", "", "", "", 0, 0, 0, 0, "", ProvinceID, DistrictID, WardID);
                            if (kq.ToInt(0) > 0)
                            {
                                CustomerID = kq.ToInt(0);
                            }
                        }
                        #endregion

                        string totalPrice = hdfTotalPrice.Value.ToString();
                        string totalPriceNotDiscount = hdfTotalPriceNotDiscount.Value;
                        int PaymentStatus = 3;
                        int ExcuteStatus = 2;
                        int PaymentType = 1;
                        int ShippingType = 1;

                        bool IsHidden = false;
                        int WayIn = 1;

                        double DiscountPerProduct = Convert.ToDouble(pDiscount.Value);

                        double TotalDiscount = Convert.ToDouble(pDiscount.Value) * Convert.ToDouble(hdfTotalQuantity.Value);
                        string FeeShipping = pFeeShip.Value.ToString();
                        double GuestPaid = Convert.ToDouble(pGuestPaid.Value);
                        double GuestChange = Convert.ToDouble(totalPrice) - GuestPaid;
                        var couponID = hdfCouponID.Value.ToInt(0);
                        var couponValue = hdfCouponValue.Value.ToDecimal(0);

                        // Cập nhật địa chỉ giao hàng
                        var deliveryAddressId = Convert.ToInt64(hdfDeliveryAddressId.Value);

                        tbl_Order order = new tbl_Order()
                        {
                            AgentID = AgentID,
                            OrderType = OrderType,
                            AdditionFee = AdditionFee,
                            DisCount = DisCount,
                            CustomerID = CustomerID,
                            CustomerName = CustomerName,
                            CustomerPhone = CustomerPhone,
                            CustomerAddress = CustomerAddress,
                            CustomerEmail = CustomerEmail,
                            TotalPrice = totalPrice,
                            TotalPriceNotDiscount = totalPriceNotDiscount,
                            PaymentStatus = PaymentStatus,
                            ExcuteStatus = ExcuteStatus,
                            IsHidden = IsHidden,
                            WayIn = WayIn,
                            CreatedDate = currentDate,
                            CreatedBy = username,
                            DiscountPerProduct = DiscountPerProduct,
                            TotalDiscount = TotalDiscount,
                            FeeShipping = FeeShipping,
                            GuestPaid = GuestPaid,
                            GuestChange = GuestChange,
                            PaymentType = PaymentType,
                            ShippingType = ShippingType,
                            OrderNote = String.Empty,
                            DateDone = DateTime.Now,
                            OtherFeeName = String.Empty,
                            OtherFeeValue = 0,
                            PostalDeliveryType = 1,
                            UserHelp = UserHelp,
                            CouponID = couponID,
                            CouponValue = couponValue,
                            DeliveryAddressId = deliveryAddressId
                        };

                        var ret = OrderController.InsertOnSystem(order);

                        int OrderID = ret.ID;
                        #endregion

                        #region Khởi tạo Other Fee
                        if (!String.IsNullOrEmpty(hdfOtherFees.Value))
                        {
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            var fees = serializer.Deserialize<List<Fee>>(hdfOtherFees.Value);
                            if (fees != null)
                            {
                                foreach (var fee in fees)
                                {
                                    fee.OrderID = ret.ID;
                                    fee.CreatedBy = acc.ID;
                                    fee.CreatedDate = DateTime.Now;
                                    fee.ModifiedBy = acc.ID;
                                    fee.ModifiedDate = DateTime.Now;
                                }

                                FeeController.Update(ret.ID, fees);
                            }
                        }
                        #endregion

                        #region Cập nhật Coupon
                        if (order.CouponID.HasValue && order.CouponID.Value > 0)
                        {
                            CouponController.updateStatusCouponCustomer(CustomerID, order.CouponID.Value, false);
                        }
                        #endregion

                        if (OrderID > 0)
                        {
                            #region Khởi tạo chi tiết đơn hàng
                            var orderAvatar = String.Empty;
                            ProductPOS POS = JsonConvert.DeserializeObject<ProductPOS>(hdfListProduct.Value);
                            List<tbl_OrderDetail> orderDetails = new List<tbl_OrderDetail>();
                            List<tbl_StockManager> stockManager = new List<tbl_StockManager>();

                            // Reverser
                            POS.productPOS.Reverse();

                            for(var i = 0; i < POS.productPOS.Count; i++)
                            {
                                var item = POS.productPOS[i];

                                #region Cập nhật avatar cho đơn hàng
                                if (String.IsNullOrEmpty(orderAvatar))
                                    orderAvatar = AnnImage.extractImage(item.ProductImageOrigin);
                                #endregion

                                orderDetails.Add(
                                    new tbl_OrderDetail()
                                    {
                                        AgentID = AgentID,
                                        OrderID = OrderID,
                                        SKU = item.SKU,
                                        ProductID = item.ProductType == 1 ? item.ProductID : 0,
                                        ProductVariableID = item.ProductType == 1 ? 0 : item.ProductVariableID,
                                        ProductVariableDescrition = item.ProductVariableSave,
                                        Quantity = item.QuantityInstock,
                                        Price = item.Giabanle,
                                        Status = 1,
                                        DiscountPrice = 0,
                                        ProductType = item.ProductType,
                                        CreatedDate = currentDate,
                                        CreatedBy = username,
                                        IsCount = true
                                    }
                                );

                                int parentID = item.ProductID;
                                var variable = ProductVariableController.GetByID(item.ProductVariableID);
                                if (variable != null)
                                {
                                    parentID = Convert.ToInt32(variable.ProductID);
                                }

                                stockManager.Add(
                                    new tbl_StockManager()
                                    {
                                        AgentID = AgentID,
                                        ProductID = item.ProductType == 1 ? item.ProductID : 0,
                                        ProductVariableID = item.ProductType == 1 ? 0 : item.ProductVariableID,
                                        Quantity = item.QuantityInstock,
                                        QuantityCurrent = 0,
                                        Type = 2,
                                        NoteID = "Xuất kho bán POS",
                                        OrderID = OrderID,
                                        Status = 3,
                                        SKU = item.SKU,
                                        CreatedDate = currentDate,
                                        CreatedBy = username,
                                        ModifiedDate = currentDate,
                                        ModifiedBy = username,
                                        MoveProID = 0,
                                        ParentID = parentID
                                    }
                                );
                            }

                            OrderDetailController.Insert(orderDetails);
                            #endregion

                            // Cập nhật lại sô lượng và giá vố vào đơn hàng
                            OrderController.updateAvatarQuantityCOGS(OrderID, orderAvatar);
                            // Cập nhật lại thông tin kho hàng
                            StockManagerController.Insert(stockManager);

                            #region Khởi tạo đơn hàng đổi trả
                            string refund = hdSession.Value;
                            if (refund != "1")
                            {
                                string[] RefundID = refund.Split('|');
                                var update = RefundGoodController.UpdateStatus(RefundID[0].ToInt(), username, 2, OrderID);
                                var updateor = OrderController.UpdateRefund(OrderID, RefundID[0].ToInt(), username);
                            }
                            #endregion

                            #region push notify telegram
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                            var pushOrder = OrderController.GetByID(ret.ID);
                            var msg = "<b>" + pushOrder.ID + "</b> - " + DateTime.Now.ToString("dd/MM HH:mm");
                            msg += "\r\n- <b>TỔNG</b>: " + string.Format("{0:N0}", Convert.ToDouble(pushOrder.TotalPrice));
                            msg += "\r\n- <b>Số lượng</b>: " + pushOrder.TotalQuantity + " cái";
                            msg += "\r\n- <b>Thanh toán</b>: Tiền mặt";
                            msg += "\r\n- <b>Giao hàng</b>: Lấy trực tiếp";
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
                            if (checkCustomer != null && !string.IsNullOrEmpty(checkCustomer.Nick))
                            {
                                msg += "\r\n- <b>Nick</b>: " + checkCustomer.Nick;
                            }
                            msg += "\r\n- <b>Điện thoại</b>: " + pushOrder.CustomerPhone;
                            msg += "\r\n- <b>Nhân viên</b>: " + pushOrder.CreatedBy;

                            var chatID = "-1001229080769";
                            var token = "bot1714400602:AAHlWZhq4IZZ18wCQxVVGA4kuZJQPkb50z0";
                            var api = "https://api.telegram.org/" + token + "/sendMessage?parse_mode=html&chat_id=" + chatID + "&text=" + HttpUtility.UrlEncode(msg);
                            WebRequest request = WebRequest.Create(api);
                            Stream rs = request.GetResponse().GetResponseStream();
                            #endregion

                            // Hoàn thành khởi tạo đơn hàng nên gán lại giá trị trang lúc ban đầu
                            hdStatusPage.Value = "Create";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { HoldOn.close(); printInvoice(" + OrderID + ") });", true);
                        }
                    }
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { handleErrorSubmit(); });", true);
            }
        }

        [WebMethod]
        public static string getCoupon(int customerID, string code, int productNumber, decimal price)
        {
            return CouponController.getCoupon(customerID, code, productNumber, price);
        }
    }
}