using IM_PJ.Controllers;
using IM_PJ.Models.Pages.dang_ky_gui_di;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class tao_don_gop : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var userName = Request.Cookies["usernameLoginSystem_ANN123"] != null
                    ? Request.Cookies["usernameLoginSystem_ANN123"].Value
                    : null;

                if (String.IsNullOrEmpty(userName))
                    Response.Redirect("/dang-nhap");

                var acc = AccountController.GetByUsername(userName);

                if (acc == null)
                    Response.Redirect("/dang-nhap");
                else if (acc.RoleID != 0)
                    Response.Redirect("/trang-chu");

                hdfStaff.Value = userName;
            }
        }
    }
}