using IM_PJ.Controllers;
using IM_PJ.Models.Pages.print_jt_express;
using Newtonsoft.Json;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace IM_PJ
{
    public partial class print_jt_express : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem_ANN123"] == null)
                {
                    Response.Redirect("/dang-nhap");
                    return;
                }

                var username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                var acc = AccountController.GetByUsername(username);

                if (acc == null || (acc.RoleID != 0 && acc.RoleID != 2))
                {
                    Response.Redirect("/dang-nhap");
                    return;
                }

                _loadData();
            }
        }

        #region Private
        private OrderResponseModel _getJtOrder(int orderId, string code)
        {
            #region Khởi tạo API
            var api = String.Format("http://ann-shop-dotnet-core.com/api/v1/jt-express/order/{0}?orderId={1}", code, orderId);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(api);

            httpWebRequest.Method = "GET";
            #endregion

            try
            {
                // Thực thi API
                var response = (HttpWebResponse)httpWebRequest.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                    return null;

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var jtOrder = JsonConvert.DeserializeObject<OrderResponseModel>(reader.ReadToEnd());

                    return jtOrder;
                }
            }
            catch (WebException we)
            {
                throw we;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private OrderResponseModel _getJtOrder(string groupCode, string code)
        {
            #region Khởi tạo API
            var api = String.Format("http://ann-shop-dotnet-core.com/api/v1/jt-express/order/{0}?groupCode={1}", code, groupCode);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(api);

            httpWebRequest.Method = "GET";
            #endregion

            try
            {
                // Thực thi API
                var response = (HttpWebResponse)httpWebRequest.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                    return null;

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var jtOrder = JsonConvert.DeserializeObject<OrderResponseModel>(reader.ReadToEnd());

                    return jtOrder;
                }
            }
            catch (WebException we)
            {
                throw we;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void _loadSenderAddress(OrderAddressResponseModel sender)
        {
            var index = 1;
            var maxLength = 34;
            var senderAddress = String.Empty;

            if (String.IsNullOrEmpty(sender.address))
                senderAddress = String.Format("{0}, {1}, {2}", sender.ward, sender.district, sender.province);
            else
                senderAddress = String.Format("{0}, {1}, {2}, {3}",  sender.address,  sender.ward,  sender.district, sender.province);

            var words = senderAddress
                .Split(' ')
                .Where(x => !String.IsNullOrEmpty(x.Trim()))
                .ToList();

            foreach (var item in words)
            {
                var hasSpace = true;
                var length = item.Length + 1;

                if (index == 1)
                    length += ltSenderAddressLine1.Text.Length;
                else if (index == 2)
                    length += ltSenderAddressLine2.Text.Length;
                else
                    length += ltSenderAddressLine3.Text.Length;

                if (length > maxLength)
                {
                    if ((length - 1) == maxLength)
                    {
                        hasSpace = false;
                    }
                    else
                    {
                        index += 1;
                        length = item.Length + 1;
                    }
                }

                if (index == 1)
                    ltSenderAddressLine1.Text += item + (hasSpace ?  " " : String.Empty);
                else if (index == 2)
                    ltSenderAddressLine2.Text += item + (hasSpace ? " " : String.Empty);
                else if (index == 3)
                    ltSenderAddressLine3.Text += item + (hasSpace ? " " : String.Empty);
                else
                    break;
            }

            if (index == 1)
            {
                ltSenderAddressLine2.Text = "&nbsp;";
                ltSenderAddressLine3.Text = "&nbsp;";
            }
            else if (index == 2)
            {
                ltSenderAddressLine3.Text = "&nbsp;";
            }
        }

        private void _loadReceiverAddress(OrderAddressResponseModel receiver)
        {
            var index = 1;
            var maxLength = 38;
            var receiverAddress = String.Empty;

            if (String.IsNullOrEmpty(receiver.address))
                receiverAddress = String.Format("{0}, {1}, {2}", receiver.ward, receiver.district, receiver.province);
            else
                receiverAddress = String.Format("{0}, {1}, {2}, {3}", receiver.address, receiver.ward, receiver.district, receiver.province);

            var words = receiverAddress
                .Split(' ')
                .Where(x => !String.IsNullOrEmpty(x.Trim()))
                .ToList();

            foreach (var item in words)
            {
                var hasSpace = true;
                var length = item.Length + 1;

                if (index == 1)
                    length += ltReceiverAddressLine1.Text.Length;
                else if (index == 2)
                    length += ltReceiverAddressLine2.Text.Length;
                else
                    length += ltReceiverAddressLine3.Text.Length;

                if (length > maxLength)
                {
                    if ((length - 1) == maxLength)
                    {
                        hasSpace = false;
                    }
                    else
                    {
                        index += 1;
                        length = item.Length + 1;
                    }
                }

                if (index == 1)
                    ltReceiverAddressLine1.Text += item + (hasSpace ? " " : String.Empty);
                else if (index == 2)
                    ltReceiverAddressLine2.Text += item + (hasSpace ? " " : String.Empty);
                else if (index == 3)
                    ltReceiverAddressLine3.Text += item + (hasSpace ? " " : String.Empty);
                else
                    break;
            }

            if (index == 1)
            {
                ltReceiverAddressLine2.Text = "&nbsp;";
                ltReceiverAddressLine3.Text = "&nbsp;";
            }
            else if (index == 2)
            {
                ltReceiverAddressLine3.Text = "&nbsp;";
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
        private void _loadInvoice (OrderResponseModel data)
        {
            // Mã vận đơn
            ltJtCode.Text = data.code;
            // Barcode 
            ltBarcode.Text = "<img class='barcode-img' src='" + createBarcode(data.code) + "' />";
            // Mã đơn khách đặt tại shop
            ltOrderIdHeader.Text = data.orderId.ToString();
            // Ngày gửi hàng
            ltSentDate.Text = String.Format("{0:yyyy-MM-dd HH:mm:ss}", data.sentDate);
            // Tên người gửi (chủ shop)
            ltSenderName.Text = data.sender.name;
            // Tên người nhận
            ltReceiverName.Text = data.receiver.name;
            // Sđt người gửi
            string senderPhone = data.sender.phone;
            //senderPhone = senderPhone.Remove(0, 6).Insert(0, "******");
            ltSenderPhone.Text = senderPhone;
            // Sđt người nhận
            string receiverPhone = data.receiver.phone;
            //receiverPhone = receiverPhone.Remove(0, 6).Insert(0, "******");
            ltReceiverPhone.Text = receiverPhone;
            // Đại chỉ gửi
            _loadSenderAddress(data.sender);
            // Địa chỉ nhận
            _loadReceiverAddress(data.receiver);
            // Mã bưu cục
            ltPostalCode.Text = data.postalCode;
            // Mã đơn khách đặt tại shop
            //ltOrderIdBody.Text = data.orderId.ToString();
            // Số lượng kiện
            ltItemNumber.Text = data.item.number.ToString();
            // Mã chi nhánh bưu cục
            ltPostalBranchCodeLine1.Text = data.postalBranchCode.Substring(0, 3);
            ltPostalBranchCodeLine2.Text = data.postalBranchCode.Substring(3);
            // Nội dung
            ltItemName.Text = data.item.name;
            // COD
            ltCODvalue.Text = data.cod > 0 ? String.Format("{0:N0}", data.cod) : "&nbsp;";
            // Trọng lượng
            ltWeight.Text = String.Format("{0:0.0#}", data.weight);
            // Note
            ltNote.Text = data.note;
        }

        private void _loadData()
        {
            var orderId = 0;
            var code = String.Empty;

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                orderId = Convert.ToInt32(Request.QueryString["id"]);

            if (!String.IsNullOrEmpty(Request.QueryString["code"]))
                code = Request.QueryString["code"];

            if (orderId > 0 && !String.IsNullOrEmpty(code))
            {
                var jtOrder = _getJtOrder(orderId, code);

                if (jtOrder != null)
                    _loadInvoice(jtOrder);
                else
                    PJUtils.ShowMessageBoxSwAlertError("Không lấy được thông tin J&T Express của đơn hàng", "e", true, "/danh-sach-don-hang", Page);
            }
        }
        #endregion
    }
}