﻿let strFormat = new StringFormat();
let orderService = new OrderService();
let controller = new PreOrderController();

let loading = false;

document.addEventListener('DOMContentLoaded', function (event) {
    _initRole();
    _initQueryParams();
    _initPage();
});

// #region Private
function _initRole() {
    let roleDOM = document.querySelector("[id$='_hdRole']");

    if (roleDOM.value == '0')
        controller.role = 0;
    else if (roleDOM.value)
        controller.role = +roleDOM.value || null;
}

function _initQueryParams() {
    let preOrderIdError = false;
    let message = null;
    let search = window.location.search;

    controller.setPreOrderId(search);

    if (controller.preOrderId == null) {
        preOrderIdError = true;
        message = 'Chưa truyền ID của đơn đặt hàng';
    }
    else if (controller.preOrderId <= 0) {
        preOrderIdError = true;
        message = 'Đơn đặt hàng có ID #' + controller.preOrderId + ' không đúng.';
    }

    if (preOrderIdError)
        return swal({
            title: 'Error',
            text: message,
            type: 'error',
            showCloseButton: true,
            html: true,
        }, function () {
            let url = window.location.origin + '/danh-sach-don-dat-hang';
            window.location.replace(url);
        });
}

function _initHeader(data) {
    // Tiêu đề đơn hàng
    let pageTitleDOM = document.getElementById('pageTitle');
    let pageTitle = 'Đơn đặt hàng ' + data.preOrderId;

    if (data.customer && data.customer.name) {
        document.title = strFormat.toTitleCase(data.customer.name) + ' - Đơn đặt hàng ';
        pageTitle += ' - ' + strFormat.toTitleCase(data.customer.name);
    }
    else if (data.address) {
        document.title = strFormat.toTitleCase(data.address.fullName) + ' - Đơn đặt hàng ';
        pageTitle += ' - ' + strFormat.toTitleCase(data.address.fullName);
    }

    pageTitleDOM.innerText = pageTitle

    // Thể loại đơn hàng
    if (data.type) {
        let orderTypeDOM = document.getElementById('orderType');

        orderTypeDOM.innerText = data.type.value;
    }

    // Nhân chịu trách nhiệm đơn hàng
    if (data.customer && data.customer.staff) {
        let staffDOM = document.getElementById('createBy');

        staffDOM.innerText = data.customer.staff;
    }

    // Thời điểm tạo đơn hàng
    if (data.createdDate) {
        let createdDateDOM = document.getElementById('createDate');

        createdDateDOM.innerText = strFormat.datetimeToString(data.createdDate, 'dd/MM/yyyy hh:mm:ss tt');
    }

    // Số lượng sản phẩm
    if (data.totalQuantity) {
        let totalQuantityDOM = document.getElementById('totalQuantityHeader');

        totalQuantityDOM.innerText = UtilsService.formatThousands(data.totalQuantity, ',');
    }

    // Tổng tiền
    if (data.total) {
        let totalHeaderDOM = document.getElementById('totalHeader');

        totalHeaderDOM.innerText = UtilsService.formatThousands(data.total, ',');
    }

    // Trạng thái dơn hàng
    if (data.status) {
        let orderStatusDOM = document.getElementById('orderStatus');

        orderStatusDOM.innerText = data.status.value;
    }
}

