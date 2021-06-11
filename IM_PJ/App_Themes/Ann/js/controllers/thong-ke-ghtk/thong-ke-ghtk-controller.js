class GhtkReportController {
    constructor() {
        this.filter = {
            search: null,
            feeStatus: null,
            fromDate: null,
            toDate: null,
            orderStatus: null,
            ghtkStatus: null,
            reviewStatus: null
        }
        this.lossMoney = 0;
        this.data = [];
        this.pagination = {
            nextPage: 'No',
            page: 1,
            pageSize: 30,
            previousPage: 'No',
            totalCount: 0,
            totalPages: 1,
        }

        this._service = new GhtkReportService();
    }

    setFilter(search) {
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

            // Tìm kiếm đơn hàng
            if (key == "search")
                if (value)
                    this.filter.search = value;
            // Trạng thái phí vận chuyển
            if (key == "feeStatus")
                if (value)
                    this.filter.feeStatus = +value || null;
            // Từ ngày
            if (key == "fromDate") {
                value = unescape(value);
                value = value.split(" ")[0] || null;

                if (value != null)
                    this.filter.fromDate = value;
            }
            // Tới ngày
            if (key == "toDate") {
                value = unescape(value);
                value = value.split(" ")[0] || null;

                if (value != null)
                    this.filter.toDate = value;
            }
            // Trạng thái đơn hàng
            if (key == "orderStatus")
                if (value)
                    this.filter.orderStatus = +value || null;
            // Trạng thái GHTK
            if (key == "ghtkStatus")
                if (value)
                {
                    if (value == "0")
                        this.filter.ghtkStatus = 0;
                    else
                        this.filter.ghtkStatus = +value || null;
                }
            // Trạng thái duyệt
            if (key == "reviewStatus")
                if (value)
                    this.filter.reviewStatus = +value || null;
            // Page
            if (key == "page")
                if (value && +value > 1)
                    this.pagination.page = +value || 1;
        }
    }

    generateQueryParams() {
        let queryParams = '';

        // Tìm kiếm đơn hàng
        if (this.filter.search)
            queryParams += '&search=' + this.filter.search;
        // Tình trạng phí vận chuyển
        if (this.filter.feeStatus)
            queryParams += '&feeStatus=' + this.filter.feeStatus;
        // Từ ngày
        if (this.filter.fromDate)
            queryParams += '&fromDate=' + this.filter.fromDate;
        // Tới ngày
        if (this.filter.toDate)
            queryParams += '&toDate=' + this.filter.toDate;
        // Trạng thái đơn hàng
        if (this.filter.orderStatus)
            queryParams += '&orderStatus=' + this.filter.orderStatus;
        // Trạng thái GHTK
        if (this.filter.ghtkStatus != null)
            queryParams += '&ghtkStatus=' + this.filter.ghtkStatus;
        // Trạng thái duyệt
        if (this.filter.reviewStatus)
            queryParams += '&reviewStatus=' + this.filter.reviewStatus;
        // Page
        if (this.pagination.page > 1)
            queryParams += '&page=' + this.pagination.page;

        if (queryParams)
            queryParams = queryParams.replace(/^&?/g, '')

        return queryParams
    }

    getGhtkReports() {
        return this._service.getGhtkReports(this.filter, this.pagination)
    }

    uploadGhtkReports(name, file) {
        return this._service.uploadGhtkReports(name, file);
    }

    approveRecord(deliverySaveId, orderId) {
        return this._service.approveRecord(deliverySaveId, orderId);
    }
}