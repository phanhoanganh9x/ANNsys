class DeliveryRefundController {
    constructor() {
        this.delivery = {
            orderType: null,
            code: null,
            deliveryMethod: null,
            shippingCode: null,
            sentDate: null,
            refundDate: null,
            staff: null,
            isNew: 1
        };

        this._service = new DeliveryRefundService();
    }

    setDelivery(data) {
        this.delivery.orderType = data.orderType;
        this.delivery.code = data.code;
        this.delivery.deliveryMethod = data.deliveryMethod;
        this.delivery.shippingCode = data.shippingCode;
        this.delivery.sentDate = data.sentDate;
        this.delivery.staff = data.staff;
        this.delivery.isNew = data.isNew;

        if (data.refundDate)
            this.delivery.refundDate = data.refundDate;
        else {
            let now = new Date(Date.now());
            this.delivery.refundDate = now.format("yyyy-mm-dd");
        }
    }

    // Lấy thông tin giao hàng đối với đơn shop ANN
    getDeliveryInfo() {
        return this._service.getDeliveryInfo(this.delivery.code);
    }

    // Lấy thông tin giao hàng
    getDelivery() {
        return this._service.getDelivery(this.delivery.orderType.key, this.delivery.code);
    }

    // Đăng ký đơn chuyển hoàn
    refundDeliveries(deliveries) {
        return this._service.refundDeliveries(deliveries);
    }
}