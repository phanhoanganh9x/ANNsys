using IM_PJ.Controllers;
using IM_PJ.Models;
using CommonModel = IM_PJ.Models.Common;
using IM_PJ.Models.Pages.print_order_image_english;
using IM_PJ.Utils;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class print_order_image_english : System.Web.UI.Page
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
        /// <summary>
        /// Chuyển đổi tiền tệ
        /// </summary>
        /// <param name="money">Số tiền cần chuyển</param>
        /// <param name="currencyRate">Tỉ giá tiền tệ</param>
        /// <returns></returns>
        private decimal _currencyConversion(decimal money, decimal currencyRate)
        {
            return Math.Ceiling((decimal)1e2 * money / currencyRate) * (decimal)1e-2;
        }

        /// <summary>
        /// Lấy dữ liệu chi tiết hóa đơn
        ///
        /// Date:   2021-07-19
        /// Author: Binh-TT
        ///
        /// Đối ứng chiết khấu từng dòng
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="merger">false: Ảnh chi tiết | true: Ảnh gộp</param>
        /// <param name="currencyRate">Tỉ giá tiền</param>
        /// <param name="defaultDiscount">Sử dụng cho các đơn hàng có 1 chiết khấu</param>
        private IList<IOrderDetailModel> _getOrderDetails(int orderId, bool merger, decimal currencyRate, decimal defaultDiscount)
        {
            try
            {
                using (var con = new inventorymanagementEntities())
                {
                    IList<IOrderDetailModel> results;

                    if (!merger)
                        results = new List<OrderDetailModel>() as IList<IOrderDetailModel>;
                    else
                        results = new List<OrderMergerDetailModel>() as IList<IOrderDetailModel>;

                    #region Lấy thông tin chi tiếc đơn hàng
                    var orderDetails = con.tbl_OrderDetail
                        .Where(o => o.OrderID == orderId)
                        .Select(x => new
                        {
                            productType = x.ProductType.HasValue ? x.ProductType.Value : 0,
                            sku = x.SKU,
                            productDescription = x.ProductVariableDescrition,
                            quantity = x.Quantity.HasValue ? x.Quantity.Value : 0,
                            price = x.Price.HasValue ? x.Price.Value : 0,
                            discount = x.DiscountPrice.HasValue ? x.DiscountPrice.Value : 0
                        })
                        .OrderBy(o => o.sku)
                        .ThenByDescending(o => o.price);

                    if (!orderDetails.Any())
                        return results;
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
                                avatar = p.ProductImage,
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
                                avatar = String.IsNullOrEmpty(v.Image) ? p.ProductImage : v.Image,
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
                        .Select(x =>
                        {
                            var categoryId = 0;
                            var sku = String.Empty;
                            var name = String.Empty;
                            var avatar = String.Empty;
                            var oldPrice = 0D;

                            if (x.detail.productType == 1)
                            {
                                categoryId = x.product.categoryId;
                                sku = x.product.sku;
                                name = x.product.name;
                                avatar = x.product.avatar;
                                oldPrice = x.product.oldPrice;
                            }
                            else
                            {
                                categoryId = x.variation.categoryId;
                                sku = x.variation.sku;
                                name = x.variation.name;
                                avatar = x.variation.avatar;
                                oldPrice = x.variation.oldPrice;
                            }

                            return new
                            {
                                categoryId = categoryId,
                                productType = x.detail.productType,
                                sku = sku,
                                name = name,
                                avatar = avatar,
                                description = x.detail.productDescription,
                                quantity = Convert.ToInt32(x.detail.quantity),
                                price = Convert.ToDecimal(x.detail.price),
                                oldPrice = Convert.ToDecimal(oldPrice),
                                discount = x.detail.discount > 0 ? Convert.ToDecimal(x.detail.discount) : defaultDiscount,
                            };
                        })
                        .ToList();
                    #endregion

                    if (!merger)
                        results = data.Select(x => new OrderDetailModel
                        {
                            avatar = x.avatar,
                            productType = x.productType,
                            sku = x.sku,
                            name = x.name,
                            description = x.description,
                            quantity = x.quantity,
                            price = x.price,
                            oldPrice = x.oldPrice,
                            discount = x.discount
                        })
                        .ToList<IOrderDetailModel>();
                    else
                    {
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

                        results = mergerData.Select(x => new OrderMergerDetailModel
                        {
                            name = x.categoryName,
                            price = x.price,
                            totalQuantity = x.totalQuantity,
                            totalDiscount = x.totalDiscount,
                            total = x.total
                        })
                        .ToList<IOrderDetailModel>();
                    }

                    return results;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Khởi tạo html hóa đơn
        ///
        /// Date:   2021-07-19
        /// Author: Binh-TT
        ///
        /// Đối ứng chiết khấu từng dòng
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pagination"></param>
        private string _createOrderHtml(OrderModel data, PaginationModel pagination)
        {
            var html = new StringBuilder();

            html.AppendLine("<div class='print-order-image'>");
            html.AppendLine(String.Format("    <div class='all print print-{0}'>", pagination.page - 1));
            html.AppendLine("        <div class='body'>");

            #region Header
            if (pagination.page == 1)
            {
                html.AppendLine("            <div class='table-1'>");

                #region Title đơn hàng
                html.AppendLine(String.Format("                <h1>INVOICE #{0}</h1>", data.id));
                html.AppendLine("                <div class='note'></div>");
                #endregion

                #region Thông tin khách hàng - nhân viên - đơn hàng
                html.AppendLine("                <table>");
                html.AppendLine("                    <colgroup >");
                html.AppendLine("                        <col class='col-left' />");
                html.AppendLine("                        <col class='col-right' />");
                html.AppendLine("                    </colgroup>");
                html.AppendLine("                    <tbody>");

                #region Tên khách hàng
                html.AppendLine("                        <tr>");
                html.AppendLine("                            <td>Customer name</td>");
                html.AppendLine(String.Format("                            <td class='customer-name'>{0}</td>", data.customer.name));
                html.AppendLine("                        </tr>");
                #endregion

                #region SĐT
                html.AppendLine("                        <tr>");
                html.AppendLine("                            <td>Phone number</td>");
                html.AppendLine(String.Format("                            <td>{0}</td>", data.customer.phone));
                html.AppendLine("                        </tr>");
                #endregion

                #region Ngày tạo đơn
                html.AppendLine("                        <tr>");
                html.AppendLine("                            <td>Invoice date</td>");
                html.AppendLine(String.Format("                            <td>{0:dd/MM/yyyy HH:mm}</td>", data.createdDate));
                html.AppendLine("                        </tr>");
                #endregion

                #region Nhân viên tạo đơn
                html.AppendLine("                        <tr>");
                html.AppendLine("                            <td>Staff</td>");
                html.AppendLine(String.Format("                            <td>{0}</td>", data.staff.name));
                html.AppendLine("                        </tr>");
                #endregion

                #region Ghi chú
                if (!String.IsNullOrEmpty(data.note))
                {
                    html.AppendLine("                        <tr>");
                    html.AppendLine("                            <td>Note</td>");
                    html.AppendLine(String.Format("                            <td>{0}</td>", data.note));
                    html.AppendLine("                        </tr>");
                }
                #endregion

                html.AppendLine("                </table>");
                #endregion

                html.AppendLine("            </div>");
            }
            #endregion

            #region Body
            html.AppendLine("            <div class='table-2'>");
            html.AppendLine("                <table>");
            html.AppendLine("                    <colgroup>");
            if (!data.merger)
            {
                html.AppendLine("                        <col class='order-item' />");
                html.AppendLine("                        <col class='image' />");
                html.AppendLine("                        <col class='name' />");
                html.AppendLine("                        <col class='quantity' />");
                html.AppendLine("                        <col class='price' />");
                html.AppendLine("                        <col class='discount' />");
                html.AppendLine("                        <col class='subtotal' />");
            }
            else
            {
                html.AppendLine("                        <col class='merger-index' />");
                html.AppendLine("                        <col class='merger-name' />");
                html.AppendLine("                        <col class='merger-total-quantity' />");
                html.AppendLine("                        <col class='merger-total-price' />");
                html.AppendLine("                        <col class='merger-total-discount' />");
                html.AppendLine("                        <col class='merger-total' />");
            }
            html.AppendLine("                    </colgroup>");
            html.AppendLine("                    <thead>");
            html.AppendLine("                        <th>#</th>");
            if (!data.merger)
                html.AppendLine("                        <th>Image</th>");
            html.AppendLine("                        <th>Item</th>");
            html.AppendLine("                        <th>Qty</th>");
            html.AppendLine("                        <th>Price</th>");
            html.AppendLine("                        <th>Discount</th>");
            html.AppendLine("                        <th>Amount</th>");
            html.AppendLine("                    </thead>");
            html.AppendLine("                    <tbody>");

            #region Thông tin chi tiết đơn hàng
            var index = ((pagination.page - 1) * pagination.pageSize) + 1;

            if (!data.merger)
            {
                var details = data.details
                    .Skip((pagination.page - 1) * pagination.pageSize)
                    .Take(pagination.pageSize)
                    .Select(x => (OrderDetailModel)x)
                    .ToList();

                foreach (var item in details)
                {
                    var avatarUrl = Thumbnail.getURL(item.avatar, Thumbnail.Size.Large);
                    var saleHtml = item.oldPrice > 0 ? "<span class='sale-icon'>SALE</span> " : String.Empty;
                    var name = PJUtils.Truncate(item.name, 30);
                    var description = item.productType == 2 && !String.IsNullOrEmpty(item.description)
                        ? String.Format("{0}", item.description.Replace("|", ". "))
                        : String.Empty;
                    var productName = String.Format("<strong>{0}</strong> - {1}{2}", item.sku, saleHtml, name);
                    var total = (item.price - item.discount) * item.quantity;

                    html.AppendLine("                    <tr>");
                    // STT
                    html.AppendLine(String.Format("                        <td id='{0}'>{1}</td>", item.sku, index));
                    // Hình ảnh
                    if (!data.merger)
                        html.AppendLine(String.Format("                        <td><image src='{0}' /></td>", avatarUrl));
                    // Sản phẩm
                    if (item.productType == 1)
                        html.AppendLine(String.Format("                        <td>{0}</td>", productName));
                    else
                        html.AppendLine(String.Format("                        <td><p>{0}</p><p class='variable'>{1}</p></td>", productName, description));
                    // Số lượng
                    html.AppendLine(String.Format("                        <td>{0:N0}</td>", item.quantity));
                    // Giá
                    html.AppendLine(String.Format("                        <td>{0:0.00}</td>", _currencyConversion(item.price, data.currencyRate)));
                    // Chiết khấu
                    html.AppendLine(String.Format("                        <td>{0:0.00}</td>", _currencyConversion(item.discount, data.currencyRate)));
                    // html
                    html.AppendLine(String.Format("                        <td>{0:0.00}</td>", _currencyConversion(total, data.currencyRate)));
                    html.AppendLine("                    </tr>");

                    index++;
                }
            }
            else
            {
                var details = data.details
                    .Skip((pagination.page - 1) * pagination.pageSize)
                    .Take(pagination.pageSize)
                    .Select(x => (OrderMergerDetailModel)x)
                    .ToList();

                foreach (var item in details)
                {
                    html.AppendLine("                    <tr>");
                    // STT
                    html.AppendLine(String.Format("                        <td>{0}</td>", index));
                    html.AppendLine(String.Format("                        <td>{0} {1:0.00}</td>", item.name, _currencyConversion(item.price, data.currencyRate)));
                    html.AppendLine(String.Format("                        <td>{0:N0}</td>", item.totalQuantity));
                    html.AppendLine(String.Format("                        <td>{0:0.00}</td>", _currencyConversion(item.price * item.totalQuantity, data.currencyRate)));
                    html.AppendLine(String.Format("                        <td>{0:0.00}</td>", _currencyConversion(item.totalDiscount, data.currencyRate)));
                    html.AppendLine(String.Format("                        <td>{0:0.00}</td>", _currencyConversion(item.total, data.currencyRate)));
                    html.AppendLine("                    </tr>");

                    index++;
                }
            }
            #endregion

            #region Thông tin tổng hợp của đơn hàng
            if (pagination.page == pagination.totalPages)
            {
                var total = 0m;
                var colspan = 6;

                if (data.merger)
                    colspan = 5;

                #region Tổng số lượng
                html.AppendLine("                    <tr>");
                html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>Total quantity</td>", colspan));
                html.AppendLine(String.Format("                        <td>{0:N0}</td>", data.totalQuantity));
                html.AppendLine("                    </tr>");
                #endregion

                #region Thành tiền
                total += data.totalPrice;

                html.AppendLine("                    <tr>");
                html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>Subtotal</td>", colspan));
                html.AppendLine(String.Format("                        <td>{0:0.00}</td>", _currencyConversion(data.totalPrice, data.currencyRate)));
                html.AppendLine("                    </tr>");
                #endregion

                #region Chiết khấu
                total -= data.totalDiscount;

                html.AppendLine("                    <tr>");
                html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>Total discount</td>", colspan));
                html.AppendLine(String.Format("                        <td>-{0:0.00}</td>", _currencyConversion(data.totalDiscount, data.currencyRate)));
                html.AppendLine("                    </tr>");
                html.AppendLine("                    <tr>");
                html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>After discount</td>", colspan));
                html.AppendLine(String.Format("                        <td>{0:0.00}</td>", _currencyConversion(total, data.currencyRate)));
                html.AppendLine("                    </tr>");
                #endregion

                #region Đơn hàng đổi trả
                if (data.refundOrder != null)
                {
                    total -= data.refundOrder.totalPrice;

                    html.AppendLine("                    <tr>");
                    html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>Return order (ID {1})</td>", colspan, data.refundOrder.id));
                    html.AppendLine(String.Format("                        <td>-{0:0.00}</td>", _currencyConversion(data.refundOrder.totalPrice, data.currencyRate)));
                    html.AppendLine("                    </tr>");
                    html.AppendLine("                    <tr>");
                    html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>Total remaining</td>", colspan));
                    html.AppendLine(String.Format("                        <td>{0:0.00}</td>", _currencyConversion(total, data.currencyRate)));
                    html.AppendLine("                    </tr>");
                }
                #endregion

                #region Phí giao hàng
                total += data.shippingFee;

                html.AppendLine("                    <tr>");
                html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>Shipping</td>", colspan));
                html.AppendLine(String.Format("                        <td>{0:0.00}</td>", _currencyConversion(data.shippingFee, data.currencyRate)));
                html.AppendLine("                    </tr>");
                #endregion

                #region Phí khác
                foreach (var otherFee in data.otherFees)
                {
                    total += otherFee.fee;

                    html.AppendLine("                    <tr>");
                    html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>{1}</td>", colspan, otherFee.name));
                    html.AppendLine(String.Format("                        <td>{0:0.00}</td>", _currencyConversion(otherFee.fee, data.currencyRate)));
                    html.AppendLine("                    </tr>");
                }
                #endregion

                #region Coupon
                if (data.coupon != null)
                {
                    total -= data.coupon.value;

                    html.AppendLine("                    <tr>");
                    html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>Coupon code: {1}</td>", colspan, data.coupon.code));
                    html.AppendLine(String.Format("                        <td>-{0:0.00}</td>", _currencyConversion(data.coupon.value, data.currencyRate)));
                    html.AppendLine("                    </tr>");
                }
                #endregion

                #region Tổng cộng
                html.AppendLine("                    <tr>");
                html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>TOTAL</td>", colspan));
                html.AppendLine(String.Format("                        <td>{0:0.00}</td>", _currencyConversion(total, data.currencyRate)));
                html.AppendLine("                    </tr>");
                #endregion
            }
            #endregion

            html.AppendLine("                    </tbody>");
            html.AppendLine("                </table>");
            html.AppendLine("            </div>");
            #endregion

            html.AppendLine("        </div>");
            html.AppendLine("    </div>");
            html.AppendLine("</div>");

            return html.ToString();
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
            var currencyCode = CommonModel.Currency.USD;

            #region Lấy thông tin đơn hàng
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                orderId = Request.QueryString["id"].ToInt(0);

            if (orderId <= 0)
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

            #region Thông tin tiền tệ
            if (!String.IsNullOrEmpty(Request.QueryString["currencyCode"]))
                currencyCode = Request.QueryString["currencyCode"].Trim();

            var currency = CurrencyController.getByCode(currencyCode);

            if (currency == null)
            {
                ltrPrintInvoice.Text = String.Format("Không tìm thấy tỉ giá tiền tệ của mã {0} này.", currencyCode);
                return;
            }
            #endregion

            if (!String.IsNullOrEmpty(Request.QueryString["merge"]))
                merger = Request.QueryString["merge"].ToInt(0);
            #endregion

            #region Kiểm tra thông tin đơn hàng
            var isOrderDetailEmpty = OrderDetailController.isOrderDetailEmpty(orderId);

            if (isOrderDetailEmpty)
            {
                ltrPrintInvoice.Text = "Đơn hàng đang rỗng!";
                return;
            }
            #endregion

            #region Lấy dữ liệu cho việc tạo hình ảnh order
            var data = new OrderModel();

            // Merger
            data.merger = merger == 1;
            // ID đơn hàng
            data.id = order.ID;
            #region Nhân viên khởi tạo đơn hàng
            data.staff = new StaffModel() { name = order.CreatedBy };

            var staffAccount = AccountController.GetByUsername(order.CreatedBy);

            if (staffAccount != null)
            {
                var staffInfo = AccountInfoController.GetByUserID(staffAccount.ID);

                if (staffInfo != null && !String.IsNullOrEmpty(staffInfo.Phone))
                    data.staff.phone = staffInfo.Phone;
            }
            #endregion
            // Thông tin khách hàng
            data.customer = new CustomerModel()
            {
                id = Convert.ToInt32(order.CustomerID),
                name = order.CustomerName,
                phone = order.CustomerPhone
            };
            // Ngày khởi tạo đơn hàng
            data.createdDate = order.CreatedDate.Value;
            // Chú thích
            data.note = order.OrderNote;
            // Tổng số lượng
            data.totalQuantity = order.TotalQuantity;
            // Tỷ giá tiền tệ
            data.currencyRate = currency.SellingRate;
            // Tổng giá trị đơn hàng
            data.totalPrice = Convert.ToDecimal(order.TotalPriceNotDiscount);
            // chiết khấu mặc định
            data.defaultDiscount = Convert.ToDecimal(order.DiscountPerProduct);
            // Tổng chiết khấu
            data.totalDiscount = Convert.ToDecimal(order.TotalDiscount);
            // Phí giao hàng
            data.shippingFee = Convert.ToDecimal(order.FeeShipping);
            #region Lấy thông tin phí khác
            data.otherFees = new List<OtherFeeModel>();

            var otherFees = FeeController.getFeeInfo(data.id);

            foreach (var item in otherFees)
            {
                var otherFee = new OtherFeeModel()
                {
                    name = item.Name,
                    fee = item.Price
                };

                data.otherFees.Add(otherFee);
            }
            #endregion
            #region Lấy thông tin giảm giá
            if (order.CouponID.HasValue)
            {
                var coupon = CouponController.getCoupon(order.CouponID.Value);

                if (coupon != null)
                    data.coupon = new CouponModel()
                    {
                        code = coupon.Code,
                        value = coupon.Value
                    };
            }
            #endregion
            #region Đơn hàn đổi trả
            if (order.RefundsGoodsID.HasValue)
            {
                var refundOrder = RefundGoodController.GetByID(order.RefundsGoodsID.Value);

                if (refundOrder != null)
                    data.refundOrder = new RefundOrderModel()
                    {
                        id = refundOrder.ID,
                        totalPrice = Convert.ToDecimal(refundOrder.TotalPrice)
                    };
            }
            #endregion
            // Lấy danh sách các đơn hàng đã mua (Bao gồm đơn hiện tại)
            data.oldOrderNumber = OrderController.GetByCustomerID(data.customer.id).Count;
            // Danh sách chi tiết đơn hàng
            data.details = _getOrderDetails(data.id, data.merger, data.currencyRate, data.defaultDiscount);
            #endregion

            #region Kiểm tra các điều kiện của đơn hàng
            var error = String.Empty;

            #region Kiểm tra đơn đổi trả
            if (order.RefundsGoodsID.HasValue && data.refundOrder == null)
                error += String.Format("Không tìm thấy đơn hàng đổi trả #{0}", order.RefundsGoodsID);
            #endregion

            #region Kiểm tra tổng tiền có đúng không
            var total = data.totalPrice - data.totalDiscount + data.shippingFee;

            if (data.otherFees.Any())
                total += data.otherFees.Sum(s => s.fee);

            if (data.coupon != null)
                total -= data.coupon.value;

            if (total != Convert.ToDecimal(order.TotalPrice))
                error += "Đơn hàng tính sai tổng tiền";
            #endregion

            if (!String.IsNullOrEmpty(error))
            {
                ltrPrintInvoice.Text = "Xảy ra lỗi: " + error;
                return;
            }
            #endregion

            #region Header
            #region Lấy ghi chú đơn hàng cũ
            var recentNotes = OrderController.GetAllNoteByCustomerID(data.customer.id, data.id);

            if (recentNotes.Any())
            {
                var noteHtml = new StringBuilder();
                noteHtml.AppendLine("<div id='old-order-note'>");
                noteHtml.AppendLine(String.Format("    <h2>{0:N0} đơn hàng cũ gần nhất có ghi chú:</h2>", recentNotes.Count()));
                noteHtml.AppendLine("    <ul>");
                foreach (var item in recentNotes)
                    noteHtml.AppendLine(String.Format("        <li>Đơn <strong><a href='/thong-tin-don-hang?id={0}' target='_blank'>{0}</a></strong> (<em>{1}<em>): {2}</li>", item.ID, item.DateDone, item.OrderNote));
                noteHtml.AppendLine("    </ul>");
                noteHtml.AppendLine("</div>");

                ltrOldOrderNote.Text = noteHtml.ToString();
            }
            #endregion

            #region Button copy link hóa đơn
            var btnCopyHtml = new StringBuilder();

            btnCopyHtml.AppendLine("<a href='javascript:;'");
            btnCopyHtml.AppendLine("   class='btn btn-violet h45-btn'");
            btnCopyHtml.AppendLine(String.Format("   onclick='copyInvoiceURLEnglish({0}, {1})'", data.id, data.customer.id));
            btnCopyHtml.AppendLine("   title='Copy link hóa đơn'");
            btnCopyHtml.AppendLine(">");
            btnCopyHtml.AppendLine("    Copy link hóa đơn");
            btnCopyHtml.AppendLine("</a>");

            ltrCopyInvoiceURL.Text = btnCopyHtml.ToString();
            #endregion

            #region Currencies Drop Down List
            var ddlLanguageHtml = new StringBuilder();

            ddlLanguageHtml.AppendLine("<select class='currency-select' id='dllCurrency' onchange='onChangeCurrency($(this).val())'>");
            ddlLanguageHtml.AppendLine("    <option value='VND'>VND - Việt Nam</option>");
            ddlLanguageHtml.AppendLine("    <option value='USD'>USD - Mỹ</option>");
            ddlLanguageHtml.AppendLine("    <option value='AUD'>AUD - Úc</option>");
            ddlLanguageHtml.AppendLine("    <option value='JPY'>JPY - Nhật</option>");
            ddlLanguageHtml.AppendLine("    <option value='SGD'>SGD - Singapore</option>");
            ddlLanguageHtml.AppendLine("    <option value='MYR'>MYR - Malaysia</option>");
            ddlLanguageHtml.AppendLine("    <option value='TWD'>TWD - Đài Loan</option>");
            ddlLanguageHtml.AppendLine("</select>");

            ltrEnglishInvoice.Text = ddlLanguageHtml.ToString();
            #endregion
            #endregion

            #region Body
            var invoiceHtml = new StringBuilder();
            var pagination = new PaginationModel()
            {
                totalCount = data.details.Count,
                page = 1,
                pageSize = data.merger ? 20 : 10
            };

            // Tổng số trang
            pagination.totalPages = (int)Math.Ceiling((decimal)pagination.totalCount / (decimal)pagination.pageSize);

            for (int i = 0; i < pagination.totalPages; i++)
            {
                var orderHtml = String.Empty;

                pagination.page = i + 1;
                orderHtml = _createOrderHtml(data, pagination);
                invoiceHtml.AppendLine(orderHtml);
            }

            ltrPrintInvoice.Text = invoiceHtml.ToString();
            #endregion
        }
    }
}