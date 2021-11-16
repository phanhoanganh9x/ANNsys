const OrderStatusEnum = {
    "Done": 2,
}

const DeliveryMethodEnum = {
    "PostOffice": 2,
    "Proship": 3,
    "DeliverySave": 6,
    "JtExpress": 10
}
let strFormat = new StringFormat();
let controller = new GroupOrderManagerController();

document.addEventListener("DOMContentLoaded", function (event) {
    _initQueryParams();
    _initDeliveryMethods();
    _initGroupOrderTable();
});

//#region Private
function _initQueryParams() {
    let hdfStaffDom = document.querySelector("[id$='_hdfStaff']");
    let search = window.location.search;

    controller.staff = hdfStaffDom.value;
    controller.setFilterByQueryParameters(search);

}

function _initDeliveryMethods() {
    let $deliveryMethod = $("#ddlDeliveryMethod");

    // Cài đặt giá trị ban đầu
    $deliveryMethod .val(null).trigger('change');

    if (controller.filter.deliveryMethod)
        controller.getDeliveryMethod()
            .then(function (response) {
                let newOption = new Option(response.value, response.key, false, false);

                $deliveryMethod.find("option").remove();
                $deliveryMethod.append(newOption).trigger('change');
            })
            .catch(function (e) {
                console.log(e);
            });

    // Cài đặt API
    $deliveryMethod.select2({
        placeholder: 'Kiểu giao hàng',
        minimumResultsForSearch: Infinity,
        ajax: {
            method: 'GET',
            url: '/api/v1/delivery/methods/select2?orderTypeId=1&hasPlaceHolder=true',
        },
        width: '100%'
    });
}

function _loadSpanReport() {
    let spanReportDom = document.querySelector("[id$='spanReport']");
    let message = "";

    if (controller.pagination.totalCount > 0)
    {
        message += "(";
        message += UtilsService.formatThousands(controller.pagination.totalCount, ',');
        message += " đơn gộp";
        message += ")";
    }

    spanReportDom.innerHTML = message;
}

function _initGroupOrderTable() {
    HoldOn.open();

    controller.getGroupOrders()
        .then(function (response) {
            controller.lossMoney = response.lossMoney;
            controller.data = response.data
            controller.pagination = response.pagination;

            _loadSpanReport();
            _loadPagination();
            _loadDeliveryTable();

            HoldOn.close();
        })
        .catch(function (e) {
            console.log(e);
            HoldOn.close();
        });
}

function _updateFilter() {
    // Tìm kiếm theo mã đơn hàng hoặc mã vận đơn (Chỉ áp dụng với đơn hàng shop ANN)
    let searchDom = document.querySelector("[id$='_txtSearch']");

    searchDom.value = searchDom.value.trim();

    if (searchDom.value)
        controller.filter.search = searchDom.value;
    else
        controller.filter.search = null;

    // Tìm theo trạng thái giao hàng
    let statusDom = document.querySelector("[id$='_ddlStatus']");

    if (statusDom.value != "0")
        controller.filter.status = parseInt(statusDom.value);
    else
        controller.filter.status = null;

    // Tìm kiếm loại giao hàng
    let $deliveryMethod = $("#ddlDeliveryMethod");

    if ($deliveryMethod.val() != "0")
        controller.filter.deliveryMethod = parseInt($deliveryMethod.val());
    else
        controller.filter.deliveryMethod = null;

    // Khoảng thời gian
    let fromDateDom = document.querySelector("[id$='_dpFromDate']");
    let toDateDom = document.querySelector("[id$='_dpToDate']");

    if (fromDateDom.value) {
        let date = fromDateDom.value.substring(0,10);
        let time = fromDateDom.value.substring(11, 16).replace(/-/g, ':');
        let strDate = date + ' ' + time;

        controller.filter.fromDate = strFormat.datetimeToString(strDate, 'MM/dd/yyyy HH:mm');
    }
    else {
        controller.filter.fromDate = '12/15/2019 00:00';
    }

    if (toDateDom.value) {
        let date = toDateDom.value.substring(0,10);
        let time = toDateDom.value.substring(11, 16).replace(/-/g, ':');
        let strDate = date + ' ' + time;

        controller.filter.toDate = strFormat.datetimeToString(strDate, 'MM/dd/yyyy HH:mm');
    }
    else {
        controller.filter.toDate = strFormat.datetimeToString(new Date(), 'MM/dd/yyyy HH:mm')
    }
}

