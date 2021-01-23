﻿let deliveryAddresses = [];

// #region Private
// #region Service
function _getDeliveryAddress(deliveryAddressId) {
    return new Promise(function (reslove, reject) {
        $.ajax({
            method: 'GET',
            url: "/api/v1/delivery/address/" + deliveryAddressId,
            success: function (data, textStatus, xhr) {
                reslove(data);
            },
            error: function (xhr, textStatus, err) {
                reject(err);
            }
        });
    });
}

function _getDeliveryAddresses(phone) {
    return new Promise(function (reslove, reject) {
        $.ajax({
            method: 'GET',
            url: "/api/v1/delivery/addresses?phone=" + phone,
            success: function (data, textStatus, xhr) {
                reslove(data);
            },
            error: function (xhr, textStatus, err) {
                reject(err);
            }
        });
    });
}

function _createDeliveryAddresses(deliveryAddress) {
    return new Promise(function (reslove, reject) {
        $.ajax({
            method: 'POST',
            url: '/api/v1/delivery/address',
            contentType: 'application/json; charset=UTF-8',
            data: JSON.stringify(deliveryAddress),
            dataType: 'json',
            success: function (data, textStatus, xhr) {
                reslove(data);
            },
            error: function (xhr, textStatus, err) {
                reject(err);
            }
        });
    });
}
// #endregion

// #region Page
// Province Address
function _initRecipientProvince() {
    $("select[id$='_ddlRecipientProvince']").val(null).trigger('change');

    // Danh sách tỉnh / thành phố
    $("select[id$='_ddlRecipientProvince']").select2({
        width: "100%",
        placeholder: 'Chọn tỉnh thành',
        ajax: {
            delay: 500,
            method: 'GET',
            url: '/api/v1/delivery-save/provinces/select2',
            data: (params) => {
                var query = {
                    search: params.term,
                    page: params.page || 1
                }

                return query;
            }
        }
    });

    // Danh sách quận / huyện
    _disabledRecipientDistrict(true, null);

    // Danh sách phường / xã
    _disabledRecipientWard(true, null);
}

function _onChangeRecipientProvince() {
    // Danh sách tỉnh / thành phố
    $("select[id$='_ddlRecipientProvince']").on('select2:select', (e) => {
        let data = e.params.data;
        $("input[id$='_hdfProvinceID']").val(data.id);
        $("input[id$='_hdfDistrictID']").val(null);
        $("input[id$='_hdfWardID']").val(null);
        _disabledRecipientDistrict(false, data.id);
        $("select[id$='_ddlRecipientDistrict']").select2('open');

        _disabledRecipientWard(true, data.id);
    });

    // Danh sách quận / huyện
    $("select[id$='_ddlRecipientDistrict']").on('select2:select', (e) => {
        let data = e.params.data;

        $("input[id$='_hdfDistrictID']").val(data.id);
        $("input[id$='_hdfWardID']").val(null);
        _disabledRecipientWard(false, data.id);
        $("select[id$='_ddlRecipientWard']").select2('open');
    });

    // Danh sách phường / xã
    $("select[id$='_ddlRecipientWard']").on('select2:select', (e) => {
        let data = e.params.data;

        $("input[id$='_hdfWardID']").val(data.id);
    });
}

function _disabledRecipientDistrict(disabled, provinceID) {
    if (disabled) {
        $("select[id$='_ddlRecipientDistrict']").attr('disabled', true);
        $("select[id$='_ddlRecipientDistrict']").attr('readonly', 'readonly');
        $("select[id$='_ddlRecipientDistrict']").val(null).trigger('change');
        $("select[id$='_ddlRecipientDistrict']").select2({
            width: "100%",
            placeholder: 'Chọn quận huyện'
        });
    }
    else {
        $("select[id$='_ddlRecipientDistrict']").removeAttr('disabled');
        $("select[id$='_ddlRecipientDistrict']").removeAttr('readonly');
        $("select[id$='_ddlRecipientDistrict']").val(null).trigger('change');
        $("select[id$='_ddlRecipientDistrict']").select2({
            width: "100%",
            placeholder: 'Chọn quận huyện',
            ajax: {
                delay: 500,
                method: 'GET',
                url: '/api/v1/delivery-save/province/' + provinceID + '/districts/select2',
                data: (params) => {
                    var query = {
                        search: params.term,
                        page: params.page || 1
                    }

                    return query;
                }
            }
        });
    }
}

