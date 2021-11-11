<%@ Page Title="Tạo đơn gộp" Language="C#" MasterPageFile="~/MasterPage.Master"  AutoEventWireup="true" CodeBehind="tao-don-gop.aspx.cs" Inherits="IM_PJ.tao_don_gop" EnableSessionState="ReadOnly" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="App_Themes/Ann/css/pages/tao-don-gop/tao-don-gop.css?v=202111091556" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panelborderheading">
                        <div class="panel-heading clear">
                            <h3 class="page-title left not-margin-bot">Tạo đơn gộp</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-5">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtCode" CssClass="form-control input-code" runat="server" placeholder="Mã đơn hàng" autocomplete="off" onKeyDown="onKeyDownCode(event)"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <a href="javascript:;" class="btn primary-btn fw-btn not-fullwidth btn-add" onClick="onClickOrderAddition()"><i class="fa fa-plus" aria-hidden="true"></i> Thêm</a>
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
                                    <th class="col-order-id">Mã</th>
                                    <th class="col-customer">Khách hàng</th>
                                    <th class="col-quatity">Mua</th>
                                    <th class="col-order-status">Xử lý</th>
                                    <th class="col-payment-status">Thanh toán</th>
                                    <th class="col-payment-method">Kiểu thanh toán</th>
                                    <th class="col-delivery-method">Giao hàng</th>
                                    <th class="col-total-price">Tổng tiền</th>
                                    <th class="col-staff">Nhân viên</th>
                                    <th class="col-created-date">Ngày tạo</th>
                                    <th class="col-done-date">Hoàn tất</th>
                                    <th class="col-btn"></th>
                                </tr>
                            </thead>
                            <tbody id="tbody-order">
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 col-md-offset-10">
                    <a href="javascript:;" id="btnSubmit" class="btn primary-btn fw-btn hidden" onclick="submitGroupOrder()">
                        <i class="fa fa-floppy-o" aria-hidden="true"></i> Khởi tạo
                    </a>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdfStaff" runat="server" />
        <script type="text/javascript" src="App_Themes/Ann/js/utils/string-format.js?v=202111091556"></script>
        <script type="text/javascript" src="App_Themes/Ann/js/services/common/utils-service.js?v=202111091556"></script>
        <script type="text/javascript" src="App_Themes/Ann/js/services/tao-don-gop/tao-don-gop-service.js?v=202111091556"></script>
        <script type="text/javascript" src="App_Themes/Ann/js/controllers/tao-don-gop/tao-don-gop-controller.js?v=202111091556"></script>
        <script type="text/javascript" src="App_Themes/Ann/js/pages/tao-don-gop/tao-don-gop.js?v=202111091556"></script>
    </main>
</asp:Content>