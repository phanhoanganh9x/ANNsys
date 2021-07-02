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

namespace IM_PJ
{
    public partial class thong_tin_dai_ly : System.Web.UI.Page
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
                            Response.Redirect("/dang-nhap");
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
                var a = AgentController.GetByID(id);
                if (a != null)
                {
                    ViewState["ID"] = id;
                    txtAgentName.Text = a.AgentName;
                    txtAgentLeader.Text = a.AgentLeader;
                    txtPhone.Text = a.AgentPhone;
                    txtAddress.Text = a.AgentAddress;
                    txtEmail.Text = a.AgentEmail;
                    if (a.IsHidden != null)
                        chkIsHidden.Checked = Convert.ToBoolean(a.IsHidden);
                }
            }
            else
            {
                Response.Redirect("/quan-ly-dai-ly");
            }

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
            {
                string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                var acc = AccountController.GetByUsername(username);
                if (acc != null)
                {
                    if (acc.RoleID == 0)
                    {
                        int ID = ViewState["ID"].ToString().ToInt(0);
                        string kq = AgentController.Update(ID, txtAgentName.Text, "", txtAddress.Text, txtPhone.Text, txtEmail.Text, txtAgentLeader.Text,
                            chkIsHidden.Checked, currentDate, username, "");
                        if (kq.ToInt(0) > 0)
                        {
                            PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công", "s", true, Page);
                        }
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