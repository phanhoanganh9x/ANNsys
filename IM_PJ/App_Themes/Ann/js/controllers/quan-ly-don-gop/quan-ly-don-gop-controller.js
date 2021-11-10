class GroupOrderManagerController {
    constructor() {
        this.filter = {
            search: null,
            status: null,
            deliveryMethod: null,
            fromDate: null,
            toDate: null
        }
        this.data = [];
        this.pagination = {
            nextPage: 'No',
            page: 1,
            pageSize: 30,
            previousPage: 'No',
            totalCount: 0,
            totalPages: 1,
        }

        this._service = new GroupOrderManagerService();
    }

    setFilterByQueryParameters(search) {
        let queryParams = search.replace(/^\?&?/g, '') || "";

        if (!queryParams)
            return;

        let params = queryParams.split('&').filter(x => x);

        for (var i = 0; i < params.length; i++) {
            let query = params[i].split("=") || [];

            if (query.length != 2)
                continue;

            let key = query[0];
            let value = query[1];

            // Tìm kiếm theo mã đơn hàng hoặc mã vận đơn (Chỉ áp dụng với đơn hàng shop ANN)
            if (key == "search")
                if (value)
                    this.filter.search = value;
            // Tìm kiếm theo tình trạng đơn hàng
            if (key == "status")
                if (value)
                    this.filter.status = +value || null;
            // Tìm kiếm theo kiểu giao hàng
            if (key == "deliveryMethod")
                if (value)
                    this.filter.deliveryMethod = +value || null;
            // Từ ngày
            if (key == "fromDate") {
                value = unescape(value);

                if (value != null)
                    this.filter.fromDate = value;
            }
            // Tới ngày
            if (key == "toDate") {
                value = unescape(value);

                if (value != null)
                    this.filter.toDate = value;
            }
            // Page
            if (key == "page")
                if (value && +value > 1)
                    this.pagination.page = +value || 1;
        }
    }

    generateQueryParams() {
        let queryParams = '';

        // Tìm kiếm theo mã đơn hàng hoặc mã vận đơn (Chỉ áp dụng với đơn hàng shop ANN)
        if (this.filter.search)
            queryParams += '&search=' + this.filter.search;
        // Tìm kiếm theo tình trạng đơn hàng
        if (this.filter.status)
            queryParams += '&status=' + String(this.filter.status);
        // Tìm kiếm theo kiểu giao hàng
        if (this.filter.deliveryMethod)
            queryParams += '&deliveryMethod=' + String(this.filter.deliveryMethod);
        // Từ ngày
        if (this.filter.fromDate)
            queryParams += '&fromDate=' + this.filter.fromDate;
        // Tới ngày
        if (this.filter.toDate)
            queryParams += '&toDate=' + this.filter.toDate;
        // Page
        if (this.pagination.page > 1)
            queryParams += '&page=' + this.pagination.page;

        if (queryParams)
            queryParams = queryParams.replace(/^&?/g, '')

        return queryParams
    }

    getDeliveryMethod() {
        return this._service.getDeliveryMethod(this.filter.deliveryMethod)
    }

    getGroupOrders() {
        return this._service.getGroupOrders(this.filter, this.pagination);
    }
}