using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using Newtonsoft.Json;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public partial class danh_sach_van_chuyen : System.Web.UI.Page
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
                        
                        if (acc.RoleID == 0 || acc.Username == "nhom_zalo406" || acc.Username == "hotline" || acc.Username == "nhom_zalo502")
                        {
                            LoadShipper();
                            LoadCreatedBy();
                            LoadTransportCompany();
                            LoadData();
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
            }
        }
        public void LoadTransportCompany()
        {
            var TransportCompany = TransportCompanyController.GetTransportCompany();
            ddlTransportCompany.Items.Clear();
            ddlTransportCompany.Items.Insert(0, new ListItem("Chành xe", "0"));
            // drop down list at update delivery modal
            ddlTransferCompanyModal.Items.Clear();
            ddlTransferCompanyModal.Items.Insert(0, new ListItem("Chành xe", "0"));

            if (TransportCompany.Count > 0)
            {
                foreach (var p in TransportCompany)
                {
                    ListItem listitem = new ListItem(p.CompanyName.ToTitleCase(), p.ID.ToString());
                    ddlTransportCompany.Items.Add(listitem);
                    ddlTransferCompanyModal.Items.Add(listitem);
                }
                ddlTransportCompany.DataBind();
                ddlTransferCompanyModal.DataBind();
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
        public void LoadShipper()
        {
            var shipper = ShipperController.getDropDownList();
            shipper[0].Text = "Nhân viên giao hàng";
            // drop down list at filter page
            ddlShipperFilter.Items.Clear();
            ddlShipperFilter.Items.AddRange(shipper.ToArray());
            ddlShipperFilter.DataBind();
            // drop down list at update delivery modal
            ddlShipperModal.Items.Clear();
            ddlShipperModal.Items.AddRange(shipper.ToArray());
            ddlShipperModal.DataBind();
            // drop down list at print delivery modal
            ddfShipperPrintModal.Items.Clear();
            ddfShipperPrintModal.Items.AddRange(shipper.ToArray());
            ddfShipperPrintModal.DataBind();
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

                string TextSearch = "";
                int TransportCompany = 0;
                var ShippingType = new List<int>() {4, 5};
                int PaymentType = 0;
                int ShipperID = 0;
                int InvoiceStatus = 0;
                int DeliveryStatus = 0;
                string CreatedBy = "";
                var isDeliverySession = false;
                int DeliveryTimes = 0;
                int Page = 1;

                if (Request.QueryString["textsearch"] != null)
                    TextSearch = Request.QueryString["textsearch"].Trim();
                if (Request.QueryString["transportcompany"] != null)
                    TransportCompany = Request.QueryString["transportcompany"].ToInt(0);
                if (Request.QueryString["shippingtype"] != null)
                {
                    ShippingType.Clear();
                    ShippingType.Add(Request.QueryString["shippingtype"].ToInt(0));
                }
                if (Request.QueryString["paymenttype"] != null)
                    PaymentType = Request.QueryString["paymenttype"].ToInt(0);
                if (Request.QueryString["shipperid"] != null)
                    ShipperID = Request.QueryString["shipperid"].ToInt(0);
                if (Request.QueryString["invoicestatus"] != null)
                    InvoiceStatus = Request.QueryString["invoicestatus"].ToInt(0);
                if (Request.QueryString["deliverystatus"] != null)
                    DeliveryStatus = Request.QueryString["deliverystatus"].ToInt(0);
                if (Request.QueryString["createdby"] != null)
                    CreatedBy = Request.QueryString["createdby"];
                if (Request.QueryString["isdeliverysession"] != null)
                    isDeliverySession = true;
                if (Request.QueryString["deliverytimes"] != null)
                    DeliveryTimes = Request.QueryString["deliverytimes"].ToInt(0);
                if (Request.QueryString["Page"] != null)
                    Page = Request.QueryString["Page"].ToInt();

                txtSearchOrder.Text = TextSearch;
                ddlTransportCompany.SelectedValue = TransportCompany.ToString();
                ddlShippingType.SelectedValue = ShippingType.Count() > 1 ? "0" : ShippingType.FirstOrDefault().ToString() ;
                ddlPaymentType.SelectedValue = PaymentType.ToString();
                ddlShipperFilter.SelectedValue = ShipperID.ToString();
                ddlInvoiceStatus.SelectedValue = InvoiceStatus.ToString();
                ddlDeliveryStatusFilter.SelectedValue = DeliveryStatus.ToString();
                ddlCreatedBy.SelectedValue = CreatedBy.ToString();
                ddlDeliveryTimes.SelectedValue = DeliveryTimes.ToString();

                // Create order fileter
                var filter = new OrderFilterModel()
                {
                    searchType = (int)SearchType.Order,
                    search = TextSearch,
                    excuteStatus = new List<int>() { (int)ExcuteStatus.Done },
                    paymentType = PaymentType,
                    shippingType = ShippingType,
                    orderCreatedBy = CreatedBy, // CreatedBy
                    //orderDate = CreatedDate, // CreatedDate
                    orderFromDate = OrderFromDate,
                    orderToDate = OrderToDate,
                    transportCompany = TransportCompany, // TransportCompany
                    shipper = ShipperID,
                    //deliveryStart = DeliveryStartAt, // DeliveryStartAt
                    deliveryTimes = DeliveryTimes, // DeliveryTimes
                    deliveryStatus = DeliveryStatus,
                    invoiceStatus = InvoiceStatus,
                    selected = isDeliverySession,
                    account = acc
                };
                // Create pagination
                var page = new PaginationMetadataModel()
                {
                    currentPage = Page
                };
                List<OrderList> rs = new List<OrderList>();
                rs = OrderController.Filter(filter, ref page);

                var deliverySession = SessionController.getDeliverySession(acc);
                hdfSession.Value = JsonConvert.SerializeObject(deliverySession);
                // Đánh dấu check cho các order đã được check
                rs = rs.GroupJoin(
                        deliverySession,
                        ord => ord.ID,
                        del => del.OrderID,
                        (ord, del) => new { ord, del }
                    )
                    .SelectMany(
                        x => x.del.DefaultIfEmpty(),
                        (parent, child) => {
                            if (child != null)
                            {
                                parent.ord.CheckDelivery = true;
                            }
                            else
                            {
                                parent.ord.CheckDelivery = false;
                            }
                            return parent;
                        }
                    )
                    .Select(x => x.ord)
                    .Distinct()
                    .ToList();

                pagingall(rs, page);

                ltrNumberOfOrder.Text = page.totalCount.ToString();

                if (acc.RoleID == 0)
                {
                    ltrBtnDoneDelivery.Text = "<a id='doneDelivery' href='javascript:;' class='btn primary-btn fw-btn width-100' onclick='changeDoneDelivery()'><i class='fa fa-check' aria-hidden='true'></i> Cập nhật đã giao</a>";
                }
                
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
            html.Append("    <th><input id='checkPrintAll' type='checkbox' onchange='changeCheckPrintAll($(this).prop(`checked`))'/></th>");
            html.Append("    <th>Mã</th>");
            html.Append("    <th class='col-customer'>Khách hàng</th>");
            html.Append("    <th>Mua</th>");
            html.Append("    <th>Kiểu giao</th>");
            html.Append("    <th>Thanh toán</th>");
            html.Append("    <th>Shipper</th>");
            html.Append("    <th>Trạng thái</th>");
            html.Append("    <th>Tổng tiền</th>");
            html.Append("    <th>COD</th>");
            html.Append("    <th>Ship</th>");
            html.Append("    <th>Ngày giao</th>");
            html.Append("    <th>Đợt</th>");
            html.Append("    <th>Ngày hoàn tất</th>");

            if (acc.RoleID == 0 || acc.Username == "nhom_zalo406" || acc.Username == "hotline" || acc.Username == "nhom_zalo502")
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
                    TrTag.AppendLine(String.Format("data-paymenttype='{0}' ", item.PaymentType));
                    TrTag.AppendLine(String.Format("data-shippingtype='{0}' ", item.ShippingType));
                    TrTag.AppendLine(String.Format("data-deliverystatus='{0}' ", item.DeliveryStatus));
                    TrTag.AppendLine(String.Format("data-invoiceimage='{0}' ", item.InvoiceImage));
                    TrTag.AppendLine(String.Format("data-coloford='{0:#,###}' ", item.CollectionOfOrder.HasValue ? item.CollectionOfOrder : Convert.ToDecimal(item.TotalPrice - item.TotalRefund) ));
                    TrTag.AppendLine(String.Format("data-shipperid='{0}' ", item.ShipperID));
                    TrTag.AppendLine(String.Format("data-cosofdev='{0:#,###}' ", item.CostOfDelivery.HasValue ? item.CostOfDelivery :  Convert.ToDecimal(item.FeeShipping) ));
                    TrTag.AppendLine(String.Format("data-deliverydate='{0:dd/MM/yyyy HH:mm}' ", item.DeliveryDate));
                    TrTag.AppendLine(String.Format("data-shippernote='{0}' ", item.ShipNote));
                    TrTag.AppendLine(String.Format("data-transfercompany='{0}' ", item.TransportCompanyID));
                    TrTag.AppendLine(String.Format("data-deliverytimes='{0}' ", item.DeliveryTimes));
                    TrTag.AppendLine("/>");

                    html.Append(TrTag.ToString());
                    // Hình thức giáo hàng là chuyển xe
                    // và gói hàng chưa được giao hoặc đang giao
                    if (item.CheckDelivery.HasValue && item.CheckDelivery.Value)
                    {
                        html.Append("   <td><input type='checkbox' onchange='changeCheckPrint($(this))' checked/></td>");
                    }
                    else
                    {
                        html.Append("   <td><input type='checkbox' onchange='changeCheckPrint($(this))'/></td>");
                    }
                    html.Append("   <td><a href='/thong-tin-don-hang?id=" + item.ID + "'>" + item.ID + "</a></td>");
                    if (!string.IsNullOrEmpty(item.Nick))
                    {
                        html.Append("   <td data-title='Khách hàng' class='customer-td'><a class='col-customer-name-link' href='/thong-tin-don-hang?id=" + item.ID + "'>" + item.CustomerName.ToTitleCase() + "</a><br><span class='name-bottom-nick'>Nick: " + item.Nick.ToTitleCase() + "</span></td>");
                    }
                    else
                    {
                        html.Append("   <td data-title='Khách hàng' class='customer-td'><a class='col-customer-name-link' href='/thong-tin-don-hang?id=" + item.ID + "'>" + item.CustomerName.ToTitleCase() + "</a></td>");
                    }
                    html.Append("   <td data-title='Đã mua'>" + item.Quantity + "</td>");
                    html.Append("   <td data-title='Kiểu giao'>" + PJUtils.ShippingType(Convert.ToInt32(item.ShippingType)) + "</td>");
                    html.Append("   <td data-title='Kiểu thanh toán'>" + PJUtils.PaymentType(Convert.ToInt32(item.PaymentType)) + "</td>");
                    html.Append("   <td data-title='Shipper' id='shiperName'>" + item.ShipperName + "</td>");
                    html.Append("   <td data-title='Trạng thái' id='deliveryStatus'>" + PJUtils.DeliveryStatus(Convert.ToInt32(item.DeliveryStatus)) + "</td>");
                    // Tổng đơn hàng
                    html.Append("   <td data-title='Tổng đơn'><strong>" + String.Format("{0:#,###}", Convert.ToDouble(item.TotalPrice - item.TotalRefund)) + "</strong></td>");
                    // Số tiền đã thu chỉ hiện khi thanh toán kiểu thu hộ và đơn hàng đã giao hoặc đang giao
                    if (item.PaymentType == 3 && (item.DeliveryStatus == 1 || item.DeliveryStatus == 3))
                        html.Append("   <td data-title='COD' id='colOfOrd'><strong>" + String.Format("{0:#,###}", item.CollectionOfOrder) + "</strong></td>");
                    else
                        html.Append("   <td data-title='COD' id='colOfOrd'></td>");
                    // Phí giao hàng khi nhân viên giao
                    if (item.ShippingType == 5 && (item.DeliveryStatus == 1 || item.DeliveryStatus == 3) && item.CostOfDelivery != 0)
                        html.Append("   <td data-title='Ship' id='cosOfDel'><strong>" + String.Format("{0:#,###}", item.CostOfDelivery) + "</strong></td>");
                    else
                        html.Append("   <td data-title='Ship' id='cosOfDel'></td>");
                    // Ngày giao
                    if(item.DeliveryStatus == 1)
                        html.Append("   <td data-title='Ngày giao' id='delDate'>" + String.Format("{0:dd/MM HH:mm}", item.DeliveryDate) + "</td>");
                    else
                        html.Append("   <td data-title='Ngày giao' id='delDate'></td>");
                    // Đợt giao
                    if(item.DeliveryTimes > 0)
                    {
                        html.Append("   <td data-title='Đợt giao' id='deliveryTimes'>Đợt " + item.DeliveryTimes.ToString() + "</td>");
                    }
                    else
                    {
                        html.Append("   <td data-title='Đợt giao' id='deliveryTimes'></td>");
                    }
                    // Ngày hoàn tất đơn
                    string datedone = "";
                    if (item.ExcuteStatus == 2)
                    {
                        datedone = string.Format("{0:dd/MM}", item.DateDone);
                    }
                    html.Append("   <td data-title='Ngày hoàn tất'>" + datedone + "</td>");
                    html.Append("   <td data-title='Nhân viên tạo đơn'>" + item.CreatedBy + "</td>");
                    html.Append("   <td data-title='Thao tác' class='update-button' id='updateButton'>");

                    if (acc.RoleID == 0)
                    {
                        html.Append("       <button type='button' class='btn primary-btn h45-btn' data-toggle='modal' data-target='#TransferBankModal' data-backdrop='static' data-keyboard='false' title='Cập nhật thông tin chuyển khoản'><span class='glyphicon glyphicon-edit'></span></button>");
                    }
                    
                    if (item.DeliveryStatus == 1 && !string.IsNullOrEmpty(item.InvoiceImage))
                    {
                        html.Append("       <a id='downloadInvoiceImage' href='javascript:;' onclick='openImageInvoice($(this))' data-link='" + item.InvoiceImage + "' title='Biên nhận gửi hàng' class='btn primary-btn btn-blue h45-btn'><i class='fa fa-file-text-o' aria-hidden='true'></i></a>");
                    }
                    html.Append("   </td>");
                    html.Append("</tr>");

                    // thông tin thêm
                    html.Append("<tr class='tr-more-info'>");
                    html.Append("   <td colspan='2' data-title='Thông tin thêm'></td>");
                    html.Append("   <td colspan='14'>");

                    if (item.ShippingType == 4)
                    {
                        if (item.TransportCompanyID.HasValue && item.TransportCompanyID != 0)
                        {
                            var transport = TransportCompanyController.GetTransportCompanyForOrderList(Convert.ToInt32(item.TransportCompanyID));
                            var transportsub = TransportCompanyController.GetReceivePlaceForOrderList(Convert.ToInt32(item.TransportCompanyID), Convert.ToInt32(item.TransportCompanySubID));
                            html.Append("<span class='order-info'><strong>Gửi xe: </strong> " + transport.CompanyName.ToTitleCase() + " (" + transportsub.ShipTo.ToTitleCase() + ")</span>");
                        }
                    }
                    if (item.TotalRefund != 0)
                    {
                        html.Append("<span class='order-info'><strong>Đổi trả:</strong> -" + string.Format("{0:N0}", item.TotalRefund) + " (<a href='xem-don-hang-doi-tra?id=" + item.RefundsGoodsID + "' target='_blank'>Xem đơn " + item.RefundsGoodsID + "</a>)</span>");
                    }
                    if (item.TotalDiscount > 0)
                    {
                        html.Append("<span class='order-info'><strong>Chiết khấu:</strong> -" + string.Format("{0:N0}", Convert.ToDouble(item.TotalDiscount)) + "</span>");
                    }
                    if (item.OtherFeeValue != 0)
                    {
                        html.Append("<span class='order-info'><strong>Phí khác:</strong> " + string.Format("{0:N0}", Convert.ToDouble(item.OtherFeeValue)) + " (<a href='#feeInfoModal' data-toggle='modal' data-backdrop='static' onclick='openFeeInfoModal(" + item.ID + ")'>" + item.OtherFeeName.Trim() + "</a>)</span>");
                    }
                    if (item.FeeShipping > 0)
                    {
                        html.Append("<span class='order-info'><strong>Ship:</strong> " + string.Format("{0:N0}", Convert.ToDouble(item.FeeShipping)) + "</span>");
                    }
                    if (!string.IsNullOrEmpty(item.CouponCode))
                    {
                        html.Append(String.Format("<span class='order-info'><strong>Coupon ({0}):</strong> -{1:N0}</span>", item.CouponCode.Trim().ToUpper(), item.CouponValue));
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
                    html.Append("<tr><td colspan='14'>Không tìm thấy đơn hàng...</td></tr>");
                }
                else
                {
                    html.Append("<tr><td colspan='13'>Không tìm thấy đơn hàng...</td></tr>");
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
            string request = "/danh-sach-van-chuyen?";

            if (search != "")
                request += "&textsearch=" + search;

            if (ddlTransportCompany.SelectedValue != "0")
                request += "&transportcompany=" + ddlTransportCompany.SelectedValue;

            if (ddlShippingType.SelectedValue != "0")
                request += "&shippingtype=" + ddlShippingType.SelectedValue;

            if (ddlPaymentType.SelectedValue != "0")
                request += "&paymenttype=" + ddlPaymentType.SelectedValue;

            if (ddlShipperFilter.SelectedValue != "0")
                request += "&shipperid=" + ddlShipperFilter.SelectedValue;

            if (ddlInvoiceStatus.SelectedValue != "0")
                request += "&invoicestatus=" + ddlInvoiceStatus.SelectedValue;

            if (ddlDeliveryStatusFilter.SelectedValue != "0")
                request += "&deliverystatus=" + ddlDeliveryStatusFilter.SelectedValue;

            if (rOrderFromDate.SelectedDate.HasValue)
            {
                request += "&orderfromdate=" + rOrderFromDate.SelectedDate.ToString();
            }

            if (rOrderToDate.SelectedDate.HasValue)
            {
                request += "&ordertodate=" + rOrderToDate.SelectedDate.ToString();
            }

            if (ddlCreatedBy.SelectedValue != "")
                request += "&createdby=" + ddlCreatedBy.SelectedValue;

            if (ddlDeliveryTimes.SelectedValue != "")
                request += "&deliverytimes=" + ddlDeliveryTimes.SelectedValue;

            // add filter selected
            if (Request.QueryString["isdeliverysession"] != null)
                request += "&isdeliverysession=1";

            Response.Redirect(request);
        }

        [WebMethod]
        public static string addOrderChoose(List<DeliverySession> deliverySession)
        {
            var username = HttpContext.Current.Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

            return SessionController.addDeliverySession(acc, deliverySession);
        }

        [WebMethod]
        public static void updateOrderChoose(List<DeliverySession> deliverySession)
        {
            var username = HttpContext.Current.Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

            SessionController.updateDeliverySession(acc, deliverySession);
            DeliveryController.updateDelivery(acc, deliverySession);
        }

        [WebMethod]
        public static string deleteOrderChoose(List<DeliverySession> deliverySession)
        {
            var username = HttpContext.Current.Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

            return SessionController.deleteDeliverySession(acc, deliverySession);
        }

        [WebMethod]
        public static void deleteAllOrderChoose()
        {
            var username = HttpContext.Current.Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            SessionController.deleteDeliverySession(acc);
        }
    }
}