<%@ Page Title="Máy tính tiền" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="pos.aspx.cs" Inherits="IM_PJ.pos" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/App_Themes/Ann/js/search-customer.js?v=2111"></script>
    <script src="/App_Themes/Ann/js/search-product.js?v=07122018"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="parent" runat="server">
        <main id="main-wrap">
            <div class="container">
                <div class="row">
                    <div class="col-md-4">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Khách hàng</h3>
                                <a href="javascript:;" class="search-customer" onclick="searchCustomer()" title="Tìm khách hàng"><i class="fa fa-search" aria-hidden="true"></i> Tìm (F1)</a>
                                <a href="javascript:;" class="change-user" onclick="changeUser()" title="Tính tiền giúp nhân viên khác"><i class="fa fa-user" aria-hidden="true"></i></a>
                                <a href="/danh-sach-don-hang" class="change-user" target="_blank" title="Danh sách đơn hàng"><i class="fa fa-list-ul" aria-hidden="true"></i></a>
                            </div>
                            <div class="panel-body">
                                <div class="form-group">
                                    <label>Họ tên</label>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFullname" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtFullname" CssClass="form-control capitalize" runat="server" placeholder="(F2)" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Điện thoại</label>
                                    <asp:RequiredFieldValidator ID="re" runat="server" ControlToValidate="txtPhone" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtPhone" CssClass="form-control" onblur="ajaxCheckCustomer()" runat="server" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Nick đặt hàng</label>
                                    <asp:TextBox ID="txtNick" CssClass="form-control capitalize" runat="server" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Địa chỉ</label>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAddress" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtAddress" CssClass="form-control capitalize" runat="server" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="form-row view-detail" style="display: none">
                                </div>
                                <div class="form-row discount-info" style="display: none">
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
                                    <telerik:RadNumericTextBox runat="server" CssClass="form-control width-notfull" Skin="MetroTouch"
                                        ID="pDiscount" MinValue="0" NumberFormat-GroupSizes="3" Width="100%" Value="0" NumberFormat-DecimalDigits="0"
                                        oninput="countTotal()" IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
                                    </telerik:RadNumericTextBox>
                                </div>
                            </div>
                            <div class="post-row clear discount hide">
                                <div class="left">Sau chiết khấu</div>
                                <div class="right priceafterchietkhau"></div>
                            </div>
                            <div class="post-row clear shipping hide">
                                <div class="left">Phí vận chuyển
                                    
                                </div>
                                <div class="right totalDiscount">
                                    <telerik:RadNumericTextBox runat="server" CssClass="form-control width-notfull" Skin="MetroTouch"
                                        ID="pFeeShip" MinValue="0" NumberFormat-GroupSizes="3" Width="100%" Value="0" NumberFormat-DecimalDigits="0"
                                        oninput="countTotal()" IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
                                    </telerik:RadNumericTextBox>
                                </div>
                            </div>
                            <div class="post-row clear otherfee hide">
                                <div class="left"><span class="otherfee-name"></span><a href="javascript:;" style="text-decoration: underline; float: right; font-size: 12px; font-style: italic; padding-left: 10px;" onclick="removeOtherFee()">(Xóa)</a></div>
                                <div class="right otherfee-value">
                                    <asp:TextBox ID="txtOtherFeeName" CssClass="form-control" runat="server" Style="display: none" ></asp:TextBox>
                                    <telerik:RadNumericTextBox runat="server" CssClass="form-control width-notfull" Skin="MetroTouch"
                                        ID="pOtherFee" NumberFormat-GroupSizes="3" Width="100%" Value="0" NumberFormat-DecimalDigits="0"
                                        oninput="countTotal()" IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
                                    </telerik:RadNumericTextBox>
                                </div>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Tổng tiền</div>
                                <div class="right totalpriceorderall price-red"></div>
                            </div>
                            <div class="post-row clear returnorder hide">
                                <div class="left">
                                    Đơn hàng trả
                                    <a href="javascript:;" class="find3 hide" style="text-decoration: underline; float: right; font-size: 12px; font-style: italic; padding-left: 10px;" onclick="searchReturnOrder()">(Tìm đơn khác)</a>
                                    <a href="javascript:;" class="find3 hide" style="text-decoration: underline; float: right; font-size: 12px; font-style: italic; padding-left: 10px;" onclick="deleteReturnOrder()">(Bỏ qua)</a>
                                    <a href="javascript:;" class="find2 hide" style="text-decoration: underline; float: right; font-size: 12px; font-style: italic; padding-left: 10px;"></a>
                                </div>
                                <div class="right totalpriceorderrefund"></div>
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
                                <a href="javascript:;" class="btn link-btn" style="background-color: #ffad00" onclick="searchReturnOrder()" title="Nhập đơn hàng đổi trả"><i class="fa fa-refresh"></i> Đổi trả</a>
                                <a href="javascript:;" class="btn link-btn" style="background-color: #00a2b7" onclick="showShipping()" title="Nhập phí vận chuyển"><i class="fa fa-truck"></i> Vận chuyển</a>
                                <a href="javascript:;" class="btn link-btn" style="background-color: #453288" onclick="showDiscount()" title="Nhập chiết khấu mỗi cái"><i class="fa fa-tag"></i> Chiết khấu</a>
                                <a href="javascript:;" class="btn link-btn" style="background-color: #607D8B;" onclick="addOtherFee()" title="Thêm phí khác vào đơn hàng"><i class="fa fa-plus"></i> Thêm phí</a>
                            </div>
                            <div class="post-table-links clear">
                                <a href="javascript:;" class="btn link-btn btn-complete-order" onclick="payAll()" title="Hoàn tất đơn hàng"><i class="fa fa-floppy-o"></i> Thanh toán (F9)</a>
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
                                <div class="search-box left" style="width: 80%;">
                                    <input type="text" id="txtSearch" class="form-control sku-input" placeholder="SKU (F3)" autocomplete="off">
                                </div>
                                <div class="right">
                                    <a href="javascript:;" class="link-btn" onclick="searchProduct()" title="Tìm sản phẩm"><i class="fa fa-search"></i></a>
                                    <a href="/quan-ly-nhap-kho" class="link-btn" target="_blank" title="Nhập kho"><i class="fa fa-cube"></i></a>
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
            <asp:HiddenField ID="hdfUsername" runat="server" />
            <asp:HiddenField ID="hdfUsernameCurrent" runat="server" />
            <asp:HiddenField ID="hdfOrderType" runat="server" />
            <asp:HiddenField ID="hdfTotalPrice" runat="server" />
            <asp:HiddenField ID="hdfTotalPriceNotDiscount" runat="server" />
            <asp:HiddenField ID="hdfListProduct" runat="server" />
            <asp:HiddenField ID="hdfIsDiscount" runat="server" />
            <asp:HiddenField ID="hdfDiscountAmount" runat="server" />
            <asp:HiddenField ID="hdfTotalPriceNotDiscountNotFee" runat="server" />
            <asp:HiddenField ID="hdfListSearch" runat="server" />
            <asp:HiddenField ID="hdfTotalQuantity" runat="server" />
            <asp:HiddenField ID="hdfcheck" runat="server" />
            <asp:HiddenField ID="hdfChietKhau" runat="server" />
            <asp:HiddenField ID="hdfListUser" runat="server" />
            <asp:HiddenField ID="hdfDonHangTra" runat="server" />
            <asp:HiddenField ID="hdfTongTienConLai" runat="server" />
            <asp:HiddenField ID="hdSession" runat="server" />
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

        @media (min-width: 992px){
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
        <script type="text/javascript">
            "use strict";

            // set height for div product list
            $(".search-product-content").height($(window).height() - 150);

            // hide header in template
            $("#header").html("");

            // search Product by SKU
            $("#txtSearch").keydown(function(event) {
                if (event.which === 13) {
                    searchProduct();
                    event.preventDefault();
                    return false;
                }
            });

            // focus to searchProduct input when page on ready
            $(document).ready(function() {
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

            });

            // check data before close page or refresh page
            function stopNavigate(event) {
                $(window).off('beforeunload');
            }

            $(window).bind('beforeunload', function(e) {
                if ($(".product-result").length > 0 || $("#<%=txtPhone.ClientID%>").val() != "" || $("#<%= txtFullname.ClientID%>").val() != "") return true;
                else e = null;
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
                if (e.which == 120) { //F9 Pay all
                    payAll();
                    return false;
                }
            });

            // quickly input change
            $("#<%=pGuestPaid.ClientID%>").keydown(function(e) {
                if (e.which == 13) {
                    var value = $("#<%=pGuestPaid.ClientID%>").val() + "000";
                    $("#<%=pGuestPaid.ClientID%>").val(value);
                    getAllPrice();
                    $("#<%=pGuestPaid.ClientID%>").blur();
                    return false;
                }
            });

            function changeUser() {
                var listUser = $("#<%=hdfListUser.ClientID%>").val();

                var list = listUser.split('|');
                var item = "<select id=\"listUser\" class=\"form-control fjx\">";
                for (var i = 0; i < list.length - 1; i++) {
                    item += "<option value=\"" + list[i] + "\">" + list[i] + "</option>";
                }
                item += "</select>";

                var html = "";
                html += "<div class=\"form-group\">";
                html += "<label>Chọn nhân viên khác: </label>";
                html += item;
                html += "<a href=\"javascript: ;\" class=\"btn link-btn\" style=\"background-color:#f87703;float:right;color:#fff;\" onclick=\"setNewUser()\">Chọn</a>";
                html += "</div>";
                showPopup(html);
            }

            function setNewUser() {
                var selectedUser = $("#listUser").val();
                $("#<%=hdfUsernameCurrent.ClientID%>").val(selectedUser);
                $("#<%=hdfUsername.ClientID%>").val(selectedUser);
                $("#<%=txtPhone.ClientID%>").val("");
                $("#<%=txtFullname.ClientID%>").val("");
                $("#<%=txtAddress.ClientID%>").val("");
                $("#<%=txtNick.ClientID%>").val("");
                closePopup();
                swal("Thông báo", "Bạn đã chọn tính tiền giúp nhân viên " + selectedUser + "", "info");
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
                    html += "<a href=\"javascript: ;\" class=\"btn link-btn\" style=\"background-color:#f87703;float:right;color:#fff;\" onclick=\"getReturnOrder()\">Tìm</a>";
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
                if (!isBlank(order)) {
                    $.ajax({
                        type: "POST",
                        url: "/pos.aspx/getReturnOrder",
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
                            } else {
                                $("#<%=hdSession.ClientID%>").val(1);
                                swal("Thông báo", "Đơn hàng đổi trả không thuộc khách hàng này!", "error");
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
                    url: "/pos.aspx/getReturnOrder",
                    data: "{order:'1', remove:'1'}",
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
                    },
                    error: function(xmlhttprequest, textstatus, errorthrow) {
                        alert('lỗi');
                    }
                });
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

            function addOtherFee() {
                swal({
                    title: "Thêm phí khác",
                    text: 'Nhập tên loại phí:',
                    type: 'input',
                    showCancelButton: true,
                    closeOnConfirm: false,
                }, function (otherFeeName) {
                    swal({
                        title: "Thêm phí khác",
                        text: 'Nhập số tiền:',
                        type: 'input',
                        showCancelButton: true,
                        closeOnConfirm: true,
                    }, function (otherFeeValue) {
                        if (otherFeeValue != false) {
                            $(".subtotal").removeClass("hide");
                            $(".otherfee-name").html(otherFeeName);
                            $("#<%=txtOtherFeeName.ClientID%>").val(otherFeeName);
                            $("#<%=pOtherFee.ClientID%>").val(otherFeeValue).focus();
                            $(".otherfee").removeClass("hide");
                            getAllPrice();
                        }
                    });
                });
            }

            function removeOtherFee() {
                $(".otherfee").addClass("hide");
                $(".otherfee-name").html("");
                $("#<%=txtOtherFeeName.ClientID%>").val("");
                $("#<%=pOtherFee.ClientID%>").val(0).focus();
                getAllPrice();
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

            function clickSubmit() {
                var totalquantity = $("#<%=hdfTotalQuantity.ClientID%>").val();
                var inputquantity = $("#SoldQuantity").val();

                if (totalquantity == inputquantity) {
                    closePopup();
                    HoldOn.open();
                    $("#<%=btnOrder.ClientID%>").click();
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

            // pay order on click button
            function payAll() {
                var phone = $("#<%=txtPhone.ClientID%>").val();
                var name = $("#<%=txtFullname.ClientID%>").val();
                var address = $("#<%=txtAddress.ClientID%>").val();
                if (phone != "" && name != "" && address != "" && phone.length <= 15) {
                    if ($(".product-result").length > 0) {
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
                        showConfirmOrder();
                    } else {
                        $("#txtSearch").focus();
                        swal("Thông báo", "Hãy nhập sản phẩm!", "error");
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
                    else if(phone.length > 15){
                        $("#<%= txtPhone.ClientID%>").focus();
                        swal("Thông báo", "Số điện thoại không được nhập quá 15 ký tự!", "error");
                    }
                    else if (address == "") {
                        $("#<%= txtAddress.ClientID%>").focus();
                        swal("Thông báo", "Hãy nhập địa chỉ khách hàng!", "error");
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

            // get all price
            function getAllPrice() {
                if ($(".product-result").length > 0) {

                    var totalprice = 0;
                    var productquantity = 0;
                    $(".product-result").each(function() {
                        var price = parseFloat($(this).find(".gia-san-pham").attr("data-price"));
                        var quantity = parseFloat($(this).find(".in-quantity").val());

                        var total = price * quantity;
                        $(this).find(".totalprice-view").html(formatThousands(total, '.'));
                        productquantity += quantity;
                        totalprice += total;
                    });
                    $("#<%=hdfTotalPriceNotDiscount.ClientID%>").val(totalprice);
                    $(".totalproductQuantity").html(formatThousands(productquantity, ',') + " sản phẩm");
                    $("#<%=hdfTotalPriceNotDiscountNotFee.ClientID%>").val(totalprice);
                    $(".totalpriceorder").html(formatThousands(totalprice, ','));
                    $("#<%=hdfTotalQuantity.ClientID%>").val(productquantity);
                    var totalDiscount = 0;
                    var totalleft = 0;
                    var totalck = 0;
                    var amount = 0;
                    var amountdiscount = parseFloat($("#<%=hdfDiscountAmount.ClientID%>").val());

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

                    if (amountdiscount > 0) {
                        if (amount < amountdiscount) {
                            amount = amountdiscount;
                        }
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

                    var of = $("#<%=pOtherFee.ClientID%>").val();
                    var otherfee = parseFloat(of.replace(',', ''));

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
                    countGuestChange();
                } else {

                    $(".totalproductQuantity").html(formatThousands(0, ',') + " sản phẩm");
                    $(".totalpriceorder").html(formatThousands(0, ','));
                    $(".totalGuestChange").html(formatThousands(0, ','));
                    $(".totalpriceorderall").html(formatThousands(0, ','));
                    $(".priceafterchietkhau").html(formatThousands(0, ','));
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
                if ($("#<%=pOtherFee.ClientID%>").val() == '') {
                    var fee = 0;
                    $("#<%=pOtherFee.ClientID%>").val(formatThousands(fee, ','));
                }

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

                var of = $("#<%=pOtherFee.ClientID%>").val();
                var otherfee = parseFloat(of.replace(/\,/g, ''));

                $("#<%=hdfcheck.ClientID%>").val(discount);

                var totalleft = total + feeship + otherfee - discount * quantity;
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
        </script>
    </telerik:RadScriptBlock>
</asp:Content>
