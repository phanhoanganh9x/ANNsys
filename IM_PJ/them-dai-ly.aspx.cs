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
    public partial class them_dai_ly : System.Web.UI.Page
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

                        string agentAPIID = "Agent-" + currentDate.Year + currentDate.Month + currentDate.Day
                                                + currentDate.Hour + currentDate.Minute + currentDate.Second
                                                + currentDate.Millisecond;
                        string agentAPICode = PJUtils.RandomStringWithText(12);

                        string kq = AgentController.Insert(txtAgentName.Text, "", txtAddress.Text, txtPhone.Text, txtEmail.Text, txtAgentLeader.Text,
                            chkIsHidden.Checked, agentAPIID, agentAPICode, currentDate, username, "");

                        if (kq.ToInt(0) > 0)
                        {
                            PJUtils.ShowMessageBoxSwAlert("Tạo mới thành công", "s", true, Page);
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