﻿using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Utils;
using MB.Extensions;
using Newtonsoft.Json;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using static IM_PJ.Controllers.OrderController;

namespace IM_PJ
{
    public partial class danh_sach_nhap_hang : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["userLoginSystem"] != null)
                {
                    string username = Request.Cookies["userLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);

                    if (acc != null)
                    {
                        
                        if (acc.RoleID == 0 || acc.Username == "nhom_zalo406")
                        {
                            LoadCreatedBy();
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
            }
        }

        public void LoadCreatedBy(tbl_Account acc = null)
        {
            if (acc != null)
            {
                ddlCreatedBy.Items.Clear();
                ddlCreatedBy.Items.Insert(0, new ListItem(acc.Username, acc.Username));
            }
            else
            {
                var CreateBy = AccountController.GetAllNotSearch().Where(x => x.RoleID == 2).ToList();
                ddlCreatedBy.Items.Clear();
                ddlCreatedBy.Items.Insert(0, new ListItem("Nhân viên tạo đơn", ""));
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
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                DateTime DateConfig = new DateTime(2019, 2, 15);

                var config = ConfigController.GetByTop1();
                if (config.ViewAllOrders == 1)
                {
                    DateConfig = new DateTime(2018, 6, 22);
                }

                DateTime fromDate = DateConfig;
                DateTime toDate = DateTime.Now;

                if (!String.IsNullOrEmpty(Request.QueryString["orderfromdate"]))
                {
                    fromDate = Convert.ToDateTime(Request.QueryString["orderfromdate"]);
                }

                if (!String.IsNullOrEmpty(Request.QueryString["ordertodate"]))
                {
                    toDate = Convert.ToDateTime(Request.QueryString["ordertodate"]).AddDays(1).AddMinutes(-1);
                }

                rFromDate.SelectedDate = fromDate;
                rFromDate.MinDate = DateConfig;
                rFromDate.MaxDate = DateTime.Now;

                rToDate.SelectedDate = toDate;
                rToDate.MinDate = DateConfig;
                rToDate.MaxDate = DateTime.Now;

                string textSearch = String.Empty;
                int registerStatus = 0;
                string createdBy = String.Empty;
                string color = String.Empty;
                string size = String.Empty;
                var isRegisterProductSession = false;
                int Page = 1;

                if (Request.QueryString["textsearch"] != null)
                    textSearch = Request.QueryString["textsearch"].Trim();
                if (Request.QueryString["registerstatus"] != null)
                    registerStatus = Request.QueryString["registerstatus"].ToInt(0);
                if (Request.QueryString["createdby"] != null)
                    createdBy = Request.QueryString["createdby"];
                if (Request.QueryString["color"] != null)
                    color = Request.QueryString["color"];
                if (Request.QueryString["size"] != null)
                    size = Request.QueryString["size"];
                if (Request.QueryString["isregisterproductsession"] != null)
                    isRegisterProductSession = true;
                if (Request.QueryString["Page"] != null)
                    Page = Request.QueryString["Page"].ToInt();

                txtSearchOrder.Text = textSearch;
                ddlRegisterStatus.SelectedValue = registerStatus.ToString();
                ddlCreatedBy.SelectedValue = createdBy.ToString();

                // Create order fileter
                var filter = new RegisterProductFilterModel()
                {
                    search = textSearch,
                    status = registerStatus,
                    createdBy = createdBy,
                    fromDate = fromDate,
                    toDate = toDate,
                    color = color,
                    size = size,
                    selected = isRegisterProductSession,
                    account = acc
                };
                // Create pagination
                var page = new PaginationMetadataModel()
                {
                    currentPage = Page
                };
                List<RegisterProductList> rs = new List<RegisterProductList>();
                rs = RegisterProductController.Filter(filter, ref page);

                var data = SessionController.getRegisterProductSession(acc);
                hdfSession.Value = JsonConvert.SerializeObject(data);

                pagingall(rs, page);
                ltrNumberOfOrder.Text = page.totalCount.ToString();
            }
        }

        #region Paging
        public void pagingall(List<RegisterProductList> acs, PaginationMetadataModel page)
        {
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);

            StringBuilder html = new StringBuilder();
            html.Append("<thead>");
            html.Append("<tr>");
            html.Append("    <th><input id='checkRegisterAll' type='checkbox' onchange='checkRegisterAll($(this).prop(`checked`))'/></th>");
            html.Append("    <th>Hình ảnh sản phẩm</th>");
            html.Append("    <th class='col-customer'>Khách hàng</th>");
            html.Append("    <th>SKU</th>");
            html.Append("    <th>Tiêu đề</th>");
            html.Append("    <th>Màu</th>");
            html.Append("    <th>Size</th>");
            html.Append("    <th>Số lượng</th>");
            html.Append("    <th>Trạng thái</th>");
            html.Append("    <th>Nhân viên</th>");
            html.Append("    <th>Ngày khởi tạo</th>");
            html.Append("    <th>Ngày dự kiến</th>");
            html.Append("    <th></th>");
            html.Append("</tr>");
            html.Append("</thead>");

            html.Append("<tbody>");
            if (acs.Count > 0)
            {
                PageCount = page.totalPages;
                Int32 Page = page.currentPage;

                foreach (var item in acs)
                {
                    // Insert transfer bank info for tr tag
                    var TrTag = new StringBuilder();
                    TrTag.AppendLine("<tr ");
                    TrTag.AppendLine(String.Format("data-registerid='{0}' ", item.id));
                    TrTag.AppendLine(String.Format("data-customer='{0}' ", item.customer));
                    TrTag.AppendLine(String.Format("data-status='{0}' ", item.status));
                    TrTag.AppendLine(String.Format("data-quantity='{0:#,###}' ", item.quantity));
                    TrTag.AppendLine(String.Format("data-expecteddate='{0:dd/MM/yyyy HH:mm}' ", item.expectedDate));
                    TrTag.AppendLine(String.Format("data-note='{0}' ", item.note));
                    TrTag.AppendLine("/>");

                    html.Append(TrTag.ToString());
                    html.Append("   <td><input type='checkbox' onchange='checkRegister($(this))'/></td>");
                    html.Append("   <td data-title='Hình ảnh sản phẩm'><a href=\"/xem-san-pham?sku=" + item.sku + "\"><img src=\"" + Thumbnail.getURL(item.image, Thumbnail.Size.Small) + "\"/></a></td>");
                    html.Append("   <td data-title='Khách hàng' class='customer-td'>" + item.customer.ToTitleCase() + "</td>");
                    html.Append("   <td data-title='Mã sản phẩm'>" + item.sku + "</td>");
                    html.Append("   <td data-title='Tiêu đề sản phẩm'>" + item.title + "</td>");
                    html.Append("   <td data-title='Màu đặt'>" + item.color + "</td>");
                    html.Append("   <td data-title='Kích thước'>" + item.size + "</td>");
                    html.Append("   <td data-title='Số lượng đặt hàng'>" + String.Format("{0:#,###}", item.quantity) + "</td>");
                    html.Append("   <td id='status' data-title='Trạng thái đơn yêu cầu nhập hàng'>");
                    switch (item.status)
                    {
                        case (int)RegisterProductStatus.Approve:
                            html.Append("   <span class='bg-green'>" + item.statusName + "</span>");
                            break;
                        case (int)RegisterProductStatus.Ordering:
                            html.Append("   <span class='bg-yellow'>" + item.statusName + "</span>");
                            break;
                        case (int)RegisterProductStatus.Done:
                            html.Append("   <span class='bg-blue'>" + item.statusName + "</span>");
                            break;
                        default:
                            html.Append("   <span class='bg-red'>" + item.statusName + "</span>");
                            break;
                    }
                    html.Append("   </td>");
                    html.Append("   <td data-title='Nhân viên tạo đơn'>" + item.staffName + "</td>");
                    html.Append("   <td data-title='Ngày khởi tạo đơn'>" + String.Format("{0:dd/MM}", item.createdDate) + "</td>");
                    html.Append("   <td id='expectedDate' data-title='Ngày dự kiến hàng về'>" + String.Format("{0:dd/MM}", item.expectedDate) + "</td>");
                    html.Append("   <td data-title='Thao tác' class='update-button' id='updateButton'>");
                    html.Append("       <button type='button' class='btn primary-btn h45-btn' data-toggle='modal' data-target='#RegisterProductModal' data-backdrop='static' data-keyboard='false' title='Cập nhật thông yêu cầu nhập hàng'><span class='glyphicon glyphicon-edit'></span></button>");
                    html.Append("   </td>");
                    html.Append("</tr>");

                    // thông tin thêm
                    html.Append("<tr class='tr-more-info'>");
                    html.Append("   <td colspan='2' data-title='Thông tin thêm'></td>");
                    html.Append("   <td colspan='14'>");
                    if (!string.IsNullOrEmpty(item.note))
                    {
                        html.Append("       <span class='order-info'><strong>Ghi chú:</strong> " + item.note + "</span>");
                    }
                    html.Append("   </td>");
                    html.Append("</tr>");
                }

            }
            else
            {
                if (acc.RoleID == 0)
                {
                    html.Append("<tr><td colspan=\"14\">Không tìm thấy đơn hàng...</td></tr>");
                }
                else
                {
                    html.Append("<tr><td colspan=\"13\">Không tìm thấy đơn hàng...</td></tr>");
                }
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
                output.Append("<li><a title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\">Trang sau</a></li>");
                output.Append("<li><a title=\"" + strText[3] + "\" href=\"" + string.Format(pageUrl, pageCount) + "\">Trang cuối</a></li>");
            }
            output.Append("</ul>");
            return output.ToString();
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearchOrder.Text.Trim();
            string request = "/danh-sach-nhap-hang?";
            // Search
            if (!String.IsNullOrEmpty(search))
                request += "&textsearch=" + search;
            // Register Status
            if (ddlRegisterStatus.SelectedValue != "0")
                request += "&registerstatus=" + ddlRegisterStatus.SelectedValue;
            // Staff
            if (!String.IsNullOrEmpty(ddlCreatedBy.SelectedValue))
                request += "&createdby=" + ddlCreatedBy.SelectedValue;
            // Created Date
            if (rFromDate.SelectedDate.HasValue)
            {
                request += "&orderfromdate=" + rFromDate.SelectedDate.ToString();
            }
            if (rToDate.SelectedDate.HasValue)
            {
                request += "&ordertodate=" + rToDate.SelectedDate.ToString();
            }
            // Color
            if (!String.IsNullOrEmpty(ddlColor.SelectedValue))
                request += "&color=" + ddlColor.SelectedValue;
            // Size
            if (!String.IsNullOrEmpty(ddlSize.SelectedValue))
                request += "&size=" + ddlSize.SelectedValue;
            // Add filter selected
            if (Request.QueryString["isregisterproductsession"] != null)
                request += "&isregisterproductsession=1";

            Response.Redirect(request);
        }

        [WebMethod]
        public static void updateRegisterProduct(List<RegisterProductSession> data)
        {
            var username = HttpContext.Current.Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);

            // Update session khi mà thực thi update thông phi đăng ký nhập hàng
            SessionController.updateRegisterProductSession(acc, data);
            RegisterProductController.Update(data, acc);
        }

        [WebMethod]
        public static string addChoose(List<RegisterProductSession> data)
        {
            var username = HttpContext.Current.Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);

            return SessionController.addRegisterProductSession(acc, data);
        }

        [WebMethod]
        public static string deleteChoose(List<RegisterProductSession> data)
        {
            var username = HttpContext.Current.Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);

            return SessionController.deleteRegisterProductSession(acc, data);
        }

        [WebMethod]
        public static void deleteAllChoose()
        {
            var username = HttpContext.Current.Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            SessionController.deleteRegisterProductSession(acc);
        }
    }
}