// get highest customer discount
function getCustomerDiscount(custID) {
    $.ajax({
        type: "POST",
        url: "/pos.aspx/getCustomerDiscount",
        data: "{ID:'" + custID + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $(".discount-info").html("").hide();
            $(".refund-info").html("").hide();
            if (msg.d !== "null") {
                var data = JSON.parse(msg.d);

                $(".discount-info").html("<strong>* Chiết khấu của khách: " + formatThousands(data.Discount, ",") + "/cái (đơn từ " + data.QuantityProduct + " cái).</strong>").show();

                let strHTML = "";

                if (data.FeeRefund == "0") {
                    strHTML += "<strong>* Miễn phí đổi hàng</strong>";
                }
                else {
                    strHTML += "<strong>* Phí đổi trả hàng: " + formatThousands(data.FeeRefund, ",") + "/cái.</strong>";
                    strHTML += "<br><strong>* Số lượng đổi trả miễn phí: " + data.RefundQuantityNoFee + " cái.</strong>"
                    strHTML += "<br><strong>* Số lượng đổi trả có phí: " + data.RefundQuantityFee + " cái.</strong>"
                }

                if (data.IsUserApp) {
                    strHTML += "<br><br><strong class='font-green'>Đã đăng ký App</strong>";
                }
                else {
                    strHTML += "<br><br><strong class='font-red'>Chưa đăng ký App</strong>";
                }

                $(".refund-info").html(strHTML).show();
                
                $("input[id$='_hdfIsDiscount']").val(data.IsDiscount ? 1 : 0);
                $("input[id$='_hdfDiscountAmount']").val(data.Discount);
                $("input[id$='_hdfQuantityRequirement']").val(data.QuantityProduct);
                $("input[id$='_hdfCustomerFeeChange']").val(data.FeeRefund);
                $("input[id$='_hdfDaysExchange']").val(data.DaysExchange);
                $("input[id$='_hdfRefundQuantityNoFee']").val(data.RefundQuantityNoFee);
                $("input[id$='_hdfRefundQuantityFee']").val(data.RefundQuantityFee);
            }
            else
            {
                $("input[id$='_hdfIsDiscount']").val(0);
                $("input[id$='_hdfDiscountAmount']").val(0);
                $("input[id$='_hdfQuantityProduct']").val(0);
                $("input[id$='_hdfCustomerFeeChange']").val(0);
                $("input[id$='_hdfDaysExchange']").val(0);
                $("input[id$='_hdfRefundQuantityNoFee']").val(0);
                $("input[id$='_hdfRefundQuantityFee']").val(0);
            }
            getAllPrice();
        },
        error: function (xmlhttprequest, textstatus, errorthrow) {
            $(".discount-info").html("").hide();
            $(".refund-info").html("").hide();
            alert('lỗi');
        }
    });
}

