let controller = new PreOrderController();


document.addEventListener("DOMContentLoaded", function (event) {
    initQuantity();
});

function initQuantity() {

}


function onKeyUp_txtSearchOrder(event) {
    if (event.keyCode === 13) {
        controller.searchOrder();
    }
}

function onClickSearchPreOrder() {
    controller.searchOrder();
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