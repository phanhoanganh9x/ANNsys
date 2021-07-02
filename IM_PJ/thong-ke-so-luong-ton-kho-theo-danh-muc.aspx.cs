using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IM_PJ.Controllers;
using NHST.Bussiness;

namespace IM_PJ
{
    public partial class thong_ke_so_luong_ton_kho_theo_danh_muc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var config = ConfigController.GetByTop1();
            if (config.ViewAllReports == 0)
            {
                Response.Redirect("/trang-chu");
            }

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
                            Response.Redirect("/dang-nhap");
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
            var cate = CategoryController.API_GetAllCategory();
            ltrList.Text = "";
            double totalCost = 0;
            int totalProduct = 0;
            if (cate.Count > 0)
            {
                foreach (var item in cate)
                {
                    ltrList.Text += "<tr>";

                    int quantity = 0;
                    double totalprice = 0;
                    var list = ProductController.GetProductReport(item.ID);
                    {
                        foreach (var temp in list)
                        {
                            quantity += Convert.ToInt32(temp.TotalProductInstockQuantityLeft);
                            totalprice += temp.CostOfGood * temp.TotalProductInstockQuantityLeft;
                        }
                    }
                    totalProduct += quantity;
                    totalCost += totalprice;
                    ltrList.Text += "<td>" + item.CategoryName + ": " + quantity +" cái"+ "</td>";
                    ltrList.Text += "<td>Tổng vốn: " + item.CategoryName + ": " + string.Format("{0:N0}", totalprice) + " VNĐ" + "</td>";
                    ltrList.Text += "</tr>";
                }
            }
            ltrTotalCost.Text = "<p>Tổng vốn: " + string.Format("{0:N0}",totalCost) + " VNĐ</p>";
            ltrTotalProduct.Text = "<p>Tổng số lượng: " + string.Format("{0:N0}", totalProduct) + " cái</p>";
        }
    }
}