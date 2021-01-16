class PreOrderController {
    constructor() {
        if (!!PreOrderController._instance)
            return PreOrder

        PreOrderController._instance = this;
        this.role = null;
        this.preOrderId = null;
        this._service = new PreOrderService();

        return this;
    }

    setPreOrderId(search) {
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

            // ID của PreOrder
            if (key == "id")
                if (value)
                    this.preOrderId = +value;
        }
    }

    getPreOrder() {
        return this._service.getPreOrder(this.preOrderId);
    }
}