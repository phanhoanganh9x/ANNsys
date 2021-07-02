using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using IM_PJ.Utils;
using IM_PJ.Models.Pages.thong_ke_chuyen_kho;

namespace IM_PJ
{
    public partial class thong_ke_chuyen_kho : System.Web.UI.Page
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
                        LoadData();
                        LoadCategory();
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
            var category = CategoryController.GetAllWithIsHidden(false);
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new ListItem("Danh mục", "0"));
            if (category.Count > 0)
            {
                addItemCategory(0, "");
                ddlCategory.DataBind();
            }
        }

        public void addItemCategory(int id, string h = "")
        {
            var categories = CategoryController.GetByParentID("", id);

            if (categories.Count > 0)
            {
                foreach (var c in categories)
                {
                    ListItem listitem = new ListItem(h + c.CategoryName, c.ID.ToString());
                    ddlCategory.Items.Add(listitem);

                    addItemCategory(c.ID, h + "---");
                }
            }
        }
        public void LoadData()
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                DateTime DateConfig = new DateTime(2019, 12, 15);

                var config = ConfigController.GetByTop1();
                if (config.ViewAllOrders == 1)
                {
                    DateConfig = new DateTime(2018, 6, 22);
                }

                DateTime fromDate = DateTime.Today;
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

                string TextSearch = "";
                string CreatedDate = "today";
                int CategoryID = 0;
                int stockSelected = 0;
                int Page = 1;

                if (Request.QueryString["textsearch"] != null)
                    TextSearch = Request.QueryString["textsearch"].Trim();
                if (Request.QueryString["categoryid"] != null)
                    CategoryID = Request.QueryString["categoryid"].ToInt();
                if (Request.QueryString["createddate"] != null)
                    CreatedDate = Request.QueryString["createddate"];
                // Add filter stock
                if (Request.QueryString["stock"] != null)
                    stockSelected = Convert.ToInt32(Request.QueryString["stock"]);
                if (Request.QueryString["Page"] != null)
                    Page = Request.QueryString["Page"].ToInt();
                

                txtSearchProduct.Text = TextSearch;
                ddlCategory.SelectedValue = CategoryID.ToString();
                ddlStock.SelectedValue = stockSelected.ToString();
                // Create order fileter
                var filter = new ProductFilterModel()
                {
                    search = TextSearch,
                    category = CategoryID,
                    fromDate = fromDate,
                    toDate = toDate,
                    warehouse = stockSelected
                };

                // Create pagination
                var page = new PaginationMetadataModel()
                {
                    currentPage = Page
                };
                int totalQuantityInput = 0;
                List<StockTransferReport> data = new List<StockTransferReport>(); 
                
                if (stockSelected == 1)
                    data = StockManagerController.getStock1TransferReport(filter, ref page, ref totalQuantityInput);
                else if (stockSelected == 2)
                    data = StockManagerController.getStock2TransferReport(filter, ref page, ref totalQuantityInput);

                pagingall(data, page);

                ltrNumberOfProduct.Text = String.Format("{0} sản phẩm - {1:#,###} cái", page.totalCount.ToString(), totalQuantityInput);
            }
        }

        #region Paging
        public void pagingall(List<StockTransferReport> data, PaginationMetadataModel page)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

            StringBuilder html = new StringBuilder();
            html.AppendLine("<thead>");
            html.AppendLine("    <tr>");
            html.AppendLine("        <th class='stt-column'>#</th>");
            html.AppendLine("        <th class='image-column'>Ảnh</th>");
            html.AppendLine("        <th class='name-column'>Sản phẩm</th>");
            html.AppendLine("        <th class='sku-column'>Mã</th>");
            html.AppendLine("        <th class='stock-column'>Số lượng</th>");
            html.AppendLine("        <th class='stock-column'>Kho hiện tại</th>");
            html.AppendLine("        <th class='category-column'>Danh mục</th>");
            html.AppendLine("        <th class='date-column'>Ngày chuyển</th>");
            html.AppendLine("        <th class='action-column'></th>");
            html.AppendLine("    </tr>");
            html.AppendLine("</thead>");

            html.AppendLine("<tbody>");
            if (data.Count > 0)
            {
                PageCount = page.totalPages;
                var Page = page.currentPage;
                var i = 0;
                foreach (var item in data)
                {
                    i++;
                    html.AppendLine("    <tr class='parent-row'>");
                    html.AppendLine(String.Format("        <td>{0}</td>", i));
                    html.AppendLine("        <td>");
                    html.AppendLine(String.Format("            <a href='/xem-san-pham?sku={0}'>", item.sku));
                    html.AppendLine(String.Format("                <img src='{0}'/>", Thumbnail.getURL(item.image, Thumbnail.Size.Small)));
                    html.AppendLine("            </a>");
                    html.AppendLine("        </td>");
                    html.AppendLine("        <td class='customer-name-link'>");
                    html.AppendLine(String.Format("            <a href='/xem-san-pham?sku={0}'>{1}</a>", item.sku, item.title));
                    html.AppendLine("        </td>");
                    html.AppendLine(String.Format("        <td data-title='Mã' class='customer-name-link'>{0}</td>", item.sku));
                    html.AppendLine(String.Format("        <td data-title='Nhập mới'>{0:N0}</td>", item.quantityTransfer));
                    html.AppendLine("        <td data-title='Kho hiện tại'>");
                    html.AppendLine(String.Format("        <a target='_blank' href='/thong-ke-san-pham?SKU={0}'>{1:N0}</a>", item.sku, item.quantityAvailable));
                    html.AppendLine("        </td>");
                    html.AppendLine(String.Format("        <td data-title='Danh mục'>{0}</td>", item.categoryName));
                    html.AppendLine(String.Format("        <td data-title='Ngày tạo'>{0:dd/MM/yyyy HH:mm}</td>", item.transferDate));
                    html.AppendLine("        <td data-title='Thao tác' class='update-button'>");
                    if (item.isVariable)
                    {
                        html.AppendLine(String.Format("      <a href='javascript:;' title='Xem thông tin sản phẩm con' class='btn primary-btn h45-btn btn-blue' onclick='showSubGoodsReceipt($(this), `{0}`, `{1:dd/MM/yyyy HH:mm}`)'>", item.sku, item.transferDate));
                        html.AppendLine("           <i class='fa fa-chevron-down' aria-hidden='true'></i>");
                        html.AppendLine("      </a>");
                    }
                    html.AppendLine("        </td>");
                    html.AppendLine("    </tr>");

                    foreach (var subItem in item.children)
                    {
                        html.AppendLine(String.Format("    <tr class='{0} child-row hide' data-sku='{0}' data-transfer-date='{1:dd/MM/yyyy HH:mm}'>", item.sku, item.transferDate));
                        html.AppendLine("        <td></td>");
                        html.AppendLine("        <td></td>");
                        html.AppendLine("        <td>");
                        html.AppendLine(String.Format("            <a href='/xem-san-pham?sku={0}'>", subItem.sku));
                        html.AppendLine(String.Format("                <img src='{0}'/>", Thumbnail.getURL(subItem.image, Thumbnail.Size.Small)));
                        html.AppendLine("            </a>");
                        html.AppendLine("        </td>");
                        html.AppendLine(String.Format("        <td data-title='Mã' class='customer-name-link'>{0}</td>", subItem.sku));
                        html.AppendLine(String.Format("        <td data-title='Số lượng'>{0:N0}</td>", subItem.quantityTransfer));
                        html.AppendLine("        <td data-title='Kho hiện tại'>");
                        html.AppendLine(String.Format("            <a target='_blank' href='/thong-ke-san-pham?SKU={0}'>{1:N0}</a>", subItem.sku, subItem.quantityAvailable));
                        html.AppendLine("        </td>");
                        html.AppendLine("        <td data-title='Danh mục'></td>");
                        html.AppendLine(String.Format("        <td data-title='Ngày chuyển'>{0:dd/MM/yyyy HH:mm}</td>", subItem.transferDate));
                        html.AppendLine("        <td data-title='Thao tác' class='update-button'></td>");
                        html.AppendLine("</tr>");
                    }
                }

            }
            else
            {
                html.Append("<tr><td colspan='11'>Không tìm thấy sản phẩm...</td></tr>");
            }
            html.Append("</tbody>");

            ltrList.Text = html.ToString();
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
            string request = "/thong-ke-chuyen-kho?";

            if (search != "")
                request += "&textsearch=" + search;
            if (ddlCategory.SelectedValue != "0")
                request += "&categoryid=" + ddlCategory.SelectedValue;
            if (rFromDate.SelectedDate.HasValue)
                request += "&fromdate=" + rFromDate.SelectedDate.ToString();
            if (rToDate.SelectedDate.HasValue)
                request += "&todate=" + rToDate.SelectedDate.ToString();
            // Add filter stock
            if (!String.IsNullOrEmpty(ddlStock.SelectedValue))
                request += "&stock=" + ddlStock.SelectedValue;

            Response.Redirect(request);
        }
    }
}