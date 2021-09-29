let strFormat = new StringFormat();
let orderService = new OrderService();
let controller = new PreOrderController();


document.addEventListener("DOMContentLoaded", function (event) {
    _initRole();
    _initQueryParams();
    _initQuantity();
    _initPreOrderTable();
});

// #region Private
function _loadSpanNumber(quantity) {
    let spanNumber = "";
    let spanNumberDOM = document.querySelector("[id$='spanNumber']");

    if (controller.pagination.totalCount > 0)
        spanNumber = "(" + UtilsService.formatThousands(controller.pagination.totalCount, ',') + ")";

    spanNumberDOM.innerHTML = spanNumber;
}

function _initRole() {
    let roleDOM = document.querySelector("[id$='_hdRole']");

    if (roleDOM.value == "0")
        controller.role = 0;
    else if (roleDOM.value)
        controller.role = +roleDOM.value || null;
}

function _initQueryParams() {
    let search = window.location.search;

    controller.setFilter(search);
}

function _initQuantity() {
    if (!controller.filter.quantityFilter)
        return;

    let divGreaterDOM = document.querySelector(".greaterthan");
    let divBetweenDOM = document.querySelector(".between");

    if (controller.filter.quantityFilter == 1 || controller.filter.quantityFilter == 2) {
        divGreaterDOM.classList.remove("hide");
        divBetweenDOM.classList.add("hide");
    }
    else if (controller.filter.quantityFilter == 3) {
        divGreaterDOM.classList.add("hide");
        divBetweenDOM.classList.remove("hide");
    }
}

function _initPreOrderTable() {
    HoldOn.open();
    controller.getPreOrders()
        .then(function (response) {
            controller.data = response.data
            controller.pagination = response.pagination;

            _loadSpanNumber();
            _loadPagination();
            _loadPreOrderTable();

            HoldOn.close();
        })
        .catch(function (e) {
            HoldOn.close();
        });
}

