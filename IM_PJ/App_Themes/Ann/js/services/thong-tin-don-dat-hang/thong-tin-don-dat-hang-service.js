class PreOrderService {
    constructor() {
        if (!!PreOrderService._instance)
            return OrderService._instance;

        PreOrderService._instance = this;
        this.api = '/api/v1';

        return this;
    }

    getPreOrder(preOrderId) {
        let url = this.api + '/pre-order/' + preOrderId;

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

    createOrder(preOrderId, staff) {
        let url = this.api + '/pre-order/' + preOrderId + '/order?staff=' + staff;

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

    checkOldOrder(customerId) {
        let url = '/them-moi-don-hang.aspx/checkOrderOld';
        let data = JSON.stringify({ 'customerId': customerId });

        return new Promise(function (reslove, reject) {
            $.ajax({
                url: url,
                type: "POST",
                data: data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    reslove(response.d);
                },
                error: function (err) {
                    reject(err);
                }
            });
        });
    }
}