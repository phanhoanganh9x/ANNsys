using IM_PJ.Controllers;
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

        public static string htmlAll = "";
        public static int element = 0;
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

                        if (acc.RoleID == 2)
                        {
                            Response.Redirect("/trang-chu");
                        }
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
                LoadCategory();
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
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            if (acc != null)
            {
                if (acc.RoleID == 0 || acc.RoleID == 1)
                {
                    int cateID = hdfParentID.Value.ToInt();
                    if (cateID > 0)
                    {
                        string Title = txtTitle.Text.ToString();
                        string Content = pContent.Content.ToString();

                        string kq = PostController.Insert(Title, Content, "", ddlFeatured.SelectedValue.ToInt(), cateID, 1, acc.Username, currentDate);

                        //Phần thêm ảnh đại diện
                        string path = "/uploads/images/";
                        string Image = "";
                        if (ProductThumbnailImage.UploadedFiles.Count > 0)
                        {
                            foreach (UploadedFile f in ProductThumbnailImage.UploadedFiles)
                            {
                                var o = path + kq + '-' + Slug.ConvertToSlug(Path.GetFileName(f.FileName));
                                try
                                {
                                    f.SaveAs(Server.MapPath(o));
                                    Image = o;
                                }
                                catch { }
                            }
                        }

                        string updateImage = PostController.UpdateImage(kq.ToInt(), Image);


                        //Phần thêm thư viện ảnh
                        string IMG = "";
                        if (hinhDaiDien.UploadedFiles.Count > 0)
                        {
                            foreach (UploadedFile f in hinhDaiDien.UploadedFiles)
                            {
                                var o = path + kq + '-' + Slug.ConvertToSlug(Path.GetFileName(f.FileName));
                                try
                                {
                                    f.SaveAs(Server.MapPath(o));
                                    IMG = o;
                                    PostImageController.Insert(kq.ToInt(), IMG, username, currentDate);
                                }
                                catch { }
                            }
                        }


                        if (kq.ToInt(0) > 0)
                        {
                            PJUtils.ShowMessageBoxSwAlertCallFunction("Tạo sản phẩm thành công", "s", true, "redirectTo(" + kq + ")", Page);
                        }

                    }
                   
                }
            }

        }
    }
}