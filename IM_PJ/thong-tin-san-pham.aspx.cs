#region .NET Framework
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

#region Package (third-party)
using MB.Extensions;
using Newtonsoft.Json;
using Telerik.Web.UI;
#endregion

#region ANN Shop
using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Models.Pages.thong_tin_san_pham;
using IM_PJ.Utils;

using NHST.Bussiness;
using WebUI.Business;
#endregion

namespace IM_PJ
{
    public partial class thong_tin_san_pham : System.Web.UI.Page
    {
        private static string _productSKU;
        public static string IMAGE_EXTENSION = ".png";

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
                        if (acc.RoleID == 0 || acc.RoleID == 1 || acc.Username == "nhom_zalo502")
                        {
                            //_loadSupplier();
                            _loadCategory();
                            _loadData();
                        }
                        else
                        {
                            Response.Redirect("/trang-chu");
                        }

                        hdfUserRole.Value = acc.RoleID.ToString();
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
            }
        }

        #region Private
        #region Load Data
        /// <summary>
        /// Khởi tạo List Item có dạng
        /// Quần áo nam
        /// ----Quần áo thun
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="subName"></param>
        /// <returns></returns>
        private IList<ListItem> _getCategoryDropDownList(int parentID, string subName)
        {
            var categories = CategoryController.GetByParentID(parentID);
            List<ListItem> data = new List<ListItem>();

            foreach (var c in categories)
            {
                ListItem item = new ListItem(subName + c.CategoryName, c.ID.ToString());
                data.Add(item);
                data.AddRange(_getCategoryDropDownList(c.ID, subName + "---"));
            }

            return data;
        }

        /// <summary>
        /// Tải danh sách danh mục
        /// </summary>
        private void _loadCategory()
        {
            var category = CategoryController.GetAllWithIsHidden(false);
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new ListItem("Chọn danh mục", "0"));
            if (category.Count > 0)
            {
                var data = _getCategoryDropDownList(0, String.Empty);
                ddlCategory.Items.AddRange(data.ToArray());
                ddlCategory.DataBind();
            }
        }

        /// <summary>
        /// Tải danh sách nhà cung cấp
        /// </summary>
        //private void _loadSupplier()
        //{
        //    var supplier = SupplierController.GetAllWithIsHidden(false);
        //    ddlSupplier.Items.Clear();
        //    ddlSupplier.Items.Insert(0, new ListItem("Chọn nhà cung cấp", "0"));
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

        private void _loadData()
        {
            var productID = 0;

            // Lấy product id từ query parameter
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                productID = Convert.ToInt32(Request.QueryString["id"]);

            // Kiểm tra product id có tồn tại hoặc đúng yêu cầu không
            if (productID <= 0)
            {
                PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy sản phẩm", "e", true, "/tat-ca-san-pham", Page);
                return;
            }

            // Kiểm tra xem có tồn tại sản phẩm với mã ID trong query param không
            var product = ProductController.GetByID(productID);
            if (product == null)
            {
                PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy sản phẩm " + productID, "e", true, "/tat-ca-san-pham", Page);
                return;
            }

            // Cài đặt title page
            this.Title = String.Format("{0} - Sửa sản phẩm", product.ProductSKU.ToTitleCase());
            // Cài đặt dữ liệu cho page
            hdfProductId.Value = productID.ToString();
            ViewState["ID"] = productID;
            ViewState["cateID"] = product.CategoryID;
            _productSKU = product.ProductSKU.ToUpper();
            ViewState["SKU"] = product.ProductSKU;
            this.txtImageCode.Text = String.Format("CODE: {0}", product.ProductSKU);
            hdfParentID.Value = product.CategoryID.ToString();
            hdfsetStyle.Value = product.ProductStyle.ToString();
            ltrBack2.Text = ltrBack.Text;
            // Thông tin product
            _loadProductInfo(product);

            // Thông tin biến thể
            if (product.ProductStyle.HasValue && product.ProductStyle.Value == 2)
                _loadProductVariationInfo(product.ID);

            // Thông tin chú thích
            _loadNote(product);
        }

        /// <summary>
        /// Tải danh sách các tag
        /// </summary>
        /// <param name="productID"></param>
        private void _loadTags(int productID)
        {
            var tags = ProductTagController.get(productID, 0);

            if (tags.Count > 0)
                hdfTags.Value = JsonConvert.SerializeObject(tags);
            else
                hdfTags.Value = String.Empty;
        }

        /// <summary>
        /// Tải tập hình ảnh của sản phẩm
        /// </summary>
        /// <param name="productID"></param>
        private void _loadImageGallery(int productID)
        {
            var images = ProductImageController.GetByProductID(productID);
            imageGallery.Text = "<ul class='image-gallery'>";
            foreach (var img in images)
            {
                imageGallery.Text += "<li><img src='" + Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Source) + "'><a href='javascript:;' data-image-id='" + img.ID + "' onclick='deleteImageGallery($(this))' class='btn-delete'><i class='fa fa-times' aria-hidden='true'></i> Xóa hình</a></li>";
            }
            imageGallery.Text += "</ul>";
        }

        /// <summary>
        /// Khởi tạo HTML danh mục biến thể
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns>HTM DropDownList</returns>
        private string _getSelectVariable(string Name, string Value)
        {
            var variableName = VariableController.GetByName(Name);
            var html = new StringBuilder();
            if (variableName != null)
            {
                html.AppendLine("<select name='ddlVariableValue'");
                html.AppendLine("        class='form-control select2'");
                html.AppendLine("        style='width: 100%'");
                html.AppendLine(String.Format("        data-name-id='{0}'", variableName.ID));
                html.AppendLine(String.Format("        data-name-text='{0}'", variableName.VariableName));
                html.AppendLine("        onchange='onChangeVariationValue($(this))'");
                html.AppendLine(">");
                html.AppendLine("    <option data-sku-text='' value=''>Chọn giá trị</option>");
                var variableValue = VariableValueController.GetByVariableID(variableName.ID);
                foreach (var item in variableValue)
                {
                    var isSelected = item.VariableValue == Value;
                    html.AppendLine(String.Format("    <option data-sku-text='{0}' value='{1}' {2}>{3}</option>", item.SKUText, item.ID, (isSelected ? "selected='selected'" : String.Empty), item.VariableValue));
                }
                html.AppendLine("</select>");
            }
            return html.ToString();
        }

        /// <summary>
        /// Tải thông tin sản phẩm
        /// </summary>
        /// <param name="product"></param>
        private void _loadProductInfo(tbl_Product product)
        {
            // Tên sản phẩm
            txtProductTitle.Text = product.ProductTitle;
            txtCleanName.Text = product.CleanName;
            // Danh mục
            ddlCategory.SelectedValue = product.CategoryID.ToString();
            // Đồng bộ lên KiotViet
            //rdbSyncKiotViet.SelectedValue = product.SyncKiotViet ? "true" : "false";
            // Mã sản phẩm
            txtProductSKU.Text = product.ProductSKU;
            // Chất liệu
            txtMaterials.Text = product.Materials;
            // Màu chủ đạo
            if (!String.IsNullOrEmpty(product.Color))
                ddlColor.SelectedValue = product.Color.Trim();
            // Loại hàng
            ddlPreOrder.SelectedValue = product.PreOrder ? "1" : "0";
            // Giá củ chưa sale
            pOld_Price.Text = product.Old_Price.ToString();
            // Giá sỉ
            pRegular_Price.Text = product.Regular_Price.ToString();
            // Giá vốn
            pCostOfGood.Text = product.CostOfGood.ToString();
            // Giá lẻ
            pRetailPrice.Text = product.Retail_Price.ToString();
            // Giá 10 cái
            pPrice10.Text = product.Price10.ToString();
            // Giá thùng
            pBestPrice.Text = product.BestPrice.ToString();
            // Ảnh đại diện
            if (product.ProductImage != null)
            {
                hdfProductImage.Value = product.ProductImage;
                imgProductImage.ImageUrl = Thumbnail.getURL(product.ProductImage, Thumbnail.Size.Source);
            }
            // Tags
            _loadTags(product.ID);
            // Tóm tắt sản phẩm
            pSummary.Content = product.ShortDescription;
            // Nội dung
            pContent.Content = product.ProductContent;
            // Thư viện ảnh
            _loadImageGallery(product.ID);
            // Ảnh đại diện sạch
            if (product.ProductImageClean != null)
            {
                hdfProductImageClean.Value = product.ProductImageClean;
                imgProductImageClean.ImageUrl = Thumbnail.getURL(product.ProductImageClean, Thumbnail.Size.Source);
            }
            // Ảnh đặc trưng
            if (!String.IsNullOrEmpty(product.FeaturedImage))
            {
                hdfFeaturedImage.Value = product.FeaturedImage;
                featuredImage.ImageUrl = Thumbnail.getURL(product.FeaturedImage, Thumbnail.Size.Source);
            }
        }

        /// <summary>
        /// Tải thông tin của biến thể
        /// </summary>
        /// <param name="productID"></param>
        private void _loadProductVariationInfo(int productID)
        {
            var html = new StringBuilder();
            var variations = ProductVariableController
                .GetProductID(productID)
                .OrderByDescending(o => o.ID);
            var index = variations.Count();

            foreach (var item in variations)
            {
                string VariableSKU = item.SKU;
                double RegularPrice = Convert.ToDouble(item.Regular_Price);
                double RetailPrice = Convert.ToDouble(item.RetailPrice);
                double CostOfGood = Convert.ToDouble(item.CostOfGood);
                int MinimumInventoryLevel = Convert.ToInt32(item.MinimumInventoryLevel);
                int MaximumInventoryLevel = Convert.ToInt32(item.MaximumInventoryLevel);
                string VariableImage = Thumbnail.getURL(item.Image, Thumbnail.Size.Source);
                string deleteVariableImage = "<a href='javascript:;' onclick='deleteImageVariable($(this))' class='btn-delete hide'><i class='fa fa-times' aria-hidden='true'></i> Xóa hình</a>";
                if (!string.IsNullOrEmpty(item.Image))
                {
                    deleteVariableImage = "<a href='javascript:;' onclick='deleteImageVariable($(this))' class='btn-delete'><i class='fa fa-times' aria-hidden='true'></i> Xóa hình</a>";
                }

                var value = ProductVariableValueController.GetByProductVariableID(item.ID);
                string selectVariable = "";
                string variableID = "";
                string variableName = "";
                string variableValueID = "";
                string variableValueName = "";
                string dataVariableValue = "";
                string labelVariableValue = "";
                if (value != null)
                {
                    foreach (var temp in value)
                    {
                        selectVariable += "<div class='row margin-bottom-15'><div class='col-md-5'>" + temp.VariableName + "</div><div class='col-md-7'>" + _getSelectVariable(temp.VariableName, temp.VariableValue) + "</div></div>";
                        variableID += VariableController.GetByName(temp.VariableName.ToString()).ID.ToString() + "|";
                        variableName += temp.VariableName.ToString() + "|";
                        variableValueID += temp.VariableValueID.ToString() + "|";
                        variableValueName += temp.VariableValue.ToString() + "|";
                        dataVariableValue += VariableController.GetByName(temp.VariableName.ToString()).ID.ToString() + ":" + temp.VariableValueID.ToString() + "|";
                        labelVariableValue += temp.VariableName.ToString() + ": " + temp.VariableValue.ToString() + " - ";
                    }
                }

                html.AppendLine(String.Format("<div class='item-var-gen'"));
                html.AppendLine(String.Format("     data-index='{0}'", index));
                html.AppendLine(String.Format("     data-product-variation-id='{0}'", item.ID));
                html.AppendLine(String.Format("     data-name-id='{0}'", variableID));
                html.AppendLine(String.Format("     data-value-id='{0}'", variableValueID));
                html.AppendLine(String.Format("     data-name-text='{0}'", variableName));
                html.AppendLine(String.Format("     data-value-text='{0}'", variableValueName));
                html.AppendLine(String.Format("     data-name-value='{0}'", dataVariableValue));
                html.AppendLine(String.Format(">"));
                html.AppendLine(String.Format("    <div class='col-md-12'>"));
                html.AppendLine(String.Format("        <div class='col-md-10 variable-label'  onclick='showVariableContent($(this))'>"));
                html.AppendLine(String.Format("            <strong>#{0}</strong> - {1} {2}", index, labelVariableValue, VariableSKU));
                html.AppendLine(String.Format("        </div>"));
                html.AppendLine(String.Format("        <div class='col-md-2 variable-removal'>"));
                html.AppendLine(String.Format("            <a href='javascript:;' onclick='deleteVariableItem($(this))' class='btn primary-btn fw-btn not-fullwidth'>"));
                html.AppendLine(String.Format("                <i class='fa fa-times' aria-hidden='true'></i>"));
                html.AppendLine(String.Format("            </a>"));
                html.AppendLine(String.Format("        </div>"));
                html.AppendLine(String.Format("    </div>"));
                html.AppendLine(String.Format("    <div class='col-md-12 variable-content'>"));
                html.AppendLine(String.Format("        <div class='row'>"));
                html.AppendLine(String.Format("            <div class='col-md-2'>"));
                html.AppendLine(String.Format("                <input type='file' class='productVariableImage upload-btn' onchange='onChangeVariationImage(this,$(this));' name='{0}'><img class='imgpreview' onclick='openUploadImage($(this))' data-file-name='{1}' src='{1}'>{2}", VariableSKU, VariableImage, deleteVariableImage));
                html.AppendLine(String.Format("                </div>"));
                html.AppendLine(String.Format("            <div class='col-md-5'>"));
                html.AppendLine(String.Format("                {0}", selectVariable));
                html.AppendLine(String.Format("                <div class='row margin-bottom-15'>"));
                html.AppendLine(String.Format("                    <div class='col-md-5'>Mã sản phẩm</div>"));
                html.AppendLine(String.Format("                    <div class='col-md-7'><input type='text' disabled class='form-control productvariablesku sku-input' value='{0}' disabled='disabled' readonly></div>", VariableSKU));
                html.AppendLine(String.Format("                </div>"));
                html.AppendLine(String.Format("            </div>"));
                html.AppendLine(String.Format("            <div class='col-md-5'>"));
                html.AppendLine(String.Format("                <div class='row margin-bottom-15'>"));
                html.AppendLine(String.Format("                    <div class='col-md-5'>Giá sỉ</div>"));
                html.AppendLine(String.Format("                    <div class='col-md-7'><input class='form-control regularprice' type='number' value='{0}'> </div>", RegularPrice));
                html.AppendLine(String.Format("                </div>"));
                html.AppendLine(String.Format("                <div class='row margin-bottom-15 cost-of-goods'>"));
                html.AppendLine(String.Format("                    <div class='col-md-5'>Giá vốn</div>"));
                html.AppendLine(String.Format("                    <div class='col-md-7'><input class='form-control costofgood cost-price' type='number' value='{0}'></div>", CostOfGood));
                html.AppendLine(String.Format("                </div>"));
                html.AppendLine(String.Format("                <div class='row margin-bottom-15'>"));
                html.AppendLine(String.Format("                    <div class='col-md-5'>Giá bán lẻ</div>"));
                html.AppendLine(String.Format("                    <div class='col-md-7'><input class='form-control retailprice' type='number' value='{0}'></div>", RetailPrice));
                html.AppendLine(String.Format("                </div>"));
                html.AppendLine(String.Format("            </div>"));
                html.AppendLine(String.Format("        </div>"));
                html.AppendLine(String.Format("    </div>"));
                html.AppendLine(String.Format("</div>"));

                index--;
            }

            if (variations.Count() > 0)
                ltrVariables.Text = html.ToString();
        }

        /// <summary>
        /// Tải thông tin chú thích
        /// </summary>
        /// <param name="product"></param>
        private void _loadNote(tbl_Product product)
        {
            string ProductInfo = "<p><strong>Ngày tạo</strong>: " + product.CreatedDate + "</p>";
            ProductInfo += "<p><strong>Người viết</strong>: " + product.CreatedBy + "</p>";
            ProductInfo += "<p><strong>Ngày cập nhật</strong>: " + product.ModifiedDate + "</p>";
            ProductInfo += "<p><strong>Người cập nhật</strong>: " + product.ModifiedBy + "</p>";
            ltrProductInfo.Text = ProductInfo;
        }
        #endregion

        /// <summary>
        /// Vẽ mã lên hình ảnh của sản phẩm
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="fileName">Tên file hình ảnh</param>
        /// <param name="code">Nội dung muốn ghi lên hình</param>
        /// <returns></returns>
        private void _drawCode(string fileName, string code)
        {
            #region Khởi tạo API
            var api = "http://ann-shop-dotnet-core.com/api/v1/image/draw-code";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(api);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    fileName = fileName,
                    code = code
                });

                streamWriter.Write(json);
            }
            #endregion

            // Thực thi API
            httpWebRequest.GetResponse();
        }

        #region Update
        /// <summary>
        /// Upload image
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="uploadedFile"></param>
        /// <returns>File Name</returns>
        private string _uploadImage(int productID, UploadedFile uploadedFile)
        {
            // Upload image
            var folder = Server.MapPath("/uploads/images");
            var fileName = Slug.ConvertToSlug(Path.GetFileName(uploadedFile.FileName), isFile: true, extension: ".png");
            var filePath = String.Format("{0}/{1}-{2}", folder, productID, fileName);

            if (File.Exists(filePath))
            {
                fileName = String.Format("{0}-{1}-{2}", productID, DateTime.UtcNow.ToString("HHmmssffff"), fileName);
                filePath = String.Format("{0}/{1}", folder, fileName);
            }

            uploadedFile.SaveAs(filePath);

            #region Draw Code
            if (!String.IsNullOrEmpty(txtImageCode.Text.Trim()))
            {
                fileName = Path.GetFileName(filePath);
                var extension = Path.GetExtension(filePath);

                _drawCode(fileName, txtImageCode.Text.Trim());

                if (extension != IMAGE_EXTENSION)
                    filePath = filePath.Replace(extension, IMAGE_EXTENSION);
            }
            #endregion

            #region Thumbnail
            Thumbnail.create(filePath, 85, 113);
            Thumbnail.create(filePath, 159, 212);
            Thumbnail.create(filePath, 240, 320);
            Thumbnail.create(filePath, 350, 467);
            Thumbnail.create(filePath, 420, 420);
            Thumbnail.create(filePath, 600, 0);
            #endregion

            return Path.GetFileName(filePath);
        }

        /// <summary>
        /// Upload image
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="httpPostedFile"></param>
        /// <returns>File Name</returns>
        private string _uploadImage(int productID, HttpPostedFile httpPostedFile)
        {
            // Upload image
            var folder = Server.MapPath("/uploads/images");
            var fileName = Slug.ConvertToSlug(Path.GetFileName(httpPostedFile.FileName), isFile: true, extension: ".png");
            var filePath = String.Format("{0}/{1}-{2}", folder, productID, fileName);

            if (File.Exists(filePath))
            {
                fileName = String.Format("{0}-{1}-{2}", productID, DateTime.UtcNow.ToString("HHmmssffff"), fileName);
                filePath = String.Format("{0}/{1}", folder, fileName);
            }

            httpPostedFile.SaveAs(filePath);

            #region Draw Code
            if (!String.IsNullOrEmpty(txtImageCode.Text.Trim()))
            {
                fileName = Path.GetFileName(filePath);
                var extension = Path.GetExtension(filePath);

                _drawCode(fileName, txtImageCode.Text.Trim());

                if (extension != IMAGE_EXTENSION)
                    filePath = filePath.Replace(extension, IMAGE_EXTENSION);
            }
            #endregion

            #region Thumbnail
            Thumbnail.create(filePath, 85, 113);
            Thumbnail.create(filePath, 159, 212);
            Thumbnail.create(filePath, 240, 320);
            Thumbnail.create(filePath, 350, 467);
            Thumbnail.create(filePath, 420, 420);
            Thumbnail.create(filePath, 600, 0);
            #endregion

            return Path.GetFileName(filePath);
        }

        /// <summary>
        /// Cập nhật hình ảnh không có mã SKU
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="uploadedFile"></param>
        /// <returns>File Name</returns>
        private string _uploadImageClean(int productID, UploadedFile uploadedFile)
        {
            // Upload image
            var folder = Server.MapPath("/uploads/images");
            var fileName = Slug.ConvertToSlug(Path.GetFileName(uploadedFile.FileName), isFile: true);
            var filePath = String.Format("{0}/{1}-clean-{2}", folder, productID, fileName);

            if (File.Exists(filePath))
            {
                filePath = String.Format("{0}/{1}-clean-{2}-{3}", folder, productID, DateTime.UtcNow.ToString("HHmmssffff"), fileName);
            }

            uploadedFile.SaveAs(filePath);

            // Thumbnail
            Thumbnail.create(filePath, 85, 113);
            Thumbnail.create(filePath, 159, 212);
            Thumbnail.create(filePath, 240, 320);
            Thumbnail.create(filePath, 350, 467);
            Thumbnail.create(filePath, 420, 420);
            Thumbnail.create(filePath, 600, 0);

            return Path.GetFileName(filePath);
        }

        /// <summary>
        /// Cập nhật hình ảnh đặc trưng
        /// </summary>
        /// <param name="produuctId"></param>
        /// <param name="uploadedFile"></param>
        /// <returns>File Name</returns>
        private string _uploadFeaturedImageClean(int produuctId, UploadedFile uploadedFile)
        {
            // Upload image
            var folder = Server.MapPath("/uploads/images");
            var fileName = Slug.ConvertToSlug(Path.GetFileName(uploadedFile.FileName), isFile: true);
            var filePath = String.Format("{0}/featured-{1}-{2}", folder, produuctId, fileName);

            if (File.Exists(filePath))
            {
                var strNow = DateTime.Now.ToString("HHmmssffff");
                var extension = Path.GetExtension(fileName);
                var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

                filePath = String.Format("{0}/featured-{1}-{2}-{3}{4}", folder, produuctId, nameWithoutExtension, strNow, extension);
            }

            uploadedFile.SaveAs(filePath);

            return Path.GetFileName(filePath);
        }

        #region Cập nhật Prodcut
        /// <summary>
        /// Cập nhật thông tin product
        /// </summary>
        private void _updateProduct(tbl_Account acc)
        {
            int productID = ViewState["ID"].ToString().ToInt(0);
            var updatedData = new tbl_Product()
            {
                ID = productID,
                ProductOldID = 0,
                ProductStock = 0,
                StockStatus = 0,
                ManageStock = true,
                ProductType = 0,
                IsHidden = false,
                ModifiedDate = DateTime.Now,
                ModifiedBy = acc.Username,
            };

            // Tên sản phẩm
            updatedData.ProductTitle = Regex.Replace(txtProductTitle.Text, @"\s*\,\s*|\s*\;\s*", " - ");
            updatedData.CleanName = Regex.Replace(txtCleanName.Text, @"\s*\,\s*|\s*\;\s*", " - ");
            // Danh mục
            updatedData.CategoryID = Convert.ToInt32(hdfParentID.Value);
            // Đồng bộ lên KiotViet
            //updatedData.SyncKiotViet = rdbSyncKiotViet.SelectedValue == "true";
            // Mã sản phẩm
            updatedData.ProductSKU = ViewState["SKU"].ToString();
            // Chất liệu
            updatedData.Materials = txtMaterials.Text;
            // Màu chủ đạo
            updatedData.Color = ddlColor.SelectedValue.ToString();
            // Loại hàng
            updatedData.PreOrder = ddlPreOrder.SelectedValue == "1" ? true : false;
            // Tồn kho ít nhât
            updatedData.MinimumInventoryLevel = 5;
            // Tồn kho nhiều nhất
            updatedData.MaximumInventoryLevel = 20;
            // Nhà cung cấp
            updatedData.SupplierID = 0;
            updatedData.SupplierName = "";
            // Giá củ chưa sale
            updatedData.Old_Price = String.IsNullOrEmpty(pOld_Price.Text) ? 0 : Convert.ToDouble(pOld_Price.Text);
            // Giá sỉ
            updatedData.Regular_Price = Convert.ToDouble(pRegular_Price.Text);
            // Giá vốn
            updatedData.CostOfGood = Convert.ToDouble(pCostOfGood.Text);
            // Giá lẻ
            updatedData.Retail_Price = Convert.ToDouble(pRetailPrice.Text);
            // Giá 10 cái
            updatedData.Price10 = String.IsNullOrEmpty(pPrice10.Text) ? 0 : Convert.ToDouble(pPrice10.Text);
            // Giá thùng
            updatedData.BestPrice = String.IsNullOrEmpty(pBestPrice.Text) ? 0 : Convert.ToDouble(pBestPrice.Text);
            // Ảnh đại diện
            if (uploadProductImage.UploadedFiles.Count > 0)
            {
                var imageFile = uploadProductImage.UploadedFiles[0];
                updatedData.ProductImage = _uploadImage(productID, imageFile);
            }
            else
            {
                updatedData.ProductImage = hdfProductImage.Value;
            }
            // Cập nhật video
            if (!String.IsNullOrEmpty(hdfNewVideoId.Value))
            {
                var productId = Convert.ToInt32(hdfProductId.Value);
                var linkDownload = String.IsNullOrEmpty(txtLinkDownload.Text) ? null : txtLinkDownload.Text.Trim();
                var isActive = rdbActiveVideo.SelectedValue == "true";

                _updateProductVideo(hdfOldVideoId.Value, hdfNewVideoId.Value, linkDownload, productId, isActive);
            }
            else if (!String.IsNullOrEmpty(hdfOldVideoId.Value))
            {
                var productId = Convert.ToInt32(hdfProductId.Value);

                _deleteProductVideo(hdfOldVideoId.Value, productId);
            }
            // Cập nhật tóm tắt sản phẩm
            updatedData.ShortDescription = pSummary.Content;
            // Nội dung
            updatedData.ProductContent = pContent.Content;
            // Ảnh sạch
            if (uploadProductImageClean.UploadedFiles.Count > 0)
            {
                var imageCleanFile = uploadProductImageClean.UploadedFiles[0];
                updatedData.ProductImageClean = _uploadImageClean(productID, imageCleanFile);
            }
            else
            {
                updatedData.ProductImageClean = hdfProductImageClean.Value;
            }
            // Ảnh đặc trưng
            if (rauFeaturedImage.UploadedFiles.Count > 0)
            {
                var file = rauFeaturedImage.UploadedFiles[0];
                updatedData.FeaturedImage = _uploadFeaturedImageClean(productID, file);
            }
            else
                updatedData.FeaturedImage = hdfFeaturedImage.Value;

            // Update product
            ProductController.Update(updatedData);

            // Cập nhật thư viện hình ảnh sản phẩm
            _updateImageGallery(productID, acc);

            // Cập nhật tags
            if (!String.IsNullOrEmpty(hdfTags.Value))
                _updateTag(updatedData, acc);
        }

        /// <summary>
        /// Cập nhật thư viện hình ảnh sản phẩm
        /// </summary>
        /// <param name="productID"></param>
        private void _updateImageGallery(int productID, tbl_Account acc)
        {
            // Delete Image
            if (!String.IsNullOrEmpty(hdfDeleteImageGallery.Value))
            {
                var imageIDList = hdfDeleteImageGallery.Value
                    .Split(',')
                    .Where(x => !String.IsNullOrEmpty(x))
                    .ToList();

                foreach (var strID in imageIDList)
                {
                    ProductImageController.Delete(Convert.ToInt32(strID));
                }
            }
            // Add Image
            if (uploadImageGallery.HasFiles)
            {
                foreach (HttpPostedFile httpPostedFile in uploadImageGallery.PostedFiles)
                {
                    var fileName = _uploadImage(productID, httpPostedFile);
                    ProductImageController.Insert(productID, fileName, false, DateTime.Now, acc.Username);
                }
            }
        }

        /// <summary>
        /// Cập nhật table Tags
        /// </summary>
        /// <param name="product"></param>
        /// <param name="acc"></param>
        private void _updateTag(tbl_Product product, tbl_Account acc)
        {
            var tagList = JsonConvert.DeserializeObject<List<TagModel>>(hdfTags.Value);

            if (tagList.Count > 0)
            {
                // Get tag new
                var tagNew = TagController.insert(tagList, acc);

                var productTag = tagList
                    .GroupJoin(
                        tagNew,
                        t => t.name.ToLower(),
                        n => n.Name.ToLower(),
                        (t, n) => new { t, n }
                    )
                    .SelectMany(
                        x => x.n.DefaultIfEmpty(),
                        (parent, child) => new ProductTag
                        {
                            TagID = child != null ? child.ID : parent.t.id,
                            ProductID = product.ID,
                            ProductVariableID = 0,
                            SKU = product.ProductSKU,
                            CreatedBy = acc.ID,
                            CreatedDate = DateTime.Now
                        }
                    )
                    .ToList();

                ProductTagController.update(product.ID, productTag);
            }
            else
            {
                ProductTagController.delete(product.ID);
            }
        }
        #endregion

        #region Cập nhật sản phẩm biến thể
        private void _updateProductVariation(tbl_Account acc)
        {
            if (String.IsNullOrEmpty(hdfVariableListInsert.Value))
            {
                var strFunction = "";
                strFunction = "HoldOn.close();";
                strFunction = "swal({title: 'Lỗi', text: 'Quá trình cập nhật biến thể đã xảy ra lỗi', type: 'error'});";

                JavaScript.BeforePageLoad(Page).ExecuteCustomScript(strFunction);
                return;

            }

            var productID = ViewState["ID"].ToString().ToInt(0);
            var productSKU = ViewState["SKU"].ToString();
            var uploadPath = "/uploads/images/";
            var imageDefault = "/App_Themes/Ann/image/placeholder.png";

            #region Xóa biến thể
            var variationRemovalList = JsonConvert.DeserializeObject<List<string>>(hdfVariationRemovalList.Value);

            foreach (var sku in variationRemovalList)
            {
                ProductVariableController.removeByVariationSKU(sku);
            }
            #endregion

            #region Cập nhật biến thể
            var productVariationUpdateList = JsonConvert.DeserializeObject<List<ProductVariationUpdateModel>>(hdfVariableListInsert.Value);

            if (uploadVariationImage.HasFiles)
            {
                foreach (var file in uploadVariationImage.PostedFiles)
                {

                    var fileName = file.FileName;
                    var imageName = _uploadImage(productID, file);

                    productVariationUpdateList = productVariationUpdateList
                        .Select(x =>
                        {
                            if (x.image == uploadPath + fileName)
                                x.image = uploadPath + imageName;

                            return x;
                        })
                        .ToList();
                }
            }

            foreach (var item in productVariationUpdateList)
            {
                var now = DateTime.Now;
                // Kiểm tra sản phẩm biến thể có tồn tại hay không để thực hiện insert hoặc update
                var productVariation = ProductVariableController.GetByID(item.productVariationID);
                var image = String.Empty;

                if (item.image != imageDefault)
                {
                    image = item.image.Replace(uploadPath, String.Empty);
                }

                if (productVariation != null)
                {
                    // Thực hiện update biến thể
                    var productVariationID = ProductVariableController.Update(
                        ID: productVariation.ID,
                        ProductID: productID,
                        ParentSKU: productSKU,
                        SKU: item.sku,
                        Stock: Convert.ToDouble(productVariation.Stock),
                        StockStatus: Convert.ToInt32(productVariation.StockStatus),
                        Regular_Price: item.regularPrice,
                        CostOfGood: item.costOfGood,
                        RetailPrice: item.retailPrice,
                        Image: image,
                        ManageStock: true,
                        IsHidden: false,
                        ModifiedDate: now,
                        ModifiedBy: acc.Username,
                        SupplierID: Convert.ToInt32(productVariation.SupplierID),
                        SupplierName: productVariation.SupplierName,
                        MinimumInventoryLevel: item.minimumInventoryLevel,
                        MaximumInventoryLevel: item.maximumInventoryLevel
                    );

                    // Xóa tất cả giá trị của biến thể để cập nhật lại
                    ProductVariableValueController.DeleteByProductVariableID(productVariation.ID);

                    // Khởi tạo biến thể cho sản phẩm con
                    _createVariationValue(new VariationValueUpdateModel()
                    {
                        productVariationID = Convert.ToInt32(productVariationID),
                        productVariationSKU = item.sku,
                        variationValueID = item.variationValueID,
                        variationName = item.variationName,
                        variationValueName = item.variationValueName,
                        isHidden = false,
                        createdDate = now,
                        createdBy = acc.Username
                    });
                }
                else
                {
                    // Tạo sản phẩm biến thể mới
                    var productVariationID = ProductVariableController.Insert(
                        ProductID: productID,
                        ParentSKU: productSKU,
                        SKU: item.sku,
                        Stock: 0,
                        StockStatus: item.stockStatus,
                        Regular_Price: item.regularPrice,
                        CostOfGood: item.costOfGood,
                        RetailPrice: item.retailPrice,
                        Image: image,
                        ManageStock: true,
                        IsHidden: false,
                        CreatedDate: now,
                        CreatedBy: acc.Username,
                        SupplierID: 0,
                        SupplierName: "",
                        MinimumInventoryLevel: item.minimumInventoryLevel,
                        MaximumInventoryLevel: item.maximumInventoryLevel);

                    // Khởi tạo biến thể cho sản phẩm con
                    _createVariationValue(new VariationValueUpdateModel()
                    {
                        productVariationID = Convert.ToInt32(productVariationID),
                        productVariationSKU = item.sku,
                        variationValueID = item.variationValueID,
                        variationName = item.variationName,
                        variationValueName = item.variationValueName,
                        isHidden = false,
                        createdDate = now,
                        createdBy = acc.Username
                    });
                }
            }
            #endregion
        }
        #endregion

        #region Cập nhật giá trị biến thể của sản phẩm con
        public void _createVariationValue(VariationValueUpdateModel variationValue)
        {
            var variationValueIDList = variationValue.variationValueID.Split('|').Where(x => !String.IsNullOrEmpty(x)).ToList();
            var variationNameList = variationValue.variationName.Split('|').Where(x => !String.IsNullOrEmpty(x)).ToList();
            var variationValueNameList = variationValue.variationValueName.Split('|').Where(x => !String.IsNullOrEmpty(x)).ToList();

            for (int index = 0; index < variationValueIDList.Count; index++)
            {
                var variableValueID = variationValueIDList[index].ToInt();
                string variableName = variationNameList[index];
                string variableValueName = variationValueNameList[index];
                ProductVariableValueController.Insert(
                    ProductVariableID: variationValue.productVariationID,
                    ProductvariableSKU: variationValue.productVariationSKU,
                    VariableValueID: variableValueID,
                    VariableName: variableName,
                    VariableValue: variableValueName,
                    IsHidden: variationValue.isHidden,
                    CreatedDate: variationValue.createdDate,
                    CreatedBy: variationValue.createdBy
                );
            }
        }
        #endregion

        #region Cập nhật thông tin video
        /// <summary>
        /// Cập nhật thông tin video sản phẩm
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="oldVideoId"></param>
        /// <param name="newVideoId"></param>
        /// <param name="linkDownload"></param>
        /// <param name="productId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        private void _updateProductVideo(string oldVideoId, string newVideoId, string linkDownload, int productId, bool isActive)
        {
            #region Khởi tạo API
            var api = "http://ann-shop-dotnet-core.com/api/v1/product-video/update";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(api);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    oldVideoId = oldVideoId,
                    newVideoId = newVideoId,
                    linkDownload = linkDownload,
                    productId = productId,
                    isActive = isActive
                });

                streamWriter.Write(json);
            }
            #endregion

            // Thực thi API
            httpWebRequest.GetResponse();
        }
        #endregion
        #endregion

        #region Delete
        #region Cập nhật thông tin video
        /// <summary>
        /// Xóa thông tin video sản phẩm
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="videoId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        private void _deleteProductVideo(string videoId, int productId)
        {
            #region Khởi tạo API
            var api = "http://ann-shop-dotnet-core.com/api/v1/product-video/delete";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(api);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    videoId = videoId,
                    productId = productId
                });

                streamWriter.Write(json);
            }
            #endregion

            // Thực thi API
            httpWebRequest.GetResponse();
        }
        #endregion
        #endregion
        #endregion

        protected void txtProductSKU_Changed(object sender, EventArgs e)
        {
            var productSKU = txtProductSKU.Text.Trim().ToUpper();

            if (!String.IsNullOrEmpty(productSKU))
            {
                if (productSKU != _productSKU)
                {
                    var product = ProductController.GetBySKU(txtProductSKU.Text.Trim().ToLower());

                    if (product != null)
                    {
                        txtProductSKU.Text = ViewState["SKU"].ToString();
                        PJUtils.ShowMessageBoxSwAlert(String.Format("Mã #SKU - {0} đã tồn tại", productSKU), "e", false, Page);
                        return;
                    }
                }

                var stocks = StockManagerController.warehousing1ByParentSKU(_productSKU);

                if (stocks.Count > 0)
                {
                    txtProductSKU.Text = ViewState["SKU"].ToString();
                    PJUtils.ShowMessageBoxSwAlert(String.Format("Sản phẩm #{0} đã tạo mã vạch nhập kho rồi nên không thể sửa", ViewState["SKU"]), "i", false, Page);
                    return;
                }

                var skuOld = ViewState["SKU"].ToString().Trim().ToUpper();
                var skuNew = productSKU;

                ViewState["SKU"] = productSKU;
                JavaScript.AfterPageLoad(Page).ExecuteCustomScript("updateVariationSKUA('{0}', '{1}');", new object[] { skuOld, skuNew });
            }
            else
            {
                txtProductSKU.Text = ViewState["SKU"].ToString();
                PJUtils.ShowMessageBoxSwAlert("Bạn đã quên nhập mã sản phẩm. Tôi đã lấy mã sản phẩm củ cho bạn", "i", false, Page);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

            if (acc == null)
            {
                var message = String.Format("Không tìm thấy account #{0} trong hệ thống.", username);
                PJUtils.ShowMessageBoxSwAlert(message, "e", false, Page);
                return;
            }

            // Cập nhật product
            _updateProduct(acc);

            // Update Variable
            if (hdfsetStyle.Value == "2")
                _updateProductVariation(acc);

            PJUtils.ShowMessageBoxSwAlert("Cập nhật sản phẩm thành công", "s", true, Page);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true, XmlSerializeString = false)]
        public static List<TagModel> GetTags(string tagName)
        {
            if (!String.IsNullOrEmpty(tagName) && tagName.IndexOf(',') >= 0)
            {
                return null;
            }

            var now = DateTime.Now;
            var textInfo = new CultureInfo("vi-VN", false).TextInfo;
            var tags = new List<TagModel>();
            var tagData = TagController.get(tagName);

            if (tagData.Where(x => x.name == textInfo.ToLower(tagName)).Count() > 0)
            {
                tags.AddRange(tagData);
            }
            else
            {
                tags.Add(new TagModel()
                {
                    id = 0,
                    name = textInfo.ToLower(tagName),
                    slug = String.Format("tag-new-{0:yyyyMMddhhmmss}", now)
                });
                tags.AddRange(tagData);
            }

            return tags;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true, XmlSerializeString = false)]
        public static List<TagModel> GetTagListByNameList(string[] tagNameList)
        {
            if (tagNameList == null || tagNameList.Length == 0)
                return null;

            return TagController.get(tagNameList.ToList());
        }
    }
}