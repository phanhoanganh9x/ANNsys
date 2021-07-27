using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Models.Pages.print_order_image;
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
    public partial class print_order_image : System.Web.UI.Page
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
        /// Khởi tạo HTML chi tiết đơn hàng
        /// Trường hợp ảnh chi tiết thì cứ 10 sản phẩm sẽ tạo bảng chi tiết đơn hàng
        /// Trường hợp ảnh gộp thì cứ 20 sản phẩm sẽ tạo bảng chi tiết đơn hàng
        ///
        /// Date:   2021-07-19
        /// Author: Binh-TT
        ///
        /// Đối ứng triết khấu từng dòng
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="merger">false: Ảnh chi tiết | true: Ảnh gộp</param>
        /// <param name="defaultDiscount">Sử dụng cho các đơn hàng có 1 chiết khấu</param>
        private IList<IOrderDetailModel> _getOrderDetails(int orderId, bool merger, decimal defaultDiscount = 0)
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
                            price = Convert.ToDecimal(x.price),
                            totalQuantity = Convert.ToInt32(x.totalQuantity),
                            totalDiscount = Convert.ToDecimal(x.totalDiscount),
                            total = Convert.ToDecimal(x.total)
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

        private string _createOrderHtml(OrderModel data, PaginationModel pagination)
        {
            var html = new StringBuilder();

            html.AppendLine("<div class='print-order-image'>");
            html.AppendLine(String.Format("    <div class='all print print-{0}'>", pagination.page - 1));

            #region Logo
            if (pagination.page == 1 && data.oldOrderNumber < 4)
            {
                html.AppendLine("        <div class='head'>");
                html.AppendLine("            <div class='logo'>");
                html.AppendLine("                <img src='App_Themes/Ann/image/logo.png' />");
                html.AppendLine("            </div>");
                html.AppendLine("            <div class='ct'>");
                html.AppendLine("                <div class='ct-title'></div>");
                html.AppendLine(String.Format("                <div class='ct-detail'> {0}</div>", data.shop.address));
                html.AppendLine("            </div>");
                html.AppendLine("            <div class='ct'>");
                html.AppendLine("                <div class='ct-title'> </div>");
                html.AppendLine(String.Format("                <div class='ct-detail'> {0}</div>", !String.IsNullOrEmpty(data.staff.phone) ? data.staff.phone : data.shop.phone));
                html.AppendLine("            </div>");
                html.AppendLine("        </div>");
            }
            #endregion

            html.AppendLine("        <div class='body'>");

            #region Header
            if (pagination.page == 1)
            {
                html.AppendLine("            <div class='table-1'>");

                #region Title đơn hàng
                html.AppendLine(String.Format("                <h1>XÁC NHẬN ĐƠN HÀNG #{0}</h1>", data.id));
                html.AppendLine("                <div class='note'>");
                html.AppendLine("                    <p>- Lưu ý, hình ảnh sản phẩm có thể hiển thị không đúng màu.</p>");
                html.AppendLine("                    <p>- Quý khách vui lòng kiểm tra thuộc tính sản phẩm (Màu, Size).</p>");
                html.AppendLine("                    <p>- Nếu có sai sót, quý khách vui lòng thông báo cho nhân viên.</p>");
                html.AppendLine("                </div>");

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
                html.AppendLine("                            <td>Khách hàng</td>");
                html.AppendLine(String.Format("                            <td class='customer-name'>{0}</td>", data.customer.name));
                html.AppendLine("                        </tr>");
                #endregion

                #region SĐT
                html.AppendLine("                        <tr>");
                html.AppendLine("                            <td>Điện thoại</td>");
                html.AppendLine(String.Format("                            <td>{0}</td>", data.customer.phone));
                html.AppendLine("                        </tr>");
                #endregion

                #region Ngày tạo đơn
                html.AppendLine("                        <tr>");
                html.AppendLine("                            <td>Ngày tạo</td>");
                html.AppendLine(String.Format("                            <td>{0:dd/MM/yyyy HH:mm}</td>", data.createdDate));
                html.AppendLine("                        </tr>");
                #endregion

                #region Ngày hoàn tất đơn
                if (data.doneDate.HasValue)
                {
                    html.AppendLine("                        <tr>");
                    html.AppendLine("                            <td>Ngày tạo</td>");
                    html.AppendLine(String.Format("                            <td>{0:dd/MM/yyyy HH:mm}</td>", data.createdDate));
                    html.AppendLine("                        </tr>");
                }
                #endregion

                #region Nhân viên tạo đơn
                html.AppendLine("                        <tr>");
                html.AppendLine("                            <td>Nhân viên</td>");
                html.AppendLine(String.Format("                            <td>{0}</td>", data.staff.name));
                html.AppendLine("                        </tr>");
                #endregion

                #region Ghi chú
                if (!String.IsNullOrEmpty(data.note))
                {
                    html.AppendLine("                        <tr>");
                    html.AppendLine("                            <td>Ghi chú</td>");
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
                html.AppendLine("                        <th>Hình ảnh</th>");
            html.AppendLine("                        <th>Sản phẩm</th>");
            html.AppendLine("                        <th>SL</th>");
            html.AppendLine("                        <th>Giá</th>");
            html.AppendLine("                        <th>CK</th>");
            html.AppendLine("                        <th>Tổng</th>");
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
                    html.AppendLine(String.Format("                        <td>{0:N0}</td>", item.price));
                    // Chiết khấu
                    html.AppendLine(String.Format("                        <td>{0:N0}</td>", item.discount));
                    // html
                    html.AppendLine(String.Format("                        <td>{0:N0}</td>", total));
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
                    html.AppendLine(String.Format("                        <td>{0} {1:N0}</td>", item.name, item.price));
                    html.AppendLine(String.Format("                        <td>{0:N0}</td>", item.totalQuantity));
                    html.AppendLine(String.Format("                        <td>{0:N0}</td>", (item.price * item.totalQuantity)));
                    html.AppendLine(String.Format("                        <td>{0:N0}</td>", item.totalDiscount));
                    html.AppendLine(String.Format("                        <td>{0:N0}</td>", item.total));
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
                html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>Số lượng</td>", colspan));
                html.AppendLine(String.Format("                        <td>{0:N0}</td>", data.totalQuantity));
                html.AppendLine("                    </tr>");
                #endregion

                #region Thành tiền
                total += data.totalPrice;

                html.AppendLine("                    <tr>");
                html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>Thành tiền</td>", colspan));
                html.AppendLine(String.Format("                        <td>{0:N0}</td>", data.totalPrice));
                html.AppendLine("                    </tr>");
                #endregion

                #region Chiết khấu
                total -= data.totalDiscount;

                html.AppendLine("                    <tr>");
                html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>Trừ tổng chiết khấu</td>", colspan));
                html.AppendLine(String.Format("                        <td>-{0:N0}</td>", data.totalDiscount));
                html.AppendLine("                    </tr>");
                html.AppendLine("                    <tr>");
                html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>Sau chiết khấu</td>", colspan));
                html.AppendLine(String.Format("                        <td>{0:N0}</td>", total));
                html.AppendLine("                    </tr>");
                #endregion

                #region Đơn hàng đổi trả
                if (data.refundOrder != null)
                {
                    total -= data.refundOrder.totalPrice;

                    html.AppendLine("                    <tr>");
                    html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>Trừ hàng trả (đơn {1})</td>", colspan, data.refundOrder.id));
                    html.AppendLine(String.Format("                        <td>-{0:N0}</td>", data.refundOrder.totalPrice));
                    html.AppendLine("                    </tr>");
                    html.AppendLine("                    <tr>");
                    html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>Tổng tiền còn lại</td>", colspan));
                    html.AppendLine(String.Format("                        <td>{0:N0}</td>", total));
                    html.AppendLine("                    </tr>");
                }
                #endregion

                #region Phí giao hàng
                total += data.shippingFee;

                html.AppendLine("                    <tr>");
                html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>Phí vận chuyển</td>", colspan));
                html.AppendLine(String.Format("                        <td>{0:N0}</td>", data.shippingFee));
                html.AppendLine("                    </tr>");
                #endregion

                #region Phí khác
                foreach (var otherFee in data.otherFees)
                {
                    total += otherFee.fee;

                    html.AppendLine("                    <tr>");
                    html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>{1}</td>", colspan, otherFee.name));
                    html.AppendLine(String.Format("                        <td>{0:N0}</td>", otherFee.fee));
                    html.AppendLine("                    </tr>");
                }
                #endregion

                #region Coupon
                if (data.coupon != null)
                {
                    total -= data.coupon.value;

                    html.AppendLine("                    <tr>");
                    html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>Mã giảm giá: {1}</td>", colspan, data.coupon.code));
                    html.AppendLine(String.Format("                        <td>-{0:N0}</td>", data.coupon.value));
                    html.AppendLine("                    </tr>");
                }
                #endregion

                #region Tổng cộng
                html.AppendLine("                    <tr>");
                html.AppendLine(String.Format("                        <td colspan='{0}' class='align-right'>TỔNG CỘNG</td>", colspan));
                html.AppendLine(String.Format("                        <td>{0:N0}</td>", total));
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

            #region Kiểm tra thông tin đơn hàng
            var isOrderDetailEmpty = OrderDetailController.isOrderDetailEmpty(orderId);

            if (isOrderDetailEmpty)
            {
                ltrPrintInvoice.Text = "Đơn hàng đang rỗng!";
                return;
            }
            #endregion

            #region Lấy thông tin parameter in đơn hàng
            var merger = 0;

            if (!String.IsNullOrEmpty(Request.QueryString["merge"]))
                merger = Request.QueryString["merge"].ToInt(0);
            #endregion

            #region Lấy dữ liệu cho việc tạo hình ảnh order
            var data = new OrderModel();

            // Merger
            data.merger = merger == 1;
            // ID đơn hàng
            data.id = order.ID;
            #region Thông tin shop
            var shop = AgentController.GetByID(Convert.ToInt32(order.AgentID));

            if (shop != null)
                data.shop = new ShopModel()
                {
                    address = shop.AgentAddress,
                    phone = shop.AgentPhone
                };
            #endregion
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
            // Ngày hoàn tất đơn
            data.doneDate = order.DateDone;
            // Chú thích
            data.note = order.OrderNote;
            // Tổng số lượng
            data.totalQuantity = order.TotalQuantity;
            // Tổng giá trị đơn hàng
            data.totalPrice = Convert.ToDecimal(order.TotalPriceNotDiscount);
            // Triết khấu mặc định
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
            data.details = _getOrderDetails(data.id, data.merger, data.defaultDiscount);
            #endregion

            #region Kiểm tra các điều kiện của đơn hàng
            var error = String.Empty;

            #region Kiểm tra nhà xe trả cước trước
            if (order.ShippingType == (int)DeliveryType.TransferStation
                && order.TransportCompanyID.HasValue
                && order.TransportCompanySubID.HasValue
                && data.shippingFee == 0
            )
            {
                var transport = TransportCompanyController.GetReceivePlaceByID(
                    order.TransportCompanyID.Value,
                    order.TransportCompanySubID.Value
                );

                if (transport != null && transport.Prepay == true)
                    error += "Chành xe này trả cước trước. Hãy nhập phí vận chuyển vào đơn hàng! Nếu muốn miễn phí cho khách thì trừ phí khác!";

            }
            #endregion

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
            btnCopyHtml.AppendLine(String.Format("   onclick='copyInvoiceURL({0}, {1})'", data.id, data.customer.id));
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