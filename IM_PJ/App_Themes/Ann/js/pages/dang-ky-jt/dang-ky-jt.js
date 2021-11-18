const OrderStatusEnum = {
    "Done": 2, // Đã hoàn tất
};

const FeeTypeEnum = {
    "JtExpress": 1, // Phí GHTK
    "Shop": 2  // Phí nhân viên tính
}

const PaymentMethodEnum = {
    "Cash": 1,              // Tiền mặt
    "CashCollection": 3,    // Thu hộ
}

let _feeType, // Dùng để lấy trạng thái trước của radio Shipment
    _fee,
    _shopFee,
    _paymentMethod,
    _order,
    _weight_min,
    _personal;

$(document).ready(function () {
    $("#form12").submit(function (e) {
        e.preventDefault();
        return false;
    });
    _initParameterLocal();
    _initReceiveInfo();
    _initReceiverAddress();
    _onChangeReceiverAddress();
    _initTableProduct();
    _initFee();
    _initNote();
    _initPage();
});

$(document).bind('keydown', function (e) {
    if (e.which === 114)
        return _checkSubmit();
})

function _initParameterLocal() {
    // Product Table
    _weight_min = 0.3;

    // Fee Ship
    _feeType = FeeTypeEnum.JtExpress;
    _fee = 0;
    _shopFee = 0;

    // Phương thức thanh toán
    _paymentMethod = PaymentMethodEnum.Cash;

    // Order
    _order = {
        orderId: null,
        customer: {
            deliveryAddressId: null,
            name: null,
            phone: null,
            address: null,
            province: null,
            district: null,
            ward: null
        },
        price: 0,
        cod: 0,
        chooseShopFee: true,
        weight: _weight_min,
        note: null,
        itemName: null
    }
}

function _initReceiveInfo() {
    $("#tel").change(function () {
        _order.customer.phone = $(this).val();
    });

    $("#name").change(function () {
        _order.customer.name = $(this).val();
    });

    $("#address").change(function () {
        _order.customer.address = $(this).val();
    });
}

function _initReceiverAddress() {
    // Danh sách tỉnh / thành phố
    $('#ddlProvince').select2({
        placeholder: '(Bấm để chọn tỉnh/thành phố)',
        ajax: {
            delay: 500,
            method: 'GET',
            url: '/api/v1/jt-express/provinces/select2',
            data: function (params) {
                var query = {
                    page: params.page || 1
                }

                if (params.term)
                    query.search = params.term;

                return query;
            }
        }
    });

    // Danh sách quận / huyện
    _disabledDDLDistrict(true);

    // Danh sách phường / xã
    _disabledDDLWard(true);
}

function _onChangeReceiverAddress() {
    // // Danh sách tỉnh / thành phố
    $('#ddlProvince').on('select2:select', function (e) {
        let data = e.params.data;

        // Cập nhật order
        _order.customer.province = data.id;

        // Danh sách quận / huyện
        _disabledDDLDistrict(false);
        $('#ddlDistrict').select2('open');

        // Danh sách phường / xã
        _disabledDDLWard(true);
    });

    // Danh sách quận / huyện
    $('#ddlDistrict').on('select2:select', function (e) {
        let data = e.params.data;

        // Cập nhật order
        _order.customer.district = data.id;

        // Tính tiền phí Ship
        _calculateFee();

        // Danh sách quận / huyện
        _disabledDDLWard(false);
        $('#ddlWard').select2('open');
    })

    // Danh sách phường / xã
    $('#ddlWard').on('select2:select', function (e) {
        let data = e.params.data;

        // Cập nhật order
        _order.customer.ward = data.id;

        // Tính tiền phí Ship
        _calculateFee();
    });
}

function _initShipment() {
    let titleAlert = 'Lấy thông tin cá nhân của shop';

    $.ajax({
        method: 'GET',
        url: '/api/v1/jt-express/personal',
        beforeSend: function () {
            HoldOn.open();
        },
        success: function (data, textStatus, xhr) {
            HoldOn.close();

            if (xhr.status == 200 && data) {
                _personal = data;

                // Cập nhật hiện thị nơi nhận hàng
                $("#pick_address").val(_personal.address);
            } else {
                _alterError(titleAlert);
            }
        },
        error: function () {
            HoldOn.close();
            _alterError(titleAlert);
        }
    });
}

