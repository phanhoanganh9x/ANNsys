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
    public partial class them_gia_tri_thuoc_tinh : System.Web.UI.Page
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
                LoadDDLVariable();
                LoadData();
            }
        }
        public void LoadDDLVariable()
        {
            var listVariable = VariableController.GetAllIsHidden(false);
            if (listVariable.Count > 0)
            {
                ddlVariable.Items.Clear();
                ddlVariable.Items.Insert(0, new ListItem("-- Chọn --", "0"));
                foreach (var p in listVariable)
                {
                    ListItem listitem = new ListItem(p.VariableName, p.ID.ToString());
                    ddlVariable.Items.Add(listitem);
                }
                ddlVariable.DataBind();
            }

        }
        void BindValue(int variableID)
        {
            ddlVariableValue.Items.Clear();
            ddlVariableValue.Items.Insert(0, new ListItem("-- Chọn --", "0"));
            if (variableID > 0)
            {
                ddlVariableValue.DataSource = VariableValueController.GetByVariableIDIsHidden(variableID, false);
                ddlVariableValue.DataBind();
            }
        }
        protected void ddlVariable_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindValue(ddlVariable.SelectedValue.ToInt());
        }
        public void LoadData()
        {

            int productvariableid = Request.QueryString["productvariableid"].ToInt(0);
            if (productvariableid > 0)
            {
                var pv = ProductVariableController.GetByID(productvariableid);
                if (pv != null)
                {
                    ViewState["productvariableid"] = productvariableid;
                    ViewState["ProductvariableSKU"] = pv.SKU;
                    ltrBack.Text = "<a href=\"/gia-tri-thuoc-tinh-san-pham?productvariableid=" + productvariableid + "\" class=\"btn primary-btn fw-btn not-fullwidth\">Trở về</a>";
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            int productvariableid = ViewState["productvariableid"].ToString().ToInt(0);
            string SKU = ViewState["ProductvariableSKU"].ToString();
            int VariableValueID = ddlVariableValue.SelectedValue.ToInt();
            string VariableName = ddlVariable.SelectedItem.ToString();
            string VariableValue = ddlVariableValue.SelectedItem.ToString();
            bool isHidden = chkIsHidden.Checked;
            ProductVariableValueController.Insert(productvariableid, SKU, VariableValueID, VariableName, VariableValue, isHidden,
                currentDate, username);
            PJUtils.ShowMessageBoxSwAlert("Thêm giá trị thành công", "s", true, Page);
        }
    }
}