const OrderStatusEnum = {
    "Done": 2 // Hoàn tất
}

const PaymentStatusEnum = {
    "Waitting": 1 // Chư thanh toán
}

const DeliveryMethodEnum = {
    "Transport": 4 // Chành xe
}

let loading = false;
let stringFormat = new StringFormat();
let controller = new GroupOrderController();

document.addEventListener("DOMContentLoaded", function (event) {
    _initStaff();
});

//#region Private
function _initStaff() {
    let hdfStaffDom = document.querySelector("[id$='_hdfStaff']");

    controller.setStaff(hdfStaffDom.value);
}

function _checkValidation(data) {
    let message = '';

    message += 'Đơn hàng <strong>#<a href="thong-tin-don-hang?id="' + String(data.id) + '" target="_blank">' + String(data.id) + '</a></strong>';

    //#region Kiểm tra trạng thái đơn hàng
    if (data.status.key != OrderStatusEnum.Done) {
        message += '<br>Lỗi: Không phải là đơn <strong>hoàn tất</strong>';

        swal({
            title: 'Error',
            text: message,
            type: 'error',
            showCloseButton: true,
            html: true,
        });

        return false;
    }
    //#endregion

    //#region Kiểm tra trạng thái thanh toán
    if (data.status.key == PaymentStatusEnum.Waitting) {
        message += '<br>Lỗi: Đơn hàng chưa <strong>thanh toán</strong>';

        swal({
            title: 'Error',
            text: message,
            type: 'error',
            showCloseButton: true,
            html: true,
        });

        return false;
    }
    //#endregion

    //#region Kiểm tra mã vận đơn
    if (data.shippingCode) {
        message += '<br>Lỗi: Đơn đã đăng ký vận đơn';

        swal({
            title: 'Error',
            text: message,
            type: 'error',
            showCloseButton: true,
            html: true,
        });

        return false;
    }
    else if (data.deliveryMethod == DeliveryMethodEnum.Transport)
    {
        if (!data.transport)
        {
            message += '<br>Lỗi: Đơn hàng chưa chọn chành xe';

            swal({
                title: 'Error',
                text: message,
                type: 'error',
                showCloseButton: true,
                html: true,
            });

            return false;
        }
        else if (!data.transport.branch) {
            message += '<br>Lỗi: Đơn hàng chưa chọn nơi tới của chành xe <strong>' + data.transport.name + '</strong>';

            swal({
                title: 'Error',
                text: message,
                type: 'error',
                showCloseButton: true,
                html: true,
            });

            return false;
        }
    }
    //#endreigon

    // Trường hợp là đơn hàng đầu tiên
    if (controller.orders.length == 0)
        return true;

    //#region Kiểm tra khách hàng
    if (controller.customer.id != data.customer.id) {
        message += '<br>Lỗi: Khách hàng khác với các đơn hàng trong bảng';

        swal({
            title: 'Error',
            text: message,
            type: 'error',
            showCloseButton: true,
            html: true,
        });

        return false;
    }
    //#endregion

    //#region Kiểm tra trạng thái thanh toán
    if (controller.paymentStatus.key != data.paymentStatus.key) {
        message += '<br>Lỗi: Trạng thái thanh toán <strong>' + data.paymentStatus.value + '</strong> không đồng nhất với các đơn hàng trong bảng';

        swal({
            title: 'Error',
            text: message,
            type: 'error',
            showCloseButton: true,
            html: true,
        });

        return false;
    }
    //#endregion

    //#region Kiểm tra phương thức thanh toán
    if (controller.paymentMethod.key != data.paymentMethod.key) {
        message += '<br>Lỗi: Phương thức thanh toán <strong>' + data.paymentMethod.value + '</strong> không đồng nhất với các đơn hàng trong bảng';

        swal({
            title: 'Error',
            text: message,
            type: 'error',
            showCloseButton: true,
            html: true,
        });

        return false;
    }
    //#endregion

    //#region Kiểm tra địa chỉ nhận hàng
    if (controller.deliveryAddress.id != data.deliveryAddress.id) {
        message += '<br>Lỗi: Địa chỉ nhận hàng không đồng nhất với các đơn hàng trong bảng';

        swal({
            title: 'Error',
            text: message,
            type: 'error',
            showCloseButton: true,
            html: true,
        });

        return false;
    }
    //#endregion

    //#region Kiểm tra phương thức giao hàng
    if (controller.deliveryMethod.key != data.deliveryMethod.key) {
        message += '<br>Lỗi: Phương thức giao hàng <strong>' + data.deliveryMethod.value + '</strong> không đồng nhất với các đơn hàng trong bảng';

        swal({
            title: 'Error',
            text: message,
            type: 'error',
            showCloseButton: true,
            html: true,
        });

        return false;
    }
    //#endregion

    //#region Kiểm tra chành xe
    if (controller.deliveryMethod.key == DeliveryMethodEnum.Transport) {
        if (controller.transport.id != data.transport.id) {
            message += '<br>Lỗi: Chành xe <strong>' + data.transport.name + '</strong> không đồng nhất với các đơn hàng trong bảng';

            swal({
                title: 'Error',
                text: message,
                type: 'error',
                showCloseButton: true,
                html: true,
            });

            return false;
        }
        else if (controller.transport.branch.id != data.transport.branch.id) {
            message += '<br>Lỗi: Nơi nhận <strong>' + data.branch.address + '</strong> không đồng nhất với các đơn hàng trong bảng';

            swal({
                title: 'Error',
                text: message,
                type: 'error',
                showCloseButton: true,
                html: true,
            });

            return false;
        }
    }
    //#endregion

    return true;
}

