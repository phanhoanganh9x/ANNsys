class DeliveryRegisterController {
    constructor() {
        this.delivery = {
            orderType: null,
            code: null,
            deliveryMethod: null,
            shippingCode: null,
            sentDate: null,
            staff: null
        };

        this._service = new DeliveryRegisterService();
    }

    setDelivery(data) {
        this.delivery.orderType = data.orderType;
        this.delivery.code = data.code;
        this.delivery.deliveryMethod = data.deliveryMethod;
        this.delivery.shippingCode = data.shippingCode;
        this.delivery.staff = data.staff;

        if (data.sentDate)
            this.delivery.sentDate = data.sentDate;
        else {
            let now = new Date(Date.now());
            this.delivery.sentDate = now.format("yyyy-mm-dd");
        }
    }

    // Lấy thông tin giao hàng đối với đơn shop ANN
    getDeliveryInfo() {
        return this._service.getDeliveryInfo(this.delivery.code);
    }

    // Lấy đơn giao hàng
    getDelivery() {
        return this._service.getDelivery(this.delivery.orderType.key, this.delivery.code);
    }

    // Đăng ký đơn giao hàng
    registerDeliveries(deliveries) {
        return this._service.registerDeliveries(deliveries);
    }
}