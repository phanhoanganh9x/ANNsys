<%@ Page Title="Sản phẩm" Language="C#" MasterPageFile="~/ProductPage.Master" AutoEventWireup="true" CodeBehind="san-pham.aspx.cs" Inherits="IM_PJ.san_pham" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        body {
            background-color: #eee;
        }
        .page-title {
            margin-top: 15px;
        }
        .padding-none {
            padding: 0;
        }
        .margin-right-15px {
            margin-right: 12px!important;
        }
        .main-block {
            margin: auto;
            float: inherit;
        }
        .select2-container .select2-selection--single {
            height: 45px;
        }
        .select2-container--default .select2-selection--single .select2-selection__rendered {
            line-height: 45px;
        }
        .select2-container--default .select2-selection--single .select2-selection__arrow {
            height: 43px;
        }
        .btn {
            margin-bottom: 10px;
            border-radius: 5px;
        }
        .btn.primary-btn {
            background-color: #4bac4d;
        }
        .btn:hover {
            background-color: #000!important;
        }
        .btn-red {
            background-color: #F44336;
        }
        .btn-blue {
            background-color: #008fe5!important;
        }
        .bg-green, .bg-red, .bg-yellow {
            display: inherit;
        }
        table.shop_table_responsive > tbody > tr:nth-of-type(2n+1) td {
            border-bottom: solid 1px #e1e1e1!important;
        }
        input[type="checkbox"] {
            width: 20px;
            height: 20px;
        }
        .bg-green, .bg-red, .bg-yellow {
            display: inherit;
        }
        .table > thead > tr > th {
            background-color: #0090da;
        }
        
        img {
            border-radius: 5px;
        }
        .form-control {
            border-radius: 5px;
            margin-bottom: 15px;
        }
        .select2-container .select2-selection--single {
            border-radius: 5px;
            margin-bottom: 15px;
        }
        .filter-above-wrap {
            margin-bottom: 0;
        }
        .filter-above-wrap .action-button {
            margin-bottom: 15px;
        }
        .pagination li > a, .pagination li > a {
            height: 24px;
            border: transparent;
            line-height: 24px;
            -ms-box-orient: horizontal;
            display: -webkit-box;
            display: -moz-box;
            display: -ms-flexbox;
            display: -moz-flex;
            display: -webkit-flex;
            display: flex;
            align-items: center;
            justify-content: center;
            background-color: transparent;
            z-index: 99;
            width: auto;
            min-width: 24px;
            -webkit-border-radius: 2px;
            -moz-border-radius: 2px;
            border-radius: 2px;
            margin: 0 5px 0 0;
        }
        .pagination li.current {
            background-color: #4bac4d;
            color: #fff;
            height: 24px;
            border: transparent;
            line-height: 24px;
            -ms-box-orient: horizontal;
            align-items: center;
            justify-content: center;
            z-index: 99;
            width: auto;
            min-width: 24px;
            -webkit-border-radius: 2px;
            -moz-border-radius: 2px;
            border-radius: 2px;
            margin: 0 5px 0 0;
            text-align: center;
        }
        .pagination li:hover > a {
            background-color: #e5e5e5;
            color: #000;
        }
        @media (max-width: 768px) {
            .margin-right-15px {
                margin-right: 12px!important;
            }
            .filter-above-wrap .action-button {
                margin-top: 15px;
            }
            table.shop_table_responsive thead {
	            display: none;
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(1):before {
                content: "";
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(1) {
                height: auto;
                padding-bottom: 0;
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(2):before {
                content: "";
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(2) {
                height: auto;
                padding-bottom: 0;
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(3):before {
                content: "";
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(3) {
                height: auto;
                text-align: left;
                padding-bottom: 1px;
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(11):before {
                content: "";
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(11) {
                text-align: left;
            }
            table.shop_table_responsive > tbody > tr:nth-of-type(2n) td {
                border-top: none;
                border-bottom: none!important;
            }
            table.shop_table_responsive > tbody > tr:nth-of-type(2n+1) td {
                border-bottom: none!important;
                background-color: #fff;
            }
            table.shop_table_responsive > tbody > tr > td:first-child {
	            border-left: none;
                padding-left: 20px;
            }
            table.shop_table_responsive > tbody > tr > td:last-child {
	            border-right: none;
                padding-left: 20px;
            }
            table.shop_table_responsive > tbody > tr > td {
	            height: 40px;
            }
            table.shop_table_responsive > tbody > tr > td.customer-td {
	            height: 60px;
            }
            table.shop_table_responsive > tbody > tr > td.payment-type, table.shop_table_responsive > tbody > tr > td.shipping-type {
                height: 70px;
            }
            table.shop_table_responsive > tbody > tr > td .new-status-btn {
                display: block;
                margin-top: 10px;
            }
            table.shop_table_responsive > tbody > tr > td.update-button {
                height: 130px;
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
	            content: attr(data-title) ": ";
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
    <main>
        <div class="col-md-12 main-block">
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title left">Sản phẩm <span>(<asp:Literal ID="ltrNumberOfProduct" runat="server" EnableViewState="false"></asp:Literal>)</span></h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-4 col-xs-12">
                                    <asp:TextBox ID="txtSearchProduct" runat="server" CssClass="form-control" placeholder="Tìm sản phẩm" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlOrderBy" runat="server" CssClass="form-control" Width="100%">
                                        <asp:ListItem Value="" Text="Sắp xếp"></asp:ListItem>
                                        <asp:ListItem Value="latestOnApp" Text="Mới nhất trên app"></asp:ListItem>
                                        <asp:ListItem Value="latestOnSystem" Text="Mới nhất trên hệ thống"></asp:ListItem>
                                        <asp:ListItem Value="stockDesc" Text="Kho giảm dần"></asp:ListItem>
                                        <asp:ListItem Value="stockAsc" Text="Kho tăng dần"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlStockStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Trạng thái kho"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Còn hàng"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Hết hàng"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Nhập hàng"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlQuantityFilter" runat="server" CssClass="form-control" onchange="changeQuantityFilter($(this))">
                                        <asp:ListItem Value="" Text="Số lượng kho"></asp:ListItem>
                                        <asp:ListItem Value="greaterthan" Text="Số lượng lớn hơn"></asp:ListItem>
                                        <asp:ListItem Value="lessthan" Text="Số lượng nhỏ hơn"></asp:ListItem>
                                        <asp:ListItem Value="between" Text="Số lượng trong khoảng"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6 greaterthan lessthan">
                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" placeholder="Số lượng" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-md-2 col-xs-6 between hide">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtQuantityMin" runat="server" CssClass="form-control" placeholder="Min" autocomplete="off"></asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtQuantityMax" runat="server" CssClass="form-control" placeholder="Max" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlTag" runat="server" CssClass="form-control select2" Width="100%"></asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlColor" runat="server" CssClass="form-control select2" Width="100%">
                                        <asp:ListItem Value="" Text="Màu"></asp:ListItem>
                                        <asp:ListItem Value="cam" Text="Cam"></asp:ListItem>
                                        <asp:ListItem Value="cam tươi" Text="Cam tươi"></asp:ListItem>
                                        <asp:ListItem Value="cam đất" Text="Cam đất"></asp:ListItem>
                                        <asp:ListItem Value="cam sữa" Text="Cam sữa"></asp:ListItem>
                                        <asp:ListItem Value="caro" Text="Caro"></asp:ListItem>
                                        <asp:ListItem Value="da bò" Text="Da bò"></asp:ListItem>
                                        <asp:ListItem Value="đen" Text="Đen"></asp:ListItem>
                                        <asp:ListItem Value="đỏ" Text="Đỏ"></asp:ListItem>
                                        <asp:ListItem Value="đỏ đô" Text="Đỏ đô"></asp:ListItem>
                                        <asp:ListItem Value="đỏ tươi" Text="Đỏ tươi"></asp:ListItem>
                                        <asp:ListItem Value="dưa cải" Text="Dưa cải"></asp:ListItem>
                                        <asp:ListItem Value="gạch tôm" Text="Gạch tôm"></asp:ListItem>
                                        <asp:ListItem Value="hồng" Text="Hồng"></asp:ListItem>
                                        <asp:ListItem Value="hồng cam" Text="Hồng cam"></asp:ListItem>
                                        <asp:ListItem Value="hồng da" Text="Hồng da"></asp:ListItem>
                                        <asp:ListItem Value="hồng dâu" Text="Hồng dâu"></asp:ListItem>
                                        <asp:ListItem Value="hồng phấn" Text="Hồng phấn"></asp:ListItem>
                                        <asp:ListItem Value="hồng ruốc" Text="Hồng ruốc"></asp:ListItem>
                                        <asp:ListItem Value="hồng sen" Text="Hồng sen"></asp:ListItem>
                                        <asp:ListItem Value="kem" Text="Kem"></asp:ListItem>
                                        <asp:ListItem Value="kem tươi" Text="Kem tươi"></asp:ListItem>
                                        <asp:ListItem Value="kem đậm" Text="Kem đậm"></asp:ListItem>
                                        <asp:ListItem Value="kem nhạt" Text="Kem nhạt"></asp:ListItem>
                                        <asp:ListItem Value="nâu" Text="Nâu"></asp:ListItem>
                                        <asp:ListItem Value="nho" Text="Nho"></asp:ListItem>
                                        <asp:ListItem Value="rạch tôm" Text="Rạch tôm"></asp:ListItem>
                                        <asp:ListItem Value="sọc" Text="Sọc"></asp:ListItem>
                                        <asp:ListItem Value="tím" Text="Tím"></asp:ListItem>
                                        <asp:ListItem Value="tím cà" Text="Tím cà"></asp:ListItem>
                                        <asp:ListItem Value="tím đậm" Text="Tím đậm"></asp:ListItem>
                                        <asp:ListItem Value="tím xiêm" Text="Tím xiêm"></asp:ListItem>
                                        <asp:ListItem Value="trắng" Text="Trắng"></asp:ListItem>
                                        <asp:ListItem Value="trắng-đen" Text="Trắng-đen"></asp:ListItem>
                                        <asp:ListItem Value="trắng-đỏ" Text="Trắng-đỏ"></asp:ListItem>
                                        <asp:ListItem Value="trắng-xanh" Text="Trắng-xanh"></asp:ListItem>
                                        <asp:ListItem Value="vàng" Text="Vàng"></asp:ListItem>
                                        <asp:ListItem Value="vàng tươi" Text="Vàng tươi"></asp:ListItem>
                                        <asp:ListItem Value="vàng bò" Text="Vàng bò"></asp:ListItem>
                                        <asp:ListItem Value="vàng nghệ" Text="Vàng nghệ"></asp:ListItem>
                                        <asp:ListItem Value="vàng nhạt" Text="Vàng nhạt"></asp:ListItem>
                                        <asp:ListItem Value="xanh vỏ đậu" Text="Xanh vỏ đậu"></asp:ListItem>
                                        <asp:ListItem Value="xám" Text="Xám"></asp:ListItem>
                                        <asp:ListItem Value="xám chì" Text="Xám chì"></asp:ListItem>
                                        <asp:ListItem Value="xám chuột" Text="Xám chuột"></asp:ListItem>
                                        <asp:ListItem Value="xám nhạt" Text="Xám nhạt"></asp:ListItem>
                                        <asp:ListItem Value="xám tiêu" Text="Xám tiêu"></asp:ListItem>
                                        <asp:ListItem Value="xám xanh" Text="Xám xanh"></asp:ListItem>
                                        <asp:ListItem Value="xanh biển" Text="Xanh biển"></asp:ListItem>
                                        <asp:ListItem Value="xanh biển đậm" Text="Xanh biển đậm"></asp:ListItem>
                                        <asp:ListItem Value="xanh lá chuối" Text="Xanh lá chuối"></asp:ListItem>
                                        <asp:ListItem Value="xanh cổ vịt" Text="Xanh cổ vịt"></asp:ListItem>
                                        <asp:ListItem Value="xanh coban" Text="Xanh coban"></asp:ListItem>
                                        <asp:ListItem Value="xanh da" Text="Xanh da"></asp:ListItem>
                                        <asp:ListItem Value="xanh dạ quang" Text="Xanh dạ quang"></asp:ListItem>
                                        <asp:ListItem Value="xanh đen" Text="Xanh đen"></asp:ListItem>
                                        <asp:ListItem Value="xanh jean" Text="Xanh jean"></asp:ListItem>
                                        <asp:ListItem Value="xanh lá" Text="Xanh lá"></asp:ListItem>
                                        <asp:ListItem Value="xanh lá mạ" Text="Xanh lá mạ"></asp:ListItem>
                                        <asp:ListItem Value="xanh lính" Text="Xanh lính"></asp:ListItem>
                                        <asp:ListItem Value="xanh lông công" Text="Xanh lông công"></asp:ListItem>
                                        <asp:ListItem Value="xanh môn" Text="Xanh môn"></asp:ListItem>
                                        <asp:ListItem Value="xanh ngọc" Text="Xanh ngọc"></asp:ListItem>
                                        <asp:ListItem Value="xanh rêu" Text="Xanh rêu"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlSize" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Size"></asp:ListItem>
                                        <asp:ListItem Value="s" Text="Size S"></asp:ListItem>
                                        <asp:ListItem Value="m" Text="Size M"></asp:ListItem>
                                        <asp:ListItem Value="l" Text="Size L"></asp:ListItem>
                                        <asp:ListItem Value="xl" Text="Size XL"></asp:ListItem>
                                        <asp:ListItem Value="xxl" Text="Size XXL"></asp:ListItem>
                                        <asp:ListItem Value="xxxl" Text="Size XXXL"></asp:ListItem>
                                        <asp:ListItem Value="27" Text="Size 27"></asp:ListItem>
                                        <asp:ListItem Value="28" Text="Size 28"></asp:ListItem>
                                        <asp:ListItem Value="29" Text="Size 29"></asp:ListItem>
                                        <asp:ListItem Value="30" Text="Size 30"></asp:ListItem>
                                        <asp:ListItem Value="31" Text="Size 31"></asp:ListItem>
                                        <asp:ListItem Value="32" Text="Size 32"></asp:ListItem>
                                        <asp:ListItem Value="33" Text="Size 33"></asp:ListItem>
                                        <asp:ListItem Value="34" Text="Size 34"></asp:ListItem>
                                        <asp:ListItem Value="35" Text="Size 35"></asp:ListItem>
                                        <asp:ListItem Value="36" Text="Size 36"></asp:ListItem>
                                        <asp:ListItem Value="37" Text="Size 37"></asp:ListItem>
                                        <asp:ListItem Value="38" Text="Size 38"></asp:ListItem>
                                        <asp:ListItem Value="39" Text="Size 39"></asp:ListItem>
                                        <asp:ListItem Value="40" Text="Size 40"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <label>Từ ngày</label>
                                    <telerik:RadDatePicker RenderMode="Lightweight" ID="rFromDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate">
                                        <DateInput DisplayDateFormat="dd/MM/yyyy" runat="server">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <label>Đến ngày</label>
                                    <telerik:RadDatePicker RenderMode="Lightweight" ID="rToDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate">
                                        <DateInput DisplayDateFormat="dd/MM/yyyy" runat="server">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="filter-above-wrap clear">
                        <div class="filter-control action-button">
                            <div class="row">
                                <div class="col-md-12 col-xs-12">
                                    <a href="javascript:;" onclick="searchProduct()" class="btn primary-btn margin-right-15px"><i class="fa fa-search"></i> Lọc</a>
                                    <a href="/" class="btn primary-btn margin-right-15px"><i class="fa fa-times" aria-hidden="true"></i> Bỏ lọc</a>
                                    <a href="javascript:;" onclick="postALLProductZaloShop()" class="btn-action btn primary-btn margin-right-15px" disabled="disabled" readonly>
                                        <i class="fa fa-arrow-up"></i> Zalo
                                    </a>
                                    <a href="javascript:;" onclick="downloadAllZaloShop()" class="btn-action btn primary-btn btn-blue margin-right-15px" disabled="disabled" readonly>
                                        <i class="fa fa-download"></i> Zalo
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <h5>KiotViet</h5>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 padding-none">
                    <div class="filter-above-wrap clear">
                        <div class="col-md-3 col-xs-12">
                            <asp:DropDownList ID="ddlRetailer" runat="server" CssClass="form-control" style="background-color: #fff">
                                <asp:ListItem Value="" Text="Tên nhà bán lẻ"></asp:ListItem>
                                <asp:ListItem Value="giaminhwill" Text="TK tính tiền"></asp:ListItem>
                                <asp:ListItem Value="iwillgiaminh" Text="TK Web"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-9 col-xs-12">
                            <a href="javascript:;" id="btnSyncKvCategory" onclick="syncKvProductByCategory()" class="btn primary-btn margin-right-15px hidden" title="Đăng tất cả các sản phẩm theo danh mục">
                                <i class="fa fa-arrow-up"></i> Danh mục
                            </a>
                            <%--<a href="javascript:;" id="btnKvCategoryObserve" onclick="registerKvCategoryObservation()" class="btn primary-btn margin-right-15px hidden" title="Đăng khi có sản phẩm mới">
                                <i class="fa fa-eye"></i> Danh mục
                            </a>
                            <a href="javascript:;" id="btnKvCategoryUnObserve" onclick="deleteKvCategoryObservation()" class="btn primary-btn margin-right-15px hidden" title="Không đăng sản phẩm mới">
                                <i class="fa fa-eye-slash"></i> Danh mục
                            </a>--%>
                            <a href="javascript:;" id="btnSyncAllKvProduct" onclick="syncKvProducts()" class="btn primary-btn margin-right-15px hidden" title="Đăng sản phẩm đã chọn">
                                <i class="fa fa-arrow-up"></i> Sản phẩm
                            </a>
                            <%--<a href="javascript:;" id="btnDeleteAllKvProduct" onclick="deleteKvProducts()" class="btn primary-btn btn-red margin-right-15px hidden" title="Xóa sản phẩm đã chọn">
                                <i class="fa fa-times"></i> Sản phẩm
                            </a>--%>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel-table clear">
                        <div class="panel-footer clear">
                            <div class="pagination">
                                <%this.DisplayHtmlStringPaging1();%>
                            </div>
                        </div>
                        <div class="responsive-table">
                            <table class="table table-checkable table-product all-product-table-2 shop_table_responsive">
                                <asp:Literal ID="ltrList" runat="server" EnableViewState="false"></asp:Literal>
                            </table>
                        </div>
                        <div class="panel-footer clear">
                            <div class="pagination">
                                <%this.DisplayHtmlStringPaging1();%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <script src="/App_Themes/Ann/js/copy-product-info-public.js?v=11072020"></script>
        <script src="/App_Themes/Ann/js/download-product-image-public.js?v=11072020"></script>
        <script src="/App_Themes/Ann/js/services/common/product-service.js?v=11072020"></script>
        
        <script type="text/javascript">
            let observeKvCategory = false;
            let productSync = [];
            let productRemoval = [];

            $(document).ready(function() {
                $("#<%=txtSearchProduct.ClientID%>").keyup(function (e) {
                    if (e.keyCode == 13)
                    {
                        searchProduct();
                    }
                });

                $("td>input[type='checkbox']").change(function () {
                    let btnAction = $(".btn-action");

                    if (this.checked == true) {
                        btnAction.removeAttr("disabled");
                        btnAction.removeAttr("readonly");
                    }
                    else {
                        let $tdChecked = $("td>input[type='checkbox']:checked");

                        if ($tdChecked.length == 0) {
                            btnAction.attr("disabled", true);
                            btnAction.attr("readonly", true);
                        }

                    }
                })

                // Danh mục
                _initCategory();

                // KiotViet
                _initKv();
            })

            function _initCategory() {
                let $ddlCategory = $("#<%=ddlCategory.ClientID%>");

                $ddlCategory.change(function () {
                    let retailerName = $("#<%=ddlRetailer.ClientID%>").val();

                    _handleBtnKvCategory(retailerName, $(this).val());
                });
            }

            // Cài đặt KiotViet
            function _initKv() {
                let $ddlCategory = $("#<%=ddlCategory.ClientID%>");
                let $ddlRetailer = $("#<%=ddlRetailer.ClientID%>");

                // Header
                _handleBtnKvCategory($ddlRetailer.val(), $ddlCategory.val());
                _handleBtnKvProduct($ddlRetailer.val());
                // Row
                _handleBtnKvRow($ddlRetailer.val());

                $ddlRetailer.change(function () {
                    searchProduct();
                });
            }

            // Xử lý ẩn hiện button KiotVet theo dõi danh mục 
            function _handleBtnKvCategory(retailerName, categoryId) {
                let $btnSyncKvCategory = $("#btnSyncKvCategory");
                //let $btnKvCategoryObserve = $("#btnKvCategoryObserve");
                //let $btnKvCategoryUnObserve = $("#btnKvCategoryUnObserve");

                if (!retailerName || categoryId == "0") {
                    observeKvCategory = false;
                    // Sync Category
                    $btnSyncKvCategory.addClass("hidden");
                    // Observer Category
                    //$btnKvCategoryObserve.addClass("hidden");
                    // UnObserver Category
                    //$btnKvCategoryUnObserve.addClass("hidden");

                    return;
                }

                // Sync Category
                $btnSyncKvCategory.removeClass("hidden");

                $.ajax({
                    url: "api/v1/kiotviet/category/ann-shop/" + categoryId,
                    headers: {
                        "Authorization": "Basic " + btoa("anhtruyen:0979610642"),
                        "retailerName": retailerName
                    },
                    method: 'GET',
                    success: (response, textStatus, xhr) => {
                        let category = response;

                        if (category && category.cronJob) {
                            observeKvCategory = true;
                            // Observer Category
                            //$btnKvCategoryObserve.addClass("hidden");
                            // UnObserver Category
                            //$btnKvCategoryUnObserve.removeClass("hidden");
                        }
                        else {
                            observeKvCategory = false;
                            // Observer Category
                            //$btnKvCategoryObserve.removeClass("hidden");
                            // UnObserver Category
                            //$btnKvCategoryUnObserve.addClass("hidden");
                        }
                    }
                });
            }

            function _handleBtnKvProduct(retailerName) {
                let $btnSyncAllKvProduct = $("#btnSyncAllKvProduct");
                //let $btnDeleteAllKvProduct = $("#btnDeleteAllKvProduct");

                if (retailerName) {
                    if (productSync.length > 0 && productRemoval.length > 0) {
                        // Sync Product
                        $btnSyncAllKvProduct.addClass("hidden");
                        $btnSyncAllKvProduct.attr("disabled", true);
                        $btnSyncAllKvProduct.attr("readonly", true);
                        // Delete Product
                        //$btnDeleteAllKvProduct.addClass("hidden");
                        //$btnDeleteAllKvProduct.attr("disabled", true);
                        //$btnDeleteAllKvProduct.attr("readonly", true);
                    }
                    else if (productSync.length > 0 && productRemoval.length == 0) {
                        // Sync Product
                        $btnSyncAllKvProduct.removeClass("hidden");
                        $btnSyncAllKvProduct.attr("disabled", false);
                        $btnSyncAllKvProduct.attr("readonly", false);
                        // Delete Product
                        //$btnDeleteAllKvProduct.addClass("hidden");
                        //$btnDeleteAllKvProduct.attr("disabled", true);
                        //$btnDeleteAllKvProduct.attr("readonly", true);
                    }
                    else if (productSync.length == 0 && productRemoval.length > 0) {
                        // Sync Product
                        $btnSyncAllKvProduct.addClass("hidden");
                        $btnSyncAllKvProduct.attr("disabled", true);
                        $btnSyncAllKvProduct.attr("readonly", true);
                        // Delete Product
                        //$btnDeleteAllKvProduct.removeClass("hidden");
                        //$btnDeleteAllKvProduct.attr("disabled", false);
                        //$btnDeleteAllKvProduct.attr("readonly", false);
                    }
                    else {
                        // Sync Product
                        $btnSyncAllKvProduct.addClass("hidden");
                        $btnSyncAllKvProduct.attr("disabled", true);
                        $btnSyncAllKvProduct.attr("readonly", true);
                        // Delete Product
                        //$btnDeleteAllKvProduct.addClass("hidden");
                        //$btnDeleteAllKvProduct.attr("disabled", true);
                        //$btnDeleteAllKvProduct.attr("readonly", true);
                    }
                }
                else {
                    // Sync Product
                    $btnSyncAllKvProduct.addClass("hidden");
                    $btnSyncAllKvProduct.attr("disabled", true);
                    $btnSyncAllKvProduct.attr("readonly", true);
                    // Delete Product
                    //$btnDeleteAllKvProduct.addClass("hidden");
                    //$btnDeleteAllKvProduct.attr("disabled", true);
                    //$btnDeleteAllKvProduct.attr("readonly", true);
                }
            }

            // Xử lý ẩn hiện các button KiotViet trên mỗi dòng
            function _handleBtnKvRow(retailerName)
            {
                let $trProducts = $(".tr-product");
                let productIdList = "";

                $.each($trProducts, function (index, element) {
                    $(this).data("existsKv", false);
                    $(this).data("observeKvProduct", false);

                    if (index == 0)
                        productIdList += $(this).data("productid");
                    else
                        productIdList += "," + $(this).data("productid");
                });

                if (!productIdList)
                    return;

                if (!retailerName)
                {
                    $.each($trProducts, function (index, element) {
                        let $btnKvSyncProduct = $(this).find(".btn-sync-kv-product");
                        let $btnKvDeleteProduct = $(this).find(".btn-delete-kv-product");
                        let $btnKvObserverProduct = $(this).find(".btn-kv-product-observation");
                        let $btnKvUnObserverProduct = $(this).find(".btn-kv-product-unobserver");

                        $(this).data("existsKv", false);
                        $(this).data("observeKvProduct", false);
                        $btnKvSyncProduct.addClass("hidden");
                        $btnKvDeleteProduct.addClass("hidden");
                        $btnKvObserverProduct.addClass("hidden");
                        $btnKvUnObserverProduct.addClass("hidden");
                    });

                    return;
                }

                $.ajax({
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    url: "/api/v1/kiotviet/product?isMaster=true&referenceProductId=" + productIdList,
                    headers: {
                        "Authorization": "Basic " + btoa("anhtruyen:0979610642"),
                        "retailerName": retailerName
                    },
                    method: 'GET',
                    success: (response, textStatus, xhr) => {
                        HoldOn.close();

                        let data = response || [];

                        $.each($trProducts, function (index, element) {
                            let $btnKvSyncProduct = $(this).find(".btn-sync-kv-product");
                            let $btnKvDeleteProduct = $(this).find(".btn-delete-kv-product");
                            let $btnKvObserverProduct = $(this).find(".btn-kv-product-observation");
                            let $btnKvUnObserverProduct = $(this).find(".btn-kv-product-unobserver");
                            let productSKU =  $(this).data("productsku");
                            let product = data.find(item => item.code.startsWith(productSKU));

                            if (product) {
                                $(this).data("existsKv", true);
                                $btnKvSyncProduct.addClass("hidden");
                                $btnKvDeleteProduct.removeClass("hidden");

                                if (product.cronJob) {
                                    $(this).data("observeKvProduct", true);
                                    $btnKvObserverProduct.addClass("hidden");
                                    $btnKvUnObserverProduct.removeClass("hidden");
                                }
                                else {
                                    product.data("observeKvProduct", false);
                                    $btnKvObserverProduct.removeClass("hidden");
                                    $btnKvUnObserverProduct.addClass("hidden");
                                }
                            }
                            else {
                                $(this).data("existsKv", false);
                                $(this).data("observeKvProduct", false);
                                $btnKvSyncProduct.removeClass("hidden");
                                $btnKvDeleteProduct.addClass("hidden");
                                $btnKvObserverProduct.addClass("hidden");
                                $btnKvUnObserverProduct.addClass("hidden");
                            }
                        });
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();
                    }
                });
            }

            // Parse URL Queries
            function url_query(query)
            {
                query = query.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
                var expr = "[\\?&]" + query + "=([^&#]*)";
                var regex = new RegExp(expr);
                var results = regex.exec(window.location.href);
                if (results !== null)
                {
                    return results[1];
                }
                else
                {
                    return false;
                }
            }

            var url_param = url_query('quantityfilter');
            if (url_param) {
                if (url_param == "greaterthan" || url_param == "lessthan")
                {
                    $(".greaterthan").removeClass("hide");
                    $(".between").addClass("hide");
                }
                else if (url_param == "between")
                {
                    $(".between").removeClass("hide");
                    $(".greaterthan").addClass("hide");
                }
            }


            function changeQuantityFilter(obj)
            {
                var value = obj.val();
                if (value == "greaterthan" || value == "lessthan")
                {
                    $(".greaterthan").removeClass("hide");
                    $(".between").addClass("hide");
                    $("#<%=txtQuantity.ClientID%>").focus().select();
                }
                else if (value == "between")
                {
                    $(".between").removeClass("hide");
                    $(".greaterthan").addClass("hide");
                    $("#<%=txtQuantityMin.ClientID%>").focus().select();
                }
            }

            function searchProduct()
            {
                let request = "?";

                let search = $("#<%=txtSearchProduct.ClientID%>").val();
                let stockstatus = $("#<%=ddlStockStatus.ClientID%>").val();
                let categoryid = $("#<%=ddlCategory.ClientID%>").val();
                let fromdate = $("#<%=rFromDate.ClientID%>").val();
                let todate = $("#<%=rToDate.ClientID%>").val();
                let quantityfilter = $("#<%=ddlQuantityFilter.ClientID%>").val();
                let quantity = $("#<%=txtQuantity.ClientID%>").val();
                let quantitymin = $("#<%=txtQuantityMin.ClientID%>").val();
                let quantitymax = $("#<%=txtQuantityMin.ClientID%>").val();
                let color = $("#<%=ddlColor.ClientID%>").val();
                let size = $("#<%=ddlSize.ClientID%>").val();
                let tag = $("#<%=ddlTag.ClientID%>").val();
                let orderby = $("#<%=ddlOrderBy.ClientID%>").val();
                let retailerName = $("#<%=ddlRetailer.ClientID%>").val();

                if (search != "")
                {
                    request += "&textsearch=" + search;
                }

                if (stockstatus != "")
                {
                    request += "&stockstatus=" + stockstatus;
                }

                if (categoryid != "0")
                {
                    request += "&categoryid=" + categoryid;
                }

                if (fromdate != "")
                {
                    request += "&fromdate=" + fromdate;
                }

                if (todate != "")
                {
                    request += "&todate=" + todate;
                }

                if (quantityfilter != "")
                {
                    if (quantityfilter == "greaterthan" || quantityfilter == "lessthan")
                    {
                        request += "&quantityfilter=" + quantityfilter + "&quantity=" + quantity;
                    }

                    if (quantityfilter == "between")
                    {
                        request += "&quantityfilter=" + quantityfilter + "&quantitymin=" + quantitymin + "&quantitymax=" + quantitymax;
                    }
                }

                // Add filter valiable value
                if (color != "")
                {
                    request += "&color=" + color;
                }
                if (size != "")
                {
                    request += "&size=" + size;
                }

                // Add filter tag
                if (tag != "0")
                {
                    request += "&tag=" + tag;
                }

                // Add filter order by
                if (orderby != "")
                {
                    request += "&orderby=" + orderby;
                }

                // Add filter order by
                if (retailerName) {
                    request += "&retailerName=" + retailerName;
                }

                window.open(request, "_self");
            }

            function changeCheckAll(checked) {
                let childDOM = $("td>input[type='checkbox']").not("[disabled='disabled']");

                // Button Post ALL Product KiotViet
                let btnAction = $(".btn-action");
                if (checked) {
                    btnAction.removeAttr("disabled");
                    btnAction.removeAttr("readonly");
                }
                else {
                    btnAction.attr("disabled", true);
                    btnAction.attr("readonly", true);
                }

                // Checkbox children
                $.each(childDOM, function (index, element) {
                    let $trProduct = $(this).parent().parent();
                    $(this).prop("checked", checked);

                    if ($trProduct.data("existsKv")) {
                        if (checked) {
                            productRemoval.push($trProduct.data("productid"));
                            productRemoval = $.unique(productRemoval.sort());
                        }
                        else {
                            productRemoval = productRemoval.filter(element => element != $trProduct.data("productid"));
                        }
                    }
                    else {
                        if (checked) {
                            productSync.push($trProduct.data("productsku"));
                            productSync = $.unique(productSync.sort());
                        }
                        else {
                            productSync = productSync.filter(element => element != $trProduct.data("productsku"));
                        }
                    }
                });

                _handleBtnKvProduct($("#<%=ddlRetailer.ClientID%>").val());
            }

            function checkAll() {
                let parentDOM = $("#checkAll");
                let childDOM = $("td>input[type='checkbox']").not("[disabled='disabled']");
                if (childDOM.length == 0) {
                    parentDOM.prop('checked', false);

                }
                else {
                    childDOM.each((index, element) => {
                        parentDOM.prop('checked', element.checked);
                        
                        if (!element.checked) return false;
                    });
                }
            }

            function changeCheck(self) {
                let $trProduct = self.parent().parent();
                let checked = self.is(':checked');

                if ($trProduct.data("existsKv")) {
                    if (checked) {
                        productRemoval.push($trProduct.data("productid"));
                        productRemoval = $.unique(productRemoval.sort());
                    }
                    else {
                        productRemoval = productRemoval.filter(element => element != $trProduct.data("productid"))
                    }
                }
                else {
                    if (checked) {
                        productSync.push($trProduct.data("productsku"));
                        productSync = $.unique(productSync.sort());
                    }
                    else {
                        productSync = productSync.filter(element => element != $trProduct.data("productsku"));
                    }
                }

                _handleBtnKvProduct($("#<%=ddlRetailer.ClientID%>").val())

                // Hổ trợ xử lý check or uncheck
                checkAll();
            }

            // #region KiotViet
            // Đồng bộ sản phẩm lên KiotViet theo danh mục
            function syncKvProductByCategory() {
                let retailerName = $("#<%=ddlRetailer.ClientID%>").val();
                let $ddlCategory = $("#<%=ddlCategory.ClientID%>");

                if (!retailerName || $ddlCategory.val() == "0")
                    return;

                let titleAlert = "Sản phẩm đang được đăng từ từ lên KiotViet theo danh mục";

                $.ajax({
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    url: "/api/v1/cron-job/kiotviet/sync-product",
                    headers: {
                        "Authorization": "Basic " + btoa("anhtruyen:0979610642"),
                        "retailerName": retailerName
                    },
                    contentType: 'application/json',
                    method: 'POST',
                    dataType: "json",
                    data: JSON.stringify({ "categorySlug": $ddlCategory.find(":selected").data("slug") }),
                    success: (response, textStatus, xhr) => {
                        HoldOn.close();

                        if (xhr.status == 200) {
                            return swal({
                                title: titleAlert,
                                text: "Thành công",
                                type: "success",
                                html: true
                            }, function () {
                                location.reload();
                            });
                        } else {
                            _alterError(titleAlert);
                        }
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();
                        _alterError(titleAlert, xhr.responseJSON);
                    }
                });
            }

            // Đăng ký theo dõi danh mục
            <%--function registerKvCategoryObservation() {
                let retailerName = $("#<%=ddlRetailer.ClientID%>").val();
                let categoryId = $("#<%=ddlCategory.ClientID%>").val();

                if (!retailerName || categoryId == "0")
                    return;

                let titleAlert = "Đăng ký theo dõi danh mục";

                $.ajax({
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    url: "/api/v1/kiotviet/category/ann-shop/" + categoryId + "/observation",
                    headers: {
                        "Authorization": "Basic " + btoa("anhtruyen:0979610642"),
                        "retailerName": retailerName
                    },
                    contentType: 'application/json',
                    method: 'POST',
                    success: (response, textStatus, xhr) => {
                        HoldOn.close();
                        let $btnKvCategoryObserve = $("#btnKvCategoryObserve");
                        let $btnKvCategoryUnObserve = $("#btnKvCategoryUnObserve");

                        if (xhr.status == 200) {
                            observeKvCategory = true;
                            // Observer Category
                            $btnKvCategoryObserve.addClass("hidden");
                            // UnObserver Category
                            $btnKvCategoryUnObserve.removeClass("hidden");

                            _alterSuccess(titleAlert, "Thành công");
                        } else {
                            _alterError(titleAlert);
                        }
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();
                        _alterError(titleAlert, xhr.responseJSON);
                    }
                });
            }--%>

            // Bỏ theo dõi danh mục
            <%--function deleteKvCategoryObservation() {
                let retailerName = $("#<%=ddlRetailer.ClientID%>").val();
                let categoryId = $("#<%=ddlCategory.ClientID%>").val();

                if (!retailerName || categoryId == "0")
                    return;

                let titleAlert = "Bỏ theo theo dõi danh mục";

                $.ajax({
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    url: "/api/v1/kiotviet/category/ann-shop/" + categoryId + "/unobservation",
                    headers: {
                        "Authorization": "Basic " + btoa("anhtruyen:0979610642"),
                        "retailerName": retailerName
                    },
                    contentType: 'application/json',
                    method: 'POST',
                    success: (response, textStatus, xhr) => {
                        HoldOn.close();
                        let $btnKvCategoryObserve = $("#btnKvCategoryObserve");
                        let $btnKvCategoryUnObserve = $("#btnKvCategoryUnObserve");

                        if (xhr.status == 200) {
                            observeKvCategory = false;
                            // Observer Category
                            $btnKvCategoryObserve.removeClass("hidden");
                            // UnObserver Category
                            $btnKvCategoryUnObserve.addClass("hidden");

                            _alterSuccess(titleAlert, "Thành công");
                        } else {
                            _alterError(titleAlert);
                        }
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();
                        _alterError(titleAlert, xhr.responseJSON);
                    }
                });
            }--%>

            // Đồng bộ danh sách sản phẩm lên KiotViet
            function syncKvProducts() {
                let retailerName = $("#<%=ddlRetailer.ClientID%>").val();

                if (!retailerName || productSync.length == 0)
                    return;

                let titleAlert = "Các sản phẩm đang được đăng từ từ lên KiotViet";

                $.ajax({
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    url: "/api/v1/cron-job/kiotviet/sync-product",
                    headers: {
                        "Authorization": "Basic " + btoa("anhtruyen:0979610642"),
                        "retailerName": retailerName
                    },
                    contentType: 'application/json',
                    method: 'POST',
                    dataType: "json",
                    data: JSON.stringify({
                        "productSKU": productSync.join(','),
                        "ignoreQuantity": retailerName == "giaminhwill"
                    }),
                    success: (response, textStatus, xhr) => {
                        HoldOn.close();
                        let $btnSyncAllKvProduct = $("#btnSyncAllKvProduct");
                        //let $btnDeleteAllKvProduct = $("#btnDeleteAllKvProduct");

                        if (xhr.status == 200) {
                            $.each(productSync, function (index, element) {
                                let $trProduct = $(".tr-product[data-productsku='" + element + "']");
                                let $btnKvSyncProduct = $trProduct.find(".btn-sync-kv-product");
                                let $btnKvDeleteProduct = $trProduct.find(".btn-delete-kv-product");
                                let $btnKvObserverProduct = $trProduct.find(".btn-kv-product-observation");
                                let $btnKvUnObserverProduct = $trProduct.find(".btn-kv-product-unobserver");

                                $trProduct.data("existsKv", true);
                                $trProduct.data("observeKvProduct", true);
                                $btnKvSyncProduct.addClass("hidden");
                                $btnKvDeleteProduct.removeClass("hidden");
                                $btnKvObserverProduct.addClass("hidden");
                                $btnKvUnObserverProduct.removeClass("hidden");

                                productRemoval.push($trProduct.data("productid"));
                                productRemoval = $.unique(productRemoval.sort());
                            });
                            productSync = [];

                            // Sync Product
                            $btnSyncAllKvProduct.addClass("hidden");
                            $btnSyncAllKvProduct.attr("disabled", true);
                            $btnSyncAllKvProduct.attr("readonly", true);
                            // Delete Product
                            //$btnDeleteAllKvProduct.removeClass("hidden");
                            //$btnDeleteAllKvProduct.attr("disabled", false);
                            //$btnDeleteAllKvProduct.attr("readonly", false);

                            _alterSuccess(titleAlert, "Thành công");
                        } else {
                            _alterError(titleAlert);
                        }
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();
                        _alterError(titleAlert, xhr.responseJSON);
                    }
                });
            }

            // Xóa sản phẩm
            <%--function deleteKvProducts() {
                let retailerName = $("#<%=ddlRetailer.ClientID%>").val();

                if (!retailerName || productRemoval.length == 0)
                    return;

                return swal({
                    title: "Xóa tất cả",
                    text: "Chức năng nay đang trong quá trình phát triển",
                    type: "warning"
                })

                //let titleAlert = "Các sản phẩm đang được xóa dần trên KiotViet";

                //$.ajax({
                //    beforeSend: function () {
                //        HoldOn.open();
                //    },
                //    url: "/api/v1/cron-job/kiotviet/product/delete",
                //    headers: {
                //        "Authorization": "Basic " + btoa("anhtruyen:0979610642"),
                //        "retailerName": retailerName
                //    },
                //    contentType: 'application/json',
                //    method: 'POST',
                //    dataType: "json",
                //    data: JSON.stringify({ "referenceProductId": productRemoval.join(',') }),
                //    success: (response, textStatus, xhr) => {
                //        HoldOn.close();
                //        let $btnSyncAllKvProduct = $("#btnSyncAllKvProduct");
                //        let $btnDeleteAllKvProduct = $("#btnDeleteAllKvProduct");

                //        if (xhr.status == 200) {
                //            $.each(productRemoval, function (index, element) {
                //                let $trProduct = $(".tr-product[data-productid='" + element + "']");
                //                let $btnKvSyncProduct = $trProduct.find(".btn-sync-kv-product");
                //                let $btnKvDeleteProduct = $trProduct.find(".btn-delete-kv-product");
                //                let $btnKvObserverProduct = $trProduct.find(".btn-kv-product-observation");
                //                let $btnKvUnObserverProduct = $trProduct.find(".btn-kv-product-unobserver");

                //                $trProduct.data("existsKv", false);
                //                $trProduct.data("observeKvProduct", false);
                //                $btnKvSyncProduct.removeClass("hidden");
                //                $btnKvDeleteProduct.addClass("hidden");
                //                $btnKvObserverProduct.addClass("hidden");
                //                $btnKvUnObserverProduct.addClass("hidden");

                //                productSync.push($trProduct.data("productsku"));
                //                productSync = $.unique(productSync.sort());
                //            });
                //            productRemoval = [];

                //            // Sync Product
                //            $btnSyncAllKvProduct.removeClass("hidden");
                //            $btnSyncAllKvProduct.attr("disabled", false);
                //            $btnSyncAllKvProduct.attr("readonly", false);
                //            // Delete Product
                //            $btnDeleteAllKvProduct.addClass("hidden");
                //            $btnDeleteAllKvProduct.attr("disabled", true);
                //            $btnDeleteAllKvProduct.attr("readonly", true);

                //            _alterSuccess(titleAlert, "Thành công");
                //        } else {
                //            _alterError(titleAlert);
                //        }
                //    },
                //    error: (xhr, textStatus, error) => {
                //        HoldOn.close();
                //        _alterError(titleAlert, xhr.responseJSON);
                //    }
                //});
            }--%>

            // Đồng bộ sản phẩm lên KiotViet
            function syncKvProduct(productSKU) {
                let retailerName = $("#<%=ddlRetailer.ClientID%>").val();

                if (!retailerName || !productSKU)
                    return;

                let $trProduct = $(".tr-product[data-productsku='" + productSKU + "']");

                if ($trProduct.data("existsKv"))
                    return;

                let titleAlert = "Đăng sản phẩm KiotViet";

                $.ajax({
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    url: "/api/v1/kiotviet/product",
                    headers: {
                        "Authorization": "Basic " + btoa("anhtruyen:0979610642"),
                        "retailerName": retailerName
                    },
                    method: 'POST',
                    contentType: 'application/json',
                    dataType: "json",
                    data: JSON.stringify({
                        "productSKU": productSKU,
                        "ignoreQuantity": retailerName == "giaminhwill"
                    }),
                    success: (response, textStatus, xhr) => {
                        HoldOn.close();
                        let $btnKvSyncProduct = $trProduct.find(".btn-sync-kv-product");
                        let $btnKvDeleteProduct = $trProduct.find(".btn-delete-kv-product");
                        let $btnKvObserverProduct = $trProduct.find(".btn-kv-product-observation");
                        let $btnKvUnObserverProduct = $trProduct.find(".btn-kv-product-unobserver");

                        if (xhr.status == 200) {
                            $trProduct.data("existsKv", true);
                            $trProduct.data("observeKvProduct", true);
                            $btnKvSyncProduct.addClass("hidden");
                            $btnKvDeleteProduct.removeClass("hidden");
                            $btnKvObserverProduct.addClass("hidden");
                            $btnKvUnObserverProduct.removeClass("hidden");

                            let checked = $trProduct.find("input[type='checkbox']").is(":checked");

                            if (checked)
                            {
                                // Product Removal
                                productRemoval.push($trProduct.data("productid"));
                                productRemoval = $.unique(productRemoval.sort());
                                // Product Sync
                                productSync = productSync.filter(item => item != $trProduct.data("productsku"));
                                // Handle button KiotViet at header
                                _handleBtnKvProduct(retailerName);
                            }

                            _alterSuccess(titleAlert, "Đăng sản phẩm <strong>" + productSKU + "</strong> thành công!");
                        } else {
                            _alterError(titleAlert);
                        }
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();
                        _alterError(titleAlert, xhr.responseJSON);
                    }
                });
            }

            // Xóa sản phẩm
            function deleteKvProduct(productId) {
                let retailerName = $("#<%=ddlRetailer.ClientID%>").val();

                if (!retailerName || !productId || !$.isNumeric(productId))
                    return;

                let $trProduct = $(".tr-product[data-productid='" + productId + "']");

                if (!$trProduct.data("existsKv"))
                    return;

                let titleAlert = "Xóa sản phẩm KiotViet";

                $.ajax({
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    url: "/api/v1/kiotviet/product/delete",
                    headers: {
                        "Authorization": "Basic " + btoa("anhtruyen:0979610642"),
                        "retailerName": retailerName
                    },
                    method: 'POST',
                    contentType: 'application/json',
                    dataType: "json",
                    data: JSON.stringify({ "referenceProductId": productId }),
                    success: (response, textStatus, xhr) => {
                        HoldOn.close();
                        let $btnKvSyncProduct = $trProduct.find(".btn-sync-kv-product");
                        let $btnKvDeleteProduct = $trProduct.find(".btn-delete-kv-product");
                        let $btnKvObserverProduct = $trProduct.find(".btn-kv-product-observation");
                        let $btnKvUnObserverProduct = $trProduct.find(".btn-kv-product-unobserver");

                        if (xhr.status == 200) {
                            $trProduct.data("existsKv", false);
                            $trProduct.data("observeKvProduct", false);
                            $btnKvSyncProduct.removeClass("hidden");
                            $btnKvDeleteProduct.addClass("hidden");
                            $btnKvObserverProduct.addClass("hidden");
                            $btnKvUnObserverProduct.addClass("hidden");

                            let checked = $trProduct.find("input[type='checkbox']").is(":checked");

                            if (checked) {
                                // Product Removal
                                productSync.push($trProduct.data("productsku"));
                                productSync = $.unique(productSync.sort());
                                // Product Sync
                                productRemoval = productRemoval.filter(item => item != $trProduct.data("productid"));
                                // Handle button KiotViet at header
                                _handleBtnKvProduct(retailerName);
                            }

                            _alterSuccess(titleAlert, "Xóa sản phẩm <strong>" + $trProduct.data("productsku") + "</strong> thành công!");
                        } else {
                            _alterError(titleAlert);
                        }
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();
                        _alterError(titleAlert, xhr.responseJSON);
                    }
                });
            }

            // Đăng ký theo dõi danh mục
            function registerKvProductObservation(productSKU) {
                let retailerName = $("#<%=ddlRetailer.ClientID%>").val();

                if (!retailerName || !productSKU)
                    return;

                let $trProduct = $(".tr-product[data-productsku='" + productSKU + "']");

                if ($trProduct.data("observeKvProduct"))
                    return;

                let titleAlert = "Đăng ký theo dõi sản phẩm";

                $.ajax({
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    url: "/api/v1/kiotviet/product/" + productSKU + "/observation",
                    headers: {
                        "Authorization": "Basic " + btoa("anhtruyen:0979610642"),
                        "retailerName": retailerName
                    },
                    method: 'POST',
                    success: (response, textStatus, xhr) => {
                        HoldOn.close();
                        let $btnKvObserverProduct = $trProduct.find(".btn-kv-product-observation");
                        let $btnKvUnObserverProduct = $trProduct.find(".btn-kv-product-unobserver");

                        if (xhr.status == 200) {
                            $trProduct.data("observeKvProduct", true);
                            $btnKvObserverProduct.addClass("hidden");
                            $btnKvUnObserverProduct.removeClass("hidden");

                            _alterSuccess(titleAlert, "Sản phẩm <strong>" + productSKU + "</strong> đăng ký theo dõi thành công!");
                        } else {
                            _alterError(titleAlert);
                        }
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();
                        _alterError(titleAlert, xhr.responseJSON);
                    }
                });
            }

            // Bỏ theo dõi san pham
            function deleteKvProductObservation(productSKU) {
                let retailerName = $("#<%=ddlRetailer.ClientID%>").val();

                if (!retailerName || !productSKU)
                    return;

                let $trProduct = $(".tr-product[data-productsku='" + productSKU + "']");

                if (!$trProduct.data("observeKvProduct"))
                    return;

                let titleAlert = "Bỏ theo dõi sản phẩm";

                $.ajax({
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    url: "/api/v1/kiotviet/product/" + productSKU + "/unobservation",
                    headers: {
                        "Authorization": "Basic " + btoa("anhtruyen:0979610642"),
                        "retailerName": retailerName
                    },
                    method: 'POST',
                    success: (response, textStatus, xhr) => {
                        HoldOn.close();
                        let $btnKvObserverProduct = $trProduct.find(".btn-kv-product-observation");
                        let $btnKvUnObserverProduct = $trProduct.find(".btn-kv-product-unobserver");

                        if (xhr.status == 200) {
                            $trProduct.data("observeKvProduct", false);
                            $btnKvObserverProduct.removeClass("hidden");
                            $btnKvUnObserverProduct.addClass("hidden");

                            _alterSuccess(titleAlert, "Sản phẩm <strong>" + productSKU + "</strong> đã bỏ theo dõi thành công!");
                        } else {
                            _alterError(titleAlert);
                        }
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();
                        _alterError(titleAlert, xhr.responseJSON);
                    }
                });
            }
            // #endregion

            function postALLProductZaloShop() {
                let $checkBox = $("td>input[type='checkbox']:checked");
                let products = [];

                $checkBox.each(function (index, element) {
                    let $tr = element.parentElement.parentElement;

                    products.push($tr.dataset.productsku);
                });

                let titleAlert = "Đăng sản phẩm Zalo Shop";

                if (products.length == 0)
                    return _alterError(titleAlert, { message: "Chưa chọn sản phẩm nào!" });

                let dataJSON = JSON.stringify({ "productSKU": products.join(',') });

                $.ajax({
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    method: 'POST',
                    contentType: 'application/json',
                    dataType: "json",
                    data: dataJSON,
                    url: "/api/v1/zaloshop/product",
                    success: (response, textStatus, xhr) => {
                        HoldOn.close();

                        if (xhr.status == 200) {
                            _alterSuccess(titleAlert, "Thành công");
                        } else {
                            _alterError(titleAlert);
                        }
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();
                        _alterError(titleAlert, xhr.responseJSON);
                    }
                });
            }

            function postProductZaloShop(productSKU) {
                let titleAlert = "Đăng sản phẩm Zalo Shop";

                if (!productSKU)
                    _alterError(titleAlert, { message: "Chưa chọn sản phẩm nào!" });

                let dataJSON = JSON.stringify({ "productSKU": productSKU });

                $.ajax({
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    method: 'POST',
                    contentType: 'application/json',
                    dataType: "json",
                    data: dataJSON,
                    url: "/api/v1/zaloshop/product",
                    success: (response, textStatus, xhr) => {
                        HoldOn.close();

                        if (xhr.status == 200) {
                            _alterSuccess(titleAlert, "Đăng sản phẩm <strong>" + productSKU + "</strong> thành công!");
                        } else {
                            _alterError(titleAlert);
                        }
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();

                        _alterError(titleAlert, xhr.responseJSON);
                    }
                });
            }

            

            function deleteProductZaloShop(productSKU) {
                let titleAlert = "Xóa sản phẩm Zalo Shop";

                if (!productSKU)
                    _alterError(titleAlert, { message: "Chưa chọn sản phẩm nào!" });

                $.ajax({
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    method: 'POST',
                    contentType: 'application/json',
                    dataType: "json",
                    url: "/api/v1/zaloshop/product/" + productSKU,
                    success: (response, textStatus, xhr) => {
                        HoldOn.close();

                        if (xhr.status == 200) {
                            _alterSuccess(titleAlert, "Sản phẩm <strong>" + productSKU + "</strong> xóa thành công!");
                        } else {
                            _alterError(titleAlert);
                        }
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();

                        _alterError(titleAlert, xhr.responseJSON);
                    }
                });
            }

            function downloadProductZaloShop(productSKU) {

                if (!productSKU)
                    _alterError(titleAlert, { message: "Chưa chọn sản phẩm nào!" });

                window.open("/api/v1/zaloshop/product/export/excel?productSKU=" + productSKU, "_self");

            }

            function downloadAllZaloShop() {

                let $checkBox = $("td>input[type='checkbox']:checked");
                let products = [];

                $checkBox.each(function (index, element) {
                    let $tr = element.parentElement.parentElement;

                    products.push($tr.dataset.productsku);
                });

                if (products.length == 0)
                    return _alterError(titleAlert, { message: "Chưa chọn sản phẩm nào!" });

                window.open("/api/v1/zaloshop/product/export/excel?productSKU=" + products.join(','), "_self");

            }
            

            function _alterSuccess(title, message) {
                title = (typeof title !== 'undefined') ? title : 'Thông báo thành công';

                if (message === undefined) {
                    message = null;
                }

                return swal({
                    title: title,
                    text: message,
                    type: "success",
                    html: true
                });
            }

            function _alterError(title, responseJSON) {
                let message = '';
                title = (typeof title !== 'undefined') ? title : 'Thông báo lỗi';

                if (responseJSON === undefined || responseJSON === null) {
                    message = 'Đẫ có lỗi xãy ra.';
                }
                else {
                    if (responseJSON.message)
                        message += responseJSON.message;
                }

                return swal({
                    title: title,
                    text: message,
                    type: "error",
                    html: true
                });
            }
        </script>
    </main>
</asp:Content>
