using IM_PJ.Controllers;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class them_danh_muc : System.Web.UI.Page
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
            LoadCategory();
        }
        public void LoadCategory()
        {
            var category = CategoryController.GetAllWithIsHidden(false);
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new ListItem("Không chọn danh mục cha", "0"));
            if (category.Count > 0)
            {
                addItemCategory(0, "");
                ddlCategory.DataBind();
            }
        }
        public void addItemCategory(int id, string h = "")
        {
            var categories = CategoryController.GetByParentID("", id);

            if (categories.Count > 0)
            {
                foreach (var c in categories)
                {
                    ListItem listitem = new ListItem(h + c.CategoryName, c.ID.ToString());
                    ddlCategory.Items.Add(listitem);

                    addItemCategory(c.ID, h + "---");
                }
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            DateTime currentDate = DateTime.Now;

            int parentID = ddlCategory.SelectedValue.ToInt();
            int ID = CategoryController.Insert(txtCategoryName.Text, txtCategoryDescription.Text, parentID, parentID, false, currentDate, username);
            if (ID > 0)
            {
                PJUtils.ShowMessageBoxSwAlertCallFunction("Tạo danh mục sản phẩm thành công", "s", true, "redirectTo(" + ID + ")", Page);
            }
                    
        }
    }
}