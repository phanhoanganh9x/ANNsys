using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace IM_PJ
{
    public partial class tao_bai_viet : System.Web.UI.Page
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
        
        [WebMethod]
        public static string getParent(int parent)
        {
            List<GetOutCategory> gc = new List<GetOutCategory>();
            if (parent != 0)
            {
                var parentlist = PostCategoryController.API_GetByParentID(parent);
                if (parentlist != null)
                {

                    for (int i = 0; i < parentlist.Count; i++)
                    {
                        GetOutCategory go = new GetOutCategory();
                        go.ID = parentlist[i].ID;
                        go.CategoryName = parentlist[i].Title;
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
                string Title = txtTitle.Text.Trim();
                string PostSlug = Slug.ConvertToSlug(txtSlug.Text.Trim());
                string Content = pContent.Content.ToString();

                var newPost = new tbl_Post()
                {
                    Title = Title,
                    Content = Content,
                    Image = "",
                    Featured = ddlFeatured.SelectedValue.ToInt(),
                    CategoryID = cateID,
                    Status = 1,
                    CreatedBy = acc.Username,
                    CreatedDate = currentDate,
                    ModifiedBy = acc.Username,
                    ModifiedDate = currentDate,
                    WebPublish = false,
                    WebUpdate = currentDate,
                    Slug = PostSlug
                };

                var post = PostController.Insert(newPost);

                if (post != null)
                {
                    //Phần thêm ảnh đại diện
                    string path = "/uploads/images/posts/";
                    string Image = "";
                    if (ProductThumbnailImage.UploadedFiles.Count > 0)
                    {
                        foreach (UploadedFile f in ProductThumbnailImage.UploadedFiles)
                        {
                            var o = path + "post-" + post.ID + "-" + Slug.ConvertToSlug(Path.GetFileName(f.FileName), isFile: true);
                            try
                            {
                                f.SaveAs(Server.MapPath(o));
                                Image = o;
                            }
                            catch { }
                        }
                    }

                    string updateImage = PostController.UpdateImage(post.ID, Image);

                    //Phần thêm thư viện ảnh
                    string IMG = "";
                    if (hinhDaiDien.UploadedFiles.Count > 0)
                    {
                        foreach (UploadedFile f in hinhDaiDien.UploadedFiles)
                        {
                            var o = path + "post-" + post.ID + "-" + Slug.ConvertToSlug(Path.GetFileName(f.FileName), isFile: true);
                            try
                            {
                                f.SaveAs(Server.MapPath(o));
                                IMG = o;
                                PostImageController.Insert(post.ID, IMG, username, currentDate);
                            }
                            catch { }
                        }
                    }

                    PJUtils.ShowMessageBoxSwAlertCallFunction("Tạo bài viết thành công", "s", true, "redirectTo(" + post.ID + ")", Page);
                }
            }
        }
    }
}