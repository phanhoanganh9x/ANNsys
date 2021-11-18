using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using static IM_PJ.Controllers.ProductController;
using IM_PJ.Utils;
using IM_PJ.Models.Pages.cron_job_product_status;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class cron_job_product_status : System.Web.UI.Page
    {
        private const string CRON_JOB_NAME = "Product Status";
        private static tbl_Account acc;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                    acc = AccountController.GetByUsername(username);
                    if (acc != null && acc.RoleID == 0)
                    {
                        LoadCategory();
                        LoadData();
                    }
                    else
                    {
                        Response.Redirect("/trang-chu");
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
            }
        }

        public void LoadCategory()
        {
            var categories = CategoryController.getDropDownList();

            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new ListItem("Danh mục", "0"));

            if (categories.Count > 0)
                ddlCategory.Items.AddRange(categories.ToArray());

            ddlCategory.DataBind();
        }

        public void LoadData()
        {
            if (acc != null)
            {
                DateTime DateConfig = new DateTime(2018, 6, 22);

                DateTime fromDate = DateConfig;
                DateTime toDate = DateTime.Now;

                if (!String.IsNullOrEmpty(Request.QueryString["fromdate"]))
                {
                    fromDate = Convert.ToDateTime(Request.QueryString["fromdate"]);
                }

                if (!String.IsNullOrEmpty(Request.QueryString["todate"]))
                {
                    toDate = Convert.ToDateTime(Request.QueryString["todate"]).AddDays(1).AddMinutes(-1);
                }

                rFromDate.SelectedDate = fromDate;
                rFromDate.MinDate = DateConfig;
                rFromDate.MaxDate = DateTime.Now;

                rToDate.SelectedDate = toDate;
                rToDate.MinDate = DateConfig;
                rToDate.MaxDate = DateTime.Now;

                var search = String.Empty;
                var web = String.Empty;
                var status = 0;
                var category = 0;
                var isHidden = (Nullable<bool>)null;
                var showHomePage = String.Empty;
                var sort = String.Empty;
                var Page = 1;

                if (Request.QueryString["search"] != null)
                    search = Request.QueryString["search"].Trim();
                if (Request.QueryString["web"] != null)
                    web = Request.QueryString["web"];
                if (Request.QueryString["status"] != null)
                    status = Request.QueryString["status"].ToInt();
                if (Request.QueryString["category"] != null)
                    category = Request.QueryString["category"].ToInt();
                if (!String.IsNullOrEmpty(Request.QueryString["isHidden"]))
                    isHidden = Request.QueryString["isHidden"].ToBool();
                if (Request.QueryString["showhomepage"] != null)
                    showHomePage = Request.QueryString["showhomepage"];
                if (Request.QueryString["sort"] != null)
                    sort = Request.QueryString["sort"];
                if (Request.QueryString["Page"] != null)
                    Page = Request.QueryString["Page"].ToInt();

                txtSearchProduct.Text = search;
                ddlWebAdvertisement.SelectedValue = web;
                ddlProductStatus.SelectedValue = isHidden.HasValue ? (isHidden.Value ? "true" : "false") : "";
                ddlScheduleStatus.SelectedValue = status.ToString();
                ddlShowHomePage.SelectedValue = showHomePage;
                ddlCategory.SelectedValue = category.ToString();
                ddlSort.SelectedValue = sort;

                // Create order fileter
                var filter = new FilterModel()
                {
                    search = search,
                    web = web,
                    status = status,
                    fromDate = fromDate,
                    toDate = toDate,
                    category = category,
                    isHidden = isHidden,
                    showHomePage = showHomePage,
                    sort = sort
                };
                // Create pagination
                var page = new PaginationMetadataModel()
                {
                    currentPage = Page
                };
                var data = CronJobController.getScheduleProductStatus(filter, ref page);

                pagingall(data, page);

                ltrNumberOfSchedule.Text = page.totalCount.ToString();
            }
        }
        
        public class ProductVariable
        {
            public string VariableName { get; set; }
            public string VariableValue { get; set; }
        }
        #region Paging
        public void pagingall(List<Models.Pages.cron_job_product_status.ProductModel> acs,
                              PaginationMetadataModel page)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<thead>");
            html.AppendLine("<tr>");
            html.AppendLine("    <th class='image-column'>Ảnh</th>");
            html.AppendLine("    <th class='name-column'>Sản phẩm</th>");
            html.AppendLine("    <th class='sku-column'>Mã</th>");
            html.AppendLine("    <th class='category-column'>Danh mục</th>");
            html.AppendLine("    <th class='quantity-column'>Số lượng</th>");
            html.AppendLine("    <th class='web-column'>Web</th>");
            html.AppendLine("    <th class='show-homepage-column'>Trang quảng cáo</th>");
            html.AppendLine("    <th class='status-column'>Trạng thái</th>");
            html.AppendLine("    <th class='cron-column'>Cron</th>");
            html.AppendLine("    <th class='date-column'>Ngày chạy<br/>Chú thích</th>");
            
            html.AppendLine("    <th>Đồng bộ</th>");
            html.AppendLine("</tr>");
            html.AppendLine("</thead>");

            html.AppendLine("<tbody>");
            if (acs.Count > 0)
            {
                PageCount = page.totalPages;
                Int32 Page = page.currentPage;

                foreach (var item in acs)
                {
                    html.AppendLine("<tr>");
                    html.AppendLine("   <td>");
                    html.AppendLine("       <a target='_blank' href='/xem-san-pham?id=" + item.id + "'>");
                    html.AppendLine("           <img src='" + Thumbnail.getURL(item.image, Thumbnail.Size.Small) + "'/></a>");
                    html.AppendLine("       </a>");
                    html.AppendLine("   </td>");
                    html.AppendLine("   <td class='customer-name-link'><a target='_blank' href='/xem-san-pham?id=" + item.id + "'>" + item.title + "</a><br>" + string.Format("{0:dd/MM/yyyy}", item.productCreatedDate) + "</td>");
                    html.AppendLine("   <td data-title='Mã' class='customer-name-link'>" + item.sku + "</td>");
                    html.AppendLine("   <td data-title='Danh mục'>" + item.categoryName + "</td>");
                    html.AppendLine(String.Format("   <td data-title='Số lượng'>{0:#,###}</td>", item.quantity));
                    html.AppendLine("   <td data-title='Web'>" + item.web.Replace("https://", String.Empty) + "</td>");
                    if (item.showHomePage == 0)
                        html.Append("   <td data-title='Trang quảng cáo'><span class='bg-black bg-button'>Đang ẩn</span></td>");
                    else
                        html.Append("   <td data-title='Trang quảng cáo'><span class='bg-green bg-button'>Đang hiện</span></td>");
                    html.AppendLine("   <td data-title='Trạng Thái Sản phẩm'>");
                    if (item.isHidden)
                        html.AppendLine("      <span class='bg-red'>Ẩn</span>");
                    else
                        html.AppendLine("      <span class='bg-green'>Hiện</span>");
                    html.AppendLine("   </td>");
                    html.AppendLine("   <td data-title='Trạng Thái Cron'>");
                    if (item.cronJobStatus == 1)
                        html.AppendLine("      <span class='bg-yellow'>Chờ chạy</span>");
                    else if (item.cronJobStatus == 2)
                        html.AppendLine("      <span class='bg-blue'>Đang chạy</span>");
                    else if (item.cronJobStatus == 3)
                        html.AppendLine("      <span class='bg-brown'>Tạm dừng</span>");
                    else if (item.cronJobStatus == 4)
                        html.AppendLine("      <span class='bg-green'>Thành công</span>");
                    else if (item.cronJobStatus == 6)
                        html.AppendLine("      <span class='bg-black'>Bỏ qua</span>");
                    else
                        html.AppendLine("      <span class='bg-red'>Thất bại</span>");
                    html.AppendLine("   </td>");
                    html.AppendLine(String.Format("   <td data-title='Ngày chạy và Chú thích'>{0:dd/MM/yyyy}<br/>{1}</td>", item.startDate, item.note));
                    html.AppendLine("   <td data-title='Thao tác' class='update-button'>");
                    if (item.cronJobStatus == (int)CronJobStatus.Continue && !item.isHidden && item.showHomePage == 1)
                        html.AppendLine("       <a href='javascript:;' title='Đồng bộ sản phẩm' class='up-product-" + item.id + " btn primary-btn h45-btn' onclick='showProductSyncModal(`" + item.sku + "`, " + item.id + ", " + item.categoryID + ", `" + item.title + "`);'><i class='fa fa-refresh' aria-hidden='true'></i></a>");
                    html.Append("  </td>");
                    html.AppendLine("</tr>");
                }

            }
            else
            {
                html.Append("<tr><td colspan='16'>Không tìm thấy sản phẩm...</td></tr>");
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
            string search = txtSearchProduct.Text;
            string request = "/cron-job-product-status?";

            if (search != "")
            {
                request += "&search=" + search;
            }

            if (ddlWebAdvertisement.SelectedValue != "")
            {
                request += "&web=" + ddlWebAdvertisement.SelectedValue;
            }

            if (ddlScheduleStatus.SelectedValue != "0")
            {
                request += "&status=" + ddlScheduleStatus.SelectedValue;
            }

            if (rFromDate.SelectedDate.HasValue)
            {
                request += "&fromdate=" + rFromDate.SelectedDate.ToString();
            }

            if (rToDate.SelectedDate.HasValue)
            {
                request += "&todate=" + rToDate.SelectedDate.ToString();
            }

            if (ddlCategory.SelectedValue != "0")
            {
                request += "&category=" + ddlCategory.SelectedValue;
            }

            if (!String.IsNullOrEmpty(ddlProductStatus.SelectedValue))
            {
                request += "&isHidden=" + ddlProductStatus.SelectedValue;
            }

            if (!String.IsNullOrEmpty(ddlShowHomePage.SelectedValue))
            {
                request += "&showhomepage=" + ddlShowHomePage.SelectedValue;
            }

            if (!String.IsNullOrEmpty(ddlSort.SelectedValue))
            {
                request += "&sort=" + ddlSort.SelectedValue;
            }

            Response.Redirect(request);
        }

        [WebMethod]
        public static IM_PJ.Models.CronJob getCronJob()
        {
            var cron = CronJobController.get(CRON_JOB_NAME);
            return cron;
        }

        [WebMethod]
        public static IM_PJ.Models.CronJob updateCronJob(IM_PJ.Models.CronJob cronNew)
        {
            // Cài đặt tên của cron job
            cronNew.Name = CRON_JOB_NAME;
            var cron = CronJobController.update(cronNew);

            return cron;
        }
    }
}