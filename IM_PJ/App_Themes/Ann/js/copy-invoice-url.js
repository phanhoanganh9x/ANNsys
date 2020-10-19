window.Clipboard = (function (window, document, navigator) {
    var textArea,
        copy;

    function isOS() {
        return navigator.userAgent.match(/ipad|iphone/i);
    }

    function createTextArea(text) {
        textArea = document.createElement('textArea');
        textArea.value = text;
        document.body.appendChild(textArea);
    }

    function selectText() {
        var range,
            selection;

        if (isOS()) {
            range = document.createRange();
            range.selectNodeContents(textArea);
            selection = window.getSelection();
            selection.removeAllRanges();
            selection.addRange(range);
            textArea.setSelectionRange(0, 999999);
        } else {
            textArea.select();
        }
    }

    function copyToClipboard() {
        document.execCommand('copy');
        document.body.removeChild(textArea);
    }

    copy = function (text) {
        createTextArea(text);
        selectText();
        copyToClipboard();
    };

    return {
        copy: copy
    };
})(window, document, navigator);

function copyInvoiceURL(orderID, customerID) {
    var copyText = "kiểm tra bill giúp em nha!\r\nbấm vào link này để xem bill:\r\nhttp://khachhangann.com/hoa-don/" + orderID + "/khach-hang/" + customerID;
    Clipboard.copy(copyText);
}

function copyInvoiceURLEnglish(orderID, customerID) {
    var copyText = "Please check your invoice!\r\nClick to view:\r\nhttp://khachhangann.com/hoa-don/" + orderID + "/khach-hang/" + customerID;
    Clipboard.copy(copyText);
}