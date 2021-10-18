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
            var totalProduct = 0;
            var totalCost = 0D;
            var categories = CategoryController.API_GetAllCategory();
            
            ltrList.Text = String.Empty;

            foreach (var item in categories)
            {
                var productStock = ProductController.GetProductReport(item.ID);

                ltrList.Text += "<tr>";
                ltrList.Text += String.Format("    <td>{0}: {1:N0} cái</td>", item.CategoryName, productStock.totalStock);
                ltrList.Text += String.Format("    <td>Tổng vốn {0}: {1:N0} VNĐ</td>", item.CategoryName, productStock.totalStockValue);
                ltrList.Text += "</tr>";

                // Tính tổng tất cả danh mục
                totalProduct += productStock.totalStock;
                totalCost += productStock.totalStockValue;
            }

            ltrTotalProduct.Text = String.Format("<p>Tổng số lượng: {0:N0} cái</p>", totalProduct);
            ltrTotalCost.Text = String.Format("<p>Tổng vốn: {0:N0} VNĐ</p>", totalCost);
        }
    }
}