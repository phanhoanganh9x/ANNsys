using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web;
using IM_PJ.Controllers;
using Newtonsoft.Json;
using IM_PJ.Models.Pages.thuc_hien_kiem_kho;
using IM_PJ.Models;
using System.Collections.Generic;

namespace IM_PJ
{
    public partial class dang_ky_ghtk : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem_ANN123"] == null)
                    Response.Redirect("/dang-nhap");
            }
        }
    }
}