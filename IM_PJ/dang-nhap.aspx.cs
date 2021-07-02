using IM_PJ.Controllers;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class dang_nhap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
                {
                    Response.Redirect("/trang-chu");
                }
            }
        }
        [WebMethod]
        public static string CheckSecurityCode(string code)
        {
            string SecurityCode = PJUtils.Encrypt("scode", code);

            var p = ConfigController.GetByTop1();

            if (SecurityCode == p.SecurityCode)
            {
                HttpContext.Current.Response.Cookies["securityCode"].Value = SecurityCode;
                HttpContext.Current.Response.Cookies["securityCode"].Expires = DateTime.Now.AddDays(1);
                return "ok";
            }
            else
            {
                return "null";
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string SecurityCode = "";
            if (Response.Cookies["securityCode"] != null)
            {
                SecurityCode = Request.Cookies["securityCode"].Value;
            }

            var p = ConfigController.GetByTop1();

            if (SecurityCode == p.SecurityCode)
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;
                var ac = AccountController.Login(username.Trim(), password.Trim());
                if (ac != null)
                {
                    if (ac.Status == 1)
                    {
                        Session["usernameLoginSystem_ANN123"] = username;
                        Session.Timeout = 43200;
                        Response.Cookies["usernameLoginSystem_ANN123"].Value = username;
                        Response.Cookies["usernameLoginSystem_ANN123"].Expires = DateTime.Now.AddDays(90);
                        Response.Redirect("/trang-chu");
                    }
                    else
                    {
                        lblError.Text = "Tài khoản của bạn đã bị khóa!";
                        lblError.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "Sai thông tin đăng nhập, hãy kiểm tra lại!";
                    lblError.Visible = true;
                }
            }
            else
            {
                lblError.Text = "Sai mã bảo mật!";
                lblError.Visible = true;
            }

            Response.Cookies["securityCode"].Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(Response.Cookies["securityCode"]);
        }
    }
}