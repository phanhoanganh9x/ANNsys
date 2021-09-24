const OrderTypeEnum = {
    "ANN": 1
};

const DeliveryMethodEnum = {
    "Face": 1,
    "GHTK": 6
}

let loading = false;
let stopOnBlurCode = false;
let stringFormat = new StringFormat();
let controller = new DeliveryRegisterController();

document.addEventListener("DOMContentLoaded", function (event) {
    _initDeliveryMethod();
    _initOrderType();
    _updateDelivery();
});

//#region Private
function _initOrderType() {
    let orderTypeDOM = document.querySelector("[id$='_ddlOrderType']");

    orderTypeDOM.value = OrderTypeEnum.ANN;
    onChangeOrderType(orderTypeDOM.value);
}

function _initDeliveryMethod() {
    let deliveryMethodDOM = document.querySelector("[id$='_ddlDeliveryMethod']");

    deliveryMethodDOM.value = 0;
    onChangeDeliveryMethod(deliveryMethodDOM.value);
}

function _updateDelivery() {
    let orderTypeDOM = document.querySelector("[id$='_ddlOrderType']");
    let codeDOM = document.querySelector("[id$='_txtCode']");
    let deliveryMethodDOM = document.querySelector("[id$='_ddlDeliveryMethod']");
    let shippingCodeDOM = document.querySelector("[id$='_hdfShippingCode']");
    let sentDateDOM = document.querySelector("[id$='_rSentDate']");
    let staffDOM = document.querySelector("[id$='_hdfStaff']");
    let data = {
        orderType: {
            key: parseInt(orderTypeDOM.value),
            value: orderTypeDOM.options[orderTypeDOM.selectedIndex].text,
        },
        code: codeDOM.value,
        deliveryMethod: {
            key: parseInt(deliveryMethodDOM.value),
            value: deliveryMethodDOM.options[deliveryMethodDOM.selectedIndex].text
        },
        shippingCode: shippingCodeDOM.value,
        sentDate: sentDateDOM.value,
        staff: staffDOM.value
    };

    controller.setDelivery(data);
}

function _updateDeliveryMethod(callback) {
    if (!loading) {
        HoldOn.open();
        loading = true;
    }

    controller.getDeliveryInfo()
        .then(function (response) {
            if (loading) {
                HoldOn.close();
                loading = false;
            }

            let codeDOM = document.querySelector("[id$='_txtCode']");
            let deliveryMethodDOM = document.querySelector("[id$='_ddlDeliveryMethod']");
            let shippingCodeDOM = document.querySelector("[id$='_hdfShippingCode']");

            codeDOM.value = response.orderId;
            deliveryMethodDOM.value = response.deliveryMethod.key;
            shippingCodeDOM.value = response.shippingCode;

            if (typeof callback === 'function')
                callback();
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
            }, function() {
                if (stopOnBlurCode)
                    stopOnBlurCode = false;
            });
        });
}

function _checkDelivery(callback) {
    HoldOn.open();
    loading = true;

    controller.getDelivery()
        .then(function (response) {
            if (response) {
                if (loading) {
                    HoldOn.close();
                    loading = false;
                }

                let message = '';

                message += 'Đơn hàng ' + response.orderType.value + ' <strong>#' + response.code + '</strong><br>';
                message += 'đã được đăng ký trạng thái <strong>"' + response.status.value + '"</strong>';
                swal({
                    title: 'Error',
                    text: message,
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                }, function () {
                    if (stopOnBlurCode)
                        stopOnBlurCode = false;
                });
            }
            else {
                if (controller.delivery.orderType.key == OrderTypeEnum.ANN)
                    _updateDeliveryMethod(callback);
                else {
                    if (loading) {
                        HoldOn.close();
                        loading = false;
                    }

                    if (typeof callback === 'function')
                        callback();
                }
            }
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
            }, function () {
                if (stopOnBlurCode)
                    stopOnBlurCode = false;
            });
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
    let row = tbBodyDOM.querySelector("[data-order-type='" + orderId + "'][data-code='" + code + "']");

    return row;
}

function _checkValidation(data) {
    if (!data.code) {
        swal({
            title: 'Error',
            text: 'Bạn chưa nhập mã đơn hàng',
            type: 'error',
            showCloseButton: true,
            html: true,
        }, function () {
            if (stopOnBlurCode)
                stopOnBlurCode = false;

            document.querySelector("[id$='_txtCode']").focus();
        });

        return false;
    }

    if (!data.deliveryMethod.key) {
        swal({
            title: 'Error',
            text: 'Bạn chưa chọn kiểu vận chuyển',
            type: 'error',
            showCloseButton: true,
            html: true,
        }, function () {
            if (stopOnBlurCode)
                stopOnBlurCode = false;

            let deliveryMethodDOM = document.querySelector("[id$='_ddlDeliveryMethod']");

            if (!deliveryMethodDOM.disabled)
                deliveryMethodDOM.focus();
        });

        return false;
    }

    let row = _findRow(data.orderType.key, data.code);

    if (row)
    {
        let message = data.orderType.value + ' #' + data.code + ' đã được thêm vào rồi';

        swal({
            title: 'Error',
            text: message,
            type: 'error',
            showCloseButton: true,
            html: true,
        }, function () {
            if (stopOnBlurCode)
                stopOnBlurCode = false;

            document.querySelector("[id$='_txtCode']").focus();
        });

        return false;
    }

    return true;
}