function _initTableProduct() {
    // https://select2.org/searching#single-select
    $("#ddlProduct").select2({
        placeholder: 'Chọn tên hàng hóa',
        minimumResultsForSearch: Infinity,
        ajax: {
            method: 'GET',
            url: '/api/v1/delivery-save/products/select2'
        },
        width: '100%'
    });

    $('#ddlProduct').on('select2:select', function (e) {
        let ddlProductDOM = e.currentTarget;
        let key = +parseInt(ddlProductDOM.value) || 0;

        if (key) {
            let value = ddlProductDOM.options[ddlProductDOM.selectedIndex].text;

            _order.itemName = value
        }
        else {
            _order.itemName = null;
        }

        _calculateFee();
    });

    $("#weight").blur(function () {
        let weight = $(this).val();

        if (weight === undefined || weight === null || weight === '')
            return _alterError(
              "Lỗi nhập khối lượng gói hàng",
              { message: "Hãy nhập khối lượng gói hàng" }
            ).then(function () { $("#weight").focus(); });


        // Chuyển kiểu string thành float
        weight = parseFloat(weight);

        if (weight < _weight_min)
            return _alterError(
              "Lỗi nhập khối lượng gói hàng",
              { message: "Khối lượng gói hàng tối thiểu là " + _weight_min + "kg" }
            ).then(function () { $("#weight").focus(); });

        _order.weight = weight;
        _calculateFee();
    });
}

function _initFee() {
    $('input:radio[name="feeship"]').change(function () {
        _calculateFee();
    });
}

function _initNote() {
    let $note = $("#note");

    $note.change(function () {
        _order.note = $(this).val();
    });
}

/**
 * Cài đặt thông tin người nhận hàng
 * @param customer Thông tin người nhận hàng
 */
function _initDeliveryAddress(customer) {
    // Thông tin người nhận hàng
    _order.customer.deliveryAddressId = customer.deliveryAddressId;
    // tel
    $("#tel").val(customer.phone).trigger('change');
    // name
    $("#name").val(customer.name).trigger('change');
    // address
    $("#address").val(customer.address).trigger('change');

    //#region Danh sách tỉnh / thành
    if (customer.province) {
        let newOption = new Option(customer.province, customer.province, false, false);

        $('#ddlProvince').append(newOption).trigger('change');
        _order.customer.province = customer.province;

        // Danh sách quận / huyện
        _disabledDDLDistrict(false);
    }
    //#endregion

    //#region Danh sách quận / huyện
    if (customer.province && customer.district) {
        let $ddlDistrict = $('#ddlDistrict');
        let newOption = new Option(customer.district, customer.district, false, false);

        $ddlDistrict.removeAttr('disabled');
        $ddlDistrict.removeAttr('readonly');
        $ddlDistrict.append(newOption).trigger('change');
        _order.customer.district = customer.district;

        // Danh sách phường / xã
        _disabledDDLWard(false);
    }
    //#endregion

    //#region Danh sách quận / huyện
    if (customer.province && customer.district && customer.ward) {
        let $ddlWard = $('#ddlWard');
        let newOption = new Option(customer.ward, customer.ward, false, false);

        $ddlWard.removeAttr('disabled');
        $ddlWard.removeAttr('readonly');
        $ddlWard.append(newOption).trigger('change');
        _order.customer.ward = customer.ward;
    }
    //#endregion
}

/**
 * Cài đặt thông tin đơn hàng
 * @param order Thông tin đơn hàng
 */
function _initOrderInfo(order) {
    // Mã đơn hàng gộp
    if (order.groupCode)
    {
        _order.groupOrderCode = order.groupCode;
        $("#client_id").val(_order.groupOrderCode);
    }

    // Mã đơn hàng
    if (order.id)
    {
        _order.orderId = order.id;
        $("#client_id").val(_order.orderId);
    }

    // Trạng thái đơn hàng
    if (order.status == OrderStatusEnum.Done) {
        let $btnRegister = $("#btnRegister");

        $btnRegister.removeAttr("disabled");
        $btnRegister.html('<i class="fa fa-upload" aria-hidden="true"></i> Đồng bộ đơn hàng (F3)');
    }

    // Hình thức thanh toán
    _paymentMethod = order.paymentMethod;

    // Trọng lượng đơn hàng
    if (order.weight > 0)
        $("#weight").val(order.weight).trigger('blur');

    // Giá trị của đơn hàng
    _order.price = order.price;

    // Tiền thu hộ
    $("#cod").val(UtilsService.formatThousands(order.cod, ','));
    _order.cod = order.cod - order.fee; // trừ phí ship của shop để tính lại ở phía dưới

    // Có phí trong đơn hàng
    if (order.fee) {
        _shopFee = order.fee;

        $("#divFeeShop").show();
        $("#labelFeeShop").html(UtilsService.formatThousands(_shopFee, ','));
        $("#fee_entered").prop('checked', true).trigger('change');

        if (_paymentMethod != PaymentMethodEnum.CashCollection) {
            let $shopFee = $("#feeship_shop");

            $shopFee.attr("disabled", true);
            $shopFee.parent().hide();
        }
    }
    else {
        $("#divFeeShop").hide();

        let $shopFee = $("#feeship_shop");

        if (_paymentMethod != PaymentMethodEnum.CashCollection) {
            $shopFee.attr("disabled", true);
            $shopFee.hide();
            $shopFee.parent().hide();
        }
        else {
            $shopFee.removeAttr("disabled");
            $shopFee.prop('checked', true).trigger('change');
        }
    }
}

