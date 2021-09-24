class DeliveryManagerService {
    constructor() {
        this.api = '/api/v1';
    }

    _generateQueryParams(filter, pagination) {
        let queryParams = '';

        // Tìm kiếm theo mã đơn hàng hoặc mã vận đơn (Chỉ áp dụng với đơn hàng shop ANN)
        if (filter.search)
            queryParams += '&search=' + filter.search;
        // Loại đơn hàng
        if (filter.orderType)
            queryParams += '&orderType=' + filter.orderType;
        // Kiểu giao hàng
        if (filter.deliveryMethod)
            queryParams += '&deliveryMethod=' + filter.deliveryMethod;
        // Từ ngày
        if (filter.fromDate)
            queryParams += '&fromDate=' + filter.fromDate;
        // Tới ngày
        if (filter.toDate)
            queryParams += '&toDate=' + filter.toDate;
        // Tình trạng đơn hàng
        if (filter.status)
            queryParams += '&status=' + filter.status;
        // Nhân viên
        if (filter.staff)
            queryParams += '&staff=' + filter.staff;

        queryParams += '&pageSize=' + pagination.pageSize;
        queryParams += '&page=' + pagination.page;

        return queryParams.substring(1);
    }

    getDeliveries(filter, pagination) {
        let url = this.api + '/deliveries';
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
}