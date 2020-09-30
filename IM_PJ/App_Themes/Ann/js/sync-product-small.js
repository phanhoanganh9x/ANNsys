function ShowUpProductToWeb(sku, id, category, up, renew, visibility) {
    var web = ["ann.com.vn", "khohangsiann.com", "bosiquanao.net", "quanaogiaxuong.com", "bansithoitrang.net", "quanaoxuongmay.com", "annshop.vn", "panpan.vn", "nhapsionline.com", "thoitrangann.com"];
    var web_dobo = ["chuyensidobo.com"];
    var web_vaydam = ["damgiasi.vn"];

    if (category == 18) {
        web = web.concat(web_dobo);
    }
    if (category == 17) {
        web = web.concat(web_vaydam);
    }

    HoldOn.open();
    if ($(".hidden-" + id).hasClass("product-hidden"))
    {
        for (var i = 0; i < web.length; i++)
        {
            upProductToWeb(web[i], sku, id, up, renew, i, "visible");
        }

        HoldOn.close();
        HoldOn.open();
        ProductService.updateHidden(id, false)
            .then(() => {
                swal({
                    title: "Xác nhận",
                    text: "Bạn muốn phục hồi lại kho?",
                    type: "warning",
                    showCancelButton: true,
                    closeOnConfirm: true,
                    cancelButtonText: "Để em xem lại...",
                    confirmButtonText: "Đúng rồi sếp!",
                }, function (confirm) {
                    if (confirm) {
                        HoldOn.open();
                        ProductService.recoverLiquidated(id, sku)
                            .then(data => {
                                if (data) {
                                    $(".hidden-" + id).html("<i class=\"fa fa-times\" aria-hidden=\"true\"></i> Ẩn");
                                    $(".hidden-" + id).removeClass("product-hidden");

                                    let quantityDOM = document.querySelector(".product-number[data-product-id='" + id + "']");
                                    quantityDOM.innerHTML = '<p>🔖 <span class="bg-green">Còn hàng</span> (' + data.TotalProductInstockQuantityLeft + ' cái)</p>';
                                }
                            })
                            .catch(err => {
                                setTimeout(function () {
                                    swal("Thông báo", "Sản phẩn này không thể phục hồi xã kho được nhe!", "error");
                                }, 500);
                            })
                            .finally(() => { HoldOn.close(); });
                    }
                });
            })
            .catch(err => {
                setTimeout(function () {
                    swal("Thông báo", "Có lỗi trong qua trình ẩn sản phẩm", "error");
                }, 500);
            })
            .finally(() => { HoldOn.close(); });
    }
    else
    {
        for (var i = 0; i < web.length; i++)
        {
            upProductToWeb(web[i], sku, id, up, renew, i, visibility);
        }

        HoldOn.close();
        HoldOn.open();
        ProductService.updateHidden(id, true)
            .then(() => {
                swal({
                    title: "Xác nhận",
                    text: "Bạn muốn xả kho sản phẩm này không?",
                    type: "warning",
                    showCancelButton: true,
                    closeOnConfirm: true,
                    cancelButtonText: "Để em xem lại...",
                    confirmButtonText: "Đúng rồi sếp!",
                }, function (confirm) {
                    if (confirm) {
                        HoldOn.open();
                        ProductService.liquidate(id)
                            .then(data => {
                                if (data) {
                                    $(".hidden-" + id).html("<i class=\"fa fa-check\" aria-hidden=\"true\"></i> Hiện");
                                    $(".hidden-" + id).addClass("product-hidden");

                                    let quantityDOM = document.querySelector(".product-number[data-product-id='" + id + "']");
                                    quantityDOM.innerHTML = '<p>🔖 <span class="bg-red">Hết hàng</span> (0 cái)</p>';
                                }
                            })
                            .catch(err => {
                                setTimeout(function () {
                                    swal("Thông báo", "Có lỗi trong qua trình xã kho", "error");
                                }, 500);
                            })
                            .finally(() => { HoldOn.close(); });
                    }
                });
            })
            .catch(err => {
                setTimeout(function () {
                    swal("Thông báo", "Có lỗi trong qua trình ẩn sản phẩm", "error");
                }, 500);
            })
            .finally(() => { HoldOn.close(); });
    }
}

function upProductToWeb(web, sku, id, up, renew, i, visibility) {
    var url_web = "https://" + web + "/up-product";
    if (web == "panpan.vn") {
        url_web = url_web + ".html";
    }
    $.ajax({
        type: "POST",
        url: url_web,
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
        },
        success: function (data) {
            if (data.success === "true") {
                if(visibility == "hidden") {
                    $(".hidden-" + id).html("<i class=\"fa fa-times\" aria-hidden=\"true\"></i> Ẩn");
                    $(".hidden-" +id).addClass("product-hidden");
                }
                else {
                    $(".hidden-" + id).html("<i class=\"fa fa-check\" aria-hidden=\"true\"></i> Hiện");
                    $(".hidden-" +id).removeClass("product-hidden");
                }
            }
            else {
                if (data.content === "notfound") {
                    $(".hidden-" + id).html("<i class=\"fa fa-exclamation-triangle\" aria-hidden=\"true\"></i> Notfound");
                }
            }
        },
        error: function () {

        }
    });
}