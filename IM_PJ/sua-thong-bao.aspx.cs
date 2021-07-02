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
    public partial class sua_thong_bao : System.Web.UI.Page
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
            var category = NotifyCategoryController.GetAll();
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
            var categories = NotifyCategoryController.GetByParentID("", id);

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
                var p = NotifyController.GetByID(id);
                if (p == null)
                {
                    PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy bài viết " + id, "e", true, "/danh-sach-thong-bao", Page);
                }
                else
                {
                    this.Title = String.Format("{0} - Sửa thông báo", p.Title.ToTitleCase());

                    ViewState["ID"] = id;
                    ViewState["cateID"] = p.CategoryID;
                    hdfParentID.Value = p.CategoryID.ToString();
                    ltrBack.Text = "<a href='/xem-thong-bao?id=" + p.ID + "' class='btn primary-btn fw-btn not-fullwidth'><i class='fa fa-arrow-left' aria-hidden='true'></i> Trở về</a>";
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
                    pSummary.Content = p.Summary;
                    pContent.Content = p.Content;
                    if (p.Thumbnail != null)
                    {
                        ListPostPublicThumbnail.Value = p.Thumbnail;
                        PostPublicThumbnail.ImageUrl = p.Thumbnail;
                    }
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
            DateTime currentDate = DateTime.Now;

            int PostID = ViewState["ID"].ToString().ToInt(0);
            var post = NotifyController.GetByID(PostID);

            if (post != null)
            {
                int CategoryID = hdfParentID.Value.ToInt();
                var category = NotifyCategoryController.GetByID(CategoryID);
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

                //Phần thêm ảnh đại diện sản phẩm
                string path = "/uploads/images/posts/";
                string Thumbnail = ListPostPublicThumbnail.Value;
                if (PostPublicThumbnailImage.UploadedFiles.Count > 0)
                {
                    foreach (UploadedFile f in PostPublicThumbnailImage.UploadedFiles)
                    {
                        var o = path + "notify-" + PostID + '-' + Slug.ConvertToSlug(Path.GetFileName(f.FileName), isFile: true);
                        try
                        {
                            f.SaveAs(Server.MapPath(o));
                            Thumbnail = o;
                        }
                        catch { }
                    }
                }
                if (Thumbnail != ListPostPublicThumbnail.Value)
                {
                    if (File.Exists(Server.MapPath(ListPostPublicThumbnail.Value)))
                    {
                        File.Delete(Server.MapPath(ListPostPublicThumbnail.Value));
                    }
                }

                // Update post
                var oldNotify = new NotifyNew()
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
                    CreatedDate = post.CreatedDate,
                    CreatedBy = acc.Username,
                    ModifiedDate = currentDate,
                    ModifiedBy = acc.Username
                };

                var updatePost = NotifyController.Update(oldNotify);

                if (updatePost != null)
                {
                    PJUtils.ShowMessageBoxSwAlertCallFunction("Cập nhật bài viết thành công", "s", true, "redirectTo(" + updatePost.ID.ToString() + ")", Page);
                }
            }
        }
    }
}