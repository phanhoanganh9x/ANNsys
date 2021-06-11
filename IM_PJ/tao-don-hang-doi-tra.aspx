<%@ Page Title="Thêm đơn trả hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="tao-don-hang-doi-tra.aspx.cs" Inherits="IM_PJ.tao_don_hang_doi_tra" EnableSessionState="ReadOnly" EnableEventValidation="false"%>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/App_Themes/Ann/js/search-customer.js?v=202106081515"></script>
    <script src="/Scripts/moment.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="parent" runat="server">
        <main id="main-wrap">
            <div class="container">
                <div class="row">
                    <div class="col-md-12">
                        <h3 class="page-title left">Đổi trả hàng</h3>
                    </div>
                </div>
                <div class="row">
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
                                            <asp:TextBox ID="txtFullname" CssClass="form-control capitalize" runat="server" autocomplete="off" placeholder="Họ tên thật của khách"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Điện thoại</label>
                                            <asp:TextBox ID="txtPhone" CssClass="form-control" runat="server" autocomplete="off" placeholder="Số điện thoại khách hàng"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Nick đặt hàng</label>
                                            <asp:TextBox ID="txtNick" CssClass="form-control capitalize" runat="server" autocomplete="off" placeholder="Tên nick đặt hàng"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Facebook</label>
                                            <div class="row">
                                                <div class="col-md-10 fb width-100">
                                                <asp:TextBox ID="txtFacebook" CssClass="form-control" runat="server" autocomplete="off" placeholder="Đường link chat Facebook"></asp:TextBox>
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
                                            <asp:TextBox ID="txtAddress" CssClass="form-control capitalize" runat="server" autocomplete="off" placeholder="Địa chỉ khách hàng"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-row view-detail" style="display: none">
                                </div>
                                <div class="form-row discount-info" style="display: none">
                                </div>
                                <div class="form-row refund-info" style="display: none">
                                </div>
                                <div class="form-row result-check">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel-post">
                            <div class="post-above clear">
                                <div class="search-box left">
                                    <input type="text" id="txtSearch" class="form-control sku-input" placeholder="NHẬP MÃ SẢN PHẨM (F3)" autocomplete="off">
                                </div>
                            </div>
                            <div class="post-body search-product-content clear">
                                <table class="table table-checkable table-product table-return-order">
                                    <thead>
                                        <tr>
                                            <th class="image-column">Ảnh</th>
                                            <th class="name-column">Sản phẩm</th>
                                            <th class="sku-column">Mã</th>
                                            <th class="price-column">Giá niêm yết</th>
                                            <th class="sold-price-column">Giá đã bán</th>
                                            <th class="quantity-column">Cần đổi</th>
                                            <th class="type-column">Hình thức</th>
                                            <th class="fee-column">Phí đổi hàng</th>
                                            <th class="total-column">Thành tiền</th>
                                            <th class="trash-column">Xóa</th>
                                        </tr>
                                    </thead>
                                    <tbody class="content-product">
                                    </tbody>
                                </table>
                            </div>
                            <div class="post-footer">
                                <div class="post-row clear">
                                    <div class="left">Số lượng</div>
                                    <div class="right totalProductQuantity"></div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">Số lượng miễn tiền phí</div>
                                    <div class="right totalProductQuantityNoFee"></div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">Phí đổi hàng</div>
                                    <div class="right totalRefund"></div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">Tổng tiền (đã trừ phí)</div>
                                    <div class="right totalPriceOrder"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Trạng thái đơn hàng</h3>
                            </div>
                            <div class="panel-body">
                                <div class="form-row">
                                    <div class="row-left">
                                        Trạng thái
                                    </div>
                                    <div class="row-right">
                                        <asp:DropDownList ID="ddlRefundStatus" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="1" Text="Chưa trừ tiền"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Đã trừ tiền"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="row-left">
                                        Ghi chú đơn hàng
                                    </div>
                                    <div class="row-right">
                                        <asp:TextBox ID="txtRefundsNote" runat="server" CssClass="form-control" placeholder="Ghi chú"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="panel-post">
                                    <div class="post-table-links clear">
                                        <a href="javascript:;" class="btn link-btn" style="background-color: #f87703; float: right;" onclick="payall()" id="payall"><i class="fa fa-floppy-o"></i> Xác nhận</a>
                                        <a href="javascript:;" class="btn link-btn" style="background-color: #F44336; float: right;" onclick="deleteProduct()"><i class="fa fa-times" aria-hidden="true"></i> Làm lại</a>
                                        <a href="javascript:;" class="btn link-btn minus-discount" style="background-color: #009688; float: right;" onclick="minusDiscount()"><i class="fa fa-arrow-down" aria-hidden="true"></i> Trừ chiết khấu</a>
                                        <a href="javascript:;" class="btn link-btn restore-discount hide" style="background-color: #009688; float: right;" onclick="restoreDiscount()"><i class="fa fa-arrow-up" aria-hidden="true"></i> Khôi phục giá bán cũ</a>
                                        <a href="javascript:;" class="btn link-btn" style="background-color: #607D8B; float: right;" onclick="changeFee()"><i class="fa fa-external-link" aria-hidden="true"></i> Nhập phí đổi hàng khác</a>
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
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #f87703; float: right;" onclick="payall()"><i class="fa fa-floppy-o"></i> Xác nhận</a>
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #F44336; float: right;" onclick="deleteProduct()"><i class="fa fa-times" aria-hidden="true"></i> Làm lại</a>
                                    <a href="javascript:;" class="btn link-btn minus-discount" style="background-color: #009688; float: right;" onclick="minusDiscount()"><i class="fa fa-arrow-down" aria-hidden="true"></i> Trừ chiết khấu</a>
                                    <a href="javascript:;" class="btn link-btn restore-discount hide" style="background-color: #009688; float: right;" onclick="restoreDiscount()"><i class="fa fa-arrow-up" aria-hidden="true"></i> Khôi phục giá bán cũ</a>
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #607D8B; float: right;" onclick="changeFee()"><i class="fa fa-external-link" aria-hidden="true"></i> Nhập phí đổi hàng khác</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Style="display: none" />
            <asp:HiddenField ID="hdRefundGoodsID" runat="server" />
            <asp:HiddenField ID="hdfUsername" runat="server" />
            <asp:HiddenField ID="hdfUsernameCurrent" runat="server" />
            <asp:HiddenField ID="hdfOrderSaleID" runat="server" />
            <asp:HiddenField ID="hdfPhone" runat="server" />
            <asp:HiddenField ID="hdfIsDiscount" runat="server" />
            <asp:HiddenField ID="hdfDiscountAmount" runat="server" />
            <asp:HiddenField ID="hdfCustomerFeeChange" runat="server" />
            <asp:HiddenField ID="hdfTotalPrice" runat="server" Value="0" />
            <asp:HiddenField ID="hdfTotalQuantity" runat="server" Value="0" />
            <asp:HiddenField ID="hdfTotalRefund" runat="server" Value="0" />
            <asp:HiddenField ID="hdfListProduct" runat="server" />
            <asp:HiddenField ID="hdfCustomerID" runat="server" />
            <asp:HiddenField ID="hdfDaysExchange" runat="server" />
            <asp:HiddenField ID="hdfRefundQuantityNoFee" runat="server" />
            <asp:HiddenField ID="hdfRefundQuantityFee" runat="server" />
            <asp:HiddenField ID="hdfProvinceID" runat="server" />
            <asp:HiddenField ID="hdfDistrictID" runat="server" />
            <asp:HiddenField ID="hdfWardID" runat="server" />
        </main>
    </asp:Panel>
    <style>
        .search-product-content {
            min-height: 200px;
            background: #fff;
        }
    </style>
    <telerik:RadAjaxManager ID="rAjax" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="parent" LoadingPanelID="rxLoading"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadScriptBlock ID="sc" runat="server">
        <script src="https://code.jquery.com/jquery-3.3.1.min.js" integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8=" crossorigin="anonymous"></script>

        <script type="text/javascript">

            function redirectTo(ID, username) {
                HoldOn.open();
                window.onbeforeunload = null;

                let usernameString = "";
                if (username != "")
                {
                    usernameString = "&username=" + username;
                }
                window.location.href = "/xem-don-hang-doi-tra?id=" + ID + usernameString;
            }

            // check data before close page or refresh page
            window.onbeforeunload = function () {
                if ($(".product-result").length > 0 || $("#<%=txtPhone.ClientID%>").val() != "" || $("#<%= txtFullname.ClientID%>").val() != "")
                        return "You're leaving the site.";
            };

            //disable input
            $("#<%= txtPhone.ClientID%>").prop('readonly', true);
            $("#<%= txtFullname.ClientID%>").prop('readonly', true);
            $("#<%= txtAddress.ClientID%>").prop('readonly', true);
            $("#<%= txtNick.ClientID%>").prop('readonly', true);
            $("#<%= txtFacebook.ClientID%>").prop('readonly', true);
            $("select[id$='_ddlProvince']").attr('disabled', true);
            $("select[id$='_ddlProvince']").attr('readonly', 'readonly');
            // Create model entity
            class RefundDetailModel{
                constructor(ProductID
                            , ProductVariableID
                            , ProductStyle
                            , ProductImage
                            , ProductTitle
                            , ParentSKU
                            , ChildSKU
                            , VariableValue
                            , Price
                            , ReducedPrice
                            , QuantityRefund
                            , ChangeType
                            , FeeRefund
                            , FeeRefundDefault
                            , TotalFeeRefund
                            , SaleDate
                            , OrderID) {
                    this.RowIndex = uuid.v4();
                    this.ProductID = ProductID;
                    this.ProductVariableID = ProductVariableID;
                    this.ProductStyle = ProductStyle;
                    this.ProductImage = ProductImage;
                    this.ProductTitle = ProductTitle;
                    this.ParentSKU = ParentSKU;
                    this.ChildSKU = ChildSKU;
                    this.VariableValue = VariableValue;
                    this.Price = Price;
                    this.ReducedPrice = ReducedPrice;
                    this.QuantityRefund = QuantityRefund;
                    this.ChangeType = ChangeType;
                    this.FeeRefund = FeeRefund;
                    this.FeeRefundDefault = FeeRefundDefault;
                    this.TotalFeeRefund = TotalFeeRefund;
                    this.SaleDate = SaleDate;
                    this.OrderID = OrderID;
                }

                isTarget(RowIndex, ProductID, ProductVariableID) {
                    return this.RowIndex == RowIndex && this.ProductID == ProductID && this.ProductVariableID == ProductVariableID;
                }

                stringJSON() {
                    return JSON.stringify(this);
                }
            }

            // Create model
            var productRefunds = [];
            var productDeleteRefunds = [];

            function minusDiscount() {
                swal({
                    title: "Trừ chiết khấu",
                    text: 'Nhập số tiền cần trừ:',
                    type: 'input',
                    showCancelButton: true,
                    closeOnConfirm: true,
                    cancelButtonText: "Để em xem lại",
                    confirmButtonText: "OK sếp!!",
                }, function (discount) {
                    if (typeof(discount) == "boolean") {
                        discount = 0;
                    }
                    else if (typeof(discount) == "string") {
                        discount = discount.replace(/,|\./g, "");
                        discount = +discount || 0;
                    }
                    else {
                        discount = 0;
                    }

                    $("input.reducedPrice").each(function (index) {
                        oldValue = $(this).val().replace(/,/g, "");

                        if (discount > 0) {
                            newValue = formatThousands(parseInt(oldValue) - discount);
                        }
                        else {
                            newValue = formatThousands(parseInt(oldValue) + discount);
                        }

                        $(this).val(newValue);
                        changeRow($(this));
                    });

                    $(".minus-discount").addClass("hide");
                    $(".restore-discount").removeClass("hide");
                });
            }

            function restoreDiscount() {
                swal({
                    title: "Khôi phục lại giá bán",
                    text: "Giá bán sẽ khôi phục lại như lúc đầu (giống giá niêm yết).. OK không?",
                    type: 'warning',
                    showCancelButton: true,
                    closeOnConfirm: true,
                    cancelButtonText: "Để em coi lại..",
                    confirmButtonText: "OK sếp !!!",
                }, function (isConfirm) {
                    if (isConfirm) {
                        $("input.reducedPrice").each(function (index) {
                            oldValue = $(this).parent().parent().find("td.Price").html();

                            $(this).val(oldValue);
                            changeRow($(this));
                        });

                        $(".minus-discount").removeClass("hide");
                        $(".restore-discount").addClass("hide");
                    }
                });
            }

            function changeFee() {
                swal({
                    title: "Nhập phí đổi hàng",
                    text: "Nhập <strong>phí đổi sản phẩm</strong> cho đơn này:",
                    type: "input",
                    showCancelButton: true,
                    closeOnConfirm: true,
                    cancelButtonText: "Để em coi lại..",
                    confirmButtonText: "Nhập thôi !!!",
                    html: true
                }, function (newFee) {
                    if (typeof(newFee) != "boolean" && newFee != "") {
                        newFee = newFee.replace(/,|\./g, "");
                        newFee = +newFee || 0;

                        if (newFee >= 1000 && newFee <= 25000) {
                            $("input.reducedPrice").each(function (index) {
                                let row = $(this).parent().parent();
                                let feeRefundDOM = row.find(".feeRefund");
                                let oldFee = +feeRefundDOM.val().replace(/,/g, "") || 0;

                                row.attr('data-feerefund', newFee);
                                row.data('feerefund', newFee);
                                row.attr('data-is-fee-refund-other', 1);
                                row.data('is-fee-refund-other', 1);

                                if (oldFee > 0) {
                                    feeRefundDOM.val(formatThousands(newFee));
                                    feeRefundDOM.attr('value', formatThousands(newFee));
                                    changeRow($(this), { 'isChangeFeeOther': true });
                                }
                            });

                            $(".minus-discount").removeClass("hide");
                            $(".restore-discount").addClass("hide");
                        }
                        else {
                            alert("Phí đổi hàng phải là số dương lớn hơn 1,000 và nhỏ hơn 25,000.");
                        }
                    }
                    else {
                        alert("Chưa nhập phí");
                    }
                });
            }

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

            $('#txtSearch').keydown(function (event) {
                if (event.which === 13) {
                    searchProduct();
                    event.preventDefault();
                    return false;
                }
            });

            function getAllPrice(update_by_hand) {
                if(update_by_hand === undefined)
                    update_by_hand = false;
                let totalPrice = 0;
                let productQuantity = 0;
                let productQuantityNoFee = 0;
                let totalRefund = 0;

                let isDiscount = parseInt($("#<%=hdfIsDiscount.ClientID%>").val());
                let discount = parseFloat($("#<%=hdfDiscountAmount.ClientID%>").val());
                let feerefund = parseFloat($("#<%=hdfCustomerFeeChange.ClientID%>").val());
                let refundNoFee = +$("#<%=hdfRefundQuantityNoFee.ClientID%>").val() || 0;
                let refundFee = +$("#<%=hdfRefundQuantityFee.ClientID%>").val() || 0;

                productRefunds.forEach(function(item){
                    let row = $("tr[data-rowIndex='" + item.RowIndex + "']");
                    let isFeeOther = +row.attr('data-is-fee-refund-other') || 0;

                    if (update_by_hand == false && item.ChangeType == 2)
                    {
                        if (isDiscount == 1)
                        {
                            item.ReducedPrice = item.Price - discount;
                            item.FeeRefund = isFeeOther ? item.FeeRefund : feerefund;
                        }
                        else
                        {
                            item.ReducedPrice = item.Price;
                            item.FeeRefund = isFeeOther || item.FeeRefund > 0 ? item.FeeRefund : item.FeeRefundDefault;
                        }

                        item.FeeRefund = refundNoFee >= productQuantityNoFee + item.QuantityRefund ? 0 : item.FeeRefund;

                        row.attr("data-sold-price", item.ReducedPrice);
                        row.attr("data-feerefund", item.FeeRefund);
                    }

                    // Ruler Price - ReducedPrice >= 10,000 VND
                    if ((item.Price - item.ReducedPrice) < discount) {
                        alert("Giá đã bán phải giảm ít nhất " + formatThousands(discount, ","));
                        let sold_price = row.data("sold-price");
                        row.find(".reducedPrice").val(formatThousands(sold_price, ","));
                        item.ReducedPrice = sold_price;
                    }

                    if ((item.Price - item.ReducedPrice) > 25000) {
                        alert("Giá đã bán không được giảm quá 25,000");
                        let sold_price = row.data("sold-price");
                        row.find(".reducedPrice").val(formatThousands(sold_price, ","));
                        item.ReducedPrice = sold_price;
                    }

                    if (item.ChangeType == 2){
                        item.TotalFeeRefund = (item.ReducedPrice - item.FeeRefund) * item.QuantityRefund;
                        totalPrice += item.TotalFeeRefund;
                        productQuantity += item.QuantityRefund;
                        totalRefund += item.FeeRefund * item.QuantityRefund;
                    }
                    else {
                        item.TotalFeeRefund = item.ReducedPrice * item.QuantityRefund;
                        totalPrice += item.TotalFeeRefund;
                        productQuantity += item.QuantityRefund;
                        totalRefund += 0;
                    }

                    // Đánh dấu những sản phẩm đổi trả không tính phí
                    if (item.ChangeType == 2)
                        productQuantityNoFee += item.FeeRefund == 0 ? item.QuantityRefund : 0;

                    // Update price on browser
                    reducedDom = row.children("td").children("input.form-control.reducedPrice")
                    reducedDom.val(formatThousands(item.ReducedPrice))
                    feeDom = row.children("td").children("input.form-control.feeRefund")
                    feeDom.val(formatThousands(item.FeeRefund))
                    totalDom = row.children("td.totalFeeRefund")
                    totalDom.html(formatThousands(item.TotalFeeRefund))
                });

                $(".totalPriceOrder").html(formatThousands(totalPrice, ","));
                $(".totalProductQuantity").html(formatThousands(productQuantity, ","));
                $(".totalProductQuantityNoFee").html(formatThousands(productQuantityNoFee, ","));
                $(".totalRefund").html(formatThousands(totalRefund, ","));

                $("#<%=hdfTotalPrice.ClientID%>").val(totalPrice);
                $("#<%=hdfTotalQuantity.ClientID%>").val(productQuantity);
                $("#<%=hdfTotalRefund.ClientID%>").val(totalRefund);
            }

            function changeRefundType(obj){
                let row = obj.parent().parent();
                let RefundType = obj.val();
                let feeRefundDom = row.find(".feeRefund");
                if (RefundType == 2){
                    row.find(".feeRefund").val(formatThousands(row.data("feerefund"), ","))
                    feeRefundDom.attr('value', formatThousands(row.data("feerefund")));
                    feeRefundDom.prop('disabled', false);
                }
                else
                {
                    feeRefundDom.val(0);
                    feeRefundDom.attr('value', 0);
                    feeRefundDom.prop('disabled', true);
                }
                changeRow(obj);
            }

            function changeRowFeeRefund(obj) {
                obj.parent().parent().attr("data-feerefund", obj.val().replace(/,/g, ""));
                changeRow(obj);
            }

            function changeRow(obj, opts)
            {
                let row = obj.parent().parent();
                let RowIndex = row.attr("data-rowIndex");
                let ProductID = parseInt(row.attr("data-productID"));
                let ProductVariableID = parseInt(row.attr("data-productVariableID"));
                let ReducedPrice = +row.find(".reducedPrice").val().replace(/,/g, "") || 0;
                let Quantity = +row.find(".quantityRefund").val().replace(/,/g, "") || 0;
                let ChangeType = +row.find(".changeType").val() || 0;
                let FeeRefund = +row.find(".feeRefund").val().replace(/,/g, "") || 0;

                if (Quantity == 0)
                {
                    row.find(".quantityRefund").val(1);
                    row.find(".quantityRefund").html(1);
                    row.find(".quantityRefund").focus();
                }

                // nếu là nhấp thây đổi phí khác thì không cần check chính sách đổi hàng
                if (opts && opts['isChangeFeeOther'])
                {
                    // Cập nhật lại thông tin dòng đã edit
                    productRefunds.forEach(function (item) {
                        if (item.isTarget(RowIndex, ProductID, ProductVariableID)) {
                            item.ReducedPrice = ReducedPrice;
                            item.QuantityRefund = Quantity;
                            item.ChangeType = ChangeType;
                            item.FeeRefund = FeeRefund;
                        }
                    });

                    getAllPrice(update_by_hand = true);

                    return;
                }

                // Check xem số lượng đổi trả đã quá quy định chưa
                let error = false;
                let message = ""

                let changeFeeOfCustomer = +$("#<%=hdfCustomerFeeChange.ClientID%>").val() || 0;
                let refundNoFee = +$("#<%=hdfRefundQuantityNoFee.ClientID%>").val() || 0;
                let refundFee = +$("#<%=hdfRefundQuantityFee.ClientID%>").val() || 0;
                let totalRefundNow = +$(".totalProductQuantity").html().replace(/,/g, "") || 0;
                let totalRefundNoFeeNow = +$(".totalProductQuantityNoFee").html().replace(/,/g, "") || 0;

                let refundFilter = productRefunds.filter(item => {
                    return item.isTarget(RowIndex, ProductID, ProductVariableID)
                });
                let quantityRefundOld = +refundFilter[0].QuantityRefund || 0;

                // Tính toán tổng số lượng đổi trả
                totalRefundNow = totalRefundNow + Quantity - quantityRefundOld;
                totalRefundNoFeeNow = totalRefundNoFeeNow + Quantity - quantityRefundOld;

                // setup message
                message += "<div class='row' style='max-height:300px; overflow: auto;'>";
                message += "Có vấn đề khi thêm sản phẩm này!<br/>";
                message += "<h4>Hãy xem lại trước khi xác nhận!<h4>";

                // Nếu số lượng mới thêm vượt quá quy định đổi trả thì thông báo lên
                if (totalRefundNow > (refundNoFee + refundFee))
                {
                    error = true;
                    message += '<h3><span class="label label-warning" style="text-align: left">Vượt quá tổng số lượng được đổi trả:</span></h3><br/>';
                    message += 'Số lượng được đổi: <strong>' + formatThousands(refundNoFee + refundFee, ",") + '</strong> cái<br/>';
                    message += 'Hiện tại: <strong>' + formatThousands(totalRefundNow, ',') + '</strong> cái<br/>';
                }

                // Áp dụng cho trường hợp phí đổi trả của khách > 0
                // Kiểm tra nhưng sản phâm nào thuộc (2: Đổi trả sản phẩm khác) và có fee là 0 đồng
                // và số lượng nhập đổi trả miển phí lớn hơn sô lượng quy định thì thông báo
                if (changeFeeOfCustomer > 0 && ChangeType == 2 && FeeRefund == 0 && totalRefundNoFeeNow > refundNoFee)
                {
                    error = true;
                    message += '<h3><span class="label label-warning" style="text-align: left">Vượt quá số lượng đổi trả miễn phí:</span></h3><br/>';
                    message += 'Số lượng được đổi: <strong>' + formatThousands(refundNoFee, ",") + '</strong> cái<br/>';
                    message += 'Hiện tại: <strong>' + formatThousands(totalRefundNoFeeNow, ',') + '</strong> cái<br/>';
                }

                // kết thúc message
                message += "</div>";

                if (error)
                {
                    let quantityRecommend = 0;
                    let confirmButtonText = "Vẫn thêm vào đơn";

                    if (changeFeeOfCustomer > 0 && ChangeType == 2 && FeeRefund == 0 && totalRefundNoFeeNow > refundNoFee)
                    {
                        quantityRecommend = Quantity - (totalRefundNoFeeNow - refundNoFee);
                        confirmButtonText = "Vẫn thêm vào đơn (" + quantityRecommend + " cái)";
                    }

                    swal({
                        title: "Thông báo",
                        text: message,
                        type: "warning",
                        showCancelButton: true,
                        closeOnConfirm: true,
                        cancelButtonText: "Để xem lại",
                        confirmButtonText: confirmButtonText,
                        html: true
                    }, function (confirm) {
                        if (confirm) {

                            if (changeFeeOfCustomer > 0 && ChangeType == 2 && FeeRefund == 0 && totalRefundNoFeeNow > refundNoFee)
                            {
                                row.find(".quantityRefund").val(quantityRecommend);
                                row.find(".quantityRefund").html(formatThousands(quantityRecommend, ","));
                                row.find(".quantityRefund").focus();
                                Quantity = quantityRecommend;
                            }

                            // Cập nhật lại thông tin dòng đã edit
                            productRefunds.forEach(function (item) {
                                if (item.isTarget(RowIndex, ProductID, ProductVariableID)) {
                                    item.ReducedPrice = ReducedPrice;
                                    item.QuantityRefund = Quantity;
                                    item.ChangeType = ChangeType;
                                    item.FeeRefund = FeeRefund;
                                }
                            });

                            getAllPrice(update_by_hand = true);
                        }
                        else {
                            row.find(".quantityRefund").val(quantityRefundOld);
                            row.find(".quantityRefund").html(formatThousands(quantityRefundOld, ","));
                            row.find(".quantityRefund").focus();
                        }
                    });
                }
                else {
                    // Cập nhật lại thông tin dòng đã edit
                    productRefunds.forEach(function (item) {
                        if (item.isTarget(RowIndex, ProductID, ProductVariableID)) {
                            item.ReducedPrice = ReducedPrice;
                            item.QuantityRefund = Quantity;
                            item.ChangeType = ChangeType;
                            item.FeeRefund = FeeRefund;
                        }
                    });

                    getAllPrice(update_by_hand = true);
                }
            }

            function clickrow(obj) {
                if (!obj.find("td").eq(1).hasClass("checked")) {
                    obj.find("td").addClass("checked");
                }
                else {
                    obj.find("td").removeClass("checked");
                }
            }

            function addHtmlProductResult(item) {
                let html = "";
                let variable = "";
                if (item.ProductStyle == 2) {
                    variable = "<br><br>" + item.VariableValue.replace(/\|/g, "<br>");
                }
                html += "<tr ondblclick='clickrow($(this))' class='product-result' "
                                + "data-rowIndex='" + item.RowIndex + "' "
                                + "data-productID='" + item.ProductID + "' "
                                + "data-productVariableID='" + item.ProductVariableID + "' "
                                + "data-productStyle='" + item.ProductStyle + "' "
                                + "data-productImage='" + item.ProductImage + "' "
                                + "data-productTitle='" + item.ProductTitle + "' "
                                + "data-parentSKU='" + item.ParentSKU + "' "
                                + "data-childSKU='" + item.ChildSKU + "' "
                                + "data-variableValue='" + item.VariableValue + "' "
                                + "data-price='" + item.Price + "' "
                                + "data-sold-price='" + item.ReducedPrice + "' "
                                + "data-feeRefund='" + item.FeeRefundDefault + "' >\n";
                html += "    <td class='image-item'><img onclick='openImage($(this))' src='/uploads/images/159x212/" + item.ProductImage + "''></td>\n";
                html += "    <td class='name-item'><a href='/xem-san-pham?id=" + item.ProductID + "&variableid=" + item.ProductVariableID + "' target='_blank'>" + item.ProductTitle + "</a>"  + variable + "</td>\n";
                if (item.ProductStyle == 1) {
                    html += "    <td class='sku-item'>" + item.ParentSKU + "</td>\n";
                }
                else if (item.ProductStyle == 2) {
                    html += "    <td class='sku-item'>" + item.ChildSKU + "</td>\n";
                }
                html += "    <td class='price-item Price'>" + formatThousands(item.Price, ",") + "</td>\n";
                html += "    <td>\n";
                html += "           <input type='text' class='form-control reducedPrice' min='1' value='" + formatThousands(item.ReducedPrice, ",") + "' onblur='changeRow($(this))' onkeypress='return event.charCode >= 48 && event.charCode <= 57'/>\n";
                html += "    </td>\n";
                html += "    <td>\n";
                html += "           <input type='text' class='form-control quantityRefund' min='1' value='" + formatThousands(item.QuantityRefund, ",") + "' onblur='changeRow($(this))' onkeypress='return event.charCode >= 48 && event.charCode <= 57'/>\n";
                html += "    </td>\n";
                html += "    <td>\n";
                html += "           <select class='form-control changeType' onchange='changeRefundType($(this))'>\n";
                if (item.ProductStyle == 1) {
                    if (item.ChangeType == 1) {
                        html += "               <option value='2'>Đổi sản phẩm khác</option>\n";
                        html += "               <option value='4'>Đổi sản phẩm khác (miễn phí)</option>\n";
                        html += "               <option value='3'>Đổi hàng lỗi</option>\n";
                    }
                    else if (item.ChangeType == 2) {
                        html += "               <option value='2' selected>Đổi sản phẩm khác</option>\n";
                        html += "               <option value='4'>Đổi sản phẩm khác (miễn phí)</option>\n";
                        html += "               <option value='3'>Đổi hàng lỗi</option>\n";
                    }
                    else {
                        html += "               <option value='2'>Đổi sản phẩm khác</option>\n";
                        html += "               <option value='4'>Đổi sản phẩm khác (miễn phí)</option>\n";
                        html += "               <option value='3' selected>Đổi hàng lỗi</option>\n";
                    }
                }
                else {
                    if (item.ChangeType == 1) {
                        html += "               <option value='1' selected>Đổi size - màu</option>\n";
                        html += "               <option value='2'>Đổi sản phẩm khác</option>\n";
                        html += "               <option value='4'>Đổi sản phẩm khác (miễn phí)</option>\n";
                        html += "               <option value='3'>Đổi hàng lỗi</option>\n";
                    }
                    else if (item.ChangeType == 2) {
                        html += "               <option value='1'>Đổi size - màu</option>\n";
                        html += "               <option value='2' selected>Đổi sản phẩm khác</option>\n";
                        html += "               <option value='4'>Đổi sản phẩm khác (miễn phí)</option>\n";
                        html += "               <option value='3'>Đổi hàng lỗi</option>\n";
                    }
                    else {
                        html += "               <option value='1'>Đổi size - màu</option>\n";
                        html += "               <option value='2'>Đổi sản phẩm khác</option>\n";
                        html += "               <option value='4'>Đổi sản phẩm khác (miễn phí)</option>\n";
                        html += "               <option value='3' selected>Đổi hàng lỗi</option>\n";
                    }
                }


                html += "           </select>\n";
                html += "    </td>\n";
                if (item.ChangeType == 2) {
                    html += "   <td><input type='text' class='form-control feeRefund' min='0' value='" + formatThousands(item.FeeRefund) + "' onblur='changeRowFeeRefund($(this))' onkeypress='return event.charCode >= 48 && event.charCode <= 57'/></td>\n";
                }
                else {
                    html += "   <td><input type='text' class='form-control feeRefund' min='0' value='0' onblur='changeRowFeeRefund($(this))' onkeypress='return event.charCode >= 48 && event.charCode <= 57'/></td>\n";
                }
                html += "   <td class='totalFeeRefund'>" + formatThousands(item.TotalFeeRefund) + "</td>\n";
                html += "   <td class='trash-column'><a href='javascript:;' class='link-btn' onclick='deleteRow($(this))'><i class='fa fa-trash'></i></a></td>\n";
                html += "</tr>\n";

                $(".content-product").prepend(html);
            }

            // select a variable product
            function selectProduct() {
                let products = [];

                $(".search-popup").each(function (index, element) {
                    if ($(this).find(".check-popup").is(':checked')) {
                        let sku = $(this).find("td.key").html();
                        let quantity = +$(this).find("input.quantity").val() || 0;

                        if (quantity == 0)
                            return true;

                        let productTarget = productVariableSearch.filter(function (x) { return x.ChildSKU == sku })[0];

                        // get quantity new
                        productTarget.QuantityRefund = quantity;

                        // update total price
                        productTarget.TotalFeeRefund = (productTarget.ReducedPrice - productTarget.FeeRefund) * productTarget.QuantityRefund;

                        // Filter ra những product được chọn
                        products.push(productTarget);
                    }
                });

                // Kiểm tra chính sách trả hàng
                // Nếu thỏa thì sẽ thêm product vào table
                // Ngược lại sẽ lên thông báo warning
                checkPolicy(products);

                closePopup();
                $("#txtSearch").val("");
            }

            // select all variable product
            function check_all() {
                if ($('#check-all').is(":checked")) {
                    $(".check-popup").prop('checked', true);
                } else {
                    $(".check-popup").prop('checked', false);
                }
            }

            // Show list product variable
            function showProductVariable(productVariables) {
                let html = "";

                // Header Popup
                html += "<div class='header-list'>"
                html += "   <table class='table table-checkable table-product table-popup-product'>";
                html += "      <tr id='search-product-header'>";
                html += "         <th class='order-item check-column'>";
                html += "             <input type='checkbox' id='check-all'onchange='check_all()'/>";
                html += "         </th>";
                html += "         <th class='image-column'>Ảnh</td>";
                html += "         <th class='name-column'>Sản phẩm</td>";
                html += "         <th class='sku-column'>Mã</td>";
                html += "         <th class='variable-column'>Thuộc tính</td>";
                html += "         <th class='quantity-column'>Số lượng</td>";
                html += "      </tr>";
                html += "   </table>";
                html += "</div>"
                html += "<div class='div-product-list scrollbar'>";
                html += "   <table class='table table-checkable table-product table-popup-product'>";

                // Body Popup
                productVariables.forEach(function (item) {
                    html += "      <tr class='search-popup' id='search-product-detail';>";
                    html += "         <td class='order-item check-column'>";
                    html += "             <input class='check-popup' data-productVariableID='" + item.ProductVariableID + "' type='checkbox' onchange='check($(this))' />";
                    html += "         </td>";
                    html += "         <td class='image-column'><img src='/uploads/images/85x113/" + item.ProductImage + "'></td>";
                    html += "         <td class='name-column'>" + item.ProductTitle + "</td>";
                    html += "         <td class='sku-column key'>" + item.ChildSKU + "</td>";
                    html += "         <td class='variable-column'>" + item.VariableValue.replace(/\|/g, "<br>") + "</td>";
                    html += "         <td class='quantity-column'>";
                    html += "             <input type='text' class='form-control quantity in-quantity' "
                                              + "pattern='[0-9]{1,3}' "
                                              + "onkeypress='return event.charCode >= 48 && event.charCode <= 57' "
                                              + "value='1' >";
                    html += "         </td>";
                    html += "      </tr>";
                });

                html += "   </table>";
                html += "</div>";

                // Footer Popup
                html += "<div class='footer-list'>";
                html += "   <a href='javascript: ;' class='btn link-btn' style='background-color:#f87703;float:right;color:#fff;' onclick='selectProduct()'>Chọn</a>";
                html += "</div >";

                showPopup(html);
            }

            function searchProduct() {
                var text = $('#txtSearch').val();
                var regex = /^[\x20-\x7E]*$/;
                if (!regex.test(text)) {
                    $("#txtSearch").val(text).select();
                    swal("Thông báo", "Hãy tắt bộ gõ tiếng việt", "error");
                }
                else {
                    let txtPhone = $("#<%=txtPhone.ClientID%>").val();
                    let txtSearch = $("#txtSearch").val();

                    $("#txtSearch").val("");

                    var product = null;

                    productVariableSearch = [];

                    if (isBlank(txtPhone)) {

                        swal({
                            title: "Từ từ nè",
                            text: "Tìm khách hàng trước đã!<br><br>Nếu chưa có thì phải tạo khách hàng trước nha!",
                            type: "warning",
                            showCancelButton: true,
                            closeOnConfirm: false,
                            confirmButtonText: "Để em tìm",
                            cancelButtonText: "Tạo khách hàng",
                            html: true,
                        }, function (isConfirm) {
                            if (isConfirm) {
                                swal.close();
                                searchCustomer();
                            }
                            else
                            {
                                searchCustomer();
                                window.open("/them-moi-khach-hang", "_blank");
                            }
                        });
                        return;
                    }

                    if (isBlank(txtSearch)) {
                        swal({
                            title: "Ủa Ủa",
                            text: "Sao chưa nhập gì hết mà!",
                            type: "warning",
                            showCancelButton: false,
                            closeOnConfirm: false,
                            confirmButtonText: "Hehe em quên!",
                        });
                        return;
                    }

                    productDeleteRefunds.forEach(function (item) {
                        if (item.ProductStyle == 1 && item.ParentSKU.toUpperCase() == txtSearch.toUpperCase()) {
                            product = item;
                        }
                        else if (item.ProductStyle == 2 && item.ChildSKU.toUpperCase() == txtSearch.toUpperCase()) {
                            product = item;
                        }
                    });

                    if (product != null) {
                        // remove product by SKU
                        productDeleteRefunds = productDeleteRefunds.filter(function (item) {
                            return !(item.ParentSKU == product.ParentSKU && item.ChildSKU == product.ChildSKU)
                        });

                        // Kiểm tra chính sách trả hàng
                        // Nếu thỏa thì sẽ thêm product vào table
                        // Ngược lại sẽ lên thông báo warning
                        checkPolicy(product);

                        $("#txtSearch").val("");
                    }
                    else {
                        let customerID = $("#<%=hdfCustomerID.ClientID%>").val();

                        $.ajax({
                            type: "POST",
                            url: "/tao-don-hang-doi-tra.aspx/getProduct",
                            data: JSON.stringify({'customerID': customerID, 'sku': txtSearch}),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                let data = JSON.parse(msg.d);

                                if (data != null && data.length > 0) {
                                    if (data.length > 1) {
                                        data.forEach(function (item) {
                                            let productVariable = new RefundDetailModel(
                                                ProductID = item.ProductID
                                                , ProductVariableID = item.ProductVariableID
                                                , ProductStyle = item.ProductStyle
                                                , ProductImage = item.ProductImage
                                                , ProductTitle = item.ProductTitle
                                                , ParentSKU = item.ParentSKU
                                                , ChildSKU = item.ChildSKU
                                                , VariableValue = item.VariableValue
                                                , Price = item.Price
                                                , ReducedPrice = item.ReducedPrice
                                                , QuantityRefund = item.QuantityRefund
                                                , ChangeType = item.ChangeType
                                                , FeeRefund = item.FeeRefund
                                                , FeeRefundDefault =  item.FeeRefund
                                                , TotalFeeRefund = item.TotalFeeRefund
                                                , SaleDate = item.SaleDate
                                                , OrderID = item.OrderID
                                            );

                                            productVariableSearch.push(productVariable);
                                        });

                                        showProductVariable(productVariableSearch);
                                    }
                                    else {
                                        // Check xem số lượng đổi trả đã quá quy định chưa
                                        let refundNoFee = +$("#<%=hdfRefundQuantityNoFee.ClientID%>").val() || 0;
                                        let refundFee = +$("#<%=hdfRefundQuantityFee.ClientID%>").val() || 0;
                                        let totalRefundNow = +$(".totalProductQuantity").html().replace(/,/g, "") || 0;

                                        product = new RefundDetailModel(
                                            ProductID = data[0].ProductID
                                            , ProductVariableID = data[0].ProductVariableID
                                            , ProductStyle = data[0].ProductStyle
                                            , ProductImage = data[0].ProductImage
                                            , ProductTitle = data[0].ProductTitle
                                            , ParentSKU = data[0].ParentSKU
                                            , ChildSKU = data[0].ChildSKU
                                            , VariableValue = data[0].VariableValue
                                            , Price = data[0].Price
                                            , ReducedPrice = data[0].ReducedPrice
                                            , QuantityRefund = data[0].QuantityRefund
                                            , ChangeType = data[0].ChangeType
                                            , FeeRefund = data[0].FeeRefund
                                            , FeeRefundDefault = data[0].FeeRefund
                                            , TotalFeeRefund = data[0].TotalFeeRefund
                                            , SaleDate = data[0].SaleDate
                                            , OrderID = data[0].OrderID
                                        );

                                        // Kiểm tra chính sách trả hàng
                                        // Nếu thỏa thì sẽ thêm product vào table
                                        // Ngược lại sẽ lên thông báo warning
                                        checkPolicy(product);

                                        $("#txtSearch").val("");
                                    }
                                }
                                else {
                                    $("#txtSearch").val(txtSearch).select();
                                    swal("Thông báo", "Không tìm thấy sản phẩm", "error");
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                let msg = '';

                                if (jqXHR.status == 500) {
                                    msg = jqXHR.responseJSON.Message;
                                } else {
                                    msg = 'lỗi'
                                }

                                alert(msg);
                                return;
                            }
                        });
                    }
                }
            }

            function deleteRow(obj) {
                let c = confirm('Bạn muốn xóa sản phẩm này?');

                if (c) {
                    let row = obj.parent().parent();
                    let RowIndex = row.attr("data-rowIndex");
                    let ProductID = parseFloat(row.attr("data-productID"));
                    let ProductVariableID = parseFloat(row.attr("data-productVariableID"));

                    productRefunds.forEach(function (product) {
                        if (product.isTarget(RowIndex, ProductID, ProductVariableID)) {
                            product.QuantityRefund = 1; // return default
                            product.ChangeType = 2; // return default: Đổi sản phẩm khác
                            product.TotalFeeRefund = product.Price - product.FeeRefund; // return default
                            productDeleteRefunds.push(product);
                        }
                    });

                    productRefunds = productRefunds.filter(function (product) {
                        return !product.isTarget(RowIndex, ProductID, ProductVariableID);
                    });

                    row.remove();
                    getAllPrice(update_by_hand=true);
                }
            }

            function deleteProduct() {
                let c = confirm("Bạn muốn xóa tất cả sản phẩm?");

                if (c) {
                    productDeleteRefunds = productRefunds;
                    productRefunds = [];

                    $(".product-result").remove();

                    getAllPrice(update_by_hand=true);
                }
            }

            function payall() {
                window.onbeforeunload = null;

                var phone = $("#<%=txtPhone.ClientID%>").val();
                var name = $("#<%= txtFullname.ClientID%>").val();
                var nick = $("#<%= txtNick.ClientID%>").val();
                var address = $("#<%= txtAddress.ClientID%>").val();
                if (phone != "" && name != "" && nick != "" && address != "") {
                    productRefunds = productRefunds.filter(item => { return item.QuantityRefund > 0});
                    if (productRefunds.length > 0) {
                        let dataJSON = '{ "RefundDetails" : [';

                        productRefunds.forEach(function (item) {
                            dataJSON += item.stringJSON() + ","
                        });

                        dataJSON = dataJSON.replace(/.$/, "") + "]}";

                        HoldOn.open();
                        $("#<%=hdfListProduct.ClientID%>").val(dataJSON);
                        $("#<%=btnSave.ClientID%>").click();

                    }
                    else {
                        $("#txtSearch").focus();
                        swal("Thông báo", "Hãy nhập sản phẩm cần đổi trả!");
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
                        $("#<%= txtNick.ClientID%>").prop('readonly', false);
                        $("#<%= txtNick.ClientID%>").focus();
                        swal("Thông báo", "Hãy nhập Nick đặt hàng của khách hàng!", "error");
                    }
                    else if (address == "") {
                        $("#<%= txtAddress.ClientID%>").focus();
                        swal("Thông báo", "Hãy nhập địa chỉ khách hàng!", "error");
                    }
                }
            }

            var formatThousands = function (n, dp) {
                var s = '' + (Math.floor(n)), d = n % 1, i = s.length, r = '';
                while ((i -= 3) > 0) { r = ',' + s.substr(i, 3) + r; }
                return s.substr(0, i + 3) + r +
                    (d ? '.' + Math.round(d * Math.pow(10, dp || 2)) : '');
            };

            function check() {
                var temp = 0;
                var temp2 = 0;
                $(".product-result").each(function () {
                    if ($(this).find(".check-popup").is(':checked')) {
                        temp++;
                    }
                    else {
                        temp2++;
                    }
                    if (temp2 > 0) {
                        $("#check-all").prop('checked', false);
                    }
                    else {
                        $("#check-all").prop('checked', true);
                    }
                });
            }

            $(document).ready(function () {

                _initReceiverAddress();
                _onChangeReceiverAddress();

                let dataJSON = $("#<%=hdfListProduct.ClientID%>").val();

                if (!isBlank(dataJSON)) {

                    let refundGoodModel = jQuery.parseJSON(dataJSON);

                    // Làm lại đơn hàng đổi trả
                    $("#<%=hdRefundGoodsID.ClientID%>").val(refundGoodModel.RefundGoodsID)
                    // Init discount for customer
                    if (refundGoodModel.CustomerID)
                    {
                        getCustomerDiscount(refundGoodModel.CustomerID);
                    }
                    // Init header search Customer
                    $("#<%=txtFullname.ClientID%>").val(refundGoodModel.CustomerName);
                    $("#<%=txtPhone.ClientID%>").val(refundGoodModel.CustomerPhone);
                    $("#<%=txtNick.ClientID%>").val(refundGoodModel.CustomerNick);
                    $("#<%=txtAddress.ClientID%>").val(refundGoodModel.CustomerAddress);
                    $("#<%=txtFacebook.ClientID%>").val(refundGoodModel.CustomerFacebook);

                    _getCustomerAddress(refundGoodModel.CustomerPhone);
                    $("input[id$='_txtFullname']").prop('readonly', false);
                    $("input[id$='_txtAddress']").prop('readonly', false);
                    $("input[id$='_txtNick']").prop('readonly', false);
                    $("input[id$='_txtFacebook']").prop('readonly', false);
                    $("select[id$='_ddlProvince']").attr('disabled', false);
                    $("select[id$='_ddlProvince']").attr('readonly', false);

                    // Init detail refund product
                    refundGoodModel.RefundDetails.reverse().forEach(function (item) {
                        let product = new RefundDetailModel(
                                        ProductID = item.ProductID
                                        , ProductVariableID = item.ProductVariableID
                                        , ProductStyle = item.ProductStyle
                                        , ProductImage = item.ProductImage
                                        , ProductTitle = item.ProductTitle
                                        , ParentSKU = item.ParentSKU
                                        , ChildSKU = item.ChildSKU
                                        , VariableValue = item.VariableValue
                                        , Price = item.Price
                                        , ReducedPrice = item.ReducedPrice
                                        , QuantityRefund = item.QuantityRefund
                                        , ChangeType = item.ChangeType
                                        , FeeRefund = item.FeeRefund
                                        , FeeRefundDefault = item.FeeRefundDefault
                                        , TotalFeeRefund = item.TotalFeeRefund
                                        , SaleDate = item.SaleDate
                                        , OrderID = item.OrderID
                                    );

                        productRefunds.push(product);
                        addHtmlProductResult(product);

                        getAllPrice();
                    });

                    // Init total refund product
                    $(".totalPriceOrder").html(formatThousands(refundGoodModel.TotalPrice, ","));
                    $(".totalProductQuantity").html(formatThousands(refundGoodModel.TotalQuantity, ","));
                    $(".totalRefund").html(formatThousands(refundGoodModel.TotalFreeRefund, ","));

                    // Init footer status
                    $("#<%=ddlRefundStatus.ClientID%>").val(refundGoodModel.Status);
                    $("#<%=txtRefundsNote.ClientID%>").val(refundGoodModel.Note);

                    // Init hiden footer
                    $("#<%=hdfCustomerID.ClientID%>").val(refundGoodModel.CustomerID);
                    $("#<%=hdfPhone.ClientID%>").val(refundGoodModel.CustomerPhone);
                    $("#<%=hdfTotalPrice.ClientID%>").val(refundGoodModel.TotalPrice);
                    $("#<%=hdfTotalQuantity.ClientID%>").val(refundGoodModel.TotalQuantity);
                    $("#<%=hdfTotalRefund.ClientID%>").val(refundGoodModel.TotalFreeRefund);
                    $("#<%=hdfUsername.ClientID%>").val(refundGoodModel.CreateBy);
                    $("#<%=hdfUsernameCurrent.ClientID%>").val(refundGoodModel.CreateBy);
                    $("#<%=hdfOrderSaleID.ClientID%>").val(refundGoodModel.OrderSaleID);
                }
            });

            // Kiểm tra xem hàng trả đã quá hạng chưa
            function checkSaleDate(item) {
                let now = new moment();
                let daysExchange = +$("input[id$='_hdfDaysExchange']").val() || 0;
                let dateExchangeMin = now.add(-daysExchange, 'day').format('YYYY-MM-DD');

                if (item.SaleDate < dateExchangeMin)
                    return false;

                return true;
            }

            // Kiểm tra sản phẩm khi thêm vào bảng
            function checkPolicy(product) {
                // Phân loại product
                let productNoOrder = [];
                let productExpired = [];
                let quantityRefund = 0;

                if (Array.isArray(product))
                {
                    product.forEach(item => {
                        quantityRefund += item.QuantityRefund;

                        // Lọc ra sản phẩm chưa từng mua
                        if (!item.OrderID)
                            productNoOrder.push(item);

                        // Lọc ra sản phẩm hết hạn đổi tra
                        if (!checkSaleDate(item))
                            productExpired.push(item);
                    });
                }
                else
                {
                    quantityRefund += product.QuantityRefund;

                    // Lọc ra sản phẩm chưa từng mua
                    if (!product.OrderID)
                        productNoOrder.push(product);

                    // Lọc ra sản phẩm hết hạn đổi tra
                    if (!checkSaleDate(product))
                        productExpired.push(product);
                }

                let message = "";
                let error = false;

                // Check xem số lượng đổi trả đã quá quy định chưa
                let refundNoFee = +$("#<%=hdfRefundQuantityNoFee.ClientID%>").val() || 0;
                let refundFee = +$("#<%=hdfRefundQuantityFee.ClientID%>").val() || 0;
                let totalRefundNow = +$(".totalProductQuantity").html().replace(/,/g, "") || 0;

                // setup message
                message += "<div class='row' style='max-height:300px; overflow: auto;'>";
                message += "Có vấn đề khi thêm sản phẩm này!<br/>";
                message += "<h4>Hãy xem lại trước khi xác nhận!<h4>";

                // Check xem số lượng đổi trả đã quá quy định chưa
                if ((totalRefundNow + quantityRefund) > (refundNoFee + refundFee)) {
                    error = true;
                    message += '<h3><span class="label label-warning" style="text-align: left">Vượt quá tổng số lượng được đổi trả:</span></h3><br/>';
                    message += 'Số lượng được đổi: <strong>' + formatThousands(refundNoFee + refundFee, ",") + '</strong> cái<br/>';
                    message += 'Hiện tại: <strong>' + formatThousands(totalRefundNow + quantityRefund, ',') + '</strong> cái<br/>';
                }

                // Thông báo những sản phẩm này khách hàng chưa từng mua
                if (productNoOrder.length > 0) {
                    error = true;
                    message += '<h3><span class="label label-warning" style="text-align: left">Sản phẩm chưa từng mua:</span></h3><br/>';
                    productNoOrder.forEach(item => {
                        if (item.ProductStyle == 2)
                            message += '<strong>' + item.ChildSKU + ' - ' + item.ProductTitle + '</strong><br/>';
                        else
                            message += '<strong>' + item.ParentSKU + ' - ' + item.ProductTitle + '</strong><br/>';
                    });
                }

                // Thông báo những sản phẩm này đã hết hạng đổi trả
                if (productExpired.length > 0) {
                    error = true;
                    message += '<h3><span class="label label-danger" style="text-align: left">Sản phẩm hết hạn đổi trả:</span></h3><br/>';
                    productExpired.forEach(item => {
                        if (item.ProductStyle == 2)
                            message += '<a href="/thong-tin-don-hang?id=' + item.OrderID + '" target="_blank">' + item.ChildSKU + ' - ' + item.ProductTitle + '</a><br/>';
                        else
                            message += '<a href="/thong-tin-don-hang?id=' + item.OrderID + '" target="_blank">' + item.ParentSKU + ' - ' + item.ProductTitle + '</a><br/>';
                    });
                }

                // kết thúc message
                message += "</div>";

                if (error)
                {
                    swal({
                        title: "Thông báo",
                        text: message,
                        type: "warning",
                        showCancelButton: true,
                        closeOnConfirm: true,
                        cancelButtonText: "Để xem lại",
                        confirmButtonText: "Vẫn thêm vào đơn",
                        html: true
                    }, function (confirm) {
                        if (confirm) {
                            if (Array.isArray(product))
                            {
                                product.forEach(item => {
                                    productRefunds.push(item);
                                    addHtmlProductResult(item);
                                });
                            }
                            else
                            {
                                productRefunds.push(product);
                                addHtmlProductResult(product);
                            }

                            getAllPrice();
                        }
                    });
                }
                else
                {
                    if (Array.isArray(product)) {
                        product.forEach(item => {
                            productRefunds.push(item);
                            addHtmlProductResult(item);
                        });
                    }
                    else {
                        productRefunds.push(product);
                        addHtmlProductResult(product);
                    }

                    getAllPrice();
                }
            }
        </script>
    </telerik:RadScriptBlock>
</asp:Content>
