let strFormat = new StringFormat();
let orderService = new GhtkReportService();
let controller = new GhtkReportController();

document.addEventListener("DOMContentLoaded", function (event) {
    _initQueryParams();
    _initBtnUpload();
    _initReportTable();
});

//#region Private
function _loadSpanReport() {
    let spanReportDOM = document.querySelector("[id$='spanReport']");
    let spanReport = "";

    if (controller.pagination.totalCount > 0)
    {
        spanReport += "(";
        spanReport += UtilsService.formatThousands(controller.pagination.totalCount, ',');
        spanReport += " số đơn GHTK - Số tiền phí lệch: ";
        spanReport += UtilsService.formatThousands(controller.lossMoney, ',');
        spanReport += " VND)";
    }

    spanReportDOM.innerHTML = spanReport;
}

function _initQueryParams() {
    let search = window.location.search;

    controller.setFilter(search);
}

function _initBtnUpload() {
    let $btnUpload = $("#btnUpload");

    $btnUpload.attr("disabled", "disabled");
}

function _initReportTable() {
    HoldOn.open();

    controller.getGhtkReports()
        .then(function (response) {
            controller.lossMoney = response.lossMoney;
            controller.data = response.data
            controller.pagination = response.pagination;

            _loadSpanReport();
            _loadPagination();
            _loadReportTable();

            HoldOn.close();
        })
        .catch(function (e) {
            console.log(e);
            HoldOn.close();
        });
}

function _updateFilter() {
    // Tìm kiếm đơn hàng
    let searchDOM = document.querySelector("[id$='_txtSearch']");

    if (searchDOM.value)
        controller.filter.search = searchDOM.value;
    else
        controller.filter.search = null;

    // Trạng thái phí vận chuyển
    let feeStatusDOM = document.querySelector("[id$='_ddlFeeStatus']");

    if (feeStatusDOM.value)
        controller.filter.feeStatus = +feeStatusDOM.value || null;
    else
        controller.filter.feeStatus = null;

    // Khoảng thời gian
    let fromDateDOM = document.querySelector("[id$='_dpFromDate_dateInput']");
    let toDateDOM = document.querySelector("[id$='_dpToDate_dateInput']");

    if (fromDateDOM.value.trim()) {
        let date = fromDateDOM.value.split('/').filter(x => x);

        if (date.length == 3)
            controller.filter.fromDate = date[1] + '/' + date[0] + '/' + date[2];
    }
    else {
        controller.filter.fromDate = '12/15/2019';
    }

    if (toDateDOM.value.trim()) {
        let date = toDateDOM.value.split('/').filter(x => x);

        if (date.length == 3)
            controller.filter.toDate = date[1] + '/' + date[0] + '/' + date[2];
    }
    else {
        controller.filter.toDate = strFormat.datetimeToString(new Date(), 'MM/dd/yyyy')
    }

    // Trạng thái đơn hàng
    let orderStatusDOM = document.querySelector("[id$='_ddlOrderStatus']");

    if (orderStatusDOM.value)
        controller.filter.orderStatus = +orderStatusDOM.value || null;
    else
        controller.filter.orderStatus = null;

    // Trạng thái GHTK
    let ghtkStatusDOM = document.querySelector("[id$='_ddlGhtkStatus']");

    if (ghtkStatusDOM.value)
        controller.filter.ghtkStatus = +ghtkStatusDOM.value || null;
    else
        controller.filter.ghtkStatus = null;

    // Trạng thái duyệt
    let reviewStatusDOM = document.querySelector("[id$='_ddlReviewStatus']");

    if (reviewStatusDOM.value)
        controller.filter.reviewStatus = +reviewStatusDOM.value || null;
    else
        controller.filter.reviewStatus = null;
}

function _replaceUrl() {
    let title = document.title;
    let url = window.location.pathname;
    let queryParam = controller.generateQueryParams();

    if (queryParam)
        url += '?' + queryParam

    window.history.replaceState({}, title, url);
}

