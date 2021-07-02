using IM_PJ.Controllers;
using IM_PJ.Models;
using System;
using System.Web.UI;

namespace IM_PJ
{
    public partial class chi_tiet_noi_den_nha_xe : System.Web.UI.Page
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
                        var SubID = Request.QueryString["SubID"];

                        if (ID != null && SubID != null)
                        {
                            Initialize(Convert.ToInt32(ID), Convert.ToInt32(SubID));
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
        /// <param name="ID"></param>
        private void Initialize(int ID, int SubID)
        {
            // Init value
            var transportCompany = TransportCompanyController.GetAllReceivePlaceByID(ID, SubID);

            if (transportCompany != null)
            {
                this.hdfID.Value = transportCompany.ID.ToString();
                this.hdfSubID.Value = transportCompany.SubID.ToString();
                this.txtCompanyName.Text = transportCompany.CompanyName;
                this.txtCompanyPhone.Text = transportCompany.CompanyPhone;
                this.txtCompanyAddress.Text = transportCompany.CompanyAddress;
                this.txtShipTo.Text = transportCompany.ShipTo;
                this.txtAddress.Text = transportCompany.Address;
                this.rdbPrepay.SelectedValue = transportCompany.Prepay? "true" : "false";
                this.rdbCOD.SelectedValue = transportCompany.COD ? "true" : "false";
                this.pNote.Text = transportCompany.Note;
            }
            else {
                Response.Redirect("/danh-sach-nha-xe");
            }

            // Setting display
            this.txtShipTo.Focus();

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;

            tbl_TransportCompany receivePlaceUpdate = new tbl_TransportCompany();

            receivePlaceUpdate.ID = Convert.ToInt32(this.hdfID.Value);
            receivePlaceUpdate.SubID = Convert.ToInt32(this.hdfSubID.Value);
            receivePlaceUpdate.ShipTo = this.txtShipTo.Text;
            receivePlaceUpdate.Address = this.txtAddress.Text;
            receivePlaceUpdate.Prepay = Convert.ToBoolean(this.rdbPrepay.SelectedValue);
            receivePlaceUpdate.COD = Convert.ToBoolean(this.rdbCOD.SelectedValue);
            receivePlaceUpdate.Note = this.pNote.Text;
            receivePlaceUpdate.ModifiedBy = username;

            TransportCompanyController.UpdateReceivePlace(receivePlaceUpdate);

            Response.Redirect(String.Format("/chi-tiet-nha-xe/?id={0}", this.hdfID.Value));
        }
    }
}