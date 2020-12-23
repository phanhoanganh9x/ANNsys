using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Models.Common;
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
        
        #region Private
        private int _getCategory(string SKU, int ProductType)
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

        private void _printProduct(List<tbl_OrderDetail> orderdetails, ref double TotalQuantity, ref double TotalOrder, ref string Print) {
            var t = 0;
            var print = 1;

            foreach (var item in orderdetails)
            {
                TotalQuantity += Convert.ToDouble(item.Quantity);

                var ProductType = Convert.ToInt32(item.ProductType);
                var ItemPrice = Convert.ToDouble(item.Price);
                var SKU = item.SKU;
                var ProductName = String.Empty;
                var ProductImage = String.Empty;
                var SubTotal = Convert.ToInt32(ItemPrice) * Convert.ToInt32(item.Quantity);

                t++;
                Print += "<tr>";
                Print += "<td id='" + SKU + "'>" + t + "</td>";

                if (ProductType == 1)
                {
                    var product = ProductController.GetBySKU(SKU);

                    if (product != null)
                    {
                        if (String.IsNullOrEmpty(product.EnName))
                        {
                            var category = CategoryController.GetByID(product.CategoryID.Value);

                            if (category == null)
                                ProductName = SKU;
                            else
                                ProductName = String.Format("{0} - {1}", SKU, category.EnName);
                        }
                        else {
                            ProductName = product.EnName;
                        }
                        
                        if (!String.IsNullOrEmpty(product.ProductImage))
                            ProductImage = product.ProductImage;

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
                            if (String.IsNullOrEmpty(parent_product.EnName))
                            {
                                var category = CategoryController.GetByID(parent_product.CategoryID.Value);

                                if (category == null)
                                    ProductName = SKU;
                                else
                                    ProductName = String.Format("{0} - {1}", SKU, category.EnName);
                            }
                            else
                            {
                                ProductName = parent_product.EnName;
                            }

                            if (string.IsNullOrEmpty(productvariable.Image))
                                ProductImage = parent_product.ProductImage;
                            else
                                ProductImage = productvariable.Image;
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

        private void _printProductGroup(List<tbl_OrderDetail> orderdetails, ref double TotalQuantity, ref double TotalOrder, ref string Print)
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
                    int categoryID1 = _getCategory(orderdetails[i].SKU, Convert.ToInt32(orderdetails[i].ProductType));

                    int quantity = Convert.ToInt32(orderdetails[i].Quantity);

                    for (int j = i + 1; j < orderdetails.Count; j++)
                    {
                        if (orderdetails[j] != null)
                        {
                            int categoryID2 = _getCategory(orderdetails[j].SKU, Convert.ToInt32(orderdetails[j].ProductType));

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

        private void _printBody(int mergeprint, List<tbl_OrderDetail> orderdetails, ref double TotalQuantity, ref double TotalOrder, ref string Print)
        {
            if (mergeprint == 0)
                _printProduct(orderdetails, ref TotalQuantity, ref TotalOrder, ref Print);
            else
                _printProductGroup(orderdetails, ref TotalQuantity, ref TotalOrder, ref Print);
        }
        #endregion

        public void LoadData()
        {
            #region Lấy thông tin query params
            var ID = 0;
            var mergeprint = 0;
            var currencyCode = Currency.USD;

            #region Lấy thông tin đơn hàng
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                ID = Request.QueryString["id"].ToInt(0);
            
            if (ID <= 0)
            {
                ltrPrintInvoice.Text = "Xảy ra lỗi!!!";
                return;
            }

            var order = OrderController.GetByID(ID);

            if (order == null)
            {
                ltrPrintInvoice.Text = "Không tìm thấy đơn hàng " + ID;
                return;
            }
            #endregion

            #region Thông tin tiền tệ
            if (!String.IsNullOrEmpty(Request.QueryString["currencyCode"]))
                currencyCode = Request.QueryString["currencyCode"].Trim();

            var currency = CurrencyController.getByCode(currencyCode);

            if (currency == null)
            {
                ltrPrintInvoice.Text = String.Format("Không tìm thấy tỉ giá tiền tệ của mã {0} này.", currencyCode);
                return;
            }
            
            var currencyRate = Convert.ToDouble(currency.SellingRate);
            #endregion

            if (!String.IsNullOrEmpty(Request.QueryString["merge"]))
                mergeprint = Request.QueryString["merge"].ToInt(0);
            #endregion
                            
            #region Tạo hoá đơn
            var error = String.Empty;
            var Print = String.Empty;

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

            #region Support buttons
            ltrCopyInvoiceURL.Text = "<a href='javascript:;' onclick='copyInvoiceURLEnglish(" + order.ID + ", " + order.CustomerID + ")' title='Copy link hóa đơn' class='btn btn-violet h45-btn'>Copy link hóa đơn</a>";

            #region Currencies Drop Down List
            ltrEnglishInvoice.Text += "<select id='dllCurrency' onchange='onChangeCurrency($(this).val())'>";
            ltrEnglishInvoice.Text += "    <option value='VND'>VND - Việt Nam Đồng</option>";
            ltrEnglishInvoice.Text += "    <option value='USD'>$ - US Dollar</option>";
            ltrEnglishInvoice.Text += "    <option value='AUD'>A$ - Australian Dollar</option>";
            ltrEnglishInvoice.Text += "    <option value='JPY'>¥ - Japanese Yen</option>";
            ltrEnglishInvoice.Text += "    <option value='SGD'>SGD - Singapore Dollar</option>";
            ltrEnglishInvoice.Text += "    <option value='MYR'>MYR - Malaysian Ringgit</option>";
            ltrEnglishInvoice.Text += "    <option value='TWD'>NT$ - TWD - Taiwan New Dollar</option>";
            ltrEnglishInvoice.Text += "</select>";
            #endregion
            #endregion

            #region Thông tin đơn hơn    
            var productPrint = String.Empty;
            var shtml = String.Empty;   
            var TotalQuantity = 0D;
            var TotalOrder = 0D;
            var orderdetails = OrderDetailController.GetByIDSortBySKU(ID)
                .Select(x => {
                    var price = x.Price.HasValue ? x.Price.Value : 0;

                    x.Price = Math.Ceiling(1e2 * price / currencyRate) * 1e-2;
                    
                    return x; 
                })
                .ToList();

            if (orderdetails.Count == 0)
                return;
            
            #region Title của hoá đơn
            productPrint += "<div class=\"body\">";
            productPrint += "<div class=\"table-1\">";
            productPrint += "<h1>INVOICE #" + order.ID + "</h1>";
            productPrint += "<div class=\"note\">";
            productPrint += "</div>";
            #endregion

            #region Thông tin khách hàng
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
            productPrint += String.Format("<td>{0:dd/MM/yyyy HH:mm}</td>", order.CreatedDate);
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
            #endregion

            #region Thông tin sản phẩm
            productPrint += "<div class=\"table-2\">";
            productPrint += "<table>";
            productPrint += "<colgroup>";
            productPrint += "<col class=\"order-item\" />";

            if(mergeprint == 0)
                productPrint += "<col class=\"image\" />";
            
            productPrint += "<col class=\"name\" />";
            productPrint += "<col class=\"quantity\" />";
            productPrint += "<col class=\"price\" />";
            productPrint += "<col class=\"subtotal\"/>";
            productPrint += "</colgroup>";
            productPrint += "<thead>";
            productPrint += "<th>#</th>";

            if (mergeprint == 0)
                productPrint += "<th>Image</th>";
            
            productPrint += "<th>Item</th>";
            productPrint += "<th>Qty</th>";
            productPrint += "<th>Unit Price</th>";
            productPrint += "<th>Amount</th>";
            productPrint += "</thead>";
            productPrint += "<tbody>";
            _printBody(mergeprint, orderdetails, ref TotalQuantity, ref TotalOrder, ref Print);
            productPrint += Print;
            productPrint += "<tr>";

            string colspan = "5";
            if (mergeprint == 1)
            {
                colspan = "4";
            }
            #endregion

            #region Thông tin tổng hợp đơn hàng
            productPrint += "<td colspan=\"" + colspan + "\" class=\"align-right\">Total quantity</td>";
            productPrint += "<td>" + TotalQuantity + "</td>";
            productPrint += "</tr>";
            productPrint += "<tr>";
            productPrint += "<td colspan=\"" + colspan + "\" class=\"align-right\">Subtotal</td>";
            productPrint += "<td>" + string.Format("{0:N0}", TotalOrder) + "</td>";
            productPrint += "</tr>";

            #region Thông tin triết khấu 
            double TotalPrice = TotalOrder;

            if (order.DiscountPerProduct > 0)
            {
                var TotalDiscount = 0D;
                var discount = order.DiscountPerProduct.HasValue ? order.DiscountPerProduct.Value : 0;
                
                discount = Math.Floor(1e2 * discount / currencyRate) * 1e-2;
                TotalDiscount = discount * TotalQuantity;
                TotalOrder = TotalOrder - TotalDiscount;
                TotalPrice = TotalPrice - TotalDiscount;
                productPrint += "<tr>";
                productPrint += "<td colspan=\"" + colspan + "\" class=\"align-right\">Discount per unit</td>";
                productPrint += String.Format("<td>{0:N0}</td>", discount);
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
            #endregion

            #region Thông tin đổi trả hàng
            if (order.RefundsGoodsID != null && order.RefundsGoodsID != 0)
            {
                var refund = RefundGoodController.GetByID(Convert.ToInt32(order.RefundsGoodsID));

                if (refund != null)
                {
                    var totalRefund = Convert.ToDouble(refund.TotalPrice);
                    
                    totalRefund = Math.Floor(1e2 * totalRefund / currencyRate) * 1e-2;
                    TotalOrder = TotalOrder - totalRefund;

                    productPrint += "<tr>";
                    productPrint += "<td colspan=\"" + colspan + "\" class=\"align-right\">Return order (ID " + order.RefundsGoodsID + ")</td>";
                    productPrint += String.Format("<td>-{0:N0}</td>", totalRefund);
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
            #endregion

            #region Thông tin phí giao hàng
            var feeShipping = Convert.ToDouble(order.FeeShipping);
            
            feeShipping = Math.Ceiling(1e2 * feeShipping / currencyRate) * 1e-2;

            if (feeShipping > 0)
            {
                TotalOrder = TotalOrder + feeShipping;
                TotalPrice = TotalPrice + feeShipping;
                productPrint += "<tr>";
                productPrint += "<td colspan=\"" + colspan + "\" class=\"align-right\">Shipping</td>";
                productPrint += String.Format("<td>{0:N0}</td>", feeShipping);
                productPrint += "</tr>";
            }
            #endregion

            #region Thông tin phí khách
            var fees = FeeController.getFeeInfo(ID);

            foreach (var fee in fees)
            {
                var feeOther = Convert.ToDouble(fee.Price);

                feeOther = Math.Ceiling(1e2 * feeOther / currencyRate) * 1e-2;

                if (feeOther > 0)
                {
                    TotalOrder = TotalOrder + feeOther;
                    TotalPrice = TotalPrice + feeOther;
                    productPrint += "<tr>";
                    productPrint += "<td colspan=\"" + colspan + "\" class=\"align-right\">" + fee.Name + "</td>";
                    productPrint += String.Format("<td>{0:N0}</td>", feeOther);
                    productPrint += "</tr>";
                }
            }
            #endregion

            #region Thông tin giảm giá
            if (order.CouponID.HasValue && order.CouponID.Value > 0)
            {
                var coupon = CouponController.getCoupon(order.CouponID.Value);
                var couponValue = Convert.ToDouble(coupon.Value);

                couponValue = Math.Floor(1e2 * couponValue / currencyRate) * 1e-2;
                TotalOrder = TotalOrder - couponValue;
                TotalPrice = TotalPrice - couponValue;
                productPrint += "<tr>";
                productPrint += String.Format("<td colspan='{0}' class='align-right'>Coupon code: {1}</td>", colspan, coupon.Code);
                productPrint += String.Format("<td>-{0:N0}</td>", couponValue);
                productPrint += "</tr>";
            }
            #endregion

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
            #endregion
            #endregion
            #endregion

            #region Xuất hoá đơn
            shtml += "<div class=\"print-order-image\">";
            shtml += "<div class=\"all print print-0\">";
            shtml += productPrint;
            shtml += "</div>";
            shtml += "</div>";

            if (String.IsNullOrEmpty(error))
                ltrPrintInvoice.Text = "Xảy ra lỗi: " + error;
            else
                ltrPrintInvoice.Text = shtml;
            #endregion
        }
    }
}