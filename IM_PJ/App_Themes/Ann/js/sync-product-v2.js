﻿function ShowUpProductToWeb(sku, id, category, up, renew, visibility) {
    var web = ["pqstore.vn"];
    //var web_dobo = ["chuyensidobo.com"];
    //var web_vaydam = ["damgiasi.vn"];

    //if (category == 18) {
    //    web = web.concat(web_dobo);
    //}
    //if (category == 17) {
    //    web = web.concat(web_vaydam);
    //}

    closePopup();
    var html = "<div class='row'><div class='col-md-12'><h2>Đồng bộ sản phẩm " + sku + "</h2><br></div></div>";
    html += "<div class='row'><div class='col-md-12'><p><span><a href='javascript:;' class='btn primary-btn h45-btn' onclick='ShowUpProductToWeb(`" + sku + "`, `" + id + "`, `" + category + "`, `true`, `false`, `null`)'><i class='fa fa-upload' aria-hidden='true'></i> Up tất cả</a><a href='javascript:;' class='btn primary-btn h45-btn btn-black print-invoice-merged' onclick='ShowUpProductToWeb(`" + sku + "`, `" + id + "`, `" + category + "`, `false`, `true`, `null`)'><i class='fa fa-cloud-upload' aria-hidden='true'></i> Đăng lên tất cả web</a><a href='javascript:;' class='btn primary-btn h45-btn btn-black print-invoice-merged' onclick='ShowUpProductToWeb(`" + sku + "`, `" + id + "`, `" + category + "`, `false`, `true`, `null`)'><i class='fa fa-refresh' aria-hidden='true'></i> Làm mới hình tất cả web</a><a href='javascript:;' class='btn primary-btn h45-btn btn-black print-invoice-merged' onclick='ShowUpProductToWeb(`" + sku + "`, `" + id + "`, `" + category + "`, `false`, `false`, `hidden`)'><i class='fa fa-refresh' aria-hidden='true'></i> Ẩn tất cả</a><a href='javascript:;' class='btn primary-btn h45-btn btn-black print-invoice-merged' onclick='ShowUpProductToWeb(`" + sku + "`, `" + id + "`, `" + category + "`, `false`, `false`, `visible`)'><i class='fa fa-refresh' aria-hidden='true'></i> Hiện tất cả</a></span></p></div></div><div class='web-list'></div>";
    showPopup(html, 10);
    HoldOn.open();
    
    if (renew == "true")
    {
        for (var i = 0; i < web.length; i++)
        {
            checkProduct(web[i], sku, id, up, renew, i, visibility, 'false');
        }
    }
    else
    {
        for (var i = 0; i < web.length; i++)
        {
            checkProduct(web[i], id, sku);
        }
    }
}

