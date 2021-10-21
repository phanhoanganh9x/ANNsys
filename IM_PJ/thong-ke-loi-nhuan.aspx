<%@ Page Title="Lợi nhuận" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="thong-ke-loi-nhuan.aspx.cs" Inherits="IM_PJ.thong_ke_loi_nhuan" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/Ann/js/Chart.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title left">Thống kê lợi nhuận</h3>
                    <div class="right above-list-btn">
                        <a href="/bao-cao" class="h45-btn btn" style="background-color: #ff3f4c">Trở về</a>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-5">
                                </div>
                                <div class="col-md-3">
                                    <label>Từ ngày</label>
                                    <telerik:RadDatePicker RenderMode="Lightweight" ID="rFromDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate" MinDate="01/01/2018">
                                        <DateInput DisplayDateFormat="dd/MM/yyyy" runat="server">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </div>
                                <div class="col-md-3">
                                    <label>Đến ngày</label>
                                    <telerik:RadDatePicker RenderMode="Lightweight" ID="rToDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate" MinDate="01/01/2018">
                                        <DateInput DisplayDateFormat="dd/MM/yyyy" runat="server">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </div>
                                <div class="col-md-1">
                                    <a href="javascript:;" onclick="submitReport()" class="btn primary-btn h45-btn"><i class="fa fa-search"></i></a>
                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn primary-btn h45-btn" OnClick="btnSearch_Click" Style="display: none" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-table clear">
                        <div class="responsive-table">
                            <div class="row margin-bottom-15">
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            Tổng lợi nhuận:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrTotalProfit" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            Lợi nhuận mỗi ngày:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrProfitPerDay" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            Lợi nhuận mỗi đơn hàng:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrProfitPerOrder" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row margin-bottom-15">
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            SL còn lại:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrTotalRemainQuantity" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            SL còn lại mỗi ngày:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrAverageRemainQuantity" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            SL bán ra:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrTotalSoldQuantity" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row margin-bottom-15">
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            Tổng doanh thu bán ra:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrTotalSalePrice" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            Tổng giá vốn bán ra:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrTotalSaleCost" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            Tổng chiết khấu:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrTotalDisount" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row margin-bottom-15">
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            Tổng tiền hàng trả:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrTotalRefundPrice" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            Tổng vốn hàng trả:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrTotalRefundCost" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            Tổng phí hàng trả:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrTotalRefundFee" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row margin-bottom-15">
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            Tổng doanh thu thực tế:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrTotalActualRevenue" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            Tổng phí vận chuyển:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrTotalShippingFee" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            Tổng phí khác:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrTotalOtherFee" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                </div>
                            </div>
                            <div class="row margin-bottom-15">
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            Tổng giảm giá:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrTotalCouponValue" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            Tổng đơn chuyển hoàn:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrTotalReturnCount" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="report-column">
                                        <div class="report-label">
                                            Tổng phí chuyển hoàn:
                                        </div>
                                        <div class="report-value">
                                            <asp:Literal ID="ltrTotalReturnFee" runat="server" EnableViewState="false"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row margin-bottom-15">
                                <div class="col-md-12">
                                    <canvas id="canvas"></canvas>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <script type="text/javascript">
            function submitReport() {
                $("#<%= btnSearch.ClientID%>").click();
            }
        </script>
        <asp:Literal ID="ltrChartData" runat="server" EnableViewState="false"></asp:Literal>
        <script>
            window.onload = function () {
                if (typeof lineChartData !== 'undefined')
                {
                    var ctx = document.getElementById('canvas').getContext('2d');
                    window.myLine = Chart.Line(ctx, {
                        data: lineChartData,
                        options: {
                            responsive: true,
                            aspectRatio: 3.5,
                            hoverMode: 'index',
                            stacked: false,
                            title: {
                                display: true,
                                text: 'Biểu đồ lợi nhuận'
                            },
                            scales: {
                                yAxes: [{
                                    type: 'linear', // only linear but allow scale type registration. This allows extensions to exist solely for log scale for instance
                                    display: true,
                                    position: 'left',
                                    id: 'y-axis-1',
                                    ticks: {
                                        beginAtZero: true,
                                        callback: function (label, index, labels) {
                                            return label.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                                        }
                                    }
                                }, {
                                    type: 'linear', // only linear but allow scale type registration. This allows extensions to exist solely for log scale for instance
                                    display: true,
                                    position: 'right',
                                    id: 'y-axis-2',
                                    ticks: {
                                        beginAtZero: true,
                                        callback: function (label, index, labels) {
                                            return label.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                                        }
                                    },
                                    // grid line settings
                                    gridLines: {
                                        drawOnChartArea: false, // only want the grid lines for one axis to show up
                                    },
                                }],
                            },
                            tooltips: {
                                callbacks: {
                                    label: function (tooltipItems, data) {
                                        let label = data.datasets[tooltipItems.datasetIndex].label;
                                        let value = tooltipItems.yLabel.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

                                        return label + ' : ' + value;
                                    }
                                }
                            }
                        }
                    });
                }
		    };
	    </script>
    </main>
</asp:Content>