function _disabledRecipientWard(disabled, districtID) {
    if (disabled) {
        $("select[id$='_ddlRecipientWard']").attr('disabled', true);
        $("select[id$='_ddlRecipientWard']").attr('readonly', 'readonly');
        $("select[id$='_ddlRecipientWard']").val(null).trigger('change');
        $("select[id$='_ddlRecipientWard']").select2({
            width: "100%",
            placeholder: 'Chọn phường xã'
        });
    }
    else {
        $("select[id$='_ddlRecipientWard']").removeAttr('disabled');
        $("select[id$='_ddlRecipientWard']").removeAttr('readonly');
        $("select[id$='_ddlRecipientWard']").val(null).trigger('change');
        $("select[id$='_ddlRecipientWard']").select2({
            width: "100%",
            placeholder: 'Chọn phường xã',
            ajax: {
                delay: 500,
                method: 'GET',
                url: '/api/v1/delivery-save/district/' + districtID + '/wards/select2',
                data: (params) => {
                    var query = {
                        search: params.term,
                        page: params.page || 1
                    }

                    return query;
                }
            }
        });
    }
}

function _initDeliveryAddress(data) {
    // ID địa chị nhận hàng
    $("input[id$='_hdfDeliveryAddressId']").val(data.id);
    // Họ tên người nhận hàng
    $("input[id$='_txtRecipientFullName']").val(data.fullName);
    $("input[id$='_hdfRecipientFullName']").val(data.fullName);
    // SĐT người nhận
    $("input[id$='_txtRecipientPhone']").val(data.phone)
    $("input[id$='_hdfRecipientPhone']").val(data.phone);

    // Danh sách tỉnh / thành phố
    let newProvinceOption = new Option(data.provinceName, data.provinceId, false, false);
    $("select[id$='_ddlRecipientProvince']").find("option").remove();
    $("select[id$='_ddlRecipientProvince']").append(newProvinceOption).trigger('change');
    $("input[id$='_hdfRecipientProvinceId']").val(data.provinceId);
    _disabledRecipientDistrict(false, data.provinceId);

    // Danh sách quận / huyện
    let newDistrictOption = new Option(data.districtName, data.districtId, false, false);
    $("select[id$='_ddlRecipientDistrict']").removeAttr('disabled');
    $("select[id$='_ddlRecipientDistrict']").removeAttr('readonly');
    $("select[id$='_ddlRecipientDistrict']").find("option").remove();
    $("select[id$='_ddlRecipientDistrict']").append(newDistrictOption).trigger('change');
    $("input[id$='_hdfRecipientDistrictId']").val(data.districtId);
    _disabledRecipientWard(false, data.districtId);

    // Danh sách phường / xã
    let newWardOption = new Option(data.wardName, data.wardId, false, false);
    $("select[id$='_ddlRecipientWard']").removeAttr('disabled');
    $("select[id$='_ddlRecipientWard']").removeAttr('readonly');
    $("select[id$='_ddlRecipientWard']").find("option").remove();
    $("select[id$='_ddlRecipientWard']").append(newWardOption).trigger('change');
    $("input[id$='_hdfRecipientWardId']").val(data.wardId);

    if (data.address) {
        $("input[id$='_txtRecipientAddress']").val(data.address);
        $("input[id$='_hdfOldRecipientAddress']").val(data.address);
        $("input[id$='_hdfRecipientAddress']").val(data.address);
    }
}

