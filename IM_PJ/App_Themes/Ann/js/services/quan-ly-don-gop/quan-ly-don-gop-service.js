class GroupOrderManagerService {
    constructor() {
        this.api = '/api/v1';
    }

    _generateQueryParams(filter, pagination) {
        let queryParams = '';

        // Tìm kiếm theo mã đơn hàng, sđt khách hàng, mã vận đơn, tên khách hàng, mã gộp
        if (filter.search)
            queryParams += '&search=' + filter.search;
        // Tình trạng đơn hàng
        if (filter.status)
            queryParams += '&status=' + filter.status;
        // Kiểu giao hàng
        if (filter.deliveryMethod)
            queryParams += '&deliveryMethod=' + filter.deliveryMethod;
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

    getDeliveryMethod(deliveryMethod) {
        let url = this.api + '/delivery/method/' + deliveryMethod;

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
    }

    getGroupOrders(filter, pagination) {
        let url = this.api + '/group-orders';
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

    cancelGroupOrder(groupCode, staff) {
        let url = this.api + '/group-order/' + groupCode + '/cancel';

        return new Promise((reslove, reject) => {
            $.ajax({
                method: 'POST',
                url: url,
                contentType: 'application/json; charset=utf-8',
                dataType: "json",
                data: JSON.stringify({ "staff": staff }),
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