function _createPaginationHTML(pagination, pageNumberDisplay) {
    let html = "";

    // Trường hợp chỉ có một trang
    if (pagination.totalPages == 0)
        return html;

    let start = 0;
    let end = 0;

    if (pagination.totalPages < pageNumberDisplay) {
        start = 1;
        end = pagination.totalPages;
    }
    else {
        if (pagination.page <= Math.ceil(pageNumberDisplay / 2.0)) {
            start = 1;
            end = pageNumberDisplay;
        }
        else if (pagination.page > pagination.totalPages - Math.ceil(pageNumberDisplay / 2.0)) {
            start = pagination.totalPages - pageNumberDisplay + 1;
            end = pagination.totalPages;
        }
        else {
            start = pagination.page - Math.ceil(pageNumberDisplay / 2.0);
            end = start + pageNumberDisplay - 1;
        }
    }

    html += "<ul>";

    if (pagination.previousPage == 'Yes') {
        html += "<li><a title='Trang đầu' href='javascript:;' onclick='onClickPagination(1)'>Trang đầu</a></li>";
        html += "<li><a title='Trang trước' href='javascript:;' onclick='onClickPagination(" + (pagination.page - 1).toString() + ")'>Trang trước</a></li>";

        if (start > Math.ceil(length / 2.0))
            html += "<li><a href='javascript:;'>&hellip;</a></li>";
    }

    for (var page = start; page <= end; page++)
        if (pagination.page == page)
            html += "<li class='current'><a>" + page + "</a></li>";
        else
            html += "<li><a href='javascript:;' onclick='onClickPagination(" + page + ")'>" + page + "</a></li>";

    if (pagination.nextPage == 'Yes') {
        if (end < pagination.totalPages - Math.ceil(length / 2.0))
            html += "<li><a href='javascript:;'>&hellip;</a></li>";

        html += "<li><a title='Trang sau' href='javascript:;' onclick='onClickPagination(" + (pagination.page + 1).toString() + ")'>Trang sau</a></li>";
        html += "<li><a title='Trang cuối' href='javascript:;' onclick='onClickPagination(" + pagination.totalPages + ")'>Trang cuối</a></li>";
    }

    html += "</ul>";

    return html;
}

function _loadPagination() {
    let pageNumberDisplay = 6;
    let paginationDOM = document.querySelectorAll(".pagination");
    let html = "";

    if (controller.pagination.totalPages > 1)
        html = _createPaginationHTML(controller.pagination, pageNumberDisplay);

    paginationDOM.forEach(function (element) {
        element.innerHTML = html;
    })
}

