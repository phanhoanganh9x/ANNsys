using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
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

namespace IM_PJ
{
    public partial class print_invoice : System.Web.UI.Page
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
                        if (acc.RoleID == 0 || acc.RoleID == 2)
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

        /// <summary>
        /// Date:   2021-07-19
        /// Author: Binh-TT
        ///
        /// Đối ứng triết khấu từng dòng
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="merge">0: In chi tiết | 1: In gộp</param>
        public string createBodyHtml(int orderId, int merge)
        {
            var index = 0;
            var bodyHtml = new StringBuilder();
            var orderdetails = OrderDetailController.GetByIDSortBySKU(orderId);

            if (orderdetails.Count > 0)
            {
                if (merge == 0)
                {
                    foreach (var item in orderdetails)
                    {
                        var productType = Convert.ToInt32(item.ProductType);
                        var sku = item.SKU;
                        var productName = String.Empty;
                        var quantity = Convert.ToInt32(item.Quantity);
                        var price = Convert.ToDouble(item.Price);
                        // 2021-07-19: Đối ứng triết khấu từng dòng
                        var discount = item.DiscountPrice.HasValue ? item.DiscountPrice.Value : 0;
                        var total = (price - discount) * quantity;

                        index++;
                        #region Sản phẩm
                        bodyHtml.AppendLine("<tr>");

                        if (productType == 1)
                        {
                            var product = ProductController.GetBySKU(sku);

                            if (product != null)
                                productName = product.ProductTitle;

                            bodyHtml.AppendLine("    <td colspan='3'>");
                            bodyHtml.AppendLine("        " + index + "&ensp;<strong>" + sku + "</strong> - " + (product.Old_Price > 0 ? "<span class='sale-icon'>SALE</span>" : String.Empty));
                            bodyHtml.AppendLine("        " + PJUtils.Truncate(productName, 28));
                            bodyHtml.AppendLine("    </td>");
                        }
                        else
                        {
                            var productvariable = ProductVariableController.GetBySKU(sku);

                            if (productvariable != null)
                            {
                                var parent_product = ProductController.GetByID(Convert.ToInt32(productvariable.ProductID));

                                if (parent_product != null)
                                    productName = parent_product.ProductTitle;

                                bodyHtml.AppendLine("    <td colspan='3'>");
                                bodyHtml.AppendLine("        " + + index + "&ensp;<strong>" + sku + "</strong> - " + (parent_product.Old_Price > 0 ? "<span class='sale-icon'>SALE</span> " : String.Empty));
                                bodyHtml.AppendLine("        " + PJUtils.Truncate(productName, 37));
                                bodyHtml.AppendLine("    </td>");
                            }
                        }
                        bodyHtml.AppendLine("</tr>");
                        #endregion
                        bodyHtml.AppendLine("<tr>");
                        // SL
                        bodyHtml.AppendLine("    <td>" + item.Quantity + "</td>");
                        // Giá
                        bodyHtml.AppendLine("    <td>" + String.Format("{0:N0}", price) + "</td>");
                        // Chiết khấu
                        bodyHtml.AppendLine("    <td>" + String.Format("{0:N0}", discount) + "</td>");
                        // Tổng
                        bodyHtml.AppendLine("    <td>" + string.Format("{0:N0}", total) + "</td>");
                        bodyHtml.AppendLine("</tr>");
                    }
                }
                else
                {
                    for (int i = 0; i < orderdetails.Count; i++)
                    {
                        var item = orderdetails[i];

                        if (item != null)
                        {
                            var categoryId = getCategory(item.SKU, Convert.ToInt32(item.ProductType));
                            var category = CategoryController.GetByID(categoryId);
                            var quantity = Convert.ToInt32(item.Quantity);
                            var price = Convert.ToDouble(item.Price);
                            var discount = Convert.ToDouble(item.DiscountPrice);
                            var total = (price - discount) * quantity;

                            for (int j = i + 1; j < orderdetails.Count; j++)
                            {
                                var item2 = orderdetails[j];

                                if (item2 != null)
                                {
                                    var categoryId2 = getCategory(item2.SKU, Convert.ToInt32(item2.ProductType));
                                    var quantity2 = Convert.ToInt32(item2.Quantity);
                                    var price2 = Convert.ToDouble(item2.Price);
                                    var discount2 = Convert.ToDouble(item2.DiscountPrice);

                                    if (categoryId == categoryId2)
                                    {
                                        quantity += quantity2;
                                        discount += discount2;
                                        total += (price2 - discount2) * quantity2;
                                        item2 = null;
                                    }
                                }
                            }


                            index++;
                            #region Sản phẩm
                            bodyHtml.AppendLine("<tr>");
                            bodyHtml.AppendLine("    <td colspan='3'>");
                            bodyHtml.AppendLine("        " + index + "&ensp;<strong>" + category.CategoryName + " đồng giá " + String.Format("{0:N0}", price) + "</strong>");
                            bodyHtml.AppendLine("    </td>");
                            bodyHtml.AppendLine("</tr>");
                            #endregion
                            bodyHtml.AppendLine("<tr>");
                            // Số lượng
                            bodyHtml.AppendLine("    <td>" + quantity + "</td>");
                            // Giá
                            bodyHtml.AppendLine("    <td>" + String.Format("{0:N0}", price) + "</td>");
                            // Chiết khấu
                            bodyHtml.AppendLine("    <td>" + String.Format("{0:N0}", discount) + "</td>");
                            // Tổng
                            bodyHtml.AppendLine("    <td>" + String.Format("{0:N0}", total) + "</td>");
                            bodyHtml.AppendLine("</tr>");
                        }
                    }
                }
            }

            return bodyHtml.ToString();
        }

