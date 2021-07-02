using IM_PJ.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class thong_ke_nhan_vien : System.Web.UI.Page
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
                LoadAccountInfo();
                LoadData();
            }
        }

        public void LoadData()
        {
            string accountName = String.Empty;
            DateTime fromdate = DateTime.Today;
            DateTime todate = fromdate.AddDays(1).AddMinutes(-1);

            double totalRevenue = 0;
            double averageRevenue = 0;
            double totalProfit = 0;
            int totalSoldQuantity = 0;
            int averageSoldQuantity = 0;
            int totalRefundQuantity = 0;
            int averageRefundQuantity = 0;
            int totalDays = 0;

            if (!String.IsNullOrEmpty(Request.QueryString["accountName"]))
            {
                accountName = Request.QueryString["accountName"];
            }

            if (!String.IsNullOrEmpty(Request.QueryString["fromdate"]))
            {
                fromdate = Convert.ToDateTime(Request.QueryString["fromdate"]);
            }

            if (!String.IsNullOrEmpty(Request.QueryString["todate"]))
            {
                todate = Convert.ToDateTime(Request.QueryString["todate"]).AddDays(1).AddMinutes(-1);
            }

            ddlAccountInfo.SelectedValue = accountName;
            rFromDate.SelectedDate = fromdate;
            rToDate.SelectedDate = todate;
            totalDays = Convert.ToInt32((todate - fromdate).TotalDays);

            var userReport = OrderController.getUserReport(accountName, fromdate, todate);

            totalRevenue = userReport.totalRevenue;
            averageRevenue = totalRevenue / totalDays;
            totalSoldQuantity = userReport.totalSoldQuantity;
            averageSoldQuantity = totalSoldQuantity / totalDays;

            var userRefundReport = RefundGoodController.getUserReport(accountName, fromdate, todate);

            totalRefundQuantity = userRefundReport.totalRefundQuantity;
            averageRefundQuantity = totalRefundQuantity / totalDays;
            totalProfit = (userReport.totalRevenue - userReport.totalCost) - (userRefundReport.totalRevenue - userRefundReport.totalCost) + userRefundReport.totalRefundFee;

            // Tổng hệ thống

            var systemReport = OrderController.getUserReport("", fromdate, todate);
            var systemRefundReport = RefundGoodController.getUserReport("", fromdate, todate);

            int totalSystemQuantity = systemReport.totalSoldQuantity - systemRefundReport.totalRefundQuantity;
            int PercentQuantityOfSystem = 0;
            if (totalSystemQuantity > 0)
            {
                PercentQuantityOfSystem = (totalSoldQuantity - totalRefundQuantity) * 100 / totalSystemQuantity;
            }

            double totalSystemProfit = (systemReport.totalRevenue - systemReport.totalCost) - (systemRefundReport.totalRevenue - systemRefundReport.totalCost) + systemRefundReport.totalRefundFee;
            double PercentProfitOfSystem = 0;
            if (totalSystemProfit > 0)
            {
                PercentProfitOfSystem = totalProfit * 100 / totalSystemProfit;
            }

            // Khách mới
            var newCustomer = CustomerController.Report(accountName, fromdate, todate);

            ltrTotalRevenue.Text = String.Format("{0:N0}", totalRevenue);
            ltrAverageRevenue.Text = String.Format("{0:N0}/ngày", averageRevenue);
            ltrTotalSoldQuantity.Text = totalSoldQuantity.ToString() + " cái";
            ltrAverageSoldQuantity.Text = averageSoldQuantity.ToString() + " cái/ngày";
            ltrTotalRefundQuantity.Text = totalRefundQuantity.ToString() + " cái";
            ltrAverageRefundQuantity.Text = averageRefundQuantity.ToString() + " cái/ngày";
            ltrTotalRemainQuantity.Text = (totalSoldQuantity - totalRefundQuantity).ToString() + " cái";
            ltrAverageRemainQuantity.Text = ((totalSoldQuantity - totalRefundQuantity) / totalDays).ToString() + " cái/ngày";
            ltrTotalProfit.Text = String.Format("{0:N0}", totalProfit);
            ltrQuantityPercentOfSystem.Text = PercentQuantityOfSystem.ToString() + "%";
            ltrProfitPercentOfSystem.Text = Math.Round(PercentProfitOfSystem, 1).ToString() + "%";
            ltrTotalNewCustomer.Text = newCustomer.Count() + " khách mới";
        }

        /// <summary>
        /// Init drop down list account
        /// </summary>
        private void LoadAccountInfo()
        {
            ddlAccountInfo.Items.Clear();
            ddlAccountInfo.Items.Insert(0, new ListItem("Chọn nhân viên", ""));
            var accounts = AccountController.GetAllUser();
            foreach (var acc in accounts)
            {
                this.ddlAccountInfo.Items.Add(new ListItem(acc.FullName, acc.Username));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string accountName = this.ddlAccountInfo.SelectedValue;
            string fromdate = rFromDate.SelectedDate.ToString();
            string todate = rToDate.SelectedDate.ToString();

            Response.Redirect(String.Format("/thong-ke-nhan-vien?accountName={0}&fromdate={1}&todate={2}", accountName,fromdate, todate));
        }
    }
}