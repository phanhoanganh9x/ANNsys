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

namespace IM_PJ
{
    public partial class danh_sach_don_hang_chuyen_hoan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
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
        public void LoadData()
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0 || acc.RoleID == 2)
                {
                    int agentID = Convert.ToInt32(acc.AgentID);
                    int orderType = 0;
                    int paymentStatus = 0;
                    int payment = 0;
                    int ship = 0;
                    string s = "";
                    string sku = "";
                    int by = 0;
                    if (Request.QueryString["o"] != null)
                    {
                        orderType = Request.QueryString["o"].ToInt(0);
                    }
                    if (Request.QueryString["p"] != null)
                    {
                        paymentStatus = Request.QueryString["p"].ToInt(0);
                    }
                    if (Request.QueryString["pay"] != null)
                    {
                        payment = Request.QueryString["pay"].ToInt(0);
                    }
                    if (Request.QueryString["sh"] != null)
                    {
                        ship = Request.QueryString["sh"].ToInt(0);
                    }
                    if (Request.QueryString["s"] != null)
                    {
                        s = Request.QueryString["s"].Trim();
                    }
                    if (Request.QueryString["sku"] != null)
                    {
                        sku = Request.QueryString["sku"];
                    }
                    if (Request.QueryString["sku"] != null)
                    {
                        sku = Request.QueryString["sku"];
                    }
                    if (Request.QueryString["by"] != null)
                    {
                        by = Request.QueryString["by"].ToInt(0);
                    }
                    txtAgentName.Text = s;
                    ddlOrderType.SelectedValue = orderType.ToString();
                    ddlPaymentStatus.SelectedValue = paymentStatus.ToString();
                    ddlPaymentType.SelectedValue = payment.ToString();
                    ddlShippingType.SelectedValue = ship.ToString();
                    ddlCreateBy.SelectedValue = by.ToString();

                
                    var rs = OrderController.SearchByStatical(orderType, paymentStatus, 4, s, agentID, payment, ship, sku);

                    //DateTime year = new DateTime(2019, 10, 23);

                    //var config = ConfigController.GetByTop1();
                    //if (config.ViewAllOrders == 1)
                    //{
                    //    year = new DateTime(2018, 6, 22);
                    //}

                    //rs = rs.Where(x => x.CreatedDate >= year).ToList();

                    if (rs.Count > 0)
                    {
                        if (acc.RoleID == 0)
                        {
                            hdfcreate.Value = "1";
                            if (by > 0)
                            {
                                var create = AccountController.GetByID(by);
                                if (create != null)
                                {
                                    pagingall(rs.Where(x=>x.CreatedBy == create.Username).OrderByDescending(x=>x.ModifiedDate).ToList());
                                }
                            }
                            else
                            {
                                pagingall(rs.OrderByDescending(x => x.ModifiedDate).ToList());
                            }
                        }
                        else
                        {
                            pagingall(rs.Where(x => x.CreatedBy == acc.Username).OrderByDescending(x => x.ModifiedDate).ToList());
                        }
                    }

                    ltrNumberOfOrder.Text = rs.Count().ToString();
                }
            }
        }

        public void LoadCreatedBy(int AgentID, tbl_Account acc = null)
        {
            if(acc != null)
            {
                ddlCreateBy.Items.Clear();
                ddlCreateBy.Items.Insert(0, new ListItem(acc.Username, acc.ID.ToString()));
            }
            else
            {
                var CreateBy = AccountController.GetAllNotSearch();
                ddlCreateBy.Items.Clear();
                ddlCreateBy.Items.Insert(0, new ListItem("Nhân viên", "0"));
                if (CreateBy.Count > 0)
                {
                    foreach (var p in CreateBy)
                    {
                        ListItem listitem = new ListItem(p.Username, p.ID.ToString());
                        ddlCreateBy.Items.Add(listitem);
                    }
                    ddlCreateBy.DataBind();
                }
            }
            
        }
        #region Paging
        public void pagingall(List<tbl_Order> acs)
        {
            int PageSize = 30;
            StringBuilder html = new StringBuilder();
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
                    html.Append("<tr>");
                    html.Append("   <td><a href=\"/thong-tin-don-hang?id=" + item.ID + "\">" + item.ID + "</a></td>");
                    html.Append("   <td>" + PJUtils.OrderTypeStatus(Convert.ToInt32(item.OrderType)) + "</td>");
                    html.Append("   <td>" + item.CustomerPhone + "</td>");
                    var customer = CustomerController.GetByID(Convert.ToInt32(item.CustomerID));
                    if(customer != null)
                    {
                        if (!string.IsNullOrEmpty(customer.Nick))
                        {
                            html.Append("   <td><a class=\"customer-name-link\" href=\"/thong-tin-don-hang-chuyen-hoan?id=" + item.ID + "\">" + customer.Nick.ToTitleCase() + "</a><br><span class=\"name-bottom-nick\">(" + item.CustomerName.ToTitleCase() + ")</span></td>");
                        }
                        else
                        {
                            html.Append("   <td><a class=\"customer-name-link\" href=\"/thong-tin-don-hang-chuyen-hoan?id=" + item.ID + "\">" + item.CustomerName.ToTitleCase() + "</a></td>");
                        }
                    }
                    else
                    {
                        html.Append("   <td><a class=\"customer-name-link\" href=\"/thong-tin-don-hang-chuyen-hoan?id=" + item.ID + "\">" + item.CustomerName.ToTitleCase() + "</a></td>");
                    }
                    
                    var orderdetails = OrderDetailController.GetByOrderID(item.ID);
                    int quantity = 0;
                    if (orderdetails.Count > 0)
                    {
                        foreach (var temp in orderdetails)
                        {
                            quantity += Convert.ToInt32(temp.Quantity);
                        }
                    }
                    html.Append("   <td>" + quantity + "</td>");
                    html.Append("   <td>" + PJUtils.OrderPaymentStatus(Convert.ToInt32(item.PaymentStatus)) + "</td>");
                    html.Append("   <td>" + PJUtils.OrderExcuteStatus(Convert.ToInt32(item.ExcuteStatus)) + "</td>");
                    html.Append("   <td>" + PJUtils.PaymentType(Convert.ToInt32(item.PaymentType)) + "</td>");
                    html.Append("   <td>" + PJUtils.ShippingType(Convert.ToInt32(item.ShippingType)) + "</td>");
                    html.Append("   <td>" + string.Format("{0:N0}", Convert.ToDouble(item.TotalPrice)) + "</td>");

                    html.Append("   <td>" + string.Format("{0:dd/MM}", item.DateDone) + "</td>");

                    html.Append("   <td>" + string.Format("{0:dd/MM}", item.ModifiedDate) + "</td>");

                    html.Append("   <td>");
                    html.Append("       <a href=\"/chi-tiet-khach-hang?id=" + item.CustomerID + "\" title=\"Thông tin khách hàng " + item.CustomerName + "\" target=\"_blank\" class=\"btn primary-btn btn-black h45-btn\"><i class=\"fa fa-user-circle\" aria-hidden=\"true\"></i></a>");
                    html.Append("   </td>");
                    html.Append("</tr>");
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
            string search = txtAgentName.Text;
            Response.Redirect("/danh-sach-don-hang-chuyen-hoan?s=" + search + "&o=" + ddlOrderType.SelectedValue + "&p=" + ddlPaymentStatus.SelectedValue + "&pay=" + ddlPaymentType.SelectedValue + "&sh=" + ddlShippingType.SelectedValue + "&by=" + ddlCreateBy.SelectedValue + "");
        }
        public class danhmuccon1
        {
            public tbl_Category cate1 { get; set; }
            public string parentName { get; set; }
        }
    }
}