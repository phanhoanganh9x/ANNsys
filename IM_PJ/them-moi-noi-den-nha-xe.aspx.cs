﻿using IM_PJ.Controllers;
using IM_PJ.Models;
using System;
using System.Web.UI;

namespace IM_PJ
{
    public partial class them_moi_noi_den_nha_xe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["userLoginSystem"] != null)
                {
                    string username = Request.Cookies["userLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);

                    if (acc != null)
                    {
                        if (acc.RoleID == 1)
                        {
                            Response.Redirect("/trang-chu");
                        }

                        // Check mode of page
                        var ID = Request.QueryString["ID"];

                        if (ID != null)
                        {
                            Initialize(Convert.ToInt32(ID));
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
        private void Initialize(int ID)
        {
            // Init value
            var transportCompany = TransportCompanyController.GetTransportCompanyByID(ID);

            if (transportCompany != null)
            {
                this.hdfID.Value = transportCompany.ID.ToString();
                this.txtCompanyName.Text = transportCompany.CompanyName;
                this.txtCompanyPhone.Text = transportCompany.CompanyPhone;
                this.txtCompanyAddress.Text = transportCompany.CompanyAddress;
            }
            else
            {
                Response.Redirect("/danh-sach-nha-xe");
            }

            // Setting display
            this.txtShipTo.Focus();

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["userLoginSystem"].Value;

            tbl_TransportCompany receivePlaceNew = new tbl_TransportCompany();

            receivePlaceNew.ID = Convert.ToInt32(this.hdfID.Value);
            receivePlaceNew.CompanyName = this.txtCompanyName.Text;
            receivePlaceNew.CompanyPhone = this.txtCompanyPhone.Text;
            receivePlaceNew.CompanyAddress = this.txtCompanyAddress.Text;
            receivePlaceNew.ShipTo = this.txtShipTo.Text;
            receivePlaceNew.Address = this.txtAddress.Text;
            receivePlaceNew.Prepay = Convert.ToBoolean(this.rdbPrepay.SelectedValue);
            receivePlaceNew.COD = Convert.ToBoolean(this.rdbCOD.SelectedValue);
            receivePlaceNew.Note = this.pNote.Text;
            receivePlaceNew.CreatedBy = username;

            TransportCompanyController.InsertReceivePlace(receivePlaceNew);

            Response.Redirect(String.Format("/chi-tiet-nha-xe/?id={0}", this.hdfID.Value));
        }
    }
}