// Province Address
function initRecipientProvince() {
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
    disabledRecipientDistrict(true, 0);

    // Danh sách phường / xã
    disabledRecipientWard(true, 0);
}

function onChangeRecipientProvince() {
    // // Danh sách tỉnh / thành phố
    $("select[id$='_ddlRecipientProvince']").on('select2:select', (e) => {
        let data = e.params.data;
        $("input[id$='_hdfProvinceID']").val(data.id);
        $("input[id$='_hdfDistrictID']").val(0);
        $("input[id$='_hdfWardID']").val(0);
        disabledRecipientDistrict(false, data.id);
        $("select[id$='_ddlRecipientDistrict']").select2('open');

        disabledRecipientWard(true, data.id);
    });

    // Danh sách quận / huyện
    $("select[id$='_ddlRecipientDistrict']").on('select2:select', (e) => {
        let data = e.params.data;

        $("input[id$='_hdfDistrictID']").val(data.id);
        $("input[id$='_hdfWardID']").val(0);
        disabledRecipientWard(false, data.id);
        $("select[id$='_ddlRecipientWard']").select2('open');
    });

    // Danh sách phường / xã
    $("select[id$='_ddlRecipientWard']").on('select2:select', (e) => {
        let data = e.params.data;

        $("input[id$='_hdfWardID']").val(data.id);
    });
}

function disabledRecipientDistrict(disabled, provinceID) {
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

function disabledRecipientWard(disabled, districtID) {
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

function getDeliveryAddress(deliveryAddressId) {
    $.ajax({
        method: 'GET',
        url: "/api/v1/delivery/address/" + deliveryAddressId,
        success: (data, textStatus, xhr) => {
            // province
            if (data.provinceID) {
                let newOption = new Option(data.provinceName, data.provinceID, false, false);
                $("select[id$='_ddlRecipientProvince']").find("option").remove();
                $("select[id$='_ddlRecipientProvince']").append(newOption).trigger('change');

                $("input[id$='_hdfProvinceID']").val(data.provinceID);
                // Danh sách quận / huyện
                disabledRecipientDistrict(false, data.provinceID);
            }
            if (data.provinceID && data.districtID) {
                let newOption = new Option(data.districtName, data.districtID, false, false);
                $("select[id$='_ddlRecipientDistrict']").removeAttr('disabled');
                $("select[id$='_ddlRecipientDistrict']").removeAttr('readonly');
                $("select[id$='_ddlRecipientDistrict']").find("option").remove();
                $("select[id$='_ddlRecipientDistrict']").append(newOption).trigger('change');

                $("input[id$='_hdfProvinceID']").val(data.provinceID);
                $("input[id$='_hdfDistrictID']").val(data.districtID);
                // Danh sách phường / xã
                disabledRecipientWard(false, data.districtID);
            }
            if (data.provinceID && data.districtID && data.wardID) {
                let newOption = new Option(data.wardName, data.wardID, false, false);
                $("select[id$='_ddlRecipientWard']").removeAttr('disabled');
                $("select[id$='_ddlRecipientWard']").removeAttr('readonly');
                $("select[id$='_ddlRecipientWard']").find("option").remove();
                $("select[id$='_ddlRecipientWard']").append(newOption).trigger('change');

                $("input[id$='_hdfProvinceID']").val(data.provinceID);
                $("input[id$='_hdfDistrictID']").val(data.districtID);
                $("input[id$='_hdfWardID']").val(data.wardID);
            }
        },
        error: (xhr, textStatus, error) => {
        }
    });
}

function clearDeliveryAddress() {
    $("input[id$='_hdfRecipientProvinceID']").val(0);
    $("input[id$='_hdfRecipientDistrictID']").val(0);
    $("input[id$='_hdfRecipientWardID']").val(0);

    $("select[id$='_ddlRecipientProvince']").find("option").remove();
    $("select[id$='_ddlRecipientDistrict']").find("option").remove();
    $("select[id$='_ddlRecipientWard']").find("option").remove();

    initRecipientProvince();
}