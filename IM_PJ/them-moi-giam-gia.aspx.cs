using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using Newtonsoft.Json;
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
    public partial class them_moi_giam_gia : System.Web.UI.Page
    {
        private tbl_Account acc;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                    acc = AccountController.GetByUsername(username);

                    if (!AccountController.isPermittedLoading(acc, "them-moi-giam-gia"))
                        Response.Redirect("/trang-chu");
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }

                LoadData();
            }
        }

        private void LoadAccount()
        {
            var accounts = AccountController.getDropDownList();

            if (acc != null)
                accounts = accounts
                    .Where(x => x.Value != acc.ID.ToString())
                    .ToList();

            accounts[0].Text = "Chọn nhân viên để cấp quyền truy cập nhóm này";
            ddlAccount.Items.Clear();
            ddlAccount.Items.AddRange(accounts.ToArray());
            ddlAccount.DataBind();
        }

        private void LoadData()
        {
            // Init drop down list accout
            LoadAccount();
            // Create tag Owner
            var owner = new { value = acc.ID, text = acc.Username };
            hdfPermittedRead.Value = owner.value.ToString();
            var script = "$(function () { addOwner(" + JsonConvert.SerializeObject(owner) + ") });";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", script, true);
        }

        protected void btnCreateDiscountGroup_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0)
                {
                    var now = DateTime.Now;
                    var data = new tbl_DiscountGroup()
                    {
                        DiscountName = txtDiscountName.Text,
                        DiscountAmount = pDiscountAmount.Value.HasValue ? pDiscountAmount.Value.Value : 0,
                        DiscountAmountPercent = 0,
                        QuantityProduct = pQuantityProduct.Value.HasValue ? Convert.ToInt32(pQuantityProduct.Value.Value) : 0,
                        FeeRefund = rRefundGoods.Value.HasValue ? rRefundGoods.Value.Value : 0,
                        NumOfDateToChangeProduct = pNumOfDateToChangeProduct.Value.HasValue ? pNumOfDateToChangeProduct.Value.Value : 0,
                        NumOfProductCanChange = pNumOfProductCanChange.Value.HasValue ? pNumOfProductCanChange.Value.Value : 0,
                        RefundQuantityNoFee = pRefundQuantityNoFee.Value.HasValue ? Convert.ToInt32(pRefundQuantityNoFee.Value.Value) : 0,
                        DiscountNote = pDiscountNote.Content,
                        IsHidden = chkIsHidden.Checked,
                        CreatedBy = username,
                        CreatedDate = now,
                        ModifiedBy = username,
                        ModifiedDate = now,
                        PermittedRead = hdfPermittedRead.Value,
                        QuantityRequired = pQuantityRequired.Value.HasValue ? Convert.ToInt32(pQuantityRequired.Value.Value) : 0
                    };

                    DiscountGroupController.Insert(data);

                    Response.Redirect("/danh-sach-nhom-khach-hang.aspx");
                    PJUtils.ShowMessageBoxSwAlert("Tạo mới thành công", "s", true, Page);
                }
            }
        }
    }
}