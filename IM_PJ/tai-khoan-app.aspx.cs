using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static IM_PJ.Controllers.UserController;

namespace IM_PJ
{
    public partial class tai_khoan_app : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);
                    int agent = acc.AgentID.ToString().ToInt();

                    if (acc == null)
                    {
                        Response.Redirect("/trang-chu");
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
                LoadData();
                LoadCity();
            }
        }
        
        public void LoadCity()
        {
            var pro = ProvinceController.GetAll();
            ddlCity.Items.Clear();
            ddlCity.Items.Insert(0, new ListItem("Tỉnh thành", "0"));
            if (pro.Count > 0)
            {
                foreach (var p in pro)
                {
                    ListItem listitem = new ListItem(p.Name, p.Name);
                    ddlCity.Items.Add(listitem);
                }
                ddlCity.DataBind();
            }
        }
        public void LoadData()
        {
            string username = Request.Cookies["usernameLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                int Status = 0;
                string TextSearch = "";
                string Gender = "";
                string CreatedDate = "";
                string City = "";

                if (Request.QueryString["textsearch"] != null)
                {
                    TextSearch = Request.QueryString["textsearch"].Trim();
                }
                if (Request.QueryString["gender"] != null)
                {
                    Gender = Request.QueryString["gender"];
                    ddlGender.SelectedValue = Gender.ToString();
                }
                if (Request.QueryString["status"] != null)
                {
                    Status = Request.QueryString["status"].ToInt(0);
                    ddlStatus.SelectedValue = Status.ToString();
                }
                if (Request.QueryString["createddate"] != null)
                {
                    CreatedDate = Request.QueryString["createddate"];
                }
                if (Request.QueryString["city"] != "")
                {
                    City = Request.QueryString["city"];
                }

                txtSearch.Text = TextSearch;
                
                ddlCreatedDate.SelectedValue = CreatedDate.ToString();
                ddlCity.SelectedValue = City;

                var rs = UserController.Filter(TextSearch, Gender, City, Status, CreatedDate);

                pagingall(rs);

                ltrNumberOfOrder.Text = rs.Count().ToString();

            }
        }
        [WebMethod]
        public static string updateStatus(int id, int value)
        {
            int ID = UserController.UpdateStatus(id, value);

            if (ID != 0)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        #region Paging
        public void pagingall(List<AppUserOut> acs)
        {
            string username = Request.Cookies["usernameLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);

            int PageSize = 30;

            StringBuilder html = new StringBuilder();
            html.Append("<thead>");
            html.Append("<tr>");
            html.Append("    <th>#</th>");
            html.Append("    <th>Tên</th>");
            html.Append("    <th>Điện thoại</th>");
            html.Append("    <th>Giới tính</th>");
            html.Append("    <th>Địa chỉ</th>");
            html.Append("    <th>Tỉnh</th>");
            html.Append("    <th>Trạng thái</th>");
            html.Append("    <th>Ngày đăng ký</th>");
            html.Append("</tr>");
            html.Append("</thead>");

            html.Append("<tbody>");
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

                    var TrTag = new StringBuilder();
                    TrTag.AppendLine("<tr ");
                    TrTag.AppendLine(String.Format("data-userid='{0}' ", item.ID));
                    TrTag.AppendLine("/>");

                    html.Append(TrTag.ToString());
                    html.Append("   <td data-title='#'>" + (i + 1).ToString() + "</td>");
                    html.Append("   <td data-title='Tên'><strong>" + item.FullName + "</strong></td>");
                    string phone = "<a href='https://zalo.me/" + item.Phone + "' target='_blank'>" + item.Phone + "</a>";
                    if (item.Status == 1)
                    {
                        phone = "<a href='javascript:;' data-id='" + item.ID + "' data-phone='" + item.Phone + "' data-update='2' onclick='getPhone($(this))'>xem</a>";
                    }
                    html.Append("   <td id='phone' data-title='Điện thoại'>" + phone + "</td>");
                    string gender = "";
                    if (item.Gender == "F")
                    {
                        gender = "Nữ";
                    }
                    html.Append("   <td data-title='Giới tính'><strong>" + gender + "</strong></td>");
                    html.Append("   <td data-title='Địa chỉ'>" + item.Address + "</td>");
                    html.Append("   <td data-title='Tỉnh'>" + item.City + "</td>");

                    string status = "";
                    if (item.Status == 1)
                    {
                        status = "<span class='bg-black'>Chưa xem</span>";
                    }
                    else if (item.Status == 2)
                    {
                        status = "<span class='bg-yellow'>Đã xem</span>";
                    }
                    else if (item.Status == 3)
                    {
                        status = "<span class='bg-green'>Khách hàng</span>";
                    }
                    html.Append("   <td id='status' data-title='Trạng thái'>" + status + "</td>");

                    string date = string.Format("{0:dd/MM HH:mm}", item.CreatedDate);
                    html.Append("   <td data-title='Ngày đăng ký'>" + date + "</td>");
                    html.Append("</tr>");
                }
            }
            else
            {
                html.Append("<tr><td colspan='12'>Không tìm thấy tài khoản...</td></tr>");
            }

            html.Append("</tbody>");

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
            string search = txtSearch.Text.Trim();
            string request = "/tai-khoan-app?";

            if (search != "")
            {
                request += "&textsearch=" + search;
            }
            if (ddlGender.SelectedValue != "")
            {
                request += "&gender=" + ddlGender.SelectedValue;
            }
            if (ddlStatus.SelectedValue != "0")
            {
                request += "&status=" + ddlStatus.SelectedValue;
            }

            if (ddlCity.SelectedValue != "0")
            {
                request += "&city=" + ddlCity.SelectedValue;
            }

            if (ddlCreatedDate.SelectedValue != "")
            {
                request += "&createddate=" + ddlCreatedDate.SelectedValue;
            }

            Response.Redirect(request);
        }
       
    }
}