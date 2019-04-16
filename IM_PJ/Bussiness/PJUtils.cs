using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.IO;
using System.Web.Caching;
using System.Text;
using System.Web.Security;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Net;
using System.Xml;
using System.Security.Cryptography;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web.Script.Serialization;
using WebUI.Business;
using MB.Extensions;

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using static QRCoder.QRCodeGenerator;
using Supremes;
using IM_PJ.Controllers;
using QRCoder;
using IM_PJ.Bussiness;

namespace NHST.Bussiness
{
    public class PJUtils
    {
        public static string Truncate(string input, int length)
        {
            if (input == null || input.Length < length)
                return input;
            int iNextSpace = input.LastIndexOf(" ", length, StringComparison.Ordinal);
            return string.Format("{0}..", input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
        }
        public static string Encrypt(string key, string data)
        {
            data = data.Trim();
            byte[] keydata = Encoding.ASCII.GetBytes(key);
            string md5String = BitConverter.ToString(new
            MD5CryptoServiceProvider().ComputeHash(keydata)).Replace("-", "").ToLower();
            byte[] tripleDesKey = Encoding.ASCII.GetBytes(md5String.Substring(0, 24));
            TripleDES tripdes = TripleDESCryptoServiceProvider.Create();
            tripdes.Mode = CipherMode.ECB;
            tripdes.Key = tripleDesKey;
            tripdes.GenerateIV();
            MemoryStream ms = new MemoryStream();
            CryptoStream encStream = new CryptoStream(ms, tripdes.CreateEncryptor(),
            CryptoStreamMode.Write);
            encStream.Write(Encoding.ASCII.GetBytes(data), 0,
            Encoding.ASCII.GetByteCount(data));
            encStream.FlushFinalBlock();
            byte[] cryptoByte = ms.ToArray();
            ms.Close();
            encStream.Close();
            return Convert.ToBase64String(cryptoByte, 0, cryptoByte.GetLength(0)).Trim();
        }

        public static string Decrypt(string key, string data)
        {
            byte[] keydata = System.Text.Encoding.ASCII.GetBytes(key);
            string md5String = BitConverter.ToString(new
            MD5CryptoServiceProvider().ComputeHash(keydata)).Replace("-", "").ToLower();
            byte[] tripleDesKey = Encoding.ASCII.GetBytes(md5String.Substring(0, 24));
            TripleDES tripdes = TripleDESCryptoServiceProvider.Create();
            tripdes.Mode = CipherMode.ECB;
            tripdes.Key = tripleDesKey;
            byte[] cryptByte = Convert.FromBase64String(data);
            MemoryStream ms = new MemoryStream(cryptByte, 0, cryptByte.Length);
            ICryptoTransform cryptoTransform = tripdes.CreateDecryptor();
            CryptoStream decStream = new CryptoStream(ms, cryptoTransform,
            CryptoStreamMode.Read);
            StreamReader read = new StreamReader(decStream);
            return (read.ReadToEnd());
        }

        public static bool SendMail(string strFrom, string pass, string strTo, string strSubject, string strMsg, string cc)
        {
            try
            {
                // Create the mail message
                MailMessage objMailMsg = new MailMessage(strFrom, strTo);

                objMailMsg.BodyEncoding = Encoding.UTF8;
                objMailMsg.Subject = strSubject;
                objMailMsg.CC.Add(cc);
                objMailMsg.IsBodyHtml = true;
                objMailMsg.Body = strMsg;
                SmtpClient objSMTPClient = new SmtpClient();

                objSMTPClient.Host = "202.43.110.136";
                objSMTPClient.Port = 25;
                objSMTPClient.EnableSsl = false;
                objSMTPClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                objSMTPClient.Credentials = new NetworkCredential(strFrom, pass);
                objSMTPClient.Timeout = 20000;
                objSMTPClient.Send(objMailMsg);
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool SendMailGmail(string strFrom, string pass, string strTo, string strSubject, string strMsg, string cc)
        {
            try
            {
                string fromAddress = strFrom;
                string mailPassword = pass;       // Mail id password from where mail will be sent.
                string messageBody = strMsg;


                // Create smtp connection.
                SmtpClient client = new SmtpClient();
                client.Port = 587;//outgoing port for the mail.
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential(fromAddress, mailPassword);


                // Fill the mail form.
                var send_mail = new MailMessage();
                send_mail.IsBodyHtml = true;
                //address from where mail will be sent.
                send_mail.From = new MailAddress(strFrom);
                //address to which mail will be sent.           
                send_mail.To.Add(new MailAddress(strTo));
                //subject of the mail.
                send_mail.Subject = strSubject;
                send_mail.Body = messageBody;
                client.Send(send_mail);



                // Create the mail message
                //MailMessage objMailMsg = new MailMessage(strFrom, strTo);

                //objMailMsg.BodyEncoding = Encoding.UTF8;
                //objMailMsg.Subject = strSubject;
                ////objMailMsg.CC.Add(cc);
                //objMailMsg.IsBodyHtml = true;
                //objMailMsg.Body = strMsg;
                //SmtpClient objSMTPClient = new SmtpClient();

                //objSMTPClient.Host = "smtp.gmail.com";
                //objSMTPClient.Port = 587;
                //objSMTPClient.EnableSsl = true;
                //objSMTPClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                //objSMTPClient.Credentials = new NetworkCredential(strFrom, pass);
                //objSMTPClient.Timeout = 20000;
                //objSMTPClient.Send(objMailMsg);
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static void ExportToExcel(DataTable dt)
        {

            if (dt.Rows.Count > 0)
            {

                string filename = "ProjectReport_" + DateTime.Now.Date + ".xls";

                string excelHeader = "Project Report";

                System.IO.StringWriter tw = new System.IO.StringWriter();

                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

                DataGrid dgGrid = new DataGrid();

                dgGrid.DataSource = dt;

                dgGrid.DataBind();

                // Report Header
                hw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                hw.WriteLine("<b><u><font size='3'> " + excelHeader + " </font></u></b>");

                //Get the HTML for the control.

                dgGrid.RenderControl(hw);

                //Write the HTML back to the browser.

                //Response.ContentType = “application/vnd.ms-excel”;

                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");

                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                //this.EnableViewState = false;

                HttpContext.Current.Response.Write(tw.ToString());

                HttpContext.Current.Response.End();
            }
        }
        public static void ShowMsg(string txt, bool? isRefresh, System.Web.UI.Page page)
        {
            //isRefresh = isRefresh == null;
            var content = txt;
            var _type = string.Empty;
            switch (txt.Trim().ToLower())
            {
                case "100":
                    content = "Tên hoặc mã đã được sử dụng";
                    _type = "i";
                    isRefresh = false;
                    break;
                case "101":
                    content = "Không tìm thấy đối tượng";
                    _type = "i";
                    isRefresh = false;
                    break;
                case "102":
                    content = "Thực hiện thành công !";
                    _type = "";
                    isRefresh = true;
                    break;
                case "103":
                    content = "Thực hiện thất bại !";
                    _type = "e";
                    isRefresh = false;
                    break;
            }
            ShowMessageBoxSwAlert(content, _type, isRefresh, page);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="type">e: Error,i: warning, default: succes</param>
        /// <param name="isRefresh"></param>
        /// <param name="page"></param>
        /// 
        public static void ShowMessageBoxSwAlert(string txt, string type, bool? isRefresh, System.Web.UI.Page page)
        {
            txt = new JavaScriptSerializer().Serialize(txt);
            string p;
            switch (type)
            {

                case "e":
                    p = "error";
                    break;
                case "i":
                    p = "warning";
                    break;

                default:
                    p = "success";
                    break;
            }
            JavaScript.AfterPageLoad(page).ExecuteCustomScript("swal({ title: 'Thông báo',text:' " + txt + "', type: '" + p + "'}" + (Convert.ToBoolean(isRefresh.ToString()) ? ", function () { window.location.replace(window.location.href); });" : ");"));
        }
        public static void ShowMessageBoxSwAlertError(string txt, string type, bool? isRefresh, string url, System.Web.UI.Page page)
        {
            txt = new JavaScriptSerializer().Serialize(txt);
            string p;
            switch (type)
            {

                case "e":
                    p = "error";
                    break;
                case "i":
                    p = "info";
                    break;
                case "w":
                    p = "warning";
                    break;

                default:
                    p = "success";
                    break;
            }
            JavaScript.AfterPageLoad(page).ExecuteCustomScript("swal({ title: 'Thông báo',text:' " + txt + "', type: '" + p + "'}" + (Convert.ToBoolean(isRefresh.ToString()) ? ", function () { window.location.replace('" + url + "'); });" : ");"));
        }
        public static void ShowMessageBoxSwAlertCallFunction(string txt, string type, bool? isCall, string functionName, System.Web.UI.Page page)
        {
            txt = new JavaScriptSerializer().Serialize(txt);
            string p;
            switch (type)
            {

                case "e":
                    p = "error";
                    break;
                case "i":
                    p = "warning";
                    break;

                default:
                    p = "success";
                    break;
            }
            JavaScript.AfterPageLoad(page).ExecuteCustomScript("swal({ title: 'Thông báo',text:' " + txt + "',type: '" + p + "'}" + (Convert.ToBoolean(isCall.ToString()) ? ", function () { window.location.replace(window.location.href); " + functionName + "  });" : ");"));
        }
        public static void SwAlertCallFunction(string txt, string txtConfirm, string type, bool? isCall, string functionName, System.Web.UI.Page page)
        {
            txt = new JavaScriptSerializer().Serialize(txt);
            string p;
            switch (type)
            {

                case "e":
                    p = "error";
                    break;
                case "i":
                    p = "warning";
                    break;

                default:
                    p = "success";
                    break;
            }
            JavaScript.AfterPageLoad(page).ExecuteCustomScript("swal({ title: 'Thông báo', text:' " + txt + "', type: '" + p + "', showCancelButton: true, confirmButtonText:' " + txtConfirm + "', closeOnConfirm: true, html: true}" + (Convert.ToBoolean(isCall.ToString()) ? ", function () { " + functionName + " });" : ");"));
        }
        public static string GetIcon(object o)
        {
            if (o == null)
                return "/no_thumbnails.gif";
            if (!string.IsNullOrEmpty(o.ToString()))
                return o.ToString();
            return "/no_thumbnails.gif";
        }
        public static string SubString(string title, int length)
        {
            if (string.IsNullOrEmpty(title))
                return "";

            if (!title.Contains(" "))
            {
                if (title.Length > length)
                    title = title.Substring(0, length - 1) + "...";
            }
            else if (title.Length >= length)
            {
                int i = length - 1;
                while (title.Substring(i--, 1) != " " && i > 0) ;
                if (i == 0)
                    return title.Substring(0, length - 4) + " ...";
                else
                    return title.Substring(0, i + 1) + " ...";
            }

            return title;
        }
        public static string RandomString(int numberrandom)
        {
            //var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var chars = "0123456789";
            var stringChars = new char[numberrandom];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
        public static string RandomStringWithText(int numberrandom)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            //var chars = "0123456789";
            var stringChars = new char[numberrandom];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }

        public static bool ConvertStringToBool(string i)
        {
            i = i.ToLower();
            if (i == "1" || i == "true")
                return true;
            return false;
        }
        public static string StatusToRequest(object i)
        {
            if (i != null)
            {
                if (i.ToString() == "1")
                {
                    return "<span class='yellow'>Chưa kích hoạt</span>";
                }
                else if (i.ToString() == "2")
                {
                    return "<span class='blue'>Đã kích hoạt</span>";
                }
                else
                {
                    return "<span class='red'>Đang bị khóa</span>";
                }

            }
            else return "<span class='red'>Đang bị khóa</span>";
        }
        public static string IntToRequestAdmin(int i)
        {
            if (i == 0)
                return "<span class=\"bg-red\">Chưa đặt cọc</span>";
            else if (i == 1)
                return "<span class=\"bg-black\">Hủy đơn hàng</span>";
            else if (i == 2)
                return "<span class=\"bg-bronze\">Đã đặt cọc</span>";
            else if (i == 3)
                return "<span class=\"bg-green\">Chờ duyệt đơn</span>";
            else if (i == 4)
                return "<span class=\"bg-green\">Đã duyệt đơn</span>";
            else if (i == 5)
                return "<span class=\"bg-green\">Đã đặt hàng</span>";
            else if (i == 6)
                return "<span class=\"bg-green\">Đã nhận hàng tại TQ</span>";
            else if (i == 7)
                return "<span class=\"bg-green\">Đã nhận hàng tại VN</span>";
            else if (i == 8)
                return "<span class=\"bg-yellow\">Chờ thanh toán</span>";
            else if (i == 9)
                return "<span class=\"bg-blue\">Khách đã thanh toán</span>";
            else if (i == 10)
                return "<span class=\"bg-blue\">Khách đã nhận hàng</span>";
            else
                return "";

        }
        public static string OrderTypeStatus(int OrderType)
        {
            string ret = "";
            if (OrderType == 1)
            {
                ret = "<span class=\"bg-yellow\">Lẻ</span>";
            }
            else
            {
                ret = "<span class=\"bg-blue\">Sỉ</span>";

            }
            return ret;
        }
        public static string OrderPaymentStatus(int PaymentStatus)
        {
            string ret = "";
            if (PaymentStatus == 1)
            {
                ret = "<span class=\"bg-black\">Chưa thanh toán</span>";
            }
            else if (PaymentStatus == 2)
            {
                ret = "<span class=\"bg-red\">Thanh toán thiếu</span>";
            }
            else
            {
                ret = "<span class=\"bg-blue\">Đã thanh toán</span>";

            }
            return ret;
        }
        public static string PaymentType(int PaymentType)
        {
            string ret = "";
            if (PaymentType == 1)
            {
                ret = "<span class=\"bg-black\">Tiền mặt</span>";
            }
            else if (PaymentType == 2)
            {
                ret = "<span class=\"bg-red\">Chuyển khoản</span>";
            }
            else if (PaymentType == 3)
            {
                ret = "<span class=\"bg-yellow\">Thu hộ</span>";
            }
            else if (PaymentType == 4)
            {
                ret = "<span class=\"bg-blue\">Công nợ</span>";

            }
            else
            {
                ret = "<span class=\"bg-red\">Chưa xác định</span>";
            }
            return ret;
        }
        public static string ShippingType(int ShippingType)
        {
            string ret = "";
            if (ShippingType == 1)
            {
                ret = "<span class=\"bg-black\">Lấy trực tiếp</span>";
            }
            else if (ShippingType == 2)
            {
                ret = "<span class=\"bg-red\">Chuyển bưu điện</span>";
            }
            else if (ShippingType == 3)
            {
                ret = "<span class=\"bg-yellow\">Dịch vụ ship</span>";
            }
            else if (ShippingType == 4)
            {
                ret = "<span class=\"bg-blue\">Chuyển xe</span>";
            }
            else if (ShippingType == 5)
            {
                ret = "<span class=\"bg-bronze\">Nhân viên giao</span>";
            }
            else
            {
                ret = "<span class=\"bg-red\">Chưa xác định</span>";
            }
            return ret;
        }
        public static string OrderExcuteStatus(int ExcuteStatus)
        {
            string ret = "";
            if (ExcuteStatus == 1)
            {
                ret = "<span class=\"bg-yellow\">Đang xử lý</span>";
            }
            else if (ExcuteStatus == 2)
            {
                ret = "<span class=\"bg-green\">Đã hoàn tất</span>";
            }
            else if(ExcuteStatus == 3)
            {
                ret = "<span class=\"bg-red\">Đã hủy</span>";
            }
            else if(ExcuteStatus == 4)
            {
                ret = "<span class=\"bg-brown\">Chuyển hoàn</span>";
            }
            return ret;
        }

        public static string RefundStatus(int Status)
        {
            string ret = "";
            if (Status == 1)
            {
                ret = "<span class=\"bg-red\">Chưa trừ tiền</span>";
            }
            else
            {
                ret = "<span class=\"bg-green\">Đã trừ tiền</span>";

            }
            return ret;
        }
        //info order
        public static string OrderExcute(int ExcuteStatus)
        {
            string ret = "";
            if (ExcuteStatus == 1)
            {
                ret = "<span>Đang xử lý</span>";
            }
            else if (ExcuteStatus == 2)
            {
                ret = "<span>Đã hoàn tất</span>";
            }
            else if (ExcuteStatus == 3)
            {
                ret = "<span>Đã hủy</span>";
            }

            return ret;
        }

        public static string OrderType(int OrderType)
        {
            string ret = "";
            if (OrderType == 1)
            {
                ret = "<span>Đơn hàng lẻ</span>";
            }
            else
            {
                ret = "<span>Đơn hàng sỉ</span>";

            }
            return ret;
        }
        //end
        public static string IsHiddenStatus(bool IsHidden)
        {
            string ret = "";
            if (IsHidden == true)
            {
                ret = "<input type=\"checkbox\" disabled=\"disabled\" checked=\"checked\" />";
            }
            else
            {
                ret = "<input type=\"checkbox\" disabled=\"disabled\"/>";
            }
            return ret;
        }
        public static string IntToRequestClient(int i)
        {
            //if (i == 0)
            //    return "<span class=\"bg-red\">Chưa đặt cọc</span>";
            //else if (i == 1)
            //    return "<span class=\"bg-black\">Hủy đơn hàng</span>";
            //else if (i == 2)
            //    return "<span class=\"bg-bronze\">Đã đặt cọc</span>";
            //else if (i >= 3 && i < 8)
            //    return "<span class=\"bg-green\">Đang xử lý</span>";
            //else if (i == 8)
            //    return "<span class=\"bg-yellow\">Chờ thanh toán</span>";
            //else if (i == 9)
            //    return "<span class=\"bg-blue\">Đã xong</span>";
            //else if (i == 10)
            //    return "<span class=\"bg-blue\">Đã giao hàng</span>";
            //else
            //    return "";
            if (i == 0)
                return "<span class=\"bg-red\">Chưa đặt cọc</span>";
            else if (i == 1)
                return "<span class=\"bg-black\">Hủy đơn hàng</span>";
            else if (i == 2)
                return "<span class=\"bg-bronze\">Đã đặt cọc</span>";
            else if (i == 3)
                return "<span class=\"bg-green\">Chờ duyệt đơn</span>";
            else if (i == 4)
                return "<span class=\"bg-green\">Đã duyệt đơn</span>";
            else if (i == 5)
                return "<span class=\"bg-green\">Đã đặt hàng</span>";
            else if (i == 6)
                return "<span class=\"bg-green\">Đã nhận hàng tại TQ</span>";
            else if (i == 7)
                return "<span class=\"bg-green\">Đã nhận hàng tại VN</span>";
            else if (i == 8)
                return "<span class=\"bg-yellow\">Chờ thanh toán</span>";
            else if (i == 9)
                return "<span class=\"bg-blue\">Khách đã thanh toán</span>";
            else if (i == 10)
                return "<span class=\"bg-blue\">Khách đã nhận hàng</span>";
            else
                return "";


        }
        public static string BoolToRequest(object i)
        {
            if (i != null)
            {
                return ConvertStringToBool(i.ToString()) == true ? "<span class='red'>Đang yêu cầu</span>" : "<span class='blue'>Không</span>";
            }
            else return "<span class='blue'>Không</span>";
        }
        public static string ShowStatusPayHistory(int status)
        {
            if (status == 2)
                return "<span class=\"bg-bronze\">Đặt cọc</span>";
            else if (status == 3)
                return "<span class=\"bg-yellow\">Đặt cọc</span>";
            else if (status == 12)
                return "<span class=\"bg-red\">Sản phẩm hết hàng hoặc giảm giá trả lại cọc</span>";
            else
                return "<span class=\"bg-blue\">Thanh toán</span>";
        }

        public static string ShowStockStatus(int status)
        {
            if (status == 1)
                return "<span class=\"bg-green\">Còn hàng</span>";
            else if (status == 2)
                return "<span class=\"bg-red\">Hết hàng</span>";
            else
                return "<span class=\"bg-yellow\">Đang chờ nhập hàng</span>";
        }
        public static string BoolToStatus(string i)
        {
            return ConvertStringToBool(i) == true ? "<span class='show-stat-s'>Hiện</span>" : "<span class='show-stat-w'>Ẩn</span>";

        }
        public static string GetTradeType(int TradeType)
        {
            if (TradeType == 1)
            {
                return "Xác nhận đơn hàng";
            }
            else if (TradeType == 2)
            {
                return "Nhận lại tiền xác nhận đơn hàng";
            }
            else if (TradeType == 3)
            {
                return "Thanh toán đơn hàng";
            }
            else if (TradeType == 4)
            {
                return "Nạp tiền";
            }
            else if (TradeType == 5)
            {
                return "Rút tiền";
            }
            else if (TradeType == 6)
            {
                return "Hủy lệnh rút tiền";
            }
            else
            {
                return "...";
            }


        }
        public static string GetHistoryPayWalletTradeType(int TradeType)
        {
            if (TradeType == 1)
            {
                return "Đặt cọc";
            }
            else if (TradeType == 2)
            {
                return "Nhận lại tiền đặt cọc";
            }
            else if (TradeType == 3)
            {
                return "Thanh toán hóa đơn";
            }
            else if (TradeType == 4)
            {
                return "Nạp tiền";
            }
            else if (TradeType == 5)
            {
                return "Rút tiền";
            }
            else if (TradeType == 6)
            {
                return "Hủy lệnh rút tiền";
            }
            else if (TradeType == 7)
            {
                return "Nhận tiền khiếu nại đơn hàng";
            }
            else
            {
                return "...";
            }


        }
        public static string BoolToStatusShow(string i)
        {
            return ConvertStringToBool(i) == true ? "<span class='show-stat-w'>Ẩn</span>" : "<span class='show-stat-s'>Hiện</span>";

        }
        public static string ReturnStatusWithdraw(int status)
        {
            if (status == 1)
            {
                return "<span class='bg-red'>Đang chờ duyệt</span>";
            }
            else if (status == 2)
            {
                return "<span class='bg-blue'>Đã duyệt</span>";
            }
            else
            {
                return "Hủy lệnh";
            }
        }
        public static string ReturnRoleName(string name)
        {
            if (name == "Store")
            {
                return "<span class='yellow'>Cửa hàng</span>";
            }
            else if (name == "Customer")
            {
                return "<span class=''>Người dùng</span>";
            }
            return name;
        }
        public static string ReturnSymbol(int Type)
        {
            if (Type == 1)
            {
                return "-";
            }
            else
                return "+";
        }

        public static string ReturnStatusRequest(string status)
        {
            if (status == "1")
            {
                return "<span class='red'>Đang chờ</span>";
            }
            else if (status == "2")
            {
                return "<span class='blue'>Đã hoàn tất</span>";
            }
            else
            {
                return "<span class='orange'>Đã hủy</span>";
            }

        }
        public static string ReturnStatusOrder(int status)
        {
            if (status == 0)
            {
                return "<span class='bg-black'>Đã hủy</span>";
            }
            else if (status == 1)
            {
                return "<span class='bg-red'>Chưa đặt cọc</span>";
            }
            else if (status == 2)
            {
                return "<span class='bg-blue'>Đã xác nhận</span>";
            }
            else if (status == 3)
            {
                return "<span class='bg-orange'>Đang đặt hàng</span>";
            }
            else if (status == 4)
            {
                return "<span class='bg-yellow'>Đã đặt hàng</span>";
            }
            else if (status == 5)
            {
                return "<span class='bg-bronze'>Đã về kho Sài Gòn</span>";
            }
            else if (status == 6)
            {
                return "<span class='bg-bronze'>Yêu cầu giao hàng</span>";
            }
            else
            {
                return "<span class='bg-green'>Đã nhận hàng</span>";
            }
        }
        public static string ReturnStatusMovePro(int status)
        {
            if (status == 1)
            {
                return "<span class='bg-red'>Chưa chuyển</span>";
            }
            else if (status == 2)
            {
                return "<span class='bg-yellow'>Đã chuyển</span>";
            }
            else
            {
                return "<span class='bg-blue'>Đã hoàn tất</span>";
            }
        }
        public static List<countries> loadprefix()
        {
            string file = HttpContext.Current.Server.MapPath("~/Models/phonecode.json");
            //deserialize JSON from file  
            string Json = System.IO.File.ReadAllText(file);

            JavaScriptSerializer ser = new JavaScriptSerializer();
            var personlist = ser.Deserialize<List<countries>>(Json);
            List<countries> cs = new List<countries>();
            foreach (var item in personlist)
            {
                countries c = new countries();
                c.name = item.name;
                c.dial_code = item.dial_code;
                c.code = item.code;
                cs.Add(c);
            }
            return cs;
        }
        public class countries
        {
            public string name { get; set; }
            public string dial_code { get; set; }
            public string code { get; set; }
        }
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public static string IntToStringStatusPackage(int status)
        {
            if (status == 0)
                return "<span class=\"bg-bronze\">Mới tạo</span>";
            else if (status == 1)
                return "<span class=\"bg-green\">Đang chuyển về VN</span>";
            else if (status == 2)
                return "<span class=\"bg-blue\">Đã nhận hàng tại VN</span>";
            else
                return "<span class=\"bg-red\">Đã hủy</span>";
        }
        public static string IntToStringStatusSmallPackage(int status)
        {
            if (status == 1)
                return "Đã nhận hàng tại TQ";
            else if (status == 2)
                return "Đang chuyển về VN";
            else if (status == 3)
                return "Đã nhận hàng tại VN";
            else
                return "Đã giao cho khách";
        }
        public static string RemoveHTMLTags(string content)
        {
            var cleaned = string.Empty;
            try
            {
                StringBuilder textOnly = new StringBuilder();
                using (var reader = XmlNodeReader.Create(new System.IO.StringReader("<xml>" + content + "</xml>")))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Text)
                            textOnly.Append(reader.ReadContentAsString());
                    }
                }
                cleaned = textOnly.ToString();
            }
            catch
            {
                //A tag is probably not closed. fallback to regex string clean.
                string textOnly = string.Empty;
                Regex tagRemove = new Regex(@"<[^>]*(>|$)");
                Regex compressSpaces = new Regex(@"[\s\r\n]+");
                textOnly = tagRemove.Replace(content, string.Empty);
                textOnly = compressSpaces.Replace(textOnly, " ");
                cleaned = textOnly;
            }

            return cleaned;
        }
        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
            "đ",
            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
            "í","ì","ỉ","ĩ","ị",
            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
            "ý","ỳ","ỷ","ỹ","ỵ",};

            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
            "d",
            "e","e","e","e","e","e","e","e","e","e","e",
            "i","i","i","i","i",
            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
            "u","u","u","u","u","u","u","u","u","u","u",
            "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
        public static bool CheckUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
            "đ",
            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
            "í","ì","ỉ","ĩ","ị",
            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
            "ý","ỳ","ỷ","ỹ","ỵ",};
            bool check = false;
            for (int i = 0; i < arr1.Length; i++)
            {
                if (text.Contains(arr1[i]))
                    check = true;
            }
            return check;
        }

