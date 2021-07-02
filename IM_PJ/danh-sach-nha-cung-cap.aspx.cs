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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class danh_sach_nha_cung_cap : System.Web.UI.Page
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
                        if (acc.RoleID != 0)
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
        public void LoadData()
        {
            string s = "";
            if (Request.QueryString["s"] != null)
                s = Request.QueryString["s"];
            //txtAgentName.Text = s;
            var orders = SupplierController.GetAll(s);
            if (orders.Count > 0)
            {
                pagingall(orders);
            }

        }
        #region Paging
        public void pagingall(List<tbl_Supplier> acs)
        {
            int PageSize = 15;
            StringBuilder html = new StringBuilder();
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
                    //html.Append("   <td>" + item.ID + "</td>");
                    html.Append("   <td>" + item.SupplierName + "</td>");
                    html.Append("   <td>" + item.SupplierPhone + "</td>");
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
                    html.Append("   <td>" + ishidden + "</td>");
                    string date = string.Format("{0:dd/MM/yyyy}", item.CreatedDate);
                    html.Append("   <td>" + date + "</td>");
                    html.Append("   <td>");
                    html.Append("       <a href=\"/chi-tiet-nha-cung-cap?id=" + item.ID + "\" class=\"btn primary-btn h45-btn\">Chi tiết</a>");                    
                    html.Append("   </td>");
                    html.Append("</tr>");
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

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    string search = txtAgentName.Text;
           
        //        Response.Redirect("/danh-sach-nha-cung-cap?s=" + search + "");
            
        //}
    }
}