/**
 * Lấy thông tin đơn hàng gộp để đăng ký GHTK
 * @param groupOrderCode Mã đơn hàng gộp shop ANN
 */
function _initGroupOrder(groupCode) {
    let titleAlert = "Lấy thông tin đơn hàng gộp";

    $.ajax({
        method: 'GET',
        url: "/api/v1/group-order/" + groupCode + "/jt-express",
        beforeSend: function () {
            HoldOn.open();
        },
        success: function (response) {
            HoldOn.close();

            let data = response;

            // Thông tin người nhận
            if (data.customer)
                _initDeliveryAddress(data.customer);

            if (data.order)
                _initOrderInfo(data.order);

            // Chú thích đơn hàng
            if (data.note)
                $("#note").val(data.note).trigger('change');
        },
        error: function (xhr) {
            HoldOn.close();

            return _alterError(titleAlert, xhr.responseJSON)
              .then(function () { window.location.href = "/danh-sach-don-hang"; });
        }
    });
}

/**
 * Lấy thông tin đơn hàng để đăng ký GHTK
 * @param orderId ID đơn hàng shop ANN
 */
function _initOrder(id) {
    let titleAlert = "Lấy thông tin đơn hàng";

    $.ajax({
        method: 'GET',
        url: "/api/v1/order/" + id + "/jt-express",
        beforeSend: function () {
            HoldOn.open();
        },
        success: function (response) {
            HoldOn.close();

            let data = response;

            // Thông tin người nhận
            if (data.customer)
                _initDeliveryAddress(data.customer);

            if (data.order)
                _initOrderInfo(data.order);

            // Chú thích đơn hàng
            if (data.note)
                $("#note").val(data.note).trigger('change');
        },
        error: function (xhr) {
            HoldOn.close();

            return _alterError(titleAlert, xhr.responseJSON)
              .then(function () { window.location.href = "/danh-sach-don-hang"; });
        }
    });
}

function _initPage() {
    let urlParams = new URLSearchParams(window.location.search);

    //#region Cài đặt trọng lượng mặc định cho sản phẩm
    let weight = +urlParams.get('weight') || _weight_min;

    if (weight > 0)
        $("#weight").val(weight).trigger('blur');
    //#endregion

    // Cập nhật hiện thị nơi nhận hàng
    $("#pick_address").val('133 Đường C12');

    //#region Lấy thông tin query parameter
    let orderId = 0;
    let groupOrderCode = '';

    if (urlParams.get('orderID'))
        orderId =  +urlParams.get('orderID') || 0;

    if (urlParams.get('groupCode'))
        groupOrderCode = urlParams.get('groupCode').trim();
    //#endregion

    //#region Kiểm tra thông tin query parameter
    if (!orderId && !groupOrderCode) {
        swal({
            title: "Lỗi",
            text: "Không tìm thấy mã đơn hàng và mã đơn hàng gộp",
            icon: "error",
        })
        .then(function () { window.location.href = "/danh-sach-don-hang"; });

        return;
    } else if (orderId && groupOrderCode) {
        swal({
            title: "Lỗi",
            text: "Query parameter sai. Vì có cùng lúc mã đơn hàng và mã đơn hàng gộp",
            icon: "error",
        })
        .then(function () { window.location.href = "/danh-sach-don-hang"; });

        return;
    }
    //#endregion

    //#region Tải trang
    if (groupOrderCode) {
        _initGroupOrder(groupOrderCode);
        return;
    }

    if (orderId) {
        _initOrder(orderId);
        return;
    }
    //#endregion
}

