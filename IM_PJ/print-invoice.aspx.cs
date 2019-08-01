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
    public partial class print_invoice : System.Web.UI.Page
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
                LoadData();
            }
        }
        public static int getCategory(string SKU, int ProductType)
        {
            int categoryID = 0;
            if (ProductType == 1)
            {
                var product = ProductController.GetBySKU(SKU);
                if (product != null)
                {
                    categoryID = Convert.ToInt32(product.CategoryID);
                }
            }
            else
            {
                var productvariable = ProductVariableController.GetBySKU(SKU);
                if (productvariable != null)
                {
                    var product1 = ProductController.GetByID(Convert.ToInt32(productvariable.ProductID));
                    if (product1 != null)
                    {
                        categoryID = Convert.ToInt32(product1.CategoryID);
                    }
                }
            }
            return categoryID;
        }
        public void printItemList(ref int ID, ref int mergeprint, ref double TotalQuantity, ref double TotalOrder, ref string Print)
        {
            var orderdetails = OrderDetailController.GetByIDSortBySKU(ID);
            if (orderdetails.Count > 0)
            {
                if(mergeprint == 0)
                {
                    int t = 0;
                    foreach (var item in orderdetails)
                    {
                        TotalQuantity += Convert.ToDouble(item.Quantity);

                        int ProductType = Convert.ToInt32(item.ProductType);
                        double ItemPrice = Convert.ToDouble(item.Price);
                        string SKU = item.SKU;
                        string ProductName = "";
                        int SubTotal = Convert.ToInt32(ItemPrice) * Convert.ToInt32(item.Quantity);

                        t++;
                        Print += "<tr>";
                        if (ProductType == 1)
                        {
                            var product = ProductController.GetBySKU(SKU);
                            if (product != null)
                            {
                                ProductName = product.ProductTitle;
                                Print += "<td colspan='3'>" + t + "&ensp;<strong>" + SKU + "</strong> - " + PJUtils.Truncate(ProductName, 32) + "</td> ";
                            }
                        }
                        else
                        {
                            var productvariable = ProductVariableController.GetBySKU(SKU);
                            if (productvariable != null)
                            {
                                var parent_product = ProductController.GetByID(Convert.ToInt32(productvariable.ProductID));
                                if (parent_product != null)
                                {
                                    ProductName = parent_product.ProductTitle;
                                }
                                Print += "<td colspan='3'>" + t + "&ensp;<strong>" + SKU + "</strong> - " + PJUtils.Truncate(ProductName, 40) + " - " + item.ProductVariableDescrition.Replace("|", ". ") + "</td> ";
                            }
                        }
                        Print += "</tr>";
                        Print += "<tr>";
                        Print += "<td>" + item.Quantity + "</td>";
                        Print += "<td>" + string.Format("{0:N0}", ItemPrice) + "</td>";
                        Print += "<td>" + string.Format("{0:N0}", SubTotal) + "</td>";
                        Print += "</tr>";

                        TotalOrder += SubTotal;
                    }
                }
                else
                {
                    int t = 0;
                    for (int i = 0; i < orderdetails.Count; i++)
                    {
                        if (orderdetails[i] != null)
                        {
                            t++;
                            Print += "<tr>";

                            double ItemPrice1 = Convert.ToDouble(orderdetails[i].Price);
                            int categoryID1 = getCategory(orderdetails[i].SKU, Convert.ToInt32(orderdetails[i].ProductType));

                            int quantity = Convert.ToInt32(orderdetails[i].Quantity);

                            for (int j = i + 1; j < orderdetails.Count; j++)
                            {
                                if (orderdetails[j] != null)
                                {
                                    int categoryID2 = getCategory(orderdetails[j].SKU, Convert.ToInt32(orderdetails[j].ProductType));

                                    double ItemPrice2 = Convert.ToDouble(orderdetails[j].Price);

                                    if (categoryID1 == categoryID2 && orderdetails[i].Price == orderdetails[j].Price)
                                    {
                                        quantity += Convert.ToInt32(orderdetails[j].Quantity);
                                        orderdetails[j] = null;
                                    }
                                }
                            }

                            var category = CategoryController.GetByID(categoryID1);
                            double SubTotal = ItemPrice1 * quantity;
                            Print += "<td colspan='3'>" + t + "&ensp;<strong>" + category.CategoryName + " đồng giá " + string.Format("{0:N0}", ItemPrice1) + "</strong></td>";
                            Print += "</tr>";
                            Print += "<tr>";
                            Print += "<td>" + quantity + "</td>";
                            Print += "<td>" + string.Format("{0:N0}", ItemPrice1) + "</td>";
                            Print += "<td>" + string.Format("{0:N0}", SubTotal) + "</td>";
                            Print += "</tr>";
                            TotalOrder += SubTotal;
                            TotalQuantity += quantity;
                        }
                    }
                }
            }
        }
        public void LoadData()
        {
            int ID = Request.QueryString["id"].ToInt(0);
            int mergeprint = 0;
            if(Request.QueryString["merge"] != null)
            {
                mergeprint = Request.QueryString["merge"].ToInt(0);
            }
            
            if (ID > 0)
            {
                var order = OrderController.GetByID(ID);
                
                if (order != null)
                {
                    string error = "";
                    string Print = "";

                    double TotalQuantity = 0;
                    double TotalOrder = 0;
                    

                    var orderdetails = OrderDetailController.GetByIDSortBySKU(ID);

                    var numberOfOrders = OrderController.GetByCustomerID(Convert.ToInt32(order.CustomerID));
                    var customer = CustomerController.GetByID(Convert.ToInt32(order.CustomerID));

                    if (orderdetails.Count > 0)
                    {
                        printItemList(ref ID, ref mergeprint, ref TotalQuantity, ref TotalOrder, ref Print);

                        string productPrint = "";
                        string shtml = "";

                        productPrint += "<div class=\"body\">";
                        productPrint += "<div class=\"table-1\">";
                        string mergeAlert = "";
                        if (mergeprint == 1)
                        {
                            mergeAlert += "<p class=\"merge-alert\">(Đã gộp sản phẩm)<p>";
                        }
                        productPrint += "<h1>HÓA ĐƠN #" + order.ID + mergeAlert + "</h1>";
                        
                        productPrint += "<table>";
                        productPrint += "<colgroup >";
                        productPrint += "<col class=\"col-left\"/>";
                        productPrint += "<col class=\"col-right\"/>";
                        productPrint += "</colgroup>";
                        productPrint += "<tbody>";
                        productPrint += "<tr>";
                        productPrint += "<td>Khách hàng</td>";
                        productPrint += "<td>" + order.CustomerName.ToTitleCase() + "</td>";
                        productPrint += "</tr>";
                        productPrint += "<tr>";
                        productPrint += "<td>Nick</td>";
                        productPrint += "<td>" + customer.Nick.ToTitleCase() + "</td>";
                        productPrint += "</tr>";
                        productPrint += "<tr>";
                        productPrint += "<td>Điện thoại</td>";
                        productPrint += "<td>" + order.CustomerPhone + "</td>";
                        productPrint += "</tr>";

                        productPrint += "<tr>";
                        if (numberOfOrders.Count < 4)
                        {
                            productPrint += "<td>Loại đơn</td>";
                            if (order.OrderType == 1)
                                productPrint += "<td>Mua lẻ</td>";
                            if (order.OrderType == 2)
                                productPrint += "<td>Mua sỉ</td>";
                            productPrint += "</tr>";
                        }
                        if (!string.IsNullOrEmpty(order.DateDone.ToString()))
                        {
                            productPrint += "<tr>";
                            productPrint += "<td>Ngày hoàn tất</td>";
                            string datedone = string.Format("{0:dd/MM/yyyy HH:mm}", order.DateDone);
                            productPrint += "<td>" + datedone + "</td>";
                            productPrint += "</tr>";
                        }
                        else
                        {
                            error += "Đơn hàng chưa hoàn tất";
                        }

                        productPrint += "<tr>";
                        productPrint += "<td>Nhân viên</td>";
                        productPrint += "<td>" + order.CreatedBy + "</td>";
                        productPrint += "</tr>";

                        if (!string.IsNullOrEmpty(order.OrderNote)) {
                            productPrint += "<tr>";
                            productPrint += "<td>Ghi chú</td>";
                            productPrint += "<td>" + order.OrderNote + "</td>";
                            productPrint += "</tr>";
                        }

                        productPrint += "</tbody>";
                        productPrint += "</table>";
                        productPrint += "</div>";

                        productPrint += "<div class=\"table-2\">";
                        productPrint += "<table>";
                        productPrint += "<colgroup>";
                        productPrint += "<col class=\"soluong\" />";
                        productPrint += "<col class=\"gia\" />";
                        productPrint += "<col class=\"tong\"/>";
                        productPrint += "</colgroup>";
                        productPrint += "<thead>";
                        productPrint += "<th>Số lượng</th>";
                        productPrint += "<th>Giá</th>";
                        productPrint += "<th>Tổng</th>";
                        productPrint += "</thead>";
                        productPrint += "<tbody>";
                        productPrint += Print;
                        productPrint += "</tbody>";
                        productPrint += "</table>";
                        productPrint += "</div>";

                        productPrint += "<div class=\"table-3\">";
                        productPrint += "<table>";
                        productPrint += "<tr>";
                        productPrint += "<td colspan=\"2\">Số lượng</td>";
                        productPrint += "<td>" + TotalQuantity + "</td>";
                        productPrint += "</tr>";
                        productPrint += "<tr>";
                        productPrint += "<td colspan=\"2\">Thành tiền</td>";
                        productPrint += "<td>" + string.Format("{0:N0}", TotalOrder) + "</td>";
                        productPrint += "</tr>";

                        double TotalPrice = TotalOrder;

                        if (order.DiscountPerProduct > 0)
                        {
                            var TotalDiscount = Convert.ToDouble(order.DiscountPerProduct) * Convert.ToDouble(TotalQuantity);
                            TotalOrder = TotalOrder - TotalDiscount;
                            TotalPrice = TotalPrice - TotalDiscount;
                            productPrint += "<tr>";
                            productPrint += "<td colspan=\"2\">Chiết khấu mỗi cái </td>";
                            productPrint += "<td>" + string.Format("{0:N0}", Convert.ToDouble(order.DiscountPerProduct)) + "</td>";
                            productPrint += "</tr>";
                            productPrint += "<tr>";
                            productPrint += "<td colspan=\"2\">Trừ tổng chiết khấu</td>";
                            productPrint += "<td>" + string.Format("{0:N0}", TotalDiscount) + "</td>";
                            productPrint += "</tr>";
                            productPrint += "<tr>";
                            productPrint += "<td colspan=\"2\">Sau chiết khấu</td>";
                            productPrint += "<td>" + string.Format("{0:N0}", TotalOrder) + "</td>";
                            productPrint += "</tr>";
                        }
                        
                        if (order.RefundsGoodsID != null)
                        {
                            var refund = RefundGoodController.GetByID(Convert.ToInt32(order.RefundsGoodsID));
                            if(refund != null)
                            {
                                TotalOrder = TotalOrder - Convert.ToDouble(refund.TotalPrice);

                                productPrint += "<tr>";
                                productPrint += "<td colspan=\"2\">Trừ hàng trả (đơn " + order.RefundsGoodsID + ")</td>";
                                productPrint += "<td>" + string.Format("{0:N0}", Convert.ToDouble(refund.TotalPrice)) + "</td>";
                                productPrint += "</tr>";

                                productPrint += "<tr>";
                                productPrint += "<td colspan=\"2\">Tổng tiền còn lại</td>";
                                productPrint += "<td>" + string.Format("{0:N0}", TotalOrder) + "</td>";
                                productPrint += "</tr>";
                            }
                            else
                            {
                                error += "Không tìm thấy đơn hàng đổi trả " + order.RefundsGoodsID.ToString();
                            }

                        }
                        

                        if (Convert.ToDouble(order.FeeShipping) > 0)
                        {
                            TotalOrder = TotalOrder + Convert.ToDouble(order.FeeShipping);
                            TotalPrice = TotalPrice + Convert.ToDouble(order.FeeShipping);
                            productPrint += "<tr>";
                            productPrint += "<td colspan=\"2\">Phí vận chuyển</td>";
                            productPrint += "<td>" + string.Format("{0:N0}", Convert.ToDouble(order.FeeShipping)) + "</td>";
                            productPrint += "</tr>";
                        }

                        // Check fee
                        var fees = FeeController.getFeeInfo(ID);
                        foreach (var fee in fees)
                        {
                            TotalOrder = TotalOrder + Convert.ToDouble(fee.Price);
                            TotalPrice = TotalPrice + Convert.ToDouble(fee.Price);
                            productPrint += "<tr>";
                            productPrint += "<td colspan=\"2\">" + fee.Name + "</td>";
                            productPrint += "<td>" + string.Format("{0:N0}", Convert.ToDouble(fee.Price)) + "</td>";
                            productPrint += "</tr>";
                        }

                        if (TotalPrice != Convert.ToDouble(order.TotalPrice))
                        {
                            error += "Đơn hàng tính sai tổng tiền";
                        }

                        productPrint += "<tr>";
                        productPrint += "<td class=\"strong\" colspan=\"2\">TỔNG CỘNG</td>";
                        productPrint += "<td class=\"strong\">" + string.Format("{0:N0}", TotalOrder) + "</td>";
                        productPrint += "</tr>";
                        
                        
                        productPrint += "</tbody>";
                        productPrint += "</table>";
                        productPrint += "</div>";
                        productPrint += "</div>";


                        string address = "";
                        string phone = "";
                        string facebook = "";
                        var agent = AgentController.GetByID(Convert.ToInt32(order.AgentID));
                        if (agent != null)
                        {
                            address = agent.AgentAddress;
                            phone = agent.AgentPhone;
                            facebook = agent.AgentFacebook;
                        }

                        string dateOrder = string.Format("{0:dd/MM/yyyy HH:mm}", order.DateDone);

                        shtml += "<div class=\"hoadon\">";
                        shtml += "<div class=\"all\">";
                        shtml += "<div class=\"head\">";

                        if (numberOfOrders.Count < 5)
                        {
                            shtml += "<div class=\"logo\"><div class=\"img\"><img src=\"App_Themes/Ann/image/logo.png\" /></div></div>";
                        }

                        if (numberOfOrders.Count < 4)
                        {
                            shtml += "<div class=\"info\">";

                            shtml += "<div class=\"ct\">";
                            shtml += "<div class=\"ct-title\"></div>";
                            shtml += "<div class=\"ct-detail\"> " + address + "</div>";
                            shtml += "</div>";

                            shtml += "<div class=\"ct\">";
                            shtml += "<div class=\"ct-title\"> </div>";
                            shtml += "<div class=\"ct-detail\"> " + phone + "</div>";
                            shtml += "</div>";

                            shtml += "<div class=\"ct\">";
                            shtml += "<div class=\"ct-title\"></div>";
                            shtml += "<div class=\"ct-detail\">https://ann.com.vn</div>";
                            shtml += "</div>";

                            shtml += "</div>";
                        }

                        shtml += "</div>";

                        shtml += productPrint;

                        if (numberOfOrders.Count < 5)
                        {
                            shtml += "<div class=\"footer\"><h3>CẢM ƠN QUÝ KHÁCH! HẸN GẶP LẠI !</h3></div> ";
                        }

                        var config = ConfigController.GetByTop1();
                        string rule = "";
                        if(order.OrderType == 2)
                        {
                            rule = config.ChangeGoodsRule;
                        }
                        else
                        {
                            rule = config.RetailReturnRule;
                        }
                        
                        if (numberOfOrders.Count < 4)
                        {
                            shtml += "<div class=\"footer\">" + rule +"</div> ";
                        }
                        else
                        {
                            shtml += "<div class=\"footer\">";
                            shtml += "<p>ANN rất vui khi quý khách đã mua được " + numberOfOrders.Count + " đơn hàng!</p>";
                            shtml += "</div>";
                        }

                        shtml += "</div>";
                        shtml += "</div>";

                        if (error != "")
                        {
                            ltrPrintInvoice.Text = "Xảy ra lỗi: " + error;
                            ltrPrintEnable.Text = "";
                        }
                        else
                        {
                            ltrPrintInvoice.Text = shtml;
                            ltrPrintEnable.Text = "<div class=\"print-enable true\"></div>";
                        }
                    }
                }
                else
                {
                    ltrPrintInvoice.Text = "Không tìm thấy đơn hàng " + ID;
                }
            }
            else
            {
                ltrPrintInvoice.Text = "Xảy ra lỗi!!!";
            }
        }
    }
}