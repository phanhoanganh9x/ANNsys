﻿using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace IM_PJ
{
    public partial class danh_sach_khach_giam_gia : System.Web.UI.Page
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
                        if (acc.RoleID != 0)
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

        public void LoadCustomerNotIn(int groupID)
        {
            var customer = CustomerController.GetNotInGroupByGroupID(groupID);
            ddlCustomer.Items.Clear();
            ddlCustomer.Items.Insert(0, new ListItem("Chọn khách hàng", "0"));
            if (customer.Count > 0)
            {
                customer = customer.OrderBy(o => o.CustomerName).ToList();
                foreach (var p in customer)
                {
                    ListItem listitem = new ListItem(p.CustomerName + " - " + p.CustomerPhone, p.ID.ToString());
                    ddlCustomer.Items.Add(listitem);
                }
                ddlCustomer.DataBind();
            }
        }
        public void LoadCustomerIn(int groupID)
        {
            var c = DiscountCustomerController.GetByGroupID(groupID);
            if (c.Count > 0)
            {
                pagingall(c.OrderBy(o => o.CustomerName).ToList());
            }
        }
        public void LoadData()
        {
            int id = Request.QueryString["id"].ToInt(0);
            if (id > 0)
            {
                var d = DiscountGroupController.GetByID(id);
                if (d != null)
                {
                    ViewState["ID"] = id;
                    LoadCustomerIn(id);
                    LoadCustomerNotIn(id);
                }
            }
        }
        #region Paging
        public void pagingall(List<tbl_DiscountCustomer> acs)
        {
            int PageSize = 15;
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
                    html.Append("<tr data-id=\"" + item.ID + "\">");
                    html.Append("   <td>" + item.CustomerName + "</td>");
                    html.Append("   <td>" + item.CustomerPhone + "</td>");
                    html.Append("   <td class=\"ishidden\">" + PJUtils.IsHiddenStatus(Convert.ToBoolean(item.IsHidden)) + "</td>");
                    string date = "";
                    if (item.CreatedDate != null)
                        date = string.Format("{0:dd/MM/yyyy}", item.CreatedDate);
                    html.Append("   <td>" + date + "</td>");
                    html.Append("   <td>");
                    if (item.IsHidden == true)
                        html.Append("       <a href=\"javascript:;\" onclick=\"showcustomer($(this))\" class=\"btn primary-btn fw-btn not-fullwidth\">Hiện</a>");
                    else
                        html.Append("       <a href=\"javascript:;\" onclick=\"hiddencustomer($(this))\" class=\"btn primary-btn fw-btn not-fullwidth\">Ẩn</a>");

                    html.Append("       <a href=\"javascript:;\" onclick=\"deletecustomer($(this))\" class=\"btn primary-btn fw-btn not-fullwidth\">Xóa</a>");
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
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            int groupID = Convert.ToInt32(ViewState["ID"]);
            string username = Request.Cookies["userLoginSystem"].Value;
            DateTime currentDate = DateTime.Now;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0)
                {
                    int custID = ddlCustomer.SelectedValue.ToInt(0);
                    if (custID > 0)
                    {
                        var cus = CustomerController.GetByID(custID);
                        if (cus != null)
                        {
                            DiscountCustomerController.Insert(groupID, cus.ID, cus.CustomerName, cus.CustomerPhone, false, currentDate, username);
                            PJUtils.ShowMessageBoxSwAlert("Thêm khách hàng vào nhóm thành công", "s", true, Page);
                        }
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Vui lòng chọn khách hàng cần thêm", "e", true, Page);
                    }
                }
            }
        }

        protected void btnHidden_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0)
                {
                    int ID = hdfCustomerID.Value.ToInt(0);
                    if (ID > 0)
                    {
                        var c = DiscountCustomerController.GetByID(ID);
                        if (c != null)
                        {
                            DiscountCustomerController.UpdateIsHidden(c.ID, true, DateTime.Now, username);
                            PJUtils.ShowMessageBoxSwAlert("Ẩn khách hàng thành công", "s", true, Page);
                        }
                    }
                }
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0)
                {
                    int ID = hdfCustomerID.Value.ToInt(0);
                    if (ID > 0)
                    {
                        var c = DiscountCustomerController.GetByID(ID);
                        if (c != null)
                        {
                            DiscountCustomerController.UpdateIsHidden(c.ID, false, DateTime.Now, username);
                            PJUtils.ShowMessageBoxSwAlert("Hiện khách hàng thành công", "s", true, Page);
                        }
                    }
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0)
                {
                    int ID = hdfCustomerID.Value.ToInt(0);
                    if (ID > 0)
                    {
                        var c = DiscountCustomerController.GetByID(ID);
                        if (c != null)
                        {
                            DiscountCustomerController.Delete(c.ID);
                            PJUtils.ShowMessageBoxSwAlert("Xóa khách hàng thành công", "s", true, Page);
                        }
                    }
                }
            }
        }
    }
}