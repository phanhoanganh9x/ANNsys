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

namespace IM_PJ
{
    public partial class thong_tin_anh_san_pham : System.Web.UI.Page
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
                        if (acc.RoleID != 0)
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
                ViewState["ID"] = id;
                var pi = ProductImageController.GetByID(id);
                if (pi != null)
                {
                    if (pi.ProductImage != null)
                        imgDaiDien.ImageUrl = pi.ProductImage;
                    chkIsHidden.Checked = Convert.ToBoolean(pi.IsHidden);
                    int productid = Convert.ToInt32(pi.ProductID);
                    if (productid > 0)
                    {
                        var product = ProductController.GetByID(productid);
                        if (product != null)
                        {
                            ViewState["productid"] = productid;
                            ltrBack.Text = "<a href=\"/danh-sach-anh-san-pham?id=" + product.ID + "\" class=\"btn primary-btn fw-btn not-fullwidth\">Trở về</a>";
                        }
                    }
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0)
                {
                    int id = Convert.ToInt32(ViewState["ID"]);
                    var pi = ProductImageController.GetByID(id);
                    if (pi != null)
                    {
                        bool isHidden = chkIsHidden.Checked;
                        ///Lưu ảnh
                        string duongdan = "/uploads/images/";
                        string IMG = "";
                        if (hinhDaiDien.UploadedFiles.Count > 0)
                        {
                            foreach (UploadedFile f in hinhDaiDien.UploadedFiles)
                            {
                                var o = duongdan + Guid.NewGuid() + f.GetExtension();
                                try
                                {
                                    f.SaveAs(Server.MapPath(o));
                                    IMG = o;
                                }
                                catch { }
                            }
                        }
                        else
                        {
                            IMG = imgDaiDien.ImageUrl;
                        }
                        ProductImageController.Update(id, IMG, isHidden, currentDate, username);
                        PJUtils.ShowMessageBoxSwAlert("Cập nhật hình ảnh thành công", "s", true, Page);
                    }
                }
            }
        }
    }
}