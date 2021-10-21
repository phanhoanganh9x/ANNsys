using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IM_PJ.Controllers;

namespace IM_PJ
{
    public partial class thong_ke_loi_nhuan : System.Web.UI.Page
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
            DateTime fromdate = DateTime.Today;
            DateTime todate = fromdate.AddDays(1).AddMinutes(-1);

            if (!String.IsNullOrEmpty(Request.QueryString["fromdate"]))
            {
                fromdate = Convert.ToDateTime(Request.QueryString["fromdate"]);
            }

            if (!String.IsNullOrEmpty(Request.QueryString["todate"]))
            {
                todate = Convert.ToDateTime(Request.QueryString["todate"]).AddDays(1).AddMinutes(-1);
            }

            rFromDate.SelectedDate = fromdate;
            rToDate.SelectedDate = todate;

            int day = Convert.ToInt32((todate - fromdate).TotalDays);

            var reportModel = OrderController.GetProfitReport(fromdate, todate);

            if (reportModel == null || reportModel.Count == 0)
                return;

            var sumReport = reportModel
                .GroupBy(g => 1)
                .Select(x => new
                {
                    // Order
                    TotalNumberOfOrder = x.Sum(s => s.TotalNumberOfOrder),
                    TotalSoldQuantity = x.Sum(s => s.TotalSoldQuantity),
                    TotalSaleCost = x.Sum(s => s.TotalSaleCost),
                    TotalSalePrice = x.Sum(s => s.TotalSalePrice),
                    TotalSaleDiscount = x.Sum(s => s.TotalSaleDiscount),
                    TotalCoupon = x.Sum(s => s.TotalCoupon),
                    // Phí giao hàng chỉ dùng để tham khảo
                    TotalShippingFee = x.Sum(s => s.TotalShippingFee),
                    // Phí khác chỉ dùng để tham khảo
                    TotalOtherFee = x.Sum(s => s.TotalOtherFee),
                    // Return Order
                    TotalReturnOrderCount = x.Sum(s => s.TotalReturnOrderCount),
                    TotalReturnFee = x.Sum(s => s.TotalReturnFee),
                    // Refund
                    TotalRefundQuantity = x.Sum(s => s.TotalRefundQuantity),
                    TotalRefundCost = x.Sum(s => s.TotalRefundCost),
                    TotalRefundPrice = x.Sum(s => s.TotalRefundPrice),
                    TotalRefundFee = x.Sum(s => s.TotalRefundFee),
                    // Profit
                    TotalProfit = x.Sum(s => (s.TotalSalePrice - s.TotalSaleCost) - s.TotalSaleDiscount - s.TotalCoupon - s.TotalReturnFee - (s.TotalRefundPrice - s.TotalRefundCost) + s.TotalRefundFee)
                })
                .Single();

            // Profit
            var TotalProfitPerDay = sumReport.TotalProfit / (double)day;
            var TotalProfitPerOrder = sumReport.TotalNumberOfOrder > 0 ? Math.Ceiling(sumReport.TotalProfit / (double)sumReport.TotalNumberOfOrder) : 0D;
            var TotalActualRevenue = sumReport.TotalSalePrice - sumReport.TotalRefundPrice - sumReport.TotalSaleDiscount;
            // Quantity
            var TotalRemainQuantity = sumReport.TotalSoldQuantity - sumReport.TotalRefundQuantity;

            ltrTotalProfit.Text = string.Format("{0:N0}", sumReport.TotalProfit);
            ltrProfitPerDay.Text = string.Format("{0:N0}", TotalProfitPerDay);
            ltrProfitPerOrder.Text = string.Format("{0:N0}", TotalProfitPerOrder);

            ltrTotalRemainQuantity.Text = TotalRemainQuantity.ToString() + " cái";
            ltrAverageRemainQuantity.Text = (TotalRemainQuantity / day).ToString() + " cái/ngày";
            ltrTotalSoldQuantity.Text = sumReport.TotalSoldQuantity.ToString() + " cái";

            ltrTotalSalePrice.Text = string.Format("{0:N0}", sumReport.TotalSalePrice);
            ltrTotalSaleCost.Text = string.Format("{0:N0}", sumReport.TotalSaleCost);
            ltrTotalDisount.Text = string.Format("{0:N0}", sumReport.TotalSaleDiscount);

            ltrTotalRefundPrice.Text = string.Format("{0:N0}", sumReport.TotalRefundPrice);
            ltrTotalRefundCost.Text = string.Format("{0:N0}", sumReport.TotalRefundCost);
            ltrTotalRefundFee.Text = string.Format("{0:N0}", sumReport.TotalRefundFee);

            ltrTotalActualRevenue.Text = string.Format("{0:N0}", TotalActualRevenue);
            ltrTotalOtherFee.Text = string.Format("{0:N0}", sumReport.TotalOtherFee);
            ltrTotalShippingFee.Text = string.Format("{0:N0}", sumReport.TotalShippingFee);
            ltrTotalCouponValue.Text = string.Format("{0:N0}", sumReport.TotalCoupon);

            if (day > 1)
            {
                string chartLabelDays = "";
                string chartTotalProfit = "";
                string chartTotalRemainQuantity = "";

                List<string> dataDays = new List<string>();
                List<string> dataTotalProfit = new List<string>();
                List<string> dataTotalRemainQuantity = new List<string>();

                foreach (var item in reportModel)
                {
                    dataDays.Add(String.Format("'{0:d/M}'", item.DateDone));

                    double chartTotalRevenue = item.TotalSalePrice - item.TotalRefundPrice;
                    double chartTotalCost = item.TotalSaleCost - item.TotalRefundCost;
                    dataTotalProfit.Add((chartTotalRevenue - chartTotalCost - item.TotalSaleDiscount + item.TotalRefundFee).ToString());

                    dataTotalRemainQuantity.Add((item.TotalSoldQuantity - item.TotalRefundQuantity).ToString());
                }

                chartLabelDays = String.Join(", ", dataDays);
                chartTotalProfit = String.Join(", ", dataTotalProfit);
                chartTotalRemainQuantity = String.Join(", ", dataTotalRemainQuantity);

                StringBuilder html = new StringBuilder();
                html.Append("<script>");
                html.Append("var lineChartData = {");
                html.Append("    labels: [" + chartLabelDays + "],");
                html.Append("    datasets: [{");
                html.Append("        label: 'Lợi nhuận',");
                html.Append("        borderColor: 'rgb(255, 99, 132)',");
                html.Append("        backgroundColor: 'rgb(255, 99, 132)',");
                html.Append("        fill: false,");
                html.Append("        data: [" + chartTotalProfit + "],");
                html.Append("        yAxisID: 'y-axis-1',");
                html.Append("    }, {");
                html.Append("        label: 'Sản lượng',");
                html.Append("        borderColor: 'rgb(54, 162, 235)',");
                html.Append("        backgroundColor: 'rgb(54, 162, 235)',");
                html.Append("        fill: false,");
                html.Append("        data: [" + chartTotalRemainQuantity + "],");
                html.Append("        yAxisID: 'y-axis-2'");
                html.Append("    }]");
                html.Append("};");
                html.Append("</script>");

                ltrChartData.Text = html.ToString();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string fromdate = rFromDate.SelectedDate.ToString();
            string todate = rToDate.SelectedDate.ToString();

            Response.Redirect("/thong-ke-loi-nhuan?fromdate=" + fromdate + "&todate=" + todate + "");
        }
    }
}