class PreOrderController {
    constructor() {
        this.filter = {
            searchOrder: 1, // preOrder
            fromDate: null,
            toDate: null,
            orderStatus: null,
            search: null,
            searchType: null,
            hasDiscount: null,
            paymentMethod: null,
            deliveryMethod: null,
            hasCoupon: null,
            quantityFilter: null,
            quantity: null,
            minQuantity: null,
            maxQuantity: null,
            staff: null
        }
        this.agination = {
            pageSize: 30,
            page: 1
        }
    }


    getPreOrders() {

    }

    getQueryParam(url, query) {
        let expr = "[\\?&]" + query.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]") + "=([^&#]*)";
        let regex = new RegExp(expr);
        let results = regex.exec(url);

        if (results)
            return results[1];
        else
            return false;
    };

    searchOrder() {
        document.querySelector("[id$='_btnSearch']").click();
    };

    openFeeInfoModal(tbodyDOM, orderID) {
        window.HoldOn.open();
        OrderListService.openFeeInfoModal(orderID)
            .then(data => {
                let feeTotal = 0;

                data.forEach((item) => {
                    feeTotal += item.FeePrice;
                    tbodyDOM.innerHTML += this._createFeeInfoHTML(item);
                });
                tbodyDOM.innerHTML += this._createFeeInfoHTML({ "FeeTypeName": "Tổng", "FeePrice": feeTotal }, true);
            })
            .catch(err => {
                console.log(err);
                setTimeout(() => {
                    swal("Thông báo", "Có lỗi trong quá trình lấy thông tin phí", "error");
                }, 500);
            })
            .finally(() => {
                window.HoldOn.close();
            });
    };

    _createFeeInfoHTML(fee, is_total) {
        if (!is_total) {
            is_total = false;
        }
        let addHTML = "";

        if (is_total) {

            addHTML += "<tr class='info'>";
            addHTML += "    <td style='text-align: right'>" + fee.FeeTypeName + "</td>";
            addHTML += "    <td>" + UtilsService.formatThousands(fee.FeePrice) + "</td>";
            addHTML += "</tr>";
        }
        else {
            addHTML += "<tr>";
            addHTML += "    <td>" + fee.FeeTypeName + "</td>";
            addHTML += "    <td>" + UtilsService.formatThousands(fee.FeePrice) + "</td>";
            addHTML += "</tr>";
        }

        return addHTML;
    }

    changeFinishStatusOrder(orderStatusDOM, orderID) {
        window.HoldOn.open();
        OrderListService.changeFinishStatusOrder(orderID)
            .then(data => {
                if (data) {
                    orderStatusDOM.classList.remove("bg-green");
                    orderStatusDOM.classList.add("bg-yellow");
                    orderStatusDOM.removeAttribute("style");
                    orderStatusDOM.innerText = 'Đang xử lý';
                    orderStatusDOM.removeAttribute("onclick");

                    setTimeout(() => {
                        swal("Thông báo", "Đơn đã chuyển thành trạng thái đang xử lý", "success");
                    }, 500);
                }
                else {
                    setTimeout(() => {
                        swal("Thông báo", "Có lỗi trong quá trình lấy thông tin phí", "error");
                    }, 500);
                }
            })
            .catch(err => {
                console.log(err);
                setTimeout(() => {
                    swal("Thông báo", "Quá trình chuyển đổi trạng thái đơn hàng đã có vấn đề", "error");
                }, 500);
            })
            .finally(() => {
                window.HoldOn.close();
            });
    };
};