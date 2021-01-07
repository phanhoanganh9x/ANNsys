<%@ Page Title="Thông tin đơn đặt hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="thong-tin-don-dat-hang.aspx.cs" Inherits="IM_PJ.thong_tin_don_dat_hang" EnableSessionState="ReadOnly" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="App_Themes/Ann/css/pages/thong-tin-don-dat-hang/thong-tin-don-dat-hang.css?v=202101051418" />
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
                                    <asp:Literal ID="ltrHeading" runat="server"></asp:Literal>
                                </h3>
                            </div>
                            <div class="panel-body">
                                <div class="row pad">
                                    <div class="col-md-3">
                                        <label class="left pad10">Loại đơn: </label>
                                        <div id="orderType"></div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Nhân viên: </label>
                                        <div id="createBy"></div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Ngày tạo: </label>
                                        <div id="createDate"></div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Hoàn tất: </label>
                                    </div>
                                </div>
                                <div class="row pad">
                                    <div class="col-md-3">
                                        <label class="left pad10">Số lượng: </label>
                                        <div id="totalQuantity"></div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Tổng tiền: </label>
                                        <div id="total">
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Trạng thái: </label>
                                        <div id="orderStatus"></div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Ghi chú: </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="infor-customer" class="row disable">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Thông tin khách hàng</h3>
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
                                            <asp:TextBox ID="txtPhone" CssClass="form-control" runat="server" Enabled="false" placeholder="Số điện thoại khách hàng" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Nick đặt hàng</label>
                                            <asp:TextBox ID="txtNick" CssClass="form-control capitalize" runat="server" Enabled="true" placeholder="Tên nick đặt hàng" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Facebook</label>
                                            <div class="row">
                                                <div class="col-md-9 fb">
                                                    <asp:TextBox ID="txtFacebook" CssClass="form-control" runat="server" Enabled="true" placeholder="Đường link chat Facebook" autocomplete="off"></asp:TextBox>
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
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Tỉnh thành</label>
                                            <asp:DropDownList ID="ddlProvince" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Quận huyện</label>
                                            <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Phường xã</label>
                                            <asp:DropDownList ID="ddlWard" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Địa chỉ</label>
                                            <asp:TextBox ID="txtAddress" CssClass="form-control capitalize" runat="server" Enabled="true" placeholder="Địa chỉ khách hàng" autocomplete="off"></asp:TextBox>
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
                                        <br />
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
                                                <th class="quantity-item">Kho</th>
                                                <th class="quantity-item">Số lượng</th>
                                                <th class="total-item">Thành tiền</th>
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
                                    <div class="right totalDiscount">
                                        <a href="javascript:;" class="btn btn-cal-discount link-btn" onclick="refreshDiscount()"><i class="fa fa-refresh" aria-hidden="true"></i>Tính lại</a>
                                        <telerik:RadNumericTextBox runat="server" CssClass="form-control width-notfull input-discount" Skin="MetroTouch"
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
                                        <a class="btn btn-feeship link-btn btn-green hide" href="javascript:;" id="getShipGHTK" onclick="getShipGHTK()"><i class="fa fa-check-square-o" aria-hidden="true"></i>Lấy phí GHTK</a>
                                        <a class="btn btn-feeship link-btn" href="javascript:;" id="calfeeship" onclick="calFeeShip()"><i class="fa fa-check-square-o" aria-hidden="true"></i>Miễn phí</a>
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
                                        <a id="btnGenerateCouponG25" class="btn btn-coupon btn-violet" title="Kiểm tra mã giảm giá G25" onclick="couponG25()"><i class="fa fa-gift"></i>G25</a>
                                        <a id="btnOpenCouponModal" class="btn btn-coupon btn-violet" title="Nhập mã giảm giá" onclick="openCouponModal()"><i class="fa fa-gift"></i>Nhập mã</a>
                                        <a href="javascript:;" id="btnRemoveCouponCode" class="btn btn-coupon link-btn hide" onclick="removeCoupon()"><i class="fa fa-times" aria-hidden="true"></i>Xóa</a>
                                        <asp:TextBox ID="txtCouponValue" runat="server" CssClass="form-control text-right width-notfull input-coupon" value="0" disabled="disabled"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left"><strong>TỔNG TIỀN</strong> (đơn hàng <strong>
                                        <asp:Literal ID="ltrOrderID" runat="server"></asp:Literal></strong>)</div>
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
                                        <a href="javascript:;" class="find3 hide btn btn-return-order link-btn btn-edit-fee" onclick="searchReturnOrder()"><i class="fa fa-refresh" aria-hidden="true"></i>Chọn đơn khác</a>
                                        <a href="javascript:;" class="find3 hide btn btn-feeship link-btn" onclick="deleteReturnOrder()"><i class="fa fa-times" aria-hidden="true"></i>Xóa</a>
                                        <span class="totalpriceorderrefund">
                                            <asp:Literal runat="server" ID="ltrTotalPriceRefund"></asp:Literal></span>
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
                                            ID="txtWeight" MinValue="0" Value="0" NumberFormat-DecimalDigits="1" IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
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
                                        <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control" onchange="onchangePaymentType($(this))">
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
                                        <asp:DropDownList ID="ddlShippingType" runat="server" CssClass="form-control shipping-type">
                                            <asp:ListItem Value="1" Text="Lấy trực tiếp"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Bưu điện"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Proship"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Chuyển xe"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Nhân viên giao"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="GHTK"></asp:ListItem>
                                            <asp:ListItem Value="7" Text="Viettel"></asp:ListItem>
                                            <asp:ListItem Value="8" Text="Grab"></asp:ListItem>
                                            <asp:ListItem Value="9" Text="AhaMove"></asp:ListItem>
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
                                        <a href="javascript:;" class="btn link-btn" id="payall" style="background-color: #f87703; float: right" title="Tạo đơn hàng" onclick="payAll()"><i class="fa fa-floppy-o"></i>Tạo đơn hàng</a>
                                        <asp:Button ID="btnOrder" runat="server" OnClick="btnOrder_Click" Style="display: none" />
                                    </div>
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
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #f87703; float: right" title="Tạo đơn hàng" onclick="payAll()"><i class="fa fa-floppy-o"></i>Tạo đơn hàng</a>
                                </div>
                            </div>
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
        <script type="text/javascript" src="/App_Themes/Ann/js/search-customer.js?v=02112020"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/pages/danh-sach-khach-hang/generate-coupon-for-customer.js?v=02112020"></script>

        <script type="text/javascript">
        </script>
    </telerik:RadScriptBlock>
</asp:Content>