// view Customer detail by click button
function viewCustomerDetail(custID) {
    $.ajax({
        type: "POST",
        url: "/pos.aspx/getCustomerDetail",
        data: "{ID:'" + custID + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d !== "null") {
                
                var alldata = JSON.parse(msg.d);

                var data = alldata.Customer;
                var dataDiscout = alldata.AllDiscount;
                var jsonDate = data.CreatedDate;
                var dateString = jsonDate.substr(6);
                var currentTime = new Date(parseInt(dateString));
                var month = currentTime.getMonth() + 1;
                var day = currentTime.getDate();
                var year = currentTime.getFullYear();
                var date = day + "/" + month + "/" + year;
                var html = "";
                html += "<div class=\"responsive-table\">";
                html += "<table class=\"table table-checkable table-product\">";
                html += "   <thead>";
                html += "       <tr>";
                html += "           <td style=\"width:25%;\">Khách hàng:</td>";
                html += "           <td class=\"capitalize\">" + data.CustomerName + "</td>";
                html += "       </tr>";
                if (data.Nick != null && data.Nick != "") {
                    html += "       <tr>";
                    html += "           <td style=\"width:25%;\">Nick đặt hàng:</td>";
                    html += "           <td class=\"capitalize\">" + data.Nick + "</td>";
                    html += "       </tr>";
                }
                
                html += "       <tr>";
                html += "           <td style=\"width:25%;\">Điện thoại</td>";
                html += "           <td>" + data.CustomerPhone + "</td>";
                html += "       </tr>";
                html += "       <tr>";
                html += "           <td style=\"width:25%;\">Địa chỉ</td>";
                html += "           <td class=\"capitalize\">" + data.CustomerAddress + "</td>";
                html += "       </tr>";
                if (data.CustomerEmail != null && data.CustomerEmail != "") {
                    html += "       <tr>";
                    html += "           <td style=\"width:25%;\">Email</td>";
                    html += "           <td>" + data.CustomerEmail + "</td>";
                    html += "       </tr>";
                }
                if (data.Zalo != null && data.Zalo != "") {
                    html += "       <tr>";
                    html += "           <td style=\"width:25%;\">Zalo</td>";
                    html += "           <td>" + data.Zalo + "</td>";
                    html += "       </tr>";
                }
                if (data.Facebook != null && data.Facebook != "") {
                    html += "       <tr>";
                    html += "           <td style=\"width:25%;\">Facebook</td>";
                    html += "           <td>" + data.Facebook + "</td>";
                    html += "       </tr>";
                }
                html += "       <tr>";
                html += "           <td style=\"width:25%;\">Nhân viên phục vụ</td>";
                html += "           <td>" + data.CreatedBy + "</td>";
                html += "       </tr>";
                html += "       <tr>";
                html += "           <td style=\"width:25%;\">Ngày tạo</td>";
                html += "           <td>" + date + "</td>";
                html += "       </tr>";
                html += "   </thead>";
                html += "</table>";
                html += "</div>";

                var htmlDiscount = "";
                if (dataDiscout.length > 0) {
                    htmlDiscount += "<div class=\"responsive-table\">";
                    htmlDiscount += "<table class=\"table table-checkable table-product\">";
                    htmlDiscount += "       <tr>";
                    htmlDiscount += "           <td><strong>Tên nhóm</strong></td>";
                    htmlDiscount += "           <td><strong>Chiết khấu</strong></td>";
                    htmlDiscount += "       </tr>";
                    for (var i = 0; i < dataDiscout.length; i++) {
                        htmlDiscount += "       <tr>";
                        htmlDiscount += "           <td>" + dataDiscout[i].DiscountName + "</td>";
                        htmlDiscount += "           <td>" + formatThousands(dataDiscout[i].DiscountAmount, ",") + " vnđ/sản phẩm</td>";
                        htmlDiscount += "       </tr>";
                    }
                    htmlDiscount += "</table>";
                    htmlDiscount += "</div>";
                }

                showPopup(html + htmlDiscount);
            }
        },
        error: function (xmlhttprequest, textstatus, errorthrow) {
            //alert('lỗi 1');
        }
    });
}

// search Customer by name, nick, phone, zalo, facebook
function searchCustomer() {
    var html = "";
    html += "<div class=\"form-row\">";
    html += "<label>Tìm khách hàng: </label>";
    html += "<input id=\"txtSearchCustomer\" class=\"form-control fjx\"></input>";
    html += "<a href=\"javascript:;\" class=\"btn primary-btn float-right-btn link-btn\" onclick=\"showCustomerList()\"><i class=\"fa fa-search\" aria-hidden=\"true\"></i> Tìm</a>";
    html += "<div class=\"customer-list hide\">";
    html += "</div>";
    html += "</div>";
    showPopup(html, 9);
    $("#txtSearchCustomer").focus();
    $('#txtSearchCustomer').keydown(function (event) {
        if (event.which === 13) {
            showCustomerList();
            event.preventDefault();
            return false;
        }
    });
}