function _initCustomer(customer) {
    // ID khách hàng
    if (customer.id) {
        let hdfCustomerDOM = document.querySelector('[id$="_hdfCustomer"]');

        hdfCustomerDOM.value = customer.id;
    }

    // Họ tên khách hàng
    if (customer.name) {
        let fullNameDOM = document.getElementById('txtFullname');

        fullNameDOM.value = strFormat.toTitleCase(customer.name);
    }

    // Só điện thoại
    if (customer.phone) {
        let phoneDOM = document.getElementById('txtPhone');

        phoneDOM.value = customer.phone;
    }

    // Nick đặt hàng
    if (customer.nick) {
        let nickDOM = document.getElementById('txtNick');

        nickDOM.value = customer.nick;
    }

    // Facebook
    let facebookHTML = '';
    let facebookDOM = document.getElementById('divFacebook');

    if (customer.facebook) {
        if (customer.facebook.match(/^https:\/\/www.facebook.com/g))
        {
            facebookHTML += '<div class="col-md-9 fb">';
            facebookHTML += '    <input type="text" class="form-control capitalize" value="' + customer.facebook + '" disabled="disabled"/>';
            facebookHTML += '</div>';
            facebookHTML += '<div class="col-md-3">';
            facebookHTML += '    <div class="row">';
            facebookHTML += '        <span class="link-facebook">';
            facebookHTML += '            <a class="btn primary-btn fw-btn not-fullwidth" href="' + customer.facebook + '" target="_blank">Xem</a>';
            facebookHTML += '        </span>';
            facebookHTML += '    </div>';
            facebookHTML += '</div>';
        }
        else
        {
            facebookHTML += '<div class="col-md-12 fb">';
            facebookHTML += '    <input type="text" class="form-control capitalize" value="' + customer.facebook + '" disabled="disabled"/>';
            facebookHTML += '</div>';
        }
    }
    else {
        facebookHTML += '<div class="col-md-12 fb">';
        facebookHTML += '    <input type="text" class="form-control capitalize" placeholder="Đường link chat Facebook" disabled="disabled"/>';
        facebookHTML += '</div>';
    }

    facebookDOM.innerHTML = facebookHTML;

    // Customer View
    let customerViewHTML = '';
    let customerViewDOM = document.getElementById('divCustomerView');

    if (customer.registeredApp)
        customerViewHTML += '<strong class="font-green">Đã đăng ký App</strong>';
    else
        customerViewHTML += '<strong class="font-red">Chưa đăng ký App</strong>';

    customerViewDOM.innerHTML = customerViewHTML;

    // Discount Info
    if (customer.discount && customer.discount.amount > 0) {
        let discountInfoHTML = '';
        let discountInfoDOM = document.getElementById('discountInfo');

        discountInfoHTML += '<br>';
        discountInfoHTML += '<strong>* Chiết khấu của khách: ' + UtilsService.formatThousands(customer.discount.amount, ',') + '/cái (đơn từ ' + customer.discount.quantity + ' cái).</strong>'

        discountInfoDOM.innerHTML = discountInfoHTML;
    }
}

function _initDeliveryAddress(deliveryAddress) {
    // Họ tên khách hàng
    if (deliveryAddress.fullName) {
        let fullNameDOM = document.getElementById('txtRecipientFullName');

        fullNameDOM.value = strFormat.toTitleCase(deliveryAddress.fullName);
    }

    // Só điện thoại
    if (deliveryAddress.phone) {
        let phoneDOM = document.getElementById('txtRecipientPhone');

        phoneDOM.value = deliveryAddress.phone;
    }

    // Thông tin địa chỉ khách hàng
    // Tỉnh thành
    if (deliveryAddress.provinceId) {
        let ddlProvinceDOM = document.getElementById('ddlRecipientProvince');

        ddlProvinceDOM.innerHTML = '<option value="' + deliveryAddress.provinceId + '" title="' + deliveryAddress.provinceName + '">' + deliveryAddress.provinceName + '</option>';

        // Quận huyện
        if (deliveryAddress.districtId) {
            let ddlDistrictDOM = document.getElementById('ddlRecipientDistrict');

            ddlDistrictDOM.innerHTML = '<option value="' + deliveryAddress.districtId + '" title="' + deliveryAddress.districtName + '">' + deliveryAddress.districtName + '</option>';

            // Phường xã
            if (deliveryAddress.wardId) {
                let ddlWardDOM = document.getElementById('ddlRecipientWard');

                ddlWardDOM.innerHTML = '<option value="' + deliveryAddress.wardId + '" title="' + deliveryAddress.wardName + '">' + deliveryAddress.wardName + '</option>';
            }
        }
    }

    // Địa chỉ
    if (deliveryAddress.address) {
        let addressDOM = document.getElementById('txtRecipientAddress');

        addressDOM.value = deliveryAddress.address
    }
}

