﻿using IM_PJ.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class thong_ke_san_luong : System.Web.UI.Page
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
                if (Request.Cookies["userLoginSystem"] != null)
                {
                    string username = Request.Cookies["userLoginSystem"].Value;
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
            double TotalSalePrice = reportModel.Sum(x => x.TotalSalePrice);
            double TotalRefundPrice = reportModel.Sum(x => x.TotalRefundPrice);
            double TotalRevenue = TotalSalePrice - TotalRefundPrice;
            double TotalSaleCost = reportModel.Sum(x => x.TotalSaleCost);
            double TotalRefundCost = reportModel.Sum(x => x.TotalRefundCost);
            double TotalCost = TotalSaleCost - TotalRefundCost;
            double TotalSaleDiscount = reportModel.Sum(x => x.TotalSaleDiscount);
            double TotalRefundFee = reportModel.Sum(x => x.TotalRefundFee);
            double TotalProfit = TotalRevenue - TotalCost - TotalSaleDiscount + TotalRefundFee;
            double AverageProfitPerProduct = 0;
            int TotalSoldQuantity = reportModel.Sum(x => x.TotalSoldQuantity);
            int TotalRefundQuantity = reportModel.Sum(x => x.TotalRefundQuantity);
            int TotalRemainQuantity = TotalSoldQuantity - TotalRefundQuantity;

            int TotalNumberOfOrder = reportModel.Sum(x => x.TotalNumberOfOrder);
            if (TotalNumberOfOrder > 0)
            {
                AverageProfitPerProduct = Math.Ceiling(TotalProfit / TotalRemainQuantity);
            }

            ltrTotalRemain.Text = (TotalRemainQuantity).ToString() + " cái";
            ltrAverageTotalRemain.Text = (TotalRemainQuantity / day).ToString() + " cái/ngày";
            ltrTotalSales.Text = (TotalSoldQuantity).ToString() + " cái";
            ltrAverageTotalSales.Text = (TotalSoldQuantity / day).ToString() + " cái/ngày";
            ltrTotalRefund.Text = (TotalRefundQuantity).ToString() + " cái";
            ltrAverageTotalRefund.Text = (TotalRefundQuantity / day).ToString() + " cái/ngày";
            ltrAverageProfitPerProduct.Text = string.Format("{0:N0}", AverageProfitPerProduct) + " đ/cái";

            if (day > 1)
            {
                string chartLabelDays = "";
                string chartTotalRemainQuantity = "";
                string chartTotalSoldQuantity = "";
                string chartTotalRefundQuantity = "";

                List<string> dataDays = new List<string>();
                List<string> dataTotalRemainQuantity = new List<string>();
                List<string> dataTotalSoldQuantity = new List<string>();
                List<string> dataTotalRefundQuantity = new List<string>();

                foreach (var item in reportModel)
                {
                    dataDays.Add(String.Format("'{0:d/M}'", item.DateDone));
                    dataTotalRemainQuantity.Add((item.TotalSoldQuantity - item.TotalRefundQuantity).ToString());
                    dataTotalSoldQuantity.Add(item.TotalSoldQuantity.ToString());
                    dataTotalRefundQuantity.Add(item.TotalRefundQuantity.ToString());
                }

                chartLabelDays = String.Join(", ", dataDays);
                chartTotalRemainQuantity = String.Join(", ", dataTotalRemainQuantity);
                chartTotalSoldQuantity = String.Join(", ", dataTotalSoldQuantity);
                chartTotalRefundQuantity = String.Join(", ", dataTotalRefundQuantity);

                StringBuilder html = new StringBuilder();
                html.Append("<script>");
                html.Append("var lineChartData = {");
                html.Append("	labels: [" + chartLabelDays + "],");
                html.Append("	datasets: [{");
                html.Append("		label: 'Số lượng còn lại',");
                html.Append("		borderColor: 'rgb(255, 99, 132)',");
                html.Append("		backgroundColor: 'rgb(255, 99, 132)',");
                html.Append("		fill: false,");
                html.Append("		data: [" + chartTotalRemainQuantity + "],");
                html.Append("		yAxisID: 'y-axis-1',");
                html.Append("	}, {");
                html.Append("		label: 'Số lượng bán ra',");
                html.Append("		borderColor: 'rgb(54, 162, 235)',");
                html.Append("		backgroundColor: 'rgb(54, 162, 235)',");
                html.Append("		fill: false,");
                html.Append("		data: [" + chartTotalSoldQuantity + "],");
                html.Append("		yAxisID: 'y-axis-1'");
                html.Append("	}, {");
                html.Append("		label: 'Số lượng đổi trả',");
                html.Append("		borderColor: 'rgb(255, 205, 86)',");
                html.Append("		backgroundColor: 'rgb(255, 205, 86)',");
                html.Append("		fill: false,");
                html.Append("		data: [" + chartTotalRefundQuantity + "],");
                html.Append("		yAxisID: 'y-axis-1'");
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

            Response.Redirect("/thong-ke-san-luong?fromdate=" + fromdate + "&todate=" + todate + "");
        }
    }
}