<%@ Page Title="Tất cả sản phẩm" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="tat-ca-san-pham.aspx.cs" Inherits="IM_PJ.tat_ca_san_pham" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .select2-container .select2-selection--single {
            height: 45px;
        }
        .select2-container--default .select2-selection--single .select2-selection__rendered {
            line-height: 45px;
        }
        .select2-container--default .select2-selection--single .select2-selection__arrow {
            height: 43px;
        }
        .btn.download-btn {
            background-color: #000;
            color: #fff;
            border-radius: 0;
            text-transform: uppercase;
            width: 100%;
            height: 35px;
            line-height: 8px;
        }
        .btn.download-btn:hover {
            color: #ff8400;
        }
        table.shop_table_responsive > tbody > tr:nth-of-type(2n+1) td {
            border-bottom: solid 1px #e1e1e1!important;
        }
        @media (max-width: 768px) {
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
                padding-bottom: 1px;
                text-align: left;
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(13):before {
                content: "";
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(13) {
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
                height: 85px;
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
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title left">Sản phẩm <span>(<asp:Literal ID="ltrNumberOfProduct" runat="server" EnableViewState="false"></asp:Literal>) <a href="/sp" target="_blank" class="btn primary-btn h45-btn">Mở rộng</a></span></h3>
                    <div class="right above-list-btn">
                        <asp:Literal ID="ltrAddProduct" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-3 col-xs-12">
                                    <asp:TextBox ID="txtSearchProduct" runat="server" CssClass="form-control" placeholder="Tìm sản phẩm" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlWebPublish" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Trang xem hàng"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Đang ẩn"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Đang hiện"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlShowHomePage" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Trang chủ"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Đang ẩn"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Đang hiện"></asp:ListItem>
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
                                <div class="col-md-1 col-xs-6 search-button">
                                    <a href="javascript:;" onclick="searchProduct()" class="btn primary-btn h45-btn"><i class="fa fa-search"></i></a>
                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn primary-btn h45-btn" OnClick="btnSearch_Click" Style="display: none" />
                                    <a href="/tat-ca-san-pham" class="btn primary-btn h45-btn"><i class="fa fa-times" aria-hidden="true"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-3 col-xs-6">
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
                                        <asp:ListItem Value="4" Text="Hàng order"></asp:ListItem>
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
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlPreOrder" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Loại hàng"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Hàng có sẵn"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Hàng order"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1 col-xs-6">
                                    <a href="javascript:;" onclick="copyProductID()" class="btn primary-btn h45-btn"><i class="fa fa-clone"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-3 col-xs-6">
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
                                    <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Chọn kho"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Kho 1"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Kho 2"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1 col-xs-6">
                                </div>
                            </div>
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
                            <table class="table table-checkable table-product all-product-table shop_table_responsive">
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

        <script src="/App_Themes/Ann/js/copy-product-info.js?v=01122021"></script>
        <script src="/App_Themes/Ann/js/sync-product-v2.js?v=01122021"></script>
        <script src="/App_Themes/Ann/js/download-product-image.js?v=01122021"></script>
        <script src="/App_Themes/Ann/js/services/common/product-service.js?v=01122021"></script>
        
        <script type="text/javascript">
            $("#<%=txtSearchProduct.ClientID%>").keyup(function (e) {
                if (e.keyCode == 13)
                {
                    $("#<%= btnSearch.ClientID%>").click();
                }
            });

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
                $("#<%= btnSearch.ClientID%>").click();
            }

            function updateHotProduct(obj) {
                let productID = obj.attr("data-id");

                $.ajax({
                    type: "POST",
                    url: "/tat-ca-san-pham.aspx/updateHotProduct",
                    data: "{productID: " + productID + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d == "hot") {
                            $(obj).html("<i class='fa fa-star' aria-hidden='true'></i>");
                        }
                        else if (msg.d == "noHot") {
                            $(obj).html("<i class='fa fa-star-o' aria-hidden='true'></i>");
                        }
                        else if (msg.d == "productNotfound") {
                            alert("Lỗi không tìm thấy sản phẩm");
                        }
                    }
                });
            }
            
            function updateShowHomePage(obj)
            {
                var ID = obj.attr("data-product-id");
                var update = obj.attr("data-update");
                $.ajax({
                    type: "POST",
                    url: "/tat-ca-san-pham.aspx/updateShowHomePage",
                    data: "{id: " + ID + ", value: " + update + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    success: function (msg) {
                        HoldOn.close();
                        if (msg.d == "nocleanimage")
                        {
                            swal("Thông báo", "Sản phẩm này chưa có hình đại diện sạch!", "warning");
                        }
                        else if (msg.d == "sameimage")
                        {
                            swal("Thông báo", "Có thể hình đại diện sạch giống với hình có mã.<br> Hãy kiểm tra lại!", "warning");
                        }
                        else if (msg.d == "true")
                        {
                            if (update == 1)
                            {
                                $('#showHomePage_' + ID).html("<a href='javascript:;' data-product-id='" + ID + "' data-update='0' class='bg-green bg-button' onclick='updateShowHomePage($(this))'>Đang hiện</a>");
                                $(".up-product-" + ID).removeClass("hide");
                            }
                            else
                            {
                                $('#showHomePage_' + ID).html("<a href='javascript:;' data-product-id='" + ID + "' data-update='1' class='bg-black bg-button' onclick='updateShowHomePage($(this))'>Đang ẩn</a>");
                                $(".up-product-" + ID).addClass("hide");
                            }
                        }
                        else
                        {
                            alert("Lỗi");
                        }
                    },
                    complete: function () {
                        HoldOn.close();
                    }
                });
            }

            function updateShowWebPublish(obj)
            {
                var ID = obj.attr("data-product-id");
                var update = obj.attr("data-update");
                $.ajax({
                    type: "POST",
                    url: "/tat-ca-san-pham.aspx/updateWebPublish",
                    data: "{id: " + ID + ", value: " + update + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    success: function (msg) {
                        if (msg.d == "true")
                        {
                            if (update == "true")
                            {
                                $('#showWebPublish_' + ID).html("<a href='javascript:;' data-product-id='" + ID + "' data-update='false' class='bg-red bg-button' onclick='updateShowWebPublish($(this))'>Đang hiện</a>");
                                $(".webupdate-product-" + ID).removeClass("hide");
                            }
                            else
                            {
                                $('#showWebPublish_' + ID).html("<a href='javascript:;' data-product-id='" + ID + "' data-update='true' class='bg-black bg-button' onclick='updateShowWebPublish($(this))'>Đang ẩn</a>");
                                $(".webupdate-product-" + ID).addClass("hide");
                            }
                        }
                        else
                        {
                            alert("Lỗi");
                        }
                    },
                    complete: function () {
                        HoldOn.close();
                    }
                });
            }

            function upTopWebUpdate(obj)
            {
                var productID = obj.attr("data-id");

                $.ajax({
                    type: "POST",
                    url: "/tat-ca-san-pham.aspx/upTopWebUpdate",
                    data: "{id: " + productID + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    success: function (msg) {
                        if (msg.d == "true")
                        {
                            swal("Thông báo", "Up thành công!", "success");
                        }
                        else
                        {
                            alert("Lỗi");
                        }
                    },
                    complete: function () {
                        HoldOn.close();
                    }
                });
            }

            function liquidateProduct(categoryID, productID, sku)
            {
                swal({
                    title: "Xác nhận",
                    text: "Bạn muốn xả kho sản phẩm này?",
                    type: "warning",
                    showCancelButton: true,
                    closeOnConfirm: true,
                    cancelButtonText: "Để em xem lại...",
                    confirmButtonText: "Đúng rồi sếp!",
                }, function (confirm) {
                    if (confirm) {
                        HoldOn.open();
                        ProductService.liquidate(productID)
                            .then(data => {
                                if (data) {
                                    let btnLiquidateDOM = document.querySelector(".liquidation-product-" + productID);
                                    let rowDOM = btnLiquidateDOM.parentElement.parentElement;
                                    let skuDOM = rowDOM.querySelector("[data-title='Mã']").querySelector(".customer-name-link");
                                    let quantityDOM = rowDOM.querySelector("[data-title='Số lượng']").querySelector("span");
                                    let stockDOM = rowDOM.querySelector("[data-title='Kho']");

                                    // Cập nhật trạng thái xã kho
                                    btnLiquidateDOM.remove();
                                    rowDOM.querySelector(".update-button").innerHTML += btnRecoverLiquidatedHTML(categoryID, productID, sku);
                                    quantityDOM.innerHTML = '<a target="_blank" href="/thong-ke-san-pham?SKU=' + skuDOM.innerText + '">0</a>';
                                    stockDOM.innerHTML = '<span class="bg-red">Hết hàng</span>';

                                    //setTimeout(function () {
                                    //    swal({
                                    //        title: "Thông báo",
                                    //        text: "Xã kho thành công!",
                                    //        type: "success"
                                    //    });
                                    //}, 500);
                                }
                                else {
                                    setTimeout(function () {
                                        swal("Thông báo", "Có lỗi trong qua trình xã kho", "error");
                                    }, 500);
                                }
                            })
                            .catch(err => {
                                setTimeout(function () {
                                    swal("Thông báo", "Có lỗi trong qua trình xã kho", "error");
                                }, 500);
                            })
                            .finally(() => { HoldOn.close(); });
                    }
                });
            }

            function recoverLiquidatedProduct(categoryID, productID, sku) {
                swal({
                    title: "Xác nhận",
                    text: "Bạn muốn phục hồi xã kho?",
                    type: "warning",
                    showCancelButton: true,
                    closeOnConfirm: true,
                    cancelButtonText: "Để em xem lại...",
                    confirmButtonText: "Đúng rồi sếp!",
                }, function (confirm) {
                    if (confirm) {
                        HoldOn.open();
                        ProductService.recoverLiquidated(productID, sku)
                            .then(data => {
                                if (data) {
                                    let btnRecoverLiquidatedDOM = document.querySelector(".recover-liquidation-product-" + productID);
                                    let rowDOM = btnRecoverLiquidatedDOM.parentElement.parentElement;
                                    let skuDOM = rowDOM.querySelector("[data-title='Mã']").querySelector(".customer-name-link");
                                    let quantityDOM = rowDOM.querySelector("[data-title='Số lượng']").querySelector("span");
                                    let stockDOM = rowDOM.querySelector("[data-title='Kho']");

                                    // Cập nhật trạng thái xã kho
                                    btnRecoverLiquidatedDOM.remove();
                                    rowDOM.querySelector(".update-button").innerHTML += btnLiquidateHTML(categoryID, productID, sku);
                                    quantityDOM.innerHTML = '<a target="_blank" href="/thong-ke-san-pham?SKU=' + skuDOM.innerText + '">' + data.TotalProductInstockQuantityLeft + '</a>';
                                    stockDOM.innerHTML = '<span class="bg-green">Còn hàng</span>';

                                    setTimeout(function () {
                                        swal({
                                            title: "Thông báo", 
                                            text: "Phục hồi xã kho thành công!", 
                                            type: "success"
                                        });
                                    }, 500);
                                }
                                else {
                                    setTimeout(function () {
                                        swal("Thông báo", "Có lỗi trong qua trình phục hồi lại xã kho", "error");
                                    }, 500);
                                }
                            })
                            .catch(err => {
                                setTimeout(function () {
                                    swal("Thông báo", "Có lỗi trong qua trình phục hồi lại xã kho", "error");
                                }, 500);
                            })
                            .finally(() => { HoldOn.close(); });
                    }
                });
            }

            function btnLiquidateHTML(categoryID, productID, sku)
            {
                let strHTML = "";

                strHTML += "<a href='javascript:;' ";
                strHTML += "       title='Xả kho' ";
                strHTML += "       class='liquidation-product-" + productID + " btn primary-btn btn-red h45-btn' ";
                strHTML += "       onclick='liquidateProduct(" + categoryID + ", " + productID + ", `" + sku + "`);'>";
                strHTML += "   <i class='glyphicon glyphicon-trash' aria-hidden='true'></i>";
                strHTML += "</a>";

                return strHTML;
            };

            function btnRecoverLiquidatedHTML(categoryID, productID, sku)
            {
                let strHTML = "";

                strHTML += "<a href='javascript:;' ";
                strHTML += "       title='Phục hồi xả kho' ";
                strHTML += "       class='recover-liquidation-product-" + productID + " btn primary-btn btn-green h45-btn' ";
                strHTML += "       onclick='recoverLiquidatedProduct(" + categoryID + ", " + productID + ", `" + sku + "`);'>";
                strHTML += "   <i class='glyphicon glyphicon-repeat' aria-hidden='true'></i>";
                strHTML += "</a>";

                return strHTML;
            };

            function hiddenProduct(categoryID, productID, sku) {
                setTimeout(() => {
                    swal({
                        title: "Xác nhận",
                        text: "Bạn đang muốn ẩn sản phẩm đúng không?",
                        type: "warning",
                        showCancelButton: true,
                        closeOnConfirm: true,
                        cancelButtonText: "Để em xem lại...",
                        confirmButtonText: "Đúng rồi sếp!",
                    }, (confirm) => {
                        if (confirm) {
                            // Thực hiện update giá trị trong database
                            HoldOn.open();
                            ProductService.updateHidden(productID, true)
                                .then(() => {
                                    // Thực hiện ẩn sản phẩm trên các trang quảng cáo
                                    HoldOn.open();
                                    ProductService.handleSyncProduct(categoryID, productID, sku, false, false, true)
                                        .then(() => {
                                            setTimeout(() => {
                                                swal("Thông báo", "Ẩn sản phẩm trên trang quảng cáo thành công!", "success");
                                            }, 500);
                                        })
                                        .catch(err => {
                                            setTimeout(() => {
                                                swal("Thông báo", "Có lỗi trong qua trình ẩn sản phẩm trên các trang quảng cáo", "error");
                                            }, 500);
                                        })
                                        .finally(() => { HoldOn.close(); });
                                })
                                .catch(err => {
                                    setTimeout(() => {
                                        swal("Thông báo", "Có lỗi trong qua trình ẩn sản phẩm", "error");
                                    }, 500);
                                })
                                .finally(() => { HoldOn.close(); });
                        }
                    });
                }, 1000);
            };

            function unhiddenProduct(categoryID, productID, sku, up, renew) {
                setTimeout(() => {
                    swal({
                        title: "Xác nhận",
                        text: "Bạn đang muốn hiện sản phẩm đúng không?",
                        type: "warning",
                        showCancelButton: true,
                        closeOnConfirm: true,
                        cancelButtonText: "Để em xem lại...",
                        confirmButtonText: "Đúng rồi sếp!",
                    }, (confirm) => {
                        if (confirm) {
                            // Thực hiện update giá trị trong database
                            HoldOn.open();
                            ProductService.updateHidden(productID, false)
                                .then(() => {
                                    // Thực hiện hiển thị sản phẩm trên các trang quảng cáo
                                    HoldOn.open();
                                    ProductService.handleSyncProduct(categoryID, productID, sku, false, false, false)
                                        .then(() => {
                                            setTimeout(()  => {
                                                swal("Thông báo", "Hiện sản phẩm trên trang quảng cáo thành công!", "success");
                                            }, 500);
                                        })
                                        .catch(err => {
                                            setTimeout(() => {
                                                swal("Thông báo", "Có lỗi trong qua trình hiện sản phẩm trên các trang quảng cáo", "error");
                                            }, 500);
                                        })
                                        .finally(() => { HoldOn.close(); });
                                })
                                .catch(err => {
                                    setTimeout(() => {
                                        swal("Thông báo", "Có lỗi trong qua trình hiển sản phẩm", "error");
                                    }, 500);
                                })
                                .finally(() => { HoldOn.close(); });
                        }
                    });
                }, 1000);
            };

            function copyProductID() {
                $("body").append("<div class='copy-product-id hide'></div>");

                $(".table-product > tbody > tr").each(function (i, tr) {
                    let showHomePage = $(tr).attr("data-home-page");
                    let stockQuantity = $(tr).attr("data-stock-quantity");
                    if (showHomePage == 1) {
                        $(".copy-product-id").append($(tr).attr("data-id") + "\n");
                    }
                });

                Clipboard.copy($(".copy-product-id").text());
                $(".copy-product-id").remove();
            }

            function downloadProductShopee(productSKU) {

                if (!productSKU)
                    _alterError(titleAlert, { message: "Chưa chọn sản phẩm nào!" });

                window.open("/api/v1/shopee/product/export/excel?productSKU=" + productSKU, "_self");

            }
        </script>
    </main>
</asp:Content>
