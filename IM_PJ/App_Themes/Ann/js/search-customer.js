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

                $(".discount-info").html("<strong>* Chiết khấu của khách: " + formatThousands(data.Discount, ",") + "đ/cái.</strong>").show();

                if (data.FeeRefund == "0") {
                    $(".refund-info").html("<strong>* Miễn phí đổi hàng</strong>").show();
                }
                else {
                    $(".refund-info").html("<strong>* Phí đổi hàng của khách: " + formatThousands(data.FeeRefund, ",") + "đ/cái.</strong>").show();
                }
                
                $("input[id$='_hdfIsDiscount']").val("1");
                $("input[id$='_hdfDiscountAmount']").val(data.Discount);
                $("input[id$='_hdfCustomerFeeChange']").val(data.FeeRefund);

            }
            else
            {
                $("input[id$='_hdfIsDiscount']").val(0);
                $("input[id$='_hdfDiscountAmount']").val(0);
                $("input[id$='_hdfCustomerFeeChange']").val(0);
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
                html += "       <tr>";
                html += "           <td style=\"width:25%;\">Nick đặt hàng:</td>";
                html += "           <td class=\"capitalize\">" + data.Nick + "</td>";
                html += "       </tr>";
                html += "       <tr>";
                html += "           <td style=\"width:25%;\">Điện thoại</td>";
                html += "           <td>" + data.CustomerPhone + "</td>";
                html += "       </tr>";
                html += "       <tr>";
                html += "           <td style=\"width:25%;\">Địa chỉ</td>";
                html += "           <td class=\"capitalize\">" + data.CustomerAddress + "</td>";
                html += "       </tr>";
                html += "       <tr>";
                html += "           <td style=\"width:25%;\">Email</td>";
                html += "           <td>" + data.CustomerEmail + "</td>";
                html += "       </tr>";
                html += "       <tr>";
                html += "           <td style=\"width:25%;\">Zalo</td>";
                html += "           <td>" + data.Zalo + "</td>";
                html += "       </tr>";
                html += "       <tr>";
                html += "           <td style=\"width:25%;\">Facebook</td>";
                html += "           <td>" + data.Facebook + "</td>";
                html += "       </tr>";
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
                } else {
                    htmlDiscount += "<div class=\"responsive-table\">";
                    htmlDiscount += "<table class=\"table table-checkable table-product\">";
                    htmlDiscount += "       <tr>";
                    htmlDiscount += "           <td><strong>Hiện tại khách hàng chưa được chiết khấu</strong></td>";
                    htmlDiscount += "       </tr>";
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
    var username = $("input[id$='_hdfUsernameCurrent']").val();
    if (!isBlank(textsearch)) {
        $.ajax({
            type: "POST",
            url: "/pos.aspx/searchCustomerByText",
            data: "{textsearch:'" + textsearch + "', createdby:'" + username + "'}",
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
                        swal("Thông báo", "Không tìm thấy khách hàng của bạn. Nhưng tìm thấy khách tương tự của nhân viên khác...", "error");
                    }
                    html = "";
                    html += ("<div class=\"margin-top-15\">");
                    html += ("<table class=\"table table-checkable table-product table-list-customer\">");
                    html += ("<thead>");
                    html += ("<tr>");
                    html += ("<th class=\"select-column\"></th>");
                    html += ("<th class=\"nick-column\">Nick đặt hàng</th>");
                    html += ("<th class=\"name-column\">Họ tên</th>");
                    html += ("<th class=\"phone-column\">Điện thoại</th>");
                    html += ("<th class=\"zalo-column\">Zalo</th>");
                    html += ("<th class=\"facebook-column\">Facebook</th>");
                    html += ("<th class=\"province-column\">Nhân viên</th>");
                    html += ("<th class=\"address-column\">Địa chỉ</th>");
                    html += ("<th class=\"province-column\">Tỉnh</th>");
                    html += ("</tr>");
                    html += ("</thead>");
                    html += ("</table>");
                    html += ("</div>");
                    html += ("<div class=\"form-row list-customer scrollbar\">");
                    html += ("<table class=\"table table-checkable table-product table-list-customer\" id=\"tableCustomer\">");
                    html += ("<tbody>");

                    data.listCustomer.forEach(function (item) {
                        html += ("<tr>");

                        html += ("<td class=\"select-column\"><a class=\"btn primary-btn link-btn\" href=\"javascript:;\"><i class=\"fa fa-check-square-o\" aria-hidden=\"true\"></i></a></td>");
                        if (!isBlank(item.Nick)) {
                            html += ("<td class=\"nick nick-column\">" + item.Nick + "</td>");
                        } else {
                            html += ("<td class=\"nick nick-column\"></td>");
                        }
                        html += ("<td class=\"name name-column\">" + item.CustomerName + "</td>");
                        html += ("<td class=\"phone phone-column\">" + item.CustomerPhone + "</td>");
                        html += ("<td class=\"id\" style=\"display:none\">" + item.ID + "</td>");
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

                        if (!isBlank(item.CreatedBy)) {
                            html += ("<td class=\"createdby province-column\">" + item.CreatedBy + "</td>");
                        } else {
                            html += ("<td class=\"createdby province-column\"></td>");
                        }

                        if (!isBlank(item.CustomerAddress)) {
                            html += ("<td class=\"address address-column\">" + item.CustomerAddress + "</td>");
                        } else {
                            html += ("<td class=\"address address-column\"></td>");
                        }

                        if (!isBlank(item.Province)) {
                            html += ("<td class=\"province province-column\">" + item.Province + "</td>");
                        } else {
                            html += ("<td class=\"province province-column\"></td>");
                        }
                        html += ("</tr>");
                    });

                    html += ("</tbody>");
                    html += ("</table>");
                    html += ("</div>");
                    $("#txtSearchCustomer").val("");
                    $(".customer-list").html(html);
                    $(".customer-list").removeClass('hide').addClass('show');
                    selectCustomer(username);
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
                    $("input[id$='_txtFullname']").val(name).prop('readonly', true);
                    $("input[id$='_txtNick']").val(nick).prop('readonly', true);
                    $("input[id$='_txtAddress']").val(address).prop('readonly', true);
                    $("input[id$='_txtZalo']").val(zalo).prop('readonly', true);
                    $("input[id$='_txtFacebook']").parent().removeClass("width-100");
                    $("input[id$='_txtFacebook']").val(facebook).prop('readonly', true);
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

function selectCustomer(username) {
    $("#tableCustomer tr td").on('click', function (e) {
        var phone = $(this).closest('tr').find("td.phone").html();
        var name = $(this).closest('tr').find("td.name").html();
        var nick = $(this).closest('tr').find("td.nick").html();
        var address = $(this).closest('tr').find("td.address").html();
        var zalo = $(this).closest('tr').find("td.zalo").html();
        var facebook = $(this).closest('tr').find("td.facebook").attr("data-value");
        var id = $(this).closest('tr').find("td.id").html();
        var createdby = $(this).closest('tr').find("td.createdby").html();
        if (createdby !== username) {
            swal({
                title: 'Lưu ý',
                text: 'Chọn khách hàng này đồng nghĩa em đang tính tiền giúp nhân viên <strong>' + createdby + '</strong>.<br><br>Đồng ý không???',
                type: 'warning',
                showCancelButton: true,
                closeOnConfirm: false,
                cancelButtonText: "Để em suy nghỉ lại!",
                confirmButtonText: "OK sếp ơi..",
                html: true,
            }, function (confirm) {
                if (confirm) {
                    $(".change-user").hide();
                    $("input[id$='_txtPhone']").val(phone).prop('readonly', true);
                    $("input[id$='_txtFullname']").val(name).prop('readonly', true);

                    if (nick != "") {
                        $("input[id$='_txtNick']").val(nick).prop('readonly', true);
                    }
                    else {
                        $("input[id$='_txtNick']").val("").prop('readonly', false);
                    }

                    if (address != "") {
                        $("input[id$='_txtAddress']").val(address).prop('readonly', true);
                    }
                    else {
                        $("input[id$='_txtAddress']").val("").prop('readonly', false);
                    }

                    if (zalo != "") {
                        $("input[id$='_txtZalo']").val(zalo).prop('readonly', true);
                    }
                    else {
                        $("input[id$='_txtZalo']").val("").prop('readonly', false);
                    }

                    $("input[id$='_txtFacebook']").parent().removeClass("width-100");
                    
                    if (facebook === "null") {
                        $("input[id$='_txtFacebook']").val("").prop('readonly', false);
                        $(".link-facebook").hide();
                        $("input[id$='_txtFacebook']").parent().addClass("width-100");
                    }
                    else {
                        $("input[id$='_txtFacebook']").val(facebook).prop('readonly', true);
                        $("input[id$='_txtFacebook']").parent().removeClass("width-100");
                        $(".link-facebook").html("<a href=\"" + facebook + "\" class=\"btn primary-btn fw-btn not-fullwidth\" target=\"_blank\">Xem</a>").show();
                    }

                    var button = "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth\" onclick=\"viewCustomerDetail('" + id + "')\"><i class=\"fa fa-address-card-o\" aria-hidden=\"true\"></i> Xem chi tiết</a>";
                    button += "<a href=\"chi-tiet-khach-hang?id=" + id + "\" class=\"btn primary-btn fw-btn not-fullwidth edit-customer-btn\" target=\"_blank\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Chỉnh sửa</a>";
                    button += "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth clear-btn\" onclick=\"clearCustomerDetail()\"><i class=\"fa fa-times\" aria-hidden=\"true\"></i> Bỏ qua</a>";
                    $(".view-detail").html(button).show();

                    getCustomerDiscount(id);

                    closePopup();
                    if (typeof checkCustomer === 'function') {
                        checkCustomer();
                    }
                    $("input[id$='_hdfUsernameCurrent']").val(createdby);
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
            $("input[id$='_txtFullname']").val(name).prop('readonly', true);

            if (nick != "") {
                $("input[id$='_txtNick']").val(nick).prop('readonly', true);
            }
            else {
                $("input[id$='_txtNick']").val("").prop('readonly', false);
            }

            if (address != "") {
                $("input[id$='_txtAddress']").val(address).prop('readonly', true);
            }
            else {
                $("input[id$='_txtAddress']").val("").prop('readonly', false);
            }

            if (zalo != "") {
                $("input[id$='_txtZalo']").val(zalo).prop('readonly', true);
            }
            else {
                $("input[id$='_txtZalo']").val("").prop('readonly', false);
            }

            $("input[id$='_txtFacebook']").parent().removeClass("width-100");
            
            if (facebook === "null") {
                $("input[id$='_txtFacebook']").val("").prop('readonly', false);
                $(".link-facebook").hide();
                $("input[id$='_txtFacebook']").parent().addClass("width-100");
            }
            else {
                $("input[id$='_txtFacebook']").val(facebook).prop('readonly', true);
                $("input[id$='_txtFacebook']").parent().removeClass("width-100");
                $(".link-facebook").html("<a href=\"" + facebook + "\" class=\"btn primary-btn fw-btn not-fullwidth\" target=\"_blank\">Xem</a>").show();
            }

            var button = "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth\" onclick=\"viewCustomerDetail('" + id + "')\"><i class=\"fa fa-address-card-o\" aria-hidden=\"true\"></i> Xem chi tiết</a>";
            button += "<a href=\"chi-tiet-khach-hang?id=" + id + "\" class=\"btn primary-btn fw-btn not-fullwidth edit-customer-btn\" target=\"_blank\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Chỉnh sửa</a>";
            button += "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth clear-btn\" onclick=\"clearCustomerDetail()\"><i class=\"fa fa-times\" aria-hidden=\"true\"></i> Bỏ qua</a>";
            $(".view-detail").html(button).show();

            getCustomerDiscount(id);

            closePopup();
            if (typeof checkCustomer === 'function') {
                checkCustomer();
            }

            $("input[id$='_hdfUsernameCurrent']").val(createdby);
            swal.close();
        }
    });
}

// clear customer detail
function clearCustomerDetail() {

    $("input[id$='_txtPhone']").val("").prop('readonly', false).prop('disabled', false);
    $("input[id$='_txtFullname']").val("").prop('readonly', false).prop('disabled', false);
    $("input[id$='_txtNick']").val("").prop('readonly', false).prop('disabled', false);
    $("input[id$='_txtAddress']").val("").prop('readonly', false).prop('disabled', false);
    $("input[id$='_txtZalo']").val("").prop('readonly', false).prop('disabled', false);
    $("input[id$='_txtFacebook']").val("").prop('readonly', false).prop('disabled', false);
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
    getAllPrice();
}

function selectCustomerDetail(data) {
    $("input[id$='_txtPhone']").val(data.CustomerPhone).prop('readonly', true);
    $("input[id$='_txtFullname']").val(data.CustomerName).prop('readonly', true);
    $("input[id$='_txtNick']").val(data.Nick).prop('readonly', true);
    $("input[id$='_txtAddress']").val(data.CustomerAddress).prop('readonly', true);
    $("input[id$='_txtZalo']").val(data.Zalo).prop('readonly', true);
    $("input[id$='_txtFacebook']").parent().removeClass("width-100");
    $("input[id$='_txtFacebook']").val(data.Facebook).prop('readonly', true);
    if (data.Facebook === null) {
        $(".link-facebook").hide();
        $("input[id$='_txtFacebook']").parent().addClass("width-100");
    }
    else {
        $("input[id$='_txtFacebook']").parent().removeClass("width-100");
        $(".link-facebook").html("<a href=\"" + data.Facebook + "\" class=\"btn primary-btn fw-btn not-fullwidth\" target=\"_blank\">Xem</a>").show();
    }

    var button = "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth\" onclick=\"viewCustomerDetail('" + data.ID + "')\"><i class=\"fa fa-address-card-o\" aria-hidden=\"true\"></i> Xem chi tiết</a>";
    button += "<a href=\"chi-tiet-khach-hang?id=" + data.ID + "\" class=\"btn primary-btn fw-btn not-fullwidth edit-customer-btn\" target=\"_blank\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Chỉnh sửa</a>";
    button += "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth clear-btn\" onclick=\"clearCustomerDetail()\"><i class=\"fa fa-times\" aria-hidden=\"true\"></i> Bỏ qua</a>";
    $(".view-detail").html(button).show();

    getCustomerDiscount(data.ID);

    closePopup();
    if (typeof checkCustomer === 'function') {
        checkCustomer();
    }
}

function ajaxCheckCustomer() {
    var phone = $("input[id$='_txtPhone']").val();
    if (phone != "") {
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
                            text: "Số điện thoại này đã được tạo cho khách hàng <strong>" + data.CustomerName + "</strong>. Nhưng khách này do <strong>" + data.CreatedBy + "</strong> phụ trách.<br><br>Cưng có lấy dữ liệu của khách này để tính tiền luôn không?",
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
                                        $("input[id$='_txtPhone']").val("");
                                    });
                                }
                                else {
                                    swal({
                                        title: "Nếu như vậy thì...",
                                        text: "Đơn hàng này sẽ được cưng tính tiền dùm cho <strong>" + data.CreatedBy + "</strong>.<br><br>Cưng đồng ý không?",
                                        type: "info",
                                        showCancelButton: true,
                                        closeOnConfirm: true,
                                        cancelButtonText: "Để em suy nghỉ lại..",
                                        confirmButtonText: "OK sếp ơi..",
                                        html: true,
                                    }, function (confirm) {
                                        if (confirm) {
                                            $("input[id$='_hdfUsernameCurrent']").val(data.CreatedBy);
                                            selectCustomerDetail(data);
                                        }
                                        else {
                                            $("input[id$='_txtPhone']").val("");
                                        }
                                    });
                                }
                            }
                            else {
                                $("input[id$='_txtPhone']").val("");
                                
                            }
                        });
                    }
                    else {
                        swal({
                            title: "Cười lên nào!!",
                            text: "Số điện thoại này đã được tạo cho khách hàng <strong>" + data.CustomerName + "</strong>.<br><br>Cưng có lấy dữ liệu của khách này để tính tiền không?",
                            type: "info",
                            showCancelButton: true,
                            closeOnConfirm: true,
                            cancelButtonText: "Để em kiểm tra lại..",
                            confirmButtonText: "Lấy luôn sếp ơi..",
                            html: true,
                        }, function (confirm) {
                            if (confirm) {
                                selectCustomerDetail(data);
                            }
                            else {
                                $("input[id$='_txtPhone']").val("");
                            }
                        });
                    }

                }
            }
        });

    }
    
}