function _handleCode() {
    let txtCodeDom = document.querySelector("[id$='_txtCode']");

    HoldOn.open();
    controller.getOrderBasic(txtCodeDom.value)
        .then(function (response) {
            HoldOn.close();

            if (!response) {
                let message = 'Mã đơn hàng #' + txtCodeDom + ' không tồn tại trong hệ thống';

                swal({
                    title: 'Error',
                    text: message,
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                });

                return;
            }

            if (!_checkValidation(response))
                return;

            // Cập nhật thông tin đơn hàng vào bảng
            _addOrder(response);
            controller.addOrder(response);

            // Clear mã đơn hàng
            txtCodeDom.value = '';

            // Cập nhật hiển thị button submit
            _updateDisplaySubmit();
        })
        .catch(function (err) {
            HoldOn.close();
            txtCodeDom.focus();

            let message = null;

            if (err.responseJSON)
                message = err.responseJSON.message;
            else
                message = err.message || null;

            if (!message)
                message = "Đã có lỗi trong quá trình xứ lý mã đơn hàng";

            swal({
                title: 'Error',
                text: message,
                type: 'error',
                showCloseButton: true,
                html: true,
            });
        });
}

function _createOrderHtml(index, data) {
    let html = '';

    //#region Dòng thông tin đơn hàng
    html += '<tr class="row-data" data-id="' + String(data.id) + '">';
    html += '    <td>' + String(index) + '</td>';
    html += '    <td><strong>' + String(data.id) + '</strong></td>';
    html += '    <td>';
    html += '        <strong>' + data.customer.nick + '</strong>';
    html += '        <br>' + data.customer.name;
    html += '        <br>' + data.customer.phone;
    html += '    </td>';
    html += '    <td>' + UtilsService.formatThousands(data.quantity, ',') + '</td>';
    html += '    <td><span class="bg-order-status bg-order-status-' + data.status.key + '">' + data.status.value + '</span></td>';
    html += '    <td><span class="bg-payment-status bg-payment-status-' + data.paymentStatus.key + '">' + data.paymentStatus.value + '</span></td>';
    html += '    <td><span class="bg-payment-method bg-payment-method-' + data.paymentMethod.key + '">' + data.paymentMethod.value + '</span></td>';
    html += '    <td><span class="bg-delivery-type bg-delivery-type-' + data.deliveryMethod.key + '">' + data.deliveryMethod.value + '</span></td>';
    html += '    <td>' + UtilsService.formatThousands(data.totalPrice, ',') + '</td>';
    html += '    <td>' + data.staff + '</td>';
    html += '    <td>';
    html += '        <strong>' + stringFormat.datetimeToString(data.createdDate, 'dd/MM') + '</strong>';
    html += '        <br>' + stringFormat.datetimeToString(data.createdDate, 'HH:mm');
    html += '    </td>';
    html += '    <td>';
    html += '        <strong>' + stringFormat.datetimeToString(data.doneDate, 'dd/MM') + '</strong>';
    html += '        <br>' + stringFormat.datetimeToString(data.doneDate, 'HH:mm');
    html += '    </td>';
    html += '    <td rowspan="2">';
    html += '        <a href="javascript:;"';
    html += '           title="Xóa"';
    html += '           class="btn primary-btn btn-red h45-btn"';
    html += '           onclick="onClickOrderRemoval(' + String(data.id) + ')"';
    html += '        >';
    html += '            <i class="fa fa-times" aria-hidden="true"></i>';
    html += '        </a>';
    html += '    </td>';
    html += '</tr>';
    //#endregion

    //#region Dòng chú thích
    html += '<tr class="row-info" data-id="' + String(data.id) + '">';
    html += '    <td>';
    html += '    </td>';
    html += '    <td colspan="11">';
    //#region Đổi trả
    if (data.refundAmount > 0)
    {
        html += "        <span class='order-info'>";
        html += "            <strong>Đổi trả:</strong> " + UtilsService.formatThousands(data.refundAmount * -1, '');
        html += "        </span>";
    }
    //#endregion
    //#region Chiết khấu
    if (data.discount > 0)
    {
        html += "        <span class='order-info'>";
        html += "            <strong>Chiết khấu:</strong> " + UtilsService.formatThousands(data.discount * -1, ',');
        html += "        </span>";
    }
    //#endregion
    //#region Phí khác
    if (data.otherFees && data.otherFees.length > 0)
    {
        html += "        <span class='order-info'>";
        html += "            <strong>Phí khác:</strong> " + UtilsService.formatThousands(data.totalFees - data.shippingFee, ',');
        html += "        </span>";
    }
    //#endregion
    //#region Phí giao hàng
    if (data.shippingFee > 0)
    {
        html += "        <span class='order-info'>";
        html += "            <strong>Ship:</strong> " + UtilsService.formatThousands(data.shippingFee, ',');
        html += "        </span>";
    }
    //#endregion
    //#region Coupon
    if (data.couponValue > 0)
    {
        html += "        <span class='order-info'>";
        html += "            <strong>Coupon:</strong> " + UtilsService.formatThousands(data.couponValue, ',');
        html += "        </span>";
    }
    //#endregion
    html += '    </td>';
    html += '</tr>';
    //#endregion

    return html;
}

