// parameter temporary
var _productVariable = [];
var _isStock = false;

// order of item list
var orderItem = 0;

function showProductVariable(productVariables) {
    let html = "";

    // Header Popup
    html += "<div class='header-list'>"
    html += "   <table class='table table-checkable table-product table-popup-product'>";
    html += "      <tr id='search-product-header'>";
    html += "         <th class='order-item check-column'>";
    html += "             <input type='checkbox' id='check-all'onchange='check_all()'/>";
    html += "         </th>";
    html += "         <th class='image-column'>Ảnh</td>";
    html += "         <th class='name-column'>Sản phẩm</td>";
    html += "         <th class='sku-column'>Mã</td>";
    html += "         <th class='variable-column'>Thuộc tính</td>";
    html += "         <th class='quantity-column'>Số lượng</td>";
    html += "      </tr>";
    html += "   </table>";
    html += "</div>"
    html += "<div class='div-product-list scrollbar'>";
    html += "   <table class='table table-checkable table-product table-popup-product'>";

    // Body Popup
    productVariables.forEach(function(item) {
        html += "      <tr class='search-popup' id='search-product-detail';>";
        html += "         <td class='order-item check-column'>";
        html += "             <input class='check-popup' data-productVariableID='" + item.ProductVariableID + "' type='checkbox' onchange='check($(this))' />";
        html += "         </td>";
        html += "         <td class='image-column'><img src='/uploads/images/85x113/" + item.ProductImage + "'></td>";
        html += "         <td class='name-column'>" + item.ProductTitle + "</td>";
        html += "         <td class='sku-column key'>" + item.ChildSKU + "</td>";
        html += "         <td class='variable-column'>" + item.VariableValue + "</td>";
        html += "         <td class='quantity-column'>";
        html += "             <input type='text' class='form-control quantity in-quantity' "
                                  + "pattern='[0-9]{1,3}' "
                                  + "onblur='changeQuantityPopup($(this))' "
                                  + "onkeyup='pressKeyQuantityPopup($(this))' "
                                  + "onkeypress='return event.charCode >= 48 && event.charCode <= 57' "
                                  + "value='1' >";
        html += "         </td>";
        html += "      </tr>";
    });

    html += "   </table>";
    html += "</div>";

    // Footer Popup
    html += "<div class='footer-list'>";
    html += "   <a href='javascript: ;' class='btn link-btn' style='background-color:#f87703;float:right;color:#fff;' onclick='selectProduct()'>Chọn sản phẩm</a>";
    html += "</div >";

    showPopup(html);
}

// select all variable product
function check_all() {
    if ($('#check-all').is(":checked")) {
        $(".check-popup").prop('checked', true);
    } else {
        $(".check-popup").prop('checked', false);
    }
}

function check(row) {
    let productCount = $(".search-popup").length;
    let productCheckNumber = $(".search-popup > .order-item > .check-popup:checked").length;

    if (productCheckNumber === productCount) {
        $("#search-product-header > .order-item > #check-all").prop('checked', true);
    }
    else {
        $("#search-product-header > .order-item > #check-all").prop('checked', false);
    }
}

function changeQuantityPopup(obj) {
    var current = obj.val();

    if (current == 0 || current === "" || current === null) {
        obj.val("1");
    }
}

function pressKeyQuantityPopup(obj) {
    $(".in-quantity").keyup(function (e) {
        if (/\D/g.test(this.value)) {
            // Filter non-digits from input value.
            this.value = this.value.replace(/\D/g, '');
        }
        else if (e.which == 40) {
            // press down
            $(this).closest('tr').next().find('td:eq(' + $(this).closest('td').index() + ')').find(".in-quantity").focus().select();
        }
        else if (e.which == 38) {
            // press up
            $(this).closest('tr').prev().find('td:eq(' + $(this).closest('td').index() + ')').find(".in-quantity").focus().select();
        }
    });
}

// select a variable product
function selectProduct() {
    let productChecked = [];

    $(".search-popup > .order-item > .check-popup:checked").each(function (index, element) {
        let id = $(element).attr("data-productVariableID");
        let quantity = parseFloat($(element).parent().parent().find("input.quantity").val());

        let productTarget = _productVariable.filter(function (x) { return x.ProductVariableID == id })[0];

        // get quantity new
        productTarget.Quantity = quantity;

        productChecked.push(productTarget);
    });

    addProduct(productChecked);

    closePopup();
    $("#txtSearch").focus();
}