function _replaceUrl() {
    let title = document.title;
    let url = window.location.pathname;
    let queryParam = controller.generateQueryParams();

    if (queryParam)
        url += '?' + queryParam

    window.history.replaceState({}, title, url);
}

function _createPaginationHTML(pagination, pageNumberDisplay) {
    let html = "";

    // Trường hợp chỉ có một trang
    if (pagination.totalPages == 0)
        return html;

    let start = 0;
    let end = 0;

    if (pagination.totalPages < pageNumberDisplay) {
        start = 1;
        end = pagination.totalPages;
    }
    else {
        if (pagination.page <= Math.ceil(pageNumberDisplay / 2.0)) {
            start = 1;
            end = pageNumberDisplay;
        }
        else if (pagination.page > pagination.totalPages - Math.ceil(pageNumberDisplay / 2.0)) {
            start = pagination.totalPages - pageNumberDisplay + 1;
            end = pagination.totalPages;
        }
        else {
            start = pagination.page - Math.ceil(pageNumberDisplay / 2.0);
            end = start + pageNumberDisplay - 1;
        }
    }

    html += "<ul>";

    if (pagination.previousPage == 'Yes') {
        html += "<li><a title='Trang đầu' href='javascript:;' onclick='onClickPagination(1)'>Trang đầu</a></li>";
        html += "<li><a title='Trang trước' href='javascript:;' onclick='onClickPagination(" + (pagination.page - 1).toString() + ")'>Trang trước</a></li>";

        if (start > Math.ceil(length / 2.0))
            html += "<li><a href='javascript:;'>&hellip;</a></li>";
    }

    for (var page = start; page <= end; page++)
        if (pagination.page == page)
            html += "<li class='current'><a>" + page + "</a></li>";
        else
            html += "<li><a href='javascript:;' onclick='onClickPagination(" + page + ")'>" + page + "</a></li>";

    if (pagination.nextPage == 'Yes') {
        if (end < pagination.totalPages - Math.ceil(length / 2.0))
            html += "<li><a href='javascript:;'>&hellip;</a></li>";

        html += "<li><a title='Trang sau' href='javascript:;' onclick='onClickPagination(" + (pagination.page + 1).toString() + ")'>Trang sau</a></li>";
        html += "<li><a title='Trang cuối' href='javascript:;' onclick='onClickPagination(" + pagination.totalPages + ")'>Trang cuối</a></li>";
    }

    html += "</ul>";

    return html;
}

function _loadPagination() {
    let pageNumberDisplay = 6;
    let paginationDom = document.querySelectorAll(".pagination");
    let html = "";

    if (controller.pagination.totalPages > 1)
        html = _createPaginationHTML(controller.pagination, pageNumberDisplay);

    paginationDom.forEach(function (element) {
        element.innerHTML = html;
    });
}

