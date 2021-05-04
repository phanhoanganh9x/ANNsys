﻿var API = "/api/v1/woocommerce/product/";
var webList = ["ann.com.vn", "khohangsiann.com", "bosiquanao.net", "quanaogiaxuong.com", "bansithoitrang.net", "panpan.vn", "quanaoxuongmay.com", "annshop.vn", "thoitrangann.com", "nhapsionline.com"];
var webCosmetics = ["khosimypham.com", "simyphamonline.com"];
var cosmeticCategory = [44, 45, 56, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76];

function getWeblist(categoryID) {
    // San pham thuoc danh muc my pham, nuoc hoa, thuc pham chuc nang thi them web my pham vao
    var inCosmeticCategory = cosmeticCategory.includes(categoryID);

    // xoa web cosmetics ra khoi web list truoc khi check category
    webList = webList.filter(item => !webCosmetics.includes(item));
    if (inCosmeticCategory) {
        webList = webList.concat(webCosmetics);
    }
}
function showProductSyncModal(productSKU, productID, categoryID) {
    closePopup();

    var html = "";
    html += "<div class='row'><div class='col-md-12'><h2>Đồng bộ sản phẩm " + productSKU + "</h2><br></div></div>";
    html += "<div class='row'>";
    html += "    <div class='col-md-12 item-website' data-web='all' data-product-sku='" + productSKU + "' data-product-id='" + productID + "'>";
    html += "       <span>";
    html += "        	<a href='javascript:;' class='btn primary-btn btn-green' onclick='postProduct($(this))'><i class='fa fa-cloud-upload' aria-hidden='true'></i> Đăng</a>";
    html += "        	<a href='javascript:;' class='btn primary-btn btn-blue' onclick='upTopProduct($(this))'><i class='fa fa-upload' aria-hidden='true'></i> Up</a>";
    html += "        	<a href='javascript:;' class='btn primary-btn btn-black' onclick='renewProduct($(this))'><i class='fa fa-refresh' aria-hidden='true'></i> Làm mới</a>";
    html += "        	<a href='javascript:;' class='btn primary-btn btn-black' onclick='updateProductTag($(this))'><i class='fa fa-refresh' aria-hidden='true'></i> Thêm tag</a>";
    html += "        	<a href='javascript:;' class='btn primary-btn btn-black' onclick='updatePrice($(this))'><i class='fa fa-refresh' aria-hidden='true'></i> Sửa giá</a>";
    html += "        	<a href='javascript:;' class='btn primary-btn btn-black' onclick='updateProductSKU($(this))'><i class='fa fa-refresh' aria-hidden='true'></i> Sửa SKU</a>";
    html += "        	<a href='javascript:;' class='btn primary-btn btn-black' onclick='toggleProduct($(this), `hide`)'><i class='fa fa-refresh' aria-hidden='true'></i> Ẩn</a>";
    html += "        	<a href='javascript:;' class='btn primary-btn' onclick='toggleProduct($(this), `show`)'><i class='fa fa-refresh' aria-hidden='true'></i> Hiện</a>";
    html += "        	<a href='javascript:;' class='btn primary-btn btn-red' onclick='deleteProduct($(this), `show`)'><i class='fa fa-times' aria-hidden='true'></i> Xóa</a>";
    html += "       </span>";
    html += "    </div>";
    html += "</div>";
    html += "<div class='web-list'></div>";

    showPopup(html, 8);
    HoldOn.open();
    
    getWeblist(categoryID);

    for (var i = 0; i < webList.length; i++) {

        var button = "";
        button += "<span class='btn-not-found hide'>";
        button += "<a href='javascript:;' class='btn primary-btn btn-green' onclick='postProduct($(this))'>Đăng</a>";
        button += "</span>";
        button += "<span class='btn-had-found hide'>";
        button += "<a href='javascript:;' onclick='upTopProduct($(this))' class='btn primary-btn btn-blue'>Up</a>";
        button += "<a href='javascript:;' onclick='renewProduct($(this))' class='btn primary-btn btn-black'>Làm mới</a>";
        button += "<a href='javascript:;' onclick='viewProduct($(this))' class='btn primary-btn btn-yellow'>Xem</a>";
        button += "<a href='javascript:;' onclick='editProduct($(this))' class='btn primary-btn btn-black'>Sửa</a>";
        button += "<a href='javascript:;' onclick='updateProductTag($(this))' class='btn primary-btn btn-black'>Thêm tag</a>";
        button += "<a href='javascript:;' onclick='updatePrice($(this))' class='btn primary-btn btn-black'>Sửa giá</a>";
        button += "<a href='javascript:;' onclick='updateProductSKU($(this))' class='btn primary-btn btn-black'>Sửa SKU</a>";
        button += "<a href='javascript:;' onclick='toggleProduct($(this), `hide`)' class='btn primary-btn btn-black'>Ẩn</a>";
        button += "<a href='javascript:;' onclick='toggleProduct($(this), `show`)' class='btn primary-btn'>Hiện</a>";
        button += "<a href='javascript:;' onclick='deleteProduct($(this), `show`)' class='btn primary-btn btn-red'>Xóa</a>";
        button += "</span>";

        var webItem = "";
        webItem += "<div class='col-md-2 item-name'>" + webList[i] + "</div>";
        webItem += "<div class='col-md-2 item-status'><span class='bg-yellow'>Đang kết nối web...</span></div>";
        webItem += "<div class='col-md-8 item-button'>" + button + "</div>";

        $(".web-list").append("<div class='row item-website' data-web='" + webList[i] + "' data-product-sku='" + productSKU + "' data-product-id='" + productID + "' data-web-product-id=''>" + webItem + "</div>");

        getProduct(webList[i], productID);
    }
}

