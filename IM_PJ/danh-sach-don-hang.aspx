﻿<%@ Page Title="Danh sách đơn hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="danh-sach-don-hang.aspx.cs" Inherits="IM_PJ.danh_sach_don_hang" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .search-box-type .col-1, .search-box-type .col-2 {
            float: left;
        }
        .search-box-type .col-1 {
            width: calc(100% - 115px);
        }
        .search-box-type .col-1 input {
            border-right: none;
        }
        .search-box-type .col-2 {
            width: 115px;
        }
        .search-box-type .col-2 select {
            border-left: none;
            background-color: #f7f7f7!important;
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
        @media (max-width: 768px) {
            table.shop_table_responsive thead {
	            display: none;
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(1):before {
                content: "#";
                font-size: 20px;
                margin-right: 2px;
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(1) {
                text-align: left;
                font-size: 20px;
                font-weight: bold;
                height: 50px;
            }
            table.shop_table_responsive > tbody > tr:nth-of-type(2n) td {
                border-top: none;
                border-bottom: none!important;
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
                    <h3 class="page-title left">Đơn hàng <span>(<asp:Literal ID="ltrNumberOfOrder" runat="server" EnableViewState="false"></asp:Literal>)</span></h3>
                    <div class="right above-list-btn">
                        <a href="/them-moi-don-hang" class="h45-btn primary-btn btn">Thêm mới</a>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-3 col-xs-12 search-box-type">
                                    <div class="col-1">
                                        <asp:TextBox ID="txtSearchOrder" runat="server" CssClass="form-control" placeholder="Tìm đơn hàng" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="col-2">
                                        <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="1" Text="Đơn hàng"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Sản phẩm"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlDiscount" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Chiết khấu"></asp:ListItem>
                                        <asp:ListItem Value="yes" Text="Có chiết khấu"></asp:ListItem>
                                        <asp:ListItem Value="no" Text="Không chiết khấu"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlOtherFee" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Phí khác"></asp:ListItem>
                                        <asp:ListItem Value="yes" Text="Có phí khác"></asp:ListItem>
                                        <asp:ListItem Value="no" Text="Không phí khác"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <label>Từ ngày</label>
                                    <telerik:RadDatePicker RenderMode="Lightweight" ID="rOrderFromDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate">
                                        <DateInput DisplayDateFormat="dd/MM/yyyy" runat="server">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <label>Đến ngày</label>
                                    <telerik:RadDatePicker RenderMode="Lightweight" ID="rOrderToDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate">
                                        <DateInput DisplayDateFormat="dd/MM/yyyy" runat="server">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </div>
                                <div class="col-md-1 col-xs-6 search-button">
                                    <a href="javascript:;" onclick="searchOrder()" class="btn primary-btn h45-btn"><i class="fa fa-search"></i></a>
                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn primary-btn h45-btn" OnClick="btnSearch_Click" Style="display: none" />
                                    <a href="/danh-sach-don-hang" class="btn primary-btn h45-btn"><i class="fa fa-times" aria-hidden="true"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-1 col-xs-6">
                                    <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Loại"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Lẻ"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Sỉ"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlExcuteStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Xử lý"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Đang xử lý"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Đã hoàn tất"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Đã hủy"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlPaymentStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Thanh toán"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Chưa thanh toán"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Thanh toán thiếu"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Đã thanh toán"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Đã duyệt"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Kiểu thanh toán"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Tiền mặt"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Chuyển khoản"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Thu hộ"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Công nợ"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlShippingType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Kiểu giao hàng"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Lấy trực tiếp"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Chuyển bưu điện"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Dịch vụ Proship"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Chuyển xe"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="Nhân viên giao"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="GHTK"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="Viettel"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlCreatedBy" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-1">
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlOrderNote" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Ghi chú"></asp:ListItem>
                                        <asp:ListItem Value="yes" Text="Có ghi chú"></asp:ListItem>
                                        <asp:ListItem Value="no" Text="Không ghi chú"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlShipperFilter" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlTransportCompany" runat="server" CssClass="form-control select2" Width="100%"></asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlQuantityFilter" runat="server" CssClass="form-control" onchange="changeQuantityFilter($(this))">
                                        <asp:ListItem Value="" Text="Số lượng mua"></asp:ListItem>
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
                                <div class="col-md-1 col-xs-6">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-table clear">
                        <div class="panel-footer clear">
                            <div class="pagination">
                                <%this.DisplayHtmlStringPaging1();%>
                            </div>
                        </div>
                        <div class="responsive-table">
                            <table class="table table-checkable table-product table-new-product shop_table_responsive">
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
        
        <!-- Modal phí khác -->
        <div class="modal fade" id="feeInfoModal" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Các loại phí khác</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <table class="table table-striped">
                                <thead>
                                    <tr>
                                    <th>Tên loại phí</th>
                                    <th>Số tiền</th>
                                    </tr>
                                </thead>
                                <tbody id="feeInfo">
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                    </div>
                </div>
            </div>
        </div>

        <script type="text/javascript">
            $("#<%=txtSearchOrder.ClientID%>").keyup(function (e) {
                if (e.keyCode == 13)
                {
                    $("#<%= btnSearch.ClientID%>").click();
                }
            });

            // Parse URL Queries
            function url_query(query) {
                query = query.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
                var expr = "[\\?&]" + query + "=([^&#]*)";
                var regex = new RegExp(expr);
                var results = regex.exec(window.location.href);
                if (results !== null) {
                    return results[1];
                } else {
                    return false;
                }
            }

            var url_param = url_query('quantityfilter');
            if (url_param) {
                if (url_param == "greaterthan" || url_param == "lessthan") {
                    $(".greaterthan").removeClass("hide");
                    $(".between").addClass("hide");
                }
                else if (url_param == "between") {
                    $(".between").removeClass("hide");
                    $(".greaterthan").addClass("hide");
                }
            }

            function changeQuantityFilter(obj) {
                var value = obj.val();
                if (value == "greaterthan" || value == "lessthan") {
                    $(".greaterthan").removeClass("hide");
                    $(".between").addClass("hide");
                    $("#<%=txtQuantity.ClientID%>").focus().select();
                }
                else if (value == "between") {
                    $(".between").removeClass("hide");
                    $(".greaterthan").addClass("hide");
                    $("#<%=txtQuantityMin.ClientID%>").focus().select();
                }
            }

            function searchOrder() {
                $("#<%= btnSearch.ClientID%>").click();
            }

            var formatThousands = function (n, dp) {
                var s = '' + (Math.floor(n)), d = n % 1, i = s.length, r = '';
                while ((i -= 3) > 0) { r = ',' + s.substr(i, 3) + r; }
                return s.substr(0, i + 3) + r +
                    (d ? '.' + Math.round(d * Math.pow(10, dp || 2)) : '');
            };

            function createFeeInfoHTML(fee, is_total) {
                if (!is_total) {
                    is_total = false;
                }
                let addHTML = "";

                if (is_total) {
                    addHTML += "<tr class='info'>";
                    addHTML += "    <td style='text-align: right'>" + fee.FeeTypeName + "</td>";
                    addHTML += "    <td>" + formatThousands(fee.FeePrice) + "</td>";
                    addHTML += "</tr>";
                }
                else {
                    addHTML += "<tr>";
                    addHTML += "    <td>" + fee.FeeTypeName + "</td>";
                    addHTML += "    <td>" + formatThousands(fee.FeePrice) + "</td>";
                    addHTML += "</tr>";
                }

                return addHTML;
            }

            function openFeeInfoModal(orderID) {
                let tbodyDOM = $("tbody[id='feeInfo']");
                // Clear body
                tbodyDOM.html("");
                $.ajax({
                    type: "POST",
                    url: "/danh-sach-don-hang.aspx/getFeeInfo",
                    data: JSON.stringify({ 'orderID': orderID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (response) => {
                        if (response.d) {
                            let data = JSON.parse(response.d);
                            let feeTotal = 0;
                            data.forEach((item) => {
                                feeTotal += item.FeePrice;
                                tbodyDOM.append(createFeeInfoHTML(item));
                            });
                            tbodyDOM.append(createFeeInfoHTML({ "FeeTypeName": "Tổng", "FeePrice": feeTotal }, true));
                        }
                    },
                    error: (xmlhttprequest, textstatus, errorthrow) => {
                        swal("Thông báo", "Có lỗi trong quá trình lấy thông tin phí", "error");
                    }
                })
            }
        </script>
    </main>
</asp:Content>
