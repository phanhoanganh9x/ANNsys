const OrderTypeEnum = {
    "ANN": 1
};

const DeliveryMethodEnum = {
    "Face": 1,
    "GHTK": 6
}

let loading = false;
let stringFormat = new StringFormat();
let controller = new DeliveryRegisterController();

document.addEventListener("DOMContentLoaded", function (event) {
    _initOrderType();
    _updateDelivery();
});

//#region Private
function _initOrderType() {
    onChangeOrderType();
}

function _initDeliveryMethod() {
    let $deliveryMethod = $("#ddlDeliveryMethod");

    // Cài đặt giá trị ban đầu
    $deliveryMethod.val(null).trigger('change');

    // Cài đặt API
    let url = '/api/v1/delivery/methods/select2';

    if (controller.delivery.orderType.key)
        url += '?orderTypeId=' + controller.delivery.orderType.key;

    $deliveryMethod.select2({
        placeholder: 'Kiểu giao hàng',
        minimumResultsForSearch: Infinity,
        ajax: {
            method: 'GET',
            url: url,
        },
        width: '100%'
    });

    // trigger onchange
    onChangeDeliveryMethod();
}

function _updateDelivery() {
    //#region Thông tin loại đơn hàng
    let orderTypeDOM = document.querySelector("[id$='_ddlOrderType']");
    let orderType = {
        key: parseInt(orderTypeDOM.value),
        value: orderTypeDOM.options[orderTypeDOM.selectedIndex].text,
    };
    //#endregion

    // Mã đơn hàng hoặc mã vận đơn
    let codeDOM = document.querySelector("[id$='_txtCode']");

    //#region Thông tin phương thức giao hàng
    let $deliveryMethod = $("#ddlDeliveryMethod");
    let deliveryMethod = {
        key: parseInt($deliveryMethod.val()),
        value: $deliveryMethod.find("option:selected").text()
    };
    //#endregion

    // Mã vận đơn
    let shippingCodeDOM = document.querySelector("[id$='_hdfShippingCode']");

    //#region Ngày gửi hàng
    let sentDateDOM = document.querySelector("[id$='_rSentDate']");
    let sentDateTime = '';

    if (sentDateDOM.value) {
        let sentDate = sentDateDOM.value.substring(0, 10);
        let sentTime = sentDateDOM.value.substring(11).replace(/-/g, ':');

        sentDateTime = sentDate + ' ' + sentTime;
    }
    //#endregion

    // Nhân viên tạo đơn
    let staffDOM = document.querySelector("[id$='_hdfStaff']");

    controller.setDelivery({
        orderType: orderType,
        code: codeDOM.value,
        deliveryMethod: deliveryMethod,
        shippingCode: shippingCodeDOM.value,
        sentDate: sentDateTime,
        staff: staffDOM.value
    });
}

function _updateDeliveryMethod(callback) {
    controller.getDeliveryInfo()
        .then(function (response) {
            // Mã đơn hàng
            let codeDOM = document.querySelector("[id$='_txtCode']");

            codeDOM.value = response.orderId;

            // Phương thức vận chuyển
            let $deliveryMethod = $("#ddlDeliveryMethod");
            let newOption = new Option(response.deliveryMethod.value, response.deliveryMethod.key, false, false);

            $deliveryMethod.find("option").remove();
            $deliveryMethod.append(newOption).trigger('change');

            // Mã vận đơn
            let shippingCodeDOM = document.querySelector("[id$='_hdfShippingCode']");

            shippingCodeDOM.value = response.shippingCode;

            // Thực thi callback
            if (typeof callback === 'function')
                callback();
        })
        .catch(function (err) {
            controller.delivery.error = err.responseJSON.message;

            if (typeof callback === 'function')
                callback();
        });
}

function _checkDelivery(callback) {
    controller.getDelivery()
        .then(function (response) {
            if (response) {
                let message = '';

                message += 'Đơn hàng ' + response.orderType.value + ' <strong>#' + response.code + '</strong> ';
                message += 'đã cập nhật trạng thái <strong>"' + response.status.value + '"</strong>';

                controller.delivery.error = message;

                if (typeof callback === 'function')
                    callback();
            }
            else {
                if (controller.delivery.orderType.key == OrderTypeEnum.ANN)
                    _updateDeliveryMethod(callback);
                else {
                    if (typeof callback === 'function')
                        callback();
                }
            }
        });
}