// show customer list after search
function showCustomerList() {
    var textsearch = $("#txtSearchCustomer").val();
    var usernamecurrent = $("input[id$='_hdfUsernameCurrent']").val();
    if (!isBlank(textsearch)) {
        $.ajax({
            type: "POST",
            url: "/pos.aspx/searchCustomerByText",
            data: "{textsearch:'" + textsearch + "', createdby:'" + usernamecurrent + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var count = 0;
                var data = JSON.parse(msg.d);
                if (data === null) {
                    swal({
                        title: "Thông báo",
                        text: 'Không tìm thấy khách hàng !!',
                        type: 'error',
                        showCancelButton: true,
                        closeOnConfirm: true,
                        cancelButtonText: "Để em thêm khách mới..",
                        confirmButtonText: "Để em tìm lại..",
                        html: true,
                    }, function (confirm) {
                        if (confirm) {
                            $("#txtSearchCustomer").focus();
                        }
                        else {
                            $("input[id$='_txtFullname']").focus();
                            closePopup();
                        }
                    });
                }
                else {
                    var html = "";
                    if (data.employee === 1 && $("input[id$='_notAcceptChangeUser']").val() === "1") {
                        return swal({
                            title: "Thông báo",
                            text: 'Không tìm thấy khách hàng !!',
                            type: 'error',
                            showCancelButton: true,
                            closeOnConfirm: true,
                            cancelButtonText: "Để em thêm khách mới..",
                            confirmButtonText: "Để em tìm lại..",
                            html: true,
                        }, function (confirm) {
                            if (confirm) {
                                $("#txtSearchCustomer").focus();
                            }
                            else {
                                $("input[id$='_txtFullname']").focus();
                                closePopup();
                            }
                        });
                    }
                    if (data.employee === 1) {
                        swal("Thông báo", "Không tìm thấy khách hàng của bạn. Nhưng tìm thấy khách tương tự của nhân viên khác...", "info");
                    }
                    html = "";
                    html += ("<div class=\"margin-top-15\">");
                    html += ("<table class=\"table table-checkable table-product table-list-customer\">");
                    html += ("<thead>");
                    html += ("<tr>");
                    html += ("<th class=\"select-column\">Chọn</th>");
                    html += ("<th class=\"nick-column\">Nick đặt hàng</th>");
                    html += ("<th class=\"name-column\">Họ tên</th>");
                    html += ("<th class=\"phone-column\">Điện thoại</th>");
                    html += ("<th class=\"zalo-column\">Zalo</th>");
                    html += ("<th class=\"facebook-column\">Facebook</th>");
                    html += ("<th class=\"province-column\">Nhân viên</th>");
                    html += ("<th class=\"address-column\">Địa chỉ</th>");
                    html += ("</tr>");
                    html += ("</thead>");
                    html += ("</table>");
                    html += ("</div>");
                    html += ("<div class=\"form-row list-customer scrollbar\">");
                    html += ("<table class=\"table table-checkable table-product table-list-customer\" id=\"tableCustomer\">");
                    html += ("<tbody>");

                    i = 0;
                    data.listCustomer.forEach(function (item) {
                        html += ("<tr tabindex=\"" + i + "\" data-id=\"" + item.ID + "\">");

                        html += ("<td class=\"select-column\"><a class=\"btn primary-btn link-btn\" href=\"javascript:;\"><i class=\"fa fa-check-square-o\" aria-hidden=\"true\"></i></a></td>");
                        if (!isBlank(item.Nick)) {
                            html += ("<td class=\"nick nick-column\">" + item.Nick + "</td>");
                        } else {
                            html += ("<td class=\"nick nick-column\"></td>");
                        }
                        html += ("<td class=\"name name-column\">" + item.CustomerName + "</td>");
                        html += ("<td class=\"phone phone-column\">" + item.CustomerPhone + "</td>");

                        if (!isBlank(item.Zalo)) {
                            html += ("<td class=\"zalo zalo-column\">" + item.Zalo + "</td>");
                        } else {
                            html += ("<td class=\"zalo zalo-column\"></td>");
                        }

                        if (!isBlank(item.Facebook)) {
                            html += ("<td class=\"facebook\" data-value=\"" + item.Facebook + "\"><a class=\"link\" href=\"" + item.Facebook + "\" target=\"_blank\">Xem</a></td>");
                        } else {
                            html += ("<td class=\"facebook\" data-value=\"null\"></td>");
                        }
                        
                        html += ("<td class=\"createdby province-column\">" + item.CreatedBy + "</td>");
                        html += ("<td class=\"address address-column\">" + item.CustomerAddress + "</td>");
                        html += ("</tr>");
                        i++;
                    });

                    html += ("</tbody>");
                    html += ("</table>");
                    html += ("</div>");
                    $("#txtSearchCustomer").val("");
                    $(".customer-list").html(html);
                    $(".customer-list").removeClass('hide');

                    selectCustomer();
                }
            },
            error: function (xmlhttprequest, textstatus, errorthrow) {
                alert('lỗi');
            }
        });
    } else {
        $("#txtSearchCustomer").focus();
        swal("Thông báo", "Hãy nhập nội dung tìm kiếm", "error");
    }
}

function checkKey(e) {
    var event = window.event ? window.event : e;
    if (event.keyCode == 40) { //down
        var idx = $("tr:focus").attr("tabindex");
        idx++;
        $("tr[tabindex=" + idx + "]").focus();
    }
    if (event.keyCode == 38) { //up
        var idx = $("tr:focus").attr("tabindex");
        idx--;
        $("tr[tabindex=" + idx + "]").focus();
    }
    if (event.keyCode == 13) { //enter
        $("tr:focus").click();
    }
}