function checkProduct(web, id, sku) {

    $.ajax({
        type: "GET",
        url: "http://ann-product-sync.com/api/v1/product/" + id,
        headers: {
            'domain': 'https://' + web + '/wp-json/wc/v3/',
            'user': 'ck_122f779b24b395477fa9cae87aed72b2ed09f5de',
            'pass': 'cs_b2b23553086c6bd6c037cb95301e1181251568ab',
            'price_type': 'Retail Price'
        },
        async: true,
        datatype: "json",
        beforeSend: function () {
            $(".content-upload-" + i).html("<span class='bg-yellow'>Đang xử lý</span>");
        },
        success: function (data) {
            HoldOn.close();

            if (data.length > 0) {
                var content = "";
                var button = "";
                content = "<span class='bg-blue'>Tìm thấy sản phẩm</span>";
                button = "<a href='javascript:;' class='btn primary-btn h45-btn' onclick='upProductToWeb(`" + web + "`, `" + sku + "`, `" + id + "`, `true`, 'false', `" + i + "`, `null`, `false`)'><i class='fa fa-upload' aria-hidden='true'></i> Up</a>";
                button += "<a href='javascript:;' class='btn primary-btn btn-black h45-btn print-invoice-merged' onclick='upProductToWeb(`" + web + "`, `" + sku + "`, `" + id + "`, `false`, `true`, `" + i + "`, `null`, `true`)'><i class='fa fa-refresh' aria-hidden='true'></i> Làm mới hình</a>";
                button += "<a href='https://" + web + "/?s=" + sku + "&post_type=product' target='_blank' class='btn primary-btn btn-black h45-btn print-invoice-merged'><i class='fa fa-link' aria-hidden='true'></i> Xem</a>";
                button += "<a href='https://" + web + "/wp-admin/edit.php?s=" + sku + "&post_type=product' target='_blank' class='btn primary-btn btn-black h45-btn print-invoice-merged'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Sửa</a>";
                button += "<a href='javascript:;' class='btn primary-btn h45-btn print-invoice-merged' onclick='upProductToWeb(`" + web + "`, `" + sku + "`, `" + id + "`, `false`, `false`, `" + i + "`, `hidden`, `false`)'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Ẩn</a>";
                button += "<a href='javascript:;' class='btn primary-btn h45-btn print-invoice-merged' onclick='upProductToWeb(`" + web + "`, `" + sku + "`, `" + id + "`, `false`, `false`, `" + i + "`, `visible`, `false`)'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Hiện</a>";
            }
            else {
                content = "<span class='bg-blue'>Chưa tìm thấy sản phẩm</span>";
                button = "<a href='javascript:;' class='btn primary-btn h45-btn' onclick='upProductToWeb(`" + web + "`, `" + sku + "`, `" + id + "`, `true`, `true`, `" + i + "`, `visible`, `true`)'><i class='fa fa-cloud-upload' aria-hidden='true'></i> Đăng web</a>";
            }

            web_item = "<div class='col-md-3'><p><i class='fa fa-arrow-right' aria-hidden='true'></i> " + web + "</p></div><div class='col-md-3'><p><span class='content-upload-" + i + "'>" + content + "</span></p></div><div class='col-md-6'><p>" + button + "</p></div>";

            if ($("div").hasClass("upload-" + i) === true) {
                $(".upload-" + i).html(web_item);
            }
            else {
                $(".web-list").append("<div class='row upload-" + i + "'>" + web_item + "</div>");
            }

        },
        error: function () {

            web_item = "<div class='col-md-3'><p><i class='fa fa-arrow-right' aria-hidden='true'></i> " + web + "</p></div><div class='col-md-9'><p><span class='bg-red'>Lỗi kết nối trang con</span></p></div>";

            if ($("div").hasClass("upload-" + i) === true) {
                $(".upload-" + i).html(web_item);
            }
            else {
                $(".web-list").append("<div class='row upload-" + i + "'>" + web_item + "</div>");
            }

        }
    });

}

