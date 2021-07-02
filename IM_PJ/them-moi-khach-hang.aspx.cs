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
using Telerik.Web.UI;

namespace IM_PJ
{
    public partial class them_moi_khach_hang : System.Web.UI.Page
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
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
                LoadProvince();
                LoadTransportCompany();
                LoadTransportCompanySubID();
            }
        }

        public void LoadProvince()
        {
            var pro = ProvinceController.GetAll();
            ddlProvince.Items.Clear();
            ddlProvince.Items.Insert(0, new ListItem("Chọn tỉnh thành", "0"));
            if (pro.Count > 0)
            {
                foreach (var p in pro)
                {
                    ListItem listitem = new ListItem(p.Name, p.ID.ToString());
                    ddlProvince.Items.Add(listitem);
                }
                ddlProvince.DataBind();
            }
        }

        public void LoadTransportCompany()
        {
            var TransportCompany = TransportCompanyController.GetTransportCompany();
            ddlTransportCompanyID.Items.Clear();
            ddlTransportCompanyID.Items.Insert(0, new ListItem("Chọn chành xe", "0"));
            if (TransportCompany.Count > 0)
            {
                foreach (var p in TransportCompany)
                {
                    ListItem listitem = new ListItem(p.CompanyName, p.ID.ToString());
                    ddlTransportCompanyID.Items.Add(listitem);
                }
                ddlTransportCompanyID.DataBind();
            }
        }

        public void LoadTransportCompanySubID(int ID = 0)
        {
            ddlTransportCompanySubID.Items.Clear();
            ddlTransportCompanySubID.Items.Insert(0, new ListItem("Chọn nơi nhận", "0"));
            if (ID > 0)
            {
                var ShipTo = TransportCompanyController.GetReceivePlace(ID); ;

                if (ShipTo.Count > 0)
                {
                    foreach (var p in ShipTo)
                    {
                        ListItem listitem = new ListItem(p.ShipTo, p.SubID.ToString());
                        ddlTransportCompanySubID.Items.Add(listitem);
                    }
                }
                ddlTransportCompanySubID.DataBind();
            }
        }

        protected void ddlTransportCompanyID_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTransportCompanySubID(ddlTransportCompanyID.SelectedValue.ToInt(0));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID != 1)
                {
                    string phone = txtCustomerPhone.Text.Trim().Replace(" ", "");
                    var checkPhone = CustomerController.GetByPhone(phone);
                    if (checkPhone != null)
                    {
                        lblError.Text = "Số điện thoại đã tồn tại";
                        lblError.Visible = true;
                    }
                    else
                    {
                        lblError.Visible = false;

                        //Phần thêm ảnh đại diện khách hàng
                        string path = "/uploads/avatars/";
                        string Avatar = "";
                        if (UploadAvatarImage.UploadedFiles.Count > 0)
                        {
                            foreach (UploadedFile f in UploadAvatarImage.UploadedFiles)
                            {
                                var o = path + Guid.NewGuid() + f.GetExtension();
                                try
                                {
                                    f.SaveAs(Server.MapPath(o));
                                    Avatar = o;
                                }
                                catch { }
                            }
                        }

                        int PaymentType = ddlPaymentType.SelectedValue.ToInt(0);
                        int ShippingType = ddlShippingType.SelectedValue.ToInt(0);

                        int TransportCompanyID = 0;
                        int TransportCompanySubID = 0;
                        if (ddlTransportCompanyID.SelectedValue != "0")
                        {
                            TransportCompanyID = ddlTransportCompanyID.SelectedValue.ToInt();
                        }
                        if(ddlTransportCompanySubID.SelectedValue != "0")
                        {
                            TransportCompanySubID = ddlTransportCompanySubID.SelectedValue.ToInt();
                        }
                        

                        CustomerController.Insert(txtCustomerName.Text, phone, txtAddress.Text, "", 0, 1, DateTime.Now, username, false, txtZalo.Text, txtFacebook.Text, txtNote.Text, txtNick.Text, Avatar, ShippingType, PaymentType, TransportCompanyID, TransportCompanySubID, txtCustomerPhone2.Text);

                        PJUtils.ShowMessageBoxSwAlert("Tạo khách hàng thành công", "s", true, Page);
                    }
                }
            }
        }
    }
}