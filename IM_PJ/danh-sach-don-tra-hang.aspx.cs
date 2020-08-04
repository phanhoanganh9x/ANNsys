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
using static IM_PJ.Controllers.RefundGoodController;

namespace IM_PJ
{
    public partial class danh_sach_don_tra_hang : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);
                    int agent = acc.AgentID.ToString().ToInt();

                    if (acc != null)
                    {
                        if(acc.RoleID == 0)
                        {
                            LoadCreatedBy(agent);
                        }
                        else if(acc.RoleID == 2)
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
            string username = Request.Cookies["usernameLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                string TextSearch = String.Empty;
                string RefundFee = String.Empty;
                int Status = 0;
                string CreatedBy = String.Empty;
                string CreatedDate = String.Empty;
                int Page = 1;

                if (Request.QueryString["textsearch"] != null)
                {
                    TextSearch = Request.QueryString["textsearch"].Trim();
                }
                if (Request.QueryString["status"] != null)
                {
                    Status = Request.QueryString["status"].ToInt();
                }
                if (Request.QueryString["refundfee"] != null)
                {
                    RefundFee = Request.QueryString["refundfee"];
                }
                if (Request.QueryString["CreatedBy"] != null)
                {
                    CreatedBy = Request.QueryString["createdby"];
                }
                if (Request.QueryString["createddate"] != null)
                {
                    CreatedDate = Request.QueryString["createddate"];
                }
                if (Request.QueryString["Page"] != null)
                {
                    Page = Request.QueryString["Page"].ToInt();
                }

                txtSearchOrder.Text = TextSearch;
                ddlStatus.SelectedValue = Status.ToString();
                ddlRefundFee.SelectedValue = RefundFee.ToString();
                ddlCreatedBy.Text = CreatedBy.ToString();
                ddlCreatedDate.SelectedValue = CreatedDate.ToString();
                ddlCreatedBy.Enabled = acc.RoleID == 0;

                var filter = new RefundFilterModel()
                {
                    search = TextSearch,
                    status = Status,
                    feeStatus = RefundFee,
                    dateTimePicker = CreatedDate,
                    staff = acc.RoleID != 0 ? acc.Username : CreatedBy
                };
                // Create pagination
                var pagination = new PaginationMetadataModel()
                {
                    currentPage = Page
                };
                var rs = RefundGoodController.Filter(filter, ref pagination);

                pagingall(rs, pagination);
                ltrNumberOfOrder.Text = pagination.totalCount.ToString();
                PageCount = pagination.totalPages;
            }
        }
        #region Paging
        public void pagingall(List<RefundOrder> data, PaginationMetadataModel pagination)
        {
            string username = Request.Cookies["usernameLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);

            StringBuilder html = new StringBuilder();
            html.Append("<thead>");
            html.Append("<tr>");
            html.Append("    <th>Mã</th>");
            html.Append("    <th>Khách hàng</th>");
            html.Append("    <th>Số lượng</th>");
            html.Append("    <th>Phí đổi hàng</th>");
            html.Append("    <th>Tổng tiền</th>");
            html.Append("    <th>Trạng thái</th>");
            html.Append("    <th>Đơn hàng trừ tiền</th>");
            if (acc.RoleID == 0)
            {
                html.Append("    <th>Nhân viên</th>");
            }
            html.Append("    <th>Ngày tạo</th>");
            html.Append("    <th></th>");
            html.Append("</thead>");

            html.Append("<tbody>");

            foreach (var item in data)
            {
                html.Append("<tr>");
                html.Append("   <td data-title='Mã đơn'><a target='_blank' href='/xem-don-hang-doi-tra?id=" + item.ID + "'>" + item.ID + "</a></td>");

                if (!string.IsNullOrEmpty(item.Nick))
                {
                    html.Append("   <td data-title='Khách hàng' class='customer-td'><a target='_blank' class='customer-name-link' href='/xem-don-hang-doi-tra?id=" + item.ID + "'>" + item.Nick.ToTitleCase() + "</a><br><span class='name-bottom-nick'>(" + item.CustomerName.ToTitleCase() + ")</span></td>");
                }
                else
                {
                    html.Append("   <td data-title='Khách hàng' class='customer-td'><a target='_blank' class='customer-name-link' href='/xem-don-hang-doi-tra?id=" + item.ID + "'>" + item.CustomerName.ToTitleCase() + "</a></td>");
                }

                html.Append("   <td data-title='Số lượng'>" + string.Format("{0:N0}", Convert.ToDouble(item.Quantity)) + "</td>");
                html.Append("   <td data-title='Phí đổi hàng'>" + string.Format("{0:N0}", Convert.ToDouble(item.TotalRefundFee)) + "</td>");
                html.Append("   <td data-title='Tổng tiền'><strong>" + string.Format("{0:N0}", Convert.ToDouble(item.TotalPrice)) + "</strong></td>");
                html.Append("   <td data-title='Trạng thái'>" + PJUtils.RefundStatus(Convert.ToInt32(item.Status)) + "</td>");

                if (item.OrderSaleID > 0)
                {
                    html.Append("   <td data-title='Đơn trừ tiền'><a class='customer-name-link' target='_blank' title='Bấm vào xem đơn hàng trừ tiền' href='/thong-tin-don-hang?id=" + item.OrderSaleID + "'>" + item.OrderSaleID + "</a></td>");
                }
                else
                {
                    html.Append("   <td data-title='Đơn trừ tiền'></td>");
                }

                if (acc.RoleID == 0)
                {
                    html.Append("   <td data-title='Nhân viên'>" + item.CreatedBy + "</td>");
                }

                string date = string.Format("<strong>{0:dd/MM}</strong><br>{0:HH:mm}", item.CreatedDate);
                html.Append("   <td data-title='Ngày tạo'>" + date + "</td>");

                html.Append("   <td data-title='Thao tác' class='update-button'>");
                html.Append("       <a href='/print-invoice-return?id=" + item.ID + "' title='In hóa đơn' target='_blank' class='btn primary-btn h45-btn'><i class='fa fa-print' aria-hidden='true'></i></a>");
                html.Append("       <a href='/print-return-order-image?id=" + item.ID + "' title='Lấy ảnh đơn hàng' target='_blank' class='btn primary-btn btn-red h45-btn'><i class='fa fa-picture-o' aria-hidden='true'></i></a>");
                html.Append("   </td>");
                html.Append("</tr>");
            }

            if (data.Count == 0)
            {
                if (acc.RoleID == 0)
                    html.Append("<tr><td colspan='10'>Không tìm thấy đơn hàng...</td></tr>");
                else
                    html.Append("<tr><td colspan='9'>Không tìm thấy đơn hàng...</td></tr>");
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


        #region Print
        [WebMethod]
        public static string getOrder(int ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string username = HttpContext.Current.Request.Cookies["usernameLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                int AgentID = Convert.ToInt32(acc.AgentID);
                var agent = AgentController.GetByID(AgentID);
                if (agent != null)
                {

                }
                //List<GetOutOrder> go = new List<GetOutOrder>();
                GetOutRefundOrder getOrder = new GetOutRefundOrder();
                var refund = RefundGoodController.GetByID(ID);
                if (refund != null)
                {
                    var refunddetail = RefundGoodDetailController.GetByRefundGoodsID(refund.ID);
                    if (refunddetail != null)
                    {
                        //table 1
                        getOrder.ID = refund.ID;
                        getOrder.CustomerName = refund.CustomerName;
                        getOrder.CustomerPhone = refund.CustomerPhone;
                        if (refund.Status == 1)
                            getOrder.Status = "Chưa trừ tiền";
                        if (refund.Status == 2)
                            getOrder.Status = "Đã trừ tiền";
                        getOrder.CreatedBy = refund.CreatedBy;
                        getOrder.CreateDate = string.Format("{0:dd/MM/yyyy}", refund.CreatedDate);


                        getOrder.ListAgent += agent.AgentAddress + "|";
                        getOrder.ListAgent += agent.AgentPhone;


                        for (int j = 0; j < refunddetail.Count(); j++)
                        {
                            getOrder.ListProduct += refunddetail[j].SKU + ";" + refunddetail[j].ProductName + ";";
                            var productvalue = ProductVariableValueController.GetByProductVariableSKU(refunddetail[j].SKU);
                            string value = "";
                            if (productvalue != null)
                            {
                                foreach (var item in productvalue)
                                {
                                    value += item.VariableName + ":" + item.VariableValue + "|";
                                }
                            }
                            getOrder.ListProduct += value + ";" + refunddetail[j].Quantity +";" + refunddetail[j].SoldPricePerProduct + ";" + refunddetail[j].RefundFeePerProduct +";" + refunddetail[j].TotalPriceRow + "*";

                        }

                        getOrder.TotalQuantity = Convert.ToInt32(refund.TotalQuantity);
                        getOrder.TotalPrice = Convert.ToInt32(refund.TotalPrice);
                        getOrder.TotalRefundPrice = Convert.ToInt32(refund.TotalRefundFee);
                    
                    }
                }

                return serializer.Serialize(getOrder);
            }
            return serializer.Serialize(null);
        }

        public class GetOutRefundOrder
        {
            //table 1
            public int ID { get; set; }
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public string CreateDate { get; set; }
            public string CreatedBy { get; set; }
            public string Status { get; set; }

            //table 2
            public string ListProduct { get; set; }
            public int TotalQuantity { get; set; }
            public int TotalRefundPrice { get; set; }
            public int TotalPrice { get; set; }

            public string ListAgent { get; set; }
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearchOrder.Text.Trim();
            string request = "/danh-sach-don-tra-hang?";

            if (search != "")
            {
                request += "&textsearch=" + search;
            }

            if (ddlStatus.SelectedValue != "0")
            {
                request += "&status=" + ddlStatus.SelectedValue;
            }

            if (ddlRefundFee.SelectedValue != "")
            {
                request += "&refundfee=" + ddlRefundFee.SelectedValue;
            }

            if (ddlCreatedBy.SelectedValue != "")
            {
                request += "&createdby=" + ddlCreatedBy.SelectedValue;
            }

            if (ddlCreatedDate.SelectedValue != "")
            {
                request += "&createddate=" + ddlCreatedDate.SelectedValue;
            }

            Response.Redirect(request);

        }
        public class danhmuccon1
        {
            public tbl_Category cate1 { get; set; }
            public string parentName { get; set; }
        }

        //protected void ddlAgentName_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var AgentID = ddlAgentName.SelectedValue.ToInt();
        //    LoadDLLCreateBy(AgentID);
        //}
    }
}