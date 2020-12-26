let controller = new PreOrderController();


document.addEventListener("DOMContentLoaded", function (event) {
    _initQueryParams()
    _initQuantity();
});

function _initQueryParams() {
    let search = window.location.search;
    
    controller.setFilter(search);
}

function _initQuantity() {
    if (!controller.filter.quantityFilter)
        return;

    let divGreaterDOM = document.querySelector(".greaterthan");
    let divBetweenDOM = document.querySelector(".between");

    if (controller.filter.quantityFilter in [1, 2]) {
        divGreaterDOM.classList.remove("hide");
        divBetweenDOM.classList.add("hide");
    }
    else if (controller.filter.quantityFilter == 3) {
        divGreaterDOM.classList.add("hide");
        divBetweenDOM.classList.remove("hide");
    }
}

function _updateFilter() {
    // Datetime Picker
    let fromDateDOM = document.querySelector("[id$='_rOrderFromDate_dateInput']");
    let toDateDOM = document.querySelector("[id*='_rOrderToDate_dateInput']");

    if (fromDateDOM.value.trim()) {
        let date = fromDateDOM.value.split('/').filter(x => x);

        if (date.length == 3)
            controller.filter.fromDate = date[1] + '/' + date[0] + '/' + date[2];
    }

    if (toDateDOM.value.trim()) {
        let date = fromDateDOM.value.split('/').filter(x => x);

        if (date.length == 3)
            controller.filter.toDate = date[1] + '/' + date[0] + '/' + date[2];
    }

    // Order Status
    let orderStatusDOM = document.querySelector("[id$='_ddlExcuteStatus']");

    if (orderStatusDOM.value)
        controller.filter.orderStatus = +orderStatusDOM.value || 0;

    // Search
    let searchDOM = document.querySelector("[id$='_txtSearchOrder']");

    if (searchDOM.value) {
        controller.filter.search = searchDOM.value;

        let searchTypeDOM = document.querySelector("[id$='_ddlSearchType']");

        if (searchTypeDOM.value)
            controller.filter.searchType = +searchTypeDOM.value || 3; // Trường hợp đơn đặt hàng
    }
    
    // Discount
    let dicountDOM = document.querySelector("[id$='_ddlDiscount']");

    if (dicountDOM.value)
        controller.filter.hasDiscount = +discountDOM.value || 0;

    // Payment Method
    let paymentDOM = document.querySelector("[id$='_ddlPaymentType']");

    if (paymentDOM.value)
        controller.filter.paymentMethod = +paymentDOM.value || 0;

    // Delivery Method
    let deliveryDOM = document.querySelector("[id$='_ddlShippingType']");

    if (deliveryDOM.value)
        controller.filter.deliveryMethod = +deliveryDOM.value || 0;
    
    // Coupon
    let couponDOM = document.querySelector("[id$='_ddlCouponStatus']");

    if (couponDOM.value)
        controller.filter.hasCoupon = +couponDOM.value || 0;

    // Quantity
    let quantityFilterDOM = document.querySelector("[id$='_ddlQuantityFilter']");

    if (quantityFilterDOM.value)
    {
        controller.filter.quantityFilter = +quantityFilterDOM.value || 0

        if (controller.filter.quantityFilter == 3) {
            let minQuantityDOM = document.querySelector("[id$='_txtQuantityMin']");
            let maxQuantityDOM = document.querySelector("[id$='_txtQuantityMax']");

            if (!minQuantityDOM.value.trim() && !maxQuantityDOM.value.trim()) {
                controller.filter.quantityFilter = null;
                controller.filter.minQuantity = null;
                controller.filter.maxQuantity = null;
            }
            else {
                if (minQuantityDOM.value.trim())
                    controller.filter.minQuantity = +minQuantityDOM.value || 0;

                if (maxQuantityDOM.value.trim())
                    controller.filter.maxQuantity = +maxQuantityDOM.value || 0;
            }
        }
        else {
            let quantityDOM = document.querySelector("[id$='_txtQuantity']");

            if (quantityDOM.value.trim())
                controller.filter.quantity = +quantityDOM.value || 0;
            else {
                controller.filter.quantityFilter = null;
                controller.filter.quantity = null;
            }
        }
    }
    
    // Staff
    let staffDOM = document.querySelector("[id$='_ddlCreatedBy']");

    if (staffDOM.value)
        controller.filter.staffDOM = staffDOM.value;
}

function onKeyUp_txtSearchOrder(event) {
    if (event.keyCode === 13) {
        _updateFilter();
        controller.getPreOrders();
    }
}

function onClickSearchPreOrder() {
    _updateFilter();
    controller.getPreOrders();
};

function onClick_spFinishStatusOrder(self, orderID) {
    swal({
        title: "Xác nhận",
        text: 'Bạn muốn chuyển trạng thái đơn là đang xử lý, phải không?',
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: true,
        cancelButtonText: "Để em xem lại...",
        confirmButtonText: "Đúng rồi sếp!",
    }, function (confirm) {
        if (confirm) {
            controller.changeFinishStatusOrder(self, orderID);
        }
    });
};