function _gennerateDeliveryAddesses(deliveryAddresses) {
    let deliveryAddressId = $("input[id$='_hdfDeliveryAddressId']").val();
    let html = '';

    html += '<div class="form-row">';
    html += '    <label>Danh sách địa chỉ nhận hàng</label>';
    html += '    <div class="delivery-addresses">';
    html += '        <table class="table table-checkable table-product">';
    html += '            <thead>';
    html += '                <tr>';
    html += '                    <th class="name-column">Họ tên</th>';
    html += '                    <th class="phone-column">Điện thoại</th>';
    html += '                    <th class="province-column">Tỉnh thành</th>';
    html += '                    <th class="disctrict-column">Quận huyện</th>';
    html += '                    <th class="ward-column">Phường xã</th>';
    html += '                    <th class="address-column">Địa chỉ</th>';
    html += '                </tr>';
    html += '            </thead>';
    html += '        </table>';
    html += '        <div class="form-row list-customer scrollbar">';
    html += '            <table class="table table-checkable table-product table-list-customer">';
    html += '                <tbody>';
    deliveryAddresses.forEach(function (element, index) {
        if (String(element.id) === deliveryAddressId) {
            html += '                    <tr>';
            html += '                        <td class="name-column" style="background-color: yellow">' + element.fullName + '</td>';
            html += '                        <td class="phone-column" style="background-color: yellow">' + element.phone + '</td>';
            html += '                        <td class="province-column" style="background-color: yellow">' + element.provinceName + '</td>';
            html += '                        <td class="disctrict-column" style="background-color: yellow">' + element.districtName + '</td>';
            html += '                        <td class="ward-column" style="background-color: yellow">' + element.wardName + '</td>';
            html += '                        <td class="address-column" style="background-color: yellow">' + element.address + '</td>';
            html += '                    </tr>';
        }
        else {
            html += '                    <tr tabindex="' + index + '" onclick="onClickDeliveryAddressTable(' + element.id + ')">';
            html += '                        <td class="name-column">' + element.fullName + '</td>';
            html += '                        <td class="phone-column">' + element.phone + '</td>';
            html += '                        <td class="province-column">' + element.provinceName + '</td>';
            html += '                        <td class="disctrict-column">' + element.districtName + '</td>';
            html += '                        <td class="ward-column">' + element.wardName + '</td>';
            html += '                        <td class="address-column">' + element.address + '</td>';
            html += '                    </tr>';
        }
    });
    html += '                </tbody>';
    html += '            </table>';
    html += '        </div>';
    html += '    </div>';
    html += '</div>';

    return html;
}

function _clearDeliveryAddress() {
    // #region thông tin địa chỉ giao hàng mới
    $("input[id$='_hdfDeliveryAddressId']").val(null);
    $("input[id$='_hdfRecipientFullName']").val(null);
    $("input[id$='_hdfRecipientPhone']").val(null);
    $("input[id$='_hdfRecipientProvinceId']").val(null);
    $("input[id$='_hdfRecipientDistrictId']").val(null);
    $("input[id$='_hdfRecipientWardId']").val(null);
    $("input[id$='_hdfRecipientAddress']").val(null);
    // #endregion

    $("input[id$='_txtRecipientFullName']").val('');
    $("input[id$='_txtRecipientPhone']").val('');
    $("select[id$='_ddlRecipientProvince']").find("option").remove();
    $("select[id$='_ddlRecipientDistrict']").find("option").remove();
    $("select[id$='_ddlRecipientWard']").find("option").remove();
    $("input[id$='_txtRecipientAddress']").val('');
}
// #endregion
// #endregion

// #region Public
function initDeliveryAddress() {
    _initRecipientProvince();
    _onChangeRecipientProvince();

    let hdfDeliveryAddressDOM = $("input[id$='_hdfDeliveryAddressId']");

    if (hdfDeliveryAddressDOM.val())
        _getDeliveryAddress(hdfDeliveryAddressDOM.val())
            .then(function (data) {
                _initDeliveryAddress(data);
            })
            .catch(function (err) {
                console.log(err);
            });
}

function showDeliveryAddresses() {
    let phone = $('input[id$="_txtPhone"]');

    if (phone.val()) {
        HoldOn.open();
        deliveryAddresses = [];

        _getDeliveryAddresses(phone.val())
            .then(function (data) {
                HoldOn.close();
                deliveryAddresses = [...data]
                showPopup(_gennerateDeliveryAddesses(deliveryAddresses), 9);
            })
            .catch(function (err) {
                HoldOn.close();

                return swal({
                    title: 'Error',
                    text: 'Đã có lỗi trong việc lấy danh sách địa chỉ nhận hàng',
                    type: 'error',
                    showCloseButton: true,
                    html: true,
                });
            })
    }
    else {
        return swal({
            title: 'Error',
            text: 'Thông tin số điện thoại khách hàng đang rỗng',
            type: 'error',
            showCloseButton: true,
            html: true,
        });
    }
}

function onClickDeliveryAddressTable(id) {
    _clearDeliveryAddress();

    let data = deliveryAddresses.filter(x => x.id == id);

    if (data.length == 1)
        _initDeliveryAddress(data[0]);

    closePopup();
}

