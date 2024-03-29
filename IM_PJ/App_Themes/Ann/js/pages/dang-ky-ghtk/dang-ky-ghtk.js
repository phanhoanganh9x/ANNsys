﻿const OrderStatusEnum = {
    "Done": 2, // Đã hoàn tất
};

const FeeTypeEnum = {
    "Ghtk": 1, // Phí GHTK
    "Shop": 2  // Phí nhân viên tính
}

const PaymentMethodEnum = {
    "Cash": 1, // Tiền mặt
    "CashCollection": 3 // Thu hộ
}

let _feeType, // Dùng để lấy trạng thái trước của radio Shipment
    _fee,
    _shopFee,
    _order,
    _weight_min,
    _product,
    _personal,
    _postOffice;

$(document).ready(function () {
    $("#form12").submit(function (e) {
        e.preventDefault();
        return false;
    });
    _initParameterLocal();
    _initReceiveInfo();
    _initReceiverAddress();
    _onChangeReceiverAddress();
    _initShipment();
    _initTableProduct();
    _initDeliveryMethod();
    _initFee();
    _initNote();
    _initPage();
});

$(document).bind('keydown', function (e) {
    if (e.which === 114)
        return _checkSubmit();
})

function _initParameterLocal() {
    // Fee type
    _feeType = FeeTypeEnum.Ghtk;
    _fee = 0;
    _shopFee = 0;

    // payment Type
    _paymentMethod = PaymentMethodEnum.Cash;

    // Order
    _order = {
        groupCode: null,
        id: null,
        pick_name: null,
        pick_money: 0,
        pick_address_id: null,
        pick_address: null,
        pick_province: null,
        pick_district: null,
        pick_ward: null,
        pick_street: null,
        pick_tel: null,
        pick_email: null,
        name: null,
        address: null,
        province_id: null,
        province: null,
        district_id: null,
        district: null,
        ward_id: null,
        ward: null,
        street: null,
        tel: null,
        note: null,
        email: null,
        use_return_address: 0,
        is_freeship: 1,
        weight_option: 'kilogram',
        pick_work_shift: null,
        pick_date: null,
        value: 0,
        opm: null,
        pick_option: null,
        actual_transfer_method: 'road',
        transport: 'road',
        is_part_deliver: 0
    }

    // Product Table
    _weight_min = 0.3;
    _product = {
        name: null,
        weight: _weight_min,
    }
}

function _initReceiveInfo() {
    $("#tel").change(function () {
        _order.tel = $(this).val();
    });

    $("#name").change(function () {
        _order.name = $(this).val();
    });

    $("#address").change(function () {
        _order.address = $(this).val();
        _order.street = _order.address;
    });
}