function _handleCode(callback) {
    let orderTypeDOM = document.querySelector("[id$='_ddlOrderType']");
    let codeDOM = document.querySelector("[id$='_txtCode']");
    let shippingCodeDOM = document.querySelector("[id$='_hdfShippingCode']");

    shippingCodeDOM.value = '';
    controller.delivery.orderType = {
        key: parseInt(orderTypeDOM.value),
        value: orderTypeDOM.options[orderTypeDOM.selectedIndex].text
    };
    controller.delivery.code = codeDOM.value;
    _checkDelivery(callback);
}

function _findRow(orderId, code) {
    let tbBodyDOM = document.getElementById("tbody-delivery");
    let rows = tbBodyDOM.querySelectorAll("[data-order-type='" + orderId + "'][data-code='" + code + "']");

    return rows;
}

function _checkValidation(data) {
    if (!data.code) {
        document.querySelector("[id$='_txtCode']").focus();
        document.querySelector("[id$='_txtCode']").select();

        return false;
    }

    let rows = _findRow(data.orderType.key, data.code);

    if (rows.length > 0) {
        document.querySelector("[id$='_txtCode']").focus();
        document.querySelector("[id$='_txtCode']").select();

        return false;
    }

    if (!controller.delivery.error)
        if (!data.deliveryMethod.key)
            controller.delivery.error = 'Kiểu vận chuyển chưa được chọn';

    return true;
}

function _createDeliveryHtml(index, data) {
    let html = '';

    html += '<tr class="data"';
    html += '    data-order-type="' + String(data.orderType.key) + '"';
    html += '    data-code="' + data.code + '"';
    html += '    data-delivery-method="' + String(data.deliveryMethod.key) + '"';
    html += '    data-shipping-code="' + data.shippingCode + '"';
    html += '    data-sent-date="' + data.sentDate + '"';
    html += '    data-staff="' + data.staff + '"';
    if (data.error)
        html += '    data-error="' + 1 + '"';
    else
        html += '    data-error="' + 0 + '"';
    html += '>';
    html += '    <td>' + String(index) + '</td>';
    html += '    <td><span class="bg-order-type bg-order-type-' + data.orderType.key + '">' + data.orderType.value + '</span></td>';
    html += '    <td>' + data.code + '</td>';
    html += '    <td><span class="bg-delivery-type bg-delivery-type-' + data.deliveryMethod.key + '">' + data.deliveryMethod.value + '</span></td>';
    html += '    <td>' + data.shippingCode + '</td>';
    html += '    <td>' + stringFormat.datetimeToString(data.sentDate, 'dd/MM/yyyy HH:mm') + '</td>';
    html += '    <td>';
    html += '        <a href="javascript:;"';
    html += '           title="Xóa"';
    html += '           class="btn primary-btn btn-red h45-btn"';
    html += '           onclick="onClickDeliveryRemoval(' + data.orderType.key + ',`' + data.code + '`)"';
    html += '        >';
    html += '            <i class="fa fa-times" aria-hidden="true"></i>';
    html += '        </a>';
    html += '    </td>';
    html += '</tr>';

    if (data.error) {
        html += '<tr class="error"';
        html += '    data-order-type="' + String(data.orderType.key) + '"';
        html += '    data-code="' + data.code + '"';
        html += '>';
        html += '    <td colspan="7">';
        html += '        <span class="bg-red"><strong>Lỗi:</strong> '
        html += '            ' + data.error
        html += '        </span>'
        html += '    </td>'
        html += '</tr>';
    }

    return html;
}

function _addDelivery(data) {
    let tbodyDeliveryDOM = document.getElementById("tbody-delivery");
    let index = tbodyDeliveryDOM.querySelectorAll("tr.data").length + 1;
    let newRowHtml = _createDeliveryHtml(index, data);

    tbodyDeliveryDOM.innerHTML = newRowHtml + tbodyDeliveryDOM.innerHTML;
}

function _clearDelivery() {
    let codeDOM = document.querySelector("[id$='_txtCode']");
    let shippingCodeDOM = document.querySelector("[id$='_hdfShippingCode']");

    codeDOM.value = '';
    shippingCodeDOM.value = '';
    controller.delivery.error = null;
    _updateDelivery();
}

function _removeDelivery(orderId, code) {
    let rows = _findRow(orderId, code);

    rows.forEach(function (item) { item.remove(); });
}