function addHtmlProductResult(item) {
    let customerType = $(".customer-type").val();
    let html = "";
    let SKU = "";
    let ProductVariableName = "";
    let ProductVariableValue = "";
    let ProductVariableSave = "";

    if (item.ProductStyle == 1) {
        SKU = item.ParentSKU;
    }
    else {
        SKU = item.ChildSKU;
    }

    if (item.VariableValue !== null && !isBlank(item.VariableValue)) {
        item.VariableValue.split(/:\s|<br\/>/).forEach(function (item, index) {
            if (index % 2) {
                ProductVariableValue += item + "|";
            }
            else {
                if (!isBlank(item)) {
                    ProductVariableName += item + "|";
                }
            }
        });
    }

    ProductVariableSave = item.VariableValue.replace(/<br\/>/g, "|");

    // for page them-moi-don-hang
    if (_isStock) {
        orderItem++;
    }

    if (typeof updateTemplateStock === "function") {
        // update stock which removed
        updateTemplateStock(item);
    }

    html += "<tr ondblclick='clickrow($(this))' class='product-result' ";

    if (typeof searchRemovedList === "function") {
        let data_orderdetail = searchRemovedList(SKU);

        html += "          " + data_orderdetail + " ";
    }

    html += "          data-giabansi='" + item.RegularPrice + "' "
                    + "data-giabanle='" + item.RetailPrice + "' "
                    + "data-quantityinstock='" + item.QuantityCurrent + "' "
                    + "data-productimageorigin='" + item.ProductImage + "' "
                    + "data-productvariable='" + item.VariableValue + "' "
                    + "data-productname='" + item.ProductTitle + "' "
                    + "data-sku='" + SKU + "' "
                    + "data-producttype='" + item.ProductStyle + "' "
                    + "data-productid='" + item.ProductID + "' "
                    + "data-productvariableid='" + item.ProductVariableID + "' "
                    + "data-productvariablename='" + ProductVariableName + "' "
                    + "data-productvariablevalue ='" + ProductVariableValue + "' "
                    + "data-price10 ='" + item.Price10 + "' "
                    + "data-bestprice ='" + item.BestPrice + "' "
                    + "data-productvariablesave ='" + ProductVariableSave + "'>";

    // for page them-moi-don-hang
    if (_isStock) {
        html += "   <td class='order-item'>" + orderItem + "</td>";
        html += "   <td class='image-item'><img onclick='openImage($(this))' src='/uploads/images/159x212/" + item.ProductImage + "'></td>";
    }
    else {
        html += "   <td class='image-item'><img onclick='openImage($(this))' src='/uploads/images/85x113/" + item.ProductImage + "'></td>";
    }
    html += "   <td class='name-item'><a href='/xem-san-pham?id=" + item.ProductID + "&variableid=" + item.ProductVariableID + "' target='_blank'>" + (customerType == 2 && item.OldPrice > 0 ? "<span class='sale-icon'>SALE</span> " : "") + item.ProductTitle + "</a></td>";
    html += "   <td class='sku-item'>" + SKU + "</td>";
    html += "   <td class='variable-item'>" + item.VariableValue + "</td>";

    if (customerType == 1) {
        if (item.RetailPrice > 0) {
            html += "   <td class='price-item gia-san-pham' data-price='" + item.RetailPrice + "'>" + formatThousands(item.RetailPrice, ',') + "</td>";
        }
        else {
            html += "   <td class='price-item gia-san-pham' data-price='" + item.RegularPrice + "'>" + formatThousands(item.RegularPrice, ',') + "</td>";
        }
    } else {
        html += "   <td class='price-item gia-san-pham' data-price='" + item.RegularPrice + "'>" + formatThousands(item.RegularPrice, ',') + "</td>";
    }

    //#region Triết khấu
    let discount = 0;

    if (typeof getDiscount === "function")
    {
        let totalQuantity = +$("input[id$='_hdfTotalQuantity']").val() || 0;

        discount = getDiscount(totalQuantity);
    }

    html += "    <td class='discount-item'> ";
    html += "        <input type='text' class='form-control discount' ";
    html += "               onclick='this.select()' ";
    html += "               onblur='onBlurDiscount($(this))' ";
    html += "               onkeyup='pressKeyDiscount($(this))' ";
    html += "               onkeypress='return event.charCode >= 48 && event.charCode <= 57' ";
    html += "               value='" + formatThousands(discount, ",") + "' />";
    html += "   </td>";
    //#endregion

    // for page them-moi-don-hang
    if (_isStock) {
        html += "<td class='quantity-item'>" + formatThousands(item.QuantityCurrent, ',') + "</td>";
    }

    html += "   <td class='quantity-item'> "
    html += "       <input type='text' class='form-control in-quantity' "
                        + "pattern='[0-9]{1,3}' "
                        + "onblur='checkQuantiy($(this))' "
                        + "onkeyup='pressKeyQuantity($(this))' "
                        + "onkeypress='return event.charCode >= 48 && event.charCode <= 57' ";

    // if product exists in table, increase quantity
    if (item.Quantity !== undefined) {
        html += "              value='" + formatThousands(item.Quantity, ',') + "' />";
    }
    else {
        html += "              value='1' />";
    }
    html += "   </td>";
    html += "   <td class='total-item totalprice-view'>" + formatThousands(item.RegularPrice, ',') + "</td>";
    html += "   <td class='trash-item'><a href='javascript:;' class='link-btn' onclick='deleteRow($(this))'><i class='fa fa-trash'></i></a></td>";
    html += "</tr>";

    $(".content-product").prepend(html);

    if (typeof removeProductExisted === "function") {
        removeProductExisted(item);
    }
}

