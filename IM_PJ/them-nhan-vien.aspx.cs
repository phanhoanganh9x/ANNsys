﻿using IM_PJ.Controllers;
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
    public partial class them_nhan_vien : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["userLoginSystem"] != null)
                {
                    int agentID = Request.QueryString["agentid"].ToInt(0);
                    string username = Request.Cookies["userLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);
                    if (acc != null)
                    {
                        if (acc.RoleID == 0)
                        {
                            pnAdmin.Visible = true;
                        }
                        //else if (acc.AgentID == agentID && acc.RoleID == 1)
                        //{

                        //}
                        else
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
            int agentID = Request.QueryString["agentid"].ToInt(0);
            ltrBack.Text = "<a href=\"/quan-ly-nhan-vien?agentid=" + agentID + "\" class=\"btn primary-btn fw-btn not-fullwidth\">Trở về</a>";
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            int agentID = Request.QueryString["agentid"].ToInt(0);
            string username_current = Request.Cookies["userLoginSystem"].Value;
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
                                PJUtils.ShowMessageBoxSwAlert("Tạo mới thành công", "s", true, Page);
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