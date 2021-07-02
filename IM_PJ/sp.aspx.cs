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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.Services;
using static IM_PJ.Controllers.ProductController;
using IM_PJ.Utils;

namespace IM_PJ
{
    public partial class sp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["loginHiddenPage"] != null)
                {
                    LoadData();
                    LoadCategory();
                }
                else
                {
                    Response.Redirect("/login-hidden-page");
                }
            }
        }
        public void LoadCategory()
        {
            var category = CategoryController.GetAllWithIsHidden(false);
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new ListItem("Danh mục", "0"));
            if (category.Count > 0)
            {
                addItemCategory(0, "");
                ddlCategory.DataBind();
            }
        }

        public void addItemCategory(int id, string h = "")
        {
            var categories = CategoryController.GetByParentID("", id);

            if (categories.Count > 0)
            {
                foreach (var c in categories)
                {
                    ListItem listitem = new ListItem(h + c.CategoryName, c.ID.ToString());
                    ddlCategory.Items.Add(listitem);

                    addItemCategory(c.ID, h + "---");
                }
            }
        }
        public void LoadData()
        {
            string TextSearch = "";
            string CreatedDate = "";
            int CategoryID = 0;
            int StockStatus = 0;
            string strColor = String.Empty;
            string strSize = String.Empty;
            int Page = 1;

            if (Request.QueryString["textsearch"] != null)
                TextSearch = Request.QueryString["textsearch"].Trim();
            if (Request.QueryString["stockstatus"] != null)
                StockStatus = Request.QueryString["stockstatus"].ToInt();
            if (Request.QueryString["categoryid"] != null)
                CategoryID = Request.QueryString["categoryid"].ToInt();
            if (Request.QueryString["createddate"] != null)
                CreatedDate = Request.QueryString["createddate"];
            if (Request.QueryString["color"] != null)
                strColor = Request.QueryString["color"].Trim();
            if (Request.QueryString["size"] != null)
                strSize = Request.QueryString["size"].Trim();
            if (Request.QueryString["Page"] != null)
                Page = Request.QueryString["Page"].ToInt();

            txtSearchProduct.Text = TextSearch;
            ddlColor.SelectedValue = strColor;
            ddlSize.SelectedValue = strSize;
            ddlCategory.SelectedValue = CategoryID.ToString();
            ddlCreatedDate.SelectedValue = CreatedDate.ToString();
            ddlStockStatus.SelectedValue = StockStatus.ToString();

            // Create order fileter
            var filter = new ProductFilterModel()
            {
                category = CategoryID,
                search = TextSearch,
                color = strColor,
                size = strSize,
                stockStatus = StockStatus,
                productDate = CreatedDate
            };
            // Create pagination
            var page = new PaginationMetadataModel()
            {
                currentPage = Page,
                pageSize = 24
            };
            List<ProductSQL> a = new List<ProductSQL>();
            a = ProductController.GetAllSql(filter, ref page);

            pagingall(a, page);

        }
        [WebMethod]
        #region Paging
        public void pagingall(List<ProductSQL> acs, PaginationMetadataModel page)
        {
            string username = Request.Cookies["loginHiddenPage"].Value;
            var acc = AccountController.GetByUsername(username);

            var config = ConfigController.GetByTop1();

            StringBuilder html = new StringBuilder();
            html.Append("<div class='row'>");

            if (acs.Count > 0)
            {
                PageCount = page.totalPages;
                Int32 Page = page.currentPage;
                var index = 0;

                foreach (var item in acs)
                {
                    html.Append("<div class='col-md-3 item-" + index + " product-item'>");
                    html.Append("<div class='row'>");
                    html.Append("     <div class='col-xs-12'>");
                    html.Append("   <p><a href='/xem-sp?id=" + item.ID + "'><img src='" + Thumbnail.getURL(item.ProductImage, Thumbnail.Size.Large) + "'></a></p>");
                    html.Append("   <h3 class='product-name'><a href='/xem-sp?id=" + item.ID + "'>" + item.ProductSKU + " - " + item.ProductTitle + "</a></h3>");
                    html.Append("   <h3 class='product-price'>" + string.Format("{0:N0}", item.RegularPrice) + "</h3>");

                    if (!string.IsNullOrEmpty(item.Materials))
                    {
                        html.Append("   <p>🔖 Chất liệu: " + item.Materials + "</p>");
                    }

                    string content = Regex.Replace(item.ProductContent != null ? item.ProductContent : "", "<.*?>", "").ToString();
                    if (content.Trim() != "")
                    {
                        html.Append("   <p>🔖 " + content.Substring(0, content.Length > 100 ? 100 : content.Length) + "</p>");
                    }
                    var strStock2Quantity = String.Empty;
                    if (item.HasStock2)
                        strStock2Quantity = String.Format(" (Kho 2: {0:N0} cái)", item.Stock2Quantity);
                    html.Append(String.Format("   <p class='product-number' data-sku='{0}' data-product-id='{1}'>🔖 {2} ({3:N0} cái){4}</p>", item.ProductSKU, item.ID, item.ProductInstockStatus, item.TotalProductInstockQuantityLeft, strStock2Quantity));
                    html.Append("   <p>🔖 " + string.Format("{0:dd/MM/yyyy}", item.CreatedDate) + "</p>");
                    html.Append("     </div>");
                    html.Append("</div>");

                    html.Append("<div class='row'>");
                    html.Append("     <div class='col-xs-12'>");
                    html.Append("          <div class='col-xs-6'>");
                    html.Append("               <div class='row'>");
                    html.Append("                  <a href='javascript:;' class='btn primary-btn copy-btn h45-btn' onclick='copyProductInfo(`" + item.ID + "`);'><i class='fa fa-files-o' aria-hidden='true'></i> Copy</a>");
                    html.Append("               </div>");
                    html.Append("          </div>");
                    html.Append("          <div class='col-xs-6'>");
                    html.Append("               <div class='row'>");
                    html.Append("                  <a href ='javascript:;' class='btn primary-btn h45-btn' onclick='getAllProductImage(`" + item.ProductSKU + "`);'><i class='fa fa-cloud-download' aria-hidden='true'></i> Tải</a>");
                    html.Append("               </div>");
                    html.Append("          </div>");
                    //html.Append("          <div class='col-xs-4'>");
                    //html.Append("               <div class='row'>");
                    //html.Append("                  <a href='/dang-ky-nhap-hang?&textsearch=" + item.ProductSKU + "' class='btn primary-btn h45-btn download-btn'><i class='fa fa-cart-plus' aria-hidden='true'></i> Order</a>");
                    //html.Append("               </div>");
                    //html.Append("          </div>");
                    html.Append("     </div>");
                    html.Append("</div>");

                    if (acc.RoleID == 0 || acc.RoleID == 3)
                    {
                        if (config.HideProduct == 1)
                        {
                            html.Append("<div class='row'>");
                            html.Append("     <div class='col-xs-12'>");
                            html.Append("          <div class='col-xs-12'>");
                            html.Append("               <div class='row'>");
                            if (item.TotalProductInstockQuantityLeft > 0)
                            {
                                html.AppendLine("       <a href='javascript:;' title='Xả hết kho' class='liquidation-product-" + item.ID + " btn primary-btn btn-red h45-btn' onclick='liquidateProduct(" + item.CategoryID + ", " + item.ID + ", `" + item.ProductSKU + "`);'><i class='glyphicon glyphicon-trash' aria-hidden='true'></i> Xả kho</a>");
                            }
                            else if (item.Liquidated)
                            {
                                html.AppendLine("       <a href='javascript:;' title='Phục hồi xả kho' class='recover-liquidation-product-" + item.ID + " btn primary-btn btn-green h45-btn' onclick='recoverLiquidatedProduct(" + item.CategoryID + ", " + item.ID + ", `" + item.ProductSKU + "`);'><i class='glyphicon glyphicon-repeat' aria-hidden='true'></i>Phục hồi kho</a>");
                            }
                            html.Append("               </div>");
                            html.Append("          </div>");
                            html.Append("     </div>");
                            html.Append("</div>");
                        }
                    }

                    html.Append("</div>");

                    if((index + 1) % 4 == 0)
                    {
                        html.Append("<div class='clear'></div>");
                    }
                    index++;
                }
            }
            else
            {
                html.Append("<div class='col-md-12'>Không tìm thấy sản phẩm...</div>");
            }
            html.Append("</div>");

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
                Response.Write(GetHtmlPagingAdvanced(4, CurrentPage, PageCount, Context.Request.RawUrl, strText));

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
                output.Append("<li><a title=\"" + strText[0] + "\" href=\"" + string.Format(pageUrl, 1) + "\"><i class=\"fa fa-angle-left\"></i><i class=\"fa fa-angle-left\"></i></a></li>");
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

            //Link Next(Trang tiếp) và Last(Trang cuối)
            if (currentPage != pageCount)
            {
                output.Append("<li><a title=\"" + strText[3] + "\" href=\"" + string.Format(pageUrl, pageCount) + "\"><i class=\"fa fa-angle-right\"></i><i class=\"fa fa-angle-right\"></i></a></li>");
            }

            output.Append("</ul>");
            //output.Append("</div>");
            return output.ToString();
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearchProduct.Text;
            string request = "/sp?";

            if (search != "")
            {
                request += "&textsearch=" + search;
            }

            if (!String.IsNullOrEmpty(ddlColor.SelectedValue))
            {
                request += "&color=" + ddlColor.SelectedValue;
            }
            if (!String.IsNullOrEmpty(ddlSize.SelectedValue))
            {
                request += "&size=" + ddlSize.SelectedValue;
            }

            if (ddlStockStatus.SelectedValue != "")
            {
                request += "&stockstatus=" + ddlStockStatus.SelectedValue;
            }

            if (ddlCategory.SelectedValue != "0")
            {
                request += "&categoryid=" + ddlCategory.SelectedValue;
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

        [WebMethod]
        public static bool updateHidden(int productID, bool isHidden)
        {
            var loginHiddenPage = HttpContext.Current.Request.Cookies["loginHiddenPage"];
            var usernameLoginSystem = HttpContext.Current.Request.Cookies["usernameLoginSystem_ANN123"];
            tbl_Account acc;

            if (loginHiddenPage != null)
                acc = AccountController.GetByUsername(loginHiddenPage.Value);
            else if (usernameLoginSystem != null)
                acc = AccountController.GetByUsername(usernameLoginSystem.Value);
            else
                throw new Exception("Có vấn đề trong việc lấy thông tin User");

            return ProductController.updateHidden(acc, productID, isHidden);
        }
    }
}