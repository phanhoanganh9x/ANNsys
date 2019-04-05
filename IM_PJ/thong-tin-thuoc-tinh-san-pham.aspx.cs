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
    public partial class thong_tin_thuoc_tinh_san_pham : System.Web.UI.Page
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
                            hdfcost.Value = "ok";
                        }
                        else if (acc.RoleID == 1)
                        {

                        }
                        else
                        {
                            Response.Redirect("/dang-nhap");
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
            int id = Request.QueryString["id"].ToInt(0);
            if (id > 0)
            {
                var pv = ProductVariableController.GetByID(id);
                if (pv != null)
                {
                    ViewState["ID"] = id;
                    ViewState["SKU"] = pv.SKU;
                    chkIsHidden.Checked = Convert.ToBoolean(pv.IsHidden);
                    lblSKU.Text = pv.SKU;
                    pRegular_Price.Text = pv.Regular_Price.ToString();
                    pCostOfGood.Text = pv.CostOfGood.ToString();
                    pRetailPrice.Text = pv.RetailPrice.ToString();
                    pMinimumInventoryLevel.Text = pv.MinimumInventoryLevel.ToString();
                    pMaximumInventoryLevel.Text = pv.MaximumInventoryLevel.ToString();
                    if (pv.Image != null)
                    {
                        ListProductThumbnail.Value = pv.Image;
                        ProductThumbnail.ImageUrl = pv.Image;
                    }
                    int productid = Convert.ToInt32(pv.ProductID);
                    if (productid > 0)
                    {
                        var product = ProductController.GetByID(productid);
                        if (product != null)
                        {
                            ViewState["productid"] = productid;
                            ViewState["productsku"] = product.ProductSKU;
                            ltrBack.Text = "<a href=\"/thuoc-tinh-san-pham?id=" + product.ID + "\" class=\"btn primary-btn fw-btn not-fullwidth\">Trở về</a>";
                        }
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Request.Cookies["userLoginSystem"].Value;
            int id = Convert.ToInt32(ViewState["ID"]);
            var pv = ProductVariableController.GetByID(id);
            if (pv != null)
            {
                string parentSKU = ViewState["productsku"].ToString();
                int productID = ViewState["productid"].ToString().ToInt(0);

                bool isHidden = chkIsHidden.Checked;
                string SKU = ViewState["SKU"].ToString();
                double Stock = Convert.ToDouble(pv.Stock);
                double Regular_Price = Convert.ToDouble(pRegular_Price.Text);
                double CostOfGood = Convert.ToDouble(pCostOfGood.Text);
                double RetailPrice = Convert.ToDouble(pRetailPrice.Text);
                int StockStatus = Convert.ToInt32(pv.StockStatus);
                bool ManageStock = true;

                //Phần thêm ảnh đại diện sản phẩm
                string path = "/uploads/images/";
                string ProductImage = ListProductThumbnail.Value;
                if (ProductThumbnailImage.UploadedFiles.Count > 0)
                {
                    foreach (UploadedFile f in ProductThumbnailImage.UploadedFiles)
                    {
                        var o = path + productID + '-' + Slug.ConvertToSlug(Path.GetFileName(f.FileName));
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

                ProductVariableController.Update(id, productID, parentSKU, SKU, Stock, StockStatus, Regular_Price,
                    CostOfGood, RetailPrice, ProductImage, ManageStock, isHidden, currentDate, username, Convert.ToInt32(pv.SupplierID),
                    pv.SupplierName, Convert.ToDouble(pMinimumInventoryLevel.Text),
                    Convert.ToDouble(pMaximumInventoryLevel.Text));
                PJUtils.ShowMessageBoxSwAlert("Cập nhật thuộc tính thành công", "s", true, Page);
            }
        }
    }
}