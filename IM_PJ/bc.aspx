<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bc.aspx.cs" Inherits="IM_PJ.bc" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Báo cáo</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1, user-scalable=yes">
    <meta name="format-detection" content="telephone=no">
    <meta name="robots" content="noindex, nofollow">
    <link rel="stylesheet" href="/App_Themes/Ann/css/style.css?v=04092021" media="all">
    <link rel="stylesheet" href="/App_Themes/Ann/css/style-P.css?v=04092021" media="all">
    <link rel="stylesheet" href="/App_Themes/Ann/css/style-sp.css?v=04092021" media="all">
    <link href="/App_Themes/NewUI/js/sweet/sweet-alert.css" rel="stylesheet" />
    <script type="text/javascript" src="/App_Themes/Ann/js/jquery-2.1.3.min.js"></script>
    <script type="text/javascript" src="/App_Themes/Ann/js/Chart.min.js"></script>
</head>
<body>
    <form id="form12" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager runat="server" ID="scr">
        </asp:ScriptManager>
        <div>
            <main>
                <div class="container">
                    <div class="row">
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="/sp" class="btn btn-menu primary-btn h45-btn btn-product"><i class="fa fa-sign-in" aria-hidden="true"></i> Sản phẩm</a>
                            </div>
                        </div>
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="/bv" class="btn btn-menu primary-btn h45-btn btn-post"><i class="fa fa-sign-in" aria-hidden="true"></i> Bài viết</a>
                            </div>
                        </div>
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="/dang-ky-nhap-hang" class="btn btn-menu primary-btn h45-btn btn-order"><i class="fa fa-cart-plus" aria-hidden="true"></i> Đặt hàng</a>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="/kh" class="btn primary-btn h45-btn btn-customer"><i class="fa fa-sign-in" aria-hidden="true"></i> Khách hàng</a>
                            </div>
                        </div>
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="/bc" class="btn primary-btn h45-btn btn-report"><i class="fa fa-sign-in" aria-hidden="true"></i> Báo cáo</a>
                            </div>
                        </div>
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="/nhan-vien-dat-hang" class="btn primary-btn h45-btn btn-list-order"><i class="fa fa-cart-plus" aria-hidden="true"></i> DS đặt hàng</a>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 col-md-offset-3 col-xs-12">
                            <div class="filter-above-wrap clear">
                                <div class="filter-control">
                                    <div class="row">
                                        <div class="col-md-5 margin-bottom-15">
                                            <label>Từ ngày:</label>
                                            <telerik:RadDatePicker RenderMode="Lightweight" ID="rFromDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate" MinDate="01/01/2018">
                                                <DateInput DisplayDateFormat="dd/MM/yyyy" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                        </div>
                                        <div class="col-md-5 margin-bottom-15">
                                            <label>Đến ngày</label>
                                            <telerik:RadDatePicker RenderMode="Lightweight" ID="rToDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate" MinDate="01/01/2018">
                                                <DateInput DisplayDateFormat="dd/MM/yyyy" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                        </div>
                                        <div class="col-md-2 margin-bottom-15">
                                            <a href="javascript:;" onclick="searchReport()" class="btn primary-btn h45-btn"><i class="fa fa-search"></i> Xem</a>
                                            <asp:Button ID="btnSearch" runat="server" CssClass="btn primary-btn h45-btn" OnClick="btnSearch_Click" Style="display: none" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-table clear">
                                <div class="row margin-bottom-15">
                                    <div class="col-md-12">
                                        <h3>Báo cáo nhân viên</h3>
                                        <p><asp:Literal ID="ltrAccount" runat="server" EnableViewState="false"></asp:Literal></p>
                                    </div>
                                </div>
                                <div class="row margin-bottom-15">
                                    <div class="col-md-6">
                                        <div class="report-column">
                                            <div class="report-label">
                                                Tổng sản lượng (đã trừ hàng trả):
                                            </div>
                                            <div class="report-value">
                                                <asp:Literal ID="ltrTotalRemainQuantity" runat="server" EnableViewState="false"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="report-column">
                                            <div class="report-label">
                                                Sản lượng mỗi ngày (đã trừ hàng trả):
                                            </div>
                                            <div class="report-value">
                                                <asp:Literal ID="ltrAverageRemainQuantity" runat="server" EnableViewState="false"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row margin-bottom-15">
                                    <div class="col-md-6">
                                        <div class="report-column">
                                            <div class="report-label">
                                                Tổng số đơn hàng hoàn tất:
                                            </div>
                                            <div class="report-value">
                                                <asp:Literal ID="ltrTotalSaleOrder" runat="server" EnableViewState="false"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="report-column">
                                            <div class="report-label">
                                                Số đơn hoàn tất mỗi ngày:
                                            </div>
                                            <div class="report-value">
                                                <asp:Literal ID="ltrAverageSaleOrder" runat="server" EnableViewState="false"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row margin-bottom-15">
                                    <div class="col-md-6">
                                        <div class="report-column">
                                            <div class="report-label">
                                                Tổng số bán ra:
                                            </div>
                                            <div class="report-value">
                                                <asp:Literal ID="ltrTotalSoldQuantity" runat="server" EnableViewState="false"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="report-column">
                                            <div class="report-label">
                                                Bán ra mỗi ngày:
                                            </div>
                                            <div class="report-value">
                                                <asp:Literal ID="ltrAverageSoldQuantity" runat="server" EnableViewState="false"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row margin-bottom-15">
                                    <div class="col-md-6">
                                        <div class="report-column">
                                            <div class="report-label">
                                                Tổng số đổi trả:
                                            </div>
                                            <div class="report-value">
                                                <asp:Literal ID="ltrTotalRefundQuantity" runat="server" EnableViewState="false"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="report-column">
                                            <div class="report-label">
                                                Đổi trả mỗi ngày:
                                            </div>
                                            <div class="report-value">
                                                <asp:Literal ID="ltrAverageRefundQuantity" runat="server" EnableViewState="false"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row margin-bottom-15">
                                    <div class="col-md-6">
                                        <div class="report-column">
                                            <div class="report-label">
                                                Phần trăm / hệ thống:
                                            </div>
                                            <div class="report-value">
                                                <asp:Literal ID="ltrPercentOfSystem" runat="server" EnableViewState="false"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="report-column">
                                            <div class="report-label">
                                                Tổng số khách mới:
                                            </div>
                                            <div class="report-value">
                                                <asp:Literal ID="ltrTotalNewCustomer" runat="server" EnableViewState="false"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row margin-bottom-15">
                        <div class="col-md-12">
                            <canvas id="canvas"></canvas>
                        </div>
                    </div>
                    <div class="row margin-bottom-15">
                        <div class="col-md-12">
                            <canvas id="canvas2"></canvas>
                        </div>
                    </div>
                    <div class="row margin-bottom-15">
                        <div class="col-md-12">
                            <canvas id="canvas3"></canvas>
                        </div>
                    </div>
                </div>
                <asp:Literal ID="ltrChartData" runat="server" EnableViewState="false"></asp:Literal>
            </main>
            <script>
                window.onload = function () {
                    if (typeof lineChartData !== 'undefined')
                    {
                        var ctx = document.getElementById('canvas').getContext('2d');
                        window.myLine = Chart.Line(ctx, {
                            data: lineChartData,
                            options: {
                                responsive: true,
                                aspectRatio: 3,
                                hoverMode: 'index',
                                stacked: false,
                                title: {
                                    display: true,
                                    text: 'Biểu đồ sản lượng còn lại'
                                },
                                scales: {
                                    yAxes: [{
                                        type: 'linear', // only linear but allow scale type registration. This allows extensions to exist solely for log scale for instance
                                        display: true,
                                        position: 'left',
                                        id: 'y-axis-1',
                                        ticks: {
                                            beginAtZero: true
                                        }
                                    }],
                                }
                            }
                        });
                    }

                    if (typeof lineChartData2 !== 'undefined') {
                        var ctx = document.getElementById('canvas2').getContext('2d');
                        window.myLine = Chart.Line(ctx, {
                            data: lineChartData2,
                            options: {
                                responsive: true,
                                aspectRatio: 3,
                                hoverMode: 'index',
                                stacked: false,
                                title: {
                                    display: true,
                                    text: 'Biểu đồ khách mới'
                                },
                                scales: {
                                    yAxes: [{
                                        type: 'linear', // only linear but allow scale type registration. This allows extensions to exist solely for log scale for instance
                                        display: true,
                                        position: 'left',
                                        id: 'y-axis-1',
                                        ticks: {
                                            beginAtZero: true
                                        }
                                    }],
                                }
                            }
                        });
                    }
		        };
	        </script>
            <script type="text/javascript">
                function searchReport() {
                    $("#<%= btnSearch.ClientID%>").click();
                }
            </script>
        </div>
    </form>
</body>
</html>