function _createDeliveryHtml(index, data) {
    let html = '';

    html += '<tr';
    html += '    data-order-type="' + String(data.orderType.key) + '"';
    html += '    data-code="' + data.code + '"';
    html += '    data-delivery-method="' + String(data.deliveryMethod.key) + '"';
    html += '    data-shipping-code="' + data.shippingCode + '"';
    html += '    data-sent-date="' + data.sentDate + '"';
    html += '    data-staff="' + data.staff + '"';
    html += '>';
    html += '    <td>' + String(index) + '</td>';
    html += '    <td>' + data.orderType.value + '</td>';
    html += '    <td>' + data.code + '</td>';
    html += '    <td>' + data.deliveryMethod.value + '</td>';
    html += '    <td>' + data.shippingCode + '</td>';
    html += '    <td>' + stringFormat.datetimeToString(data.sentDate, 'dd/MM/yyyy') + '</td>';
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

    return html;
}

function _addDelivery(data) {
    let tbodyDeliveryDOM = document.getElementById("tbody-delivery");
    let index = tbodyDeliveryDOM.querySelectorAll("tr").length + 1;
    let newRowHtml = _createDeliveryHtml(index, data);

    tbodyDeliveryDOM.innerHTML = newRowHtml + tbodyDeliveryDOM.innerHTML;
}

function _clearDelivery() {
    let codeDOM = document.querySelector("[id$='_txtCode']");
    let shippingCodeDOM = document.querySelector("[id$='_hdfShippingCode']");

    codeDOM.value = '';
    shippingCodeDOM.value = '';
    _updateDelivery();
}

function _removeDelivery(orderId, code) {
    let row = _findRow(orderId, code);

    row.remove();
}

function _updateIndexColumn() {
    let rows = document.getElementById("tbody-delivery").querySelectorAll("tr");

    for (let i = 0; i < rows.length; i++) {
        let indexDOM = rows[i].querySelector("td");

        indexDOM.innerHTML = String(rows.length - i);
    }
}

function _updateDisplaySubmit() {
    let btnSubmitDOM = document.getElementById("btnSubmit");
    let rowsDOM = document.getElementById("tbody-delivery").querySelectorAll("tr");

    if (rowsDOM.length > 0)
        btnSubmitDOM.classList.remove('hidden');
    else
        btnSubmitDOM.classList.add('hidden');
}
//#endregion

//#region Public
function onChangeOrderType(selectedValue) {
    let value = +selectedValue || 0;
    let codeDOM = document.querySelector("[id$='_txtCode']");
    let deliveryMethodDOM = document.querySelector("[id$='_ddlDeliveryMethod']");

    // Trường hợp đơn hàng là ANN Shop
    if (value == OrderTypeEnum.ANN) {
        deliveryMethodDOM.value = 0
        deliveryMethodDOM.disabled = true;
        onChangeDeliveryMethod(deliveryMethodDOM.value);
    }
    else {
        deliveryMethodDOM.disabled = false;

        if (deliveryMethodDOM.value == 0)
        {
            deliveryMethodDOM.value = DeliveryMethodEnum.GHTK;
            onChangeDeliveryMethod(deliveryMethodDOM.value);
        }
    }

    codeDOM.value = '';
}

function onBlurCode() {
    if (stopOnBlurCode)
        return;

    let codeDOM = document.querySelector("[id$='_txtCode']");
    codeDOM.value = codeDOM.value.trim();

    if (codeDOM.value)
        _handleCode();
}

function onKeyUpCode(event) {
    if (event.key == 'Enter') {
        let codeDOM = document.querySelector("[id$='_txtCode']");
        codeDOM.value = codeDOM.value.trim();

        if (codeDOM.value) {
            let callback = function () {
                onClickDeliveryAddition();

                if (stopOnBlurCode)
                    stopOnBlurCode = false;
            };

            stopOnBlurCode = true;
            _handleCode(callback);
        }
    }
}

function onChangeDeliveryMethod(selectedValue) {
    let value = +selectedValue || 0;
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
    let rows = document.getElementById("tbody-delivery").querySelectorAll("tr");

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
                text: 'Đăng ký danh sách đơn gửi đi.<br><strong>Thành công!</strong>',
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