function _createReportTableHTML(data) {
    let html = "";

    html += "<thead>";
    html += "    <tr>";
    html += "        <th>Mã</th>";
    html += "        <th class='col-customer'>Khách hàng</th>";
    html += "        <th>Điện Thoại</th>";
    html += "        <th>Trạng thái</th>";
    html += "        <th>Ngày gửi</th>";
    html += "        <th>Ngày phát</th>";
    html += "        <th>COD <br/>(GHTK)</th>";
    html += "        <th>COD <br/>(Hệ thống)</th>";
    html += "        <th>Phí <br/>(GHTK)</th>";
    html += "        <th>Phí <br/>(Hệ thống)</th>";
    html += "        <th>Nhân viên</th>";
    html += "        <th></th>";
    html += "    </tr>";
    html += "</thead>";
    html += "<tbody>";

    if (data.length == 0)
        html += "    <tr><td colspan='12'>Không tìm thấy báo cáo...</td></tr>";
    else
        data.forEach(function (item) {
            try {
                html += "    <tr data-id='" + item.id + "' data-order-id='" + (item.orderId ? item.orderId : '') + "'>";
                if (item.orderId)
                {
                    // Mã hóa đơn
                    html += "        <td data-title='Mã hóa đơn'>";
                    html += "            <a target='_blank' href='/thong-tin-don-hang?id=" + item.orderId + "'>" + item.orderId + "</a>";
                    html += "        </td>";
                    // Khách hàng
                    html += "        <td data-title='Khách hàng' class='customer-td'>";
                    html += "            <a target='_blank' href='/thong-tin-don-hang?id=" + item.orderId + "'>" + strFormat.toTitleCase(item.customerName) + "</a>";
                    html += "        </td>";
                }
                else
                {
                    // Mã hóa đơn
                    html += "        <td data-title='Mã hóa đơn'>";
                    html += "        </td>";
                    // Khách hàng
                    html += "        <td data-title='Khách hàng' class='customer-td'>";
                    html += "            <a href='javascript:;'>" + strFormat.toTitleCase(item.customerName) + "</a>";
                    html += "        </td>";
                }
                // Điện thoại
                html += "        <td data-title='Điện Thoại'>";
                html += "            <a target='_blank' href='/danh-sach-don-hang?&searchtype=1&textsearch=" + item.phone + "&shippingtype=6'>" + item.phone + "</a>";
                html += "        </td>";
                // Trạng thái GHTK
                html += "        <td data-title='Trạng thái'>";
                html += "            " + item.orderStatus;
                html += "        </td>";
                // Ngày gửi
                html += "        <td  data-title='Ngày gửi'>";
                html += "            " + strFormat.datetimeToString(item.createdDate, 'dd/MM/yyyy');
                html += "        </td>";
                // Ngày phát
                html += "        <td  data-title='Ngày phát'>";
                html += "            " + strFormat.datetimeToString(item.doneDate, 'dd/MM/yyyy');
                html += "        </td>";
                // COD (GHTK)
                html += "        <td data-title='COD (GHTK)'>";
                html += "            <strong>" + UtilsService.formatThousands(item.ghtkCod, ',') + "</strong>";
                html += "        </td>";
                // COD (Hệ thống)
                html += "        <td data-title='COD (Hệ thống)'>";
                if (item.cod)
                    html += "            <strong>" + UtilsService.formatThousands(item.cod, ',') + "</strong>";
                html += "        </td>";
                // Phí (GHTK)
                html += "        <td data-title='Phí (GHTK)'>";
                html += "            <strong>" + UtilsService.formatThousands(item.ghtkFee, ',') + "</strong>";
                html += "        </td>";
                // Phí (Hệ thống)
                html += "        <td data-title='Phí (Hệ thống)'>";
                if (item.fee)
                    html += "            <strong>" + UtilsService.formatThousands(item.fee, ',') + "</strong>";
                html += "        </td>";
                // Nhân viên
                html += "        <td data-title='Nhân viên tạo đơn'>";
                if (item.staff)
                    html += "            " + item.staff;
                html += "        </td>";
                // Thao tác
                html += "        <td class='handle-button' data-title='Thao tác'>";
                // Nếu khác trạng thái approve
                if (item.reviewStatus != 2)
                {
                    html += "            <button type='button'";
                    html += "                class='btn primary-btn h45-btn'";
                    html += "                title='Duyệt đơn hàng'";
                    html += "                style='background-color: #73a724'";
                    html += "                onclick='openApproveModal(" + item.id + ", " + item.orderId + ")'";
                    html += "            >";
                    html += "                <span class='glyphicon glyphicon-check'></span>";
                    html += "            </button>";
                }
                html += "        </td>";
                html += "    </tr>";

                html += "    <tr class='tr-more-info'>";
                // Mã đơn GHTK
                html += "        <td colspan='2' data-title='Thông tin mã vẫn đơn'>";
                html += "            <span class='bg-blue' style='display: inherit'><strong>" + item.ghtkCode + "</strong></span>";
                html += "        </td>";
                // Thông tin file name và địa chỉ
                html += "        <td colspan='10' data-title='Thông tin thêm địa chỉ khách hàng'>";
                html += "            <span class='order-info'><strong>FileName:</strong> " + item.fileName + "</span>";
                if (item.address)
                    html += "            <br><span class='order-info'><strong>Địa chỉ:</strong> " + item.address + "</span>";
                html += "        </td>";
                html += "    </tr>";
            } catch (e) {
                console.log(item);
                console.log(e);
                return false;
            }
        });

    html += "<tbody>";

    return html;
}

function _loadReportTable() {
    let tbReportDOM = document.querySelector("#tbReport");
    let html = _createReportTableHTML(controller.data);

    tbReportDOM.innerHTML = html;
}
//#endregion

//#region Public
function onChangeFileName(name) {
    let $btnUpload = $("#btnUpload");

    if (name || name.trim())
        $btnUpload.removeAttr("disabled");
    else
        $btnUpload.attr("disabled", "disabled");
}

function onChangeUpload($file) {
    let file = $file[0].files[0] || null;
    let txtFileNameDOM = document.querySelector('#txtFileName');
    let lbUploadStatusDOM = document.querySelector("[id$='_lbUploadStatus']");

    if (file) {
        txtFileNameDOM.value = file.name.replace('.xls', '');
        txtFileNameDOM.disabled = false;
    }
    else {
        txtFileNameDOM.value = "";
        txtFileNameDOM.disabled = true;
    }

    txtFileNameDOM.dispatchEvent(new Event('change'));
    lbUploadStatusDOM.innerHTML = "";
}

