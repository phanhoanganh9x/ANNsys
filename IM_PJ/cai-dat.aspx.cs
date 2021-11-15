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
    public partial class cai_dat : System.Web.UI.Page
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
                        if (acc.RoleID != 0)
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
            var p = ConfigController.GetByTop1();
            if (p != null)
            {
                pNumOfDateToChangeProduct.Value = Convert.ToDouble(p.NumOfDateToChangeProduct);
                pNumOfProductCanChange.Value = Convert.ToDouble(p.NumOfProductCanChange);
                pFeeChangeProduct.Value = Convert.ToDouble(p.FeeChangeProduct);
                pReturnRule1.Content = p.ChangeGoodsRule;
                pReturnRule2.Content = p.RetailReturnRule;
                pCSSPrintBarcode.Text = p.CSSPrintBarcode;
                ddlHideProduct.SelectedValue = p.HideProduct.ToString();
                ddlViewAllOrders.SelectedValue = p.ViewAllOrders.ToString();
                ddlViewAllReports.SelectedValue = p.ViewAllReports.ToString();

                txtWPFBClothes.Text = p.WPFBClothes;
                txtWPFBCosmetics.Text = p.WPFBCosmetics;
                txtWPNewsClothes.Text = p.WPNewsClothes;
                txtWPNewsCosmetics.Text = p.WPNewsCosmetics;
                txtWPZaloClothes.Text = p.WPZaloClothes;
                txtWPZaloCosmetics.Text = p.WPZaloCosmetics;
                txtWPHotlineClothes.Text = p.WPHotlineClothes;
                txtWPHotlineCosmetics.Text = p.WPHotlineCosmetics;
                txtWPImageClothes.Text = p.WPImageClothes;
                txtWPImageCosmetics.Text = p.WPImageCosmetics;
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            string securityCode = txtSecurityCode.Text.Trim();
            if (acc != null)
            {
                if (acc.RoleID == 0)
                {
                    if (!string.IsNullOrEmpty(securityCode))
                    {
                        string confirmSecurityCode = txtConfirmSecurityCode.Text;
                        if (!string.IsNullOrEmpty(confirmSecurityCode))
                        {
                            if (securityCode == confirmSecurityCode)
                            {
                                ConfigController.UpdateSecurityCode(securityCode);
                            }
                            else
                            {
                                lblError.Text = "Xác nhận mã bảo mật không đúng.";
                                lblError.Visible = true;
                            }
                        }
                        else
                        {
                            lblError.Text = "Không để trống xác nhận mã bảo mật";
                            lblError.Visible = true;
                        }
                    }

                    ConfigController.Update(
                        1, 
                        Convert.ToDouble(pNumOfDateToChangeProduct.Value), 
                        Convert.ToDouble(pNumOfProductCanChange.Value), 
                        Convert.ToDouble(pFeeChangeProduct.Value), 
                        0, 
                        pReturnRule1.Content, 
                        pReturnRule2.Content, 
                        DateTime.Now, 
                        username, 
                        pCSSPrintBarcode.Text, 
                        Convert.ToInt32(ddlHideProduct.SelectedValue),
                        Convert.ToInt32(ddlViewAllOrders.SelectedValue),
                        Convert.ToInt32(ddlViewAllReports.SelectedValue),
                        txtWPNewsClothes.Text,
                        txtWPNewsCosmetics.Text,
                        txtWPZaloClothes.Text,
                        txtWPZaloCosmetics.Text,
                        txtWPFBClothes.Text,
                        txtWPFBCosmetics.Text,
                        txtWPHotlineClothes.Text,
                        txtWPHotlineCosmetics.Text,
                        txtWPImageClothes.Text,
                        txtWPImageCosmetics.Text
                        );

                    PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công", "s", true, Page);
                }
            }

        }
    }
}