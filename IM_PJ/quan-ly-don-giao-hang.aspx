<%@ Page Title="Quản lý vận đơn" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="quan-ly-don-giao-hang.aspx.cs" Inherits="IM_PJ.quan_ly_don_giao_hang" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/moment.min.js"></script>
    <script src="/Scripts/moment-with-locales.min.js"></script>
    <script src="/Scripts/bootstrap-datetimepicker.min.js"></script>
    <link rel="stylesheet" href="App_Themes/Ann/css/pages/quan-ly-don-giao-hang/quan-ly-don-giao-hang.css?v=202109271901" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title left">Quản lý vận đơn <span id="spanReport"></span></h3>
                    <div class="right above-list-btn">
                        <a href="/dang-ky-gui-di" class="h45-btn btn-green btn">
                            <i class="fa fa-paper-plane" aria-hidden="true"></i> Gửi hàng đi
                        </a>
                        <a href="/dang-ky-chuyen-hoan" class="h45-btn primary-btn btn">
                            <i class="fa fa-undo" aria-hidden="true"></i> Chuyển hoàn
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
                                    <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-control dropdown-order-type" onChange="onChangeOrderType($(this).val())"></asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <select id="ddlDeliveryMethod" class="form-control"></select>
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
                                    <a href="/quan-ly-don-giao-hang" class="btn primary-btn h45-btn"><i class="fa fa-times" aria-hidden="true"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-7 col-xs-6">
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlCreatedBy" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-1 col-xs-6">
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
                            <table id="tbDelivery" class="table shop_table_responsive table-checkable table-product">
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

        <asp:HiddenField ID="hdfRole" runat="server" />

        <script type="text/javascript" src="/App_Themes/Ann/js/utils/string-format.js?v=202109250230"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/common/utils-service.js?v=202109250230"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/quan-ly-don-giao-hang/quan-ly-don-giao-hang-service.js?v=202109271901"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/controllers/quan-ly-don-giao-hang/quan-ly-don-giao-hang-controller.js?v=202109271901"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/pages/quan-ly-don-giao-hang/quan-ly-don-giao-hang.js?v=202109271901"></script>
    </main>
</asp:Content>