function onClickUpload(name, $file) {
    let lbUploadStatusDOM = document.querySelector("[id$='_lbUploadStatus']");
    let file = $file[0].files[0] || null;

    if (file) {
        HoldOn.open();

        controller.uploadGhtkReports(name, file)
            .then(function (response) {
                HoldOn.close();

                if (response.success) {
                    let txtFileNameDOM = document.querySelector('#txtFileName');

                    txtFileNameDOM.value = "";
                    txtFileNameDOM.disabled = true;
                    txtFileNameDOM.dispatchEvent(new Event('change'));
                    lbUploadStatusDOM.innerText = 'Import file thành công';

                    onClickSearch();
                }
                else
                    lbUploadStatusDOM.innerText = 'Thông báo: Đã có lỗi xãy ra trong quá trình import file';
            })
            .catch(function (e) {
                console.log(e);
                HoldOn.close();

                let response = e.responseJSON;

                if (e.status == 400)
                {
                    let errorHtml = '';

                    errorHtml += 'Thông báo: ' + response.message;

                    if (response.error && response.error.length > 0)
                        response.error.forEach(function (item) {
                            for (key in item) {
                                errorHtml += '<br>' + key + ': ' + item[key];
                            };
                        });

                    lbUploadStatusDOM.innerHTML = errorHtml;
                }
                else
                    lbUploadStatusDOM.innerText = 'Thông báo: ' + response.message;
            });
    }
    else {
        lbUploadStatusDOM.innerText = "Vui lòng nhập file import";
    }
}

function onKeyUpSearch(event) {
    if (event.keyCode === 13)
        onClickSearch();
}

function onClickSearch() {
    HoldOn.open();
    controller.pagination.page = 1;
    _updateFilter();
    _replaceUrl();

    controller.getGhtkReports()
        .then(function (response) {
            HoldOn.close();
            controller.lossMoney = response.lossMoney;
            controller.data = response.data
            controller.pagination = response.pagination;

            _loadSpanReport();
            _loadPagination();
            _loadReportTable();
        })
        .catch(function (e) {
            console.log(e);
            HoldOn.close();
        });
};

function onClickPagination(page) {
    HoldOn.open();
    controller.pagination.page = page;
    _replaceUrl();

    controller.getGhtkReports()
        .then(function (response) {
            HoldOn.close();
            controller.lossMoney = response.lossMoney;
            controller.data = response.data
            controller.pagination = response.pagination;

            _loadSpanReport();
            _loadPagination();
            _loadReportTable();
        })
        .catch(function (e) {
            console.log(e);
            HoldOn.close();
        });
}

function openApproveModal(deliverySaveId, orderId) {
    if (!deliverySaveId)
        return;

    let $modal = $("#approveModal");
    let $orderId = $modal.find("[id$='_txtOrderId']");
    let $deliverySaveId = $modal.find("[id$='_hdfdeliverySaveId']");

    //#region Init giá trị cho modal
    $deliverySaveId.val(deliverySaveId);

    if ($orderId)
        $orderId.val(orderId);
    else
        $orderId.val('');
    //#endregion

    $modal.modal({ show: 'true', backdrop: 'static' });
}

function onClickApprove() {
    let $modal = $("#approveModal");
    let $orderId = $modal.find("[id$='_txtOrderId']");
    let $deliverySaveId = $modal.find("[id$='_hdfdeliverySaveId']");

    if (!$deliverySaveId.val() && !$orderId.val())
        return;

    $modal.find("#closeApprove").click();
    HoldOn.open();

    controller.approveRecord($deliverySaveId.val(), $orderId.val())
        .then(function (response) {
            HoldOn.close();

            if (response.success) {
                let $handleButton = $("tr[data-id='" + $deliverySaveId.val() + "'").find(".handle-button");

                $handleButton.html('');
                $handleButton.append("<span class='bg-blue-hoki'>Đã duyệt</span>");
                swal("Thông báo", "Đã duyệt thành công!", "success");
            }
            else {
                swal("Thông báo", response.message, "error");
            }
        })
        .catch(function (e) {
            console.log(e);
            HoldOn.close();

            let response = e.responseJSON;

            if (response) {
                if (response.message)
                    swal("Thông báo", e.responseJSON.message, "error");
                else
                    swal("Thông báo", "Đã có lỗi trong quá trình duyệt đơn GHTK", "error");
            }
            else
                swal("Thông báo", "Đã có lỗi trong quá trình duyệt đơn GHTK", "error");
        });;
}
//#endregion