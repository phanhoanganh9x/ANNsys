using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using Newtonsoft.Json;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static IM_PJ.Controllers.CustomerController;

namespace IM_PJ
{
    public partial class danh_sach_khach_hang : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                    var acc = AccountController.GetByUsername(username);
                    int agent = acc.AgentID.ToString().ToInt();

                    if (acc != null)
                    {
                        if (acc.RoleID == 0)
                        {
                            LoadCreatedBy(agent);
                        }
                        else if (acc.RoleID == 2)
                        {
                            LoadCreatedBy(agent, acc);
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
                LoadProvince();
            }
        }

        public void LoadProvince()
        {
            var pro = ProvinceController.GetAll();
            ddlProvince.Items.Clear();
            ddlProvince.Items.Insert(0, new ListItem("Tỉnh thành", "0"));
            if (pro.Count > 0)
            {
                foreach (var p in pro)
                {
                    ListItem listitem = new ListItem(p.Name, p.ID.ToString());
                    ddlProvince.Items.Add(listitem);
                }
                ddlProvince.DataBind();
            }
        }
        public void LoadCreatedBy(int AgentID, tbl_Account acc = null)
        {
            if (acc != null)
            {
                ddlCreatedBy.Items.Clear();
                ddlCreatedBy.Items.Insert(0, new ListItem(acc.Username, acc.Username));
            }
            else
            {
                var CreateBy = AccountController.GetAllNotSearch();
                ddlCreatedBy.Items.Clear();
                ddlCreatedBy.Items.Insert(0, new ListItem("Nhân viên phụ trách", ""));
                if (CreateBy.Count > 0)
                {
                    foreach (var p in CreateBy)
                    {
                        ListItem listitem = new ListItem(p.Username, p.Username);
                        ddlCreatedBy.Items.Add(listitem);
                    }
                    ddlCreatedBy.DataBind();
                }
            }

        }
        public void LoadData()
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                DateTime year = new DateTime(2018, 6, 22);

                var config = ConfigController.GetByTop1();
                if (config.ViewAllReports == 0)
                {
                    year = DateTime.Now.AddMonths(-2);
                }

                DateTime DateConfig = year;

                DateTime FromDate = DateConfig;
                DateTime ToDate = DateTime.Now;

                if (!String.IsNullOrEmpty(Request.QueryString["fromdate"]))
                {
                    FromDate = Convert.ToDateTime(Request.QueryString["fromdate"]);
                }

                if (!String.IsNullOrEmpty(Request.QueryString["todate"]))
                {
                    ToDate = Convert.ToDateTime(Request.QueryString["todate"]).AddDays(1).AddMinutes(-1);
                }

                rFromDate.SelectedDate = FromDate;
                rFromDate.MinDate = DateConfig;
                rFromDate.MaxDate = DateTime.Now;

                rToDate.SelectedDate = ToDate;
                rToDate.MinDate = DateConfig;
                rToDate.MaxDate = DateTime.Now;

                string TextSearch = "";
                int Province = 0;
                string CreatedBy = "";
                string Sort = "";

                if (Request.QueryString["textsearch"] != null)
                {
                    TextSearch = Request.QueryString["textsearch"].Trim();
                }
                if (Request.QueryString["createdby"] != null)
                {
                    CreatedBy = Request.QueryString["createdby"];
                }
                if (Request.QueryString["province"] != null)
                {
                    Province = Request.QueryString["province"].ToInt();
                }
                if (Request.QueryString["sort"] != null)
                {
                    Sort = Request.QueryString["sort"];
                }

                txtTextSearch.Text = TextSearch;
                ddlProvince.SelectedValue = Province.ToString();
                ddlCreatedBy.SelectedValue = CreatedBy.ToString();
                ddlSort.SelectedValue = Sort.ToString();

                List<CustomerOut> rs = new List<CustomerOut>();

                rs = CustomerController.Filter(TextSearch, CreatedBy, Province, Sort, FromDate, ToDate);

                if (acc.RoleID != 0 && acc.Username != "nhom_zalo502")
                {
                    rs = rs.Where(x => x.CreatedBy == acc.Username).ToList();
                    ddlCreatedBy.Enabled = false;
                    ddlCreatedBy.Visible = false;
                }