function _createReportTableHTML(data) {
    let html = "";

    html += "<thead>";
    html += "    <tr>";
    html += "        <th class='col-code'>Mã</th>";
    html += "        <th class='col-cutomer'>Khách hàng</th>";
    html += "        <th class='col-quantity'>Mua</th>";
    html += "        <th class='col-status'>Xử lý</th>";
    html += "        <th class='col-payment-status'>Thanh toán</th>";
    html += "        <th class='col-payment-method'>Kiểu thanh toán</th>";
    html += "        <th class='col-delivery-method'>Giao hàng</th>";
    html += "        <th class='col-total-price'>Tiền</th>";
    html += "        <th class='col-created-date'>Ngày tạo</th>";
    html += "        <th class='col-btn'></th>";
    html += "    </tr>";
    html += "</thead>";
    html += "<tbody>";

    if (data.length == 0)
        html += "    <tr class='row-info'><td colspan='10'>Không tìm thấy đơn gộp...</td></tr>";
    else
        data.forEach(function (order) {
            try {
                //#region Thông tin chính của đơn hàng
                html += "    <tr class='row-data' data-group-code='" + order.code +"'>";
                // Mã hóa đơn
                html += "        <td data-title='Mã đơn'><strong>" + order.code + "</strong></td>";
                // Khách hàng
                html += "        <td data-title='Khách hàng'>";
                html += "            <a class='col-customer-name-link' ";
                html += "               href='/danh-sach-don-hang?&searchtype=1&textsearch=" + order.customer.phone + "' target='_blank' ";
                html += "            >";
                html += "                " + order.customer.nick;
                html += "            </a>";
                html += "            <br>" + order.customer.name;
                html += "            <br>" + order.customer.phone;
                html += "        </td>";
                // Số lượng mua
                html += "        <td data-title='Số lượng mua'>" + UtilsService.formatThousands(order.quantity, ',') + "</td>";
                // Trạng thái đơn
                html += "        <td data-title='Trạng thái đơn'>";
                html += "             <span class='bg-order-status bg-order-status-" + String(order.status.key) + "'>" + order.status.value + "</span>";
                html += "        </td>";
                // Trạng thái thanh toán
                html += "        <td data-title='Trạng thái thanh toán'>";
                html += "             <span class='bg-payment-status bg-payment-status-" + String(order.paymentStatus.key) + "'>" + order.paymentStatus.value + "</span>";
                html += "        </td>";
                // Phương thức thanh toán
                html += "        <td data-title='Phương thức thanh toán'>";
                html += "             <span class='bg-payment-method bg-payment-method-" + String(order.paymentMethod.key) + "'>" + order.paymentMethod.value + "</span>";
                html += "        </td>";
                // Phương thức vận chuyển
                html += "        <td data-title='Phương thức vận chuyển'>";
                html += "             <span class='bg-delivery-type bg-delivery-type-" + String(order.deliveryMethod.key) + "'>" + order.deliveryMethod.value + "</span>";
                html += "        </td>";
                // Tổng tiền
                html += "        <td data-title='Tổng tiền'>" + UtilsService.formatThousands(order.price, ',') + "</td>";
                // Ngày khởi tạo
                html += "        <td data-title='Ngày khởi tạo'>";
                html += "            " + strFormat.datetimeToString(order.createdDate, 'dd/MM/yyyy HH:mm');
                html += "        </td>";
                html += "        <td rowspan='2'>"
                //#region Button về giao hàng (Tạo đơn giao hàng, in phiếu giao hàng)
                switch (order.deliveryMethod.key) {
                    case DeliveryMethodEnum.JtExpress:
                        if (!order.shippingCode)
                        {
                            html += "           <a href='/dang-ky-jt?groupCode=" + order.code + "' target='_blank' ";
                            html += "              title='Tạo đơn JT Express' ";
                            html += "              class='btn primary-btn btn-jt h45-btn' ";
                            html += "           >";
                            html += "               <i class='fa fa-upload' aria-hidden='true'></i>";
                            html += "           </a>";

                        }
                        else {
                            html += "           <a href='/print-jt-express?groupCode=" + order.code + "' target='_blank' ";
                            html += "              title='In phiếu gửi hàng' ";
                            html += "              class='btn primary-btn btn-blue h45-btn' ";
                            html += "           >";
                            html += "               <i class='fa fa-file-text-o' aria-hidden='true'></i>";
                            html += "           </a>";
                            html += "           <a href='javascript:;'";
                            html += "              title='Hủy đơn JT Express' ";
                            html += "              class='btn primary-btn btn-red h45-btn' ";
                            html += '              onclick="cancelJtExpress(`' + order.shippingCode + '`, `' + order.code + '`)"';
                            html += "           >";
                            html += "               <i class='fa fa-trash' aria-hidden='true'></i>";
                            html += "           </a>";
                        }

                        break;
                    case DeliveryMethodEnum.PostOffice:
                        if (!order.shippingCode)
                            break;
                    case DeliveryMethodEnum.Proship:
                        if (!order.shippingCode)
                            break;
                    case DeliveryMethodEnum.DeliverySave:
                        if (!order.shippingCode)
                        {
                            html += "           <a href='/dang-ky-ghtk?groupCode=" + order.code + "' target='_blank' ";
                            html += "              title='Tạo đơn GHTK' ";
                            html += "              class='btn primary-btn btn-ghtk h45-btn' ";
                            html += "           >";
                            html += "               <i class='fa fa-upload' aria-hidden='true'></i>";
                            html += "           </a>";

                            break;
                        }
                        else {
                            html += "           <a href='javascript:;'";
                            html += "              title='Hủy đơn JT Express' ";
                            html += "              class='btn primary-btn btn-red h45-btn' ";
                            html += '              onclick="cancelGhtk(`' + order.shippingCode + '`, `' + order.code + '`)"';
                            html += "           >";
                            html += "               <i class='fa fa-trash' aria-hidden='true'></i>";
                            html += "           </a>";
                        }
                    default:
                        //#region In phiếu gửi hàng
                        html += "           <a href='/print-shipping-note?groupCode=" + order.code + "' target='_blank' ";
                        html += "              title='In phiếu gửi hàng' ";
                        html += "              class='btn primary-btn btn-blue h45-btn' ";
                        html += "           >";
                        html += "               <i class='fa fa-file-text-o' aria-hidden='true'></i>";
                        html += "           </a>";
                        //#endregion

                        break;
                }
                //#endregion

                //#region Hủy gộp đơn
                if (order.status.key == OrderStatusEnum.Done) {
                    html += '           <a href="javascript:;"';
                    html += '              title="Huỷ gộp đơn"';
                    html += '              class="btn primary-btn btn-yellow h45-btn"';
                    html += '              onclick="cancelGroupOrder(`' + order.code + '`)"';
                    html += '           >';
                    html += '               <i class="fa fa-times" aria-hidden="true"></i>';
                    html += '           </a>';
                }
                //#endregion

                html += "        </td>"
                html += "    </tr>";
                //#endregion

                //#region Thông tin phụ đơn hàng
                html += "    <tr class='row-info' data-group-code='" + order.code + "'>";
                html += "        <td colspan='2'>";
                //#region Danh sách đơn gộp
                html += "            <span class='order-id'>";
                html += "                <strong>Đơn hàng:</strong> "
                order.orderIds.forEach(function (id, index) {
                    if (index != 0)
                        html += ", "
                    html += "<a href='/thong-tin-don-hang?id=" + String(id) + "' target='_blank'>" + String(id) + "</a>";
                });
                html += "            </span>"
                //#endregion
                html += "        </td>";
                html += "        <td colspan='8'>";
                //#region Đổi trả
                if (order.refundAmount > 0)
                {
                    html += "            <span class='order-info'>";
                    html += "                <strong>Đổi trả:</strong> " + UtilsService.formatThousands(order.refundAmount * -1, '');
                    html += "            </span>";
                }
                //#endregion
                //#region Chiết khấu
                if (order.discount > 0)
                {
                    html += "            <span class='order-info'>";
                    html += "                <strong>Chiết khấu:</strong> " + UtilsService.formatThousands(order.discount * -1, ',');
                    html += "            </span>";
                }
                //#endregion
                //#region Phí khác
                if (order.otherFees != 0)
                {
                    html += "            <span class='order-info'>";
                    html += "                <strong>Phí khác:</strong> " + UtilsService.formatThousands(order.otherFees, ',');
                    html += "            </span>";
                }
                //#endregion
                //#region Mã vận đơn
                if (order.shippingCode)
                {
                    html += "            <span class='order-info'>";
                    html += "                <strong>Vận đơn:</strong> " + order.shippingCode;
                    html += "            </span>";
                }
                //#endregion
                //#region Phí giao hàng
                if (order.shippingFee > 0)
                {
                    html += "            <span class='order-info'>";
                    html += "                <strong>Ship:</strong> " + UtilsService.formatThousands(order.shippingFee, ',');
                    html += "            </span>";
                }
                //#endregion
                //#region Coupon
                if (order.couponValue > 0)
                {
                    html += "            <span class='order-info'>";
                    html += "                <strong>Coupon:</strong> " + UtilsService.formatThousands(order.couponValue, ',');
                    html += "            </span>";
                }
                //#endregion
                html += "        </td>";
                html += "    </tr>";
                //#endregion
            } catch (e) {
                console.log(order);
                console.log(e);
                return false;
            }
        });

    html += "<tbody>";

    return html;
}

