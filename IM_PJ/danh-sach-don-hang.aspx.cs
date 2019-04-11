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
    public partial class danh_sach_don_hang : System.Web.UI.Page
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
                        else if (acc.RoleID == 2)
                        {
                            LoadCreatedBy(agent, acc);
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
            }
        }
        public void LoadData()
        {
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                int OrderType = 0;
                int PaymentStatus = 0;
                int ExcuteStatus = 0;
                int PaymentType = 0;
                int ShippingType = 0;
                string Discount = "";
                string OtherFee = "";
                string TextSearch = "";
                string CreatedBy = "";
                string CreatedDate = "";
                string QuantityFilter = "";
                int Quantity = 0;
                int QuantityMin = 0;
                int QuantityMax = 0;

                if (Request.QueryString["textsearch"] != null)
                {
                    TextSearch = Request.QueryString["textsearch"].Trim();
                }
                if (Request.QueryString["ordertype"] != null)
                {
                    OrderType = Request.QueryString["ordertype"].ToInt(0);
                }
                if (Request.QueryString["paymentstatus"] != null)
                {
                    PaymentStatus = Request.QueryString["paymentstatus"].ToInt(0);
                }
                if (Request.QueryString["excutestatus"] != null)
                {
                    ExcuteStatus = Request.QueryString["excutestatus"].ToInt(0);
                }
                if (Request.QueryString["paymenttype"] != null)
                {
                    PaymentType = Request.QueryString["paymenttype"].ToInt(0);
                }
                if (Request.QueryString["shippingtype"] != null)
                {
                    ShippingType = Request.QueryString["shippingtype"].ToInt(0);
                }
                if (Request.QueryString["discount"] != null)
                {
                    Discount = Request.QueryString["discount"].ToString();
                }
                if (Request.QueryString["otherfee"] != null)
                {
                    OtherFee = Request.QueryString["otherfee"].ToString();
                }
                if (Request.QueryString["createdby"] != null)
                {
                    CreatedBy = Request.QueryString["createdby"];
                }
                if (Request.QueryString["createdby"] != null)
                {
                    CreatedBy = Request.QueryString["createdby"];
                }
                if(Request.QueryString["createddate"] != null)
                {
                    CreatedDate = Request.QueryString["createddate"];
                }
                if (Request.QueryString["quantityfilter"] != null)
                {
                    QuantityFilter = Request.QueryString["quantityfilter"];

                    if (QuantityFilter == "greaterthan" || QuantityFilter == "lessthan")
                    {
                        Quantity = Request.QueryString["quantity"].ToInt();
                    }
                    if (QuantityFilter == "between")
                    {
                        QuantityMin = Request.QueryString["quantitymin"].ToInt();
                        QuantityMax = Request.QueryString["quantitymax"].ToInt();
                    }
                }

                txtSearchOrder.Text = TextSearch;
                ddlOrderType.SelectedValue = OrderType.ToString();
                ddlExcuteStatus.SelectedValue = ExcuteStatus.ToString();
                ddlPaymentStatus.SelectedValue = PaymentStatus.ToString();
                ddlPaymentType.SelectedValue = PaymentType.ToString();
                ddlShippingType.SelectedValue = ShippingType.ToString();
                ddlDiscount.SelectedValue = Discount.ToString();
                ddlOtherFee.SelectedValue = OtherFee.ToString();
                ddlCreatedBy.SelectedValue = CreatedBy.ToString();
                ddlCreatedDate.SelectedValue = CreatedDate.ToString();

                ddlQuantityFilter.SelectedValue = QuantityFilter.ToString();
                txtQuantity.Text = Quantity.ToString();
                txtQuantityMin.Text = QuantityMin.ToString();
                txtQuantityMax.Text = QuantityMax.ToString();


                List<OrderList> rs = new List<OrderList>();
                rs = OrderController.Filter(TextSearch, OrderType, ExcuteStatus, PaymentStatus, PaymentType, ShippingType, Discount, OtherFee, CreatedBy, CreatedDate);

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

                if (QuantityFilter != "")
                {
                    if (QuantityFilter == "greaterthan")
                    {
                        rs = rs.Where(p => p.Quantity >= Quantity).ToList();
                    }
                    else if (QuantityFilter == "lessthan")
                    {
                        rs = rs.Where(p => p.Quantity <= Quantity).ToList();
                    }
                    else if (QuantityFilter == "between")
                    {
                        rs = rs.Where(p => p.Quantity >= QuantityMin && p.Quantity <= QuantityMax).ToList();
                    }
                }

                pagingall(rs);



                ltrNumberOfOrder.Text = rs.Count().ToString();

                // THỐNG KÊ ĐƠN HÀNG
                int TotalOrders = rs.Count;
                int Type1Orders = 0;
                int Type2Orders = 0;
                int TotalProducts = 0;

                int ShippingType1 = 0;
                int ShippingType2 = 0;
                int ShippingType3 = 0;
                int ShippingType4 = 0;

                double TotalMoney = 0;
                double TotalDiscount = 0;
                double FeeShipping = 0;
                double OtherFeeValue = 0;

                for (int i = 0; i < rs.Count; i++)
                {
                    var item = rs[i];

                    // Tính tổng số sản phẩm trong tổng số đơn hàng
                    TotalProducts += item.Quantity;
                    // Tính tổng đơn hàng sỉ và lẻ

                    if (item.OrderType == 2)
                    {
                        Type2Orders++;
                    }
                    if (item.OrderType == 1)
                    {
                        Type1Orders++;
                    }

                    // Tính số đơn dựa vào kiểu vận chuyển
                    if (item.ShippingType == 1)
                    {
                        ShippingType1++;
                    }
                    if (item.ShippingType == 2)
                    {
                        ShippingType2++;
                    }
                    if (item.ShippingType == 3)
                    {
                        ShippingType3++;
                    }
                    if (item.ShippingType == 4)
                    {
                        ShippingType4++;
                    }

                    // Tính số tiền
                    TotalMoney += item.TotalPrice;
                    TotalDiscount += item.TotalDiscount;
                    FeeShipping += item.FeeShipping;
                    OtherFeeValue += item.OtherFeeValue;
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
                htmlReport.AppendLine(String.Format("        <label class='left pad10'>Số đơn hàng sỉ: </label>"));
                htmlReport.AppendLine(String.Format("        <div class='ordercreateby'>"));
                htmlReport.AppendLine(String.Format("            {0}", Type2Orders.ToString()));
                htmlReport.AppendLine(String.Format("        </div>"));
                htmlReport.AppendLine(String.Format("    </div>"));
                htmlReport.AppendLine(String.Format("    <div class='col-md-3'>"));
                htmlReport.AppendLine(String.Format("        <label class='left pad10'>Số đơn hàng lẻ: </label>"));
                htmlReport.AppendLine(String.Format("        <div class='ordercreatedate'>"));
                htmlReport.AppendLine(String.Format("            {0}", Type1Orders.ToString()));
                htmlReport.AppendLine(String.Format("        </div>"));
                htmlReport.AppendLine(String.Format("    </div>"));
                htmlReport.AppendLine(String.Format("    <div class='col-md-3'> "));
                htmlReport.AppendLine(String.Format("        <label class='left pad10'>Tổng sản phẩm: </label>"));
                htmlReport.AppendLine(String.Format("        <div class='ordernote'>"));
                htmlReport.AppendLine(String.Format("            {0}", TotalProducts.ToString()));
                htmlReport.AppendLine(String.Format("        </div>"));
                htmlReport.AppendLine(String.Format("    </div>"));
                htmlReport.AppendLine(String.Format("</div>"));
                if (acc.RoleID == 0)
                {
                    htmlReport.AppendLine(String.Format("<div class='row pad'>"));
                    htmlReport.AppendLine(String.Format("    <div class='col-md-3'>"));
                    htmlReport.AppendLine(String.Format("        <label class='left pad10'>Tổng số tiền: </label>"));
                    htmlReport.AppendLine(String.Format("        <div class='orderquantity'>"));
                    htmlReport.AppendLine(String.Format("            {0}", string.Format("{0:N0}", Convert.ToDouble(TotalMoney)).ToString()));
                    htmlReport.AppendLine(String.Format("        </div>"));
                    htmlReport.AppendLine(String.Format("    </div>"));
                    htmlReport.AppendLine(String.Format("    <div class='col-md-3'>"));
                    htmlReport.AppendLine(String.Format("        <label class='left pad10'>Tổng chiết khấu: </label>"));
                    htmlReport.AppendLine(String.Format("        <div class='ordertotalprice'>"));
                    htmlReport.AppendLine(String.Format("            {0}", string.Format("{0:N0}", Convert.ToDouble(TotalDiscount)).ToString()));
                    htmlReport.AppendLine(String.Format("        </div>"));
                    htmlReport.AppendLine(String.Format("    </div>"));
                    htmlReport.AppendLine(String.Format("    <div class='col-md-3'>"));
                    htmlReport.AppendLine(String.Format("        <label class='left pad10'>Tổng phí vận chuyển: </label>"));
                    htmlReport.AppendLine(String.Format("        <div class='ordertotalprice'>"));
                    htmlReport.AppendLine(String.Format("            {0}", string.Format("{0:N0}", Convert.ToDouble(FeeShipping)).ToString()));
                    htmlReport.AppendLine(String.Format("        </div>"));
                    htmlReport.AppendLine(String.Format("    </div>"));
                    htmlReport.AppendLine(String.Format("    <div class='col-md-3'>"));
                    htmlReport.AppendLine(String.Format("        <label class='left pad10'>Tổng phí khác: </label>"));
                    htmlReport.AppendLine(String.Format("        <div class='ordertotalprice'>"));
                    htmlReport.AppendLine(String.Format("            {0}", string.Format("{0:N0}", Convert.ToDouble(OtherFeeValue)).ToString()));
                    htmlReport.AppendLine(String.Format("        </div>"));
                    htmlReport.AppendLine(String.Format("    </div>"));
                    htmlReport.AppendLine(String.Format("</div>"));
                }
                htmlReport.AppendLine(String.Format("<div class='row pad'>"));
                htmlReport.AppendLine(String.Format("    <div class='col-md-3'>"));
                htmlReport.AppendLine(String.Format("        <label class='left pad10'>Số đơn lấy trực tiếp: </label>"));
                htmlReport.AppendLine(String.Format("        <div class='orderquantity'>"));
                htmlReport.AppendLine(String.Format("            {0}", ShippingType1.ToString()));
                htmlReport.AppendLine(String.Format("        </div>"));
                htmlReport.AppendLine(String.Format("    </div>"));
                htmlReport.AppendLine(String.Format("    <div class='col-md-3'>"));
                htmlReport.AppendLine(String.Format("        <label class='left pad10'>Số đơn chuyển bưu điện: </label>"));
                htmlReport.AppendLine(String.Format("        <div class='ordertotalprice'>"));
                htmlReport.AppendLine(String.Format("            {0}", ShippingType2.ToString()));
                htmlReport.AppendLine(String.Format("        </div>"));
                htmlReport.AppendLine(String.Format("    </div>"));
                htmlReport.AppendLine(String.Format("    <div class='col-md-3'>"));
                htmlReport.AppendLine(String.Format("        <label class='left pad10'>Số đơn gửi dịch vụ: </label>"));
                htmlReport.AppendLine(String.Format("        <div class='orderstatus'>"));
                htmlReport.AppendLine(String.Format("            {0}", ShippingType3.ToString()));
                htmlReport.AppendLine(String.Format("        </div>"));
                htmlReport.AppendLine(String.Format("    </div>"));
                htmlReport.AppendLine(String.Format("    <div class='col-md-3'> "));
                htmlReport.AppendLine(String.Format("        <label class='left pad10'>Số đơn chuyển xe: </label>"));
                htmlReport.AppendLine(String.Format("        <div class='ordernote'>"));
                htmlReport.AppendLine(String.Format("            {0}", ShippingType4.ToString()));
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
            html.Append("    <th>Xử lý</th>");
            html.Append("    <th>Thanh toán</th>");
            html.Append("    <th>Thanh toán</th>");
            html.Append("    <th>Giao hàng</th>");
            html.Append("    <th>Tổng tiền</th>");
            if (acc.RoleID == 0)
            {
                html.Append("    <th>Nhân viên</th>");
            }
            html.Append("    <th>Ngày tạo</th>");
            html.Append("    <th>Hoàn tất</th>");
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

                    int RefundsGoodsID = 0;
                    double TotalRefund = 0;
                    if (item.RefundsGoodsID != null)
                    {
                        var refund = RefundGoodController.GetByID(Convert.ToInt32(item.RefundsGoodsID));
                        if (refund != null)
                        {
                            RefundsGoodsID = refund.ID;
                            TotalRefund = Convert.ToDouble(refund.TotalPrice);
                        }
                    }
                    
                    html.Append("<tr>");
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
                    html.Append("   <td>" + PJUtils.OrderExcuteStatus(Convert.ToInt32(item.ExcuteStatus)) + "</td>");
                    html.Append("   <td>" + PJUtils.OrderPaymentStatus(Convert.ToInt32(item.PaymentStatus)) + "</td>");
                    #region Phương thức thanh toán
                    html.Append("   <td>");
                    html.Append(PJUtils.PaymentType(Convert.ToInt32(item.PaymentType)));
                    // Đã nhận tiền
                    if (item.TransferStatus.HasValue && item.TransferStatus.Value == 1)
                        html.Append("       <br/><span class='bg-blue'>Đã nhận tiền</span>");
                    html.Append("   </td>");
                    #endregion
                    html.Append("   <td>" + PJUtils.ShippingType(Convert.ToInt32(item.ShippingType)) + "</td>");
                    html.Append("   <td><strong>" + string.Format("{0:N0}", Convert.ToDouble(item.TotalPrice - TotalRefund)) + "</strong></td>");

                    if (acc.RoleID == 0)
                    {
                        html.Append("   <td>" + item.CreatedBy + "</td>");
                    }

                    string date = string.Format("{0:dd/MM}", item.CreatedDate);
                    html.Append("   <td>" + date + "</td>");

                    string datedone = "";
                    if (item.ExcuteStatus == 2)
                    {
                        datedone = string.Format("{0:dd/MM}", item.DateDone);
                    }
                    html.Append("   <td>" + datedone + "</td>");

                    html.Append("   <td>");
                    html.Append("       <a href=\"/print-invoice?id=" + item.ID + "\" title=\"In hóa đơn\" target=\"_blank\" class=\"btn primary-btn h45-btn\"><i class=\"fa fa-print\" aria-hidden=\"true\"></i></a>");
                    html.Append("       <a href=\"/print-shipping-note?id=" + item.ID + "\" title=\"In phiếu gửi hàng\" target=\"_blank\" class=\"btn primary-btn btn-red h45-btn\"><i class=\"fa fa-file-text-o\" aria-hidden=\"true\"></i></a>");
                    html.Append("       <a href=\"/chi-tiet-khach-hang?id=" + item.CustomerID + "\" title=\"Thông tin khách hàng " + item.CustomerName + "\" target=\"_blank\" class=\"btn primary-btn btn-black h45-btn\"><i class=\"fa fa-user-circle\" aria-hidden=\"true\"></i></a>");
                    html.Append("   </td>");
                    html.Append("</tr>");

                    // thông tin thêm

                    html.Append("<tr class='tr-more-info'>");
                    html.Append("   <td colspan='2'>");
                    html.Append("   </td>");
                    html.Append("   <td colspan='4'>");

                    if(RefundsGoodsID != 0)
                    {
                        html.Append("<span class='order-info'><strong>Trừ hàng trả:</strong> " + string.Format("{0:N0}", TotalRefund) + " (<a href='xem-don-hang-doi-tra?id=" + RefundsGoodsID + "' target='_blank'>Xem đơn " + RefundsGoodsID + "</a>)</span>");
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
                    if(item.ShippingType == 4)
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
                    html.Append("   <td colspan='7'>");
                    
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
            string search = txtSearchOrder.Text.Trim();
            string request = "/danh-sach-don-hang?";

            if (search != "")
            {
                request += "&textsearch=" + search;
            }

            if(ddlOrderType.SelectedValue != "")
            {
                request += "&ordertype=" + ddlOrderType.SelectedValue;
            }

            if (ddlPaymentStatus.SelectedValue != "")
            {
                request += "&paymentstatus=" + ddlPaymentStatus.SelectedValue;
            }

            if (ddlExcuteStatus.SelectedValue != "")
            {
                request += "&excutestatus=" + ddlExcuteStatus.SelectedValue;
            }

            if (ddlPaymentType.SelectedValue != "")
            {
                request += "&paymenttype=" + ddlPaymentType.SelectedValue;
            }

            if (ddlShippingType.SelectedValue != "")
            {
                request += "&shippingtype=" + ddlShippingType.SelectedValue;
            }

            if (ddlDiscount.SelectedValue != "")
            {
                request += "&discount=" + ddlDiscount.SelectedValue;
            }

            if (ddlOtherFee.SelectedValue != "")
            {
                request += "&otherfee=" + ddlOtherFee.SelectedValue;
            }

            if (ddlCreatedBy.SelectedValue != "")
            {
                request += "&createdby=" + ddlCreatedBy.SelectedValue;
            }

            if (ddlCreatedDate.SelectedValue != "")
            {
                request += "&createddate=" + ddlCreatedDate.SelectedValue;
            }

            if (ddlQuantityFilter.SelectedValue != "")
            {
                if (ddlQuantityFilter.SelectedValue == "greaterthan" || ddlQuantityFilter.SelectedValue == "lessthan")
                {
                    request += "&quantityfilter=" + ddlQuantityFilter.SelectedValue + "&quantity=" + txtQuantity.Text;
                }

                if (ddlQuantityFilter.SelectedValue == "between")
                {
                    request += "&quantityfilter=" + ddlQuantityFilter.SelectedValue + "&quantitymin=" + txtQuantityMin.Text + "&quantitymax=" + txtQuantityMax.Text;
                }
            }
            Response.Redirect(request);
        }
        public class danhmuccon1
        {
            public tbl_Category cate1 { get; set; }
            public string parentName { get; set; }
        }
    }
}