function ajaxUpProductToWeb(web, sku, id, up, renew, i, visibility) {

    $.ajax({
        type: "POST",
        url: url_web,
        headers: {
            'Authorization': 'Basic xxxxxxxxxxxxx',
            'X-CSRF-TOKEN': 'xxxxxxxxxxxxxxxxxxxx',
            'Content-Type': 'application/json'
        },
        data: {
            sku: sku,
            systemid: id,
            up: up,
            renew: renew,
            visibility: visibility,
            key: '828327'
        },
        async: true,
        datatype: "json",
        beforeSend: function () {
            $(".content-upload-" + i).html("<span class='bg-yellow'>Đang xử lý</span>");
        },
        success: function (data) {
            HoldOn.close();

            if (data.success === "true") {
                var content = "";
                var button = "";
                if (data.content === "found") {
                    content = "<span class='bg-blue'>Tìm thấy sản phẩm</span>";
                    button = "<a href=\"javascript:;\" class=\"btn primary-btn h45-btn\" onclick=\"upProductToWeb('" + web + "', '" + sku + "', '" + id + "', 'true', 'false', '" + i + "', 'null', 'false')\"><i class=\"fa fa-upload\" aria-hidden=\"true\"></i> Up</a>";
                    button += "<a href=\"javascript:;\" class=\"btn primary-btn btn-black h45-btn print-invoice-merged\" onclick=\"upProductToWeb('" + web + "', '" + sku + "', '" + id + "', 'false', 'true', '" + i + "', 'null', 'true')\"><i class=\"fa fa-refresh\" aria-hidden=\"true\"></i> Làm mới hình</a>";
                }
                else if (data.content === "updone") {
                    content = "<span class='bg-green'>Up thành công</span>";
                    button = "<a href=\"javascript:;\" class=\"btn primary-btn btn-black h45-btn\" onclick=\"upProductToWeb('" + web + "', '" + sku + "', '" + id + "', 'false', 'true', '" + i + "', 'null', 'true')\"><i class=\"fa fa-refresh\" aria-hidden=\"true\"></i> Làm mới hình</a>";
                }
                else if (data.content === "renewdone") {
                    content = "<span class='bg-green'>Đăng web thành công</span>";
                }
                else if (data.content === "hidealldone") {
                    content = "<span class='bg-green'>Ẩn web thành công</span>";
                    button += "<a href=\"javascript:;\" class=\"btn primary-btn btn-black h45-btn print-invoice-merged\" onclick=\"upProductToWeb('" + web + "', '" + sku + "', '" + id + "', 'false', 'true', '" + i + "', 'null', 'true')\"><i class=\"fa fa-refresh\" aria-hidden=\"true\"></i> Làm mới hình</a>";
                }
                else if (data.content === "visiblealldone") {
                    content = "<span class='bg-green'>Hiện web thành công</span>";
                    button = "<a href=\"javascript:;\" class=\"btn primary-btn h45-btn\" onclick=\"upProductToWeb('" + web + "', '" + sku + "', '" + id + "', 'true', 'false', '" + i + "', 'null', 'false')\"><i class=\"fa fa-upload\" aria-hidden=\"true\"></i> Up</a>";
                    button += "<a href=\"javascript:;\" class=\"btn primary-btn btn-black h45-btn print-invoice-merged\" onclick=\"upProductToWeb('" + web + "', '" + sku + "', '" + id + "', 'false', 'true', '" + i + "', 'null', 'true')\"><i class=\"fa fa-refresh\" aria-hidden=\"true\"></i> Làm mới hình</a>";
                }
                button += "<a href=\"https://" + web + "/?s=" + sku + "&post_type=product\" target=\"_blank\" class=\"btn primary-btn btn-black h45-btn print-invoice-merged\"><i class=\"fa fa-link\" aria-hidden=\"true\"></i> Xem</a>";
                button += "<a href=\"https://" + web + "/wp-admin/edit.php?s=" + sku + "&post_type=product\" target=\"_blank\" class=\"btn primary-btn btn-black h45-btn print-invoice-merged\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Sửa</a>";
                button += "<a href=\"javascript:;\" class=\"btn primary-btn h45-btn print-invoice-merged\" onclick=\"upProductToWeb('" + web + "', '" + sku + "', '" + id + "', 'false', 'false', '" + i + "', 'hidden', 'false')\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Ẩn</a>";
                button += "<a href=\"javascript:;\" class=\"btn primary-btn h45-btn print-invoice-merged\" onclick=\"upProductToWeb('" + web + "', '" + sku + "', '" + id + "', 'false', 'false', '" + i + "', 'visible', 'false')\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i> Hiện</a>";
            }
            else {
                if (data.content === "uperror") {
                    content = "<span class='bg-red'>Lỗi khi up sản phẩm</span>";
                }
                else if (data.content === "renewerror") {
                    content = "<span class='bg-red'>Lỗi khi đăng lên web</span>";
                }
                else if (data.content === "notfound") {
                    content = "<span class='bg-blue'>Chưa tìm thấy sản phẩm</span>";
                    button = "<a href=\"javascript:;\" class=\"btn primary-btn h45-btn\" onclick=\"upProductToWeb('" + web + "', '" + sku + "', '" + id + "', 'true', 'true', '" + i + "', 'visible', 'true')\"><i class=\"fa fa-cloud-upload\" aria-hidden=\"true\"></i> Đăng web</a>";
                }
            }

            web_item = "<div class='col-md-3'><p><i class=\"fa fa-arrow-right\" aria-hidden=\"true\"></i> " + web + "</p></div><div class='col-md-3'><p><span class='content-upload-" + i + "'>" + content + "</span></p></div><div class='col-md-6'><p>" + button + "</p></div>";

            if ($("div").hasClass("upload-" + i) === true) {
                $(".upload-" + i).html(web_item);
            }
            else {
                $(".web-list").append("<div class='row upload-" + i + "'>" + web_item + "</div>");
            }

        },
        error: function () {

            web_item = "<div class='col-md-3'><p><i class=\"fa fa-arrow-right\" aria-hidden=\"true\"></i> " + web + "</p></div><div class='col-md-9'><p><span class='bg-red'>Lỗi kết nối trang con</span></p></div>";

            if ($("div").hasClass("upload-" + i) === true) {
                $(".upload-" + i).html(web_item);
            }
            else {
                $(".web-list").append("<div class='row upload-" + i + "'>" + web_item + "</div>");
            }

        }
    });
}