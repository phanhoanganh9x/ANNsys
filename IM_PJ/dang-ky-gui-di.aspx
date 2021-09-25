<%@ Page Title="Thêm đơn hàng gửi đi" Language="C#" MasterPageFile="~/MasterPage.Master"  AutoEventWireup="true" CodeBehind="dang-ky-gui-di.aspx.cs" Inherits="IM_PJ.dang_ky_gui_di" EnableSessionState="ReadOnly" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="App_Themes/Ann/css/pages/dang-ky-gui-di/dang-ky-gui-di.css?v=202109250230" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panelborderheading">
                        <div class="panel-heading clear">
                            <h3 class="page-title left not-margin-bot">Thêm đơn hàng gửi đi</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-5">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-control dropdown-order-type" onChange="onChangeOrderType($(this).val())"></asp:DropDownList>
                                            <asp:TextBox ID="txtCode" CssClass="form-control input-code" runat="server" placeholder="Mã đơn hàng" autocomplete="off" onBlur="onBlurCode()" onKeyUp="onKeyUpCode(event)"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlDeliveryMethod" runat="server" CssClass="form-control dropdown-delivery-method" onChange="onChangeDeliveryMethod($(this).val())"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>Ngày gửi</label>
                                        <telerik:RadDatePicker RenderMode="Lightweight" ID="rSentDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate">
                                            <DateInput DisplayDateFormat="dd/MM/yyyy" runat="server">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <a href="javascript:;" class="btn primary-btn fw-btn not-fullwidth btn-add" onClick="onClickDeliveryAddition()"><i class="fa fa-plus" aria-hidden="true"></i> Thêm</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel">
                        <table class="table">
                            <thead>
                                <tr>
                                    <th class="col-index">#</th>
                                    <th class="col-order-type">Đơn hàng</th>
                                    <th class="col-code">Mã đơn</th>
                                    <th class="col-delivery-method">Vận chuyển</th>
                                    <th class="col-shipping-code">Mã vận đơn</th>
                                    <th class="col-sent-date">Ngày gửi</th>
                                    <th class="col-btn"></th>
                                </tr>
                            </thead>
                            <tbody id="tbody-delivery">
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 col-md-offset-10">
                    <a href="javascript:;" id="btnSubmit" class="btn primary-btn fw-btn hidden" onclick="submitDeliveries()">
                        <i class="fa fa-floppy-o" aria-hidden="true"></i> Cập nhật
                    </a>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdfShippingCode" runat="server" />
        <asp:HiddenField ID="hdfStaff" runat="server" />
        <script type="text/javascript" src="App_Themes/Ann/js/utils/string-format.js?v=202109250230"></script>
        <script type="text/javascript" src="App_Themes/Ann/js/services/dang-ky-gui-di/dang-ky-gui-di-service.js?v=202109250230"></script>
        <script type="text/javascript" src="App_Themes/Ann/js/controllers/dang-ky-gui-di/dang-ky-gui-di-controller.js?v=202109250230"></script>
        <script type="text/javascript" src="App_Themes/Ann/js/pages/dang-ky-gui-di/dang-ky-gui-di.js?v=202109250230"></script>
    </main>
</asp:Content>