        public static void CreateBarcodeNew(string code)
        {
            var myBitmap = new Bitmap(500, 50);
            var g = Graphics.FromImage(myBitmap);
            var jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            g.Clear(Color.White);

            var strFormat = new StringFormat { Alignment = StringAlignment.Center };
            g.DrawString(code, new Font("Free 3 of 9", 50), Brushes.Black, new RectangleF(0, 0, 500, 50), strFormat);

            var myEncoder = System.Drawing.Imaging.Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);

            var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            myBitmap.Save(HttpContext.Current.Server.MapPath("~/uploads/Barcode.jpg"), jgpEncoder, myEncoderParameters);
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            var codecs = ImageCodecInfo.GetImageDecoders();

            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        public static Bitmap CreateBarcode1(string data)
        {
            Bitmap barCode = new Bitmap(1, 1);
            Font threeOfNine = new Font("Free 3 of 9", 60, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Graphics graphics = Graphics.FromImage(barCode);
            SizeF dataSize = graphics.MeasureString(data, threeOfNine);
            Bitmap barCode1 = new Bitmap(barCode, dataSize.ToSize());
            graphics = Graphics.FromImage(barCode1);
            graphics.Clear(Color.White);
            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            graphics.DrawString(data, threeOfNine, new SolidBrush(Color.Black), 0, 0);
            graphics.Flush();
            threeOfNine.Dispose();
            graphics.Dispose();
            barCode.Dispose();

            return barCode1;
        }

        public static Bitmap CreateBarcode2(string data)
        {
            Bitmap barCode = new Bitmap(1, 1);
            Font threeOfNine = new Font("Arial", 60, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Graphics graphics = Graphics.FromImage(barCode);
            SizeF dataSize = graphics.MeasureString(data, threeOfNine);
            Bitmap barCode1 = new Bitmap(barCode, dataSize.ToSize());
            graphics = Graphics.FromImage(barCode1);
            graphics.Clear(Color.White);
            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            graphics.DrawString(data, threeOfNine, new SolidBrush(Color.Black), 0, 0);
            graphics.Flush();
            threeOfNine.Dispose();
            graphics.Dispose();
            barCode.Dispose();

            return barCode1;
        }
        //barcode 128

        private const int CQuietWidth = 10;

        private static readonly int[,] CPatterns =
                                                   {
                                                     { 2, 1, 2, 2, 2, 2, 0, 0 }, // 0
                                                     { 2, 2, 2, 1, 2, 2, 0, 0 }, // 1
                                                     { 2, 2, 2, 2, 2, 1, 0, 0 }, // 2
                                                     { 1, 2, 1, 2, 2, 3, 0, 0 }, // 3
                                                     { 1, 2, 1, 3, 2, 2, 0, 0 }, // 4
                                                     { 1, 3, 1, 2, 2, 2, 0, 0 }, // 5
                                                     { 1, 2, 2, 2, 1, 3, 0, 0 }, // 6
                                                     { 1, 2, 2, 3, 1, 2, 0, 0 }, // 7
                                                     { 1, 3, 2, 2, 1, 2, 0, 0 }, // 8
                                                     { 2, 2, 1, 2, 1, 3, 0, 0 }, // 9
                                                     { 2, 2, 1, 3, 1, 2, 0, 0 }, // 10
                                                     { 2, 3, 1, 2, 1, 2, 0, 0 }, // 11
                                                     { 1, 1, 2, 2, 3, 2, 0, 0 }, // 12
                                                     { 1, 2, 2, 1, 3, 2, 0, 0 }, // 13
                                                     { 1, 2, 2, 2, 3, 1, 0, 0 }, // 14
                                                     { 1, 1, 3, 2, 2, 2, 0, 0 }, // 15
                                                     { 1, 2, 3, 1, 2, 2, 0, 0 }, // 16
                                                     { 1, 2, 3, 2, 2, 1, 0, 0 }, // 17
                                                     { 2, 2, 3, 2, 1, 1, 0, 0 }, // 18
                                                     { 2, 2, 1, 1, 3, 2, 0, 0 }, // 19
                                                     { 2, 2, 1, 2, 3, 1, 0, 0 }, // 20
                                                     { 2, 1, 3, 2, 1, 2, 0, 0 }, // 21
                                                     { 2, 2, 3, 1, 1, 2, 0, 0 }, // 22
                                                     { 3, 1, 2, 1, 3, 1, 0, 0 }, // 23
                                                     { 3, 1, 1, 2, 2, 2, 0, 0 }, // 24
                                                     { 3, 2, 1, 1, 2, 2, 0, 0 }, // 25
                                                     { 3, 2, 1, 2, 2, 1, 0, 0 }, // 26
                                                     { 3, 1, 2, 2, 1, 2, 0, 0 }, // 27
                                                     { 3, 2, 2, 1, 1, 2, 0, 0 }, // 28
                                                     { 3, 2, 2, 2, 1, 1, 0, 0 }, // 29
                                                     { 2, 1, 2, 1, 2, 3, 0, 0 }, // 30
                                                     { 2, 1, 2, 3, 2, 1, 0, 0 }, // 31
                                                     { 2, 3, 2, 1, 2, 1, 0, 0 }, // 32
                                                     { 1, 1, 1, 3, 2, 3, 0, 0 }, // 33
                                                     { 1, 3, 1, 1, 2, 3, 0, 0 }, // 34
                                                     { 1, 3, 1, 3, 2, 1, 0, 0 }, // 35
                                                     { 1, 1, 2, 3, 1, 3, 0, 0 }, // 36
                                                     { 1, 3, 2, 1, 1, 3, 0, 0 }, // 37
                                                     { 1, 3, 2, 3, 1, 1, 0, 0 }, // 38
                                                     { 2, 1, 1, 3, 1, 3, 0, 0 }, // 39
                                                     { 2, 3, 1, 1, 1, 3, 0, 0 }, // 40
                                                     { 2, 3, 1, 3, 1, 1, 0, 0 }, // 41
                                                     { 1, 1, 2, 1, 3, 3, 0, 0 }, // 42
                                                     { 1, 1, 2, 3, 3, 1, 0, 0 }, // 43
                                                     { 1, 3, 2, 1, 3, 1, 0, 0 }, // 44
                                                     { 1, 1, 3, 1, 2, 3, 0, 0 }, // 45
                                                     { 1, 1, 3, 3, 2, 1, 0, 0 }, // 46
                                                     { 1, 3, 3, 1, 2, 1, 0, 0 }, // 47
                                                     { 3, 1, 3, 1, 2, 1, 0, 0 }, // 48
                                                     { 2, 1, 1, 3, 3, 1, 0, 0 }, // 49
                                                     { 2, 3, 1, 1, 3, 1, 0, 0 }, // 50
                                                     { 2, 1, 3, 1, 1, 3, 0, 0 }, // 51
                                                     { 2, 1, 3, 3, 1, 1, 0, 0 }, // 52
                                                     { 2, 1, 3, 1, 3, 1, 0, 0 }, // 53
                                                     { 3, 1, 1, 1, 2, 3, 0, 0 }, // 54
                                                     { 3, 1, 1, 3, 2, 1, 0, 0 }, // 55
                                                     { 3, 3, 1, 1, 2, 1, 0, 0 }, // 56
                                                     { 3, 1, 2, 1, 1, 3, 0, 0 }, // 57
                                                     { 3, 1, 2, 3, 1, 1, 0, 0 }, // 58
                                                     { 3, 3, 2, 1, 1, 1, 0, 0 }, // 59
                                                     { 3, 1, 4, 1, 1, 1, 0, 0 }, // 60
                                                     { 2, 2, 1, 4, 1, 1, 0, 0 }, // 61
                                                     { 4, 3, 1, 1, 1, 1, 0, 0 }, // 62
                                                     { 1, 1, 1, 2, 2, 4, 0, 0 }, // 63
                                                     { 1, 1, 1, 4, 2, 2, 0, 0 }, // 64
                                                     { 1, 2, 1, 1, 2, 4, 0, 0 }, // 65
                                                     { 1, 2, 1, 4, 2, 1, 0, 0 }, // 66
                                                     { 1, 4, 1, 1, 2, 2, 0, 0 }, // 67
                                                     { 1, 4, 1, 2, 2, 1, 0, 0 }, // 68
                                                     { 1, 1, 2, 2, 1, 4, 0, 0 }, // 69
                                                     { 1, 1, 2, 4, 1, 2, 0, 0 }, // 70
                                                     { 1, 2, 2, 1, 1, 4, 0, 0 }, // 71
                                                     { 1, 2, 2, 4, 1, 1, 0, 0 }, // 72
                                                     { 1, 4, 2, 1, 1, 2, 0, 0 }, // 73
                                                     { 1, 4, 2, 2, 1, 1, 0, 0 }, // 74
                                                     { 2, 4, 1, 2, 1, 1, 0, 0 }, // 75
                                                     { 2, 2, 1, 1, 1, 4, 0, 0 }, // 76
                                                     { 4, 1, 3, 1, 1, 1, 0, 0 }, // 77
                                                     { 2, 4, 1, 1, 1, 2, 0, 0 }, // 78
                                                     { 1, 3, 4, 1, 1, 1, 0, 0 }, // 79
                                                     { 1, 1, 1, 2, 4, 2, 0, 0 }, // 80
                                                     { 1, 2, 1, 1, 4, 2, 0, 0 }, // 81
                                                     { 1, 2, 1, 2, 4, 1, 0, 0 }, // 82
                                                     { 1, 1, 4, 2, 1, 2, 0, 0 }, // 83
                                                     { 1, 2, 4, 1, 1, 2, 0, 0 }, // 84
                                                     { 1, 2, 4, 2, 1, 1, 0, 0 }, // 85
                                                     { 4, 1, 1, 2, 1, 2, 0, 0 }, // 86
                                                     { 4, 2, 1, 1, 1, 2, 0, 0 }, // 87
                                                     { 4, 2, 1, 2, 1, 1, 0, 0 }, // 88
                                                     { 2, 1, 2, 1, 4, 1, 0, 0 }, // 89
                                                     { 2, 1, 4, 1, 2, 1, 0, 0 }, // 90
                                                     { 4, 1, 2, 1, 2, 1, 0, 0 }, // 91
                                                     { 1, 1, 1, 1, 4, 3, 0, 0 }, // 92
                                                     { 1, 1, 1, 3, 4, 1, 0, 0 }, // 93
                                                     { 1, 3, 1, 1, 4, 1, 0, 0 }, // 94
                                                     { 1, 1, 4, 1, 1, 3, 0, 0 }, // 95
                                                     { 1, 1, 4, 3, 1, 1, 0, 0 }, // 96
                                                     { 4, 1, 1, 1, 1, 3, 0, 0 }, // 97
                                                     { 4, 1, 1, 3, 1, 1, 0, 0 }, // 98
                                                     { 1, 1, 3, 1, 4, 1, 0, 0 }, // 99
                                                     { 1, 1, 4, 1, 3, 1, 0, 0 }, // 100
                                                     { 3, 1, 1, 1, 4, 1, 0, 0 }, // 101
                                                     { 4, 1, 1, 1, 3, 1, 0, 0 }, // 102
                                                     { 2, 1, 1, 4, 1, 2, 0, 0 }, // 103
                                                     { 2, 1, 1, 2, 1, 4, 0, 0 }, // 104
                                                     { 2, 1, 1, 2, 3, 2, 0, 0 }, // 105
                                                     { 2, 3, 3, 1, 1, 1, 2, 0 } // 106
                                                   };

      
        public static System.Drawing.Image MakeBarcodeImage(string inputData, int barWeight, bool addQuietZone)
        {
            // get the Code128 codes to represent the message
            var content = new Code128Content(inputData);
            var codes = content.Codes;

            var width = (((codes.Length - 3) * 11) + 35) * barWeight;
            var height = Convert.ToInt32(Math.Ceiling(Convert.ToSingle(width) * .15F));

            if (addQuietZone)
            {
                width += 2 * CQuietWidth * barWeight; // on both sides
            }

            // get surface to draw on
            System.Drawing.Image myImage = new Bitmap(width, height);
            using (var gr = Graphics.FromImage(myImage))
            {
                // set to white so we don't have to fill the spaces with white
                gr.FillRectangle(Brushes.White, 0, 0, width, height);

                // skip quiet zone
                var cursor = addQuietZone ? CQuietWidth * barWeight : 0;

                for (var codeIdx = 0; codeIdx < codes.Length; codeIdx++)
                {
                    var code = codes[codeIdx];

                    // take the bars two at a time: a black and a white
                    for (var bar = 0; bar < 8; bar += 2)
                    {
                        var barWidth = CPatterns[code, bar] * barWeight;
                        var spcWidth = CPatterns[code, bar + 1] * barWeight;

                        // if width is zero, don't try to draw it
                        if (barWidth > 0)
                        {
                            gr.FillRectangle(Brushes.Black, cursor, 0, barWidth, height);
                        }

                        // note that we never need to draw the space, since we 
                        // initialized the graphics to all white

                        // advance cursor beyond this pair
                        cursor += barWidth + spcWidth;
                    }
                }
            }

            return myImage;
        }

        public static string GenQRCode(string code)
        {
            string IMG = "/uploads/QRCode/" + code + ".jpg";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            Bitmap bitmap = qrCode.GetGraphic(20);
            bitmap.Save(HttpContext.Current.Server.MapPath("~" + IMG), System.Drawing.Imaging.ImageFormat.Png);

            return IMG;
        }
        public static void CreateBarCode(string barCodedDetail, string fontName, int fontSize, string physicalPath)
        {

            //Find the Width for barcode
            int width = barCodedDetail.Length * 35;

            //create Bitmap object with Width and Height
            Bitmap barCode = new Bitmap(width, 120);

            //path where you want to save the barcode Image
            string filePath = string.Format("{0}{1}.png", physicalPath, barCodedDetail);

            //create the barcoded font object
            Font barCodeFont = new Font(fontName, fontSize, FontStyle.Regular, GraphicsUnit.Point);

            //creating the graphics object for the Bitmap.
            Graphics graphics = Graphics.FromImage(barCode);

            SizeF sizeF = graphics.MeasureString(barCodedDetail, barCodeFont);

            barCode = new Bitmap(barCode, sizeF.ToSize());

            graphics = Graphics.FromImage(barCode);

            SolidBrush brushBlack = new SolidBrush(Color.Black);

            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;

            //putting * before and after the barCodedDetail,
            //this is because scanner only read the data which is started and end with *
            graphics.DrawString("*" + barCodedDetail + "*", barCodeFont, brushBlack, 1, 1);

            graphics.Dispose();
            //Saving the Image file
            barCode.Save(filePath, ImageFormat.Png);
            barCode.Dispose();
            HttpContext.Current.Response.Clear();

        }
        public static string TranslateText(string input, string languagePair)
        {
            string url = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", input, languagePair);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
            request.Method = "GET";
            var content = String.Empty;
            HttpStatusCode statusCode;
            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                var contentType = response.ContentType;
                Encoding encoding = null;
                if (contentType != null)
                {
                    var match = Regex.Match(contentType, @"(?<=charset\=).*");
                    if (match.Success)
                        encoding = Encoding.GetEncoding(match.ToString());
                }

                encoding = encoding ?? Encoding.UTF8;

                statusCode = ((HttpWebResponse)response).StatusCode;
                using (var reader = new StreamReader(stream, encoding))
                    content = reader.ReadToEnd();
            }
            var doc = Dcsoup.Parse(content);
            var scoreDiv = doc.Select("html").Select("span[id=result_box]").Html;
            return scoreDiv;
        }
        public static string GenBarCode(string Code, string physicalPath)
        {
            string filePath = string.Format("{0}{1}.jpg", physicalPath, Code);
            // Multiply the lenght of the code by 40 (just to have enough width)
            int w = Code.Length * 40;

            // Create a bitmap object of the width that we calculated and height of 100
            Bitmap oBitmap = new Bitmap(w, 100);

            // then create a Graphic object for the bitmap we just created.
            Graphics oGraphics = Graphics.FromImage(oBitmap);

            // Now create a Font object for the Barcode Font
            // (in this case the IDAutomationHC39M) of 18 point size
            Font oFont = new Font("IDAutomationHC39M", 18);

            // Let's create the Point and Brushes for the barcode
            PointF oPoint = new PointF(2f, 2f);
            SolidBrush oBrushWrite = new SolidBrush(Color.Black);
            SolidBrush oBrush = new SolidBrush(Color.White);

            // Now lets create the actual barcode image
            // with a rectangle filled with white color
            oGraphics.FillRectangle(oBrush, 0, 0, w, 100);

            // We have to put prefix and sufix of an asterisk (*),
            // in order to be a valid barcode
            oGraphics.DrawString("*" + Code + "*", oFont, oBrushWrite, oPoint);

            // Then we send the Graphics with the actual barcode
            HttpContext.Current.Response.ContentType = "image/jpeg";
            oBitmap.Save(filePath, ImageFormat.Jpeg);
            oBitmap.Dispose();
            HttpContext.Current.Response.Clear();
            return filePath;
        }
        public static double ProductQuantityInstock(int AgentID, string SKU)
        {
            double currentQuantity = 0;
            var ps = StockManagerController.GetBySKU(AgentID, SKU);
            if (ps.Count > 0)
            {
                double quantity_pIn = 0;

                var ps_in = ps.Where(p => p.Type == 1).ToList();
                if (ps_in.Count > 0)
                {
                    foreach (var p in ps_in)
                    {
                        quantity_pIn += Convert.ToDouble(p.Quantity);
                    }
                }
                currentQuantity = quantity_pIn;
            }
            return currentQuantity;
        }
        public static double ProductQuantityOutstock(int AgentID, string SKU)
        {
            double currentQuantity = 0;
            var ps = StockManagerController.GetBySKU(AgentID, SKU);
            if (ps.Count > 0)
            {
                double quantity_pOut = 0;
                var ps_out = ps.Where(p => p.Type == 2).ToList();
                if (ps_out.Count > 0)
                {
                    foreach (var p in ps_out)
                    {
                        quantity_pOut += Convert.ToDouble(p.Quantity);
                    }
                }
                currentQuantity = quantity_pOut;
            }
            return currentQuantity;
        }
        public static double TotalProductQuantityInstock(int AgentID, string SKU)
        {
            double currentQuantity = 0;
            var ps = StockManagerController.GetBySKU(AgentID, SKU).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

            if (ps != null)
            {
                double quantity = 0;
                double quantityCurrent = 0;

                if (ps.Quantity.HasValue)
                {
                    quantity = ps.Quantity.Value;
                }

                if (ps.QuantityCurrent.HasValue)
                {
                    quantityCurrent = ps.QuantityCurrent.Value;
                }

                switch (ps.Type)
                {
                    case 1:
                        currentQuantity = quantityCurrent + quantity;
                        break;
                    case 2:
                        currentQuantity = quantityCurrent - quantity;
                        break;
                    default:
                        currentQuantity = 0;
                        break;
                }
            }

            return currentQuantity;
        }

        public static double GetSotckProduct(int AgentID, string SKU)
        {
            double currentQuantity = 0;
            var ps = StockManagerController.GetBySKU(AgentID, SKU).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

            if (ps != null)
            {
                double quantity = 0;
                double quantityCurrent = 0;

                if (ps.Quantity.HasValue)
                {
                    quantity = ps.Quantity.Value;
                }

                if (ps.QuantityCurrent.HasValue)
                {
                    quantityCurrent = ps.QuantityCurrent.Value;
                }

                switch (ps.Type)
                {
                    case 1:
                        currentQuantity = quantityCurrent + quantity;
                        break;
                    case 2:
                        currentQuantity = quantityCurrent - quantity;
                        break;
                    default:
                        currentQuantity = 0;
                        break;
                }
            }
            else
            {
                currentQuantity = -1;
            }

            return currentQuantity;
        }

        public static string StockStatusBySKU(int AgentID, string SKU)
        {
            double currentQuantity = 0;
            var ps = StockManagerController.GetBySKU(AgentID, SKU).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

            if (ps != null)
            {
                double quantity = 0;
                double quantityCurrent = 0;

                if (ps.Quantity.HasValue)
                {
                    quantity = ps.Quantity.Value;
                }

                if (ps.QuantityCurrent.HasValue)
                {
                    quantityCurrent = ps.QuantityCurrent.Value;
                }

                switch (ps.Type)
                {
                    case 1:
                        currentQuantity = quantityCurrent + quantity;
                        break;
                    case 2:
                        currentQuantity = quantityCurrent - quantity;
                        break;
                    default:
                        currentQuantity = 0;
                        break;
                }
                if (currentQuantity > 0)
                    return "<span class=\"bg-green\">Còn hàng</span>";
                else
                    return "<span class=\"bg-red\">Hết hàng</span>";
            }
            else
            {
                return "<span class=\"bg-yellow\">Nhập hàng</span>";
            }
            
        }

        public static double TotalProductQuantityInstockBySKU(string SKU)
        {
            double currentQuantity = 0;
            var ps = StockManagerController.GetBySKU(SKU);
            if (ps.Count > 0)
            {
                double quantity_pIn = 0;
                double quantity_pOut = 0;

                var ps_in = ps.Where(p => p.Type == 1).ToList();
                if (ps_in.Count > 0)
                {
                    foreach (var p in ps_in)
                    {
                        quantity_pIn += Convert.ToDouble(p.Quantity);
                    }
                }
                var ps_out = ps.Where(p => p.Type == 2).ToList();
                if (ps_out.Count > 0)
                {
                    foreach (var p in ps_out)
                    {
                        quantity_pOut += Convert.ToDouble(p.Quantity);
                    }
                }
                if (quantity_pIn > quantity_pOut)
                {
                    currentQuantity = quantity_pIn - quantity_pOut;
                }
            }
            return currentQuantity;
        }

        public static string DeliveryStatus(int status)
        {
            switch (status)
            {
                case 1:
                    return String.Format("<span class='bg-green'>Đã giao</span>");
                case 2:
                    return String.Format("<span class='bg-red'>Chưa giao</span>");
                case 3:
                    return String.Format("<span class='bg-blue'>Đang giao</span>");
                default:
                    return String.Empty;
            }
        }
    }

}