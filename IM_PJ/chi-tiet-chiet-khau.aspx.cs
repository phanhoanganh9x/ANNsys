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
    public partial class chi_tiet_chiet_khau : System.Web.UI.Page
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
                var d = DiscountController.GetByID(id);
                if (d != null)
                {
                    ViewState["ID"] = id;
                    txtQuantity.Text = d.Quantity.ToString();
                    txtDiscountPerProduct.Text = d.DiscountPerProduct.ToString();
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0)
                {
                    int id = ViewState["ID"].ToString().ToInt(0);
                    if (id > 0)
                    {
                        var d = DiscountGroupController.GetByID(id);
                        if (d != null)
                        {
                            var dc = DiscountController.update(id, txtQuantity.Text.ToInt(), txtDiscountPerProduct.Text.ToInt(), username);
                            if (dc > 0)
                                PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công", "s", true, Page);
                            else
                                PJUtils.ShowMessageBoxSwAlert("Thất bại", "e", false, Page);
                        }
                    }
                }
            }
        }
    }
}