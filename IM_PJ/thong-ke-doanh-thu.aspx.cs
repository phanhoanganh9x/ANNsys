using IM_PJ.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class thong_ke_doanh_thu : System.Web.UI.Page
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
                if (Request.Cookies["usernameLoginSystem"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem"].Value;
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
                    TotalNumberOfOrder = x.Sum(s => s.TotalNumberOfOrder),
                    TotalSalePrice = x.Sum(s => s.TotalSalePrice),
                    TotalSaleDiscount = x.Sum(s => s.TotalSaleDiscount),
                    TotalRefundPrice = x.Sum(s => s.TotalRefundPrice),
                })
                .Single();


            double TotalSale = sumReport.TotalSalePrice - sumReport.TotalRefundPrice - sumReport.TotalSaleDiscount;

            ltrTotalNumberOfOrder.Text = sumReport.TotalNumberOfOrder.ToString() + " đơn";
            ltrNumberOfOrderPerDay.Text = (sumReport.TotalNumberOfOrder / day).ToString() + " đơn/ngày";
            ltrTotalRevenue.Text = string.Format("{0:N0}", TotalSale);
            ltrAverageRevenue.Text = string.Format("{0:N0}", TotalSale / day) + "đ/ngày";

            if (sumReport.TotalNumberOfOrder == 0)
            {
                ltrRevenuePerOrder.Text = "0";
            }
            else
            {
                ltrRevenuePerOrder.Text = string.Format("{0:N0}", TotalSale / sumReport.TotalNumberOfOrder) + "đ/đơn";
            }


            if (day > 1)
            {
                string chartLabelDays = "";
                string chartTotalSalePrice = "";
                string chartTotalNumberOfOrder = "";

                List<string> dataDays = new List<string>();
                List<string> dataTotalSalePrice = new List<string>();
                List<string> dataTotalNumberOfOrder = new List<string>();

                foreach (var item in reportModel)
                {
                    dataDays.Add(String.Format("'{0:d/M}'", item.DateDone));

                    dataTotalSalePrice.Add((item.TotalSalePrice).ToString());

                    dataTotalNumberOfOrder.Add((item.TotalNumberOfOrder).ToString());
                }

                chartLabelDays = String.Join(", ", dataDays);
                chartTotalSalePrice = String.Join(", ", dataTotalSalePrice);
                chartTotalNumberOfOrder = String.Join(", ", dataTotalNumberOfOrder);

                StringBuilder html = new StringBuilder();
                html.Append("<script>");
                html.Append("var lineChartData = {");
                html.Append("	labels: [" + chartLabelDays + "],");
                html.Append("	datasets: [{");
                html.Append("		label: 'Doanh thu',");
                html.Append("		borderColor: 'rgb(255, 99, 132)',");
                html.Append("		backgroundColor: 'rgb(255, 99, 132)',");
                html.Append("		fill: false,");
                html.Append("		data: [" + chartTotalSalePrice + "],");
                html.Append("		yAxisID: 'y-axis-1',");
                html.Append("	}, {");
                html.Append("		label: 'Số đơn',");
                html.Append("		borderColor: 'rgb(54, 162, 235)',");
                html.Append("		backgroundColor: 'rgb(54, 162, 235)',");
                html.Append("		fill: false,");
                html.Append("		data: [" + chartTotalNumberOfOrder + "],");
                html.Append("		yAxisID: 'y-axis-2'");
                html.Append("	}]");
                html.Append("};");
                html.Append("</script>");

                ltrChartData.Text = html.ToString();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string fromdate = rFromDate.SelectedDate.ToString();
            string todate = rToDate.SelectedDate.ToString();

            Response.Redirect("/thong-ke-doanh-thu?fromdate=" + fromdate + "&todate=" + todate + "");
        }
    }
}