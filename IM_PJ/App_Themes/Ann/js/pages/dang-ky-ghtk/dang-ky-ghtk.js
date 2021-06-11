let _feeShipment, // Dùng để lấy trạng thái trước của radio Shipment
    _fee,
    _insuranceFee,
    _feeShop,
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
    _initFee();
    _initNote();
    _initPage();
});

$(document).bind('keydown', function (e) {
    if (e.which === 114)
        return _checkSubmit();
})

function _initParameterLocal() {
    // Fee Ship
    _feeShipment = 1;
    _fee = 0;
    _insuranceFee = 0;
    _feeShop = 0;

    // payment Type
    _paymentType = 0;

    // Order
    _order = {
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
        //deliver_work_shift: null,
        pick_date: null,
        //deliver_date: null,
        value: 0,
        opm: null,
        pick_option: null,
        actual_transfer_method: 'road',
        transport: 'road'
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

    $("#address").blur(function () {
        _updateAddress();
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
        _order.province_id = data.id;
        _order.province = data.text;
        _updateAddress();

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
        _order.district_id = data.id;
        _order.district = data.text;
        _updateAddress();

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
        _order.ward_id = data.id;
        _order.ward = data.text;
        _updateAddress();

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
                success: (data, textStatus, xhr) => {
                    HoldOn.close();

                    if (xhr.status == 200 && data) {
                        _personal = data;
                        return _onChangeShipment('shop');
                    } else {
                        _alterError(titleAlert);
                    }
                },
                error: (xhr, textStatus, error) => {
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
                success: (data, textStatus, xhr) => {
                    if (xhr.status == 200 && data) {
                        _postOffice = data;
                        _onChangeShipment('post_office');
                    } else {
                        _alterError(titleAlert);
                    }
                },
                error: (xhr, textStatus, error) => {
                    _alterError(titleAlert);
                }
            });
        }
    });

    // Cài đặt drop dơwn list pick_work_shift
    let now = new Date();
    let strNow = "";
    let hours = now.getHours() + 1;
    let pick_work_shift = [];

    strNow = now.toISOString().substring(0, 10).replace(/-/g, '/');

    if (hours <= 10) {
        pick_work_shift.push({ id: 1, text: "Sáng nay" });
        pick_work_shift.push({ id: 2, text: "Chiều nay" });
        pick_work_shift.push({ id: 3, text: "Tối nay" });

        _order.pick_date = strNow;
        _order.pick_work_shift = 1;
    }
    else if (hours <= 16) {
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

    $("#pick_work_shift").select2({
        minimumResultsForSearch: Infinity,
        data: pick_work_shift
    });
    //$("#deliver_work_shift").select2({
    //    allowClear: true,
    //});
    _onChangePickWorkShift(pick_work_shift[0].text);


    $('#pick_work_shift').on('select2:select', function (e) {
        let data = e.params.data;

        _onChangePickWorkShift(data.text);
    });

    //$('#deliver_work_shift').on('select2:select', function (e) {
    //    let data = e.params.data;
    //    let now = new Date();
    //    let tomorrow = new Date();
    //    let dateAfterTomorrow = new Date();
    //    let strNow = "";
    //    let strTomorrow = "";
    //    let strDateAfterTomorrow = "";

    //    tomorrow.setDate(tomorrow.getDate() + 1);
    //    dateAfterTomorrow.setDate(tomorrow.getDate() + 2);
    //    strNow = now.toISOString().substring(0, 10).replace(/-/g, '/');
    //    strTomorrow = tomorrow.toISOString().substring(0, 10).replace(/-/g, '/');
    //    strDateAfterTomorrow = dateAfterTomorrow.toISOString().substring(0, 10).replace(/-/g, '/');

    //    switch (data.text) {
    //        case "Tối nay":
    //            _order.deliver_date = _order.pick_date
    //            _order.deliver_work_shift = 3;
    //            break;
    //        case "Sáng mai":
    //            _order.deliver_date = strTomorrow;
    //            _order.deliver_work_shift = 1;
    //            break;
    //        case "Chiều mai":
    //            _order.deliver_date = strTomorrow;
    //            _order.deliver_work_shift = 2;
    //            break;
    //        case "Tối mai":
    //            _order.deliver_date = strTomorrow;
    //            _order.deliver_work_shift = 3;
    //            break;
    //        case "Sáng ngày kia":
    //            _order.deliver_date = strDateAfterTomorrow;
    //            _order.deliver_work_shift = 1;
    //            break;
    //        case "Chiều ngày kia":
    //            _order.deliver_date = strDateAfterTomorrow;
    //            _order.deliver_work_shift = 2;
    //            break;
    //        case "Tối ngày kia":
    //            _order.deliver_date = strDateAfterTomorrow;
    //            _order.deliver_work_shift = 3;
    //            break;
    //        default:
    //            break;
    //    }
    //});
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

    $('#ddlProduct').on('select2:select', (e) => {
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

function _initFee() {
    $('input:radio[name="feeship"]').change(function () {
        _calculateFee();
    });
}

function _initNote() {
    $("#note").change(function () {
        _order.note = $(this).val();
    });
}

function _initPage() {
    // Cài đặt trọng lượng mặc định cho sản phẩm
    $("#weight").val(_weight_min).trigger('blur');

    // Lấy thông tin query parameter
    let urlParams = new URLSearchParams(window.location.search);
    let orderID = +urlParams.get('orderID') || 0;
    let weight = +urlParams.get('weight') || 0;
    if (weight != 0) {
        $("#weight").val(weight).trigger('blur');
    }
    if (orderID == 0)
        return swal({
            title: "Lỗi",
            text: "Giá trị query param orderID không đúng",
            icon: "error",
        })
          .then(() => {
              window.location.href = "/danh-sach-don-hang";
          });

    let titleAlert = "Lấy thông tin đơn hàng";

    $.ajax({
        method: 'GET',
        url: "/api/v1/order/" + orderID + "/delivery-save",
        beforeSend: function () {
            HoldOn.open();
        },
        success: (response, textStatus, xhr) => {
            HoldOn.close();

            let data = response;

            if (data.executeStatus == 2) {
                $("#btnRegister").removeAttr("disabled");
                $("#btnRegister").html('<i class="fa fa-upload" aria-hidden="true"></i> Đồng bộ đơn hàng (F3)');
            }

            // tel
            $("#tel").val(data.customerPhone).trigger('change');
            // name
            $("#name").val(data.customerName).trigger('change');
            // pick_money (trừ phí ship của shop để tính lại ở phía dưới)
            _order.pick_money = data.money - data.feeShop;
            $("#pick_money").val(_formatThousand(data.money));
            // value
            _order.value = data.value;
            $("#value").val(_formatThousand(data.money));
            $("#total_money").val(_formatThousand(data.money));
            // id
            _order.id = data.orderID;
            $("#client_id").val(data.orderID);

            // Shipping type
            _paymentType = data.paymentType;

            // address
            $("#address").val(data.customerAddress).trigger('change');

            // province
            if (data.customerProvinceID) {
                // Danh sách tỉnh / thành
                _order.province_id = data.customerProvinceID;
                _order.province = data.customerProvinceName;

                let newOption = new Option(data.customerProvinceName, data.customerProvinceID, false, false);
                $('#ddlProvince').append(newOption).trigger('change');

                // Danh sách quận / huyện
                _disabledDDLDistrict(false);
            }

            if (data.customerProvinceID && data.customerDistrictID) {
                // Danh sách quận / huyện
                _order.district_id = data.customerDistrictID;
                _order.district = data.customerDistrictName;

                let newOption = new Option(data.customerDistrictName, data.customerDistrictID, false, false);
                $('#ddlDistrict').removeAttr('disabled');
                $('#ddlDistrict').removeAttr('readonly');
                $('#ddlDistrict').append(newOption).trigger('change');

                // Danh sách phường / xã
                _disabledDDLWard(false);
            }

            if (data.customerProvinceID && data.customerDistrictID && data.customerWardID != null) {
                // Danh sách quận / huyện
                _order.ward_id = data.customerWardID;
                _order.ward = data.customerWardName;

                let newOption = new Option(data.customerWardName, data.customerWardID, false, false);
                $('#ddlWard').removeAttr('disabled');
                $('#ddlWard').removeAttr('readonly');
                $('#ddlWard').append(newOption).trigger('change');
            }

            if (data.weightMin) {
                _weight_min = parseFloat(data.weightMin);
            }

            if (data.weight) {
                if (data.weight != 0 && weight == 0) {
                    $("#weight").val(data.weight).trigger('blur');
                }
            }

            // nếu đã có phí trong đơn hàng
            if (data.feeShop) {
                _feeShop = data.feeShop;
                $("#divFeeShop").show();
                $("#labelFeeShop").html(_formatThousand(data.feeShop));
                $("#fee_entered").prop('checked', true).trigger('change');
                if (data.money == 0) {
                    $("#feeship_shop").attr("disabled", true);
                    $("#feeship_receiver").attr("disabled", true);
                    $("#feeship_shop").parent().hide();
                    $("#feeship_receiver").parent().hide();
                }
                else {
                    $("#feeship_receiver").attr("disabled", true);
                    $("#feeship_receiver").parent().hide();
                }
            }
            else {
                $("#divFeeShop").hide();
                // nếu không thu hộ
                if (data.money == 0) {
                    $("#feeship_shop").attr("disabled", true);
                    $("#feeship_shop").hide();
                    $("#feeship_shop").parent().hide();
                    $("#feeship_receiver").prop('checked', true).trigger('change');
                }
                else {
                    $("#feeship_shop").removeAttr("disabled");
                    $("#feeship_receiver").attr("disabled", true);
                    $("#feeship_receiver").parent().hide();
                    $("#feeship_shop").prop('checked', true).trigger('change');
                }
            }

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
    //let deliver_work_shift = [];
    let now = new Date();
    let tomorrow = new Date();
    //let dateAfterTomorrow = new Date();
    let strNow = "";
    let strTomorrow = "";
    //let strDateAfterTomorrow = "";

    tomorrow.setDate(tomorrow.getDate() + 1);
    //dateAfterTomorrow.setDate(tomorrow.getDate() + 2);
    strNow = now.toISOString().substring(0, 10).replace(/-/g, '/');
    strTomorrow = tomorrow.toISOString().substring(0, 10).replace(/-/g, '/');
    //strDateAfterTomorrow = dateAfterTomorrow.toISOString().substring(0, 10).replace(/-/g, '/');

    if (pick_work_shift == "Sáng nay") {
        //deliver_work_shift.push({ id: 3, text: "Tối nay" });
        //deliver_work_shift.push({ id: 1, text: "Sáng mai" });
        //deliver_work_shift.push({ id: 2, text: "Chiều mai" });

        _order.pick_date = strNow;
        _order.pick_work_shift = 1;
        //_order.deliver_date = strNow;
        //_order.deliver_work_shift = 3;
    }
    else if (pick_work_shift == "Chiều nay") {
        //deliver_work_shift.push({ id: 3, text: "Tối nay" });
        //deliver_work_shift.push({ id: 1, text: "Sáng mai" });
        //deliver_work_shift.push({ id: 2, text: "Chiều mai" });

        _order.pick_date = strNow;
        _order.pick_work_shift = 2;
        //_order.deliver_date = strNow;
        //_order.deliver_work_shift = 3;
    }
    else if (pick_work_shift == "Tối nay") {
        //deliver_work_shift.push({ id: 1, text: "Sáng mai" });
        //deliver_work_shift.push({ id: 2, text: "Chiều mai" });
        //deliver_work_shift.push({ id: 3, text: "Tối mai" });

        _order.pick_date = strNow;
        _order.pick_work_shift = 3;
        //_order.deliver_date = strTomorrow;
        //_order.deliver_work_shift = 1;
    }
    else if (pick_work_shift == "Sáng mai") {
        //deliver_work_shift.push({ id: 2, text: "Chiều mai" });
        //deliver_work_shift.push({ id: 3, text: "Tối mai" });
        //deliver_work_shift.push({ id: 1, text: "Sáng ngày kia" });

        _order.pick_date = strTomorrow;
        _order.pick_work_shift = 1;
        //_order.deliver_date = strTomorrow;
        //_order.deliver_work_shift = 2;
    }
    else if (pick_work_shift == "Chiều mai") {
        //deliver_work_shift.push({ id: 3, text: "Tối mai" });
        //deliver_work_shift.push({ id: 1, text: "Sáng ngày kia" });
        //deliver_work_shift.push({ id: 2, text: "Chiều ngày kia" });

        _order.pick_date = strTomorrow;
        _order.pick_work_shift = 2;
        //_order.deliver_date = strTomorrow;
        //_order.deliver_work_shift = 3;
    }
    else if (pick_work_shift == "Tối mai") {
        //deliver_work_shift.push({ id: 1, text: "Sáng ngày kia" });
        //deliver_work_shift.push({ id: 2, text: "Chiều ngày kia" });
        //deliver_work_shift.push({ id: 3, text: "Tối ngày kia" });

        _order.pick_date = strTomorrow;
        _order.pick_work_shift = 3;
        //_order.deliver_date = strDateAfterTomorrow;
        //_order.deliver_work_shift = 1;
    }

    //let $deliver_work_shift = $('#deliver_work_shift');

    //// https://github.com/select2/select2/issues/2830
    ////clear chosen items
    //$deliver_work_shift.val(null).trigger('change');

    ////destroy select2
    //$deliver_work_shift.select2("destroy");

    ////remove options physically from the HTML
    //$deliver_work_shift.find("option").remove();

    //$deliver_work_shift.select2({
    //    minimumResultsForSearch: Infinity,
    //    data: deliver_work_shift
    //});
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
          .then(() => { $('#tel').focus(); });

    if (!_order.province)
        return swal({
            title: titleAlert,
            text: "Địa chỉ tỉnh/thành khách hàng chưa chọn",
            icon: "error",
        })
          .then(() => { $("#ddlProvince").select2('open'); });
    if (_order.province && !_order.district)
        return swal({
            title: titleAlert,
            text: "Địa chỉ quận/huyện khách hàng chưa chọn",
            icon: "error",
        })
          .then(() => { $("#ddlDistrict").select2('open'); });
    if (_order.province && _order.district && !_order.ward)
        return swal({
            title: titleAlert,
            text: "Địa chỉ phường/xã khách hàng chưa chọn",
            icon: "error",
        })
          .then(() => { $("#ddlDistrict").select2('open'); });
    if (!_order.address)
        return swal({
            title: titleAlert,
            text: "Địa chỉ khách hàng chưa nhập",
            icon: "error",
        })
          .then(() => { $('#address').focus(); });
    if (!_product.name)
        return swal({
            title: titleAlert,
            text: "Chưa chọn loại sản phẩm",
            icon: "error",
        })
          .then(() => { $("#ddlProduct").select2('open'); });

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

    _calculateFee();
    _submit();
}

function _calculateFee() {
    let $fee = $("#feeship");
    let $insuranceFee = $("#insuranceFee");
    let $insuranceFeeDiv = $(".insurance-fee");

    if (!_order.pick_province
        || !_order.pick_district
        || !_order.province
        || !_order.district
        || !_order.ward
    ) {
        _fee = 0;
        _insuranceFee = 0;

        // Phí GHTK
        $fee.html("0");
        // Phí bảo hiểm
        $insuranceFee.html("0");
        $insuranceFeeDiv.addClass("hiden");
    }
    else {
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
            success: (data, textStatus, xhr) => {
                HoldOn.close();

                if (xhr.status == 200 && data) {
                    if (data.success) {
                        if (_feeShipment == 0 || _feeShipment == 1) {
                            _order.pick_money = _order.pick_money - (_fee - _insuranceFee);
                            _order.value = _order.value - (_fee - _insuranceFee);
                        }
                        else if (_feeShipment == 2) {
                            // thu hộ
                            if (_paymentType == 3) {
                                _order.pick_money = _order.pick_money - _feeShop;
                            }
                            else {
                                _order.pick_money = 0;
                            }

                            _order.value = _order.value - _feeShop;
                        }

                        _fee = data.fee.fee;
                        _insuranceFee = data.fee.insurance_fee;

                        // Phí GHTK
                        $fee.html(_formatThousand(_fee));

                        // Phí bảo hiểm
                        if (_insuranceFee > 0) {
                            $insuranceFee.html(_formatThousand(_insuranceFee));
                            $insuranceFeeDiv.removeClass("hide");
                        }
                        else {
                            $insuranceFee.html("0");
                            $insuranceFeeDiv.addClass("hiden");
                        }

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
    let $pick_money = $("#pick_money");
    let $value = $("#value");
    let $total_money = $("#total_money");
    let feeShipment = +$("input:radio[name='feeship']:checked").val() || 0;
    let total_money = 0;

    if (feeShipment == 0 || feeShipment == 1) {
        _order.pick_money = _order.pick_money + (_fee - _insuranceFee);
        _order.value = _order.value + (_fee - _insuranceFee);
    }
    else if (feeShipment == 2) {
        // thu hộ
        if (_paymentType == 3) {
            _order.pick_money = _order.pick_money + _feeShop;
        }
            // chuyển khoản || tiền mặt || công nợ
        else {
            _order.pick_money = 0;
        }
        _order.value = _order.value + _feeShop;
    }

    _feeShipment = feeShipment;
    $pick_money.val(_formatThousand(_order.pick_money));
    $value.val(_formatThousand(_order.value));
    $total_money.val(_formatThousand(_order.pick_money));
}

function _submit() {
    let titleAlert = "Đồng bộ đơn hàng GHTK";

    //let $value = $("#value");
    //let $total_money = $("#total_money");
    //_order.pick_money = +parseInt($total_money.val().replace(/,/g, '')) || 0;
    //_order.value = +parseInt($value.val().replace(/,/g, '')) || 0;


    $.ajax({
        method: 'POST',
        contentType: 'application/json',
        dataType: "json",
        data: JSON.stringify({ products: [_product], order: _order }),
        url: "/api/v1/delivery-save/register-order",
        beforeSend: function () {
            HoldOn.open();
        },
        success: (data, textStatus, xhr) => {
            HoldOn.close();

            if (xhr.status == 200 && data) {
                if (data.success)
                    return swal({
                        title: titleAlert,
                        text: "Đồng bộ thành công",
                        icon: "success",
                    })
                    .then(() => {
                        let label = data.order['label'];
                        window.location.href = "https://khachhang.giaohangtietkiem.vn/khachhang?code=" + label;
                    });
                else
                    return _alterError(titleAlert, { message: data.message });
            } else {
                return _alterError(titleAlert);
            }
        },
        error: (xhr, textStatus, error) => {
            HoldOn.close();

            return _alterError(titleAlert);
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

function _formatThousand(value) {
    nfObject = new Intl.NumberFormat('en-US');

    return nfObject.format(value);
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

function _updateAddress() {
    if (!_order || !_order.tel || (!_order.province_id && !_order.district_id && !_order.ward_id && !_order.address))
        return;

    let titleAlert = "Cập nhật thông tin địa chỉ khách hàng";
    let dataJSON = JSON.stringify({
        provinceID: _order.province_id,
        districtID: _order.district_id,
        wardID: _order.ward_id,
        address: _order.address
    });

    $.ajax({
        method: 'POST',
        contentType: 'application/json',
        dataType: "json",
        data: dataJSON,
        url: "/api/v1/customer/" + _order.tel + "/update-address",
        success: (response, textStatus, xhr) => {
            if (xhr.status == 200 && response.success) {

            } else {
                _alterError(titleAlert);
            }
        },
        error: (xhr, textStatus, error) => {
            _alterError(titleAlert, xhr.responseJSON);
        }
    });
    _calculateFee();
}