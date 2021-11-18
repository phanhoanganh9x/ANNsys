<%@ Page Title="Quản lý đơn gộp" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="quan-ly-don-gop.aspx.cs" Inherits="IM_PJ.quan_ly_don_gop" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/moment.min.js"></script>
    <script src="/Scripts/moment-with-locales.min.js"></script>
    <script src="/Scripts/bootstrap-datetimepicker.min.js"></script>
    <link rel="stylesheet" href="App_Themes/Ann/css/pages/quan-ly-don-gop/quan-ly-don-gop.css?v=202111161836" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title left">Quản lý đơn gộp<span id="spanReport"></span></h3>
                    <div class="right above-list-btn">
                        <a href="/tao-don-gop" class="h45-btn primary-btn btn">
                            <i class="fa fa-plus" aria-hidden="true"></i> Tạo đơn gộp
                        </a>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-3 col-xs-6">
                                    <asp:TextBox ID="txtSearch" CssClass="form-control input-code" runat="server" placeholder="Tìm đơn hàng" autocomplete="off" onKeyUp="onKeyUpSearch(event)"></asp:TextBox>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <select id="ddlDeliveryMethod" class="form-control"></select>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <label>Từ ngày</label>
                                    <telerik:RadDateTimePicker RenderMode="Lightweight" ID="dpFromDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate" DateInput-EmptyMessage="Từ ngày">
                                        <DateInput DisplayDateFormat="dd/MM/yyyy HH:mm" runat="server">
                                        </DateInput>
                                    </telerik:RadDateTimePicker>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <label>Đến ngày</label>
                                    <telerik:RadDateTimePicker RenderMode="Lightweight" ID="dpToDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate" DateInput-EmptyMessage="Đến ngày">
                                        <DateInput DisplayDateFormat="dd/MM/yyyy HH:mm" runat="server">
                                        </DateInput>
                                    </telerik:RadDateTimePicker>
                                </div>
                                <div class="col-md-1 col-xs-6 search-button">
                                    <a href="javascript:;" onclick="onClickSearch()" class="btn primary-btn h45-btn"><i class="fa fa-search"></i></a>
                                    <a href="/quan-ly-don-gop" class="btn primary-btn h45-btn"><i class="fa fa-times" aria-hidden="true"></i></a>
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
                            <table id="tbGroupOrder" class="table shop_table_responsive table-checkable table-product">
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

        <%-- Hidden Field --%>
        <asp:HiddenField ID="hdfStaff" runat="server"/>

        <%-- Javascript --%>
        <script type="text/javascript" src="/App_Themes/Ann/js/utils/string-format.js?v=202111161836"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/common/utils-service.js?v=202111161836"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/quan-ly-don-gop/quan-ly-don-gop-service.js?v=202111161836"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/controllers/quan-ly-don-gop/quan-ly-don-gop-controller.js?v=202111161836"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/pages/quan-ly-don-gop/quan-ly-don-gop.js?v=202111161836"></script>
    </main>
</asp:Content>
