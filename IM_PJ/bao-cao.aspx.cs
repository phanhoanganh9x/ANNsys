using IM_PJ.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class bao_cao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var config = ConfigController.GetByTop1();
            if (config.ViewAllReports == 0)
            {
                Response.Redirect("/trang-chu");
            }

            if (!IsPostBack)
            {
                if (!IsPostBack)
                {
                    if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
                    {
                        string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                        var acc = AccountController.GetByUsername(username);
                        if (acc != null)
                        {
                            if(acc.RoleID != 0)
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
        }
    }
}