function _generatePreOrderDetailHTML(item, index) {
    let html = '';

    html += '<tr>';
    html += '    <td class="order-item">' + index + '</td>';
    html += '    <td class="image-item">';
    html += '        <img src="' + item.avatar + '">';
    html += '    </td>';
    html += '    <td class="name-item">';
    html += '        <a href="javascript:;">' + item.name + '</a>';
    html += '    </td>';
    html += '    <td class="sku-item">';
    html += '        ' + item.sku;
    html += '    </td>';
    html += '    <td class="variable-item">';
    if (item.color)
        html += '        Màu:&nbsp;' + item.color + '<br>';
    if (item.size)
        html += '        Size:&nbsp;' + item.size + '<br>';
    html += '    </td>';
    html += '    <td class="price-item gia-san-pham">';
    html += '        ' + UtilsService.formatThousands(+item.price || 0, ',');
    html += '    </td>';
    html += '    <td class="discount-item">';
    html += '        ' + UtilsService.formatThousands(+item.discount || 0, ',');
    html += '    </td>';
    html += '    <td class="quantity-item soluong">';
    html += '        ' + UtilsService.formatThousands(+item.stock || 0, ',');
    html += '    </td>';
    html += '    <td class="quantity-item">';
    html += '        ' + UtilsService.formatThousands(+item.quantity || 0, ',');
    html += '    </td>';
    html += '    <td class="total-item totalprice-view">';
    html += '        ' + UtilsService.formatThousands(+item.total || 0, ',');
    html += '    </td>';
    html += '</tr>';

    return html;
}

function _initBody(details) {
    let preOrderTableDOM = document.getElementById('preOrderTable');

    preOrderTableDOM.innerHTML = '';

    for (let i = 0; i < details.length; i++) {
        let item = details[i];
        let index = i + 1;
        let preOrderDetailHTML = _generatePreOrderDetailHTML(item, index);

        preOrderTableDOM.innerHTML += preOrderDetailHTML;
    }
}

function _initFooter(data) {
    // Số lượng
    let quantity = +data.totalQuantity || 0;
    let totalQuantityFooterDOM = document.getElementById('totalQuantityFooter');

    totalQuantityFooterDOM.innerText = UtilsService.formatThousands(quantity, ',') + ' cái';

    // Thành tiền
    let totalPrice = +data.totalPrice || 0;
    let totalPriceDOM = document.getElementById('totalPrice');

    totalPriceDOM.innerText = UtilsService.formatThousands(totalPrice, ',');

    // Chiết khấu
    let totalDiscount = +data.totalDiscount || 0;
    let discountDOM = document.getElementById('discount');

    if (totalDiscount > 0)
        discountDOM.innerText = '-' + UtilsService.formatThousands(totalDiscount, ',');
    else
        discountDOM.innerText = UtilsService.formatThousands(totalDiscount, ',');

    // Sau chiết khấu
    let totalAfterDiscount = totalPrice - totalDiscount;
    let totalAfterDiscountDOM = document.getElementById('totalAfterDiscount');

    totalAfterDiscountDOM.innerText = UtilsService.formatThousands(totalAfterDiscount, ',');

    // Phí vận chuyển
    let feeship = +data.shipFee || 0;
    let feeShipDOM = document.getElementById('feeShip');

    feeShipDOM.innerText = UtilsService.formatThousands(feeship, ',');

    // Mã khuyến mãi
    if (data.coupon) {
        let couponValue = +data.coupon.value || 0;
        let couponDOM = document.getElementById('coupon');

        couponDOM.innerHTML = '<i class="fa fa-gift"></i>&nbsp;' + data.coupon.code + ': -' + UtilsService.formatThousands(couponValue, ',')
    }

    // Tổng tiền
    let total = +data.total || 0;
    let preOrderIdDOM = document.getElementById('preOrderId');
    let totalFooterDOM = document.getElementById('totalFooter');

    preOrderIdDOM.innerText = data.preOrderId
    totalFooterDOM.innerText = UtilsService.formatThousands(total, ',')

    // Ghi chú
    if (data.note) {
        let noteDOM = document.querySelector("[id$='_txtOrderNote']");

        noteDOM.value = data.note;
    }
}