function selectCustomer() {
    $("#tableCustomer tr[tabindex=0]").focus();
    document.onkeydown = checkKey;

    $("#tableCustomer tr").on('click', function (e) {
        var phone = $(this).closest('tr').find("td.phone").html();
        var name = $(this).closest('tr').find("td.name").html();
        var nick = $(this).closest('tr').find("td.nick").html();
        var address = $(this).closest('tr').find("td.address").html();
        var zalo = $(this).closest('tr').find("td.zalo").html();
        var facebook = $(this).closest('tr').find("td.facebook").attr("data-value");
        var id = $(this).closest('tr').attr("data-id");
        var createdby = $(this).closest('tr').find("td.createdby").html();
        var username = $("input[id$='_hdfUsername']").val();

        

        if (createdby !== username) {
            swal({
                title: 'Lưu ý',
                text: 'Chọn khách hàng này đồng nghĩa em đang tính tiền giúp <strong>' + createdby + '</strong>.<br><br>Đồng ý không???',
                type: 'warning',
                showCancelButton: true,
                closeOnConfirm: false,
                cancelButtonText: "Để em suy nghỉ lại!",
                confirmButtonText: "OK sếp ơi..",
                html: true,
            }, function (confirm) {
                if (confirm) {
                    $("input[id$='_txtPhone']").val(phone).prop('readonly', true);
                    $("input[id$='_txtFullname']").val(name);
                    $("input[id$='_txtNick']").val(nick);
                    $("input[id$='_txtAddress']").val(address);
                    $("input[id$='_txtZalo']").val(zalo);
                    $("input[id$='_txtFacebook']").parent().removeClass("width-100");

                    if (facebook === "null") {
                        $("input[id$='_txtFacebook']").val("");
                        $(".link-facebook").hide();
                        $("input[id$='_txtFacebook']").parent().addClass("width-100");
                    }
                    else {
                        $("input[id$='_txtFacebook']").val(facebook);
                        $("input[id$='_txtFacebook']").parent().removeClass("width-100");
                        $(".link-facebook").html("<a href=\"" + facebook + "\" class=\"btn primary-btn fw-btn not-fullwidth\" target=\"_blank\">Xem</a>").show();
                    }

                    var button = "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth\" onclick=\"viewCustomerDetail('" + id + "')\"><i class=\"fa fa-address-card-o\" aria-hidden=\"true\"></i> Xem</a>";
                    button += "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth clear-btn\" onclick=\"clearCustomerDetail()\"><i class=\"fa fa-times\" aria-hidden=\"true\"></i> Bỏ</a>";
                    $(".view-detail").html(button).show();

                    _clearCustomerAddress();
                    _getCustomerAddress(phone);

                    getCustomerDiscount(id);

                    if (typeof removeCoupon === 'function') {
                        removeCoupon();
                    }

                    closePopup();

                    if (typeof checkCustomer === 'function') {
                        checkCustomer();
                    }
                    $("input[id$='_hdfCustomerID']").val(id);
                    $("input[id$='_hdfUsernameCurrent']").val(createdby);

                    if (typeof getDeliveryAddressDefault(phone) === 'function')
                        getDeliveryAddressDefault(phone);

                    swal.close();
                }
                else {
                    $("#txtSearchCustomer").focus();
                    swal.close();
                }
            });
        }
        else {
            $("input[id$='_txtPhone']").val(phone).prop('readonly', true);
            $("input[id$='_txtFullname']").val(name);
            $("input[id$='_txtNick']").val(nick);
            $("input[id$='_txtAddress']").val(address);
            $("input[id$='_txtZalo']").val(zalo);
            $("input[id$='_txtFacebook']").parent().removeClass("width-100");

            if (facebook === "null") {
                $("input[id$='_txtFacebook']").val("");
                $(".link-facebook").hide();
                $("input[id$='_txtFacebook']").parent().addClass("width-100");
            }
            else {
                $("input[id$='_txtFacebook']").val(facebook);
                $("input[id$='_txtFacebook']").parent().removeClass("width-100");
                $(".link-facebook").html("<a href=\"" + facebook + "\" class=\"btn primary-btn fw-btn not-fullwidth\" target=\"_blank\">Xem</a>").show();
            }

            var button = "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth\" onclick=\"viewCustomerDetail('" + id + "')\"><i class=\"fa fa-address-card-o\" aria-hidden=\"true\"></i> Xem</a>";
            button += "<a href=\"chi-tiet-khach-hang?id=" + id + "\" class=\"btn primary-btn fw-btn not-fullwidth edit-customer-btn\" target=\"_blank\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Sửa</a>";
            button += "<a href=\"danh-sach-don-hang?searchtype=1&textsearch=" + phone + "\" class=\"btn primary-btn fw-btn not-fullwidth edit-customer-btn\" target=\"_blank\"><i class=\"fa fa-history\" aria-hidden=\"true\"></i> Lịch sử</a>";
            button += "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth clear-btn\" onclick=\"clearCustomerDetail()\"><i class=\"fa fa-times\" aria-hidden=\"true\"></i> Bỏ</a>";
            $(".view-detail").html(button).show();

            _clearCustomerAddress();
            _getCustomerAddress(phone);

            getCustomerDiscount(id);

            if (typeof removeCoupon === 'function') {
                removeCoupon();
            }
            
            closePopup();

            $("input[id$='_hdfCustomerID']").val(id);
            $("input[id$='_hdfUsernameCurrent']").val(createdby);

            if (typeof getDeliveryAddressDefault(phone) === 'function')
                getDeliveryAddressDefault(phone);

            swal.close();

            // Trường hợp là them-moi-don-hang page 
            // Kiểm tra xem có đơn hàng cũ đang xử lý và đơn đổi tra hàng nào chưa trừ tiền không
            checkOrderOld(id);

            $("#txtSearch").focus();
        }


        // xử lý riêng màn hình tao-don-hang-doi-tra
        var page = window.location.pathname;
        if (page === "/tao-don-hang-doi-tra" || page === "/xem-don-hang-doi-tra") {
            $("input[id$='_txtFullname']").prop('readonly', false);
            $("input[id$='_txtAddress']").prop('readonly', false);
            $("input[id$='_txtNick']").prop('readonly', false);
            $("input[id$='_txtFacebook']").prop('readonly', false);
            $("select[id$='_ddlProvince']").attr('disabled', false);
            $("select[id$='_ddlProvince']").attr('readonly', false);
        }

    });
}

