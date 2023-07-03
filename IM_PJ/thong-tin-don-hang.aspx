<%@ Page Title="Thông tin đơn hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="thong-tin-don-hang.aspx.cs" Inherits="IM_PJ.thong_tin_don_hang" EnableSessionState="ReadOnly" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/Ann/js/search-customer.js?v=202110012121"></script>
    <script type="text/javascript" src="/App_Themes/Ann/js/search-product.js?v=03122022"></script>
    <script type="text/javascript" src="/App_Themes/Ann/js/copy-invoice-url.js?v=03122022"></script>
    <script type="text/javascript" src="/App_Themes/Ann/js/pages/danh-sach-khach-hang/generate-coupon-for-customer.js?v=03122022"></script>
    <style>
        .panel-post {
            margin-bottom: 20px;
        }
        #old-order-note ul {
            margin-left: 15px;
        }
        #old-order-note a {
            color: #ff8400;
        }
        .search-product-content {
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
        table.shop_table_responsive > tbody > tr:nth-of-type(2n+1) td {
            border-bottom: solid 1px #e1e1e1!important;
        }

        .coupon .right {
            display: flex;
        }

        .red {
            background-color: #F44336!important;
        }

        *.select2-container.select2-container--default.select2-container--open {
            z-index: 99991;
        }

        @media (max-width: 769px) {
            label {
                margin-bottom: 0;
            }

            .btn {
                width: 100% !important;
                float: left;
                margin-bottom: 10px;
                margin-left: 0;
            }
            .search-box {
                width: 70%;
            }
            .table-sale-order .order-item,
            .table-sale-order .image-item,
            .table-sale-order .image-item,
            .table-sale-order .name-item,
            .table-sale-order .sku-item,
            .table-sale-order .variable-item,
            .table-sale-order .price-item,
            .table-sale-order .quantity-item,
            .table-sale-order .total-item,
            .table-sale-order .trash-item {
                width: 100%;
                text-align: right!important;
            }
            table.shop_table_responsive thead {
                display: none;
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(1):before {
                content: "#";
                font-size: 20px;
                margin-right: 2px;
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(1) {
                text-align: left!important;
                font-size: 20px;
                font-weight: bold;
                height: 50px;
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(2) {
                height: auto;
                padding-top: 0;
                padding-bottom: 0;
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(2) img {
                width: 30%;
            }

            table.shop_table_responsive > tbody > tr:nth-of-type(2n) td {
                border-top: none;
                border-bottom: none!important;
                background: #fff;
            }
            table.shop_table_responsive > tbody > tr > td:first-child {
                border-left: none;
                padding-left: 20px;
            }
            table.shop_table_responsive > tbody > tr > td:last-child {
                border-right: none;
                padding-left: 20px;
                height: 60px;
                text-align: right!important;
            }
            table.shop_table_responsive > tbody > tr:nth-of-type(2n+1) td {
                border-bottom: none!important;
            }
            table.shop_table_responsive > tbody > tr > td {
                height: 40px;
            }
            table.shop_table_responsive > tbody > tr > td.sku-item:before {
                content: "Mã";
            }
            table.shop_table_responsive > tbody > tr > td.variable-item {
                height: 60px;
            }
            table.shop_table_responsive > tbody > tr > td.variable-item:before {
                content: "Thuộc tính";
            }
            table.shop_table_responsive > tbody > tr > td.price-item:before {
                content: "Giá bán";
            }
            table.shop_table_responsive > tbody > tr > td.quantity-item:before {
                content: "Số lượng";
            }
            table.shop_table_responsive > tbody > tr > td.soluong:before {
                content: "Kho";
            }
            table.shop_table_responsive > tbody > tr > td.total-item:before {
                content: "Thành tiền";
            }
            table.shop_table_responsive > tbody > tr > td.quantity-item {
                height: 60px;
            }
            table.shop_table_responsive > tbody > tr > td.quantity-item input {
                width: 50%;
                float: right;
            }
            table.shop_table_responsive > tbody > tr > td.soluong {
                height: 40px;
            }
            table.shop_table_responsive .bg-bronze,
            table.shop_table_responsive .bg-red,
            table.shop_table_responsive .bg-blue,
            table.shop_table_responsive .bg-yellow,
            table.shop_table_responsive .bg-black,
            table.shop_table_responsive .bg-green {
                display: initial;
                float: right;
            }
            table.shop_table_responsive tbody td {
                background-color: #f8f8f8;
                display: block;
                text-align: right;
                border: none;
                padding: 20px;
            }
            table.shop_table_responsive > tbody > tr.tr-more-info > td {
                height: initial;
            }
            table.shop_table_responsive > tbody > tr.tr-more-info > td span {
                display: block;
                text-align: left;
                margin-bottom: 10px;
                margin-right: 0;
            }
            table.shop_table_responsive > tbody > tr.tr-more-info > td:nth-child(2):before {
                content: none;
            }
            table.shop_table_responsive tbody td:before {
                font-weight: 700;
                float: left;
                text-transform: uppercase;
                font-size: 14px;
            }
            table.shop_table_responsive tbody td:empty {
                display: none;
            }
        }
    </style>
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
                                <a href="javascript:;" class="search-customer" onclick="searchCustomer()"><i class="fa fa-search" aria-hidden="true"></i> Tìm khách hàng (F1)</a>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Họ tên</label>
                                            <asp:TextBox ID="txtFullname" CssClass="form-control capitalize" runat="server" Enabled="true" placeholder="Họ tên thật của khách (F2)" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Điện thoại</label>
                                            <asp:TextBox ID="txtPhone" CssClass="form-control" onblur="ajaxCheckCustomer()" runat="server" Enabled="false" placeholder="Số điện thoại" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Nick đặt hàng</label>
                                            <asp:TextBox ID="txtNick" CssClass="form-control capitalize" runat="server" Enabled="true" placeholder="Nick đặt hàng" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Facebook</label>
                                            <div class="row">
                                                <div class="col-md-9 fb">
                                                    <asp:TextBox ID="txtFacebook" CssClass="form-control" runat="server" Enabled="true" placeholder="Đường link Facebook" autocomplete="off"></asp:TextBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="row">
                                                        <span class="link-facebook">
                                                            <asp:Literal ID="ltrFb" runat="server"></asp:Literal>
                                                        </span>
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
                                        <br /><asp:Literal ID="ltrDiscountInfo" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="deliveryAddress" class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Địa chỉ nhận hàng</h3>
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
                                <div id="notificationFee" class="row hide">
                                    <div class="col-md-12">
                                        <strong class="font-red">Phí vận chuyển đã thay đổi. Hãy tính lại phí!</strong>
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
                            </div>
                            <div class="post-body search-product-content clear">
                                <div class="search-product-content">
                                    <table class="table table-checkable table-product table-sale-order shop_table_responsive">
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
                                            <asp:Literal ID="ltrProducts" runat="server"></asp:Literal>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="post-footer">
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
                                    <div class="right priceafterchietkhau">
                                        <asp:Literal ID="ltrTotalAfterCK" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">Phí vận chuyển</div>
                                    <div class="right shipping-fee">
                                        <a class="btn btn-feeship link-btn btn-green hide" href="javascript:;" id="btnJtExpresFee" onclick="getJtExpresFee()">
                                            <i class="fa fa-check-square-o" aria-hidden="true"></i> Lấy phí J&T
                                        </a>
                                        <a class="btn btn-feeship link-btn btn-green hide" href="javascript:;" id="getShipGHTK" onclick="getShipGHTK()"><i class="fa fa-check-square-o" aria-hidden="true"></i> Lấy phí GHTK</a>
                                        <a class="btn btn-feeship link-btn" href="javascript:;" id="calfeeship" onclick="calFeeShip()"><i class="fa fa-check-square-o" aria-hidden="true"></i> Miễn phí</a>
                                        <telerik:RadNumericTextBox runat="server" CssClass="form-control width-notfull input-coupon input-feeship" Skin="MetroTouch"
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
                                    <div class="left"><strong>TỔNG TIỀN</strong> (đơn hàng <strong><asp:Literal ID="ltrOrderID" runat="server"></asp:Literal></strong>)</div>
                                    <div class="right totalpriceorderall price-red">
                                        <asp:Literal ID="ltrTotalprice" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="post-row clear returnorder hide">
                                    <div class="left">
                                        Trừ hàng trả
                                    </div>
                                    <div class="right">
                                        <a href="javascript:;" class="find2 hide btn btn-return-order link-btn"></a>
                                        <a href="javascript:;" class="find3 hide btn btn-return-order link-btn btn-edit-fee" onclick="searchReturnOrder()"><i class="fa fa-refresh" aria-hidden="true"></i> Chọn đơn khác</a>
                                        <a href="javascript:;" class="find3 hide btn btn-feeship link-btn" onclick="deleteReturnOrder()"><i class="fa fa-times" aria-hidden="true"></i> Xóa</a>
                                        <span class="totalpriceorderrefund"><asp:Literal runat="server" ID="ltrTotalPriceRefund"></asp:Literal></span>
                                    </div>
                                </div>
                                <div class="post-row clear refund hide">
                                    <div class="left"><strong>TỔNG TIỀN CÒN LẠI</strong></div>
                                    <div class="right totalpricedetail">
                                        <asp:Literal runat="server" ID="ltrtotalpricedetail"></asp:Literal>
                                    </div>
                                </div>
                                <div class="post-row clear weight-input hide">
                                    <div class="left">Khối lượng đơn hàng (kg)</div>
                                    <div class="right">
                                        <telerik:RadNumericTextBox runat="server" CssClass="form-control width-notfull input-weight" Skin="MetroTouch"
                                            ID="txtWeight" MinValue="0" Value="0" NumberFormat-DecimalDigits="1" IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false" onchange="onChangeWeight()">
                                        </telerik:RadNumericTextBox>
                                    </div>
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
                                        <asp:DropDownList ID="ddlExcuteStatus" runat="server" CssClass="form-control" onchange="onChangeExcuteStatus()"></asp:DropDownList>
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
                                <div id="row-bank" class="form-row">
                                    <div class="row-left">
                                        Ngân hàng nhận tiền
                                    </div>
                                    <div class="row-right">
                                        <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="row-shipping-type" class="form-row">
                                    <div class="row-left">
                                        Phương thức giao hàng
                                    </div>
                                    <div class="row-right">
                                        <asp:DropDownList ID="ddlShippingType" runat="server" CssClass="form-control shipping-type"></asp:DropDownList>
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
                                <div id="row-createdby" class="form-row hide">
                                    <div class="row-left">
                                        Nhân viên phụ trách
                                    </div>
                                    <div class="row-right">
                                        <asp:DropDownList ID="ddlCreatedBy" runat="server" CssClass="form-control createdby"></asp:DropDownList>
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
                <asp:Literal ID="ltrOldOrderNote" runat="server" EnableViewState="false"></asp:Literal>
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
            <asp:HiddenField ID="hdfUsername" runat="server" />
            <asp:HiddenField ID="hdfUsernameCurrent" runat="server" />
            <asp:HiddenField ID="hdfRoleID" runat="server" />
            <asp:HiddenField ID="hdfOrderType" runat="server" />
            <asp:HiddenField ID="hdfTotalPrice" runat="server" />
            <asp:HiddenField ID="hdfTotalPriceNotDiscount" runat="server" />
            <asp:HiddenField ID="hdfListProduct" runat="server" />
            <asp:HiddenField ID="hdfPaymentStatus" runat="server" />
            <asp:HiddenField ID="hdfExcuteStatus" runat="server" />
            <%-- Khách hàng có nằm trong nhóm chiết khấu không --%>
            <asp:HiddenField ID="hdfIsDiscount" runat="server" />
            <%-- Chiết khấu của nhóm --%>
            <asp:HiddenField ID="hdfDiscountAmount" runat="server" />
            <%-- Khối lượng yêu cầu để hưởng chiết khẩu của nhóm --%>
            <asp:HiddenField ID="hdfQuantityRequirement" runat="server" />
            <asp:HiddenField ID="hdfIsMain" runat="server" />
            <asp:HiddenField ID="hdfListSearch" runat="server" />
            <asp:HiddenField ID="hdfTotalQuantity" runat="server" />
            <asp:HiddenField ID="hdftotal" runat="server" />
            <asp:HiddenField ID="hdfCurrentValue" runat="server" />
            <asp:HiddenField ID="hdfDelete" runat="server" />
            <asp:HiddenField ID="hdfDele" runat="server" />
            <asp:HiddenField ID="hdfDonHangTra" runat="server" />
            <asp:HiddenField ID="hdfChietKhau" runat="server" />
            <asp:HiddenField ID="hdfTongTienConLai" runat="server" />
            <asp:HiddenField ID="hdfSoLuong" runat="server" />
            <asp:HiddenField ID="hdfcheckR" runat="server" />
            <asp:HiddenField ID="hdOrderInfoID" runat="server" />
            <asp:HiddenField ID="hdSession" runat="server" />
            <asp:HiddenField ID="hdfFeeType" runat="server" />
            <asp:HiddenField ID="hdfOtherFees" runat="server" />
            <asp:HiddenField ID="hdfCustomerID" runat="server" />
            <asp:HiddenField ID="hdfTransportCompanySubID" runat="server" />
            <asp:HiddenField ID="hdfCouponID" runat="server" />
            <asp:HiddenField ID="hdfCouponValue" runat="server" />
            <asp:HiddenField ID="hdfCouponProductNumber" runat="server" />
            <asp:HiddenField ID="hdfCouponPriceMin" runat="server" />
            <asp:HiddenField ID="hdfCouponIDOld" runat="server" />
            <asp:HiddenField ID="hdfCouponCodeOld" runat="server" />
            <asp:HiddenField ID="hdfCouponValueOld" runat="server" />
            <asp:HiddenField ID="hdfCouponProductNumberOld" runat="server" />
            <asp:HiddenField ID="hdfCouponPriceMinOld" runat="server" />
            <asp:HiddenField ID="hdfShippingType" runat="server" />
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
            <asp:HiddenField ID="hdfUpdateJtExpress" runat="server" Value="0" />
            <asp:HiddenField ID="hdfJtRecipientProvince" runat="server" />
            <asp:HiddenField ID="hdfJtRecipientDistrict" runat="server" />
            <asp:HiddenField ID="hdfJtRecipientWard" runat="server" />
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
                                <div class="col-xs-12 text-align-left text-danger"><p></p></div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button id="closeCoupon" type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                            <button id="insertCoupon" type="button" class="btn btn-primary" onclick="getCoupon()">Xác nhận</button>
                        </div>
                    </div>
                </div>
            </div>

            <!-- J&T Express Modal -->
            <div class="modal fade" id="jtExpressModal" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Cập nhật địa chỉ giao hàng J&T Express</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row form-group">
                                <label>Tỉnh thành</label>
                                <asp:DropDownList ID="ddlJtRecipientProvince" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="row form-group">
                                <label>Quận huyện</label>
                                <asp:DropDownList ID="ddlJtRecipientDistrict" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="row form-group">
                                <label>Phường xã</label>
                                <asp:DropDownList ID="ddlJtRecipientWard" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button id="closeJtExpress" type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                            <button id="updateJtExpress" type="button" class="btn btn-primary" onclick="updateJtRecipientAddress()">Cập nhật</button>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    </asp:Panel>
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
        <script type="text/javascript" src="App_Themes/Ann/js/delivery-address.js?v=03122022"></script>
        <script type="text/javascript">
            const PaymentMethodEnum = {
                /*
                 * 1: Tiền mặt
                 */
                "Cash": 1,
                /*
                 * 3: Thu hộ
                 */
                "CashCollection": 3,
            }

            const ShippingTypeEnum = {
                /*
                 * 1: Lấy trực tiếp
                 */
                "Face": 1,
                /*
                 * 8: Grab
                 */
                "Grab": 8,
            }

            // #region Private
            // Kiểm tra xác nhận thông tin khách hàng
            function _checkCustomerValidation() {
                let $name = $("#<%= txtFullname.ClientID%>");
                let $phone = $("#<%=txtPhone.ClientID%>");
                let $nick = $("#<%= txtNick.ClientID%>");
                let $facebook = $("#<%= txtFacebook.ClientID%>");
                let $username = $("#<%= hdfUsernameCurrent.ClientID%>");

                // Tên khách hàng
                if (!$name.val()) {
                    $name.focus();
                    swal("Thông báo", "Hãy nhập tên khách hàng!", "error");
                    return false;
                }

                // SDT khách hàng
                if (!$phone.val()) {
                    $phone.focus();
                    swal("Thông báo", "Hãy nhập số điện thoại khách hàng!", "error");
                    return false;
                }

                // Nick
                if (!$nick.val()) {
                    $nick.focus();
                    swal("Thông báo", "Hãy nhập Nick đặt hàng của khách hàng!", "error");
                    return false;
                }

                return true;
            }

            /* ============================================================
             * Kiểm tra hình thức "Lấy trực tiếp"
             *
             * Date:   2023-07-03
             * Author: Binh-TT
             *
             * Ràng buộc đã "Lấy trực tiếp" thì không cho "Thu hộ"
             * ============================================================
             */
            function _checkFace() {
                let paymentTypeDom = document.body.querySelector("#<%=ddlPaymentType.ClientID%>");
                let paymentType = paymentTypeDom.value ? parseInt(paymentTypeDom.value) : String(0);

                // Trường hợp "Thu hộ"
                if (paymentType === PaymentMethodEnum.CashCollection) {
                    let title = "Lạ vậy";
                    let msg = "Sao <strong>Lấy trực tiếp</strong> mà còn <strong>Thu hộ</strong>?<br><br>Xem lại nha!";

                    swal({
                        title,
                        text: msg,
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

            /* ============================================================
             * Kiểm tra hình thức giao "Grab"
             *
             * Date:   2023-07-03
             * Author: Binh-TT
             *
             * Ràng buộc đã "Grab" thì không cho "Thu hộ"
             * ============================================================
             */
            function _checkGrab() {
                let paymentTypeDom = document.body.querySelector("#<%=ddlPaymentType.ClientID%>");
                let paymentType = paymentTypeDom.value ? parseInt(paymentTypeDom.value) : String(0);

                // Trường hợp "Thu hộ"
                if (paymentType === PaymentMethodEnum.CashCollection) {
                    let title = "Lạ vậy";
                    let msg = "Sao <strong>Grab</strong> mà còn <strong>Thu hộ</strong>?<br><br>Xem lại nha!";

                    swal({
                        title,
                        text: msg,
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

            /* ============================================================
             * Kiểm tra phương thức giao hàng
             *
             * Date:   2023-07-03
             * Author: Binh-TT
             *
             * Ràng buộc "Lấy trực tiếp", "Gab" thì không có "Thu hộ"
             * ============================================================
             */
            function _checkShippingType() {
                let shippingTypeDom = document.body.querySelector("#<%=ddlShippingType.ClientID%>");
                let shippingType = shippingTypeDom.value ? parseInt(shippingTypeDom.value) : String(0);
                let checked = true;

                // Kiểm tra trường hợp "Lấy trực tiếp"
                if (shippingType === ShippingTypeEnum.Face)
                    checked = _checkFace();
                // Kiểm tra trường hợp "Gab"
                else if (shippingType === ShippingTypeEnum.Grab)
                    checked = _checkGrab();

                return checked;
            }

            function _checkValidation() {
                if (!_checkCustomerValidation())
                    return false;

                if (!checkDeliveryAddressValidation())
                    return false;

                // Kiểm tra phương thức giao hàng
                if (!_checkShippingType())
                    return false;

                return true;
            }

            function _updateDeliveryAddress() {
                // Cập nhật thông tin địa chỉ giao hàng
                let $phone = $("#<%=txtPhone.ClientID%>");

                return updateDeliveryAddress($phone.val());
            }

            /* ============================================================
             * Lấy thông tin để cập nhật đơn hàng
             *
             * Date:   2021-07-19
             * Author: Binh-TT
             *
             * Đối ứng chiết khấu từng dòng
             * ============================================================
             */
            function _updateOrder() {
                // Nếu có sản phẩm trong đơn hàng
                if ($(".product-result").length > 0) {
                    getAllPrice(true);
                    var list = "";
                    var ordertype = $(".customer-type").val();
                    var checkoutin = false;

                    $(".product-result").each(function () {
                        // 2021-07-19: Đối ứng chiết khấu từng dòng
                        let $discount = $(this).find(".discount");

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
                        // 2021-07-19: Đối ứng chiết khấu từng dòng
                        let discount = +$discount.val().replace(/,/g, '') || 0;

                        if (quantity > 0) {
                            if (quantity > quantityInstock)
                                checkoutin = true;

                            list += id + "," + sku + "," + producttype + "," + productvariablename + "," + productvariablevalue + "," + quantity + "," +
                                productname + "," + productimageorigin + "," + productvariablesave + "," + price + "," + productvariablesave + "," +
                                orderDetailID + "," + productvariableid + "," + discount + ";";
                        }
                    });

                    // Kiểm tra trạng thái xử lý
                    let excuteStatus = Number($("#<%=ddlExcuteStatus.ClientID%>").val());
                    let shippingType = Number($("#<%=ddlShippingType.ClientID%>").val());

                    // Nếu chọn trạng thái hủy
                    if (excuteStatus == 3) {
                        // Nếu trạng thái cũ là đã hủy thì thông báo hủy rồi
                        if ($("#<%=hdfExcuteStatus.ClientID%>").val() == 3) {
                            swal("Thông báo", "Đơn hàng này đã hủy trước đó!", "warning");
                        }
                        else {
                            swal({
                                title: "Xác nhận",
                                text: "Cưng có chắc hủy đơn hàng này không?",
                                type: "warning",
                                showCancelButton: true,
                                closeOnConfirm: false,
                                cancelButtonText: "Đợi em xem tí!",
                                confirmButtonText: "Chắc chắn sếp ơi..",
                            }, function (isConfirm) {
                                if (isConfirm) {
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
                                        if (orderNote != "") {
                                            $("#<%=txtOrderNote.ClientID %>").val(orderNote);
                                            $("#<%=ddlPaymentStatus.ClientID %>").val(1);
                                            deleteOrder();
                                            $("#<%=hdfOrderType.ClientID%>").val(ordertype);
                                            $("#<%=hdfListProduct.ClientID%>").val(list);
                                            insertOrder();
                                        }
                                        else {
                                            swal("Hủy đơn hàng thất bại!", "Chưa nhập lý do nên không được hủy", "error");
                                        }
                                    });
                                }
                            });
                        }
                    }
                    else if (excuteStatus != 3 && $("#<%=hdfExcuteStatus.ClientID%>").val() == 3) {
                        // Khôi phục đơn hàng đã hủy
                        // Chỉ admin mới được khôi phục đơn hàng hủy
                        if ($("#<%=hdfRoleID.ClientID%>").val() == 0) {
                            deleteOrder();
                            $("#<%=hdfOrderType.ClientID %>").val(ordertype);
                            $("#<%=hdfListProduct.ClientID%>").val(list);
                            insertOrder();
                        }
                        else {
                            swal("Không thể khôi phục đơn hàng đã hủy!", "Hãy báo cáo chị Ngọc để khôi phục", "error");
                        }
                    }
                    else if (excuteStatus == 1 && $("#<%=hdfExcuteStatus.ClientID%>").val() == 2) {
                        // Chỉ admin mới được đổi trạng thái Đã hoàn tất sang trạng thái Đang xử lý
                        if ($("#<%=hdfRoleID.ClientID%>").val() == 0) {
                            deleteOrder();
                            $("#<%=hdfOrderType.ClientID %>").val(ordertype);
                            $("#<%=hdfListProduct.ClientID%>").val(list);
                            insertOrder();
                        }
                        else {
                            swal("Không thể đổi trạng thái từ Đã hoàn tất sang Đang xử lý!", "Hãy báo cáo chị Ngọc để khôi phục", "error");
                        }
                    }
                    // Nếu chọn trạng thái hoàn tất và cần nhập mã vận đơn
                    else if (excuteStatus == 2 && (shippingType == 2 || shippingType == 3)) {
                        let shippingCode = $("#<%=txtShippingCode.ClientID%>").val();
                        if (shippingCode.length < 3) {
                            $("#<%=txtShippingCode.ClientID%>").focus();
                            swal("Thông báo", "Chưa nhập mã vận đơn!", "warning");
                        }
                        else {
                            deleteOrder();
                            $("#<%=hdfOrderType.ClientID %>").val(ordertype);
                            $("#<%=hdfListProduct.ClientID%>").val(list);
                            insertOrder();
                        }
                    }
                    // Nếu đơn hàng hoàn tất và chưa chọn chành xe hoặc nơi nhận chành xe
                    else if (excuteStatus == 2 && shippingType == 4) {
                        let transportCompanyID = $("#<%=ddlTransportCompanyID.ClientID%>").val();
                        let transportCompanySubID = $("#<%=ddlTransportCompanySubID.ClientID%>").val();
                        if (transportCompanyID == 0) {
                            $("#<%=ddlTransportCompanyID.ClientID%>").focus();
                            swal("Thông báo", "Chưa chọn chành xe!", "warning");
                        }
                        else if (transportCompanySubID == 0) {
                            $("#<%=ddlTransportCompanySubID.ClientID%>").focus();
                            swal("Thông báo", "Chưa chọn nơi nhận của chành xe!", "warning");
                        }
                        else {
                            deleteOrder();
                            $("#<%=hdfOrderType.ClientID %>").val(ordertype);
                            $("#<%=hdfListProduct.ClientID%>").val(list);
                            insertOrder();
                        }
                    }
                    else {
                        // Nếu trạng thái không liên quan đến hủy thì xử lý..
                        deleteOrder();
                        $("#<%=hdfOrderType.ClientID %>").val(ordertype);
                        $("#<%=hdfListProduct.ClientID%>").val(list);
                        insertOrder();
                    }
                }
                else {
                    // Nếu không có sản phẩm trong đơn
                    let excuteStatus = Number($("#<%=ddlExcuteStatus.ClientID%>").val());

                    if (excuteStatus == 3) {
                        swal({
                            title: "Xác nhận",
                            text: "Đơn hàng này sẽ bị hủy. Cưng có chắc hủy đơn này không?",
                            type: "warning",
                            showCancelButton: true,
                            closeOnConfirm: false,
                            cancelButtonText: "Đợi em xem tí!",
                            confirmButtonText: "Chắc chắn sếp ơi..",
                        }, function (isConfirm) {
                            if (isConfirm) {
                                swal({
                                    title: "Nhập lý do",
                                    text: "Nhập lý do hủy đơn hàng:",
                                    type: "input",
                                    showCancelButton: true,
                                    closeOnConfirm: false,
                                    cancelButtonText: "Đợi em suy nghĩ!",
                                    confirmButtonText: "Hủy thôi..",
                                }, function (orderNote) {
                                    if (orderNote != "") {

                                        $("#<%=txtOrderNote.ClientID %>").val(orderNote);
                                        $("#<%=ddlPaymentStatus.ClientID %>").val(1);

                                        deleteOrder();

                                        let order_id = $("#<%=hdOrderInfoID.ClientID%>").val();
                                        $.ajax({
                                            type: "POST",
                                            url: "thong-tin-don-hang.aspx/UpdateStatus",
                                            data: JSON.stringify({OrderID: order_id }),
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
                                    else {
                                        swal("Hủy đơn hàng thất bại!", "Chưa nhập lý do nên không được hủy", "error");
                                    }
                                });
                            }
                        });

                    }
                    else {
                        $("#txtSearch").focus();
                        swal("Thông báo", "Hãy nhập sản phẩm!", "error");
                    }
                }
            }
            // #endregion

            let preExcuteStatus = 0;

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
                    addHTML += "        <span class='otherfee-name'><i class='fa fa-check' aria-hidden='true'></i> " + fee.FeeTypeName + fee.Note + "</span>";
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
                $("#fee-list").before(createFeeHTML(fee));
                $("#<%=hdfOtherFees.ClientID%>").val(JSON.stringify(fees));
                getAllPrice(true);
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
                getAllPrice(true);
            }

            // cal fee ship
            function calFeeShip() {
                let deliveryMethod = +$("#<%=ddlShippingType.ClientID%>").find(":selected").val() || 1;

                if (deliveryMethod == 6 || deliveryMethod == 10) {
                    let message = '';

                    message += 'Đơn này gửi ';
                    if (deliveryMethod == 6)
                        message += '<strong>GHTK</strong>';
                    else if (deliveryMethod == 10)
                        message += '<strong>J&T Express</strong>';
                    message += ' nên không chọn miễn phí ship được!';
                    message += '<br>Nếu cần miễn giảm phí thì hãy tính phí bình thường, sau đó chọn trừ phí ship trong phí khác!';

                    return swal("Thông báo", message, "error");
                }

                if ($("#<%=pFeeShip.ClientID%>").is(":disabled")) {
                    $("#<%=pFeeShip.ClientID%>").prop('disabled', false).css("background-color", "#fff").focus();
                    $("#calfeeship").html("Miễn phí").css("background-color", "#F44336");
                }
                else {
                    $("#<%=pFeeShip.ClientID%>").prop('disabled', true).css("background-color", "#eeeeee").val(0);
                    swal("Thông báo", "Đã chọn miễn phí ship cho đơn hàng này<br><strong>Hãy ghi chú lý do miễn phí ship!!!</strong>", "success");
                    getAllPrice(true);
                    $("#calfeeship").html("<i class='fa fa-pencil-square-o' aria-hidden='true'></i> Tính phí").css("background-color", "#f87703");
                }
            }

            // remove other fee by click button
            function removeOtherFee(uuid) {
                $("#" + uuid).remove();
                fees = fees.filter((item) => { return item.UUID != uuid; });
                $("#<%=hdfOtherFees.ClientID%>").val(JSON.stringify(fees));
                countTotal();
            }

            // edit other fee by click button
            function editOtherFee(uuid) {
                $("#" + uuid).find(".otherfee-value").click();
            }

            function warningShippingNote(ID) {
                let deliveryMethod = +$("#<%=ddlShippingType.ClientID%>").find(":selected").val() || 1;
                let paymentType = +$("#<%=ddlPaymentType.ClientID%>").find(":selected").val() || 1;

                if (deliveryMethod == 2 && paymentType != 3) {
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
                else if (deliveryMethod == 6 && paymentType != 3) {
                    swal({
                        title: "Ê nhỏ:",
                        text: "Đơn hàng này gửi GHTK nhưng <strong>Không Thu Hộ</strong> hở?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Đúng rồi sếp!!",
                        closeOnConfirm: false,
                        cancelButtonText: "Để em xem lại..",
                        html: true
                    }, function (isConfirm) {
                        sweetAlert.close();
                        window.open("/print-shipping-note?id=" + ID, "_blank");
                    });
                }
                else if (deliveryMethod == 10) {
                    let jtCode = $('[id$="_txtShippingCode"]').val();
                    if (!jtCode)
                        swal({
                            title: "Error",
                            text: "Không tìm thấy mã vận đơn",
                            type: "error"
                        });
                    else {
                        if (paymentType != 3)
                            swal({
                                title: "Ê nhỏ:",
                                text: "Đơn hàng này gửi J&T nhưng <strong>Không Thu Hộ</strong> hở?",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "Đúng rồi sếp!!",
                                closeOnConfirm: false,
                                cancelButtonText: "Để em xem lại..",
                                html: true
                            }, function (isConfirm) {
                                sweetAlert.close();
                                window.open("/print-jt-express?id=" + ID + "&code=" + jtCode, "_blank");
                            });
                        else
                            window.open("/print-jt-express?id=" + ID + "&code=" + jtCode, "_blank");
                    }
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
                window.open("/print-order-image?id=" + ID + "&merge=" + mergeprint, "_blank");
            }

            // Thông tin khách hàng
            function _initCustomer() {
                // Text box Phone
                $("#<%=txtPhone.ClientID%>").keyup(function (e) {
                    if (/\D/g.test(this.value)) {
                        // Filter non-digits from input value.
                        this.value = this.value.replace(/\D/g, '');
                    }
                });

                // Text box Facebook
                if ($("input[id$='_txtFacebook']").val() == "") {
                    $("input[id$='_txtFacebook']").parent().addClass("width-100");
                }
            }

            // Các loại phí khác
            function _initOtherFees() {
                // Load Fee Type List
                let strFeeType = $("#<%=hdfFeeType.ClientID%>").val();

                if (strFeeType) {
                    let data = JSON.parse(strFeeType);

                    data.forEach((item) => {
                        feetype.push(new FeeType(item.ID, item.Name, item.IsNegativeFee));
                    });
                }

                // Load Fee List
                let strOtherFees = $("#<%=hdfOtherFees.ClientID%>").val();

                if (strOtherFees) {
                    let obj = JSON.parse(strOtherFees);

                    if ($.isArray(obj)) {
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
            }

            // Hình thức thanh toán
            function _initPaymentType() {
                if ($("#<%=ddlPaymentType.ClientID%>").find(":selected").val() != 2) {
                    $("#row-bank").addClass("hide");
                }

                $("#<%=ddlPaymentType.ClientID%>").change(function () {
                    var selected = $(this).find(":selected").val();
                    if (selected == 2) {
                        $("#row-bank").removeClass("hide");
                    }
                    else {
                        $("#row-bank").addClass("hide");
                    }
                });
            }

            // Hình thức giao hàng
            function _initShippingType(roleID) {
                //#region Phí vận chuyển
                let $btnFreeShipping = $("#calfeeship");
                let $fee = $("#<%=pFeeShip.ClientID%>");

                $btnFreeShipping.removeClass("hide");
                $fee.removeAttr('style')
                $fee.removeAttr('disabled')
                //#endregion

                //#region Phương thức giao hàng
                let $btnGHTK = $("#getShipGHTK");
                let $btnJtExpresFee = $("#btnJtExpresFee");
                let $weight = $(".weight-input");
                let $ddlShippingType = $("#<%=ddlShippingType.ClientID%>");
                let $transportCompany  = $(".transport-company");
                let $postalDeliveryType = $(".postal-delivery-type");
                let $shippingCode = $(".shipping-code");

                // Lấy trục tiếp
                if ($ddlShippingType.find(":selected").val() == 1) {
                    $transportCompany.addClass("hide");
                }

                // Bưu điện
                if ($ddlShippingType.find(":selected").val() == 2) {
                    $transportCompany.addClass("hide");
                    $postalDeliveryType.removeClass("hide");
                    $shippingCode.removeClass("hide");
                }

                // Proship
                if ($ddlShippingType.find(":selected").val() == 3) {
                    $shippingCode.removeClass("hide");
                    $transportCompany.addClass("hide");
                }

                // Chuyển xe
                if ($ddlShippingType.find(":selected").val() == 4) {
                    $transportCompany.removeClass("hide");
                    $shippingCode.addClass("hide");
                }

                // Nhân viên giao hàng
                if ($ddlShippingType.find(":selected").val() == 5) {
                    $transportCompany.addClass("hide");
                    $shippingCode.addClass("hide");
                }

                // GHTK
                if ($ddlShippingType.find(":selected").val() == 6) {
                    $btnGHTK.removeClass("hide");
                    $btnFreeShipping.addClass("hide");
                    $fee.prop('disabled', true).css("background-color", "#eeeeee");
                    $weight.removeClass("hide");
                    $transportCompany.addClass("hide");
                    $shippingCode.removeClass("hide");
                }

                // Vietel
                if ($ddlShippingType.find(":selected").val() == 7) {
                    $transportCompany.addClass("hide");
                    $shippingCode.addClass("hide");
                }

                // Grab
                if ($ddlShippingType.find(":selected").val() == 8) {
                    $transportCompany.addClass("hide");
                    $shippingCode.addClass("hide");
                }

                // AhaMove
                if ($ddlShippingType.find(":selected").val() == 9) {
                    $transportCompany.addClass("hide");
                    $shippingCode.addClass("hide");
                }

                // J&T
                if ($ddlShippingType.find(":selected").val() == 10) {
                    $btnJtExpresFee.removeClass("hide");
                    $btnFreeShipping.addClass("hide");
                    $fee.prop('disabled', true).css("background-color", "#eeeeee");
                    $weight.removeClass("hide");
                    $transportCompany.addClass("hide");
                    $shippingCode.removeClass("hide");
                }

                // GHN
                if ($ddlShippingType.find(":selected").val() == 11) {
                    $weight.removeClass("hide");
                    $transportCompany.addClass("hide");
                    $shippingCode.removeClass("hide");
                }

                // Event onChange
                let $ddlPaymentType = $("#<%=ddlPaymentType.ClientID%>");
                let $ddlPostalDeliveryType = $("#<%=ddlPostalDeliveryType.ClientID%>");
                let $ddlTransportCompany = $("#<%=ddlTransportCompanyID.ClientID%>");
                let $ddlTransportCompanySub = $("#<%=ddlTransportCompanySubID.ClientID%>");
                let $txtShippingCode = $("#<%=txtShippingCode.ClientID%>");

                $ddlShippingType.change(function () {
                    // Phí vận chuyển
                    $btnGHTK.addClass("hide");
                    $btnJtExpresFee.addClass("hide");
                    $btnFreeShipping.removeClass("hide");
                    $fee.removeAttr('style')
                    $fee.removeAttr('disabled')
                    $weight.addClass("hide");

                    // Hình thức thanh toán
                    if (roleID != 0)
                        $ddlPaymentType.find("option[value='1']").remove();

                    // Hình thức chuyển phát
                    $postalDeliveryType.addClass("hide");
                    $ddlPostalDeliveryType.val(1);
                    // Chành xe
                    $transportCompany.addClass("hide");
                    $ddlTransportCompany.val(0);
                    $ddlTransportCompanySub.val(0);
                    // Mã vẫn đơn
                    $shippingCode.addClass("hide");
                    $txtShippingCode.val("");

                    let selected = $(this).find(":selected").val();

                    switch (selected) {
                        // Lấy hàng trực tiếp
                        case "1":
                            if ($ddlPaymentType.find("option[value='1']").length == 0 && roleID != 0)
                                $ddlPaymentType.append('<option value="1">Tiền mặt</option>');
                            break;
                        // Bưu điện
                        case "2":
                            $shippingCode.removeClass("hide");
                            $postalDeliveryType.removeClass("hide");
                            break;
                        // Proship
                        case "3":
                            $shippingCode.removeClass("hide");
                            break;
                        // Chuyễn xe
                        case "4":
                            $transportCompany.removeClass("hide");
                            break;
                        // Nhân viên giao hàng
                        case "5":
                            break;
                        // GHTK
                        case "6":
                            $btnFreeShipping.addClass("hide");
                            $btnGHTK.removeClass("hide");
                            $fee.prop('disabled', true).css("background-color", "#eeeeee").val(0);
                            $weight.removeClass("hide");
                            $shippingCode.removeClass("hide");
                            break;
                        // Viettel
                        case "7":
                            break;
                        // Grab
                        case "8":
                            break;
                        // AhaMove
                        case "9":
                            break;
                        // J&T
                        case "10":
                            $btnFreeShipping.addClass("hide");
                            $btnJtExpresFee.removeClass("hide");
                            $fee.prop('disabled', true).css("background-color", "#eeeeee").val(0);
                            $weight.removeClass("hide");
                            $shippingCode.removeClass("hide");
                            break;
                        // GHN
                        case "11":
                            $weight.removeClass("hide");
                            $shippingCode.removeClass("hide");
                            break;
                        default:
                            break;
                    }
                });
                //#endregion
            }

            // Nhân viên phụ trách
            function _initStaff(roleID) {
                // Show change createdby if role = admin
                if (roleID == 0) {
                    $("#row-createdby").removeClass("hide");
                }

                // search Product by SKU
                $("#txtSearch").keydown(function (event) {
                    if (event.which === 13) {
                        searchProduct();
                        event.preventDefault();
                        return false;
                    }
                });
            }

            $(document).ready(function () {
                let roleID = $("#<%=hdfRoleID.ClientID%>").val();

                // Thông tin khách hàng
                _initCustomer();

                // Thông tin địa chỉ nhận hàng
                initDeliveryAddress();

                // Các loại phí khác
                _initOtherFees();

                // Hình thức thanh toán
                _initPaymentType();

                // Hình thức giao hàng
                _initShippingType(roleID);

                // Nhân Viên phụ trách
                _initStaff(roleID);

                // onchange drop down list excute status
                preExcuteStatus = +$("#<%=ddlExcuteStatus.ClientID%>").val() || 0;
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

                $("#createReturnOrder").click(() => {
                    let customerID = $("#<%=hdfCustomerID.ClientID%>").val();

                    if (!customerID) {
                        swal("Thông báo", "Không tìm thấy ID của khách hàng này", "error");
                    }
                    else {
                        createReturnOrder(customerID);
                    }
                });

                // Init Coupon
                let couponCodeOld = $('[id$="_hdfCouponCodeOld"]').val() || "";
                let couponValueOld = +$('[id$="_hdfCouponValueOld"]').val() || 0;
                if (couponCodeOld) {
                    $('[id$="_txtCouponValue"]').val(`${couponCodeOld.trim().toUpperCase()}: -${formatThousands(couponValueOld, ',')}`);
                    $('#btnOpenCouponModal').addClass('hide');
                    $('#btnGenerateCouponG25').addClass('hide');
                    $('#btnRemoveCouponCode').removeClass('hide');
                }

                $('[id$="_txtCouponCode"]').keypress((event) => {
                    if (event.which == 13)
                        getCoupon();
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

                return addHTML;
            };

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
            };

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
                    $(".totalpriceorderrefund").html('-' + formatThousands(refundGood.TotalPrice, ","));

                    $("#closeOrderReturn").click();
                    getAllPrice(true);
                }
            }

            // view return order by click button
            function viewReturnOrder(ID) {
                var win = window.open("/xem-don-hang-doi-tra?id=" + ID + "", '_blank');
                win.focus();
            };

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
                getAllPrice(true);
            };

            // pay order on click button
            function payAll() {
                if (!_checkValidation())
                    return;

                Promise.all([_updateDeliveryAddress()])
                    .then(function () {
                        _updateOrder();
                    })
                    .catch(function () {
                    });
            };

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

            // insert order
            function insertOrder() {
                let transSub = $("#<%=ddlTransportCompanySubID.ClientID%>").val();
                $("#<%=hdfTransportCompanySubID.ClientID%>").val(transSub);

                var shippingtype = $(".shipping-type").val();
                var checkAllValue = true;
                var fs = $("#<%=pFeeShip.ClientID%>").val();
                var feeship = parseFloat(fs.replace(/\,/g, ''));

                // kiểm tra nhập phí vận chuyển chưa
                if (shippingtype == 2 || shippingtype == 3 || shippingtype == 7 || shippingtype == 11) {
                    if (feeship == 0 && $("#<%=pFeeShip.ClientID%>").is(":disabled") == false) {
                        $("#<%=pFeeShip.ClientID%>").focus();
                        swal({
                            title: "Có vấn đề:",
                            text: "Chưa nhập phí vận chuyển!<br><br>Đơn này miễn phí vận chuyển luôn hở?",
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
                    // GHN
                    if (shippingtype == 11) {
                        if (!_checkWeight())
                            checkAllValue = false;
                    }
                }
                // Trường hợp giao hàng tiết kiệm
                else if (shippingtype == 6 || shippingtype == 10) {
                    if (feeship == 0) {
                        $("#<%=pFeeShip.ClientID%>").focus();
                        swal({
                            title: "Có vấn đề:",
                            text: "Chưa nhập phí vận chuyển!<br><br>Đơn này miễn phí vận chuyển luôn hở?",
                            type: "warning",
                            confirmButtonText: "Để em tính lại phí!!",
                            html: true
                        });
                        checkAllValue = false;
                    }
                }
                else if (shippingtype == 4) {
                    if (feeship == 0 && $("#<%=pFeeShip.ClientID%>").is(":disabled") == false) {
                        var ID = $("#<%=ddlTransportCompanyID.ClientID%>").val();
                        var SubID = $("#<%=ddlTransportCompanySubID.ClientID%>").val();

                        if (ID != 0 && SubID != 0) {
                            var checkPrepay = checkPrepayTransport(ID, SubID);
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
                else if (shippingtype == 10) {
                    // J&T
                    if (!_checkWeight())
                        checkAllValue = false;
                }

                // kiểm tra phí vận chuyển có nhỏ hơn 10k
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

                //#region Kiểm tra chiết khấu
                let $products = $(".product-result");

                let errorDiscount = false;

                $products.each(function () {
                    errorDiscount = Boolean($(this).attr("data-error-discount"));

                    if (errorDiscount)
                        return false;
                });

                if (errorDiscount) {
                    checkAllValue = false;

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
                }
                //#endregion

                // chuyển người tạo đơn
                var ddlCreatedBy = $("#<%=ddlCreatedBy.ClientID%>").val();
                var createdBy = $("#<%=hdfUsername.ClientID%>").val();

                if (createdBy != ddlCreatedBy)
                {
                    checkAllValue = false;
                    swal({
                        title: "Ê nhỏ:",
                        text: "Chuyển đơn hàng này cho <strong>" + ddlCreatedBy + "</strong> phụ trách hở?<br>Nếu vậy thì khách này cũng chuyển cho nhân viên này phụ trách nhé!",
                        type: "info",
                        showCancelButton: true,
                        closeOnConfirm: true,
                        closeOnCancel: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Đúng rồi sếp!!",
                        cancelButtonText: "Để em xem lại..",
                        html: true,
                    }, function (isconfirm) {
                        if (isconfirm)
                        {
                            checkAllValue = true;
                            $("#<%=hdfUsername.ClientID%>").val(ddlCreatedBy);
                            insertOrder();
                        }
                    });
                }

                if (checkAllValue == true)
                {
                    HoldOn.open();
                    $("#<%=btnOrder.ClientID%>").click();
                }
            };

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
            };

            function searchRemovedList(ProductSKU) {
                var t = ""
                for (i = 0; i < listOrderDetail.length; i++) {
                    if (listOrderDetail[i].SKU == ProductSKU) {
                        t += "data-orderdetailid=\"" + listOrderDetail[i].ID + "\"";
                    }
                }
                return t;
            };

            function removeProductExisted(orderServer) {
                for (index in listOrderDetail) {
                    if (listOrderDetail[index].ProductID == orderServer.ID) {
                        listOrderDetail.splice(index, 1);
                    }
                }
            };

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
             * 6) Cập nhật đơn hàng
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
                    // Tính tiền từng sản phẩm
                    $products.each(function () {
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
                            console.error(err.message);
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

                // Trạng thái đơn hàng
                if ($products.length == 0)
                    $("#<%=ddlExcuteStatus.ClientID%>").val(3);

                /*
                 * Nếu trường hợp là hoàn tất thì sắp xếp lại sản phẩm theo thứ tự index tăng dần.
                 * Trường hợp khác thì sắp index theo thứ tự giảm dần
                 */
                let excuteStatus = +document.querySelector('[id$="_ddlExcuteStatus"]').value || 0;
                reIndex(excuteStatus == 1);
            };

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
                        } else {
                            $(this).find(".gia-san-pham").attr("data-price", giasi).html(formatThousands(giasi, ','));
                        }
                    });
                    getAllPrice(true);
                }
            };

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
                    if (keypressed == 8 || keypressed == 127)
                        return;
                    return false;
                }
            };

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
                if (listOrderDetail.length > 0) {
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
            };

            function onChangeExcuteStatus() {
                let excuteStatus = Number($("#<%=ddlExcuteStatus.ClientID%>").val());

                switch (excuteStatus) {
                    // Đã hoàn tất
                    case 2:
                        if ($("#<%=hdfExcuteStatus.ClientID%>").val() != 1)
                        {
                            $("#infor-customer").addClass("disable");
                            $("#deliveryAddress").addClass("disable");
                            $("#detail").addClass("disable");
                            $("#row-payment-type").addClass("disable");
                            $("#row-shipping-type").addClass("disable");
                            $("#row-transport-company").addClass("disable");
                            $("#row-postal-delivery-type").addClass("disable");
                            $("#row-shipping").addClass("disable");
                            $("#row-bank").addClass("disable");

                            if ($("#<%=hdfExcuteStatus.ClientID%>").val() == 2)
                            {
                                $("#row-payment-status").removeClass("disable");
                                $("#row-order-note").removeClass("disable");
                            }
                        }

                        $(".post-table-links").find("a").attr("disabled", false)

                        break;
                    // Đã hủy
                    case 3:
                        $("#infor-order").addClass("disable");
                        $("#infor-customer").addClass("disable");
                        $("#deliveryAddress").addClass("disable");
                        $("#detail").addClass("disable");
                        $("#status .panel-heading").addClass("disable");
                        $("#row-payment-status").addClass("disable");
                        $("#row-payment-type").addClass("disable");
                        $("#row-shipping-type").addClass("disable");
                        $("#row-transport-company").addClass("disable");
                        $("#row-postal-delivery-type").addClass("disable");
                        $("#row-shipping").addClass("disable");
                        $("#row-order-note").addClass("disable");
                        $("#row-bank").addClass("disable");
                        removeCoupon();
                        $(".post-table-links").find("a").attr("disabled", false)

                        break;
                    // Đang xử lý
                    case 1:
                        if ($("#<%=hdfExcuteStatus.ClientID%>").val() == 1) {
                            $("#infor-order").removeClass("disable");
                            $("#infor-customer").removeClass("disable");
                            $("#deliveryAddress").removeClass("disable");
                            $("#detail").removeClass("disable");
                            $("#status .panel-heading").removeClass("disable");
                            $("#row-payment-status").removeClass("disable");
                            $("#row-payment-type").removeClass("disable");
                            $("#row-shipping-type").removeClass("disable");
                            $("#row-transport-company").removeClass("disable");
                            $("#row-postal-delivery-type").removeClass("disable");
                            $("#row-shipping").removeClass("disable");
                            $("#row-order-note").removeClass("disable");
                            $("#row-bank").removeClass("disable");
                        }
                        else {
                            if($("#<%=hdfRoleID.ClientID%>").val() == 0)
                            {
                                $("#infor-order").removeClass("disable");
                                $("#infor-customer").removeClass("disable");
                                $("#deliveryAddress").removeClass("disable");
                                $("#detail").removeClass("disable");
                                $("#status .panel-heading").removeClass("disable");
                                $("#row-payment-status").removeClass("disable");
                                $("#row-payment-type").removeClass("disable");
                                $("#row-shipping-type").removeClass("disable");
                                $("#row-transport-company").removeClass("disable");
                                $("#row-postal-delivery-type").removeClass("disable");
                                $("#row-shipping").removeClass("disable");
                                $("#row-order-note").removeClass("disable");
                                $("#row-bank").removeClass("disable");
                            }
                            else
                            {
                                $("#infor-customer").addClass("disable");
                                $("#deliveryAddress").addClass("disable");
                                $("#detail").addClass("disable");
                                $("#status .panel-heading").addClass("disable");
                                $("#row-payment-type").addClass("disable");
                                $("#row-shipping-type").addClass("disable");
                                $("#row-transport-company").addClass("disable");
                                $("#row-postal-delivery-type").addClass("disable");
                                $("#row-shipping").addClass("disable");
                                $("#row-order-note").addClass("disable");
                                $("#row-bank").addClass("disable");
                            }
                        }

                        $(".post-table-links").find("a").attr("disabled", false)

                        break;
                    // Đã gửi hàng
                    case 5:
                    // Chuyển hoàn
                    case 4:
                        $("#infor-order").removeClass("disable");
                        $("#infor-customer").addClass("disable");
                        $("#deliveryAddress").addClass("disable");
                        $("#detail").addClass("disable");
                        $("#status .panel-heading").addClass("disable");
                        $("#row-payment-status").addClass("disable");
                        $("#row-payment-type").addClass("disable");
                        $("#row-shipping-type").addClass("disable");
                        $("#row-transport-company").addClass("disable");
                        $("#row-postal-delivery-type").addClass("disable");
                        $("#row-shipping").addClass("disable");
                        $("#row-order-note").addClass("disable");
                        $("#row-bank").addClass("disable");
                        $(".post-table-links").find("a").attr("disabled", true)

                        break;
                    default:
                        break;
                };

                // Trường hợp trạng thái trước là (hoàn tất đơn hàng || hủy)
                if (preExcuteStatus == 2 || preExcuteStatus == 3) {
                    // Chuyển qua trạng thái đang xử lý
                    if (excuteStatus == 1) {
                        let contentProduct = document.querySelector('.content-product');
                        let products = [...contentProduct.children];

                        contentProduct.innerHTML = "";
                        products.reverse();
                        products.forEach((item) => contentProduct.innerHTML += item.outerHTML);

                        // Cập nhật lại index
                        for (let i = 0; i < contentProduct.children.length; i++) {
                            let item = contentProduct.children[i];
                            item.querySelector('.order-item').innerText = contentProduct.children.length - i;
                        }
                    }
                }

                // Trường hợp trạng thái trước là đang xử lý
                if (preExcuteStatus == 1) {
                    // Chuyển qua trạng thái (hoàn tất đơn hàng || hủy)
                    if (excuteStatus == 2 || excuteStatus == 3) {
                        let contentProduct = document.querySelector('.content-product');
                        let products = [...contentProduct.children];

                        contentProduct.innerHTML = "";
                        products.reverse();
                        products.forEach((item) => contentProduct.innerHTML += item.outerHTML);

                        // Cập nhật lại index
                        for (let i = 0; i < contentProduct.children.length; i++) {
                            let item = contentProduct.children[i];
                            item.querySelector('.order-item').innerText = i + 1;
                        }
                    }
                }

                preExcuteStatus = excuteStatus;
            };

            function onChangeTransportCompany(transport) {
                let transComID = transport.val();
                $.ajax({
                    url: "thong-tin-don-hang.aspx/GetTransportSub",
                    type: "POST",
                    data: JSON.stringify({ 'transComID': transComID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        let data = JSON.parse(msg.d);
                        if (data) {
                            let tranSubDOM = $("#<%=ddlTransportCompanySubID.ClientID%>");
                            tranSubDOM.html("")
                            data.forEach((item) => {
                                tranSubDOM.append("<option value='" + item.key + "'>" + item.value + "</option>")
                            });

                            let tranSubContainerDOM = $("[id$=ddlTransportCompanySubID-container]");
                            tranSubContainerDOM.attr("title", "Chọn nơi nhận");
                            tranSubContainerDOM.html("Chọn nơi nhận");

                            $("#<%=ddlTransportCompanySubID.ClientID%>").select2("open");
                        }
                    },
                    error: function (err) {
                        swal("Thông báo", "Đã có vấn đề trong việc cập nhật thông tin vận chuyển", "error");
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
                    return swal("Thông báo", "Chưa nhập thông tin khách hàng!", "warning");

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

                let couponCodeOld = document.querySelector('[id$="_hdfCouponCodeOld"]').value || "";

                code = code.trim().toUpperCase();
                couponCodeOld = couponCodeOld ? couponCodeOld.trim().toUpperCase() : "";

                if (code === couponCodeOld) {
                    let couponIDOld = +document.querySelector('[id$="_hdfCouponIDOld"]').value || 0;
                    let couponValueOld = +document.querySelector('[id$="_hdfCouponValueOld"]').value || 0;
                    let couponProductNumberOld = +document.querySelector('[id$="_hdfCouponProductNumberOld"]').value || 0;
                    let couponPriceMinOld = +document.querySelector('[id$="_hdfCouponPriceMin"]').value || 0;

                    document.querySelector('[id$="_txtCouponValue"]').value = `${couponCodeOld}: -${formatThousands(couponValueOld, ',')}`;
                    document.querySelector('[id$="_hdfCouponID"]').value = couponIDOld;
                    document.querySelector('[id$="_hdfCouponValue"]').value = couponValueOld;
                    document.querySelector('[id$="_hdfCouponProductNumber"]').value = couponProductNumberOld;
                    document.querySelector('[id$="_hdfCouponPriceMin"]').value = couponPriceMinOld;

                    couponModalDOM.querySelector('#closeCoupon').click();
                    document.querySelector('#btnOpenCouponModal').classList.add('hide');
                    document.querySelector('#btnRemoveCouponCode').classList.remove('hide');

                    getAllPrice(true);

                    return;
                }

                $.ajax({
                    type: "POST",
                    url: "/thong-tin-don-hang.aspx/getCoupon",
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
                                document.querySelector('[id$="_txtCouponValue"]').value = `${code}: -${formatThousands(+data.value || 0, ',')}`;
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
                let customerID = $("#<%=hdfCustomerID.ClientID%>").val();
                let customerName = $("#<%=txtFullname.ClientID%>").val();
                if (!customerID)
                    return swal("Thông báo", "Chưa nhập thông tin khách hàng! Hoặc đây là khách hàng mới...", "warning");

                generateCouponG25(customerName, customerID);
            }

            function _checkWeight() {
                let $weight = $("#<%=txtWeight.ClientID%>");
                let weight = $weight.val() || 0;

                if (weight <= 0) {
                    $("#<%=txtWeight.ClientID%>").focus();
                    swal("Thông báo", "Chưa nhập khối lượng đơn hàng!", "warning");

                    return false;
                }
                else if (weight > 150) {
                    $("#<%=txtWeight.ClientID%>").focus();
                    swal("Thông báo", "Khối lượng đơn hàng phải nhỏ hơn 150kg", "warning");

                    return false;
                }

                return true;
            }

            function getShipGHTK() {
                // Kiểm tra trọng lượng
                if (!_checkWeight())
                    return;

                if (!checkDeliveryAddressValidation())
                    return;

                let url = "/api/v1/delivery-save/fee",
                    query = "",
                    address = $("#<%=txtRecipientAddress%>").val() || "",
                    province = $("[id$='_ddlRecipientProvince'] :selected").text() || "",
                    district = $("[id$='_ddlRecipientDistrict'] :selected").text() || "",
                    ward = $("[id$='_ddlRecipientWard'] :selected").text() || "",
                    $pFeeShip = $("#<%=pFeeShip.ClientID%>"),
                    fee = +$pFeeShip.val() || 0,
                    weight = $("#<%=txtWeight.ClientID%>").val() || 0,
                    value = parseInt($(".totalpriceorderall").html().replace(/,/g,''))  || 0;

                query += "&pick_address=15 Đông Hưng Thuận 45";
                query += "&pick_province=TP. Hồ Chí Minh";
                query += "&pick_district=Quận 12";
                query += "&pick_ward=Phường Tân Hưng Thuận";
                query += "&address=" + address;
                query += "&province=" + province;
                query += "&district=" + district;
                query += "&ward=" + ward;
                query += "&weight=" + (weight * 1000);
                query += "&transport=road";

                if (value > 0)
                    query += "&value=" + value;

                if (query)
                    url = url + "?" + query.substring(1);

                let titleAlert = "Tính phí giao hàng";

                $.ajax({
                    method: 'GET',
                    url: url,
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    success: (data, textStatus, xhr) => {
                        HoldOn.close();

                        if (xhr.status == 200 && data) {
                            if (data.success) {
                                fee = data.fee.fee;

                                if (data.fee.insurance_fee > 0)
                                    fee -= data.fee.insurance_fee;

                                $pFeeShip.val(formatNumber(fee.toString()));

                                countTotal();
                            }
                            else {
                                _alterError(titleAlert, { message: data.message });
                            }
                        } else {
                            _alterError(titleAlert);
                        }
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();

                        _alterError(titleAlert);
                    }
                });
            }

            function _alterError(title, responseJSON) {
                let message = 'Đẫ có lỗi xãy ra.';

                if (!title)
                    title = 'Thông báo lỗi'

                if (responseJSON.message)
                    message = responseJSON.message;

                return swal({
                    title: title,
                    text: message,
                    type: "error",
                    html: true
                });
            }

            function cancelGhtk(orderId, code) {
                let titleAlert = "Hủy đơn GHTK";

                $.ajax({
                    method: 'POST',
                    contentType: 'application/json',
                    dataType: "json",
                    data: JSON.stringify({ orderId: orderId }),
                    url: "/api/v1/delivery-save/order/" + code + "/cancel",
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    success: (data, textStatus, xhr) => {
                        HoldOn.close();

                        if (xhr.status == 200 && data) {
                            return swal({
                                title: titleAlert,
                                text: "Hủy thành công!<br>Đơn GHTK '<strong>" + code + "</strong>'",
                                icon: "success",
                            }, function (isConfirm) {
                                let $btnShowGhtk = $("#btnShowGhtk");
                                let $btnCancelGhtk = $("#btnCancelGhtk");
                                let $divParent = $btnShowGhtk.parent();
                                let $txtShippingCode = $("[id$='_txtShippingCode']");
                                let btnRegisterGhtkHtml = "";

                                btnRegisterGhtkHtml += "<a target='_blank'";
                                btnRegisterGhtkHtml += "   href='/dang-ky-ghtk?orderID=" + orderId + "'";
                                btnRegisterGhtkHtml += "   class='btn primary-btn btn-ghtk fw-btn not-fullwidth print-invoice-merged'";
                                btnRegisterGhtkHtml += ">";
                                btnRegisterGhtkHtml += "    <i class='fa fa-upload' aria-hidden='true'></i> Đẩy đơn GHTK";
                                btnRegisterGhtkHtml += "</a>";

                                $btnShowGhtk.remove();
                                $btnCancelGhtk.remove();
                                $divParent.append(btnRegisterGhtkHtml);
                                $txtShippingCode.val('');
                            });
                        } else {
                            return _alterError(titleAlert);
                        }
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();

                        return _alterError(titleAlert);
                    }
                });
            }

            function onChangeWeight() {
                let shippingType = Number($("#<%=ddlShippingType.ClientID%>").val());

                if (shippingType == 6 || shippingType == 10) {
                    let $txtWeight = $("#<%=txtWeight.ClientID%>");
                    let weight = +$txtWeight.val() || 0;

                    if (weight) {
                        if (shippingType == 6)
                            getShipGHTK();
                        else if (shippingType == 10)
                            getJtExpresFee();
                    }
                    else {
                        $("#<%=pFeeShip.ClientID%>").val(0);
                        countTotal();
                    }
                }
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
                else {
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
                let strDiscount = formatThousands(discount, ',');

                if (discount == 0 && $pDiscount.val() === '')
                    $pDiscount.val(0);

                swal({
                    title: "Chiết khấu",
                    text: "Áp dụng chiết khấu <strong>" + strDiscount + "/cái</strong> cho tất sản phẩm?",
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
                                {
                                    $discount.val(strDiscount);
                                    $discount.attr("value", strDiscount);
                                }
                                else
                                {
                                    $discount.val(0);
                                    $discount.attr("value", 0)
                                }
                                //#endregion

                                //#region Kiểm tra chiết khấu
                                let costOfGoods = +$(this).data("costOfGoods") || 0;
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

            //#region J&T Express
            function _disabledJtRecipientDistrict(disabled) {
                let placeHolder = '(Bấm để chọn: ' + $('[id$="_ddlRecipientDistrict"] option:selected').text() + ')';
                let province = $("#<%=hdfJtRecipientProvince.ClientID%>").val() || '';
                let $ddlJtRecipientDistrict = $("#<%=ddlJtRecipientDistrict.ClientID%>");

                if (disabled) {
                    $ddlJtRecipientDistrict.attr('disabled', true);
                    $ddlJtRecipientDistrict.attr('readonly', 'readonly');
                    $ddlJtRecipientDistrict.val(null).trigger('change');
                    $ddlJtRecipientDistrict.select2({
                        width: "100%",
                        placeholder: placeHolder
                    });
                }
                else {
                    $ddlJtRecipientDistrict.removeAttr('disabled');
                    $ddlJtRecipientDistrict.removeAttr('readonly');
                    $ddlJtRecipientDistrict.val(null).trigger('change');
                    $ddlJtRecipientDistrict.select2({
                        width: "100%",
                        placeholder: placeHolder,
                        ajax: {
                            delay: 500,
                            method: 'GET',
                            url: '/api/v1/jt-express/districts/select2',
                            data: (params) => {
                                var query = {
                                    province: province,
                                    page: params.page || 1
                                }

                                if (params.term)
                                    query.search = params.term;

                                return query;
                            }
                        }
                    });
                }
            }

            function _disabledJtRecipientWard(disabled) {
                let placeHolder = '(Bấm để chọn: ' + $('[id$="_ddlRecipientWard"] option:selected').text() + ')';
                let province = $("#<%=hdfJtRecipientProvince.ClientID%>").val() || '';
                let district = $("#<%=hdfJtRecipientDistrict.ClientID%>").val() || '';
                let $ddlJtRecipientWard = $("#<%=ddlJtRecipientWard.ClientID%>");

                if (disabled) {
                    $ddlJtRecipientWard.attr('disabled', true);
                    $ddlJtRecipientWard.attr('readonly', 'readonly');
                    $ddlJtRecipientWard.val(null).trigger('change');
                    $ddlJtRecipientWard.select2({
                        width: "100%",
                        placeholder: placeHolder
                    });
                }
                else {
                    $ddlJtRecipientWard.removeAttr('disabled');
                    $ddlJtRecipientWard.removeAttr('readonly');
                    $ddlJtRecipientWard.val(null).trigger('change');
                    $ddlJtRecipientWard.select2({
                        width: "100%",
                        placeholder: placeHolder,
                        ajax: {
                            delay: 500,
                            method: 'GET',
                            url: '/api/v1/jt-express/wards/select2',
                            data: (params) => {
                                var query = {
                                    province: province,
                                    district: district,
                                    page: params.page || 1
                                }

                                if (params.term)
                                    query.search = params.term;

                                return query;
                            }
                        }
                    });
                }
            }

            function _initJtRecipientAddress() {
                let placeHolder = '(Bấm để chọn: ' + $('[id$="_ddlRecipientProvince"] option:selected').text() + ')';
                let $ddlJtRecipientProvince = $("#<%=ddlJtRecipientProvince.ClientID%>");

                $ddlJtRecipientProvince.val(null).trigger('change');

                // Danh sách tỉnh / thành phố
                $ddlJtRecipientProvince.select2({
                    width: "100%",
                    placeholder: placeHolder,
                    ajax: {
                        delay: 500,
                        method: 'GET',
                        url: '/api/v1/jt-express/provinces/select2',
                        data: (params) => {
                            var query = {
                                page: params.page || 1
                            }

                            if (params.term)
                                query.search = params.term;

                            return query;
                        }
                    }
                });

                // Danh sách quận / huyện
                _disabledJtRecipientDistrict(true);

                // Danh sách phường / xã
                _disabledJtRecipientWard(true);
            }

            function _onChangetRecipientAddress() {
                let $ddlJtRecipientDistrict = $("#<%=ddlJtRecipientDistrict.ClientID%>");
                let $ddlJtRecipientWard = $("#<%=ddlJtRecipientWard.ClientID%>");

                // Danh sách tỉnh / thành phố
                $("#<%=ddlJtRecipientProvince.ClientID%>").on('select2:select', (e) => {
                   $("#<%=hdfJtRecipientProvince.ClientID%>").val(e.params.data.id);

                    // Danh sách quận / huyện
                    _disabledJtRecipientDistrict(false);
                    $ddlJtRecipientDistrict.select2('open');

                    // Danh sách phường / xã
                    _disabledJtRecipientWard(true);
                });

                // Danh sách quận / huyện
                $ddlJtRecipientDistrict.on('select2:select', (e) => {
                    $("#<%=hdfJtRecipientDistrict.ClientID%>").val(e.params.data.id);

                    // Danh sách quận / huyện
                    _disabledJtRecipientWard(false);
                    $ddlJtRecipientWard.select2('open');
                })

                // Danh sách phường / xã
                $ddlJtRecipientWard.on('select2:select', (e) => {
                    $("#<%=hdfJtRecipientWard.ClientID%>").val(e.params.data.id)
                });
            }

            function _openJtExpressModal() {
                let $jtExpressModal = $('#jtExpressModal');

                //#region Init
                _initJtRecipientAddress();
                _onChangetRecipientAddress();

                $jtExpressModal.modal({ show: 'true', backdrop: 'static', keyboard: false });
                //#endregion

                $jtExpressModal.on('shown.bs.modal');
            }

            function getJtExpresFee() {
                // Kiểm tra trọng lượng
                if (!_checkWeight())
                    return;

                if (!checkDeliveryAddressValidation())
                    return;

                let province = $("#<%=hdfJtRecipientProvince.ClientID%>").val() || "";
                let district = $("#<%=hdfJtRecipientDistrict.ClientID%>").val() || "";
                let ward = $("#<%=hdfJtRecipientWard.ClientID%>").val() || "";

                if (!province && !district && !ward)
                {
                    _openJtExpressModal();
                    return;
                }

                let price = +$(".totalpriceorderall").html().replace(/,/g, '') || 0;
                let paymentMethod = +$("select[id$='_ddlPaymentType']").val() || PaymentMethodEnum.Cash;
                let weight = +$("input[id$='_txtWeight']").val() || 0;
                let url = '/api/v1/jt-express/fee';
                let query = '';

                query += '&ward=' + ward;

                if (price > 0)
                {
                    query += '&price=' + price.toString();

                    if (paymentMethod == PaymentMethodEnum.CashCollection)
                        query += '&cod=' + price.toString();
                }

                query += '&weight=' + weight.toString();

                if (query)
                    url = url + "?" + query.substring(1);

                let titleAlert = "Tính phí giao hàng";

                $.ajax({
                    method: 'GET',
                    url: url,
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    success: (response, textStatus, xhr) => {
                        HoldOn.close();

                        if (xhr.status == 200 && response) {
                            if (response.success) {
                                let fee = response.data.fee;
                                let strFee = formatNumber(fee.toString());

                                $("#<%=pFeeShip.ClientID%>").val(strFee);
                                countTotal();
                            }
                            else {
                                _alterError(titleAlert, { message: response.message });
                            }
                        } else {
                            _alterError(titleAlert);
                        }
                    },
                    error: err => {
                        HoldOn.close();
                        _alterError(titleAlert);
                    }
                });
            }

            function updateJtRecipientAddress()
            {
                $("#<%=hdfUpdateJtExpress.ClientID%>").val(1);
                $('#closeJtExpress').click();
                getJtExpresFee();
            }

            function cancelJtExpress(orderId, code) {
                let titleAlert = "Hủy đơn J&T Express";

                $.ajax({
                    method: 'POST',
                    contentType: 'application/json',
                    dataType: "json",
                    data: JSON.stringify({ orderId: orderId }),
                    url: "/api/v1/jt-express/order/" + code + "/cancel",
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    success: (data, textStatus, xhr) => {
                        HoldOn.close();

                        if (xhr.status == 200 && data) {
                            if (data.success)
                                return swal({
                                    title: titleAlert,
                                    text: "Hủy thành công!<br>Đơn J&T Express '<strong>" + code + "</strong>'",
                                    icon: "success",
                                }, function (isConfirm) {
                                    let $btnShowJt = $("#btnShowJtExpress");
                                    let $btnCancelJt = $("#btnCancelJtExpress");
                                    let $divParent = $btnShowJt.parent();
                                    let $txtShippingCode = $("input[id$='_txtShippingCode']");
                                    let btnRegisterJtHtml = "";

                                    btnRegisterJtHtml += "<a target='_blank'";
                                    btnRegisterJtHtml += "   href='/dang-ky-jt?orderID=" + orderId + "'";
                                    btnRegisterJtHtml += "   class='btn primary-btn btn-ghtk fw-btn not-fullwidth print-invoice-merged'";
                                    btnRegisterJtHtml += ">";
                                    btnRegisterJtHtml += "    <i class='fa fa-upload' aria-hidden='true'></i> Đẩy đơn J&T";
                                    btnRegisterJtHtml += "</a>";

                                    $btnShowJt.remove();
                                    $btnCancelJt.remove();
                                    $divParent.append(btnRegisterJtHtml);
                                    $txtShippingCode.val('');
                                });
                            else
                                return _alterError(titleAlert, data);

                        } else
                            return _alterError(titleAlert, data);
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();

                        return _alterError(titleAlert, xhr.responseJSON);
                    }
                });
            }
            //#endregion
        </script>
    </telerik:RadScriptBlock>
</asp:Content>
