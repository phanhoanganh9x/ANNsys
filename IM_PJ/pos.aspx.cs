using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using Newtonsoft.Json;
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

namespace IM_PJ
{
    public partial class pos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 600;

            if (!IsPostBack)
            {

                if (Request.Cookies["userLoginSystem"] != null)
                {
                    hdStatusPage.Value = "Create";
                    string username = Request.Cookies["userLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);
                    if (acc != null)
                    {
                        hdfUsernameCurrent.Value = acc.Username;
                        if (acc.RoleID == 0)
                        {

                        }
                        else if(acc.RoleID == 2)
                        {
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
                                if(item.Username != acc.Username)
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
            var customer = CustomerController.Find(textsearch, createdby);
            if (customer.Count > 0)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(new { listCustomer = customer, employee = 0 });
            }
            else
            {
                var customer_other = CustomerController.Find(textsearch);
                if(customer_other.Count > 0)
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
        public static string getReturnOrder(string order, string remove)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if(remove.ToInt() == 0)
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
            var d = DiscountCustomerController.getbyCustID(ID);
            if (d.Count > 0)
            {
                var ci = new CustomerGroup();

                ci.Discount = d[0].DiscountAmount.ToString();
                ci.FeeRefund = d[0].FeeRefund.ToString();

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
            public string Discount { get; set; }
            public string FeeRefund { get; set; }
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
            DateTime currentDate = DateTime.Now;
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0 || acc.RoleID == 2)
                {
                    // Change user
                    string OrderNote = "";
                    if (username != hdfUsernameCurrent.Value)
                    {
                        OrderNote = "Được tính tiền giúp bởi " + username;
                        username = hdfUsernameCurrent.Value;
                    }

                    int AgentID = Convert.ToInt32(acc.AgentID);
                    int OrderType = hdfOrderType.Value.ToInt();
                    string AdditionFee = "0";
                    string DisCount = "0";
                    int CustomerID = 0;

                    string CustomerPhone = txtPhone.Text.Trim().Replace(" ", "");
                    string CustomerName = txtFullname.Text.Trim();
                    string Nick = txtNick.Text.Trim();
                    string CustomerEmail = "";
                    string CustomerAddress = txtAddress.Text.Trim();

                    var checkCustomer = CustomerController.GetByPhone(CustomerPhone);

                    if (checkCustomer != null)
                    {
                        CustomerID = checkCustomer.ID;
                        string kq = CustomerController.Update(CustomerID, CustomerName, checkCustomer.CustomerPhone, CustomerAddress, "", Convert.ToInt32(checkCustomer.CustomerLevelID), Convert.ToInt32(checkCustomer.Status), checkCustomer.CreatedBy, currentDate, username, false, checkCustomer.Zalo, checkCustomer.Facebook, checkCustomer.Note, checkCustomer.ProvinceID.ToString(), Nick, checkCustomer.Avatar, Convert.ToInt32(checkCustomer.ShippingType), Convert.ToInt32(checkCustomer.PaymentType), Convert.ToInt32(checkCustomer.TransportCompanyID), Convert.ToInt32(checkCustomer.TransportCompanySubID), checkCustomer.CustomerPhone2);
                    }
                    else
                    {
                        string kq = CustomerController.Insert(CustomerName, CustomerPhone, CustomerAddress, CustomerEmail, 0, 0, currentDate, username, false, "", "", "", "", Nick);
                        if (kq.ToInt(0) > 0)
                        {
                            CustomerID = kq.ToInt(0);
                        }
                    }

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

                    var ret = OrderController.InsertOnSystem(AgentID, OrderType, AdditionFee, DisCount, CustomerID, CustomerName, CustomerPhone, CustomerAddress,
                        CustomerEmail, totalPrice, totalPriceNotDiscount, PaymentStatus, ExcuteStatus, IsHidden, WayIn, currentDate, username, DiscountPerProduct,
                        TotalDiscount, FeeShipping, GuestPaid, GuestChange, PaymentType, ShippingType, OrderNote, DateTime.Now, String.Empty, 0, 1);
                    int OrderID = ret.ID;

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

                    if (OrderID > 0)
                    {
                        ProductPOS POS = JsonConvert.DeserializeObject<ProductPOS>(hdfListProduct.Value);
                        List<tbl_OrderDetail> orderDetails = new List<tbl_OrderDetail>();
                        List<tbl_StockManager> stockManager = new List<tbl_StockManager>();

                        foreach (ProductGetOut item in POS.productPOS)
                        {
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
                                    MoveProID = 0,
                                    ParentID = parentID
                                }
                                );
                        }

                        OrderDetailController.Insert(orderDetails);
                        StockManagerController.Insert(stockManager);

                        string refund = hdSession.Value;
                        if (refund != "1")
                        {
                            string[] RefundID = refund.Split('|');
                            var update = RefundGoodController.UpdateStatus(RefundID[0].ToInt(), username, 2, OrderID);
                            var updateor = OrderController.UpdateRefund(OrderID, RefundID[0].ToInt(), username);
                        }

                        // Hoàn thành khởi tạo đơn hàng nên gán lại giá trị trang lúc ban đầu
                        hdStatusPage.Value = "Create";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { printInvoice(" + OrderID + ") });", true);
                    }
                }
            }
        }
    }
}