                pagingall(rs);

                ltrNumberOfCustomer.Text = rs.Count().ToString();
            }
        }
        [WebMethod]
        public static string sendSMSIntroAPP(string customerPhone)
        {
            string customerPhoneWith84 = "84" + customerPhone.Substring(1, 9);

            string error = String.Empty;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://xuongann.com/api/sms/intro-app");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    phone = customerPhoneWith84
                });

                streamWriter.Write(json);
            }

            try
            {
                
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    CustomerController.UpdateSendSMSIntroApp(customerPhone);
                    return "true";
                }
                else
                    return "Đã có lỗi xảy ra bên api http://xuongann.com/api/sms/intro-app (Status Code != 200(OK))";
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    var httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);

                    if (httpResponse.StatusCode == HttpStatusCode.BadRequest)
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            var respone = JsonConvert.DeserializeObject<IntroAppRespondErrorModel>(reader.ReadToEnd());

                            if (respone == null)
                            {
                                error = "Đã có lỗi trong quá trình convert giá trị JSON API SMS trả về";
                                return error;
                            }

                            return respone.Message;
                        }
                    else
                        return "Đã có lỗi xảy ra bên api http://xuongann.com/api/sms/intro-app  (Status Code != 400(BadRequest))";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public class IntroAppRespondErrorModel
        {
            public string Message { get; set; }
        }

        public class SMSBrandNameSendMessageModel
        {
            public string to { get; set; }
            public string telco { get; set; }
            public string orderCode { get; set; }
            public string packageCode { get; set; }
            public int type { get; set; }
            public string from { get; set; }
            public string message { get; set; }
            public string scheduled { get; set; }
            public string requestId { get; set; }
            public int useUnicode { get; set; }
            public object ext { get; set; }
        }
        [WebMethod]
        public static string generateCouponG25(int customerID)
        {
            var coupon = CouponController.getByName("G25");
            if (coupon != null)
            {
                //generate coupon for customer
                var customerCoupon = CouponController.insertCustomerCoupon(customerID, coupon.ID);
                if (customerCoupon != null)
                {
                    return "true";
                }
            }

            return "false";
        }
        [WebMethod]
        public static string checkCouponG25(int customerID)
        {
            // check customer
            var customer = CustomerController.GetByID(customerID);
            if (customer == null)
            {
                return "customerNotFound";
            }

            // check coupon
            var coupon = CouponController.getByName("G25");
            if (coupon == null)
            {
                return "couponNotFound";
            }
            else
            {
                if (coupon.Active == false)
                {
                    return "couponNotActived";
                }
            }

            // check user app
            var userPhone = UserController.getByPhone(customer.CustomerPhone);
            var userPhone2 = UserController.getByPhone(customer.CustomerPhone2);
            if (userPhone == null && userPhone2 == null)
            {
                return "userNotFound";
            }

            // kiểm tra đã tạo mã cho khách này chưa
            var customerCoupon = CouponController.getCouponByCustomer(coupon.ID, customer.ID, customer.CustomerPhone);
            if (customerCoupon.Count == 0)
            {
                return "noCouponGeneratedYet";
            }

            // kiểm tra mã đã tạo còn cái nào actice không
            int activeCoupon = customerCoupon.Where(x => x.Active == true).Count();
            if (activeCoupon > 0)
            {
                return "activeCouponExists";
            }

            // kiểm tra khách đã sử dụng bao nhiêu mã
            int inactiveCoupon = customerCoupon.Where(x => x.Active == false).Count();
            if (inactiveCoupon == 1)
            {
                return "couponUsageLimitExceeded";
            }

            return "false";
        }

        [WebMethod]
        public static string generateCouponG15(int customerID, bool checkUser = false)
        {
            // check customer
            var customer = CustomerController.GetByID(customerID);
            if (customer == null)
            {
                return "customerNotFound";
            }

            // check coupon
            var coupon = CouponController.getByName("G15");
            if (coupon == null)
            {
                return "couponNotFound";
            }
            else
            {
                if (coupon.Active == false)
                {
                    return "couponNotActived";
                }
            }

            // check user app
            if (checkUser == true)
            {
                var userPhone = UserController.getByPhone(customer.CustomerPhone);
                var userPhone2 = UserController.getByPhone(customer.CustomerPhone2);
                if (userPhone == null && userPhone2 == null)
                {
                    return "userNotFound";
                }
            }

            //generate coupon for customer
            var customerCoupon = CouponController.insertCustomerCoupon(customerID, coupon.ID);
            if (customerCoupon != null)
            {
                return "true";
            }

            return "false";
        }

        #region Paging
        public void pagingall(List<CustomerOut> acs)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

            int PageSize = 30;
            StringBuilder html = new StringBuilder();
            html.Append("<tr>");
            html.Append("     <th class='name-column'>Họ tên</th>");
            html.Append("     <th class='nick-column'>Nick đặt hàng</th>");
            html.Append("     <th class='phone-column'>Điện thoại</th>");
            html.Append("     <th class='zalo-column'>App</th>");
            html.Append("     <th class='facebook-column'>FB</th>");
            html.Append("     <th class='province-column'>Tỉnh</th>");
            html.Append("     <th class='buy-column'>Đơn</th>");
            html.Append("     <th class='buy-column'>Mua</th>");
            if (acc.RoleID == 0)
            {
                html.Append("     <th class='staff-column'>Nhân viên</th>");
            }
            html.Append("     <th class='date-column'>Ngày tạo</th>");
            html.Append("     <th class='action-column'></th>");
            html.Append("</tr>");

            if (acs.Count > 0)
            {
                int TotalItems = acs.Count;
                if (TotalItems % PageSize == 0)
                    PageCount = TotalItems / PageSize;
                else
                    PageCount = TotalItems / PageSize + 1;

                Int32 Page = GetIntFromQueryString("Page");

                if (Page == -1) Page = 1;
                int FromRow = (Page - 1) * PageSize;
                int ToRow = Page * PageSize - 1;
                if (ToRow >= TotalItems)
                    ToRow = TotalItems - 1;
                for (int i = FromRow; i < ToRow + 1; i++)
                {
                    var item = acs[i];
                    html.Append("<tr>");

                    html.Append("   <td class='customer-name-link'><a href='/chi-tiet-khach-hang?id=" + item.ID + "'>" + item.CustomerName.ToLower().ToTitleCase() + "</a></td>");
                    html.Append("   <td class='customer-name-link'>" + item.Nick + "</td>");
                    html.Append("   <td>" + item.CustomerPhone + "</td>");

                    string inApp = "";
                    if (item.InApp == false)
                    {
                        inApp = "Không";
                    }
                    html.Append("   <td>" + inApp + "</td>");

                    if (!string.IsNullOrEmpty(item.Facebook))
                    {
                        html.Append("   <td><a class='link' href='" + item.Facebook + "' target='_blank'>Xem</a></td>");
                    }
                    else
                    {
                        html.Append("   <td></td>");
                    }

                    if (!string.IsNullOrEmpty(item.ProvinceName))
                    {
                        html.Append("   <td>" + item.ProvinceName + "</td>");
                    }
                    else
                    {
                        html.Append("   <td></td>");
                    }

                    if (item.TotalOrder > 0)
                    {
                        html.Append("   <td>" + item.TotalOrder + " đơn</td>");
                    }
                    else
                    {
                        html.Append("   <td></td>");
                    }

                    if (item.TotalQuantity > 0)
                    {
                        html.Append("   <td>" + item.TotalQuantity + " cái</td>");
                    }
                    else
                    {
                        html.Append("   <td></td>");
                    }

                    if (acc.RoleID == 0)
                    {
                        html.Append("   <td>" + item.CreatedBy + "</td>");
                    }

                    html.Append("   <td>" + string.Format("{0:dd/MM/yyyy}", item.CreatedDate) + "</td>");

                    string ishidden = "";
                    if (item.IsHidden != null)
                    {
                        bool IsHidden = Convert.ToBoolean(item.IsHidden);
                        ishidden = PJUtils.IsHiddenStatus(IsHidden);
                    }
                    else
                    {
                        ishidden = PJUtils.IsHiddenStatus(false);
                    }

                    html.Append("   <td>");
                    html.Append("       <a href='/danh-sach-don-hang?searchtype=1&textsearch=" + item.CustomerPhone + "' title='Xem đơn hàng' class='btn primary-btn h45-btn'><i class='fa fa-shopping-cart' aria-hidden='true'></i></a>");
                    html.Append("       <a href='/thong-ke-khach-hang?textsearch=" + item.CustomerPhone + "' title='Xem thống kê khách hàng' class='btn primary-btn btn-blue h45-btn' target='_blank'><i class='fa fa-line-chart' aria-hidden='true'></i></a>");
                    if (item.InApp == false && item.SendSMSIntroApp == 0)
                    {
                        string[] viettel = { "086", "096", "097", "098", "032", "033", "034", "035", "036", "037", "038", "039" };
                        string[] vinaphone = { "088", "091", "094", "081", "082", "083", "084", "085" };
                        
                        if (viettel.Any(item.CustomerPhone.StartsWith) || vinaphone.Any(item.CustomerPhone.StartsWith))
                        {
                            
                            html.Append("       <a href='javascript:;' onclick='sendSMSIntroAPP($(this), `" + item.CustomerPhone + "`)' title='Gửi tin giới thiệu app' class='btn primary-btn btn-yellow h45-btn'><i class='fa fa-paper-plane-o' aria-hidden='true'></i></a>");
                        }
                    }
                    html.Append("   </td>");
                    html.Append("</tr>");
                }
            }
            else
            {
                if (acc.RoleID == 0)
                {
                    html.Append("<tr><td colspan='12'>Không tìm thấy khách hàng...</td></tr>");
                }
                else
                {
                    html.Append("<tr><td colspan='11'>Không tìm thấy khách hàng...</td></tr>");
                }
            }

            ltrList.Text = html.ToString();
        }
        public static Int32 GetIntFromQueryString(String key)
        {
            Int32 returnValue = -1;
            String queryStringValue = HttpContext.Current.Request.QueryString[key];
            try
            {
                if (queryStringValue == null)
                    return returnValue;
                if (queryStringValue.IndexOf("#") > 0)
                    queryStringValue = queryStringValue.Substring(0, queryStringValue.IndexOf("#"));
                returnValue = Convert.ToInt32(queryStringValue);
            }
            catch
            { }
            return returnValue;
        }
        private int PageCount;
        protected void DisplayHtmlStringPaging1()
        {

            Int32 CurrentPage = Convert.ToInt32(Request.QueryString["Page"]);
            if (CurrentPage == -1) CurrentPage = 1;
            string[] strText = new string[4] { "Trang đầu", "Trang cuối", "Trang sau", "Trang trước" };
            if (PageCount > 1)
                Response.Write(GetHtmlPagingAdvanced(6, CurrentPage, PageCount, Context.Request.RawUrl, strText));

        }
        private static string GetPageUrl(int currentPage, string pageUrl)
        {
            pageUrl = Regex.Replace(pageUrl, "(\\?|\\&)*" + "Page=" + currentPage, "");
            if (pageUrl.IndexOf("?") > 0)
            {
                pageUrl += "&Page={0}";
            }
            else
            {
                pageUrl += "?Page={0}";
            }
            return pageUrl;
        }
        public static string GetHtmlPagingAdvanced(int pagesToOutput, int currentPage, int pageCount, string currentPageUrl, string[] strText)
        {
            //Nếu Số trang hiển thị là số lẻ thì tăng thêm 1 thành chẵn
            if (pagesToOutput % 2 != 0)
            {
                pagesToOutput++;
            }

            //Một nửa số trang để đầu ra, đây là số lượng hai bên.
            int pagesToOutputHalfed = pagesToOutput / 2;

            //Url của trang
            string pageUrl = GetPageUrl(currentPage, currentPageUrl);


            //Trang đầu tiên
            int startPageNumbersFrom = currentPage - pagesToOutputHalfed; ;

            //Trang cuối cùng
            int stopPageNumbersAt = currentPage + pagesToOutputHalfed; ;

            StringBuilder output = new StringBuilder();

            //Nối chuỗi phân trang
            //output.Append("<div class=\"paging\">");
            output.Append("<ul>");

            //Link First(Trang đầu) và Previous(Trang trước)
            if (currentPage > 1)
            {
                output.Append("<li><a title=\"" + strText[0] + "\" href=\"" + string.Format(pageUrl, 1) + "\">Trang đầu</a></li>");
                output.Append("<li><a title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\">Trang trước</a></li>");
                //output.Append("<li class=\"UnselectedPrev\" ><a title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\"><i class=\"fa fa-angle-left\"></i></a></li>");

                //output.Append("<span class=\"Unselect_prev\"><a href=\"" + string.Format(pageUrl, currentPage - 1) + "\"></a></span>");
            }

            /******************Xác định startPageNumbersFrom & stopPageNumbersAt**********************/
            if (startPageNumbersFrom < 1)
            {
                startPageNumbersFrom = 1;

                //As page numbers are starting at one, output an even number of pages.  
                stopPageNumbersAt = pagesToOutput;
            }

            if (stopPageNumbersAt > pageCount)
            {
                stopPageNumbersAt = pageCount;
            }

            if ((stopPageNumbersAt - startPageNumbersFrom) < pagesToOutput)
            {
                startPageNumbersFrom = stopPageNumbersAt - pagesToOutput;
                if (startPageNumbersFrom < 1)
                {
                    startPageNumbersFrom = 1;
                }
            }
            /******************End: Xác định startPageNumbersFrom & stopPageNumbersAt**********************/

            //Các dấu ... chỉ những trang phía trước  
            if (startPageNumbersFrom > 1)
            {
                output.Append("<li><a href=\"" + string.Format(GetPageUrl(currentPage - 1, pageUrl), startPageNumbersFrom - 1) + "\">&hellip;</a></li>");
            }

            //Duyệt vòng for hiển thị các trang
            for (int i = startPageNumbersFrom; i <= stopPageNumbersAt; i++)
            {
                if (currentPage == i)
                {
                    output.Append("<li class=\"current\" ><a >" + i.ToString() + "</a> </li>");
                }
                else
                {
                    output.Append("<li><a href=\"" + string.Format(pageUrl, i) + "\">" + i.ToString() + "</a> </li>");
                }
            }

            //Các dấu ... chỉ những trang tiếp theo  
            if (stopPageNumbersAt < pageCount)
            {
                output.Append("<li><a href=\"" + string.Format(pageUrl, stopPageNumbersAt + 1) + "\">&hellip;</a></li>");
            }

            //Link Next(Trang tiếp) và Last(Trang cuối)
            if (currentPage != pageCount)
            {
                //output.Append("<span class=\"Unselect_next\"><a href=\"" + string.Format(pageUrl, currentPage + 1) + "\"></a></span>");
                //output.Append("<li class=\"UnselectedNext\" ><a title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\"><i class=\"fa fa-angle-right\"></i></a></li>");
                output.Append("<li><a title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\">Trang sau</a></li>");
                output.Append("<li><a title=\"" + strText[3] + "\" href=\"" + string.Format(pageUrl, pageCount) + "\">Trang cuối</a></li>");
            }
            output.Append("</ul>");
            //output.Append("</div>");
            return output.ToString();
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string request = "/danh-sach-khach-hang?";

            if (txtTextSearch.Text != "")
            {
                request += "&textsearch=" + txtTextSearch.Text;
            }

            if (ddlProvince.SelectedValue != "")
            {
                request += "&province=" + ddlProvince.SelectedValue;
            }

            if (ddlCreatedBy.SelectedValue != "")
            {
                request += "&createdby=" + ddlCreatedBy.SelectedValue;
            }

            if (rFromDate.SelectedDate.HasValue)
            {
                request += "&fromdate=" + rFromDate.SelectedDate.ToString();
            }

            if (rToDate.SelectedDate.HasValue)
            {
                request += "&todate=" + rToDate.SelectedDate.ToString();
            }

            if (ddlSort.SelectedValue != "")
            {
                request += "&sort=" + ddlSort.SelectedValue;
            }

            Response.Redirect(request);

        }
    }
}