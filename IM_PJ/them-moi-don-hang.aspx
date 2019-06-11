<%@ Page Title="Thêm mới đơn hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="them-moi-don-hang.aspx.cs" Inherits="IM_PJ.them_moi_don_hang" EnableSessionState="ReadOnly" enableEventValidation="false"%>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/App_Themes/Ann/js/search-customer.js?v=23052019"></script>
    <script src="/App_Themes/Ann/js/search-product.js?v=15052021"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Họ tên</label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFullname" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtFullname" CssClass="form-control capitalize" runat="server" placeholder="Họ tên thật của khách (F2)" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Điện thoại</label>
                                            <asp:RequiredFieldValidator ID="re" runat="server" ControlToValidate="txtPhone" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtPhone" CssClass="form-control" autocomplete="off" onblur="ajaxCheckCustomer()" runat="server" placeholder="Số điện thoại khách hàng"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Nick đặt hàng</label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNick" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtNick" CssClass="form-control capitalize" autocomplete="off" runat="server" placeholder="Tên nick đặt hàng"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Địa chỉ</label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAddress" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtAddress" CssClass="form-control capitalize" autocomplete="off" runat="server" placeholder="Địa chỉ khách hàng"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Zalo</label>
                                            <asp:TextBox ID="txtZalo" CssClass="form-control" autocomplete="off" runat="server" placeholder="Số điện thoại Zalo"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Facebook</label>
                                            <div class="row">
                                                <div class="col-md-10 fb width-100">
                                                    <asp:TextBox ID="txtFacebook" CssClass="form-control" autocomplete="off" runat="server" placeholder="Đường link chat Facebook"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
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
                                <div class="right totalDiscount">
                                    <telerik:RadNumericTextBox runat="server" CssClass="form-control width-notfull" Skin="MetroTouch"
                                        ID="pDiscount" MinValue="0" NumberFormat-GroupSizes="3" Value="0" NumberFormat-DecimalDigits="0"
                                        oninput="countTotal()" IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
                                    </telerik:RadNumericTextBox>
                                </div>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Sau chiết khấu</div>
                                <div class="right priceafterchietkhau"></div>
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
                                <div class="right totalpriceorderall price-red"></div>
                            </div>
                            <div class="post-row clear returnorder hide">
                                <div class="left">
                                    Trừ hàng trả
                                    <a href="javascript:;" class="find2 hide btn btn-feeship link-btn"></a>
                                    <a href="javascript:;" class="find3 hide btn btn-feeship link-btn btn-edit-fee" onclick="searchReturnOrder()"><i class="fa fa-refresh" aria-hidden="true"></i> Chọn đơn khác</a>
                                    <a href="javascript:;" class="find3 hide btn btn-feeship link-btn" onclick="deleteReturnOrder()"><i class="fa fa-times" aria-hidden="true"></i> Bỏ qua</a>
                                </div>
                                <div class="right totalpriceorderrefund"></div>
                            </div>
                            <div class="post-row clear refund hide">
                                <div class="left">Tổng tiền còn lại</div>
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
            <asp:HiddenField ID="hdfIsDiscount" runat="server" />
            <asp:HiddenField ID="hdfDiscountAmount" runat="server" />
            <asp:HiddenField ID="hdfIsMain" runat="server" />
            <asp:HiddenField ID="hdfTotalPriceNotDiscountNotFee" runat="server" />
            <asp:HiddenField ID="hdfListSearch" runat="server" />
            <asp:HiddenField ID="hdfTotalQuantity" runat="server" />
            <asp:HiddenField ID="hdfcheck" runat="server" />
            <asp:HiddenField ID="hdfChietKhau" runat="server" />
            <asp:HiddenField ID="hdfDonHangTra" runat="server" />
            <asp:HiddenField ID="hdfTongTienConLai" runat="server" />
            <asp:HiddenField ID="hdSession" runat="server" />
            <asp:HiddenField ID="hdfFeeType" runat="server" />
            <asp:HiddenField ID="hdfOtherFees" runat="server" />
            <asp:HiddenField ID="hdfCustomerID" runat="server" />
            <asp:HiddenField ID="hdfTransportCompanySubID" runat="server" />

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
                                <div class="col-md-8" >
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
        .select2-container.select2-container--default.select2-container--open{
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
        <script type="text/javascript">
            "use strict";
            var feetype = [];
            var fees = [];

            class FeeType {
                constructor(ID, Name, IsNegativeFee)
                {
                    this.ID = ID;
                    this.Name = Name;
                    this.IsNegativeFee = IsNegativeFee;
                }
            }

            class Fee {
                constructor(UUID, FeeTypeID, FeeTypeName, FeePrice)
                {
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

                if (fee) {
                    let negative = fee.FeePrice > 0 ? "" : "-";

                    addHTML += "<div id='" + fee.UUID + "' class='post-row clear otherfee' data-feeid='" + fee.FeeTypeID + "' data-price='" + fee.FeePrice + "'>";
                    addHTML += "    <div class='left'>";
                    addHTML += "        <span class='otherfee-name'>" + fee.FeeTypeName + "</span>";
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
                if (isNegative)
                {
                    parent.find("#feePrice").val("-" + formatNumber(feeprice));
                    feeprice = parseInt(feeprice) * (-1);
                }
                else
                {
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
                init()

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
                // event key up txtZalo
                $("#<%=txtZalo.ClientID%>").keyup(function (e) {
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

                $("#orderInfoModal").on("show.bs.modal", (e) => {
                    let customerID = $("#<%=hdfCustomerID.ClientID%>").val();
                    let payTypeDOM = $("#<%=ddlPaymentType.ClientID%>");
                    let shipTypeDOM = $("#<%=ddlShippingType.ClientID%>");

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
            });

            // order of item list
            var orderItem = 0;

            function redirectTo(ID) {
                HoldOn.open();
                $("#payall").addClass("payall-clicked");
                window.location.href = "/thong-tin-don-hang?id=" + ID;
            }

            // check data before close page or refresh page
            function stopNavigate(event) {
                $(window).off('beforeunload');
            }

            $(window).bind('beforeunload', function(e) {
                if ($("#payall").hasClass("payall-clicked")) {
                    e = null;
                } else {
                    if ($(".product-result").length > 0 || $("#<%=txtPhone.ClientID%>").val() != "" || $("#<%= txtFullname.ClientID%>").val() != "")
                        return true;
                    else
                        e = null;
                }
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

            // cal fee ship
            function calFeeShip() {
                if ($("#<%=pFeeShip.ClientID%>").is(":disabled"))
                {
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
                getAllPrice();
            }
            // edit other fee by click button
            function editOtherFee() {
                $(".otherfee-value").click();
                //getAllPrice();
            }

            // search return order
            function createOrderReturnHTML(refundGood) {
                let createdDate = "";
                let addHTML = "";

                // Format CreateDate
                var matchs = refundGood.CreatedDate.match(/\d+/g);
                if (matchs)
                {
                    let date = new Date(parseInt(matchs[0]));
                    if (date)
                    {
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

                if (isBlank(customerID))
                {
                    swal("Thông báo", "Đây là khách hàng mới mà ^_^", "info");
                }
                else
                {
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
                    if ($(".product-result").length > 0)
                    {
                        getAllPrice(true);
                        var list = "";
                        var count = 0;
                        var ordertype = $(".customer-type").val();
                        $(".product-result").each(function() {
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
                                list += id + "," + sku + "," + producttype + "," + productvariablename + "," + productvariablevalue + "," + quantity + "," +
                                    productname + "," + productimageorigin + "," + productvariablesave + "," + price + "," + productvariablesave + "," + productvariableid + ";";
                                count++;
                            }
                        });
                        if (count > 0)
                        {
                            $("#<%=hdfOrderType.ClientID %>").val(ordertype);
                            $("#<%=hdfListProduct.ClientID%>").val(list);

                            $("#orderInfoModal").modal({ show: 'true', backdrop: 'static' });
                        }
                        else
                        {
                            $("#txtSearch").focus();
                            swal("Thông báo", "Hãy nhập sản phẩm!", "error");
                        }
                    }
                    else
                    {
                        $("#txtSearch").focus();
                        swal("Thông báo", "Hãy nhập sản phẩm!", "error");
                    }
                }
            }

            // insert order
            function insertOrder() {

                let excuteStatus = $("#<%=ddlExcuteStatus.ClientID%>").val();
                let payType = $("#<%=ddlPaymentType.ClientID%>").val();
                let bank = $("#<%=ddlBank.ClientID%>").val();
                let shippingtype = $("#<%=ddlShippingType.ClientID%>").val();
                let trans = $("#<%=ddlTransportCompanyID.ClientID%>").val();
                let transSub = $("#<%=ddlTransportCompanySubID.ClientID%>").val();

                var checkAllValue = true;

                // check shipping fee

                var fs = $("#<%=pFeeShip.ClientID%>").val();
                var feeship = parseFloat(fs.replace(/\,/g, ''));
                if (excuteStatus == 2) {
                    if (shippingtype == 2 || shippingtype == 3) {
                        if (feeship == 0 && $("#<%=pFeeShip.ClientID%>").is(":disabled") == false) {
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
                    else if (shippingtype == 4) {
                        if (feeship == 0 && $("#<%=pFeeShip.ClientID%>").is(":disabled") == false) {
                            if (trans != 0 && transSub != 0) {
                                var checkPrepay = checkPrepayTransport(trans, transSub);
                                if (checkPrepay == 1) {
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
                }
                

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

                if (discount > 11000 && $("#<%=hdfRoleID.ClientID%>").val() != 0) {
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

                if (checkAllValue == true) {

                    $("#payall").addClass("payall-clicked");
                    $("#<%=hdfTransportCompanySubID.ClientID%>").val(transSub);

                    $("#closeOrderInfo").click();

                    HoldOn.open();
                    $("#<%=btnOrder.ClientID%>").click();
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
                    text: "Em muốn xóa hết sản phẩm trong đơn thiệt hả?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Chính xác sếp ơi!!",
                    closeOnConfirm: false,
                    cancelButtonText: "Em bấm lộn zồi..",
                    html: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        $(".product-result").remove();
                        getAllPrice();
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
                getAllPrice();
            }

            // get all price
            function getAllPrice(is_payAll_call) {
                if(is_payAll_call === undefined)
                    is_payAll_call = false;
                if ($(".product-result").length > 0)
                {
                    let totalprice = 0;
                    let productquantity = 0;
                    $(".product-result").each(function() {
                        let price = parseFloat($(this).find(".gia-san-pham").attr("data-price"));
                        let quantity = parseFloat($(this).find(".in-quantity").val());
                        let total = price * quantity;

                        $(this).find(".totalprice-view").html(formatThousands(total, ','));
                        productquantity += quantity;
                        totalprice += total;
                    });

                    $("#<%=hdfTotalPriceNotDiscount.ClientID%>").val(totalprice);
                    $(".totalproductQuantity").html(formatThousands(productquantity, ',') + " sản phẩm");
                    $("#<%=hdfTotalPriceNotDiscountNotFee.ClientID%>").val(totalprice);
                    $(".totalpriceorder").html(formatThousands(totalprice, ','));
                    $("#<%=hdfTotalQuantity.ClientID%>").val(productquantity);
                    var isDiscount = $("#<%=hdfIsDiscount.ClientID%>").val();
                    var totalDiscount = 0;
                    var totalleft = 0;
                    var amount = 0;
                    var amountdiscount = 0;
                    if (isDiscount == 1)
                        amountdiscount = parseFloat($("#<%=hdfDiscountAmount.ClientID%>").val());

                    var ChietKhau = document.getElementById('<%= hdfChietKhau.ClientID%>').defaultValue;
                    var listck = ChietKhau.split('|');
                    for (var i = 0; i < listck.length - 1; i++) {
                        var item = listck[i].split('-');
                        if (i < listck.length - 2)
                        {
                            var item2 = listck[i + 1].split('-');
                            if (productquantity > (parseFloat(item[0]) - 1) && productquantity <= (parseFloat(item2[0]) - 1))
                                amount = parseFloat(item[1]);
                        }
                        else
                        {
                            if (productquantity > (parseFloat(item[0]) - 1))
                                amount = parseFloat(item[1]);
                        }
                    }

                    // Nếu dùng bằng tay để chỉnh chiết khấu
                    if (is_payAll_call === true)
                        amount = parseInt($("#<%=pDiscount.ClientID%>").val().replace(/\,/g,''));
                    else
                    {
                        // Nếu chiết khấu của khách hàng lớn hơn 0
                        if (amountdiscount > 0) 
                            // Nếu <chiết khấu nhóm> của khách hàng lớn hơn mức được <chiết khấu của đơn hàng> thì lấy <chiết khấu nhóm> để tính
                            if (amount < amountdiscount)
                                amount = amountdiscount;
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

                    if ($("#<%=hdfcheck.ClientID%>").val() != 0) {
                        var dis = $("#<%=pDiscount.ClientID%>").val();
                        var discount = parseFloat(dis.replace(/\,/g, ''));
                        var totalck = discount * productquantity;
                        var totalleft = totalprice - totalck;
                        var totalDiscount = discount;
                    }
                    var fs = $("#<%=pFeeShip.ClientID%>").val();
                    var feeship = parseFloat(fs.replace(/\,/g, ''));

                    let otherfee = 0;
                    fees.forEach((item) => {
                        otherfee += item.FeePrice != "" ? item.FeePrice : 0;
                    });

                    var priceafterchietkhau = totalleft;

                    var totalmoney = totalleft + feeship + otherfee;
                    $("#<%=pDiscount.ClientID%>").val(formatThousands(totalDiscount, ','));
                    $(".totalpriceorderall").html(formatThousands(totalmoney, ','));
                    $(".priceafterchietkhau").html(formatThousands(priceafterchietkhau, ','));
                    $("#<%=hdfTotalPrice.ClientID%>").val(totalmoney);
                    var refund = 0;
                    if (parseFloat($("#<%=hdfDonHangTra.ClientID%>").val()) > 0)
                        refund = parseFloat($("#<%=hdfDonHangTra.ClientID%>").val());

                    $(".totalpricedetail").html(formatThousands((totalmoney - refund), ","));
                    $("#<%=hdfTongTienConLai.ClientID%>").val(totalmoney - refund);
                }
                else
                {
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
                var totalleft = total + feeship + otherfee - discount * quantity;

                var priceafterchietkhau = total - discount * quantity;

                $(".priceafterchietkhau").html(formatThousands(priceafterchietkhau, ','));
                $(".totalpriceorderall").html(formatThousands(totalleft, ','));

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
                        if (customertype == 1)
                        {
                            if (giale == 0)
                                giale = giasi;
                            $(this).find(".gia-san-pham").attr("data-price", giale).html(formatThousands(giale, ','));
                        }
                        else
                        {
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
                    {
                        return;
                    }
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

            function onchangePaymentType(payType)
            {
                $("#<%=ddlBank.ClientID%>").val(0);

                // Khác hình thức chuyển khoản
                if (payType.val() != 2)
                {
                    $("#bankModal").attr("hidden", true);
                }
                else
                {
                    $("#bankModal").removeAttr("hidden");
                }
            }

            function onchangeShippingType(shipType) {
                let tranContainerDOM = $("[id$=ddlTransportCompanyID-container]");
                let tranSubContainerDOM = $("[id$=ddlTransportCompanySubID-container]");

                tranContainerDOM.val(0);
                tranSubContainerDOM.attr("title", "Nhà chành xe");
                tranSubContainerDOM.html("Nhà chành xe");
                tranSubContainerDOM.val(0);
                tranSubContainerDOM.attr("title", "Chọn nơi nhận");
                tranSubContainerDOM.html("Chọn nơi nhận");

                // Khác hình thức chuyển xe
                if (shipType.val() != 4)
                {
                    $("#transportModal").attr("hidden", true);
                    $("#transportSubModal").attr("hidden", true);
                }
                else {
                    $("#transportModal").removeAttr("hidden");
                    $("#transportSubModal").removeAttr("hidden");
                }

            }

            function onChangeTransportCompany(transport, selected)
            {
                if (selected == undefined)
                {
                    selected = "0";
                }

                let transComID = transport.val();
                $.ajax({
                    url: "/them-moi-don-hang.aspx/getTransportSub",
                    type: "POST",
                    data: JSON.stringify({'transComID': transComID}),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        let data = JSON.parse(msg.d);
                        if (data)
                        {
                            let tranSubName = "Chọn nơi nhận";
                            let tranSubDOM = $("#<%=ddlTransportCompanySubID.ClientID%>");
                            tranSubDOM.html("")
                            data.forEach((item) => {
                                if (selected == item.key)
                                {
                                    tranSubName = item.value;
                                }
                                tranSubDOM.append("<option value='" + item.key + "'>" + item.value + "</option>")
                            });

                            tranSubDOM.val(selected);
                            let tranSubContainerDOM = $("[id$=ddlTransportCompanySubID-container]");
                            tranSubContainerDOM.attr("title", tranSubName);
                            tranSubContainerDOM.html(tranSubName);

                            if (selected == "0")
                            {
                                setTimeout(function() {
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

            function suggestBank(customerID)
            {
                $.ajax({
                    url: "/them-moi-don-hang.aspx/getTransferLast",
                    type: "POST",
                    data: JSON.stringify({ 'customerID': customerID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        let data = JSON.parse(response.d);
                        let banksDOM = $("#<%=ddlBank.ClientID%>");
                        if (data)
                        {
                            banksDOM.val(data.value);
                        }
                        else
                        {
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

                        if (data) {
                            // Phương thức thanh toán
                            payType.val(data.payType);
                            onchangePaymentType(payType)
                            // Ngân hàng
                            if (data.payType == "2")
                                banksDOM.val(data.bankID);
                            else
                                banksDOM.val(0);
                            // Phương thức giao hàng
                            shipType.val(data.shipType);
                            onchangeShippingType(shipType)
                            // Chành xe & nơi tới
                            if (data.shipType == "4") {
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
            
        </script>
    </telerik:RadScriptBlock>

</asp:Content>
