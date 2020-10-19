﻿using IM_PJ.Controllers;
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

namespace IM_PJ
{
    public partial class print_order_image_english : System.Web.UI.Page
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
            var orderdetails = OrderDetailController.GetByOrderID(ID);
            if (orderdetails.Count > 0)
            {
                if (mergeprint == 0)
                {
                    int t = 0;
                    int print = 1;
                    foreach (var item in orderdetails)
                    {
                        TotalQuantity += Convert.ToDouble(item.Quantity);

                        int ProductType = Convert.ToInt32(item.ProductType);
                        double ItemPrice = Convert.ToDouble(item.Price);
                        string SKU = item.SKU;
                        string ProductName = "";
                        string ProductImage = "";
                        int SubTotal = Convert.ToInt32(ItemPrice) * Convert.ToInt32(item.Quantity);

                        t++;
                        Print += "<tr>";
                        Print += "<td id='" + SKU + "'>" + t + "</td>";

                        if (ProductType == 1)
                        {
                            var product = ProductController.GetBySKU(SKU);
                            if (product != null)
                            {
                                ProductName = product.ProductTitle;
                                if (!string.IsNullOrEmpty(product.ProductImage))
                                {
                                    ProductImage = product.ProductImage;
                                }
                                Print += "<td><image src='" + Thumbnail.getURL(ProductImage, Thumbnail.Size.Large) + "' /></td> ";
                                Print += "<td><strong>" + SKU + "</strong> - " + (product.Old_Price > 0 ? "<span class='sale-icon'>SALE</span> " : "") + PJUtils.Truncate(ProductName, 30) + "</td> ";
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

                                    if (string.IsNullOrEmpty(productvariable.Image))
                                    {
                                        ProductImage = parent_product.ProductImage;
                                    }
                                    else
                                    {
                                        ProductImage = productvariable.Image;
                                    }
                                }

                                Print += "<td id='" + parent_product.ProductSKU + "'><image src='" + Thumbnail.getURL(ProductImage, Thumbnail.Size.Large) + "' /></td>";
                                Print += "<td><p><strong>" + SKU + "</strong> - " + (parent_product.Old_Price > 0 ? "<span class='sale-icon'>SALE</span> " : "") + PJUtils.Truncate(ProductName, 30) + "</p><p class='variable'>" + item.ProductVariableDescrition.Replace("|", ". ") + "</p></td> ";
                            }
                        }

                        Print += "<td>" + item.Quantity + "</td>";
                        Print += "<td>" + string.Format("{0:N0}", ItemPrice) + "</td>";
                        Print += "<td>" + string.Format("{0:N0}", SubTotal) + "</td>";
                        Print += "</tr>";

                        TotalOrder += SubTotal;

                        if (t % 10 == 0)
                        {
                            if (t == print * 10)
                            {
                                continue;
                            }
                            Print += "</tbody>";
                            Print += "</table>";
                            Print += "</div>";
                            Print += "</div>";
                            Print += "</div>";
                            Print += "</div>";

                            Print += "<div class='print-order-image'>";
                            Print += "<div class='all print print-" + print + "'>";
                            Print += "<div class='body'>";
                            Print += "<div class='table-2'>";
                            Print += "<table>";
                            Print += "<colgroup>";
                            Print += "<col class='order-item' />";
                            Print += "<col class='image' />";
                            Print += "<col class='name' />";
                            Print += "<col class='quantity' />";
                            Print += "<col class='price' />";
                            Print += "<col class='subtotal' />";
                            Print += "</colgroup>";
                            Print += "<thead>";
                            Print += "<th>#</th>";
                            Print += "<th>Image</th>";
                            Print += "<th>Item</th>";
                            Print += "<th>Qty</th>";
                            Print += "<th>Unit Price</th>";
                            Print += "<th>Amount</th>";
                            Print += "</thead>";
                            Print += "<tbody>";
                            print++;
                        }
                    }
                }
                else
                {
                    int t = 0;
                    int print = 1;
                    for (int i = 0; i < orderdetails.Count; i++)
                    {
                        if (orderdetails[i] != null)
                        {
                            t++;
                            Print += "<tr>";
                            Print += "<td>" + t + "</td>";

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
                            Print += "<td>" + category.CategoryName + " " + string.Format("{0:N0}", ItemPrice1) + "</td>";
                            Print += "<td>" + quantity + "</td>";
                            Print += "<td>" + string.Format("{0:N0}", ItemPrice1) + "</td>";
                            Print += "<td>" + string.Format("{0:N0}", SubTotal) + "</td>";
                            Print += "</tr>";
                            TotalOrder += SubTotal;
                            TotalQuantity += quantity;
                        }

                        if (t % 20 == 0)
                        {
                            if (t == print * 20)
                            {
                                continue;
                            }
                            Print += "</tbody>";
                            Print += "</table>";
                            Print += "</div>";
                            Print += "</div>";
                            Print += "</div>";
                            Print += "</div>";

                            Print += "<div class=\"print-order-image\">";
                            Print += "<div class=\"all print print-" + print + "\">";
                            Print += "<div class=\"body\">";
                            Print += "<div class=\"table-2\">";
                            Print += "<table>";
                            Print += "<colgroup>";
                            Print += "<col class=\"order-item\" />";
                            Print += "<col class=\"name\" />";
                            Print += "<col class=\"quantity\" />";
                            Print += "<col class=\"price\" />";
                            Print += "<col class=\"subtotal\"/>";
                            Print += "</colgroup>";
                            Print += "<thead>";
                            Print += "<th>#</th>";
                            Print += "<th>Image</th>";
                            Print += "<th>Item</th>";
                            Print += "<th>Qty</th>";
                            Print += "<th>Unit Price</th>";
                            Print += "<th>Amount</th>";
                            Print += "</thead>";
                            Print += "<tbody>";
                            print++;
                        }

                    }
                }
            }
        }
        public void LoadData()
        {
            int ID = Request.QueryString["id"].ToInt(0);
            int mergeprint = 0;
            if (Request.QueryString["merge"] != null)
            {
                mergeprint = Request.QueryString["merge"].ToInt(0);
            }

            if (ID > 0)
            {
                var order = OrderController.GetByID(ID);
                
                if (order != null)
                {
                    #region Lấy ghi chú đơn hàng cũ

                    var oldOrders = OrderController.GetAllNoteByCustomerID(Convert.ToInt32(order.CustomerID), ID);
                    if (oldOrders.Count() > 1)
                    {
                        StringBuilder notestring = new StringBuilder();
                        foreach (var item in oldOrders)
                        {
                            notestring.AppendLine(String.Format("<li>Đơn <strong><a href='/thong-tin-don-hang?id={0}' target='_blank'>{0}</a></strong> (<em>{1}<em>): {2}</li>", item.ID, item.DateDone, item.OrderNote));
                        }

                        StringBuilder notehtml = new StringBuilder();
                        notehtml.AppendLine("<div id='old-order-note'>");
                        notehtml.AppendLine("   <h2>" + oldOrders.Count() + " đơn hàng cũ gần nhất có ghi chú:</h2>");
                        notehtml.AppendLine("   <ul>");
                        notehtml.AppendLine(String.Format("{0}", notestring));
                        notehtml.AppendLine("   </ul>");
                        notehtml.AppendLine("</div>");

                        ltrOldOrderNote.Text = notehtml.ToString();
                    }

                    #endregion

                    ltrCopyInvoiceURL.Text = "<a href='javascript:;' onclick='copyInvoiceURLEnglish(" + order.ID + ", " + order.CustomerID + ")' title='Copy link hóa đơn' class='btn btn-violet h45-btn'>Copy link hóa đơn</a>";
                    ltrEnglishInvoice.Text = "<a href='/print-order-image?id=" + order.ID + "&merge=0' title='Lấy ảnh đơn hàng Tiếng Việt' class='btn btn-orange h45-btn'>Ảnh Tiếng Việt</a>";
                    string error = "";
                    string Print = "";

                    double TotalQuantity = 0;
                    double TotalOrder = 0;

                    var orderdetails = OrderDetailController.GetByIDSortBySKU(ID);

                    var numberOfOrders = OrderController.GetByCustomerID(Convert.ToInt32(order.CustomerID));

                    if (orderdetails.Count > 0)
                    {
                        printItemList(ref ID, ref mergeprint, ref TotalQuantity, ref TotalOrder, ref Print);

                        string productPrint = "";
                        string shtml = "";

                        productPrint += "<div class=\"body\">";
                        productPrint += "<div class=\"table-1\">";
                        productPrint += "<h1>INVOICE #" + order.ID + "</h1>";
                        productPrint += "<div class=\"note\">";
                        productPrint += "</div>";
                        productPrint += "<table>";
                        productPrint += "<colgroup >";
                        productPrint += "<col class=\"col-left\"/>";
                        productPrint += "<col class=\"col-right\"/>";
                        productPrint += "</colgroup>";
                        productPrint += "<tbody>";
                        productPrint += "<tr>";
                        productPrint += "<td>Customer name</td>";
                        productPrint += "<td class=\"customer-name\">" + order.CustomerName + "</td>";
                        productPrint += "</tr>";
                        productPrint += "<tr>";
                        productPrint += "<td>Phone number</td>";
                        productPrint += "<td>" + order.CustomerPhone + "</td>";
                        productPrint += "</tr>";
                        productPrint += "<tr>";
                        productPrint += "<td>Invoice date</td>";
                        string date = string.Format("{0:dd/MM/yyyy HH:mm}", order.CreatedDate);
                        productPrint += "<td>" + date + "</td>";
                        productPrint += "</tr>";  
                        productPrint += "</tr>";
                        productPrint += "<tr>";
                        productPrint += "<td>Staff</td>";
                        productPrint += "<td>" + order.CreatedBy + "</td>";
                        productPrint += "</tr>";

                        if (!string.IsNullOrEmpty(order.OrderNote)) {
                            productPrint += "<tr>";
                            productPrint += "<td>Note</td>";
                            productPrint += "<td>" + order.OrderNote + "</td>";
                            productPrint += "</tr>";
                        }

                        productPrint += "</tbody>";
                        productPrint += "</table>";
                        productPrint += "</div>";
                        productPrint += "<div class=\"table-2\">";
                        productPrint += "<table>";
                        productPrint += "<colgroup>";
                        productPrint += "<col class=\"order-item\" />";

                        if(mergeprint == 0)
                        {
                            productPrint += "<col class=\"image\" />";
                        }
                        
                        productPrint += "<col class=\"name\" />";
                        productPrint += "<col class=\"quantity\" />";
                        productPrint += "<col class=\"price\" />";
                        productPrint += "<col class=\"subtotal\"/>";
                        productPrint += "</colgroup>";
                        productPrint += "<thead>";
                        productPrint += "<th>#</th>";

                        if (mergeprint == 0)
                        {
                            productPrint += "<th>Image</th>";
                        }
                        
                        productPrint += "<th>Item</th>";
                        productPrint += "<th>Qty</th>";
                        productPrint += "<th>Unit Price</th>";
                        productPrint += "<th>Amount</th>";
                        productPrint += "</thead>";
                        productPrint += "<tbody>";
                        productPrint += Print;
                        productPrint += "<tr>";

                        string colspan = "5";
                        if (mergeprint == 1)
                        {
                            colspan = "4";
                        }

                        productPrint += "<td colspan=\"" + colspan + "\" class=\"align-right\">Total quantity</td>";
                        productPrint += "<td>" + TotalQuantity + "</td>";
                        productPrint += "</tr>";
                        productPrint += "<tr>";
                        productPrint += "<td colspan=\"" + colspan + "\" class=\"align-right\">Subtotal</td>";
                        productPrint += "<td>" + string.Format("{0:N0}", TotalOrder) + "</td>";
                        productPrint += "</tr>";

                        double TotalPrice = TotalOrder;

                        if (order.DiscountPerProduct > 0)
                        {
                            var TotalDiscount = Convert.ToDouble(order.DiscountPerProduct) * Convert.ToDouble(TotalQuantity);
                            TotalOrder = TotalOrder - TotalDiscount;
                            TotalPrice = TotalPrice - TotalDiscount;
                            productPrint += "<tr>";
                            productPrint += "<td colspan=\"" + colspan + "\" class=\"align-right\">Discount per unit</td>";
                            productPrint += "<td>" + string.Format("{0:N0}", Convert.ToDouble(order.DiscountPerProduct)) + "</td>";
                            productPrint += "</tr>";
                            productPrint += "<tr>";
                            productPrint += "<td colspan=\"" + colspan + "\" class=\"align-right\">Total discount</td>";
                            productPrint += "<td>-" + string.Format("{0:N0}", TotalDiscount) + "</td>";
                            productPrint += "</tr>";
                            productPrint += "<tr>";
                            productPrint += "<td colspan=\"" + colspan + "\" class=\"align-right\">After discount</td>";
                            productPrint += "<td>" + string.Format("{0:N0}", TotalOrder) + "</td>";
                            productPrint += "</tr>";
                        }
                        
                        if (order.RefundsGoodsID != null && order.RefundsGoodsID != 0)
                        {
                            var refund = RefundGoodController.GetByID(Convert.ToInt32(order.RefundsGoodsID));
                            if (refund != null)
                            {
                                TotalOrder = TotalOrder - Convert.ToDouble(refund.TotalPrice);

                                productPrint += "<tr>";
                                productPrint += "<td colspan=\"" + colspan + "\" class=\"align-right\">Return order (ID " + order.RefundsGoodsID + ")</td>";
                                productPrint += "<td>-" + string.Format("{0:N0}", Convert.ToDouble(refund.TotalPrice)) + "</td>";
                                productPrint += "</tr>";

                                productPrint += "<tr>";
                                productPrint += "<td colspan=\"" + colspan + "\" class=\"align-right\">Total remaining</td>";
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
                            productPrint += "<td colspan=\"" + colspan + "\" class=\"align-right\">Shipping</td>";
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
                            productPrint += "<td colspan=\"" + colspan + "\" class=\"align-right\">" + fee.Name + "</td>";
                            productPrint += "<td>" + string.Format("{0:N0}", Convert.ToDouble(fee.Price)) + "</td>";
                            productPrint += "</tr>";
                        }

                        // Giảm giá
                        if (order.CouponID.HasValue && order.CouponID.Value > 0)
                        {
                            var coupon = CouponController.getCoupon(order.CouponID.Value);

                            TotalOrder = TotalOrder - Convert.ToDouble(coupon.Value);
                            TotalPrice = TotalPrice - Convert.ToDouble(coupon.Value);
                            productPrint += "<tr>";
                            productPrint += String.Format("<td colspan='{0}' class='align-right'>Coupon code: {1}</td>", colspan, coupon.Code);
                            productPrint += String.Format("<td>-{0:N0}</td>", Convert.ToDouble(coupon.Value));
                            productPrint += "</tr>";
                        }

                        if (TotalPrice != Convert.ToDouble(order.TotalPrice))
                        {
                            error += "Đơn hàng tính sai tổng tiền";
                        }

                        productPrint += "<tr>";
                        productPrint += "<td colspan=\"" + colspan + "\" class=\"strong align-right\">TOTAL</td>";
                        productPrint += "<td class=\"strong\">" + string.Format("{0:N0}", TotalOrder) + "</td>";
                        productPrint += "</tr>";
                        
                        
                        productPrint += "</tbody>";
                        productPrint += "</table>";
                        productPrint += "</div>";
                        productPrint += "</div>";

                        string dateOrder = string.Format("{0:dd/MM/yyyy HH:mm}", order.DateDone);

                        shtml += "<div class=\"print-order-image\">";
                        shtml += "<div class=\"all print print-0\">";
                        

                        shtml += productPrint;

                        shtml += "</div>";
                        shtml += "</div>";

                        if (error != "")
                        {
                            ltrPrintInvoice.Text = "Xảy ra lỗi: " + error;
                        }
                        else
                        {
                            ltrPrintInvoice.Text = shtml;
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