function getAllPostImage(id) {
    $.ajax({
        type: "POST",
        url: "/danh-sach-bai-viet-app.aspx/getAllPostImage",
        data: "{id: '" + id + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d !== "false") {
                var data = JSON.parse(msg.d);

                var link = document.createElement('a');

                for (var i = 0; i < data.length; i++) {
                    (function (i) {
                        setTimeout(function () {
                            link.setAttribute('download', '');
                            link.setAttribute('href', data[i]);
                            link.click();
                        }, 1000 * i);
                    })(i);
                }
            }
            else {
                alert("Lỗi");
            }
        }
    });
}