function checkDeliveryAddressValidation() {
    let $name = $("input[id$='_txtRecipientFullName']");
    let $phone = $("input[id$='_txtRecipientPhone']");
    let $province = $("select[id$='_ddlRecipientProvince']");
    let $district = $("select[id$='_ddlRecipientDistrict']");
    let $ward = $("select[id$='_ddlRecipientWard']");
    let $address = $("input[id$='_txtRecipientAddress']");

    // Tên khách hàng
    if (!$name.val()) {
        $name.focus();
        swal("Thông báo", "Hãy nhập tên khách hàng!", "error");
        return false;
    }

    // SDT khách hàng
    if (!$phone.val()) {
        $phone.focus();
        swal("Thông báo", "Hãy nhập số điện thoại khách hàng!", "error");
        return false;
    }

    // Tỉnh / thành phố
    if ((+$province.val() || 0) === 0) {
        swal({
            title: "Thông báo",
            text: "Chưa chọn tỉnh thành",
            type: "warning",
            showCancelButton: false,
            confirmButtonText: "Để em xem lại!!",
            closeOnConfirm: false,
            html: true
        }, function (isConfirm) {
            if (isConfirm) {
                sweetAlert.close();
                $province.select2('open');
            }
        });
        return false;
    }

    // Quận / huyện
    if ((+$district.val() || 0) === 0) {
        swal({
            title: "Thông báo",
            text: "Chưa chọn quận huyện",
            type: "warning",
            showCancelButton: false,
            confirmButtonText: "Để em xem lại!!",
            closeOnConfirm: false,
            html: true
        }, function (isConfirm) {
            if (isConfirm) {
                sweetAlert.close();
                $district.select2('open');
            }
        });
        return false;
    }

    // Phường / xã
    if ((+$ward.val() || 0) === 0) {
        swal({
            title: "Thông báo",
            text: "Chưa chọn phường xã",
            type: "warning",
            showCancelButton: false,
            confirmButtonText: "Để em xem lại!!",
            closeOnConfirm: false,
            html: true
        }, function (isConfirm) {
            if (isConfirm) {
                sweetAlert.close();
                $ward.select2('open');
            }
        });
    }

    // Địa chỉ
    if (!$address.val()) {
        $address.focus();
        swal("Thông báo", "Hãy nhập địa chỉ khách hàng!", "error");
        return false;
    }

    return true;
}

function updateDeliveryAddress(phone) {
    let $hdfDeliveryAddress = $("input[id$='_hdfDeliveryAddressId']");
    let $name = $("input[id$='_txtRecipientFullName']");
    let $phone = $("input[id$='_txtRecipientPhone']");
    let $province = $("select[id$='_ddlRecipientProvince']");
    let $district = $("select[id$='_ddlRecipientDistrict']");
    let $ward = $("select[id$='_ddlRecipientWard']");
    let $address = $("input[id$='_txtRecipientAddress']");

    if (!$hdfDeliveryAddress.val()) {
        let deliveryAddress = {
            "fullName": $name.val().trim(),
            "phone": $phone.val().trim(),
            "address": $address.val() ? $address.val().trim() : null,
            "provinceId": +$province.val() || 0,
            "districtId": +$district.val() || 0,
            "wardId": +$ward.val() || 0,
            "createdBy": phone
        };

        return _createDeliveryAddresses(deliveryAddress)
            .then(function (data) {
                _initDeliveryAddress(data);

                return true;
            })
            .catch(function (err) {
                swal("Thông báo", "Cập nhật địa chỉ giao hàng. Thất bại :(", "error");

                return false;
            });
    }
    else {
        let $hdfName = $("input[id$='_hdfRecipientFullName']");
        let $hdfPhone = $("input[id$='_hdfRecipientPhone']");
        let $hdfProvince = $("input[id$='_hdfRecipientProvinceId']");
        let $hdfDistrict = $("input[id$='_hdfRecipientDistrictId']");
        let $hdfWard = $("input[id$='_hdfRecipientWardId']");
        let $hdfAddress = $("input[id$='_hdfRecipientAddress']");

        if ($hdfName.val() !== $name.val()
            || $hdfPhone.val() !== $phone.val()
            || $hdfProvince.val() !== $province.val()
            || $hdfDistrict.val() !== $district.val()
            || $hdfWard.val() !== $ward.val()
            || $hdfAddress.val() !== $address.val()
        ) {
            let deliveryAddress = {
                "fullName": $name.val().trim(),
                "phone": $phone.val().trim(),
                "address": $address.val() ? $address.val().trim() : null,
                "provinceId": +$province.val() || 0,
                "districtId": +$district.val() || 0,
                "wardId": +$ward.val() || 0,
                "createdBy": phone
            };

            return _createDeliveryAddresses(deliveryAddress)
                .then(function (data) {
                    _initDeliveryAddress(data);

                    return true;
                })
                .catch(function (err) {
                    swal("Thông báo", "Cập nhật địa chỉ giao hàng. Thất bại :(", "error");

                    return false;
                });
        }
    }

    return new Promise(function (reslove, reject) {
        reslove(true);
    });
}
// #endregion