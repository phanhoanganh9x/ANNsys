﻿let strFormat = new StringFormat();
let orderService = new GhtkNotificationService();
let controller = new GhtkNotificationController();

document.addEventListener("DOMContentLoaded", function (event) {
    _initQueryParams();
    _initGhtkStatus();
    _initNotificationTable();
});

//#region Private
function _loadSpanReport() {
    let spanReportDOM = document.querySelector("[id$='spanReport']");
    let spanReport = "";

    if (controller.pagination.totalCount > 0)
    {
        spanReport += "(";
        spanReport += UtilsService.formatThousands(controller.pagination.totalCount, ',');
        spanReport += " số đơn GHTK";
        spanReport += ")";
    }

    spanReportDOM.innerHTML = spanReport;
}

function _initQueryParams() {
    let $hdfStaff = $("[id$='_hdfStaff']");
    let search = window.location.search;

    controller.setFilter(search);
    controller.filter.staff = $hdfStaff.val();
}

function _initGhtkStatus() {
    let $ddlGhtkStatus = $('#ddlGhtkStatus');

    // Cài đặt giá trị ban đầu
    $ddlGhtkStatus.val(null).trigger('change');

    if (controller.filter.ghtkStatus)
        controller.getGhtkStatus()
            .then(function (response) {
                let newOption = new Option(response.value, response.key, false, false);
                $ddlGhtkStatus.find("option").remove();
                $ddlGhtkStatus.append(newOption).trigger('change');
            })
            .catch(function (e) {
                console.log(e);
            });

    // Cài đặt API
    $ddlGhtkStatus.select2({
        placeholder: 'Trạng thái đơn GHHTK',
        ajax: {
            delay: 500,
            method: 'GET',
            url: '/api/v1/delivery-save/statuses/selected2',
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

function _initNotificationTable() {
    HoldOn.open();

    controller.getGhtkNotifications()
        .then(function (response) {
            controller.lossMoney = response.lossMoney;
            controller.data = response.data
            controller.pagination = response.pagination;

            _loadSpanReport();
            _loadPagination();
            _loadNotificationTable();

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

    // Trạng thái GHTK
    let ghtkStatusDOM = document.querySelector("#ddlGhtkStatus");

    if (ghtkStatusDOM.value) {
        if (ghtkStatusDOM.value == "0")
            controller.filter.ghtkStatus = 0;
        else
            controller.filter.ghtkStatus = +ghtkStatusDOM.value || null;
    }
    else
        controller.filter.ghtkStatus = null;

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
    if (controller.filter.staff == "admin")
        html += "        <th>Nhân viên</th>";
    html += "    </tr>";
    html += "</thead>";
    html += "<tbody>";

    if (data.length == 0)
        html += "    <tr><td colspan='12'>Không tìm thấy báo cáo...</td></tr>";
    else
        data.forEach(function (item) {
            try {
                html += "    <tr data-order-id='" + item.orderId + "'>";
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
                html += "            <a target='_blank' href='/danh-sach-don-hang?&searchtype=1&textsearch=" + item.customerPhone + "&shippingtype=6'>" + item.customerPhone + "</a>";
                html += "        </td>";
                // Trạng thái GHTK
                html += "        <td data-title='Trạng thái'>";
                html += "            " + item.ghtkStatus;
                html += "        </td>";
                // Ngày gửi
                html += "        <td  data-title='Ngày gửi'>";
                html += "            " + strFormat.datetimeToString(item.orderDate, 'dd/MM/yyyy');
                html += "        </td>";
                // Ngày phát
                html += "        <td  data-title='Ngày phát'>";
                html += "            " + strFormat.datetimeToString(item.ghtkDate, 'dd/MM/yyyy');
                html += "        </td>";
                // COD (GHTK)
                html += "        <td data-title='COD (GHTK)'>";
                if (item.ghtkCod)
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
                if (controller.filter.staff == "admin")
                {
                    html += "        <td data-title='Nhân viên tạo đơn'>";
                    if (item.staff)
                        html += "            " + item.staff;
                    html += "        </td>";
                }
                html += "    </tr>";
                html += "    <tr class='tr-more-info'>";
                // Mã đơn GHTK
                html += "        <td colspan='2' data-title='Thông tin mã vẫn đơn'>";
                html += "            <span class='bg-blue' style='display: inherit'><strong>" + item.ghtkCode + "</strong></span>";
                html += "        </td>";
                // Thông tin file name và địa chỉ
                html += "        <td colspan='10' data-title='Thông tin thêm địa chỉ khách hàng'>";
                if (item.reason)
                    html += "            <span class='order-info'><strong>Lý do:</strong> " + item.reason + "</span>";
                if (item.address)
                {
                    if (item.reason)
                        html += "            <br>";
                    html += "            <span class='order-info'><strong>Địa chỉ:</strong> " + item.address + "</span>";
                }
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

function _loadNotificationTable() {
    let tbReportDOM = document.querySelector("#tbReport");
    let html = _createReportTableHTML(controller.data);

    tbReportDOM.innerHTML = html;
}
//#endregion

//#region Public
function onKeyUpSearch(event) {
    if (event.keyCode === 13)
        onClickSearch();
}

function onClickSearch() {
    HoldOn.open();
    controller.pagination.page = 1;
    _updateFilter();
    _replaceUrl();

    controller.getGhtkNotifications()
        .then(function (response) {
            HoldOn.close();
            controller.lossMoney = response.lossMoney;
            controller.data = response.data
            controller.pagination = response.pagination;

            _loadSpanReport();
            _loadPagination();
            _loadNotificationTable();
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

    controller.getGhtkNotifications()
        .then(function (response) {
            HoldOn.close();
            controller.lossMoney = response.lossMoney;
            controller.data = response.data
            controller.pagination = response.pagination;

            _loadSpanReport();
            _loadPagination();
            _loadNotificationTable();
        })
        .catch(function (e) {
            console.log(e);
            HoldOn.close();
        });
}
//#endregion