        /// <summary>
        /// Date:   2021-07-19
        /// Author: Binh-TT
        ///
        /// Đối ứng triết khấu từng dòng
        /// </summary>
        public void LoadData()
        {
            #region Check ID đơn hàng
            var orderId = Request.QueryString["id"].ToInt(0);

            if (orderId == 0)
            {
                ltrPrintInvoice.Text = "Xảy ra lỗi!!!";
                return;
            }

            var order = OrderController.GetByID(orderId);

            if (order == null)
            {
                ltrPrintInvoice.Text = "Không tìm thấy đơn hàng " + orderId;
                return;
            }
            #endregion

            #region Lấy thông tin parameter in đơn hàng
            var mergep = 0;

            if (!String.IsNullOrEmpty(Request.QueryString["merge"]))
                mergep = Request.QueryString["merge"].ToInt(0);
            #endregion

            var error = String.Empty;
            // Lấy thông tin chi tiết đơn hàng
            var orderdetails = OrderDetailController.GetByIDSortBySKU(orderId);
            // Lấy danh sách các đơn hàng đã mua (Bao gồm đơn hiện tại)
            var oldOrders = OrderController.GetByCustomerID(Convert.ToInt32(order.CustomerID));
            // Lấy thông tin khách hàng
            var customer = CustomerController.GetByID(Convert.ToInt32(order.CustomerID));

            if (orderdetails.Count > 0)
            {
                #region Tạo HTML thông tin đơn hàng
                var orderHtml = new StringBuilder();

                orderHtml.AppendLine("<div class='body'>");

                #region Tiêu đề và thông tin khách hàng
                orderHtml.AppendLine("    <div class='table-1'>");

                #region Tiêu đề
                if (mergep == 1)
                    orderHtml.AppendLine("        <h1>HÓA ĐƠN #" + order.ID + "<p class='merge-alert'>(Đã gộp sản phẩm)<p></h1>");
                else
                    orderHtml.AppendLine("        <h1>HÓA ĐƠN #" + order.ID + "</h1>");
                #endregion

                #region Thông tin khách hàng
                orderHtml.AppendLine("        <table>");
                orderHtml.AppendLine("            <colgroup>");
                orderHtml.AppendLine("                <col class='col-left'/>");
                orderHtml.AppendLine("                <col class='col-right'/>");
                orderHtml.AppendLine("            </colgroup>");
                orderHtml.AppendLine("            <tbody>");

                #region Khách hàng
                orderHtml.AppendLine("                <tr>");
                orderHtml.AppendLine("                    <td>Khách hàng</td>");
                orderHtml.AppendLine("                    <td>" + order.CustomerName.ToTitleCase() + "</td>");
                orderHtml.AppendLine("                </tr>");
                #endregion

                #region Nick
                if (!String.IsNullOrEmpty(customer.Nick) && order.ShippingType != 1)
                {
                    orderHtml.AppendLine("                <tr>");
                    orderHtml.AppendLine("                    <td>Nick</td>");
                    orderHtml.AppendLine("                    <td>" + customer.Nick.ToTitleCase() + "</td>");
                    orderHtml.AppendLine("                </tr>");
                }
                #endregion

                #region Số điện thoại
                orderHtml.AppendLine("                <tr>");
                orderHtml.AppendLine("                    <td>Điện thoại</td>");
                orderHtml.AppendLine("                    <td>" + order.CustomerPhone + "</td>");
                orderHtml.AppendLine("                </tr>");
                #endregion

                #region Ngày hoàn tất
                if (order.ExcuteStatus == 2 && !String.IsNullOrEmpty(order.DateDone.ToString()))
                {
                    orderHtml.AppendLine("                <tr>");
                    orderHtml.AppendLine("                    <td>Ngày hoàn tất</td>");
                    orderHtml.AppendLine("                    <td>" + String.Format("{0:dd/MM/yyyy HH:mm}", order.DateDone) + "</td>");
                    orderHtml.AppendLine("                </tr>");
                }
                else
                {
                    error += "Đơn hàng chưa hoàn tất";
                }
                #endregion

                #region Nhân viên tạo đơn
                orderHtml.AppendLine("                <tr>");
                orderHtml.AppendLine("                    <td>Nhân viên</td>");
                orderHtml.AppendLine("                    <td>" + order.CreatedBy + "</td>");
                orderHtml.AppendLine("                </tr>");
                #endregion

                #region Chú thích
                if (!String.IsNullOrEmpty(order.OrderNote)) {
                    orderHtml.AppendLine("                <tr>");
                    orderHtml.AppendLine("                    <td>Ghi chú</td>");
                    orderHtml.AppendLine("                    <td>" + order.OrderNote + "</td>");
                    orderHtml.AppendLine("                </tr>");
                }
                #endregion

                orderHtml.AppendLine("            </tbody>");
                orderHtml.AppendLine("        </table>");
                #endregion

                orderHtml.AppendLine("    </div>");
                #endregion

                #region Thông tin chi tiết đơn hàng
                orderHtml.AppendLine("    <div class='table-2'>");
                orderHtml.AppendLine("        <table>");
                orderHtml.AppendLine("            <colgroup>");
                orderHtml.AppendLine("                <col class='soluong' />");
                orderHtml.AppendLine("                <col class='gia' />");
                orderHtml.AppendLine("                <col class='chiet-khau' />");
                orderHtml.AppendLine("                <col class='tong' />");
                orderHtml.AppendLine("            </colgroup>");
                orderHtml.AppendLine("            <thead>");
                orderHtml.AppendLine("                <th>Số lượng</th>");
                orderHtml.AppendLine("                <th>Giá</th>");
                orderHtml.AppendLine("                <th>Chiết khấu</th>");
                orderHtml.AppendLine("                <th>Tổng</th>");
                orderHtml.AppendLine("            </thead>");
                orderHtml.AppendLine("            <tbody>");
                orderHtml.AppendLine(createBodyHtml(orderId, mergep));
                orderHtml.AppendLine("            </tbody>");
                orderHtml.AppendLine("        </table>");
                orderHtml.AppendLine("    </div>");
                #endregion

                #region Thông tin tổng hợp đơn hàng
                orderHtml.AppendLine("    <div class='table-3'>");
                orderHtml.AppendLine("        <table>");
                orderHtml.AppendLine("            <tbody>");

                #region Tổng số lượng
                orderHtml.AppendLine("                <tr>");
                orderHtml.AppendLine("                    <td colspan='2'>Số lượng</td>");
                orderHtml.AppendLine("                    <td>" + String.Format("{0:N0}", order.TotalQuantity) + "&nbsp;&nbsp;&nbsp;&nbsp;</td>");
                orderHtml.AppendLine("                </tr>");
                #endregion

                #region Thành tiền
                var totalPrice = Convert.ToDouble(order.TotalPriceNotDiscount);

                orderHtml.AppendLine("                <tr>");
                orderHtml.AppendLine("                    <td colspan='2'>Thành tiền</td>");
                orderHtml.AppendLine("                    <td>" + String.Format("{0:N0}", totalPrice) + "&nbsp;</td>");
                orderHtml.AppendLine("                </tr>");
                #endregion

                #region Chiết khấu
                var totalDiscount = 0D;

                if (order.TotalDiscount > 0)
                {
                    totalDiscount = Convert.ToDouble(order.TotalDiscount);

                    orderHtml.AppendLine("                <tr>");
                    orderHtml.AppendLine("                    <td colspan='2'>Trừ tổng chiết khấu</td>");
                    orderHtml.AppendLine("                    <td>-" + String.Format("{0:N0}", totalDiscount) + "&nbsp;</td>");
                    orderHtml.AppendLine("                </tr>");
                    orderHtml.AppendLine("                <tr>");
                    orderHtml.AppendLine("                    <td colspan='2'>Sau chiết khấu</td>");
                    orderHtml.AppendLine("                    <td>" + String.Format("{0:N0}", totalPrice - totalDiscount) + "&nbsp;</td>");
                    orderHtml.AppendLine("                </tr>");
                }
                #endregion

                #region Đơn hàng đổi trả
                var totalRefunds = 0D;

                if (order.RefundsGoodsID.HasValue)
                {
                    var refund = RefundGoodController.GetByID(Convert.ToInt32(order.RefundsGoodsID));

                    if(refund != null)
                    {
                        totalRefunds = Convert.ToDouble(refund.TotalPrice);

                        orderHtml.AppendLine("                <tr>");
                        orderHtml.AppendLine("                    <td colspan='2'>Trừ hàng trả (đơn " + order.RefundsGoodsID + ")</td>");
                        orderHtml.AppendLine("                    <td>-" + String.Format("{0:N0}", totalRefunds) + "&nbsp;</td>");
                        orderHtml.AppendLine("                </tr>");
                        orderHtml.AppendLine("                <tr>");
                        orderHtml.AppendLine("                    <td colspan='2'>Tổng tiền còn lại</td>");
                        orderHtml.AppendLine("                    <td>" + String.Format("{0:N0}", totalPrice - totalDiscount - totalRefunds) + "&nbsp;</td>");
                        orderHtml.AppendLine("                </tr>");
                    }
                    else
                    {
                        error += "Không tìm thấy đơn hàng đổi trả " + order.RefundsGoodsID.ToString();
                    }

                }
                #endregion

                #region Phí giao hàng
                var shoppingFee = Convert.ToDouble(order.FeeShipping);

                if (shoppingFee > 0)
                {
                    orderHtml.AppendLine("                <tr>");
                    orderHtml.AppendLine("                    <td colspan='2'>Phí vận chuyển</td>");
                    orderHtml.AppendLine("                    <td>" + String.Format("{0:N0}", shoppingFee) + "&nbsp;</td>");
                    orderHtml.AppendLine("                </tr>");
                }
                #endregion

                #region Phí giao hàng
                var totalOtherFee = 0D;
                var fees = FeeController.getFeeInfo(orderId);

                foreach (var fee in fees)
                {
                    var otherFee = Convert.ToDouble(fee.Price);

                    totalOtherFee += otherFee;
                    orderHtml.AppendLine("                <tr>");
                    orderHtml.AppendLine("                    <td colspan='2'>" + fee.Name + "</td>");
                    orderHtml.AppendLine("                    <td>" + String.Format("{0:N0}", otherFee) + "&nbsp;</td>");
                    orderHtml.AppendLine("                </tr>");
                }
                #endregion

                #region Giảm giá
                var couponValue = 0D;

                if (order.CouponID.HasValue && order.CouponID.Value > 0)
                {
                    var coupon = CouponController.getCoupon(order.CouponID.Value);

                    if (coupon != null)
                    {
                        couponValue = Convert.ToDouble(coupon.Value);
                        orderHtml.AppendLine("                <tr>");
                        orderHtml.AppendLine(String.Format("                    <td colspan='2'>Mã giảm giá: {0}</td>", coupon.Code));
                        orderHtml.AppendLine(String.Format("                    <td>-{0:N0}&nbsp;</td>", couponValue));
                        orderHtml.AppendLine("                </tr>");
                    }
                }
                #endregion

                #region Tổng tiền
                var total = (totalPrice - totalDiscount + shoppingFee + totalOtherFee) - totalRefunds;

                if (total != Convert.ToDouble(order.TotalPrice))
                    error += "Đơn hàng tính sai tổng tiền";

                orderHtml.AppendLine("                <tr>");
                orderHtml.AppendLine("                    <td class='strong' colspan='2'>TỔNG TIỀN</td>");
                orderHtml.AppendLine("                    <td class='strong'>" + String.Format("{0:N0}", total) + "&nbsp;</td>");
                orderHtml.AppendLine("                </tr>");
                #endregion

                orderHtml.AppendLine("            </tbody>");
                orderHtml.AppendLine("        </table>");
                orderHtml.AppendLine("    </div>");
                #endregion

                orderHtml.AppendLine("</div>");
                #endregion

                #region Tạo HTML in hóa đơn
                #region Lấy thông tin Shop
                var address = String.Empty;
                var phone = String.Empty;
                var facebook = String.Empty;
                var agent = AgentController.GetByID(Convert.ToInt32(order.AgentID));

                if (agent != null)
                {
                    address = agent.AgentAddress;
                    phone = agent.AgentPhone;
                    facebook = agent.AgentFacebook;
                }

                // Lấy sđt nhân viên tạo đơn
                var acc = AccountController.GetByUsername(order.CreatedBy);

                if (acc != null)
                {
                    var accountInfo = AccountInfoController.GetByUserID(acc.ID);

                    if (accountInfo != null)
                        if (!String.IsNullOrEmpty(accountInfo.Phone))
                            phone = accountInfo.Phone;
                }
                #endregion

                var doneDate = String.Format("{0:dd/MM/yyyy HH:mm}", order.DateDone);
                var invoiceHtml = new StringBuilder();

                invoiceHtml.AppendLine("<div class='hoadon'>");
                invoiceHtml.AppendLine("    <div class='all'>");

                #region Khởi tạo Header
                if (oldOrders.Count < 4)
                {
                    invoiceHtml.AppendLine("        <div class='head'>");
                    #region Logo
                    invoiceHtml.AppendLine("            <div class='logo'>");
                    invoiceHtml.AppendLine("                <div class='img'>");
                    invoiceHtml.AppendLine("                    <img src='App_Themes/Ann/image/logo.png' />");
                    invoiceHtml.AppendLine("                </div>");
                    invoiceHtml.AppendLine("            </div>");
                    #endregion

                    #region Thông tin Shop
                    invoiceHtml.AppendLine("            <div class='info'>");

                    #region Địa chỉ
                    invoiceHtml.AppendLine("                <div class='ct'>");
                    invoiceHtml.AppendLine("                    <div class='ct-title'></div>");
                    invoiceHtml.AppendLine("                    <div class='ct-detail'> " + address + "</div>");
                    invoiceHtml.AppendLine("                </div>");
                    #endregion

                    #region Số điện thoại
                    invoiceHtml.AppendLine("                <div class='ct'>");
                    invoiceHtml.AppendLine("                    <div class='ct-title'> </div>");
                    invoiceHtml.AppendLine("                    <div class='ct-detail'> " + phone + "</div>");
                    invoiceHtml.AppendLine("                </div>");
                    #endregion

                    #region Web
                    invoiceHtml.AppendLine("                <div class='ct'>");
                    invoiceHtml.AppendLine("                    <div class='ct-title'></div>");
                    invoiceHtml.AppendLine("                    <div class='ct-detail'>http://ann.com.vn</div>");
                    invoiceHtml.AppendLine("                </div>");
                    #endregion

                    invoiceHtml.AppendLine("            </div>");
                    #endregion

                    invoiceHtml.AppendLine("        </div>");
                }
                #endregion

                // Khởi tạo Body
                invoiceHtml.AppendLine(orderHtml.ToString());

                #region Khởi tạo Footer
                if (oldOrders.Count < 4)
                {
                    var config = ConfigController.GetByTop1();
                    var rule = String.Empty;

                    if (order.OrderType == 2)
                        rule = config.ChangeGoodsRule;
                    else
                        rule = config.RetailReturnRule;

                    invoiceHtml.AppendLine("        <div class='footer'>");
                    invoiceHtml.AppendLine("            <h3>CẢM ƠN QUÝ KHÁCH! HẸN GẶP LẠI !</h3>");
                    invoiceHtml.AppendLine("        </div> ");
                    invoiceHtml.AppendLine("        <div class='footer'>" + rule +"</div> ");
                }
                else
                {
                    invoiceHtml.AppendLine("        <div class='footer'>");
                    invoiceHtml.AppendLine("            <p>ANN rất vui khi quý khách đã mua " + oldOrders.Count + " đơn hàng!</p>");
                    invoiceHtml.AppendLine("        </div>");
                }
                #endregion

                invoiceHtml.AppendLine("    </div>");
                invoiceHtml.AppendLine("</div>");

                if (!String.IsNullOrEmpty(error))
                {
                    ltrPrintInvoice.Text = "Xảy ra lỗi: " + error;
                    ltrPrintEnable.Text = "";
                }
                else
                {
                    ltrPrintInvoice.Text = invoiceHtml.ToString();
                    ltrPrintEnable.Text = "<div class='print-enable true'></div>";
                }
            }
            #endregion
        }
    }
}