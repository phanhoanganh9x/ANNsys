using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public partial class them_don_tra_hang : System.Web.UI.Page
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
            }
        }
        [WebMethod]
        public static string checkphone(string phonefullname)
        {
            RefundCust rf = new RefundCust();
            int AgentID = 0;
            string username = HttpContext.Current.Request.Cookies["usernameLoginSystem"].Value;
            if (!string.IsNullOrEmpty(username))
            {
                var a = AccountController.GetByUsername(username);
                if (a != null)
                {
                    AgentID = Convert.ToInt32(a.AgentID);
                }
            }
            var customer = CustomerController.GetByPhone(phonefullname);
            if (customer != null)
            {

                rf.CustName = customer.CustomerName;
                rf.CustPhone = customer.CustomerPhone;
                rf.CustZalo = customer.Zalo;
                rf.CustFB = customer.Facebook;
                rf.CustAddress = customer.CustomerAddress;


                int custID = customer.ID;
                double FeeRefund = 0;
                double NumOfDateToChangeProduct = 0;
                double NumOfProductCanChange = 0;
                var config = ConfigController.GetByTop1();
                if (config != null)
                {
                    FeeRefund = Convert.ToDouble(config.FeeChangeProduct);
                    NumOfDateToChangeProduct = Convert.ToDouble(config.NumOfDateToChangeProduct);
                    NumOfProductCanChange = Convert.ToDouble(config.NumOfProductCanChange);
                }
                var d = DiscountCustomerController.getbyCustID(custID);
                if (d.Count > 0)
                {
                    FeeRefund = d[0].FeeRefund;
                    NumOfDateToChangeProduct = d[0].NumOfDateToChangeProduct;
                    NumOfProductCanChange = d[0].NumOfProductCanChange;
                }

                DateTime toDate = DateTime.Now.Date;
                var fromDate = toDate.AddDays(-NumOfDateToChangeProduct);

                double totalProductRefund = 0;
                var refundList = RefundGoodController.GetByAgentIDCustomerIDFromDateToDate(AgentID, custID, fromDate, toDate.AddDays(1));
                if (refundList.Count > 0)
                {
                    foreach (var item in refundList)
                    {
                        var rfD = RefundGoodDetailController.GetByRefundGoodsID(item.ID);
                        if (rfD.Count > 0)
                        {
                            foreach (var fd in rfD)
                            {
                                totalProductRefund += Convert.ToDouble(fd.Quantity);
                            }
                        }
                    }
                }
                double leftCanchange = NumOfProductCanChange - totalProductRefund;
                if (leftCanchange > 0)
                {
                    rf.CustleftCanchange = leftCanchange.ToString();

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    return serializer.Serialize(rf);
                }
                else
                {
                    rf.CustleftCanchange = "full";
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    return serializer.Serialize(rf);
                }
            }
            else
            {
                rf.CustleftCanchange = "nocustomer";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(rf);

            }
        }

        public class RefundCust
        {
            public string CustName { get; set; }
            public string CustPhone { get; set; }
            public string CustAddress { get; set; }
            public string CustZalo { get; set; }
            public string CustFB { get; set; }
            public string CustleftCanchange { get; set; }
        }

        [WebMethod]
        public static string getProduct(string phone, string sku, int rowIndex)
        {
            int agentID = 0;
            string username = HttpContext.Current.Request.Cookies["usernameLoginSystem"].Value;

            if (!string.IsNullOrEmpty(username))
            {
                var a = AccountController.GetByUsername(username);
                if (a != null)
                {
                    agentID = Convert.ToInt32(a.AgentID);
                }
            }

            try
            {
                string sqlStoreProcedure = String.Empty;

                sqlStoreProcedure += Environment.NewLine + " GetReceiveProduct ";
                sqlStoreProcedure += Environment.NewLine + "      @UserName ";
                sqlStoreProcedure += Environment.NewLine + " ,    @AgentID ";
                sqlStoreProcedure += Environment.NewLine + " ,    @CustomerPhone ";
                sqlStoreProcedure += Environment.NewLine + " ,    @SKU ";
                sqlStoreProcedure += Environment.NewLine + " ,    @Index ";

                var parameter = new SqlParameter[]
                    {
                        new SqlParameter { ParameterName = "@UserName", Value = username, Direction = System.Data.ParameterDirection.Input}
                        , new SqlParameter { ParameterName = "@AgentID", Value = agentID, Direction = System.Data.ParameterDirection.Input}
                        , new SqlParameter { ParameterName = "@CustomerPhone", Value = phone, Direction = System.Data.ParameterDirection.Input}
                        , new SqlParameter { ParameterName = "@SKU", Value = sku, Direction = System.Data.ParameterDirection.Input}
                        , new SqlParameter { ParameterName = "@Index", Value = rowIndex, Direction = System.Data.ParameterDirection.Input}
                };

                using (var connect = new inventorymanagementEntities())
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var procedure = connect.Database.SqlQuery<ProductRefundModel>(sqlStoreProcedure, parameter).SingleOrDefault();

                    return serializer.Serialize(procedure);
                }

            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public class ProductRefundModel
        {
            public int OrderID { get; set; }
            public int OrderDetailID { get; set; }
            public int RowIndex { get; set;  }
            public string ProductName { get; set; }
            public int ProductType { get; set; }
            public string SKU { get; set; }
            public double Price { get; set; }
            public double ReducedPrice { get; set; }
            public double DiscountPerProduct { get; set; }
            public double QuantityOrder { get; set; }
            public double QuantityLeft { get; set; }
            public double FeeRefund { get; set; }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            int AgentID = 0;
            string username = Request.Cookies["usernameLoginSystem"].Value;
            if (!string.IsNullOrEmpty(username))
            {
                var a = AccountController.GetByUsername(username);
                if (a != null)
                {
                    AgentID = Convert.ToInt32(a.AgentID);
                    string phone = hdfPhone.Value;
                    if (!string.IsNullOrEmpty(phone))
                    {
                        var cust = CustomerController.GetByPhone(phone);
                        if (cust != null)
                        {
                            #region Tạo đơn hàng đổi tra
                            int custID = cust.ID;
                            string totalprice = hdfTotalPrice.Value;
                            string totalquantity = hdfTotalQuantity.Value;
                            string totalrefund = hdfTotalRefund.Value;
                            var agent = AgentController.GetByID(AgentID);
                            string agentName = "";
                            if (agent != null)
                            {
                                agentName = agent.AgentName;
                            }
                            //insert ddlstatus, refundnote
                            int status = ddlRefundStatus.SelectedValue.ToInt();
                            string RefundsNote = txtRefundsNote.Text;
                            int rID = RefundGoodController.Insert(AgentID, totalprice, status, custID, Convert.ToInt32(totalquantity),
                                totalrefund, agentName, cust.CustomerName, cust.CustomerPhone, currentDate, username, RefundsNote);
                            #endregion

                            if (rID > 0)
                            {
                                string listString = hdfListProduct.Value;
                                string[] items = listString.Split('|');
                                if (items.Length - 1 > 0)
                                {
                                    for (int i = 0; i < items.Length - 1; i++)
                                    {
                                        #region Tạo chi tiết đơn hàng đổi tra
                                        string[] element = items[i].Split(';');
                                        var sku = element[0];
                                        var orderID = element[1].ToInt(0);
                                        var orderDetailID = element[2];
                                        var ProductName = element[3];
                                        var GiavonPerProduct = Convert.ToDouble(element[5]);
                                        var SoldPricePerProduct = Convert.ToDouble(element[6]);
                                        var DiscountPricePerProduct = Convert.ToDouble(element[7]);
                                        var quantity = Convert.ToDouble(element[10]);
                                        var quantityMax = Convert.ToDouble(element[8]);
                                        var ProductType = element[4].ToInt(1);
                                        var RefundType = element[9].ToInt(1);
                                        var RefundFeePerProduct = Convert.ToDouble(element[11]);
                                        var TotalPriceRow = element[12];
                                        var PriceNotFeeRefund = SoldPricePerProduct * quantity;
                                        var rdTotalRefundFee = RefundFeePerProduct * quantity;

                                        int rdID = RefundGoodDetailController.Insert(rID, AgentID, orderID, ProductName, custID, sku, quantity,
                                            quantityMax, PriceNotFeeRefund.ToString(), ProductType, true, RefundType, RefundFeePerProduct.ToString(),
                                            rdTotalRefundFee.ToString(), GiavonPerProduct.ToString(), DiscountPricePerProduct.ToString(), SoldPricePerProduct.ToString(),
                                            TotalPriceRow, currentDate, username);
                                        #endregion

                                        #region Cập nhật stock
                                        if (rdID > 0)
                                        {
                                            if (RefundType < 3)
                                            {
                                                int typeRe = 0;
                                                string note = "";
                                                if (RefundType == 1)
                                                {
                                                    note = "Đổi size";
                                                    typeRe = 8;
                                                }
                                                else if (RefundType == 2)
                                                {
                                                    note = "Đổi sản phẩm";
                                                    typeRe = 9;
                                                }
                                                if (ProductType == 1)
                                                {
                                                    var product = ProductController.GetBySKU(sku);
                                                    if (product != null)
                                                    {
                                                        int productID = product.ID;
                                                        string ProductImageOrigin = "";
                                                        var ProductImage = ProductImageController.GetFirstByProductID(product.ID);
                                                        if (ProductImage != null)
                                                            ProductImageOrigin = ProductImage.ProductImage;
                                                        StockManagerController.Insert(
                                                            new tbl_StockManager {
                                                                AgentID = AgentID,
                                                                ProductID = productID,
                                                                ProductVariableID = 0,
                                                                Quantity = quantity,
                                                                QuantityCurrent = 0,
                                                                Type = 1,
                                                                NoteID = note,
                                                                OrderID = orderID,
                                                                Status = typeRe,
                                                                SKU = sku,
                                                                CreatedDate = currentDate,
                                                                CreatedBy = username,
                                                                ModifiedDate = currentDate,
                                                                ModifiedBy = username,
                                                                MoveProID = 0,
                                                                ParentID = productID,
                                                            });
                                                    }
                                                }
                                                else
                                                {
                                                    string ProductVariableName = "";
                                                    string ProductVariableValue = "";
                                                    string ProductVariable = "";
                                                    int parentID = 0;
                                                    string parentSKU = "";
                                                    string ProductImageOrigin = "";
                                                    int ID = 0;

                                                    var productvariable = ProductVariableController.GetBySKU(sku);
                                                    if (productvariable != null)
                                                    {
                                                        ID = productvariable.ID;
                                                        ProductImageOrigin = productvariable.Image;
                                                        parentSKU = productvariable.ParentSKU;
                                                        var variables = ProductVariableValueController.GetByProductVariableID(productvariable.ID);
                                                        if (variables.Count > 0)
                                                        {
                                                            foreach (var v in variables)
                                                            {
                                                                ProductVariable += v.VariableName.Trim() + ":" + v.VariableValue.Trim() + "|";
                                                                ProductVariableName += v.VariableName + "|";
                                                                ProductVariableValue += v.VariableValue + "|";
                                                            }
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(parentSKU))
                                                    {
                                                        var product = ProductController.GetBySKU(parentSKU);
                                                        if (product != null)
                                                            parentID = product.ID;
                                                    }


                                                    StockManagerController.Insert(
                                                        new tbl_StockManager {
                                                            AgentID = AgentID,
                                                            ProductID = 0,
                                                            ProductVariableID = ID,
                                                            Quantity = quantity,
                                                            QuantityCurrent = 0,
                                                            Type = 1,
                                                            NoteID = note,
                                                            OrderID = orderID,
                                                            Status = typeRe,
                                                            SKU = sku,
                                                            CreatedDate = currentDate,
                                                            CreatedBy = username,
                                                            ModifiedDate = currentDate,
                                                            ModifiedBy = username,
                                                            MoveProID = 0,
                                                            ParentID = parentID,
                                                        });
                                                }
                                            }
                                        }
                                        #endregion
                                    }

                                    RefundGoodController.updateQuantityCOGS(rID);

                                    PJUtils.ShowMessageBoxSwAlertCallFunction("Tạo đơn hàng đổi trả thành công", "s", true, "redirectTo(" + rID + ")", Page);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}