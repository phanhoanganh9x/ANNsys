const OrderStatusEnum = {
    "Done": 2 // Hoàn tất
}

const PaymentStatusEnum = {
    "Waitting": 1 // Chư thanh toán
}

let loading = false;
let stringFormat = new StringFormat();
let controller = new GroupOrderController();

$(document).ready(function() {
    _initStaff();
});

//#region Private
function _initStaff() {
    let $hdfStaff = $("[id$='_hdfStaff']");

    controller.setStaff($hdfStaff.val());
}

function _checkValidation(data) {
    //#region Kiểm tra trạng thái đơn hàng
    if (data.status.key != OrderStatusEnum.Done) {
        let message = 'Yêu cầu đơn hàng phải <strong>hoàn tất</strong>';
        
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
        let message = 'Yêu cầu đơn hàng phải <strong>thanh toán</strong>';
        
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

    // Trường hợp là đơn hàng đầu tiên
    if (controller.orders.length == 0)
        return true;

    //#region Kiểm tra khách hàng
    if (controller.customer.id != data.customer.id) {
        let message = 'Đơn hàng #' + String(data.id) + ' có không cùng khách hàng với các đơn hàng trong bảng';
        
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
        let message = 'Đơn hàng #' + String(data.id) + ' có trạng thái thanh toán <strong>' + data.paymentStatus.value + '</strong> không đồng nhất với các đơn hàng trong bảng';
        
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
        let message = 'Đơn hàng #' + String(data.id) + ' có phương thức thanh toán <strong>' + data.paymentMethod.value + '</strong> không đồng nhất với các đơn hàng trong bảng';
        
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
        let message = 'Đơn hàng #' + String(data.id) + ' có phương thức giao hàng <strong>' + data.deliveryMethod.value + '</strong> không đồng nhất với các đơn hàng trong bảng';
        
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
        let message = 'Đơn hàng #' + String(data.id) + ' có địa chỉ nhận hàng không đồng nhất với các đơn hàng trong bảng';
        
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
        
    return true;
}

function _handleCode() {
    let $txtCode = $("[id$='_txtCode']");

    HoldOn.open();
    controller.getOrderBasic($txtCode.val())
        .then(function (response) {
            HoldOn.close();

            if (!response) {
                let message = 'Mã đơn hàng #' + String(orderId) + ' không tồn tại trong hệ thống';
                
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
            $txtCode.val('');

            // Cập nhật hiển thị button submit
            _updateDisplaySubmit();
        })
        .catch(function (err) {
            HoldOn.close();
            $txtCode.focus();

            swal({
                title: 'Error',
                text: err.responseJSON.message,
                type: 'error',
                showCloseButton: true,
                html: true,
            });
        });
}

function _createOrderHtml(index, data) {
    let html = '';

    html += '<tr data-id="' + String(data.id) + '">';
    html += '    <td>' + String(index) + '</td>';
    html += '    <td><strong>' + String(data.id) + '</strong></td>';
    html += '    <td>';
    html += '    <strong>' + data.customer.nick + '</strong>';
    html += '    <br>' + data.customer.name;
    html += '    <br>' + data.customer.phone;
    html += '    </td>';
    html += '    <td>' + UtilsService.formatNumber(data.quantity) + '</td>';
    html += '    <td><span class="bg-order-status order-status-' + data.status.key + '">' + data.status.value + '</span></td>';
    html += '    <td><span class="bg-payment-status bg-payment-status-' + data.paymentStatus.key + '">' + data.paymentStatus.value + '</span></td>';
    html += '    <td><span class="bg-payment-method bg-payment-method-' + data.paymentMethod.key + '">' + data.paymentMethod.value + '</span></td>';
    html += '    <td><span class="bg-delivery-method bg-delivery-method-' + data.deliveryMethod.key + '">' + data.deliveryMethod.value + '</span></td>';
    html += '    <td>' + UtilsService.formatNumber(data.totalPrice) + '</td>';
    html += '    <td>' + data.staff + '</td>';
    html += '    <td>';
    html += '        <strong>' + stringFormat.datetimeToString(data.createdDate, 'dd/MM') + '</strong>';
    html += '        <br>' + stringFormat.datetimeToString(data.createdDate, 'HH:mm');
    html += '    </td>';
    html += '    <td>';
    html += '        <strong>' + stringFormat.datetimeToString(data.doneDate, 'dd/MM') + '</strong>';
    html += '        <br>' + stringFormat.datetimeToString(data.doneDate, 'HH:mm');
    html += '    </td>';
    html += '    <td>';
    html += '        <a href="javascript:;"';
    html += '           title="Xóa"';
    html += '           class="btn primary-btn btn-red h45-btn"';
    html += '           onclick="onClickOrderRemoval(' + data.id+ ',)"';
    html += '        >';
    html += '            <i class="fa fa-times" aria-hidden="true"></i>';
    html += '        </a>';
    html += '    </td>';
    html += '</tr>';

    return html;
}

function _addOrder(data) {
    let $tbodyOrder = $("#tbody-order");
    let index = $tbodyOrder.find("tr").length + 1;
    let newRowHtml = _createOrderHtml(index, data);

    $tbodyOrder.html(newRowHtml + $tbodyOrder.html());
}

function _removeOrder(orderId) {
    let $tbBody = $("tbody-order")
    let $row = $tbBody.find("[data-id='" + orderId + "']");

    $row.each(function (index, element) { element.remove(); });
    controller.removeOrder(orderId);
}

// Cập nhật lại giá trị index của table
function _updateIndexColumn() {
    let $row = $("tbody-order").find("tr");

    $row.each(function(index, element) {
        element.innerHTML = String($row.length - index);
    })
}

// Cập nhật trạng thái ẩn hiển của button submit
function _updateDisplaySubmit() {
    let $btnSubmit = $("#btnSubmit");

    if (controller.orders.length > 0)
        $btnSubmit.removeClass('hidden');
    else
        $btnSubmit.addClass('hidden');
}
//#endregion

//#region Public
function onKeyDownCode(event) {
    if (event.key == 'Enter') {
        let $txtCode = $("[id$='_txtCode']");
        
        $txtCode.val($txtCode.val().trim());

        if ($txtCode.val())
            _handleCode();
    }
}

function onClickOrderAddition() {
    let $txtCode = $("[id$='_txtCode']");
        
    $txtCode.val($txtCode.val().trim());

    if ($txtCode.val())
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

            swal({
                title: 'Error',
                text: err.responseJSON.message,
                type: 'error',
                showCloseButton: true,
                html: true,
            });
        });
}
//#endregion