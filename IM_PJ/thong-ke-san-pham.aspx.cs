using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class thong_ke_san_pham : System.Web.UI.Page
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
                LoadCreatedBy();
                LoadCategory();
                LoadData();
            }
        }
        public void LoadCategory()
        {
            var category = CategoryController.GetAllWithIsHidden(false);
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new ListItem("Danh mục", "0"));
            if (category.Count > 0)
            {
                addItemCategory(0, "");
                ddlCategory.DataBind();
            }
        }

        public void addItemCategory(int id, string h = "")
        {
            var categories = CategoryController.GetByParentID("", id);

            if (categories.Count > 0)
            {
                foreach (var c in categories)
                {
                    ListItem listitem = new ListItem(h + c.CategoryName, c.ID.ToString());
                    ddlCategory.Items.Add(listitem);

                    addItemCategory(c.ID, h + "---");
                }
            }
        }
        public void LoadCreatedBy()
        {
            var CreateBy = AccountController.GetAllNotSearch();
            ddlCreatedBy.Items.Clear();
            ddlCreatedBy.Items.Insert(0, new ListItem("Nhân viên", ""));
            if (CreateBy.Count > 0)
            {
                foreach (var p in CreateBy)
                {
                    if (p.RoleID == 0 || p.RoleID == 2)
                    {
                        ListItem listitem = new ListItem(p.Username, p.Username);
                        ddlCreatedBy.Items.Add(listitem);
                    }
                }
                ddlCreatedBy.DataBind();
            }
        }

        public void LoadData()
        {
            string SKU = String.Empty;
            int CategoryID = 0;
            DateTime fromdate = DateTime.Today;
            DateTime todate = fromdate.AddDays(1).AddMinutes(-1);

            string CreatedBy = "";
            int totalRemainQuantity = 0;
            int totalSoldQuantity = 0;
            int totalRefundQuantity = 0;
            double totalCost = 0;
            double totalProfit = 0;
            double totalRefundProfit = 0;
            double totalRevenue = 0;

            int day = 0;

            if (!String.IsNullOrEmpty(Request.QueryString["SKU"]))
            {
                SKU = Request.QueryString["SKU"];

                var product = ProductController.GetBySKU(SKU);
                if (product != null)
                {
                    fromdate = Convert.ToDateTime(product.CreatedDate);
                }
            }

            if (!String.IsNullOrEmpty(Request.QueryString["categoryid"]))
            {
                CategoryID = Request.QueryString["categoryid"].ToInt(0);
            }

            if (!String.IsNullOrEmpty(Request.QueryString["createdby"]))
            {
                CreatedBy = Request.QueryString["createdby"];
            }

            if (!String.IsNullOrEmpty(Request.QueryString["fromdate"]))
            {
                fromdate = Convert.ToDateTime(Request.QueryString["fromdate"]);
            }

            if (!String.IsNullOrEmpty(Request.QueryString["todate"]))
            {
                todate = Convert.ToDateTime(Request.QueryString["todate"]).AddDays(1).AddMinutes(-1);
            }

            txtTextSearch.Text = SKU;
            ddlCategory.SelectedValue = CategoryID.ToString();
            ddlCreatedBy.SelectedValue = CreatedBy;
            rFromDate.SelectedDate = fromdate;
            rToDate.SelectedDate = todate;
            day = Convert.ToInt32((todate - fromdate).TotalDays);

            if (day <= 0)
            {
                day = 1;
            }

            this.ltrTotalRemain.Text = String.Empty;
            this.ltrTotalRemainPerDay.Text = String.Empty;
            this.ltrTotalSold.Text = String.Empty;
            this.ltrTotalRefund.Text = String.Empty;
            this.ltrTotalProfit.Text = String.Empty;
            this.ltrTotalRevenue.Text = String.Empty;
            this.ltrTotalStock.Text = String.Empty;
            this.ltrTotalStockValue.Text = String.Empty;

            var productReport = OrderController.getProductReport(SKU, CategoryID, CreatedBy, fromdate, todate);
            totalSoldQuantity = productReport.Sum(x => x.totalSold);
            totalRevenue = productReport.Sum(x => x.totalRevenue);
            totalCost = productReport.Sum(x => x.totalCost);
            totalProfit = totalRevenue - totalCost;

            var productRefundReport = RefundGoodController.getRefundProductReport(SKU, CategoryID, CreatedBy, fromdate, todate);
            totalRefundQuantity = productRefundReport.Sum(x => x.totalRefund);
            double totalRefundRevenue = productRefundReport.Sum(x => x.totalRevenue);
            double totalRefundCost = productRefundReport.Sum(x => x.totalCost);
            double totalRefundFee = productRefundReport.Sum(x => x.totalRefundFee);
            totalRefundProfit = totalRefundRevenue - totalRefundCost;

            totalRemainQuantity = totalSoldQuantity - totalRefundQuantity;

            var productStockReport = ProductController.getProductStockReport(SKU, CategoryID);

            totalProfit = totalProfit - totalRefundProfit + totalRefundFee;

            ltrTotalSold.Text = totalSoldQuantity.ToString();
            ltrTotalRefund.Text = totalRefundQuantity.ToString();
            ltrTotalRemain.Text = totalRemainQuantity.ToString();
            ltrTotalRemainPerDay.Text = (totalRemainQuantity / day).ToString() + " cái/ngày";
            ltrTotalRevenue.Text = string.Format("{0:N0}", totalRevenue);
            ltrTotalProfit.Text = string.Format("{0:N0}", totalProfit);
            ltrAverageProfit.Text = string.Format("{0:N0}", totalProfit / day).ToString();
            ltrTotalStock.Text = productStockReport.totalStock.ToString() + " cái";
            ltrTotalStockValue.Text = string.Format("{0:N0}", productStockReport.totalStockValue);

            if (day > 1)
            {
                string chartLabelDays = "";
                string chartTotalRemainQuantity = "";

                List<string> dataDays = new List<string>();
                List<string> dataTotalRemainQuantity = new List<string>();

                while (fromdate < todate)
                {
                    // Ngày biểu đồ
                    dataDays.Add(String.Format("'{0:d/M}'", fromdate));

                    // Biểu đồ sản lượng
                    int TotalSoldQuantity = productReport.Where(x => x.reportDate.Date == fromdate).Sum(x => x.totalSold);
                    int TotalRefundQuantity = productRefundReport.Where(x => x.reportDate == fromdate).Sum(x => x.totalRefund);
                    dataTotalRemainQuantity.Add((TotalSoldQuantity - TotalRefundQuantity).ToString());

                    // Thêm 1 ngày chạy vòng lặp
                    fromdate = fromdate.AddDays(1);
                }

                chartLabelDays = String.Join(", ", dataDays);
                chartTotalRemainQuantity = String.Join(", ", dataTotalRemainQuantity);

                StringBuilder html = new StringBuilder();
                html.Append("<script>");
                html.Append("var lineChartData = {");
                html.Append("	labels: [" + chartLabelDays + "],");
                html.Append("	datasets: [{");
                html.Append("		label: 'Sản lượng',");
                html.Append("		borderColor: 'rgb(255, 99, 132)',");
                html.Append("		backgroundColor: 'rgb(255, 99, 132)',");
                html.Append("		fill: false,");
                html.Append("		data: [" + chartTotalRemainQuantity + "],");
                html.Append("		yAxisID: 'y-axis-1',");
                html.Append("	}]");
                html.Append("};");
                html.Append("</script>");

                ltrChartData.Text = html.ToString();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string SKU = txtTextSearch.Text;
            string CategoryID = ddlCategory.SelectedValue;
            string CreatedBy = ddlCreatedBy.SelectedValue;
            string fromdate = rFromDate.SelectedDate.ToString();
            string todate = rToDate.SelectedDate.ToString();
            
            Response.Redirect(String.Format("/thong-ke-san-pham?SKU={0}&categoryid={1}&createdby={2}&fromdate={3}&todate={4}", SKU, CategoryID, CreatedBy, fromdate, todate));
        }
    }
}