function _addOrder(data) {
    let tbodyOrderDom = document.querySelector("#tbody-order");
    let index = tbodyOrderDom.querySelectorAll("tr").length + 1;
    let newRowHtml = _createOrderHtml(index, data);

    tbodyOrderDom.innerHTML = newRowHtml + tbodyOrderDom.innerHTML;
}

function _removeOrder(orderId) {
    let tbodyOrderDom = document.querySelector("#tbody-order");
    let rowDom = tbodyOrderDom.querySelectorAll("[data-id='" + orderId + "']");

    rowDom.forEach(function (element) { element.remove(); });
    controller.removeOrder(orderId);
}

// Cập nhật lại giá trị index của table
function _updateIndexColumn() {
    let rowDom = document.querySelector("#tbody-order").querySelectorAll("tr");

    rowDom.forEach(function (element, index) {
        let indexDom = element.querySelector("td");

        indexDom.innerHTML = String(rowDom.length - index);
    })
}

// Cập nhật trạng thái ẩn hiển của button submit
function _updateDisplaySubmit() {
    let btnSubmitDom = document.querySelector("#btnSubmit");

    if (controller.orders.length > 1)
        btnSubmitDom.classList.remove('hidden');
    else
        btnSubmitDom.classList.add('hidden');
}
//#endregion

//#region Public
function onKeyDownCode(e) {
    if (e.key == 'Enter') {
        onClickOrderAddition();
        e.preventDefault();
    }
}

function onClickOrderAddition() {
    let txtCodeDom = document.querySelector("[id$='_txtCode']");

    txtCodeDom.value = txtCodeDom.value.trim();

    if (txtCodeDom.value)
        _handleCode();
}

function onClickOrderRemoval(orderId) {
    // Xóa dòng hiện tại
    _removeOrder(orderId);

    // Cập nhật số thứ tự
    _updateIndexColumn();

    // Cập nhật hiển thị button submit
    _updateDisplaySubmit();
}

// region Đăng ký khởi tạo đơn gộp
function submitGroupOrder() {
    HoldOn.open();

    controller.registerGroupOrder()
        .then(function () {
            HoldOn.close();

            swal({
                title: 'Thông báo',
                text: 'Khởi tạo đơn gộp thành công!',
                type: 'success',
                showCloseButton: true,
                html: true,
            }, function () {
                window.location.href = '/quan-ly-don-gop';
            });
        })
        .catch(function (err) {
            HoldOn.close();

            let message = null;

            if (err.responseJSON)
                message = err.responseJSON.message;
            else
                message = err.message || null;

            if (!message)
                message = "Đã có lỗi trong quá trình tạo đơn gộp";

            swal({
                title: 'Error',
                text: message,
                type: 'error',
                showCloseButton: true,
                html: true,
            });
        });
}
//#endregion