function _checkSubmit() {
    let titleAlert = "Thông báo lỗi";
    let orderStatus = $("#btnRegister").is(':disabled');

    if (orderStatus) {
        return swal({
            title: titleAlert,
            text: "Đơn hàng chưa hoàn tất!",
            type: "error",
        });
    }

    if (!_order.customer.phone)
        return swal({
            title: titleAlert,
            text: "Số điện thoại khách hàng chưa nhập",
            type: "error",
        }, function () {
            $('#tel').focus();
        });

    if (!_order.customer.province)
        return swal({
            title: titleAlert,
            text: "Địa chỉ tỉnh/thành khách hàng chưa chọn",
            type: "error",
        }, function () {
            $("#ddlProvince").select2('open');
        });
    if (_order.customer.province && !_order.customer.district)
        return swal({
            title: titleAlert,
            text: "Địa chỉ quận/huyện khách hàng chưa chọn",
            type: "error",
        }, function () {
            $("#ddlDistrict").select2('open');
        });
    if (_order.customer.province && _order.customer.district && !_order.customer.ward)
        return swal({
            title: titleAlert,
            text: "Địa chỉ phường/xã khách hàng chưa chọn",
            type: "error",
        }, function () {
            $("#ddlDistrict").select2('open');
        });
    if (!_order.customer.address)
        return swal({
            title: titleAlert,
            text: "Địa chỉ khách hàng chưa nhập",
            type: "error",
        }, function () {
            $('#address').focus();
        });
    if (!_order.itemName)
        return swal({
            title: titleAlert,
            text: "Bạn chưa chọn tên hàng hóa",
            type: "error",
        }, function () {
            $('#ddlProduct').select2('open');
        });

    let shipFee = parseFloat($("#feeship").text().replace(/,/g, ''));
    let insuranceFee = parseFloat($("#insuranceFee").text().replace(/,/g, ''));
    let shopFee = parseFloat($("#labelFeeShop").text().replace(/,/g, ''));
    let shippingFeeType = +$("input:radio[name='feeship']:checked").val() || 0;

    if (shopFee < (shipFee - insuranceFee) && shippingFeeType == 2) {
        return swal({
            title: titleAlert,
            text: "Phí ship nhân viên tính phải tối thiểu bằng phí ship J&T Express. Có thể chọn vào phí J&T Express tính mà không cần vào sửa đơn!",
            type: "error"
        });
    }

    _submit();
}

function _handleFee(fee) {
    let $divFee = $("#divFee");
    let $fee = $("#feeship");
    let $labelShopFeeTitle = $("#labelShopFeeTitle");
    let $rdShopFee = $("#fee_entered");

    switch (_feeType) {
        case FeeTypeEnum.JtExpress:
            _order.cod = _order.cod - _fee;
            _order.price = _order.price - _fee;

            break;
        case FeeTypeEnum.Shop:
            _order.cod = _paymentMethod == PaymentMethodEnum.CashCollection
                ? (_order.cod - _shopFee)
                : 0;
            _order.price = _order.price - _shopFee;

            break;
        default:
            break;
    }

    // Phí J&T Express
    _fee = fee;
    $fee.html(UtilsService.formatThousands(_fee, ','));

    if (_fee != _shopFee) {
        $divFee.removeClass("hide");

        $labelShopFeeTitle.text("Phí nhân viên tính");
        $rdShopFee.parent().show();
    }
    else {
        $divFee.addClass("hide");

        $labelShopFeeTitle.text("Phí");
        $rdShopFee.parent().hide();
    }

    // Tính toán lại tiền thu hộ
    _calculateMoney();
}

function _getJtExpressFee(callback) {
    let url = "/api/v1/jt-express/fee",
        query = "";

    if (_order.customer.ward)
        query += "&ward=" + _order.customer.ward;
    if (_order.price)
        query += "&price=" + _order.price.toString();
    if (_order.cod)
        query += "&cod=" + _order.cod.toString();

    query += "&weight=" + (+_order.weight || _weight_min);

    if (query)
        url = url + "?" + query.substring(1);

    let titleAlert = "Tính phí giao hàng";

    $.ajax({
        method: 'GET',
        url: url,
        beforeSend: function () {
            HoldOn.open();
        },
        success: function (response, textStatus, xhr) {
            HoldOn.close();

            if (xhr.status == 200 && response) {
                if (response.success)
                    callback(response.data.fee);
                else
                    _alterError(titleAlert, { message: data.message });
            } else {
                _alterError(titleAlert);
            }
        },
        error: function () {
            HoldOn.close();

            _alterError(titleAlert);
        }
    });
}

