<%@ Page Title="Danh sách giao hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="danh-sach-van-chuyen.aspx.cs" Inherits="IM_PJ.danh_sach_van_chuyen" EnableSessionState="ReadOnly" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/moment.min.js"></script>
    <script src="/Scripts/moment-with-locales.min.js"></script>
    <script src="/Scripts/bootstrap-datetimepicker.min.js"></script>
    <style>
        #invoice-image li {
            list-style: none;
        }
        #invoice-image img {
            width: 60%;
        }
        @media (max-width: 768px) {
            table.shop_table_responsive thead {
	            display: none;
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(1):before {
                content: none;
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(1) {
                text-align: left;
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(2):before {
                content: "#";
                font-size: 20px;
                margin-right: 2px;
            }
            table.shop_table_responsive > tbody > tr > td:nth-of-type(2) {
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
            #invoice-image img {
                width: 40%;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title left">Danh sách giao hàng  <span>(<asp:Literal ID="ltrNumberOfOrder" runat="server" EnableViewState="false"></asp:Literal> đơn)
                        </span>
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtSearchOrder" runat="server" CssClass="form-control" placeholder="Tìm đơn hàng" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlShippingType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0" Text="Kiểu giao hàng"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Chuyển xe"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="Nhân viên giao"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0" Text="Kiểu thanh toán"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Tiền mặt"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Chuyển khoản"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Thu hộ"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Công nợ"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlCreatedBy" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlCreatedDate" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Thời gian đơn hàng"></asp:ListItem>
                                        <asp:ListItem Value="today" Text="Hôm nay"></asp:ListItem>
                                        <asp:ListItem Value="yesterday" Text="Hôm qua"></asp:ListItem>
                                        <asp:ListItem Value="beforeyesterday" Text="Hôm kia"></asp:ListItem>
                                        <asp:ListItem Value="week" Text="Tuần này"></asp:ListItem>
                                        <asp:ListItem Value="7days" Text="7 ngày"></asp:ListItem>
                                        <asp:ListItem Value="thismonth" Text="Tháng này"></asp:ListItem>
                                        <asp:ListItem Value="lastmonth" Text="Tháng trước"></asp:ListItem>
                                        <asp:ListItem Value="beforelastmonth" Text="Tháng trước nữa"></asp:ListItem>
                                        <asp:ListItem Value="30days" Text="30 ngày"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1">
                                    <a href="javascript:;" onclick="searchOrder()" class="btn primary-btn h45-btn"><i class="fa fa-search"></i></a>
                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn primary-btn h45-btn" OnClick="btnSearch_Click" Style="display: none" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlTransportCompany" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlShipperFilter" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlInvoiceStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0" Text="Biên nhận gửi hàng"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Có"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Không"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlDeliveryStatusFilter" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0" Text="Trạng thái giao hàng"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Đã giao"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Chưa giao"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Đang giao"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlDeliveryStartAt" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Thời gian giao hàng"></asp:ListItem>
                                        <asp:ListItem Value="today" Text="Hôm nay"></asp:ListItem>
                                        <asp:ListItem Value="yesterday" Text="Hôm qua"></asp:ListItem>
                                        <asp:ListItem Value="beforeyesterday" Text="Hôm kia"></asp:ListItem>
                                        <asp:ListItem Value="week" Text="Tuần này"></asp:ListItem>
                                        <asp:ListItem Value="7days" Text="7 ngày"></asp:ListItem>
                                        <asp:ListItem Value="thismonth" Text="Tháng này"></asp:ListItem>
                                        <asp:ListItem Value="lastmonth" Text="Tháng trước"></asp:ListItem>
                                        <asp:ListItem Value="beforelastmonth" Text="Tháng trước nữa"></asp:ListItem>
                                        <asp:ListItem Value="30days" Text="30 ngày"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1">
                                    <a href="/danh-sach-van-chuyen" class="btn primary-btn h45-btn"><i class="fa fa-times" aria-hidden="true"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-2">
                                    <a href="javascript:;" class="btn primary-btn fw-btn width-100" onclick="getDeliverySession()">
                                        <i class="fa fa-inbox" aria-hidden="true"></i> Lấy đơn hàng đã chọn
                                    </a>
                                </div>
                                <div class="col-md-2">
                                    <a href="javascript:;" class="btn primary-btn fw-btn width-100" onclick="deleteAllDeliverySession()">
                                        <i class="fa fa-remove" aria-hidden="true"></i> Xóa tất cả đơn hàng
                                    </a>
                                </div>
                                <div class="col-md-2">
                                    <a href="javascript:;" class="btn primary-btn fw-btn width-100" onclick="openPrintModal()">
                                        <i class="fa fa-print" aria-hidden="true"></i> In phiếu giao hàng
                                    </a>
                                </div>
                                <div id="numberPrint" class="col-md-3">
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
                            <table class="table shop_table_responsive table-checkable table-product table-new-product">
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

        <!-- Modal cập nhật giao hàng-->
        <div class="modal fade" id="TransferBankModal" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Cập nhật thông tin giao hàng</h4>
                    </div>
                    <div class="modal-body">
                        <asp:HiddenField ID="hdOrderID" runat="server" />
                        <div class="row form-group">
                            <div class="col-md-3 col-xs-4">
                                <p>Trạng thái</p>
                            </div>
                            <div class="col-md-9 col-xs-8">
                                <asp:DropDownList ID="ddlDeliveryStatusModal" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0" Text="Trạng thái giao hàng"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Đã giao"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Chưa giao"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Đang giao"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-3 col-xs-4">
                                <p>Người giao</p>
                            </div>
                            <div class="col-md-9 col-xs-8">
                                <asp:DropDownList ID="ddlShipperModal" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-3 col-xs-4">
                                <p>Chành xe</p>
                            </div>
                            <div class="col-md-9 col-xs-8">
                                <asp:DropDownList ID="ddlTransferCompanyModal" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-3 col-xs-4">
                                <p>Thu hộ</p>
                            </div>
                            <div class="col-md-9 col-xs-8">
                                <asp:TextBox ID="txtColOfOrd" runat="server" CssClass="form-control text-right" placeholder="Số tiền thu hộ" data-type="currency" onkeypress='return event.charCode >= 48 && event.charCode <= 57'></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-3 col-xs-4">
                                <p>Phí</p>
                            </div>
                            <div class="col-md-9 col-xs-8">
                                <asp:TextBox ID="txtCosOfDel" runat="server" CssClass="form-control text-right" placeholder="Phí vận chuyển" data-type="currency" onkeypress='return event.charCode >= 48 && event.charCode <= 57'></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-3 col-xs-4">
                                <p>Biên nhận</p>
                            </div>
                            <div class="col-md-9 col-xs-8">
                                <asp:FileUpload runat="server" ID="uploadInvoiceImage" disabled onchange='showImageGallery(this,$(this));'/>
                                <ul id="invoice-image"></ul>
                                <asp:HiddenField ID="hdfImageOld" runat="server" />
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-3 col-xs-4">
                                <p>Ngày giao</p>
                            </div>
                            <div class="col-md-9 col-xs-8">
                                <div class="input-group date" id="dtDoneAt">
                                    <input type="text" class="form-control" />
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3 col-xs-4">
                                <p>Ghi chú</p>
                            </div>
                            <div class="col-md-9 col-xs-8">
                                <asp:TextBox ID="txtNote" runat="server" CssClass="form-control text-left" placeholder="Ghi chú"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="closeDelivery" type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                        <button id="updateDelivery" type="button" class="btn btn-primary">Lưu</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Print Delivery -->
        <div class="modal fade" id="PrintModal" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Cập nhật thông tin phiếu</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row form-group">
                            <div class="col-xs-3">
                                <p>Người giao</p>
                            </div>
                            <div class="col-xs-9">
                                <asp:DropDownList ID="ddfShipperPrintModal" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="closePrint" type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                        <button id="shipperPrint" type="button" class="btn btn-primary" onclick="startPrint(5)">Phiếu nhân viên giao</button>
                        <button id="transportPrint" type="button" class="btn btn-primary" onclick="startPrint(4)">Phiếu chuyển xe</button>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdfcreate" runat="server" />
        <asp:HiddenField ID="hdfSession" runat="server" />
        <script type="text/javascript">
            class OrderChoosed {
                constructor(OrderID, ShippingType, CreatedDate) {
                    this.OrderID = OrderID;
                    this.ShippingType = ShippingType;
                    this.CreatedDate = CreatedDate;
                }
            };

            // Danh sách các order đã chọn để in
            var orderChoosed = [];

            function init() {
                // Hổ trợ xử lý check or uncheck all print
                checkPrintAll();
                // Lấy danh sách order đã chọn
                let data = JSON.parse($("#<%=hdfSession.ClientID%>").val());

                if ($.isArray(data)) {
                    data.forEach((item) => {
                        let createdDate = "";
                        // Format CreateDate
                        var matchs = item.CreatedDate.match(/\d+/g);
                        if (matchs) {
                            let date = new Date(parseInt(matchs[0]));
                            if (date) {
                                createdDate = date.format("yyyy-MM-dd");
                            }
                        }

                        // Thêm vào mảng order đã chọn
                        orderChoosed.push(new OrderChoosed(item.OrderID, item.ShippingType, createdDate));
                    });

                    let numberPrint = orderChoosed.length;
                    if (numberPrint > 0) {
                        $("#numberPrint").html("<h5>(" + formatNumber(numberPrint.toString()) + " số lượng đơn hàng In)</h5>")
                    }
                }
            };

            $(document).ready(() => {
                init();

                $("button[data-toggle='modal']").click(e => {
                    let row = e.currentTarget.parentNode.parentNode;
                    let paymenttype = row.dataset["paymenttype"];
                    let shippingtype = row.dataset["shippingtype"];
                    let modal = $("#TransferBankModal");
                    let orderIDDOM = modal.find("#<%=hdOrderID.ClientID%>");
                    let deliveryDOM = modal.find("#<%=ddlDeliveryStatusModal.ClientID%>");
                    let transferCompanyDOM = modal.find("#<%=ddlTransferCompanyModal.ClientID%>");
                    let colOfOrdDOM = modal.find("#<%=txtColOfOrd.ClientID%>");
                    let shipperDOM = modal.find("#<%=ddlShipperModal.ClientID%>");
                    let cosOfDelDOM = modal.find("#<%=txtCosOfDel.ClientID%>");
                    let pickerDOM = modal.find('#dtDoneAt');
                    let noteDOM = modal.find("#<%=txtNote.ClientID%>");

                    // Init modal
                    pickerDOM.datetimepicker({
                        format: 'DD/MM/YYYY HH:mm',
                        date: new Date()
                    });

                    // Không phải gửi xe
                    if (shippingtype != 4) {
                        transferCompanyDOM.parent().parent().attr("hidden", true);
                        transferCompanyDOM.val("");
                    }
                    else {
                        transferCompanyDOM.parent().parent().removeAttr("hidden");
                        transferCompanyDOM.val(row.dataset["transfercompany"]);
                        transferCompanyDOM.attr("disabled", true);
                    }

                    // Không phải là thu hộ
                    if (paymenttype != 3) {
                        colOfOrdDOM.parent().parent().attr("hidden", true);
                        colOfOrdDOM.val("");
                    }
                    else {
                        colOfOrdDOM.parent().parent().removeAttr("hidden");
                        colOfOrdDOM.val(row.dataset["coloford"]);
                    }

                    // Không phải hình thức nhân viên giao
                    if (shippingtype != 5) {
                        cosOfDelDOM.parent().parent().attr("hidden", true);
                        cosOfDelDOM.val("");
                    }
                    else {
                        cosOfDelDOM.parent().parent().removeAttr("hidden");
                        cosOfDelDOM.val(row.dataset["cosofdev"]);
                    }

                    orderIDDOM.val(row.dataset["orderid"]);
                    $("#<%=uploadInvoiceImage.ClientID%>").val("");
                    $("#invoice-image").html("");
                    $("#<%=hdfImageOld.ClientID%>").val("");

                    if (row.dataset["invoiceimage"]) {
                        $("#<%=hdfImageOld.ClientID%>").val(row.dataset["invoiceimage"]);
                        addInvoiceImage(row.dataset["invoiceimage"]);
                    }

                    deliveryDOM.val(row.dataset["deliverystatus"]);

                    if (row.dataset["shipperid"])
                        shipperDOM.val(row.dataset["shipperid"]);
                    else
                        shipperDOM.val(0);

                    if (row.dataset["deliverydate"])
                        pickerDOM.data("DateTimePicker").date(row.dataset["deliverydate"]);
                    else
                        pickerDOM.data("DateTimePicker").date(moment(new Date).format('DD/MM/YYYY HH:mm'));

                    noteDOM.val(row.dataset["shippernote"]);

                    // xử lý khi chọn trạng thái giao hàng
                    if (row.dataset["deliverystatus"] == "1") {
                        $("#<%=uploadInvoiceImage.ClientID%>").prop('disabled', false);
                    }
                    else {
                        $("#<%=uploadInvoiceImage.ClientID%>").prop('disabled', true);
                    }
                });

                $("#updateDelivery").click(e => {
                    let orderID = $("#<%=hdOrderID.ClientID%>").val();
                    let status = $("#<%=ddlDeliveryStatusModal.ClientID%>").val();
                    let invoiceImages = $("#<%=uploadInvoiceImage.ClientID%>").get(0).files;
                    let imageOld = $("#<%=hdfImageOld.ClientID%>").val();
                    let colOfOrd = $("#<%=txtColOfOrd.ClientID%>").val();
                    let shipperID = $("#<%=ddlShipperModal.ClientID%>").val();
                    let cosOfDel = $("#<%=txtCosOfDel.ClientID%>").val();
                    let startAt = $("#dtDoneAt").data('date');
                    let note = $("#<%=txtNote.ClientID%>").val();

                    let data = {
                        'OrderID': orderID,
                        'ShipperID': shipperID,
                        'Status': status,
                        'Image': imageOld,
                        'COD': cosOfDel ? cosOfDel : 0,
                        'COO': colOfOrd ? colOfOrd : 0,
                        'StartAt': formatDateToInsert(startAt),
                        'ShipNote': note
                    };


                    var fileData = new FormData();
                    fileData.append("ImageNew", invoiceImages.length > 0 ? invoiceImages[0] : null);
                    fileData.append("Delivery", JSON.stringify(data));

                    if ((status == 1 || status == 3) && shipperID == 0) {
                        swal("Thông báo", "Chưa chọn người giao hàng", "error");
                    }
                    else {
                        $.ajax({
                            url: "DeliveryHandler.ashx",
                            type: "POST",
                            data: fileData,
                            contentType: false,
                            processData: false,
                            beforeSend: function () {
                                HoldOn.open();
                            },
                            success: function (result) {
                                let status = $("#<%=ddlDeliveryStatusModal.ClientID%>").val();
                                let row = $("tr[data-orderid='" + orderID + "']");
                                let deliveryStatusDom = row.children("#deliveryStatus").children("span");
                                let checkPrint = row.find("td>input[type='checkbox']");
                                let shiperName = $("#<%=ddlShipperModal.ClientID%> :selected").text();
                                let deliveryStatusName = $("#<%=ddlDeliveryStatusModal.ClientID%> :selected").text();

                                // Update screen
                                row.attr("data-shipperid", shipperID);
                                row.attr("data-deliverystatus", status);
                                row.attr("data-invoiceimage", result);
                                row.attr("data-coloford", colOfOrd);
                                row.attr("data-cosofdev", cosOfDel);
                                row.attr("data-deliverydate", startAt);
                                row.attr("data-shippernote", note);

                                // Checkbox Print disable when status delivery != "Chưa giao hàng"
                                if (status != "2")
                                {
                                    checkPrint.prop('checked', false);
                                    checkPrint.attr('disabled', true);
                                }
                                else
                                {
                                    checkPrint.removeAttr('disabled');
                                }

                                if (shipperID == "0")
                                {
                                    row.children("#shiperName").html("");
                                }
                                else
                                {
                                    row.children("#shiperName").html(shiperName);
                                }

                                deliveryStatusDom.removeClass();

                                switch (status) {
                                    case "1":
                                        deliveryStatusDom.addClass("bg-green");
                                        row.children("#delDate").html(formatDate(startAt));
                                        if (colOfOrd)
                                            row.children("#colOfOrd").html("<strong>" +
                                                 formatThousands(colOfOrd) + "</strong>");
                                        if (cosOfDel)
                                            row.children("#cosOfDel").html("<strong>" + formatThousands(cosOfDel) + "</strong>");
                                        if (result) {
                                            if (row.children("#updateButton").find('#downloadInvoiceImage').length) {
                                                row.find("#downloadInvoiceImage").show();
                                                row.find("#downloadInvoiceImage").attr("href", result);
                                            }
                                            else {
                                                row.children("#updateButton").append("<a id='downloadInvoiceImage' href='" + result + "' title='Biên nhận gửi hàng' target='_blank' class='btn primary-btn btn-blue h45-btn'><i class=\"fa fa-file-text-o\" aria-hidden=\"true\"></i></a>");
                                            }
                                        }
                                        else {
                                            row.find("#downloadInvoiceImage").hide();
                                        }
                                        break;
                                    case "2":
                                        deliveryStatusDom.addClass("bg-red");
                                        row.find("#downloadInvoiceImage").hide();
                                        row.children("#delDate").html("");
                                        break;
                                    case "3":
                                        deliveryStatusDom.addClass("bg-blue");
                                        row.find("#downloadInvoiceImage").hide();
                                        row.children("#delDate").html("");
                                        break;
                                    default:
                                        deliveryStatusName = "";
                                        break;
                                }

                                deliveryStatusDom.html(deliveryStatusName);

                                $("#closeDelivery").click();
                                HoldOn.close();
                            },
                            error: function (err) {
                                HoldOn.close();
                                swal("Thông báo", "Đã có vấn đề trong việc cập nhật thông tin vận chuyển", "error");
                            }
                        });
                    }
                });
            });

            $("#<%=ddlDeliveryStatusModal.ClientID%>").on('change', function() {
                if (this.value == "1") {
                    $("#<%=uploadInvoiceImage.ClientID%>").prop('disabled', false);
                }
                else {
                    $("#<%=uploadInvoiceImage.ClientID%>").prop('disabled', true);
                }
            });

            function clickImage() {
                $("#<%=uploadInvoiceImage.ClientID%>").click();
            }

            function formatDateToInsert(dateString) {
                var date = dateString.split(' ');
                var datetmp = date[0].split('/');
                var hourtmp = date[1].split(':');

                return datetmp[2] + '-' + datetmp[1] + '-' + datetmp[0] + ' ' + hourtmp[0] + ':' + hourtmp[1] + ':00';
            }

            function formatDate(dateString) {
                var date = dateString.split(' ');
                var datetmp = date[0].split('/');
                var hourtmp = date[1].split(':');

                return datetmp[0] + '/' + datetmp[1] + ' ' + hourtmp[0] + ':' + hourtmp[1];
            }

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

            function searchOrder() {
                $("#<%= btnSearch.ClientID%>").click();
            }

            var formatThousands = function (n, dp) {
                var s = '' + (Math.floor(n)), d = n % 1, i = s.length, r = '';
                while ((i -= 3) > 0) { r = ',' + s.substr(i, 3) + r; }
                return s.substr(0, i + 3) + r +
                    (d ? '.' + Math.round(d * Math.pow(10, dp || 2)) : '');
            };

            function isNumber(evt) {
                evt = (evt) ? evt : window.event;
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            }

            // Jquery Dependency
            $("input[data-type='currency']").on({
                keyup: function () {
                    formatCurrency($(this));
                },
                blur: function () {
                    formatCurrency($(this), "blur");
                }
            });


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

            function showImageGallery(input, obj) {
                if (input.files) {
                    if (input.files && input.files[0] && input.files[0].type.match("image.*")) {
                        let FR = new FileReader();
                        FR.addEventListener("load", function (e) {
                            addInvoiceImage(e.target.result);
                        });
                        FR.readAsDataURL(input.files[0]);
                    }
                }
            }

            function deleteImageGallery(obj) {
                swal({
                    title: "Xác nhận",
                    text: "Cưng có chắc xóa hình này?",
                    type: "warning",
                    showCancelButton: true,
                    closeOnConfirm: true,
                    cancelButtonText: "Đợi em xem tí!",
                    confirmButtonText: "Chắc chắn sếp ơi..",
                }, function (isConfirm) {
                    if (isConfirm) {
                        $("#invoice-image").html("");
                        $("#<%=uploadInvoiceImage.ClientID%>").val("");
                    }
                });
            }

            function addInvoiceImage(src) {
                let imageHTML = "";
                imageHTML += "<li>"
                imageHTML += "    <img onclick='clickImage()' src='" + src + "' />"
                imageHTML += "    <a href='javascript:;' onclick='deleteImageGallery($(this))' class='btn-delete'>"
                imageHTML += "        <i class='fa fa-times' aria-hidden='true'></i> Xóa hình"
                imageHTML += "    </a>"
                imageHTML += "</li>"
                $("#invoice-image").html(imageHTML);
            }

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

            function addOrderChoose(data)
            {
                $.ajax({
                    type: "POST",
                    url: "/danh-sach-van-chuyen.aspx/addOrderChoose",
                    data: JSON.stringify({ 'deliverySession': data }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (response) => {
                        let data = JSON.parse(response.d);
                        if (data) {
                            orderChoosed = data;

                            // Thể hiện số lượng đơn hàng sẽ In
                            let numberPrint = orderChoosed.length;
                            if (numberPrint > 0) {
                                $("#numberPrint").html("<h5>(" + formatNumber(numberPrint.toString()) + " số lượng đơn hàng In)</h5>")
                            }
                            else {
                                $("#numberPrint").html("");
                            }
                        }
                    },
                    error: (xmlhttprequest, textstatus, errorthrow) => {
                        swal("Thông báo", "Có lỗi trong quá trình thêm order cho in vận chuyển", "error");
                    }
                })
            }

            function deleteOrderChoose(data)
            {
                $.ajax({
                    type: "POST",
                    url: "/danh-sach-van-chuyen.aspx/deleteOrderChoose",
                    data: JSON.stringify({ 'deliverySession': data }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (response) => {
                        let data = JSON.parse(response.d);
                        if (data)
                            orderChoosed = data;
                        else
                            orderChoosed = [];

                        // Thể hiện số lượng đơn hàng sẽ In
                        let numberPrint = orderChoosed.length;
                        if (numberPrint > 0) {
                            $("#numberPrint").html("<h5>(" + formatNumber(numberPrint.toString()) + " số lượng đơn hàng In)</h5>")
                        }
                        else {
                            $("#numberPrint").html("");
                        }
                    },
                    error: (xmlhttprequest, textstatus, errorthrow) => {
                        swal("Thông báo", "Có lỗi trong quá trình xóa order cho in vận chuyển", "error");
                    }
                });
            }

            function changeCheckPrintAll(checked) {
                let childDOM = $("td>input[type='checkbox']").not("[disabled='disabled']");
                let data = [];
                let now = new Date();

                childDOM.each((index, element) => {
                    let parent = element.parentElement.parentElement;
                    let orderID = parent.dataset["orderid"];
                    let shippingType = parent.dataset["shippingtype"];

                    element.checked = checked;
                    data.push(new OrderChoosed(orderID, shippingType, now.format("yyyy-MM-dd")))
                });

                if (checked) {
                    addOrderChoose(data);
                }
                else {
                    deleteOrderChoose(data);
                }
            }

            // Kiểm tra xem các phần tử check box không bị disible trong trang
            // Nếu tất cả là true thì check box all là true
            // Nếu tất cả là false thì check box all là false
            function checkPrintAll() {
                let parentDOM = $("#checkPrintAll");
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

            function changeCheckPrint(self) {
                let parent = self.parent().parent();
                let orderID = parent.data("orderid");
                let shippingType = parent.data("shippingtype");

                // Thông báo khi order được chọn in đã được chuyển trạng thái hoàng tất đơn hàng
                if (self.is(":checked")) {
                    let deliveryStatus = parent.data("deliverystatus");

                    if (deliveryStatus && deliveryStatus == 1)
                    {
                        swal({
                            title: 'Thông báo',
                            text: 'Đơn hàng đã hoàn tất giao hàng.',
                            type: 'warning',
                            showCancelButton: true,
                            closeOnConfirm: true,
                            cancelButtonText: "Để em xem lại...",
                            confirmButtonText: "Tiếp tục cho In",
                        }, function (confirm) {
                            if (confirm)
                                self.prop("checked", true);
                            else
                            {
                                self.prop("checked", false);
                                // Fix bug: Bởi vì swal xử lý bất đồng bộ
                                changeCheckPrint(self);
                            }
                        });
                    }
                }

                if (self.is(":checked")) {
                    let now = new Date();
                    let data = new OrderChoosed(orderID, shippingType, now.format("yyyy-MM-dd"));
                    addOrderChoose([data]);
                }
                else {
                    let item = orderChoosed.filter((item) => { return item.OrderID == orderID; });
                    if (item.length == 1) deleteOrderChoose(item);
                }

                // Hổ trợ xử lý check or uncheck all print
                checkPrintAll();
            }

            function openPrintModal() {
                let shippers = orderChoosed.filter((item) => { return item.ShippingType == 5; })
                let transports = orderChoosed.filter((item) => { return item.ShippingType == 4; })

                if (shippers.length == 0 && transports.lenght == 0) {
                    swal("Thông báo", "Bạn chưa chọn đơn hàng nào!", "error");
                }
                else {
                    // Phiếu nhân viên giao
                    if (shippers.length == 0)
                        $("#shipperPrint").attr("style", "display: none")
                    else
                        $("#shipperPrint").removeAttr("style");
                    // Phiếu chuyển xe
                    if (transports.length == 0)
                        $("#transportPrint").attr("style", "display: none")
                    else
                        $("#transportPrint").removeAttr("style");

                    $("#PrintModal").modal({ show: 'true', backdrop: 'static', keyboard: 'false'})
                }
            }

            function startPrint(shippingType) {
                let shipperID = $("#<%=ddfShipperPrintModal.ClientID%>").val();

                if (shipperID != "0") {
                    let childDOM = null;

                    switch (shippingType) {
                        case 5: // Nhân viên giao hàng
                            childDOM = $("td[data-shippingtype='5']>input[type='checkbox']:checked");
                            break;
                        case 4: // Chuyển nhà xe
                            childDOM = $("td[data-shippingtype='4']>input[type='checkbox']:checked");
                            break;
                        default:
                            break;
                    }

                    //Cập nhật lại thông tin trên màn hình
                    if (childDOM) {
                        childDOM.each((index, element) => {
                            let parent = element.parentElement.parentElement;
                            let orderID = parent.dataset["orderid"]
                            let row = $("tr[data-orderid='" + orderID + "']");
                            let deliveryStatus = row.attr("data-deliverystatus");
                            let deliveryStatusDom = row.children("#deliveryStatus").children("span");
                            let shiperName = $("#<%=ddfShipperPrintModal.ClientID%> :selected").text();
                            let now = new Date();

                            // Update screen
                            row.attr("data-shipperid", shipperID);
                            // Trường hợp khác đơn đã hoàn thành thì chuyển về trạng thái đang giao
                            if (deliveryStatus != "1")
                            {
                                row.attr("data-deliverystatus", 3);
                                row.attr("data-deliverydate", now.format("dd/MM/yyyy HH:mm"));
                            }

                            // Update screen
                            row.children("#shiperName").html(shiperName);
                            // Trường hợp khác đơn đã hoàn thành thì chuyển về trạng thái đang giao
                            if (deliveryStatus != "1")
                            {
                                deliveryStatusDom.removeClass();
                                deliveryStatusDom.addClass("bg-blue");
                                deliveryStatusDom.html("Đang giao");
                                row.find("#downloadInvoiceImage").hide();
                                row.children("#delDate").html("");
                            }
                        });
                    }

                    // Gửi thông tin về server để tao đơn in
                    let url = "/print-delivery?shipperid=" + shipperID + "&shippingtype=" + shippingType;
                    window.open(url);
                    $("#closePrint").click();
                }
                else {
                    swal("Thông báo", "Chưa chọn người giao hàng", "error");
                }
            };

            function getDeliverySession() {
                let url = window.location.href;
                let reg = /\?/g;

                if (url.search(reg) > 0)
                    url = url + "&isdeliverysession=1";
                else
                    url = url + "?isdeliverysession=1"

                let win = window.open(url, '_self');
                if (win) {
                    //Browser has allowed it to be opened
                    win.focus();
                } else {
                    //Browser has blocked it
                    swal("Thông báo", "Vui lòng cho phép cửa sổ bật lên cho trang web này", "error");
                }
            }

            function deleteAllDeliverySession() {
                $.ajax({
                    type: "POST",
                    url: "/danh-sach-van-chuyen.aspx/deleteAllOrderChoose",
                    contentType: "application/json; charset=utf-8",
                    success: (response) => {
                        $("#<%=hdfSession.ClientID%>").val("")
                        orderChoosed = [];
                        $("#numberPrint").html("")

                        let win = window.open('/danh-sach-van-chuyen', '_self');
                        if (win) {
                            //Browser has allowed it to be opened
                            win.focus();
                        } else {
                            //Browser has blocked it
                            swal("Thông báo", "Vui lòng cho phép cửa sổ bật lên cho trang web này", "error");
                        }
                    },
                    error: (xmlhttprequest, textstatus, errorthrow) => {
                        swal("Thông báo", "Có lỗi trong quá trình xóa tất cả order đã check", "error");
                    }
                })
            }
        </script>
    </main>
</asp:Content>
