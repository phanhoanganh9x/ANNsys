<%@ Page Title="Thêm đơn trả hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="tao-don-hang-doi-tra.aspx.cs" Inherits="IM_PJ.tao_don_hang_doi_tra" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/App_Themes/Ann/js/search-customer.js?v=2110"></script>
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
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Họ tên</label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFullname" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtFullname" CssClass="form-control" runat="server" autocomplete="off" placeholder="Họ tên thật của khách"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Điện thoại</label>
                                            <asp:RequiredFieldValidator ID="re" runat="server" ControlToValidate="txtPhone" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtPhone" CssClass="form-control" runat="server" autocomplete="off" placeholder="Số điện thoại khách hàng"></asp:TextBox>
                                        </div>
                                    </div>
                                </div> 
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Nick đặt hàng</label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNick" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtNick" CssClass="form-control" runat="server" autocomplete="off" placeholder="Tên nick đặt hàng"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Địa chỉ</label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAddress" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtAddress" CssClass="form-control" runat="server" autocomplete="off" placeholder="Địa chỉ khách hàng"></asp:TextBox>
                                        </div>
                                    </div>
                                </div> 
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Zalo</label>
                                            <asp:TextBox ID="txtZalo" CssClass="form-control" runat="server" autocomplete="off" placeholder="Số điện thoại Zalo"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
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
                                <div class="search-box left" style="width: 96%;">
                                    <input type="text" id="txtSearch" class="form-control sku-input" placeholder="SKU (F3)" autocomplete="off">
                                </div>
                                <div class="right">
                                    <a href="javascript:;" class="link-btn" onclick="searchProduct()"><i class="fa fa-search"></i></a>
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
                            <div class="post-row clear">
                                <div class="left">Tổng cộng</div>
                                <div class="right totalPriceOrder"></div>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Tổng số lượng sản phẩm</div>
                                <div class="right totalProductQuantity"></div>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Phí đổi hàng</div>
                                <div class="right totalRefund"></div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Thông tin trạng thái đơn hàng đổi trả</h3>
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
            <telerik:AjaxSetting AjaxControlID="btnOrder">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="parent" LoadingPanelID="rxLoading"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadScriptBlock ID="sc" runat="server">
        <script type="text/javascript">

            function redirectTo(ID) {
                HoldOn.open();
                $("#payall").addClass("payall-clicked");
                window.location.href = "/xem-don-hang-doi-tra?id=" + ID;
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

            //disable input
            $("#<%= txtPhone.ClientID%>").prop('readonly', true);
            $("#<%= txtFullname.ClientID%>").prop('readonly', true);
            $("#<%= txtAddress.ClientID%>").prop('readonly', true);
            $("#<%= txtNick.ClientID%>").prop('readonly', true);
            $("#<%= txtZalo.ClientID%>").prop('readonly', true);
            $("#<%= txtFacebook.ClientID%>").prop('readonly', true);

            // Create model entity
            class RefundDetailModel{
                constructor(RowIndex
                            , ProductID
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
                            , TotalFeeRefund) {
                    this.RowIndex = RowIndex;
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
                }

                isTarget(RowIndex, ProductID, ProductVariableID) {
                    return this.RowIndex == RowIndex && this.ProductID == ProductID && this.ProductVariableID == ProductVariableID;
                }
                
                stringJSON() {
                    return JSON.stringify(this);
                }
            }

            // Key for row in table product refund
            var rowIndexMax = Number(0);

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
                    
                    $("input.reducedPrice").each(function (index) {
                        oldValue = $(this).val().replace(/,/g, "");

                        if (parseInt(discount) > 0) {
                            newValue = formatThousands(parseInt(oldValue) - parseInt(discount));
                        }
                        else {
                            newValue = formatThousands(parseInt(oldValue) + parseInt(discount));
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
                    title: "Thay đổi phí đổi hàng",
                    text: "Nhập phí mới cho <strong>sản phẩm đổi mẫu khác</strong> trong đơn hàng này:",
                    type: "input",
                    showCancelButton: true,
                    closeOnConfirm: true,
                    cancelButtonText: "Để em coi lại..",
                    confirmButtonText: "Nhập thôi !!!",
                    html: true
                }, function (newFee) {
                    if (newFee != "") {
                        $("input.reducedPrice").each(function (index) {
                            oldFee = $(this).parent().parent().attr("data-feerefund");
                            if (oldFee != "") {
                                $(this).parent().parent().attr("data-feerefund", newFee);
                                $(this).parent().parent().find(".feeRefund").val(formatThousands(newFee));
                                changeRow($(this));
                            }
                        });

                        $(".minus-discount").removeClass("hide");
                        $(".restore-discount").addClass("hide");
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

            function getAllPrice() {
                let totalPrice = 0;
                let productQuantity = 0;
                let totalRefund = 0;

                let isDiscount = parseInt($("#<%=hdfIsDiscount.ClientID%>").val());
                let discount = parseFloat($("#<%=hdfDiscountAmount.ClientID%>").val());
                let feerefund = parseFloat($("#<%=hdfCustomerFeeChange.ClientID%>").val());

                productRefunds.forEach(function(item){
                    let row = $("tr[data-parentsku='" + item.ParentSKU + "']");
                    
                    if (isDiscount == 1)
                    {
                        item.ReducedPrice = item.Price - discount;
                        item.FeeRefund = feerefund;
                    }
                    else
                    {
                        item.ReducedPrice = item.Price;
                        item.FeeRefund = item.FeeRefundDefault;
                    }
                    
                    if (item.ChangeType == 2){
                        item.TotalFeeRefund = (item.ReducedPrice - item.FeeRefund) * item.QuantityRefund;
                        totalPrice += item.TotalFeeRefund;
                        productQuantity += item.QuantityRefund;
                        totalRefund += item.FeeRefund * item.QuantityRefund;
                    }
                    else{
                        item.TotalFeeRefund = item.ReducedPrice * item.QuantityRefund;
                        totalPrice += item.TotalFeeRefund;
                        productQuantity += item.QuantityRefund;
                        totalRefund += 0;
                    }

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
                $(".totalRefund").html(formatThousands(totalRefund, ","));

                $("#<%=hdfTotalPrice.ClientID%>").val(totalPrice);
                $("#<%=hdfTotalQuantity.ClientID%>").val(productQuantity);
                $("#<%=hdfTotalRefund.ClientID%>").val(totalRefund);
            }

            function changeRowFeeRefund(obj) {
                obj.parent().parent().attr("data-feerefund", obj.val().replace(/,/g, ""));
                changeRow(obj);
            }

            function changeRow(obj)
            {
                let row = obj.parent().parent();
                let RowIndex = parseInt(row.attr("data-rowIndex"));
                let ProductID = parseInt(row.attr("data-productID"));
                let ProductVariableID = parseInt(row.attr("data-productVariableID"));
                let Price = parseFloat(row.find(".Price").html().replace(/,/g, ""));
                let SoldPrice = parseInt(row.attr("data-sold-price"));
                let ReducedPrice = parseFloat(row.find(".reducedPrice").val().replace(/,/g, ""));
                let Quantity = parseFloat(row.find(".quantityRefund").val().replace(/,/g, ""));
                let ChangeType = row.find(".changeType").val();
                let FeeRefund = parseFloat(row.attr("data-feeRefund"));
                let TotalFeeRefund = 0;

                // Ruler Price - ReducedPrice >= 10,000 VND
                if ((Price - ReducedPrice) > 11000) {
                    alert("Giá đã bán không thể giảm hơn 11.000đ.");
                    row.find(".reducedPrice").val(formatThousands(SoldPrice, ","));
                    ReducedPrice = SoldPrice;
                }

                if (ChangeType == 2) {
                    TotalFeeRefund = (ReducedPrice - FeeRefund) * Quantity;

                    row.find(".feeRefund").val(formatThousands(FeeRefund, ","));
                    row.find(".totalFeeRefund").html(formatThousands(TotalFeeRefund, ","));
                }
                else {
                    TotalFeeRefund = ReducedPrice * Quantity;

                    row.find(".feeRefund").val(0);
                    row.find(".totalFeeRefund").html(formatThousands(TotalFeeRefund, ","));
                }

                productRefunds.forEach(function(item){
                    if (item.isTarget(RowIndex, ProductID, ProductVariableID)) {
                        item.QuantityRefund = Quantity;
                        item.ChangeType = ChangeType;
                        item.TotalFeeRefund = TotalFeeRefund;
                        item.ReducedPrice = ReducedPrice;
                        item.FeeRefund = FeeRefund;
                        item.DiscountPricePerProduct = Price - ReducedPrice;
                    }
                });

                getAllPrice();
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
                                + "data-feeRefund='" + item.FeeRefund + "' >\n";
                html += "    <td><img src='" + item.ProductImage + "''></td>\n";
                html += "    <td>" + item.ProductTitle + variable + "</td>\n";
                if (item.ProductStyle == 1) {
                    html += "    <td>" + item.ParentSKU + "</td>\n";
                }
                else if (item.ProductStyle == 2) {
                    html += "    <td>" + item.ChildSKU + "</td>\n";
                }
                html += "    <td class='Price'>" + formatThousands(item.Price, ",") + "</td>\n";
                html += "    <td>\n";
                html += "           <input type='text' class='form-control reducedPrice' min='1' value='" + formatThousands(item.ReducedPrice, ",") + "' onblur='changeRow($(this))' onkeypress='return event.charCode >= 48 && event.charCode <= 57'/>\n";
                html += "    </td>\n";
                html += "    <td>\n";
                html += "           <input type='text' class='form-control quantityRefund' min='1' value='" + formatThousands(item.QuantityRefund, ",") + "' onblur='changeRow($(this))' onkeypress='return event.charCode >= 48 && event.charCode <= 57'/>\n";
                html += "    </td>\n";
                html += "    <td>\n";
                html += "           <select class='form-control changeType' onchange='changeRow($(this))'>\n";
                if (item.ProductStyle == 1) {
                    if (item.ChangeType == 1) {
                        html += "               <option value='2'>Đổi sản phẩm khác</option>\n";
                        html += "               <option value='3'>Đổi hàng lỗi</option>\n";
                    }
                    else if (item.ChangeType == 2) {
                        html += "               <option value='2' selected>Đổi sản phẩm khác</option>\n";
                        html += "               <option value='3'>Đổi hàng lỗi</option>\n";
                    }
                    else {
                        html += "               <option value='2'>Đổi sản phẩm khác</option>\n";
                        html += "               <option value='3' selected>Đổi hàng lỗi</option>\n";
                    }
                }
                else {
                    if (item.ChangeType == 1) {
                        html += "               <option value='1' selected>Đổi size - màu</option>\n";
                        html += "               <option value='2'>Đổi sản phẩm khác</option>\n";
                        html += "               <option value='3'>Đổi hàng lỗi</option>\n";
                    }
                    else if (item.ChangeType == 2) {
                        html += "               <option value='1'>Đổi size - màu</option>\n";
                        html += "               <option value='2' selected>Đổi sản phẩm khác</option>\n";
                        html += "               <option value='3'>Đổi hàng lỗi</option>\n";
                    }
                    else {
                        html += "               <option value='1'>Đổi size - màu</option>\n";
                        html += "               <option value='2'>Đổi sản phẩm khác</option>\n";
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
                $(".search-popup").each(function (index, element) {
                    if ($(this).find(".check-popup").is(':checked')) {
                        let sku = $(this).find("td.key").html();
                        let quantity = parseInt($(this).find("input.quantity").val());

                        let productTarget = productVariableSearch.filter(function (x) { return x.ChildSKU == sku })[0];

                        // get quantity new
                        productTarget.QuantityRefund = quantity;

                        // update total price
                        productTarget.TotalFeeRefund = (productTarget.ReducedPrice - productTarget.FeeRefund) * productTarget.QuantityRefund;

                        productRefunds.push(productTarget);
                        addHtmlProductResult(productTarget);
                    }
                });

                getAllPrice();
                closePopup();
                $("#txtSearch").focus();
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
                    html += "         <td class='image-column'><img src='" + item.ProductImage + "'></td>";
                    html += "         <td class='name-column'>" + item.ProductTitle + "</td>";
                    html += "         <td class='sku-column key'>" + item.ChildSKU + "</td>";
                    html += "         <td class='variable-column'>" + item.VariableValue.replace(/\|/g, "<br>") + "</td>";
                    html += "         <td class='quantity-column'>";
                    html += "             <input type='text' class='form-control quantity in-quantity' "
                                              + "pattern='[0-9]{1,3}' "
                                              + "onblur='changeQuantityPopup($(this))' "
                                              + "onkeyup='pressKeyQuantityPopup($(this))' "
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
                            return !(item.ParentSKU = product.ParentSKU && item.ChildSKU == product.ChildSKU)
                        });

                        productRefunds.push(product);

                        addHtmlProductResult(product);

                        $("#txtSearch").val("");
                        getAllPrice();
                    }
                    else {
                        $.ajax({
                            type: "POST",
                            url: "/tao-don-hang-doi-tra.aspx/getProduct",
                            data: "{sku:'" + txtSearch + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                let data = JSON.parse(msg.d);

                                if (data != null && data.length > 0) {
                                    if (data.length > 1) {
                                        data.forEach(function (item) {
                                            rowIndexMax = rowIndexMax + 1;

                                            let productVariable = new RefundDetailModel(
                                                RowIndex = rowIndexMax
                                                , ProductID = item.ProductID
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
                                            );

                                            productVariableSearch.push(productVariable);
                                        });

                                        showProductVariable(productVariableSearch);
                                    }
                                    else {
                                        rowIndexMax = rowIndexMax + 1;

                                        product = new RefundDetailModel(
                                            RowIndex = rowIndexMax
                                            , ProductID = data[0].ProductID
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
                                        );

                                        productRefunds.push(product);

                                        addHtmlProductResult(product);

                                        $("#txtSearch").val("");
                                        getAllPrice();
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
                    let RowIndex = parseFloat(row.attr("data-rowIndex"));
                    let ProductID = parseFloat(row.attr("data-productID"));
                    let ProductVariableID = parseFloat(row.attr("data-productVariableID"));

                    productRefunds.forEach(function (product) {
                        if (product.isTarget(RowIndex, ProductID, ProductVariableID)) {
                            product.QuantityRefund = 1; // return default
                            product.ChangeType = 1; // return default
                            product.TotalFeeRefund = product.Price; // return default
                            productDeleteRefunds.push(product);
                        }
                    });

                    productRefunds = productRefunds.filter(function (product) {
                        return !product.isTarget(RowIndex, ProductID, ProductVariableID);
                    });

                    row.remove();
                    getAllPrice();
                }
            }

            function deleteProduct() {
                let c = confirm("Bạn muốn xóa tất cả sản phẩm?");

                if (c) {
                    productDeleteRefunds = productRefunds;
                    productRefunds = [];

                    $(".product-result").remove();

                    getAllPrice();
                }
            }

            function payall() {
                $("#payall").addClass("payall-clicked");

                var phone = $("#<%=txtPhone.ClientID%>").val();
                var name = $("#<%= txtFullname.ClientID%>").val();
                var nick = $("#<%= txtNick.ClientID%>").val();
                var address = $("#<%= txtAddress.ClientID%>").val();
                if (phone != "" && name != "" && nick != "" && address != "") {
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
                let dataJSON = $("#<%=hdfListProduct.ClientID%>").val();

                if (!isBlank(dataJSON)) {
                    let refundGoodModel = jQuery.parseJSON(dataJSON);

                    // Init header search Customer
                    $("#<%=txtFullname.ClientID%>").val(refundGoodModel.CustomerName);
                    $("#<%=txtPhone.ClientID%>").val(refundGoodModel.CustomerPhone);
                    $("#<%=txtNick.ClientID%>").val(refundGoodModel.CustomerNick);
                    $("#<%=txtAddress.ClientID%>").val(refundGoodModel.CustomerAddress);
                    $("#<%=txtZalo.ClientID%>").val(refundGoodModel.CustomerZalo);
                    $("#<%=txtFacebook.ClientID%>").val(refundGoodModel.CustomerFacebook);

                    // Init detail refund product
                    let rowIndexMax = 0
                    refundGoodModel.RefundDetails.reverse().forEach(function (item) {
                        rowIndexMax = rowIndexMax + 1;

                        let product = new RefundDetailModel(
                                        RowIndex = rowIndexMax
                                        , ProductID = item.ProductID
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
                                        , TotalFeeRefund = item.TotalFeeRefund
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
                    $("#<%=hdfPhone.ClientID%>").val(refundGoodModel.CustomerPhone);
                    $("#<%=hdfTotalPrice.ClientID%>").val(refundGoodModel.TotalPrice);
                    $("#<%=hdfTotalQuantity.ClientID%>").val(refundGoodModel.TotalQuantity);
                    $("#<%=hdfTotalRefund.ClientID%>").val(refundGoodModel.TotalFreeRefund);
                    $("#<%=hdfUsername.ClientID%>").val(refundGoodModel.CreateBy);
                    $("#<%=hdfUsernameCurrent.ClientID%>").val(refundGoodModel.CreateBy);
                    $("#<%=hdfOrderSaleID.ClientID%>").val(refundGoodModel.OrderSaleID);
                }
            });
        </script>
    </telerik:RadScriptBlock>
</asp:Content>
