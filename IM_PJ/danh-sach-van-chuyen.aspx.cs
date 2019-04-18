using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
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
                if (Request.Cookies["userLoginSystem"] != null)
                {
                    string username = Request.Cookies["userLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);
                    int agent = acc.AgentID.ToString().ToInt();

                    if (acc != null)
                    {
                        
                        if (acc.RoleID == 0)
                        {
                            LoadShipper();
                            LoadCreatedBy(agent);
                            LoadTransportCompany();
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
        public void LoadTransportCompany()
        {
            var TransportCompany = TransportCompanyController.GetTransportCompany();
            ddlTransportCompany.Items.Clear();
            ddlTransportCompany.Items.Insert(0, new ListItem("Chành xe", "0"));
            if (TransportCompany.Count > 0)
            {
                foreach (var p in TransportCompany)
                {
                    ListItem listitem = new ListItem(p.CompanyName.ToTitleCase(), p.ID.ToString());
                    ddlTransportCompany.Items.Add(listitem);
                }
                ddlTransportCompany.DataBind();
            }
        }
        public void LoadCreatedBy(int AgentID, tbl_Account acc = null)
        {
            if (acc != null)
            {
                ddlCreatedBy.Items.Clear();
                ddlCreatedBy.Items.Insert(0, new ListItem(acc.Username, acc.Username));
            }
            else
            {
                var CreateBy = AccountController.GetAllNotSearch();
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
            ddlShipperFilter.Items.Clear();
            ddlShipperFilter.Items.AddRange(shipper.ToArray());
            ddlShipperFilter.DataBind();
            ddlShipperModal.Items.Clear();
            ddlShipperModal.Items.AddRange(shipper.ToArray());
            ddlShipperModal.DataBind();

        }
        public void LoadData()
        {
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                string TextSearch = "";
                int TransportCompany = 0;
                int ShipType = 0;
                int PaymentType = 0;
                int ShipperID = 0;
                int InvoiceStatus = 0;
                int DeliveryStatus = 0;
                string CreatedDate = "";
                string CreatedBy = "";

                if (Request.QueryString["textsearch"] != null)
                    TextSearch = Request.QueryString["textsearch"].Trim();
                if (Request.QueryString["transportcompany"] != null)
                    TransportCompany = Request.QueryString["transportcompany"].ToInt(0);
                if (Request.QueryString["shippingtype"] != null)
                    ShipType = Request.QueryString["shippingtype"].ToInt(0);
                if (Request.QueryString["paymenttype"] != null)
                    PaymentType = Request.QueryString["paymenttype"].ToInt(0);
                if (Request.QueryString["shipperid"] != null)
                    ShipperID = Request.QueryString["shipperid"].ToInt(0);
                if (Request.QueryString["invoicestatus"] != null)
                    InvoiceStatus = Request.QueryString["invoicestatus"].ToInt(0);
                if (Request.QueryString["deliverystatus"] != null)
                    DeliveryStatus = Request.QueryString["deliverystatus"].ToInt(0);
                if (Request.QueryString["createddate"] != null)
                    CreatedDate = Request.QueryString["createddate"];
                if (Request.QueryString["createdby"] != null)
                    CreatedBy = Request.QueryString["createdby"];

                txtSearchOrder.Text = TextSearch;
                ddlTransportCompany.SelectedValue = TransportCompany.ToString();
                ddlShippingType.SelectedValue = ShipType.ToString();
                ddlPaymentType.SelectedValue = PaymentType.ToString();
                ddlShipperFilter.SelectedValue = ShipperID.ToString();
                ddlInvoiceStatus.SelectedValue = InvoiceStatus.ToString();
                ddlDeliveryStatusFilter.SelectedValue = DeliveryStatus.ToString();
                ddlCreatedDate.SelectedValue = CreatedDate.ToString();
                ddlCreatedBy.SelectedValue = CreatedBy.ToString();

                List<OrderList> rs = new List<OrderList>();
                rs = OrderController.Filter(
                    TextSearch, 
                    0,
                    2,
                    0,
                    0,
                    PaymentType,
                    ShipType, // All
                    String.Empty, // All
                    CreatedBy, // ALL
                    CreatedDate,
                    String.Empty,
                    String.Empty,
                    TransportCompany
                );

                if (acc.RoleID == 0)
                {
                    hdfcreate.Value = "1";
                    if (CreatedBy != "")
                    {
                        rs = rs.Where(x => x.CreatedBy == CreatedBy && x.ExcuteStatus != 4).ToList();
                    }
                    else
                    {
                        rs = rs.Where(x => x.ExcuteStatus != 4).ToList();
                    }
                }
                else
                {
                    rs = rs.Where(x => x.CreatedBy == acc.Username && x.ExcuteStatus != 4).ToList();
                }

                if (ShipType == 0)
                    rs = rs.Where(x => x.ShippingType == 4 || x.ShippingType == 5).ToList();
                if (ShipperID != 0)
                    rs = rs.Where(x => x.ShipperID == ShipperID).ToList();
                if (DeliveryStatus != 0)
                    rs = rs.Where(x => x.DeliveryStatus == DeliveryStatus).ToList();

                switch(InvoiceStatus)
                {
                    case 1:
                        rs = rs.Where(x => !String.IsNullOrEmpty(x.InvoiceImage)).ToList();
                        break;
                    case 2:
                        rs = rs.Where(x => String.IsNullOrEmpty(x.InvoiceImage)).ToList();
                        break;
                    default:
                        break;
                }
                if (DeliveryStatus != 0)
                    rs = rs.Where(x => x.DeliveryStatus == DeliveryStatus).ToList();

                pagingall(rs);
                ltrNumberOfOrder.Text = rs.Count().ToString();
            }
        }


        #region Paging
        public void pagingall(List<OrderList> acs)
        {
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);

            int PageSize = 30;

            StringBuilder html = new StringBuilder();
            html.Append("<tr>");
            html.Append("    <th>Mã</th>");
            html.Append("    <th class='col-customer'>Khách hàng</th>");
            html.Append("    <th>Mua</th>");
            html.Append("    <th>Giao hàng</th>");
            html.Append("    <th>Thanh toán</th>");
            html.Append("    <th>Shipper</th>");
            html.Append("    <th>Trạng thái</th>");
            html.Append("    <th>Tổng tiền</th>");
            html.Append("    <th>Đã thu</th>");
            html.Append("    <th>Phí</th>");
            html.Append("    <th>Ngày giao</th>");
            if (acc.RoleID == 0)
            {
                html.Append("    <th>Nhân viên</th>");
            }
            html.Append("    <th></th>");
            html.Append("</tr>");

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
                    // Insert transfer bank info for tr tag
                    var TrTag = new StringBuilder();
                    TrTag.AppendLine("<tr ");
                    TrTag.AppendLine(String.Format("data-orderid='{0}' ", item.ID));
                    TrTag.AppendLine(String.Format("data-paymenttype='{0}' ", item.PaymentType));
                    TrTag.AppendLine(String.Format("data-shippingtype='{0}' ", item.ShippingType));
                    TrTag.AppendLine(String.Format("data-deliverystatus='{0}' ", item.DeliveryStatus));
                    TrTag.AppendLine(String.Format("data-invoiceimage='{0}' ", item.InvoiceImage));
                    TrTag.AppendLine(String.Format("data-coloford='{0:#}' ", item.CollectionOfOrder != null ? item.CollectionOfOrder : Convert.ToDecimal(item.TotalPrice - item.TotalRefund) ));
                    TrTag.AppendLine(String.Format("data-shipperid='{0}' ", item.ShipperID));
                    TrTag.AppendLine(String.Format("data-cosofdev='{0:#}' ", item.CostOfDelivery != null ? item.CostOfDelivery :  Convert.ToDecimal(item.FeeShipping) ));
                    TrTag.AppendLine(String.Format("data-deliverydate='{0:yyyy-MM-dd HH:mm:ss}' ", item.DeliveryDate));
                    TrTag.AppendLine(String.Format("data-shippernote='{0}' ", item.ShipNote));
                    TrTag.AppendLine("/>");

                    html.Append(TrTag.ToString());
                    html.Append("   <td><a href=\"/thong-tin-don-hang?id=" + item.ID + "\">" + item.ID + "</a></td>");
                    if (!string.IsNullOrEmpty(item.Nick))
                    {
                        html.Append("   <td><a class=\"col-customer-name-link\" href=\"/thong-tin-don-hang?id=" + item.ID + "\">" + item.Nick.ToTitleCase() + "</a><br><span class=\"name-bottom-nick\">(" + item.CustomerName.ToTitleCase() + ")</span></td>");
                    }
                    else
                    {
                        html.Append("   <td><a class=\"col-customer-name-link\" href=\"/thong-tin-don-hang?id=" + item.ID + "\">" + item.CustomerName.ToTitleCase() + "</a></td>");
                    }
                    html.Append("   <td>" + item.Quantity + "</td>");
                    html.Append("   <td>" + PJUtils.ShippingType(Convert.ToInt32(item.ShippingType)) + "</td>");
                    html.Append("   <td>" + PJUtils.PaymentType(Convert.ToInt32(item.PaymentType)) + "</td>");
                    html.Append("   <td id='shiperName'>" + item.ShipperName + "</td>");
                    html.Append("   <td id='deliveryStatus'>" + PJUtils.DeliveryStatus(Convert.ToInt32(item.DeliveryStatus)) + "</td>");
                    html.Append("   <td><strong>" + String.Format("{0:#,###}", Convert.ToDouble(item.TotalPrice - item.TotalRefund)) + "</strong></td>");
                    // Thu hộ
                    if (item.PaymentType == 3)
                        html.Append("   <td id='colOfOrd'><strong>" + String.Format("{0:#,###}", item.CollectionOfOrder) + "</strong></td>");
                    else
                        html.Append("   <td></td>");
                    // Nhân viên giao
                    if (item.ShippingType == 5)
                        html.Append("   <td id='cosOfDel'><strong>" + String.Format("{0:#,###}", item.CostOfDelivery) + "</strong></td>");
                    else
                        html.Append("   <td></td>");
                    html.Append("   <td id='delDate'>" + String.Format("{0:dd/MM HH:mm}", item.DeliveryDate) + "</td>");
                    if (acc.RoleID == 0)
                        html.Append("   <td>" + item.CreatedBy + "</td>");
                    html.Append("   <td id='updateButton'>");
                    html.Append("       <button type='button' class='btn primary-btn h45-btn' data-toggle='modal' data-target='#TransferBankModal' data-backdrop='static' data-keyboard='false' title='Cập nhật thông tin chuyển khoản'><span class='glyphicon glyphicon-edit'></span></button>");
                    if (item.DeliveryStatus == 1 && !string.IsNullOrEmpty(item.InvoiceImage))
                    {
                        html.Append("       <a id='downloadInvoiceImage' href='" + item.InvoiceImage + "' title='Biên nhận gửi hàng' target='_blank' class='btn primary-btn btn-blue h45-btn'><i class=\"fa fa-file-text-o\" aria-hidden=\"true\"></i></a>");
                    }
                    html.Append("   </td>");
                    html.Append("</tr>");

                    // thông tin thêm
                    html.Append("<tr class='tr-more-info'>");
                    html.Append("   <td colspan='1'>");
                    html.Append("   </td>");
                    html.Append("   <td colspan='12'>");

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
                        html.Append("<span class='order-info'><strong>Phí khác:</strong> " + string.Format("{0:N0}", Convert.ToDouble(item.OtherFeeValue)) + " (" + item.OtherFeeName.Trim() + ")</span>");
                    }
                    if (item.FeeShipping > 0)
                    {
                        html.Append("<span class='order-info'><strong>Phí vận chuyển:</strong> " + string.Format("{0:N0}", Convert.ToDouble(item.FeeShipping)) + "</span>");
                    }
                    if (item.ShippingType == 4)
                    {
                        if (item.TransportCompanyID != 0)
                        {
                            var transport = TransportCompanyController.GetTransportCompanyByID(Convert.ToInt32(item.TransportCompanyID));
                            var transportsub = TransportCompanyController.GetReceivePlaceByID(Convert.ToInt32(item.TransportCompanyID), Convert.ToInt32(item.TransportCompanySubID));
                            html.Append("<span class='order-info'><strong>Gửi xe: </strong> " + transport.CompanyName.ToTitleCase() + " (" + transportsub.ShipTo.ToTitleCase() + ")</span>");
                        }
                    }
                    if (!string.IsNullOrEmpty(item.OrderNote))
                    {
                        html.Append("<span class='order-info'><strong>Ghi chú:</strong> " + item.OrderNote + "</span>");
                    }
                    html.Append("   </td>");
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

            if (ddlCreatedDate.SelectedValue != "")
                request += "&createddate=" + ddlCreatedDate.SelectedValue;

            if (ddlCreatedBy.SelectedValue != "")
                request += "&createdby=" + ddlCreatedBy.SelectedValue;

            Response.Redirect(request);
        }
    }
}