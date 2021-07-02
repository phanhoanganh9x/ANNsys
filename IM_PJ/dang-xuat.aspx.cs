using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class dang_xuat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
            {
                HttpCookie userLoginSystem = new HttpCookie("usernameLoginSystem");
                userLoginSystem.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(userLoginSystem);
            }
            Session.Abandon();
            Response.Redirect("/dang-nhap");
        }
    }
}