<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="nuoc-hoa.aspx.cs" Inherits="IM_PJ.nuoc_hoa" EnableSessionState="ReadOnly" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Nước hoa giá sỉ ANN</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1, user-scalable=yes">
    <meta name="format-detection" content="telephone=no">
    <meta name="robots" content="noindex, nofollow">
    <link rel="stylesheet" href="/App_Themes/Ann/css/style.css?v=04092021" media="all">
    <link rel="stylesheet" href="/App_Themes/Ann/css/style-P.css?v=04092021" media="all">
    <link href="/App_Themes/Ann/css/HoldOn.css?v=04092021" rel="stylesheet" type="text/css" />
    <link href="/App_Themes/NewUI/js/select2/select2.css" rel="stylesheet" />
    <script type="text/javascript" src="/App_Themes/Ann/js/jquery-2.1.3.min.js"></script>
    <style>
        #ContentModal .modal-footer {
            background-color: #333;
            margin-top: 0;
            padding: 10px 20px 20px;
            border-top: none;
        }
        .div-text {
            margin-bottom: 0;
            margin-left: -20px;
            margin-right: -20px;
            padding: 10px;
        }
        .div-text h3 {
            text-transform: uppercase;
            font-size: 14px;
            line-height: 1.7;
            margin-top: 0;
        }
        .div-text p {
            margin-left: 15px;
        }
        .div-1 {
           background-color: #fba04f;
           color: #fff;
           margin-top: -21px;
        }
        .div-2 {
            background-color: #a94442;
            color: #fff;
        }
        .div-3 {
            background-color: #31708f;
            color: #fff;
        }
        .div-4 {
            background-color: #333;
            color: #fff;
            margin-bottom: -50px;
        }
        .select2-container {
            box-sizing: border-box;
            display: inline-block;
            margin: 0;
            position: relative;
            vertical-align: middle;
            max-width: 100%;
        }
        .select2-container .select2-selection--single {
            height: 45px;
            box-sizing: border-box;
            cursor: pointer;
            display: block;
            user-select: none;
            -webkit-user-select: none;
        }
        .select2-container--default .select2-selection--single {
            background-color: #fff;
            border: solid 1px #e1e1e1;
        }
        .select2-container--default .select2-selection--single .select2-selection__rendered {
            line-height: 45px;
            padding-left: 15px;
            color: #444;
        }
        .select2-container .select2-selection--single .select2-selection__rendered {
            display: block;
            padding-right: 20px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }
        .select2-container--default .select2-selection--single .select2-selection__arrow {
            height: 43px;
            position: absolute;
            top: 1px;
            right: 10px;
            width: 20px;
        }
        .select2-container--default .select2-selection--single .select2-selection__arrow b {
            border-color: #000 transparent transparent transparent;
            border-style: solid;
            border-width: 5px 4px 0 4px;
            height: 0;
            left: 50%;
            margin-left: -4px;
            margin-top: -2px;
            position: absolute;
            top: 50%;
            width: 0;
        }
        .panel-table .panel-footer {
            padding: 5px 0;
        }
        .pagination li {
            display: inline-block;
            vertical-align: middle;
            line-height: 20px;
            font-weight: 600;
            background-color: #ff8400;
            color: #fff;
            font-size: 15px;
            margin-right: 10px;
        }
        .pagination li a {
            color: #fff;
            padding: 8px 11px;
        }
        .pagination li.current > a, .pagination li:hover > a {
            color: #000;
        }
        .btn {
            width: 100%;
            margin-bottom: 10px;
        }
        .btn.btn-product {
            background-color: #F44336;
        }
        .btn.btn-post {
            background-color: #009688;
        }
        .btn.btn-order {
            background-color: #000;
        }
        
        .btn.download-btn {
            background-color: #000;
            color: #fff;
            border-radius: 0;
            font-size: 14px;
        }
        .btn.primary-btn {
            border-radius: 0;
            font-size: 14px;
            padding-right: 8px!important;
            padding-left: 8px!important;
        }
        .btn.copy-btn {
            background-color: #E91E63;
            color: #fff;
        }
        .product-item {
            margin-bottom: 40px;
            background-color: #fff;
            padding: 15px;
        }
        h3 {
            margin-top: 10px;
        }
        .product-name a {
            font-size: 16px;
            line-height: 1.5;
        }
        .product-sku {
            font-size: 16px;
            color: #0289bc;
        }
        .product-price {
            font-size: 18px;
            color: #ff0023;
        }
        .price-retail {
            color: #428bca;
        }
        .bg-green, .bg-red, .bg-yellow {
            display: initial;
        }
        .margin-bottom-0 {
            margin-bottom: 0!important;
        }
        .btn.btn-menu-2 {
            background-color: #31708f;
        }
        .btn.btn-menu-3 {
            background-color: #683b8a;
        }
        .btn.btn-menu-4 {
            background-color: #f87703;
        }
    </style>
