using IM_PJ.Controllers;
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
    public partial class bao_cao_nhan_vien : System.Web.UI.Page
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
                        if (acc.RoleID == 1)
                        {
                            Response.Redirect("/dang-nhap");
                        }
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
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
        public void LoadData()
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                string user = acc.Username;
                string SKU = String.Empty;
                int CategoryID = 0;
                DateTime fromdate = DateTime.Today;
                DateTime todate = fromdate.AddDays(1).AddMinutes(-1);

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
                rFromDate.SelectedDate = fromdate;
                rToDate.SelectedDate = todate;

                // Tính theo thời gian đã chọn
                var uReport = OrderController.getReport(SKU, CategoryID, user, fromdate, todate);
                var sysReport = OrderController.getReport(SKU, CategoryID, "", fromdate, todate);
                
                // Tính tháng trước
                DateTime fromdateLastMonth = fromdate.FirstDayOfMonth().AddMonths(-1);
                DateTime todateLastMonth = fromdateLastMonth.LastDayOfMonth().AddDays(1).AddMinutes(-1);
                var uReportLastMonth = OrderController.getReport(SKU, CategoryID, user, fromdateLastMonth, todateLastMonth);
                
                int totalDays = Convert.ToInt32((todate - fromdate).TotalDays);

                string compareTotalRemainQuantity = "";
                string compareTotalSoldQuantity = "";
                string compareTotalSaleOrder = "";
                string compareNewCustomer = "";
                string compareTotalRefundQuantity = "";
                string compareAverageRemainQuantity = "";

                if (totalDays > 1)
                {
                    // So sánh tổng số lượng còn lại với tháng trước
                    int diffTotalRemainQuantity = uReport.totalRemainQuantity - uReportLastMonth.totalRemainQuantity;
                    int percentTotalRemainQuantity = uReportLastMonth.totalRemainQuantity == 0 ? 0 : (uReport.totalRemainQuantity * 100 / uReportLastMonth.totalRemainQuantity - 100);
                    if (diffTotalRemainQuantity > 0)
                    {
                        compareTotalRemainQuantity += "<span class='font-green' title='Tăng so với Tổng tháng " + fromdateLastMonth.Month + "'>+" + diffTotalRemainQuantity + " (+" + percentTotalRemainQuantity + "%)</span>";
                    }
                    else
                    {
                        compareTotalRemainQuantity += "<span class='font-red' title='Giảm so với Tổng tháng " + fromdateLastMonth.Month + "'>" + diffTotalRemainQuantity + " (" + percentTotalRemainQuantity + "%)</span>";
                    }

                    // So sánh tổng số lượng bán ra với tháng trước
                    int diffTotalSoldQuantity = uReport.totalSoldQuantity - uReportLastMonth.totalSoldQuantity;
                    int percentTotalSoldQuantity = uReportLastMonth.totalSoldQuantity == 0 ? 0 : (uReport.totalSoldQuantity * 100 / uReportLastMonth.totalSoldQuantity - 100);
                    if (diffTotalSoldQuantity > 0)
                    {
                        compareTotalSoldQuantity += "<span class='font-green' title='Tăng so với Tổng tháng " + fromdateLastMonth.Month + "'>+" + diffTotalSoldQuantity + " (+" + percentTotalSoldQuantity + "%)</span>";
                    }
                    else
                    {
                        compareTotalSoldQuantity += "<span class='font-red' title='Giảm so với Tổng tháng " + fromdateLastMonth.Month + "'>" + diffTotalSoldQuantity + " (" + percentTotalSoldQuantity + "%)</span>";
                    }

                    // So sánh tổng số đơn với tháng trước
                    int diffTotalSaleOrder = uReport.totalSaleOrder - uReportLastMonth.totalSaleOrder;
                    int percentTotalSaleOrder = uReportLastMonth.totalSaleOrder == 0 ? 0 : (uReport.totalSaleOrder * 100 / uReportLastMonth.totalSaleOrder - 100);
                    if (diffTotalSaleOrder > 0)
                    {
                        compareTotalSaleOrder += "<span class='font-green' title='Tăng so với Tổng tháng " + fromdateLastMonth.Month + "'>+" + diffTotalSaleOrder + " (+" + percentTotalSaleOrder + "%)</span>";
                    }
                    else
                    {
                        compareTotalSaleOrder += "<span class='font-red' title='Giảm so với Tổng tháng " + fromdateLastMonth.Month + "'>" + diffTotalSaleOrder + " (" + percentTotalSaleOrder + "%)</span>";
                    }

                    // So sánh số lượng hàng trả với tháng trước
                    int diffTotalRefundQuantity = uReport.totalRefundQuantity - uReportLastMonth.totalRefundQuantity;
                    if (diffTotalRefundQuantity > 0)
                    {
                        compareTotalRefundQuantity += "<span class='font-red' title='Tăng so với Tổng tháng " + fromdateLastMonth.Month + "'>+" + diffTotalRefundQuantity + "</span>";
                    }
                    else
                    {
                        compareTotalRefundQuantity += "<span class='font-green' title='Giảm so với Tổng tháng " + fromdateLastMonth.Month + "'>" + diffTotalRefundQuantity + "</span>";
                    }

                    // So sánh số khách mới với tháng trước
                    int diffNewCustomer = uReport.totalNewCustomer - uReportLastMonth.totalNewCustomer;
                    int percentNewCustomer = uReportLastMonth.totalNewCustomer == 0 ? 0 : (uReport.totalNewCustomer * 100 / uReportLastMonth.totalNewCustomer - 100);
                    if (diffNewCustomer > 0)
                    {
                        compareNewCustomer += "<span class='font-green' title='Tăng so với Tổng tháng " + fromdateLastMonth.Month + "'>+" + diffNewCustomer + " (+" + percentNewCustomer + "%)</span>";
                    }
                    else
                    {
                        compareNewCustomer += "<span class='font-red' title='Giảm so với Tổng tháng " + fromdateLastMonth.Month + "'>" + diffNewCustomer + " (" + percentNewCustomer + "%)</span>";
                    }
                }

                // So sánh trung bình số lượng còn lại với tháng này (chỉ so sánh khi số ngày == 1)
                if (totalDays >= 1)
                {
                    // So sánh trung bình số lượng còn lại với tháng trước
                    int diffAverageRemainQuantity = uReport.averageRemainQuantity - uReportLastMonth.averageRemainQuantity;
                    var percentAverageRemainQuantity = uReportLastMonth.averageRemainQuantity == 0 ? 0 : (uReport.averageRemainQuantity * 100 / uReportLastMonth.averageRemainQuantity - 100);
                    if (diffAverageRemainQuantity > 0)
                    {
                        compareAverageRemainQuantity += "<span class='font-green' title='Tăng so với Trung bình tháng " + fromdateLastMonth.Month + "'>+" + diffAverageRemainQuantity + " (+" + percentAverageRemainQuantity + "%)</span>";
                    }
                    else
                    {
                        compareAverageRemainQuantity += "<span class='font-red' title='Giảm so với Trung bình tháng " + fromdateLastMonth.Month + "'>" + diffAverageRemainQuantity + " (" + percentAverageRemainQuantity + "%)</span>";
                    }

                    // Tính tháng này
                    DateTime fromdateThisMonth = fromdate.FirstDayOfMonth();
                    DateTime todateThisMonth = fromdate.LastDayOfMonth().AddDays(1).AddMinutes(-1);
                    if (fromdateThisMonth.Month == DateTime.Today.Month)
                    {
                        todateThisMonth = DateTime.Today.AddDays(1).AddMinutes(-1);
                    }
                    var uReportThisMonth = OrderController.getReport(SKU, CategoryID, user, fromdateThisMonth, todateThisMonth);
                    int diffThisMonthAverageRemainQuantity = uReport.averageRemainQuantity - uReportThisMonth.averageRemainQuantity;
                    int percentThisMonthAverageRemainQuantity = uReportThisMonth.averageRemainQuantity == 0 ? 0 : (uReport.averageRemainQuantity * 100 / uReportThisMonth.averageRemainQuantity - 100);
                    if (diffThisMonthAverageRemainQuantity > 0)
                    {
                        compareAverageRemainQuantity += " <span class='font-blue' title='Tăng so với Trung bình tháng " + todateThisMonth.Month + "'>+" + diffThisMonthAverageRemainQuantity + " (+" + percentThisMonthAverageRemainQuantity + "%)</span>";
                    }
                    else if (diffThisMonthAverageRemainQuantity < 0)
                    {
                        compareAverageRemainQuantity += " <span class='font-red' title='Giảm so với Trung bình tháng " + todateThisMonth.Month + "'>" + diffThisMonthAverageRemainQuantity + " (" + percentThisMonthAverageRemainQuantity + "%)</span>";
                    }
                }
                
                // Tính phần trăm / toàn hệ thống
                double PercentOfSystem = sysReport.totalProfit == 0 ? 0 : (uReport.totalProfit * 100 / sysReport.totalProfit);

                ltrTotalSaleOrder.Text = uReport.totalSaleOrder + " đơn " + compareTotalSaleOrder;
                ltrAverageSaleOrder.Text = uReport.averageSaleOrder.ToString() + " đơn/ngày";
                ltrTotalSoldQuantity.Text = uReport.totalSoldQuantity.ToString() + " cái " + compareTotalSoldQuantity;
                ltrAverageSoldQuantity.Text = uReport.averageSoldQuantity.ToString() + " cái/ngày";
                ltrTotalRefundQuantity.Text = uReport.totalRefundQuantity.ToString() + " cái (chiếm " + (uReport.totalRefundQuantity * 100 / (uReport.totalSoldQuantity > 0 ? uReport.totalSoldQuantity : 1)) + "%) " + compareTotalRefundQuantity;
                ltrAverageRefundQuantity.Text = uReport.averageRefundQuantity.ToString() + " cái/ngày";
                ltrTotalRemainQuantity.Text = uReport.totalRemainQuantity.ToString() + " cái " + compareTotalRemainQuantity;
                ltrAverageRemainQuantity.Text = uReport.averageRemainQuantity.ToString() + " cái/ngày " + compareAverageRemainQuantity;
                ltrPercentOfSystem.Text = Math.Round(PercentOfSystem, 1).ToString() + "%";
                ltrTotalNewCustomer.Text = uReport.totalNewCustomer.ToString() + " khách mới " + compareNewCustomer;

                // Xử lý biểu đồ
                int day = Convert.ToInt32((todate - fromdate).TotalDays);
                if (day > 1)
                {
                    string chartLabelDays = "";
                    string chartTotalNewCustomer = "";
                    string chartTotalRemainQuantity = "";
                    string chartPercentOfSystem = "";

                    List<string> dataDays = new List<string>();
                    List<string> dataTotalNewCustomer = new List<string>();
                    List<string> dataTotalRemainQuantity = new List<string>();
                    List<string> dataPercentOfSystem = new List<string>();

                    while (fromdate < todate)
                    {
                        var userReport = OrderController.getProductReport(SKU, CategoryID, user, fromdate, todate);
                        var userRefundReport = RefundGoodController.getRefundProductReport(SKU, CategoryID, user, fromdate, todate);
                        var newCustomer = CustomerController.Report(user, fromdate, todate);
                        // Ngày biểu đồ
                        dataDays.Add(String.Format("'{0:d/M}'", fromdate));

                        // Biểu đồ sản lượng
                        int TotalSoldQuantity = userReport.Where(x => x.reportDate.Date == fromdate).Sum(x => x.totalSold);
                        int TotalRefundQuantity = userRefundReport.Where(x => x.reportDate == fromdate).Sum(x => x.totalRefund);
                        dataTotalRemainQuantity.Add((TotalSoldQuantity - TotalRefundQuantity).ToString());

                        // Biểu đồ khách mới
                        int TotalNewCustomer = newCustomer.Where(x => x.CreatedDate.Value.Date == fromdate).Count();
                        dataTotalNewCustomer.Add(TotalNewCustomer.ToString());

                        // Thêm 1 ngày chạy vòng lặp
                        fromdate = fromdate.AddDays(1);
                    }

                    chartLabelDays = String.Join(", ", dataDays);
                    chartTotalNewCustomer = String.Join(", ", dataTotalNewCustomer);
                    chartTotalRemainQuantity = String.Join(", ", dataTotalRemainQuantity);
                    chartPercentOfSystem = String.Join(", ", dataPercentOfSystem);

                    StringBuilder html = new StringBuilder();
                    html.Append("<script>");
                    html.Append("var lineChartData = {");
                    html.Append("	labels: [" + chartLabelDays + "],");
                    html.Append("	datasets: [{");
                    html.Append("		label: 'Sản lượng còn lại',");
                    html.Append("		borderColor: 'rgb(255, 99, 132)',");
                    html.Append("		backgroundColor: 'rgb(255, 99, 132)',");
                    html.Append("		fill: false,");
                    html.Append("		data: [" + chartTotalRemainQuantity + "],");
                    html.Append("		yAxisID: 'y-axis-1',");
                    html.Append("	}]");
                    html.Append("};");
                    html.Append("var lineChartData2 = {");
                    html.Append("	labels: [" + chartLabelDays + "],");
                    html.Append("	datasets: [{");
                    html.Append("		label: 'Khách mới',");
                    html.Append("		borderColor: 'rgb(54, 162, 235)',");
                    html.Append("		backgroundColor: 'rgb(54, 162, 235)',");
                    html.Append("		fill: false,");
                    html.Append("		data: [" + chartTotalNewCustomer + "],");
                    html.Append("		yAxisID: 'y-axis-1'");
                    html.Append("	}]");
                    html.Append("};");
                    html.Append("</script>");

                    ltrChartData.Text = html.ToString();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string SKU = txtTextSearch.Text;
            string CategoryID = ddlCategory.SelectedValue;
            string fromdate = rFromDate.SelectedDate.ToString();
            string todate = rToDate.SelectedDate.ToString();

            Response.Redirect(String.Format("/bao-cao-nhan-vien?SKU={0}&categoryid={1}&fromdate={2}&todate={3}", SKU, CategoryID, fromdate, todate));
        }
    }
}