function _loadDeliveryTable() {
    let tbDeliveryDom = document.querySelector("#tbGroupOrder");
    let html = _createReportTableHTML(controller.data);

    tbDeliveryDom.innerHTML = html;
}
//#endregion

//#region Public
function onKeyUpSearch(e) {
    if (e.key == 'Enter') {
        onClickSearch();
        e.preventDefault();
    }
}

function onClickSearch() {
    HoldOn.open();
    controller.pagination.page = 1;
    _updateFilter();
    _replaceUrl();

    controller.getGroupOrders()
        .then(function (response) {
            HoldOn.close();
            controller.lossMoney = response.lossMoney;
            controller.data = response.data
            controller.pagination = response.pagination;

            _loadSpanReport();
            _loadPagination();
            _loadDeliveryTable();
        })
        .catch(function (e) {
            console.log(e);
            HoldOn.close();
        });
};

function cancelGroupOrder(groupCode) {
    swal({
        title: 'Thông báo',
        text: 'Bạn muốn xóa đơn gộp #' + groupCode + ' này không?',
        type: 'warning',
        showCancelButton: true,
        cancelButtonText: "Để xem lại",
        confirmButtonText: "Tiếp tục",
        html: true,
    },
    function (confirm) {
        if (confirm) {
            HoldOn.open();

            controller.cancelGroupOrder(groupCode)
                .then(function (response) {
                    HoldOn.close();

                    if (controller.data.length == 1)
                        controller.pagination.page -= 1;

                    if (controller.pagination.page < 1)
                        controller.pagination.page = 1;

                    onClickPagination(controller.pagination.page);
                })
                .catch(function (e) {
                    console.log(e);
                    HoldOn.close();

                    let message = null;

                    if (e.responseJSON)
                        message = e.responseJSON.message;
                    else
                        message = e.message || null;

                    if (!message)
                        message = "Đã có lỗi trong quá trình hủy đơn gộp";

                    alert(message);
                });
        }
    });
}