</head>
<body>
    <form id="form12" runat="server" enctype="multipart/form-data" onsubmit="return false;">
        <asp:ScriptManager runat="server" ID="scr">
        </asp:ScriptManager>
        <div>
            <main>
                <div class="container">
                    <div class="row">
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="http://nuochoaann.com" class="btn primary-btn h45-btn btn-order margin-bottom-0">Tất cả</a>
                            </div>
                        </div>
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="?price=30000" class="btn primary-btn h45-btn btn-product margin-bottom-0">Loại 30k</a>
                            </div>
                        </div>
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="?price=35000" class="btn primary-btn h45-btn btn-post margin-bottom-0">Loại 35k</a>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="?price=49000" class="btn primary-btn h45-btn btn-menu-2">Loại 49k</a>
                            </div>
                        </div>
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="?price=135000" class="btn primary-btn h45-btn btn-menu-3">Loại 135k</a>
                            </div>
                        </div>
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="?price=-1" class="btn primary-btn h45-btn btn-menu-4">Loại chai lớn</a>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="filter-above-wrap clear">
                                <div class="filter-control">
                                    <div class="row">
                                        <div class="col-md-9 col-xs-12">
                                            <div class="row">
                                                <div class="col-md-8 col-xs-12">
                                                </div>
                                                <div class="col-md-4 col-xs-12 margin-bottom-15">
                                                    <asp:TextBox ID="txtSearchProduct" runat="server" CssClass="form-control" placeholder="Tìm sản phẩm" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-xs-12">
                                            <div class="row">
                                                <div class="col-xs-6">
                                                    <a href="javascript:;" onclick="searchProduct()" class="btn primary-btn h45-btn"><i class="fa fa-search"></i> Tìm kiếm</a>
                                                </div>
                                                <div class="col-xs-6">
                                                    <a href="http://nuochoaann.com" class="btn primary-btn h45-btn download-btn"><i class="fa fa-home" aria-hidden="true"></i> Trang chủ</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4 col-md-offset-4 col-xs-12">
                            <a href="javascript:;" onclick="showContent()" class="btn primary-btn h45-btn btn-post"><i class="fa fa-quote-left" aria-hidden="true"></i> Chính sách bán sỉ</a>
                        </div>
                        <div class="col-md-4 col-md-offset-4 col-xs-12">
                            <a href="javascript:;" onclick="getAllCategoryImage(0)" class="btn primary-btn h45-btn btn-135k"><i class="fa fa-download" aria-hidden="true"></i> Tải tất cả hình</a>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel-table clear">
                                <div class="clear">
                                    <div class="pagination">
                                        <%this.DisplayHtmlStringPaging1();%>
                                    </div>
                                </div>
                                <div class="responsive-table">
                                    <asp:Literal ID="ltrList" runat="server" EnableViewState="false"></asp:Literal>
                                </div>
                                <div class="clear">
                                    <div class="pagination">
                                        <%this.DisplayHtmlStringPaging1();%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </main>

            <!-- Modal download image -->
            <div class="modal fade" id="ContentModal" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>
                        <div class="modal-body">
                            <div class="row form-group">
                                <div class="col-xs-12">
                                    <div class="div-text div-1">
                                        <h3>Đối với khách mới:</h3>
                                        <p> - Sỉ đơn đầu 10 chai bất kỳ (lựa mẫu thoải mái).</p>
                                        <p> - Được tính chung số lượng với quần áo (ví dụ: 6 áo + 4 nước hoa).</p>
                                        <h3>Đối với khách đã nhập sỉ quần áo:</h3>
                                        <p> - Sỉ đơn đầu 5 chai bất kỳ (lựa mẫu thoải mái).</p>
                                    </div>
                                    <div class="div-text div-2">
                                        <h3>Đối với khách cũ:</h3>
                                        <p> - Sỉ 1 chai nếu đến kho lấy (ship từ 3 sản phẩm).</p>
                                    </div>
                                    <div class="div-text div-3">
                                        <h3>Quy định đổi trả:</h3>
                                        <p> - Khách được đổi mùi hương miễn phí trong 30 ngày (chỉ áp dụng nước hoa).</p>
                                        <p> - Lưu ý, phải có hóa đơn mua hàng, không nhận trả hàng - hoàn tiền mặt.</p>
                                    </div>
                                    <div class="div-text div-4">
                                        <h3>Chiết khấu:</h3>
                                        <p> - Từ 30 sản phẩm: giảm 3.000/sp.</p>
                                        <p> - Từ 50 sản phẩm: giảm 5.000/sp.</p>
                                        <p> - Từ 70 sản phẩm: giảm 7.000/sp.</p>
                                        <p> - Từ 100 sản phẩm: giảm 10.000/sp.</p>
                                        <p> - Từ 200 sản phẩm: giảm 12.000/sp.</p>
                                        <p> - Từ 300 sản phẩm: giảm 13.000/sp.</p>
                                        <p> - Từ 500 sản phẩm: giảm 15.000/sp.</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button id="closeContentModal" type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Modal download image -->
            <div class="modal fade" id="DownloadModal" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Chọn loại sản phẩm cần tải hình</h4>
                        </div>
                        <div class="modal-body">
                            <div id="setting-shipper" class="row form-group">
                                <div class="col-xs-12">
                                    <a href="javascript:;" class="btn primary-btn h45-btn btn-product" onclick="getAllCategoryImage(30000)">Tải hình loại 30k</a>
                                    <a href="javascript:;" class="btn primary-btn h45-btn btn-post" onclick="getAllCategoryImage(35000)">Tải hình loại 35k</a>
                                    <a href="javascript:;" class="btn primary-btn h45-btn btn-menu-2" onclick="getAllCategoryImage(49000)">Tải hình loại 49k</a>
                                    <a href="javascript:;" class="btn primary-btn h45-btn btn-menu-3" onclick="getAllCategoryImage(135000)">Tải hình loại 135k</a>
                                    <a href="javascript:;" class="btn primary-btn h45-btn btn-menu-4" onclick="getAllCategoryImage(-1)">Tải hình loại chai lớn</a>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button id="closeDownloadModal" type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                        </div>
                    </div>
                </div>
            </div>

            <script src="/App_Themes/Ann/js/bootstrap.min.js"></script>
            <script src="/App_Themes/Ann/js/bootstrap-table/bootstrap-table.js"></script>
            <script src="/App_Themes/NewUI/js/select2/select2.min.js"></script>
            <script src="/App_Themes/Ann/js/master.js?v=04092021"></script>
            
            <script src="/App_Themes/Ann/js/sync-product-small.js?v=04092021"></script>
            <script src="/App_Themes/Ann/js/download-product-image.js?v=04092021"></script>
            <script src="/App_Themes/Ann/js/HoldOn.js?v=04092021"></script>

            <script type="text/javascript">
                window.Clipboard = (function (window, document, navigator) {
                    var textArea,
                        copy;

                    function isOS() {
                        return navigator.userAgent.match(/ipad|iphone/i);
                    }

                    function createTextArea(text) {
                        textArea = document.createElement('textArea');
                        textArea.style.position = 'fixed';
                        textArea.style.left = '0';
                        textArea.style.top = '0';
                        textArea.style.opacity = '0';
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


                function copyProductInfo(id) {
                    $('body').append($('<div>', {
                        "class": 'copy-product-info hide'
                    }));

                    ajaxCopyInfo(id);

                    Clipboard.copy($(".copy-product-info").text());

                    $(".copy-product-info").remove();
                }

                function ajaxCopyInfo(id) 
                {
                    $.ajax({
                        type: "POST",
                        url: "nuoc-hoa.aspx/copyProductInfo",
                        data: "{id: " + id + "}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            $(".copy-product-info").html(data.d);
                        }
                    });
                }
            </script>

            <script type="text/javascript">
                function showContent() 
                {
                    $("#ContentModal").modal({ show: 'true', backdrop: 'static', keyboard: 'false' });
                }
                function getAllCategoryImage(productPrice)
                {
                    if (productPrice == 0) 
                    {
                        $("#DownloadModal").modal({ show: 'true', backdrop: 'static', keyboard: 'false' });
                    }
                    else {
                        $.ajax({
                            type: "POST",
                            url: "/nuoc-hoa.aspx/getAllCategoryImage",
                            data: "{productPrice: " + productPrice + "}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                if (msg.d != "false") {
                                    var data = JSON.parse(msg.d);

                                    var link = document.createElement('a');

                                    for (var i = 0; i < data.length; i++) {
                                        (function (i) {
                                            setTimeout(function () {
                                                link.setAttribute('download', productPrice + '-' + (i + 1));
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
                    
                }

                $("#<%=txtSearchProduct.ClientID%>").keyup(function (e) {
                    if (e.keyCode == 13)
                    {
                        openURL();
                    }
                });

                function searchProduct()
                {
                    openURL();
                }

                function openURL(page)
                {
                    let strSearch = $("#<%=txtSearchProduct.ClientID%>").val();

                    let url = window.location.href;
                    let currentURL = new URL(url);
                    let newURL = window.location.hostname;
                    let redirectTo = "http://" + newURL;
                    let strParameter = url.match(/\?&?.+$/g);

                    if (strParameter) 
                    {
                        strParameter = strParameter[0];
                    }
                    else 
                    {
                        strParameter = "?";
                    }

                    let searchOld = currentURL.searchParams.get("textsearch");
                    if (strSearch) 
                    {
                        if (searchOld) 
                        {
                            strParameter = strParameter.replace("textsearch=" + encodeURI(searchOld), "textsearch=" + strSearch);
                        }
                        else 
                        {
                            strParameter += "&textsearch=" + strSearch
                        }
                    }
                    else 
                    {
                        if (searchOld) 
                        {
                            strParameter = strParameter.replace("textsearch=" + searchOld, "");
                        }
                    }

                    let pageOld = currentURL.searchParams.get("Page");
                    if (page) 
                    {
                        if (pageOld) 
                        {
                            strParameter = strParameter.replace("Page=" + pageOld, "Page=" + page);
                        }
                        else 
                        {
                            strParameter += "&Page=" + page
                        }
                    }
                    else 
                    {
                        if (pageOld) 
                        {
                            strParameter = strParameter.replace("Page=" + pageOld, "");
                        }
                    }

                    // Loại bỏ trường hợp price=30000&&textsearch=nuochoa => price=30000&textsearch=nuochoa 
                    strParameter = strParameter.replace(/\?&/g, "/?");
                    // Loại bỏ trường hợp price=30000&&textsearch=nuochoa => price=30000&textsearch=nuochoa 
                    strParameter = strParameter.replace(/&+/g, "&");
                    // Loại bỏ trường hợp textsearch=nuochoa& => textsearch=nuochoa 
                    strParameter = strParameter.replace(/&+$/g, "");

                    location.replace(redirectTo + strParameter);
                }

                $(document).ready(function () {
                    let url = window.location.href;
                    let currentURL = new URL(url);
                    let txtSearch = currentURL.searchParams.get("textsearch");
                    if (txtSearch)
                    {
                        $("#<%=txtSearchProduct.ClientID%>").val(txtSearch);
                    }
                    LoadSelect();
                });

                function LoadSelect() {
                    $(".select2").select2({
                        templateResult: formatState,
                        templateSelection: formatState
                    });
                    function formatState(opt) {
                        if (!opt.id) {
                            return opt.text;
                        }
                        var optimage = $(opt.element).data('image');
                        if (!optimage) {
                            return opt.text;
                        } else {
                            var $opt = $(
                                '<span>' + opt.text + '</span>'
                            );
                            return $opt;
                        }
                    };
                }


                $('.form-filter input').keyup(function (e) {
                    var $input = $(this),
                        inputContent = $input.val().toLowerCase(),
                        column = $('.form-filter input').index($input),
                        $table = $('#table-student'),
                        $rows = $table.find('tbody tr');

                    var $filteredRows = $rows.filter(function () {
                        var value = $(this).find('td').eq(column).text().toLowerCase();

                        if (value.indexOf(inputContent) > -1) {
                            $(this).show();
                        } else {
                            $(this).hide();
                        }


                    });


                    /* Clean no-result if exist */
                    /* Prepend no-result */
                    if ($table.find('tbody tr:visible').length === 0) {
                        $table.find('tbody').prepend($('<tr class="no-result text-center"><td colspan="3">No result found</td></tr>'));
                    } else {
                        $table.find('tbody .no-result').remove();
                    }
                });

                function OnClientFileSelected(sender, args) {
                    if ($telerik.isIE) return;
                    else {
                        truncateName(args);
                        //var file = args.get_fileInputField().files.item(args.get_rowIndex());
                        var file = args.get_fileInputField().files.item(0);
                        showThumbnail(file, args);
                    }
                }

                function truncateName(args) {
                    var $span = $(".ruUploadProgress", args.get_row());
                    var text = $span.text();
                    if (text.length > 23) {
                        var newString = text.substring(0, 23) + '...';
                        $span.text(newString);
                    }
                }

                function showThumbnail(file, args) {

                    var image = document.createElement("img");

                    image.file = file;
                    image.className = "ab img-responsive";

                    var $row = $(args.get_row());
                    $row.parent().className = "row ruInputs list-unstyled";
                    $row.append(image);


                    var reader = new FileReader();
                    reader.onload = (function (aImg) {
                        return function (e) {
                            aImg.src = e.target.result;
                        };
                    }(image));
                    var ret = reader.readAsDataURL(file);
                    var canvas = document.createElement("canvas");

                    ctx = canvas.getContext("2d");
                    image.onload = function () {
                        ctx.drawImage(image, 100, 100);
                    };

                }

                function isBlank(str) {
                    return (!str || /^\s*$/.test(str));
                }
            </script>
        </div>
    </form>
</body>
</html>
