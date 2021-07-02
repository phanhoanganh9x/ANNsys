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
    public partial class them_moi_thuoc_tinh : System.Web.UI.Page
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
            string s = "";
            if (Request.QueryString["s"] != null)
            {
                s = Request.QueryString["s"];
            }
            
            int ID = Request.QueryString["id"].ToInt(0);
            ViewState["ID"] = ID;
            ltrBack.Text = "<a href=\"/quan-ly-thuoc-tinh-san-pham?id=" + ID + "\" class=\"btn primary-btn fw-btn not-fullwidth\">Trở về</a>";
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            int id = Convert.ToInt32(ViewState["ID"]);
            if (id != 0)
            {
                var variname = VariableController.GetByID(id);
                if (variname != null)
                {
                    var acc = AccountController.GetByUsername(username);
                    if (acc != null)
                    {
                        if (acc.RoleID == 0)
                        {

                            int ID = id;
                            string variableName = variname.VariableName.Trim();                                
                            string VariableValue = txtVariableValue.Text.Trim();
                            string SKUText = txtSKUText.Text.Trim();
                            var check = VariableValueController.GetByValueAndSKUText(id,VariableValue, SKUText);
                            if(check != null)
                            {
                                lblError.Text = "Tên thuộc tính hoặc SKUText đã tồn tại vui lòng chọn tên khác.";
                                lblError.Visible = true;
                            }
                            else
                            {
                                lblError.Visible = false;
                                VariableValueController.Insert(ID, variableName, VariableValue, false, currentDate, username, SKUText);
                                PJUtils.ShowMessageBoxSwAlert("Thêm thuộc tính thành công", "s", true, Page);
                                Response.Redirect("quan-ly-thuoc-tinh-san-pham?id=" + id);
                            }
                                
                        }
                    }
                }
            }
        }
    }
}