function _updateFilter() {
    // Datetime Picker
    let fromDateDOM = document.querySelector("[id$='_rOrderFromDate_dateInput']");
    let toDateDOM = document.querySelector("[id$='_rOrderToDate_dateInput']");

    if (fromDateDOM.value.trim()) {
        let date = fromDateDOM.value.split('/').filter(x => x);

        if (date.length == 3)
            controller.filter.fromDate = date[1] + '/' + date[0] + '/' + date[2];
    }
    else {
        controller.filter.fromDate = '12/15/2019';
    }

    if (toDateDOM.value.trim()) {
        let date = toDateDOM.value.split('/').filter(x => x);

        if (date.length == 3)
            controller.filter.toDate = date[1] + '/' + date[0] + '/' + date[2];
    }
    else {
        controller.filter.toDate = strFormat.datetimeToString(new Date(), 'MM/dd/yyyy')
    }

    // Order Status
    let orderStatusDOM = document.querySelector("[id$='_ddlExcuteStatus']");

    if (orderStatusDOM.value == "0") {
        controller.filter.searchOrder = 1;
        controller.filter.orderStatus = 0;
    }
    else if (orderStatusDOM.value == "1") {
        controller.filter.searchOrder = 3;
        controller.filter.orderStatus = 1;
    }
    else if (orderStatusDOM.value == "3") {
        controller.filter.searchOrder = 3;
        controller.filter.orderStatus = 3;
    }
    else {
        controller.filter.searchOrder = 3;
        controller.filter.orderStatus = "0|1";
    }

    // Search
    let searchDOM = document.querySelector("[id$='_txtSearchOrder']");

    if (searchDOM.value) {
        controller.filter.search = searchDOM.value;

        let searchTypeDOM = document.querySelector("[id$='_ddlSearchType']");

        if (searchTypeDOM.value)
            controller.filter.searchType = +searchTypeDOM.value || 3; // Trường hợp đơn đặt hàng
    }
    else {
        controller.filter.search = null;
        controller.filter.searchType = null;
    }

    // Discount
    let discountDOM = document.querySelector("[id$='_ddlDiscount']");

    if (discountDOM.value == "0")
        controller.filter.hasDiscount = 0;
    else if (discountDOM.value)
        controller.filter.hasDiscount = +discountDOM.value || null;
    else
        controller.filter.hasDiscount = null;

    // Payment Method
    let paymentDOM = document.querySelector("[id$='_ddlPaymentType']");

    if (paymentDOM.value)
        controller.filter.paymentMethod = +paymentDOM.value || null;
    else
        controller.filter.paymentMethod = null;

    // Delivery Method
    let deliveryDOM = document.querySelector("[id$='_ddlShippingType']");

    if (deliveryDOM.value)
        controller.filter.deliveryMethod = +deliveryDOM.value || null;
    else
        controller.filter.deliveryMethod = null;

    // Coupon
    let couponDOM = document.querySelector("[id$='_ddlCouponStatus']");

    if (couponDOM.value == "0")
        controller.filter.hasCoupon = 0;
    else if (couponDOM.value)
        controller.filter.hasCoupon = +couponDOM.value || null;
    else
        controller.filter.hasCoupon = null;

    // Quantity
    let quantityFilterDOM = document.querySelector("[id$='_ddlQuantityFilter']");

    if (quantityFilterDOM.value) {
        controller.filter.quantityFilter = +quantityFilterDOM.value || null

        if (controller.filter.quantityFilter == 3) {
            let minQuantityDOM = document.querySelector("[id$='_txtQuantityMin']");
            let maxQuantityDOM = document.querySelector("[id$='_txtQuantityMax']");

            if (!minQuantityDOM.value.trim() && !maxQuantityDOM.value.trim()) {
                controller.filter.quantityFilter = null;
                controller.filter.minQuantity = null;
                controller.filter.maxQuantity = null;
            }
            else {
                if (minQuantityDOM.value.trim())
                    controller.filter.minQuantity = +minQuantityDOM.value || null;

                if (maxQuantityDOM.value.trim())
                    controller.filter.maxQuantity = +maxQuantityDOM.value || null;
            }
        }
        else {
            let quantityDOM = document.querySelector("[id$='_txtQuantity']");

            if (quantityDOM.value.trim())
                controller.filter.quantity = +quantityDOM.value || null;
            else {
                controller.filter.quantityFilter = null;
                controller.filter.quantity = null;
            }
        }
    }
    else {
        controller.filter.quantityFilter = null;
        controller.filter.quantity = null;
        controller.filter.minQuantity = null;
        controller.filter.maxQuantity = null;
    }

    // Staff
    let staffDOM = document.querySelector("[id$='_ddlCreatedBy']");

    if (staffDOM.value)
        controller.filter.staff = staffDOM.value;
    else
        controller.filter.staff = null;
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
        html += "<li><a title='Trang đầu' href='javascript:;' onclick='onClick_Pagination(1)'>Trang đầu</a></li>";
        html += "<li><a title='Trang trước' href='javascript:;' onclick='onClick_Pagination(" + (pagination.page - 1).toString() + ")'>Trang trước</a></li>";

        if (start > Math.ceil(length / 2.0))
            html += "<li><a href='javascript:;'>&hellip;</a></li>";
    }

    for (var page = start; page <= end; page++)
        if (pagination.page == page)
            html += "<li class='current'><a>" + page + "</a></li>";
        else
            html += "<li><a href='javascript:;' onclick='onClick_Pagination(" + page + ")'>" + page + "</a></li>";

    if (pagination.nextPage == 'Yes') {
        if (end < pagination.totalPages - Math.ceil(length / 2.0))
            html += "<li><a href='javascript:;'>&hellip;</a></li>";

        html += "<li><a title='Trang sau' href='javascript:;' onclick='onClick_Pagination(" + (pagination.page + 1).toString() + ")'>Trang sau</a></li>";
        html += "<li><a title='Trang cuối' href='javascript:;' onclick='onClick_Pagination(" + pagination.totalPages + ")'>Trang cuối</a></li>";
    }

    html += "</ul>";

    return html;
}

function _loadPagination() {
    let pageNumberDisplay = 6;
    let paginationDOM = document.querySelectorAll(".pagination");
    let html = "";

    if (controller.pagination.totalPages > 1)
        html = _createPaginationHTML(controller.pagination, pageNumberDisplay);

    paginationDOM.forEach(function (element) {
        element.innerHTML = html;
    })
}

