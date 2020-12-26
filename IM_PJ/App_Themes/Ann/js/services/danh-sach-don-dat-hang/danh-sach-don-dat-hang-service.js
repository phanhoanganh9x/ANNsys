class OrderListService {
    constructor() {
        this.backendDomain = "http://ann-shop-dotnet-core.com";
        this.api = "/api/v1";
    }

    _generateQueryParams(filter, pagination) {
        let queryParams = "";

        queryParams += "searchOrder=" + filter.searchOrder;
        // Created Date
        if (filter.fromDate)
            queryParams += "&fromDate=" + filter.fromDate;
        if (filter.toDate)
            queryParams += "&toDate=" + filter.toDate;
        // Order Status
        if (filter.orderStatus)
            queryParams += "&orderStatus=" + filter.orderStatus;
        // Search
        if (filter.search)
        {
            queryParams += "&search=" + filter.search;

            if (filter.searchType)
                queryParams += "&searchType=" + filter.searchType;
        }
        // Discount
        if (filter.hasDiscount)
            queryParams += "&hasDiscount=" + filter.hasDiscount;
        // Payment Method
        if (filter.paymentMethod)
            queryParams += "&paymentMethod=" + filter.paymentMethod;
        // Delivery Method
        if (filter.deliveryMethod)
            queryParams += "&deliveryMethod=" + filter.deliveryMethod;
        // Coupon
        if (filter.hasCoupon)
            queryParams += "&hasCoupon=" + filter.hasCoupon;
        // Quantity
        if (filter.quantityFilter)
        {
            queryParams += "&quantityFilter=" + filter.quantityFilter;

            if (filter.quantityFilter == "1" || filter.quantityFilter == "2")
            {
                if (filter.quantity)
                    queryParams += "&quantity=" + filter.quantity;
            }
            else {
                if (filter.minQuantity)
                    queryParams += "&minQuantity=" + filter.minQuantity;
                if (filter.maxQuantity)
                    queryParams += "&maxQuantity=" + filter.maxQuantity;
            }
        }
        // Staff
        if (filter.staff)
            queryParams += "&staff=" + filter.staff;

        queryParams += "&pageSize=" + pagination.pageSize;
        queryParams += "&page=" + pagination.page;

        return queryParams;
    }

    getPreOrders(filter, pagination) {
        let url = this.backendDomain + this.api + "/orders";
        let queryParams = _generateQueryParams(filter, pagination);

        if (queryParams)
            url += "?" + queryParams;

        return new Promise((reslove, reject) => {
            $.ajax({
                type: "GET",
                url: url,
                contentType: "application/json; charset=utf-8",
                success: (response) => {
                    reslove(JSON.parse(response));
                },
                error: err => {
                    reject(err);
                }
            });
        });
    };
};