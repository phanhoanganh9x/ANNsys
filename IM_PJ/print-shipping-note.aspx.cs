using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Models.Pages.print_shipping_note;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
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
    public partial class print_shipping_note : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem_ANN123"] == null)
                    Response.Redirect("/dang-nhap");

                var username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                var acc = AccountController.GetByUsername(username);

                if (acc == null || (acc.RoleID != 0 && acc.RoleID != 2))
                    Response.Redirect("/trang-chu");

                _loadPage();
            }
        }

        #region Private
        #region Kiểm tra thông tin
        /// <summary>
        /// Kiểm tra trạng thái đơn hàng
        /// </summary>
        /// <param name="status">Trạng thái đơn hàng</param>
        /// <param name="message">Thông báo lỗi</param>
        /// <returns></returns>
        private bool _checkStatus(int status, ref string message)
        {
            message = String.Empty;

            if (status != (int)ExcuteStatus.Done || status != (int)ExcuteStatus.Sent)
                message += "<p>- Đơn hàng này <strong>Chưa hoàn tất</strong>!</p>";

            return String.IsNullOrEmpty(message);
        }

        /// <summary>
        /// Kiểm tra trạng thái thanh toán
        /// </summary>
        /// <param name="paymentStatus">Trạng thái thanh toán</param>
        /// <param name="message">Thông báo lỗi</param>
        /// <returns></returns>
        private bool _checkPaymentStatus(int paymentStatus, ref string message)
        {
            message = String.Empty;

            if (paymentStatus == (int)PaymentStatus.Waitting)
                message += "<p>- Đơn hàng này <strong>Chưa thanh toán</strong>!</p>";

            return String.IsNullOrEmpty(message);
        }

        /// <summary>
        /// Kiểm tra hình thức thanh toán
        /// </summary>
        /// <param name="paymentMethod">Hình thức thanh toán</param>
        /// <param name="role"></param>
        /// <param name="message">Thông báo lỗi</param>
        /// <returns></returns>
        private bool _checkPaymentMethod(int paymentMethod, int role, ref string message)
        {
            message = String.Empty;

            if (paymentMethod == (int)PaymentType.Cash && role != 0) {
                message += "<p>";
                message += "- Đơn hàng này <strong>Thanh toán tiền mặt</strong>. ";
                message += "Hãy chuyển sang phương thức khác hoặc nhờ chị Ngọc in phiếu!";
                message += "</p>";
            }

            return String.IsNullOrEmpty(message);
        }

        /// <summary>
        /// Kiểm tra hình thức lấy hàng
        /// </summary>
        /// <param name="deliveryMethod">Hình thức lấy hàng</param>
        /// <param name="role"></param>
        /// <param name="shippingCode">Mã vận đơn</param>
        /// <param name="transport">Nhà xe</param>
        /// <param name="transportBranch">Chi nhánh nhận hàng</param>
        /// <param name="fee">Phí giao hàng</param>
        /// <param name="message">Thông báo lỗi</param>
        /// <returns></returns>
        private bool _checkDeliveryMethod(
            int deliveryMethod,
            int role,
            string shippingCode,
            tbl_TransportCompany transport,
            tbl_TransportCompany transportBranch,
            decimal fee,
            ref string message
        ) {
            message = String.Empty;

            if (deliveryMethod == (int)DeliveryType.Face && role != 0)
            {
                message += "<p>";
                message += "- Đơn hàng này <strong>Lấy trực tiếp</strong>. ";
                message += "Hãy chuyển sang phương thức khác hoặc nhờ chị Ngọc in phiếu!";
                message += "</p>";
            }

            if (String.IsNullOrEmpty(shippingCode))
            {
                switch (deliveryMethod)
                {
                    case (int)DeliveryType.PostOffice:
                        message += "<p>- Đơn hàng này <strong>gửi Bưu điện</strong> nhưng <strong>chưa nhập</strong> MÃ VẬN ĐƠN!</p>";

                        break;
                    case (int)DeliveryType.Proship:
                        message += "<p>- Đơn hàng này <strong>gửi Proship</strong> nhưng <strong>chưa nhập</strong> MÃ VẬN ĐƠN!</p>";

                        break;
                    case (int)DeliveryType.TransferStation:
                        if (transport != null)
                        {
                            if (transportBranch == null)
                                message += "<p>- Đơn hàng này gửi xe <strong>" + transport.CompanyName.ToTitleCase() + "</strong> nhưng <strong>chưa chọn Nơi nhận</strong>!</p>";
                            else if (transportBranch.Prepay == true && fee == 0)
                            {
                                message += "<p>";
                                message += "Chành xe này trả cước trước. ";
                                message += "Hãy nhập phí vận chuyển vào đơn hàng! Nếu muốn miễn phí cho khách thì trừ phí khác!";
                                message += "</p>";
                            }
                        }
                        else
                            message += "<p>- Đơn hàng này <strong>gửi xe</strong> nhưng <strong>chưa chọn Chành xe</strong> nào!</p>";

                        break;
                    case (int)DeliveryType.DeliverySave:
                        message += "<p>- Đơn hàng này <strong>gửi GHTK</strong> nhưng <strong>chưa nhập</strong> MÃ VẬN ĐƠN!</p>";

                        break;
                    default:
                        break;
                }
            }
            else if (deliveryMethod == (int)DeliveryType.DeliverySave)
            {
                var lastGhtkCode = shippingCode
                    .Split('.')
                    .Where(x => !String.IsNullOrEmpty(x))
                    .LastOrDefault();

                if (lastGhtkCode.Length < 9)
                    message += "<p>- MÃ VẬN ĐƠN của GHTK phải có ít nhất 9 số ở cuối!</p>";
            }

            return String.IsNullOrEmpty(message);
        }

        /// <summary>
        /// Kiểm tra đơn đổi trả
        /// </summary>
        /// <param name="refundId">ID đơn đổi tra</param>
        /// <param name="refunds">Danh sách đơn đổi trả</param>
        /// <param name="message">Thông báo lỗi</param>
        /// <returns></returns>
        private bool _checkRefunds(IList<int> refundIds, IList<tbl_RefundGoods> refunds, ref string message)
        {
            var missedIds = new List<int>();
            message = String.Empty;

            if (!refunds.Any())
                missedIds = (List<int>)refundIds;
            else if (refundIds.Count != refunds.Count)
                missedIds = refundIds.Where(x => !refunds.Where(y => y.ID == x).Any()).ToList();

            if (missedIds.Any())
            {
                message += "<p>";
                message += "Không tìm thấy đơn hàng đổi trả ";
                message += String.Join(", ", missedIds.Select(x => String.Concat("#", x)).ToList());
                message += " (có thể đã bị xóa khi làm lại đơn đổi trả). ";
                message += "Thêm lại đơn hàng đổi trả nhé!";
                message += "</p>";
            }

            return String.IsNullOrEmpty(message);
        }

        /// <summary>
        /// Kiểm tra đơn đổi trả
        /// </summary>
        /// <param name="refundId">ID đơn đổi tra</param>
        /// <param name="refund">Thông tin đơn đổi trả</param>
        /// <param name="message">Thông báo lỗi</param>
        /// <returns></returns>
        private bool _checkRefund(int refundId, ref tbl_RefundGoods refund, ref string message)
        {
            refund = RefundGoodController.GetByID(refundId);
            message = String.Empty;

            if (refund == null)
            {
                message += "<p>";
                message += String.Format("Không tìm thấy đơn hàng đổi trả #{0} (có thể đã bị xóa khi làm lại đơn đổi trả). ", refund.ID);
                message += "Thêm lại đơn hàng đổi trả nhé!";
                message += "</p>";

            }

            return String.IsNullOrEmpty(message);
        }

        /// <summary>
        /// Kiểm tra thông tin đơn gộp
        /// </summary>
        /// <param name="groupOrder">Thông tin đơn gộp</param>
        /// <param name="role">Role người thực hiện in hóa đơn giao hàng</param>
        /// <param name="transport">Trả về thông tin chành xe (nếu có)</param>
        /// <param name="transportBranch">Trả về thông tin chành xe nhận hàng (nếu có)</param>
        /// <param name="error">Lấy thông tin lỗi</param>
        /// <returns></returns>
        private bool _checkGroupOrder(
            GroupOrder groupOrder,
            int role,
            ref tbl_TransportCompany transport,
            ref tbl_TransportCompany transportBranch,
            ref StringBuilder error
        )
        {
            var message = String.Empty;

            error.Clear();

            // Kiểm tra trạng thái đơn hàng
            if (!_checkStatus(groupOrder.OrderStatusId, ref message))
                error.AppendLine(message);

            // Kiểm tra trạng thái thanh toán
            if (!_checkPaymentStatus(groupOrder.PaymentStatusId, ref message))
                error.AppendLine(message);

            // Kiểm tra hình thức thanh toán
            if (!_checkPaymentMethod(groupOrder.PaymentMethodId, role, ref message))
                error.AppendLine(message);

            #region Kiểm tra hình thức lấy hàng
            if (String.IsNullOrEmpty(groupOrder.ShippingCode) && groupOrder.DeliveryMethodId == (int)DeliveryType.TransferStation)
            {
                if (groupOrder.TransportId.HasValue)
                    transport = TransportCompanyController.GetTransportCompanyForOrderList(groupOrder.TransportId.Value);

                if (transport != null && groupOrder.TransportBranchId.HasValue)
                    transportBranch = TransportCompanyController.GetReceivePlaceForOrderList(transport.ID, groupOrder.TransportBranchId.Value);
            }

            if (!_checkDeliveryMethod(groupOrder.DeliveryMethodId, role, groupOrder.ShippingCode, transport, transportBranch, groupOrder.ShippingFee, ref message))
                error.AppendLine(message);
            #endregion

            #region Đơn đổi trả
            var refundIds = GroupOrderController.getRefundIds(groupOrder.Code);

            if (refundIds.Any())
            {
                var refunds = RefundGoodController.GetByGroupOrderCode(groupOrder.Code);

                if (!_checkRefunds(refundIds, refunds, ref message))
                    error.AppendLine(message);
            }
            #endregion

            return error.Length == 0;
        }

        /// <summary>
        /// Kiểm tra thông tin đơn
        /// </summary>
        /// <param name="order">Thông tin đơn</param>
        /// <param name="role">Role người thực hiện in hóa đơn giao hàng</param>
        /// <param name="transport">Trả về thông tin chành xe (nếu có)</param>
        /// <param name="transportBranch">Trả về thông tin chành xe nhận hàng (nếu có)</param>
        /// <param name="error">Lấy thông tin lỗi</param>
        /// <returns></returns>
        private bool _checkOrder(
            tbl_Order order,
            int role,
            ref tbl_RefundGoods refund,
            ref tbl_TransportCompany transport,
            ref tbl_TransportCompany transportBranch,
            ref StringBuilder error
        )
        {
            var message = String.Empty;

            error.Clear();

            // Kiểm tra trạng thái đơn hàng
            if (!_checkStatus(order.ExcuteStatus.Value, ref message))
                error.AppendLine(message);

            // Kiểm tra trạng thái thanh toán
            if (!_checkPaymentStatus(order.PaymentStatus.Value, ref message))
                error.AppendLine(message);

            // Kiểm tra hình thức thanh toán
            if (!_checkPaymentMethod(order.PaymentType.Value, role, ref message))
                error.AppendLine(message);

            #region Kiểm tra hình thức lấy hàng
            if (String.IsNullOrEmpty(order.ShippingCode) && order.ShippingType.Value == (int)DeliveryType.TransferStation)
            {
                if (order.TransportCompanyID.HasValue && order.TransportCompanyID > 0)
                    transport = TransportCompanyController.GetTransportCompanyForOrderList(order.TransportCompanyID.Value);

                if (transport != null && order.TransportCompanySubID.HasValue && order.TransportCompanySubID > 0)
                    transportBranch = TransportCompanyController.GetReceivePlaceForOrderList(transport.ID, order.TransportCompanySubID.Value);
            }

            if (!_checkDeliveryMethod(order.ShippingType.Value, role, order.ShippingCode, transport, transportBranch, Convert.ToDecimal(order.FeeShipping), ref message))
                error.AppendLine(message);
            #endregion

            #region Đơn đổi trả
            if (order.RefundsGoodsID.HasValue)
                if (!_checkRefund(order.RefundsGoodsID.Value, ref refund, ref message))
                    error.AppendLine(message);
            #endregion

            return error.Length == 0;
        }
        #endregion

        /// <summary>
        /// Lấy thông tin địa chỉ nhận hàng
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="deliveryAddressId"></param>
        /// <returns></returns>
        private AddressModel _getReceiver(int customerId, long? deliveryAddressId)
        {
            var province = (DeliverySaveAddress)null;
            var district = (DeliverySaveAddress)null;
            var ward = (DeliverySaveAddress)null;

            #region Trường hợp có ID địa chỉ nhận hàng
            if (deliveryAddressId.HasValue)
            {
                var deliveryAddress = DeliveryController.getDeliveryAddressById(deliveryAddressId.Value);

                province = ProvinceController.GetByID(deliveryAddress.ProvinceId);
                district = ProvinceController.GetByID(deliveryAddress.DistrictId);
                ward = ProvinceController.GetByID(deliveryAddress.WardId);

                return AddressModel.map(deliveryAddress, province, district, ward);
            }
            #endregion

            #region Nếu không có ID địa chỉ nhận hàng sẽ lấy theo địa chỉ của khác hàng
            var customer = CustomerController.GetByID(customerId);

            province = customer.ProvinceID.HasValue && customer.ProvinceID.Value > 0
                ? ProvinceController.GetByID(customer.ProvinceID.Value) : null;
            district = customer.DistrictId.HasValue && customer.DistrictId.Value > 0
                ? ProvinceController.GetByID(customer.DistrictId.Value) : null;
            ward = customer.WardId.HasValue && customer.WardId.Value > 0
                ? ProvinceController.GetByID(customer.WardId.Value) : null;

            return AddressModel.map(customer, province, district, ward);
            #endregion
        }

        /// <summary>
        /// Khởi tạo barcode
        /// </summary>
        /// <param name="barcodeValue"></param>
        /// <returns></returns>
        private string _createBarcode(string barcodeValue)
        {
            // Tạo barcode cho bưu điện
            var temps = new List<String>();
            var imageName = String.Format("{0}{1}.png", DateTime.UtcNow.ToString("yyyyMMddHHmmss"), Guid.NewGuid());
            var barcodeImage = "/uploads/shipping-barcodes/" + imageName;
            var barCode = PJUtils.MakeShippingBarcode(barcodeValue, 2, false);

            barCode.Save(HttpContext.Current.Server.MapPath("" + barcodeImage + ""), ImageFormat.Png);
            temps.Add(imageName);
            var imageBase64 = Convert.ToBase64String(File.ReadAllBytes(Server.MapPath("" + barcodeImage + "")));
            var result = String.Format("data:image/png;base64, {0}", imageBase64);

            // Xóa barcode sau khi tạo
            var filePaths = Directory.GetFiles(Server.MapPath("/uploads/shipping-barcodes/"));

            foreach (string filePath in filePaths)
                foreach (var item in temps)
                    if (filePath.EndsWith(item))
                        File.Delete(filePath);

            return result;
        }

        #region Khởi tạo HTML
        /// <summary>
        /// Khởi tạo HTML tag về COD
        /// </summary>
        /// <param name="paymentMethod">PHương thức thanh toán</param>
        /// <param name="cod"></param>
        /// <returns></returns>
        private string _createCodHtml(int paymentMethod, decimal cod)
        {
            var html = String.Empty;

            if (paymentMethod == (int)PaymentType.CashCollection)
                html = String.Format("<p class='cod'>Thu hộ: {0:N0}</p>", cod);
            else
                html = String.Format("<p class='cod'>Thu hộ: KHÔNG</p>");

            return html;
        }

        /// <summary>
        /// Khởi tạo HTML thông tin địa chỉ nhận hàng
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private string _createSenderHtml(AddressModel sender)
        {
            var html = new StringBuilder();

            html.AppendLine("<div class='top-left'>");
            html.AppendLine(String.Format("    <p>Người gửi: <span class='sender-name'>{0}</span></p>", sender.name));
            html.AppendLine(String.Format("    <p>{0} - {1}</p>", sender.phone, sender.phone2));
            html.AppendLine(String.Format("    <p class='agent-address'>{0}</p>", sender.address));
            html.AppendLine("</div>");

            return html.ToString();
        }

        /// <summary>
        /// Khởi tạo HTML thông tin hình thức giao hàng
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private string _createDeliveryHtml(OrderModel order)
        {
            #region Logo ANN
            var annLogo = String.Empty;

            if (order.deliveryMethod != (int)DeliveryType.PostOffice && order.deliveryMethod != (int)DeliveryType.Proship && order.deliveryMethod != (int)DeliveryType.DeliverySave)
                annLogo = "<img class='img' src='https://ann.com.vn/wp-content/uploads/ANN-logo-3.png'>";
            #endregion

            #region Thông tin giao hàng
            var deliveryInfo = new StringBuilder();

            switch (order.deliveryMethod)
            {
                case (int)DeliveryType.PostOffice:
                    var method = order.postalDeliveryMethod == 2 ? "Nhanh" : "Thường";

                    deliveryInfo.AppendLine(String.Format("<p class='delivery'><strong>Bưu điện - {0}:</strong> {1}</p>", method, order.shippingCode));
                    deliveryInfo.AppendLine(String.Format("<p><img src='{0}'></p>", _createBarcode(order.shippingCode)));

                    break;
                case (int)DeliveryType.Proship:
                    deliveryInfo.AppendLine(String.Format("<p class='delivery'><strong>Proship:</strong> {0}</p>", order.shippingCode));
                    deliveryInfo.AppendLine(String.Format("<p><img src='{0}'></p>", _createBarcode(order.shippingCode)));

                    break;
                case (int)DeliveryType.TransferStation:
                    #region Số điện thoại nhà xe
                    var transportPhone = String.Empty;

                    if (!String.IsNullOrEmpty(order.transport.phone))
                        transportPhone = String.Format("<span class='transport-info'>({0})</span>", order.transport.phone);
                    #endregion

                    #region Note
                    var transportNote = String.Empty;

                    if (!String.IsNullOrEmpty(order.transport.note))
                        transportNote = String.Format("<span class='transport-info capitalize'> - {0}</span>", order.transport.phone);
                    #endregion

                    deliveryInfo.AppendLine(String.Format("<p class='delivery'>Xe: <strong>{0}</strong> {1} {2}</p>", order.transport.name, transportPhone, transportNote));
                    deliveryInfo.AppendLine(String.Format("<p><span class='transport-info'>'{0}'</span></p>", order.transport.address));

                    break;
                case (int)DeliveryType.Shipper:
                    deliveryInfo.AppendLine("<p class='delivery'>Nhân viên giao</p>");

                    break;
                case (int)DeliveryType.DeliverySave:
                    var codes = order.shippingCode.Split('.').Where(x => !String.IsNullOrEmpty(x)).ToList();
                    var lastCode = codes.LastOrDefault();

                    deliveryInfo.AppendLine(String.Format("<p class='delivery'><strong>GHTK:</strong> {0}</p>", order.shippingCode));
                    deliveryInfo.AppendLine(String.Format("<p><img class='barcode-image' src='{0}'></p>", _createBarcode(lastCode)));
                    break;
                case (int)DeliveryType.Viettel:
                    deliveryInfo.AppendLine("<p class='delivery'><strong>Viettel</strong></p>");

                    break;
                case (int)DeliveryType.Grab:
                    deliveryInfo.AppendLine("<p class='delivery'><strong>Grab</strong></p>");

                    break;
                case (int)DeliveryType.AhaMove:
                    deliveryInfo.AppendLine("<p class='delivery'><strong>AhaMove</strong></p>");

                    break;
                case (int)DeliveryType.JT:
                    deliveryInfo.AppendLine("<p class='delivery'><strong>J&T</strong></p>");

                    break;
                case (int)DeliveryType.GHN:
                    deliveryInfo.AppendLine("<p class='delivery'><strong>GHN</strong></p>");

                    break;
                default:
                    break;
            }
            #endregion

            #region Phí nhân viên giao
            var shopFeeInfo = String.Empty;

            if (order.deliveryMethod == (int)DeliveryType.Shipper)
            {
                if (order.shopFee > 0)
                    shopFeeInfo = String.Format("<p class='shipping-fee'>Phí ship (đã cộng vào thu hộ): {0:N0}</p>", order.shopFee);
                else
                    shopFeeInfo = "<p class='shipping-fee'>Phí ship: không</p>";
            }
            #endregion

            var html = new StringBuilder();

            html.AppendLine(String.Format("<div class='top-right'>"));
            // Logo ANN
            if (!String.IsNullOrEmpty(annLogo))
                html.AppendLine(String.Format("    {0}", annLogo));
            // Thông tin giao hàng
            if (deliveryInfo.Length > 0)
                html.AppendLine(String.Format("    {0}", deliveryInfo.ToString()));
            // Thu hộ
            html.AppendLine(String.Format("    {0}", _createCodHtml(order.paymentMethod, order.cod)));
            // Phí nhân viên giao
            if (!String.IsNullOrEmpty(shopFeeInfo))
                html.AppendLine(String.Format("    {0}", shopFeeInfo));
            html.AppendLine(String.Format("</div>"));

            return html.ToString();
        }

        /// <summary>
        /// Khởi tạo HTML thông tin hóa đơn
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private string _createOrderHtml(OrderModel order)
        {
            var html = new StringBuilder();

            html.AppendLine("<div class='bottom-left'>");
            html.AppendLine(_createCodHtml(order.paymentMethod, order.cod));
            html.AppendLine(String.Format("    <p>Nhân viên: {0}</p>", order.staff));
            html.AppendLine(String.Format("    <p><img src='{0}'></p>", _createBarcode(order.code)));
            html.AppendLine(String.Format("    <p>Mã đơn hàng: <strong class='order-id'>{0}</strong></p>", order.code));
            html.AppendLine("</div>");

            return html.ToString();
        }

        /// <summary>
        /// Khởi tạo HTML thông tin người nhận
        /// </summary>
        /// <param name="order">Dữ liệu đơn hàng</param>
        /// <returns></returns>
        private string _createReceiverHtml(OrderModel order)
        {
            #region SĐT nhận
            var phoneHtml = String.Empty;

            if (order.deliveryMethod == (int)DeliveryType.DeliverySave
                || order.deliveryMethod == (int)DeliveryType.JT
                || order.deliveryMethod == (int)DeliveryType.GHN)
                phoneHtml += "<span class='phone replace-phone'>";
            else
                phoneHtml = "<span class='phone'>";

            phoneHtml += order.receiver.phone;

            if (!String.IsNullOrEmpty(order.receiver.phone2))
                phoneHtml += String.Format(" - {0}", order.receiver.phone2);

            phoneHtml += "</span>";
            #endregion

            #region Địa chỉ nhận
            var addressHtml = String.Empty;

            if (order.deliveryMethod == (int)DeliveryType.TransferStation)
            {
                if (!String.IsNullOrEmpty(order.receiver.provinceName))
                    addressHtml = String.Format("<span class='phone'>{0} ({1})</span>", order.transport.shipTo, order.receiver.provinceName);
                else
                    addressHtml = String.Format("<span class='phone'>{0}</span>", order.transport.shipTo);
            }
            else
            {
                addressHtml += order.receiver.address;

                if (!String.IsNullOrEmpty(order.receiver.wardName))
                    addressHtml += String.Format(", {0}", order.receiver.wardName);

                if (!String.IsNullOrEmpty(order.receiver.districtName))
                    addressHtml += String.Format(", {0}", order.receiver.districtName);

                if (!String.IsNullOrEmpty(order.receiver.provinceName))
                    addressHtml += String.Format(", {0}", order.receiver.provinceName);
            }
            #endregion

            var html = new StringBuilder();

            html.AppendLine("<div class='bottom-right'>");
            html.AppendLine(String.Format("    <p>Người nhận: <span class='receiver-name'>{0}</span></p>", order.receiver.name));
            html.AppendLine(String.Format("    <p>Điện thoại: {0}</p>", phoneHtml));
            html.AppendLine(String.Format("    <p><span class='address'>{0}</span></p>", addressHtml));
            html.AppendLine("</div>");

            return html.ToString();
        }

        /// <summary>
        /// Khởi tạo HTML nhãn bên trái hóa đơn
        /// </summary>
        /// <param name="destination">Mã nơi đên của GHTK</param>
        /// <returns></returns>
        private string _createLabelHtml(string destination)
        {
            var html = new StringBuilder();

            if (!String.IsNullOrEmpty(destination))
            {
                html.AppendLine("<div class='rotated ghtk'>");
                html.AppendLine(String.Format("    {0}", destination));
                html.AppendLine("</div>");
                html.AppendLine("<div class='rotated margin-left-ghtk'>");
            }
            else
                html.AppendLine("<div class='rotated'>");

            html.AppendLine("    ANN.COM.VN");
            html.AppendLine("</div>");

            return html.ToString();
        }

        /// <summary>
        /// Khởi tạo HTML hóa đơn giao hàng
        /// </summary>
        /// <param name="order">Dữ liệu đơn hàng</param>
        /// <param name="bodyClass">CSS GHTK</param>
        /// <returns></returns>
        private string _createDeliveryInvoiceHtml(OrderModel order, string bodyClass)
        {
            var html = new StringBuilder();

            html.AppendLine(String.Format("<div class='table {0}'>", bodyClass));
            html.AppendLine(_createSenderHtml(order.sender));
            html.AppendLine(_createDeliveryHtml(order));
            html.AppendLine(_createOrderHtml(order));
            html.AppendLine(_createReceiverHtml(order));
            html.AppendLine(_createLabelHtml(order.destination));
            html.AppendLine("</div>");

            return html.ToString();
        }

        /// <summary>
        /// Khởi tạo HTML về cam kết hàng chính hảng
        /// </summary>
        /// <param name="bodyClass">CSS GHTK</param>
        /// <returns></returns>
        private string _createDeliveryNoteHtml(string bodyClass)
        {
            var html = new StringBuilder();

            html.AppendLine(String.Format("<div class='table-note {0}'>", bodyClass));
            html.AppendLine("    <h2>CAM KẾT CHÍNH HÃNG 100%</h2>");
            html.AppendLine("    <p>Quý khách có nhu cầu xác minh hàng thật, vui lòng liên hệ hotline: 0922.474.777 - 0914.615.408</p>");
            html.AppendLine("    <p>Chúng tôi chấp nhận bồi thường gấp 10 lần giá trị đơn hàng, nếu quý khách phát hiện hàng giả!</p>");
            html.AppendLine("    <p>Địa chỉ công ty: 68 Đường C12, Phường 13, Tân Bình, TPHCM</p>");
            html.AppendLine("    <p>Người đại diện: Trần Yến Ngọc</p>");
            html.AppendLine("</div>");

            return html.ToString();
        }

        /// <summary>
        /// Khởi tạo HTML thông báo hàng dễ vỡ
        /// </summary>
        /// <param name="bodyClass">CSS GHTK</param>
        /// <returns></returns>
        private string _createFragileGoodsNoteHtml(string bodyClass)
        {
            var html = new StringBuilder();

            html.AppendLine(String.Format("<div class='table-fragile-goods {0}'>", bodyClass));
            html.AppendLine("    <h2>HÀNG DỄ VỠ</h2>");
            html.AppendLine("    <h2>XIN NHẸ TAY!</h2>");
            html.AppendLine("    <p>Hãy liên hệ chúng tôi, nếu khách hàng từ chối nhận hàng!</p>");
            html.AppendLine("    <p>CHÂN THÀNH CÁM ƠN!</p>");
            html.AppendLine("</div>");

            return html.ToString();
        }

        /// <summary>
        /// Khởi tạo HTML button in hóa đơn
        /// </summary>
        /// <param name="order">Dữ liệu đơn hàng</param>
        /// <param name="accRole">Role của account</param>
        /// <returns></returns>
        private string _createButtonHtml(OrderModel order, int accRole)
        {
            #region Button in phiếu giao hàng
            var errorPrint = false;
            var printerButtonHtml = "<a class='btn' href='javascript:;' onclick='printIt()'>In phiếu gửi hàng</a>";

            if (order.paymentMethod != (int)PaymentType.CashCollection && accRole != 0)
            {
                if (order.deliveryMethod == (int)DeliveryType.PostOffice)
                {
                    printerButtonHtml = "<a class='btn btn-black' href='javascript:;' onclick='printError(`Bưu điện`)'>Không in được</a>";
                    errorPrint = true;
                }

                if (order.deliveryMethod == (int)DeliveryType.Proship)
                {
                    printerButtonHtml = "<a class='btn btn-black' href='javascript:;' onclick='printError(`Proship`)'>Không in được</a>";
                    errorPrint = true;
                }

                if (order.deliveryMethod == (int)DeliveryType.DeliverySave)
                {
                    printerButtonHtml = "<a class='btn btn-black' href='javascript:;' onclick='printError(`GHTK`)'>Không in được</a>";
                    errorPrint = true;
                }
            }

            if (errorPrint)
            {
                ltrDisablePrint.Text = "<style type='text/css' media='print'>* { display: none; }</style>";
                ltrDisablePrint.Text += "<script type='text/javascript'>jQuery(document).bind('keyup keydown', function(e){ if (e.ctrlKey && e.keyCode == 80){ return false;}});</script>";
            }
            #endregion

            var html = new StringBuilder();

            html.AppendLine("<div class='print-it'>");
            html.AppendLine(String.Format("    {0}", printerButtonHtml));

            // Button hiển thị nhà xe
            if (order.deliveryMethod == (int)DeliveryType.TransferStation)
                html.AppendLine("    <a class='btn show-transport-info' href='javascript:;' onclick='showTransportInfo()'>Hiện thông tin nhà xe</a>");

            // Kiểm tra thu hộ Proship
            if (order.paymentMethod == (int)PaymentType.CashCollection && order.deliveryMethod == (int)DeliveryType.Proship)
            {
                var proshipUrl = String.Format("https://proship.vn/quan-ly-van-don/?isInvoiceFilter=1&generalInfo={0}", order.shippingCode);

                html.AppendLine(String.Format("    <a class='btn show-transport-info' href='{0}' target='_blank'>Kiểm tra thu hộ trên Proship</a>", proshipUrl));
            }

            html.AppendLine("    <a class='btn btn-green' href='javascript:;' onclick='printNote()'>In phiếu cam kết</a>");
            html.AppendLine("    <a class='btn btn-blue' href='javascript:;' onclick='printFragileGoods()'>In phiếu hàng dễ vỡ</a>");
            html.AppendLine("</div>");

            return html.ToString();
        }
        #endregion

        /// <summary>
        /// Lấy thông tin dữ liệu đơn gộp
        /// </summary>
        /// <param name="groupOrder">Thông tin đơn hàng gộp</param>
        /// <param name="refunds">Danh sách đơn đổi trả</param>
        /// <param name="transportCompany">Thông tin chành xe</param>
        /// <param name="transportBranchCompany">Thông tin chành xe nhận</param>
        /// <returns></returns>
        private OrderModel _getGroupOrder(
            GroupOrder groupOrder,
            tbl_TransportCompany transportCompany,
            tbl_TransportCompany transportBranchCompany
        )
        {
            // Thông tin địa chỉ gửi
            var sender = AgentController.GetByID(1); // Hardcode: 1 - ANN Shop
            // Thông tin địa chỉ nhận
            var receiver = _getReceiver(groupOrder.CustomerId, groupOrder.DeliveryAddressId);
            // Thông tin chành xe
            var transport = TransportCompanyModel.map(transportCompany, transportBranchCompany);
            // Tổng hợp thông tin đơn giao hàng
            var data = OrderModel.map(groupOrder, AddressModel.map(sender), receiver, transport);

            return data;
        }

        /// <summary>
        /// Lấy thông tin đơn hàng
        /// </summary>
        /// <param name="order">Thông tin đơn hàng</param>
        /// <param name="refund">Thông tin đơn hàng đổi trả</param>
        /// <param name="transportCompany">Thông tin chành xe</param>
        /// <param name="transportBranchCompany">Thông tin chành xe nhận</param>
        /// <returns></returns>
        private OrderModel _getOrder(
            tbl_Order order,
            tbl_RefundGoods refund,
            tbl_TransportCompany transportCompany,
            tbl_TransportCompany transportBranchCompany
        ) {
            // Thông tin địa chỉ gửi
            var sender = AgentController.GetByID(order.AgentID.Value); // Hardcode: 1 - ANN Shop
            // Thông tin địa chỉ nhận
            var receiver = _getReceiver(order.CustomerID.Value, order.DeliveryAddressId);
            // Thông tin chành xe
            var transport = TransportCompanyModel.map(transportCompany, transportBranchCompany);
            // Tổng hợp thông tin đơn giao hàng
            var data = OrderModel.map(order, refund, AddressModel.map(sender), receiver, transport);

            return data;
        }

        /// <summary>
        /// Load page
        /// </summary>
        private void _loadPage()
        {
            #region Kiểm tra query parameter
            #region Kiểm tra giá trị truyền
            var groupOrderCode = Request.QueryString["groupCode"];
            var orderId = Request.QueryString["id"].ToInt(0);

            if (String.IsNullOrEmpty(groupOrderCode) && orderId == 0)
            {
                ltrShippingNote.Text = "Không tìm thấy mã đơn hàng gộp hoặc ID đơn hàng";
                return;
            }

            if (!String.IsNullOrEmpty(groupOrderCode) && orderId > 0)
            {
                ltrShippingNote.Text = "Đang truyền cùng lúc mã đơn hàng gộp và ID đơn hàng";
                return;
            }
            #endregion

            #region Kiểm tra tồn tại Group Order
            var groupOrder = (GroupOrder)null;

            if (!String.IsNullOrEmpty(groupOrderCode))
            {
                groupOrderCode = groupOrderCode.Trim();
                groupOrder = GroupOrderController.getByCode(groupOrderCode);

                if (groupOrder == null)
                {
                    ltrShippingNote.Text = String.Format("Đơn hàng gộp #{0} không tồn tại trong hệ thống", groupOrderCode);
                    return;
                }
            }
            #endregion

            #region Kiểm tra tồn tại Order
            var order = (tbl_Order)null;

            if (orderId > 0)
            {
                order = OrderController.GetByID(orderId);

                if (order == null)
                {
                    ltrShippingNote.Text = String.Format("Đơn hàng #{0} không tồn tại trong hệ thống", orderId);;
                    return;
                }
            }
            #endregion
            #endregion

            #region Thông tin nhân viên
            var username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            #endregion

            #region Lấy dữ liệu phiếu giao hàng
            var transport = (tbl_TransportCompany)null;
            var transportBranch = (tbl_TransportCompany)null;
            var data = (OrderModel)null;
            var errorHtml = new StringBuilder();

            if (groupOrder != null)
                if (_checkGroupOrder(groupOrder, acc.RoleID.Value, ref transport, ref transportBranch, ref errorHtml))
                    data = _getGroupOrder(groupOrder, transport, transportBranch);

            if (order != null)
            {
                var refund = (tbl_RefundGoods)null;

                if (_checkOrder(order, acc.RoleID.Value, ref refund, ref transport, ref transportBranch, ref errorHtml))
                    data = _getOrder(order, refund, transport, transportBranch);
            }
            #endregion

            #region Trường hợp thông tin lỗi
            if (errorHtml.Length > 0)
            {
                ltrShippingNote.Text = "<h1>Lỗi:</h1>" + errorHtml.ToString();

                return;
            }
            #endregion

            #region Phiếu giao hàng
            var bodyClass = !String.IsNullOrEmpty(data.destination) ? "table-ghtk" : String.Empty;
            var shippingNote = new StringBuilder();

            // HTML in phiếu gửi hàng
            shippingNote.AppendLine(_createDeliveryInvoiceHtml(data, bodyClass));
            // HTML in phiếu cam kết chính hãng
            shippingNote.AppendLine(_createDeliveryNoteHtml(bodyClass));
            // HTML in phiếu hàng dễ vỡ
            shippingNote.AppendLine(_createFragileGoodsNoteHtml(bodyClass));

            ltrShippingNote.Text = shippingNote.ToString();
            #endregion

            ltrPrintButton.Text = _createButtonHtml(data, acc.RoleID.Value);
        }
        #endregion
    }
}