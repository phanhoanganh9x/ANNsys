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
    public partial class sua_bai_viet : System.Web.UI.Page
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
                        if (acc.RoleID == 0 || acc.Username == "nhom_zalo502")
                        {
                            LoadCategory();
                            LoadData();
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
            }
        }

        public void LoadCategory()
        {
            var category = PostCategoryController.GetAll();
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new ListItem("Chọn danh mục bài viết", "0"));
            if (category.Count > 0)
            {
                addItemCategory(0, "");
                ddlCategory.DataBind();
            }
        }

        public void addItemCategory(int id, string h = "")
        {
            var categories = PostCategoryController.GetByParentID("", id);

            if (categories.Count > 0)
            {
                foreach (var c in categories)
                {
                    ListItem listitem = new ListItem(h + c.Title, c.ID.ToString());
                    ddlCategory.Items.Add(listitem);

                    addItemCategory(c.ID, h + "---");
                }
            }
        }
        public void LoadData()
        {
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
                    this.Title = String.Format("{0} - Sửa bài viết", p.Title.ToTitleCase());

                    ViewState["ID"] = id;
                    ViewState["cateID"] = p.CategoryID;
                    hdfParentID.Value = p.CategoryID.ToString();
                    ltrBack.Text = "<a href='/xem-bai-viet?id=" + p.ID + "' class='btn primary-btn fw-btn not-fullwidth'><i class='fa fa-arrow-left' aria-hidden='true'></i> Trở về</a>";
                    ltrBack2.Text = ltrBack.Text;
                    txtTitle.Text = p.Title;
                    txtSlug.Text = p.Slug;
                    pContent.Content = p.Content;
                    ddlCategory.SelectedValue = p.CategoryID.ToString();
                    ddlFeatured.SelectedValue = p.Featured.ToString();

                    if (p.Image != null)
                    {
                        ListPostThumbnail.Value = p.Image;
                        PostThumbnail.ImageUrl = p.Image;
                    }

                    var image = PostImageController.GetByPostID(id);
                    imageGallery.Text = "<ul class='image-gallery'>";
                    if (image != null)
                    {
                        foreach (var img in image)
                        {
                            imageGallery.Text += "<li><img src='" + img.Image + "'><a href='javascript:;' data-image-id='" + img.ID + "' onclick='deleteImageGallery($(this))' class='btn-delete'><i class='fa fa-times' aria-hidden='true'></i> Xóa hình</a></li>";
                        }
                    }
                    imageGallery.Text += "</ul>";

                    string PostInfo = "<p><strong>Ngày tạo</strong>: " + p.CreatedDate + "</p>";
                    PostInfo += "<p><strong>Người viết</strong>: " + p.CreatedBy + "</p>";
                    PostInfo += "<p><strong>Ngày cập nhật</strong>: " + p.ModifiedDate + "</p>";
                    PostInfo += "<p><strong>Người cập nhật</strong>: " + p.ModifiedBy + "</p>";
                    ltrPostInfo.Text = PostInfo;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            int cateID = ViewState["cateID"].ToString().ToInt(0);
            int PostID = ViewState["ID"].ToString().ToInt(0);
            if (cateID > 0)
            {
                string Title = txtTitle.Text.Trim();
                string PostSlug = Slug.ConvertToSlug(txtSlug.Text.Trim());
                string Content = pContent.Content.ToString();
                int CategoryID = hdfParentID.Value.ToInt();

                //Phần thêm ảnh đại diện sản phẩm
                string path = "/uploads/images/posts/";
                string PostImage = ListPostThumbnail.Value;
                if (PostThumbnailImage.UploadedFiles.Count > 0)
                {
                    foreach (UploadedFile f in PostThumbnailImage.UploadedFiles)
                    {
                        var o = path + "post-" + PostID + '-' + Slug.ConvertToSlug(Path.GetFileName(f.FileName), isFile: true);
                        try
                        {
                            f.SaveAs(Server.MapPath(o));
                            PostImage = o;
                        }
                        catch { }
                    }
                }

                // Delete Image Gallery
                string deleteImageGallery = hdfDeleteImageGallery.Value;
                if (deleteImageGallery != "")
                {
                    string[] deletelist = deleteImageGallery.Split(',');

                    for (int i = 0; i < deletelist.Length - 1; i++)
                    {
                        var img = PostImageController.GetByID(Convert.ToInt32(deletelist[i]));
                        if (img != null)
                        {
                            string delete = PostImageController.Delete(img.ID);
                        }
                    }
                }

                // Update post
                string kq = PostController.Update(PostID, Title, Content, PostImage, ddlFeatured.SelectedValue.ToInt(), CategoryID, 1, PostSlug, username, DateTime.Now);

                // Upload image gallery
                if (UploadImages.HasFiles)
                {
                    foreach (HttpPostedFile uploadedFile in UploadImages.PostedFiles)
                    {
                        var o = path + "post-" + PostID + '-' + Slug.ConvertToSlug(Path.GetFileName(uploadedFile.FileName), isFile: true);
                        uploadedFile.SaveAs(Server.MapPath(o));
                        PostImageController.Insert(PostID, o, username, DateTime.Now);
                    }
                }

                if (kq.ToInt(0) > 0)
                {
                    PJUtils.ShowMessageBoxSwAlertCallFunction("Cập nhật bài viết thành công", "s", true, "redirectTo(" + kq + ")", Page);
                }
            }
        }
    }
}