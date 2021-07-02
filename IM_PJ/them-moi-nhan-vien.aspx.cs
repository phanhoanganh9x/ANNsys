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
    public partial class them_moi_nhan_vien : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
                {
                    int agentID = Request.QueryString["agentid"].ToInt(0);
                    string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                    var acc = AccountController.GetByUsername(username);
                    if (acc != null)
                    {
                        if (acc.RoleID == 0)
                        {
                            pnAdmin.Visible = true;
                        }
                        else
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
                LoadAgent();
            }
        }
        public void LoadData()
        {
            int agentID = Request.QueryString["agentid"].ToInt(0);
        }

        public void LoadAgent()
        {
            var agent = AgentController.GetAll("");
            ddlAgent.Items.Clear();
            ddlAgent.Items.Insert(0, new ListItem("Chọn chi nhánh", "0"));
            if (agent.Count > 0)
            {
                foreach (var p in agent)
                {
                    ListItem listitem = new ListItem(p.AgentName, p.ID.ToString());
                    ddlAgent.Items.Add(listitem);
                }
                ddlAgent.DataBind();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            int agentID = ddlAgent.SelectedValue.ToInt();
            string username_current = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            bool ischeck = false;
            int username_current_role = 0;
            var acc = AccountController.GetByUsername(username_current);
            if (acc != null)
            {
                if (acc.RoleID == 0)
                {
                    ischeck = true;
                    username_current_role = 0;
                }
                else if (acc.AgentID == agentID && acc.RoleID == 1)
                {
                    ischeck = true;
                    username_current_role = 1;
                }
            }
            if (ischeck == true)
            {
                string Username = txtUsername.Text.Trim().ToLower();
                string Email = txtEmail.Text.Trim();
                var checkuser = AccountController.GetByUsername(Username);
                var checkemail = AccountController.GetByEmail(Email);
                var getaccountinfor = AccountInfoController.GetByPhone(txtPhone.Text.Trim());
                bool checkusernamebool = false;
                bool checkemailbool = false;
                bool checkphonebool = false;
                string error = "";
                bool check = PJUtils.CheckUnicode(Username);
                if (Username.Contains(" "))
                {
                    lblError.Visible = true;
                    lblError.Text = "Tên đăng nhập không được có dấu cách.";
                }
                else if (check == true)
                {
                    lblError.Visible = true;
                    lblError.Text = "Tên đăng nhập không được có dấu tiếng Việt.";
                }
                else
                {
                    if (checkuser != null)
                    {
                        //lblcheckemail.Visible = true;
                        error += "Tên đăng nhập đã được sử dụng vui lòng chọn Tên đăng nhập / Nickname khác.<br/>";
                        checkusernamebool = true;
                    }
                    if (checkemail != null)
                    {
                        //lblcheckemail.Visible = true;
                        error += "Email đã được sử dụng vui lòng chọn Email khác.<br/>";
                        checkemailbool = true;
                    }
                    if (getaccountinfor != null)
                    {
                        //lblcheckemail.Visible = true;
                        error += "Số điện thoại đã được sử dụng vui lòng chọn Số điện thoại khác.<br/>";
                        checkphonebool = true;
                    }
                    if (checkusernamebool == false && checkemailbool == false && checkphonebool == false)
                    {
                        int roleID = 2;
                        if (username_current_role == 0)
                            roleID = Convert.ToInt32(ddlRole.SelectedValue);

                        int id = AccountController.Insert(agentID, Username, Email, txtPassword.Text.Trim(), roleID, Convert.ToInt32(ddlStatus.SelectedValue),
                            currentDate, username_current);
                        if (id > 0)
                        {
                            int UID = id;
                            string idai = AccountInfoController.Insert(UID, txtFullname.Text.Trim(), Convert.ToInt32(ddlGender.SelectedValue),
                               Convert.ToDateTime(rBirthday.SelectedDate), Email, txtPhone.Text.Trim(), txtAddress.Text, currentDate, username_current);
                            if (idai == "1")
                            {
                                PJUtils.ShowMessageBoxSwAlert("Tạo mới nhân viên thành công", "s", true, Page);
                            }
                        }
                    }
                    else
                    {
                        lblError.Text = error;
                        lblError.Visible = true;
                    }
                }
            }
        }
    }
}