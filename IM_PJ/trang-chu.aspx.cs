using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IM_PJ.Controllers;
using System.Text;
using System.Text.RegularExpressions;
using NHST.Bussiness;
using System.Web.Services;
using System.Configuration;
using System.IO;
using static IM_PJ.Controllers.OrderController;
using IM_PJ.Models;

namespace IM_PJ
{
    public partial class trang_chu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
                {
                    Session["usernameLoginSystem_ANN123"] = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
            }
            LoadData();
        }

        public class OrderTop
        {
            public int CusID { get; set; }
            public string CusName { get; set; }
            public string CusNick { get; set; }
            public string CusAddress { get; set; }
            public string CreateBy { get; set; }
            public string CusZalo { get; set; }
            public string CusFB { get; set; }
            public int Quantity { get; set; }
        }
        public void LoadData()
        {
            if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
            {
                string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                var acc = AccountController.GetByUsername(username);
                if (acc != null)
                {
                    if (acc.RoleID == 0 || acc.RoleID == 2)
                    {
                        DateTime fromdate = DateTime.Today;
                        DateTime todate = DateTime.Now;
                        List<OrderReportHomePage> order = new List<OrderReportHomePage>();
                        order = OrderController.ReportHomePage(fromdate, todate);
                        if (order != null)
                        {
                            pagingall(order);
                        }
                    }
                }
            }
        }
        #region Paging
        public void pagingall(List<OrderReportHomePage> acs)
        {
            int PageSize = acs.Count();
            StringBuilder html = new StringBuilder();

            if (acs.Count > 0)
            {
                html.Append("<table id='table-student' class='table table-checkable'>");
                html.Append("    <thead>");
                html.Append("        <tr>");
                html.Append("            <th>#</th>");
                html.Append("            <th>Khách hàng</th>");
                html.Append("            <th>Nick</th>");
                html.Append("            <th>Đã mua</th>");
                html.Append("            <th>Nhân viên</th>");
                html.Append("        </tr>");
                html.Append("    </thead>");
                html.Append("    <tbody>");
                for (int i = 0; i < PageSize; i++)
                {
                    var item = acs[i];
                    html.Append("<tr>");
                    html.Append("   <td>" + Convert.ToInt32(i + 1) + "</td>");
                    html.Append("   <td class='capitalize'>" + item.CustomerName + "</td>");
                    html.Append("   <td class='capitalize'>" + item.Nick + "</td>");
                    html.Append("   <td>" + item.Quantity + " cái" + "</td>");
                    html.Append("   <td>" + item.CreatedBy + "</td>");
                    html.Append("</tr>");
                }
                html.Append("    </tbody>");
                html.Append("</table>");
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
        private int PageCount = 0;
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
    }
}