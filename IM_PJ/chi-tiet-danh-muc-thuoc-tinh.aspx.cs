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
    public partial class chi_tiet_danh_muc_thuoc_tinh : System.Web.UI.Page
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
                        if (acc.RoleID == 2)
                        {
                            Response.Redirect("/trang-chu");
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
                var d = VariableController.GetByID(id);
                if (d != null)
                {
                    ViewState["ID"] = id;
                    txtCustomerName.Text = d.VariableName;
                    chkIsHidden.Checked = Convert.ToBoolean(d.IsHidden);
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
                        var d = VariableController.GetByID(id);
                        if (d != null)
                        {
                            VariableController.Update(id, txtCustomerName.Text, "", chkIsHidden.Checked, DateTime.Now, username);
                            PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công", "s", true, Page);
                        }
                    }
                }
            }
        }
    }
}