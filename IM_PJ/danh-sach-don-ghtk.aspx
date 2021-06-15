<%@ Page Title="Danh sách đơn GHTK" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="danh-sach-don-ghtk.aspx.cs" Inherits="IM_PJ.danh_sach_don_ghtk" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/moment.min.js"></script>
    <script src="/Scripts/moment-with-locales.min.js"></script>
    <script src="/Scripts/bootstrap-datetimepicker.min.js"></script>
    <link rel="stylesheet" href="App_Themes/Ann/css/pages/danh-sach-don-ghtk/danh-sach-don-ghtk.css?v=202106151531" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title left">Danh sách đơn GHTK <span id="spanReport"></span></h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-3 col-xs-6">
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Tìm đơn hàng" autocomplete="off" onKeyUp="onKeyUpSearch(event)"></asp:TextBox>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <select id="ddlGhtkStatus" class="form-control">
                                    </select>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <label>Từ ngày</label>
                                    <telerik:RadDatePicker RenderMode="Lightweight" ID="dpFromDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate">
                                        <DateInput DisplayDateFormat="dd/MM/yyyy" runat="server">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <label>Đến ngày</label>
                                    <telerik:RadDatePicker RenderMode="Lightweight" ID="dpToDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate">
                                        <DateInput DisplayDateFormat="dd/MM/yyyy" runat="server">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </div>
                                <div class="col-md-1 col-xs-6 search-button">
                                    <a href="javascript:;" onclick="onClickSearch()" class="btn primary-btn h45-btn"><i class="fa fa-search"></i></a>
                                    <a href="/danh-sach-don-ghtk" class="btn primary-btn h45-btn"><i class="fa fa-times" aria-hidden="true"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-table clear">
                        <div class="panel-footer clear">
                            <div class="pagination">
                            </div>
                        </div>
                        <div class="responsive-table">
                            <table id="tbReport" class="table shop_table_responsive table-checkable table-product table-new-product">
                            </table>
                        </div>
                        <div class="panel-footer clear">
                            <div class="pagination">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdfStaff" runat="server" />

        <script type="text/javascript" src="/App_Themes/Ann/js/utils/string-format.js?v=202106061418"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/common/utils-service.js?v=202106061418"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/danh-sach-don-ghtk/danh-sach-don-ghtk-service.js?v=202106151531"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/controllers/danh-sach-don-ghtk/danh-sach-don-ghtk-controller.js?v=202106151531"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/pages/danh-sach-don-ghtk/danh-sach-don-ghtk.js?v=202106151531"></script>
    </main>
</asp:Content>
