<%@ Page Title="Máy tính tiền" Language="C#" MasterPageFile="~/POSPage.Master" AutoEventWireup="true" CodeBehind="pos.aspx.cs" Inherits="IM_PJ.pos" EnableSessionState="ReadOnly" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/App_Themes/Ann/js/search-customer.js?v=20200703145200"></script>
    <script src="/App_Themes/Ann/js/search-product.js?v=02112020"></script>
    <script src="/App_Themes/Ann/js/pages/danh-sach-khach-hang/generate-coupon-for-customer.js?v=02112020"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .coupon .right {
            display: flex;
        }
    </style>
    <asp:Panel ID="parent" runat="server">
        <main id="main-wrap">
            <div class="container">
                <div class="row">
                    <div class="col-md-4">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Khách hàng</h3>
                                <a href="javascript:;" class="search-customer" onclick="searchCustomer()" title="Tìm khách hàng"><i class="fa fa-search" aria-hidden="true"></i>Tìm (F1)</a>
                                <a href="javascript:;" class="change-user" onclick="changeUser()" title="Tính tiền giúp nhân viên khác"><i class="fa fa-user" aria-hidden="true"></i></a>
                                <a href="/danh-sach-don-hang" class="change-user" target="_blank" title="Danh sách đơn hàng"><i class="fa fa-list-ul" aria-hidden="true"></i></a>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Họ tên</label>
                                            <asp:TextBox ID="txtFullname" CssClass="form-control capitalize" runat="server" placeholder="(F2)" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Điện thoại</label>
                                            <asp:TextBox ID="txtPhone" CssClass="form-control" onblur="ajaxCheckCustomer()" runat="server" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Tỉnh thành</label>
                                            <asp:DropDownList ID="ddlProvince" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Quận huyện</label>
                                            <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Phường xã</label>
                                            <asp:DropDownList ID="ddlWard" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Địa chỉ</label>
                                            <asp:TextBox ID="txtAddress" CssClass="form-control capitalize" runat="server" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-row view-detail" style="display: none">
                                </div>
                                <div class="form-row discount-info" style="display: none">
                                </div>
                            </div>
                        </div>
                        <div id="deliveryAddress" class="row">
                            <div class="col-md-12">
                                <div class="panel panelborderheading">
                                    <div class="panel-heading clear">
                                        <h3 class="page-title left not-margin-bot">Thông tin nhận hàng</h3>
                                        <a href="javascript:;" class="search-customer" onclick="showDeliveryAddresses()"><i class="fa fa-search" aria-hidden="true"></i>Danh sách nhận hàng</a>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label>Họ tên</label>
                                                    <asp:TextBox ID="txtRecipientFullName" runat="server" CssClass="form-control capitalize" placeholder="Họ tên người nhận hàng"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label>Điện thoại</label>
                                                    <asp:TextBox ID="txtRecipientPhone" runat="server" CssClass="form-control" placeholder="Số điện thoại người nhận hàng"></asp:TextBox>
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
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label>Địa chỉ</label>
                                                    <asp:TextBox ID="txtRecipientAddress" runat="server" CssClass="form-control capitalize" placeholder="Địa chỉ khách hàng"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel-post">
                            <div class="post-row clear totalquantity">
                                <div class="left">Số lượng</div>
                                <div class="right totalproductQuantity"></div>
                            </div>
                            <div class="post-row clear subtotal hide">
                                <div class="left">Thành tiền</div>
                                <div class="right totalpriceorder"></div>
                            </div>
                            <div class="post-row clear discount hide">
                                <div class="left">Chiết khấu</div>
                                <div class="right totalDiscount">
                                    <a href="javascript:;" class="btn btn-feeship link-btn" onclick="refreshDiscount()"><i class="fa fa-refresh" aria-hidden="true"></i></a>
                                    <telerik:RadNumericTextBox runat="server" CssClass="form-control width-notfull input-discount" Skin="MetroTouch"
                                        ID="pDiscount" MinValue="0" NumberFormat-GroupSizes="3" Value="0" NumberFormat-DecimalDigits="0"
                                        oninput="countTotal()" IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
                                    </telerik:RadNumericTextBox>
                                </div>
                            </div>
                            <div class="post-row clear discount hide">
                                <div class="left">Sau chiết khấu</div>
                                <div class="right priceafterchietkhau"></div>
                            </div>
                            <div class="post-row clear shipping hide">
                                <div class="left">Phí vận chuyển</div>
                                <div class="right totalDiscount">
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
                                    <a id="btnGenerateCouponG25" class="btn btn-coupon btn-violet" title="Kiểm tra mã giảm giá G25" onclick="couponG25()"><i class="fa fa-gift"></i>G25</a>
                                    <a id="btnOpenCouponModal" class="btn btn-coupon btn-violet" title="Nhập mã giảm giá" onclick="openCouponModal()"><i class="fa fa-gift"></i></a>
                                    <a href="javascript:;" id="btnRemoveCouponCode" class="btn btn-coupon link-btn hide" onclick="removeCoupon()"><i class="fa fa-times" aria-hidden="true"></i></a>
                                    <asp:TextBox ID="txtCouponValue" runat="server" CssClass="form-control text-right width-notfull input-coupon" value="0" disabled="disabled"></asp:TextBox>
                                </div>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Tổng tiền</div>
                                <div class="right totalpriceorderall price-red"></div>
                            </div>
                            <div class="post-row clear returnorder hide">
                                <div class="left">
                                    Hàng trả
                                </div>
                                <div class="right ">
                                    <a href="javascript:;" class="find2 hide btn btn-return-order link-btn"></a>
                                    <a href="javascript:;" class="find3 hide btn btn-return-order link-btn btn-edit-fee" onclick="searchReturnOrder()"><i class="fa fa-refresh" aria-hidden="true"></i></a>
                                    <a href="javascript:;" class="find3 hide btn btn-feeship link-btn" onclick="deleteReturnOrder()"><i class="fa fa-times" aria-hidden="true"></i></a>
                                    <span class="totalpriceorderrefund"></span>
                                </div>
                            </div>
                            <div class="post-row clear refund hide">
                                <div class="left">Tổng tiền còn lại</div>
                                <div class="right totalpricedetail"></div>
                            </div>
                            <div class="post-row clear hide">
                                <div class="left">Tiền khách trả (F4)</div>
                                <div class="right totalDiscount">
                                    <telerik:RadNumericTextBox runat="server" CssClass="form-control width-notfull" Skin="MetroTouch"
                                        ID="pGuestPaid" MinValue="0" NumberFormat-GroupSizes="3" Width="100%" Value="0" NumberFormat-DecimalDigits="0"
                                        oninput="countGuestChange()" IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
                                    </telerik:RadNumericTextBox>
                                </div>
                            </div>
                            <div class="post-row clear hide">
                                <div class="left">Tiền thối lại</div>
                                <div class="right totalGuestChange"></div>
                            </div>
                            <div class="post-table-links clear">
                                <a href="javascript:;" class="btn link-btn" style="background-color: #ffad00" onclick="searchReturnOrder()" title="Nhập đơn hàng đổi trả"><i class="fa fa-refresh"></i>Đổi trả</a>
                                <a href="javascript:;" class="btn link-btn" style="background-color: #00a2b7" onclick="showShipping()" title="Nhập phí vận chuyển"><i class="fa fa-truck"></i>Ship</a>
                                <a href="javascript:;" class="btn link-btn" style="background-color: #453288" onclick="showDiscount()" title="Nhập chiết khấu mỗi cái"><i class="fa fa-tag"></i>Chiết khấu</a>
                                <a id="feeNewStatic" href="#feeModal" class="btn link-btn" style="background-color: #607D8B;" data-toggle="modal" data-backdrop='static' title="Thêm phí khác vào đơn hàng"><i class="fa fa-plus"></i>Phí</a>
                            </div>
                            <div class="post-table-links clear">
                                <a href="javascript:;" class="btn link-btn btn-complete-order" onclick="payAll()" title="Hoàn tất đơn hàng"><i class="fa fa-floppy-o"></i>Thanh toán (F9)</a>
                                <asp:Button ID="btnOrder" runat="server" OnClick="btnOrder_Click" Style="display: none" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-8">
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
                            <div class="post-body clear">
                                <table class="table table-checkable table-product table-pos-order">
                                    <thead>
                                        <tr>
                                            <th class="image-item">Ảnh</th>
                                            <th class="name-item">Sản phẩm</th>
                                            <th class="sku-item">Mã</th>
                                            <th class="variable-item">Thuộc tính</th>
                                            <th class="price-item">Giá</th>
                                            <th class="quantity-item">Mua</th>
                                            <th class="total-item">Tổng</th>
                                            <th class="trash-item"></th>
                                        </tr>
                                    </thead>
                                </table>
                                <div class="search-product-content scrollbar">
                                    <table class="table table-checkable table-product table-pos-order">
                                        <tbody class="content-product">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdfRoleID" runat="server" />
            <asp:HiddenField ID="hdfUsername" runat="server" />
            <asp:HiddenField ID="hdfUsernameCurrent" runat="server" />
            <asp:HiddenField ID="hdfOrderType" runat="server" />
            <asp:HiddenField ID="hdfTotalPrice" runat="server" />
            <asp:HiddenField ID="hdfTotalPriceNotDiscount" runat="server" />
            <asp:HiddenField ID="hdfListProduct" runat="server" />
            <asp:HiddenField ID="hdfIsDiscount" runat="server" />
            <asp:HiddenField ID="hdfDiscountAmount" runat="server" />
            <asp:HiddenField ID="hdfQuantityRequirement" runat="server" />
            <asp:HiddenField ID="hdfTotalPriceNotDiscountNotFee" runat="server" />
            <asp:HiddenField ID="hdfListSearch" runat="server" />
            <asp:HiddenField ID="hdfTotalQuantity" runat="server" />
            <asp:HiddenField ID="hdfcheck" runat="server" />
            <asp:HiddenField ID="hdfChietKhau" runat="server" />
            <asp:HiddenField ID="hdfListUser" runat="server" />
            <asp:HiddenField ID="hdfDonHangTra" runat="server" />
            <asp:HiddenField ID="hdfTongTienConLai" runat="server" />
            <asp:HiddenField ID="hdSession" runat="server" />
            <asp:HiddenField ID="hdStatusPage" runat="server" />
            <asp:HiddenField ID="hdfFeeType" runat="server" />
            <asp:HiddenField ID="hdfOtherFees" runat="server" />
            <asp:HiddenField ID="hdfCustomerID" runat="server" />
            <asp:HiddenField ID="hdfCouponID" runat="server" />
            <asp:HiddenField ID="hdfCouponValue" runat="server" />
            <asp:HiddenField ID="hdfCouponProductNumber" runat="server" />
            <asp:HiddenField ID="hdfCouponPriceMin" runat="server" />
            <asp:HiddenField ID="hdfProvinceID" runat="server" />
            <asp:HiddenField ID="hdfDistrictID" runat="server" />
            <asp:HiddenField ID="hdfWardID" runat="server" />

            <!-- Biến đăng ký địa chỉ nhận hàng -->
            <asp:HiddenField ID="hdfDeliveryAddressId" runat="server" />
            <asp:HiddenField ID="hdfRecipientFullName" runat="server" />
            <asp:HiddenField ID="hdfRecipientPhone" runat="server" />
            <asp:HiddenField ID="hdfRecipientProvinceId" runat="server" />
            <asp:HiddenField ID="hdfRecipientDistrictId" runat="server" />
            <asp:HiddenField ID="hdfRecipientWardId" runat="server" />
            <asp:HiddenField ID="hdfRecipientAddress" runat="server" />
            <!-- Biến đăng ký địa chỉ nhận hàng -->

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
                            <button id="createReturnOrder" type="button" class="btn btn-primary" data-dismiss="modal">Tạo đơn hàng đổi trả</button>
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
                            <h4 id="warningTextOrder" class="hide">Khách hàng này đang có đơn hàng đang xử lý!</h4>
                            <h4 id="warningTextOrderReturn" class="hide">Khách hàng này đang có đơn hàng đổi trả chưa trừ tiền!</h4>
                        </div>
                        <div class="modal-footer">
                            <button id="closeOrderOld" type="button" class="btn btn-default" data-dismiss="modal">Vẫn tiếp tục</button>
                            <button id="openOrder" type="button" class="btn btn-primary">Xem đơn đang xử lý</button>
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
        .menuin #main-wrap {
            left: 0;
            width: 100%;
            margin-bottom: 0;
            margin-top: 0;
            padding-top: 0;
            padding-bottom: 0;
        }

        #main-wrap {
            top: 5px;
        }

        .search-product-content {
            background: #fff;
            overflow-y: scroll;
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

        .panel-post .post-table-links .btn {
            width: 25%;
            padding-left: 5px;
            padding-right: 5px;
        }

        @media (min-width: 992px) {
            .container {
                width: 100%;
            }
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
        <script type="text/javascript" src="App_Themes/Ann/js/delivery-address.js?v=202101211340"></script>
        <script type="text/javascript">
            "use strict";

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

            // fee type list
            var feetype = [];

            // fees list
            var fees = [];

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
                    addHTML += "        <span class='otherfee-name'>" + fee.FeeTypeName + "</span>";
                    addHTML += "    </div>";
                    addHTML += "    <div class='right otherfee-value' onclick='openFeeUpdateModal($(this))'>";
                    addHTML += "        <a href='javascript:;' class='btn btn-other-fee link-btn btn-edit-fee' onclick='editOtherFee(`" + fee.UUID + "`)'>";
                    addHTML += "            <i class='fa fa-pencil-square-o' aria-hidden='true'></i>";
                    addHTML += "        </a>";
                    addHTML += "        <a href='javascript:;' class='btn btn-other-fee link-btn' onclick='removeOtherFee(`" + fee.UUID + "`)'>";
                    addHTML += "            <i class='fa fa-times' aria-hidden='true'></i>";
                    addHTML += "        </a>";
                    addHTML += "        <input id='feePrice' type='text' class='form-control text-right' placeholder='Số tiền phí' disabled='disabled' value='" + negative + formatNumber(fee.FeePrice.toString()) + "'/>";
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
                $("#fee-list").append(createFeeHTML(fee));
                $("#<%=hdfOtherFees.ClientID%>").val(JSON.stringify(fees));
                getAllPrice();
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
                getAllPrice();
            }

            // focus to searchProduct input when page on ready
            $(document).ready(function () {

                _initReceiverAddress();
                _onChangeReceiverAddress();

                // Init Page
                $("#<%=pDiscount.ClientID%>").val(0);
                $("#<%=pFeeShip.ClientID%>").val(0);

                // Load Fee Type List
                let data = JSON.parse($("#<%=hdfFeeType.ClientID%>").val());
                data.forEach((item) => {
                    feetype.push(new FeeType(item.ID, item.Name, item.IsNegativeFee));
                });

                // hide header in template
                $("#header").html("");

                $(".account-note").html("").hide();

                $("#txtSearch").focus();

                $(".product-result").dblclick(function () {
                    if (!$(this).find("td").eq(1).hasClass("checked")) {
                        $(this).find("td").addClass("checked");
                    }
                    else {
                        $(this).find("td").removeClass("checked");
                    }
                });

                $("#<%=txtPhone.ClientID%>").keyup(function (e) {
                    if (/\D/g.test(this.value)) {
                        // Filter non-digits from input value.
                        this.value = this.value.replace(/\D/g, '');
                    }
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

                $("#createReturnOrder").click(() => {
                    let customerID = $("#<%=hdfCustomerID.ClientID%>").val();

                    if (!customerID) {
                        swal("Thông báo", "Không tìm thấy ID của khách hàng này", "error");
                    }
                    else {
                        let currentUser = $("#<%=hdfUsernameCurrent.ClientID%>").val();
                        let mainUser = $("#<%=hdfUsername.ClientID%>").val();
                        if (currentUser != mainUser) {
                            createReturnOrder(customerID, currentUser);
                        }
                        else {
                            createReturnOrder(customerID, "");
                        }
                    }
                });

                $('[id$="_txtCouponCode"]').keypress((event) => {
                    if (event.which == 13)
                        getCoupon();
                });
            });

            // set height for div product list
            $(".search-product-content").height($(window).height() - 150);

            // search Product by SKU
            $("#txtSearch").keydown(function (event) {
                if (event.which === 13) {
                    searchProduct();
                    event.preventDefault();
                    return false;
                }
            });

            // check data before close page or refresh page
            window.onbeforeunload = function () {
                if ($(".product-result").length > 0 || $("#<%=txtPhone.ClientID%>").val() != "" || $("#<%= txtFullname.ClientID%>").val() != "")
                    return "You're leaving the site.";
            };

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
                if (e.which == 120) { //F9 Pay all
                    payAll();
                    return false;
                }
            });

            // quickly input change
            $("#<%=pGuestPaid.ClientID%>").keydown(function (e) {
                if (e.which == 13) {
                    var value = $("#<%=pGuestPaid.ClientID%>").val() + "000";
                    $("#<%=pGuestPaid.ClientID%>").val(value);
                    getAllPrice();
                    $("#<%=pGuestPaid.ClientID%>").blur();
                    return false;
                }
            });

            function changeUser() {
                var username = $("#<%=hdfUsername.ClientID%>").val();
                var usernamecurrent = $("#<%=hdfUsernameCurrent.ClientID%>").val();

                if (usernamecurrent != username) {
                    swal({
                        title: 'Lưu ý',
                        text: 'Em đang tính tiền giúp nhân viên <strong>' + usernamecurrent + '</strong>.<br><br>Em không muốn tính nữa???',
                        type: 'warning',
                        showCancelButton: true,
                        closeOnConfirm: false,
                        cancelButtonText: "Vẫn tính tiền giúp!",
                        confirmButtonText: "OK không tính tiền giúp nữa..",
                        html: true,
                    }, function (confirm) {
                        if (confirm) {
                            swal.close();
                            $("#<%=hdfUsernameCurrent.ClientID%>").val(username);
                            $("#<%=hdfUsername.ClientID%>").val(username);
                            $("#<%=txtPhone.ClientID%>").val("").prop('readonly', false).prop('disabled', false);
                            $("#<%=txtFullname.ClientID%>").val("").prop('readonly', false).prop('disabled', false).focus();
                            $("#<%=txtAddress.ClientID%>").val("").prop('readonly', false).prop('disabled', false);
                            $(".view-detail").html("").hide();
                            $(".discount-info").html("").hide();
                            $(".refund-info").html("").hide();
                            $(".link-facebook").html("").hide();
                            getAllPrice();
                        }
                        else {
                            swal.close();
                        }
                    });
                }
                else {
                    var listUser = $("#<%=hdfListUser.ClientID%>").val();

                    var list = listUser.split('|');
                    var item = "<select id='listUser' class='form-control fjx'>";
                    for (var i = 0; i < list.length - 1; i++) {
                        item += "<option value='" + list[i] + "'>" + list[i] + "</option>";
                    }
                    item += "</select>";

                    var html = "";
                    html += "<div class='form-group'>";
                    html += "<label>Chọn nhân viên khác: </label>";
                    html += item;
                    html += "<a href='javascript:;' class='btn link-btn' style='background-color:#f87703;float:right;color:#fff;' onclick='setNewUser()'>Chọn</a>";
                    html += "</div>";
                    showPopup(html);
                }
            }

            function setNewUser() {
                var selectedUser = $("#listUser").val();
                $("#<%=hdfUsernameCurrent.ClientID%>").val(selectedUser);
                $("#<%=txtPhone.ClientID%>").val("").prop('readonly', false).prop('disabled', false);
                $("#<%=txtFullname.ClientID%>").val("").prop('readonly', false).prop('disabled', false).focus();
                $("#<%=txtAddress.ClientID%>").val("").prop('readonly', false).prop('disabled', false);
                $(".view-detail").html("").hide();
                $(".discount-info").html("").hide();
                $(".refund-info").html("").hide();
                $(".link-facebook").html("").hide();
                getAllPrice();
                closePopup();
                swal("Thông báo", "Em đã chọn tính tiền giúp nhân viên " + selectedUser + "", "info");
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

                                        if (confirm) {
                                            let currentUser = $("#<%=hdfUsernameCurrent.ClientID%>").val();
                                            let mainUser = $("#<%=hdfUsername.ClientID%>").val();
                                            if (currentUser != mainUser) {
                                                createReturnOrder(customerID, currentUser);
                                            }
                                            else {
                                                createReturnOrder(customerID, "");
                                            }
                                        }
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
                    $(".subtotal").removeClass("hide");
                    $(".returnorder").removeClass("hide");
                    $(".totalpriceorderall").removeClass("price-red");
                    $(".totalpricedetail").addClass("price-red");
                    $(".find3").removeClass("hide");
                    $(".find1").addClass("hide");
                    $(".find2").html("<i class='fa fa-share' aria-hidden='true'></i> " + refundGood.ID);
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
                $("#<%=pGuestPaid.ClientID%>").val(0);

                swal("Thông báo", "Đã bỏ qua đơn hàng đổi trả này!", "info");
                getAllPrice();
            }
            //end return Order

            // show shipping input by click button
            function showShipping() {
                $(".subtotal").removeClass("hide");
                $(".shipping").removeClass("hide");
                $("#<%=pFeeShip.ClientID%>").focus();
            }

            // show discount input by click button
            function showDiscount() {
                $(".subtotal").removeClass("hide");
                $(".discount").removeClass("hide");
                $("#<%=pDiscount.ClientID%>").focus();
            }
            // remove other fee by click button
            function removeOtherFee(uuid) {
                $("#" + uuid).remove();
                fees = fees.filter((item) => { return item.UUID != uuid; });
                $("#<%=hdfOtherFees.ClientID%>").val(JSON.stringify(fees));
                getAllPrice();
            }
            // edit other fee by click button
            function editOtherFee(uuid) {
                $("#" + uuid).find(".otherfee-value").click();
                //getAllPrice();
            }

            function showConfirmOrder() {
                var totalpriceorderall = $(".totalpricedetail").html();
                var html = "";
                html += "<div class=\"change-money\">";
                html += "<div class=\"form-group\">";
                html += "<label>Tổng tiền: </label>";
                html += "<input ID=\"totalMoney\" disabled class=\"form-control total-money\" value=\"" + totalpriceorderall + "\"></input>";
                html += "</div>";
                html += "<div class=\"form-group\">";
                html += "<label>Hãy kiểm tra lại số lượng và nhập vào đây: </label>";
                html += "<input ID=\"SoldQuantity\" class=\"form-control\"></input>";
                html += "</div>";
                html += "<a href=\"javascript:;\" class=\"submit-order btn link-btn\" style=\"background-color:#f87703;float:right;color:#fff;\" onclick=\"clickSubmit()\">Xác nhận</a>";
                html += "</div>";
                html += "</div>";
                showPopup(html, 5);
                $("#SoldQuantity").focus();
                $("#SoldQuantity").keydown(function (e) {
                    if (e.which == 13) {
                        clickSubmit();
                        event.preventDefault();
                        return false;
                    }
                });
            }

            function handleErrorSubmit() {
                HoldOn.close();
                if ($("#<%=hdStatusPage.ClientID%>").val() === "Submit") {
                    swal({
                        title: "Lỗi trong qua trình thánh toán",
                        text: "Xảy ra lỗi khi xác nhận đơn hàng.\nHãy nhấn lại nút thanh toán",
                        type: "error",
                        showCancelButton: false,
                        confirmButtonText: "OK Sếp!!",
                        closeOnConfirm: true,
                        html: false
                    }, function () {
                        $("#<%=btnOrder.ClientID%>").focus();
                    });
                }
            }
            function clickSubmit() {
                var totalquantity = $("#<%=hdfTotalQuantity.ClientID%>").val();
                var inputquantity = $("#SoldQuantity").val();

                if (totalquantity == inputquantity) {
                    closePopup();
                    HoldOn.open();
                    $("#<%=hdStatusPage.ClientID%>").val("Submit");
                    try {
                        $("#<%=btnOrder.ClientID%>").click();
                    }
                    finally {
                        setTimeout(handleErrorSubmit, 60000);
                    }
                }
                else {
                    $("#SoldQuantity").focus();
                    swal({
                        title: "Sai số lượng rồi...", text: "Đếm kỹ lại nha...",
                        type: "error",
                        showCancelButton: false,
                        confirmButtonText: "OK Sếp!!",
                        closeOnConfirm: true,
                        html: false
                    }, function () {
                        $("#SoldQuantity").focus();
                    });
                }
            }

            // print invoice after submit order
            function printInvoice(id) {
                swal({
                    title: "Thông báo", text: "Tạo đơn hàng thành công! Bấm OK để in hóa đơn...",
                    type: "success",
                    showCancelButton: false,
                    confirmButtonText: "OK in đê!!",
                    closeOnConfirm: true,
                    html: false
                }, function () {
                    clearCustomerDetail();
                    var url = "/print-invoice?id=" + id;
                    window.open(url);
                    window.location.replace(window.location.href);
                });
            }

            class OrderDetail {
                constructor(
                      ProductID
                    , ProductVariableID
                    , SKU
                    , ProductType
                    , ProductVariableName
                    , ProductVariableValue
                    , QuantityInstock
                    , ProductName
                    , ProductImageOrigin
                    , Giabanle
                    , ProductVariableSave) {
                    this.ProductID = ProductID;
                    this.ProductVariableID = ProductVariableID;
                    this.SKU = SKU;
                    this.ProductType = ProductType;
                    this.ProductVariableName = ProductVariableName;
                    this.ProductVariableValue = ProductVariableValue;
                    this.QuantityInstock = QuantityInstock;
                    this.ProductName = ProductName;
                    this.ProductImageOrigin = ProductImageOrigin;
                    this.Giabanle = Giabanle;
                    this.ProductVariableSave = ProductVariableSave;
                }

                stringJSON() {
                    return JSON.stringify(this);
                }
            }

            // insert order
            function insertOrder() {

                var checkAllValue = true;

                // check shipping fee

                var fs = $("#<%=pFeeShip.ClientID%>").val();
                var feeship = parseFloat(fs.replace(/\,/g, ''));

                if (feeship > 0 && feeship < 10000) {
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

                // check discount
                var ds = $("#<%=pDiscount.ClientID%>").val();
                var discount = parseFloat(ds.replace(/\,/g, ''));

                if (discount > 20000 && $("#<%=hdfRoleID.ClientID%>").val() != 0) {
                    checkAllValue = false;
                    $("#<%=pDiscount.ClientID%>").focus();
                    swal({
                        title: "Lạ vậy:",
                        text: "Sao chiết khấu lại lớn hơn <strong>20.000đ</strong> nè?<br><br>Nếu có lý do thì báo chị Ngọc nha!",
                        type: "warning",
                        showCancelButton: false,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Để em xem lại!!",
                        html: true
                    });
                }

                return checkAllValue;

            }

            // pay order on click button
            function payAll() {
                var phone = $("#<%=txtPhone.ClientID%>").val();
                var name = $("#<%=txtFullname.ClientID%>").val();
                var address = $("#<%=txtAddress.ClientID%>").val();
                var province = $("#<%=ddlProvince.ClientID%>").val();
                var district = $("#<%=ddlDistrict.ClientID%>").val();
                var ward = $("#<%=ddlWard.ClientID%>").val();

                if (name === "") {
                    $("#<%= txtFullname.ClientID%>").focus();
                    swal("Thông báo", "Hãy nhập tên khách hàng!", "error");
                }
                else if (phone === "") {
                    $("#<%= txtPhone.ClientID%>").focus();
                    swal("Thông báo", "Hãy nhập số điện thoại khách hàng!", "error");
                }
                else if (province === "0" || province === null || province === "") {
                    swal({
                        title: "Thông báo",
                        text: "Chưa chọn tỉnh thành",
                        type: "warning",
                        showCancelButton: false,
                        confirmButtonText: "Để em xem lại!!",
                        closeOnConfirm: false,
                        html: true
                    }, function (isConfirm) {
                        if (isConfirm) {
                            sweetAlert.close();
                            $("#<%=ddlProvince.ClientID%>").select2('open');
                        }
                    });
                }
                else if (district === "0" || district === null || district === "") {
                    swal({
                        title: "Thông báo",
                        text: "Chưa chọn quận huyện",
                        type: "warning",
                        showCancelButton: false,
                        confirmButtonText: "Để em xem lại!!",
                        closeOnConfirm: false,
                        html: true
                    }, function (isConfirm) {
                        if (isConfirm) {
                            sweetAlert.close();
                            $("#<%=ddlDistrict.ClientID%>").select2('open');
                        }
                    });
                }
                else if (ward === "0" || ward === null || ward === "") {
                    swal({
                        title: "Thông báo",
                        text: "Chưa chọn phường xã",
                        type: "warning",
                        showCancelButton: false,
                        confirmButtonText: "Để em xem lại!!",
                        closeOnConfirm: false,
                        html: true
                    }, function (isConfirm) {
                        if (isConfirm) {
                            sweetAlert.close();
                            $("#<%=ddlWard.ClientID%>").select2('open');
                        }
                    });
                }
                else if (address === "") {
                    $("#<%= txtAddress.ClientID%>").focus();
                    swal("Thông báo", "Hãy nhập địa chỉ khách hàng!", "error");
                }
                else if ($(".product-result").length == 0) {
                    $("#txtSearch").focus();
                    swal("Thông báo", "Hãy nhập sản phẩm!", "error");
                }
                else {
                    let orderDetails = "";
                    let ordertype = $(".customer-type").val();

                    orderDetails += '{ "productPOS" : ['

                    $(".product-result").each(function () {
                        let productID = $(this).attr("data-productid");
                        let productVariableID = $(this).attr("data-productvariableid");
                        let sku = $(this).attr("data-sku");
                        let producttype = $(this).attr("data-producttype");
                        let productvariablename = $(this).attr("data-productvariablename");
                        let productvariablevalue = $(this).attr("data-productvariablevalue");
                        let quantity = $(this).find(".in-quantity").val();
                        let productname = $(this).attr("data-productname");
                        let productimageorigin = $(this).attr("data-productimageorigin");
                        let productvariable = $(this).attr("data-productvariable");
                        let price = $(this).find(".gia-san-pham").attr("data-price");
                        let productvariablesave = $(this).attr("data-productvariablesave");

                        if (quantity > 0) {
                            let order = new OrderDetail(
                                    productID
                                    , productVariableID
                                    , sku
                                    , producttype
                                    , productvariablename
                                    , productvariablevalue
                                    , quantity
                                    , productname
                                    , productimageorigin
                                    , price
                                    , productvariablesave
                            );

                            orderDetails += order.stringJSON() + ",";
                        }
                    });

                    orderDetails = orderDetails.replace(/.$/, "") + "]}";

                    $("#<%=hdfOrderType.ClientID %>").val(ordertype);
                    $("#<%=hdfListProduct.ClientID%>").val(orderDetails);
                    $(".totalquantity").addClass("hide");

                    if (insertOrder() == true) {
                        showConfirmOrder();
                    }
                }
}

// count guest change
function countGuestChange() {
    notEmpty();
    var totalrefund = 0;
    if (parseFloat($("#<%=hdfDonHangTra.ClientID%>").val()) > 0) {
                    totalrefund = parseFloat($("#<%=hdfDonHangTra.ClientID%>").val());
                }
                var gp1 = $("#<%=pGuestPaid.ClientID%>").val();
                var gp = parseFloat(gp1.replace(/\,/g, ''));
                if (gp > 0) {
                    var totalprice = parseFloat($("#<%= hdfTotalPrice.ClientID%>").val());
                    var leftchange = gp - totalprice + totalrefund;
                    $(".totalGuestChange").html(formatThousands(leftchange, ","));
                } else {
                    var totalprice = parseFloat($("#<%= hdfTotalPrice.ClientID%>").val());
                    var leftchange = -totalprice + totalrefund;
                    $(".totalGuestChange").html(formatThousands(leftchange, ","));
                }
            }

            function searchProduct() {
                let textsearch = $("#txtSearch").val().trim().toUpperCase();

                $("#<%=hdfListSearch.ClientID%>").val(textsearch);

                $("#txtSearch").val("");

                //Get search product master
                searchProductMaster(textsearch, false);
            }

            function refreshDiscount() {
                $("#<%=hdfcheck.ClientID%>").val(0);
                getAllPrice();
            }

            // get all price
            function getAllPrice(refresh_discount) {
                if ($(".product-result").length > 0) {

                    var totalprice = 0;
                    var productquantity = 0;
                    $(".product-result").each(function () {
                        var price = parseFloat($(this).find(".gia-san-pham").attr("data-price"));
                        var quantity = parseFloat($(this).find(".in-quantity").val());

                        var total = price * quantity;
                        $(this).find(".totalprice-view").html(formatThousands(total, '.'));
                        productquantity += quantity;
                        totalprice += total;
                    });
                    $("#<%=hdfTotalPriceNotDiscount.ClientID%>").val(totalprice);
                    $(".totalproductQuantity").html(formatThousands(productquantity, ',') + " cái");
                    $("#<%=hdfTotalPriceNotDiscountNotFee.ClientID%>").val(totalprice);
                    $(".totalpriceorder").html(formatThousands(totalprice, ','));
                    $("#<%=hdfTotalQuantity.ClientID%>").val(productquantity);
                    var totalDiscount = 0;
                    var totalleft = 0;
                    var totalck = 0;
                    var amount = 0;
                    let isDiscount = +$("#<%=hdfIsDiscount.ClientID%>").val() || 0;
                    var amountdiscount = +$("#<%=hdfDiscountAmount.ClientID%>").val() || 0;
                    let quantityRequirement = +$("#<%=hdfQuantityRequirement.ClientID%>").val() || 0;;
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

                    // Nếu khách hàng năm trong nhóm chiết khấu và đạt số lượng yêu cầu không
                    if (isDiscount && productquantity >= quantityRequirement) {
                        amount = amount >= amountdiscount ? amount : amountdiscount;
                    }

                    if (amount > 0) {
                        totalDiscount = amount;
                        totalck = amount * productquantity;
                        totalleft = totalprice - totalck;
                        $(".subtotal").removeClass("hide");
                        $(".discount").removeClass("hide");
                    } else {
                        totalDiscount = 0;
                        totalleft = totalprice;
                    }

                    if ($("#<%=hdfcheck.ClientID%>").val() != 0) {
                        var dis = $("#<%=pDiscount.ClientID%>").val();
                        var discount = parseFloat(dis.replace(/\,/g, ''));
                        var totalck = discount * productquantity;
                        var totalleft = totalprice - totalck;
                        var totalDiscount = discount;
                    }
                    var fs = $("#<%=pFeeShip.ClientID%>").val();
                    var feeship = parseFloat(fs.replace(',', ''));
                    var feeship = parseFloat(fs.replace(/\,/g, ''));

                    let otherfee = 0;
                    fees.forEach((item) => {
                        otherfee += item.FeePrice;
                    });

                    var priceafterchietkhau = totalleft;
                    var totalmoney = totalleft + feeship + otherfee;
                    $("#<%=hdfTotalPrice.ClientID%>").val(totalmoney);

                    // Phiếu giảm giá
                    checkCouponCondition();
                    let priceCoupon = +$("#<%=hdfCouponValue.ClientID%>").val() || 0;

                    totalmoney -= priceCoupon;

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
                    countGuestChange();
                } else {
                    $(".totalproductQuantity").html(formatThousands(0, ',') + " cái");
                    $(".totalpriceorder").html(formatThousands(0, ','));
                    $(".totalGuestChange").html(formatThousands(0, ','));
                    $(".totalpriceorderall").html(formatThousands(0, ','));
                    $(".priceafterchietkhau").html(formatThousands(0, ','));

                    $('[id$="_hdfTotalQuantity"]').val(formatThousands(0, ','));
                    $('[id$="_hdfTotalPrice"]').val(formatThousands(0, ','));
                    checkCouponCondition();
                }
            }

            // check empty
            function notEmpty() {
                if ($("#<%=pDiscount.ClientID%>").val() == '') {
                    var dis = 0;
                    $("#<%=pDiscount.ClientID%>").val(formatThousands(dis, ','));
                }
                if ($("#<%=pFeeShip.ClientID%>").val() == '') {
                    var fee = 0;
                    $("#<%=pFeeShip.ClientID%>").val(formatThousands(fee, ','));
                }
                if ($("#<%=pGuestPaid.ClientID%>").val() == '') {
                    var pain = 0;
                    $("#<%=pGuestPaid.ClientID%>").val(formatThousands(pain, ','));
                }
                fees.forEach((item) => {
                    if (item.price == "") {
                        item.FeePrice = 0;
                        $("#" + item.UUID).val(0);
                    }
                })

            }

            // count total order
            function countTotal() {
                var total = parseFloat($("#<%=hdfTotalPriceNotDiscount.ClientID%>").val());
                var quantity = parseFloat($("#<%=hdfTotalQuantity.ClientID%>").val());
                notEmpty();
                var dis = $("#<%=pDiscount.ClientID%>").val();
                var fs = $("#<%=pFeeShip.ClientID%>").val();


                var discount = parseFloat(dis.replace(/\,/g, ''));
                var feeship = parseFloat(fs.replace(/\,/g, ''));

                let otherfee = 0;
                fees.forEach((item) => {
                    otherfee += item.FeePrice;
                });

                $("#<%=hdfcheck.ClientID%>").val(discount);

                // Phiếu giảm giá
                let priceCoupon = +$("#<%=hdfCouponValue.ClientID%>").val() || 0;

                var totalleft = total + feeship + otherfee - discount * quantity - priceCoupon;
                var priceafterchietkhau = total - discount * quantity;

                $(".totalpriceorderall").html(formatThousands(totalleft, ','));
                $(".priceafterchietkhau").html(formatThousands(priceafterchietkhau, ','));

                var refund = 0;
                if (parseFloat($("#<%=hdfDonHangTra.ClientID%>").val()) > 0) {
                    refund = parseFloat($("#<%=hdfDonHangTra.ClientID%>").val());
                }

                $(".totalpricedetail").html(formatThousands((totalleft - refund), ","));
                $("#<%=hdfTongTienConLai.ClientID%>").val(totalleft - refund);
                $("#<%=hdfTotalPrice.ClientID%>").val(totalleft);
                countGuestChange();
            }

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
                if (window.event) {
                    keypressed = window.event.keyCode; //IE
                } else {
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

            function createReturnOrder(customerID, username) {
                let userString = "";
                if (username != "") {
                    userString = "&username=" + username;
                }
                var win = window.open('/tao-don-hang-doi-tra?customerID=' + customerID + userString, '_blank');
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
                    return swal("Thông báo", "Chưa nhập thông tin khách hàng!", "warning");

                if (!productNumber) {
                    $("#txtSearch").focus();
                    return swal("Thông báo", "Chưa nhập sản phẩm!", "warning");
                }


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
                    url: "/pos.aspx/getCoupon",
                    data: JSON.stringify({ "customerID": customerID, "code": code, "productNumber": productNumber, "price": price }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (respon) => {
                        let data = JSON.parse(respon.d);

                        if (data) {
                            if (!data.status) {
                                errorDOM.classList.remove('hide');
                                errorDOM.querySelector('p').innerText = data.message;

                                codeDOM.focus();
                                codeDOM.select();
                            }
                            else {
                                $(".subtotal").removeClass("hide");
                                document.querySelector('[id$="_txtCouponValue"]').value = `${code.trim().toUpperCase()}: -${formatThousands(+data.value || 0, ',')}`;
                                document.querySelector('[id$="_hdfCouponID"]').value = +data.couponID || 0;
                                document.querySelector('[id$="_hdfCouponValue"]').value = +data.value || 0;
                                document.querySelector('[id$="_hdfCouponProductNumber"]').value = +data.productNumber || 0;
                                document.querySelector('[id$="_hdfCouponPriceMin"]').value = +data.priceMin || 0;

                                couponModalDOM.querySelector('#closeCoupon').click();
                                document.querySelector('#btnOpenCouponModal').classList.add('hide');
                                document.querySelector('#btnRemoveCouponCode').classList.remove('hide');
                                document.querySelector('#btnGenerateCouponG25').classList.add('hide');

                                getAllPrice();
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

                getAllPrice();
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
                    return swal("Thông báo", "Chưa nhập thông tin khách hàng!", "warning");

                generateCouponG25(customerName, customerID);
            }
        </script>
    </telerik:RadScriptBlock>
</asp:Content>
