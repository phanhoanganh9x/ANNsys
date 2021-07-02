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
using System.IO;

namespace IM_PJ
{
    public partial class chi_tiet_khach_hang : System.Web.UI.Page
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
                        if (acc.RoleID != 0 && acc.RoleID != 2)
                        {
                            Response.Redirect("/dang-nhap");
                        }
                    }
                }
                else
                {

                    Response.Redirect("/dang-nhap");
                }
                LoadTransportCompany();
                LoadData();
                LoadDLL();
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

        public void LoadDLL()
        {
            var agent = AccountController.GetAllNotSearch();
            ddlUser.Items.Clear();
            ddlUser.Items.Insert(0, new ListItem("Chọn nhân viên phụ trách", "0"));
            if (agent.Count > 0)
            {
                foreach (var p in agent)
                {
                    ListItem listitem = new ListItem(p.Username, p.Username);
                    ddlUser.Items.Add(listitem);
                }
                ddlUser.DataBind();
            }
        }

        public void LoadData()
        {
            int id = Request.QueryString["id"].ToInt(0);
            if (id > 0)
            {
                var d = CustomerController.GetByID(id);
                if (d == null)
                {
                    PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy khách hàng " + id, "e", true, "/danh-sach-khach-hang", Page);
                }
                else
                {
                    this.Title = String.Format("{0} - Khách hàng", d.CustomerName.ToTitleCase());

                    string username = HttpContext.Current.Request.Cookies["usernameLoginSystem_ANN123"].Value;
                    var acc = AccountController.GetByUsername(username);
                    if (acc.RoleID != 0)
                    {
                        if (d.CreatedBy != acc.Username)
                        {
                            Response.Redirect("/danh-sach-khach-hang");
                        }
                    }

                    ViewState["ID"] = id;
                    txtCustomerName.Text = d.CustomerName;
                    txtCustomerPhone.Text = d.CustomerPhone;
                    txtCustomerPhone2.Text = d.CustomerPhone2;
                    txtCustomerPhoneBackup.Text = d.CustomerPhoneBackup;
                    txtAddress.Text = d.CustomerAddress;
                    txtNick.Text = d.Nick;
                    chkIsHidden.Checked = Convert.ToBoolean(d.IsHidden);
                    txtZalo.Text = d.Zalo;
                    txtFacebook.Text = d.Facebook;
                    txtNote.Text = d.Note;
                    if (!string.IsNullOrEmpty(d.ProvinceID.ToString()))
                        ddlProvince.SelectedValue = d.ProvinceID.ToString();

                    LoadTransportCompanySubID(Convert.ToInt32(d.TransportCompanyID));
                    ddlPaymentType.SelectedValue = d.PaymentType.ToString();
                    ddlShippingType.SelectedValue = d.ShippingType.ToString();
                    ddlTransportCompanyID.SelectedValue = d.TransportCompanyID.ToString();
                    ddlTransportCompanySubID.SelectedValue = d.TransportCompanySubID.ToString();

                    AvatarThumbnail.ImageUrl = d.Avatar;
                    ListAvatarImage.Value = d.Avatar;

                    ddlUser.SelectedValue = d.CreatedBy;

                    hdfProvinceID.Value = d.ProvinceID.ToString();
                    hdfDistrictID.Value = d.DistrictId.ToString();
                    hdfWardID.Value = d.WardId.ToString();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;

            int id = ViewState["ID"].ToString().ToInt(0);
            if (id > 0)
            {
                var d = CustomerController.GetByID(id);
                if (d != null)
                {
                    //Phần thêm ảnh đại diện khách hàng
                    string path = "/uploads/avatars/";
                    string Avatar = ListAvatarImage.Value;
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

                    if (Avatar != ListAvatarImage.Value)
                    {
                        if (File.Exists(Server.MapPath(ListAvatarImage.Value)))
                        {
                            File.Delete(Server.MapPath(ListAvatarImage.Value));
                        }
                    }

                    int PaymentType = ddlPaymentType.SelectedValue.ToInt(0);
                    int ShippingType = ddlShippingType.SelectedValue.ToInt(0);
                    int TransportCompanyID = ddlTransportCompanyID.SelectedValue.ToInt(0);
                    int TransportCompanySubID = ddlTransportCompanySubID.SelectedValue.ToInt(0);
                    string note = txtNote.Text;
                    int ProvinceID = hdfProvinceID.Value.ToInt(0);
                    int DistrictID = hdfDistrictID.Value.ToInt(0);
                    int WardID = hdfWardID.Value.ToInt(0);

                    string warning = "Cập nhật khách hàng thành công";

                    string CustomerPhone = d.CustomerPhone;
                    string NewCustomerPhone = Regex.Replace(txtCustomerPhone.Text.Trim(), @"[^\d]", "");
                    // kiểm tra số điện thoại mới
                    if (NewCustomerPhone != d.CustomerPhone)
                    {
                        // kiểm tra số điện thoại mới có khả dụng ko?
                        var c = CustomerController.GetByPhone(NewCustomerPhone);
                        if (c != null && c.ID != d.ID)
                        {
                            warning = "Số điện thoại này đã tồn tại của khách khác!";
                        }
                        else
                        {
                            warning = "Cập nhật khách hàng thành công! Số điện thoại khách hàng đã được đổi.<br>Lưu ý: Các đơn hàng cũ của khách này cũng đã được đổi số điện thoại.";
                            note = "Số điện thoại cũ: " + d.CustomerPhone + ". " + note;

                            CustomerPhone = NewCustomerPhone;

                            // đổi số mới cho đơn hàng cũ
                            var orders = OrderController.GetByCustomerID(d.ID);
                            foreach (var order in orders)
                            {
                                string update = OrderController.UpdateCustomerPhone(order.ID, CustomerPhone);
                            }

                            // đổi số mới cho đơn hàng đổi trả cũ
                            var refundorders = RefundGoodController.GetByCustomerID(d.ID);
                            foreach (var refundorder in refundorders)
                            {
                                string update = RefundGoodController.UpdateCustomerPhone(refundorder.ID, CustomerPhone);
                            }
                        }
                    }

                    string CustomerPhone2 = "";
                    string NewCustomerPhone2 = Regex.Replace(txtCustomerPhone2.Text.Trim(), @"[^\d]", "");
                    if (NewCustomerPhone2 != "")
                    {
                        // kiểm tra số điện thoại 2 mới
                        var b = CustomerController.GetByPhone(NewCustomerPhone2);
                        if (b == null)
                        {
                            CustomerPhone2 = NewCustomerPhone2;
                        }
                        else
                        {
                            warning = "Số điện thoại 2 đã tồn tại của khách khác!";
                        }
                    }

                    CustomerController.Update(id, txtCustomerName.Text, CustomerPhone, txtAddress.Text, "", 0, 1, ddlUser.SelectedItem.ToString(), DateTime.Now, username, chkIsHidden.Checked, Regex.Replace(txtZalo.Text.Trim(), @"[^\d]", ""), txtFacebook.Text, note, txtNick.Text, Avatar, ShippingType, PaymentType, TransportCompanyID, TransportCompanySubID, CustomerPhone2, ProvinceID, DistrictID, WardID);
                    PJUtils.ShowMessageBoxSwAlert(warning, "s", true, Page);
                }
            }
        }
    }
}