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
    public partial class thong_tin_nhan_vien : System.Web.UI.Page
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
            }
        }
        public void LoadData()
        {
            int UID = Request.QueryString["uid"].ToInt(0);
            if (UID > 0)
            {
                var acc = AccountController.GetByID(UID);
                if (acc != null)
                {
                    int agentID = Convert.ToInt32(acc.AgentID);
                    ViewState["UID"] = UID;
                    ViewState["agentid"] = agentID;

                    lblUsername.Text = acc.Username;
                    lblEmail.Text = acc.Email;
                    ddlRole.SelectedValue = acc.RoleID.ToString();
                    ddlStatus.SelectedValue = acc.Status.ToString();
                    var ai = AccountInfoController.GetByUserID(UID);
                    if (ai != null)
                    {
                        txtFullname.Text = ai.Fullname;
                        txtAddress.Text = ai.Address;
                        txtPhone.Text = ai.Phone;
                        ddlGender.SelectedValue = ai.Gender.ToString();
                        if (ai.Birthday != null)
                            rBirthday.SelectedDate = ai.Birthday;
                        txtNote.Content = ai.Note;
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            int agentID = Convert.ToInt32(ViewState["agentid"]);
            int UID = Convert.ToInt32(ViewState["UID"]);
            string username_current = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            string pass = txtPassword.Text.Trim();
            int Status = ddlStatus.SelectedValue.ToString().ToInt();
            int RoleID = ddlRole.SelectedValue.ToString().ToInt();
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
                //else if (acc.AgentID == agentID && acc.RoleID == 1)
                //{
                //    ischeck = true;
                //    username_current_role = 1;
                //}
            }
            if (ischeck == true)
            {
                int roleID = 2;
                if (username_current_role == 0)
                    roleID = Convert.ToInt32(ddlRole.SelectedValue);

                if (!string.IsNullOrEmpty(pass))
                {
                    string confirmpass = txtConfirmPassword.Text;
                    if (!string.IsNullOrEmpty(confirmpass))
                    {
                        if (confirmpass == pass)
                        {
                            AccountController.updatestatus(UID, Status, currentDate, username_current);
                            AccountController.UpdateRole(UID, roleID, currentDate, username_current);
                            string rp = AccountController.UpdatePassword(UID, pass);

                            string r = AccountInfoController.Update(UID, txtFullname.Text.Trim(), ddlGender.SelectedValue.ToInt(),
                                Convert.ToDateTime(rBirthday.SelectedDate), lblEmail.Text, txtPhone.Text, txtAddress.Text.Trim(),
                                currentDate, username_current, txtNote.Text);
                            if (r == "1" && rp == "1")
                            {
                                PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công", "s", true, Page);
                            }
                            else if (r == "1" && rp == "0")
                            {
                                lblError.Text = "Mật khẩu mới trùng với mật khẩu cũ.";
                                lblError.Visible = true;
                            }
                            else
                            {
                                PJUtils.ShowMsg("Có lỗi trong quá trình cập nhật", true, Page);
                            }
                        }
                        else
                        {
                            lblError.Text = "Xác nhận mật khẩu không trùng với mật khẩu.";
                            lblError.Visible = true;
                        }
                    }
                    else
                    {
                        lblError.Text = "Không để trống xác nhận mật khẩu";
                        lblError.Visible = true;
                    }
                }
                else
                {
                    AccountController.updatestatus(UID, Status, currentDate, username_current);
                    AccountController.UpdateRole(UID, roleID, currentDate, username_current);
                    string r = AccountInfoController.Update(UID, txtFullname.Text.Trim(), ddlGender.SelectedValue.ToInt(),
                        Convert.ToDateTime(rBirthday.SelectedDate), lblEmail.Text, txtPhone.Text, txtAddress.Text.Trim(), currentDate, username_current, txtNote.Text);
                    if (r == "1")
                    {
                        PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công", "s", true, Page);
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình cập nhật", "e", true, Page);
                    }
                }
            }
        }
    }
}