using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using Newtonsoft.Json;
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
    public partial class tao_don_hang_doi_tra : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
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

                        LoadData();
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
            }
        }

        private void LoadData()
        {
            if (HttpContext.Current.Items["xem-don-hang-doi-tra"] != null)
            {
                this.hdfListProduct.Value = HttpContext.Current.Items["xem-don-hang-doi-tra"].ToString();

                this.Title = String.Format("Làm lại đơn hàng trả");
            }

            var customerID = HttpContext.Current.Request["customerID"];
            if (!String.IsNullOrEmpty(customerID))
            {
                var customer = CustomerController.GetByID(Convert.ToInt32(customerID));
                if (customer != null)
                {
                    var serializer = new JavaScriptSerializer();
                    var script = "$(document).ready(() => { selectCustomerDetail(" + serializer.Serialize(customer) + "); });";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", script, true);
                    // Fix bug khi truyền dữ liệu customer từ màn hình khách san
                    hdfCustomerID.Value = customer.ID.ToString();
                }
            }

            var username = HttpContext.Current.Request["username"];
            if (!String.IsNullOrEmpty(username))
            {
                var user = AccountController.GetByUsername(username);
                if(user != null)
                {
                    hdfUsernameCurrent.Value = username;
                }
            }
        }

        [WebMethod]
        public static string checkphone(string phonefullname)
        {
            var customer = CustomerController.GetByPhone(phonefullname);

            if (customer != null)
            {
                return "OK";
            }
            else
            {
                return "nocustomer";
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
        public static string getProduct(int customerID, string sku)
        {
            try
            {
                var feeChange = ConfigController.GetByTop1().FeeChangeProduct.Value;

                using (var con = new inventorymanagementEntities())
                {

                    var productTarget = con.tbl_Product
                        .Where(x => sku.Trim().ToUpper().Contains(x.ProductSKU))
                        .GroupJoin(
                            con.tbl_ProductVariable.Where(x => x.SKU.Contains(sku.Trim().ToUpper())),
                            product => new
                            {
                                ProductStyle = product.ProductStyle.Value,
                                ProductID = product.ID,
                            },
                            productVariable => new
                            {
                                ProductStyle = 2,
                                ProductID = productVariable.ProductID.Value,
                            },
                            (product, productVariable) => new {
                                product,
                                productVariable
                            })
                        .SelectMany(x =>
                            x.productVariable.DefaultIfEmpty(),
                            (parent, child) => new RefundDetailModel
                            {
                                ProductID = parent.product.ID,
                                ProductVariableID = parent.product.ProductStyle == 2 ? child.ID : 0,
                                ProductStyle = parent.product.ProductStyle.Value,
                                ProductImage = parent.product.ProductStyle == 2 ? child.Image : parent.product.ProductImage,
                                ProductTitle = parent.product.ProductTitle,
                                ParentSKU = parent.product.ProductSKU,
                                ChildSKU = parent.product.ProductStyle == 2 ? child.SKU : String.Empty,
                                Price = parent.product.ProductStyle == 2 ? child.Regular_Price.Value : parent.product.Regular_Price.Value,
                                ReducedPrice = parent.product.ProductStyle == 2 ? child.Regular_Price.Value : parent.product.Regular_Price.Value,
                                QuantityRefund = 1,
                                ChangeType = 2,
                                FeeRefund = feeChange,
                                TotalFeeRefund = parent.product.ProductStyle == 2 ? child.Regular_Price.Value : parent.product.Regular_Price.Value
                            }
                        )
                        .Where(x => x.ParentSKU == sku.Trim().ToUpper() || x.ChildSKU == sku.Trim().ToUpper())
                        .OrderBy(x => x.ProductID)
                        .ThenBy(x => x.ProductVariableID)
                        .ToList();


                    var variableValue = con.tbl_ProductVariableValue
                        .Where(x => x.ProductvariableSKU.Contains(sku.Trim().ToUpper()))
                        .OrderBy(x => x.ProductVariableID)
                        .ThenBy(x => x.ID)
                        .ToList();

                    var order = con.tbl_OrderDetail
                        .Join(
                            con.tbl_Order
                                .Where(x => x.CustomerID == customerID)
                                .Where(x => x.ExcuteStatus == (int)ExcuteStatus.Done),
                            od => od.OrderID,
                            o => o.ID,
                            (od, o) => new
                            {
                                sku = od.SKU,
                                orderId = od.OrderID.Value,
                                saleDate = od.CreatedDate.Value,
                                price = od.Price.HasValue ? od.Price.Value : 0,
                                discount = od.DiscountPrice.HasValue && od.DiscountPrice.Value > 0
                                    ? od.DiscountPrice.Value
                                    : (o.DiscountPerProduct.HasValue ? o.DiscountPerProduct.Value : 0)
                            }
                        )
                        .Where(x => x.sku.Contains(sku.Trim().ToUpper()))
                        .Select(x => new
                        {
                            sku = x.sku,
                            orderID = x.orderId,
                            saleDate = x.saleDate,
                            price = x.price - x.discount
                        })
                        .Distinct()
                        .OrderByDescending(o => o.sku)
                        .ThenByDescending(o => o.saleDate)
                        .ToList();

                    productTarget = productTarget.Select(x => {
                        string properties = String.Empty;

                        variableValue
                            .Where(y => y.ProductVariableID == x.ProductVariableID)
                            .Select(y => {
                                properties += String.Format("{0}: {1}|", y.VariableName, y.VariableValue);
                                return y;
                            })
                            .ToList();

                        var orderFilter = order
                            .Where(y => (x.ProductStyle == 1 && y.sku == x.ParentSKU) || (x.ProductStyle == 2 && y.sku == x.ChildSKU))
                            .FirstOrDefault();

                        return new RefundDetailModel()
                        {

                            ProductID = x.ProductID,
                            ProductVariableID = x.ProductVariableID,
                            ProductStyle = x.ProductStyle,
                            ProductImage = x.ProductImage,
                            ProductTitle = x.ProductTitle,
                            ParentSKU = x.ParentSKU,
                            ChildSKU = x.ChildSKU,
                            VariableValue = properties,
                            Price = x.Price,
                            ReducedPrice = orderFilter != null ? Convert.ToDouble(orderFilter.price) : x.ReducedPrice,
                            QuantityRefund = x.QuantityRefund,
                            ChangeType = x.ChangeType,
                            FeeRefund = x.FeeRefund,
                            TotalFeeRefund = x.TotalFeeRefund,
                            SaleDate = orderFilter != null ? orderFilter.saleDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd"),
                            OrderID = orderFilter != null ? orderFilter.orderID : 0
                        };
                    })
                    .ToList();


                    return new JavaScriptSerializer().Serialize(productTarget);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Làm lại đơn hàng đổi trả
            if (!String.IsNullOrEmpty(hdRefundGoodsID.Value))
            {
                RefundGoodDetailController.DeleteByRefundGoodsID(hdRefundGoodsID.Value.ToInt());
                RefundGoodController.DeleteByID(hdRefundGoodsID.Value.ToInt());
                OrderController.DeleteOrderRefund(hdRefundGoodsID.Value.ToInt());
            }
            DateTime currentDate = DateTime.Now;
            int agentID = 0;
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;

            if (!string.IsNullOrEmpty(username))
            {
                var a = AccountController.GetByUsername(username);

                if (a != null)
                {
                    #region Lấy thông tin tao đơn đổi tra
                    // Change user
                    string UserHelp = "";
                    string redirectToUsername = "";
                    if (username != hdfUsernameCurrent.Value)
                    {
                        UserHelp = username;
                        username = hdfUsernameCurrent.Value;
                        redirectToUsername = hdfUsernameCurrent.Value;
                    }

                    agentID = Convert.ToInt32(a.AgentID);

                    string CustomerPhone = txtPhone.Text;
                    string CustomerName = txtFullname.Text;
                    string Nick = txtNick.Text.Trim();
                    string CustomerAddress = txtAddress.Text.Trim();
                    string Zalo = "";
                    string Facebook = txtFacebook.Text.Trim();
                    string RefundNote = txtRefundsNote.Text;

                    int ProvinceID = hdfProvinceID.Value.ToInt(0);
                    int DistrictID = hdfDistrictID.Value.ToInt(0);
                    int WardID = hdfWardID.Value.ToInt(0);
                    #endregion

                    if (!string.IsNullOrEmpty(CustomerPhone))
                    {
                        var checkCustomer = CustomerController.GetByPhone(CustomerPhone);

                        if (checkCustomer != null)
                        {
                            #region Cập nhật thông tin khách hàng
                            int custID = checkCustomer.ID;

                            string kq = CustomerController.Update(custID, CustomerName, checkCustomer.CustomerPhone, CustomerAddress, "", checkCustomer.CustomerLevelID.Value, checkCustomer.Status.Value, checkCustomer.CreatedBy, currentDate, username, false, Zalo, Facebook, checkCustomer.Note, Nick, checkCustomer.Avatar, checkCustomer.ShippingType.Value, checkCustomer.PaymentType.Value, checkCustomer.TransportCompanyID.Value, checkCustomer.TransportCompanySubID.Value, checkCustomer.CustomerPhone2, ProvinceID, DistrictID, WardID);
                            #endregion

                            #region Tạo đơn hàng đổi trả
                            double totalPrice = Convert.ToDouble(hdfTotalPrice.Value);
                            double totalRefund = Convert.ToDouble(hdfTotalRefund.Value);
                            int OrderSaleID = hdfOrderSaleID.Value.ToInt(0);

                            var agent = AgentController.GetByID(agentID);
                            string agentName = String.Empty;

                            if (agent != null)
                            {
                                agentName = agent.AgentName;
                            }

                            //insert ddlstatus, refundnote
                            int status = ddlRefundStatus.SelectedValue.ToInt();

                            if (OrderSaleID != 0)
                            {
                                status = 2;
                            }

                            int rID = RefundGoodController.Insert(
                                new tbl_RefundGoods()
                                {
                                    AgentID = agentID,
                                    TotalPrice = totalPrice.ToString(),
                                    Status = status,
                                    CustomerID = custID,
                                    TotalRefundFee = totalRefund.ToString(),
                                    CreatedDate = currentDate,
                                    CreatedBy = username,
                                    CustomerName = checkCustomer.CustomerName,
                                    CustomerPhone = checkCustomer.CustomerPhone,
                                    AgentName = agentName,
                                    RefundNote = RefundNote,
                                    OrderSaleID = OrderSaleID,
                                    UserHelp = UserHelp
                                });
                            #endregion

                            if (rID > 0)
                            {
                                #region Cập nhật đơn đổi hàng vào order
                                if (OrderSaleID != 0)
                                {
                                    OrderController.UpdateRefund(OrderSaleID, rID, username);
                                }
                                #endregion

                                #region Thực hiện tạo chi tiết đơn đổi hàng
                                RefundGoodModel refundModel = JsonConvert.DeserializeObject<RefundGoodModel>(hdfListProduct.Value);
                                var refundDetails = new List<tbl_RefundGoodsDetails>();
                                var stocks = new List<tbl_StockManager>();
                                int t = 0;
                                int time = 0;

                                foreach (RefundDetailModel item in refundModel.RefundDetails)
                                {
                                    #region Tạo từng dòng chi tiết đổi hàng
                                    t++;
                                    time += 20;
                                    refundDetails.Add( new tbl_RefundGoodsDetails()
                                    {
                                        RefundGoodsID = rID,
                                        AgentID = agentID,
                                        OrderID = 0,
                                        ProductName = item.ProductTitle,
                                        CustomerID = custID,
                                        SKU = item.ProductStyle == 1 ? item.ParentSKU : item.ChildSKU,
                                        Quantity = item.QuantityRefund,
                                        QuantityMax = item.QuantityRefund,
                                        PriceNotFeeRefund = (item.QuantityRefund * item.ReducedPrice).ToString(),
                                        ProductType = item.ProductStyle,
                                        IsCount = true,
                                        RefundType = item.ChangeType,
                                        RefundFeePerProduct = item.ChangeType == 2 ? item.FeeRefund.ToString() : "0",
                                        TotalRefundFee = item.ChangeType == 2 ? (item.FeeRefund * item.QuantityRefund).ToString() : "0",
                                        GiavonPerProduct = item.Price.ToString(),
                                        DiscountPricePerProduct = (item.Price - item.ReducedPrice).ToString(),
                                        SoldPricePerProduct = item.ReducedPrice.ToString(),
                                        TotalPriceRow = item.TotalFeeRefund.ToString(),
                                        CreatedDate = currentDate.AddMilliseconds(time),
                                        CreatedBy = username
                                    });
                                    #endregion

                                    #region Cập nhật thông tin đổi hàng vào Stock
                                    if (item.ChangeType < 3)
                                    {
                                        int typeRe = 0;
                                        string note = "";

                                        if (item.ChangeType == 1)
                                        {
                                            note = "Đổi size đơn " + rID;
                                            typeRe = 8;
                                        }
                                        else if (item.ChangeType == 2)
                                        {
                                            note = "Đổi sản phẩm khác đơn " + rID;
                                            typeRe = 9;
                                        }

                                        if (item.ChangeType == 1 || item.ChangeType == 2)
                                        {
                                            stocks.Add(new tbl_StockManager()
                                            {
                                                AgentID = agentID,
                                                ProductID = item.ProductStyle == 1 ? item.ProductID : 0,
                                                ProductVariableID = item.ProductVariableID,
                                                Quantity = item.QuantityRefund,
                                                QuantityCurrent = 0,
                                                Type = 1,
                                                NoteID = note,
                                                OrderID = 0,
                                                Status = typeRe,
                                                SKU = item.ProductStyle == 1 ? item.ParentSKU : item.ChildSKU,
                                                CreatedDate = currentDate.AddMilliseconds(time),
                                                CreatedBy = username,
                                                ModifiedDate = currentDate.AddMilliseconds(time),
                                                ModifiedBy = username,
                                                MoveProID = 0,
                                                ParentID = item.ProductID,
                                            });
                                        }
                                    }
                                    #endregion
                                }

                                refundDetails = RefundGoodDetailController.Insert(refundDetails);
                                RefundGoodController.updateQuantityCOGS(rID);
                                StockManagerController.Insert(stocks);
                                #endregion

                                PJUtils.ShowMessageBoxSwAlertCallFunction("Tạo đơn hàng đổi trả thành công", "s", true, "redirectTo(" + rID + ",'" + redirectToUsername + "')", Page);
                            }
                        }
                    }
                }
            }
        }
    }
}