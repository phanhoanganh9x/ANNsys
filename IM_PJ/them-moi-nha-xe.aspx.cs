using IM_PJ.Controllers;
using IM_PJ.Models;
using System;
using System.Web.UI;

namespace IM_PJ
{
    public partial class them_moi_nha_xe : System.Web.UI.Page
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

                        // Check mode of page
                        Initialize();

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
        /// <param name="ID"></param>
        private void Initialize()
        {
            // Setting display
            this.txtCompanyName.Focus();

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;

            tbl_TransportCompany transportCompanyNew = new tbl_TransportCompany();

            transportCompanyNew.CompanyName = this.txtCompanyName.Text;
            transportCompanyNew.CompanyPhone = this.txtCompanyPhone.Text;
            transportCompanyNew.CompanyAddress = this.txtCompanyAddress.Text;
            transportCompanyNew.Prepay = Convert.ToBoolean(this.rdbPrepay.SelectedValue);
            transportCompanyNew.COD = Convert.ToBoolean(this.rdbCOD.SelectedValue);
            transportCompanyNew.Note = this.pNote.Text;
            transportCompanyNew.CreatedBy = username;
            transportCompanyNew.Status = 1;

            int ID = TransportCompanyController.InsertTransportCompany(transportCompanyNew);

            Response.Redirect(String.Format("/chi-tiet-nha-xe?id={0}", ID));
        }
    }
}