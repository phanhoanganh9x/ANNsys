let controller = new OrderListController();

document.addEventListener("DOMContentLoaded", function (event) {
    initQuantity();
    initCsv();
});

function initQuantity() {
    let param = controller.getQueryParam(window.location.href,"quantityfilter");

    if (param) {
        let divGreaterThanDOM = document.querySelector(".greaterthan");
        let divBetweenDOM = document.querySelector(".between");

        if (param === "greaterthan" || param === "lessthan") {
            divGreaterThanDOM.classList.remove("hide");
            divBetweenDOM.classList.add("hide");
        }
        else if (param == "between") {
            divGreaterThanDOM.classList.add("hide");
            divBetweenDOM.classList.remove("hide");
        }
    }
}

function initCsv() {
    let exportCsvDom = document.querySelector("#exportCsv");
    let csvDataDom = document.querySelector("[id$='_hdfCSV']");
    let csvData = csvDataDom.value;

    if (csvData) {
        exportCsvDom.style.display = "initial";
    }
    else {
        exportCsvDom.style.display = "none";
    }
}

function onChange_ddlQuantityFilter(self) {
    let value = self.value;
    let divGreaterThanDOM = document.querySelector(".greaterthan");
    let divBetweenDOM = document.querySelector(".between");

    if (value == "greaterthan" || value == "lessthan") {
        let txtQuantityDOM = document.querySelector("[id$='_txtQuantity']");

        divGreaterThanDOM.classList.remove("hide");
        divBetweenDOM.classList.add("hide");

        txtQuantityDOM.focus();
        txtQuantityDOM.select();
    }
    else if (value == "between") {
        let txtQuantityMinDOM = document.querySelector("[id$='txtQuantityMin']");

        divGreaterThanDOM.classList.add("hide");
        divBetweenDOM.classList.remove("hide");

        txtQuantityMinDOM.focus();
        txtQuantityMinDOM.select();
    }
}

function onKeyUp_txtSearchOrder(event) {
    if (event.keyCode === 13) {
        controller.searchOrder();
    }
}

function onClick_aSearchOrder() {
    controller.searchOrder();
};

function onClick_aFeeInfoModal(orderID) {
    let modalDOM = document.querySelector("#feeInfoModal");
    let tbodyDOM = modalDOM.querySelector("tbody[id='feeInfo']");

    tbodyDOM.innerHTML = "";
    controller.openFeeInfoModal(tbodyDOM, orderID);
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

function onClick_exportCsv() {
    const csvDataDom = document.querySelector("[id$='_hdfCSV']");
    const csvData = csvDataDom.value;

    if (!csvData) return;

    const now = new Date();

    // Khởi tạo CSV
    let csvFile = "danh-sach-don-hang";
    const csvContentType = "data:text/csv;charset=utf-8,";

    // _YYY-MM-DD
    csvFile += "_" + String(now.getFullYear()) + "-" + String(now.getMonth() + 1) + "-" + String(now.getDate())
    // _HH-mm-ss
    csvFile += "-" + String(now.getHours()) + "-" + String(now.getMinutes()) + "-" + String(now.getMilliseconds())

    // Khởi tạo dữ liệu
    const rows = JSON.parse(csvData);
    const csvContent = rows.map(e => e.join(",")).join("\n");

    // Khởi tạo DOM link để tải file
    // https://stackoverflow.com/questions/42462764/javascript-export-csv-encoding-utf-8-issue
    const universalBOM = "\uFEFF";
    const encodedUri = encodeURIComponent(universalBOM + csvContent);
    const link = document.createElement("a");

    link.setAttribute("href", csvContentType + encodedUri);
    link.setAttribute("download", csvFile + ".csv");
    document.body.appendChild(link); // Required for FF

    // Thực thi tải file CSV
    link.click();
}