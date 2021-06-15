class GhtkNotificationService {
    constructor() {
        this.api = '/api/v1';
    }

    _generateQueryParams(filter, pagination) {
        let queryParams = '';

        // Tìm kiếm đơn hàng
        if (filter.search)
            queryParams += '&search=' + filter.search;
        // Trạng thái GHTK
        if (filter.ghtkStatus != null)
            queryParams += '&ghtkStatus=' + filter.ghtkStatus;
        // Từ ngày
        if (filter.fromDate)
            queryParams += '&fromDate=' + filter.fromDate;
        // Tới ngày
        if (filter.toDate)
            queryParams += '&toDate=' + filter.toDate;

        queryParams += '&pageSize=' + pagination.pageSize;
        queryParams += '&page=' + pagination.page;

        return queryParams.substring(1);
    }

    getGhtkNotifications(filter, pagination) {
        let url = this.api + '/delivery-save/notifications';
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

    getGhtkStatus(statusId) {
        let url = this.api + '/delivery-save/status/' + statusId;

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
}