function _updateIndexColumn() {
    let rows = document.getElementById("tbody-delivery").querySelectorAll("tr.data");

    for (let i = 0; i < rows.length; i++) {
        let indexDOM = rows[i].querySelector("td");

        indexDOM.innerHTML = String(rows.length - i);
    }
}

function _updateDisplaySubmit() {
    let btnSubmitDOM = document.getElementById("btnSubmit");
    let rowsDOM = document.getElementById("tbody-delivery").querySelectorAll("tr.data");

    if (rowsDOM.length > 0)
        btnSubmitDOM.classList.remove('hidden');
    else
        btnSubmitDOM.classList.add('hidden');
}
//#endregion

//#region Public
function onChangeOrderType() {
    //#region Loại đơn hàng
    let orderTypeDOM = document.querySelector("[id$='_ddlOrderType']");
    let key = parseInt(orderTypeDOM.value);
    let value = orderTypeDOM.options[orderTypeDOM.selectedIndex].text;

    controller.delivery.orderType = {
        key: parseInt(key),
        value: value
    };
    //#endregion

    //#region Mã đơn hàng
    let codeDOM = document.querySelector("[id$='_txtCode']");

    codeDOM.value = '';
    //#endregion

    //#region Phương thức vận chuyển
    let $deliveryMethod = $("#ddlDeliveryMethod");

    _initDeliveryMethod();

    if (key == OrderTypeEnum.ANN) {
        $deliveryMethod.attr('disabled', true);
        $deliveryMethod.attr('readonly', 'readonly');

        codeDOM.focus();
    }
    else {
        $deliveryMethod.removeAttr('disabled');
        $deliveryMethod.removeAttr('readonly');

        $deliveryMethod.select2('open');
    }
    //#endregion
}

function onKeyUpCode(event) {
    if (event.key == 'Enter') {
        let codeDOM = document.querySelector("[id$='_txtCode']");
        codeDOM.value = codeDOM.value.trim();

        if (codeDOM.value) {
            let callback = function () {
                onClickDeliveryAddition();
            };

            _handleCode(callback);
        }
    }
}

function onChangeDeliveryMethod() {
    let shippingCodeDOM = document.querySelector("[id$='_hdfShippingCode']");

    shippingCodeDOM.value = '';
}

function onClickDeliveryAddition() {
    // Cập nhật thông tin đăng ký gửi hàng
    _updateDelivery();

    // Kiểm tra thông tin
    if (!_checkValidation(controller.delivery))
        return;

    // Cập nhật thông tin vào bảng đăng ký
    _addDelivery(controller.delivery);
    _clearDelivery();

    // Cập nhật hiển thị button submit
    _updateDisplaySubmit();
}

function onClickDeliveryRemoval(orderId, code) {
    // Xóa dòng hiện tại
    _removeDelivery(orderId, code);

    // Cập nhật số thứ tự
    _updateIndexColumn();

    // Cập nhật hiển thị button submit
    _updateDisplaySubmit();
}

function submitDeliveries() {
    //#region Kiểm tra xem có đơn giao hàng nào đăng ký chưa
    let rows = document.getElementById("tbody-delivery").querySelectorAll("tr.data[data-error='0']");

    if (rows.length == 0)
        return;
    //#endregion

    //#region Tổng hợp dữ liệu đơn hàng sẽ đăng ký
    let deliveries = [];

    for (let i = 0; i < rows.length; i++) {
        let dataset = rows[i].dataset;
        let data = {
            orderType: parseInt(dataset.orderType),
            code: dataset.code,
            deliveryMethod: parseInt(dataset.deliveryMethod),
            shippingCode: dataset.shippingCode,
            sentDate: dataset.sentDate,
            staff: dataset.staff
        }

        deliveries.push(data);
    }
    //#endregion

    //#region Đăng ký đơn giao hàng
    HoldOn.open();
    loading = true;

    controller.registerDeliveries(deliveries)
        .then(function (response) {
            if (loading) {
                HoldOn.close();
                loading = false;
            }

            swal({
                title: 'Thông báo',
                text: 'Thêm đơn hàng gửi đi thành công!',
                type: 'success',
                showCloseButton: true,
                html: true,
            }, function () {
                window.location.href = '/quan-ly-don-giao-hang';
            });
        })
        .catch(function (err) {
            if (loading) {
                HoldOn.close();
                loading = false;
            }

            swal({
                title: 'Error',
                text: err.responseJSON.message,
                type: 'error',
                showCloseButton: true,
                html: true,
            });
        });
    //#endregion
}
//#endregion