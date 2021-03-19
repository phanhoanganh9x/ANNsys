class OrderService {
    contructor() {
        if (!!OrderService._instance)
            return OrderService._instance;

        OrderService._instance = this;

        return this;
    }

    generateOrderUrl(status, id, isPreOrder) {
        if (isPreOrder == undefined)
            isPreOrder = true;

        if (status == 0)
            return '/thong-tin-don-dat-hang?id=' + id;
        else if (status == 1)
            return '/thong-tin-don-hang?id=' + id;
        else if (status == 2)
            return '/thong-tin-don-hang?id=' + id;
        else if (status == 3)
            return (isPreOrder ? '/thong-tin-don-dat-hang?id=' : '/thong-tin-don-hang?id=') + id;
        else
            return ''
    }

    generateOrderTypeHTML(orderType) {
        if (orderType == 1)
            return "<span class='bg-yellow'>Lẻ</span>";
        else
            return "<span class='bg-blue'>Sỉ</span>";
    }

    generateOrderStatusHTML(status) {
        if (status == 0)
            return "<span class='bg-blue'>Chờ tiếp nhận</span>";
        else if (status == 1)
            return "<span class='bg-yellow'>Đang xử lý</span>";
        else if (status == 2)
            return "<span class='bg-green'>Đã hoàn tất</span>";
        else if (status == 3)
            return "<span class='bg-red'>Đã hủy</span>";
        else
            return "<span class='bg-brown'>Chuyển hoàn</span>";
    }

    generatePaymentStatusHTML(paymentStatus) {
        if (paymentStatus == 1)
            return "<span class='bg-black'>Chưa thanh toán</span>";
        else if (paymentStatus == 2)
            return "<span class='bg-red'>Thanh toán thiếu</span>";
        else if (paymentStatus == 3)
            return "<span class='bg-blue'>Đã thanh toán</span>";
        else if (paymentStatus == 4)
            return "<span class='bg-blue-hoki'>Đã duyệt</span>";
    }

    generatePaymentMethodHTML(paymentMethod) {
        if (paymentMethod == 1)
            return "<span class='bg-black'>Tiền mặt</span>";
        else if (paymentMethod == 2)
            return "<span class='bg-red'>Chuyển khoản</span>";
        else if (paymentMethod == 3)
            return "<span class='bg-yellow'>Thu hộ</span>";
        else if (paymentMethod == 4)
            return "<span class='bg-blue'>Công nợ</span>";
        else
            return "<span class='bg-red'>Chưa xác định</span>";
    }

    generateDeliveryMethodHTML(deliveryMethod) {
        if (deliveryMethod == 1)
            return "<span class='bg-black'>Lấy trực tiếp</span>";
        else if (deliveryMethod == 2)
            return "<span class='bg-yellow'>Bưu điện</span>";
        else if (deliveryMethod == 3)
            return "<span class='bg-proship'>Proship</span>";
        else if (deliveryMethod == 4)
            return "<span class='bg-orange'>Chuyển xe</span>";
        else if (deliveryMethod == 5)
            return "<span class='bg-bronze'>Nhân viên giao</span>";
        else if (deliveryMethod == 6)
            return "<span class='bg-ghtk'>GHTK</span>";
        else if (deliveryMethod == 7)
            return "<span class='bg-blue-hoki'>Viettel</span>";
        else if (deliveryMethod == 8)
            return "<span class='bg-grab'>Grab</span>";
        else if (deliveryMethod == 9)
            return "<span class='bg-ahamove'>AhaMove</span>";
        else
            return "<span class='bg-red'>Chưa xác định</span>";
    }
}