function checkOrderOld(customerID) {
    $.ajax({
        url: "/them-moi-don-hang.aspx/checkOrderOld",
        type: "POST",
        data: JSON.stringify({ 'customerID': customerID, 'status': 1 }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            let data = JSON.parse(response.d);
            if (data) {
                let openOrderDOM = $("#openOrder");
                let openOrderReturnDOM = $("#openOrderReturn");
                let show = 0;

                // Thông tin đơn hàng cũ chưa xử lý
                if (data.numberOrder) {
                    show = 1;
                    $("#warningTextOrder").removeClass("hide");
                    openOrderDOM.removeAttr('style');
                    openOrderDOM.attr('onClick', "window.open('/danh-sach-don-hang?searchtype=1&textsearch=" + data.phone + "&excutestatus=1', '_blank')")
                }
                else {
                    $("#warningTextOrder").addClass("hide");
                    openOrderDOM.removeAttr('onClick');
                    openOrderDOM.attr('style', 'display: none');
                }

                // Thông tin đơn hàng đổ trả chưa trừ tiền
                if (data.numberOrderReturn) {
                    show = 1;
                    $("#warningTextOrderReturn").removeClass("hide");
                    openOrderReturnDOM.removeAttr('style');
                    openOrderReturnDOM.attr('onClick', "window.open('/danh-sach-don-tra-hang?&textsearch=" + data.phone + "&status=1', '_blank')")
                }
                else {
                    $("#warningTextOrderReturn").addClass("hide");
                    openOrderReturnDOM.removeAttr('onClick');
                    openOrderReturnDOM.attr('style', 'display: none');
                }

                // Show thông báo
                if (show == 1) {
                    $("#orderOldModal").modal({ show: 'true', backdrop: 'static', keyboard: 'false' });
                }

            }
        },
        error: function (err) {
            swal("Thông báo", "Đã có vần đề trong việc check đơn hàng cũ", "error");
        }
    });
}

function refreshCustomerInfo(ID) {
    if (!isBlank(ID)) {
        $.ajax({
            type: "POST",
            url: "/pos.aspx/getCustomerDetail",
            data: "{ID:'" + ID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if (msg.d !== "null") {
                    var alldata = JSON.parse(msg.d);
                    var data = alldata.Customer;
                    var phone = data.CustomerPhone;
                    var name = data.CustomerName;
                    var nick = data.Nick;
                    var address = data.CustomerAddress;
                    var zalo = data.Zalo;
                    var facebook = data.Facebook;
                    var id = data.ID;
                    $("input[id$='_txtPhone']").val(phone).prop('readonly', true);
                    $("input[id$='_txtFullname']").val(name);
                    $("input[id$='_txtNick']").val(nick);
                    $("input[id$='_txtAddress']").val(address);
                    $("input[id$='_txtZalo']").val(zalo);
                    $("input[id$='_txtFacebook']").parent().removeClass("width-100");
                    $("input[id$='_txtFacebook']").val(facebook);
                    if (facebook === null) {
                        $(".link-facebook").hide();
                        $("input[id$='_txtFacebook']").parent().addClass("width-100");
                    }
                    else {
                        $("input[id$='_txtFacebook']").parent().removeClass("width-100");
                        $(".link-facebook").html("<a href=\"" + facebook + "\" class=\"btn primary-btn fw-btn not-fullwidth\" target=\"_blank\">Xem</a>").show();
                    }
                    swal("Thông báo", "Đã làm mới thông tin khách hàng!", "success");
                } else {
                    swal("Thông báo", "Không tìm thấy khách hàng", "error");
                }

                _getCustomerAddress(phone);

                getCustomerDiscount(id);
            },
            error: function (xmlhttprequest, textstatus, errorthrow) {
                alert('lỗi');
            }
        });
    } else {
        $("#txtSearchCustomer").focus();
        swal("Thông báo", "Hãy nhập nội dung tìm kiếm", "error");
    }
}

