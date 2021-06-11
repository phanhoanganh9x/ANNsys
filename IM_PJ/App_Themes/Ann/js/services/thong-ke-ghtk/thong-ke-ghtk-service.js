class GhtkReportService {
    constructor() {
        this.api = '/api/v1';
    }

    _generateQueryParams(filter, pagination) {
        let queryParams = '';

        // Tìm kiếm đơn hàng
        if (filter.search)
            queryParams += '&search=' + filter.search;
        // Trạng thái phí giao hàng
        if (filter.feeStatus)
            queryParams += '&feeStatus=' + filter.feeStatus;
        // Từ ngày
        if (filter.fromDate)
            queryParams += '&fromDate=' + filter.fromDate;
        // Tới ngày
        if (filter.toDate)
            queryParams += '&toDate=' + filter.toDate;
        // Trạng thái đơn hàng
        if (filter.orderStatus)
            queryParams += '&orderStatus=' + filter.orderStatus
        // Trạng thái GHTK
        if (filter.ghtkStatus != null)
            queryParams += '&ghtkStatus=' + filter.ghtkStatus;
        // Trạng thái duyệt
        if (filter.reviewStatus)
            queryParams += '&reviewStatus=' + filter.reviewStatus;

        queryParams += '&pageSize=' + pagination.pageSize;
        queryParams += '&page=' + pagination.page;

        return queryParams.substring(1);
    }

    getGhtkReports(filter, pagination) {
        let url = this.api + '/delivery-save/bien-ban-doi-soat';
        let queryParams = this._generateQueryParams(filter, pagination);

        if (queryParams)
            url += '?' + queryParams;

        return new Promise((reslove, reject) => {
            $.ajax({
                method: 'GET',
                url: url,
                contentType: 'application/json; charset=utf-8',
                success: (response) => {
                    reslove(response);
                },
                error: err => {
                    reject(err);
                }
            });
        });
    };

    uploadGhtkReports(name, file) {
        let url = this.api + '/delivery-save/import/bien-ban-doi-soat/excel';
        let fd = new FormData();

        fd.append('name', name);
        fd.append('file', file);

        return new Promise((reslove, reject) => {
            $.ajax({
                method: 'POST',
                url: url,
                data: fd,
                processData: false,
                contentType: false,
                success: (response) => {
                    reslove(response);
                },
                error: err => {
                    reject(err);
                }
            });
        });
    }

    approveRecord(deliverySaveId, orderId) {
        let url = this.api + '/delivery-save/bien-ban-doi-soat/' + deliverySaveId;
        let data = {
            'orderId': orderId
        };

        return new Promise((reslove, reject) => {
            $.ajax({
                method: 'POST',
                url: url,
                data: JSON.stringify(data),
                contentType: 'application/json; charset=utf-8',
                success: (response) => {
                    reslove(response);
                },
                error: err => {
                    reject(err);
                }
            });
        });
    }
}