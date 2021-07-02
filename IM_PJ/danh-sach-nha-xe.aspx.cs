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
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class danh_sach_nha_xe : System.Web.UI.Page
    {
        private int PageCount;

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
                        if (acc.RoleID == 1)
                        {
                            Response.Redirect("/trang-chu");
                        }
                        else if (acc.RoleID == 0)
                        {
                            LoadStatus();
                        }
                        else if (acc.RoleID == 2)
                        {
                            LoadStatus(acc);
                        }
                        LoadData();
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
            }
        }
        public void LoadStatus(tbl_Account acc = null)
        {
            if (acc != null)
            {
                ddlStatus.Items.Clear();
                ddlStatus.Items.Insert(0, new ListItem("Đang hiện", "1"));
            }
            else
            {
                ddlStatus.Items.Clear();
                ddlStatus.Items.Insert(0, new ListItem("Trạng thái", ""));
                ddlStatus.Items.Insert(1, new ListItem("Đang hiện", "1"));
                ddlStatus.Items.Insert(2, new ListItem("Đang ẩn", "0"));
            }
        }
        /// <summary>
        /// Setting init when load page
        /// </summary>
        private void LoadData()
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

            string TextSearch = "";
            string COD = "";
            string Prepay = "";
            string CreatedDate = "";
            string Status = "";

            if (Request.QueryString["textsearch"] != null)
            {
                TextSearch = Request.QueryString["textsearch"].Trim();
            }
            if (Request.QueryString["cod"] != null)
            {
                COD = Request.QueryString["cod"];
            }
            if (Request.QueryString["prepay"] != null)
            {
                Prepay = Request.QueryString["prepay"];
            }
            if (Request.QueryString["createddate"] != null)
            {
                CreatedDate = Request.QueryString["createddate"];
            }
            if (Request.QueryString["status"] != null)
            {
                Status = Request.QueryString["status"];
            }

            txtTextSearch.Text = TextSearch;
            ddlCOD.SelectedValue = COD.ToString();
            ddlPrepay.SelectedValue = Prepay.ToString();
            ddlCreatedDate.SelectedValue = CreatedDate.ToString();
            ddlStatus.SelectedValue = Status.ToString();

            var rs = TransportCompanyController.Filter(TextSearch);

            if (acc.RoleID == 2)
            {
                rs = rs.Where(x => x.Status == 1).ToList();
            }

            if (acc.RoleID == 0)
            {
                if (Status != "")
                {
                    rs = rs.Where(x => x.Status == Status.ToInt()).ToList();
                }
            }

            if (COD != "")
            {
                rs = rs.Where(x => x.COD == COD.ToBool()).ToList();
            }

            if (Prepay != "")
            {
                rs = rs.Where(x => x.Prepay == Prepay.ToBool()).ToList();
            }

            if (CreatedDate != "")
            {
                DateTime fromdate = DateTime.Today;
                DateTime todate = DateTime.Now;
                switch (CreatedDate)
                {
                    case "today":
                        fromdate = DateTime.Today;
                        todate = DateTime.Now;
                        break;
                    case "yesterday":
                        fromdate = fromdate.AddDays(-1);
                        todate = DateTime.Today;
                        break;
                    case "beforeyesterday":
                        fromdate = DateTime.Today.AddDays(-2);
                        todate = DateTime.Today.AddDays(-1);
                        break;
                    case "week":
                        int days = DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.Today.DayOfWeek;
                        fromdate = fromdate.AddDays(-days + 1);
                        todate = DateTime.Now;
                        break;
                    case "thismonth":
                        fromdate = new DateTime(fromdate.Year, fromdate.Month, 1);
                        todate = DateTime.Now;
                        break;
                    case "7days":
                        fromdate = DateTime.Today.AddDays(-6);
                        todate = DateTime.Now;
                        break;
                    case "30days":
                        fromdate = DateTime.Today.AddDays(-29);
                        todate = DateTime.Now;
                        break;
                }
                rs = rs.Where(x => x.CreatedDate >= fromdate && x.CreatedDate <= todate).ToList();
            }

            pagingall(rs.Distinct().ToList());

            ltrNumberOfTransport.Text = rs.Count().ToString();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtTextSearch.Text;
            string request = "/danh-sach-nha-xe?";

            if (search != "")
            {
                request += "&textsearch=" + search;
            }

            if (ddlCOD.SelectedValue != "")
            {
                request += "&cod=" + ddlCOD.SelectedValue;
            }

            if (ddlPrepay.SelectedValue != "")
            {
                request += "&prepay=" + ddlPrepay.SelectedValue;
            }

            if (ddlCreatedDate.SelectedValue != "")
            {
                request += "&createddate=" + ddlCreatedDate.SelectedValue;
            }

            if (ddlStatus.SelectedValue != "")
            {
                request += "&status=" + ddlStatus.SelectedValue;
            }

            Response.Redirect(request);

        }
        [WebMethod]
        public static string updateStatus(int ID, int SubID)
        {
            string update = TransportCompanyController.UpdateStatus(ID, SubID);

            if (update == "true")
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }
        #region Paging
        public void pagingall(List<tbl_TransportCompany> transports)
        {
            int PageSize = 30;
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

            StringBuilder html = new StringBuilder();
            html.Append("<tr>");
            html.Append("     <th class=\"column-name\">Tên nhà xe</th>");
            html.Append("     <th class=\"column-phone\">Điện thoại</th>");
            html.Append("     <th class=\"column-address\">Địa chỉ</th>");
            html.Append("     <th class=\"column-prepay\">Trả cước</th>");
            html.Append("     <th class=\"column-cod\">Thu hộ</th>");
            html.Append("     <th class=\"column-createddate\">Ngày tạo</th>");
            html.Append("     <th class=\"column-action\"></th>");
            html.Append("</tr>");

            if (transports.Count > 0)
            {
                int TotalItems = transports.Count;

                if (TotalItems % PageSize == 0)
                {
                    PageCount = TotalItems / PageSize;
                }
                else
                {
                    PageCount = TotalItems / PageSize + 1;
                }

                int Page = GetIntFromQueryString();

                int FromRow = (Page - 1) * PageSize;
                int ToRow = Page * PageSize - 1;

                if (ToRow >= TotalItems)
                {
                    ToRow = TotalItems - 1;
                }
                for (int i = FromRow; i < ToRow + 1; i++)
                {
                    var company = transports[i];
                    String rowHtml = String.Empty;

                    rowHtml += Environment.NewLine + String.Format("<tr class='status-" + company.Status + "'>");
                    rowHtml += Environment.NewLine + String.Format("    <td class=\"customer-name-link\"><a href=\"/chi-tiet-nha-xe?id={0}\">{1}</a></td>", company.ID, company.CompanyName.ToTitleCase());
                    rowHtml += Environment.NewLine + String.Format("    <td>{0}</td>", company.CompanyPhone);
                    rowHtml += Environment.NewLine + String.Format("    <td>{0}</td>", company.CompanyAddress.ToTitleCase());
                    rowHtml += Environment.NewLine + String.Format("    <td>{0}</td>", company.Prepay ? "Trả trước" : "Trả sau");
                    rowHtml += Environment.NewLine + String.Format("    <td>{0}</td>", company.COD ? "Có" : "Không");
                    rowHtml += Environment.NewLine + String.Format("    <td>{0:dd/MM/yyyy}</td>", company.CreatedDate);
                    rowHtml += Environment.NewLine + String.Format("    <td>");
                    rowHtml += Environment.NewLine + String.Format("        <a href=\"/chi-tiet-nha-xe?id={0}\" title=\"Quản lý nơi nhận\" class=\"btn primary-btn h45-btn\"><i class=\"fa fa-list\" aria-hidden=\"true\"></i></a>", company.ID);
                    rowHtml += Environment.NewLine + String.Format("        <a href=\"/sua-thong-tin-nha-xe?id={0}\" title=\"Sửa thông tin nhà xe\" class=\"btn primary-btn h45-btn\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i></a>", company.ID);
                    if (acc.RoleID == 0)
                    {
                        if (company.Status == 1)
                        {
                            rowHtml += Environment.NewLine + String.Format("        <a href=\"javascript:;\" title=\"Ẩn nhà xe\" data-id=\"{0}\" data-subid=\"{1}\" data-status=\"{2}\" onclick=\"updateStatus($(this))\" class=\"btn primary-btn h45-btn btn-red\"><i class=\"fa fa-times\" aria-hidden=\"true\"></i></a>", company.ID, company.SubID, company.Status);
                        }
                        else
                        {
                            rowHtml += Environment.NewLine + String.Format("        <a href=\"javascript:;\" title=\"Hiện nhà xe\" data-id=\"{0}\" data-subid=\"{1}\" data-status=\"{2}\" onclick=\"updateStatus($(this))\" class=\"btn primary-btn h45-btn btn-blue\"><i class=\"fa fa-refresh\" aria-hidden=\"true\"></i></a>", company.ID, company.SubID, company.Status);
                        }
                    }
                    rowHtml += Environment.NewLine + String.Format("    </td>");
                    rowHtml += Environment.NewLine + String.Format("</tr>");

                    html.AppendLine(rowHtml);
                }
            }
            else
            {
                html.Append("<tr><td colspan=\"8\">Không tìm thấy nhà xe...</td></tr>");
            }

            this.ltrList.Text = html.ToString();

        }

        private static int GetIntFromQueryString()
        {
            int returnValue = 1;

            String queryStringValue = HttpContext.Current.Request.QueryString["Page"];
            try
            {
                if (queryStringValue != null)
                {
                    if (queryStringValue.IndexOf("#") > 0)
                    {
                        queryStringValue = queryStringValue.Substring(0, queryStringValue.IndexOf("#"));
                    }
                    else
                    {
                        returnValue = Convert.ToInt32(queryStringValue);
                    }
                }
            }
            catch
            {
                returnValue = 1;
            }
            return returnValue;
        }

        
        protected void DisplayHtmlStringPaging1()
        {

            int CurrentPage = Convert.ToInt32(Request.QueryString["Page"]);

            // Check min page
            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }

            string[] strText = new string[4] { "Trang đầu", "Trang cuối", "Trang sau", "Trang trước" };

            if (PageCount > 1)
            {
                Response.Write(GetHtmlPagingAdvanced(6, CurrentPage, PageCount, Context.Request.RawUrl, strText));
            }

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

            String pageHtml = String.Empty;

            //Nối chuỗi phân trang
            pageHtml += Environment.NewLine + String.Format("<ul>");

            //Link First(Trang đầu) và Previous(Trang trước)
            if (currentPage > 1)
            {
                pageHtml += Environment.NewLine + String.Format("    <li><a title='{0}' href='{1}'>Trang đầu</a></li>", strText[0], String.Format(pageUrl, 1));
                pageHtml += Environment.NewLine + String.Format("    <li><a title='{0}' href='{1}'>Trang trước</a></li>", strText[1], String.Format(pageUrl, currentPage - 1));
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
                pageHtml += Environment.NewLine + String.Format("    <li><a href='{0}'>&hellip;</a></li>", String.Format(GetPageUrl(currentPage - 1, pageUrl), startPageNumbersFrom - 1));
            }

            //Duyệt vòng for hiển thị các trang
            for (int i = startPageNumbersFrom; i <= stopPageNumbersAt; i++)
            {
                if (currentPage == i)
                {
                    pageHtml += Environment.NewLine + String.Format("    <li class=\"current\" ><a >{0}</a></li>", i.ToString());
                }
                else
                {
                    pageHtml += Environment.NewLine + String.Format("    <li><a href='{0}'>{1}</a></li>", String.Format(pageUrl, i), i.ToString());
                }
            }

            //Các dấu ... chỉ những trang tiếp theo  
            if (stopPageNumbersAt < pageCount)
            {
                pageHtml += Environment.NewLine + String.Format("    <li><a href='{0}'>&hellip;</a></li>", String.Format(pageUrl, stopPageNumbersAt + 1));
            }

            //Link Next(Trang tiếp) và Last(Trang cuối)
            if (currentPage != pageCount)
            {
                pageHtml += Environment.NewLine + String.Format("    <li><a title='{0}' href='{1}'>Trang sau</a></li>", strText[2], String.Format(pageUrl, currentPage + 1));
                pageHtml += Environment.NewLine + String.Format("    <li><a title='{0}' href='{1}'>Trang cuối</a></li>", strText[3], String.Format(pageUrl, pageCount));
            }
            pageHtml += Environment.NewLine + String.Format("</ul>");

            return pageHtml;
        }
        #endregion

    }
}