// clear customer detail
function clearCustomerDetail() {

    $("input[id$='_txtPhone']").val("").prop('readonly', false).prop('disabled', false);
    $("input[id$='_txtFullname']").val("");
    $("input[id$='_txtNick']").val("");
    $("input[id$='_txtAddress']").val("");
    $("input[id$='_txtZalo']").val("");
    $("input[id$='_txtFacebook']").val("");
    $(".view-detail").html("").hide();
    $(".discount-info").html("").hide();
    $(".refund-info").html("").hide();
    $(".link-facebook").html("").hide();
    $("input[id$='_txtFacebook']").parent().addClass("width-100");
    $("input[id$='_txtFullname']").focus();
    $("input[id$='_hdfUsernameCurrent']").val($("input[id$='_hdfUsername']").val());
    // Fix bug when clearing in pos
    $("input[id$='_hdfIsDiscount']").val(0);
    $("input[id$='_hdfDiscountAmount']").val(0);
    $("input[id$='_hdfCustomerFeeChange']").val(0);

    $("input[id$='_hdfCustomerID']").val("");

    if (typeof removeCoupon === 'function') {
        removeCoupon();
    }

    _clearCustomerAddress();

    getAllPrice();
}

function selectCustomerDetail(data) {
    $("input[id$='_txtPhone']").val(data.CustomerPhone).prop('readonly', true);
    $("input[id$='_txtFullname']").val(data.CustomerName);
    $("input[id$='_txtNick']").val(data.Nick);
    $("input[id$='_txtAddress']").val(data.CustomerAddress);
    $("input[id$='_txtZalo']").val(data.Zalo);
    $("input[id$='_txtFacebook']").parent().removeClass("width-100");
    $("input[id$='_txtFacebook']").val(data.Facebook);
    if (!data.Facebook) {
        $(".link-facebook").hide();
        $("input[id$='_txtFacebook']").parent().addClass("width-100");
    }
    else {
        $("input[id$='_txtFacebook']").parent().removeClass("width-100");
        $(".link-facebook").html("<a href=\"" + data.Facebook + "\" class=\"btn primary-btn fw-btn not-fullwidth\" target=\"_blank\">Xem</a>").show();
    }

    var button = "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth\" onclick=\"viewCustomerDetail('" + data.ID + "')\"><i class=\"fa fa-address-card-o\" aria-hidden=\"true\"></i> Xem</a>";

    var createdby = data.CreatedBy;
    var username = $("input[id$='_hdfUsername']").val();

    if (createdby === username) {
        button += "<a href=\"chi-tiet-khach-hang?id=" + data.ID + "\" class=\"btn primary-btn fw-btn not-fullwidth edit-customer-btn\" target=\"_blank\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Sửa</a>";
        button += "<a href=\"danh-sach-don-hang?searchtype=1&textsearch=" + data.CustomerPhone + "\" class=\"btn primary-btn fw-btn not-fullwidth edit-customer-btn\" target=\"_blank\"><i class=\"fa fa-history\" aria-hidden=\"true\"></i> Lịch sử</a>";
    }
    
    button += "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth clear-btn\" onclick=\"clearCustomerDetail()\"><i class=\"fa fa-times\" aria-hidden=\"true\"></i> Bỏ</a>";
    $(".view-detail").html(button).show();

    _getCustomerAddress(data.CustomerPhone);

    getCustomerDiscount(data.ID);

    if (typeof removeCoupon === 'function') {
        removeCoupon();
    }

    closePopup();
    if (typeof checkCustomer === 'function') {
        checkCustomer();
    }
}

