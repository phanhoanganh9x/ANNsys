<%@ Page Title="Thông tin đơn hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="thong-tin-don-hang.aspx.cs" Inherits="IM_PJ.thong_tin_don_hang" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/App_Themes/Ann/js/search-customer.js?v=2115"></script>
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
                                <h3 class="page-title left not-margin-bot"><asp:Literal ID="ltrHeading" runat="server"></asp:Literal></h3>
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
                                                        <span class="link-facebook"><asp:Literal ID="ltrFb" runat="server"></asp:Literal></span>
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
                            <div class="post-row clear otherfee hide">
                                <div class="left"><span class="otherfee-name"><asp:Literal ID="ltrOtherFeeName" runat="server"></asp:Literal></span><a href="javascript:;" style="text-decoration: underline; float: right; font-size: 12px; font-style: italic; padding-left: 10px;" onclick="removeOtherFee()">(Xóa)</a></div>
                                <div class="right otherfee-value">
                                    <asp:TextBox ID="txtOtherFeeName" CssClass="form-control" runat="server" Style="display: none" ></asp:TextBox>
                                    <telerik:RadNumericTextBox runat="server" CssClass="form-control width-notfull" Skin="MetroTouch"
                                        ID="pOtherFee" NumberFormat-GroupSizes="3" Value="0" NumberFormat-DecimalDigits="0"
                                        oninput="countTotal()" IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
                                    </telerik:RadNumericTextBox>
                                </div>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Tổng tiền</div>
                                <div class="right totalpriceorderall price-red">
                                    <asp:Literal ID="ltrTotalprice" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="post-row clear returnorder hide">
                                <div class="left">Đơn hàng trả
                                    <a href="javascript:;" class="find3 hide" style="text-decoration: underline; float: right; font-size: 12px; font-style: italic; padding-left: 10px;" onclick="searchReturnOrder()">(Tìm đơn khác)</a>
                                    <a href="javascript:;" class="find3 hide" style="text-decoration: underline; float: right; font-size: 12px; font-style: italic; padding-left: 10px;" onclick="deleteReturnOrder()">(Bỏ qua)</a>
                                    <a href="javascript:;" class="find2 hide" style="text-decoration: underline; float: right; font-size: 12px; font-style: italic; padding-left: 10px;"></a>
                                </div>
                                <div class="right totalpriceorderrefund"><asp:Literal runat="server" ID="ltrTotalPriceRefund"></asp:Literal></div>
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
                                    <asp:UpdatePanel ID="up" runat="server">
                                        <ContentTemplate>
                                            <div class="form-row">
                                                <div class="row-left">
                                                    Chành xe
                                                </div>
                                                <div class="row-right">
                                                    <asp:DropDownList ID="ddlTransportCompanyID" DataTextField="TransportCompanyID" DataValueField="ID" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddlTransportCompanyID_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="row-left">
                                                    Nơi nhận
                                                </div>
                                                <div class="row-right">
                                                    <asp:DropDownList ID="ddlTransportCompanySubID" DataTextField="TransportCompanySubID" DataValueField="ID" AppendDataBoundItems="True" AutoPostBack="false" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
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
                                        <a href="javascript:;" class="btn link-btn" style="background-color: #607D8B; float: right;" title="Thêm phí khác vào đơn hàng" onclick="addOtherFee()"><i class="fa fa-plus"></i> Thêm phí khác</a>
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
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #607D8B; float: right;" title="Thêm phí khác vào đơn hàng" onclick="addOtherFee()"><i class="fa fa-plus"></i> Thêm phí khác</a>
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

            // orders detail remove
            var listOrderDetail = [];

            // order of item list
            var orderItem = $(".product-result").length;

            // search Product by SKU
            $("#txtSearch").keydown(function(event) {
                if (event.which === 13) {
                    searchProduct();
                    event.preventDefault();
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
                    swal("Thông báo", "Đã chọn miễn phí vận chuyển cho đơn hàng này", "success");
                    getAllPrice();
                    $("#calfeeship").html("<i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Tính phí").css("background-color", "#f87703");
                }
        
            }

            function addOtherFee() {
                swal({
                    title: "Thêm phí khác",
                    text: 'Nhập tên loại phí. Ghi cho rõ nha:',
                    type: 'input',
                    showCancelButton: true,
                    closeOnConfirm: false,
                    cancelButtonText: "Đợi em tí!",
                    confirmButtonText: "Tiếp đê..",
                }, function (otherFeeName) {
                    swal({
                        title: "Thêm phí khác",
                        text: 'Nhập số tiền nè. Nhập số âm nếu trừ phí:',
                        type: 'input',
                        showCancelButton: true,
                        closeOnConfirm: true,
                        cancelButtonText: "Để em xem lại!",
                        confirmButtonText: "OK nè..",
                    }, function (otherFeeValue) {
                        if (otherFeeValue != false) {
                            if (!isNaN(otherFeeValue)) {
                                $(".otherfee-name").html(otherFeeName);
                                $("#<%=txtOtherFeeName.ClientID%>").val(otherFeeName);
                                $("#<%=pOtherFee.ClientID%>").val(otherFeeValue).focus();
                                $(".otherfee").removeClass("hide");

                                getAllPrice();
                            }
                            else {
                                alert("Sai rồi! Phải nhập phí khác là số (ví dụ: 10000 hoặc -10000)");
                            }
                        }
                    });
                });
            }

            function removeOtherFee() {

                $(".otherfee").addClass("hide");
                $(".otherfee-name").html("");
                $("#<%=txtOtherFeeName.ClientID%>").val("");
                $("#<%=pOtherFee.ClientID%>").val(0).focus();
                countTotal();
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

            $(document).ready(function () {

                if ($("#<%=pOtherFee.ClientID%>").val() != 0) {
                    $(".otherfee").removeClass("hide");
                }

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
                $(".find2").html("(Xem đơn hàng " + t[0] + ")");
                $(".find2").attr("onclick", "viewReturnOrder(" + t[0] + ")");
                $(".find2").removeClass("hide");
                $(".returnorder").removeClass("hide");
                $(".totalpriceorderall").removeClass("price-red");
                $(".totalpricedetail").addClass("price-red");
            }

            // search return order
            function searchReturnOrder() {
                var phone = $("#<%=txtPhone.ClientID%>").val();
                var name = $("#<%=txtFullname.ClientID%>").val();
                if (isBlank(phone) || isBlank(name)) {
                    swal("Thông báo", "Hãy nhập thông tin khách hàng trước!", "info");
                } else {
                    var html = "";
                    html += "<div class=\"form-group\">";
                    html += "<label>Mã đơn hàng đổi trả: </label>";
                    html += "<input ID=\"txtOrderRefund\" class=\"form-control fjx\"></input>";
                    html += "<a href=\"javascript: ;\" class=\"btn link- btn\" style=\"background-color:#f87703;float:right;color:#fff;\" onclick=\"getReturnOrder()\">Tìm</a>";
                    html += "</div>";
                    showPopup(html);
                    $("#txtOrderRefund").focus();
                    $('#txtOrderRefund').keydown(function(event) {
                        if (event.which === 13) {
                            getReturnOrder();
                            event.preventDefault();
                            return false;
                        }
                    });
                }
            }

            // get return order
            function getReturnOrder() {
                var order = $("#txtOrderRefund").val();
                var name = $("#<%=txtFullname.ClientID%>").val();
                var phone = $("#<%=txtPhone.ClientID%>").val();
                var t = document.getElementById('<%= hdfcheckR.ClientID%>').defaultValue;
                var existorder = t.split(',');
                if (!isBlank(order)) {
                    $.ajax({
                        type: "POST",
                        url: "/thong-tin-don-hang.aspx/findReturnOrder",
                        data: "{order:'" + order + "', remove:'0'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function(msg) {
                            if (msg.d != "null") {
                                var data = JSON.parse(msg.d);
                                $("#<%=hdSession.ClientID%>").val(data.ID + "|" + data.TotalPrice);
                                if (data.CustomerName == name && data.CustomerPhone == phone) {
                                    $(".subtotal").removeClass("hide");
                                    $(".returnorder").removeClass("hide");
                                    $(".totalpriceorderall").removeClass("price-red");
                                    $(".totalpricedetail").addClass("price-red");
                                    $(".find3").removeClass("hide");
                                    $(".find1").addClass("hide");
                                    $(".find2").html("(Xem đơn hàng " + data.ID + ")");
                                    $(".find2").attr("onclick", "viewReturnOrder(" + data.ID + ")");
                                    $(".find2").removeClass("hide");
                                    var refundprice = 0;
                                    if (parseFloat($("#<%=hdfTotalPrice.ClientID%>").val() > 0)) {
                                        refundprice = parseFloat($("#<%=hdfTotalPrice.ClientID%>").val());
                                    }
                                    $(".totalpricedetail").html(formatThousands((refundprice - data.TotalPrice), ","));
                                    $("#<%=hdfDonHangTra.ClientID%>").val(data.TotalPrice);
                                    $(".refund").removeClass("hide");
                                    $(".totalpriceorderrefund").html(formatThousands(data.TotalPrice, ","));
                                    closePopup();
                                    getAllPrice();
                                } else {
                                    swal("Thông báo", "Đơn hàng đổi trả không thuộc khách hàng này!", "error");
                                }
                            } else if (order == existorder[0]) {
                                $("#<%=hdSession.ClientID%>").val(0);
                                swal("Thông báo", "Đơn hàng đổi trả này đã thêm vào trước đó!\nHãy đổi về trạng thái 'Chưa trừ tiền' để thêm lại lần nữa!\nSau khi đổi trạng thái, trở lại giao diện này để thêm lại!", "error");
                                viewReturnOrder(order);
                            } else {
                                swal("Thông báo", "Đơn hàng đổi trả không tồn tại hoặc đã được trừ tiền!", "error");
                            }
                        },
                        error: function(xmlhttprequest, textstatus, errorthrow) {
                            alert('lỗi');
                        }
                    });
                } else {
                    swal("Thông báo", "Hãy nhập thông tin khách hàng trước!", "info");
                }
            }

            // view return order by click button
            function viewReturnOrder(ID) {
                var win = window.open("/thong-tin-tra-hang?id=" + ID + "", '_blank');
                win.focus();
            }

            // delete return order
            function deleteReturnOrder() {
                $.ajax({
                    type: "POST",
                    url: "/thong-tin-don-hang.aspx/findReturnOrder",
                    data: "{order:'0', remove:'1'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(msg) {
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
                    },
                    error: function(xmlhttprequest, textstatus, errorthrow) {
                        alert('lỗi');
                    }
                });
            }

            // pay order on click button
            function payAll() {

                var phone = $("#<%=txtPhone.ClientID%>").val();
                var name = $("#<%= txtFullname.ClientID%>").val();
                var nick = $("#<%= txtNick.ClientID%>").val();
                var address = $("#<%= txtAddress.ClientID%>").val();
                if (phone != "" && name != "" && nick != "" && address != "") {
                    
                    // Nếu có sản phẩm trong đơn hàng
                    if ($(".product-result").length > 0) {
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

                            if (quantity > 0) {
                                if (quantity > quantityInstock) {
                                    checkoutin = true;
                                }

                                list += id + "," + sku + "," + producttype + "," + productvariablename + "," + productvariablevalue + "," + quantity + "," +
                                    productname + "," + productimageorigin + "," + productvariablesave + "," + price + "," + productvariablesave + "," +
                                    orderDetailID + "," + productvariableid + ";";
                            }
                        });

                        // Kiểm tra trạng thái xử lý
                        let excuteStatus = Number($("#<%=ddlExcuteStatus.ClientID%>").val());

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
                            // Khôi phục đơn hàng đã hủy
                        else if (excuteStatus != 3 && $("#<%=hdfExcuteStatus.ClientID%>").val() == 3 ) {

                            // Chỉ admin mới được khôi phục đơn hàng hủy
                            if ($("#<%=hdfRoleID.ClientID%>").val() == 0) {

                                $("#<%=txtOrderNote.ClientID %>").val("Đã khôi phục từ trạng thái hủy bởi " + $("#<%=hdfUsernameCurrent.ClientID%>").val());
                                deleteOrder();
                                $("#<%=hdfOrderType.ClientID %>").val(ordertype);
                                $("#<%=hdfListProduct.ClientID%>").val(list);
                                insertOrder();
                            }
                            else {
                                swal("Không thể khôi phục đơn hàng đã hủy!", "Hãy báo cáo chị Ngọc để khôi phục", "error");
                            }
                        }
                            // Đổi trạng thái Đã hoàn tất sang trạng thái Đang xử lý
                        else if (excuteStatus == 1 && $("#<%=hdfExcuteStatus.ClientID%>").val() == 2) {

                            // Chỉ admin mới được đổi trạng thái Đã hoàn tất sang trạng thái Đang xử lý
                            if ($("#<%=hdfRoleID.ClientID%>").val() == 0) {

                                $("#<%=txtOrderNote.ClientID %>").val("Đã đổi trạng thái từ Đã hoàn tất sang Đang xử lý bởi " + $("#<%=hdfUsernameCurrent.ClientID%>").val());
                                deleteOrder();
                                $("#<%=hdfOrderType.ClientID %>").val(ordertype);
                                $("#<%=hdfListProduct.ClientID%>").val(list);
                                insertOrder();
                            }
                            else {
                                swal("Không thể đổi trạng thái từ Đã hoàn tất sang Đang xử lý!", "Hãy báo cáo chị Ngọc để khôi phục", "error");
                            }
                        }
                            // Nếu trạng thái không liên quan đến hủy thì xử lý..
                        else {
                            deleteOrder(); 
                            $("#<%=hdfOrderType.ClientID %>").val(ordertype);
                            $("#<%=hdfListProduct.ClientID%>").val(list);
                            insertOrder();
                        }
                    }
                        // Nếu không có sản phẩm trong đơn
                    else {
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
                                        else {
                                            swal("Hủy đơn hàng thất bại!", "Chưa nhập lý do nên không được hủy", "error");
                                        }
                                    });
                                }
                            });

                        } else {
                            $("#txtSearch").focus();
                            swal("Thông báo", "Hãy nhập sản phẩm!", "error");
                        }
                    }

                } else {
                    if (name == "") {
                        $("#<%= txtFullname.ClientID%>").focus();
                        swal("Thông báo", "Hãy nhập tên khách hàng!", "error");
                    }
                    else if (phone == "") {
                        $("#<%= txtPhone.ClientID%>").focus();
                        swal("Thông báo", "Hãy nhập số điện thoại khách hàng!", "error");
                    }
                    else if (nick == "") {
                        $("#<%= txtNick.ClientID%>").prop('disabled', false);
                        $("#<%= txtNick.ClientID%>").focus();
                        swal("Thông báo", "Hãy nhập Nick đặt hàng của khách hàng!", "error");
                        
                    }
                    else if (address == "") {
                        $("#<%= txtAddress.ClientID%>").focus();
                        swal("Thông báo", "Hãy nhập địa chỉ khách hàng!", "error");
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
            function getAllPrice(is_payAll_call=false) {
                if ($(".product-result").length > 0) {
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

                    var of = $("#<%=pOtherFee.ClientID%>").val();
                    var otherfee = parseFloat(of.replace(/\,/g, ''));

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
                } else {
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
                if ($("#<%=pDiscount.ClientID%>").val() == '') {
                    var dis = 0;
                    $("#<%=pDiscount.ClientID%>").val(formatThousands(dis, ','));
                }
                if ($("#<%=pFeeShip.ClientID%>").val() == '') {
                    var fee = 0;
                    $("#<%=pFeeShip.ClientID%>").val(formatThousands(fee, ','));
                }
                if ($("#<%=pOtherFee.ClientID%>").val() == '') {
                    var fee = 0;
                    $("#<%=pOtherFee.ClientID%>").val(formatThousands(fee, ','));
                }
            }

            // count total order
            function countTotal() {
                var total = parseFloat($("#<%=hdfTotalPriceNotDiscount.ClientID%>").val());
                var quantity = 0;
                if (!isBlank($("#<%=hdfTotalQuantity.ClientID%>").val())) {
                    quantity = parseFloat($("#<%=hdfTotalQuantity.ClientID%>").val());
                }
                notEmpty();
                var dis = $("#<%=pDiscount.ClientID%>").val();
                var fs = $("#<%=pFeeShip.ClientID%>").val();
                var totalproduct = parseFloat($("#<%=hdftotal.ClientID%>").val());

                var discount = parseFloat(dis.replace(/\,/g, ''));
                var feeship = parseFloat(fs.replace(/\,/g, ''));

                var of = $("#<%=pOtherFee.ClientID%>").val();
                var otherfee = parseFloat(of.replace(/\,/g, ''));

                $("#<%=hdfcheck.ClientID%>").val(discount);
                $("#<%=hdfDiscountInOrder.ClientID%>").val(discount);

                if (quantity > 0) {
                    var totalleft = total + feeship + otherfee - discount * quantity;
                    var priceafterchietkhau = total - discount * quantity;
                    $("#<%=hdfTotalQuantity.ClientID%>").val(quantity);
                } else {
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
                if ($(".product-result").length > 0) {
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
        </script>
    </telerik:RadScriptBlock>
</asp:Content>
