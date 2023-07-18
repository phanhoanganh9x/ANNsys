using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using Newtonsoft.Json;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace IM_PJ
{
    public partial class tao_bai_viet_app : System.Web.UI.Page
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
                        if (acc.RoleID == 0 || acc.Username == "baiviet" || acc.RoleID == 2)
                        {
                            LoadCategory();
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
        /// <summary>
        /// Khởi tao thông tin video bài post
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="videoId"></param>
        /// <param name="linkDownload"></param>
        /// <param name="postId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        private void _createPostVideo(string videoId, string linkDownload, int postId, bool isActive)
        {
            #region Khởi tạo API
            var api = "http://ann-shop-dotnet-core.com/api/v1/post-video/create";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(api);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    videoId = videoId,
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

        [WebMethod]
        public static string getParent(int parent)
        {
            List<GetOutCategory> gc = new List<GetOutCategory>();
            if (parent != 0)
            {
                var parentlist = PostPublicCategoryController.API_GetByParentID(parent);
                if (parentlist != null)
                {

                    for (int i = 0; i < parentlist.Count; i++)
                    {
                        GetOutCategory go = new GetOutCategory();
                        go.ID = parentlist[i].ID;
                        go.CategoryName = parentlist[i].Name;
                        gc.Add(go);
                    }
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(gc);
        }

        public class GetOutCategory
        {
            public int ID { get; set; }
            public string CategoryName { get; set; }
            public string CategoryLevel { get; set; }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            int cateID = hdfParentID.Value.ToInt();
            if (cateID > 0)
            {
                int CategoryID = ddlCategory.Value.ToInt();
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
                bool IsPolicy = false;

                var newPostPublic = new PostPublic()
                {
                    CategoryID = CategoryID,
                    CategorySlug = CategorySlug,
                    Title = Title,
                    Thumbnail = "",
                    Summary = Summary,
                    Content = Content,
                    Action = Action,
                    ActionValue = ActionValue,
                    AtHome = AtHome,
                    IsPolicy = IsPolicy,
                    CreatedDate = currentDate,
                    CreatedBy = acc.Username,
                    ModifiedDate = currentDate,
                    ModifiedBy = acc.Username
                };

                var post = PostPublicController.Insert(newPostPublic);

                if (post != null)
                {
                    #region Khởi tạo thông tin video sản phẩm
                    if (!String.IsNullOrEmpty(hdfVideoId.Value))
                    {
                        var videoId = hdfVideoId.Value;
                        var linkDownload = String.IsNullOrEmpty(txtLinkDownload.Text) ? null : txtLinkDownload.Text.Trim();
                        var isActive = rdbActiveVideo.SelectedValue == "true";

                        _createPostVideo(videoId, linkDownload, post.ID, isActive);
                    }
                    #endregion

                    // Thêm ảnh đại diện
                    string path = "/uploads/images/posts/";
                    string Image = "";
                    if (PostPublicThumbnailImage.UploadedFiles.Count > 0)
                    {
                        foreach (UploadedFile f in PostPublicThumbnailImage.UploadedFiles)
                        {
                            var o = path + "post-app-" + post.ID.ToString() + "-" + Slug.ConvertToSlug(Path.GetFileName(f.FileName), isFile: true);
                            try
                            {
                                f.SaveAs(Server.MapPath(o));
                                Image = o;
                            }
                            catch { }
                        }
                    }
                    string updateImage = PostPublicController.UpdateImage(post.ID, Image);

                    // Thêm thư viện ảnh
                    string IMG = "";
                    if (ImageGallery.UploadedFiles.Count > 0)
                    {
                        foreach (UploadedFile f in ImageGallery.UploadedFiles)
                        {
                            var o = path + "post-app-" + post.ID.ToString() + "-" + Slug.ConvertToSlug(Path.GetFileName(f.FileName), isFile: true);
                            try
                            {
                                f.SaveAs(Server.MapPath(o));
                                IMG = o;
                                PostPublicImageController.Insert(post.ID, IMG, username, currentDate);
                            }
                            catch { }
                        }
                    }

                    // Copy bài viết vào hệ thống gốc
                    if (ddlCopyToSystem.SelectedValue == "True" && post.Action == "view_more")
                    {
                        var categorySystem = PostCategoryController.GetByName(category.Name);
                        var postSystem = new tbl_Post()
                        {
                            Title = post.Title,
                            Content = post.Content,
                            Image = Image,
                            Featured = 1,
                            CategoryID = categorySystem != null ? categorySystem.ID : 0,
                            Status = 1,
                            CreatedBy = post.CreatedBy,
                            CreatedDate = post.CreatedDate,
                            ModifiedBy = post.ModifiedBy,
                            ModifiedDate = post.ModifiedDate,
                            WebPublish = true,
                            WebUpdate = post.CreatedDate,
                            Slug = post.ActionValue
                        };

                        PostController.Insert(postSystem);

                        // Copy image
                        if (postSystem != null)
                        {
                            var imagePostPublic = PostPublicImageController.GetByPostID(post.ID);
                            if (imagePostPublic.Count > 0)
                            {
                                foreach (var item in imagePostPublic)
                                {
                                    PostImageController.Insert(postSystem.ID, item.Image, postSystem.CreatedBy, DateTime.Now);
                                }
                            }

                        }

                    }


                    // Tạo phiên bản cho wordpress
                    if (!String.IsNullOrEmpty(hdfPostVariants.Value))
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        var variants = serializer.Deserialize<List<PostClone>>(hdfPostVariants.Value);
                        if (variants != null)
                        {
                            foreach (var item in variants)
                            {
                                var newPostClone = new PostClone()
                                {
                                    PostPublicID = post.ID,
                                    Web = item.Web,
                                    PostWebID = 0,
                                    CategoryID = post.CategoryID,
                                    CategoryName = category.Name,
                                    Title = !String.IsNullOrEmpty(item.Title) ? item.Title : post.Title,
                                    Summary = post.Summary,
                                    Content = post.Content,
                                    Thumbnail = Image,
                                    CreatedDate = post.CreatedDate,
                                    CreatedBy = acc.Username,
                                    ModifiedDate = post.ModifiedDate,
                                    ModifiedBy = acc.Username
                                };

                                PostCloneController.Insert(newPostClone);
                            }
                        }
                    }

                    PJUtils.ShowMessageBoxSwAlertCallFunction("Tạo bài viết thành công", "s", true, "redirectTo(" + post.ID.ToString() + ")", Page);
                }
            }
        }
    }
}