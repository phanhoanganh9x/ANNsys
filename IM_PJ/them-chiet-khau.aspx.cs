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
    public partial class them_chiet_khau : System.Web.UI.Page
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
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
            {
                string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                var acc = AccountController.GetByUsername(username);
                if (acc != null)
                {
                    if (acc.RoleID == 0)
                    {
                        
                        var kq = DiscountController.insert(txtQuantity.Text.ToInt(), txtDiscountPerProduct.Text.ToInt(), username);

                        if (kq != null)
                        {
                            PJUtils.ShowMessageBoxSwAlert("Tạo mới thành công", "s", true, Page);
                        }
                    }
                }
            }
            else
            {

                Response.Redirect("/dang-nhap");
            }

        }
    }
}