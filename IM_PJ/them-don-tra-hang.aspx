<%@ Page Title="Thêm đơn trả hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="them-don-tra-hang.aspx.cs" Inherits="IM_PJ.them_don_tra_hang" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/App_Themes/Ann/js/search-customer.js?v=2118"></script>
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
                                <a href="javascript:;" class="search-customer" onclick="searchCustomer2()"><i class="fa fa-search" aria-hidden="true"></i> Tìm khách hàng (F1)</a>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Họ tên</label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFullname" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtFullname" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Điện thoại</label>
                                            <asp:RequiredFieldValidator ID="re" runat="server" ControlToValidate="txtPhone" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtPhone" Enabled="false" CssClass="form-control" runat="server" onchange="checkCustomer()" onpaste="checkCustomer()"></asp:TextBox>
                                        </div>
                                    </div>
                                </div> 
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Nick đặt hàng</label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNick" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtNick" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Địa chỉ</label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAddress" ErrorMessage="(*)" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtAddress" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div> 
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Zalo</label>
                                            <asp:TextBox ID="txtZalo" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Facebook</label>
                                            <div class="row">
                                                <div class="col-md-10 fb width-100">
                                                <asp:TextBox ID="txtFacebook" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
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
                                    <input type="text" id="txtSearch" class="form-control" placeholder="NHẬP MÃ SẢN PHẨM (F3)">
                                </div>
                                <div class="right">
                                    <a href="javascript:;" class="link-btn" onclick="searchProduct()"><i class="fa fa-search"></i></a>
                                </div>
                            </div>
                            <div class="post-body search-product-content clear">
                                <table class="table table-checkable table-product custom-font-size-12">
                                    <thead>
                                        <tr>
                                            <th>Mã đơn hàng</th>
                                            <th>Tên sản phẩm</th>
                                            <th>SKU</th>
                                            <th>Giá gốc</th>
                                            <th>Giá đã bán</th>
                                            <th>Tổng số lượng</th>
                                            <th>Đổi tối đa</th>
                                            <th>Cần đổi</th>
                                            <th>Hình thức</th>
                                            <th>Phí đổi hàng</th>
                                            <th>Thành tiền</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody class="content-product">
                                    </tbody>
                                </table>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Tổng cộng</div>
                                <div class="right totalpriceorder"></div>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Tổng số lượng sản phẩm</div>
                                <div class="right totalproductQuantity"></div>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Phí đổi hàng</div>
                                <div class="right totalrefund"></div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Thông tin trạng thái đơn</h3>
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
                                <div class="post-table-links clear">
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #f87703; float: right; color: #fff;" onclick="payall()">Thanh toán</a>
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #f87703; float: right; margin-right: 10px; color: #fff;" onclick="deleteProduct()">Xóa</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Style="display: none" />
            <asp:HiddenField ID="hdfUsername" runat="server" />
            <asp:HiddenField ID="hdfIsRefund" runat="server" />
            <asp:HiddenField ID="hdfTotalCanchange" runat="server" Value="0" />
            <asp:HiddenField ID="hdfPhone" runat="server" />
            <asp:HiddenField ID="hdfTotalQuantity" runat="server" Value="0" />
            <asp:HiddenField ID="hdfTotalPrice" runat="server" Value="0" />
            <asp:HiddenField ID="hdfTotalRefund" runat="server" Value="0" />
            <asp:HiddenField ID="hdfListProduct" runat="server" />
            <asp:HiddenField ID="hdfCustomerID" runat="server" />
        </main>
    </asp:Panel>
    <style>
        .search-product-content {
            height: 350px;
            background: #fff;
            overflow-y: scroll;
            padding: 5px;
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
            class ProductRefundModel{
                constructor(OrderID
                            , OrderDetailID
                            , RowIndex
                            , ProductName
                            , ProductType
                            , SKU
                            , Price
                            , ReducedPrice
                            , DiscountPerProduct
                            , QuantityOrder
                            , QuantityLeft
                            , QuantityRefund
                            , ChangeType
                            , FeeRefund) {
                    this.OrderID = OrderID;
                    this.OrderDetailID = OrderDetailID;
                    this.RowIndex = RowIndex;
                    this.ProductName = ProductName;
                    this.ProductType = ProductType;
                    this.SKU = SKU;
                    this.Price = Price;
                    this.ReducedPrice = ReducedPrice;
                    this.DiscountPerProduct = DiscountPerProduct;
                    this.QuantityOrder = QuantityOrder;
                    this.QuantityLeft = QuantityLeft;
                    this.QuantityRefund = QuantityRefund;
                    this.ChangeType = ChangeType;
                    this.FeeRefund = FeeRefund;
                    this.TotalFeeRefund = QuantityRefund * ReducedPrice;
                }

                isTarget(OrderID, OrderDetailID, SKU) {
                    return this.OrderID == OrderID && this.OrderDetailID == OrderDetailID && this.SKU == SKU;
                }

                canAddOderDetailNew(QuantityRefund) {
                    return QuantityRefund > this.QuantityLeft
                }
            }

            var productRefunds = [];
            var productDeleteRefunds = [];

            function redirectTo(ID) {
                window.location.href = "/thong-tin-tra-hang?id=" + ID;
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

            function payall() {
                if ($(".product-result").length > 0) {
                    var totalCanchange = parseFloat($("#<%=hdfTotalCanchange.ClientID%>").val());
                    var totalRequest = 0;
                    var list = "";
                    $(".product-result").each(function () {

                        var quantity = parseFloat($(this).find(".soluongcandoi").val());
                        totalRequest += quantity;
                    });
                    if (totalRequest <= totalCanchange) {
                        $(".product-result").each(function () {
                            var sku = $(this).attr("data-sku");
                            var orderID = $(this).attr("data-orderID");
                            var orderDetailID = $(this).attr("data-orderDetailID");
                            var ProductName = $(this).attr("data-ProductName");
                            var ProductType = $(this).attr("data-ProductType");
                            var Giagoc = $(this).attr("data-Giagoc");
                            var Giadaban = $(this).attr("data-Giadaban");
                            var TienGiam = $(this).attr("data-TienGiam");
                            var Soluongtoida = $(this).attr("data-Soluongtoida");
                            var RefundType = $(this).find(".changeType").val();
                            var quantity = parseFloat($(this).find(".soluongcandoi").val());
                            var RefundFee = $(this).find(".phidoihang").attr("data-RefundFee");
                            var priceRow = $(this).find(".thanhtien").attr("data-thanhtien");
                            if (quantity > 0) {
                                list += sku + ";" + orderID + ";" + orderDetailID + ";" + ProductName + ";" + ProductType + ";" + Giagoc + ";" + Giadaban + ";" +
                                    TienGiam + ";" + Soluongtoida + ";" + RefundType + ";" + quantity + ";" + RefundFee + ";" + priceRow + "|";
                            }
                            $("#<%=hdfListProduct.ClientID%>").val(list);
                            $("#<%=btnSave.ClientID%>").click();
                        });
                    }
                    else {
                        alert('Số lượng cần đổi đã vượt quá số lượng cho phép');
                    }


                }
                else {
                    alert('Vui lòng chọn sản phẩm đổi trả');
                }
            }

            function deleteProduct() {
                var c = confirm("Bạn muốn xóa tất cả sản phẩm?");
                if (c == true) {
                    productDeleteRefunds = productRefunds;
                    productRefunds = [];

                    $(".product-result").remove();

                    getAllPrice();
                    $(".totalpriceorder").html("0");
                    $(".totalproductQuantity").html("0");
                    $(".totalrefund").html("0");
                }
            }

            function getAllPrice() {
                let totalprice = 0;
                let productquantity = 0;
                let totalRefund = 0;

                if ($(".product-result").length > 0) {
                    $(".product-result").each(function () {
                        var giadaban = parseFloat($(this).attr("data-giadaban"));
                        var feeRefund = parseFloat($(this).attr("data-RefundFee"));
                        var changetype = $(this).find(".changeType").val();
                        var quantity = parseFloat($(this).find(".soluongcandoi").val());
                        var refundRow = 0;
                        if (changetype == 2) {
                            refundRow = feeRefund * quantity;
                            $(this).find(".phidoihang").html(formatThousands(feeRefund, ","));
                            $(this).find(".phidoihang").attr("data-RefundFee", feeRefund);
                        }
                        else {
                            $(this).find(".phidoihang").html("0");
                            $(this).find(".phidoihang").attr("data-RefundFee", "0");
                        }
                        var totalRow = giadaban * quantity - refundRow;
                        totalprice += totalRow;
                        productquantity += quantity;
                        totalRefund += refundRow;
                        $(this).find(".thanhtien").attr("data-thanhtien", totalRow);
                        $(this).find(".thanhtien").html(formatThousands(totalRow, ","));

                    });
                }

                $(".totalpriceorder").html(formatThousands(totalprice, ","));
                $(".totalproductQuantity").html(formatThousands(productquantity, ","));
                $(".totalrefund").html(formatThousands(totalRefund, ","));

                $("#<%=hdfTotalQuantity.ClientID%>").val(productquantity);
                $("#<%=hdfTotalRefund.ClientID%>").val(totalRefund);
                $("#<%=hdfTotalPrice.ClientID%>").val(totalprice);
            }

            function addHtmlProductResult(item) {
                let html = "";

                html += "<tr class='product-result' "
                                + "data-orderID='" + item.OrderID + "' "
                                + "data-orderDetailID='" + item.OrderDetailID + "' "
                                + "data-sku='" + item.SKU + "' "
                                + "data-rowIndex='" + item.RowIndex + "' "
                                + "data-ProductName='" + item.ProductName + "' "
                                + "data-ProductType='" + item.ProductType + "' "
                                + "data-Giagoc='" + item.Price + "' "
                                + "data-Giadaban='" + item.ReducedPrice + "' "
                                + "data-TienGiam='" + item.DiscountPerProduct + "' "
                                + "data-Soluongtoida='" + item.QuantityLeft + "' "
                                + "data-RefundFee='" + item.FeeRefund + "' >\n";
                html += "    <td>" + item.OrderID + "</td>\n";
                html += "    <td>" + item.ProductName + "</td>\n";
                html += "    <td>" + item.SKU + "</td>\n";
                html += "    <td class='giagoc' data-giagoc='" + item.Price + "'>" + formatThousands(item.Price) + "</td>\n";
                if (item.DiscountPerProduct > 0) {
                    html += "    <td class='giadaban' data-giadaban='" + item.ReducedPrice + "'>\n"
                    html += "        " + formatThousands(item.ReducedPrice) + "(CK " + formatThousands(item.DiscountPerProduct) + ")\n"
                    html += "    </td>\n";
                }
                else {
                    html += "    <td class='giadaban' data-giadaban='" + item.ReducedPrice + "'>" + formatThousands(item.ReducedPrice) + "</td>\n";
                }
                html += "    <td class='slmax' data-slmax='" + item.QuantityOrder + "'>" + formatThousands(item.QuantityOrder) + "</td>\n";
                html += "    <td class='sltoida' data-sltoida='" + item.QuantityLeft + "'>" + formatThousands(item.QuantityLeft) + "</td>\n";
                html += "    <td class='slcandoi'>\n";
                html += "           <input type='text' min='1' max='" + item.QuantityLeft + "' class='form-control soluongcandoi' style='width: 40%;margin: 0 auto;' value='1'  onkeyup='checkQuantiy($(this))' onkeypress='return event.charCode >= 48 && event.charCode <= 57'/>\n";
                html += "    </td>\n";
                html += "    <td>\n";
                html += "           <select class='changeType form-control' onchange='getAllPrice()'>\n";
                html += "               <option value='1'>Đổi size</option>\n";
                html += "               <option value='2'>Đổi sản phẩm khác</option>\n";
                html += "               <option value='3'>Đổi hàng lỗi</option>\n";
                html += "           </select>\n";
                html += "    </td>\n";
                html += "   <td class='phidoihang'>0</td>\n";
                html += "   <td class='thanhtien' data-thanhtien='" + item.TotalFeeRefund + "'>" + formatThousands(item.TotalFeeRefund) + "</td>\n";
                html += "   <td><a href='javascript:;' class='link-btn' onclick='deleteRow($(this))'><i class='fa fa-trash'></i></a></td>\n";
                html += "</tr>\n";

                return html;
            }

            function searchProduct() {
                let phone = $("#<%=hdfPhone.ClientID%>").val();
                let txtSearch = $("#txtSearch").val();
                let rowIndex = 1;
                let totalCanChange = $("#<%=hdfTotalCanchange.ClientID%>").val();
                let totalProductRefund = $(".totalproductQuantity").val();
                let isProductNew = true; // true when QuantityLeft == QuantityRefund or SKU != txtSearch
                var product = null;

                if (isBlank(phone)) {
                    alert('Vui lòng nhập số điện thoại.');
                    return;
                }

                if (isBlank(txtSearch)) {
                    alert('Vui lòng nhập nội dung tìm kiếm');
                    return;
                }

                if (totalCanChange <= totalProductRefund) {
                    alert('Số lượng sản phẩm đổi trả đã đạt tối đa!');
                    return;
                }

                if (productRefunds.length > 0) {
                    for (let i = 0; i < productRefunds.length; i++) {
                        let item = productRefunds[i];

                        if (item.SKU.toUpperCase() == txtSearch.toUpperCase()) {
                            if (item.QuantityLeft > item.QuantityRefund) {
                                item.QuantityRefund += 1;

                                $(".product-result").each(function () {
                                    let orderID = $(this).attr("data-orderID");
                                    let orderDetailID = $(this).attr("data-orderDetailID");
                                    let sku = $(this).attr("data-sku");

                                    if (item.isTarget(orderID, orderDetailID, sku)) {
                                        $("#txtSearch").val("");
                                        $(this).find(".soluongcandoi").val(formatThousands(item.QuantityRefund));
                                        getAllPrice();

                                        return;
                                    }
                                });

                                isProductNew = false;
                                break;
                            } else {
                                isProductNew = true

                                if (rowIndex <= item.RowIndex) {
                                    rowIndex = item.RowIndex + 1;
                                }
                            }
                        } else {
                            isProductNew = true;
                        }
                    }
                }

                if (isProductNew) {

                    productDeleteRefunds.forEach(function (item) {
                        if (item.SKU.toUpperCase() == txtSearch.toUpperCase()) {
                            if (product == null) {
                                product = item;
                            } else {
                                if (product.rowIndex > item.RowIndex) {
                                    product = item;
                                }
                            }
                        }
                    });

                    if (product != null) {
                        productDeleteRefunds = productDeleteRefunds.filter(function (item) {
                            return item.SKU != product.SKU && item.RowIndex != product.RowIndex
                        });

                        productRefunds.push(product);

                        $(".content-product").append(addHtmlProductResult(product));

                        $("#txtSearch").val("");
                        if (productRefunds.length > 0) {
                            $(".excute-in").show();
                        }

                        getAllPrice();

                    } else {
                        $.ajax({
                            type: "POST",
                            url: "/them-don-tra-hang.aspx/getProduct",
                            data: "{phone:'" + phone + "', sku:'" + txtSearch + "', rowIndex:" + rowIndex + "}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                var data = JSON.parse(msg.d);

                                if (data != null) {
                                    product = new ProductRefundModel(
                                        OrderID = data.OrderID
                                        , OrderDetailID = data.OrderDetailID
                                        , RowIndex = data.RowIndex
                                        , ProductName = data.ProductName
                                        , ProductType = data.ProductType
                                        , SKU = data.SKU
                                        , Price = data.Price
                                        , ReducedPrice = data.ReducedPrice
                                        , DiscountPerProduct = data.DiscountPerProduct
                                        , QuantityOrder = data.QuantityOrder
                                        , QuantityLeft = data.QuantityLeft
                                        , QuantityRefund = 1
                                        , ChangeType = data.ChangeType
                                        , FeeRefund = data.FeeRefund
                                        );

                                    productRefunds.push(product);

                                    $(".content-product").append(addHtmlProductResult(product));

                                    $("#txtSearch").val("");
                                    if (productRefunds.length > 0) {
                                        $(".excute-in").show();
                                    }

                                    getAllPrice();
                                }
                                else {
                                    alert('Không tìm thấy sản phẩm');
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

            function checkQuantiy(obj) {
                let row = obj.parent().parent();
                let orderID = parseFloat(row.attr("data-orderid"));
                let orderDetailID = parseFloat(row.attr("data-orderdetailid"));
                let sku = row.attr("data-sku");

                let currentVal = parseFloat(obj.val());

                if (currentVal < obj.attr("min")) {
                    currentVal = 1;
                }
                else if (currentVal > obj.attr("max")) {
                    currentVal = obj.attr("max");
                }

                obj.val(currentVal);
                productRefunds.forEach(function (product) {
                    if (product.isTarget(orderID, orderDetailID, sku)) {
                        product.QuantityRefund = currentVal;
                    }
                });

                getAllPrice();
            }

            function deleteRow(obj) {
                let c = confirm('Bạn muốn xóa sản phẩm này?');

                if (c) {
                    let row = obj.parent().parent();
                    let orderID = parseFloat(row.attr("data-orderid"));
                    let orderDetailID = parseFloat(row.attr("data-orderdetailid"));
                    let sku = row.attr("data-sku");

                    productRefunds.forEach(function (product) {
                        if (product.isTarget(orderID, orderDetailID, sku)) {
                            product.QuantityRefund = 1; // return default
                            productDeleteRefunds.push(product);
                        }
                    });

                    productRefunds = productRefunds.filter(function (product) {
                        return !product.isTarget(orderID, orderDetailID, sku);
                    });

                    row.remove();
                    getAllPrice();

                    if (productRefunds.length == 0) {
                        $(".excute-in").hide();

                    }

                }
            }

            function searchCustomer2() {
                searchCustomer();
                //checkCustomer();
            }

            function checkCustomer() {
                
                    var phone = $("#<%=txtPhone.ClientID%>").val();

                    $.ajax({
                        type: "POST",
                        url: "/them-don-tra-hang.aspx/checkphone",
                        data: "{phonefullname:'" + phone + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            if (msg.d != "0") {
                                var data = JSON.parse(msg.d);
                                if (data.CustleftCanchange == "full") {
                                    alert('Bạn đã đổi hết số lượng sản phẩm được phép đổi');
                                }
                                else if (data.CustleftCanchange == "nocustomer") {
                                    alert('Không tìm thấy khách hàng trong hệ thống');
                                }
                                else {
                                    $(".result-check").html("<span class=\"numcanchange\">Số lượng sản phẩm được phép đổi tối đa: " + data.CustleftCanchange + " sản phẩm</span>");
                                    $("#<%=hdfTotalCanchange.ClientID%>").val(data.CustleftCanchange);
                                    $("#<%=hdfPhone.ClientID%>").val(phone);
                                }
                            }
                        },
                        error: function (xmlhttprequest, textstatus, errorthrow) {
                            alert('lỗi');
                        }
                    });
                
                
            }

            var formatThousands = function (n, dp) {
                var s = '' + (Math.floor(n)), d = n % 1, i = s.length, r = '';
                while ((i -= 3) > 0) { r = ',' + s.substr(i, 3) + r; }
                return s.substr(0, i + 3) + r +
                    (d ? '.' + Math.round(d * Math.pow(10, dp || 2)) : '');
            };
        </script>
    </telerik:RadScriptBlock>
</asp:Content>