function ajaxCheckCustomer() {
    var txtPhone = $("input[id$='_txtPhone']");
    var phone = txtPhone.val();
    if (phone != "" && !txtPhone.is('[readonly]')) {
        $.ajax({
            type: "POST",
            url: "/pos.aspx/searchCustomerByPhone",
            data: "{phone:'" + phone + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var data = JSON.parse(msg.d);
                var username = $("input[id$='_hdfUsername']").val();
                if (data !== null) {
                    if (username !== data.CreatedBy) {
                        swal({
                            title: "Tin nửa vui nửa buồn!!",
                            text: "Số điện thoại này của khách hàng <strong>" + data.CustomerName + "</strong>. Khách này do <strong>" + data.CreatedBy + "</strong> phụ trách.<br><br>Em có lấy thông tin khách này tính tiền không?",
                            type: "info",
                            showCancelButton: true,
                            closeOnConfirm: false,
                            closeOnCancel: true,
                            cancelButtonText: "Để em kiểm tra lại..",
                            confirmButtonText: "Lấy luôn sếp ơi..",
                            html: true,
                        }, function (isconfirm) {
                            if (isconfirm) {
                                if ($("input[id$='_notAcceptChangeUser']").val() === "1") {
                                    swal({
                                        title: "Giởn thôi...",
                                        text: "Làm vậy không ổn đâu cưng à!!<br><br>Bắt buộc bàn giao đơn hàng này cho <strong>" + data.CreatedBy + "</strong> thôi!<br><br>Hihi!",
                                        type: "info",
                                        confirmButtonText: "Hihi OK sếp..",
                                        html: true,
                                    }, function () {
                                        $("input[id$='_hdfCustomerID']").val("");
                                        $("input[id$='_txtPhone']").val("");
                                    });
                                }
                                else {
                                    swal({
                                        title: "Nếu như vậy thì...",
                                        text: "Đơn hàng này sẽ được cưng tính tiền giúp cho <strong>" + data.CreatedBy + "</strong>.<br><br>Cưng đồng ý không?",
                                        type: "info",
                                        showCancelButton: true,
                                        closeOnConfirm: true,
                                        cancelButtonText: "Để em suy nghỉ lại..",
                                        confirmButtonText: "OK sếp ơi..",
                                        html: true,
                                    }, function (confirm) {
                                        if (confirm) {
                                            $("input[id$='_hdfCustomerID']").val(data.ID);
                                            $("input[id$='_hdfUsernameCurrent']").val(data.CreatedBy);
                                            selectCustomerDetail(data);

                                            if (typeof getDeliveryAddressDefault(phone) === 'function')
                                                getDeliveryAddressDefault(phone);

                                            $("#txtSearch").focus();
                                        }
                                        else {
                                            $("input[id$='_hdfCustomerID']").val("");
                                            $("input[id$='_txtPhone']").val("");
                                        }
                                    });
                                }
                            }
                            else {
                                $("input[id$='_hdfCustomerID']").val("");
                                $("input[id$='_txtPhone']").val("");
                            }
                        });
                    }
                    else {
                        swal({
                            title: "Cười lên nào!!",
                            text: "Số điện thoại này của khách hàng <strong>" + data.CustomerName + "</strong>.<br><br>Em có lấy thông tin khách này tính tiền không?",
                            type: "info",
                            showCancelButton: true,
                            closeOnConfirm: true,
                            cancelButtonText: "Để em kiểm tra lại..",
                            confirmButtonText: "Lấy luôn sếp ơi..",
                            html: true,
                        }, function (confirm) {
                            if (confirm) {
                                $("input[id$='_hdfCustomerID']").val(data.ID);
                                selectCustomerDetail(data);

                                if (typeof getDeliveryAddressDefault(phone) === 'function')
                                    getDeliveryAddressDefault(phone);

                                $("#txtSearch").focus();
                            }
                            else {
                                $("input[id$='_hdfCustomerID']").val("");
                                $("input[id$='_txtPhone']").val("");
                            }
                        });
                    }

                }
            }
        });

    }
    
}


// Customer Address
function _initReceiverAddress() {
    
    $("select[id$='_ddlProvince']").val(null).trigger('change');
    // Danh sách tỉnh / thành phố
    $("select[id$='_ddlProvince']").select2({
        width: "100%",
        placeholder: 'Chọn tỉnh thành',
        ajax: {
            delay: 500,
            method: 'GET',
            url: '/api/v1/delivery-save/provinces/select2',
            data: (params) => {
                var query = {
                    search: params.term,
                    page: params.page || 1
                }

                return query;
            }
        }
    });

    // Danh sách quận / huyện
    _disabledDDLDistrict(true, 0);

    // Danh sách phường / xã
    _disabledDDLWard(true, 0);
}

function _onChangeReceiverAddress() {
    // // Danh sách tỉnh / thành phố
    $("select[id$='_ddlProvince']").on('select2:select', (e) => {
        let data = e.params.data;
        $("input[id$='_hdfProvinceID']").val(data.id);
        $("input[id$='_hdfDistrictID']").val(0);
        $("input[id$='_hdfWardID']").val(0);
        _disabledDDLDistrict(false, data.id);
        $("select[id$='_ddlDistrict']").select2('open');

        _disabledDDLWard(true, data.id);
    });

    // Danh sách quận / huyện
    $("select[id$='_ddlDistrict']").on('select2:select', (e) => {
        let data = e.params.data;

        $("input[id$='_hdfDistrictID']").val(data.id);
        $("input[id$='_hdfWardID']").val(0);
        _disabledDDLWard(false, data.id);
        $("select[id$='_ddlWard']").select2('open');
    });

    // Danh sách phường / xã
    $("select[id$='_ddlWard']").on('select2:select', (e) => {
        let data = e.params.data;

        $("input[id$='_hdfWardID']").val(data.id);
    });
}