// reindex item order
function reIndex(reverse) {
    if (reverse === undefined)
        reverse = false;

    var item = $(".order-item");
    for (var i = 1; i < item.length + 1; i++) {
        $(".order-item:eq(" + i + ")").html(reverse ? (item.length - i) : i);
    }
}

// change quantity of product
function checkQuantiy(obj) {
    var current = obj.val();

    if (current == 0 || current === "" || current === null)
        obj.val("1");

    getAllPrice(true);
}

function clickrow(obj) {
    if (!obj.find("td").eq(1).hasClass("checked")) {
        obj.find("td").addClass("checked");
    }
    else {
        obj.find("td").removeClass("checked");
    }
}

function pressKeyQuantity(obj) {
    $(".in-quantity").keyup(function (e) {
        if (/\D/g.test(this.value)) {
            // Filter non-digits from input value.
            this.value = this.value.replace(/\D/g, '');
        }
        else if (e.which == 40) {
            // press down
            $(this).closest('tr').next().find('td:eq(' + $(this).closest('td').index() + ')').find(".in-quantity").focus().select();
        }
        else if (e.which == 38) {
            // press up
            $(this).closest('tr').prev().find('td:eq(' + $(this).closest('td').index() + ')').find(".in-quantity").focus().select();
        }
    });
}

// remove product form list
function deleteRow(obj) {
    swal({
        title: "Xác nhận",
        text: "Bạn muốn xóa sản phẩm này?",
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: true,
        cancelButtonText: "Để em xem lại...",
        confirmButtonText: "Đúng rồi sếp!",
    }, function (confirm) {
        if (confirm) {
            let row = obj.parent().parent();

            if (row.attr("data-orderdetailid") > 0) {
                let id = 0;
                let sku = "";
                let productID = 0;
                let quantity = 0;

                id = row.attr("data-orderdetailid");
                sku = row.attr("data-sku");

                if (row.attr("data-producttype") == 1) {
                    productID = row.attr("data-productid");
                }
                else {
                    productID = row.attr("data-productvariableid");
                }

                quantity = row.find(".in-quantity").val();

                listOrderDetail.push(
                    new OrderDetailModel(
                        ID = id,
                        SKU = sku,
                        ProductID = productID,
                        Quantity = quantity)
                    );
            }

            row.remove();
            getAllPrice();
        }
    });
}

// Check product exists. Then add table or increase product old
function addProduct(products) {

    products.forEach(function (item) {
        let sku = item.SKU;
        let check = false;

        let target = $(".product-result[data-productid='" + item.ProductID + "'][data-productvariableid='" + item.ProductVariableID + "']");

        // Check SKU exists in table.
        if (target.length > 0) {
            let quantityinstock = parseFloat(target.attr("data-quantityinstock"));
            let quantityCurrent = parseFloat(target.find(".in-quantity").val());
            let newquantity = quantityCurrent;

            if (item.Quantity === undefined) {// case search match SKU
                newquantity = newquantity + 1;
            }
            else { // case choose product variable popup
                newquantity = newquantity + item.Quantity;
            }

            target.find(".in-quantity").val(newquantity);

            let price = parseFloat(newquantity) * parseFloat(item.Giabansi);
            target.find(".totalprice-view").html(formatThousands(price, ','));
        }
        else {
            addHtmlProductResult(item);
        }
    });

    getAllPrice(true);
    $("#txtSearch").val("");
}

// search product by SKU
function searchProductMaster(textsearch, isStock) {
    var regex = /^[\x20-\x7E]*$/;
    if (!regex.test(textsearch)) {
        $("#txtSearch").val(textsearch).select();
        swal("Tắt bộ gõ Tiếng Việt", "Hãy tắt bộ gõ Tiếng Việt có dấu:<br><br>Nhấn phím <strong>Ctrl + Shirt</strong> cùng lúc", "error");
    }
    else {
        _productVariable = [];
        _isStock = isStock;

        if (!isBlank(textsearch)) {
            $.ajax({
                type: "POST",
                url: "ServiceCommon.asmx/GetProduct",
                data: JSON.stringify({ SKU: textsearch, isStock: _isStock }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    let data = JSON.parse(msg.d);

                    if (data !== null && data.length > 0) {
                        if (data.length > 1) {
                            // init value array product variable
                            _productVariable = data;

                            // show popup which choose product
                            showProductVariable(_productVariable);
                        }
                        else {
                            addProduct(data);
                        }
                    }
                    else {
                        $("#txtSearch").val(textsearch).select();
                        swal("Không tìm thấy", "Không tìm thấy sản phẩm", "error");
                    }
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    alert('lỗi');
                    $("#txtSearch").val(textsearch).select();
                }
            });
        } else {
            $("#txtSearch").focus();
            swal("Hãy nhập sản phẩm", "Hãy nhập mã sản phẩm", "error");
        }
    }
}