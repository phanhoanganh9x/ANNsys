﻿using System;
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
    public partial class thuc_hien_kiem_kho : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["loginHiddenPage"] == null)
                    Response.Redirect("/login-hidden-page");
            }
        }
    }
}