function _createBtnCancelPreOrder(preOrderId, staff) {
    let html = "";

    html += "<a href='javascript:;' ";
    html += "   onclick='cancelPreOrder(" + preOrderId + ", `" + staff + "`)' ";
    html += "   title='Hủy đơn đặt hàng' ";
    html += "   class='btn primary-btn btn-red h45-btn btn-cancel-pre-order'>";
    html += "    <i class='fa fa-remove' aria-hidden='true'></i>";
    html += "</a>";

    return html;
}

function _createBtnRecoveryPreOrder(preOrderId, staff) {
    let html = "";

    html += "<a href='javascript:;' ";
    html += "   onclick='recoveryPreOrder(" + preOrderId + ", `" + staff + "`)' ";
    html += "   title='Phục hồi đơn đặt hàng' ";
    html += "   class='btn primary-btn h45-btn btn-recovery-pre-order'>"
    html += "    <i class='fa fa-reply' aria-hidden='true'></i>";
    html += "</a>";

    return html;
}

function _createPreOrderTableHTML(data) {
    let html = "";

    html += "<thead>";
    html += "    <tr>";
    html += "        <th>Mã</th>";
    html += "        <th></th>";
    html += "        <th class='col-customer'>Khách hàng</th>";
    html += "        <th>Mua</th>";
    html += "        <th>Xử lý</th>";
    html += "        <th>Thanh toán</th>";
    html += "        <th>Kiểu thanh toán</th>";
    html += "        <th>Giao hàng</th>";
    html += "         <th>Tổng tiền</th>";

    if (controller.role == 0)
        html += "         <th>Nhân viên</th>";

    html += "         <th>Ngày tạo</th>";
    html += "         <th></th>";
    html += "    </tr>";
    html += "</thead>";
    html += "<tbody>";

    if (data.length == 0) {
        if (controller.role == 0)
            html += "    <tr><td colspan='12'>Không tìm thấy đơn hàng...</td></tr>";
        else
            html += "    <tr><td colspan='11'>Không tìm thấy đơn hàng...</td></tr>";
    }
    else {
        data.forEach(function (item) {
            try {
                html += "    <tr data-pre-order-id='" + item.id + "'>";
                html += "        <td data-title='Mã đơn'>";
                html += "            <a target='_blank' href='" + orderService.generateOrderUrl(item.status.key, item.id) + "'>" + item.id + "</a>";
                html += "        </td>";
                html += "        <td data-title='Loại đơn'>" + orderService.generateOrderTypeHTML(item.orderType.key) + "</td>";

                if (item.customer.nick) {
                    html += "        <td  class='customer-td' data-title='Khách hàng'>";
                    html += "            <a class='col-customer-name-link' target='_blank' href='" + orderService.generateOrderUrl(item.status.key, item.id) + "'>" + strFormat.toTitleCase(item.customer.nick) + "</a>";
                    html += "            <br><span class='name-bottom-nick'>(" + strFormat.toTitleCase(item.customer.name) + ")</span>";
                    html += "        </td>";
                }
                else if (item.customer.name) {
                    html += "        <td  class='customer-td' data-title='Khách hàng'>";
                    html += "            <a class='col-customer-name-link' target='_blank' href='" + orderService.generateOrderUrl(item.status.key, item.id) + "'>" + strFormat.toTitleCase(item.customer.name) + "</a>";
                    html += "        </td>";
                } else {
                    html += "        <td  class='customer-td' data-title='Khách hàng'><span class='bg-yellow'>Vùi lòng tạo thông tin khách hàng</span></td>";
                }

                html += "        <td data-title='Đã mua'>" + item.quantity + "</td>";
                html += "        <td data-pre-order-status='" + item.status.key + "' data-title='Xử lý'>" + orderService.generateOrderStatusHTML(item.status.key) + "</td>";
                html += "        <td data-title='Thanh toán'>" + orderService.generatePaymentStatusHTML(item.paymentStatus.key) + "</td>";
                html += "        <td class='payment-type' data-title='Kiểu thanh toán'>" + orderService.generatePaymentMethodHTML(item.paymentMethod.key) + "</td>";
                html += "        <td class='shipping-type' data-title='Giao hàng'>" + orderService.generateDeliveryMethodHTML(item.deliveryMethod.key) + "</td>";

                if (controller.role == 0) {
                    html += "        <td data-title='Tổng tiền'>";
                    html += "           <strong>" + UtilsService.formatThousands(item.price, ',') + "</strong>";
                    if (item.profit > 0)
                        html += "           <br/><span class='bg-green'><strong>" + UtilsService.formatThousands(item.profit, ',') + "</strong></span>";
                    else
                        html += "           <br/><span class='bg-red'><strong>" + UtilsService.formatThousands(item.profit, ',') + "</strong></span>";
                    html += "        </td>";
                }
                else {
                    html += "        <td data-title='Tổng tiền'>";
                    html += "           <strong>" + UtilsService.formatThousands(item.price, ',') + "</strong>";
                    html += "        </td>";
                }

                if (controller.role == 0)
                    if (item.staff)
                        html += "        <td data-title='Nhân viên'>" + item.staff + "</td>";
                    else
                        html += "        <td data-title='Nhân viên'></td>";

                html += "        <td data-title='Ngày tạo'>";
                html += "           <strong>" + strFormat.datetimeToString(item.createdDate, 'dd/MM') + "</strong>";
                html += "           <br>" + strFormat.datetimeToString(item.createdDate, 'HH:mm');
                html += "        </td>";
                html += "        <td class='update-button' data-title='Thao tác'>";
                if (item.staff) {
                    if (item.status.key == 0)
                        html += _createBtnCancelPreOrder(item.id, item.staff);
                    else if (item.status.key == 3)
                        html += _createBtnRecoveryPreOrder(item.id, item.staff);
                }

                if (item.customer.id) {
                    html += "           <a class='btn primary-btn btn-black h45-btn' ";
                    html += "              target='_blank' href='/chi-tiet-khach-hang?id=" + item.customer.id + "' ";
                    html += "              title='Thông tin khách hàng " + strFormat.toTitleCase(item.customer.name) + "' ";
                    html += "           >";
                    html += "               <i class='fa fa-user-circle' aria-hidden='true'></i>";
                    html += "           </a";
                }
                html += "        </td>";
                html += "    </tr>";

                // thông tin thêm
                html += "    <tr class='tr-more-info'>";
                html += "        <td colspan='2'><strong>" + item.source + "</strong></td>";
                html += "        <td colspan='11'>";

                if (item.discount > 0) {
                    html += "            <span class='order-info'>";
                    html += "                <strong>Chiết khấu:</strong> -" + UtilsService.formatThousands(item.discount, ',');
                    html += "            </span>";
                }

                if (item.coupon) {
                    html += "            <span class='order-info'>";
                    html += "                <strong>Coupon (" + item.coupon.code + "):</strong> -" + UtilsService.formatThousands(item.coupon.value, ',');
                    html += "            </span>";
                }

                html += "        </td>";
                html += "    </tr>";
            } catch (e) {
                console.log(item);
                console.log(e);
                return false;
            }
        });
    }

    html += "<tbody>";

    return html;
}

