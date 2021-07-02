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

namespace IM_PJ
{
    public partial class xem_bai_viet : System.Web.UI.Page
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
                        LoadData();
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
            }
        }
        public void LoadData()
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

            int id = Request.QueryString["id"].ToInt(0);
            if (id > 0)
            {
                var p = PostController.GetByID(id);
                if (p == null)
                {
                    PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy bài viết " + id, "e", true, "/danh-sach-bai-viet", Page);
                }
                else
                {
                    this.Title = String.Format("{0} - Bài viết", p.Title.ToTitleCase());
                    ltrEditTop.Text = "";
                    if (acc.RoleID == 0 || acc.Username == "nhom_zalo502")
                    {
                        ltrEditTop.Text += "<a href='/sua-bai-viet?id=" + p.ID + "' class='btn primary-btn fw-btn not-fullwidth'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Chỉnh sửa</a>";
                    }
                    ltrEditTop.Text += "<a href='javascript:;' onclick='copyPostInfo(" + p.ID + ");' class='btn primary-btn not-fullwidth print-invoice-merged'><i class='fa fa-files-o'></i> Copy nội dung</a>";
                    ltrEditTop.Text += "<a href='javascript:;' onclick='getAllPostImage(" + p.ID + ");' class='btn primary-btn not-fullwidth print-invoice-merged'><i class='fa fa-cloud-download'></i> Tải hình</a>";
                    ltrEditTop.Text += "<a href='javascript:;' onclick='copyPostToApp(" + p.ID + ");' class='btn primary-btn not-fullwidth print-invoice-merged'><i class='fa fa-cloud-download'></i> Copy vào App</a>";
                    ltrEditBottom.Text = ltrEditTop.Text;
                    ltrTitle.Text = p.Title;
                    ltrContent.Text = p.Content;

                    // thư viện ảnh
                    imageGallery.Text = "<ul class='image-gallery'>";
                    if (!String.IsNullOrEmpty(p.Image))
                    {
                        imageGallery.Text += "<li><img src='" + p.Image + "' /><a href='" + p.Image + "' download class='btn download-btn download-image h45-btn'><i class='fa fa-cloud-download'></i> Tải hình này</a></li>";
                    }
                    var image = PostImageController.GetByPostID(id);
                    if (image != null)
                    {
                        foreach (var img in image)
                        {
                            if (img.Image != p.Image)
                            {
                                imageGallery.Text += "<li><img src='" + img.Image + "' /><a href='" + img.Image + "' download class='btn download-btn download-image h45-btn'><i class='fa fa-cloud-download'></i> Tải hình này</a></li>";
                            }
                        }
                    }
                    imageGallery.Text += "</ul>";
                }
            }
        }
    }
}