function getProduct(web, productID) {
    $.ajax({
        type: "GET",
        url: API + productID,
        headers: {
            'domain': web,
        },
        async: true,
        datatype: "json",
        success: function (data) {
            HoldOn.close();

            if (data.length > 0) {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-blue'>Tìm thấy sản phẩm</span>");
                $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").removeClass("hide");
                $("*[data-web='" + web + "']").attr("data-web-product-id", data[0].id);
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-blue'>Chưa tìm thấy sản phẩm</span>");
                $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").removeClass("hide");
            }
        },
        error: function () {
            HoldOn.close();
            $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
        }
    });
}

function postProduct(obj) {
    let web = obj.closest(".item-website").attr("data-web");
    let productID = obj.closest(".item-website").attr("data-product-id");

    if (web == "all") {
        for (var i = 0; i < webList.length; i++) {
            ajaxPostProduct(webList[i], productID);
        }
    }
    else {
        ajaxPostProduct(web, productID);
    }
}

function ajaxPostProduct(web, productID) {
    $.ajax({
        type: "POST",
        url: API + productID,
        headers: {
            'domain': web,
        },
        async: true,
        datatype: "json",
        beforeSend: function () {
            HoldOn.open();
            $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-yellow'>Đang đăng lên web...</span>");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").addClass("hide");
        },
        success: function (data, textStatus, xhr) {
            HoldOn.close();

            // Thành công
            if (xhr.status === 200) {
                if (data.id > 0) {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-green'>Đăng web thành công</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").removeClass("hide");
                    $("*[data-web='" + web + "']").attr("data-web-product-id", data.id);
                }
                else {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Đăng web thất bại</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").removeClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").addClass("hide");
                }
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
        },
        error: function (xhr, textStatus, error) {
            HoldOn.close();

            if (xhr.status === 500) {
                let data = xhr.responseJSON;
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>" + data.message + "</span>");
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
        }
    });
}

function upTopProduct(obj) {
    let web = obj.closest(".item-website").attr("data-web");
    let productID = obj.closest(".item-website").attr("data-product-id");

    if (web == "all") {
        for (var i = 0; i < webList.length; i++) {
            ajaxUpTopProduct(webList[i], productID);
        }
    }
    else {
        ajaxUpTopProduct(web, productID);
    }
}

function ajaxUpTopProduct(web, productID) {
    $.ajax({
        type: "POST",
        url: API + productID + "/uptop",
        headers: {
            'domain': web,
        },
        async: true,
        datatype: "json",
        beforeSend: function () {
            HoldOn.open();
            $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-yellow'>Đang up lên đầu web...</span>");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").addClass("hide");
        },
        success: function (data, textStatus, xhr) {
            HoldOn.close();

            // Thành công
            if (xhr.status === 200) {
                if (data.id > 0) {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-green'>Up lên đầu web thành công</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").removeClass("hide");
                }
                else {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Up lên đầu web thất bại</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").removeClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").addClass("hide");
                }
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
        },
        error: function (xhr, textStatus, error) {
            HoldOn.close();
            let data = xhr.responseJSON;
            if (xhr.status === 500) {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>" + data.message + "</span>");
            }
            else if (xhr.status === 400) {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>" + data.message + "</span>");
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
        }
    });
}

function renewProduct(obj) {
    let web = obj.closest(".item-website").attr("data-web");
    let productID = obj.closest(".item-website").attr("data-product-id");

    if (web == "all") {
        for (var i = 0; i < webList.length; i++) {
            ajaxRenewProduct(webList[i], productID);
        }
    }
    else {
        ajaxRenewProduct(web, productID);
    }
}

function ajaxRenewProduct(web, productID) {
    $.ajax({
        type: "POST",
        url: API + productID + "/renew",
        headers: {
            'domain': web,
        },
        async: true,
        datatype: "json",
        beforeSend: function () {
            HoldOn.open();
            $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-yellow'>Đang làm mới sản phẩm...</span>");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").addClass("hide");
        },
        success: function (data, textStatus, xhr) {
            HoldOn.close();

            // Thành công
            if (xhr.status === 200) {
                if (data.id > 0) {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-green'>Làm mới sản phẩm thành công</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").removeClass("hide");
                    $("*[data-web='" + web + "']").attr("data-web-product-id", data.id);
                }
                else {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Làm mới sản phẩm thất bại</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").removeClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").addClass("hide");
                }
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
        },
        error: function (xhr, textStatus, error) {
            HoldOn.close();
            let data = xhr.responseJSON;
            if (xhr.status === 500) {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>" + data.message + "</span>");
            }
            else if (xhr.status === 400) {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>" + data.message + "</span>");
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
        }
    });
}

function toggleProduct(obj, toggle) {
    let web = obj.closest(".item-website").attr("data-web");
    let productID = obj.closest(".item-website").attr("data-product-id");

    if (web == "all") {
        for (var i = 0; i < webList.length; i++) {
            ajaxToggleProduct(webList[i], productID, toggle);
        }
    }
    else {
        ajaxToggleProduct(web, productID, toggle);
    }
}

function ajaxToggleProduct(web, productID, toggle) {
    let status = "ẩn";
    if (toggle === "show") {
        status = "hiện";
    }

    $.ajax({
        type: "POST",
        url: API + productID + "/" + toggle,
        headers: {
            'domain': web,
        },
        async: true,
        datatype: "json",
        beforeSend: function () {
            HoldOn.open();
            
            $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-yellow'>Đang " + status + " sản phẩm</span>");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").addClass("hide");
        },
        success: function (data, textStatus, xhr) {
            HoldOn.close();

            // Thành công
            if (xhr.status === 200) {
                if (data.id > 0) {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-green'>Đã " + status + " sản phẩm thành công</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").removeClass("hide");
                }
                else {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Đã " + status + " sản phẩm thất bại</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").removeClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").addClass("hide");
                }
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
        },
        error: function (xhr, textStatus, error) {
            HoldOn.close();
            let data = xhr.responseJSON;
            if (xhr.status === 500) {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>" + data.message + "</span>");
            }
            else if (xhr.status === 400) {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>" + data.message + "</span>");
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
        }
    });
}

function updateProductSKU(obj) {
    let oldSKU = "";
    let newSKU = "";

    swal({
        title: "Nhập mã SKU cũ",
        text: "Nhập mã sản phẩm cũ:",
        type: "input",
        showCancelButton: true,
        closeOnConfirm: false,
        cancelButtonText: "Đóng",
        confirmButtonText: "Tiếp tục",
    }, function (inputOldSKU) {
        if (inputOldSKU != "") {
            oldSKU = inputOldSKU;
            swal({
                title: "Nhập mã SKU mới",
                text: "Nhập mã sản phẩm mới:",
                type: "input",
                showCancelButton: true,
                closeOnConfirm: false,
                cancelButtonText: "Đóng",
                confirmButtonText: "Xác nhận",
            }, function (inputNewSKU) {
                if (inputNewSKU != "") {
                    newSKU = inputNewSKU;

                    let web = obj.closest(".item-website").attr("data-web");

                    if (oldSKU !== "" && newSKU !== "") {
                        if (oldSKU === newSKU) {
                            swal("Thông báo", "Mã SKU cũ và mới giống nhau!", "error");
                        }
                        else {
                            swal.close();
                            if (web == "all") {
                                for (var i = 0; i < webList.length; i++) {
                                    ajaxUpdateProductSKU(webList[i], oldSKU, newSKU);
                                }
                            }
                            else {
                                ajaxUpdateProductSKU(web, oldSKU, newSKU);
                            }
                        }
                    }
                }
                else {
                    swal("Lỗi!", "Chưa nhập mã sản phẩm mới", "error");
                }
            });
        }
        else {
            swal("Lỗi!", "Chưa nhập mã sản phẩm cũ", "error");
        }
    });
    
    
}

function ajaxUpdateProductSKU(web, oldSKU, newSKU) {

    $.ajax({
        type: "POST",
        url: API + "updateSKU/" + oldSKU + "/" + newSKU,
        headers: {
            'domain': web,
        },
        async: true,
        datatype: "json",
        beforeSend: function () {
            HoldOn.open();

            $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-yellow'>Đang cập nhật SKU sản phẩm</span>");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").addClass("hide");
        },
        success: function (data, textStatus, xhr) {
            HoldOn.close();

            // Thành công
            if (xhr.status === 200) {
                if (data.id > 0) {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-green'>Cập nhật SKU thành công</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").removeClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").addClass("hide");
                }
                else {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Cập nhật SKU thất bại</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").removeClass("hide");
                }
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
        },
        error: function (xhr, textStatus, error) {
            HoldOn.close();
            let data = xhr.responseJSON;
            if (xhr.status === 500) {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>" + data.message + "</span>");
            }
            else if (xhr.status === 400) {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>" + data.message + "</span>");
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").removeClass("hide");
        }
    });
}

function updateProductTag(obj) {
    let web = obj.closest(".item-website").attr("data-web");
    let productID = obj.closest(".item-website").attr("data-product-id");

    swal({
        title: "Xác nhận",
        text: "Bạn muốn cập nhật tags cho sản phẩm này?",
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: true,
        cancelButtonText: "Để em xem lại...",
        confirmButtonText: "Đúng rồi sếp!",
    }, function (confirm) {
        if (confirm) {
            if (web == "all") {
                for (var i = 0; i < webList.length; i++) {
                    ajaxUpdateProductTag(webList[i], productID);
                }
            }
            else {
                ajaxUpdateProductTag(web, productID);
            }
        }
    });
}

function ajaxUpdateProductTag(web, productID) {

    $.ajax({
        type: "POST",
        url: API + "updateProductTag/" + productID,
        headers: {
            'domain': web,
        },
        async: true,
        datatype: "json",
        beforeSend: function () {
            HoldOn.open();

            $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-yellow'>Đang cập nhật tags sản phẩm</span>");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").addClass("hide");
        },
        success: function (data, textStatus, xhr) {
            HoldOn.close();

            // Thành công
            if (xhr.status === 200) {
                if (data.id > 0) {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-green'>Cập nhật tags thành công</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").removeClass("hide");
                }
                else {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Cập nhật tags thất bại</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").removeClass("hide");
                }
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
        },
        error: function (xhr, textStatus, error) {
            HoldOn.close();
            let data = xhr.responseJSON;
            if (xhr.status === 500) {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>" + data.message + "</span>");
            }
            else if (xhr.status === 400) {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>" + data.message + "</span>");
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").removeClass("hide");
        }
    });
}

function updatePrice(obj) {
    let web = obj.closest(".item-website").attr("data-web");
    let productID = obj.closest(".item-website").attr("data-product-id");

    swal({
        title: "Xác nhận",
        text: "Bạn muốn cập nhật giá cho sản phẩm này?",
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: true,
        cancelButtonText: "Để em xem lại...",
        confirmButtonText: "Đúng rồi sếp!",
    }, function (confirm) {
        if (confirm) {
            if (web == "all") {
                for (var i = 0; i < webList.length; i++) {
                    ajaxUpdatePrice(webList[i], productID);
                }
            }
            else {
                ajaxUpdatePrice(web, productID);
            }
        }
    });
}

function ajaxUpdatePrice(web, productID) {

    $.ajax({
        type: "POST",
        url: API + "updatePrice/" + productID,
        headers: {
            'domain': web,
        },
        async: true,
        datatype: "json",
        beforeSend: function () {
            HoldOn.open();

            $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-yellow'>Đang cập nhật giá sản phẩm</span>");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").addClass("hide");
        },
        success: function (data, textStatus, xhr) {
            HoldOn.close();

            // Thành công
            if (xhr.status === 200) {
                if (data.id > 0) {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-green'>Cập nhật giá thành công</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").removeClass("hide");
                }
                else {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Cập nhật giá thất bại</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").removeClass("hide");
                }
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
        },
        error: function (xhr, textStatus, error) {
            HoldOn.close();
            let data = xhr.responseJSON;
            if (xhr.status === 500) {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>" + data.message + "</span>");
            }
            else if (xhr.status === 400) {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>" + data.message + "</span>");
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").removeClass("hide");
        }
    });
}

function viewProduct(obj) {
    let web = obj.closest(".item-website").attr("data-web");
    let ID = obj.closest(".item-website").attr("data-web-product-id");
    let URL = "http://" + web + "/?p=" + ID;
    window.open(URL, '_blank');
}

function editProduct(obj) {
    let web = obj.closest(".item-website").attr("data-web");
    let ID = obj.closest(".item-website").attr("data-web-product-id");
    let URL = "http://" + web + "/wp-admin/post.php?post=" + ID + "&action=edit";
    window.open(URL, '_blank');
}

function deleteProduct(obj, toggle) {
    let web = obj.closest(".item-website").attr("data-web");
    let productID = obj.closest(".item-website").attr("data-product-id");

    if (web == "all") {
        for (var i = 0; i < webList.length; i++) {
            ajaxDeleteProduct(webList[i], productID);
        }
    }
    else {
        ajaxDeleteProduct(web, productID);
    }
}

function ajaxDeleteProduct(web, productID) {

    $.ajax({
        type: "POST",
        url: API + productID + "/delete",
        headers: {
            'domain': web,
        },
        async: true,
        datatype: "json",
        beforeSend: function () {
            HoldOn.open();

            $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-yellow'>Đang xóa sản phẩm</span>");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
            $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").addClass("hide");
        },
        success: function (data, textStatus, xhr) {
            HoldOn.close();

            // Thành công
            if (xhr.status === 200) {
                if (data.id > 0) {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-green'>Xóa sản phẩm thành công</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").removeClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").addClass("hide");
                    $("*[data-web='" + web + "']").attr("data-web-product-id", "");
                }
                else {
                    $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Xóa sản phẩm thất bại</span>");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-not-found").addClass("hide");
                    $("*[data-web='" + web + "']").find(".item-button").find(".btn-had-found").removeClass("hide");
                }
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
        },
        error: function (xhr, textStatus, error) {
            HoldOn.close();
            let data = xhr.responseJSON;
            if (xhr.status === 500) {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>" + data.message + "</span>");
            }
            else if (xhr.status === 400) {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>" + data.message + "</span>");
            }
            else {
                $("*[data-web='" + web + "']").find(".item-status").html("<span class='bg-red'>Lỗi kết nối trang con</span>");
            }
        }
    });
}