function _loadPreOrderTable() {
    let tbPreOrderDOM = document.querySelector("#tbPreOrder");
    let html = _createPreOrderTableHTML(controller.data);

    tbPreOrderDOM.innerHTML = html;
}
// #endregion

// #region Public
function onKeyUp_txtSearchOrder(event) {
    if (event.keyCode === 13)
        onClickSearchPreOrder();
}

function onClickSearchPreOrder() {
    HoldOn.open();
    controller.pagination.page = 1;
    _updateFilter();
    _replaceUrl();
    controller.getPreOrders()
        .then(function (response) {
            controller.data = response.data
            controller.pagination = response.pagination;

            _loadSpanNumber();
            _loadPagination();
            _loadPreOrderTable();

            HoldOn.close();
        })
        .catch(function (e) {
            HoldOn.close();
        });
};

function onChange_ddlQuantityFilter(self) {
    let value = self.value;
    let divGreaterDOM = document.querySelector(".greaterthan");
    let divBetweenDOM = document.querySelector(".between");

    if (value == "1" || value == "2") {
        let txtQuantityDOM = document.querySelector("[id$='_txtQuantity']");

        divGreaterDOM.classList.remove("hide");
        divBetweenDOM.classList.add("hide");

        txtQuantityDOM.focus();
        txtQuantityDOM.select();
    }
    else if (value == "3") {
        let txtQuantityMinDOM = document.querySelector("[id$='_txtQuantityMin']");

        divGreaterDOM.classList.add("hide");
        divBetweenDOM.classList.remove("hide");

        txtQuantityMinDOM.focus();
        txtQuantityMinDOM.select();
    }
    else {
        let txtQuantityDOM = document.querySelector("[id$='_txtQuantity']");

        divGreaterDOM.classList.remove("hide");
        divBetweenDOM.classList.add("hide");

        txtQuantityDOM.value = "";
    }
}

