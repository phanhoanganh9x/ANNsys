class GroupOrderController {
    constructor() {
        this.staff = null;
        this.paymentStatus = null;
        this.paymentMethod = null;
        this.deliveryMethod = null;
        this.deliveryAddress = null;
        this.customer = null;
        this.orders = [];

        this._service = new GroupOrderService();
    }

    setStaff(staff) {
        this.staff = staff;
    }

    addOrder(order) {
        if (this.orders.length == 0) {
            this.paymentStatus = order.paymentStatus;
            this.paymentMethod = order.paymentMethod;
            this.deliveryMethod = order.deliveryMethod;
            this.deliveryAddress = order.deliveryAddress;
            this.customer = order.customer;
        }

        this.orders.push(order);
    }

    removeOrder(orderId) {
        this.orders = this.orders.filter(x => x.id != orderId);
    }

    // Lấy thông tin cơ bản của đơn hàng
    getOrderBasic(orderId) {
        return this._service.getOrderBasic(orderId);
    }

    // Đăng ký khởi tạo đơn gộp
    registerGroupOrder() {
        var data = {
            staff: this.staff,
            orderIds: this.orders.map(function (item) { return item.id; })
        }
        return this._service.registerGroupOrder(data);
    }
}