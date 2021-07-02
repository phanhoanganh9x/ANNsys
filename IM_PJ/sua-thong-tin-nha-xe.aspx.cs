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
    public partial class sua_thong_tin_nha_xe : System.Web.UI.Page
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
                        if (acc.RoleID == 1)
                        {
                            Response.Redirect("/trang-chu");
                        }

                        var ID = Request.QueryString["ID"];

                        if (ID != null)
                        {
                            LoadData(Convert.ToInt32(ID));
                        }
                        else
                        {
                            Response.Redirect("/danh-sach-nha-xe");
                        }
                    }
                }
                else
                {

                    Response.Redirect("/dang-nhap");
                }
            }
        }

        /// <summary>
        /// Setting init when load page
        /// </summary>
        private void LoadData(int ID)
        {
            var company = TransportCompanyController.GetAllTransportCompanyByID(ID);

            if (company == null)
            {
                PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy nhà xe " + ID, "e", true, "/danh-sach-nha-xe", Page);
            }
            else
            {
                this.hdfID.Value = ID.ToString();
                this.txtCompanyName.Text = company.CompanyName;
                this.txtCompanyPhone.Text = company.CompanyPhone;
                this.txtCompanyAddress.Text = company.CompanyAddress;
                this.rdbPrepay.SelectedValue = company.Prepay ? "true" : "false";
                this.rdbCOD.SelectedValue = company.COD ? "true" : "false";
                this.pNote.Text = company.Note;
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;

            tbl_TransportCompany transportCompany = new tbl_TransportCompany();

            transportCompany.ID = Convert.ToInt32(this.hdfID.Value);
            transportCompany.CompanyName = this.txtCompanyName.Text;
            transportCompany.CompanyPhone = this.txtCompanyPhone.Text;
            transportCompany.CompanyAddress = this.txtCompanyAddress.Text;
            transportCompany.Prepay = Convert.ToBoolean(this.rdbPrepay.SelectedValue);
            transportCompany.COD = Convert.ToBoolean(this.rdbCOD.SelectedValue);
            transportCompany.Note = this.pNote.Text;
            transportCompany.ModifiedBy = username;

            int ID = TransportCompanyController.UpdateTransportCompany(transportCompany);

            Response.Redirect(String.Format("/chi-tiet-nha-xe?id={0}", this.hdfID.Value));
        }
    }
}