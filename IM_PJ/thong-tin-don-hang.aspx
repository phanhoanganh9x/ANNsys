﻿<%@ Page Title="Thông tin đơn hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="thong-tin-don-hang.aspx.cs" Inherits="IM_PJ.thong_tin_don_hang" EnableSessionState="ReadOnly" enableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/App_Themes/Ann/js/search-customer.js?v=2117"></script>
    <script src="/App_Themes/Ann/js/search-product.js?v=07122018"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="parent" runat="server">
        <main id="main-wrap">
            <div class="container">
                <div id="infor-order" class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">
                                    <asp:Literal ID="ltrHeading" runat="server"></asp:Literal></h3>
                            </div>
                            <div class="panel-body">
                                <div class="row pad">
                                    <div class="col-md-3">
                                        <label class="left pad10">Loại đơn: </label>
                                        <div class="ordertype">
                                            <asp:Literal ID="ltrOrderType" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Nhân viên: </label>
                                        <div class="ordercreateby">
                                            <asp:Literal ID="ltrCreateBy" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Ngày tạo: </label>
                                        <div class="ordercreatedate">
                                            <asp:Literal ID="ltrCreateDate" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Hoàn tất: </label>
                                        <div class="orderdatedone">
                                            <asp:Literal ID="ltrDateDone" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="row pad">
                                    <div class="col-md-3">
                                        <label class="left pad10">Số lượng: </label>
                                        <div class="orderquantity">
                                            <asp:Literal ID="ltrOrderQuantity" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Tổng tiền: </label>
                                        <div class="ordertotalprice">
                                            <asp:Literal ID="ltrOrderTotalPrice" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Trạng thái: </label>
                                        <div class="orderstatus">
                                            <asp:Literal ID="ltrOrderStatus" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Ghi chú: </label>
                                        <div class="ordernote">
                                            <asp:Literal ID="ltrOrderNote" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Literal ID="ltrPrint" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="infor-customer" class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Thông tin khách hàng</h3>
                                <a href="javascript:;" class="search-customer" onclick="searchCustomer()"><i class="fa fa-search" aria-hidden="true"></i>Tìm khách hàng (F1)</a>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Họ tên</label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFullname" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtFullname" CssClass="form-control capitalize" runat="server" Enabled="false" placeholder="Họ tên thật của khách (F2)" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Điện thoại</label>
                                            <asp:RequiredFieldValidator ID="re" runat="server" ControlToValidate="txtPhone" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtPhone" CssClass="form-control" onblur="ajaxCheckCustomer()" runat="server" Enabled="false" placeholder="Số điện thoại khách hàng" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Nick đặt hàng</label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNick" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtNick" CssClass="form-control capitalize" runat="server" Enabled="false" placeholder="Tên nick đặt hàng" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Địa chỉ</label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAddress" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtAddress" CssClass="form-control capitalize" runat="server" Enabled="false" placeholder="Địa chỉ khách hàng" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Zalo</label>
                                            <asp:TextBox ID="txtZalo" CssClass="form-control" runat="server" Enabled="false" placeholder="Số điện thoại Zalo" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Facebook</label>
                                            <div class="row">
                                                <div class="col-md-10 fb">
                                                    <asp:TextBox ID="txtFacebook" CssClass="form-control" runat="server" Enabled="false" placeholder="Đường link chat Facebook" autocomplete="off"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="row">
                                                        <span class="link-facebook">
                                                            <asp:Literal ID="ltrFb" runat="server"></asp:Literal></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12 view-detail">
                                        <asp:Literal ID="ltrViewDetail" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12 discount-info">
                                        <asp:Literal ID="ltrDiscountInfo" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="detail" class="row">
                    <div class="col-md-12">
                        <div class="panel-post">
                            <asp:Literal ID="ltrCustomerType" runat="server"></asp:Literal>
                            <div class="post-above clear">
                                <div class="search-box left">
                                    <input type="text" id="txtSearch" class="form-control sku-input" placeholder="NHẬP MÃ SẢN PHẨM (F3)" autocomplete="off">
                                </div>
                                <div class="right">
                                    <a href="javascript:;" class="link-btn" onclick="searchProduct()"><i class="fa fa-search"></i></a>
                                </div>
                            </div>
                            <div class="post-body search-product-content clear">
                                <div class="search-product-content">
                                    <table class="table table-checkable table-product table-sale-order">
                                        <thead>
                                            <tr>
                                                <th class="order-item">#</th>
                                                <th class="image-item">Ảnh</th>
                                                <th class="name-item">Sản phẩm</th>
                                                <th class="sku-item">Mã</th>
                                                <th class="variable-item">Thuộc tính</th>
                                                <th class="price-item">Giá</th>
                                                <th class="quantity-item">Kho</th>
                                                <th class="quantity-item">Số lượng</th>
                                                <th class="total-item">Thành tiền</th>
                                                <th class="trash-item">Xóa</th>
                                            </tr>
                                        </thead>
                                        <tbody class="content-product">
                                            <asp:Literal ID="ltrProducts" runat="server"></asp:Literal>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Số lượng</div>
                                <div class="right totalproductQuantity">
                                    <asp:Literal ID="ltrProductQuantity" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Thành tiền</div>
                                <div class="right totalpriceorder">
                                    <asp:Literal ID="ltrTotalNotDiscount" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Chiết khấu</div>
                                <div class="right totalDiscount">
                                    <telerik:RadNumericTextBox runat="server" CssClass="form-control width-notfull" Skin="MetroTouch"
                                        ID="pDiscount" MinValue="0" NumberFormat-GroupSizes="3" Value="0" NumberFormat-DecimalDigits="0"
                                        oninput="countTotal()" IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
                                    </telerik:RadNumericTextBox>
                                </div>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Sau chiết khấu</div>
                                <div class="right priceafterchietkhau">
                                    <asp:Literal ID="ltrTotalAfterCK" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Phí vận chuyển</div>
                                <div class="right totalDiscount">
                                    <a class="btn btn-feeship link-btn" href="javascript:;" id="calfeeship" onclick="calFeeShip()"><i class="fa fa-check-square-o" aria-hidden="true"></i> Miễn phí</a>
                                    <telerik:RadNumericTextBox runat="server" CssClass="form-control width-notfull" Skin="MetroTouch"
                                        ID="pFeeShip" MinValue="0" NumberFormat-GroupSizes="3" Value="0" NumberFormat-DecimalDigits="0"
                                        oninput="countTotal()" IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
                                    </telerik:RadNumericTextBox>
                                </div>
                            </div>
                            <div id="fee-list"></div>
                            <div class="post-row clear">
                                <div class="left">Tổng tiền</div>
                                <div class="right totalpriceorderall price-red">
                                    <asp:Literal ID="ltrTotalprice" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="post-row clear returnorder hide">
                                <div class="left">
                                    Trừ hàng trả
                                    <a href="javascript:;" class="find2 hide btn btn-feeship link-btn"></a>
                                    <a href="javascript:;" class="find3 hide btn btn-feeship link-btn btn-edit-fee" onclick="searchReturnOrder()"><i class="fa fa-refresh" aria-hidden="true"></i> Chọn đơn khác</a>
                                    <a href="javascript:;" class="find3 hide btn btn-feeship link-btn" onclick="deleteReturnOrder()"><i class="fa fa-times" aria-hidden="true"></i> Bỏ qua</a>
                                </div>
                                <div class="right totalpriceorderrefund">
                                    <asp:Literal runat="server" ID="ltrTotalPriceRefund"></asp:Literal>
                                </div>
                            </div>
                            <div class="post-row clear refund hide">
                                <div class="left">Tổng tiền còn lại</div>
                                <div class="right totalpricedetail">
                                    <asp:Literal runat="server" ID="ltrtotalpricedetail"></asp:Literal>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="status" class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Trạng thái đơn hàng</h3>
                            </div>
                            <div class="panel-body">
                                <div id="row-excute-status" class="form-row">
                                    <div class="row-left">
                                        Trạng thái xử lý
                                    </div>
                                    <div class="row-right">
                                        <asp:DropDownList ID="ddlExcuteStatus" runat="server" CssClass="form-control" onchange="onChangeExcuteStatus()">
                                            <asp:ListItem Value="1" Text="Đang xử lý"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Đã hoàn tất"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Đã hủy"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="row-payment-status" class="form-row">
                                    <div class="row-left">
                                        Trạng thái thanh toán
                                    </div>
                                    <div class="row-right">
                                        <asp:DropDownList ID="ddlPaymentStatus" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="1" Text="Chưa thanh toán"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Thanh toán thiếu"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Đã thanh toán"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="row-payment-type" class="form-row">
                                    <div class="row-left">
                                        Phương thức thanh toán
                                    </div>
                                    <div class="row-right">
                                        <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="1" Text="Tiền mặt"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Chuyển khoản"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Thu hộ"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Công nợ"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="row-shipping-type" class="form-row">
                                    <div class="row-left">
                                        Phương thức giao hàng
                                    </div>
                                    <div class="row-right">
                                        <asp:DropDownList ID="ddlShippingType" runat="server" CssClass="form-control shipping-type">
                                            <asp:ListItem Value="1" Text="Lấy trực tiếp"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Chuyển bưu điện"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Dịch vụ ship"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Chuyển xe"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Nhân viên giao hàng"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="row-transport-company" class="form-row transport-company">
                                    <div class="form-row">
                                        <div class="row-left">
                                            Chành xe
                                        </div>
                                        <div class="row-right">
                                            <asp:DropDownList ID="ddlTransportCompanyID" runat="server" CssClass="form-control customerlist select2" Height="40px" Width="100%" onchange="onChangeTransportCompany($(this))"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="row-left">
                                            Nơi nhận
                                        </div>
                                        <div class="row-right">
                                            <asp:DropDownList ID="ddlTransportCompanySubID" runat="server" CssClass="form-control customerlist select2" Height="40px" Width="100%"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div id="row-postal-delivery-type" class="form-row postal-delivery-type hide">
                                    <div class="row-left">
                                        Hình thức chuyển phát
                                    </div>
                                    <div class="row-right">
                                        <asp:DropDownList ID="ddlPostalDeliveryType" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="1" Text="Chuyển phát thường"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Chuyển phát nhanh"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="row-shipping" class="form-row shipping-code hide">
                                    <div class="row-left">
                                        Mã vận đơn
                                    </div>
                                    <div class="row-right">
                                        <asp:TextBox ID="txtShippingCode" runat="server" CssClass="form-control" placeholder="Nhập mã vận đơn"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="row-order-note" class="form-row">
                                    <div class="row-left">
                                        Ghi chú đơn hàng
                                    </div>
                                    <div class="row-right">
                                        <asp:TextBox ID="txtOrderNote" runat="server" CssClass="form-control" placeholder="Ghi chú"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="panel-post">
                                    <div class="post-table-links clear">
                                        <a href="javascript:;" class="btn link-btn" id="payall" style="background-color: #f87703; float: right" title="Hoàn tất đơn hàng" onclick="payAll()"><i class="fa fa-floppy-o"></i> Xác nhận</a>
                                        <asp:Button ID="btnOrder" runat="server" OnClick="btnOrder_Click" Style="display: none" />
                                        <a href="javascript:;" class="btn link-btn" style="background-color: #ffad00; float: right;" title="Nhập đơn hàng đổi trả" onclick="searchReturnOrder()"><i class="fa fa-refresh"></i> Đổi trả</a>
                                        <a id="feeNewStatic" href="#feeModal" class="btn link-btn" style="background-color: #607D8B; float: right;" title="Thêm phí khác vào đơn hàng" data-toggle="modal" data-backdrop='static'><i class="fa fa-plus"></i> Thêm phí khác</a>
                                    </div>
                                    <div id="img-out"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="buttonbar">
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel-buttonbar">
                            <div class="panel-post">
                                <div class="post-table-links clear">
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #f87703; float: right" title="Hoàn tất đơn hàng" onclick="payAll()"><i class="fa fa-floppy-o"></i> Xác nhận</a>
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #ffad00; float: right;" title="Nhập đơn hàng đổi trả" onclick="searchReturnOrder()"><i class="fa fa-refresh"></i> Đổi trả</a>
                                    <a id="feeNewDynamic" href="#feeModal" class="btn link-btn" style="background-color: #607D8B; float: right;" title="Thêm phí khác vào đơn hàng" data-toggle="modal" data-backdrop='static'><i class="fa fa-plus"></i> Thêm phí khác</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="notAcceptChangeUser" Value="1" runat="server" />
            <asp:HiddenField ID="hdfDiscountInOrder" runat="server" />
            <asp:HiddenField ID="hdfUsername" runat="server" />
            <asp:HiddenField ID="hdfUsernameCurrent" runat="server" />
            <asp:HiddenField ID="hdfRoleID" runat="server" />
            <asp:HiddenField ID="hdfOrderType" runat="server" />
            <asp:HiddenField ID="hdfTotalPrice" runat="server" />
            <asp:HiddenField ID="hdfTotalPriceNotDiscount" runat="server" />
            <asp:HiddenField ID="hdfListProduct" runat="server" />
            <asp:HiddenField ID="hdfPaymentStatus" runat="server" />
            <asp:HiddenField ID="hdfExcuteStatus" runat="server" />
            <asp:HiddenField ID="hdfIsDiscount" runat="server" />
            <asp:HiddenField ID="hdfDiscountAmount" runat="server" />
            <asp:HiddenField ID="hdfIsMain" runat="server" />
            <asp:HiddenField ID="hdfTotalPriceNotDiscountNotFee" runat="server" />
            <asp:HiddenField ID="hdfListSearch" runat="server" />
            <asp:HiddenField ID="hdfTotalQuantity" runat="server" />
            <asp:HiddenField ID="hdfcheck" runat="server" />
            <asp:HiddenField ID="hdftotal" runat="server" />
            <asp:HiddenField ID="hdfCurrentValue" runat="server" />
            <asp:HiddenField ID="hdfDelete" runat="server" />
            <asp:HiddenField ID="hdfDele" runat="server" />
            <asp:HiddenField ID="hdfDonHangTra" runat="server" />
            <asp:HiddenField ID="hdfChietKhau" runat="server" />
            <asp:HiddenField ID="hdfTongTienConLai" runat="server" />
            <asp:HiddenField ID="hdfSoLuong" runat="server" />
            <asp:HiddenField runat="server" ID="hdfcheckR" />
            <asp:HiddenField ID="hdOrderInfoID" runat="server" />
            <asp:HiddenField ID="hdSession" runat="server" />
            <asp:HiddenField ID="hdfFeeType" runat="server" />
            <asp:HiddenField ID="hdfOtherFees" runat="server" />
            <asp:HiddenField ID="hdfCustomerID" runat="server" />

            <!-- Modal -->
            <div class="modal fade" id="feeModal" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Cập nhật phí</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row form-group">
                                <asp:HiddenField ID="hdfUUID" runat="server" />
                                <div class="col-xs-8">
                                    <asp:DropDownList ID="ddlFeeType" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-xs-4">
                                    <asp:TextBox ID="txtFeePrice" runat="server" CssClass="form-control text-right" placeholder="Số tiền phí" data-type="currency" onkeypress='return event.charCode >= 48 && event.charCode <= 57'></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button id="closeFee" type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                            <button id="updateFee" type="button" class="btn btn-primary">Lưu</button>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Order Return Modal -->
            <div class="modal fade" id="orderReturnModal" role="dialog">
                <div class="modal-dialog modal-lg">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Danh sách đổi trả hàng</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row form-group">
                                <table class="table table-striped">
                                    <thead>
                                        <tr>
                                            <th>Mã</th>
                                            <th>Số lượng</th>
                                            <th>Phí đổi hàng</th>
                                            <th>Tổng tiền</th>
                                            <th>Nhân viên</th>
                                            <th>Ngày tạo</th>
                                        </tr>
                                    </thead>
                                    <tbody id="orderReturn">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button id="closeOrderReturn" type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    </asp:Panel>
    <style>
        .search-product-content {
            background: #fff;
        }

        .search-box {
            width: 90%;
        }

        #txtSearch {
            width: 100%;
        }

        #popup_content2 {
            min-height: 10px;
            position: fixed;
            background-color: #fff;
            top: 15%;
            z-index: 9999;
            left: 0;
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            padding: 20px;
            right: 0%;
            margin: 0 auto;
        }

        .pad {
            padding-bottom: 15px;
        }

        .pad10 {
            padding-right: 10px;
        }

        .padinfo {
            padding-bottom: 15px;
        }

        .disable {
            pointer-events: none;
            opacity: 0.7;
        }
    </style>
    <telerik:RadAjaxManager ID="rAjax" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnOrder">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="parent" LoadingPanelID="rxLoading"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadScriptBlock ID="sc" runat="server">
        <script type="text/javascript">
            // OrderDetailModel
            class OrderDetailModel {
                constructor(ID, SKU, ProductID, Quantity) {
                    this.ID = ID,
                    this.SKU = SKU,
                    this.ProductID = ProductID,
                    this.Quantity = Quantity
                }

                stringJSON() {
                    return JSON.stringify(this);
                }
            }

            // Fee Type
            class FeeType {
                constructor(ID, Name, IsNegativeFee) {
                    this.ID = ID;
                    this.Name = Name;
                    this.IsNegativeFee = IsNegativeFee;
                }
            }

            // FeeModel
            class Fee {
                constructor(UUID, FeeTypeID, FeeTypeName, FeePrice, Note) {
                    this.UUID = UUID;
                    this.FeeTypeID = FeeTypeID;
                    this.FeeTypeName = FeeTypeName;
                    this.FeePrice = FeePrice;
                    this.Note = Note ? " (" + Note + ")" : "";
                }

                stringJSON() {
                    return JSON.stringify(this);
                }
            }

            // orders detail remove
            var listOrderDetail = [];

            // fee type list
            var feetype = [];

            // fees list
            var fees = [];

            // order of item list
            var orderItem = $(".product-result").length;

            function formatNumber(n) {
                // format number 1000000 to 1,234,567
                return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
            }

            function formatCurrency(input, blur) {
                // appends $ to value, validates decimal side
                // and puts cursor back in right position.

                // get input value
                var input_val = input.val();

                // don't validate empty input
                if (input_val === "") { return; }

                // original length
                var original_len = input_val.length;

                // initial caret position 
                var caret_pos = input.prop("selectionStart");

                // check for decimal
                if (input_val.indexOf(".") >= 0) {

                    // get position of first decimal
                    // this prevents multiple decimals from
                    // being entered
                    var decimal_pos = input_val.indexOf(".");

                    // split number by decimal point
                    var left_side = input_val.substring(0, decimal_pos);
                    var right_side = input_val.substring(decimal_pos);

                    // add commas to left side of number
                    left_side = formatNumber(left_side);

                    // validate right side
                    right_side = formatNumber(right_side);

                    // On blur make sure 2 numbers after decimal
                    if (blur === "blur") {
                        right_side += "00";
                    }

                    // Limit decimal to only 2 digits
                    right_side = right_side.substring(0, 2);

                    // join number by .
                    input_val = left_side;

                } else {
                    // no decimal entered
                    // add commas to number
                    // remove all non-digits
                    input_val = formatNumber(input_val);
                    input_val = input_val;
                }

                // send updated string to input
                input.val(input_val);

                // put caret back in the right position
                var updated_len = input_val.length;
                caret_pos = updated_len - original_len + caret_pos;
                input[0].setSelectionRange(caret_pos, caret_pos);
            }

            // Load Fee Modal
            function loadFeeModel(obj, is_new) {
                if (is_new === undefined)
                    is_new = false;

                let idDOM = $("#<%=hdfUUID.ClientID%>");
                let feeTypeDOM = $("#<%=ddlFeeType.ClientID%>");
                let feePriceDOM = $("#<%=txtFeePrice.ClientID%>");

                // Init
                idDOM.val("");
                feeTypeDOM.val(0);
                feePriceDOM.val("");
                if (!is_new)
                {
                    let parent = obj.parent();
                    if (parent.attr("id"))
                        idDOM.val(parent.attr("id"));
                    if (parent.data("feeid"))
                        feeTypeDOM.val(parent.data("feeid"));
                    if (parent.data("price"))
                        feePriceDOM.val(formatNumber(parent.data("price").toString()));
                }

                if (feeTypeDOM.val() == "0")
                {
                    feePriceDOM.val("");
                    feePriceDOM.attr("disabled", true);
                }
                else
                {
                    feePriceDOM.removeAttr("disabled");
                }
            }

            function openFeeUpdateModal(obj)
            {
                loadFeeModel(obj);
                $('#feeModal').modal({ show: 'true', backdrop: 'static' });
            }

            // Create Fee
            function createFeeHTML(fee) {
                let addHTML = "";

                if (fee)
                {
                    let negative = fee.FeePrice > 0 ? "" : "-";

                    addHTML += "<div id='" + fee.UUID + "' class='post-row clear otherfee' data-feeid='" + fee.FeeTypeID + "' data-price='" + fee.FeePrice + "'>";
                    addHTML += "    <div class='left'>";
                    addHTML += "        <span class='otherfee-name'>" + fee.FeeTypeName + fee.Note + "</span>";
                    addHTML += "        <a href='javascript:;' class='btn btn-feeship link-btn' onclick='removeOtherFee(`" + fee.UUID + "`)'>";
                    addHTML += "            <i class='fa fa-times' aria-hidden='true'></i> Xóa";
                    addHTML += "        </a>";
                    addHTML += "        <a href='javascript:;' class='btn btn-feeship link-btn btn-edit-fee' onclick='editOtherFee()'>";
                    addHTML += "            <i class='fa fa-pencil-square-o' aria-hidden='true'></i> Sửa";
                    addHTML += "        </a>";
                    addHTML += "    </div>";
                    addHTML += "    <div class='right otherfee-value' onclick='openFeeUpdateModal($(this))'>";
                    addHTML += "        <input id='feePrice' type='text' class='form-control text-right' placeholder='Số tiền phí' disabled='disabled' value='" + negative + formatNumber(fee.FeePrice.toString()) + "'/>";
                    addHTML += "    </div>";
                    addHTML += "</div>";
                }

                return addHTML;
            }

            function addFeeNew()
            {
                let id = $("#<%=hdfUUID.ClientID%>").val();
                let feeid = $("#<%=ddlFeeType.ClientID%>").val();
                let feename = $("#<%=ddlFeeType.ClientID%> :selected").text();
                let feeprice = parseInt($("#<%=txtFeePrice.ClientID%>").val().replace(/\,/g, ''));
                let isNegative = false;
                feetype.forEach((item) => {
                    if (item.ID == feeid) {
                        isNegative = item.IsNegativeFee;
                        return;
                    }
                });
                if (isNegative) feeprice = feeprice * (-1);
                let fee = new Fee(id, feeid, feename, feeprice);

                fees.push(fee);
                $("#fee-list").append(createFeeHTML(fee));
                $("#<%=hdfOtherFees.ClientID%>").val(JSON.stringify(fees));
                getAllPrice();
            }

            // Update Fee
            function updateFee()
            {
                let id = $("#<%=hdfUUID.ClientID%>").val();
                if (!id) return;

                let feeid = $("#<%=ddlFeeType.ClientID%>").val();
                let feename = $("#<%=ddlFeeType.ClientID%> :selected").text();
                let isNegative = false;
                feetype.forEach((item) => {
                    if (item.ID == feeid) {
                        isNegative = item.IsNegativeFee;
                        return;
                    }
                });
                let feeprice = $("#<%=txtFeePrice.ClientID%>").val().replace(/\,/g, '');
                let parent = $("#" + id);

                parent.data("feeid", feeid);
                parent.data("feeprice", feeprice);

                parent.find("span.otherfee-name").html(feename);
                if (isNegative) {
                    parent.find("#feePrice").val("-" + formatNumber(feeprice));
                    feeprice = parseInt(feeprice) * (-1);
                }
                else {
                    parent.find("#feePrice").val(formatNumber(feeprice));
                    feeprice = parseInt(feeprice);
                }
                
                fees.forEach((fee) => {
                    if(fee.UUID == id)
                    {
                        fee.FeeTypeID = feeid;
                        fee.FeeTypeName = feename;
                        fee.FeePrice = feeprice;
                    }
                });
                $("#<%=hdfOtherFees.ClientID%>").val(JSON.stringify(fees));
                getAllPrice();
            }

            // cal fee ship
            function calFeeShip() {
                if ($("#<%=pFeeShip.ClientID%>").is(":disabled")) {
                    $("#<%=pFeeShip.ClientID%>").prop('disabled', false).css("background-color", "#fff").focus();
                    $("#calfeeship").html("Miễn phí").css("background-color", "#F44336");
                }
                else {
                    $("#<%=pFeeShip.ClientID%>").prop('disabled', true).css("background-color", "#eeeeee").val(0);
                    swal("Thông báo", "Đã chọn miễn phí vận chuyển cho đơn hàng này", "success");
                    getAllPrice();
                    $("#calfeeship").html("<i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Tính phí").css("background-color", "#f87703");
                }
            }
            // remove other fee by click button
            function removeOtherFee(uuid) {
                $("#" + uuid).remove();
                fees = fees.filter((item) => { return item.UUID != uuid; });
                $("#<%=hdfOtherFees.ClientID%>").val(JSON.stringify(fees));
                countTotal();
                //getAllPrice();
            }
            // edit other fee by click button
            function editOtherFee() {
                $(".otherfee-value").click();
                //getAllPrice();
            }

            function warningShippingNote(ID) {

                if ($("#<%=ddlShippingType.ClientID%>").find(":selected").val() == 2 && $("#<%=ddlPaymentType.ClientID%>").find(":selected").val() != 3) {
                    swal({
                        title: "Ê nhỏ:",
                        text: "Đơn hàng này gửi Bưu điện nhưng <strong>Không Thu Hộ</strong> hở?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Đúng rồi sếp!!",
                        closeOnConfirm: false,
                        cancelButtonText: "Để em xem lại..",
                        html: true
                    }, function (isConfirm) {
                        if (isConfirm) {
                            if ($("#<%=ddlPostalDeliveryType.ClientID%>").find(":selected").val() == 2) {
                                swal({
                                    title: "Còn nữa:",
                                    text: "Đơn hàng này gửi Bưu điện <strong>Chuyển Phát Nhanh</strong> đúng không?",
                                    type: "warning",
                                    showCancelButton: true,
                                    confirmButtonColor: "#DD6B55",
                                    confirmButtonText: "OK sếp ơi!!",
                                    closeOnConfirm: false,
                                    cancelButtonText: "Em lộn zồi..",
                                    html: true
                                }, function (isConfirm) {
                                    if (isConfirm) {
                                        sweetAlert.close();
                                        window.open("/print-shipping-note?id=" + ID, "_blank");
                                    }
                                });
                            }
                            else {
                                sweetAlert.close();
                                window.open("/print-shipping-note?id=" + ID, "_blank");
                            }
                        }
                    });
                }
                else {
                    window.open("/print-shipping-note?id=" + ID, "_blank");
                }
            }

            function warningPrintInvoice(ID) {
                if ($("#<%=ddlShippingType.ClientID%>").find(":selected").val() != 1 && $("#<%=pFeeShip.ClientID%>").val() == 0) {
                    swal({
                        title: "Nhỏ ơi:",
                        text: "Đơn hàng này <strong>Miễn Phí Vận Chuyển</strong> đúng không?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Chính xác ạ!!",
                        closeOnConfirm: false,
                        cancelButtonText: "Để em xem lại..",
                        html: true
                    }, function (isConfirm) {
                        if (isConfirm) {
                            sweetAlert.close();
                            window.open("/print-invoice?id=" + ID, "_blank");
                        }
                    });
                }
                else {
                    window.open("/print-invoice?id=" + ID, "_blank");
                }
            }

            function warningGetOrderImage(ID, mergeprint) {
                swal({
                    title: "Lưu ý nè",
                    text: "Nhớ lưu đơn hàng trước khi lấy ảnh nha!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Đã lưu rồi sếp ơi!!",
                    closeOnConfirm: true,
                    cancelButtonText: "Chờ tí! Để em lưu..",
                    html: true
                }, function () {
                    window.open("/print-order-image?id=" + ID + "&merge=" + mergeprint, "_blank");
                });
            }

            function init() {
                // Load Fee Type List
                let data = JSON.parse($("#<%=hdfFeeType.ClientID%>").val());
                data.forEach((item) => {
                    feetype.push(new FeeType(item.ID, item.Name, item.IsNegativeFee));
                });

                // Load Fee List
                let obj = JSON.parse($("#<%=hdfOtherFees.ClientID%>").val());
                if ($.isArray(obj))
                {
                    obj.forEach((item) => {
                        let fee = new Fee(
                            item.UUID,
                            item.FeeTypeID,
                            item.FeeTypeName,
                            item.FeePrice,
                            item.Note
                        );

                        fees.push(fee);
                        $("#fee-list").append(createFeeHTML(fee));
                    });
                }
            }

            $(document).ready(function () {
                init();

                // search Product by SKU
                $("#txtSearch").keydown(function (event) {
                    if (event.which === 13) {
                        searchProduct();
                        event.preventDefault();
                        return false;
                    }
                });

                $("#<%=txtPhone.ClientID%>").keyup(function (e) {
                    if (/\D/g.test(this.value)) {
                        // Filter non-digits from input value.
                        this.value = this.value.replace(/\D/g, '');
                    }
                });

                $("#<%=txtZalo.ClientID%>").keyup(function (e) {
                    if (/\D/g.test(this.value)) {
                        // Filter non-digits from input value.
                        this.value = this.value.replace(/\D/g, '');
                    }
                });

                // hide shipping code

                if ($("#<%=ddlShippingType.ClientID%>").find(":selected").val() == 1) {
                    $(".transport-company").addClass("hide");
                }

                if ($("#<%=ddlShippingType.ClientID%>").find(":selected").val() == 2) {
                    $(".shipping-code").removeClass("hide");
                    $(".postal-delivery-type").removeClass("hide");
                    $(".transport-company").addClass("hide");
                }

                if ($("#<%=ddlShippingType.ClientID%>").find(":selected").val() == 3) {
                    $(".shipping-code").removeClass("hide");
                    $(".transport-company").addClass("hide");
                }

                if ($("#<%=ddlShippingType.ClientID%>").find(":selected").val() == 4) {
                    $(".transport-company").removeClass("hide");
                }

                $("#<%=ddlShippingType.ClientID%>").change(function () {
                    var selected = $(this).find(":selected").val();
                    switch(selected) {
                        case "1":
                            $(".shipping-code").addClass("hide");
                            $(".postal-delivery-type").addClass("hide");
                            $(".transport-company").addClass("hide");
                            $("#<%=txtShippingCode.ClientID%>").val("");
                            $("#<%=ddlPostalDeliveryType.ClientID%>").val(1);
                            $("#<%=ddlTransportCompanyID.ClientID%>").val(0);
                            $("#<%=ddlTransportCompanySubID.ClientID%>").val(0);
                            break;
                        case "2":
                            $(".shipping-code").removeClass("hide");
                            $(".postal-delivery-type").removeClass("hide");
                            $(".transport-company").addClass("hide");
                            $("#<%=ddlTransportCompanyID.ClientID%>").val(0);
                            $("#<%=ddlTransportCompanySubID.ClientID%>").val(0);
                            break;
                        case "3":
                            $(".shipping-code").removeClass("hide");
                            $(".postal-delivery-type").addClass("hide");
                            $(".transport-company").addClass("hide");
                            $("#<%=ddlPostalDeliveryType.ClientID%>").val(1);
                            $("#<%=ddlTransportCompanyID.ClientID%>").val(0);
                            $("#<%=ddlTransportCompanySubID.ClientID%>").val(0);
                            break;
                        case "4":
                            $(".shipping-code").addClass("hide");
                            $(".postal-delivery-type").addClass("hide");
                            $(".transport-company").removeClass("hide");
                            $("#<%=txtShippingCode.ClientID%>").val("");
                            $("#<%=ddlPostalDeliveryType.ClientID%>").val(1);
                            break;
                        case "5":
                            $(".shipping-code").addClass("hide");
                            $(".postal-delivery-type").addClass("hide");
                            $(".transport-company").addClass("hide");
                            $("#<%=txtShippingCode.ClientID%>").val("");
                            $("#<%=ddlPostalDeliveryType.ClientID%>").val(1);
                            $("#<%=ddlTransportCompanyID.ClientID%>").val(0);
                            $("#<%=ddlTransportCompanySubID.ClientID%>").val(0);
                            break;
                    }
                });

                // add class full width for txtFacebook if it's null
                if ($("input[id$='_txtFacebook']").val() == "") {
                    $("input[id$='_txtFacebook']").parent().addClass("width-100");
                }

                // onchange drop down list excute status
                onChangeExcuteStatus();

                // Jquery Dependency
                $("input[data-type='currency']").on({
                    keyup: function () {
                        formatCurrency($(this));
                    },
                    blur: function () {
                        formatCurrency($(this), "blur");
                    }
                });
                // event create fee new
                $("[id^='feeNew']").click(e => { loadFeeModel(e.target, true); });

                // event change drop down list
                $("#<%=ddlFeeType.ClientID%>").change(e => {
                    let feePriceDOM = $("#<%=txtFeePrice.ClientID%>");
                    if (e.target.value == "0")
                    {
                        feePriceDOM.val("");
                        feePriceDOM.attr("disabled", true);
                    }
                    else
                    {
                        feePriceDOM.removeAttr("disabled");
                        feePriceDOM.focus();
                    }
                });
                // event press the enter key in txtFeePrice
                $("#<%=txtFeePrice.ClientID%>").keydown(function (event) {
                    if (event.which === 13) {
                        $("#updateFee").click();
                        return false;
                    }
                });
                // event updateFee click
                $("#updateFee").click(e => {
                    let id = $("#<%=hdfUUID.ClientID%>").val();
                    let price = $("#<%=txtFeePrice.ClientID%>").val().replace(/\,/g, '');

                    if (!(price && parseInt(price) >= 1000 && parseInt(price) % 1000 == 0))
                    {
                        swal({
                            title: "Thông báo",
                            text: "Có nhập sai số tiền không đó",
                            type: "error",
                            html: true,
                        }, function() {
                            $("#<%=txtFeePrice.ClientID%>").focus();
                        });
                        return;
                    }

                    if (!id)
                    {
                        $("#<%=hdfUUID.ClientID%>").val(uuid.v4());
                        addFeeNew();
                    }
                    else
                    {
                        updateFee();
                    }

                    $("#closeFee").click();
                });
            });

            // key press F1 - F4
            $(document).keydown(function(e) {
                if (e.which == 112) { //F1 Search Customer
                    searchCustomer();
                    return false;
                }
                if (e.which == 113) { //F2 Input Fullname
                    $("#<%= txtFullname.ClientID%>").focus();
                    return false;
                }
                if (e.which == 114) { //F3 Search Product
                    $("#txtSearch").focus();
                    return false;
                }
            });

            // display return order after page load
            var returnorder = document.getElementById('<%= hdfcheckR.ClientID%>').defaultValue;
            if (returnorder != "") {
                $(".refund").removeClass("hide");
                var t = returnorder.split(',');
                $("#<%=hdfDonHangTra.ClientID%>").val(t[1]);
                $(".find3").removeClass("hide");
                $(".find1").addClass("hide");
                $(".find2").html("<i class='fa fa-share' aria-hidden='true'></i> Xem đơn hàng trả " + t[0]);
                $(".find2").attr("onclick", "viewReturnOrder(" + t[0] + ")");
                $(".find2").removeClass("hide");
                $(".returnorder").removeClass("hide");
                $(".totalpriceorderall").removeClass("price-red");
                $(".totalpricedetail").addClass("price-red");
            }

            // search return order
            function createOrderReturnHTML(refundGood) {
                let createdDate = "";
                let addHTML = "";

                // Format CreateDate
                var matchs = refundGood.CreatedDate.match(/\d+/g);
                if (matchs) {
                    let date = new Date(parseInt(matchs[0]));
                    if (date) {
                        createdDate = date.format("yyyy-MM-dd");
                    }
                }
                addHTML += "<tr onclick='getReturnOrder(" + JSON.stringify(refundGood) + ")' style='cursor: pointer'>";
                addHTML += "    <td>" + refundGood.ID + "</td>";
                addHTML += "    <td>" + formatNumber(refundGood.TotalQuantity.toString()) + "</td>";
                addHTML += "    <td>" + formatNumber(refundGood.TotalRefundFee.toString()) + "</td>";
                addHTML += "    <td>" + formatNumber(refundGood.TotalPrice.toString()) + "</td>";
                addHTML += "    <td>" + refundGood.CreatedBy + "</td>";
                addHTML += "    <td>" + createdDate + "</td>";
                addHTML += "</tr>";

                return addHTML
            }

            function searchReturnOrder() {
                let customerID = $("#<%=hdfCustomerID.ClientID%>").val();

                if (isBlank(customerID)) {
                    swal("Thông báo", "Hãy nhập thông tin khách hàng trước!", "info");
                } else {
                    let modalDOM = $("#orderReturnModal");
                    let customerName = $("#<%=txtFullname.ClientID%>").val();

                    // Setting title modal
                    modalDOM.find(".modal-title").html("Danh sách đổi trả hàng (" + customerName + ")");
                    // Clear body modal
                    modalDOM.find("tbody[id='orderReturn']").html("");
                    $.ajax({
                        type: 'POST',
                        url: '/them-moi-don-hang.aspx/getOrderReturn',
                        data: JSON.stringify({ 'customerID': customerID }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: (response) => {
                            if (response.d) {
                                let data = JSON.parse(response.d);
                                if (data.length == 0)
                                {
                                    swal("Thông báo", "Khách hàng này không có đơn đổi trả hoặc đã được trừ tiền!", "error");
                                }
                                else
                                {
                                    data.forEach((item) => {
                                        modalDOM.find("tbody[id='orderReturn']").append(createOrderReturnHTML(item))
                                    });

                                    modalDOM.modal({ show: 'true', backdrop: 'static' });
                                }
                            }
                        },
                        error: (xmlhttprequest, textstatus, errorthrow) => {
                            swal("Thông báo", "Đã xảy ra lỗi trong quá trình lấy danh sách đơn hàng đổi trả", "error");
                        }
                    });
                }
            }

            // get return order
            function getReturnOrder(refundGood) {
                if (refundGood)
                {
                    let totalPrice = $("#<%=hdfTotalPrice.ClientID%>").val();
                    if (totalPrice)
                    {
                        totalPrice = parseFloat(totalPrice) - refundGood.TotalPrice;
                        if (totalPrice < 0) {
                            totalPrice = "-" + formatNumber(totalPrice.toString());
                        }
                        else {
                            totalPrice = formatNumber(totalPrice.toString());
                        }
                    }
                    else
                    {
                        totalPrice = "-" + formatNumber(refundGood.TotalPrice.toString());
                    }

                    $("#<%=hdSession.ClientID%>").val(refundGood.ID + "|" + refundGood.TotalPrice);
                    $(".subtotal").removeClass("hide");
                    $(".returnorder").removeClass("hide");
                    $(".totalpriceorderall").removeClass("price-red");
                    $(".totalpricedetail").addClass("price-red");
                    $(".find3").removeClass("hide");
                    $(".find1").addClass("hide");
                    $(".find2").html("<i class='fa fa-share' aria-hidden='true'></i> Xem đơn hàng trả " + refundGood.ID);
                    $(".find2").attr("onclick", "viewReturnOrder(" + refundGood.ID + ")");
                    $(".find2").removeClass("hide");
                    $(".totalpricedetail").html(totalPrice);
                    $("#<%=hdfDonHangTra.ClientID%>").val(refundGood.TotalPrice);
                    $(".refund").removeClass("hide");
                    $(".totalpriceorderrefund").html(formatThousands(refundGood.TotalPrice, ","));

                    $("#closeOrderReturn").click();
                    getAllPrice();
                }
            }

            // view return order by click button
            function viewReturnOrder(ID) {
                var win = window.open("/xem-don-hang-doi-tra?id=" + ID + "", '_blank');
                win.focus();
            }

            // delete return order
            function deleteReturnOrder() {
                $(".find3").addClass("hide");
                $(".find1").removeClass("hide");
                $(".find2").addClass("hide");
                $(".find2").html("");
                $(".find2").removeAttr("onclick");
                $(".totalpricedetail").html("0");
                $("#<%=hdfDonHangTra.ClientID%>").val(0);
                $("#<%=hdSession.ClientID%>").val(0);
                $(".refund").addClass("hide");
                $(".totalpriceorderrefund").html("0");
                $("#txtOrderRefund").val(0);
                $(".returnorder").addClass("hide");
                $(".totalpriceorderall").addClass("price-red");
                $(".totalpricedetail").removeClass("price-red");

                swal("Thông báo", "Đã bỏ qua đơn hàng đổi trả này!", "info");
                getAllPrice();
            }

            // pay order on click button
            function payAll() {

                var phone = $("#<%=txtPhone.ClientID%>").val();
                var name = $("#<%= txtFullname.ClientID%>").val();
                var nick = $("#<%= txtNick.ClientID%>").val();
                var address = $("#<%= txtAddress.ClientID%>").val();
                var facebooklink = $("#<%= txtFacebook.ClientID%>").val();

                if (phone == "" || name == "" || nick == "" || address == "" || (facebooklink == "" && $("#<%= hdfUsernameCurrent.ClientID%>").val() == "nhom_facebook") )
                {
                    if (name == "")
                    {
                        $("#<%= txtFullname.ClientID%>").focus();
                        swal("Thông báo", "Hãy nhập tên khách hàng!", "error");
                    }
                    else if (phone == "")
                    {
                        $("#<%= txtPhone.ClientID%>").focus();
                        swal("Thông báo", "Hãy nhập số điện thoại khách hàng!", "error");
                    }
                    else if (nick == "")
                    {
                        $("#<%= txtNick.ClientID%>").prop('disabled', false);
                        $("#<%= txtNick.ClientID%>").focus();
                        swal("Thông báo", "Hãy nhập Nick đặt hàng của khách hàng!", "error");
                    }
                    else if (facebooklink == "" && $("#<%= hdfUsernameCurrent.ClientID%>").val() == "nhom_facebook")
                    {
                        $("#<%= txtFacebook.ClientID%>").prop('disabled', false);
                        $("#<%= txtFacebook.ClientID%>").focus();
                        swal("Thông báo", "Hãy nhập link Facebook của khách này!", "error");
                    }
                    else if (address == "")
                    {
                        $("#<%= txtAddress.ClientID%>").focus();
                        swal("Thông báo", "Hãy nhập địa chỉ khách hàng!", "error");
                    }

                }
                else
                {
                    // Nếu có sản phẩm trong đơn hàng
                    if ($(".product-result").length > 0)
                    {
                        getAllPrice(true);
                        var list = "";
                        var ordertype = $(".customer-type").val();
                        var checkoutin = false;

                        $(".product-result").each(function () {
                            var orderDetailID = $(this).attr("data-orderdetailid");
                            var id = $(this).attr("data-productid");
                            var sku = $(this).attr("data-sku");
                            var producttype = $(this).attr("data-producttype");
                            var productvariablename = $(this).attr("data-productvariablename");
                            var productvariablevalue = $(this).attr("data-productvariablevalue");
                            var productname = $(this).attr("data-productname");
                            var productimageorigin = $(this).attr("data-productimageorigin");
                            var productvariable = $(this).attr("data-productvariable");
                            var price = $(this).find(".gia-san-pham").attr("data-price");
                            var productvariablesave = $(this).attr("data-productvariablesave");
                            var quantity = parseFloat($(this).find(".in-quantity").val());
                            var quantityInstock = parseFloat($(this).attr("data-quantityinstock"));
                            var productvariableid = $(this).attr("data-productvariableid");

                            if (quantity > 0)
                            {
                                if (quantity > quantityInstock)
                                    checkoutin = true;

                                list += id + "," + sku + "," + producttype + "," + productvariablename + "," + productvariablevalue + "," + quantity + "," +
                                    productname + "," + productimageorigin + "," + productvariablesave + "," + price + "," + productvariablesave + "," +
                                    orderDetailID + "," + productvariableid + ";";
                            }
                        });

                        // Kiểm tra trạng thái xử lý
                        let excuteStatus = Number($("#<%=ddlExcuteStatus.ClientID%>").val());

                        // Nếu chọn trạng thái hủy
                        if (excuteStatus == 3)
                        {
                            // Nếu trạng thái cũ là đã hủy thì thông báo hủy rồi
                            if ($("#<%=hdfExcuteStatus.ClientID%>").val() == 3)
                            {
                                swal("Thông báo", "Đơn hàng này đã hủy trước đó!", "warning");
                            }
                            else
                            {
                                swal({
                                    title: "Xác nhận",
                                    text: "Cưng có chắc hủy đơn hàng này không?",
                                    type: "warning",
                                    showCancelButton: true,
                                    closeOnConfirm: false,
                                    cancelButtonText: "Đợi em xem tí!",
                                    confirmButtonText: "Chắc chắn sếp ơi..",
                                }, function (isConfirm) {
                                    if (isConfirm)
                                    {
                                        swal({
                                            title: "Nhập lý do",
                                            text: "Nhập lý do hủy đơn hàng:",
                                            type: "input",
                                            showCancelButton: true,
                                            closeOnConfirm: false,
                                            cancelButtonText: "Đợi em suy nghĩ!",
                                            confirmButtonText: "Hủy thôi..",
                                        }, function (orderNote) {
                                            // Kiểm tra xem có nhập lý do hủy không? Không nhập thì không cho hủy
                                            if (orderNote != "")
                                            {
                                                $("#<%=txtOrderNote.ClientID %>").val(orderNote);
                                                $("#<%=ddlPaymentStatus.ClientID %>").val(1);
                                                deleteOrder();
                                                $("#<%=hdfOrderType.ClientID%>").val(ordertype);
                                                $("#<%=hdfListProduct.ClientID%>").val(list);
                                                insertOrder();
                                            }
                                            else
                                            {
                                                swal("Hủy đơn hàng thất bại!", "Chưa nhập lý do nên không được hủy", "error");
                                            }
                                        });
                                    }
                                });
                            }
                        }
                        else if (excuteStatus != 3 && $("#<%=hdfExcuteStatus.ClientID%>").val() == 3 )
                        {
                            // Khôi phục đơn hàng đã hủy
                            // Chỉ admin mới được khôi phục đơn hàng hủy
                            if ($("#<%=hdfRoleID.ClientID%>").val() == 0)
                            {
                                deleteOrder();
                                $("#<%=hdfOrderType.ClientID %>").val(ordertype);
                                $("#<%=hdfListProduct.ClientID%>").val(list);
                                insertOrder();
                            }
                            else
                            {
                                swal("Không thể khôi phục đơn hàng đã hủy!", "Hãy báo cáo chị Ngọc để khôi phục", "error");
                            }
                        }
                        else if (excuteStatus == 1 && $("#<%=hdfExcuteStatus.ClientID%>").val() == 2)
                        {
                            // Chỉ admin mới được đổi trạng thái Đã hoàn tất sang trạng thái Đang xử lý
                            if ($("#<%=hdfRoleID.ClientID%>").val() == 0)
                            {
                                deleteOrder();
                                $("#<%=hdfOrderType.ClientID %>").val(ordertype);
                                $("#<%=hdfListProduct.ClientID%>").val(list);
                                insertOrder();
                            }
                            else
                            {
                                swal("Không thể đổi trạng thái từ Đã hoàn tất sang Đang xử lý!", "Hãy báo cáo chị Ngọc để khôi phục", "error");
                            }
                        }
                        else
                        {
                            // Nếu trạng thái không liên quan đến hủy thì xử lý..
                            deleteOrder(); 
                            $("#<%=hdfOrderType.ClientID %>").val(ordertype);
                            $("#<%=hdfListProduct.ClientID%>").val(list);
                            insertOrder();
                        }
                    }
                    else
                    {
                        // Nếu không có sản phẩm trong đơn
                        let excuteStatus = Number($("#<%=ddlExcuteStatus.ClientID%>").val());

                        if (excuteStatus == 3)
                        {
                            swal({
                                title: "Xác nhận",
                                text: "Đơn hàng này sẽ bị hủy. Cưng có chắc hủy đơn này không?",
                                type: "warning",
                                showCancelButton: true,
                                closeOnConfirm: false,
                                cancelButtonText: "Đợi em xem tí!",
                                confirmButtonText: "Chắc chắn sếp ơi..",
                            }, function (isConfirm) {
                                if (isConfirm)
                                {
                                    swal({
                                        title: "Nhập lý do",
                                        text: "Nhập lý do hủy đơn hàng:",
                                        type: "input",
                                        showCancelButton: true,
                                        closeOnConfirm: false,
                                        cancelButtonText: "Đợi em suy nghĩ!",
                                        confirmButtonText: "Hủy thôi..",
                                    }, function (orderNote) {
                                        if (orderNote != "")
                                        {

                                            $("#<%=txtOrderNote.ClientID %>").val(orderNote);
                                            $("#<%=ddlPaymentStatus.ClientID %>").val(1);

                                            deleteOrder();

                                            let order_id = $("#<%=hdOrderInfoID.ClientID%>").val()
                                            $.ajax({
                                                type: "POST",
                                                url: "/thong-tin-don-hang.aspx/UpdateStatus",
                                                data: "{OrderId: " + order_id + "}",
                                                contentType: "application/json; charset=utf-8",
                                                dataType: "json",
                                                success: function (msg) {
                                                    if (msg.d != null) {
                                                        window.location.assign("/danh-sach-don-hang.aspx");
                                                    }
                                                },
                                                error: function (xmlhttprequest, textstatus, errorthrow) {
                                                    alert('lỗi');
                                                }
                                            });
                                        }
                                        else
                                        {
                                            swal("Hủy đơn hàng thất bại!", "Chưa nhập lý do nên không được hủy", "error");
                                        }
                                    });
                                }
                            });

                        }
                        else
                        {
                            $("#txtSearch").focus();
                            swal("Thông báo", "Hãy nhập sản phẩm!", "error");
                        }
                    }
                }
            }

            function checkPrepayTransport(ID, SubID) {
                var t = 0;
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/them-moi-don-hang.aspx/checkPrepayTransport",
                    data: "{ID:" + ID + ", SubID:" + SubID + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d == "yes") {
                            t = 1;
                        } else {
                            t = 0;
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                        alert('lỗi');
                    }
                });
                return t;
            }

            // insert order
            function insertOrder() {
                var shippingtype = $(".shipping-type").val();
                var checkAllValue = true;
                var fs = $("#<%=pFeeShip.ClientID%>").val();
                var feeship = parseFloat(fs.replace(/\,/g, ''));

                        if (shippingtype == 2 || shippingtype == 3)
                        {
                            if (feeship == 0 && $("#<%=pFeeShip.ClientID%>").is(":disabled") == false)
                            {
                                $("#<%=pFeeShip.ClientID%>").focus();
                                swal({
                                    title: "Có vấn đề:",
                                    text: "Chưa nhập phí vận chuyển!<br><br>Hỏng lẻ miễn phí vận chuyển luôn hở?",
                                    type: "warning",
                                    showCancelButton: true,
                                    confirmButtonColor: "#DD6B55",
                                    confirmButtonText: "Để em tính phí!!",
                                    closeOnConfirm: false,
                                    cancelButtonText: "Để em bấm nút miễn phí",
                                    html: true
                                });
                                checkAllValue = false;
                            }
                        }
                        else if (shippingtype == 4)
                        {
                            if (feeship == 0 && $("#<%=pFeeShip.ClientID%>").is(":disabled") == false)
                            {
                                var ID = $("#<%=ddlTransportCompanyID.ClientID%>").val();
                                var SubID = $("#<%=ddlTransportCompanySubID.ClientID%>").val();

                                if (ID != 0 && SubID != 0)
                                {
                                    var checkPrepay = checkPrepayTransport(ID, SubID);
                                    if (checkPrepay == 1)
                                    {
                                        $("#<%=pFeeShip.ClientID%>").focus();
                                        swal({
                                            title: "Coi nè:",
                                            text: "Chưa nhập phí vận chuyển do nhà xe này <strong>trả cước trước</strong> nè!<br><br>Hay là miễn phí vận chuyển luôn hở?",
                                            type: "warning",
                                            showCancelButton: true,
                                            confirmButtonColor: "#DD6B55",
                                            confirmButtonText: "Để em nhập phí!!",
                                            closeOnConfirm: false,
                                            cancelButtonText: "Để em coi lại..",
                                            html: true
                                        });
                                        checkAllValue = false;
                                    }
                                }
                            }
                        }
                        

                    if (feeship > 0 && feeship < 10000)
                    {
                        checkAllValue = false;
                        $("#<%=pFeeShip.ClientID%>").focus();
                        swal({
                            title: "Lạ vậy:",
                            text: "Sao phí vận chuyển lại nhỏ hơn <strong>10.000đ</strong> nè?<br><br>Xem lại nha!",
                            type: "warning",
                            showCancelButton: false,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "Để em xem lại!!",
                            html: true
                        });
                    }

                    var ds = $("#<%=pDiscount.ClientID%>").val();
                    var discount = parseFloat(ds.replace(/\,/g, ''));

                    if (discount > 11000 && $("#<%=hdfRoleID.ClientID%>").val() != 0)
                    {
                        checkAllValue = false;
                        $("#<%=pDiscount.ClientID%>").focus();
                        swal({
                            title: "Lạ vậy:",
                            text: "Sao chiết khấu lại lớn hơn <strong>11.000đ</strong> nè?<br><br>Nếu có lý do thì báo chị Ngọc nha!",
                            type: "warning",
                            showCancelButton: false,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "Để em xem lại!!",
                            html: true
                        });
                    }

                    if (checkAllValue == true)
                    {
                        HoldOn.open();
                        $("#<%=btnOrder.ClientID%>").click();
                    }
            }

            // search product by SKU
            function searchProduct() {
                let textsearch = $("#txtSearch").val().trim().toUpperCase();

                $("#<%=hdfListSearch.ClientID%>").val(textsearch);

                $("#txtSearch").val("");

                //Get search product master
                searchProductMaster(textsearch, true);
            }


            // update stock which removed
            function updateTemplateStock(orderServer) {
                listOrderDetail.forEach(function (orderClient) {
                    if (orderClient.ProductID == orderServer.ID) {
                        orderServer.QuantityInstockString = (Number(orderServer.QuantityInstockString) + Number(orderClient.Quantity)).toString();
                    }
                });
            }

            function searchRemovedList(ProductSKU) {
                var t = ""
                for (i = 0; i< listOrderDetail.length; i++) {
                    if (listOrderDetail[i].SKU == ProductSKU) {
                        t += "data-orderdetailid=\"" + listOrderDetail[i].ID + "\"";
                    }
                }
                return t;
            }

            function removeProductExisted(orderServer) {
                for (index in listOrderDetail) {
                    if (listOrderDetail[index].ProductID == orderServer.ID) {
                        listOrderDetail.splice(index,1);
                    }
                }
            }

            // get all price
            function getAllPrice(is_payAll_call) {
                if (is_payAll_call == undefined)
                    is_payAll_call = false;
                if ($(".product-result").length > 0)
                {
                    var totalprice = 0;
                    var productquantity = 0;
                    $(".product-result").each(function() {
                        var price = parseFloat($(this).find(".gia-san-pham").attr("data-price"));
                        var quantity = parseFloat($(this).find(".in-quantity").val());
                        var total = price * quantity;

                        $(this).find(".totalprice-view").html(formatThousands(total, ','));

                        productquantity += quantity;
                        totalprice += total;
                    });

                    $("#<%=hdfTotalPriceNotDiscount.ClientID%>").val(totalprice);
                    $(".totalproductQuantity").html(formatThousands(productquantity, ',') + " sản phẩm");
                    
                    $(".totalpriceorder").html(formatThousands(totalprice, ','));
                    $("#<%=hdfTotalPriceNotDiscountNotFee.ClientID%>").val(totalprice);
                    $("#<%=hdfTotalQuantity.ClientID%>").val(productquantity);
                    var isDiscount = $("#<%=hdfIsDiscount.ClientID%>").val();
                    var totalDiscount = 0;
                    var totalleft = 0;
                    var amount = 0;
                    var amountdiscount = 0;

                    // Kiểm tra khách hàng có được chiết khấu trong nhóm ko?
                    if (isDiscount == 1) {
                        amountdiscount = parseFloat($("#<%=hdfDiscountAmount.ClientID%>").val());
                    }

                    // Lấy các mức chiết khấu từ hệ thống
                    var ChietKhau = document.getElementById('<%= hdfChietKhau.ClientID%>').defaultValue;

                    var listck = ChietKhau.split('|');
                    for (var i = 0; i < listck.length - 1; i++) {
                        var item = listck[i].split('-');
                        if (i < listck.length - 2) {
                            var item2 = listck[i + 1].split('-');
                            if (productquantity > (parseFloat(item[0]) - 1) && productquantity <= (parseFloat(item2[0]) - 1)) {
                                amount = parseFloat(item[1]);
                            }
                        } else {
                            if (productquantity > (parseFloat(item[0]) - 1)) {
                                amount = parseFloat(item[1]);
                            }
                        }
                    }

                    // Nếu dùng bằng tay để chỉnh chiết khấu
                    if (is_payAll_call === true)
                    {
                        amount = parseInt($("#<%=pDiscount.ClientID%>").val().replace(/\,/g,''));
                    }
                    else
                    {
                        // Nếu chiết khấu của khách hàng lớn hơn 0
                        if (amountdiscount > 0) {
                            // Nếu <chiết khấu nhóm> của khách hàng lớn hơn mức được <chiết khấu của đơn hàng> thì lấy <chiết khấu nhóm> để tính
                            if (amount < amountdiscount) {
                                amount = amountdiscount;
                            }
                        }
                    }

                    // Nếu đơn hàng được chiết khấu sau khi tính toán
                    if (amount > 0) {
                        totalDiscount = amount;
                        var totalck = amount * productquantity;
                        totalleft = totalprice - totalck;
                    } else {
                        totalDiscount = 0;
                        totalleft = totalprice;
                    }

                    // Nếu đơn hàng có sẵn chiết khấu và lớn hơn chiết khấu được tính phía trên
                    if ($("#<%=hdfDiscountInOrder.ClientID%>").val() != 0 && $("#<%=hdfDiscountInOrder.ClientID%>").val() > amount ) {
                        var dis = $("#<%=hdfDiscountInOrder.ClientID%>").val();
                        var discount = parseFloat(dis.replace(/\,/g, ''));
                        var totalck = discount * productquantity;
                        var totalleft = totalprice - totalck;
                        var totalDiscount = discount;
                    }

                    var fs = $("#<%=pFeeShip.ClientID%>").val();
                    var feeship = parseFloat(fs.replace(/\,/g, ''));

                    let otherfee = 0;
                    fees.forEach((item) => {
                        otherfee += item.FeePrice;
                    });

                    var priceafterchietkhau = totalleft;

                    var totalmoney = totalleft + feeship + otherfee;

                    $("#<%=pDiscount.ClientID%>").val(formatThousands(totalDiscount, ','));

                    $(".totalpriceorderall").html(formatThousands(totalmoney, ','));
                    $(".priceafterchietkhau").html(formatThousands(priceafterchietkhau, ','));
                    $("#<%=hdfTotalPrice.ClientID%>").val(totalmoney);
                    var refund = 0;
                    if (parseFloat($("#<%=hdfDonHangTra.ClientID%>").val()) > 0) {
                        refund = parseFloat($("#<%=hdfDonHangTra.ClientID%>").val());
                    }

                    $(".totalpricedetail").html(formatThousands((totalmoney - refund), ","));
                    $("#<%=hdfTongTienConLai.ClientID%>").val(totalmoney - refund);
                }
                else
                {
                    // update status order
                    $("#<%=ddlExcuteStatus.ClientID%>").val(3);

                    $(".totalproductQuantity").html(formatThousands(0, ',') + " sản phẩm");
                    $(".totalpriceorder").html(formatThousands(0, ','));
                    $(".totalpriceorderall").html(formatThousands(0, ','));
                    $(".priceafterchietkhau").html(formatThousands(0, ','));

                }
                reIndex();
            }

            // check empty
            function notEmpty() {
                if ($("#<%=pDiscount.ClientID%>").val() == '')
                {
                    var dis = 0;
                    $("#<%=pDiscount.ClientID%>").val(formatThousands(dis, ','));
                }
                if ($("#<%=pFeeShip.ClientID%>").val() == '')
                {
                    var fee = 0;
                    $("#<%=pFeeShip.ClientID%>").val(formatThousands(fee, ','));
                }
                fees.forEach((item) => {
                    if (item.price == "")
                    {
                        item.FeePrice = 0;
                        $("#" + item.UUID).val(0);
                    }
                })
            }

            // count total order
            function countTotal() {
                var total = parseFloat($("#<%=hdfTotalPriceNotDiscount.ClientID%>").val());
                var quantity = 0;
                if (!isBlank($("#<%=hdfTotalQuantity.ClientID%>").val()))
                {
                    quantity = parseFloat($("#<%=hdfTotalQuantity.ClientID%>").val());
                }
                notEmpty();
                var dis = $("#<%=pDiscount.ClientID%>").val();
                var fs = $("#<%=pFeeShip.ClientID%>").val();
                var totalproduct = parseFloat($("#<%=hdftotal.ClientID%>").val());

                var discount = parseFloat(dis.replace(/\,/g, ''));
                var feeship = parseFloat(fs.replace(/\,/g, ''));

                let otherfee = 0;
                fees.forEach((item) => {
                    otherfee += item.FeePrice;
                });

                $("#<%=hdfcheck.ClientID%>").val(discount);
                $("#<%=hdfDiscountInOrder.ClientID%>").val(discount);

                if (quantity > 0)
                {
                    var totalleft = total + feeship + otherfee - discount * quantity;
                    var priceafterchietkhau = total - discount * quantity;
                    $("#<%=hdfTotalQuantity.ClientID%>").val(quantity);
                }
                else
                {
                    var totalleft = total + feeship + otherfee - discount * totalproduct;
                    var priceafterchietkhau = total - discount * totalproduct;
                    $("#<%=hdfTotalQuantity.ClientID%>").val(totalproduct);
                }
                $(".totalpriceorderall").html(formatThousands(totalleft, ','));
                $(".priceafterchietkhau").html(formatThousands(priceafterchietkhau, ','));

                var refund = 0;
                if (parseFloat($("#<%=hdfDonHangTra.ClientID%>").val()) > 0) {
                    refund = parseFloat($("#<%=hdfDonHangTra.ClientID%>").val());
                }

                $(".totalpricedetail").html(formatThousands((totalleft - refund), ","));
                $("#<%=hdfTongTienConLai.ClientID%>").val(totalleft - refund);

                $("#<%=hdfTotalPrice.ClientID%>").val(totalleft);
            }

            // get product price
            function getProductPrice(obj) {
                var customertype = obj.val();
                if ($(".product-result").length > 0)
                {
                    var totalprice = 0;
                    $(".product-result").each(function() {
                        var giasi = $(this).attr("data-giabansi");
                        var giale = $(this).attr("data-giabanle");
                        if (customertype == 1) {
                            if (giale == 0)
                                giale = giasi;
                            $(this).find(".gia-san-pham").attr("data-price", giale).html(formatThousands(giale, ','));
                        } else {
                            $(this).find(".gia-san-pham").attr("data-price", giasi).html(formatThousands(giasi, ','));
                        }
                    });
                    getAllPrice();
                }
            }

            // press key
            function keypress(e) {
                var keypressed = null;
                if (window.event)
                {
                    keypressed = window.event.keyCode; //IE
                }
                else
                {
                    keypressed = e.which; //NON-IE, Standard
                }
                if (keypressed < 48 || keypressed > 57)
                {
                    if (keypressed == 8 || keypressed == 127)
                        return;
                    return false;
                }
            }

                // format price
            var formatThousands = function(n, dp) {
                var s = '' + (Math.floor(n)),
                    d = n % 1,
                    i = s.length,
                    r = '';
                while ((i -= 3) > 0) {
                    r = ',' + s.substr(i, 3) + r;
                }
                return s.substr(0, i + 3) + r +
                    (d ? '.' + Math.round(d * Math.pow(10, dp || 2)) : '');
            };

            function deleteOrder() {
                if (listOrderDetail.length > 0)
                {
                    let getDataJSON = function () {
                        let stringJSON = "{listOrderDetail: [";

                        for (index in listOrderDetail) {
                            if (index == 0) {
                                stringJSON += listOrderDetail[index].stringJSON();
                            } else {
                                stringJSON += ", " + listOrderDetail[index].stringJSON();
                            }
                        }

                        stringJSON += "]}";

                        return stringJSON;
                    };

                    $.ajax({
                        type: "POST",
                        async: false,
                        url: "/thong-tin-don-hang.aspx/Delete",
                        data: getDataJSON(),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json"
                    });
                }
            }

            function onChangeExcuteStatus() {
                let excuteStatus = Number($("#<%=ddlExcuteStatus.ClientID%>").val());

                switch (excuteStatus) {
                    case 2:
                        $("#infor-customer").addClass("disable");
                        $("#detail").addClass("disable");
                        if ($("#<%=hdfExcuteStatus.ClientID%>").val() == 2) {
                            $("#row-payment-status").removeClass("disable");
                            $("#row-payment-type").removeClass("disable");
                            $("#row-shipping-type").removeClass("disable");
                            $("#row-transport-company").removeClass("disable");
                            $("#row-shipping").removeClass("disable");
                            $("#row-order-note").removeClass("disable");
                        }
                        break;
                    case 3:
                        $("#infor-order").addClass("disable");
                        $("#infor-customer").addClass("disable");
                        $("#detail").addClass("disable");
                        $("#status .panel-heading").addClass("disable");
                        $("#row-payment-status").addClass("disable");
                        $("#row-payment-type").addClass("disable");
                        $("#row-shipping-type").addClass("disable");
                        $("#row-transport-company").addClass("disable");
                        $("#row-shipping").addClass("disable");
                        $("#row-order-note").addClass("disable");

                        break;
                    case 1:
                        
                        if ($("#<%=hdfExcuteStatus.ClientID%>").val() == 1) {
                            $("#infor-order").removeClass("disable");
                            $("#infor-customer").removeClass("disable");
                            $("#detail").removeClass("disable");
                            $("#status .panel-heading").removeClass("disable");
                            $("#row-payment-status").removeClass("disable");
                            $("#row-payment-type").removeClass("disable");
                            $("#row-shipping-type").removeClass("disable");
                            $("#row-transport-company").removeClass("disable");
                            $("#row-shipping").removeClass("disable");
                            $("#row-order-note").removeClass("disable");
                        }
                        else {
                            $("#infor-customer").addClass("disable");
                            $("#detail").addClass("disable");
                            $("#status .panel-heading").addClass("disable");
                            $("#row-payment-status").addClass("disable");
                            $("#row-payment-type").addClass("disable");
                            $("#row-shipping-type").addClass("disable");
                            $("#row-transport-company").addClass("disable");
                            $("#row-shipping").addClass("disable");
                            $("#row-order-note").addClass("disable");
                        }

                        break;
                }
            }
            
            function onChangeTransportCompany(transport)
            {
                let transComID = transport.val();
                $.ajax({
                    url: "thong-tin-don-hang.aspx/GetTransportSub",
                    type: "POST",
                    data: JSON.stringify({'transComID': transComID}),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        let data = JSON.parse(msg.d);
                        if (data)
                        {
                            let tranSubDOM = $("#<%=ddlTransportCompanySubID.ClientID%>");
                            tranSubDOM.html("")
                            data.forEach((item) => {
                                tranSubDOM.append("<option value='" + item.key + "'>" + item.value + "</option>")
                            });

                            let tranSubContainerDOM = $("[id$=ddlTransportCompanySubID-container]");
                            tranSubContainerDOM.attr("title", "Chọn nơi nhận");
                            tranSubContainerDOM.html("Chọn nơi nhận");
                            
                            setTimeout(function() {
                                $("#<%=ddlTransportCompanySubID.ClientID%>").select2("open");
                            }, 200);  
                            
                        }
                    },
                    error: function (err) {
                        swal("Thông báo", "Đã có vấn đề trong việc cập nhật thông tin vận chuyển", "error");
                    }
                });
            }
        </script>
    </telerik:RadScriptBlock>
</asp:Content>
