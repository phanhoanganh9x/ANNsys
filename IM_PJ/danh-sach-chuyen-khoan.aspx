<%@ Page Title="Danh sách đơn hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="danh-sach-chuyen-khoan.aspx.cs" Inherits="IM_PJ.danh_sach_chuyen_khoan" EnableSessionState="ReadOnly" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="/Scripts/moment.min.js"></script>
    <script src="/Scripts/moment-with-locales.min.js"></script>
    <script src="/Scripts/bootstrap-datetimepicker.min.js"></script>

    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title left">Danh sách đơn hàng 
                        <span>(<asp:Literal ID="ltrNumberOfOrder" runat="server" EnableViewState="false"></asp:Literal>đơn)
                        </span>
                    </h3>
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
                                <div class="col-md-5">
                                    <asp:TextBox ID="txtSearchOrder" runat="server" CssClass="form-control" placeholder="Tìm đơn hàng" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlCreatedDate" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Tất cả thời gian"></asp:ListItem>
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
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlCreatedBy" runat="server" CssClass="form-control"></asp:DropDownList>
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
                                <div class="col-md-5">
                                    <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Loại"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Lẻ"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Sỉ"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlPaymentStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Thanh toán"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Chưa thanh toán"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Thanh toán thiếu"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Đã thanh toán"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-1">
                                    <a href="/danh-sach-chuyen-khoan" class="btn primary-btn h45-btn"><i class="fa fa-times" aria-hidden="true"></i></a>
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
                            <table class="table table-checkable table-product table-new-product">
                                <tbody>
                                    <asp:Literal ID="ltrList" runat="server" EnableViewState="false"></asp:Literal>
                                </tbody>
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

        <!-- Modal -->
        <div class="modal fade" id="TransferBankModal" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Cập nhật thông tin chuyển khoản</h4>
                    </div>
                    <div class="modal-body">
                        <asp:HiddenField ID="hdOrderID" runat="server" />
                        <div class="row">
                            <div class="col-xs-3">
                                <p>Chuyển từ</p>
                            </div>
                            <div class="col-xs-9">
                                <asp:DropDownList ID="ddlCustomerBank" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-3">
                                <p>Tài khoản nhận</p>
                            </div>
                            <div class="col-xs-9">
                                <asp:DropDownList ID="ddlAccoutBank" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-3">
                                <p>Trạng Thái</p>
                            </div>
                            <div class="col-xs-9">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-3">
                                <p>Tổng đơn</p>
                            </div>
                            <div class="col-xs-9 text-right">
                                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control text-align-right" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-3">
                                <p>Đã nhận</p>
                            </div>
                            <div class="col-xs-9">
                                <asp:TextBox ID="txtMoneyReceived" runat="server" CssClass="form-control text-right" placeholder="Số tiền khách đã chuyển" data-type="currency" onkeypress='return event.charCode >= 48 && event.charCode <= 57'></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-3">
                                <p>Thời gian chuyển</p>
                            </div>
                            <div class="col-xs-9">
                                <div class="form-group">
                                    <div class="input-group date" id="dtDoneAt">
                                        <input type="text" class="form-control" />
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="closeTransfer" type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <button id="updateTransfer" type="button" class="btn btn-primary">Save changes</button>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdfcreate" runat="server" />
        <script type="text/javascript">
            $(document).ready(() => {
                $("button[data-toggle='modal']").click(e => {
                    let row = e.currentTarget.parentNode.parentNode;
                    let modal = $("#TransferBankModal");
                    let orderIDDOM = modal.find("#<%=hdOrderID.ClientID%>");
                    let cusBankDOM = modal.find("#<%=ddlCustomerBank.ClientID%>");
                    let accBankDOM = modal.find("#<%=ddlAccoutBank.ClientID%>");
                    let priceDOM = modal.find("#<%=txtPrice.ClientID%>");
                    let moneyReceivedDOM = modal.find("#<%=txtMoneyReceived.ClientID%>");
                    let statusDOM = modal.find("#<%=ddlStatus.ClientID%>");
                    let pickerDOM = modal.find('#dtDoneAt');

                    orderIDDOM.val(row.dataset["orderid"]);
                    pickerDOM.datetimepicker({
                        format: 'YYYY-MM-DD hh:mm:ss',
                        date: new Date()
                    });

                    let moneyReceive = row.dataset["moneyreceived"]
                    if (moneyReceive)
                    {
                        moneyReceivedDOM.val(formatThousands(row.dataset["moneyreceived"]));
                    }
                    else
                    {
                        moneyReceivedDOM.val("")
                    }

                    let status = row.dataset["statusid"];
                    // Đã nhận tiền
                    if (status && status == 1) {
                        cusBankDOM.val(row.dataset["cusbankid"]);
                        accBankDOM.val(row.dataset["accbankid"]);
                        if (row.dataset["moneyreceived"]) {
                            moneyReceivedDOM.val(formatThousands(row.dataset["moneyreceived"]));
                        }
                        pickerDOM.val(row.dataset["doneat"]);
                    }

                    statusDOM.val(status)
                    priceDOM.val(formatThousands(row.dataset["price"]))
                })

                // Change status tien reset = 0
                $("#<%=ddlStatus.ClientID%>").change(e => {
                    let status = $(this).val();
                    let moneyReceivedDOM = modal.find("#<%=txtMoneyReceived.ClientID%>");

                    if (status != 1)
                    {
                        moneyReceivedDOM.val("");
                        moneyReceivedDOM.attr("disabled", true);
                    }
                    else
                    {
                        moneyReceivedDOM.attr("disabled", false);
                    }
                });

                $("#updateTransfer").click(e => {
                    let orderID = $("#<%=hdOrderID.ClientID%>").val();
                    let cusBankID = $("#<%=ddlCustomerBank.ClientID%>").val();
                    let accBankID = $("#<%=ddlAccoutBank.ClientID%>").val();
                    let status = $("#<%=ddlStatus.ClientID%>").val();
                    let money = $("#<%=txtMoneyReceived.ClientID%>").val().replace(/\,/g, '');
                    let doneAt = $("#dtDoneAt").data('date');

                    let data = {
                        'OrderID': orderID,
                        'CusBankID': cusBankID,
                        'AccBankID': accBankID,
                        'Money': money ? money : 0,
                        'DoneAt': doneAt,
                        'Status': 1,
                    };
                    $.ajax({
                        type: "POST",
                        url: "/danh-sach-chuyen-khoan.aspx/updateTransfer",
                        data: JSON.stringify({'transfer': data}),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            let row = $("tr[data-orderid='" + orderID + "'");
                            let cusBankName = $("#<%=ddlCustomerBank.ClientID%> :selected").text();
                            let accBankName = $("#<%=ddlAccoutBank.ClientID%> :selected").text();
                            let statusName = $("#<%=ddlStatus.ClientID%> :selected").text();

                            // Update screen
                            row.attr("data-cusbankid", cusBankID);
                            row.attr("data-cusbankname", cusBankName);
                            row.attr("data-accbankid", accBankID);
                            row.attr("data-accbankname", accBankName);
                            row.attr("data-statusid", status);
                            row.attr("data-statusname", statusName);
                            row.attr("data-moneyreceived", money);
                            row.attr("data-doneat", doneAt);

                            row.find('#cusBankName').html(cusBankName);
                            row.find('#accBankName').html(accBankName);
                            row.find('#statusName').html(statusName);
                            row.find('#moneyReceive').html(formatThousands(money));
                            row.find('#doneAt').html(doneAt);

                            $("#closeTransfer").click();
                        },
                        error: function (xmlhttprequest, textstatus, errorthrow) {
                            swal("Thông báo", "Đã có vấn đề trong việc cập nhật thông tin chuyển khoản", "error");
                        }
                    });
                });
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
        </script>
    </main>
</asp:Content>
