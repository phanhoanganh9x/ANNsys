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
    public partial class xem_san_pham : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                    var acc = AccountController.GetByUsername(username);
                    if (acc != null)
                    {
                        LoadCategory();
                        //LoadSupplier();
                        LoadData();
                    }
                }
                else
                {

                    Response.Redirect("/dang-nhap");
                }
            }
        }
        //public void LoadSupplier()
        //{
        //    var supplier = SupplierController.GetAllWithIsHidden(false);
        //    ddlSupplier.Items.Clear();
        //    ddlSupplier.Items.Insert(0, new ListItem("Chưa chọn nhà cung cấp", "0"));
        //    if (supplier.Count > 0)
        //    {
        //        foreach (var p in supplier)
        //        {
        //            ListItem listitem = new ListItem(p.SupplierName, p.ID.ToString());
        //            ddlSupplier.Items.Add(listitem);
        //        }
        //        ddlSupplier.DataBind();
        //    }
        //}
        public void LoadCategory()
        {
            var category = CategoryController.GetAllWithIsHidden(false);
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new ListItem("Chọn danh mục sản phẩm", "0"));
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
            string username = HttpContext.Current.Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

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
                if(variableid != 0)
                {
                    p = ProductController.GetByVariableID(variableid);
                }
                else
                {
                    if(sku != "")
                    {
                        p = ProductController.GetBySKU(sku);
                        if(p == null)
                        {
                            p = ProductController.GetByVariableSKU(sku);
                        }
                    }
                }
            }

            if (p == null)
            {
                PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy sản phẩm " + id, "e", true, "/tat-ca-san-pham", Page);
            }
            else
            {
                this.Title = String.Format("{0} - Sản phẩm", p.ProductSKU.ToTitleCase());

                hdfProductId.Value = id.ToString();
                ViewState["ID"] = id;
                ViewState["cateID"] = p.CategoryID;
                ViewState["SKU"] = p.ProductSKU;

                ltrEdit1.Text = "";
                if (acc.RoleID == 0 || acc.RoleID == 1 || acc.Username == "baiviet")
                {
                    ltrEdit1.Text += "<a href='/thong-tin-san-pham?id=" + p.ID + "' class='btn primary-btn fw-btn not-fullwidth'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Chỉnh sửa</a>";
                    ltrEdit1.Text += "<a href='/tao-san-pham' class='btn primary-btn fw-btn not-fullwidth print-invoice-merged'><i class='fa fa-file-text-o' aria-hidden='true'></i> Thêm mới</a>";
                    if (p.ShowHomePage == 1)
                    {
                        ltrEdit1.Text += "<a href='javascript:;' onclick='showProductSyncModal(`" + p.ProductSKU + "`, " + p.ID + ", " + p.CategoryID + ", `" + p.ProductTitle + "`);' class='btn primary-btn not-fullwidth print-invoice-merged'><i class='fa fa-upload' aria-hidden='true'></i> Đồng bộ</a>";
                    }
                }
                ltrEdit1.Text += "<a href='javascript:;' onclick='copyProductInfo(" + p.ID + ")' class='btn primary-btn not-fullwidth print-invoice-merged'><i class='fa fa-files-o'></i> Copy thông tin</a>";
                ltrEdit1.Text += "<a href='javascript:;' onclick='getAllProductImage(`" + p.ProductSKU + "`);' class='btn primary-btn not-fullwidth print-invoice-merged'><i class='fa fa-cloud-download'></i> Tải tất cả hình ảnh</a>";

                if (acc.RoleID == 0 || acc.Username == "baiviet")
                {
                    ltrEdit1.Text += "<a href='javascript:;' onclick='deleteProduct(" + p.ID + ");' class='btn primary-btn not-fullwidth print-invoice-merged btn-red'><i class='fa fa-times'></i> Xóa sản phẩm</a>";
                    ltrEdit1.Text += "<a href='javascript:;' data-toggle='modal' data-target='#modalUpdateProductSKU' data-backdrop='static' data-keyboard='false' class='btn primary-btn not-fullwidth print-invoice-merged btn-blue'><i class='fa fa-times'></i> Sửa mã sản phẩm</a>";
                }

                ltrEdit2.Text = ltrEdit1.Text;
                lbProductTitle.Text = p.ProductTitle;
                lbCleanName.Text = p.CleanName;

                string categoryName = "";
                var category = CategoryController.GetByID(p.CategoryID.Value);
                if (category != null)
                {
                    categoryName = " " + category.CategoryName.ToUpper();
                }

                pSummary.Text = p.ShortDescription;


                pContent.Text = p.ProductContent;


                lblSKU.Text = p.ProductSKU;
                txtOldSKU.Text = p.ProductSKU;
                lbSlug.Text = p.Slug;

                // Create order fileter
                var filter = new ProductFilterModel() { search = p.ProductSKU };
                // Create pagination
                var page = new PaginationMetadataModel();
                var a = ProductController.GetAllSql(filter, ref page);
                if (page.totalCount > 0)
                {
                    foreach (var item in a)
                    {
                        lbProductStock.Text = item.TotalProductInstockQuantityLeft.ToString();
                        ddlStockStatus.SelectedValue = item.StockStatus.ToString();
                    }
                }
                else
                {
                    lbProductStock.Text = "0";
                }

                lbOldPrice.Text = string.Format("{0:N0}", p.Old_Price);
                lbRegularPrice.Text = string.Format("{0:N0}", p.Regular_Price);
                lbPrice10.Text = string.Format("{0:N0}", p.Price10);
                lbBestPrice.Text = string.Format("{0:N0}", p.BestPrice);
                /// 2023-10-31: BinhTT
                /// Thêm giá chót
                lbLastPrice.Text = string.Format("{0:N0}", p.LastPrice);
                lbCostOfGood.Text = "";
                if (acc.RoleID == 0)
                {
                    lbCostOfGood.Text =  string.Format("{0:N0}", p.CostOfGood);
                }

                lbRetailPrice.Text = string.Format("{0:N0}", p.Retail_Price);
                ddlCategory.SelectedValue = p.CategoryID.ToString();
                lbMaterials.Text = p.Materials;

                // Hàng order
                ddlPreOrder.SelectedValue = p.PreOrder ? "1" : "0";

                // Ảnh đại diện
                thumbnailImage.Text += "<ul class='image-gallery'><li><img src='" + Thumbnail.getURL(p.ProductImage, Thumbnail.Size.Normal) + "'><a href='" + Thumbnail.getURL(p.ProductImage, Thumbnail.Size.Source) + "' download class='btn download-btn download-image h45-btn'><i class='fa fa-cloud-download'></i> Tải hình này</a></li></ul>";

                // Ảnh đại diện sạch
                thumbnailImageClean.Text += "<ul class='image-gallery'><li><img src='" + Thumbnail.getURL(p.ProductImageClean, Thumbnail.Size.Normal) + "'><a href='" + Thumbnail.getURL(p.ProductImageClean, Thumbnail.Size.Source) + "' download class='btn download-btn download-image h45-btn'><i class='fa fa-cloud-download'></i> Tải hình này</a></li></ul>";

                // thư viện ảnh
                var image = ProductImageController.GetByProductID(id);
                imageGallery.Text = "<ul class='image-gallery'>";
                imageGallery.Text += "<li><img src='" + Thumbnail.getURL(p.ProductImage, Thumbnail.Size.Normal) + "'><a href='" + Thumbnail.getURL(p.ProductImage, Thumbnail.Size.Source) + "' download class='btn download-btn download-image h45-btn'><i class='fa fa-cloud-download'></i> Tải hình này</a></li>";
                if (image != null)
                {
                    foreach (var img in image)
                    {
                        if (img.ProductImage != p.ProductImage)
                        {
                            imageGallery.Text += "<li><img src='" + Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Normal) + "'><a href='" + Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Source) + "' download class='btn download-btn download-image h45-btn'><i class='fa fa-cloud-download'></i> Tải hình này</a></li>";
                        }
                    }
                }
                imageGallery.Text += "</ul>";

                // Ảnh đặc trưng
                ltrFeaturedImage.Text = "";
                ltrFeaturedImage.Text += "<ul class='image-gallery'>";
                ltrFeaturedImage.Text += "    <li>";
                ltrFeaturedImage.Text += "        <img src='" + Thumbnail.getURL(p.FeaturedImage, Thumbnail.Size.Source) + "'>";
                ltrFeaturedImage.Text += "        <a href='" + Thumbnail.getURL(p.FeaturedImage, Thumbnail.Size.Source) + "' download class='btn download-btn download-image h45-btn'>";
                ltrFeaturedImage.Text += "            <i class='fa fa-cloud-download'></i> Tải hình này";
                ltrFeaturedImage.Text += "        </a>";
                ltrFeaturedImage.Text += "    </li>";
                ltrFeaturedImage.Text += "</ul>";

                hdfTable.Value = p.ProductStyle.ToString();

                // Init Tag
                var tagIDList = ProductTagController.get(p.ID, 0)
                    .Select(x => x.name)
                    .ToList();
                hdfTags.Value = String.Join(",", tagIDList);

                List<tbl_ProductVariable> b = new List<tbl_ProductVariable>();

                b = ProductVariableController.SearchProductID(p.ID, "");
                if (b != null)
                {
                    pagingall(b, Convert.ToInt32(acc.RoleID));
                }
            }
        }
        #region Paging
        public void pagingall(List<tbl_ProductVariable> acs, int userRole)
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
                html.Append("    <th class='sku-column'>Mã</th>");
                html.Append("    <th class='wholesale-price-column'>Giá sỉ</th>");
                if(userRole == 0)
                {
                    html.Append("    <th class='cost-price-column'>Giá vốn</th>");
                }
                html.Append("    <th class='retail-price-column'>Giá lẻ</th>");
                html.Append("    <th class='stock-column'>Kho</th>");
                html.Append("    <th class='stock-status-column'>Trạng thái</th>");
                html.Append("    <th class='date-column'>Ngày tạo</th>");
                html.Append("    <th class='hide-column'>Ẩn</th>");
                html.Append("    <th class='action-column'>Thao tác</th>");
                html.Append("</tr>");
                for (int i = FromRow; i < ToRow + 1; i++)
                {
                    var item = acs[i];
                    var stock2 = StockManagerController.getQuantityStock2BySKU(item.SKU);
                    html.Append("<tr>");
                    html.Append("   <td><img src='" + Thumbnail.getURL(item.Image, Thumbnail.Size.Small) + "'></td>");

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
                    }

                    html.Append("   <td>" + item.SKU + "</td>");

                    html.Append("   <td>" + string.Format("{0:N0}", item.Regular_Price) + "</td>");

                    if (userRole == 0)
                    {
                        html.Append("   <td>" + string.Format("{0:N0}", item.CostOfGood) + "</td>");
                    }

                    html.Append("   <td>" + string.Format("{0:N0}", item.RetailPrice) + "</td>");

                    if (stock2.HasValue)
                        html.Append(String.Format("   <td>{0:N0}<br>(Kho 2: {1:N0})</td>", PJUtils.TotalProductQuantityInstock(1, item.SKU), stock2.Value));
                    else
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
    }
}