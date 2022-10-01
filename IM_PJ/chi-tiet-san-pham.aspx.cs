using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Utils;
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
    public partial class chi_tiet_san_pham : System.Web.UI.Page
    {
        private const string domain = "kho.xuongann.com";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        public void LoadData()
        {
            int id = Request.QueryString["id"].ToInt(0);
            string sku = Request.QueryString["sku"];

            var p = new tbl_Product();

            if (id > 0)
            {
                p = ProductController.GetByID(id);
            }
            else if (id == 0)
            {
                int variableid = Request.QueryString["variableid"].ToInt(0);
                if (variableid != 0)
                {
                    p = ProductController.GetByVariableID(variableid);
                }
                else
                {
                    if (sku != "")
                    {
                        p = ProductController.GetBySKU(sku);
                        if (p == null)
                        {
                            p = ProductController.GetByVariableSKU(sku);
                        }
                    }
                }
            }

            if (p == null)
            {
                PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy sản phẩm " + id, "e", true, "/san-pham", Page);
            }
            else
            {
                this.Title = String.Format("{0} - Sản phẩm", p.ProductSKU);

                ltrEdit1.Text = "";
                ltrEdit1.Text += "<a href='javascript:;' onclick='copyProductInfo(" + p.ID + ")' class='btn primary-btn margin-right-15px'><i class='fa fa-files-o'></i> Copy</a>";
                ltrEdit1.Text += "<a href='javascript:;' onclick='getAllProductImage(`" + p.ProductSKU + "`)' class='btn primary-btn margin-right-15px'><i class='fa fa-download'></i> Tải hình</a>";
                //ltrEdit1.Text += "<a href='javascript:;' onclick='postProductKiotViet(`" + p.ProductSKU + "`)' class='btn primary-btn margin-right-15px'><i class='fa fa-arrow-up'></i> Đăng Kiot</a>";
                ltrEdit1.Text += "<a href='javascript:;' onclick='postProductZaloShop(`" + p.ProductSKU + "`)' class='btn primary-btn margin-right-15px'><i class='fa fa-arrow-up'></i> Đăng Zalo</a>";
                ltrEdit1.Text += "<a href='javascript:;' onclick='downloadProductZaloShop(`" + p.ProductSKU + "`)' class='btn primary-btn btn-blue margin-right-15px'><i class='fa fa-download'></i> Tải Zalo</a>";
                ltrEdit1.Text += "<a href='javascript:;' onclick='deleteProductZaloShop(`" + p.ProductSKU + "`)' class='btn primary-btn btn-red margin-right-15px'><i class='fa fa-times' aria-hidden='true'></i> Xóa Zalo</a>";
                if (p.ProductType == 2)
                {
                    ltrEdit1.Text += "<a href='#variantList' class='btn primary-btn margin-right-15px'><i class='fa fa-th-list'></i> Thuộc tính</a>";
                }
                ltrProductName.Text = p.ProductSKU + " - " + p.ProductTitle;
                var categoryName = CategoryController.GetByID(p.CategoryID.Value);
                ltrCategory.Text = categoryName.CategoryName;
                ltrMaterials.Text = p.Materials;
                pContent.Text = p.ProductContent;
                ltrSKU.Text = p.ProductSKU;
                ltrRegularPrice.Text = string.Format("{0:N0}", p.Regular_Price);
                ltrRetailPrice.Text = string.Format("{0:N0}", p.Retail_Price);

                // Create order fileter
                var filter = new ProductFilterModel() { search = p.ProductSKU };
                // Create pagination
                var page = new PaginationMetadataModel();
                var a = ProductController.GetAllSql(filter, ref page);
                if (page.totalCount > 0)
                {
                    foreach (var item in a)
                    {
                        ltrStock.Text = item.TotalProductInstockQuantityLeft.ToString();
                        if (item.StockStatus == 1)
                        {
                            ltrStockStatus.Text = "còn hàng";
                        }
                        else if (item.StockStatus == 2)
                        {
                            ltrStockStatus.Text = "hết hàng";
                        }
                        else
                        {
                            ltrStockStatus.Text = "đang nhập hàng";
                        }
                    }
                }
                else
                {
                    ltrStock.Text = "0";
                }

                // thư viện ảnh
                var image = ProductImageController.GetByProductID(id);
                imageGallery.Text = "<ul class='image-gallery'>";
                imageGallery.Text += "<li><a href='" + Thumbnail.getURL(p.ProductImage, Thumbnail.Size.Source) + "' target='_blank'><img src='" + Thumbnail.getURL(p.ProductImage, Thumbnail.Size.Source) + "'></a><a href='" + Thumbnail.getURL(p.ProductImage, Thumbnail.Size.Source) + "' download class='btn download-btn download-image h45-btn'><i class='fa fa-cloud-download'></i> Tải hình</a></li>";
                if (image != null)
                {
                    foreach (var img in image)
                    {
                        if (img.ProductImage != p.ProductImage)
                        {
                            imageGallery.Text += "<li><a href='" + Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Source) + "' target='_blank'><img src='" + Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Source) + "'></a><a href='" + Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Source) + "' download class='btn download-btn download-image h45-btn'><i class='fa fa-cloud-download'></i> Tải hình</a></li>";
                        }
                    }
                }
                imageGallery.Text += "</ul>";


                hdfTable.Value = p.ProductStyle.ToString();
                List<tbl_ProductVariable> b = new List<tbl_ProductVariable>();
                b = ProductVariableController.SearchProductID(p.ID, "");
                if (b != null)
                {
                    pagingall(b);
                }
            }
        }
        #region Paging
        public void pagingall(List<tbl_ProductVariable> acs)
        {
            int PageSize = 40;
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
                html.Append("<tr>");
                html.Append("    <th class='image-column'>Ảnh</th>");
                html.Append("    <th class='variable-column'>Thuộc tính</th>");
                html.Append("    <th class='wholesale-price-column'>Giá sỉ</th>");
                html.Append("    <th class='stock-column'>Kho</th>");
                html.Append("</tr>");
                for (int i = FromRow; i < ToRow + 1; i++)
                {
                    var item = acs[i];
                    html.Append("<tr>");
                    html.Append("   <td><img src='" + Thumbnail.getURL(item.Image, Thumbnail.Size.Small) + "'></td>");
                    html.Append("<td>");
                    html.Append("<strong>" + item.SKU + "</strong><br><br>");
                    var value = ProductVariableValueController.GetByProductVariableID(item.ID);
                    if (value != null)
                    {
                        
                        string list = "";
                        foreach (var temp in value)
                        {
                            html.Append("" + temp.VariableName + ": " + temp.VariableValue + "</br>" + "");
                            list += temp.VariableName + "|";
                        }
                        html.Append("<br>" + PJUtils.StockStatusBySKU(1, item.SKU));
                    }
                    html.Append("</td>");
                    html.Append("   <td>" + string.Format("{0:N0}", item.Regular_Price) + "</td>");
                    html.Append("   <td>" + PJUtils.TotalProductQuantityInstock(1, item.SKU) + "</td>");
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
            int CurrentPage = 1;
            CurrentPage = Convert.ToInt32(Request.QueryString["Page"]);
            if (CurrentPage < 1) CurrentPage = 1;
            string[] strText = new string[4] { "Trang đầu", "Trang trước", "Trang sau", "Trang cuối" };
            if (PageCount > 1)
                Response.Write(GetHtmlPagingAdvanced(4, CurrentPage, PageCount, HttpContext.Current.Request.Url.Query, strText));

        }
        private static string GetPageUrl(int currentPage, string pageUrl)
        {
            pageUrl = Regex.Replace(pageUrl, "(\\?|\\&)*" + "Page=" + currentPage, "");
            if (pageUrl.Contains("?"))
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
            output.Append("<ul>");

            //Link First(Trang đầu) và Previous(Trang trước)
            if (currentPage > 1)
            {
                output.Append("<li><a title=\"" + strText[0] + "\" href=\"" + string.Format(pageUrl, 1) + "\"><i class='fa fa-fast-backward' aria-hidden='true'></i></a></li>");
                output.Append("<li><a title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\"><i class='fa fa-backward' aria-hidden='true'></i></a></li>");
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
                output.Append("<li><a href=\"" + string.Format(pageUrl, startPageNumbersFrom - 1) + "\">&hellip;</a></li>");
            }

            //Duyệt vòng for hiển thị các trang
            for (int i = startPageNumbersFrom; i <= stopPageNumbersAt; i++)
            {
                if (currentPage == i)
                {
                    output.Append("<li class=\"current\">" + i.ToString() + "</li>");
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
                output.Append("<li><a title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\"><i class='fa fa-forward' aria-hidden='true'></i></a></li>");
                output.Append("<li><a title=\"" + strText[3] + "\" href=\"" + string.Format(pageUrl, pageCount) + "\"><i class='fa fa-fast-forward' aria-hidden='true'></i></a></li>");
            }
            output.Append("</ul>");
            return output.ToString();
        }
        #endregion
    }
}