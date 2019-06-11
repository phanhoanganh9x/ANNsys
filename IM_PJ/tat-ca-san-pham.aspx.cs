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
    public partial class tat_ca_san_pham : System.Web.UI.Page
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
                        LoadData();
                        LoadCategory();
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
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
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0 || acc.RoleID == 1)
                {
                    ltrAddProduct.Text = "<a href=\"/tao-san-pham\" class=\"h45-btn btn primary-btn\">Thêm mới</a>";
                }

                string TextSearch = "";
                string CreatedDate = "";
                int CategoryID = 0;
                int StockStatus = 0;
                string ShowHomePage = "";
                string QuantityFilter = "";
                int Quantity = 0;
                int QuantityMin = 0;
                int QuantityMax = 0;

                if (Request.QueryString["textsearch"] != null)
                    TextSearch = Request.QueryString["textsearch"].Trim();
                if (Request.QueryString["stockstatus"] != null)
                    StockStatus = Request.QueryString["stockstatus"].ToInt();
                if (Request.QueryString["categoryid"] != null)
                    CategoryID = Request.QueryString["categoryid"].ToInt();
                if (Request.QueryString["createddate"] != null)
                    CreatedDate = Request.QueryString["createddate"];
                if (Request.QueryString["showhomepage"] != null)
                    ShowHomePage = Request.QueryString["showhomepage"];

                if (Request.QueryString["quantityfilter"] != null)
                {
                    QuantityFilter = Request.QueryString["quantityfilter"];

                    if (QuantityFilter == "greaterthan" || QuantityFilter == "lessthan")
                    {
                        Quantity = Request.QueryString["quantity"].ToInt();
                    }
                    if(QuantityFilter == "between")
                    {
                        QuantityMin = Request.QueryString["quantitymin"].ToInt();
                        QuantityMax = Request.QueryString["quantitymax"].ToInt();
                    }
                }

                txtSearchProduct.Text = TextSearch;
                ddlCategory.SelectedValue = CategoryID.ToString();
                ddlCreatedDate.SelectedValue = CreatedDate.ToString();
                ddlStockStatus.SelectedValue = StockStatus.ToString();
                ddlShowHomePage.SelectedValue = ShowHomePage.ToString();

                ddlQuantityFilter.SelectedValue = QuantityFilter.ToString();
                txtQuantity.Text = Quantity.ToString();
                txtQuantityMin.Text = QuantityMin.ToString();
                txtQuantityMax.Text = QuantityMax.ToString();

                List<ProductSQL> a = new List<ProductSQL>();
                a = ProductController.GetAllSql(CategoryID, TextSearch);

                if (StockStatus != 0)
                {
                    a = a.Where(p => p.StockStatus == StockStatus).ToList();
                }

                if(ShowHomePage != "")
                {
                    a = a.Where(p => p.ShowHomePage == ShowHomePage.ToInt()).ToList();
                }

                if (CreatedDate != "")
                {
                    DateTime fromdate = DateTime.Today;
                    DateTime todate = DateTime.Now;
                    switch (CreatedDate)
                    {
                        case "today":
                            fromdate = DateTime.Today;
                            todate = DateTime.Now;
                            break;
                        case "yesterday":
                            fromdate = fromdate.AddDays(-1);
                            todate = DateTime.Today;
                            break;
                        case "beforeyesterday":
                            fromdate = DateTime.Today.AddDays(-2);
                            todate = DateTime.Today.AddDays(-1);
                            break;
                        case "week":
                            int days = DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.Today.DayOfWeek;
                            fromdate = fromdate.AddDays(-days + 1);
                            todate = DateTime.Now;
                            break;
                        case "month":
                            fromdate = new DateTime(fromdate.Year, fromdate.Month, 1);
                            todate = DateTime.Now;
                            break;
                        case "7days":
                            fromdate = DateTime.Today.AddDays(-6);
                            todate = DateTime.Now;
                            break;
                        case "30days":
                            fromdate = DateTime.Today.AddDays(-29);
                            todate = DateTime.Now;
                            break;
                    }
                    a = a.Where(p => p.CreatedDate >= fromdate && p.CreatedDate <= todate ).ToList();
                }

                if(QuantityFilter != "")
                {
                    if (QuantityFilter == "greaterthan")
                    {
                        a = a.Where(p => p.TotalProductInstockQuantityLeft >= Quantity).ToList();
                    }
                    else if(QuantityFilter == "lessthan")
                    {
                        a = a.Where(p => p.TotalProductInstockQuantityLeft <= Quantity).ToList();
                    }
                    else if (QuantityFilter == "between")
                    {
                        a = a.Where(p => p.TotalProductInstockQuantityLeft >= QuantityMin && p.TotalProductInstockQuantityLeft <= QuantityMax).ToList();
                    }
                }

                pagingall(a);

                ltrNumberOfProduct.Text = a.Count().ToString();
            }
        }
        [WebMethod]
        public static string getAllProductImage(string sku)
        {
            var product = ProductController.GetBySKU(sku);
            List<string> images = new List<string>();
            if (product != null)
            {
                images.Add(product.ProductImage);

                // lấy ảnh sản phẩm từ thư viện

                var imageProduct = ProductImageController.GetByProductID(product.ID);

                if(imageProduct != null)
                {
                    foreach (var img in imageProduct)
                    {
                        images.Add(img.ProductImage);
                    }
                }
                

                // lấy ảnh sản phẩm từ biến thể

                var variable = ProductVariableController.GetByParentSKU(product.ProductSKU);

                if(variable != null)
                {
                    foreach (var v in variable)
                    {
                        images.Add(v.Image);
                    }
                }

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(images.Distinct().ToList());
        }
        [WebMethod]
        public static string updateShowHomePage(int id, int value)
        {
            string update = ProductController.updateShowHomePage(id, value);
            if(update != null)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }
        [WebMethod]
        public static string copyProductInfo(int id)
        {
            var product = ProductController.GetByID(id);
            StringBuilder html = new StringBuilder();
            if (product != null)
            {
                html.Append("<p>" + product.ProductSKU + " - Sỉ " + (product.Regular_Price / 1000).ToString() + "k - " + product.ProductTitle + "</p>\r\n");
                html.Append("<p></p>\r\n");
                html.Append("<p>📌 Giá sỉ: " + (product.Regular_Price / 1000).ToString() + "k</p>\r\n");
                html.Append("<p></p>\r\n");
                html.Append("<p>📌 Giá lẻ: " + ((product.Retail_Price + 25000) / 1000).ToString() + "k</p>\r\n");
                html.Append("<p></p>\r\n");

                if (!string.IsNullOrEmpty(product.Materials))
                {
                    html.Append("<p>🔖 Chất liệu: " + product.Materials + "</p>\r\n");
                    html.Append("<p></p>\r\n");
                }

                if (!string.IsNullOrEmpty(product.ProductContent))
                {
                    html.Append("<p>🔖 Mô tả: " + product.ProductContent + "</p>\r\n");
                    html.Append("<p></p>\r\n");
                }

                // liệt kê thuộc tính sản phẩm

                List<ProductVariable> variableTemp = new List<ProductVariable>();

                List<tbl_ProductVariable> v = new List<tbl_ProductVariable>();

                v = ProductVariableController.SearchProductID(id, "");

                string Variable = "";
                if (v.Count > 0)
                {
                    for (int i = 0; i < v.Count; i++)
                    {
                        var item = v[i];
                        var value = ProductVariableValueController.GetByProductVariableIDSortByName(item.ID);
                        if (value != null)
                        {
                            for (int j = 0; j < value.Count; j++)
                            {
                                variableTemp.Add(new ProductVariable() { VariableName = value[j].VariableName, VariableValue = value[j].VariableValue });

                            }
                        }
                    }
                    var vari = variableTemp.GroupBy(x => new { x.VariableName, x.VariableValue }).Select(x => new { VariableName = x.Key.VariableName, VariableValue = x.Key.VariableValue }).OrderBy(x => x.VariableName).ToList();

                    string stringVariable = vari[0].VariableName;

                    Variable = "<p><strong>📚 " + vari[0].VariableName + "</strong>: ";

                    for (int y = 0; y < vari.Count; y++)
                    {
                        if (stringVariable == vari[y].VariableName)
                        {
                            Variable += vari[y].VariableValue + "; ";
                        }
                        else
                        {
                            Variable += "</p>\r\n";
                            Variable += "<p></p>\r\n";
                            Variable += "<p><strong>📐 " + vari[y].VariableName + "</strong>: " + vari[y].VariableValue + "; ";
                            stringVariable = vari[y].VariableName;
                        }
                    }

                    html.Append(Variable);
                }

                html.Append("<p></p>\r\n");
                html.Append("<p></p>\r\n");

                if (product.ID%3 == 0)
                {
                    // thông tin liên hệ
                    
                    html.Append("<p>-----------------------------------------------------------</p>\r\n");
                    html.Append("<p></p>\r\n");
                    html.Append("<p>⚡⚡ Hàng có sẵn tại KHO HÀNG SỈ ANN ⚡⚡</p>\r\n");
                    html.Append("<p></p>\r\n");
                    html.Append("<p>🏭 68 Đường C12, P.13, Tân Bình, TP.HCM</p>\r\n");
                    html.Append("<p></p>\r\n");
                    html.Append("<p>⭐ Web: ANN.COM.VN</p>\r\n");
                    html.Append("<p></p>\r\n");
                    html.Append("<p>⭐ Zalo đặt hàng: 0936786404 - 0913268406 - 0918567409</p>\r\n");
                    html.Append("<p></p>\r\n");
                    html.Append("<p>⭐ Facebook: https://facebook.com/bosiquanao.net </p>\r\n");
                    html.Append("<p></p>\r\n");
                    html.Append("<p>⭐ Zalo xem Quần Áo Nam: 0977399405 (Zalo này không trả lời tin nhắn)</p>\r\n");
                    html.Append("<p></p>\r\n");
                    html.Append("<p>⭐ Zalo xem Đồ Bộ Nữ: 0975442402 (Zalo này không trả lời tin nhắn)</p>\r\n");
                    html.Append("<p></p>\r\n");
                    html.Append("<p>⭐ Zalo xem Váy Đầm - Áo Nữ - Quần Nữ: 0987409403 (Zalo này không trả lời tin nhắn)</p>\r\n");
                    html.Append("<p></p>\r\n");
                }
            }

            return html.ToString();
        }
        public class ProductVariable
        {
            public string VariableName { get; set; }
            public string VariableValue { get; set; }
        }
        #region Paging
        public void pagingall(List<ProductSQL> acs)
        {
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);

            int PageSize = 30;
            StringBuilder html = new StringBuilder();
            html.Append("<thead>");
            html.Append("<tr>");
            html.Append("    <th class='image-column'>Ảnh</th>");
            html.Append("    <th class='name-column'>Sản phẩm</th>");
            html.Append("    <th class='sku-column'>Mã</th>");
            html.Append("    <th class='wholesale-price-column'>Giá sỉ</th>");
            if (acc.RoleID == 0)
            {
                html.Append("    <th class='cost-price-column'>Giá vốn</th> ");
            }
            html.Append("    <th class='retail-price-column'>Giá lẻ</th>");
            html.Append("    <th class='stock-column'>Kho</th>");
            html.Append("    <th class='stock-status-column'>Trạng thái</th>");
            html.Append("    <th class='category-column'>Danh mục</th>");
            html.Append("    <th class='date-column'>Ngày tạo</th>");
            if (acc.RoleID == 0)
            {
                html.Append("    <th class='show-homepage-column'>Trang chủ</th>");
            }
            html.Append("    <th class='action-column'></th>");
            html.Append("</tr>");
            html.Append("</thead>");

            html.Append("<tbody>");
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

                    html.Append("<td>");
                    html.Append("   <a href=\"/xem-san-pham?id=" + item.ID + "\"><img src=\"" + Thumbnail.getURL(item.ProductImage, Thumbnail.Size.Small) + "\"/></a>");
                    html.Append("   <a href=\"javascript:;\" onclick=\"copyProductInfo(" + item.ID + ")\" class=\"btn download-btn h45-btn\"><i class=\"fa fa-files-o\"></i> Copy</a>");
                    html.Append("</td>");

                    html.Append("   <td class=\"customer-name-link\"><a href=\"/xem-san-pham?id=" + item.ID + "\">" + item.ProductTitle + "</a></td>");
                    html.Append("   <td data-title='Mã' class=\"customer-name-link\">" + item.ProductSKU + "</td>");
                    html.Append("   <td data-title='Giá sỉ'>" + string.Format("{0:N0}", item.RegularPrice) + "</td>");
                    if (acc.RoleID == 0)
                    {
                        html.Append("   <td data-title='Giá vốn'>" + string.Format("{0:N0}", item.CostOfGood) + "</td>");
                    }
                    html.Append("   <td data-title='Giá lẻ'>" + string.Format("{0:N0}", item.RetailPrice) + "</td>");
                    html.Append("   <td data-title='Kho'><a target=\"_blank\" href=\"/thong-ke-san-pham?SKU=" + item.ProductSKU + "\">" + string.Format("{0:N0}", item.TotalProductInstockQuantityLeft) + "</a></td>");
                    html.Append("   <td data-title='Trạng thái'>" + item.ProductInstockStatus + "</td>");
                    html.Append("   <td data-title='Danh mục'>" + item.CategoryName + "</td>");
                    string date = string.Format("{0:dd/MM/yyyy}", item.CreatedDate);
                    html.Append("   <td data-title='Ngày tạo'>" + date + "</td>");

                    if (acc.RoleID == 0 || acc.RoleID == 1)
                    {
                        if (item.ShowHomePage == 0)
                        {
                            html.Append("   <td data-title='Trang chủ'><span id='showHomePage_" + item.ID + "'><a href='javascript:;' data-product-id='" + item.ID + "' data-value='0' class='bg-black bg-button' onclick='changeShowHomePage($(this))'>Đang ẩn</a></span></td>");
                        }
                        else
                        {
                            html.Append("   <td data-title='Trang chủ'><span id='showHomePage_" + item.ID + "'><a href='javascript:;' data-product-id='" + item.ID + "' data-value='1' class='bg-green bg-button' onclick='changeShowHomePage($(this))'>Đang hiện</a></span></td>");
                        }
                    }

                    html.Append("   <td data-title='Thao tác' class='update-button'>");
                    html.Append("       <a href=\"javascript:;\" title=\"Download tất cả hình sản phẩm này\" class=\"btn primary-btn h45-btn\" onclick=\"getAllProductImage('" + item.ProductSKU + "');\"><i class=\"fa fa-file-image-o\" aria-hidden=\"true\"></i></a>");
                    html.Append("       <a target=\"_blank\" href=\"https://www.facebook.com/search/posts/?q=" + item.ProductSKU + "&filters_rp_author=%7B%22name%22%3A%22author%22%2C%22args%22%3A%22100012594165130%22%7D&filters_rp_chrono_sort=%7B%22name%22%3A%22chronosort%22%2C%22args%22%3A%22%22%7D\" title=\"Tìm trên facebook\" class=\"btn primary-btn btn-black h45-btn\"><i class=\"fa fa-facebook-official\" aria-hidden=\"true\"></i></a>");

                    if (acc.RoleID == 0 || acc.RoleID == 1)
                    {
                        html.Append("       <a href=\"javascript:;\" title=\"Đồng bộ sản phẩm\" class=\"up-product-" + item.ID + " btn primary-btn h45-btn " + (item.ShowHomePage == 1 ? "" : "hide") + "\" onclick=\"ShowUpProductToWeb('" + item.ProductSKU + "', '" + item.ID + "', '" + item.CategoryID + "', 'false', 'false', 'null');\"><i class=\"fa fa-upload\" aria-hidden=\"true\"></i></a>");
                    }

                    html.Append("  </td>");
                    html.Append("</tr>");
                }

            }
            else
            {
                html.Append("<tr><td colspan=\"11\">Không tìm thấy sản phẩm...</td></tr>");
            }
            html.Append("</tbody>");

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
            string request = "/tat-ca-san-pham?";

            if (search != "")
            {
                request += "&textsearch=" + search;
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

            if (ddlShowHomePage.SelectedValue != "")
            {
                request += "&showhomepage=" + ddlShowHomePage.SelectedValue;
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