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

namespace IM_PJ
{
    public partial class them_moi_don_hang : System.Web.UI.Page
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
                        if (acc.RoleID == 0)
                        {
                            hdfRoleID.Value = acc.RoleID.ToString();
                            hdfUsernameCurrent.Value = acc.Username;
                        }
                        else if (acc.RoleID == 2)
                        {
                            hdfRoleID.Value = acc.RoleID.ToString();
                            hdfUsername.Value = acc.Username;
                            hdfUsernameCurrent.Value = acc.Username;
                        }
                        else
                        {
                            Response.Redirect("/trang-chu");
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
                        var agent = acc.AgentID;

                        hdSession.Value = "1";

                        if (agent == 1)
                        {
                            hdfIsMain.Value = "1";
                        }
                        else
                        {
                            hdfIsMain.Value = "0";
                        }
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
                LoadData();
            }
        }
        public void LoadData()
        {
            
            // Fix bug, case setting value for pDiscount on HTML but don't change value
            pDiscount.Value = 1;
            pFeeShip.Value = 1;

            // Init drop down list ddlFeeType
            var feeTypes = FeeTypeController.getDropDownList();
            feeTypes[0].Text = "Loại Phí";
            ddlFeeType.Items.Clear();
            ddlFeeType.Items.AddRange(feeTypes.ToArray());
            ddlFeeType.DataBind();
            ddlFeeType.SelectedIndex = 0;

            // Init Price Type List
            hdfFeeType.Value = FeeTypeController.getFeeTypeJSON();

            // Init drop down list Excute Status
            var excuteStatus = new List<ListItem>();
            excuteStatus.Add(new ListItem("Đang xử lý", "1"));
            excuteStatus.Add(new ListItem("Đã hoàn tất", "2"));

            ddlExcuteStatus.Items.Clear();
            ddlExcuteStatus.Items.AddRange(excuteStatus.ToArray());
            ddlExcuteStatus.DataBind();
            ddlExcuteStatus.SelectedIndex = 0;

            // Init drop down list Payment Status
            var payStatus = new List<ListItem>();
            payStatus.Add(new ListItem("Chưa thanh toán", "1"));
            payStatus.Add(new ListItem("Thanh toán thiếu", "2"));
            payStatus.Add(new ListItem("Đã thanh toán", "3"));

            ddlPaymentStatus.Items.Clear();
            ddlPaymentStatus.Items.AddRange(payStatus.ToArray());
            ddlPaymentStatus.DataBind();
            ddlPaymentStatus.SelectedIndex = 0;

            // Init drop down list Payment Type
            var payType = new List<ListItem>();
            payType.Add(new ListItem("Tiền mặt", "1"));
            payType.Add(new ListItem("Chuyển khoản", "2"));
            payType.Add(new ListItem("Thu hộ", "3"));
            payType.Add(new ListItem("Công nợ", "4"));

            ddlPaymentType.Items.Clear();
            ddlPaymentType.Items.AddRange(payType.ToArray());
            ddlPaymentType.DataBind();
            ddlPaymentType.SelectedIndex = 1;

            // Init drop down list Bank
            var banks = BankController.getDropDownList();
            banks[0].Text = "Chọn ngân hàng";

            ddlBank.Items.Clear();
            ddlBank.Items.AddRange(banks.ToArray());
            ddlBank.DataBind();

            // Init drop down list Shipping Type
            var shipType = new List<ListItem>();
            shipType.Add(new ListItem("Lấy trực tiếp", "1"));
            shipType.Add(new ListItem("Bưu điện", "2"));
            shipType.Add(new ListItem("Proship", "3"));
            shipType.Add(new ListItem("Chuyển xe", "4"));
            shipType.Add(new ListItem("Nhân viên giao", "5"));
            shipType.Add(new ListItem("GHTK", "6"));
            shipType.Add(new ListItem("Viettel", "7"));
            shipType.Add(new ListItem("Grab", "8"));
            shipType.Add(new ListItem("AhaMove", "9"));

            ddlShippingType.Items.Clear();
            ddlShippingType.Items.AddRange(shipType.ToArray());
            ddlShippingType.DataBind();
            ddlShippingType.SelectedIndex = 3;

            // Init drop down list Bank
            var trans = TransportCompanyController.getDropDownListTrans();
            trans[0].Text = "Chọn chành xe";

            ddlTransportCompanyID.Items.Clear();
            
            ddlTransportCompanyID.Items.AddRange(trans.ToArray());
            ddlTransportCompanyID.DataBind();
            ddlTransportCompanyID.SelectedIndex = 0;

            ddlTransportCompanySubID.Items.Clear();
            ddlTransportCompanySubID.Items.Add(new ListItem("Chọn nơi nhận", "0"));
            ddlTransportCompanySubID.DataBind();
            ddlTransportCompanySubID.SelectedIndex = 0;
        }

        [WebMethod]
        public static string checkPrepayTransport(int ID, int SubID)
        {
            var a = TransportCompanyController.GetReceivePlaceByID(ID, SubID);
            if (a != null)
            {
                if (a.Prepay == true)
                {
                    return "yes";
                }
                else
                {
                    return "no";
                }
            }
            return "null";
        }

        [WebMethod]
        public static string getOrderReturn(int customerID)
        {
            return RefundGoodController.getOrderReturnJSON(customerID);
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
                    int AgentID = Convert.ToInt32(acc.AgentID);
                    int OrderType = hdfOrderType.Value.ToInt();
                    string AdditionFee = "0";
                    string DisCount = "0";
                    int CustomerID = 0;

                    string CustomerPhone = Regex.Replace(txtPhone.Text.Trim(), @"[^\d]", "");
                    string CustomerName = txtFullname.Text.Trim().ToLower().ToTitleCase();
                    string Nick = txtNick.Text.Trim();
                    string CustomerAddress = txtAddress.Text.Trim().ToTitleCase();
                    string Zalo = "";
                    string Facebook = txtFacebook.Text.Trim();
                    int PaymentStatus = ddlPaymentStatus.SelectedValue.ToInt();
                    int ExcuteStatus = ddlExcuteStatus.SelectedValue.ToInt();
                    int PaymentType = ddlPaymentType.SelectedValue.ToInt();
                    int ShippingType = ddlShippingType.SelectedValue.ToInt();
                    int ProvinceID = hdfProvinceID.Value.ToInt(0);
                    int DistrictID = hdfDistrictID.Value.ToInt(0);
                    int WardID = hdfWardID.Value.ToInt(0);
                    int TransportCompanyID = ddlTransportCompanyID.SelectedValue.ToInt(0);
                    int TransportCompanySubID = hdfTransportCompanySubID.Value.ToInt(0);

                    var checkCustomer = CustomerController.GetByPhone(CustomerPhone);
                    if (checkCustomer != null)
                    {
                        CustomerID = checkCustomer.ID;
                        string kq = CustomerController.Update(CustomerID, CustomerName, checkCustomer.CustomerPhone, CustomerAddress, "", checkCustomer.CustomerLevelID.Value, checkCustomer.Status.Value, checkCustomer.CreatedBy, currentDate, username, false, Zalo, Facebook, checkCustomer.Note, Nick, checkCustomer.Avatar, checkCustomer.ShippingType.Value, checkCustomer.PaymentType.Value, checkCustomer.TransportCompanyID.Value, checkCustomer.TransportCompanySubID.Value, checkCustomer.CustomerPhone2, ProvinceID, DistrictID, WardID);
                    }
                    else
                    {
                        string kq = CustomerController.Insert(CustomerName, CustomerPhone, CustomerAddress, "", 0, 0, currentDate, username, false, Zalo, Facebook, "", Nick, "", ShippingType, PaymentType, TransportCompanyID, TransportCompanySubID, "", ProvinceID, DistrictID, WardID);
                        if (kq.ToInt(0) > 0)
                        {
                            CustomerID = kq.ToInt();
                        }
                    }

                    string totalPrice = hdfTotalPrice.Value.ToString();
                    string totalPriceNotDiscount = hdfTotalPriceNotDiscount.Value;
                    double DiscountPerProduct = Convert.ToDouble(pDiscount.Value);
                    double TotalDiscount = Convert.ToDouble(pDiscount.Value) * Convert.ToDouble(hdfTotalQuantity.Value);
                    string FeeShipping = pFeeShip.Value.ToString();

                    bool IsHidden = false;
                    int WayIn = 1;

                    string datedone = "";

                    if(ExcuteStatus == 2)
                    {
                        datedone = DateTime.Now.ToString();
                    }

                    var couponID = hdfCouponID.Value.ToInt(0);
                    var couponValue = hdfCouponValue.Value.ToDecimal(0);

                    var orderNew = new tbl_Order()
                    {
                        AgentID = AgentID,
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
                        IsHidden = IsHidden,
                        WayIn = WayIn,
                        CreatedDate = currentDate,
                        CreatedBy = username,
                        DiscountPerProduct = Convert.ToDouble(pDiscount.Value),
                        TotalDiscount = TotalDiscount,
                        FeeShipping = FeeShipping,
                        PaymentType = PaymentType,
                        ShippingType = ShippingType,
                        GuestPaid = 0,
                        GuestChange = 0,
                        TransportCompanyID = TransportCompanyID,
                        TransportCompanySubID = TransportCompanySubID,
                        OtherFeeName = String.Empty,
                        OtherFeeValue = 0,
                        PostalDeliveryType = 1,
                        CouponID = couponID,
                        CouponValue = couponValue
                    };

                    if (!String.IsNullOrEmpty(datedone))
                        orderNew.DateDone = Convert.ToDateTime(datedone);

                    var ret = OrderController.Insert(orderNew);

                    // Insert Other Fee
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
                    // Insert Transfer Bank
                    var bankID = ddlBank.SelectedValue.ToInt(0);
                    if (bankID != 0)
                    {
                        BankTransferController.Create(ret, bankID, acc);
                    }

                    // Inactive code coupon
                    if (orderNew.CouponID.HasValue && orderNew.CouponID.Value > 0)
                    {
                        CouponController.updateStatusCouponCustomer(CustomerID, orderNew.CouponID.Value, false);
                    }

                    int OrderID = ret.ID;

                    if (OrderID > 0)
                    {
                        var orderDetails = new List<tbl_OrderDetail>();
                        var stockManager = new List<tbl_StockManager>();
                        string list = hdfListProduct.Value;
                        var items = list.Split(';').Where(x => !String.IsNullOrEmpty(x)).ToList();
                        if (items.Count > 0)
                            items.Reverse();

                        foreach (var item in items)
                        {
                            string[] itemValue = item.Split(',');

                            int ProductID = itemValue[0].ToInt();
                            int ProductVariableID = itemValue[11].ToInt();
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

                            orderDetails.Add(new tbl_OrderDetail()
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
                                ProductType = ProductType,
                                CreatedDate = currentDate,
                                CreatedBy = username,
                                IsCount = true
                            });

                            stockManager.Add(
                                new tbl_StockManager
                                {
                                    AgentID = AgentID,
                                    ProductID = ProductID,
                                    ProductVariableID = ProductVariableID,
                                    Quantity = Quantity,
                                    QuantityCurrent = 0,
                                    Type = 2,
                                    NoteID = "Xuất kho khi tạo đơn",
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

                        OrderDetailController.Insert(orderDetails);
                        OrderController.updateQuantityCOGS(OrderID);
                        StockManagerController.Insert(stockManager);

                        string refund = hdSession.Value;
                        if (refund != "1")
                        {
                            string[] RefundID = refund.Split('|');
                            var update = RefundGoodController.UpdateStatus(RefundID[0].ToInt(), username, 2, OrderID);
                            var updateor = OrderController.UpdateRefund(OrderID, RefundID[0].ToInt(), username);
                        }

                        PJUtils.ShowMessageBoxSwAlertCallFunction("Tạo đơn hàng thành công", "s", true, "redirectTo(" + OrderID + ")", Page);
                    }
                }
            }
        }

        [WebMethod]
        public static string getTransferLast(int customerID)
        {
            return BankTransferController.getTransferLastJSON(customerID);
        }

        [WebMethod]
        public static string getDeliveryLast(int customerID)
        {
            return DeliveryController.getDeliveryLast(customerID);
        }

        [WebMethod]
        public static string getTransportSub(int transComID)
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
        public static string getOrderLast(int customerID)
        {
            return OrderController.getLastJSON(customerID);
        }

        /// <summary>
        /// Lấy thông tin của khách hàng về số lượng order đang xử lý
        /// và số lượng đơn hàng đổi trả chưa trừ tiền
        /// </summary>
        /// <param name="customerID">Mã khách hàng</param>
        /// <param name="status">1: Đơn hàng đang xử lý | Đơn hàng chưa trừ tiền</param>
        /// <returns></returns>
        [WebMethod]
        public static string checkOrderOld(int customerID, int status = 1)
        {
            var customer = CustomerController.GetByID(customerID);
            var order = OrderController.GetByCustomerID(customerID, status);
            int orderReturn = RefundGoodController.GetByCustomerID(customerID, status);
            var serializer = new JavaScriptSerializer();

            if (customer != null)
            {
                return serializer.Serialize(new
                {
                    phone = customer.CustomerPhone,
                    numberOrder = order.Count,
                    numberOrderReturn = orderReturn
                });
            }
            else
            {
                return "null";
            }
        }

        [WebMethod]
        public static string getCoupon(int customerID, string code, int productNumber, decimal price)
        {
            return CouponController.getCoupon(customerID, code, productNumber, price);
        }
    }
}