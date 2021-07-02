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
    public partial class chi_tiet_giam_gia : System.Web.UI.Page
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
                    if (!AccountController.isPermittedLoading(acc, "chi-tiet-giam-gia"))
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

        public void LoadData()
        {
            LoadAccount();

            int id = Request.QueryString["id"].ToInt(0);
            if (id > 0)
            {
                var discountGroup = DiscountGroupController.GetByID(id);
                if (discountGroup != null)
                {
                    ViewState["ID"] = id;
                    txtDiscountName.Text = discountGroup.DiscountName;
                    pQuantityRequired.Value = discountGroup.QuantityRequired;
                    pDiscountAmount.Value = discountGroup.DiscountAmount;
                    pQuantityProduct.Value = discountGroup.QuantityProduct;
                    rRefundGoods.Value = discountGroup.FeeRefund;
                    pNumOfDateToChangeProduct.Value = discountGroup.NumOfDateToChangeProduct;
                    pNumOfProductCanChange.Value = discountGroup.NumOfProductCanChange;
                    pRefundQuantityNoFee.Value = discountGroup.RefundQuantityNoFee;
                    pDiscountNote.Content = discountGroup.DiscountNote;
                    chkIsHidden.Checked = discountGroup.IsHidden.HasValue ? discountGroup.IsHidden.Value : false;
                    hdfPermittedRead.Value = discountGroup.PermittedRead;

                    // Create tag Owner
                    var owner = new { value = acc.ID, text = acc.Username };

                    // Create tag orther
                    var accountOther = AccountController.GetAllUser()
                        .Where(x =>
                            x.ID.ToString() == hdfPermittedRead.Value ||
                            hdfPermittedRead.Value.StartsWith(x.ID.ToString() + ",") ||
                            hdfPermittedRead.Value.Contains("," + x.ID.ToString() + ",") ||
                            hdfPermittedRead.Value.EndsWith("," + x.ID.ToString())
                        )
                        .Where(x => x.ID != owner.value)
                        .Select(x => new
                        {
                            value = x.ID,
                            text = x.Username
                        })
                        .ToList();

                    var script = "$(function () { showOwner(" + JsonConvert.SerializeObject(owner) + "); showAccoutPermittedAccess(" + JsonConvert.SerializeObject(accountOther) + "); });";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", script, true);
                }
            }
        }

        protected void btnUpdateDiscountGroup_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0)
                {
                    int id = ViewState["ID"].ToString().ToInt(0);
                    if (id > 0)
                    {
                        var now = DateTime.Now;

                        var data = new tbl_DiscountGroup()
                        {
                            ID = id,
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
                            ModifiedBy = username,
                            ModifiedDate = now,
                            PermittedRead = hdfPermittedRead.Value,
                            QuantityRequired = pQuantityRequired.Value.HasValue ? Convert.ToInt32(pQuantityRequired.Value.Value) : 0,
                        };


                        var result = DiscountGroupController.Update(data);

                        if (!String.IsNullOrEmpty(result))
                            PJUtils.ShowMessageBoxSwAlert("Cập nhật nhóm khách hàng thành công", "s", true, Page);
                    }
                }
            }
        }
    }
}