function _initReceiverAddress() {
    // Danh sách tỉnh / thành phố
    $('#ddlProvince').select2({
        placeholder: '(Bấm để chọn tỉnh/thành phố)',
        ajax: {
            delay: 500,
            method: 'GET',
            url: '/api/v1/delivery-save/provinces/select2',
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
        _order.province_id = data.id;
        _order.province = data.text;

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
        _order.district_id = data.id;
        _order.district = data.text;

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
        _order.ward_id = data.id;
        _order.ward = data.text;

        // Tính tiền phí Ship
        _calculateFee();
    });
}

function _initShipment() {
    $('input:radio[name="shipment_type"]').change(function () {

        if ($(this).val() == 'shop') {
            if (_personal) {
                return _onChangeShipment('shop')
            }

            let titleAlert = 'Lấy thông tin cá nhân của shop';

            $.ajax({
                method: 'GET',
                url: '/api/v1/delivery-save/personal',
                beforeSend: function () {
                    HoldOn.open();
                },
                success: function (data, textStatus, xhr) {
                    HoldOn.close();

                    if (xhr.status == 200 && data) {
                        _personal = data;
                        return _onChangeShipment('shop');
                    } else {
                        _alterError(titleAlert);
                    }
                },
                error: function () {
                    HoldOn.close();
                    _alterError(titleAlert);
                }
            });
        } else {
            if (_postOffice) {
                return _onChangeShipment('post_office');
            }

            let titleAlert = 'Lấy thông tin của buc cục GHTK';

            $.ajax({
                method: 'GET',
                url: '/api/v1/delivery-save/post-office',
                success: function (data, textStatus, xhr) {
                    if (xhr.status == 200 && data) {
                        _postOffice = data;
                        _onChangeShipment('post_office');
                    } else {
                        _alterError(titleAlert);
                    }
                },
                error: function () {
                    _alterError(titleAlert);
                }
            });
        }
    });

    // Cài đặt drop dơwn list pick_work_shift
    let now = new Date();
    let strNow = "";
    let hours = now.getHours() + 1;
    let minutes = now.getMinutes();
    let pick_work_shift = [];

    strNow = now.toISOString().substring(0, 10).replace(/-/g, '/');

    if (hours < 10 || (hours == 10 && minutes <= 30)) {
        pick_work_shift.push({ id: 1, text: "Sáng nay" });
        pick_work_shift.push({ id: 2, text: "Chiều nay" });
        pick_work_shift.push({ id: 3, text: "Tối nay" });

        _order.pick_date = strNow;
        _order.pick_work_shift = 1;
    }
    else if (hours < 16 || (hours == 16 && minutes <= 30)) {
        pick_work_shift.push({ id: 1, text: "Chiều nay" });
        pick_work_shift.push({ id: 2, text: "Tối nay" });
        pick_work_shift.push({ id: 3, text: "Sáng mai" });

        _order.pick_date = strNow;
        _order.pick_work_shift = 2;
    }
    else {
        pick_work_shift.push({ id: 1, text: "Tối nay" });
        pick_work_shift.push({ id: 2, text: "Sáng mai" });
        pick_work_shift.push({ id: 3, text: "Chiều mai" });

        _order.pick_date = strNow;
        _order.pick_work_shift = 3;
    }

    //now.setDate(now.getDate() + 1)
    //let strPickDate = now.toISOString().substring(0, 10).replace(/-/g, '/');

    //_order.pick_date = strPickDate;
    //_order.pick_work_shift = 1;

    //pick_work_shift.push({ id: 1, text: "Sáng mai" });
    //pick_work_shift.push({ id: 2, text: "Chiều mai" });
    //pick_work_shift.push({ id: 3, text: "Sáng nay" });

    $("#pick_work_shift").select2({
        minimumResultsForSearch: Infinity,
        data: pick_work_shift
    });

    _onChangePickWorkShift(pick_work_shift[0].text);

    $('#pick_work_shift').on('select2:select', function (e) {
        let data = e.params.data;

        _onChangePickWorkShift(data.text);
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
        let data = e.params.data;
        _product.name = data.text;
        _calculateFee();
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

        let $shipmentShop = $("#shipment_shop");
        let $shipmentPostOffice = $("#shipment_post_office");

        if (weight < 5) {
            $shipmentPostOffice.prop('checked', false);
            $shipmentPostOffice.attr('disabled', true);

            $shipmentShop.removeAttr('disabled');
            $shipmentShop.prop('checked', true).trigger('change');
        } else {
            $shipmentShop.prop('checked', false);
            $shipmentShop.attr('disabled', true);

            $shipmentPostOffice.removeAttr('disabled');
            $shipmentPostOffice.prop('checked', true).trigger('change');
        }

        _product.weight = weight;
        _calculateFee();
    });
}

function _initDeliveryMethod() {
    $('#part-delivery').change(function () {
        let $note = $("#note");
        let note = $note.val().replace(/^giao hàng 1 phần, /g, '');

        _order.is_part_deliver = this.checked ? 1 : 0;
        
        if (_order.is_part_deliver)
            note = 'giao hàng 1 phần, ' + note;

        $("#note").val(note).trigger('change');
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
    // tel
    $("#tel").val(customer.phone).trigger('change');
    // name
    $("#name").val(customer.name).trigger('change');
    // address
    $("#address").val(customer.address).trigger('change');

    //#region Danh sách tỉnh / thành
    if (customer.province) {
        let newOption = new Option(customer.province.value, customer.province.key, false, false);

        $('#ddlProvince').append(newOption).trigger('change');
        _order.province_id = customer.province.key;
        _order.province = customer.province.value;

        // Danh sách quận / huyện
        _disabledDDLDistrict(false);
    }
    //#endregion

    //#region Danh sách quận / huyện
    if (customer.province && customer.district) {
        let $ddlDistrict = $('#ddlDistrict');
        let newOption = new Option(customer.district.value, customer.district.key, false, false);

        $ddlDistrict.removeAttr('disabled');
        $ddlDistrict.removeAttr('readonly');
        $ddlDistrict.append(newOption).trigger('change');
        _order.district_id = customer.district.key;
        _order.district = customer.district.value;

        // Danh sách phường / xã
        _disabledDDLWard(false);
    }
    //#endregion

    //#region Danh sách quận / huyện
    if (customer.province && customer.district && customer.ward) {
        let $ddlWard = $('#ddlWard');
        let newOption = new Option(customer.ward.value, customer.ward.key, false, false);

        $ddlWard.removeAttr('disabled');
        $ddlWard.removeAttr('readonly');
        $ddlWard.append(newOption).trigger('change');
        _order.ward_id = customer.ward.key;
        _order.ward = customer.ward.value;
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
         _order.groupCode = order.groupCode;
         $("#client_id").val(_order.groupCode);
     }

     // Mã đơn hàng
     if (order.id)
     {
         _order.id = order.id;
         $("#client_id").val(_order.id);
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
    _order.value = order.price;

    // Tiền thu hộ
    $("#pick_money").val(UtilsService.formatThousands(order.cod, ','));
    _order.pick_money = order.cod - order.fee; // trừ phí ship của shop để tính lại ở phía dưới

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
        url: "/api/v1/group-order/" + groupCode + "/delivery-save",
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
        url: "/api/v1/order/" + id + "/delivery-save",
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

function _onChangeShipment(shipment) {
    if (shipment == 'shop') {
        // Cập nhật vô order
        _order.pick_name = _personal.contactName;
        _order.pick_address_id = _personal.warehouseCode;
        _order.pick_address = _personal.address;
        _order.pick_province = _personal.province;
        _order.pick_district = _personal.district;
        _order.pick_ward = _personal.ward;
        _order.pick_street = _personal.street;
        _order.pick_tel = _personal.tel;
        _order.pick_email = _personal.email;
        _order.pick_option = "cod";
        // Cập nhật hiện thị nơi nhận hàng
        $("#pick_address").val(_personal.address);
    }
    else {
        // Cập nhật vô order
        _order.pick_option = "post";
        // Cập nhật hiện thị nơi nhận hàng
        $("#pick_address").val(_postOffice.displayName);
    }
}

function _onChangePickWorkShift(pick_work_shift) {
    let now = new Date();
    let tomorrow = new Date();
    let strNow = "";
    let strTomorrow = "";

    tomorrow.setDate(tomorrow.getDate() + 1);
    strNow = now.toISOString().substring(0, 10).replace(/-/g, '/');
    strTomorrow = tomorrow.toISOString().substring(0, 10).replace(/-/g, '/');

    if (pick_work_shift == "Sáng nay") {
        _order.pick_date = strNow;
        _order.pick_work_shift = 1;
    }
    else if (pick_work_shift == "Chiều nay") {
        _order.pick_date = strNow;
        _order.pick_work_shift = 2;
    }
    else if (pick_work_shift == "Tối nay") {
        _order.pick_date = strNow;
        _order.pick_work_shift = 3;
    }
    else if (pick_work_shift == "Sáng mai") {
        _order.pick_date = strTomorrow;
        _order.pick_work_shift = 1;
    }
    else if (pick_work_shift == "Chiều mai") {
        _order.pick_date = strTomorrow;
        _order.pick_work_shift = 2;
    }
    else if (pick_work_shift == "Tối mai") {
        _order.pick_date = strTomorrow;
        _order.pick_work_shift = 3;
    }

    _calculateFee();
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

    if (!_order.tel)
        return swal({
            title: titleAlert,
            text: "Số điện thoại khách hàng chưa nhập",
            icon: "error",
        })
          .then(function () { $('#tel').focus(); });

    if (!_order.province)
        return swal({
            title: titleAlert,
            text: "Địa chỉ tỉnh/thành khách hàng chưa chọn",
            icon: "error",
        })
          .then(function () { $("#ddlProvince").select2('open'); });
    if (_order.province && !_order.district)
        return swal({
            title: titleAlert,
            text: "Địa chỉ quận/huyện khách hàng chưa chọn",
            icon: "error",
        })
          .then(function () { $("#ddlDistrict").select2('open'); });
    if (_order.province && _order.district && !_order.ward)
        return swal({
            title: titleAlert,
            text: "Địa chỉ phường/xã khách hàng chưa chọn",
            icon: "error",
        })
          .then(function () { $("#ddlDistrict").select2('open'); });
    if (!_order.address)
        return swal({
            title: titleAlert,
            text: "Địa chỉ khách hàng chưa nhập",
            icon: "error",
        })
          .then(function () { $('#address').focus(); });
    if (!_product.name)
        return swal({
            title: titleAlert,
            text: "Chưa chọn loại sản phẩm",
            icon: "error",
        })
          .then(function () { $("#ddlProduct").select2('open'); });

    let ship_GHTK = parseFloat($("#feeship").text().replace(/,/g, ''));
    let insurance_GHTK = parseFloat($("#insuranceFee").text().replace(/,/g, ''));
    let ship_Entered = parseFloat($("#labelFeeShop").text().replace(/,/g, ''));
    let feeShipment = +$("input:radio[name='feeship']:checked").val() || 0;
    if (ship_Entered < (ship_GHTK - insurance_GHTK) && feeShipment == 2) {
        return swal({
            title: titleAlert,
            text: "Phí ship nhân viên tính phải tối thiểu bằng phí ship GHTK. Có thể chọn vào phí GHTK tính mà không cần vào sửa đơn!",
            icon: "error"
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
        case FeeTypeEnum.Ghtk:
            _order.pick_money = _order.pick_money - _fee;
            _order.value = _order.value - _fee;

            break;
        case FeeTypeEnum.Shop:
            _order.pick_money = _paymentMethod == PaymentMethodEnum.CashCollection
                ? (_order.pick_money - _shopFee)
                : 0;
            _order.value = _order.value - _shopFee;

            break;
        default:
            break;
    }

    // Phí GHTK
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

function _getGhtkFee(callback) {
    let url = "/api/v1/delivery-save/fee",
        query = "";

    if (_order.pick_address_id)
        query += "&pick_address_id=" + _order.pick_address_id;
    if (_order.pick_address)
        query += "&pick_address=" + _order.pick_address;
    if (_order.pick_province)
        query += "&pick_province=" + _order.pick_province;
    if (_order.pick_district)
        query += "&pick_district=" + _order.pick_district;
    if (_order.pick_ward)
        query += "&pick_ward=" + _order.pick_ward;
    if (_order.pick_street)
        query += "&pick_street=" + _order.pick_street;
    if (_order.address)
        query += "&address=" + _order.address;
    if (_order.province)
        query += "&province=" + _order.province;
    if (_order.district)
        query += "&district=" + _order.district;
    if (_order.ward)
        query += "&ward=" + _order.ward;
    if (_order.street)
        query += "&street=" + _order.street;

    query += "&weight=" + ((+_product.weight || 0) * 1000);
    query += "&transport=road";

    // tính phí có bảo hiểm
    if ((+_order.value || 0) > 0)
        query += "&value=" + (+_order.value || 0);

    if (query)
        url = url + "?" + query.substring(1);

    let titleAlert = "Tính phí giao hàng";

    $.ajax({
        method: 'GET',
        url: url,
        beforeSend: function () {
            HoldOn.open();
        },
        success: function (data, textStatus, xhr) {
            HoldOn.close();

            if (xhr.status == 200 && data) {
                if (data.success)
                    _handleFee(data.fee.fee);
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
    if (!_order.pick_province || !_order.pick_district || !_order.province || !_order.district || !_order.ward)
        _handleFee(_shopFee);
    else
        _getGhtkFee(_handleFee);
}

function _calculateMoney() {
    let $pick_money = $("#pick_money");
    let feeType = +$("input:radio[name='feeship']:checked").val() || FeeTypeEnum.Ghtk;

    switch (feeType) {
        case FeeTypeEnum.Ghtk:
            _order.pick_money = _order.pick_money + _fee;
            _order.value = _order.value + _fee;

            break;
        case FeeTypeEnum.Shop:
            _order.pick_money = _paymentMethod == PaymentMethodEnum.CashCollection
            ? (_order.pick_money + _shopFee)
            : 0;
            _order.value = _order.value + _shopFee;

            break;
        default:
            break;
    }

    _feeType = feeType;
    $pick_money.val(UtilsService.formatThousands(_order.pick_money, ','));
}

/**
 * Thông báo hỏi có muốn in hoá đơn không
 * Trường hợp không đồng ý sẽ chuyển tới trang web GHTK với mã đơn đã tạo
 * @param ghtkCode Mã đơn GHTK
 */
function _alertInvoicePrint(titleAlert, ghtkCode) {
    swal({
        title: titleAlert,
        text: "Đồng bộ thành công",
        icon: "success",
        buttons: {
            cancel: "Đóng",
            text: "In phiếu gửi hàng",
            closeModal: false,
        },
        dangerMode: true,
    })
    .then(function (confirm) {
        let url = "";

        if (confirm) {
            url += "/print-shipping-note?";

            if (_order.groupCode)
                url += "groupCode=" + _order.groupCode;

            if (_order.id)
                url += "id=" + _order.id;
        }
        else
            url += "https://khachhang.giaohangtietkiem.vn/web/don-hang?customer_created_from=1970-01-01+07:00:00&customer_info=" + ghtkCode;

        window.location.href = url;
    });
}

function _submit() {
    let titleAlert = "Đồng bộ đơn hàng GHTK";
    let parameters = {
        products: [_product],
        order: _order,
        shopFee: _shopFee,
        chooseShopFee: _feeType == FeeTypeEnum.Shop
    };

    $.ajax({
        method: 'POST',
        contentType: 'application/json',
        dataType: "json",
        data: JSON.stringify(parameters),
        url: "/api/v1/delivery-save/order/register",
        beforeSend: function () {
            HoldOn.open();
        },
        success: function (data, textStatus, xhr) {
            HoldOn.close();

            if (xhr.status == 200 && data) {
                if (data.success)
                    _alertInvoicePrint(titleAlert, data.order.label);
                else
                    _alterError(titleAlert, { message: data.message });
            } else {
                _alterError(titleAlert);
            }
        },
        error: function (xhr) {
            HoldOn.close();

            _alterError(titleAlert, xhr.responseJSON);
        }
    });
}

function _alterError(title, responseJSON) {
    let message = '';
    title = (typeof title !== 'undefined') ? title : 'Thông báo lỗi';

    if (responseJSON === undefined || responseJSON === null) {
        message = 'Đẫ có lỗi xãy ra.';
    }
    else {
        if (responseJSON.message)
            message += responseJSON.message;
    }

    return swal({
        title: title,
        text: message,
        icon: "error",
    });
}

function _disabledDDLDistrict(disabled) {
    // Update Order
    _order.district = null;
    _order.district_id = null;

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
                url: '/api/v1/delivery-save/province/' + _order.province_id + '/districts/select2',
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
    }
}

function _disabledDDLWard(disabled) {
    // Update Order
    _order.ward = null;
    _order.ward_id = null;

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
                url: '/api/v1/delivery-save/district/' + _order.district_id + '/wards/select2',
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
    }
}