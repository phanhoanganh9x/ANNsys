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
    public partial class chi_tiet_nha_xe : System.Web.UI.Page
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

                        var ID = Request.QueryString["ID"];

                        if (ID != null)
                        {
                            Initialize(Convert.ToInt32(ID));
                        }
                        else
                        {
                            Response.Redirect("/danh-sach-nha-xe");
                        }
                    }
                }
                else
                {

                    Response.Redirect("/dang-nhap");
                }
            }
        }

        /// <summary>
        /// Setting init when load page
        /// </summary>
        private void Initialize(int ID)
        {
            var company = TransportCompanyController.GetAllTransportCompanyByID(ID);

            if (company == null)
            {
                PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy nhà xe " + ID, "e", true, "/danh-sach-nha-xe", Page);
            }
            else
            {
                this.hdfID.Value = ID.ToString();
                this.txtCompanyName.Text = company.CompanyName;
                this.txtCompanyPhone.Text = company.CompanyPhone;
                this.txtCompanyAddress.Text = company.CompanyAddress;
                this.txtNote.Text = company.Note;
                this.txtPrepay.Text = company.Prepay ? "Trả cước trước" : "Trả cước sau";
                this.txtCOD.Text = company.COD ? "Có thu hộ" : "Không thu hộ";

                ltrEditButton.Text = "<a href=\"/sua-thong-tin-nha-xe?id=" + ID.ToString() + "\" class=\"btn primary-btn fw-btn not-fullwidth\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Chỉnh sửa</a>";

                var transprots = TransportCompanyController.GetAllReceivePlace(ID);

                pagingall(transprots);
            }
        }

        #region Paging
        public void pagingall(List<tbl_TransportCompany> transprots)
        {
            int PageSize = 40;
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

            StringBuilder html = new StringBuilder();
            html.Append("<tr>");
            html.Append("     <th>#</th>");
            html.Append("     <th>Nơi nhận</th>");
            html.Append("     <th>Địa chỉ chành</th>");
            html.Append("     <th>Trả cước</th>");
            html.Append("     <th>Thu hộ</th>");
            html.Append("     <th>Ngày tạo</th>");
            html.Append("     <th>Nhân viên</th>");
            html.Append("     <th></th>");
            html.Append("</tr>");

            if (transprots.Count > 0)
            {
                int TotalItems = transprots.Count;

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
                    var company = transprots[i];
                    String rowHtml = String.Empty;

                    rowHtml += Environment.NewLine + String.Format("<tr class='status-" + company.Status + "'>");
                    rowHtml += Environment.NewLine + String.Format("    <td>{0}</td>", i + 1);
                    rowHtml += Environment.NewLine + String.Format("    <td class=\"customer-name-link\"><a href=\"/chi-tiet-noi-den-nha-xe?id={0}&subid={1}\">{2}</a></td>", company.ID, company.SubID, company.ShipTo.ToTitleCase());
                    rowHtml += Environment.NewLine + String.Format("    <td>{0}</td>", company.Address.ToTitleCase());
                    rowHtml += Environment.NewLine + String.Format("    <td>{0}</td>", company.Prepay ? "Trả trước" : "Trả sau");
                    rowHtml += Environment.NewLine + String.Format("    <td>{0}</td>", company.COD ? "Có" : "Không");
                    rowHtml += Environment.NewLine + String.Format("    <td>{0:dd/MM/yyyy}</td>", company.CreatedDate);
                    rowHtml += Environment.NewLine + String.Format("    <td>{0}</td>", company.CreatedBy);
                    rowHtml += Environment.NewLine + String.Format("    <td>");
                    rowHtml += Environment.NewLine + String.Format("        <a href=\"/chi-tiet-noi-den-nha-xe?id={0}&subid={1}\" title=\"Sửa thông tin nơi nhận\" class=\"btn primary-btn h45-btn\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i></a>", company.ID, company.SubID);
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
                html.Append("<tr><td colspan=\"8\">Chưa có nơi nhận...</td></tr>");
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