function _initPreOrderStatus(data) {
    // Trạng thái đơn hàng
    if (data.status) {
        let statusDOM = document.getElementById('ddlExcuteStatus');

        statusDOM.innerHTML = '<option value="' + data.status.key + '" title="' + data.status.value + '">' + data.status.value + '</option>'
    }

    // Phương thức thanh toán
    if (data.paymentMethod) {
        let paymentMethodDOM = document.getElementById('ddlPaymentType');

        paymentMethodDOM.innerHTML = '<option value="' + data.paymentMethod.key + '" title="' + data.paymentMethod.value + '">' + data.paymentMethod.value + '</option>'
    }

    // Phương thức thanh toán
    if (data.deliveryMethod) {
        let deliveryMethodDOM = document.getElementById('ddlShippingType');

        deliveryMethodDOM.innerHTML = '<option value="' + data.deliveryMethod.key + '" title="' + data.deliveryMethod.value + '">' + data.deliveryMethod.value + '</option>'
    }

    // Nhân viên phụ trách
    if (data.customer && data.customer.staff) {
        let staffDOM = document
            .querySelector('[id$="_ddlCreatedBy"]')
            .querySelector('option[value="' + data.customer.staff + '"]');

        if (staffDOM)
            staffDOM.selected = true;
    }
}

function _initBtnSubmit(role, preOrderStatus) {
    let btnCreateOrderDOM = document.querySelectorAll('.btn-create-order');
    let btnCancelPrOrderDOM = document.querySelectorAll('.btn-cancel-preorder');
    let btnRecoveryPrOrderDOM = document.querySelectorAll('.btn-recovery-preorder');

    btnCreateOrderDOM.forEach(function (element) {
        if (role == 4)
            element.classList.add('hidden');
        else if (preOrderStatus == 0)
            element.classList.remove('hidden');
    });

    btnCancelPrOrderDOM.forEach(function (element) {
        if (role == 4)
            element.classList.add('hidden');
        else if (preOrderStatus == 0)
            element.classList.remove('hidden');
    });

    btnRecoveryPrOrderDOM.forEach(function (element) {
        if (role == 4)
            element.classList.add('hidden');
        else if (preOrderStatus == 3)
            element.classList.remove('hidden');
    });
}

function _initPage() {
    HoldOn.open();

    controller.getPreOrder()
        .then(function (data) {
            if (!data)
                return swal({
                    title: 'error',
                    text: 'Đơn đặt hàng #' + controller.preOrderId + ' không tồn tại trong hệ thống',
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                }, function () {
                    let url = window.location.origin + '/danh-sach-don-dat-hang';
                    window.location.replace(url);
                });

            // Kiểm tra xem đơn hàng này có được phép xem không
            if (data.customer && data.customer.staff) {
                if (controller.role != 0) {
                    let staffDOM = document.querySelector('[id$="_ddlCreatedBy"]');

                    if (staffDOM.value != data.customer.staff)
                        return swal({
                            title: 'error',
                            text: 'Đơn đặt hàng #' + controller.preOrderId + ' là của nhân viên #' + data.customer.staff,
                            type: 'error',
                            showCloseButton: true,
                            html: true,
                        }, function () {
                            let url = window.location.origin + '/danh-sach-don-dat-hang';
                            window.location.replace(url);
                        });
                }
            }

            _initHeader(data);
            if (data.customer)
                _initCustomer(data.customer);
            if (data.deliveryAddress)
                _initDeliveryAddress(data.deliveryAddress);
            if (data.details && data.details.length > 0)
                _initBody(data.details);
            _initFooter(data);
            _initPreOrderStatus(data);
            _initBtnSubmit(data.role, data.status.key);

            HoldOn.close();
        })
        .catch(function (e) {
            HoldOn.close();
            console.log(e);

            return swal({
                title: 'Error',
                text: 'Đã có lỗi xảy ra trong quá trình lấy thông tin đơn đặt hàng #' + controller.preOrderId + '.',
                type: 'error',
                showCloseButton: true,
                html: true,
            }, function () {
                let url = window.location.origin + '/danh-sach-don-dat-hang';
                window.location.replace(url);
            });
        });
}