function _calculateFee() {
    if (!_order.customer.ward)
        _handleFee(_shopFee);
    else
        _getJtExpressFee(_handleFee);
}

function _calculateMoney() {
    let $cod = $("#cod");
    let feeType = +$("input:radio[name='feeship']:checked").val() || FeeTypeEnum.JtExpress;

    switch (feeType) {
        case FeeTypeEnum.JtExpress:
            _order.cod = _order.cod + _fee;
            _order.price = _order.price + _fee;

            break;
        case FeeTypeEnum.Shop:
            _order.cod = _paymentMethod == PaymentMethodEnum.CashCollection
                ? (_order.cod + _shopFee)
                : 0;
            _order.price = _order.price + _shopFee;

            break;
        default:
            break;
    }

    _feeType = feeType;
    $cod.val(UtilsService.formatThousands(_order.cod, ','));
}

function _submit() {
    let titleAlert = "Đồng bộ J&T Express";

    _order.shopFee = _shopFee;
    _order.chooseShopFee = _feeType == FeeTypeEnum.Shop;

    $.ajax({
        method: 'POST',
        contentType: 'application/json',
        dataType: "json",
        data: JSON.stringify(_order),
        url: "/api/v1/jt-express/order/register",
        beforeSend: function () {
            HoldOn.open();
        },
        success: function (response, textStatus, xhr) {
            HoldOn.close();

            if (xhr.status == 200 && response) {
                if (response.success)
                    return swal({
                        title: titleAlert,
                        text: "Đồng bộ thành công",
                        type: "success",
                        showCancelButton: true,
                        cancelButtonText: "Đóng",
                        confirmButtonText: "In phiếu gửi hàng",
                        closeOnConfirm: true,
                    }, function (confirm) {
                        if (confirm) {
                            sweetAlert.close();
                            window.location.href = "/print-jt-express?code=" + response.data.code;
                        }
                        else {
                            window.close();
                        }
                    });
                else
                    return _alterError(titleAlert, response);
            } else {
                return _alterError(titleAlert, response);
            }
        },
        error: function (xhr) {
            HoldOn.close();

            return _alterError(titleAlert, xhr.responseJSON);
        }
    });
}

function _alterError(title, responseJSON) {
    let message = 'Đẫ có lỗi xãy ra.';

    if (!title)
        title = 'Thông báo lỗi'

    if (responseJSON.message)
        message = responseJSON.message;

    return swal({
        title: title,
        text: message,
        type: "error",
        html: true
    });
}

function _disabledDDLDistrict(disabled) {
    // Update Order
    _order.customer.district = null;

    if (disabled) {
        $('#ddlDistrict').attr('disabled', true);
        $('#ddlDistrict').attr('readonly', 'readonly');
        $('#ddlDistrict').select2({ placeholder: '(Bấm để chọn quận/huyện)' });
    }
    else {
        $('#ddlDistrict').removeAttr('disabled');
        $('#ddlDistrict').removeAttr('readonly');
        $('#ddlDistrict').val(null).trigger('change');
        $('#ddlDistrict').select2({
            placeholder: '(Bấm để chọn tỉnh/thành phố)',
            ajax: {
                delay: 500,
                method: 'GET',
                url: '/api/v1/jt-express/districts/select2',
                data: function (params) {
                    var query = {
                        province: _order.customer.province,
                        page: params.page || 1
                    }

                    if (params.term)
                        query.search = params.term;

                    return query;
                }
            }
        });
    }
}

function _disabledDDLWard(disabled) {
    // Update Order
    _order.customer.ward = null;

    if (disabled) {
        $('#ddlWard').attr('disabled', true);
        $('#ddlWard').attr('readonly', 'readonly');
        $('#ddlWard').select2({ placeholder: '(Bấm để chọn phường/xã)' });
    }
    else {
        $('#ddlWard').removeAttr('disabled');
        $('#ddlWard').removeAttr('readonly');
        $('#ddlWard').val(null).trigger('change');
        $('#ddlWard').select2({
            placeholder: '(Bấm để chọn phường/xã)',
            ajax: {
                delay: 500,
                method: 'GET',
                url: '/api/v1/jt-express/wards/select2',
                data: function (params) {
                    var query = {
                        province: _order.customer.province,
                        district: _order.customer.district,
                        page: params.page || 1
                    }

                    if (params.term)
                        query.search = params.term;

                    return query;
                }
            }
        });
    }
}