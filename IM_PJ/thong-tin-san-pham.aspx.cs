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
using Telerik.Web.UI;
using System.IO;

namespace IM_PJ
{
    public partial class thong_tin_san_pham : System.Web.UI.Page
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
                        if (acc.RoleID == 0)
                        {

                        }
                        else if (acc.RoleID == 1)
                        {

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
                LoadSupplier();
                LoadCategory();
                LoadData();
            }
        }

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
        public void LoadSupplier()
        {
            var supplier = SupplierController.GetAllWithIsHidden(false);
            ddlSupplier.Items.Clear();
            ddlSupplier.Items.Insert(0, new ListItem("Chọn nhà cung cấp", "0"));
            if (supplier.Count > 0)
            {
                foreach (var p in supplier)
                {
                    ListItem listitem = new ListItem(p.SupplierName, p.ID.ToString());
                    ddlSupplier.Items.Add(listitem);
                }
                ddlSupplier.DataBind();
            }
        }
        public static string getSelectVariable(string Name, string Value)
        {
            var variablename = VariableController.GetByName(Name);
            string html = "";
            if (variablename != null)
            {
                html += "<select name='ddlVariableValue' id='ddlVariableValue' data-name-id='" + variablename.ID + "' data-name-text='" + variablename.VariableName + "' class='form-control' onchange='changeVariable($(this))'>";
                html += "<option data-sku-text='' value=''>Chọn giá trị</option>";
                var variablevalue = VariableValueController.GetByVariableID(variablename.ID);
                foreach (var p in variablevalue)
                {
                    if(p.VariableValue == Value)
                    {
                        html += "<option data-sku-text='" + p.SKUText + "' selected='selected' value='" + p.ID + "'>" + p.VariableValue + "</option>";
                    }
                    else
                    {
                        html += "<option data-sku-text='" + p.SKUText + "' value='" + p.ID + "'>" + p.VariableValue + "</option>";
                    }
                }
                html += "</select>";
            }
            return html;
        }
        public void LoadData()
        {
            int id = Request.QueryString["id"].ToInt(0);
            if (id > 0)
            {
                var p = ProductController.GetByID(id);
                if (p == null)
                {
                    PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy sản phẩm " + id, "e", true, "/tat-ca-san-pham", Page);
                }
                else
                {
                    this.Title = String.Format("{0} - Sửa sản phẩm", p.ProductSKU.ToTitleCase());

                    ViewState["ID"] = id;
                    ViewState["cateID"] = p.CategoryID;
                    ViewState["SKU"] = p.ProductSKU;
                    hdfParentID.Value = p.CategoryID.ToString();
                    hdfsetStyle.Value = p.ProductStyle.ToString();
                    ltrBack.Text = "<a href=\"/xem-san-pham?id=" + p.ID + "\" class=\"btn primary-btn fw-btn not-fullwidth\">Trở về</a>";
                    txtProductTitle.Text = p.ProductTitle;
                    pContent.Content = p.ProductContent;
                    txtProductSKU.Text = p.ProductSKU;

                    pRegular_Price.Text = p.Regular_Price.ToString();
                    pCostOfGood.Text = p.CostOfGood.ToString();
                    pRetailPrice.Text = p.Retail_Price.ToString();
                    chkIsHidden.Checked = Convert.ToBoolean(p.IsHidden);
                    ddlSupplier.SelectedValue = p.SupplierID.ToString();
                    ddlCategory.SelectedValue = p.CategoryID.ToString();
                    txtMaterials.Text = p.Materials;
                    pMinimumInventoryLevel.Text = p.MinimumInventoryLevel.ToString();
                    pMaximumInventoryLevel.Text = p.MaximumInventoryLevel.ToString();
                    if(p.ProductImage != null)
                    {
                        ListProductThumbnail.Value = p.ProductImage;
                        ProductThumbnail.ImageUrl = p.ProductImage;
                    }

                    var image = ProductImageController.GetByProductID(id);
                    imageGallery.Text = "<ul class=\"image-gallery\">";
                    if (image != null)
                    {
                        foreach (var img in image)
                        {
                            imageGallery.Text += "<li><img src='" + img.ProductImage + "' /><a href='javascript:;' data-image-id='" + img.ID + "' onclick='deleteImageGallery($(this))' class='btn-delete'><i class=\"fa fa-times\" aria-hidden=\"true\"></i> Xóa hình</a></li>";
                        }
                    }
                    imageGallery.Text += "</ul>";

                    // Lấy tất cả biến thể ra

                    List<tbl_ProductVariable> a = new List<tbl_ProductVariable>();
                    a = ProductVariableController.GetProductID(p.ID);

                    if(a.Count > 0)
                    {
                        StringBuilder html = new StringBuilder();
                        int t = 1;
                        foreach (var item in a)
                        {
                            string VariableSKU = item.SKU;
                            double RegularPrice = Convert.ToDouble(item.Regular_Price);
                            double RetailPrice = Convert.ToDouble(item.RetailPrice);
                            double CostOfGood = Convert.ToDouble(item.CostOfGood);
                            int MinimumInventoryLevel = Convert.ToInt32(item.MinimumInventoryLevel);
                            int MaximumInventoryLevel = Convert.ToInt32(item.MaximumInventoryLevel);
                            string VariableImage = "/App_Themes/Ann/image/placeholder.png";
                            string deleteVariableImage = "<a href='javascript:;' onclick='deleteImageVariable($(this))' class='btn-delete hide'><i class=\"fa fa-times\" aria-hidden=\"true\"></i> Xóa hình</a>";
                            if (!string.IsNullOrEmpty(item.Image))
                            {
                                VariableImage = item.Image;
                                deleteVariableImage = "<a href='javascript:;' onclick='deleteImageVariable($(this))' class='btn-delete'><i class=\"fa fa-times\" aria-hidden=\"true\"></i> Xóa hình</a>";
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
                                    selectVariable += "<div class='row margin-bottom-15'><div class='col-md-5'>" + temp.VariableName + "</div><div class='col-md-7'>" + getSelectVariable(temp.VariableName, temp.VariableValue) + "</div></div>";
                                    variableID += VariableController.GetByName(temp.VariableName.ToString()).ID.ToString() + "|";
                                    variableName += temp.VariableName.ToString() + "|";
                                    variableValueID += temp.VariableValueID.ToString() + "|";
                                    variableValueName += temp.VariableValue.ToString() + "|";
                                    dataVariableValue += VariableController.GetByName(temp.VariableName.ToString()).ID.ToString() + ":" + temp.VariableValueID.ToString() + "|";
                                    labelVariableValue += temp.VariableName.ToString() + ": " + temp.VariableValue.ToString() + " - ";
                                }
                            }

                            html.AppendLine(String.Format("<div class='item-var-gen' data-name-id='{0}' data-value-id='{1}' data-name-text='{2}' data-value-text='{3}' data-name-value='{4}'>", variableID, variableValueID, variableName, variableValueName, dataVariableValue));
                            html.AppendLine(String.Format("    <div class='col-md-12 variable-label' onclick='showVariableContent($(this))'>"));
                            html.AppendLine(String.Format("    	<strong>#{0}</strong> - {1} {2}", t, labelVariableValue, VariableSKU));
                            html.AppendLine(String.Format("    </div>"));
                            html.AppendLine(String.Format("    <div class='col-md-12 variable-content'>"));
                            html.AppendLine(String.Format("    	<div class='row'>"));
                            html.AppendLine(String.Format("		    <div class='col-md-2'>"));
                            html.AppendLine(String.Format("		    	<input type='file' class='productVariableImage upload-btn' onchange='imagepreview(this,$(this));' name='{0}'><img class='imgpreview' onclick='openUploadImage($(this))' data-file-name='{1}' src='{1}'>{2}", dataVariableValue, VariableImage, deleteVariableImage));
                            html.AppendLine(String.Format("		    	</div>"));
                            html.AppendLine(String.Format("		    <div class='col-md-5'>"));
                            html.AppendLine(String.Format("		    	{0}", selectVariable));
                            html.AppendLine(String.Format("		    	<div class='row margin-bottom-15'>"));
                            html.AppendLine(String.Format("		    	    <div class='col-md-5'>Mã sản phẩm</div>"));
                            html.AppendLine(String.Format("		    	    <div class='col-md-7'><input type='text' disabled class='form-control productvariablesku sku-input' value='{0}'></div>", VariableSKU));
                            html.AppendLine(String.Format("		    	</div>"));
                            html.AppendLine(String.Format("		    </div>"));
                            html.AppendLine(String.Format("		    <div class='col-md-5'>"));
                            html.AppendLine(String.Format("		    	<div class='row margin-bottom-15'>"));
                            html.AppendLine(String.Format("		    	    <div class='col-md-5'>Giá sỉ</div>"));
                            html.AppendLine(String.Format("		    	    <div class='col-md-7'><input class='form-control regularprice' type='text' value='{0}'> </div>", RegularPrice));
                            html.AppendLine(String.Format("		    	</div>"));
                            html.AppendLine(String.Format("		    	<div class='row margin-bottom-15 cost-of-goods'>"));
                            html.AppendLine(String.Format("		    	    <div class='col-md-5'>Giá vốn</div>"));
                            html.AppendLine(String.Format("		    	    <div class='col-md-7'><input class='form-control costofgood cost-price' type='text' value='{0}'></div>", CostOfGood));
                            html.AppendLine(String.Format("		    	</div>"));
                            html.AppendLine(String.Format("		    	<div class='row margin-bottom-15'>"));
                            html.AppendLine(String.Format("		    	    <div class='col-md-5'>Giá bán lẻ</div>"));
                            html.AppendLine(String.Format("		    	    <div class='col-md-7'><input class='form-control retailprice' type='text' value='{0}'></div>", RetailPrice));
                            html.AppendLine(String.Format("		    	</div>"));
                            html.AppendLine(String.Format("		    	<div class='row margin-bottom-15'>"));
                            html.AppendLine(String.Format("		    	    <div class='col-md-5'>Tồn kho ít nhất</div>"));
                            html.AppendLine(String.Format("		    	    <div class='col-md-7'><input class='form-control minimum' type='text' value='{0}'></div>", MinimumInventoryLevel));
                            html.AppendLine(String.Format("		    	</div>"));
                            html.AppendLine(String.Format("		    	<div class='row margin-bottom-15'>"));
                            html.AppendLine(String.Format("		    	    <div class='col-md-5'>Tồn kho nhiều nhất</div>"));
                            html.AppendLine(String.Format("		    	    <div class='col-md-7'><input class='form-control maximum' type='text' value='{0}'></div>", MaximumInventoryLevel));
                            html.AppendLine(String.Format("		    	</div>"));
                            html.AppendLine(String.Format("			</div>"));
                            html.AppendLine(String.Format("		</div>"));
                            html.AppendLine(String.Format("	</div>"));
                            html.AppendLine(String.Format("</div>"));

                            t++;
                        }

                        ltrVariables.Text = html.ToString();
                    }

                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            int cateID = ViewState["cateID"].ToString().ToInt(0);
            int ProductID = ViewState["ID"].ToString().ToInt(0);
            if (cateID > 0)
            {
                string ProductTitle = Regex.Replace(txtProductTitle.Text, @"\s*\,\s*|\s*\;\s*", " - ");
                string ProductContent = pContent.Content;
                string ProductSKU = ViewState["SKU"].ToString();
                double ProductStock = 0;
                int StockStatus = 0;
                bool ManageStock = true;
                double Regular_Price = Convert.ToDouble(pRegular_Price.Text);
                double CostOfGood = Convert.ToDouble(pCostOfGood.Text);
                double Retail_Price = Convert.ToDouble(pRetailPrice.Text);
                bool IsHidden = chkIsHidden.Checked;
                int CategoryID = hdfParentID.Value.ToInt();

                double MinimumInventoryLevel = 0;
                if (pMinimumInventoryLevel.Text != "")
                {
                    MinimumInventoryLevel = Convert.ToDouble(pMinimumInventoryLevel.Text);
                }

                double MaximumInventoryLevel = 0;
                if (pMaximumInventoryLevel.Text != "")
                {
                    MaximumInventoryLevel = Convert.ToDouble(pMaximumInventoryLevel.Text);
                }

                //Phần thêm ảnh đại diện sản phẩm
                string path = "/uploads/images/";
                string ProductImage = ListProductThumbnail.Value;
                if (ProductThumbnailImage.UploadedFiles.Count > 0)
                {
                    foreach (UploadedFile f in ProductThumbnailImage.UploadedFiles)
                    {
                        var o = path + ProductID + '-' + Slug.ConvertToSlug(Path.GetFileName(f.FileName));
                        try
                        {
                            f.SaveAs(Server.MapPath(o));
                            ProductImage = o;
                        }
                        catch { }
                    }
                }

                if (ProductImage != ListProductThumbnail.Value)
                {
                    if (File.Exists(Server.MapPath(ListProductThumbnail.Value)))
                    {
                        File.Delete(Server.MapPath(ListProductThumbnail.Value));
                    }
                }

                // Delete Image Gallery

                string deleteImageGallery = hdfDeleteImageGallery.Value;

                if(deleteImageGallery != "")
                {
                    string[] deletelist = deleteImageGallery.Split(',');

                    for(int i = 0; i < deletelist.Length - 1; i++)
                    {
                        var img = ProductImageController.GetByID(Convert.ToInt32(deletelist[i]));
                        if(img != null)
                        {
                            var product = ProductController.GetByID(ProductID);

                            // Delete image
                            if (!string.IsNullOrEmpty(img.ProductImage) && img.ProductImage != product.ProductImage)
                            {
                                string fileImage = Server.MapPath(img.ProductImage);
                                File.Delete(fileImage);
                            }
                            string delete = ProductImageController.Delete(img.ID);
                        }
                    }
                }

                // Update product

                string kq = ProductController.Update(ProductID, CategoryID, 0, ProductTitle, ProductContent, ProductSKU, ProductStock,
                    StockStatus, ManageStock, Regular_Price, CostOfGood, Retail_Price, ProductImage, 0,
                    IsHidden, DateTime.Now, username, ddlSupplier.SelectedValue.ToInt(0), ddlSupplier.SelectedItem.ToString(),
                    txtMaterials.Text, MinimumInventoryLevel, MaximumInventoryLevel);

                // Upload image gallery

                if (UploadImages.HasFiles)
                {
                    foreach (HttpPostedFile uploadedFile in UploadImages.PostedFiles)
                    {
                        var o = path + ProductID + '-' + Slug.ConvertToSlug(Path.GetFileName(uploadedFile.FileName));
                        uploadedFile.SaveAs(Server.MapPath(o));
                        ProductImageController.Insert(ProductID, o, false, DateTime.Now, username);
                    }
                }



                if (kq.ToInt(0) > 0)
                {
                    // Update Variable
                    if (hdfsetStyle.Value == "2")
                    {
                        string variable = hdfVariableListInsert.Value;
                        if (!string.IsNullOrEmpty(variable))
                        {
                            string[] items = variable.Split(',');
                            for (int i = 0; i < items.Length - 1; i++)
                            {
                                string item = items[i];
                                string[] itemElement = item.Split(';');

                                string datanameid = itemElement[0];
                                string[] datavalueid = itemElement[1].Split('|');
                                string datanametext = itemElement[2];
                                string datavaluetext = itemElement[3];
                                string productvariablesku = itemElement[4].Trim().ToUpper();
                                string regularprice = itemElement[5];
                                string costofgood = itemElement[6];
                                string retailprice = itemElement[7];
                                string[] datanamevalue = itemElement[8].Split('|');
                                string imageUpload = itemElement[8];
                                int _MaximumInventoryLevel = itemElement[9].ToInt(0);
                                int _MinimumInventoryLevel = itemElement[10].ToInt(0);
                                int stockstatus = itemElement[11].ToInt();
                                string imageSrc = itemElement[13];
                                string kq1 = "";

                                // Check variable
                                var Variable = ProductVariableController.GetBySKU(productvariablesku);
                                if (Variable != null)
                                {

                                    // Update image

                                    string image = Variable.Image;
                                    if (imageSrc == "/App_Themes/Ann/image/placeholder.png")
                                    {
                                        // Delete old image
                                        if (!string.IsNullOrEmpty(Variable.Image))
                                        {
                                            string fileImage = Server.MapPath(Variable.Image);
                                            File.Delete(fileImage);
                                            image = "";
                                        }
                                    }
                                    else
                                    {
                                        if (imageSrc != Variable.Image)
                                        {
                                            HttpPostedFile postedFile = Request.Files["" + imageUpload + ""];
                                            if (postedFile != null && postedFile.ContentLength > 0)
                                            {
                                                // Upload image
                                                var o = path + ProductID + '-' + Slug.ConvertToSlug(Path.GetFileName(postedFile.FileName));
                                                postedFile.SaveAs(Server.MapPath(o));
                                                image = o;

                                                // Delete old image
                                                if (!string.IsNullOrEmpty(Variable.Image))
                                                {
                                                    string fileImage = Server.MapPath(Variable.Image);
                                                    File.Delete(fileImage);
                                                }
                                            }
                                            else
                                            {
                                                // Delete old image
                                                if (!string.IsNullOrEmpty(Variable.Image))
                                                {
                                                    string fileImage = Server.MapPath(Variable.Image);
                                                    File.Delete(fileImage);
                                                    image = "";
                                                }
                                            }
                                        }
                                    }

                                    // Update variable

                                    kq1 = ProductVariableController.Update(Variable.ID, ProductID, Variable.ParentSKU, productvariablesku, Convert.ToDouble(Variable.Stock), Convert.ToInt32(Variable.StockStatus), Convert.ToDouble(regularprice), Convert.ToDouble(costofgood), Convert.ToDouble(retailprice), image, true, false, DateTime.Now, username, Convert.ToInt32(Variable.SupplierID), Variable.SupplierName, _MinimumInventoryLevel, _MaximumInventoryLevel);

                                    // Delete all productVariableValue

                                    bool deleteVariableValue = ProductVariableValueController.DeleteByProductVariableID(Variable.ID);
                                }
                                else
                                {
                                    string image = "";

                                    HttpPostedFile postedFile = Request.Files["" + imageUpload + ""];
                                    if (postedFile != null && postedFile.ContentLength > 0)
                                    {
                                        // Upload image
                                        var o = path + ProductID + '-' + Slug.ConvertToSlug(Path.GetFileName(postedFile.FileName));
                                        postedFile.SaveAs(Server.MapPath(o));
                                        image = o;
                                    }

                                    // Insert new variable

                                    kq1 = ProductVariableController.Insert(ProductID, ProductSKU, productvariablesku, 0, stockstatus, Convert.ToDouble(regularprice),
                                            Convert.ToDouble(costofgood), Convert.ToDouble(retailprice), image, true, false, DateTime.Now, username,
                                            ddlSupplier.SelectedValue.ToInt(0), ddlSupplier.SelectedItem.ToString(), _MinimumInventoryLevel, _MaximumInventoryLevel);
                                }
                                
                                // Update ProductVariableValue

                                if (kq1.ToInt(0) > 0)
                                {
                                    string[] Data = datanametext.Split('|');
                                    string[] DataValue = datavaluetext.Split('|');
                                    for (int k = 0; k < Data.Length - 1; k++)
                                    {
                                        int variablevalueID = datavalueid[k].ToInt();
                                        string variableName = Data[k];
                                        string variableValueName = DataValue[k];
                                        ProductVariableValueController.Insert(kq1.ToInt(), productvariablesku, variablevalueID, variableName, variableValueName, false, DateTime.Now, username);
                                    }
                                }

                            }
                        }
                    }

                    PJUtils.ShowMessageBoxSwAlert("Cập nhật sản phẩm thành công", "s", true, Page);
                }
            }
        }
    }
}