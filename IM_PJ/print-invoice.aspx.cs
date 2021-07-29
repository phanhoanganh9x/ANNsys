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

        #region Private
        /// <summary>
        /// Khởi tạo HTML chi tiết đơn hàng
        ///
        /// Date:   2021-07-19
        /// Author: Binh-TT
        ///
        /// Đối ứng chiết khấu từng dòng
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="merge">0: In chi tiết | 1: In gộp</param>
        /// <param name="defaultDiscount">Sử dụng cho các đơn hàng có 1 chiết khấu</param>
        private string _createOrderDetailHtml(int orderId, int merge, double defaultDiscount)
        {
            try
            {
                using (var con = new inventorymanagementEntities())
                {
                    #region Lấy thông tin chi tiếc đơn hàng
                    var orderDetails = con.tbl_OrderDetail
                        .Where(o => o.OrderID == orderId)
                        .Select(x => new
                        {
                            productType = x.ProductType.HasValue ? x.ProductType.Value : 0,
                            sku = x.SKU,
                            quantity = x.Quantity.HasValue ? x.Quantity.Value : 0,
                            price = x.Price.HasValue ? x.Price.Value : 0,
                            discount = x.DiscountPrice.HasValue ? x.DiscountPrice.Value : 0
                        })
                        .OrderBy(o => o.sku)
                        .ThenByDescending(o => o.price);

                    if (!orderDetails.Any())
                        return String.Empty;
                    #endregion

                    #region Lấy thông tin tên sản phẩm và giá củ (đối với sản phẩm sale)
                    var productSkus = orderDetails
                        .Where(x => x.productType == 1)
                        .GroupBy(x => x.sku)
                        .Select(x => new { sku = x.Key });

                    var products = con.tbl_Product
                        .Join(
                            productSkus,
                            p => p.ProductSKU,
                            f => f.sku,
                            (p, f) => new
                            {
                                categoryId = p.CategoryID.HasValue ? p.CategoryID.Value : 0,
                                sku = p.ProductSKU,
                                name = p.ProductTitle,
                                oldPrice = p.Old_Price.HasValue ? p.Old_Price.Value : 0
                            }
                        )
                        .OrderBy(o => o.sku)
                        .ToList();

                    var variationSkus = orderDetails
                        .Where(x => x.productType == 2)
                        .GroupBy(x => x.sku)
                        .Select(x => new { sku = x.Key });

                    var variations = con.tbl_Product
                        .Join(
                            con.tbl_ProductVariable.Where(x => variationSkus.Any(y => x.SKU == y.sku)),
                            p => p.ID,
                            v => v.ProductID,
                            (p, v) => new
                            {
                                categoryId = p.CategoryID.HasValue ? p.CategoryID.Value : 0,
                                sku = v.SKU,
                                name = p.ProductTitle,
                                oldPrice = p.Old_Price.HasValue ? p.Old_Price.Value : 0
                            }
                        )
                        .ToList();
                    #endregion

                    #region Tổng hợp dữ liệu
                    var data = orderDetails.ToList()
                        .GroupJoin(
                            products,
                            d => new { d.productType, d.sku },
                            p => new { productType = 1, p.sku },
                            (detail, product) => new { detail, product }
                        )
                        .SelectMany(
                            x => x.product.DefaultIfEmpty(),
                            (parent, child) => new { parent.detail, product = child }
                        )
                        .GroupJoin(
                            variations,
                            temp => new { temp.detail.productType, temp.detail.sku },
                            v => new { productType = 2, v.sku },
                            (temp, variation) => new { temp.detail, temp.product, variation }
                        )
                        .SelectMany(
                            x => x.variation.DefaultIfEmpty(),
                            (parent, child) => new { parent.detail, parent.product, variation = child }
                        )
                        .Select(x => {
                            var categoryId = 0;
                            var sku = String.Empty;
                            var name = String.Empty;
                            var oldPrice = 0D;

                            if (x.detail.productType == 1)
                            {
                                categoryId = x.product.categoryId;
                                sku = x.product.sku;
                                name = x.product.name;
                                oldPrice = x.product.oldPrice;
                            }
                            else
                            {
                                categoryId = x.variation.categoryId;
                                sku = x.variation.sku;
                                name = x.variation.name;
                                oldPrice = x.variation.oldPrice;
                            }

                            return new
                            {
                                categoryId = categoryId,
                                productType = x.detail.productType,
                                sku = sku,
                                name = name,
                                oldPrice = oldPrice,
                                quantity = x.detail.quantity,
                                price = x.detail.price,
                                discount = x.detail.discount > 0 ? x.detail.discount : defaultDiscount,
                            };
                        })
                        .ToList();
                    #endregion

                    var index = 0;
                    var bodyHtml = new StringBuilder();

                    if (merge == 0)
                    {
                        #region In hóa đơn chi tiết
                        foreach (var item in data)
                        {
                            index++;

                            #region Dòng 1
                            var saleHtml = item.oldPrice > 0 ? "<span class='sale-icon'>SALE</span> " : String.Empty;
                            var name = PJUtils.Truncate(item.name, item.productType == 1 ? 28 : 37);
                            var total = (item.price - item.discount) * item.quantity;

                            bodyHtml.AppendLine("<tr>");
                            bodyHtml.AppendLine("    <td colspan='3'>");
                            bodyHtml.AppendLine(String.Format("        {0}&ensp;<strong>{1}</strong> - {2}{3}", index, item.sku, saleHtml, name));
                            bodyHtml.AppendLine("    </td>");
                            bodyHtml.AppendLine("</tr>");
                            #endregion

                            #region Dòng 2
                            bodyHtml.AppendLine("<tr>");
                            // SL
                            bodyHtml.AppendLine("    <td>" + item.quantity + "</td>");
                            // Giá
                            bodyHtml.AppendLine("    <td>" + String.Format("{0:N0}", item.price) + "</td>");
                            // Chiết khấu
                            bodyHtml.AppendLine("    <td>" + String.Format("{0:N0}", item.discount) + "</td>");
                            // Tổng
                            bodyHtml.AppendLine("    <td>" + String.Format("{0:N0}", total) + "</td>");
                            bodyHtml.AppendLine("</tr>");
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region In hóa đơn gộp
                        #region Lấy thông tin danh mục
                        var categoryIds = data.GroupBy(g => g.categoryId).Select(x => x.Key).ToList();
                        var categories = con.tbl_Category
                            .Where(x => categoryIds.Contains(x.ID))
                            .Select(x => new
                            {
                                id = x.ID,
                                name = x.CategoryName
                            })
                            .ToList();
                        #endregion

                        #region Tổng hợp dữ liệu gộp
                        var mergerData = data
                            .GroupBy(g => new { g.categoryId, g.price })
                            .Select(x => new {
                                categoryId = x.Key.categoryId,
                                price = x.Key.price,
                                totalQuantity = x.Sum(s => s.quantity),
                                totalDiscount = x.Sum(s => s.discount * s.quantity),
                                total = x.Sum(s => (s.price - s.discount) * s.quantity)
                            })
                            .GroupJoin(
                                categories,
                                d => d.categoryId,
                                c => c.id,
                                (details, category) => new { details, category }
                            )
                            .SelectMany(
                                x => x.category.DefaultIfEmpty(),
                                (parent, child) => new { parent.details, category = child }
                            )
                            .Select(x => new
                            {
                                categoryName = x.category.name,
                                price = x.details.price,
                                totalQuantity = x.details.totalQuantity,
                                totalDiscount = x.details.totalDiscount,
                                total = x.details.total
                            })
                            .ToList();
                        #endregion

                        index++;

                        foreach (var item in mergerData)
                        {
                            #region Dòng 1
                            bodyHtml.AppendLine("<tr>");
                            bodyHtml.AppendLine("    <td colspan='3'>");
                            bodyHtml.AppendLine(String.Format("        {0}&ensp;<strong>{1} đồng giá {2:N0}</strong>", index, item.categoryName, item.price));
                            bodyHtml.AppendLine("    </td>");
                            bodyHtml.AppendLine("</tr>");
                            #endregion

                            #region Dòng 2
                            bodyHtml.AppendLine("<tr>");
                            // Số lượng
                            bodyHtml.AppendLine("    <td>" + item.totalQuantity + "</td>");
                            // Giá
                            bodyHtml.AppendLine("    <td>" + String.Format("{0:N0}", (item.price * item.totalQuantity)) + "</td>");
                            // Chiết khấu
                            bodyHtml.AppendLine("    <td>" + String.Format("{0:N0}", item.totalDiscount) + "</td>");
                            // Tổng
                            bodyHtml.AppendLine("    <td>" + String.Format("{0:N0}", item.total) + "</td>");
                            bodyHtml.AppendLine("</tr>");
                            #endregion
                        }
                        #endregion
                    }

                    #region Khởi tạo HTML chi tiết đơn hàng
                    var orderDetailHtml = new StringBuilder();

                    orderDetailHtml.AppendLine("<div class='table-2'>");
                    orderDetailHtml.AppendLine("    <table>");
                    orderDetailHtml.AppendLine("        <colgroup>");
                    orderDetailHtml.AppendLine("            <col class='soluong' />");
                    orderDetailHtml.AppendLine("            <col class='gia' />");
                    orderDetailHtml.AppendLine("            <col class='chiet-khau' />");
                    orderDetailHtml.AppendLine("            <col class='tong' />");
                    orderDetailHtml.AppendLine("        </colgroup>");
                    orderDetailHtml.AppendLine("        <thead>");
                    orderDetailHtml.AppendLine("            <th>Số lượng</th>");
                    orderDetailHtml.AppendLine("            <th>Giá</th>");
                    orderDetailHtml.AppendLine("            <th>Chiết khấu</th>");
                    orderDetailHtml.AppendLine("            <th>Tổng</th>");
                    orderDetailHtml.AppendLine("        </thead>");
                    orderDetailHtml.AppendLine("        <tbody>");
                    orderDetailHtml.AppendLine(bodyHtml.ToString());
                    orderDetailHtml.AppendLine("        </tbody>");
                    orderDetailHtml.AppendLine("    </table>");
                    orderDetailHtml.AppendLine("</div>");
                    #endregion

                    return orderDetailHtml.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        /// <summary>
        /// Date:   2021-07-19
        /// Author: Binh-TT
        ///
        /// Đối ứng chiết khấu từng dòng
        /// </summary>
        public void LoadData()
        {
            #region Lấy thông tin query params
            var orderId = 0;
            var merger = 0;

            #region Check ID đơn hàng
            orderId = Request.QueryString["id"].ToInt(0);

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
            if (!String.IsNullOrEmpty(Request.QueryString["merge"]))
                merger = Request.QueryString["merge"].ToInt(0);
            #endregion
            #endregion

            #region Kiểm tra thông tin đơn hàng
            var isOrderDetailEmpty = OrderDetailController.isOrderDetailEmpty(orderId);

            if (isOrderDetailEmpty)
            {
                ltrPrintInvoice.Text = "Đơn hàng đang rỗng!";
                return;
            }
            #endregion

            var error = String.Empty;
            // Lấy thông tin khách hàng
            var customer = CustomerController.GetByID(Convert.ToInt32(order.CustomerID));

            #region Tạo HTML thông tin đơn hàng
            var orderHtml = new StringBuilder();

            orderHtml.AppendLine("<div class='body'>");

            #region Tiêu đề và thông tin khách hàng
            orderHtml.AppendLine("    <div class='table-1'>");

            #region Tiêu đề
            if (merger == 1)
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
            if (!String.IsNullOrEmpty(order.OrderNote))
            {
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

            // Thông tin chi tiết đơn hàng
            var defaultDiscount = order.DiscountPerProduct.HasValue ? order.DiscountPerProduct.Value : 0;
            orderHtml.AppendLine(_createOrderDetailHtml(orderId, merger, defaultDiscount));

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

                if (refund != null)
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
            var total = totalPrice - totalDiscount + shoppingFee + totalOtherFee - couponValue;

            if (total != Convert.ToDouble(order.TotalPrice))
                error += "Đơn hàng tính sai tổng tiền";

            orderHtml.AppendLine("                <tr>");
            orderHtml.AppendLine("                    <td class='strong' colspan='2'>TỔNG TIỀN</td>");
            orderHtml.AppendLine("                    <td class='strong'>" + String.Format("{0:N0}", total - totalRefunds) + "&nbsp;</td>");
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
            // Lấy danh sách các đơn hàng đã mua (Bao gồm đơn hiện tại)
            var oldOrders = OrderController.GetByCustomerID(Convert.ToInt32(order.CustomerID));

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
                invoiceHtml.AppendLine("        <div class='footer'>" + rule + "</div> ");
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
            #endregion

            if (!String.IsNullOrEmpty(error))
            {
                ltrPrintInvoice.Text = "Xảy ra lỗi: " + error;
                ltrPrintEnable.Text = String.Empty;
            }
            else
            {
                ltrPrintInvoice.Text = invoiceHtml.ToString();
                ltrPrintEnable.Text = "<div class='print-enable true'></div>";
            }
        }
    }
}