function _createOrder(staff) {
    if (!loading) {
        HoldOn.open();
        loading = true;
    }

    controller.createOrder(staff)
        .then(function (response) {
            if (loading) {
                HoldOn.close();
                loading = false;
            }

            if (response.success)
                swal({
                    title: 'Thành Công',
                    text: 'Đơn đặt hàng #' + controller.preOrderId + ' này đã được chuyển thành đơn hàng #' + response.data + '.',
                    type: 'success',
                    showCloseButton: true,
                    html: true,
                }, function () {
                    let url = window.location.origin + '/thong-tin-don-hang?id=' + response.data;
                    window.location.replace(url);
                });
            else {
                swal({
                    title: 'Error',
                    text: response.message,
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                });
            }
        })
        .catch(function (e) {
            console.log(e);

            if (loading) {
                HoldOn.close();
                loading = false;
            }

            if (e.status == 400)
                swal({
                    title: 'Error',
                    text: e.responseJSON.message,
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                });
            else
                swal({
                    title: 'Error',
                    text: 'Đã có lỗi xảy ra trong quá trình lấy thông tin tạo đơn hàng.',
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                });
        });
};
// #endregion

// #region Public
function createOrder() {
    // #region Kiểm tra thông tin nhân viên xử lý đơn
    let staffDOM = document.querySelector('[id$="_ddlCreatedBy"]');

    if (!staffDOM.value) {
        swal({
            title: 'Error',
            text: 'Vui lòng chọn nhân viên xử lý đơn hàng',
            type: 'error',
            showCloseButton: true,
            html: true,
        });
        return;
    }
    // #endregion

    // #region Kiểm thông tin đơn củ của khách hàng
    let hdfCustomerDOM = document.querySelector('[id$="_hdfCustomer"]');
    let customerId = parseInt(hdfCustomerDOM.value) || 0;

    if (customerId) {
        if (!loading) {
            HoldOn.open();
            loading = true;
        }

        controller.checkOldOrder(customerId)
            .then(function (response) {
                if (loading) {
                    HoldOn.close();
                    loading = false;
                }

                if (response)
                {
                    let data = JSON.parse(response);

                    // Đơn hàng
                    let $txtOrder = $("#txtOrder");
                    let $btnOpenOrder = $("#btnOpenOrder");
                    // Đơn hàng đổi trả
                    let $txtRefundGoods = $("#txtRefundGoods");
                    let $btnOpenRefundGoods = $("#btnOpenRefundGoods");
                    // Button đóng modal
                    let $btnCloseOrderOld = $("#btnCloseOrderOld");
                    let show = 0;

                    // Thông tin đơn hàng cũ chưa xử lý hoặc hoàn tất trong ngày
                    if (data.orderId && data.orderStatus) {
                        let message = 'Khách hàng này đang có đơn hàng ' + (data.orderStatus == 2 ? 'đã hoàn tất' : 'đang xử lý');

                        $txtOrder.removeClass("hide");
                        $txtOrder.html(message);
                        $btnOpenOrder.removeAttr('style');
                        $btnOpenOrder.attr(
                            'onClick',
                            "window.open('/thong-tin-don-hang?id=" + data.orderId + "', '_blank')"
                        );

                        show = 1;
                    }
                    else {
                        $txtOrder.addClass("hide");
                        $btnOpenOrder.removeAttr('onClick');
                        $btnOpenOrder.attr('style', 'display: none');
                    }

                    // Thông tin đơn hàng đổ trả chưa trừ tiền
                    if (data.refundGoodsId) {
                        $txtRefundGoods.removeClass("hide");
                        $btnOpenRefundGoods.removeAttr('style');
                        $btnOpenRefundGoods.attr(
                            'onClick',
                            "window.open('/xem-don-hang-doi-tra?id=" + data.refundGoodsId + "', '_blank')"
                        );

                        show = 1;
                    }
                    else {
                        $txtRefundGoods.addClass("hide");
                        $btnOpenRefundGoods.removeAttr('onClick');
                        $btnOpenRefundGoods.attr('style', 'display: none');
                    }

                    // Show thông báo
                    if (show == 1) {
                        // Clear event củ của button close modal
                        $btnCloseOrderOld.removeAttr('onClick');
                        // Add event mới (tên nhân viên mới) của button modal
                        $btnCloseOrderOld.attr(
                            'onClick',
                            '$("#oldOrderModal").modal("toggle"); _createOrder("' + staffDOM.value + '")'
                        );
                        // Mở modal
                        $("#oldOrderModal").modal({
                            show: 'true',
                            backdrop: 'static',
                            keyboard: 'false'
                        });
                    }
                }

                _createOrder(staffDOM.value);
            })
            .catch(function (e) {
                console.log(e);

                if (loading) {
                    HoldOn.close();
                    loading = false;
                }

                swal({
                    title: 'Error',
                    text: 'Đã có lỗi xảy ra trong quá trình kiểm tra đơn hàng cũ.',
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                });
            });

        return;
    }
    // #endregion

    _createOrder(staffDOM.value);
}

