class DeliveryRegisterService {
    constructor() {
        this.api = '/api/v1';
    }

    getDeliveryInfo(code) {
        let url = this.api + '/order/delivery?code=' + code;

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

    getDelivery(orderType, code) {
        let url = this.api + '/delivery?orderType=' + orderType + '&code=' + code;

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

   registerDeliveries(deliveries) {
        let url = this.api + '/deliveries/register';

        return new Promise((reslove, reject) => {
            $.ajax({
                method: 'POST',
                url: url,
                contentType: 'application/json; charset=utf-8',
                dataType: "json",
                data: JSON.stringify(deliveries),
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