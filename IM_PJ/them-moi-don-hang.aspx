<%@ Page Title="Thêm mới đơn hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="them-moi-don-hang.aspx.cs" Inherits="IM_PJ.them_moi_don_hang" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/App_Themes/Ann/js/search-customer.js?v=2115"></script>
    <script src="/App_Themes/Ann/js/search-product.js?v=07122018"></script>
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
                                <a href="javascript:;" class="search-customer" onclick="searchCustomer()"><i class="fa fa-search" aria-hidden="true"></i>Tìm khách hàng (F1)</a>
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
                                    <a class="btn btn-feeship link-btn" href="javascript:;" id="calfeeship" onclick="calFeeShip()"><i class="fa fa-check-square-o" aria-hidden="true"></i>Miễn phí</a>
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
                            <div class="post-table-links clear">
                                <a href="javascript:;" class="btn link-btn" id="payall" style="background-color: #f87703; float: right" title="Hoàn tất đơn hàng" onclick="payAll()"><i class="fa fa-floppy-o"></i>Xác nhận</a>
                                <asp:Button ID="btnOrder" runat="server" OnClick="btnOrder_Click" Style="display: none" />
                                <a href="javascript:;" class="btn link-btn" style="background-color: #ffad00; float: right;" title="Nhập đơn hàng đổi trả" onclick="searchReturnOrder()"><i class="fa fa-refresh"></i>Đổi trả</a>
                                <a id="feeNewStatic" href="#feeModal" class="btn link-btn" style="background-color: #607D8B; float: right;" title="Thêm phí khác vào đơn hàng" data-toggle="modal" data-backdrop='static'><i class="fa fa-plus"></i>Thêm phí khác</a>
                                <a href="javascript:;" class="btn link-btn" style="background-color: #F44336; float: right;" title="Xóa tất cả sản phẩm" onclick="deleteProduct()"><i class="fa fa-times" aria-hidden="true"></i>Làm lại</a>
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
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #f87703; float: right" title="Hoàn tất đơn hàng" onclick="payAll()"><i class="fa fa-floppy-o"></i>Xác nhận</a>
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #ffad00; float: right;" title="Nhập đơn hàng đổi trả" onclick="searchReturnOrder()"><i class="fa fa-refresh"></i>Đổi trả</a>
                                    <a id="feeNewDynamic" href="#feeModal" class="btn link-btn" style="background-color: #607D8B; float: right;" title="Thêm phí khác vào đơn hàng" data-toggle="modal" data-backdrop='static'><i class="fa fa-plus"></i>Thêm phí khác</a>
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #F44336; float: right;" title="Xóa tất cả sản phẩm" onclick="deleteProduct()"><i class="fa fa-times" aria-hidden="true"></i>Làm lại</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="notAcceptChangeUser" Value="1" runat="server" />
            <asp:HiddenField ID="hdfUsername" runat="server" />
            <asp:HiddenField ID="hdfUsernameCurrent" runat="server" />
            <asp:HiddenField ID="hdfOrderType" runat="server" />
            <asp:HiddenField ID="hdfTotalPrice" runat="server" />
            <asp:HiddenField ID="hdfTotalPriceNotDiscount" runat="server" />
            <asp:HiddenField ID="hdfListProduct" runat="server" />
            <asp:HiddenField ID="hdfPaymentStatus" runat="server" />
            <asp:HiddenField ID="hdfExcuteStatus" runat="server" />
            <asp:HiddenField ID="hdfPaymentType" runat="server" />
            <asp:HiddenField ID="hdfShippingType" runat="server" />
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
            <asp:HiddenField ID="hdfOtherFees" runat="server" />

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
                                <div class="col-xs-5">
                                    <asp:DropDownList ID="ddlFeeType" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-xs-3">
                                    <asp:DropDownList ID="ddlPriceType" runat="server" CssClass="form-control"></asp:DropDownList>
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
        </main>
    </asp:Panel>
    <style>
        .search-product-content {
            height: initial !important;
            min-height: 200px;
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
            padding: 20px 20px;
            right: 0%;
            margin: 0 auto;
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
            var fees = [];

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
                let priceTypeDOM = $("#<%=ddlPriceType.ClientID%>");
                let feePriceDOM = $("#<%=txtFeePrice.ClientID%>");

                // Init
                idDOM.val("");
                feeTypeDOM.val(0);
                priceTypeDOM.val(1);
                feePriceDOM.val("");
                if (!is_new)
                {
                    let parent = obj.parent();
                    if (parent.attr("id") != undefined)
                        idDOM.val(parent.attr("id"));
                    if (parent.data("feeid") != "")
                        feeTypeDOM.val(parent.data("feeid"));
                    if (parent.data("pricetype") != "")
                        priceTypeDOM.val(parent.data("pricetype"));
                    if (parent.data("price") != "")
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
                    let positiveNumber = fee.FeePrice > 0 ? "1" : "0";
                    let negative = fee.FeePrice > 0 ? "" : "-";

                    addHTML += "<div id='" + fee.UUID + "' class='post-row clear otherfee' data-feeid='" + fee.FeeTypeID + "' data-pricetype='" + positiveNumber + "' data-price='" + fee.FeePrice + "'>";
                    addHTML += "    <div class='left'>";
                    addHTML += "        <span class='otherfee-name'>" + fee.FeeTypeName + "</span>";
                    addHTML += "        <a href='javascript:;' style='text-decoration: underline; float: right; font-size: 12px; font-style: italic; padding-left: 10px;' onclick='removeOtherFee(`" + fee.UUID + "`)'>";
                    addHTML += "            (Xóa)";
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
                let pricetype = $("#<%=ddlPriceType.ClientID%>").val();
                let isNegative = pricetype == "1" ? "" : "-";
                let feeprice = $("#<%=txtFeePrice.ClientID%>").val().replace(/\,/g, '');
                let fee = new Fee(id, feeid, feename, parseInt(isNegative + feeprice));

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
                let pricetype = $("#<%=ddlPriceType.ClientID%>").val();
                let isNegative = pricetype == "1" ? "" : "-";
                let feeprice = $("#<%=txtFeePrice.ClientID%>").val().replace(/\,/g, '');
                let parent = $("#" + id);

                parent.data("feeid", feeid);
                parent.data("pricetype", pricetype);
                parent.data("feeprice", feeprice);

                parent.find("span.otherfee-name").html(feename);
                parent.find("#feePrice").val(isNegative + formatNumber(feeprice));
                
                fees.forEach((fee) => {
                    if(fee.UUID == id)
                    {
                        fee.FeeTypeID = feeid;
                        fee.FeeTypeName = pricetype;
                        fee.FeePrice = pricetype == 1 ? parseInt(feeprice) : parseInt(feeprice) * (-1);
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

            function removeOtherFee(uuid) {
                $("#" + uuid).remove();
                fees = fees.filter((item) => { return item.UUID != uuid; });
                $("#<%=hdfOtherFees.ClientID%>").val(JSON.stringify(fees));
                getAllPrice();
            }

            // search return order
            function searchReturnOrder() {
                var phone = $("#<%=txtPhone.ClientID%>").val();
                var name = $("#<%=txtFullname.ClientID%>").val();
                if (isBlank(phone) || isBlank(name))
                {
                    swal("Thông báo", "Hãy nhập thông tin khách hàng trước!", "info");
                }
                else
                {
                    var html = "";
                    html += "<div class=\"form-row\">";
                    html += "<label>Mã đơn hàng đổi trả: </label>";
                    html += "<input ID=\"txtOrderRefund\" class=\"form-control fjx\"></input>";
                    html += "<a href=\"javascript:;\" class=\"btn primary-btn float-right-btn link-btn\" onclick=\"getReturnOrder()\"><i class=\"fa fa-search\" aria-hidden=\"true\"></i> Tìm</a>";
                    html += "</div>";
                    showPopup(html);
                    $("#txtOrderRefund").focus();
                    $('#txtOrderRefund').keydown(function(event) {
                        if (event.which === 13)
                        {
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
                if (!isBlank(order))
                {
                    $.ajax({
                        type: "POST",
                        url: "/them-moi-don-hang.aspx/getReturnOrder",
                        data: "{order:'" + order + "', remove:'0'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function(msg) {
                            if (msg.d != "null")
                            {
                                var data = JSON.parse(msg.d);
                                $("#<%=hdSession.ClientID%>").val(data.ID + "|" + data.TotalPrice);
                            
                                if (data.CustomerName == name && data.CustomerPhone == phone)
                                {
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
                                }
                                else
                                {
                                    swal("Thông báo", "Đơn hàng đổi trả không thuộc khách hàng này!", "error");
                                }
                            }
                            else
                            {
                                $("#<%=hdSession.ClientID%>").val(1)
                                swal("Thông báo", "Đơn hàng đổi trả không tồn tại hoặc đã được trừ tiền!", "error");
                            }
                        },
                        error: function(xmlhttprequest, textstatus, errorthrow) {
                            alert('lỗi');
                        }
                    });
                }
                else
                {
                    swal("Thông báo", "Hãy nhập thông tin khách hàng trước!", "prompt");
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
                    url: "/them-moi-don-hang.aspx/getReturnOrder",
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

                            showOrderStatus();
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
                
                var paymentStatus = $(".payment-status").val();

                var excuteStatus = 1;
                if ($(".excute-status").length > 0)
                    excuteStatus = $(".excute-status").val();

                var paymenttype = 2;
                if ($(".payment-type").length > 0)
                    paymenttype = $(".payment-type").val();

                var shippingtype = 4;
                if ($(".shipping-type").length > 0)
                    shippingtype = $(".shipping-type").val();

                $("#<%=hdfPaymentStatus.ClientID%>").val(paymentStatus);
                $("#<%=hdfExcuteStatus.ClientID%>").val(excuteStatus);
                $("#<%=hdfPaymentType.ClientID%>").val(paymenttype);
                $("#<%=hdfShippingType.ClientID%>").val(shippingtype);

                $("#payall").addClass("payall-clicked");

                if (shippingtype == 2 || shippingtype == 3)
                {
                    if ($("#<%=pFeeShip.ClientID%>").val() == 0 && $("#<%=pFeeShip.ClientID%>").is(":disabled") == false)
                    {
                        closePopup();
                        $("#<%=pFeeShip.ClientID%>").focus();
                        swal({
                            title: "Ủa ủa:",
                            text: "Sao không nhập phí vận chuyển?<br><br>Hỏng lẻ miễn phí vận chuyển?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "Khoan.. để em nhập phí!!",
                            closeOnConfirm: false,
                            cancelButtonText: "Để em tạm thời cho miễn phí",
                            html: true
                        });
                    }
                    else
                    {
                        HoldOn.open();
                        $("#<%=btnOrder.ClientID%>").click();
                    }
                }
                else
                {
                    HoldOn.open();
                    $("#<%=btnOrder.ClientID%>").click();
                }
            }

            // show popup Order status
            function showOrderStatus() {
                var fr = "         <div class=\"form-row\">";
                fr += "             <h2>Hoàn tất đơn hàng</h2>";
                fr += "         </div>";
                fr += "         <div class=\"form-row\">";
                fr += "             <div class=\"row-left\">Trạng thái xử lý:</div>";
                fr += "             <div class=\"row-right\">"
                fr += "                 <select class=\"form-control excute-status\">";
                fr += "                     <option value=\"1\">Đang xử lý</option>";
                fr += "                     <option value=\"2\">Đã hoàn tất</option>";
                fr += "                 </select>";
                fr += "             </div>";
                fr += "         </div>";
                fr += "         <div class=\"form-row\">";
                fr += "             <div class=\"row-left\">Trạng thái thanh toán:</div>";
                fr += "             <div class=\"row-right\">"
                fr += "                 <select class=\"form-control payment-status\">";
                fr += "                     <option value=\"1\">Chưa thanh toán</option>";
                fr += "                     <option value=\"2\">Thanh toán thiếu</option>";
                fr += "                     <option value=\"3\">Đã thanh toán</option>";
                fr += "                 </select>";
                fr += "             </div>";
                fr += "         </div>";
                fr += "         <div class=\"form-row\">";
                fr += "             <div class=\"row-left\">Phương thức thanh toán:</div>";
                fr += "             <div class=\"row-right\">"
                fr += "                 <select class=\"form-control payment-type\">";
                fr += "                     <option value=\"1\">Tiền mặt</option>";
                fr += "                     <option value=\"2\" selected>Chuyển khoản</option>";
                fr += "                     <option value=\"3\">Thu hộ</option>";
                fr += "                     <option value=\"4\">Công nợ</option>";
                fr += "                 </select>";
                fr += "             </div>";
                fr += "         </div>";
                fr += "         <div class=\"form-row\">";
                fr += "             <div class=\"row-left\">Phương thức giao hàng:</div>";
                fr += "             <div class=\"row-right\">"
                fr += "                 <select class=\"form-control shipping-type\">";
                fr += "                     <option value=\"1\">Lấy trực tiếp</option>";
                fr += "                     <option value=\"2\">Chuyển bưu điện</option>";
                fr += "                     <option value=\"3\">Dịch vụ ship</option>";
                fr += "                     <option value=\"4\" selected>Chuyển xe</option>";
                fr += "                     <option value=\"5\">Nhân viên giao hàng</option>";
                fr += "                 </select>";
                fr += "             </div>";
                fr += "         </div>";
                fr += "         <div class=\"btn-content\">";
                fr += "             <a class=\"btn primary-btn fw-btn not-fullwidth\" href=\"javascript:;\" onclick=\"insertOrder()\">Tạo đơn hàng</a>";
                fr += "         </div>";
                showPopup(fr);
            }

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
        </script>
    </telerik:RadScriptBlock>

</asp:Content>
