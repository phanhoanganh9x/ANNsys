using IM_PJ.Controllers;
using IM_PJ.Models.Pages.thong_ke_doanh_thu_khach_hang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class thong_ke_khach_hang : System.Web.UI.Page
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
                        if (acc.RoleID == 1)
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
            
            DateTime fromDate = DateTime.Today;
            DateTime toDate = fromDate.AddDays(1).AddMinutes(-1);
            string phoneNumber = String.Empty;

            if (!String.IsNullOrEmpty(Request.QueryString["textsearch"]))
            {
                phoneNumber = Request.QueryString["textsearch"].Trim();
            }

            if (phoneNumber != String.Empty)
            {
                var customer = CustomerController.GetByPhone(phoneNumber);
                if (customer != null)
                {
                    fromDate = Convert.ToDateTime(customer.CreatedDate);
                }
            }

            if (!String.IsNullOrEmpty(Request.QueryString["fromdate"]))
            {
                fromDate = Convert.ToDateTime(Request.QueryString["fromdate"]);
            }

            if (!String.IsNullOrEmpty(Request.QueryString["todate"]))
            {
                toDate = Convert.ToDateTime(Request.QueryString["todate"]).AddDays(1).AddMinutes(-1);
            }

            rFromDate.SelectedDate = fromDate;
            rToDate.SelectedDate = toDate;

            var reportModel = OrderController.reportProfitByCustomer(phoneNumber, fromDate, toDate);

            if (reportModel != null)
            {
                initCustomer(reportModel.customer);
                initReport(reportModel.data, fromDate, toDate);
                initChart(reportModel.data, fromDate, toDate);
                txtTextSearch.Text = phoneNumber;
            }
        }

        private void initCustomer(CustomerModel customer)
        {
            var html = new StringBuilder();

            html.AppendLine("<div class='row'>");
            html.AppendLine("    <div class='col-md-6'>");
            html.AppendLine("        <div class='form-group'>");
            html.AppendLine("            <label>Họ tên</label>");
            html.AppendLine("            <span class='form-control input-disabled'>" + customer.name + "</span>");
            html.AppendLine("        </div>");
            html.AppendLine("    </div>");
            html.AppendLine("    <div class='col-md-6'>");
            html.AppendLine("        <div class='form-group'>");
            html.AppendLine("            <label>Điện thoại</label>");
            html.AppendLine("            <span class='form-control input-disabled'>" + customer.phone + "</span>");
            html.AppendLine("        </div>");
            html.AppendLine("    </div>");
            html.AppendLine("</div>");
            html.AppendLine("<div class='row'>");
            html.AppendLine("    <div class='col-md-6'>");
            html.AppendLine("        <div class='form-group'>");
            html.AppendLine("            <label>Nick đặt hàng</label>");
            html.AppendLine("            <span class='form-control input-disabled'>" + customer.nick + "</span>");
            html.AppendLine("        </div>");
            html.AppendLine("    </div>");
            html.AppendLine("    <div class='col-md-6'>");
            html.AppendLine("        <div class='form-group'>");
            html.AppendLine("            <label>Địa chỉ</label>");
            html.AppendLine("            <span class='form-control input-disabled'>" + customer.address + "</span>");
            html.AppendLine("        </div>");
            html.AppendLine("    </div>");
            html.AppendLine("</div>");

            ltrInfo.Text = html.ToString();
        }

        private void initReport(List<OrderInfoModel> data, DateTime fromDate, DateTime toDate)
        {
            var day = Convert.ToInt32((toDate - fromDate).TotalDays);

            var report = data
                .GroupBy(g => 1)
                .Select(x => new
                    {
                        totalQuantityOrder = x.Sum(s => s.quantityOrder),
                        totalQuantityProduct = x.Sum(s => s.quantityProduct),
                        totalCostOfGoods = x.Sum(s => s.costOfGoods),
                        totalPrice = x.Sum(s => s.price),
                        totalDiscount = x.Sum(s => s.discount),
                        totalCoupon = x.Sum(s => s.coupon),
                        // Phí giao hàng dùng để tham khảo
                        totalFeeShipping = x.Sum(s => s.feeShipping),
                        totalQuantityRefund = x.Sum(s => s.quantityRefund),
                        totalQuantityProductRefund = x.Sum(s => s.quantityProductRefund),
                        // Tiền hoàn trả đã bao gồm phí hoàn trả
                        totalRefundMoney = x.Sum(s => s.refundMoney),
                        totalRefundFee = x.Sum(s => s.refundFee),
                        totalRefundCapital = x.Sum(s => s.refundCapital)
                }
                )
                .FirstOrDefault();

            if (report != null)
            {
                // Dòng 1
                ltrTotalQuantityOrder.Text = String.Format("{0:N0} đơn", report.totalQuantityOrder);
                ltrTotalQuantityProduct.Text = String.Format("{0:N0} cái", report.totalQuantityProduct);
                ltrTotalQuantityRefund.Text = String.Format("{0:N0} đơn", report.totalQuantityRefund);
                ltrTotalQuantityProductRefund.Text = String.Format("{0:N0} cái ({1}%)", report.totalQuantityProductRefund, report.totalQuantityProductRefund * 100/ report.totalQuantityProduct);
                ltrTotalProductLeft.Text = String.Format("{0:N0} cái ({1:N0} cái/đơn)", report.totalQuantityProduct - report.totalQuantityProductRefund, (report.totalQuantityProduct - report.totalQuantityProductRefund)/report.totalQuantityOrder);
                ltrAverageProduct.Text = String.Format("{0:#,##0.##} cái/ngày", Math.Round((report.totalQuantityProduct - report.totalQuantityProductRefund) / (day * 1.0), 2));
                // Dòng 2
                ltrTotalPrice.Text = String.Format("{0:N0} đ", report.totalPrice);
                ltrTotalDiscount.Text = String.Format("{0:N0} đ", report.totalDiscount);
                // Phí giao hàng dùng để tham khảo
                ltrTotalFeeShipping.Text = String.Format("{0:N0} đ", report.totalFeeShipping);
                ltrTotalRefundMoney.Text = String.Format("{0:N0} đ", report.totalRefundMoney);
                ltrTotalRefundFee.Text = String.Format("{0:N0} đ", report.totalRefundFee);
                ltrTotalProfit.Text = String.Format("{0:N0} đ", (report.totalPrice - report.totalCostOfGoods) - report.totalDiscount - report.totalCoupon - (report.totalRefundMoney - report.totalRefundCapital) + report.totalRefundFee);
            }
        }

        private void initChart(List<OrderInfoModel> data, DateTime fromDate, DateTime toDate)
        {
            var day = Convert.ToInt32((toDate - fromDate).TotalDays);

            if (day > 1)
            {
                string chartDays = "";
                string chartProfit = "";
                string chartProduct = "";
                string chartSaleProduct = "";
                string chartRefundProduct = "";

                List<string> dataDays = new List<string>();
                List<string> dataProfit = new List<string>();
                List<string> dataSaleProduct = new List<string>();
                List<string> dataRefundProduct = new List<string>();
                List<string> dataProduct = new List<string>();

                foreach (var item in data)
                {
                    var date = String.Format("'{0:d/M}'", item.dateDone);
                    double profit = 0;

                    // Tính số tiền lời dựa trên các sản phẩm đã bán
                    // Tiền lời bằng tiền bán - tiền vốn - tiền chiết khấu + phí shipping + phí khác
                    profit += item.price - item.costOfGoods - item.discount;
                    // Tính số tiền lời sau khi sản phẩn được đổi trả
                    // Tiền lời bằng tiền lời đã bán - số liền lời của sản phẩn đổi trả + phí đổi trả
                    profit += -(item.refundMoney - item.refundCapital);

                    dataDays.Add(date);
                    dataProfit.Add(profit.ToString());
                    dataSaleProduct.Add(item.quantityProduct.ToString());
                    dataRefundProduct.Add(item.quantityProductRefund.ToString());
                    dataProduct.Add((item.quantityProduct - item.quantityProductRefund).ToString());
                }

                #region Tạo dữ liệu cho sale chart
                chartDays = String.Join(", ", dataDays);
                chartProfit = String.Join(", ", dataProfit);

                StringBuilder html1 = new StringBuilder();
                html1.Append("<script>");
                html1.Append("var lineSaleData = {");
                html1.Append("    labels: [" + chartDays + "],");
                html1.Append("    datasets: [{");
                html1.Append("        label: 'Lợi nhuận',");
                html1.Append("        borderColor: 'rgb(255, 99, 132)',");
                html1.Append("        backgroundColor: 'rgb(255, 99, 132)',");
                html1.Append("        fill: false,");
                html1.Append("        data: [" + chartProfit + "],");
                html1.Append("        yAxisID: 'y-axis-1',");
                html1.Append("    }]");
                html1.Append("};");
                html1.Append("</script>");

                ltrSaleData.Text = html1.ToString();
                #endregion

                #region Tạo dữ liệu cho product chart
                chartSaleProduct = String.Join(", ", dataSaleProduct);
                chartRefundProduct = String.Join(", ", dataRefundProduct);
                chartProduct = String.Join(", ", dataProduct);

                StringBuilder html2 = new StringBuilder();
                html2.Append("<script>");
                html2.Append("var lineProductData = {");
                html2.Append("   labels: [" + chartDays + "],");
                html2.Append("   datasets: [{");
                html2.Append("       label: 'Số lượng còn lại',");
                html2.Append("       borderColor: 'rgb(255, 99, 132)',");
                html2.Append("       backgroundColor: 'rgb(255, 99, 132)',");
                html2.Append("       fill: false,");
                html2.Append("       data: [" + chartProduct + "],");
                html2.Append("       yAxisID: 'y-axis-1',");
                html2.Append("   }, {");
                html2.Append("       label: 'Số lượng bán ra',");
                html2.Append("       borderColor: 'rgb(54, 162, 235)',");
                html2.Append("       backgroundColor: 'rgb(54, 162, 235)',");
                html2.Append("       fill: false,");
                html2.Append("       data: [" + chartSaleProduct + "],");
                html2.Append("       yAxisID: 'y-axis-1'");
                html2.Append("   }, {");
                html2.Append("       label: 'Số lượng đổi trả',");
                html2.Append("       borderColor: 'rgb(255, 205, 86)',");
                html2.Append("       backgroundColor: 'rgb(255, 205, 86)',");
                html2.Append("       fill: false,");
                html2.Append("       data: [" + chartRefundProduct + "],");
                html2.Append("       yAxisID: 'y-axis-1'");
                html2.Append("   }]");
                html2.Append("};");
                html2.Append("</script>");

                ltrProductData.Text = html2.ToString();
                #endregion
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string request = "/thong-ke-khach-hang?";

            if (txtTextSearch.Text != "")
            {
                request += "&textsearch=" + txtTextSearch.Text;
            }

            if (rFromDate.SelectedDate.HasValue)
            {
                request += "&fromdate=" + rFromDate.SelectedDate.ToString();
            }

            if (rToDate.SelectedDate.HasValue)
            {
                request += "&todate=" + rToDate.SelectedDate.ToString();
            }

            Response.Redirect(request);
        }
    }
}