function cancelJtExpress(code, groupCode) {
    let titleAlert = "Hủy đơn J&T Express";

    $.ajax({
        method: 'POST',
        contentType: 'application/json',
        dataType: "json",
        data: JSON.stringify({ groupOrderCode: groupCode }),
        url: "/api/v1/jt-express/order/" + code + "/cancel",
        beforeSend: function () {
            HoldOn.open();
        },
        success: (data, textStatus, xhr) => {
            HoldOn.close();

            if (xhr.status == 200 && data) {
                if (data.success)
                    return swal({
                        title: titleAlert,
                        text: "Hủy thành công!<br>Đơn J&T Express '<strong>" + code + "</strong>'",
                        icon: "success",
                    }, function (isConfirm) {
                        onClickPagination(controller.pagination.page);
                    });
                else
                    return _alterError(titleAlert, data);

            } else
                return _alterError(titleAlert, data);
        },
        error: (xhr, textStatus, error) => {
            HoldOn.close();

            return _alterError(titleAlert, xhr.responseJSON);
        }
    });
}

function cancelGhtk(code, groupCode) {
    let titleAlert = "Hủy đơn GHTK";

    $.ajax({
        method: 'POST',
        contentType: 'application/json',
        dataType: "json",
        data: JSON.stringify({ groupOrderCode: groupCode }),
        url: "/api/v1/delivery-save/order/" + code + "/cancel",
        beforeSend: function () {
            HoldOn.open();
        },
        success: (data, textStatus, xhr) => {
            HoldOn.close();

            if (xhr.status == 200 && data) {
                return swal({
                    title: titleAlert,
                    text: "Hủy thành công!<br>Đơn GHTK '<strong>" + code + "</strong>'",
                    icon: "success",
                }, function (isConfirm) {
                    onClickPagination(controller.pagination.page);
                });
            } else {
                return _alterError(titleAlert);
            }
        },
        error: (xhr, textStatus, error) => {
            HoldOn.close();

            return _alterError(titleAlert);
        }
    });
}

function cancelGhtk(code, groupCode) {
    let titleAlert = "Hủy đơn GHTK";

    $.ajax({
        method: 'POST',
        contentType: 'application/json',
        dataType: "json",
        data: JSON.stringify({ groupOrderCode: groupCode }),
        url: "/api/v1/delivery-save/order/" + code + "/cancel",
        beforeSend: function () {
            HoldOn.open();
        },
        success: (data, textStatus, xhr) => {
            HoldOn.close();

            if (xhr.status == 200 && data) {
                return swal({
                    title: titleAlert,
                    text: "Hủy thành công!<br>Đơn GHTK '<strong>" + code + "</strong>'",
                    icon: "success",
                }, function (isConfirm) {
                    onClickPagination(controller.pagination.page);
                });
            } else {
                return _alterError(titleAlert);
            }
        },
        error: (xhr, textStatus, error) => {
            HoldOn.close();

            return _alterError(titleAlert);
        }
    });
}

function onClickPagination(page) {
    HoldOn.open();
    controller.pagination.page = page;
    _replaceUrl();

    controller.getGroupOrders()
        .then(function (response) {
            HoldOn.close();
            controller.data = response.data
            controller.pagination = response.pagination;

            _loadSpanReport();
            _loadPagination();
            _loadDeliveryTable();
        })
        .catch(function (e) {
            console.log(e);
            HoldOn.close();
        });
}
//#endregion