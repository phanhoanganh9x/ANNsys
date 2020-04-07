﻿using IM_PJ.Controllers;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace IM_PJ
{
    public partial class uploadf : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UploadFile(sender, e);
                Response.Write("ok");
            }
        }

        protected void UploadFile(object sender, EventArgs e)
        {
            try
            {
                HttpFileCollection fileCollection = Request.Files;
                string savedfile = "";
                for (int i = 0; i < fileCollection.Count; i++)
                {
                    try
                    {
                        HttpPostedFile upload = fileCollection[i];
                        int f = fileCollection[i].ContentLength;
                        string filename = "/ProductImages/" + fileCollection[i].FileName;
                        upload.SaveAs(Server.MapPath(filename));
                        savedfile += fileCollection[i].FileName;
                    }
                    catch
                    {

                    }

                }
            }
            catch (Exception ex)
            {
                List<string> ff = new List<string>();
                ff.Add(ex.Message.ToString());
                System.IO.File.WriteAllLines(Server.MapPath("/ProductImages/Error.txt"), ff);
            }

        }
    }
}