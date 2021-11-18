<%@ Page Title="Thêm mới đơn hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="them-moi-don-hang.aspx.cs" Inherits="IM_PJ.them_moi_don_hang" EnableSessionState="ReadOnly" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/App_Themes/Ann/js/search-customer.js?v=202110131611"></script>
    <script src="/App_Themes/Ann/js/search-product.js?v=18112021"></script>
    <script type="text/javascript" src="/App_Themes/Ann/js/pages/danh-sach-khach-hang/generate-coupon-for-customer.js?v=18112021"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .coupon .right {
            display: flex;
        }

        .red {
            background-color: #F44336!important;
        }
    </style>
    <asp:Panel ID="parent" runat="server">
        <main id="main-wrap">
            <div class="container">
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Thông tin khách hàng</h3>
                                <a href="javascript:;" class="search-customer" onclick="searchCustomer()"><i class="fa fa-search" aria-hidden="true"></i> Tìm khách hàng (F1)</a>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Họ tên</label>
                                            <asp:TextBox ID="txtFullname" CssClass="form-control capitalize" runat="server" placeholder="Họ tên thật của khách (F2)" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Điện thoại</label>
                                            <asp:TextBox ID="txtPhone" CssClass="form-control" autocomplete="off" onblur="ajaxCheckCustomer()" runat="server" placeholder="Số điện thoại"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Nick đặt hàng</label>
                                            <asp:TextBox ID="txtNick" CssClass="form-control capitalize" autocomplete="off" runat="server" placeholder="Nick đặt hàng"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Facebook</label>
                                            <div class="row">
                                                <div class="col-md-9 fb width-100">
                                                    <asp:TextBox ID="txtFacebook" CssClass="form-control" autocomplete="off" runat="server" placeholder="Đường link Chat Facebook"></asp:TextBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="row">
                                                        <span class="link-facebook"></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-row view-detail" style="display: none">
                                </div>
                                <div class="form-row discount-info" style="display: none">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="deliveryAddress" class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Địa chỉ giao hàng</h3>
                                <a href="javascript:;" class="search-customer" onclick="showDeliveryAddresses()"><i class="fa fa-search" aria-hidden="true"></i> Tìm địa chỉ</a>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Họ tên</label>
                                            <asp:TextBox ID="txtRecipientFullName" runat="server" CssClass="form-control capitalize" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Điện thoại</label>
                                            <asp:TextBox ID="txtRecipientPhone" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Địa chỉ</label>
                                            <asp:TextBox ID="txtRecipientAddress" runat="server" CssClass="form-control capitalize" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Tỉnh thành</label>
                                            <asp:DropDownList ID="ddlRecipientProvince" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Quận huyện</label>
                                            <asp:DropDownList ID="ddlRecipientDistrict" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Phường xã</label>
                                            <asp:DropDownList ID="ddlRecipientWard" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel-post">
                            <select class="form-control customer-type" onchange="getProductPrice($(this))">
                                <option value="2">Khách mua sỉ</option>
                                <option value="1">Khách mua lẻ</option>
                            </select>
                            <div class="post-above clear">
                                <div class="search-box left">
                                    <input type="text" id="txtSearch" class="form-control sku-input" placeholder="NHẬP MÃ SẢN PHẨM (F3)" autocomplete="off">
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
                                                <th class="discount-item">Chiết khấu</th>
                                                <th class="quantity-item">Kho</th>
                                                <th class="quantity-item">Số lượng</th>
                                                <th class="total-item">Thành tiền</th>
                                                <th class="trash-item">Xóa</th>
                                            </tr>
                                        </thead>
                                        <tbody class="content-product">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="post-footer">
                                <div class="post-row clear">
                                    <div class="left">Số lượng</div>
                                    <div class="right totalproductQuantity"></div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">Thành tiền</div>
                                    <div class="right totalpriceorder"></div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">Chiết khấu</div>
                                    <div class="right">
                                        <a href="javascript:;" class="btn btn-cal-discount link-btn" onclick="refreshDiscount()"><i class="fa fa-refresh" aria-hidden="true"></i> Gợi ý</a>
                                        <telerik:RadNumericTextBox runat="server" CssClass="form-control width-notfull input-discount" Skin="MetroTouch"
                                            ID="pDiscount" MinValue="0" NumberFormat-GroupSizes="3" Value="0" NumberFormat-DecimalDigits="0"
                                            onblur="onBlurPDiscount($(this))" IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
                                        </telerik:RadNumericTextBox>
                                    </div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">Tổng chiết khấu</div>
                                    <div class="right totalDiscount">
                                        <asp:Literal ID="ltrTotalDiscount" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">Sau chiết khấu</div>
                                    <div class="right priceafterchietkhau"></div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">Phí vận chuyển</div>
                                    <div class="right">
                                        <a class="btn btn-feeship link-btn" href="javascript:;" id="calfeeship" onclick="calFeeShip()"><i class="fa fa-check-square-o" aria-hidden="true"></i> Miễn phí</a>
                                        <telerik:RadNumericTextBox runat="server" CssClass="form-control width-notfull input-feeship" Skin="MetroTouch"
                                            ID="pFeeShip" MinValue="0" NumberFormat-GroupSizes="3" Value="0" NumberFormat-DecimalDigits="0"
                                            oninput="countTotal()" IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
                                        </telerik:RadNumericTextBox>
                                    </div>
                                </div>
                                <div id="fee-list"></div>
                                <div class="post-row clear coupon">
                                    <div class="left">Mã giảm giá</div>
                                    <div class="right">
                                        <a id="btnGenerateCouponG25" class="btn btn-coupon btn-violet" title="Kiểm tra mã giảm giá G25" onclick="couponG25()"><i class="fa fa-gift"></i> G25</a>
                                        <a id="btnOpenCouponModal" class="btn btn-coupon btn-violet" title="Nhập mã giảm giá" onclick="openCouponModal()"><i class="fa fa-gift"></i> Nhập mã</a>
                                        <a href="javascript:;" id="btnRemoveCouponCode" class="btn btn-coupon link-btn hide" onclick="removeCoupon()"><i class="fa fa-times" aria-hidden="true"></i> Xóa</a>
                                        <asp:TextBox ID="txtCouponValue" runat="server" CssClass="form-control text-right width-notfull input-coupon" value="0" disabled="disabled"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left"><strong>TỔNG TIỀN</strong></div>
                                    <div class="right totalpriceorderall price-red"></div>
                                </div>
                                <div class="post-row clear returnorder hide">
                                    <div class="left">
                                        Trừ hàng trả
                                    </div>
                                    <div class="right">
                                        <a href="javascript:;" class="find2 hide btn btn-return-order link-btn"></a>
                                        <a href="javascript:;" class="find3 hide btn btn-return-order link-btn btn-edit-fee" onclick="searchReturnOrder()"><i class="fa fa-refresh" aria-hidden="true"></i> Chọn đơn khác</a>
                                        <a href="javascript:;" class="find3 hide btn btn-feeship link-btn" onclick="deleteReturnOrder()"><i class="fa fa-times" aria-hidden="true"></i>Xóa</a>
                                        <span class="totalpriceorderrefund"></span>
                                    </div>
                                </div>
                                <div class="post-row clear refund hide">
                                    <div class="left"><strong>TỔNG TIỀN CÒN LẠI</strong></div>
                                    <div class="right totalpricedetail"></div>
                                </div>
                                <div class="post-table-links clear">
                                    <a href="javascript:;" class="btn link-btn" id="payall" style="background-color: #f87703; float: right" title="Hoàn tất đơn hàng" onclick="payAll()"><i class="fa fa-floppy-o"></i> Xác nhận</a>
                                    <asp:Button ID="btnOrder" runat="server" OnClick="btnOrder_Click" Style="display: none" />
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #ffad00; float: right;" title="Nhập đơn hàng đổi trả" onclick="searchReturnOrder()"><i class="fa fa-refresh"></i> Đổi trả</a>
                                    <a id="feeNewStatic" href="#feeModal" class="btn link-btn" style="background-color: #607D8B; float: right;" title="Thêm phí khác vào đơn hàng" data-toggle="modal" data-backdrop='static'><i class="fa fa-plus"></i> Thêm phí khác</a>
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #F44336; float: right;" title="Xóa tất cả sản phẩm" onclick="deleteProduct()"><i class="fa fa-times" aria-hidden="true"></i> Làm lại</a>
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
                                <div class="post-table-links clear row">
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #f87703; float: right" title="Hoàn tất đơn hàng" onclick="payAll()"><i class="fa fa-floppy-o"></i> Xác nhận</a>
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #ffad00; float: right;" title="Nhập đơn hàng đổi trả" onclick="searchReturnOrder()"><i class="fa fa-refresh"></i> Đổi trả</a>
                                    <a id="feeNewDynamic" href="#feeModal" class="btn link-btn" style="background-color: #607D8B; float: right;" title="Thêm phí khác vào đơn hàng" data-toggle="modal" data-backdrop='static'><i class="fa fa-plus"></i> Thêm phí khác</a>
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #F44336; float: right;" title="Xóa tất cả sản phẩm" onclick="deleteProduct()"><i class="fa fa-times" aria-hidden="true"></i> Làm lại</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="notAcceptChangeUser" Value="1" runat="server" />
            <asp:HiddenField ID="hdfRoleID" runat="server" />
            <asp:HiddenField ID="hdfUsername" runat="server" />
            <asp:HiddenField ID="hdfUsernameCurrent" runat="server" />
            <asp:HiddenField ID="hdfOrderType" runat="server" />
            <asp:HiddenField ID="hdfTotalPrice" runat="server" />
            <asp:HiddenField ID="hdfTotalPriceNotDiscount" runat="server" />
            <asp:HiddenField ID="hdfListProduct" runat="server" />
            <asp:HiddenField ID="hdfOrderNote" runat="server" />
            <%-- Khách hàng có nằm trong nhóm chiết khấu không --%>
            <asp:HiddenField ID="hdfIsDiscount" runat="server" />
            <%-- Chiết khấu của nhóm --%>
            <asp:HiddenField ID="hdfDiscountAmount" runat="server" />
            <%-- Khối lượng yêu cầu để hưởng chiết khẩu của nhóm --%>
            <asp:HiddenField ID="hdfQuantityRequirement" runat="server" />
            <asp:HiddenField ID="hdfIsMain" runat="server" />
            <asp:HiddenField ID="hdfListSearch" runat="server" />
            <asp:HiddenField ID="hdfTotalQuantity" runat="server" />
            <asp:HiddenField ID="hdfChietKhau" runat="server" />
            <asp:HiddenField ID="hdfDonHangTra" runat="server" />
            <asp:HiddenField ID="hdfTongTienConLai" runat="server" />
            <asp:HiddenField ID="hdSession" runat="server" />
            <asp:HiddenField ID="hdfFeeType" runat="server" />
            <asp:HiddenField ID="hdfOtherFees" runat="server" />
            <asp:HiddenField ID="hdfCustomerID" runat="server" />
            <asp:HiddenField ID="hdfTransportCompanySubID" runat="server" />
            <asp:HiddenField ID="hdfCouponID" runat="server" />
            <asp:HiddenField ID="hdfCouponValue" runat="server" />
            <asp:HiddenField ID="hdfCouponProductNumber" runat="server" />
            <asp:HiddenField ID="hdfCouponPriceMin" runat="server" />
            <%-- Tổng chiết khấu của đơn hàng --%>
            <asp:HiddenField ID="hdfTotalDiscount" runat="server" />

            <!-- Biến đăng ký địa chỉ nhận hàng -->
            <asp:HiddenField ID="hdfDeliveryAddressId" runat="server" />
            <asp:HiddenField ID="hdfRecipientFullName" runat="server" />
            <asp:HiddenField ID="hdfRecipientPhone" runat="server" />
            <asp:HiddenField ID="hdfRecipientProvinceId" runat="server" />
            <asp:HiddenField ID="hdfRecipientDistrictId" runat="server" />
            <asp:HiddenField ID="hdfRecipientWardId" runat="server" />
            <asp:HiddenField ID="hdfRecipientAddress" runat="server" />
            <!-- Biến đăng ký địa chỉ nhận hàng -->

            <!-- Fee Modal -->
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
                            <button id="createReturnOrder" type="button" class="btn btn-primary" data-dismiss="modal">Tạo đơn hàng đổi trả</button>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Order Return Modal -->
            <div class="modal fade" id="orderInfoModal" role="dialog" data-backdrop="false">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Thông tin đơn hàng</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row form-group">
                                <div class="col-md-4 text-align-left">
                                    Trạng thái xử lý
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlExcuteStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col-md-4 text-align-left">
                                    Trạng thái thanh toán
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlPaymentStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col-md-4 text-align-left">
                                    Phương thức thanh toán
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control" onchange="onchangePaymentType($(this))"></asp:DropDownList>
                                </div>
                            </div>
                            <div id="bankModal" class="row form-group">
                                <div class="col-md-4 text-align-left">
                                    Ngân hàng nhận tiền
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col-md-4 text-align-left">
                                    Phương thức giao hàng
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlShippingType" runat="server" CssClass="form-control" onchange="onchangeShippingType($(this))"></asp:DropDownList>
                                </div>
                            </div>
                            <div id="transportModal" class="row form-group">
                                <div class="col-md-4 text-align-left">
                                    Chành xe
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlTransportCompanyID" runat="server" CssClass="form-control customerlist select2" Height="40px" Width="100%" onchange="onChangeTransportCompany($(this))"></asp:DropDownList>
                                </div>
                            </div>
                            <div id="transportSubModal" class="row form-group">
                                <div class="col-md-4 text-align-left">
                                    Nơi nhận
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlTransportCompanySubID" runat="server" CssClass="form-control customerlist select2" Height="40px" Width="100%"></asp:DropDownList>
                                </div>
                            </div>
                            <div id="shippingFeeModal" class="row form-group">
                                <div class="col-md-4 text-align-left">
                                    Phí vận chuyển
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtShippingFeeModal" placeholder="Có thể bỏ qua" runat="server" CssClass="form-control text-right" data-type="currency" onkeypress='return event.charCode >= 48 && event.charCode <= 57' onkeyup="updateShippingFeeFromModal()"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button id="closeOrderInfo" type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                            <button id="updateOrderInfo" type="button" class="btn btn-primary" onclick="insertOrder()">Tạo đơn hàng</button>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Check Order Old Modal -->
            <div class="modal fade" id="orderOldModal" role="dialog" data-backdrop="false">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Thông báo</h4>
                        </div>
                        <div class="modal-body">
                            <h4 id="txtPreOrder" class="hide">Khách hàng này đang có đơn online</h4>
                            <h4 id="txtOrder" class="hide">Khách hàng này đang có đơn hàng</h4>
                            <h4 id="warningTextOrderReturn" class="hide">Khách hàng này đang có đơn hàng đổi trả chưa trừ tiền!</h4>
                        </div>
                        <div class="modal-footer">
                            <button id="closeOrderOld" type="button" class="btn btn-default" data-dismiss="modal">Vẫn tiếp tục</button>
                            <button id="btnOpenPreOrder" type="button" class="btn btn-primary">Xem đơn online</button>
                            <button id="btnOpenOrder" type="button" class="btn btn-primary">Xem đơn</button>
                            <button id="openOrderReturn" type="button" class="btn btn-primary">Xem đơn đổi trả</button>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Coupon Modal -->
            <div class="modal fade" id="couponModal" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Nhập mã giảm giá</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row form-group">
                                <div class="col-xs-12">
                                    <asp:TextBox ID="txtCouponCode" runat="server" CssClass="form-control text-left" placeholder="Mã giảm giá"></asp:TextBox>
                                </div>
                            </div>
                            <div id="errorCoupon" class="row form-group hide">
                                <div class="col-xs-12 text-align-left text-danger">
                                    <p></p>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button id="closeCoupon" type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                            <button id="insertCoupon" type="button" class="btn btn-primary" onclick="getCoupon()">Xác nhận</button>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    </asp:Panel>
    <style>
        .search-product-content {
            height: initial !important;
            min-height: 200px;
            background: #fff;
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
            padding: 20px 20px;
            right: 0%;
            margin: 0 auto;
        }

        *.select2-container.select2-container--default.select2-container--open {
            z-index: 99991;
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
        <script src="https://code.jquery.com/jquery-3.3.1.min.js" integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8=" crossorigin="anonymous"></script>
        <script type="text/javascript" src="App_Themes/Ann/js/delivery-address.js?v=18112021"></script>
        <script type="text/javascript">
            "use strict";

            // #region Private
            function _checkOrderStatus() {
                let excuteStatus = $("#<%=ddlExcuteStatus.ClientID%>").val();

                // Trạng thái hoàn tất đơn hàng
                if (excuteStatus == 2) {
                    let shippingType = $("#<%=ddlShippingType.ClientID%>").val();

                    // 2: Bưu điện | 7: Viettel | 8: Grab | 9: AhaMove
                    if (shippingType == 2 || shippingType == 7 || shippingType == 8 || shippingType == 9) {
                        let $feeShip = $("#<%=pFeeShip.ClientID%>");
                        let feeShip = parseFloat($feeShip.val().replace(/\,/g, ''));

                        // Miễn phí ship
                        if (feeShip == 0 && $feeShip.is(":disabled") == false) {
                            $("#<%=txtShippingFeeModal.ClientID%>").select();
                            swal({
                                title: "Có vấn đề:",
                                text: "Chưa nhập phí vận chuyển!<br><br>Hay miễn phí vận chuyển luôn?",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "Để em tính phí!!",
                                closeOnConfirm: false,
                                cancelButtonText: "Để em bấm nút miễn phí (cẩn thận)",
                                html: true
                            });

                            return false;
                        }
                    }
                    // 3: Proship | 6: GHTK | 10: J&T
                    else if (shippingType == 3 || shippingType == 6 || shippingType == 10) {
                        swal({
                            title: "Có vấn đề:",
                            text: "Không <strong>Hoàn tất</strong> đơn ngay lúc này được! Hãy chọn trạng thái Đang xử lý đơn!",
                            type: "warning",
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "OK!!",
                            html: true
                        });

                        return false;
                    }
                    // 4: Chuyển xe
                    else if (shippingType == 4) {
                        let $transportCompany = $("#<%=ddlTransportCompanyID.ClientID%>");
                        let $transportCompanySub = $("#<%=ddlTransportCompanySubID.ClientID%>");
                        let $feeShip = $("#<%=pFeeShip.ClientID%>");
                        let feeShip = parseFloat($feeShip.val().replace(/\,/g, ''));

                        // Chành xe
                        if ($transportCompany.val() == 0) {
                            $transportCompany.focus();
                            swal("Thông báo", "Chưa chọn chành xe!", "warning");

                            return false;
                        }
                        // Nơi nhận
                        else if ($transportCompanySub.val() == 0) {
                            $transportCompanySub.focus();
                            swal("Thông báo", "Chưa chọn nơi nhận của chành xe!", "warning");

                            return false;
                        }
                        // Miễn phí ship
                        else if (feeShip == 0 && $feeShip.is(":disabled") == false) {
                            let checkPrepay = checkPrepayTransport($transportCompany.val(), $transportCompanySub.val());

                            if (checkPrepay == 1) {
                                $("#<%=txtShippingFeeModal.ClientID%>").select();
                                swal({
                                    title: "Coi nè:",
                                    text: "Chưa nhập phí vận chuyển do nhà xe này <strong>trả cước trước</strong>!<br><br>Hay là miễn phí vận chuyển luôn?",
                                    type: "warning",
                                    showCancelButton: true,
                                    confirmButtonColor: "#DD6B55",
                                    confirmButtonText: "Để em nhập phí!!",
                                    closeOnConfirm: false,
                                    cancelButtonText: "Để em coi lại..",
                                    html: true
                                });

                                return false;
                            }
                        }
                    }
                }

                return true;
            }

            function _checkFeeShip() {
                let $feeShip = $("#<%=pFeeShip.ClientID%>");
                let feeShip = parseFloat($feeShip.val().replace(/\,/g, ''));

                if (feeShip > 0 && feeShip < 10000) {
                    $("#<%=txtShippingFeeModal.ClientID%>").select();
                    swal({
                        title: "Lạ vậy:",
                        text: "Sao phí vận chuyển lại nhỏ hơn <strong>10.000đ</strong> nè?<br><br>Xem lại nha!",
                        type: "warning",
                        showCancelButton: false,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Để em xem lại!!",
                        html: true
                    });

                    return false;
                }

                return true;
            }

            function _checkDiscount() {
                let $products = $(".product-result");

                let errorDiscount = false;

                $products.each(function () {
                    errorDiscount = Boolean($(this).attr("data-error-discount"));

                    if (errorDiscount)
                        return false;
                });

                if (errorDiscount)
                {
                    let message = "";

                    message += "Chiết khấu sao nhiều vậy nè?";
                    message += "<br><br>Nếu có lý do thì báo chị Ngọc nha!";

                    swal({
                        title: "Lạ vậy:",
                        text: message,
                        type: "warning",
                        showCancelButton: false,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Để em xem lại!!",
                        html: true
                    });

                    return false;
                }

                return true;
            }

            function _checkValidation() {
                if (!checkDeliveryAddressValidation())
                    return false;

                // Kiểm tra trạng thái đơn hàng
                if (!_checkOrderStatus())
                    return false;

                // Kiểm tra phí giao hàng
                if (!_checkFeeShip())
                    return false;

                // Kiểm tra chiết khấu
                if (!_checkDiscount())
                    return false;

                return true;
            }

            function _updateDeliveryAddress() {
                // Cập nhật thông tin địa chỉ giao hàng
                let $phone = $("#<%=txtPhone.ClientID%>");

                return updateDeliveryAddress($phone.val());
            }
            // #endregin

            function redirectTo(ID) {
                HoldOn.close();
                window.onbeforeunload = null;
                window.location.href = "/thong-tin-don-hang?id=" + ID;
            }

            // check data before close page or refresh page
            window.onbeforeunload = function () {
                if ($(".product-result").length > 0 || $("#<%=txtPhone.ClientID%>").val() != "" || $("#<%= txtFullname.ClientID%>").val() != "")
                    return "You're leaving the site.";
            };

                var feetype = [];
                var fees = [];

                class FeeType {
                    constructor(ID, Name, IsNegativeFee) {
                        this.ID = ID;
                        this.Name = Name;
                        this.IsNegativeFee = IsNegativeFee;
                    }
                }

                class Fee {
                    constructor(UUID, FeeTypeID, FeeTypeName, FeePrice) {
                        this.UUID = UUID;
                        this.FeeTypeID = FeeTypeID;
                        this.FeeTypeName = FeeTypeName;
                        this.FeePrice = FeePrice;
                    }

                    stringJSON() {
                        return JSON.stringify(this);
                    }
                }

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
                if (!is_new) {
                    let parent = obj.parent();
                    if (parent.attr("id"))
                        idDOM.val(parent.attr("id"));
                    if (parent.data("feeid"))
                        feeTypeDOM.val(parent.data("feeid"));
                    if (parent.data("price"))
                        feePriceDOM.val(formatNumber(parent.data("price").toString()));
                }

                if (feeTypeDOM.val() == "0") {
                    feePriceDOM.val("");
                    feePriceDOM.attr("disabled", true);
                }
                else {
                    feePriceDOM.removeAttr("disabled");
                }
            }

            function openFeeUpdateModal(obj) {
                loadFeeModel(obj);
                $('#feeModal').modal({ show: 'true', backdrop: 'static' });
            }

            // Create Fee
            function createFeeHTML(fee) {
                let addHTML = "";

                if (fee) {
                    let negative = fee.FeePrice > 0 ? "" : "-";

                    addHTML += "<div id='" + fee.UUID + "' class='post-row clear otherfee' data-feeid='" + fee.FeeTypeID + "' data-price='" + fee.FeePrice + "'>";
                    addHTML += "    <div class='left'>";
                    addHTML += "        <span class='otherfee-name'><i class='fa fa-check' aria-hidden='true'></i> " + fee.FeeTypeName + "</span>";
                    addHTML += "    </div>";
                    addHTML += "    <div class='right otherfee-value' onclick='openFeeUpdateModal($(this))'>";
                    addHTML += "        <a href='javascript:;' class='btn btn-other-fee link-btn btn-edit-fee' onclick='editOtherFee(`" + fee.UUID + "`)'>";
                    addHTML += "            <i class='fa fa-pencil-square-o' aria-hidden='true'></i> Sửa";
                    addHTML += "        </a>";
                    addHTML += "        <a href='javascript:;' class='btn btn-other-fee link-btn' onclick='removeOtherFee(`" + fee.UUID + "`)'>";
                    addHTML += "            <i class='fa fa-times' aria-hidden='true'></i> Xóa";
                    addHTML += "        </a>";
                    addHTML += "        <input id='feePrice' type='text' class='form-control text-right width-notfull input-coupon' placeholder='Số tiền phí' disabled='disabled' value='" + negative + formatNumber(fee.FeePrice.toString()) + "'/>";
                    addHTML += "    </div>";
                    addHTML += "</div>";
                }

                return addHTML;
            }

            function addFeeNew() {
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
                $("#fee-list").before(createFeeHTML(fee));
                $("#<%=hdfOtherFees.ClientID%>").val(JSON.stringify(fees));
                getAllPrice(true);
            }

            // Update Fee
            function updateFee() {
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
                    if (fee.UUID == id) {
                        fee.FeeTypeID = feeid;
                        fee.FeeTypeName = feename;
                        fee.FeePrice = feeprice;
                    }
                });
                $("#<%=hdfOtherFees.ClientID%>").val(JSON.stringify(fees));
                getAllPrice(true);
            }

            function init() {
                // focus to searchProduct input when page on ready
                $("#txtSearch").focus();
                $("#<%=pDiscount.ClientID%>").val(0);
                $("#<%=pFeeShip.ClientID%>").val(0);

                let data = JSON.parse($("#<%=hdfFeeType.ClientID%>").val());
                data.forEach((item) => {
                    feetype.push(new FeeType(item.ID, item.Name, item.IsNegativeFee));
                });
            }

            $(document).ready(() => {

                _initReceiverAddress();
                _onChangeReceiverAddress();

                // Thông tin địa chỉ nhận hàng
                initDeliveryAddress();

                init();


                // search Product by SKU
                $("#txtSearch").keydown(function (event) {
                    if (event.which === 13) {
                        searchProduct();
                        event.preventDefault();
                        return false;
                    }
                });

                // event key up txtPhone
                $("#<%=txtPhone.ClientID%>").keyup(function (e) {
                    if (/\D/g.test(this.value))
                        // Filter non-digits from input value.
                        this.value = this.value.replace(/\D/g, '');
                });
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
                    if (e.target.value == "0") {
                        feePriceDOM.val("");
                        feePriceDOM.attr("disabled", true);
                    }
                    else {
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

                    if (!(price && parseInt(price) >= 1000 && parseInt(price) % 1000 == 0)) {
                        swal({
                            title: "Thông báo",
                            text: "Có nhập sai số tiền không đó",
                            type: "error",
                            html: true,
                        }, function () {
                            $("#<%=txtFeePrice.ClientID%>").focus();
                        });
                        return;
                    }

                    if (!id) {
                        $("#<%=hdfUUID.ClientID%>").val(uuid.v4());
                        addFeeNew();
                    }
                    else {
                        updateFee();
                    }

                    $("#closeFee").click();
                });

                $("#orderInfoModal").on("show.bs.modal", (e) => {
                    let customerID = $("#<%=hdfCustomerID.ClientID%>").val();
                    let payTypeDOM = $("#<%=ddlPaymentType.ClientID%>");
                    let shipTypeDOM = $("#<%=ddlShippingType.ClientID%>");
                    let shippingFeeDOM = $("#<%=txtShippingFeeModal.ClientID%>");
                    let shippingFeeValue = $("#<%=pFeeShip.ClientID%>").val();
                    shippingFeeDOM.val(shippingFeeValue);
                    onchangePaymentType(payTypeDOM);
                    onchangeShippingType(shipTypeDOM);

                    if (customerID) {
                        suggestOrderLast(customerID);
                    }
                })

                $("#createReturnOrder").click(() => {
                    let customerID = $("#<%=hdfCustomerID.ClientID%>").val();

                    if (!customerID) {
                        swal("Thông báo", "Không tìm thấy ID của khách hàng này", "error");
                    }
                    else {
                        createReturnOrder(customerID);
                    }
                });

                // Handling min value for shipper fee
                $("#<%=txtShippingFeeModal.ClientID%>").change(e => {
                    let value = e.target.value;

                    if (value == "") {
                        e.target.value = 0;
                    }
                });

                $('[id$="_txtCouponCode"]').keypress((event) => {
                    if (event.which == 13)
                        getCoupon();
                });
            });

            // order of item list
            var orderItem = 0;



            // key press F1 - F4
            $(document).keydown(function (e) {
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

            // cal fee ship
            function calFeeShip() {
                if ($("#<%=pFeeShip.ClientID%>").is(":disabled")) {
                    $("#<%=pFeeShip.ClientID%>").prop('disabled', false).css("background-color", "#fff").focus();
                    $("#calfeeship").html("Miễn phí").css("background-color", "#F44336");
                }
                else {
                    $("#<%=pFeeShip.ClientID%>").prop('disabled', true).css("background-color", "#eeeeee").val(0);
                    swal("Thông báo", "Đã chọn miễn phí vận chuyển cho đơn hàng này<br><strong>Hãy ghi chú lý do miễn phí vận chuyển!!!</strong>", "success");
                    getAllPrice(true);
                    $("#calfeeship").html("<i class='fa fa-pencil-square-o' aria-hidden='true'></i> Tính phí").css("background-color", "#f87703");
                }
            }

            // remove other fee by click button
            function removeOtherFee(uuid) {
                $("#" + uuid).remove();
                fees = fees.filter((item) => { return item.UUID != uuid; });
                $("#<%=hdfOtherFees.ClientID%>").val(JSON.stringify(fees));
                getAllPrice(true);
            }

            // edit other fee by click button
            function editOtherFee(uuid) {
                $("#" + uuid).find(".otherfee-value").click();
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
                    swal("Thông báo", "Đây là khách hàng mới mà ^_^", "info");
                }
                else {
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
                                if (data.length == 0) {
                                    swal({
                                        title: 'Thông báo',
                                        text: 'Khách hàng này không có đơn đổi trả hoặc đã được trừ tiền!',
                                        type: 'warning',
                                        showCancelButton: true,
                                        closeOnConfirm: true,
                                        cancelButtonText: "Để em xem lại...",
                                        confirmButtonText: "Tạo đơn hàng đổi trả",
                                    }, function (confirm) {
                                        if (confirm) createReturnOrder(customerID);
                                    });
                                }
                                else {
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
                if (refundGood) {
                    let totalPrice = $("#<%=hdfTotalPrice.ClientID%>").val();
                    if (totalPrice) {
                        totalPrice = parseFloat(totalPrice) - refundGood.TotalPrice;
                        if (totalPrice < 0) {
                            totalPrice = "-" + formatNumber(totalPrice.toString());
                        }
                        else {
                            totalPrice = formatNumber(totalPrice.toString());
                        }
                    }
                    else {
                        totalPrice = "-" + formatNumber(refundGood.TotalPrice.toString());
                    }

                    $("#<%=hdSession.ClientID%>").val(refundGood.ID + "|" + refundGood.TotalPrice);
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
                    getAllPrice(true);
                }
            }

            // view return order by click button
            function viewReturnOrder(ID) {
                var win = window.open("/thong-tin-tra-hang?id=" + ID + "", '_blank');
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
                $("#<%=hdSession.ClientID%>").val(1);
                $(".refund").addClass("hide");
                $(".totalpriceorderrefund").html("0");
                $("#txtOrderRefund").val(0);
                $(".returnorder").addClass("hide");
                $(".totalpriceorderall").addClass("price-red");
                $(".totalpricedetail").removeClass("price-red");

                swal("Thông báo", "Đã bỏ qua đơn hàng đổi trả này!", "info");
                getAllPrice(true);
            }

            /* ============================================================
             * Lấy thông tin để tạo đơn hàng
             *
             * Date:   2021-07-19
             * Author: Binh-TT
             *
             * Đối ứng chiết khấu từng dòng
             * ============================================================
             */
            function payAll() {
                var phone = $("#<%=txtPhone.ClientID%>").val();
                var name = $("#<%= txtFullname.ClientID%>").val();
                var nick = $("#<%= txtNick.ClientID%>").val();
                var facebooklink = $("#<%= txtFacebook.ClientID%>").val();

                if (name === "") {
                    $("#<%= txtFullname.ClientID%>").focus();
                    swal("Thông báo", "Hãy nhập tên khách hàng!", "error");
                }
                else if (phone === "") {
                    $("#<%= txtPhone.ClientID%>").focus();
                    swal("Thông báo", "Hãy nhập số điện thoại khách hàng!", "error");
                }
                else if (nick === "") {
                    $("#<%= txtNick.ClientID%>").focus();
                    swal("Thông báo", "Hãy nhập Nick đặt hàng của khách hàng!", "error");
                }
                else if ($(".product-result").length == 0) {
                    $("#txtSearch").focus();
                    swal("Thông báo", "Hãy nhập sản phẩm!", "error");
                }
                else {
                    getAllPrice(true);
                    var list = "";
                    var count = 0;
                    var ordertype = $(".customer-type").val();
                    $(".product-result").each(function () {
                        // 2021-07-19: Đối ứng chiết khấu từng dòng
                        let $discount = $(this).find(".discount");

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
                        // 2021-07-19: Đối ứng chiết khấu từng dòng
                        let discount = +$discount.val().replace(/,/g, '') || 0;

                        if (quantity > 0) {
                            list += id + "," + sku + "," + producttype + "," + productvariablename + "," + productvariablevalue + "," + quantity + "," +
                                productname + "," + productimageorigin + "," + productvariablesave + "," + price + "," + productvariablesave + "," +
                                productvariableid + "," + discount + ";";
                            count++;
                        }
                    });
                    if (count > 0) {
                        $("#<%=hdfOrderType.ClientID %>").val(ordertype);
                        $("#<%=hdfListProduct.ClientID%>").val(list);

                        $("#orderInfoModal").modal({ show: 'true', backdrop: 'static' });
                    }
                    else {
                        $("#txtSearch").focus();
                        swal("Thông báo", "Hãy nhập sản phẩm!", "error");
                    }
                }
            }

            // insert order
            function insertOrder() {
                HoldOn.open();

                if (!_checkValidation()) {
                    HoldOn.close();
                    return;
                }

                Promise.all([_updateDeliveryAddress()])
                    .then(function () {
                        window.onbeforeunload = null;

                        let $transportCompanySub = $("#<%=ddlTransportCompanySubID.ClientID%>");
                        $("#<%=hdfTransportCompanySubID.ClientID%>").val($transportCompanySub.val());
                        $("#closeOrderInfo").click();
                        $("#<%=btnOrder.ClientID%>").click();
                    })
                    .catch(function (err) {
                        HoldOn.close();
                    });
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
            };

            // search product by SKU
            function searchProduct() {
                let textsearch = $("#txtSearch").val().trim().toUpperCase();

                $("#<%=hdfListSearch.ClientID%>").val(textsearch);
                $("#txtSearch").val("");
                //Get search product master
                searchProductMaster(textsearch, true);
            }

            // delete all product by click button
            function deleteProduct() {
                swal({
                    title: "Hết sức lưu ý:",
                    text: "Em muốn xóa hết sản phẩm trong đơn hả?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Chính xác sếp ơi!!",
                    closeOnConfirm: false,
                    cancelButtonText: "Em bấm lộn rồi..",
                    html: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        $(".product-result").remove();
                        getAllPrice(true);
                        $(".totalproductQuantity").html("0");
                        $(".totalpriceorder").html("0");
                        $("#<%=pDiscount.ClientID%>").val(0);
                        $(".priceafterchietkhau").html("0");
                        $("#<%=pFeeShip.ClientID%>").val(0);
                        $(".totalpriceorderall").html("0");

                        $("#<%=hdfTotalPriceNotDiscount.ClientID%>").val(0);
                        $("#<%=hdfTotalPrice.ClientID%>").val(0);
                        sweetAlert.close();
                    }
                });
            }

            // change quantity of product
            function checkQuantiy(obj) {
                var current = obj.val();
                if (current == 0 || current == "" || current == null)
                    obj.val("1");
                getAllPrice(true);
            }

            /* ============================================================
             * Tính lại chiết khấu
             *
             * Date:   2021-07-19
             * Author: Binh-TT
             *
             * Đối ứng chiết khấu từng dòng
             * ============================================================
             */
            function refreshDiscount() {
                let totalQuantity = +$("#<%=hdfTotalQuantity.ClientID%>").val() || 0;
                let discount = getDiscount(totalQuantity);
                let message = "";

                message += "Khách hàng được chiết khấu <strong>" + formatThousands(discount, ',') + "/cái</strong>";
                message += "<br/>Áp dụng cho tất cả sản phẩm?"

                swal({
                    title: "Gợi ý chiết khấu",
                    text: message,
                    type: "warning",
                    showCancelButton: true,
                    closeOnConfirm: true,
                    cancelButtonText: "Bỏ qua",
                    confirmButtonText: "Xác nhận",
                    html: true
                }, function (isConfirm) {
                    if (isConfirm)
                    {
                        $(".product-result").each(function() {
                            try {
                                //#region Cài đặt chiết khấu
                                /* ============================================================
                                 * Date:   2021-07-19
                                 * Author: Binh-TT
                                 *
                                 * Kiểm tra từng dòng coi chiết khấu nào nhỏ hơn thì áp vào, lớn hơn thì bỏ qua
                                 * ============================================================
                                 */
                                //let $discount = $(this).find(".discount");

                                //if (discount != 0)
                                //    $discount.val(formatThousands(discount, ','));
                                //else
                                //    $discount.val(0);
                                //#endregion

                                //#region Kiểm tra chiết khấu
                                let costOfGoods = $(this).data("costOfGoods");
                                let price = +$(this).find(".gia-san-pham").data("price") || 0;

                                if ((price - discount) < costOfGoods) {
                                    $(this).attr("data-error-discount", true);
                                    $(this).find('td').each(function () { $(this).addClass('red'); });
                                }
                                else if (Boolean($(this).attr("data-error-discount"))) {
                                    $(this).removeAttr("data-error-discount");
                                    $(this).find('td').each(function () { $(this).removeClass('red'); });
                                }
                                //#endregion
                            }
                            catch (err) {
                                console.error(err.message);
                            }
                        });
                        getAllPrice();
                    }
                });
            }

            /* ============================================================
             * Tính lại tiền đơn hàng
             * Được gọi tại:
             * 1) Thêm sản phẩm
             * 2) chiết khấu
             * 3) Coupon
             * 4) Đổi trả hàng
             * 5) Phí khác
             * 6) Khởi tạo đơn hàng
             *
             * Date:   2021-07-19
             * Author: Binh-TT
             *
             * Đối ứng chiết khấu từng dòng
             * ============================================================
             */
            function getAllPrice(isPayAllCall) {
                let $products = $(".product-result");

                let totalQuantity = 0
                ,   totalPrice = 0
                ,   totalDiscount = 0
                ,   totalLeft = 0
                ,   shippingFee = 0
                ,   otherFee = 0
                ,   couponValue = 0
                ,   total = 0;
                // Trường hợp có đơn hàng đổi trả
                let totalRefund = 0
                ,   subTotal = 0;

                if ($products.length > 0) {
                    // Fix bug: Tính chiết khấu
                    $products.find('.in-quantity').each(function (index, item) {
                        totalQuantity += (+item.value || 0);
                    });

                    let totalprice = 0;
                    let productquantity = 0;
                    $(".product-result").each(function () {
                        try {
                            let $discount = $(this).find(".discount");
                            let $totalRow = $(this).find(".totalprice-view");

                            let quantity = +$(this).find(".in-quantity").val() || 0;
                            let price = +$(this).find(".gia-san-pham").attr("data-price") || 0;
                            let discount = +$discount.val().replace(/,/g, '') || 0;
                            let totalRow = 0;

                            // Tính chiết khấu
                            if (!(typeof (isPayAllCall) === "boolean" && isPayAllCall))
                            {
                                let tempDiscount = getDiscount(totalQuantity);

                                if (discount < tempDiscount)
                                {
                                    discount = tempDiscount;
                                    $discount.val(formatThousands(discount, ','))
                                }
                            }

                            totalDiscount += (discount * quantity);
                            totalPrice += (price * quantity);
                            totalRow = (price - discount) * quantity;

                            $totalRow.html(formatThousands(totalRow, ','));
                        }
                        catch (err) {
                            console.error(err.mesage);
                        }
                    });

                    // Tổng tiền sau chiết khấu
                    totalLeft = totalPrice - totalDiscount;
                    // Tổng phí
                    shippingFee = +$("#<%=pFeeShip.ClientID%>").val().replace(/\,/g, '') || 0;
                    otherFee = 0;

                    fees.forEach((item) => { otherFee += item.FeePrice; });
                    // Phiếu giảm giá
                    checkCouponCondition();
                    couponValue = +$("#<%=hdfCouponValue.ClientID%>").val() || 0;
                    // Tổng tiền
                    total = totalLeft + shippingFee + otherFee - couponValue;
                    // Trường hợp có đơn hàng đổi trả
                    totalRefund = +$("#<%=hdfDonHangTra.ClientID%>").val() || 0;
                    subTotal = total - totalRefund;
                }

                // Tổng số lượng
                let $hdfTotalQuantity = $("#<%=hdfTotalQuantity.ClientID%>");
                let $totalQuantity = $(".totalproductQuantity");

                $hdfTotalQuantity.val(totalQuantity);
                $totalQuantity.html(formatThousands(totalQuantity, ',') + " cái");
                // Tổng tiền chưa chiết khấu
                let $hdfTotalPrice = $("#<%=hdfTotalPriceNotDiscount.ClientID%>");
                let $totalPrice = $(".totalpriceorder");

                $hdfTotalPrice.val(totalPrice);
                $totalPrice.html(formatThousands(totalPrice, ','));
                // Tổng tiền chiết khấu
                let $hdfTotalDiscount = $("#<%=hdfTotalDiscount.ClientID%>");
                let $totalDiscount = $(".totalDiscount");

                $hdfTotalDiscount.val(totalDiscount);
                $totalDiscount.html(formatThousands(totalDiscount, ','));
                // Tổng tiền sau chiết khấu
                let $totalLeft = $(".priceafterchietkhau");

                $totalLeft.html(formatThousands(totalLeft, ','))
                // Tổng tiền
                let $hdfTotal = $("#<%=hdfTotalPrice.ClientID%>");
                let $total = $(".totalpriceorderall");

                $hdfTotal.val(total);
                $total.html(formatThousands(total, ','));
                // Trường hợp có đơn hàng đổi trả
                let $hdfSubTotal = $("#<%=hdfTongTienConLai.ClientID%>");
                let $subTotal = $(".totalpricedetail");

                $hdfSubTotal.val(subTotal);
                $subTotal.html(formatThousands(subTotal, ","));

                reIndex(true);
            }

            /* ============================================================
             * Tính lại tiền đơn hàng
             * Được gọi tại:
             * 1) Xóa sản phẩm
             * 2) Phí vận chuyển
             * 3) Trọng lượng đơn hàng
             *
             * Date:   2021-07-19
             * Author: Binh-TT
             *
             * Đối ứng chiết khấu từng dòng
             * ============================================================
             */
            function countTotal() {
                let $products = $(".product-result");
                let $shoppingFee = $("#<%=pFeeShip.ClientID%>");

                let totalQuantity = 0
                ,   totalPrice = 0
                ,   totalDiscount = 0
                ,   totalLeft = 0
                ,   shippingFee = 0
                ,   otherFee = 0
                ,   couponValue = 0
                ,   total = 0;
                // Trường hợp có đơn hàng đổi trả
                let totalRefund = 0
                ,   subTotal = 0;

                if ($products.length > 0) {
                    // Tính tiền từng sản phẩm
                    $products.each(function () {
                        try {
                            let $discount = $(this).find(".discount");
                            let $totalRow = $(this).find(".totalprice-view");

                            let quantity = +$(this).find(".in-quantity").val() || 0;
                            let price = +$(this).find(".gia-san-pham").attr("data-price") || 0;
                            let discount = +$discount.val().replace(/,/g, '') || 0;
                            let totalRow = 0;


                            totalQuantity += quantity;
                            totalDiscount += (discount * quantity);
                            totalPrice += (price * quantity);
                            totalRow = (price - discount) * quantity;

                            $totalRow.html(formatThousands(totalRow, ','));
                        }
                        catch (err) {
                            console.error(err.message);
                        }
                    });

                    // Tổng tiền sau chiết khấu
                    totalLeft = totalPrice - totalDiscount;
                    // Tổng phí
                    shippingFee = +$shoppingFee.val().replace(/\,/g, '') || 0;
                    otherFee = 0;

                    fees.forEach((item) => { otherFee += item.FeePrice; });
                    // Phiếu giảm giá
                    checkCouponCondition();
                    couponValue = +$("#<%=hdfCouponValue.ClientID%>").val() || 0;
                    // Tổng tiền
                    total = totalLeft + shippingFee + otherFee - couponValue;
                    // Trường hợp có đơn hàng đổi trả
                    totalRefund = +$("#<%=hdfDonHangTra.ClientID%>").val() || 0;
                    subTotal = total - totalRefund;
                }

                // Tổng số lượng
                let $hdfTotalQuantity = $("#<%=hdfTotalQuantity.ClientID%>");
                let $totalQuantity = $(".totalproductQuantity");

                $hdfTotalQuantity.val(totalQuantity);
                $totalQuantity.html(formatThousands(totalQuantity, ',') + " cái");
                // Tổng tiền chưa chiết khấu
                let $hdfTotalPrice = $("#<%=hdfTotalPriceNotDiscount.ClientID%>");
                let $totalPrice = $(".totalpriceorder");

                $hdfTotalPrice.val(totalPrice);
                $totalPrice.html(formatThousands(totalPrice, ','));
                // Chiết khấu
                let $discount = $("#<%=pDiscount.ClientID%>");

                if ($discount.val() === '')
                    $discount.val(0);
                // Tổng tiền chiết khấu
                let $hdfTotalDiscount = $("#<%=hdfTotalDiscount.ClientID%>");
                let $totalDiscount = $(".totalDiscount");

                $hdfTotalDiscount.val(totalDiscount);
                $totalDiscount.html(formatThousands(totalDiscount, ','));
                // Tổng tiền sau chiết khấu
                let $totalLeft = $(".priceafterchietkhau");

                $totalLeft.html(formatThousands(totalLeft, ','))
                // Phí giao hàng
                if ($shoppingFee.val() === '')
                    $shoppingFee.val(0);
                // Phí khác
                fees.forEach((item) => {
                    if (item.price === '') {
                        item.FeePrice = 0;
                        $('#' + item.UUID).val(0);
                    }
                })
                // Tổng tiền
                let $hdfTotal = $("#<%=hdfTotalPrice.ClientID%>");
                let $total = $(".totalpriceorderall");

                $hdfTotal.val(total);
                $total.html(formatThousands(total, ','));
                // Trường hợp có đơn hàng đổi trả
                let $hdfSubTotal = $("#<%=hdfTongTienConLai.ClientID%>");
                let $subTotal = $(".totalpricedetail");

                $hdfSubTotal.val(subTotal);
                $subTotal.html(formatThousands(subTotal, ","));
            };

            // get product price
            function getProductPrice(obj) {
                var customertype = obj.val();
                if ($(".product-result").length > 0) {
                    var totalprice = 0;
                    $(".product-result").each(function () {
                        var giasi = $(this).attr("data-giabansi");
                        var giale = $(this).attr("data-giabanle");
                        if (customertype == 1) {
                            if (giale == 0)
                                giale = giasi;
                            $(this).find(".gia-san-pham").attr("data-price", giale).html(formatThousands(giale, ','));
                        }
                        else {
                            $(this).find(".gia-san-pham").attr("data-price", giasi).html(formatThousands(giasi, ','));
                        }
                    });
                    getAllPrice(true);
                }
            }

            // press key
            function keypress(e) {
                var keypressed = null;
                if (window.event) {
                    keypressed = window.event.keyCode; //IE
                }
                else {
                    keypressed = e.which; //NON-IE, Standard
                }
                if (keypressed < 48 || keypressed > 57) {
                    if (keypressed == 8 || keypressed == 127) {
                        return;
                    }
                    return false;
                }
            }

            // format price
            var formatThousands = function (n, dp) {
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

            function updateShippingFeeFromModal() {
                let newValue = +$("#<%=txtShippingFeeModal.ClientID%>").val() || 0;
                let shippingFeeDOM = $("#<%=pFeeShip.ClientID%>");
                shippingFeeDOM.val(newValue);
                getAllPrice(true);
            }

            function onchangePaymentType(payType) {
                $("#<%=ddlBank.ClientID%>").val(0);

                // Khác hình thức chuyển khoản
                if (payType.val() != 2) {
                    $("#bankModal").attr("hidden", true);
                }
                else {
                    $("#bankModal").removeAttr("hidden");
                }
            }

            function onchangeShippingType(shipType) {
                let $transportModal = $("#transportModal");
                let $transportSubModal = $("#transportSubModal");
                let $shippingFeeModal = $("#shippingFeeModal");

                $shippingFeeModal.removeAttr("hidden");
                $transportModal.attr("hidden", true);
                $transportSubModal.attr("hidden", true);

                if (shipType.val() == 4) {
                    let tranContainerDOM = $("[id$=ddlTransportCompanyID-container]");
                    let tranSubContainerDOM = $("[id$=ddlTransportCompanySubID-container]");

                    tranContainerDOM.val(0);
                    tranSubContainerDOM.attr("title", "Nhà chành xe");
                    tranSubContainerDOM.html("Nhà chành xe");
                    tranSubContainerDOM.val(0);
                    tranSubContainerDOM.attr("title", "Chọn nơi nhận");
                    tranSubContainerDOM.html("Chọn nơi nhận");

                    $transportModal.removeAttr("hidden");
                    $transportSubModal.removeAttr("hidden");
                }
                else if (shipType.val() == 6 || shipType.val() == 10) {
                    $shippingFeeModal.attr("hidden", true);
                    $("<%=txtShippingFeeModal.ClientID%>").val(0);

                    let $pFeeShip = $("#<%=pFeeShip.ClientID%>");
                    let fee = parseInt($pFeeShip.val().replace(/,/g, '')) || 0

                    if (fee > 0)
                    {
                        $("#<%=pFeeShip.ClientID%>").val(0);

                        getAllPrice(true);
                    }
                }
            }

            function onChangeTransportCompany(transport, selected) {
                if (selected == undefined) {
                    selected = "0";
                }

                let transComID = transport.val();
                $.ajax({
                    url: "/them-moi-don-hang.aspx/getTransportSub",
                    type: "POST",
                    data: JSON.stringify({ 'transComID': transComID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        let data = JSON.parse(msg.d);
                        if (data) {
                            let tranSubName = "Chọn nơi nhận";
                            let tranSubDOM = $("#<%=ddlTransportCompanySubID.ClientID%>");
                            tranSubDOM.html("")
                            data.forEach((item) => {
                                if (selected == item.key) {
                                    tranSubName = item.value;
                                }
                                tranSubDOM.append("<option value='" + item.key + "'>" + item.value + "</option>")
                            });

                            tranSubDOM.val(selected);
                            let tranSubContainerDOM = $("[id$=ddlTransportCompanySubID-container]");
                            tranSubContainerDOM.attr("title", tranSubName);
                            tranSubContainerDOM.html(tranSubName);

                            if (selected == "0") {
                                setTimeout(function () {
                                    $("#<%=ddlTransportCompanySubID.ClientID%>").select2("open");
                                }, 200);
                            }
                        }
                    },
                    error: function (err) {
                        swal("Thông báo", "Đã có vấn đề trong việc cập nhật thông tin vận chuyển", "error");
                    }
                });
            }

            function suggestBank(customerID) {
                $.ajax({
                    url: "/them-moi-don-hang.aspx/getTransferLast",
                    type: "POST",
                    data: JSON.stringify({ 'customerID': customerID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        let data = JSON.parse(response.d);
                        let banksDOM = $("#<%=ddlBank.ClientID%>");
                        if (data) {
                            banksDOM.val(data.value);
                        }
                        else {
                            banksDOM.val(0);
                        }
                    },
                    error: function (err) {
                        swal("Thông báo", "Đã có vần đề trong việc lấy thông tin gợi ý bank", "error");
                    }
                });
            }

            function suggestDelivery(customerID) {
                $.ajax({
                    url: "/them-moi-don-hang.aspx/getDeliveryLast",
                    type: "POST",
                    data: JSON.stringify({ 'customerID': customerID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        let data = JSON.parse(response.d);
                        let transDOM = $("#<%=ddlTransportCompanyID.ClientID%>");
                        let transSubDOM = $("#<%=ddlTransportCompanySubID.ClientID%>");
                        let tranContainerDOM = $("[id$=ddlTransportCompanyID-container]");
                        let tranSubContainerDOM = $("[id$=ddlTransportCompanySubID-container]");

                        if (data) {
                            transDOM.val(data.tranID);
                            tranContainerDOM.attr("title", data.tranName);
                            tranContainerDOM.html(data.tranName);
                            onChangeTransportCompany(transDOM, data.tranSubID);
                        }
                        else {
                            tranContainerDOM.val(0);
                            tranContainerDOM.attr("title", "Nhà chành xe");
                            tranContainerDOM.html("Nhà chành xe");
                            tranSubContainerDOM.val(0);
                            tranSubContainerDOM.attr("title", "Chọn nơi nhận");
                            tranSubContainerDOM.html("Chọn nơi nhận");
                        }
                    },
                    error: function (err) {
                        swal("Thông báo", "Đã có vần đề trong việc lấy thông tin gợi ý bank", "error");
                    }
                });
            };

            function suggestOrderLast(customerID) {
                $.ajax({
                    url: "/them-moi-don-hang.aspx/getOrderLast",
                    type: "POST",
                    data: JSON.stringify({ 'customerID': customerID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        let data = JSON.parse(response.d);
                        let payType = $("#<%=ddlPaymentType.ClientID%>");
                        let banksDOM = $("#<%=ddlBank.ClientID%>");
                        let shipType = $("#<%=ddlShippingType.ClientID%>");
                        let transDOM = $("#<%=ddlTransportCompanyID.ClientID%>");
                        let transSubDOM = $("#<%=ddlTransportCompanySubID.ClientID%>");
                        let tranContainerDOM = $("[id$=ddlTransportCompanyID-container]");
                        let tranSubContainerDOM = $("[id$=ddlTransportCompanySubID-container]");
                        let shippingFeeDOM = $("#<%=txtShippingFeeModal.ClientID%>");
                        if (data) {
                            // Phương thức thanh toán
                            payType.val(data.payType);
                            onchangePaymentType(payType);
                            // Ngân hàng
                            if (data.payType == "2")
                                banksDOM.val(data.bankID);
                            else
                                banksDOM.val(0);
                            // Phương thức giao hàng
                            shipType.val(data.shipType);
                            onchangeShippingType(shipType);

                            // Chành xe & nơi tới
                            if (data.shipType == "4") {
                                transDOM.val(data.tranID);
                                tranContainerDOM.attr("title", data.tranName);
                                tranContainerDOM.html(data.tranName);
                                onChangeTransportCompany(transDOM, data.tranSubID);
                            }

                            // Phí vận chuyển
                            if (data.shipType != "6" && shippingFeeDOM.val() == 0) {
                                shippingFeeDOM.val(formatNumber(data.shippingFee));
                                $("#<%=pFeeShip.ClientID%>").val(formatNumber(data.shippingFee));

                                getAllPrice(true);
                            }
                        }
                        else {
                            // Ngân hàng
                            banksDOM.val(0);
                            // Chành xe & nơi tới
                            tranContainerDOM.val(0);
                            tranContainerDOM.attr("title", "Nhà chành xe");
                            tranContainerDOM.html("Nhà chành xe");
                            tranSubContainerDOM.val(0);
                            tranSubContainerDOM.attr("title", "Chọn nơi nhận");
                            tranSubContainerDOM.html("Chọn nơi nhận");
                        }
                    },
                    error: function (err) {
                        swal("Thông báo", "Đã có vần đề trong việc lấy thông tin gợi ý bank", "error");
                    }
                });
            };

            function createReturnOrder(customerID) {
                var win = window.open('/tao-don-hang-doi-tra?customerID=' + customerID, '_blank');
                if (win) {
                    //Browser has allowed it to be opened
                    win.focus();
                } else {
                    //Browser has blocked it
                    swal("Thông báo", "Vui lòng cho phép cửa sổ bật lên cho trang web này", "error");
                }
            };

            function openCouponModal() {
                let customerID = +document.querySelector('[id$="_hdfCustomerID"]').value || 0;
                let productNumber = +document.querySelector('[id$="_hdfTotalQuantity"]').value || 0;

                if (!customerID)
                    return swal("Thông báo", "Chưa nhập thông tin khách hàng! Hoặc đây là khách hàng mới...", "warning");

                if (!productNumber)
                    return swal("Thông báo", "Chưa nhập sản phẩm!", "warning");

                let couponModalDOM = $('#couponModal');
                let codeDOM = couponModalDOM.find("[id$='_txtCouponCode']");
                let errorDOM = couponModalDOM.find("#errorCoupon");
                let txtCouponValue = $('[id$="_txtCouponValue"]');
                let hdfCouponID = $('[id$="_hdfCouponID"]');
                let hdfCouponValue = $('[id$="_hdfCouponValue"]');

                if (codeDOM)
                    codeDOM.val('');

                if (errorDOM) {
                    errorDOM.addClass('hide');
                    errorDOM.find('p').html('');
                }

                if (txtCouponValue)
                    txtCouponValue.val('0');

                if (hdfCouponID)
                    hdfCouponID.val('0');

                if (hdfCouponValue)
                    hdfCouponValue.val('0');

                couponModalDOM.modal({ show: 'true', backdrop: 'static', keyboard: false });
                couponModalDOM.on('shown.bs.modal', function () {
                    codeDOM.focus();
                });
            }

            function getCoupon() {
                let couponModalDOM = document.querySelector('#couponModal');
                let codeDOM = document.querySelector('[id$="_txtCouponCode"]');
                let errorDOM = document.querySelector('#errorCoupon');

                let customerID = +document.querySelector('[id$="_hdfCustomerID"]').value || 0;
                let code = codeDOM.value.trim() || "";
                let productNumber = +document.querySelector('[id$="_hdfTotalQuantity"]').value || 0;
                let price = +document.querySelector('[id$="_hdfTotalPrice"]').value || 0;

                if (!code) {
                    errorDOM.classList.remove('hide')
                    errorDOM.querySelector('p').innerText = "Hãy nhập mã giảm giá!";

                    codeDOM.focus();
                    codeDOM.select();

                    return;
                }

                $.ajax({
                    type: "POST",
                    url: "/them-moi-don-hang.aspx/getCoupon",
                    data: JSON.stringify({ "customerID": customerID, "code": code, "productNumber": productNumber, "price": price }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (respon) => {
                        let data = JSON.parse(respon.d);

                        if (data) {
                            if (!data.status) {
                                errorDOM.classList.remove('hide')
                                errorDOM.querySelector('p').innerText = data.message;

                                codeDOM.focus();
                                codeDOM.select();
                            }
                            else {
                                document.querySelector('[id$="_txtCouponValue"]').value = `${code.trim().toUpperCase()}: -${formatThousands(+data.value || 0, ',')}`;
                                document.querySelector('[id$="_hdfCouponID"]').value = +data.couponID || 0;
                                document.querySelector('[id$="_hdfCouponValue"]').value = +data.value || 0;
                                document.querySelector('[id$="_hdfCouponProductNumber"]').value = +data.productNumber || 0;
                                document.querySelector('[id$="_hdfCouponPriceMin"]').value = +data.priceMin || 0;

                                couponModalDOM.querySelector('#closeCoupon').click();
                                document.querySelector('#btnOpenCouponModal').classList.add('hide');
                                document.querySelector('#btnRemoveCouponCode').classList.remove('hide');
                                document.querySelector('#btnGenerateCouponG25').classList.add('hide');

                                getAllPrice(true);
                            }
                        }
                        else {
                            errorDOM.classList.remove('hide')
                            errorDOM.querySelector('p').innerText = `Mã giảm giá ${code} không tồn tại!`;

                            codeDOM.focus();
                            codeDOM.select();
                        }
                    },
                    error: (xmlhttprequest, textstatus, errorthrow) => {
                        swal("Thông báo", "Đã xảy ra lỗi trong quá trình lấy mã giảm giá", "error");
                    }
                });
            }

            function removeCoupon() {
                document.querySelector('[id$="_txtCouponValue"]').value = 0;
                document.querySelector('[id$="_hdfCouponID"]').value = 0;
                document.querySelector('[id$="_hdfCouponValue"]').value = 0;
                document.querySelector('[id$="_hdfCouponProductNumber"]').value = 0;
                document.querySelector('[id$="_hdfCouponPriceMin"]').value = 0;
                document.querySelector('#btnOpenCouponModal').classList.remove('hide');
                document.querySelector('#btnRemoveCouponCode').classList.add('hide');
                document.querySelector('#btnGenerateCouponG25').classList.remove('hide');

                getAllPrice(true);
            }

            function checkCouponCondition() {
                let couponID = +document.querySelector('[id$="_hdfCouponID"]').value || 0;

                if (couponID > 0) {
                    let couponProductNumber = +document.querySelector('[id$="_hdfCouponProductNumber"]').value || 0;
                    let couponPriceMin = +document.querySelector('[id$="_hdfCouponPriceMin"]').value || 0;
                    let productNumber = +document.querySelector('[id$="_hdfTotalQuantity"]').value || 0;
                    let price = +document.querySelector('[id$="_hdfTotalPrice"]').value || 0;

                    if (!(productNumber >= couponProductNumber && price >= couponPriceMin)) {
                        removeCoupon();
                        return setTimeout(_ => { swal("Thông báo", "Đã xóa mã giảm giá, do không đạt yêu cầu!", "warning") }, 300);
                    }
                }
            }

            function couponG25() {
                let customerID = +document.querySelector('[id$="_hdfCustomerID"]').value || 0;
                let customerName = $("#<%=txtFullname.ClientID%>").val();
                if (!customerID)
                    return swal("Thông báo", "Chưa nhập thông tin khách hàng! Hoặc đây là khách hàng mới...", "warning");

                generateCouponG25(customerName, customerID);
            }

            /* ============================================================
             * Date:   2021-07-19
             * Author: Binh-TT
             *
             * Đối ứng chiết khấu từng dòng
             * ============================================================
             */
            function onChangeDiscount($input) {
                let value = $input.val();

                if (value === "" || value === null)
                    $input.val("0");
                else
                {
                    value = +value.replace(/,/g, '') || 0;
                    $input.val(formatThousands(value, ','));

                    let $row = $input.closest('tr');
                    let costOfGoods = +$row.data('costOfGoods') || 0;
                    let price = +$row.find('.gia-san-pham').data('price') || 0;

                    if ((price - value) < costOfGoods) {
                        $row.attr("data-error-discount", true);
                        $row.find('td').each(function () { $(this).addClass('red'); });
                    }
                    else if (Boolean($row.attr("data-error-discount"))) {
                        $row.removeAttr("data-error-discount");
                        $row.find('td').each(function () { $(this).removeClass('red'); });
                    }
                }

                getAllPrice(true);
            }

            function pressKeyDiscount($input) {
                let notNumberReg = new RegExp(/\D/g);

                if (notNumberReg.test($input.val()))
                    $input.val().replace(notNumberReg, '');

                let keyCode = $input.which;
                let $row = $input.closest('tr');

                $input.keyup(function (e) {
                    if (e.which == 40) {
                        // press down
                        let $nextRow = $row.next();
                        let $inputDown = $nextRow.find('.discount-item').find('input');

                        $inputDown.focus().select();
                    }
                    else if (e.which == 38) {
                        // press up
                        let $prevRow = $row.prev();
                        let $inputAbove = $prevRow.find('.discount-item').find('input');

                        $inputAbove.focus().select();
                    }
                });
            }

            // Lấy ra mức chiết khấu của khách hàng
            function getDiscount(totalQuantity) {
                let discount = 0;

                //#region Lấy các mức chiết khấu của hệ thống
                let discountPolicy = $("input[id$='_hdfChietKhau']").val() || "";

                if (discountPolicy) {
                    let quantityDiscounts = discountPolicy.split('|').filter(x => x);

                    // Lấy chiết khấu thỏa điều kiện
                    quantityDiscounts = quantityDiscounts
                        .filter(x => parseInt(x.split('-')[0]) <= totalQuantity);

                    if (quantityDiscounts.length > 0) {
                        // Lấy chiết khấu cuối
                        let lastQuantityDiscount = quantityDiscounts.slice(-1).pop();

                        discount = +lastQuantityDiscount.split('-')[1] || 0;
                    }
                }
                //#endregion

                //#region Lấy chiết khấu theo nhóm chiết khấu khách hàng
                let isDiscount = +$("#<%=hdfIsDiscount.ClientID%>").val() || 0;
                let quantityRequirement = +$("#<%=hdfQuantityRequirement.ClientID%>").val() || 0;

                if (isDiscount && totalQuantity >= quantityRequirement)
                {
                    let groupDiscount = +$("#<%=hdfDiscountAmount.ClientID%>").val() || 0;

                    if (discount < groupDiscount)
                        discount = groupDiscount;
                }
                //#endregion

                return discount;
            }

            function onBlurPDiscount($pDiscount) {
                let $product = $(".product-result");

                if ($product.length == 0)
                    return;

                //#region Tính lại chiết khấu
                let discount = +$pDiscount.val().replace(/,/g, '') || 0;

                if (discount == 0 && $pDiscount.val() === '')
                    $pDiscount.val(0);

                swal({
                    title: "Chiết khấu",
                    text: "Áp dụng chiết khấu <strong>" + formatThousands(discount, ',') + "/cái</strong> cho tất sản phẩm?",
                    type: "warning",
                    showCancelButton: true,
                    closeOnConfirm: true,
                    cancelButtonText: "Bỏ qua",
                    confirmButtonText: "Xác nhận",
                    html: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        $product.each(function () {
                            try {
                                //#region Cài đặt chiết khấu
                                let $discount = $(this).find('.discount');

                                if (discount != 0)
                                    $discount.val(formatThousands(discount, ','));
                                else
                                    $discount.val(0);
                                //#endregion

                                //#region Kiểm tra chiết khấu
                                let costOfGoods = $(this).data("costOfGoods");
                                let price = +$(this).find(".gia-san-pham").data("price") || 0;

                                if ((price - discount) < costOfGoods) {
                                    $(this).attr("data-error-discount", true);
                                    $(this).find('td').each(function () { $(this).addClass('red'); });
                                }
                                else if (Boolean($(this).attr("data-error-discount"))) {
                                    $(this).removeAttr("data-error-discount");
                                    $(this).find('td').each(function () { $(this).removeClass('red'); });
                                }
                                //#endregion
                            }
                            catch (err) {
                                console.error(err.message);
                            }
                        });

                        $pDiscount.val(0);
                        getAllPrice(true);
                    }
                });
                //#endregion
            }
            /* ============================================================
             * Đối ứng chiết khấu từng dòng (END)
             * ============================================================
             */
        </script>
    </telerik:RadScriptBlock>

</asp:Content>
