﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xem-nuoc-hoa.aspx.cs" Inherits="IM_PJ.xem_nuoc_hoa" EnableSessionState="ReadOnly" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Xem sản phẩm</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1, user-scalable=yes">
    <meta name="format-detection" content="telephone=no">
    <meta name="robots" content="noindex, nofollow">
    <link rel="stylesheet" href="/App_Themes/Ann/css/style.css?v=20092023" media="all">
    <link rel="stylesheet" href="/App_Themes/Ann/css/style-P.css?v=202311140145" media="all">
    <link href="/App_Themes/Ann/css/HoldOn.css?v=20092023" rel="stylesheet" type="text/css" />
    <link href="/App_Themes/NewUI/js/select2/select2.css" rel="stylesheet" />
    <script type="text/javascript" src="/App_Themes/Ann/js/jquery-2.1.3.min.js"></script>
    <style>
        ul.image-gallery li {
            width: 100%;
        }
        img {
            width: auto;
            max-width: 100%;
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
        .btn.download-btn {
            background-color: #000;
            color: #fff;
            border-radius: 0;
            font-size: 14px;
            text-transform: uppercase;
        }
        .btn.primary-btn {
            border-radius: 0;
            font-size: 14px;
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
            font-size: 18px;
            margin-top: 10px;
        }
        .product-name a {
            font-size: 18px;
            line-height: 1.5;
        }
        .product-sku {
            font-size: 18px;
            color: #0289bc;
        }
        .product-price {
            font-size: 20px;
            color: #ff0023;
        }
        .selected {
            background-color: #e0fcff;
        }
    </style>
</head>
<body>
    <form id="form12" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager runat="server" ID="scr">
        </asp:ScriptManager>
            <main>
                <div class="container">
                    <div class="row">
                        <a href="javascript:;" onclick="goBack()" class="btn primary-btn h45-btn"><i class="fa fa-angle-left"></i><i class="fa fa-angle-left"></i> Trở về</a>
                    </div>
                    <div class="row">
                        <div class="col-md-12 product-item">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="product-info">
                                        <h3 class="product-name"><asp:Literal ID="ltrProductName" runat="server"></asp:Literal></h3>
                                        <asp:Literal ID="ltrRegularPrice" runat="server"></asp:Literal>
                                        <asp:Literal ID="ltrRetailPrice" runat="server"></asp:Literal>
                                        <asp:Literal ID="ltrMaterials" runat="server"></asp:Literal>
                                        <asp:Literal ID="ltrContent" runat="server"></asp:Literal>
                                        <asp:Literal ID="ltrVariable" runat="server"></asp:Literal>
                                        <asp:Literal ID="ltrProductStock" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Literal ID="ltrButton" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Literal ID="ProductThumbnail" runat="server"></asp:Literal>
                                    <asp:Literal ID="imageGallery" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row tableview" id="variableTable">
                        <div class="col-md-12">
                            <div class="panel-table clear">
                                <asp:Literal ID="ltrVariableList" runat="server"></asp:Literal>
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
                    <div class="row">
                        <a href="javascript:;" onclick="goBack()" class="btn primary-btn h45-btn"><i class="fa fa-angle-left"></i><i class="fa fa-angle-left"></i> Trở về</a>
                    </div>
                </div>
            </main>

            <a href="javascript:;" class="scroll-top-link" id="scroll-top"><i class="fa fa-angle-up"></i></a>
            <a href="javascript:;" class="scroll-bottom-link" id="scroll-bottom"><i class="fa fa-angle-down"></i></a>

            <script src="/App_Themes/Ann/js/bootstrap.min.js"></script>
            <script src="/App_Themes/Ann/js/bootstrap-table/bootstrap-table.js"></script>
            <script src="/App_Themes/NewUI/js/select2/select2.min.js"></script>
            <script src="/App_Themes/Ann/js/master.js?v=2011"></script>
            <script src="/App_Themes/Ann/js/download-product-image.js?v=20092023"></script>

            <script src="/App_Themes/Ann/js/HoldOn.js?v=2011"></script>

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

                function copyProduct(id) {
                    $(".product-info").addClass("selected");
                    copyProductInfo(id);
                }
                function goBack() {
                    window.history.back();
                }

                $(document).ready(function () {
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
    </form>
</body>
</html>
