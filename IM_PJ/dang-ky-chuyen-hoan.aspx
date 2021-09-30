<%@ Page Title="Thêm đơn hàng chuyển hoàn" Language="C#" MasterPageFile="~/MasterPage.Master"  AutoEventWireup="true" CodeBehind="dang-ky-chuyen-hoan.aspx.cs" Inherits="IM_PJ.dang_ky_chuyen_hoan" EnableSessionState="ReadOnly" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="App_Themes/Ann/css/pages/dang-ky-chuyen-hoan/dang-ky-chuyen-hoan.css?v=202109271901" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panelborderheading">
                        <div class="panel-heading clear">
                            <h3 class="page-title left not-margin-bot">Thêm đơn hàng chuyển hoàn</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-5">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-control dropdown-order-type" onChange="onChangeOrderType()"></asp:DropDownList>
                                            <asp:TextBox ID="txtCode" CssClass="form-control input-code" runat="server" placeholder="Mã đơn hàng" autocomplete="off" onKeyUp="onKeyUpCode(event)"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <select id="ddlDeliveryMethod" class="form-control" onChange="onChangeDeliveryMethod()" disabled></select>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>Ngày chuyển hoàn</label>
                                        <telerik:RadDateTimePicker RenderMode="Lightweight" ID="rRefundDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate" DateInput-EmptyMessage="Ngày chuyển hoàn">
                                            <DateInput DisplayDateFormat="dd/MM/yyyy HH:mm" runat="server">
                                            </DateInput>
                                        </telerik:RadDateTimePicker>
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
                                    <th class="col-refund-date">Ngày chuyền hoàn</th>
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

        <asp:HiddenField ID="hdfSentDate" runat="server" />
        <asp:HiddenField ID="hdfShippingCode" runat="server" />
        <asp:HiddenField ID="hdfStaff" runat="server" />
        <asp:HiddenField ID="hdfIsNew" runat="server" value="1"/>
        <script type="text/javascript" src="App_Themes/Ann/js/utils/string-format.js?v=202109302253"></script>
        <script type="text/javascript" src="App_Themes/Ann/js/services/dang-ky-chuyen-hoan/dang-ky-chuyen-hoan-service.js?v=202109271901"></script>
        <script type="text/javascript" src="App_Themes/Ann/js/controllers/dang-ky-chuyen-hoan/dang-ky-chuyen-hoan-controller.js?v=202109271901"></script>
        <script type="text/javascript" src="App_Themes/Ann/js/pages/dang-ky-chuyen-hoan/dang-ky-chuyen-hoan.js?v=202109302151"></script>
    </main>
</asp:Content>