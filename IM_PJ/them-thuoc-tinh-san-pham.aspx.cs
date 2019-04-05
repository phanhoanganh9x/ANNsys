using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


namespace IM_PJ
{
    public partial class them_thuoc_tinh_san_pham : System.Web.UI.Page
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
                        hdfUserRole.Value = acc.RoleID.ToString();

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
            int productid = Request.QueryString["productid"].ToInt(0);
            if (productid > 0)
            {
                var product = ProductController.GetByID(productid);
                if (product != null)
                {
                    txtProductSKU.Text = product.ProductSKU.Trim().ToUpper();
                    pMinimumInventoryLevel.Text = product.MinimumInventoryLevel.ToString();
                    pMaximumInventoryLevel.Text = product.MaximumInventoryLevel.ToString();
                    pRegular_Price.Text = product.Regular_Price.ToString();
                    pCostOfGood.Text = product.CostOfGood.ToString();
                    pRetailPrice.Text = product.Retail_Price.ToString();
                    ViewState["productid"] = productid;
                    ViewState["productsku"] = product.ProductSKU;
                    ltrBack.Text = "<a href=\"/thuoc-tinh-san-pham?id=" + product.ID + "\" class=\"btn primary-btn fw-btn not-fullwidth\">Trở về</a>";
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Request.Cookies["userLoginSystem"].Value;

            bool check = true;
            string SKU = txtProductSKU.Text.Trim().ToUpper();
            var productcheck = ProductController.GetBySKU(SKU);
            if (productcheck != null)
            {
                check = false;
            }
            else
            {
                var productvariable = ProductVariableController.GetBySKU(SKU);
                if (productvariable != null)
                    check = false;
            }
            if (check == false)
            {
                PJUtils.ShowMessageBoxSwAlert("Trùng mã sản phẩm vui lòng kiểm tra lại", "e", false, Page);
            }
            else
            {
                int parentID = ViewState["productid"].ToString().ToInt(0);

                var parentProduct = ProductController.GetByID(parentID);

                bool isHidden = false;

                double Stock = 0;
                double Regular_Price = Convert.ToDouble(pRegular_Price.Text);
                double CostOfGood = Convert.ToDouble(pCostOfGood.Text);
                double RetailPrice = Convert.ToDouble(pRetailPrice.Text);
                int StockStatus = 1;
                bool ManageStock = true;

                //Phần thêm ảnh đại diện sản phẩm
                string path = "/uploads/images/";
                string ProductImage = "";
                if (ProductThumbnailImage.UploadedFiles.Count > 0)
                {
                    foreach (UploadedFile f in ProductThumbnailImage.UploadedFiles)
                    {
                        var o = path + parentID.ToString() + '-' + Slug.ConvertToSlug(Path.GetFileName(f.FileName));
                        try
                        {
                            f.SaveAs(Server.MapPath(o));
                            ProductImage = o;
                        }
                        catch { }
                    }
                }
                ProductVariableController.Insert(parentProduct.ID, parentProduct.ProductSKU, SKU, Stock, StockStatus, Regular_Price,
                    CostOfGood, RetailPrice, ProductImage, ManageStock, isHidden, currentDate, username, Convert.ToInt32(parentProduct.SupplierID),
                    parentProduct.SupplierName, Convert.ToDouble(pMinimumInventoryLevel.Text),
                    Convert.ToDouble(pMaximumInventoryLevel.Text));

                PJUtils.ShowMessageBoxSwAlert("Thêm biến thể sản phẩm thành công", "s", true, Page);
            }
        }
    }
}