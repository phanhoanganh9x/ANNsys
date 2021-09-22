const OrderTypeEnum = {
    "ANN": 1
};

const DeliveryMethodEnum = {
    "Face": 1,
    "GHTK": 6
}

let loading = false;
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
    let shippingCodeDOM = document.querySelector("[id$='_txtShippingCode']");
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

            let deliveryMethodDOM = document.querySelector("[id$='_ddlDeliveryMethod']");
            let shippingCodeDOM = document.querySelector("[id$='_txtShippingCode']");

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

                let message = 'Đơn hàng ' + response.orderType.value + '#' + response.code + ' đã được đăng ký trạng thái ' + response.status.value;

                swal({
                    title: 'Error',
                    text: message,
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                });
            }
            else {
                _updateDeliveryMethod(callback);
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
            });
        });
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
            let deliveryMethodDOM = document.querySelector("[id$='_ddlDeliveryMethod']");

            if (!deliveryMethodDOM.disabled)
                deliveryMethodDOM.focus();
        });

        return false;
    }

    if (!data.shippingCode) {
        let message = 'Bạn chưa nhập mã vận đơn'
        let callback = function () {
            let shippingCodeDOM = document.querySelector("[id$='_txtShippingCode']");

            if (!shippingCodeDOM.disabled)
                shippingCodeDOM.focus();
        };

        if (data.orderType.key == OrderTypeEnum.ANN) {
            message = 'Vui lòng cập nhật thông tin mã vận đơn<br>';
            message += 'Đơn hàng: <a href="/thong-tin-don-hang?id=' + data.code + '" target="_blank"><b>#' + data.code + '</b></a>';
            callback = function () { };
        }

        swal({
            title: 'Error',
            text: message,
            type: 'error',
            showCloseButton: true,
            html: true,
        }, callback);

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
            document.querySelector("[id$='_txtShippingCode']").focus();
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
    html += '    <td>' + data.sentDate + '</td>';
    html += '    <td>';
    html += '        <a href="javascript:;"';
    html += '           title="Xóa"';
    html += '           class="btn primary-btn btn-red h45-btn"';
    html += '           onclick="onClickDeliveryRemoval(' + data.orderType.key + ',' + data.code + ')"';
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
    let orderTypeDOM = document.querySelector("[id$='_ddlOrderType']");

    // Loại đơn mặc định là ANN
    orderTypeDOM.value = OrderTypeEnum.ANN;
    onChangeOrderType(orderTypeDOM.value);

    _updateDelivery();
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

function onBlurCode(callback) {
    let codeDOM = document.querySelector("[id$='_txtCode']");
    let shippingCodeDOM = document.querySelector("[id$='_txtShippingCode']");

    shippingCodeDOM.value = '';

    if (codeDOM.value)
    {
        codeDOM.value = codeDOM.value.trim();

        // Lấy thông tin hình thức giao hàng và mã vận đơn đối với đơn hàng shop ANN
        let orderTypeDOM = document.querySelector("[id$='_ddlOrderType']");

        if (orderTypeDOM.value == OrderTypeEnum.ANN) {
            let orderId = parseInt(codeDOM.value);

            if (orderId)
            {
                controller.delivery.orderType = {
                    key: parseInt(orderTypeDOM.value),
                    value: orderTypeDOM.options[orderTypeDOM.selectedIndex].text
                };
                controller.delivery.code = codeDOM.value;
                _checkDelivery(callback);
            }
            else
                swal({
                    title: 'Error',
                    text: 'Mã đơn hàng shop ANN nhập không đúng.',
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                }, function () {
                    document.querySelector("[id$='_txtCode']").focus();
                });
        }
    }
}

function onKeyUpCode(event) {
    if (event.key == 'Enter') {
        let callback = function () {
            onClickDeliveryAddition();
        };

        onBlurCode(callback);
    }
}

function onChangeDeliveryMethod(selectedValue) {
    let value = +selectedValue || 0;
    let shippingCodeDOM = document.querySelector("[id$='_txtShippingCode']");

    // Trường hợp không lấy được các phương thức vận chuyên
    // Trường hơp lấy trực tiếp
    if (value == 0 || value == DeliveryMethodEnum.Face)
        shippingCodeDOM.disabled = true;
    else
        shippingCodeDOM.disabled = false;

    shippingCodeDOM.value = '';
}

function onBlurShippingCode() {
    let shippingCodeDOM = document.querySelector("[id$='_txtShippingCode']");

    if (shippingCodeDOM.value)
        shippingCodeDOM.value = shippingCodeDOM.value.trim();
}

function onClickDeliveryAddition() {
    _updateDelivery();

    if (!_checkValidation(controller.delivery))
        return;

    _addDelivery(controller.delivery);
    _clearDelivery();
}

function onClickDeliveryRemoval(orderId, code) {
    // Xóa dòng hiện tại
    let row = _findRow(orderId, code);

    row.remove();

    // Cập nhật số thứ tự
    let rows = document.getElementById("tbody-delivery").querySelectorAll("tr");

    for (let i = 0; i < rows.length; i++) {
        let indexDOM = rows[i].querySelector("td");

        indexDOM.innerHTML = String(rows.length - i);
    }
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
            orderType: dataset.orderType,
            code: dataset.code,
            deliveryMethod: dataset.deliveryMethod,
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
                window.location.href = '/quan-ly-giao-hang';
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