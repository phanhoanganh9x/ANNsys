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
using System.Web.UI.WebControls;
using static IM_PJ.Controllers.OrderController;

namespace IM_PJ
{
    public partial class danh_sach_don_dat_hang : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Check Author
                if (Request.Cookies["usernameLoginSystem"] != null)
                {
                    var username = Request.Cookies["usernameLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);

                    if (acc != null)
                    {
                        var allowRole = new List<int>() { 0, 2 };

                        if (allowRole.Where(x => x == acc.RoleID).Any())
                            _initPage(acc);
                        else
                            Response.Redirect("/trang-chu");
                    }
                    else
                        Response.Redirect("/dang-nhap");
                }
                else
                    Response.Redirect("/dang-nhap");
            }
        }

        #region Private
        /// <summary>
        /// Cài đặt datetime picker fromDate và toDate
        /// </summary>
        private void _initDatetimePicker()
        {
            // ẩn sản phẩm theo thời gian
            var dateConfig = new DateTime(2019, 12, 15);
            var config = ConfigController.GetByTop1();

            if (config.ViewAllOrders == 1)
                dateConfig = new DateTime(2018, 6, 22);
            else if (config.ViewAllReports == 0)
                dateConfig = DateTime.Now.AddMonths(-2);

            #region Cài đặt fromDate
            var fromDate = dateConfig;
            var queryFromDate = Request.QueryString["fromDate"];

            if (!String.IsNullOrEmpty(queryFromDate))
                fromDate = Convert.ToDateTime(queryFromDate);

            rOrderFromDate.SelectedDate = fromDate;
            rOrderFromDate.MinDate = dateConfig;
            rOrderFromDate.MaxDate = DateTime.Now;
            #endregion

            #region Cài đặt toDate
            var toDate = DateTime.Now;
            var queryToDate = Request.QueryString["toDate"];

            if (!String.IsNullOrEmpty(queryToDate))
                toDate = Convert.ToDateTime(queryToDate).AddDays(1).AddMinutes(-1);

            rOrderToDate.SelectedDate = toDate;
            rOrderToDate.MinDate = dateConfig;
            rOrderToDate.MaxDate = DateTime.Now;
            #endregion
        }

        private void _initCreatedBy(tbl_Account acc = null)
        {
            if (acc != null)
            {
                ddlCreatedBy.Items.Clear();
                ddlCreatedBy.Items.Insert(0, new ListItem(acc.Username, acc.Username));
            }
            else
            {
                var CreateBy = AccountController.GetAllNotSearch().Where(x => x.RoleID == 0 || x.RoleID == 2).ToList();
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

        private void _initPage(tbl_Account acc)
        {
            _initDatetimePicker();
            _initCreatedBy(acc.RoleID == 2 ? acc : null);
        }
        #endregion

        #region Paging
        public void pagingall(List<OrderList> acs, PaginationMetadataModel page)
        {
            string username = Request.Cookies["usernameLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);

            StringBuilder html = new StringBuilder();
            html.AppendLine("<thead>");
            html.AppendLine("<tr>");
            html.AppendLine("    <th>Mã</th>");
            html.AppendLine("    <th></th>");
            html.AppendLine("    <th class='col-customer'>Khách hàng</th>");
            html.AppendLine("    <th>Mua</th>");
            html.AppendLine("    <th>Xử lý</th>");
            html.AppendLine("    <th>Thanh toán</th>");
            html.AppendLine("    <th>Kiểu thanh toán</th>");
            html.AppendLine("    <th>Giao hàng</th>");
            html.AppendLine("    <th>Tổng tiền</th>");
            if (acc.RoleID == 0)
            {
                html.AppendLine("    <th>Nhân viên</th>");
            }
            html.AppendLine("    <th>Ngày tạo</th>");
            html.AppendLine("    <th>Hoàn tất</th>");
            html.AppendLine("    <th></th>");
            html.AppendLine("</tr>");
            html.AppendLine("</thead>");

            html.AppendLine("<tbody>");
            if (acs.Count > 0)
            {
                PageCount = page.totalPages;
                Int32 Page = page.currentPage;

                foreach(var item in acs)
                {
                    html.AppendLine("<tr>");
                    html.AppendLine("   <td data-title='Mã đơn'><a target='_blank' href='/thong-tin-don-hang?id=" + item.ID + "'>" + item.ID + "</a></td>");
                    html.AppendLine("   <td data-title='Loại đơn'>" + PJUtils.OrderTypeStatus(Convert.ToInt32(item.OrderType)) + "</td>");

                    if (!string.IsNullOrEmpty(item.Nick))
                    {
                        html.AppendLine("   <td data-title='Khách hàng' class='customer-td'><a class='col-customer-name-link' target='_blank' href='/thong-tin-don-hang?id=" + item.ID + "'>" + item.Nick.ToTitleCase() + "</a><br><span class='name-bottom-nick'>(" + item.CustomerName.ToTitleCase() + ")</span></td>");
                    }
                    else
                    {
                        html.AppendLine("   <td data-title='Khách hàng' class='customer-td'><a class='col-customer-name-link' target='_blank' href='/thong-tin-don-hang?id=" + item.ID + "'>" + item.CustomerName.ToTitleCase() + "</a></td>");
                    }

                    html.AppendLine("   <td data-title='Đã mua'>" + item.Quantity + "</td>");
                    if (acc.RoleID == 0 && item.ExcuteStatus == 2)
                        html.AppendLine("   <td data-title='Xử lý'><span class='bg-green' style='cursor: pointer' onclick='onClick_spFinishStatusOrder(this, " + item.ID + ")'>Đã hoàn tất</span></td>");
                    else
                        html.AppendLine("   <td data-title='Xử lý'>" + PJUtils.OrderExcuteStatus(Convert.ToInt32(item.ExcuteStatus)) + "</td>");
                    html.AppendLine("   <td data-title='Thanh toán'>" + PJUtils.OrderPaymentStatus(Convert.ToInt32(item.PaymentStatus)) + "</td>");

                    #region Phương thức thanh toán
                    html.AppendLine("   <td data-title='Kiểu thanh toán' class='payment-type'>");
                    html.AppendLine(PJUtils.PaymentType(Convert.ToInt32(item.PaymentType)));
                    // Đã nhận tiền chuyển khoản
                    if(item.PaymentType == 2)
                    {
                        if (item.TransferStatus.HasValue && item.TransferStatus.Value == 1)
                        {
                            html.AppendLine("       <br/><div class='new-status-btn'><span class='bg-green'>Đã nhận tiền</span></div>");
                        }
                        else
                        {
                            if (acc.RoleID == 0)
                                html.AppendLine("       <br/><a class='new-status-btn' target='_blank' href='/danh-sach-chuyen-khoan?&textsearch=" + item.ID + "'><span class='bg-black'>Cập nhật</span></a>");
                        }
                    }
                    html.AppendLine("   </td>");
                    #endregion


                    #region Giao hàng
                    html.AppendLine("   <td data-title='Giao hàng' class='shipping-type'>");
                    html.AppendLine(PJUtils.ShippingType(Convert.ToInt32(item.ShippingType)));
                    // Đã giao hàng
                    if (item.DeliveryStatus.HasValue && item.DeliveryStatus.Value == 1)
                        html.AppendLine("       <br/><div class='new-status-btn'><span class='bg-green'>Đã giao</span></div>");
                    html.AppendLine("   </td>");
                    #endregion

                    // Tổng tiền
                    if (acc.RoleID == 0)
                    {
                        html.AppendLine("   <td data-title='Tổng tiền'>");
                        html.AppendLine(String.Format("        <strong>{0:N0}</strong>", item.TotalPrice - item.TotalRefund));
                        html.AppendLine(String.Format("        <br/><span class='{0}'><strong>{1:N0}</strong></span>", item.TotalProfit > 0 ? "bg-green" : "bg-red", item.TotalProfit));
                        html.AppendLine("   </td>");
                    }
                    else
                    {
                        html.AppendLine(String.Format("   <td data-title='Tổng tiền'><strong>{0:N0}</strong></td>", item.TotalPrice - item.TotalRefund));
                    }

                    if (acc.RoleID == 0)
                    {
                        html.AppendLine("   <td data-title='Nhân viên'>" + item.CreatedBy + "</td>");
                    }

                    string date = string.Format("<strong>{0:dd/MM}</strong><br>{0:HH:mm}", item.CreatedDate);
                    html.AppendLine("   <td data-title='Ngày tạo'>" + date + "</td>");

                    string datedone = "";
                    if (item.ExcuteStatus == 2)
                    {
                        datedone = string.Format("<strong>{0:dd/MM}</strong><br>{0:HH:mm}", item.DateDone);
                    }
                    html.AppendLine("   <td data-title='Hoàn tất'>" + datedone + "</td>");

                    html.AppendLine("   <td data-title='Thao tác' class='update-button'>");
                    html.AppendLine("       <a href='/print-invoice?id=" + item.ID + "' title='In hóa đơn' target='_blank' class='btn primary-btn h45-btn'><i class='fa fa-print' aria-hidden='true'></i></a>");
                    html.AppendLine("       <a href='/print-shipping-note?id=" + item.ID + "' title='In phiếu gửi hàng' target='_blank' class='btn primary-btn btn-red h45-btn'><i class='fa fa-file-text-o' aria-hidden='true'></i></a>");
                    html.AppendLine("       <a href='/chi-tiet-khach-hang?id=" + item.CustomerID + "' title='Thông tin khách hàng " + item.CustomerName + "' target='_blank' class='btn primary-btn btn-black h45-btn'><i class='fa fa-user-circle' aria-hidden='true'></i></a>");
                    if (item.DeliveryStatus.HasValue && item.DeliveryStatus.Value == 1 && !string.IsNullOrEmpty(item.InvoiceImage))
                        html.AppendLine("       <a href='javascript:;' onclick='openImageInvoice($(this))' data-link='" + item.InvoiceImage + "' title='Biên nhận gửi hàng' class='btn primary-btn btn-blue h45-btn'><i class='fa fa-file-text-o' aria-hidden='true'></i></a>");
                    html.AppendLine("       <a href='javascript:;' onclick='copyInvoiceURL(" + item.ID + ", " + item.CustomerID + ")' title='Copy link hóa đơn' class='btn primary-btn btn-violet h45-btn'><i class='glyphicon glyphicon-list-alt' aria-hidden='true'></i></a>");
                    html.AppendLine("   </td>");
                    html.AppendLine("</tr>");

                    // thông tin thêm
                    html.AppendLine("<tr class='tr-more-info'>");
                    html.AppendLine("<td colspan='2'></td>");
                    html.AppendLine("<td colspan='11'>");

                    if(item.TotalRefund != 0)
                    {
                        html.AppendLine("<span class='order-info'><strong>Đổi trả:</strong> -" + string.Format("{0:N0}", item.TotalRefund) + " (<a href='xem-don-hang-doi-tra?id=" + item.RefundsGoodsID + "' target='_blank'>Đơn " + item.RefundsGoodsID + "</a>)</span>");
                    }

                    if (item.TotalDiscount > 0)
                    {
                        html.AppendLine("<span class='order-info'><strong>Chiết khấu:</strong> -" + string.Format("{0:N0}", Convert.ToDouble(item.TotalDiscount)) + "</span>");
                    }
                    if (item.OtherFeeValue != 0)
                    {
                        html.AppendLine("<span class='order-info'><strong>Phí khác:</strong> " + string.Format("{0:N0}", Convert.ToDouble(item.OtherFeeValue)) + " (<a href='#feeInfoModal' data-toggle='modal' data-backdrop='static' onclick='onClick_aFeeInfoModal(" + item.ID + ")'>" + item.OtherFeeName.Trim() + "</a>)</span>");
                    }
                    if (item.ShippingType == 4)
                    {
                        if (item.TransportCompanyID != 0)
                        {
                            var transport = TransportCompanyController.GetTransportCompanyForOrderList(Convert.ToInt32(item.TransportCompanyID));
                            var transportsub = TransportCompanyController.GetReceivePlaceForOrderList(Convert.ToInt32(item.TransportCompanyID), Convert.ToInt32(item.TransportCompanySubID));
                            html.AppendLine("<span class='order-info'><strong>Gửi xe: </strong> " + transport.CompanyName.ToTitleCase() + " (" + transportsub.ShipTo.ToTitleCase() + ")</span>");
                        }
                    }
                    if (!string.IsNullOrEmpty(item.ShippingCode))
                    {
                        string moreInfo = "";
                        if (item.ShippingType == 3 && !String.IsNullOrEmpty(item.ShippingCode))
                        {
                            moreInfo = " (<a href='https://proship.vn/quan-ly-van-don/?isInvoiceFilter=1&amp;generalInfo=" + item.ShippingCode + "' target='_blank'>Xem</a>)";
                        }
                        if (item.ShippingType == 2)
                        {
                            moreInfo = " (Chuyển " + ((item.PostalDeliveryType == 1) ? "thường" : "nhanh") + ")";
                        }
                        if (item.ShippingType == 6 && !String.IsNullOrEmpty(item.ShippingCode))
                        {
                            moreInfo = " (<a href='https://khachhang.giaohangtietkiem.vn/khachhang?code=" + item.ShippingCode + "' target='_blank'>Xem</a>)";
                        }
                        html.AppendLine("<span class='order-info'><strong>Vận đơn:</strong> " + item.ShippingCode + moreInfo + "</span>");
                    }
                    if (item.FeeShipping > 0)
                    {
                        html.AppendLine("<span class='order-info'><strong>Ship:</strong> " + string.Format("{0:N0}", Convert.ToDouble(item.FeeShipping)) + "</span>");
                    }
                    if ((item.ShippingType == 4 || item.ShippingType == 5) && !string.IsNullOrEmpty(item.ShipperName))
                    {
                        html.AppendLine("<span class='order-info'><strong>Shipper:</strong> " + item.ShipperName + "</span>");
                    }
                    if (!string.IsNullOrEmpty(item.CouponCode))
                    {
                        html.AppendLine(String.Format("<span class='order-info'><strong>Coupon ({0}):</strong> -{1:N0}</span>", item.CouponCode.Trim().ToUpper(), item.CouponValue));
                    }
                    if (!string.IsNullOrEmpty(item.OrderNote))
                    {
                        html.AppendLine("<span class='order-info'><strong>Ghi chú:</strong> " + item.OrderNote + "</span>");
                    }
                    html.AppendLine("</td>");
                    html.AppendLine("</tr>");
                }

            }
            else
            {
                if (acc.RoleID == 0)
                {
                    html.AppendLine("<tr><td colspan='13'>Không tìm thấy đơn hàng...</td></tr>");
                }
                else
                {
                    html.AppendLine("<tr><td colspan='12'>Không tìm thấy đơn hàng...</td></tr>");
                }
            }
            html.AppendLine("</tbody>");
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
            //output.AppendLine("<div class=\"paging\">");
            output.AppendLine("<ul>");

            //Link First(Trang đầu) và Previous(Trang trước)
            if (currentPage > 1)
            {
                output.AppendLine("<li><a title=\"" + strText[0] + "\" href=\"" + string.Format(pageUrl, 1) + "\">Trang đầu</a></li>");
                output.AppendLine("<li><a title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\">Trang trước</a></li>");
                //output.AppendLine("<li class=\"UnselectedPrev\" ><a title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\"><i class=\"fa fa-angle-left\"></i></a></li>");

                //output.AppendLine("<span class=\"Unselect_prev\"><a href=\"" + string.Format(pageUrl, currentPage - 1) + "\"></a></span>");
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
                output.AppendLine("<li><a href=\"" + string.Format(GetPageUrl(currentPage - 1, pageUrl), startPageNumbersFrom - 1) + "\">&hellip;</a></li>");
            }

            //Duyệt vòng for hiển thị các trang
            for (int i = startPageNumbersFrom; i <= stopPageNumbersAt; i++)
            {
                if (currentPage == i)
                {
                    output.AppendLine("<li class=\"current\" ><a >" + i.ToString() + "</a> </li>");
                }
                else
                {
                    output.AppendLine("<li><a href=\"" + string.Format(pageUrl, i) + "\">" + i.ToString() + "</a> </li>");
                }
            }

            //Các dấu ... chỉ những trang tiếp theo
            if (stopPageNumbersAt < pageCount)
            {
                output.AppendLine("<li><a href=\"" + string.Format(pageUrl, stopPageNumbersAt + 1) + "\">&hellip;</a></li>");
            }

            //Link Next(Trang tiếp) và Last(Trang cuối)
            if (currentPage != pageCount)
            {
                //output.AppendLine("<span class=\"Unselect_next\"><a href=\"" + string.Format(pageUrl, currentPage + 1) + "\"></a></span>");
                //output.AppendLine("<li class=\"UnselectedNext\" ><a title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\"><i class=\"fa fa-angle-right\"></i></a></li>");
                output.AppendLine("<li><a title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\">Trang sau</a></li>");
                output.AppendLine("<li><a title=\"" + strText[3] + "\" href=\"" + string.Format(pageUrl, pageCount) + "\">Trang cuối</a></li>");
            }
            output.AppendLine("</ul>");
            //output.AppendLine("</div>");
            return output.ToString();
        }
        #endregion
    }
}
