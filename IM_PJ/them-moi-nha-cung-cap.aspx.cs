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

namespace IM_PJ
{
    public partial class them_moi_nha_cung_cap : System.Web.UI.Page
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
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0)
                {
                    SupplierController.Insert(txtSupplierName.Text, pDiscountNote.Content, txtSupplierPhone.Text, txtSupplierAddress.Text,
                        txtSupplierEmail.Text, chkIsHidden.Checked, DateTime.Now, username);
                    Response.Redirect("/danh-sach-nha-cung-cap.aspx");
                    //PJUtils.ShowMessageBoxSwAlert("Tạo mới thành công", "s", true, Page);
                }
            }
        }
    }
}