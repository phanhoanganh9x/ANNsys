class PreOrderService {
    constructor() {
        this.api = '/api/v1';
    }

    _generateQueryParams(filter, pagination) {
        let queryParams = '';

        queryParams += 'searchOrder=' + filter.searchOrder;
        // Created Date
        if (filter.fromDate)
            queryParams += '&fromDate=' + filter.fromDate;
        if (filter.toDate)
            queryParams += '&toDate=' + filter.toDate;
        // Order Status
        if (filter.orderStatus == 0)
            queryParams += '&orderStatus=0'
        else if (filter.orderStatus)
            queryParams += '&orderStatus=' + filter.orderStatus;
        // Search
        if (filter.search)
        {
            queryParams += '&search=' + filter.search;

            if (filter.searchType)
                queryParams += '&searchType=' + filter.searchType;
        }
        // Discount
        if (filter.hasDiscount == 0)
            queryParams += '&hasDiscount=false';
        else if (filter.hasDiscount)
            queryParams += '&hasDiscount=' + Boolean(filter.hasDiscount);
        // Payment Method
        if (filter.paymentMethod)
            queryParams += '&paymentMethod=' + filter.paymentMethod;
        // Delivery Method
        if (filter.deliveryMethod)
            queryParams += '&deliveryMethod=' + filter.deliveryMethod;
        // Coupon
        if (filter.hasCoupon == 0)
            queryParams += '&hasCoupon=false';
        else if (filter.hasCoupon)
            queryParams += '&hasCoupon=' + Boolean(filter.hasCoupon);
        // Quantity
        if (filter.quantityFilter)
        {
            if (filter.quantityFilter == 1 || filter.quantityFilter == 2)
            {
                if (filter.quantity)
                {
                    queryParams += '&quantityFilter=' + filter.quantityFilter;
                    queryParams += '&quantity=' + filter.quantity;
                }
            }
            else if (filter.quantityFilter == 3) {
                if (filter.minQuantity || filter.maxQuantity) {
                    queryParams += '&quantityFilter=' + filter.quantityFilter;

                    if (filter.minQuantity)
                        queryParams += '&minQuantity=' + filter.minQuantity;
                    if (filter.maxQuantity)
                        queryParams += '&maxQuantity=' + filter.maxQuantity;
                }
            }
        }
        // Staff
        if (filter.staff)
            queryParams += '&staff=' + filter.staff;

        queryParams += '&pageSize=' + pagination.pageSize;
        queryParams += '&page=' + pagination.page;

        return queryParams;
    }

    getPreOrders(filter, pagination) {
        let url = this.api + '/orders';
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

    cancelPreOrder(preOrderId, staff) {
        let url = this.api + '/pre-order/' + preOrderId + '/cancel?staff=' + staff;

        return new Promise((reslove, reject) => {
            $.ajax({
                method: 'Post',
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

    recoveryPreOrder(preOrderId, staff) {
        let url = this.api + '/pre-order/' + preOrderId + '/recovery?staff=' + staff;

        return new Promise((reslove, reject) => {
            $.ajax({
                method: 'Post',
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
};