class GroupOrderService {
    constructor() {
        this.api = '/api/v1';
    }

    getOrderBasic(orderId) {
        let url = this.api + '/order/' + orderId + '/basic';

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

    registerGroupOrder(data) {
        let url = this.api + '/group-order/register';

        return new Promise((reslove, reject) => {
            $.ajax({
                method: 'POST',
                url: url,
                contentType: 'application/json; charset=utf-8',
                dataType: "json",
                data: JSON.stringify(data),
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