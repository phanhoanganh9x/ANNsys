using IM_PJ.Controllers;
using IM_PJ.Models;
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
        public string createBarcode(string barcodeValue)
        {
            // Tạo barcode cho bưu điện
            var temps = new List<String>();
            var imageName = String.Format("{0}{1}.png", DateTime.UtcNow.ToString("yyyyMMddHHmmss"), Guid.NewGuid());
            string barcodeImage = "/uploads/shipping-barcodes/" + imageName;
            System.Drawing.Image barCode = PJUtils.MakeShippingBarcode(barcodeValue, 2, false);
            barCode.Save(HttpContext.Current.Server.MapPath("" + barcodeImage + ""), ImageFormat.Png);
            temps.Add(imageName);
            string result = "data:image/png;base64, " + Convert.ToBase64String(File.ReadAllBytes(Server.MapPath("" + barcodeImage + "")));

            // Xóa barcode sau khi tạo
            string[] filePaths = Directory.GetFiles(Server.MapPath("/uploads/shipping-barcodes/"));
            foreach (string filePath in filePaths)
            {
                foreach (var item in temps)
                {
                    if (filePath.EndsWith(item))
                    {
                        File.Delete(filePath);
                    }
                }
            }

            return result;
        }
        public void LoadData()
        {
            string username = Request.Cookies["usernameLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);

            string error = "";

            String rowHtml = String.Empty;

            string PrintButton = "";

            int ID = Request.QueryString["id"].ToInt(0);
            var order = OrderController.GetByID(ID);
            if (order == null)
            {
                error += "Không tìm thấy đơn hàng!";
            }
            else
            {
                if (order.PaymentStatus == 1)
                {
                    error += "<p>- Đơn hàng này <strong>Chưa thanh toán</strong>!</p>";
                }

                if (order.ExcuteStatus != 2)
                {
                    error += "<p>- Đơn hàng này <strong>Chưa hoàn tất</strong>!</p>";
                }

                if (order.ShippingType == 1 && acc.RoleID != 0)
                {
                    error += "<p>- Đơn hàng này <strong>Lấy trực tiếp</strong>. Hãy chuyển sang phương thức khác hoặc nhờ chị Ngọc in phiếu!</p>";
                }

                if (order.PaymentType == 1 && acc.RoleID != 0)
                {
                    error += "<p>- Đơn hàng này <strong>Thanh toán tiền mặt</strong>. Hãy chuyển sang phương thức khác hoặc nhờ chị Ngọc in phiếu!</p>";
                }

                string address = "";
                string phone = "";
                string leader = "";
                var agent = AgentController.GetByID(Convert.ToInt32(order.AgentID));

                if (agent != null)
                {
                    address = agent.AgentAddress;
                    leader = agent.AgentLeader;
                    phone = agent.AgentPhone;
                }

                double TotalOrder = Convert.ToDouble(order.TotalPrice);

                if (order.RefundsGoodsID != null)
                {
                    var refund = RefundGoodController.GetByID(Convert.ToInt32(order.RefundsGoodsID));
                    if (refund != null)
                    {
                        TotalOrder = TotalOrder - Convert.ToDouble(refund.TotalPrice);
                    }
                    else
                    {
                        error += "<p>Không tìm thấy đơn hàng đổi trả " + order.RefundsGoodsID.ToString() + " (có thể đã bị xóa khi làm lại đơn đổi trả). Thêm lại đơn hàng đổi trả nhé!</p>";
                    }
                }

                var customer = CustomerController.GetByID(order.CustomerID.Value);
                string CustomerAddress = String.Empty;
                string addressDetail = "";
                string ProvinceName = "";

                if (order.DeliveryAddressId.HasValue)
                {
                    var deliveryAddress = DeliveryController.getDeliveryAddressById(order.DeliveryAddressId.Value);
                    var Province = ProvinceController.GetByID(deliveryAddress.ProvinceId);
                    var District = ProvinceController.GetByID(deliveryAddress.DistrictId);
                    var Ward = ProvinceController.GetByID(customer.WardId.Value);

                    ProvinceName = Province.Name;
                    addressDetail = ", " + Province.Name;
                    addressDetail = ", " + District.Name + addressDetail;
                    addressDetail = ", " + Ward.Name + addressDetail;

                    CustomerAddress = order.CustomerAddress.ToTitleCase() + addressDetail;
                }
                else
                {
                    if (customer.ProvinceID.HasValue)
                    {
                        var Province = ProvinceController.GetByID(customer.ProvinceID.Value);

                        ProvinceName = Province.Name;
                        addressDetail = ", " + Province.Name;
                    }
                    if (customer.DistrictId.HasValue)
                    {
                        var District = ProvinceController.GetByID(customer.DistrictId.Value);

                        addressDetail = ", " + District.Name + addressDetail;
                    }
                    if (customer.WardId.HasValue && customer.WardId.Value > 0)
                    {
                        var Ward = ProvinceController.GetByID(customer.WardId.Value);

                        addressDetail = ", " + Ward.Name + addressDetail;
                    }

                    CustomerAddress = order.CustomerAddress.ToTitleCase() + addressDetail;
                }
                 
                string DeliveryInfo = "";
                string ShippingFeeInfo = "";
                string ShipperFeeInfo = "";

                // BƯU ĐIỆN
                if (order.ShippingType == 2)
                {
                    if (!string.IsNullOrEmpty(order.ShippingCode))
                    {
                        string PostalDeliveryType = "Thường";
                        if (order.PostalDeliveryType == 2)
                        {
                            PostalDeliveryType = "Nhanh";
                        }
                        DeliveryInfo = String.Format("<p class='delivery'><strong>Bưu điện - {0}:</strong> {1}</p><p><img src='{2}'></p>", PostalDeliveryType, order.ShippingCode, createBarcode(order.ShippingCode));
                    }
                    else
                    {
                        error += "<p>- Đơn hàng này <strong>gửi Bưu điện</strong> nhưng <strong>chưa nhập</strong> MÃ VẬN ĐƠN!</p>";
                    }

                    if (order.PaymentType != 3 && acc.RoleID != 0)
                    {
                        PrintButton = "<a class='btn btn-black' href='javascript:;' onclick='printError(`Bưu điện`)'>Không in được</a>";
                    }
                }
                // PROSHIP
                else if (order.ShippingType == 3)
                {
                    if (!string.IsNullOrEmpty(order.ShippingCode))
                    {
                        DeliveryInfo = String.Format("<p class='delivery'><strong>Proship:</strong> {0}</p><p><img src='{1}'></p>", order.ShippingCode, createBarcode(order.ShippingCode));
                    }
                    else
                    {
                        error += "<p>- Đơn hàng này <strong>gửi Proship</strong> nhưng <strong>chưa nhập</strong> MÃ VẬN ĐƠN!</p>";
                    }

                    if (order.PaymentType != 3 && acc.RoleID != 0)
                    {
                        PrintButton = "<a class='btn btn-black' href='javascript:;' onclick='printError(`Proship`)'>Không in được</a>";
                    }
                }
                // GỬI XE
                else if (order.ShippingType == 4)
                {
                    var company = TransportCompanyController.GetTransportCompanyForOrderList(Convert.ToInt32(order.TransportCompanyID));
                    if (company != null)
                    {
                        string transportCompany = "";
                        string transportCompanyPhone = "";
                        string transportCompanyAddress = "";
                        string transportCompanyNote = "";

                        transportCompany = "<strong>" + company.CompanyName.ToTitleCase() + "</strong>";
                        if (company.CompanyPhone != "")
                        {
                            transportCompanyPhone = "<span class='transport-info'>(" + company.CompanyPhone + ")</span>";
                        }
                        transportCompanyAddress = "<span class='transport-info'>" + company.CompanyAddress.ToTitleCase() + "</span>";
                        if (company.Note != "")
                        {
                            transportCompanyNote = "<span class='transport-info capitalize'> - " + company.Note.ToTitleCase() + "</span>";
                        }

                        var subID = Convert.ToInt32(order.TransportCompanySubID);
                        var shipto = TransportCompanyController.GetReceivePlaceForOrderList(company.ID, subID);
                        if (shipto != null && subID > 0)
                        {
                            // Kiểm tra nhà xe trả cước trước
                            if (shipto.Prepay == true && order.FeeShipping == "0")
                            {
                                error += "Chành xe này trả cước trước. Hãy nhập phí vận chuyển vào đơn hàng! Nếu muốn miễn phí cho khách thì trừ phí khác!";
                            }

                            if (!String.IsNullOrEmpty(ProvinceName))
                            {
                                CustomerAddress = "<span class='phone'>" + shipto.ShipTo.ToTitleCase() + " (" + ProvinceName + ")</span>";
                            }
                            else
                            {
                                CustomerAddress = "<span class='phone'>" + shipto.ShipTo.ToTitleCase()+ "</span>";
                            }
                        }
                        else
                        {
                            error += "<p>- Đơn hàng này gửi xe " + transportCompany + " nhưng <strong>chưa chọn Nơi nhận</strong>!</p>";
                        }

                        DeliveryInfo = String.Format("<p class='delivery'>Xe: {0} {1} {2}</p><p>{3}</p>", transportCompany, transportCompanyPhone, transportCompanyNote, transportCompanyAddress);
                    }
                    else
                    {
                        error += "<p>- Đơn hàng này <strong>gửi xe</strong> nhưng <strong>chưa chọn Chành xe</strong> nào!</p>";
                    }
                }
                // NHÂN VIÊN GIAO
                else if (order.ShippingType == 5)
                {
                    DeliveryInfo = String.Format("<p class='delivery'>Nhân viên giao</p>");
                }
                // GHTK
                else if (order.ShippingType == 6)
                {
                    if (!string.IsNullOrEmpty(order.ShippingCode))
                    {
                        string[] barcode = order.ShippingCode.Split('.');
                        string newCode = barcode[barcode.Length - 1];
                        if (newCode.Length < 9)
                        {
                            error += "<p>- MÃ VẬN ĐƠN của GHTK phải có ít nhất 9 số ở cuối!</p>";
                        }
                        DeliveryInfo = String.Format("<p class='delivery'><strong>GHTK:</strong> {0}</p>", order.ShippingCode);
                        DeliveryInfo += String.Format("<p><img src='{0}'></p>", createBarcode(newCode));
                    }
                    else
                    {
                        error += "<p>- Đơn hàng này <strong>gửi GHTK</strong> nhưng <strong>chưa nhập</strong> MÃ VẬN ĐƠN!</p>";
                    }

                    if (order.PaymentType != 3 && acc.RoleID != 0)
                    {
                        PrintButton = "<a class='btn btn-black' href='javascript:;' onclick='printError(`GHTK`)'>Không in được</a>";
                    }
                }
                // VIETTEL
                else if (order.ShippingType == 7)
                {
                    DeliveryInfo = String.Format("<p class='delivery'><strong>Viettel</strong></p>");
                }
                // Grab
                else if (order.ShippingType == 8)
                {
                    DeliveryInfo = String.Format("<p class='delivery'><strong>Grab</strong></p>");
                }
                // AhaMove
                else if (order.ShippingType == 9)
                {
                    DeliveryInfo = String.Format("<p class='delivery'><strong>AhaMove</strong></p>");
                }

                // Lấy tiền THU HỘ
                if (order.PaymentType == 3)
                {
                    ShippingFeeInfo = String.Format("<p class='cod'>Thu hộ: {0}</p>", string.Format("{0:N0}", TotalOrder));
                }
                else
                {
                    ShippingFeeInfo = String.Format("<p class='cod'>Thu hộ: KHÔNG</p>");
                }

                // Lấy phí nhân viên giao
                if (order.ShippingType == 5)
                {
                    if (Convert.ToDouble(order.FeeShipping) > 0)
                    {
                        ShipperFeeInfo = String.Format("<p class='shipping-fee'>Phí ship (đã cộng vào thu hộ): {0}</p>", string.Format("{0:N0}", Convert.ToDouble(order.FeeShipping)));
                    }
                    else
                    {
                        ShipperFeeInfo = String.Format("<p class='shipping-fee'>Phí ship: không</p>");
                    }
                }

                // Lấy số điện thoại 2 nếu có
                string CustomerPhone = order.CustomerPhone;
                
                if (!string.IsNullOrEmpty(customer.CustomerPhone2))
                {
                    CustomerPhone += " - " + customer.CustomerPhone2;
                }

                // Lấy logo ANN
                string LogoANN = "";
                if (order.ShippingType != 2 && order.ShippingType != 3 && order.ShippingType != 6)
                {
                    LogoANN = String.Format("<img class='img' src='https://ann.com.vn/wp-content/uploads/ANN-logo-3.png'>");
                }

                // Xử lý phiếu GHTK
                string cssClass = "";
                string bodyClass = "";
                string destination = "";
                if (order.ShippingType == 6 && !string.IsNullOrEmpty(order.ShippingCode))
                {
                    string[] barcode = order.ShippingCode.Split('.');
                    if (barcode.Length < 6 && barcode.Length > 3)
                    {
                        destination = String.Format("<p>{0}.{1}</p>", barcode[barcode.Length - 3], barcode[barcode.Length - 2]);
                    }
                    else if (barcode.Length >= 6)
                    {
                        destination = String.Format("<p>{0}.{1}.{2}</p>", barcode[barcode.Length - 4], barcode[barcode.Length - 3], barcode[barcode.Length - 2]);
                    }
                }
                if (destination != "")
                {
                    bodyClass = "table-ghtk";
                }

                // HTML in phiếu gửi hàng
                rowHtml += Environment.NewLine + String.Format("<div class='table {0}'>", bodyClass);
                rowHtml += Environment.NewLine + String.Format("    <div class='top-left'>");
                rowHtml += Environment.NewLine + String.Format("        <p>Người gửi: <span class='name'>{0}</span></p>", leader);
                rowHtml += Environment.NewLine + String.Format("        <p>{0}</p>", phone);
                rowHtml += Environment.NewLine + String.Format("        <p class='agent-address'>{0}</p>", address);
                rowHtml += Environment.NewLine + String.Format("        <p class='web'>ANN.COM.VN</p>");
                rowHtml += Environment.NewLine + String.Format("    </div>");
                rowHtml += Environment.NewLine + String.Format("    <div class='bottom-left'>");
                rowHtml += Environment.NewLine + String.Format("    {0}", ShippingFeeInfo);
                rowHtml += Environment.NewLine + String.Format("        <p>Nhân viên: {0}</p>", order.CreatedBy);
                rowHtml += Environment.NewLine + String.Format("        <p><img src='{0}'></p>", createBarcode(order.ID.ToString()));
                rowHtml += Environment.NewLine + String.Format("        <p>Mã đơn hàng: {0}</p>", order.ID);
                rowHtml += Environment.NewLine + String.Format("    </div>");
                rowHtml += Environment.NewLine + String.Format("    <div class='top-right'>");
                rowHtml += Environment.NewLine + String.Format("        {0}", LogoANN);
                rowHtml += Environment.NewLine + String.Format("        {0}", DeliveryInfo);
                rowHtml += Environment.NewLine + String.Format("        {0}", ShippingFeeInfo);
                rowHtml += Environment.NewLine + String.Format("        {0}", ShipperFeeInfo);
                rowHtml += Environment.NewLine + String.Format("    </div>");
                rowHtml += Environment.NewLine + String.Format("    <div class='bottom-right'>");
                rowHtml += Environment.NewLine + String.Format("        <p>Người nhận: <span class='name'>{0}</span></p>", order.CustomerName.ToTitleCase());
                rowHtml += Environment.NewLine + String.Format("        <p>Điện thoại: <span class='phone'>{0}</span></p>", CustomerPhone);
                rowHtml += Environment.NewLine + String.Format("        <p>Địa chỉ: <span class='address'>{0}</span></p>", CustomerAddress);
                rowHtml += Environment.NewLine + String.Format("    </div>");
                if (destination != "")
                {
                    rowHtml += Environment.NewLine + String.Format("    <div class='rotated ghtk'>");
                    rowHtml += Environment.NewLine + String.Format("        {0}", destination);
                    rowHtml += Environment.NewLine + String.Format("    </div>");
                    cssClass = "margin-left-ghtk";
                }
                rowHtml += Environment.NewLine + String.Format("    <div class='rotated {0}'>", cssClass);
                rowHtml += Environment.NewLine + String.Format("        KHO HÀNG SỈ ANN");
                rowHtml += Environment.NewLine + String.Format("    </div>");
                rowHtml += Environment.NewLine + String.Format("</div>");
                // Kết thúc HTML in phiếu gửi hàng
            }
            
            /// Hiển thị lỗi nếu có
            if (error != "")
            {
                ltrShippingNote.Text = "<h1>Lỗi:</h1>" + error;
            }
            else
            {
                ltrShippingNote.Text = rowHtml;
                ltrPrintButton.Text = "<div class='print-it'>";
                if (!string.IsNullOrEmpty(PrintButton))
                {
                    ltrPrintButton.Text += PrintButton;
                    ltrDisablePrint.Text = "<style type='text/css' media='print'>* { display: none; }</style>";
                    ltrDisablePrint.Text += "<script type='text/javascript'>jQuery(document).bind('keyup keydown', function(e){ if (e.ctrlKey && e.keyCode == 80){ return false;}});</script>";
                }
                else
                {
                    ltrPrintButton.Text += "<a class='btn' href='javascript:;' onclick='printIt()'>In phiếu gửi hàng</a>";
                }
                if (order.ShippingType == 4)
                {
                    ltrPrintButton.Text += "<a class='btn show-transport-info' href='javascript:;' onclick='showTransportInfo()'>Hiện thông tin nhà xe</a>";
                }
                if (order.ShippingType == 3 && order.PaymentType == 3)
                {
                    ltrPrintButton.Text += "<a class='btn show-transport-info' href='https://proship.vn/quan-ly-van-don/?isInvoiceFilter=1&generalInfo=" + order.ShippingCode + "' target='_blank'>Kiểm tra thu hộ trên Proship</a>";
                }
                ltrPrintButton.Text += "</div>";
            }
        }
    }
}