function cancelPreOrder() {
    let staffDOM = document.querySelector('[id$="_ddlCreatedBy"]');

    if (!staffDOM.value) {
        return swal({
            title: 'Error',
            text: 'Vui lòng chọn nhân viên xử lý đơn hàng',
            type: 'error',
            showCloseButton: true,
            html: true,
        });
    }

    let r = confirm('Thông báo\nBạn muốn hủy đơn đặt hàng #' + controller.preOrderId);

    if (!r)
        return;

    HoldOn.open();

    controller.cancelPreOrder(staffDOM.value)
        .then(function (response) {
            HoldOn.close();

            if (response.success)
                return swal({
                    title: 'Thành Công',
                    text: 'Đơn đặt hàng #' + controller.preOrderId + ' đã được hủy.',
                    type: 'success',
                    showCloseButton: true,
                    html: true,
                }, function () {
                    window.location.reload();

                    return false;
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

function recoveryPreOrder() {
    let staffDOM = document.querySelector('[id$="_ddlCreatedBy"]');

    if (!staffDOM.value) {
        return swal({
            title: 'Error',
            text: 'Vui lòng chọn nhân viên xử lý đơn hàng',
            type: 'error',
            showCloseButton: true,
            html: true,
        });
    }

    let r = confirm('Thông báo\nBạn muốn phục hồi đơn đặt hàng #' + controller.preOrderId);

    if (!r)
        return;

    HoldOn.open();

    controller.recoveryPreOrder(staffDOM.value)
        .then(function (response) {
            HoldOn.close();

            if (response.success)
                return swal({
                    title: 'Thành Công',
                    text: 'Đơn đặt hàng #' + controller.preOrderId + ' đã được phục hồi.',
                    type: 'success',
                    showCloseButton: true,
                    html: true,
                }, function () {
                    window.location.reload();

                    return false;
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