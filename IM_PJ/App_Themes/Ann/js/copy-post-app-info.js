window.Clipboard = (function (window, document, navigator) {
    var textArea,
        copy;

    function isOS() {
        return navigator.userAgent.match(/ipad|iphone/i);
    }

    function createTextArea(text) {
        textArea = document.createElement('textarea');
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


function copyPostInfo(id) {
    $("body").append("<div class='copy-content hide'></div>");

    ajaxCopyInfo(id);

    Clipboard.copy($(".copy-content").text());

    $(".copy-content").remove();
}

function ajaxCopyInfo(id) {
    $.ajax({
        type: "POST",
        url: "/danh-sach-bai-viet-app.aspx/copyPostInfo",
        data: "{id: " + id + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var content = data.d;
            content = content.replace(/<br\s*[\/]?>/gi, "\n");
            content = content.replace(/<\/p\s*[\/]?>/gi, "</p>\n");
            $(".copy-content").html(content);
        }
    });
}