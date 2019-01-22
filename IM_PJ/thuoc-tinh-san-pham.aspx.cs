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

namespace IM_PJ
{
    public partial class thuoc_tinh_san_pham : System.Web.UI.Page
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
                        ViewState["role"] = acc.RoleID;
                        if(acc.RoleID == 0)
                        {
                            hdfcost.Value = "ok";
                        }
                        else if (acc.RoleID == 1)
                        {
                           
                        }
                        else if (acc.RoleID == 2)
                        {

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
            string s = "";
            if (Request.QueryString["s"] != null)
                s = Request.QueryString["s"];

            List<tbl_ProductVariable> a = new List<tbl_ProductVariable>();
            int productID = Request.QueryString["id"].ToInt(0);
            if (productID > 0)
            {
                ViewState["ID"] = productID;
                var product = ProductController.GetByID(productID);
                if (product != null)
                {
                    ViewState["ProductName"] = product.ProductTitle;
                    var cate = CategoryController.GetByID(Convert.ToInt32(product.CategoryID));
                    lblCategoryName.Text = product.ProductTitle;
                    ltrAddnew.Text = "<a href=\"/them-thuoc-tinh-san-pham?productid=" + productID + "\" class=\"h45-btn primary-btn btn\">Thêm mới</a>";
                }
                a = ProductVariableController.SearchProductID(productID, s);
                pagingall(a);
            }
        }
        #region Paging
        public void pagingall(List<tbl_ProductVariable> acs)
        {

            string productname = ViewState["ProductName"].ToString();
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
                    html.Append("<tr>");
                    if (!string.IsNullOrEmpty(item.Image))
                    {
                        html.Append("   <td><img src=\"" + item.Image + "\"/></td>");
                    }
                    else
                    {
                        html.Append("   <td><img src=\"/App_Themes/Ann/image/placeholder.png\"/></td>");
                    }
                    
                    string date = string.Format("{0:dd/MM/yyyy}", item.CreatedDate);
                    string ishidden = "";
                    if (item.IsHidden != null)
                    {
                        bool IsHidden = Convert.ToBoolean(item.IsHidden);
                        ishidden = PJUtils.IsHiddenStatus(IsHidden);
                    }
                    else
                    {
                        ishidden = PJUtils.IsHiddenStatus(false);
                    }

                    var value = ProductVariableValueController.GetByProductVariableID(item.ID);
                    if (value != null)
                    {
                        html.Append("   <td>");
                        string list = "";
                        foreach (var temp in value)
                        {
                            html.Append("" + temp.VariableName + ": " + temp.VariableValue + "</br>" + "");
                            list += temp.VariableName + "|";
                        }
                        html.Append("</td>");
                        hdfSearch.Value = list;
                    }

                    html.Append("   <td>" + item.SKU + "</td>");

                    html.Append("   <td>" + string.Format("{0:N0}", item.Regular_Price) + "</td>");

                    int k = Convert.ToInt32(ViewState["role"]);
                    if(k == 0)
                    {
                        html.Append("   <td>" + string.Format("{0:N0}", item.CostOfGood) + "</td>");
                    }

                    html.Append("   <td>" + string.Format("{0:N0}", item.RetailPrice) + "</td>");

                    html.Append("   <td>" + PJUtils.TotalProductQuantityInstock(1, item.SKU) + "</td>");
                    html.Append("   <td>" + PJUtils.StockStatusBySKU(1, item.SKU) + "</td>");

                    html.Append("   <td>" + date + "</td>");
                    html.Append("   <td>" + ishidden + "</td>");
                    html.Append("   <td>");
                    html.Append("       <a href=\"/thong-tin-thuoc-tinh-san-pham?id=" + item.ID + "\" title=\"Xem chi tiết\" class=\"btn primary-btn h45-btn\"><i class=\"fa fa-info-circle\" aria-hidden=\"true\"></i></a>");
                    html.Append("       <a href=\"/gia-tri-thuoc-tinh-san-pham?productvariableid=" + item.ID + "\" title=\"Xem thuộc tính\" class=\"btn primary-btn h45-btn\"><i class=\"fa fa-file-text-o\" aria-hidden=\"true\"></i></a>");
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
            string search = txtSearchProduct.Text;
            string id = ViewState["ID"].ToString();
            Response.Redirect("/thuoc-tinh-san-pham?id=" + id + "&s=" + search + "");

        }
    }
}