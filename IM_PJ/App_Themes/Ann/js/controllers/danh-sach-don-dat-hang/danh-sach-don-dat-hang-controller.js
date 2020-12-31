class PreOrderController {
    constructor() {
        this.filter = {
            searchOrder: 1, // preOrder
            fromDate: null,
            toDate: null,
            orderStatus: null,
            search: null,
            searchType: null,
            hasDiscount: null,
            paymentMethod: null,
            deliveryMethod: null,
            hasCoupon: null,
            quantityFilter: null,
            quantity: null,
            minQuantity: null,
            maxQuantity: null,
            staff: null
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

        this.service = new PreOrderService();
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

            // Datetime picker
            if (key == "fromDate") {
                value = unescape(value);
                value = value.split(" ")[0] || null;

                if (value != null)
                    this.filter.fromDate = value;
            }

            if (key == "toDate") {
                value = unescape(value);
                value = value.split(" ")[0] || null;

                if (value != null)
                    this.filter.toDate = value;
            }

            // Order Status
            if (key == "orderStatus")
            {
                if (value == "0")
                    this.filter.orderStatus = 0
                else if (value)
                    this.filter.orderStatus = +value || null;
            }

            // Search
            if (key == "search")
                if (value)
                    this.filter.search = value;

            // Search Type
            if (key == "searchType")
                if (value)
                    this.filter.searchType = +value || 3;

            // Discount
            if (key == "hasDiscount")
            {
                if (value == 0)
                    this.filter.hasDiscount = 0;
                else if (value)
                    this.filter.hasDiscount = +value || null;
            }

            // Payment Method
            if (key == "paymentMethod")
                if (value)
                    this.filter.paymentMethod = +value || null;

            // Delivery Method
            if (key == "deliveryMethod")
                if (value)
                    this.filter.deliveryMethod = +value || null;

            // Coupon
            if (key == "hasCoupon")
            {
                if (value == "0")
                    this.filter.hasCoupon = 0;
                else if (value)
                    this.filter.hasCoupon = +value || null;
            }

            // Quantity
            if (key == "quantityFilter")
                if (value)
                    this.filter.quantityFilter = +value || null;

            if (key == "quantity")
                if (value)
                    this.filter.quantity = +value || null;

            if (key == "minQuantity")
                if (value)
                    this.filter.minQuantity = +value || null;

            if (key == "maxQuantity")
                if (value)
                    this.filter.maxQuantity = +value || null;

            // Staff
            if (key == "staff")
                if (value)
                    this.filter.staff = value;

            // Page
            if (key == "page")
                if (value && +value > 1)
                    this.pagination.page = +value || 1;
        }
    }

    generateQueryParams() {
        let queryParams = '';

        // Created Date
        if (this.filter.fromDate)
            queryParams += '&fromDate=' + this.filter.fromDate;
        if (this.filter.toDate)
            queryParams += '&toDate=' + this.filter.toDate;
        // Order Status
        if (this.filter.orderStatus == 0)
            queryParams += '&orderStatus=0'
        else if (this.filter.orderStatus)
            queryParams += '&orderStatus=' + this.filter.orderStatus;
        // Search
        if (this.filter.search) {
            queryParams += '&search=' + this.filter.search;

            if (this.filter.searchType)
                queryParams += '&searchType=' + this.filter.searchType;
        }
        // Discount
        if (this.filter.hasDiscount == 0)
            queryParams += '&hasDiscount=0'
        else if (this.filter.hasDiscount)
            queryParams += '&hasDiscount=' + this.filter.hasDiscount;
        // Payment Method
        if (this.filter.paymentMethod)
            queryParams += '&paymentMethod=' + this.filter.paymentMethod;
        // Delivery Method
        if (this.filter.deliveryMethod)
            queryParams += '&deliveryMethod=' + this.filter.deliveryMethod;
        // Coupon
        if (this.filter.hasCoupon == 0)
            queryParams += '&hasCoupon=0'
        else if (this.filter.hasCoupon)
            queryParams += '&hasCoupon=' + this.filter.hasCoupon;
        // Quantity
        if (this.filter.quantityFilter) {


            if (this.filter.quantityFilter == 1 || this.filter.quantityFilter == 2) {
                if (this.filter.quantity)
                {
                    queryParams += '&quantityFilter=' + this.filter.quantityFilter;
                    queryParams += '&quantity=' + this.filter.quantity;
                }
            }
            else if (this.filter.quantityFilter == 3) {
                if (this.filter.minQuantity || this.filter.maxQuantity) {
                    queryParams += '&quantityFilter=' + this.filter.quantityFilter;

                    if (this.filter.minQuantity)
                        queryParams += '&minQuantity=' + this.filter.minQuantity;
                    if (this.filter.maxQuantity)
                        queryParams += '&maxQuantity=' + this.filter.maxQuantity;
                }
            }
        }

        // Staff
        if (this.filter.staff)
            queryParams += '&staff=' + this.filter.staff;

        // Page
        if (this.pagination.page > 1)
            queryParams += '&page=' + this.pagination.page;

        if (queryParams)
            queryParams = queryParams.replace(/^&?/g, '')

        return queryParams
    }

    getPreOrders() {
        return this.service.getPreOrders(this.filter, this.pagination)
    }
};