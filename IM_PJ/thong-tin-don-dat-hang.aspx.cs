using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Utils;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class thong_tin_don_dat_hang : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem"] != null)
                {
                    var username = Request.Cookies["usernameLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);

                    if (acc != null)
                    {
                        if (acc.RoleID == 0 || acc.RoleID == 2)
                            _initPage(acc);
                        else
                            Response.Redirect("/trang-chu");
                    }
                    else
                        Response.Redirect("/dang-nhap");
                }
                else
                    Response.Redirect("/dang-nhap");
            }
        }

        #region Private
        /// <summary>
        /// Cài đặt bộ lọc nhân viên phụ trách
        /// </summary>
        /// <param name="acc"></param>
        private void _initCreatedBy(tbl_Account acc = null)
        {
            hdRole.Value = acc == null ? "0" : acc.RoleID.ToString();

            if (acc != null)
            {
                ddlCreatedBy.Enabled = false;
                ddlCreatedBy.Items.Clear();
                ddlCreatedBy.Items.Insert(0, new ListItem(acc.Username, acc.Username));
            }
            else
            {
                var items = AccountController
                    .GetAllNotSearch()
                    .Where(x => x.RoleID == 0 || x.RoleID == 2)
                    .Select(x => new ListItem(x.Username, x.Username))
                    .ToArray();

                ddlCreatedBy.Enabled = true;
                ddlCreatedBy.Items.Clear();
                ddlCreatedBy.Items.Insert(0, new ListItem("Nhân viên tạo đơn", ""));
                ddlCreatedBy.Items.AddRange(items);
                ddlCreatedBy.DataBind();


                var staff = Request.QueryString["staff"];
                if (!String.IsNullOrEmpty(staff))
                    for (int i = 0; i < ddlCreatedBy.Items.Count; i++)
                        if (ddlCreatedBy.Items[i].Value == staff)
                            ddlCreatedBy.SelectedIndex = i;
            }
        }

        private void _initPage(tbl_Account acc)
        {
            _initCreatedBy(acc.RoleID == 2 ? acc : null);
        }
        #endregion

        #region Public
        public void LoadCreatedBy(tbl_Account acc = null)
        {
            if (acc != null)
            {
                ddlCreatedBy.Items.Clear();
                ddlCreatedBy.Items.Insert(0, new ListItem(acc.Username, acc.Username));
            }
            else
            {
                var CreateBy = AccountController.GetAllNotSearch();
                ddlCreatedBy.Items.Clear();
                if (CreateBy.Count > 0)
                {
                    foreach (var p in CreateBy)
                    {
                        if (p.RoleID == 2 || p.RoleID == 0)
                        {
                            ListItem listitem = new ListItem(p.Username, p.Username);
                            ddlCreatedBy.Items.Add(listitem);
                        }
                    }
                    ddlCreatedBy.DataBind();
                }
            }
        }
        #endregion
    }
}