function _disabledDDLDistrict(disabled, provinceID) {
    if (disabled) {
        $("select[id$='_ddlDistrict']").attr('disabled', true);
        $("select[id$='_ddlDistrict']").attr('readonly', 'readonly');
        $("select[id$='_ddlDistrict']").val(null).trigger('change');
        $("select[id$='_ddlDistrict']").select2({
            width: "100%",
            placeholder: 'Chọn quận huyện'
        });
    }
    else {
        $("select[id$='_ddlDistrict']").removeAttr('disabled');
        $("select[id$='_ddlDistrict']").removeAttr('readonly');
        $("select[id$='_ddlDistrict']").val(null).trigger('change');
        $("select[id$='_ddlDistrict']").select2({
            width: "100%",
            placeholder: 'Chọn quận huyện',
            ajax: {
                delay: 500,
                method: 'GET',
                url: '/api/v1/delivery-save/province/' + provinceID + '/districts/select2',
                data: (params) => {
                    var query = {
                        search: params.term,
                        page: params.page || 1
                    }

                    return query;
                }
            }
        });
    }
}

function _disabledDDLWard(disabled, districtID) {
    if (disabled) {
        $("select[id$='_ddlWard']").attr('disabled', true);
        $("select[id$='_ddlWard']").attr('readonly', 'readonly');
        $("select[id$='_ddlWard']").val(null).trigger('change');
        $("select[id$='_ddlWard']").select2({
            width: "100%",
            placeholder: 'Chọn phường xã'
        });
    }
    else {
        $("select[id$='_ddlWard']").removeAttr('disabled');
        $("select[id$='_ddlWard']").removeAttr('readonly');
        $("select[id$='_ddlWard']").val(null).trigger('change');
        $("select[id$='_ddlWard']").select2({
            width: "100%",
            placeholder: 'Chọn phường xã',
            ajax: {
                delay: 500,
                method: 'GET',
                url: '/api/v1/delivery-save/district/' + districtID + '/wards/select2',
                data: (params) => {
                    var query = {
                        search: params.term,
                        page: params.page || 1
                    }

                    return query;
                }
            }
        });
    }
}

function _getCustomerAddress(customerPhone) {
    $.ajax({
        method: 'GET',
        url: "/api/v1/customer/" + customerPhone + "/address",
        success: (data, textStatus, xhr) => {
            // province
            if (data.provinceID) {
                let newOption = new Option(data.provinceName, data.provinceID, false, false);
                $("select[id$='_ddlProvince']").find("option").remove();
                $("select[id$='_ddlProvince']").append(newOption).trigger('change');

                $("input[id$='_hdfProvinceID']").val(data.provinceID);
                // Danh sách quận / huyện
                _disabledDDLDistrict(false, data.provinceID);
            }
            if (data.provinceID && data.districtID) {
                let newOption = new Option(data.districtName, data.districtID, false, false);
                $("select[id$='_ddlDistrict']").removeAttr('disabled');
                $("select[id$='_ddlDistrict']").removeAttr('readonly');
                $("select[id$='_ddlDistrict']").find("option").remove();
                $("select[id$='_ddlDistrict']").append(newOption).trigger('change');

                $("input[id$='_hdfProvinceID']").val(data.provinceID);
                $("input[id$='_hdfDistrictID']").val(data.districtID);
                // Danh sách phường / xã
                _disabledDDLWard(false, data.districtID);
            }
            if (data.provinceID && data.districtID && data.wardID) {
                let newOption = new Option(data.wardName, data.wardID, false, false);
                $("select[id$='_ddlWard']").removeAttr('disabled');
                $("select[id$='_ddlWard']").removeAttr('readonly');
                $("select[id$='_ddlWard']").find("option").remove();
                $("select[id$='_ddlWard']").append(newOption).trigger('change');

                $("input[id$='_hdfProvinceID']").val(data.provinceID);
                $("input[id$='_hdfDistrictID']").val(data.districtID);
                $("input[id$='_hdfWardID']").val(data.wardID);
            }
        },
        error: (xhr, textStatus, error) => {
        }
    });
}

function _clearCustomerAddress() {
    $("input[id$='_hdfProvinceID']").val(0);
    $("input[id$='_hdfDistrictID']").val(0);
    $("input[id$='_hdfWardID']").val(0);

    $("select[id$='_ddlProvince']").find("option").remove();
    $("select[id$='_ddlDistrict']").find("option").remove();
    $("select[id$='_ddlWard']").find("option").remove();
    
    _initReceiverAddress();
}