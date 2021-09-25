const OrderTypeEnum = {
    "ANN": 1
};

let strFormat = new StringFormat();
let controller = new DeliveryManagerController();

document.addEventListener("DOMContentLoaded", function (event) {
    _initQueryParams();
    _initDeliveryTable();
});

//#region Private
function _loadSpanReport() {
    let spanReportDOM = document.querySelector("[id$='spanReport']");
    let spanReport = "";

    if (controller.pagination.totalCount > 0)
    {
        spanReport += "(";
        spanReport += UtilsService.formatThousands(controller.pagination.totalCount, ',');
        spanReport += " đơn giao hàng";
        spanReport += ")";
    }

    spanReportDOM.innerHTML = spanReport;
}

function _initQueryParams() {
    let roleDOM = document.querySelector("[id$='_hdfRole']");
    let search = window.location.search;

    controller.role = parseInt(roleDOM.value);
    controller.setFilterByQueryParameters(search);

    if (controller.role != 0) {
        let staffDOM = document.querySelector("[id$='_ddlCreatedBy']");

        controller.filter.staff = staffDOM.value;
    }
}

function _initDeliveryTable() {
    HoldOn.open();

    controller.getDeliveries()
        .then(function (response) {
            controller.lossMoney = response.lossMoney;
            controller.data = response.data
            controller.pagination = response.pagination;

            _loadSpanReport();
            _loadPagination();
            _loadDeliveryTable();

            HoldOn.close();
        })
        .catch(function (e) {
            console.log(e);
            HoldOn.close();
        });
}

function _updateFilter() {
    // Tìm kiếm theo mã đơn hàng hoặc mã vận đơn (Chỉ áp dụng với đơn hàng shop ANN)
    let searchDOM = document.querySelector("[id$='_txtSearch']");

    if (searchDOM.value)
        controller.filter.search = searchDOM.value;
    else
        controller.filter.search = null;

    // Tìm kiếm theo đơn hàng
    let orderTypeDOM = document.querySelector("[id$='_ddlOrderType']");

    if (orderTypeDOM.value != "0")
        controller.filter.orderType = parseInt(orderTypeDOM.value);
    else
        controller.filter.orderType = null;

    // Tìm kiếm loại giao hàng
    let deliveryMethodDOM = document.querySelector("[id$='_ddlDeliveryMethod']");

    if (deliveryMethodDOM.value != "0")
        controller.filter.deliveryMethod = parseInt(deliveryMethodDOM.value);
    else
        controller.filter.deliveryMethod = null;

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

    // Tìm theo trạng thái giao hàng
    let statusDOM = document.querySelector("[id$='_ddlStatus']");

    if (statusDOM.value != "0")
        controller.filter.status = parseInt(statusDOM.value);
    else
        controller.filter.status = null;

    // Tìm theo nhân viên
    if (controller.role == 0) {
        let staffDOM = document.querySelector("[id$='_ddlCreatedBy']");

        if (staffDOM.value)
            controller.filter.staff = staffDOM.value;
        else
            controller.filter.staff = null;
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
    html += "        <th>Loại đơn</th>";
    html += "        <th>Mã đơn</th>";
    html += "        <th>Vận chuyển</th>";
    html += "        <th>Mã vận đơn</th>";
    html += "        <th>Trạng thái</th>";
    html += "        <th>Ngày gửi</th>";
    html += "        <th>Ngày chuyển hoàn</th>";
    if (controller.role == 0)
        html += "        <th>Nhân viên</th>";
    html += "    </tr>";
    html += "</thead>";
    html += "<tbody>";

    if (data.length == 0) {
        if (controller.role == 0)
            html += "    <tr><td colspan='8'>Không tìm thấy báo cáo...</td></tr>";
        else
            html += "    <tr><td colspan='7'>Không tìm thấy báo cáo...</td></tr>";
    }
    else
        data.forEach(function (item) {
            try {
                html += "    <tr>";
                // Loại đơn hàng
                html += "        <td data-title='Loại đơn'><span class='bg-order-type bg-order-type-" + item.orderType.key + "'>" + item.orderType.value + "</span></td>";
                // Mã hóa đơn
                html += "        <td data-title='Mã đơn'>";
                if (item.orderType.key == OrderTypeEnum.ANN)
                    html += "            <a target='_blank' href='/thong-tin-don-hang?id=" + item.code + "'>" + item.code + "</a>";
                else
                    html += "            " + item.code;
                html += "        </td>";
                // Kiểu vận chuyển
                html += "        <td data-title='Vận chuyển'>";
                html += "            <span class='bg-delivery-type bg-delivery-type-" + item.deliveryMethod.key + "'>" + item.deliveryMethod.value;
                html += "        </span></td>";
                // Mã vận đơn
                html += "        <td data-title='Mã vận đơn'>";
                if (item.orderType.key == OrderTypeEnum.ANN)
                    html += "            " + (item.shippingCode || '');
                else
                    html += "            ";
                html += "        </td>";
                // Trạng thái giao hàng
                html += "        <td data-title='Trạng thái'>";
                html += "             <span class='bg-delivery-status bg-delivery-status-" + item.status.key + "'>" + item.status.value;
                html += "        </span></td>";
                // Ngày gửi
                html += "        <td  data-title='Ngày gửi'>";
                if (item.sentDate)
                    html += "            " + strFormat.datetimeToString(item.sentDate, 'dd/MM/yyyy');
                html += "        </td>";
                // Ngày phát
                html += "        <td  data-title='Ngày chuyển hoàn'>";
                if (item.refundDate)
                    html += "            " + strFormat.datetimeToString(item.refundDate, 'dd/MM/yyyy');
                html += "        </td>";
                // Nhân viên
                if (controller.role == 0)
                {
                    html += "        <td data-title='Nhân viên'>";
                    html += "            " + item.staff;
                    html += "        </td>";
                }
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

function _loadDeliveryTable() {
    let tbDeliveryDOM = document.querySelector("#tbDelivery");
    let html = _createReportTableHTML(controller.data);

    tbDeliveryDOM.innerHTML = html;
}
//#endregion

//#region Public
function onKeyUpSearch(event) {
    if (event.key == 'Enter') {
        let codeDOM = document.querySelector("[id$='_txtSearch']");
        codeDOM.value = codeDOM.value.trim();

        onClickSearch()
    }
}

function onClickSearch() {
    HoldOn.open();
    controller.pagination.page = 1;
    _updateFilter();
    _replaceUrl();

    controller.getDeliveries()
        .then(function (response) {
            HoldOn.close();
            controller.lossMoney = response.lossMoney;
            controller.data = response.data
            controller.pagination = response.pagination;

            _loadSpanReport();
            _loadPagination();
            _loadDeliveryTable();
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

    controller.getDeliveries()
        .then(function (response) {
            HoldOn.close();
            controller.lossMoney = response.lossMoney;
            controller.data = response.data
            controller.pagination = response.pagination;

            _loadSpanReport();
            _loadPagination();
            _loadDeliveryTable();
        })
        .catch(function (e) {
            console.log(e);
            HoldOn.close();
        });
}
//#endregion