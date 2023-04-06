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
using System.Web.Script.Serialization;
using System.Net;
using Newtonsoft.Json;

namespace IM_PJ
{
    public partial class sua_bai_viet_app : System.Web.UI.Page
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
                        if (acc.RoleID == 0 || acc.Username == "baiviet")
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

        #region Private
        #region Update
        #region Cập nhật thông tin video
        /// <summary>
        /// Cập nhật thông tin video bài post
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="oldVideoId"></param>
        /// <param name="newVideoId"></param>
        /// <param name="linkDownload"></param>
        /// <param name="postId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        private void _updatePostVideo(string oldVideoId, string newVideoId, string linkDownload, int postId, bool isActive)
        {
            #region Khởi tạo API
            var api = "http://ann-shop-dotnet-core.com/api/v1/post-video/update";
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
                    postId = postId,
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
        /// Xóa thông tin video bài post
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="videoId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        private void _deletePostVideo(string videoId, int postId)
        {
            #region Khởi tạo API
            var api = "http://ann-shop-dotnet-core.com/api/v1/post-video/delete";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(api);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    videoId = videoId,
                    postId = postId
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

        public void LoadCategory()
        {
            var category = PostPublicCategoryController.GetAll();
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new ListItem("Chọn danh mục", "0"));
            if (category.Count > 0)
            {
                addItemCategory(0, "");
                ddlCategory.DataBind();
            }
        }

        public void addItemCategory(int id, string h = "")
        {
            var categories = PostPublicCategoryController.GetByParentID("", id);

            if (categories.Count > 0)
            {
                foreach (var c in categories)
                {
                    ListItem listitem = new ListItem(h + c.Name, c.ID.ToString());
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
                var p = PostPublicController.GetByID(id);
                if (p == null)
                {
                    PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy bài viết " + id, "e", true, "/danh-sach-bai-viet-app", Page);
                }
                else
                {
                    this.Title = String.Format("{0} - Sửa bài viết App", p.Title.ToTitleCase());

                    hdfPostId.Value = id.ToString();
                    ViewState["ID"] = id;
                    ViewState["cateID"] = p.CategoryID;
                    hdfParentID.Value = p.CategoryID.ToString();
                    ltrBack.Text = "<a href='/xem-bai-viet-app?id=" + p.ID + "' class='btn primary-btn fw-btn not-fullwidth'><i class='fa fa-arrow-left' aria-hidden='true'></i> Trở về</a>";
                    ltrBack2.Text = ltrBack.Text;
                    txtTitle.Text = p.Title;
                    if (p.Action == "show_web")
                    {
                        txtLink.Text = p.ActionValue;
                    }
                    else if (p.Action == "view_more")
                    {
                        txtSlug.Text = p.ActionValue;
                    }
                    ddlCategory.SelectedValue = p.CategoryID.ToString();
                    ddlAction.SelectedValue = p.Action.ToString();
                    hdfAction.Value = p.Action.ToString();
                    ddlAtHome.SelectedValue = p.AtHome.ToString();
                    ddlIsPolicy.SelectedValue = p.IsPolicy.ToString();
                    pSummary.Content = p.Summary;
                    pContent.Content = p.Content;
                    if (p.Thumbnail != null)
                    {
                        ListPostPublicThumbnail.Value = p.Thumbnail;
                        PostPublicThumbnail.ImageUrl = p.Thumbnail;
                    }

                    var image = PostPublicImageController.GetByPostID(id);
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

                    hdfPostVariants.Value = PostCloneController.getFeesJSON(id);
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;

            int PostID = ViewState["ID"].ToString().ToInt(0);
            var post = PostPublicController.GetByID(PostID);

            if (post != null)
            {
                int CategoryID = hdfParentID.Value.ToInt();
                var category = PostPublicCategoryController.GetByID(CategoryID);
                string CategorySlug = category.Slug;
                string Title = txtTitle.Text.Trim();
                string Slugs = Slug.ConvertToSlug(txtSlug.Text.Trim());
                string Link = txtLink.Text.Trim();
                string Content = pContent.Content.ToString();
                string Summary = HttpUtility.HtmlDecode(pSummary.Content.ToString());
                string Action = ddlAction.SelectedValue.ToString();
                string ActionValue = "";
                if (Action == "view_more")
                {
                    ActionValue = Slugs;
                }
                else if (Action == "show_web")
                {
                    ActionValue = Link;
                }
                bool AtHome = ddlAtHome.SelectedValue.ToBool();
                bool IsPolicy = ddlIsPolicy.SelectedValue.ToBool();

                //Phần thêm ảnh đại diện sản phẩm
                string path = "/uploads/images/posts/";
                string Thumbnail = ListPostPublicThumbnail.Value;
                if (PostPublicThumbnailImage.UploadedFiles.Count > 0)
                {
                    foreach (UploadedFile f in PostPublicThumbnailImage.UploadedFiles)
                    {
                        var o = path + "post-app-" + PostID + '-' + Slug.ConvertToSlug(Path.GetFileName(f.FileName), isFile: true);
                        try
                        {
                            f.SaveAs(Server.MapPath(o));
                            Thumbnail = o;
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
                        var img = PostPublicImageController.GetByID(Convert.ToInt32(deletelist[i]));
                        if (img != null)
                        {
                            string delete = PostPublicImageController.Delete(img.ID);
                        }
                    }
                }

                // Update post
                var oldPostPublic = new PostPublic()
                {
                    ID = PostID,
                    CategoryID = CategoryID,
                    CategorySlug = CategorySlug,
                    Title = Title,
                    Thumbnail = Thumbnail,
                    Summary = Summary,
                    Content = Content,
                    Action = Action,
                    ActionValue = ActionValue,
                    AtHome = AtHome,
                    IsPolicy = IsPolicy,
                    CreatedDate = post.CreatedDate,
                    CreatedBy = acc.Username,
                    ModifiedDate = currentDate,
                    ModifiedBy = acc.Username
                };

                var updatePost = PostPublicController.Update(oldPostPublic);

                if (updatePost != null)
                {
                    // Cập nhật video
                    if (!String.IsNullOrEmpty(hdfNewVideoId.Value))
                    {
                        var productId = Convert.ToInt32(hdfPostId.Value);
                        var linkDownload = String.IsNullOrEmpty(txtLinkDownload.Text) ? null : txtLinkDownload.Text.Trim();
                        var isActive = rdbActiveVideo.SelectedValue == "true";

                        _updatePostVideo(hdfOldVideoId.Value, hdfNewVideoId.Value, linkDownload, productId, isActive);
                    }
                    else if (!String.IsNullOrEmpty(hdfOldVideoId.Value))
                    {
                        var productId = Convert.ToInt32(hdfPostId.Value);

                        _deletePostVideo(hdfOldVideoId.Value, productId);
                    }

                    // Cập nhật thư viện ảnh cho bài viết
                    if (UploadImages.HasFiles)
                    {
                        foreach (HttpPostedFile uploadedFile in UploadImages.PostedFiles)
                        {
                            var o = path + "post-app-" + PostID + '-' + Slug.ConvertToSlug(Path.GetFileName(uploadedFile.FileName), isFile: true);
                            uploadedFile.SaveAs(Server.MapPath(o));
                            PostPublicImageController.Insert(PostID, o, username, DateTime.Now);
                        }
                    }

                    // tạo phiên bản cho wordpress
                    if (!String.IsNullOrEmpty(hdfPostVariants.Value))
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        var variants = serializer.Deserialize<List<PostClone>>(hdfPostVariants.Value);
                        if (variants != null)
                        {
                            foreach (var item in variants)
                            {
                                var getpostClone = PostCloneController.Get(updatePost.ID, item.Web);
                                if (getpostClone == null)
                                {
                                    continue;
                                }

                                var oldPostClone = new PostClone()
                                {
                                    ID = getpostClone.ID,
                                    PostPublicID = updatePost.ID,
                                    Web = getpostClone.Web,
                                    PostWebID = getpostClone.PostWebID,
                                    CategoryName = category.Name,
                                    CategoryID = updatePost.CategoryID,
                                    Title = !String.IsNullOrEmpty(item.Title) ? item.Title : updatePost.Title,
                                    Summary = updatePost.Summary,
                                    Content = updatePost.Content,
                                    Thumbnail = updatePost.Thumbnail,
                                    CreatedBy = getpostClone.CreatedBy,
                                    CreatedDate = getpostClone.CreatedDate,
                                    ModifiedDate = currentDate,
                                    ModifiedBy = acc.Username
                                };

                                PostCloneController.Update(oldPostClone);
                            }
                        }
                    }

                    PJUtils.ShowMessageBoxSwAlertCallFunction("Cập nhật bài viết thành công", "s", true, "redirectTo(" + updatePost.ID.ToString() + ")", Page);
                }
            }
        }
    }
}