function onClick_Pagination(page) {
    HoldOn.open();
    controller.pagination.page = page;
    _replaceUrl();

    controller.getPreOrders()
        .then(function (response) {
            controller.data = response.data
            controller.pagination = response.pagination;

            _loadSpanNumber();
            _loadPagination();
            _loadPreOrderTable();

            HoldOn.close();
        })
        .catch(function (e) {
            HoldOn.close();
        });
}

function cancelPreOrder(preOrderId, staff) {
    let r = confirm('Thông báo\nBạn muốn hủy đơn đặt hàng #' + preOrderId);

    if (!r)
        return;

    HoldOn.open();

    controller.cancelPreOrder(preOrderId, staff)
        .then(function (response) {
            HoldOn.close();

            if (response.success)
                return swal({
                    title: 'Thành Công',
                    text: 'Đơn đặt hàng #' + preOrderId + ' đã được hủy.',
                    type: 'success',
                    showCloseButton: true,
                    html: true,
                }, function () {
                    let trDOM = document.querySelector('tr[data-pre-order-id="' + preOrderId + '"]');
                    let tdStatus = trDOM.querySelector('td[data-pre-order-status="0"]');
                    let tdBtn = trDOM.querySelector('td[class="update-button"]');
                    let btnCancelPreOrder = tdBtn.querySelector('.btn-cancel-pre-order');

                    tdStatus.dataset.preOrderStatus = 3;
                    tdStatus.innerHTML = orderService.generateOrderStatusHTML(3);
                    btnCancelPreOrder.remove();
                    tdBtn.innerHTML = _createBtnRecoveryPreOrder(preOrderId, staff) + tdBtn.innerHTML;
                });
            else {
                return swal({
                    title: 'Error',
                    text: response.message,
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                });
            }
        })
        .catch(function (e) {
            HoldOn.close();
            console.log(e);

            if (e.status == 400)
                return swal({
                    title: 'Error',
                    text: e.responseJSON.message,
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                });
            else
                return swal({
                    title: 'Error',
                    text: 'Đã có lỗi xảy ra trong quá trình hủy đơn hàng.',
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                });
        });
}

function recoveryPreOrder(preOrderId, staff) {
    let r = confirm('Thông báo\nBạn muốn phục hồi đơn đặt hàng #' + preOrderId);

    if (!r)
        return;

    HoldOn.open();

    controller.recoveryPreOrder(preOrderId, staff)
        .then(function (response) {
            HoldOn.close();

            if (response.success)
                return swal({
                    title: 'Thành Công',
                    text: 'Đơn đặt hàng #' + preOrderId + ' đã được phục hồi.',
                    type: 'success',
                    showCloseButton: true,
                    html: true,
                }, function () {
                    let trDOM = document.querySelector('tr[data-pre-order-id="' + preOrderId + '"]');
                    let tdStatus = trDOM.querySelector('td[data-pre-order-status="3"]');
                    let tdBtn = trDOM.querySelector('td[class="update-button"]');
                    let btnRecoveryPreOrder = tdBtn.querySelector('.btn-recovery-pre-order');

                    tdStatus.dataset.preOrderStatus = 0;
                    tdStatus.innerHTML = orderService.generateOrderStatusHTML(0);
                    btnRecoveryPreOrder.remove();
                    tdBtn.innerHTML = _createBtnCancelPreOrder(preOrderId, staff) + tdBtn.innerHTML;
                });
            else {
                return swal({
                    title: 'Error',
                    text: response.message,
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                });
            }
        })
        .catch(function (e) {
            HoldOn.close();
            console.log(e);

            if (e.status == 400)
                return swal({
                    title: 'Error',
                    text: e.responseJSON.message,
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                });
            else
                return swal({
                    title: 'Error',
                    text: 'Đã có lỗi xảy ra trong quá trình phục đơn hàng.',
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                });
        });
}
// #endregion
