let _feeShipment, // Dùng để lấy trạng thái trước của radio Shipment
    _fee,
    _feeShop,
    _paymentType,
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
    _feeShipment = 2;
    _fee = 0;
    _feeShop = 0;

    // payment Type
    _paymentType = 1; // Tiền mặt

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
        chooseShopFee: true,
        weight: _weight_min,
        note: null,
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
            data: (params) => {
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
    $('#ddlProvince').on('select2:select', (e) => {
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
    $('#ddlDistrict').on('select2:select', (e) => {
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
    $('#ddlWard').on('select2:select', (e) => {
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
        success: (data, textStatus, xhr) => {
            HoldOn.close();

            if (xhr.status == 200 && data) {
                _personal = data;

                // Cập nhật hiện thị nơi nhận hàng
                $("#pick_address").val(_personal.address);
            } else {
                _alterError(titleAlert);
            }
        },
        error: (xhr, textStatus, error) => {
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
        width: '100%'
    });

    $("#weight").blur(function () {
        let weight = $(this).val();

        if (weight === undefined || weight === null || weight === '')
            return _alterError(
              "Lỗi nhập khối lượng gói hàng",
              { message: "Hãy nhập khối lượng gói hàng" }
            ).then(() => $("#weight").focus());


        // Chuyển kiểu string thành float
        weight = parseFloat(weight);

        if (weight < _weight_min)
            return _alterError(
              "Lỗi nhập khối lượng gói hàng",
              { message: "Khối lượng gói hàng tối thiểu là " + _weight_min + "kg" }
            ).then(() => $("#weight").focus());

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

    _order.note = $note.val();

    $note.change(function () {
        _order.note = $(this).val();
    });
}

function _initPage() {
    // Cập nhật hiện thị nơi nhận hàng
    $("#pick_address").val('133 Đường C12');

    // Cài đặt loại hàng hóa mặc định
    let newProduct = new Option('Hàng hóa', 1, false, false);
    $('#ddlProduct').append(newProduct).trigger('change');
    $('#ddlProduct').attr('disabled', true);
    $('#ddlProduct').attr('readonly', 'readonly');

    // Cài đặt trọng lượng mặc định cho sản phẩm
    $("#weight").val(_weight_min).trigger('blur');

    // Lấy thông tin query parameter
    let urlParams = new URLSearchParams(window.location.search);
    let orderId = +urlParams.get('orderID') || 0;
    let weight = +urlParams.get('weight') || 0;
    if (weight != 0) {
        $("#weight").val(weight).trigger('blur');
    }
    if (orderId == 0)
        return swal({
            title: "Lỗi",
            text: "Giá trị query param orderId không đúng",
            icon: "error",
        })
          .then(() => {
              window.location.href = "/danh-sach-don-hang";
          });

    let titleAlert = "Lấy thông tin đơn hàng";

    $.ajax({
        method: 'GET',
        url: "/api/v1/order/" + orderId + "/jt-express",
        beforeSend: function () {
            HoldOn.open();
        },
        success: (response, textStatus, xhr) => {
            HoldOn.close();

            let data = response;

            // id
            _order.orderId = data.orderId;
            $("#client_id").val(data.orderId);

            // Trạng thái đơn hàng
            if (data.orderStatus == 2) {
                $("#btnRegister").removeAttr("disabled");
                $("#btnRegister").html('<i class="fa fa-upload" aria-hidden="true"></i> Đồng bộ đơn hàng (F3)');
            }

            // Hình thức thanh toán
            _paymentType = data.paymentType;

            // Giá trị đơn hàng
            _order.price = data.price;

            // Thông tin người nhận hàng
            _order.customer.deliveryAddressId = data.customer.deliveryAddressId;
            // tel
            $("#tel").val(data.customer.phone).trigger('change');
            // name
            $("#name").val(data.customer.name).trigger('change');
            // address
            $("#address").val(data.customer.address).trigger('change');
            // province
            if (data.customer.province) {
                // Danh sách tỉnh / thành
                _order.customer.province = data.customer.province;

                let newOption = new Option(data.customer.province, data.customer.province, false, false);
                $('#ddlProvince').append(newOption).trigger('change');

                // Danh sách quận / huyện
                _disabledDDLDistrict(false);
            }
            // district
            if (data.customer.province && data.customer.district) {
                // Danh sách quận / huyện
                _order.customer.district = data.customer.district;

                let newOption = new Option(data.customer.district, data.customer.district, false, false);
                $('#ddlDistrict').removeAttr('disabled');
                $('#ddlDistrict').removeAttr('readonly');
                $('#ddlDistrict').append(newOption).trigger('change');

                // Danh sách phường / xã
                _disabledDDLWard(false);
            }
            // ward
            if (data.customer.province && data.customer.district && data.customer.ward) {
                // Danh sách phường / xã
                _order.customer.ward = data.customer.ward;

                let newOption = new Option(data.customer.ward, data.customer.ward, false, false);
                $('#ddlWard').removeAttr('disabled');
                $('#ddlWard').removeAttr('readonly');
                $('#ddlWard').append(newOption).trigger('change');
            }

            // Trọng lượng
            if (data.weight && data.weight > _weight_min)
                $("#weight").val(data.weight).trigger('blur');

            // Phí trong đơn hàng
            _feeShop = data.feeShop;
            $("#divFeeShop").show();
            $("#labelFeeShop").html(_formatThousand(data.feeShop));
            $("#fee_entered").prop('checked', true).trigger('change');

            // Tiền thu hộ
            let $cod = $("#cod");


            if (_paymentType == 3)
                $("#cod").val(_formatThousand(_order.price));
            else
                $("#cod").val(0);

            if (data.note)
                $("#note").val($("#note").val() + ". " + data.note).trigger('change');
        },
        error: (xhr, textStatus, error) => {
            HoldOn.close();

            return _alterError(titleAlert, xhr.responseJSON)
              .then(() => {
                  window.location.href = "/danh-sach-don-hang";
              });
        }
    });
}

function _checkSubmit() {
    let titleAlert = "Thông báo lỗi";
    let orderStatus = $("#btnRegister").is(':disabled');

    if (orderStatus) {
        return swal({
            title: titleAlert,
            text: "Đơn hàng chưa hoàn tất!",
            icon: "error",
        });
    }

    if (!_order.customer.phone)
        return swal({
            title: titleAlert,
            text: "Số điện thoại khách hàng chưa nhập",
            icon: "error",
        })
          .then(() => { $('#tel').focus(); });

    if (!_order.customer.province)
        return swal({
            title: titleAlert,
            text: "Địa chỉ tỉnh/thành khách hàng chưa chọn",
            icon: "error",
        })
          .then(() => { $("#ddlProvince").select2('open'); });
    if (_order.customer.province && !_order.customer.district)
        return swal({
            title: titleAlert,
            text: "Địa chỉ quận/huyện khách hàng chưa chọn",
            icon: "error",
        })
          .then(() => { $("#ddlDistrict").select2('open'); });
    if (_order.customer.province && _order.customer.district && !_order.customer.ward)
        return swal({
            title: titleAlert,
            text: "Địa chỉ phường/xã khách hàng chưa chọn",
            icon: "error",
        })
          .then(() => { $("#ddlDistrict").select2('open'); });
    if (!_order.customer.address)
        return swal({
            title: titleAlert,
            text: "Địa chỉ khách hàng chưa nhập",
            icon: "error",
        })
          .then(() => { $('#address').focus(); });

    let shipFee = parseFloat($("#feeship").text().replace(/,/g, ''));
    let insuranceFee = parseFloat($("#insuranceFee").text().replace(/,/g, ''));
    let shopFee = parseFloat($("#labelFeeShop").text().replace(/,/g, ''));
    let shippingFeeType = +$("input:radio[name='feeship']:checked").val() || 0;

    if (shopFee < (shipFee - insuranceFee) && shippingFeeType == 2) {
        return swal({
            title: titleAlert,
            text: "Phí ship nhân viên tính phải tối thiểu bằng phí ship J&T Express. Có thể chọn vào phí J&T Express tính mà không cần vào sửa đơn!",
            icon: "error"
        });
    }

    _submit();
}

function _calculateFee() {
    let $divFee = $("#divFee");
    let $fee = $("#feeship");
    let $labelShopFeeTitle = $("#labelShopFeeTitle");
    let $rdShopFee = $("#fee_entered");

    if (!_order.customer.ward) {
        _fee = 0;

        // Phí J&T Express
        $divFee.addClass("hiden");
        $fee.html("0");

        // Phí shop
        $labelShopFeeTitle.text("Phí");
        $rdShopFee.parent().hide();
    }
    else {
        let url = "/api/v1/jt-express/fee",
          query = "";

        if (_order.customer.ward)
            query += "&ward=" + _order.customer.ward;
        if (_order.price) {
            query += "&price=" + _order.price.toString();

            if (_paymentType == 3)
                query += "&cod=" + _order.price.toString();
        }

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
            success: (response, textStatus, xhr) => {
                HoldOn.close();

                if (xhr.status == 200 && response) {
                    if (response.success) {
                        let newFee = response.data.fee;

                        // Fix bug trường hợp phí thay đổi
                        if (_feeShipment == 1)
                            _order.price = _order.price - _fee + newFee;

                        // Phí J&T Express
                        _fee = newFee;
                        $fee.html(_formatThousand(_fee));

                        if (_fee != _feeShop) {
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
                    else {
                        _alterError(titleAlert, { message: data.message });
                    }
                } else {
                    _alterError(titleAlert);
                }
            },
            error: (xhr, textStatus, error) => {
                HoldOn.close();

                _alterError(titleAlert);
            }
        });
    }
}

function _calculateMoney() {
    let $cod = $("#cod");
    let feeShipment = +$("input:radio[name='feeship']:checked").val() || 2;

    switch (_feeShipment) {
        case 1:
            _order.price = _order.price - _fee;
            break;
        case 2:
            _order.price = _order.price - _feeShop;
            break;
        default:
            break;
    }

    switch (feeShipment) {
        case 1:
            _order.price = _order.price + _fee;
            break;
        case 2:
            _order.price = _order.price + _feeShop;
            break;
        default:
            break;
    }

    _feeShipment = feeShipment;
    _order.chooseShopFee = _feeShipment == 2;

    if (_paymentType == 3)
        $cod.val(_formatThousand(_order.price));
    else
        $cod.val(0);
}

function _submit() {
    let titleAlert = "Đồng bộ đơn hàng J&T Express";

    $.ajax({
        method: 'POST',
        contentType: 'application/json',
        dataType: "json",
        data: JSON.stringify(_order),
        url: "/api/v1/jt-express/order/register",
        beforeSend: function () {
            HoldOn.open();
        },
        success: (response, textStatus, xhr) => {
            HoldOn.close();

            if (xhr.status == 200 && response) {
                if (response.success)
                    return swal({
                        title: titleAlert,
                        text: "Đồng bộ thành công",
                        icon: "success",
                    })
                    .then(() => {
                        let code = response.data.code;
                        window.location.href = "https://vip.jtexpress.vn/#/service/expressTrack?id=" + code;
                    });
                else
                    return _alterError(titleAlert, response);
            } else {
                return _alterError(titleAlert, response);
            }
        },
        error: (xhr, textStatus, error) => {
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
        icon: "error",
        html: true
    });
}

function _formatThousand(value) {
    nfObject = new Intl.NumberFormat('en-US');

    return nfObject.format(value);
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
                data: (params) => {
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
                data: (params) => {
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