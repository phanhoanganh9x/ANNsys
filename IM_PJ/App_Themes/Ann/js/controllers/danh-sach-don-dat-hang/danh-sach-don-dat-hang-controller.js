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
        this.pagination = {
            pageSize: 30,
            page: 1
        }

        this.service = new PreOrderService();
    }

    setFilter(search) {
        let queryParams = search.replace(/^\?&?/g, '') || "";

        if (!queryParams)
            return;

        queryParams.split('&')
            .filter(x => x)
            .forEach(function(item) {
                let query = item.split("=") || [];

                if (query.length == 2)
                {
                    let key = query[0];
                    let value = query[1];

                    // Datetime picker
                    if (key == "fromDate")
                    {
                        value = unescape(value);
                        value = value.split(" ")[0] || null;

                        if (value != null)
                            this.filter.fromDate = value;
                    }

                    if (key == "toDate")
                    {
                        value = unescape(value);
                        value = value.split(" ")[0] || null;

                        if (value != null)
                            this.filter.toDate = value;
                    }

                    // Order Status
                    if (key == "orderStatus")
                        if (value)
                            this.filter.orderStatus = +value || 0;

                    // Search
                    if (key == "search")
                        if (value)
                            this.filter.search = value;

                    // Search Type
                    if (key == "searchType")
                        if (value)
                            this.filter.searchType = +value || 0;

                    // Discount
                    if (key == "hasDiscount")
                        if (value)
                            this.filter.hasDiscount = +value || 0;

                    // Payment Method
                    if (key == "paymentMethod")
                        if (value)
                            this.filter.paymentMethod = +value || 0;

                    // Delivery Method
                    if (key == "deliveryMethod")
                        if (value)
                            this.filter.deliveryMethod = +value || 0;

                    // Coupon
                    if (key == "hasCoupon")
                        if (value)
                            this.filter.hasCoupon = +value || 0;

                    // Quantity
                    if (key == "quantityFilter")
                        if (value)
                            this.filter.quantityFilter = +value || 0;

                    if (key == "quantity")
                        if (value)
                            this.filter.quantity = +value || 0;

                    if (key == "minQuantity")
                        if (value)
                            this.filter.minQuantity = +value || 0;

                    if (key == "maxQuantity")
                        if (value)
                            this.filter.maxQuantity = +value || 0;

                    // Staff
                    if (key == "staff")
                        if (value)
                            this.filter.staff = value;
                }
            });
    }

    getPreOrders() {
        return this.service.getPreOrders(this.filter, this.pagination)
    }
};