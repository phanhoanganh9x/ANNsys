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
    public partial class chi_tiet_thuoc_tinh : System.Web.UI.Page
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
                var d = VariableValueController.GetByID(id);
                if (d != null)
                {
                    ViewState["ID"] = id;
                    txtVariableValue.Text = d.VariableValue;
                    txtSKUText.Text = d.SKUText;
                    int ID = Convert.ToInt32(d.VariableID);
                    chkIsHidden.Checked = Convert.ToBoolean(d.IsHidden);
                    ltrBack.Text = "<a href=\"/quan-ly-thuoc-tinh-san-pham?id=" + ID + "\" class=\"btn primary-btn fw-btn not-fullwidth\">Trở về</a>";
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;

            int id = ViewState["ID"].ToString().ToInt(0);
            if (id > 0)
            {
                var d = VariableValueController.GetByID(id);
                if (d != null)
                {
                    int variID =Convert.ToInt32(d.VariableID);
                    string variName = d.VariableName;
                    VariableValueController.Update(id, variID, variName, txtVariableValue.Text,chkIsHidden.Checked, DateTime.Now, username,txtSKUText.Text);
                    PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công", "s", true, Page);
                }
            }
        }
    }
}