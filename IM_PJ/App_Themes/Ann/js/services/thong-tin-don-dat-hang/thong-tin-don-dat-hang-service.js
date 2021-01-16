class PreOrderService {
    constructor() {
        if (!!PreOrderService._instance)
            return OrderService._instance;

        PreOrderService._instance = this;
        this.backendDomain = 'http://ann-shop-dotnet-core.com';
        this.api = '/api/v1';

        return this;
    }

    getPreOrder(preOrderId) {
        let url = this.backendDomain + this.api + '/pre-order/' + preOrderId;

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