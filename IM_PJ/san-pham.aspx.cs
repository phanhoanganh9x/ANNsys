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
using System.Drawing;
using System.Web.Hosting;
using System.IO;

namespace IM_PJ
{
    public partial class san_pham : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                LoadCategory();
                LoadTag();
            }
        }
        public void LoadTag()
        {
            var tag = TagController.getAll();
            ddlTag.Items.Clear();
            ddlTag.Items.Insert(0, new ListItem("Tag", "0"));
            if (tag.Count > 0)
            {
                foreach (var t in tag)
                {
                    ListItem item = new ListItem(t.Name, t.ID.ToString());
                    ddlTag.Items.Add(item);
                }
                ddlTag.DataBind();
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
                    listitem.Attributes.Add("data-slug", c.Slug);
                    ddlCategory.Items.Add(listitem);

                    addItemCategory(c.ID, h + "---");
                }
            }
        }
        public void LoadData()
        {
            DateTime year = new DateTime(2018, 6, 22);
            DateTime DateConfig = year;

            DateTime fromDate = DateConfig;
            DateTime toDate = DateTime.Now;

            if (!String.IsNullOrEmpty(Request.QueryString["fromdate"]))
            {
                fromDate = Convert.ToDateTime(Request.QueryString["fromdate"]);
            }

            if (!String.IsNullOrEmpty(Request.QueryString["todate"]))
            {
                toDate = Convert.ToDateTime(Request.QueryString["todate"]).AddDays(1).AddMinutes(-1);
            }

            rFromDate.SelectedDate = fromDate;
            rFromDate.MinDate = DateConfig;
            rFromDate.MaxDate = DateTime.Now;

            rToDate.SelectedDate = toDate;
            rToDate.MinDate = DateConfig;
            rToDate.MaxDate = DateTime.Now;

            string TextSearch = String.Empty;
            int CategoryID = 0;
            int StockStatus = 0;
            string strColor = String.Empty;
            string strSize = String.Empty;
            int Page = 1;
            // add filter quantity
            string Quantity = String.Empty;
            int QuantityFrom = 0;
            int QuantityTo = 0;
                
            string orderBy = String.Empty;
            int tag = 0;

            // KiotViet
            string retailerName = String.Empty;

            if (Request.QueryString["textsearch"] != null)
                TextSearch = Request.QueryString["textsearch"].Trim();
            if (Request.QueryString["stockstatus"] != null)
                StockStatus = Request.QueryString["stockstatus"].ToInt();
            if (Request.QueryString["categoryid"] != null)
                CategoryID = Request.QueryString["categoryid"].ToInt();
            // Add filter valiable value
            if (Request.QueryString["color"] != null)
                strColor = Request.QueryString["color"].Trim();
            if (Request.QueryString["size"] != null)
                strSize = Request.QueryString["size"].Trim();
            if (Request.QueryString["Page"] != null)
            {
                Page = Request.QueryString["Page"].ToInt();
                if (Page < 1)
                {
                    Page = 1;
                }
            }
            // add filter quantity
            if (Request.QueryString["quantityfilter"] != null)
            {
                Quantity = Request.QueryString["quantityfilter"];

                if (Quantity == "greaterthan")
                {
                    QuantityFrom = Request.QueryString["quantity"].ToInt();
                }
                else if (Quantity == "lessthan")
                {
                    QuantityTo = Request.QueryString["quantity"].ToInt();
                }
                else if (Quantity == "between")
                {
                    QuantityFrom = Request.QueryString["quantitymin"].ToInt();
                    QuantityTo = Request.QueryString["quantitymax"].ToInt();
                }
            }
            if (Request.QueryString["orderby"] != null)
            {
                orderBy = Request.QueryString["orderby"];
            }
            if (Request.QueryString["tag"] != null)
            {
                tag = Request.QueryString["tag"].ToInt();
            }

            // KiotViet
            if (!String.IsNullOrEmpty(Request.QueryString["retailerName"]))
                retailerName = Request.QueryString["retailerName"];

            txtSearchProduct.Text = TextSearch;
            ddlCategory.SelectedValue = CategoryID.ToString();
            ddlStockStatus.SelectedValue = StockStatus.ToString();
            // add filter quantity
            ddlQuantityFilter.SelectedValue = Quantity.ToString();
            if (Quantity == "greaterthan")
            {
                txtQuantity.Text = QuantityFrom.ToString();
                txtQuantityMin.Text = "0";
                txtQuantityMax.Text = "0";
            }
            else if (Quantity == "lessthan")
            {
                txtQuantity.Text = QuantityTo.ToString();
                txtQuantityMin.Text = "0";
                txtQuantityMax.Text = "0";
            }
            else if (Quantity == "between")
            {
                txtQuantity.Text = "0";
                txtQuantityMin.Text = QuantityFrom.ToString();
                txtQuantityMax.Text = QuantityTo.ToString();
            }

            // Add filter valiable value
            ddlColor.SelectedValue = strColor;
            ddlSize.SelectedValue = strSize;

            ddlTag.SelectedValue = tag.ToString();
            ddlOrderBy.SelectedValue = orderBy;

            // KiotViet
            if (!String.IsNullOrEmpty(retailerName))
                ddlRetailer.SelectedValue = retailerName;

            // Create order filter
            var filter = new ProductFilterModel()
            {
                category = CategoryID,
                search = TextSearch,
                color = strColor,
                size = strSize,
                stockStatus = StockStatus,
                quantity = Quantity,
                quantityFrom = QuantityFrom,
                quantityTo = QuantityTo,
                fromDate = fromDate,
                toDate = toDate,
                showHomePage = "",
                webPublish = "",
                preOrder = "",
                tag = tag,
                orderBy = orderBy
            };
            // Create pagination
            var page = new PaginationMetadataModel()
            {
                currentPage = Page,
                pageSize = 20
            };

            List<ProductSQL> a = new List<ProductSQL>();
            a = ProductController.GetAllSql(filter, ref page);

            pagingall(a, page);

            ltrNumberOfProduct.Text = page.totalCount.ToString();
        }
        [WebMethod]
        public static string getAllProductImage1MB(string sku)
        {
            List<string> result = new List<string>();

            string rootPath = HostingEnvironment.ApplicationPhysicalPath;
            string uploadsImagesPath = rootPath + "/uploads/images/";
            var product = ProductController.GetBySKU(sku);

            if (product != null)
            {
                List<string> images = new List<string>();

                // lấy ảnh đại diện
                string imgAdd = product.ProductImage;
                if (File.Exists(uploadsImagesPath + imgAdd))
                {
                    images.Add(imgAdd);
                }

                // lấy ảnh sản phẩm từ thư viện
                var imageProduct = ProductImageController.GetByProductID(product.ID);
                if (imageProduct != null)
                {
                    foreach (var img in imageProduct)
                    {
                        imgAdd = img.ProductImage;
                        if (File.Exists(uploadsImagesPath + imgAdd))
                        {
                            images.Add(imgAdd);
                        }
                    }
                }

                // lấy ảnh sản phẩm từ biến thể
                var variable = ProductVariableController.GetByParentSKU(product.ProductSKU);
                if (variable != null)
                {
                    foreach (var v in variable)
                    {
                        if (!String.IsNullOrEmpty(v.Image))
                        {
                            imgAdd = v.Image;
                            if (File.Exists(uploadsImagesPath + imgAdd))
                            {
                                images.Add(imgAdd);
                            }
                        }
                    }
                }

                // lọc hình trùng lặp
                images = images.Distinct().ToList();

                if (images.Count() > 0)
                {
                    for (int i = 0; i < images.Count - 1; i++)
                    {
                        for (int y = i + 1; y < images.Count; y++)
                        {
                            string img1 = Thumbnail.getURL(images[i], Thumbnail.Size.Micro);
                            string img2 = Thumbnail.getURL(images[y], Thumbnail.Size.Micro);

                            // so sánh 2 hình và lọc hình trùng lặp
                            Bitmap bmp1 = (Bitmap)Bitmap.FromFile(rootPath + img1);
                            Bitmap bmp2 = (Bitmap)Bitmap.FromFile(rootPath + img2);
                            if (CompareBitmapsLazy(bmp1, bmp2))
                            {
                                images.RemoveAt(y);
                                y--;
                            }
                        }
                    }
                }

                // lấy hình dưới 1MB
                for (int i = 0; i < images.Count; i++)
                {
                    string item = uploadsImagesPath + images[i];
                    var size = new System.IO.FileInfo(item).Length;
                    if (size > 1000000)
                    {
                        // kiểm tra size hình rộng 600px
                        item = uploadsImagesPath + "600/" + images[i];
                        if (File.Exists(item))
                        {
                            size = new System.IO.FileInfo(item).Length;
                            if (size > 1000000)
                            {
                                result.Add(Thumbnail.getURL(images[i], Thumbnail.Size.Large));
                            }
                            else
                            {
                                result.Add(Thumbnail.getURL(images[i], Thumbnail.Size.XLarge));
                            }
                        }
                        else
                        {
                            result.Add(Thumbnail.getURL(images[i], Thumbnail.Size.Large));
                        }
                    }
                    else
                    {
                        result.Add(Thumbnail.getURL(images[i], Thumbnail.Size.Source));
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(result.ToList());
        }
        public static bool CompareBitmapsLazy(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1 == null || bmp2 == null)
                return false;
            if (object.Equals(bmp1, bmp2))
                return true;
            if (!bmp1.Size.Equals(bmp2.Size) || !bmp1.PixelFormat.Equals(bmp2.PixelFormat))
                return false;

            //Compare bitmaps using GetPixel method
            for (int column = 0; column < bmp1.Width; column++)
            {
                for (int row = 0; row < bmp1.Height; row++)
                {
                    if (!bmp1.GetPixel(column, row).Equals(bmp2.GetPixel(column, row)))
                        return false;
                }
            }

            return true;
        }

        [WebMethod]
        public static string copyProductInfo(int id)
        {
            var product = ProductController.GetByID(id);
            StringBuilder html = new StringBuilder();
            if (product != null)
            {
                html.AppendLine("<p>" + product.ProductSKU + " - " + product.ProductTitle + "</p>\r\n");
                html.AppendLine("<p>📌 Sỉ: " + (product.Regular_Price / 1000).ToString() + "k</p>\r\n");
                html.AppendLine("<p>📌 Lẻ: " + (product.Retail_Price / 1000).ToString() + "k</p>\r\n");

                if (!string.IsNullOrEmpty(product.Materials))
                {
                    html.AppendLine("<p>🔖 " + (product.CategoryID == 44 ? "" : "Chất liệu: ")  + product.Materials + "</p>\r\n");
                }

                if (!string.IsNullOrEmpty(product.ProductContent))
                {
                    string content = Regex.Replace(product.ProductContent, @"<img\s[^>]*>(?:\s*?</img>)?", "").ToString();
                    html.AppendLine("<p>🔖 " + content + "</p>\r\n");
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
                    var vari = variableTemp
                                .GroupBy(x => new { x.VariableName, x.VariableValue })
                                .Select(x => new { VariableName = x.Key.VariableName, VariableValue = x.Key.VariableValue })
                                .OrderBy(x => x.VariableName)
                                .ToList();

                    string stringVariable = vari[0].VariableName;

                    Variable = "<p><strong>📚 " + vari[0].VariableName + "</strong>: ";

                    int count = 0;

                    for (int y = 0; y < vari.Count; y++)
                    {
                        if (stringVariable == vari[y].VariableName)
                        {
                            if (vari[y].VariableValue.IndexOf("Mẫu") >= 0)
                            {
                                count = count + 1;

                                if (y == (vari.Count - 1))
                                {
                                    Variable += count.ToString() + " mẫu khác nhau";
                                }
                            }
                            else
                            {
                                Variable += vari[y].VariableValue + "; ";
                            }
                        }
                        else
                        {
                            if (count > 0)
                            {
                                Variable += count.ToString() + " mẫu khác nhau";
                            }
                            Variable += "</p>\r\n";
                            Variable += "<p><strong>📐 " + vari[y].VariableName + "</strong>: " + vari[y].VariableValue + "; ";
                            stringVariable = vari[y].VariableName;
                        }
                    }
                    
                    html.AppendLine(Variable);
                }

                html.AppendLine("<p></p>\r\n");
            }

            return html.ToString();
        }
        public class ProductVariable
        {
            public string VariableName { get; set; }
            public string VariableValue { get; set; }
        }
        #region Paging
        public void pagingall(List<ProductSQL> acs, PaginationMetadataModel page)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<thead>");
            html.AppendLine("<tr>");
            html.AppendLine("    <th class='checkbox-column'><input id='checkAll' type='checkbox' onchange='changeCheckAll($(this).prop(`checked`))'/></th>");
            html.AppendLine("    <th class='image-column'>Ảnh</th>");
            html.AppendLine("    <th class='name-column'>Sản phẩm</th>");
            html.AppendLine("    <th class='sku-column'>Mã</th>");
            html.AppendLine("    <th class='wholesale-price-column'>Giá sỉ</th>");
            html.AppendLine("    <th class='retail-price-column'>Giá lẻ</th>");
            html.AppendLine("    <th class='stock-column'>SL</th>");
            html.AppendLine("    <th class='stock-status-column'>Kho</th>");
            html.AppendLine("    <th class='category-column'>Danh mục</th>");
            html.AppendLine("    <th class='date-column'>Ngày tạo</th>");
            html.AppendLine("    <th class='action-column'></th>");
            html.AppendLine("</tr>");
            html.AppendLine("</thead>");

            html.AppendLine("<tbody>");
            if (acs.Count > 0)
            {
                PageCount = page.totalPages;
                Int32 Page = page.currentPage;

                foreach (var item in acs)
                {
                    html.AppendLine(String.Format("<tr class='tr-product' data-productid='{0}' data-productsku='{1}'>", item.ID, item.ProductSKU));
                    html.AppendLine("   <td><input type='checkbox' onchange='changeCheck($(this))' /></td>");
                    html.AppendLine("<td>");
                    html.AppendLine("   <a target='_blank' href='/chi-tiet-san-pham?id=" + item.ID + "'><img src='" + Thumbnail.getURL(item.ProductImage, Thumbnail.Size.Large) + "'></a>");
                    html.AppendLine("</td>");
                    html.AppendLine("   <td>");
                    html.AppendLine("       <a target='_blank' class='customer-name-link' href='/chi-tiet-san-pham?id=" + item.ID + "'>" + item.ProductTitle + "</a>");
                    html.AppendLine("       <p class='p-paterials'><strong>Chất liệu:</strong> " + item.Materials + "<p>");

                    if (!String.IsNullOrEmpty(item.Tags))
                    {
                        var tagList = item.Tags.Split(',').Select(x => x.Trim()).ToList();

                        foreach (var tag in tagList)
                        {
                            if (tag != "hot")
                            {
                                html.AppendLine(String.Format("       <span class='tag-blue'>{0}</span>", tag.ToLower()));
                            }
                        }
                    }
                    html.AppendLine("   </td>");

                    html.AppendLine("   <td data-title='Mã' class='customer-name-link'>" + item.ProductSKU + "</td>");
                    html.AppendLine("   <td data-title='Giá sỉ'>" + string.Format("{0:N0}", item.RegularPrice) + (item.OldPrice > 0 ? " <span class='old-price'>" + string.Format("{0:N0}", item.OldPrice) + "</span>" : "") + "</td>");
                    html.AppendLine("   <td data-title='Giá lẻ'>" + string.Format("{0:N0}", item.RetailPrice) + "</td>");
                    var strStock2Quantity = String.Empty;
                    if (item.HasStock2)
                        strStock2Quantity = String.Format("<br>(Kho 2: {0:N0})", item.Stock2Quantity);
                    html.AppendLine(String.Format("   <td data-title='Số lượng'>{1:N0}{2}</td>", item.ProductSKU, item.TotalProductInstockQuantityLeft, strStock2Quantity));
                    html.AppendLine("   <td data-title='Kho'>" + item.ProductInstockStatus + "</td>");
                    html.AppendLine("   <td data-title='Danh mục'>" + item.CategoryName + "</td>");
                    string date = string.Format("<strong>{0:dd/MM/yyyy}</strong>", item.CreatedDate);
                    html.AppendLine("   <td data-title='Ngày tạo'>" + date + "</td>");
                    html.AppendLine("   <td class='update-button'>");
                    html.AppendLine("       <a href='javascript:;' title='Copy thông tin sản phẩm này' class='btn primary-btn' onclick='copyProductInfo(" + item.ID + ")'><i class='fa fa-files-o' aria-hidden='true'></i> Copy</a>");
                    html.AppendLine("       <a href='javascript:;' title='Tải tất cả hình sản phẩm này' class='btn primary-btn' onclick='getAllProductImage(`" + item.ProductSKU + "`)'><i class='fa fa-download' aria-hidden='true'></i> Hình</a>");
                    html.AppendLine("       <a href='javascript:;' title='Đăng sản phẩm lên KiotViet' class='btn primary-btn btn-sync-kv-product hidden' onclick='syncKvProduct(`" + item.ProductSKU + "`)'><i class='fa fa-arrow-up' aria-hidden='true'></i> Kiot</a>");
                    html.AppendLine("       <a href='javascript:;' title='Xóa sản phẩm trên KiotViet' class='btn primary-btn btn-red btn-delete-kv-product hidden' onclick='deleteKvProduct(`" + item.ID + "`)'><i class='fa fa-times' aria-hidden='true'></i> Kiot</a>");
                    html.AppendLine("       <a href='javascript:;' title='Theo dõi sản phẩm' class='btn primary-btn btn-kv-product-observation hidden' onclick='registerKvProductObservation(`" + item.ProductSKU + "`)'><i class='fa fa-eye' aria-hidden='true'></i> Kiot</a>");
                    html.AppendLine("       <a href='javascript:;' title='Bỏ theo dõi sản phẩm' class='btn primary-btn btn-kv-product-unobserver hidden' onclick='deleteKvProductObservation(`" + item.ProductSKU + "`)'><i class='fa fa-eye-slash' aria-hidden='true'></i> Kiot</a>");
                    html.AppendLine("       <a href='javascript:;' title='Đăng sản phẩm lên Zalo Shop' class='btn primary-btn' onclick='postProductZaloShop(`" + item.ProductSKU + "`)'><i class='fa fa-arrow-up' aria-hidden='true'></i> Zalo</a>");
                    html.AppendLine("       <a href='javascript:;' title='Tải file sản phẩm Zalo Shop' class='btn primary-btn btn-blue' onclick='downloadProductZaloShop(`" + item.ProductSKU + "`)'><i class='fa fa-download' aria-hidden='true'></i> Zalo</a>");
                    html.AppendLine("       <a href='javascript:;' title='Xóa sản phẩm trên Zalo Shop' class='btn primary-btn btn-red' onclick='deleteProductZaloShop(`" + item.ProductSKU + "`)'><i class='fa fa-times' aria-hidden='true'></i> Zalo</a>");
                    html.AppendLine("  </td>");
                    html.AppendLine("</tr>");
                }

            }
            else
            {
                html.AppendLine("<tr><td colspan='11'>Không tìm thấy sản phẩm...</td></tr>");
            }
            html.AppendLine("</tbody>");

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
            if (pageUrl.Contains('?'))
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