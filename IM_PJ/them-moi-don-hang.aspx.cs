﻿using IM_PJ.Controllers;
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
                if (Request.Cookies["userLoginSystem"] != null)
                {
                    string username = Request.Cookies["userLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);
                    if (acc != null)
                    {
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

                        Response.Cookies["refundt"].Value = "1";

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
                //LoadData();
            }
        }
        [WebMethod]
        public static string checkPrepayTransport(int ID, int SubID)
        {
            var a = TransportCompanyController.GetReceivePlaceByID(ID, SubID);
            if(a != null)
            {
                if(a.Prepay == true)
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
        public static string getReturnOrder(string order, string remove)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (remove.ToInt() == 0)
            {
                var or = RefundGoodController.GetOrderByID(order.ToInt());
                if (or != null)
                {
                    HttpContext.Current.Response.Cookies["refundt"].Value = or.ID + "|" + or.TotalPrice;
                    return serializer.Serialize(or);
                }
                else
                {
                    return serializer.Serialize(null);
                }
            }
            else
            {
                HttpContext.Current.Response.Cookies["refundt"].Value = "1";
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
        protected void btnOrder_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Request.Cookies["userLoginSystem"].Value;
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

                    string CustomerPhone = txtPhone.Text.Trim().Replace(" ","");
                    string CustomerName = txtFullname.Text.Trim();
                    string Nick = txtNick.Text.Trim();
                    string CustomerAddress = txtAddress.Text.Trim();
                    string Zalo = txtZalo.Text.Trim();
                    string Facebook = txtFacebook.Text.Trim();
                    int PaymentStatus = hdfPaymentStatus.Value.ToInt(1);
                    int ExcuteStatus = hdfExcuteStatus.Value.ToInt(1);
                    int PaymentType = hdfPaymentType.Value.ToInt(1);
                    int ShippingType = hdfShippingType.Value.ToInt(1);

                    var checkCustomer = CustomerController.GetByPhone(CustomerPhone);

                    if (checkCustomer != null)
                    {
                        CustomerID = checkCustomer.ID;
                        string kq = CustomerController.Update(CustomerID, CustomerName, checkCustomer.CustomerPhone, CustomerAddress, "", Convert.ToInt32(checkCustomer.CustomerLevelID), Convert.ToInt32(checkCustomer.Status), checkCustomer.CreatedBy, currentDate, username, false, Zalo, Facebook, checkCustomer.Note, checkCustomer.ProvinceID.ToString(), Nick, checkCustomer.Avatar, Convert.ToInt32(checkCustomer.ShippingType), Convert.ToInt32(checkCustomer.PaymentType), Convert.ToInt32(checkCustomer.TransportCompanyID), Convert.ToInt32(checkCustomer.TransportCompanySubID), checkCustomer.CustomerPhone2);
                    }
                    else
                    {
                        string kq = CustomerController.Insert(CustomerName, CustomerPhone, CustomerAddress, "", 0, 0, currentDate, username, false, Zalo, Facebook, "", "", Nick, "", ShippingType, PaymentType);
                        if (kq.ToInt(0) > 0)
                        {
                            CustomerID = kq.ToInt();
                        }
                    }

                    var Customer = CustomerController.GetByID(CustomerID);

                    int TransportCompanyID = 0;
                    int TransportCompanySubID = 0;
                    if (Customer.ShippingType == ShippingType)
                    {
                        TransportCompanyID = Convert.ToInt32(Customer.TransportCompanyID);
                        TransportCompanySubID = Convert.ToInt32(Customer.TransportCompanySubID);
                    }

                    string totalPrice = hdfTotalPrice.Value.ToString();
                    string totalPriceNotDiscount = hdfTotalPriceNotDiscount.Value;
                    

                    double DiscountPerProduct = Convert.ToDouble(pDiscount.Value);

                    double TotalDiscount = Convert.ToDouble(pDiscount.Value) * Convert.ToDouble(hdfTotalQuantity.Value);
                    string FeeShipping = pFeeShip.Value.ToString();

                    string OtherFeeName = txtOtherFeeName.Text;
                    double OtherFeeValue = Convert.ToDouble(pOtherFee.Value);

                    bool IsHidden = false;
                    int WayIn = 1;

                    string datedone = "";

                    if(ExcuteStatus == 2)
                    {
                        datedone = DateTime.Now.ToString();
                    }

                    var ret = OrderController.Insert(AgentID, OrderType, AdditionFee, DisCount, CustomerID, CustomerName, CustomerPhone, CustomerAddress,
                        "", totalPrice, totalPriceNotDiscount, PaymentStatus, ExcuteStatus, IsHidden, WayIn, currentDate, username, Convert.ToDouble(pDiscount.Value),
                        TotalDiscount, FeeShipping, PaymentType, ShippingType, datedone, 0, 0, TransportCompanyID, TransportCompanySubID, OtherFeeName, OtherFeeValue, 1);

                    int OrderID = ret.ID;

                    double totalQuantity = 0;
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

                                OrderDetailController.Insert(AgentID, OrderID, SKU, ProductID, ProductVariableID, ProductVariableSave, Quantity, Price, 1, 0,
                                    ProductType, currentDate, username, true);

                                StockManagerController.Insert(
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
                                        MoveProID = 0,
                                        ParentID = parentID,
                                    });
                                totalQuantity += Quantity;
                            }
                        }

                        string refund = Request.Cookies["refundt"].Value;
                        if (refund != "1")
                        {
                            string[] RefundID = refund.Split('|');
                            var update = RefundGoodController.UpdateStatus(RefundID[0].ToInt(), username, 2, OrderID);
                            var updateor = OrderController.UpdateRefund(OrderID, RefundID[0].ToInt(), username);
                        }

                        Response.Cookies["refundt"].Expires = DateTime.Now.AddDays(-1d);
                        Response.Cookies.Add(Response.Cookies["refundt"]);

                        PJUtils.ShowMessageBoxSwAlertCallFunction("Tạo đơn hàng thành công", "s", true, "redirectTo(" + OrderID + ")", Page);
                    }
                }
            }
        }
    }
}