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

        /// <summary>
        /// Load page
        /// </summary>
        private void _loadPage()
        {
            #region Thông tin nhân viên
            var username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            #endregion

            #region Kiểm tra thông tin đơn hàng
            var orderId = Request.QueryString["id"].ToInt(0);
            var order = OrderController.GetByID(orderId);

            // Kiểm tra thông tin đơn hàng có tồn tại không
            if (order == null)
            {
                ltrShippingNote.Text = "Không tìm thấy đơn hàng!";

                return;
            }

            var errorHtml = new StringBuilder();

            // Kiểm tra trạng thái đơn hàng
            if (!(order.ExcuteStatus == (int)ExcuteStatus.Done || order.ExcuteStatus == (int)ExcuteStatus.Sent || order.ExcuteStatus == (int)ExcuteStatus.Return))
                errorHtml.AppendLine("<p>- Đơn hàng này <strong>Chưa hoàn tất</strong>!</p>");

            // Kiểm tra trạng thái thanh toán
            if (order.PaymentStatus == (int)PaymentStatus.Waitting)
                errorHtml.AppendLine("<p>- Đơn hàng này <strong>Chưa thanh toán</strong>!</p>");

            // Kiểm tra hình thức thanh toán
            if (order.PaymentType == (int)PaymentType.Cash && acc.RoleID != 0)
                errorHtml.AppendLine("<p>- Đơn hàng này <strong>Thanh toán tiền mặt</strong>. Hãy chuyển sang phương thức khác hoặc nhờ chị Ngọc in phiếu!</p>");

            #region Kiểm tra hình thức lấy hàng
            var transportCompany = (tbl_TransportCompany)null;
            var transportSubCompany = (tbl_TransportCompany)null;

            if (order.ShippingType == (int)DeliveryType.Face && acc.RoleID != 0)
                errorHtml.AppendLine("<p>- Đơn hàng này <strong>Lấy trực tiếp</strong>. Hãy chuyển sang phương thức khác hoặc nhờ chị Ngọc in phiếu!</p>");

            if (String.IsNullOrEmpty(order.ShippingCode))
            {
                switch (order.ShippingType)
                {
                    case (int)DeliveryType.PostOffice:
                        errorHtml.AppendLine("<p>- Đơn hàng này <strong>gửi Bưu điện</strong> nhưng <strong>chưa nhập</strong> MÃ VẬN ĐƠN!</p>");

                        break;
                    case (int)DeliveryType.Proship:
                        errorHtml.AppendLine("<p>- Đơn hàng này <strong>gửi Proship</strong> nhưng <strong>chưa nhập</strong> MÃ VẬN ĐƠN!</p>");

                        break;
                    case (int)DeliveryType.TransferStation:
                        var transportCompanyId = Convert.ToInt32(order.TransportCompanyID);

                        transportCompany = TransportCompanyController.GetTransportCompanyForOrderList(transportCompanyId);

                        if (transportCompany != null)
                        {
                            var transportSubCompanyID = Convert.ToInt32(order.TransportCompanySubID);

                            transportSubCompany = TransportCompanyController.GetReceivePlaceForOrderList(transportCompanyId, transportSubCompanyID);

                            if (transportSubCompany == null)
                                errorHtml.AppendLine("<p>- Đơn hàng này gửi xe <strong>" + transportCompany.CompanyName.ToTitleCase() + "</strong> nhưng <strong>chưa chọn Nơi nhận</strong>!</p>");
                            else if (transportSubCompany.Prepay == true && order.FeeShipping == "0")
                                errorHtml.AppendLine("Chành xe này trả cước trước. Hãy nhập phí vận chuyển vào đơn hàng! Nếu muốn miễn phí cho khách thì trừ phí khác!");
                        }
                        else
                            errorHtml.AppendLine("<p>- Đơn hàng này <strong>gửi xe</strong> nhưng <strong>chưa chọn Chành xe</strong> nào!</p>");

                        break;
                    case (int)DeliveryType.DeliverySave:
                        errorHtml.AppendLine("<p>- Đơn hàng này <strong>gửi GHTK</strong> nhưng <strong>chưa nhập</strong> MÃ VẬN ĐƠN!</p>");

                        break;
                    default:
                        break;
                }
            }
            else if (order.ShippingType == (int)DeliveryType.DeliverySave)
            {
                var lastGhtkCode = order.ShippingCode
                    .Split('.')
                    .Where(x => !String.IsNullOrEmpty(x))
                    .LastOrDefault();

                if (lastGhtkCode.Length < 9)
                    errorHtml.AppendLine("<p>- MÃ VẬN ĐƠN của GHTK phải có ít nhất 9 số ở cuối!</p>");
            }
            #endregion

            #region Đơn đổi trả
            var refund = (tbl_RefundGoods)null;

            if (order.RefundsGoodsID != null)
            {
                refund = RefundGoodController.GetByID(Convert.ToInt32(order.RefundsGoodsID));

                if (refund == null)
                    errorHtml.AppendLine(String.Format("<p>Không tìm thấy đơn hàng đổi trả {0} (có thể đã bị xóa khi làm lại đơn đổi trả). Thêm lại đơn hàng đổi trả nhé!</p>", order.RefundsGoodsID));
            }
            #endregion

            if (errorHtml.Length > 0)
            {
                ltrShippingNote.Text = "<h1>Lỗi:</h1>" + errorHtml.ToString();

                return;
            }
            #endregion

            // Thông tin địa chỉ gửi
            var sender = AgentController.GetByID(Convert.ToInt32(order.AgentID));
            // Thông tin địa chỉ nhận
            var receiver = _getReceiver(order.CustomerID.Value, order.DeliveryAddressId);
            // Thông tin chành xe
            var transport = TransportCompanyModel.map(transportCompany, transportSubCompany);
            // Tổng hợp thông tin đơn giao hàng
            var data = OrderModel.map(order, refund, AddressModel.map(sender), receiver, transport);
            var bodyClass = !String.IsNullOrEmpty(data.destination) ? "table-ghtk" : String.Empty;

            #region Phiếu giao hàng
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