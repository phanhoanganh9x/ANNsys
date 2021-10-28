using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Models.Pages.dang_ky_gui_di;
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
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static IM_PJ.Controllers.OrderController;

namespace IM_PJ
{
    public partial class danh_sach_chuyen_khoan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Kiểm tra cookies
                if (Request.Cookies["usernameLoginSystem_ANN123"] == null)
                {
                    Response.Redirect("/dang-nhap");
                    return;
                }
                #endregion

                #region Kiểm tra user name
                string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                var acc = AccountController.GetByUsername(username);

                if (acc == null || (acc.RoleID != 0 && acc.RoleID != 2))
                {
                    Response.Redirect("/dang-nhap");
                    return;
                }
                #endregion

                // Tình trạng đơn hàng
                _loadStatus();

                // Nhân viên tạo đơn
                if (acc.RoleID == 0)
                    LoadCreatedBy();
                else if (acc.RoleID == 2)
                    LoadCreatedBy(acc);

                // Danh sách đơn chuyển khoản
                LoadData();
            }
        }

        #region Private
        private void _loadStatus()
        {
            #region Khởi tạo API
            var api = "http://ann-shop-dotnet-core.com/api/v1/order/statuses?page=danh-sach-don-hang";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(api);

            httpWebRequest.Method = "GET";
            #endregion

            try
            {
                // Thực thi API
                var response = (HttpWebResponse)httpWebRequest.GetResponse();

                ddlExcuteStatus.Items.Clear();
                ddlExcuteStatus.Items.Add(new ListItem("Xử lý", ""));

                if (response.StatusCode == HttpStatusCode.OK)
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var statuses = JsonConvert.DeserializeObject<IList<KeyValueModel>>(reader.ReadToEnd());
                        var listItems = statuses
                            .Select(x => new ListItem(x.value, x.key.ToString()))
                            .ToArray();

                        ddlExcuteStatus.Items.AddRange(listItems);
                    }

                ddlExcuteStatus.DataBind();
            }
            catch (WebException we)
            {
                throw we;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        private void LoadCreatedBy(tbl_Account acc = null)
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

                // Add Customer Bank drop down list
                var cusBanks = BankController.getDropDownList();
                cusBanks[0].Text = "Ngân hàng chuyển";
                ddlCustomerBank.Items.Clear();
                ddlCustomerBank.Items.AddRange(cusBanks.ToArray());
                ddlCustomerBank.DataBind();

                // Add Account Bank drop down list
                var accBanks = BankAccountController.getDropDownList();
                accBanks[0].Text = "Ngân hàng nhận";
                // Filter at header
                ddlBankReceive.Items.Clear();
                ddlBankReceive.Items.AddRange(accBanks.ToArray());
                ddlBankReceive.DataBind();
                // Confirm modal
                ddlAccoutBank.Items.Clear();
                ddlAccoutBank.Items.AddRange(accBanks.ToArray());
                ddlAccoutBank.DataBind();

                // Add Order status
                ddlStatus.Items.Clear();
                ddlStatus.Items.Add(new ListItem("Trạng thái giao dịch", "0"));
                ddlStatus.Items.Add(new ListItem("Đã nhận tiền", "1"));
                ddlStatus.Items.Add(new ListItem("Chưa nhận tiền", "2"));
                ddlStatus.SelectedIndex = 0;
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

                // Filter Order Date

                DateTime OrderFromDate = DateConfig;
                DateTime OrderToDate = DateTime.Now;

                if (!String.IsNullOrEmpty(Request.QueryString["orderfromdate"]))
                {
                    OrderFromDate = Convert.ToDateTime(Request.QueryString["orderfromdate"]);
                }

                if (!String.IsNullOrEmpty(Request.QueryString["ordertodate"]))
                {
                    OrderToDate = Convert.ToDateTime(Request.QueryString["ordertodate"]).AddDays(1).AddMinutes(-1);
                }

                rOrderFromDate.SelectedDate = OrderFromDate;
                rOrderFromDate.MinDate = DateConfig;
                rOrderFromDate.MaxDate = DateTime.Now;

                rOrderToDate.SelectedDate = OrderToDate;
                rOrderToDate.MinDate = DateConfig;
                rOrderToDate.MaxDate = DateTime.Now;

                int TransferStatus = 0;
                var ExcuteStatus = new List<int>() { 1, 2, 5, 4 };
                int BankReceive = 0;
                string TextSearch = "";
                string CreatedBy = "";
                int Page = 1;

                if (Request.QueryString["textsearch"] != null)
                {
                    TextSearch = Request.QueryString["textsearch"].Trim();
                }
                if (Request.QueryString["transferstatus"] != null)
                {
                    TransferStatus = Request.QueryString["transferstatus"].ToInt(0);
                }
                if (Request.QueryString["excutestatus"] != null)
                {
                    ExcuteStatus.Clear();
                    ExcuteStatus.Add(Request.QueryString["excutestatus"].ToInt(0));
                }
                if (Request.QueryString["createdby"] != null)
                {
                    CreatedBy = Request.QueryString["createdby"];
                }
                if (Request.QueryString["bankreceive"] != null)
                {
                    BankReceive = Request.QueryString["bankreceive"].ToInt(0);
                }
                if (Request.QueryString["Page"] != null)
                {
                    Page = Request.QueryString["Page"].ToInt();
                }

                txtSearchOrder.Text = TextSearch;
                ddlTransferStatus.SelectedValue = TransferStatus.ToString();
                ddlExcuteStatus.SelectedValue = ExcuteStatus.Count() > 1 ? "0" : ExcuteStatus.FirstOrDefault().ToString();
                ddlBankReceive.SelectedValue = BankReceive.ToString();
                ddlCreatedBy.SelectedValue = CreatedBy.ToString();

                // Create order fileter
                var filter = new OrderFilterModel() {
                    searchType = (int)SearchType.Order,
                    search = TextSearch,
                    excuteStatus = ExcuteStatus,
                    transferStatus = TransferStatus,
                    paymentType = (int)PaymentType.Bank,
                    orderCreatedBy = CreatedBy,
                    orderFromDate = OrderFromDate,
                    orderToDate = OrderToDate,
                    bankReceive = BankReceive
                };
                // Create pagination
                var page = new PaginationMetadataModel() {
                    currentPage = Page
                };
                List<OrderList> rs = new List<OrderList>();
                rs = OrderController.Filter(filter, ref page);

                pagingall(rs, page);

                ltrNumberOfOrder.Text = page.totalCount.ToString();
            }
        }


        #region Paging
        public void pagingall(List<OrderList> acs, PaginationMetadataModel page)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

            StringBuilder html = new StringBuilder();
            html.Append("<thead>");
            html.Append("<tr>");
            html.Append("    <th>Mã</th>");
            html.Append("    <th class='col-customer'>Khách hàng</th>");
            html.Append("    <th>Mua</th>");
            html.Append("    <th>Xử lý đơn</th>");
            html.Append("    <th>Ngân hàng</th>");
            html.Append("    <th>Trạng thái</th>");
            html.Append("    <th>Tổng đơn</th>");
            html.Append("    <th>Đã nhận</th>");
            html.Append("    <th>Ngày nhận tiền</th>");
            html.Append("    <th>Hoàn tất đơn</th>");
            if (acc.RoleID == 0)
            {
                html.Append("    <th>Nhân viên</th>");
            }
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
                    TrTag.AppendLine(String.Format("data-orderid='{0}' ", item.ID));
                    TrTag.AppendLine(String.Format("data-cusID='{0}' ", item.CustomerID));
                    TrTag.AppendLine(String.Format("data-cusbankid='{0:#}' ", item.CusBankID));
                    TrTag.AppendLine(String.Format("data-cusbankname='{0}' ", item.CusBankName));
                    TrTag.AppendLine(String.Format("data-accbankid='{0:#}' ", item.AccBankID));
                    TrTag.AppendLine(String.Format("data-accbankname='{0}' ", item.AccBankName));
                    TrTag.AppendLine(String.Format("data-statusid='{0:#}' ", item.TransferStatus));
                    TrTag.AppendLine(String.Format("data-statusname='{0}' ", item.StatusName));
                    TrTag.AppendLine(String.Format("data-price='{0:#}' ", Convert.ToDouble(item.TotalPrice - item.TotalRefund)));
                    TrTag.AppendLine(String.Format("data-moneyreceived='{0:#}' ", item.MoneyReceive != 0 ? item.MoneyReceive : Convert.ToDecimal(item.TotalPrice - item.TotalRefund)));
                    TrTag.AppendLine(String.Format("data-doneat='{0:dd/MM/yyyy HH:mm}' ", item.DoneAt));
                    TrTag.AppendLine(String.Format("data-transfernote='{0}' ", item.TransferNote));
                    TrTag.AppendLine("/>");

                    html.Append(TrTag.ToString());
                    html.Append("   <td data-title='Mã đơn'><a href=\"/thong-tin-don-hang?id=" + item.ID + "\">" + item.ID + "</a></td>");
                    if (!string.IsNullOrEmpty(item.Nick))
                    {
                        html.Append("   <td data-title='Khách hàng' class='customer-td'><a class=\"col-customer-name-link\" href=\"/thong-tin-don-hang?id=" + item.ID + "\">" + item.Nick.ToTitleCase() + "</a><br><span class=\"name-bottom-nick\">(" + item.CustomerName.ToTitleCase() + ")</span></td>");
                    }
                    else
                    {
                        html.Append("   <td data-title='Khách hàng' class='customer-td'><a class=\"col-customer-name-link\" href=\"/thong-tin-don-hang?id=" + item.ID + "\">" + item.CustomerName.ToTitleCase() + "</a></td>");
                    }
                    html.Append("   <td data-title='Đã mua'>" + item.Quantity + "</td>");
                    html.Append("   <td data-title='Xử lý đơn'>" + PJUtils.OrderExcuteStatus(Convert.ToInt32(item.ExcuteStatus)) + "</td>");
                    html.Append("   <td data-title='Ngân hàng' id='accBankName'>" + item.AccBankName + "</td>");
                    if (item.TransferStatus == 1)
                    {
                        html.Append("   <td id='statusName' data-title='Trạng thái'><span class='bg-green'>" + item.StatusName + "</span></td>");
                    }
                    else
                    {
                        html.Append("   <td id='statusName' data-title='Trạng thái'><span class='bg-red'>" + (string.IsNullOrEmpty(item.StatusName) ? "Chưa nhận tiền" : item.StatusName) + "</span></td>");
                    }
                    html.Append("   <td data-title='Tổng tiền'><strong>" + String.Format("{0:#,###}", Convert.ToDouble(item.TotalPrice - item.TotalRefund)) + "</strong></td>");
                    if (item.TransferStatus == 1)
                    {
                        html.Append("   <td data-title='Đã nhận' id='moneyReceive'><strong>" + String.Format("{0:#,###}", item.MoneyReceive) + "</strong></td>");
                        html.Append("   <td data-title='Ngày nhận tiền' id='doneAt'>" + String.Format("{0:dd/MM HH:mm}", item.DoneAt) + "</td>");
                    }
                    else
                    {
                        html.Append("   <td data-title='Đã nhận' id='moneyReceive'></td>");
                        html.Append("   <td data-title='Ngày nhận tiền' id='doneAt'></td>");
                    }

                    string datedone = "";
                    if (item.ExcuteStatus == (int)ExcuteStatus.Done || item.ExcuteStatus == (int)ExcuteStatus.Sent || item.ExcuteStatus == (int)ExcuteStatus.Return)
                    {
                        datedone = string.Format("{0:dd/MM}", item.DateDone);
                    }
                    html.Append("   <td data-title='Ngày hoàn tất đơn'>" + datedone + "</td>");

                    if (acc.RoleID == 0)
                    {
                        html.Append("   <td data-title='Nhân viên tạo đơn'>" + item.CreatedBy + "</td>");
                    }
                    html.Append("   <td data-title='Thao tác' class='update-button'>");
                    html.Append("       <button type='button' class='btn primary-btn h45-btn' data-toggle='modal' data-target='#TransferBankModal' data-backdrop='static' data-keyboard='false' title='Cập nhật thông tin chuyển khoản'><span class='glyphicon glyphicon-edit'></span></button>");
                    html.Append("   </td>");
                    html.Append("</tr>");

                    // thông tin thêm
                    html.Append("<tr class='tr-more-info'>");
                    html.Append("<td colspan='1'></td>");
                    html.Append("<td colspan='12'>");

                    if (item.TotalRefund != 0)
                    {
                        html.Append("<span class='order-info'><strong>Trừ hàng trả:</strong> " + string.Format("{0:N0}", item.TotalRefund) + " (<a href='xem-don-hang-doi-tra?id=" + item.RefundsGoodsID + "' target='_blank'>Xem đơn " + item.RefundsGoodsID + "</a>)</span>");
                    }

                    if (item.TotalDiscount > 0)
                    {
                        html.Append("<span class='order-info'><strong>Chiết khấu:</strong> " + string.Format("{0:N0}", Convert.ToDouble(item.TotalDiscount)) + "</span>");
                    }
                    if (item.OtherFeeValue != 0)
                    {
                        html.Append("<span class='order-info'><strong>Phí khác:</strong> " + string.Format("{0:N0}", Convert.ToDouble(item.OtherFeeValue)) + " (<a href='#feeInfoModal' data-toggle='modal' data-backdrop='static' onclick='openFeeInfoModal(" + item.ID + ")'>" + item.OtherFeeName.Trim() + "</a>)</span>");
                    }
                    if (item.FeeShipping > 0)
                    {
                        html.Append("<span class='order-info'><strong>Phí vận chuyển:</strong> " + string.Format("{0:N0}", Convert.ToDouble(item.FeeShipping)) + "</span>");
                    }
                    if (!string.IsNullOrEmpty(item.ShippingCode))
                    {
                        string moreInfo = "";
                        if (item.ShippingType == 3)
                        {
                            moreInfo = " (<a href='https://proship.vn/quan-ly-van-don/?isInvoiceFilter=1&amp;generalInfo=" + item.ShippingCode + "' target='_blank'>Xem</a>)";
                        }
                        if (item.ShippingType == 2)
                        {
                            moreInfo = " (Chuyển " + ((item.PostalDeliveryType == 1) ? "thường" : "nhanh") + ")";
                        }
                        html.Append("<span class='order-info'><strong>Mã vận đơn:</strong> " + item.ShippingCode + moreInfo + "</span>");
                    }
                    if (item.ShippingType == 4)
                    {
                        if (item.TransportCompanyID.HasValue && item.TransportCompanyID != 0)
                        {
                            var transport = TransportCompanyController.GetTransportCompanyForOrderList(Convert.ToInt32(item.TransportCompanyID));
                            var transportsub = TransportCompanyController.GetReceivePlaceForOrderList(Convert.ToInt32(item.TransportCompanyID), Convert.ToInt32(item.TransportCompanySubID));
                            html.Append("<span class='order-info'><strong>Gửi xe: </strong> " + transport.CompanyName.ToTitleCase() + " (" + transportsub.ShipTo.ToTitleCase() + ")</span>");
                        }
                    }
                    if (!string.IsNullOrEmpty(item.OrderNote))
                    {
                        html.Append("<span class='order-info'><strong>Ghi chú:</strong> " + item.OrderNote + "</span>");
                    }
                    html.Append("</td>");
                    html.Append("</tr>");
                }

            }
            else
            {
                if (acc.RoleID == 0)
                {
                    html.Append("<tr><td colspan=\"13\">Không tìm thấy đơn hàng...</td></tr>");
                }
                else
                {
                    html.Append("<tr><td colspan=\"12\">Không tìm thấy đơn hàng...</td></tr>");
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
            string request = "/danh-sach-chuyen-khoan?";

            if (search != "")
            {
                request += "&textsearch=" + search;
            }

            if (ddlExcuteStatus.SelectedValue != "")
            {
                request += "&excutestatus=" + ddlExcuteStatus.SelectedValue;
            }

            if (ddlTransferStatus.SelectedValue != "")
            {
                request += "&transferstatus=" + ddlTransferStatus.SelectedValue;
            }

            if (ddlCreatedBy.SelectedValue != "")
            {
                request += "&createdby=" + ddlCreatedBy.SelectedValue;
            }

            if (rOrderFromDate.SelectedDate.HasValue)
            {
                request += "&orderfromdate=" + rOrderFromDate.SelectedDate.ToString();
            }

            if (rOrderToDate.SelectedDate.HasValue)
            {
                request += "&ordertodate=" + rOrderToDate.SelectedDate.ToString();
            }

            if (ddlBankReceive.SelectedValue != "0")
            {
                request += "&bankreceive=" + ddlBankReceive.SelectedValue;
            }

            Response.Redirect(request);
        }

        [WebMethod]
        public static string getTransferLast(int orderID, int cusID)
        {
            var last = BankTransferController.getTransferLast(orderID, cusID);
            return new JavaScriptSerializer().Serialize(last);
        }

        [WebMethod]
        public static void updateTransfer(BankTransfer transfer)
        {
            string username = HttpContext.Current.Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

            // Update transfer infor
            transfer.UUID = Guid.NewGuid();
            transfer.CreatedBy = acc.ID;
            transfer.CreatedDate = DateTime.Now;
            transfer.ModifiedBy = acc.ID;
            transfer.ModifiedDate = DateTime.Now;

            BankTransferController.Update(transfer);
        }
    }
}