<%@ Page Title="Danh sách khách giảm giá" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="danh-sach-khach-giam-gia.aspx.cs" Inherits="IM_PJ.danh_sach_khach_giam_gia" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="App_Themes/Ann/css/pages/danh-sach-khach-giam-gia/danh-sach-khach-giam-gia.css?v=04092021" />
    <link rel="stylesheet" href="App_Themes/Ann/css/common/search-customer.css?v=04092021" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panelborderheading">
                        <div class="panel-heading clear">
                            <h3 class="page-title left not-margin-bot">Thêm khách hàng vào nhóm: <strong>
                                <asp:Literal ID="ltrGroupName" runat="server" EnableViewState="false"></asp:Literal></strong></h3>
                            <a href="/danh-sach-nhom-khach-hang" class="btn primary-btn fw-btn not-fullwidth right">Trở về</a>
                        </div>
                        <div class="panel-body">
                            <div class="form-row">
                                <div class="col-xs-3">
                                    <label class="text-left">Số lượng khách: </label>
                                    <asp:Literal ID="ltrNumberCustomer" runat="server"></asp:Literal>
                                </div>
                                <div class="col-xs-3">
                                    <label class="text-left">Chiết khấu: </label>
                                    <asp:Literal ID="ltrDiscount" runat="server"></asp:Literal>
                                </div>
                                <div class="col-xs-3">
                                    <label class="text-left">Số lượng yêu cầu: </label>
                                    <asp:Literal ID="ltrQuantityRequired" runat="server"></asp:Literal>
                                </div>
                                <div class="col-xs-3">
                                    <label class="text-left">Số lượng tối thiểu: </label>
                                    <asp:Literal ID="ltrQuantityProduct" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="col-xs-3">
                                    <label class="text-left">Phí đổi trả hàng: </label>
                                    <asp:Literal ID="ltrFeeRefund" runat="server"></asp:Literal>
                                </div>
                                <div class="col-xs-3">
                                    <label class="text-left">Số ngày đổi trả: </label>
                                    <asp:Literal ID="ltrNumberOfDateToChnageProduct" runat="server"></asp:Literal>
                                </div>
                                <div class="col-xs-3">
                                    <label class="text-left">Số lượng đổi/số ngày: </label>
                                    <asp:Literal ID="ltrNumberOfProductCanChange" runat="server"></asp:Literal>
                                </div>
                                <div class="col-xs-3">
                                    <label class="text-left">Số lượng đổi miễn phí: </label>
                                    <asp:Literal ID="ltrRefundQuantityNoFee" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div class="form-row">
                                <div class="search-box">
                                    <input type="text" id="txtSearch" class="form-control" placeholder="Nhập thông tin khách hàng (F1)" autocomplete="off" onkeypress="txtSearchKeyPress(event)" />
                                    <button type="button" class="btn btn-primary" onclick="showSearchCustomerModal()"><i class="fa fa-search"></i></button>
                                    <asp:Button ID="btnAddCustomer" runat="server" OnClick="btnAddCustomer_Click" style="display: none;"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel-table clear">
                        <div class="responsive-table">
                            <table class="table table-checkable table-product">
                                <tbody>
                                    <tr>
                                        <th>Họ tên</th>
                                        <th>Nick</th>
                                        <th>Điện thoại</th>
                                        <th>Nhân viên</th>
                                        <th>Ngày vào nhóm</th>
                                        <th>Thao tác</th>
                                    </tr>
                                    <asp:Literal ID="ltrList" runat="server" EnableViewState="false"></asp:Literal>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Check Order Modal -->
        <div class="modal fade" id="modal-order" role="dialog" data-backdrop="false">
            <div class="modal-dialog modal-lg">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title" data-id="custom-name">Danh sách đơn hàng</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group" data-id="nick"></div>
                        <div class="form-group" data-id="phone"></div>
                        <div class="form-group" data-id="address"></div>
                        <div class="form-group" data-id="discount" style="display: none;"></div>
                        <div class="form-group">
                            <table class="table table-striped">
                                <thead style="width: 100%;">
                                    <tr>
                                        <th class="col-xs-2">Mã</th>
                                        <th class="col-xs-2">Số lượng</th>
                                        <th class="col-xs-2">Tiền</th>
                                        <th class="col-xs-2">Shipping</th>
                                        <th class="col-xs-2">Nhân viên</th>
                                        <th class="col-xs-2">Ngày tạo</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdfDiscountGroupID" runat="server" />
        <asp:HiddenField ID="hdfCustomer" runat="server" />
        <asp:HiddenField ID="hdfOrder" runat="server" />

        <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Style="display: none" />
        <script type="text/javascript" src="/App_Themes/Ann/js/HoldOn.js"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/common/utils-service.js?v=04092021"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/models/common/discount-group-model.js?v=04092021"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/models/common/customer-model.js?v=04092021"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/models/common/order-model.js?v=04092021"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/common/search-customer-service.js?v=04092021"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/controllers/common/search-customer-controller.js?v=04092021"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/danh-sach-khach-giam-gia/danh-sach-khach-giam-gia-service.js?v=04092021"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/controllers/danh-sach-khach-giam-gia/danh-sach-khach-giam-gia-controller.js?v=04092021"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/pages/danh-sach-khach-giam-gia/danh-sach-khach-giam-gia.js?v=04092021"></script>
    </main>
</asp:Content>
