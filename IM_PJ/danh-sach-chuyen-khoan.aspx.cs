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
                if (Request.Cookies["userLoginSystem"] != null)
                {
                    string username = Request.Cookies["userLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);
                    int agent = acc.AgentID.ToString().ToInt();

                    if (acc != null)
                    {
                        if (acc.RoleID == 0)
                        {
                            LoadCreatedBy(agent);
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
                ddlCreatedBy.Items.Insert(0, new ListItem("Nhân viên", ""));
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
                cusBanks[0].Text = "Ngân hàng chuyển khoản";
                ddlCustomerBank.Items.Clear();
                ddlCustomerBank.Items.AddRange(cusBanks.ToArray());
                ddlCustomerBank.DataBind();

                // Add Account Bank drop down list
                var accBanks = BankAccountController.getDropDownList();
                accBanks[0].Text = "Ngân hàng thụ hưởng";
                ddlBankReceive.Items.Clear();
                ddlBankReceive.Items.AddRange(accBanks.ToArray());
                ddlBankReceive.DataBind();

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
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                int OrderType = 0;
                int TransferStatus = 0;
                int ExcuteStatus = 0;
                int BankReceive = 0;
                string TextSearch = "";
                string CreatedBy = "";
                string CreatedDate = "";
                string TransferDoneAt = "";

                if (Request.QueryString["textsearch"] != null)
                {
                    TextSearch = Request.QueryString["textsearch"].Trim();
                }
                if (Request.QueryString["ordertype"] != null)
                {
                    OrderType = Request.QueryString["ordertype"].ToInt(0);
                }
                if (Request.QueryString["transferstatus"] != null)
                {
                    TransferStatus = Request.QueryString["transferstatus"].ToInt(0);
                }
                if (Request.QueryString["excutestatus"] != null)
                {
                    ExcuteStatus = Request.QueryString["excutestatus"].ToInt(0);
                }
                if (Request.QueryString["createdby"] != null)
                {
                    CreatedBy = Request.QueryString["createdby"];
                }
                if (Request.QueryString["transferdoneat"] != null)
                {
                    TransferDoneAt = Request.QueryString["transferdoneat"];
                }
                if (Request.QueryString["bankreceive"] != null)
                {
                    BankReceive = Request.QueryString["bankreceive"].ToInt(0);
                }

                txtSearchOrder.Text = TextSearch;
                ddlOrderType.SelectedValue = OrderType.ToString();
                ddlTransferStatus.SelectedValue = TransferStatus.ToString();
                ddlBankReceive.SelectedValue = BankReceive.ToString();
                ddlCreatedBy.SelectedValue = CreatedBy.ToString();
                ddlTransferDoneAt.SelectedValue = TransferDoneAt.ToString();

                List<OrderList> rs = new List<OrderList>();
                rs = OrderController.Filter(
                    TextSearch, OrderType,
                    ExcuteStatus,
                    0,
                    TransferStatus,
                    2, // Chuyển khoản
                    0, // All
                    String.Empty, // All
                    String.Empty, // All
                    CreatedBy,
                    CreatedDate,
                    TransferDoneAt,
                    0
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

                if (BankReceive != 0)
                {
                    rs = rs.Where(x => x.AccBankID == BankReceive).ToList();
                }

                pagingall(rs);

                ltrNumberOfOrder.Text = rs.Count().ToString();

                // THỐNG KÊ ĐƠN HÀNG
                int TotalOrders = rs.Count;
                int TotalProducts = 0;

                double TotalMoneyOrder = 0;
                double TotalMoneyReceive = 0;

                for (int i = 0; i < rs.Count; i++)
                {
                    var item = rs[i];

                    // Tính tổng số sản phẩm trong tổng số đơn hàng
                    TotalProducts += item.Quantity;

                    // Tính số tiền
                    TotalMoneyOrder += item.TotalPrice;
                    TotalMoneyReceive += Convert.ToDouble(item.MoneyReceive);
                }

                StringBuilder htmlReport = new StringBuilder();

                htmlReport.AppendLine(String.Format("<div class='row pad'>"));
                htmlReport.AppendLine(String.Format("    <div class='col-md-3'>"));
                htmlReport.AppendLine(String.Format("        <label class='left pad10'>Tổng số đơn hàng: </label>"));
                htmlReport.AppendLine(String.Format("        <div class='ordertype'>"));
                htmlReport.AppendLine(String.Format("            {0}", TotalOrders.ToString()));
                htmlReport.AppendLine(String.Format("        </div>"));
                htmlReport.AppendLine(String.Format("    </div>"));
                htmlReport.AppendLine(String.Format("    <div class='col-md-3'>"));
                htmlReport.AppendLine(String.Format("        <label class='left pad10'>Tổng tiền đơn hàng: </label>"));
                htmlReport.AppendLine(String.Format("        <div class='orderquantity'>"));
                htmlReport.AppendLine(String.Format("            {0}", string.Format("{0:N0}", TotalMoneyOrder).ToString()));
                htmlReport.AppendLine(String.Format("        </div>"));
                htmlReport.AppendLine(String.Format("    </div>"));
                htmlReport.AppendLine(String.Format("    <div class='col-md-3'>"));
                htmlReport.AppendLine(String.Format("        <label class='left pad10'>Tổng tiền đã nhận: </label>"));
                htmlReport.AppendLine(String.Format("        <div class='ordertotalprice'>"));
                htmlReport.AppendLine(String.Format("            {0}", string.Format("{0:N0}", TotalMoneyReceive).ToString()));
                htmlReport.AppendLine(String.Format("        </div>"));
                htmlReport.AppendLine(String.Format("    </div>"));
                htmlReport.AppendLine(String.Format("    <div class='col-md-3'> "));
                htmlReport.AppendLine(String.Format("        <label class='left pad10'>Tổng sản phẩm: </label>"));
                htmlReport.AppendLine(String.Format("        <div class='ordernote'>"));
                htmlReport.AppendLine(String.Format("            {0}", TotalProducts.ToString()));
                htmlReport.AppendLine(String.Format("        </div>"));
                htmlReport.AppendLine(String.Format("    </div>"));
                htmlReport.AppendLine(String.Format("</div>"));

                ltrReport.Text = htmlReport.ToString();
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
            html.Append("    <th>Loại</th>");
            html.Append("    <th class='col-customer'>Khách hàng</th>");
            html.Append("    <th>Mua</th>");
            html.Append("    <th>Chuyển từ</th>");
            html.Append("    <th>Tài khoản nhận</th>");
            html.Append("    <th>Trạng thái</th>");
            html.Append("    <th>Tổng đơn</th>");
            html.Append("    <th>Đã nhận</th>");
            html.Append("    <th>Ngày nhận</th>");
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
                    TrTag.AppendLine(String.Format("data-cusID='{0}' ", item.CustomerID));
                    TrTag.AppendLine(String.Format("data-cusbankid='{0:#}' ", item.CusBankID));
                    TrTag.AppendLine(String.Format("data-cusbankname='{0}' ", item.CusBankName));
                    TrTag.AppendLine(String.Format("data-accbankid='{0:#}' ", item.AccBankID));
                    TrTag.AppendLine(String.Format("data-accbankname='{0}' ", item.AccBankName));
                    TrTag.AppendLine(String.Format("data-statusid='{0:#}' ", item.TransferStatus));
                    TrTag.AppendLine(String.Format("data-statusname='{0}' ", item.StatusName));
                    TrTag.AppendLine(String.Format("data-price='{0:#}' ", Convert.ToDouble(item.TotalPrice - item.TotalRefund)));
                    TrTag.AppendLine(String.Format("data-moneyreceived='{0:#}' ", item.MoneyReceive != 0 ? item.MoneyReceive : Convert.ToDecimal(item.TotalPrice - item.TotalRefund)));
                    TrTag.AppendLine(String.Format("data-doneat='{0:yyyy-MM-dd HH:mm:ss}' ", item.DoneAt));
                    TrTag.AppendLine(String.Format("data-transfernote='{0}' ", item.TransferNote));
                    TrTag.AppendLine("/>");

                    html.Append(TrTag.ToString());
                    html.Append("   <td><a href=\"/thong-tin-don-hang?id=" + item.ID + "\">" + item.ID + "</a></td>");
                    html.Append("   <td>" + PJUtils.OrderTypeStatus(Convert.ToInt32(item.OrderType)) + "</td>");
                    if (!string.IsNullOrEmpty(item.Nick))
                    {
                        html.Append("   <td><a class=\"col-customer-name-link\" href=\"/thong-tin-don-hang?id=" + item.ID + "\">" + item.Nick.ToTitleCase() + "</a><br><span class=\"name-bottom-nick\">(" + item.CustomerName.ToTitleCase() + ")</span></td>");
                    }
                    else
                    {
                        html.Append("   <td><a class=\"col-customer-name-link\" href=\"/thong-tin-don-hang?id=" + item.ID + "\">" + item.CustomerName.ToTitleCase() + "</a></td>");
                    }
                    html.Append("   <td>" + item.Quantity + "</td>");
                    html.Append("   <td id='cusBankName'>" + item.CusBankName + "</td>");
                    html.Append("   <td id='accBankName'>" + item.AccBankName + "</td>");
                    if (item.TransferStatus == 1)
                    {
                        html.Append("   <td id='statusName'><span class='bg-green'>" + item.StatusName + "</span></td>");
                    }
                    else
                    {
                        html.Append("   <td id='statusName'><span class='bg-red'>" + item.StatusName + "</span></td>");
                    }
                    html.Append("   <td><strong>" + String.Format("{0:#,###}", Convert.ToDouble(item.TotalPrice - item.TotalRefund)) + "</strong></td>");
                    if (item.TransferStatus == 1)
                    {
                        html.Append("   <td id='moneyReceive'><strong>" + String.Format("{0:#,###}", item.MoneyReceive) + "</strong></td>");
                        html.Append("   <td id='doneAt'>" + String.Format("{0:dd/MM HH:mm}", item.DoneAt) + "</td>");
                    }
                    else
                    {
                        html.Append("   <td id='moneyReceive'></td>");
                        html.Append("   <td id='doneAt'></td>");
                    }
                    if (acc.RoleID == 0)
                    {
                        html.Append("   <td>" + item.CreatedBy + "</td>");
                    }
                    html.Append("   <td>");
                    html.Append("       <button type='button' class='btn primary-btn h45-btn' data-toggle='modal' data-target='#TransferBankModal' data-backdrop='static' data-keyboard='false' title='Cập nhật thông tin chuyển khoản'><span class='glyphicon glyphicon-edit'></span></button>");
                    html.Append("   </td>");
                    html.Append("</tr>");

                    // thông tin thêm
                    html.Append("<tr class='tr-more-info'>");
                    html.Append("   <td colspan='2'>");
                    html.Append("   </td>");
                    html.Append("   <td colspan='11'>");

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
            string request = "/danh-sach-chuyen-khoan?";

            if (search != "")
            {
                request += "&textsearch=" + search;
            }

            if (ddlOrderType.SelectedValue != "")
            {
                request += "&ordertype=" + ddlOrderType.SelectedValue;
            }

            if (ddlTransferStatus.SelectedValue != "")
            {
                request += "&transferstatus=" + ddlTransferStatus.SelectedValue;
            }

            if (ddlCreatedBy.SelectedValue != "")
            {
                request += "&createdby=" + ddlCreatedBy.SelectedValue;
            }

            if (ddlTransferDoneAt.SelectedValue != "")
            {
                request += "&transferdoneat=" + ddlTransferDoneAt.SelectedValue;
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
            string username = HttpContext.Current.Request.Cookies["userLoginSystem"].Value;
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