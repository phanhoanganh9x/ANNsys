﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sp.aspx.cs" Inherits="IM_PJ.sp" EnableSessionState="ReadOnly" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Sản phẩm ANN</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1, user-scalable=yes">
    <meta name="format-detection" content="telephone=no">
    <meta name="robots" content="noindex, nofollow">
    <link rel="stylesheet" href="/App_Themes/Ann/css/style.css?v=0110" media="all">
    <link rel="stylesheet" href="/App_Themes/Ann/css/style-P.css?v=0110" media="all">
    <link href="/App_Themes/NewUI/js/select2/select2.css" rel="stylesheet" />
    <script type="text/javascript" src="/App_Themes/Ann/js/jquery-2.1.3.min.js"></script>
    <style>
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
        .btn.download-btn {
            background-color: #000;
            color: #fff;
            border-radius: 0;
            font-size: 16px;
            text-transform: uppercase;
        }
        .btn.primary-btn {
            border-radius: 0;
            font-size: 16px;
            text-transform: uppercase;
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
    </style>
</head>
<body>
    <form id="form12" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager runat="server" ID="scr">
        </asp:ScriptManager>
        <div>
            <main>
                <div class="container">
                    <div class="row">
                        <div class="col-xs-6">
                            <div class="row">
                                <a href="/sp" class="btn primary-btn h45-btn btn-product"><i class="fa fa-sign-in" aria-hidden="true"></i> Sản phẩm</a>
                            </div>
                        </div>
                        <div class="col-xs-6">
                            <div class="row">
                                <a href="/bv" class="btn primary-btn h45-btn btn-post"><i class="fa fa-sign-in" aria-hidden="true"></i> Bài viết</a>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="filter-above-wrap clear">
                                <div class="filter-control">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtSearchProduct" runat="server" CssClass="form-control sku-input" placeholder="Tìm sản phẩm" autocomplete="off"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:DropDownList ID="ddlStockStatus" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="" Text="Trạng thái"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Còn hàng"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Hết hàng"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Nhập hàng"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:DropDownList ID="ddlCreatedDate" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="" Text="Tất cả thời gian"></asp:ListItem>
                                                <asp:ListItem Value="today" Text="Hôm nay"></asp:ListItem>
                                                <asp:ListItem Value="yesterday" Text="Hôm qua"></asp:ListItem>
                                                <asp:ListItem Value="beforeyesterday" Text="Hôm kia"></asp:ListItem>
                                                <asp:ListItem Value="week" Text="Tuần này"></asp:ListItem>
                                                <asp:ListItem Value="month" Text="Tháng này"></asp:ListItem>
                                                <asp:ListItem Value="7days" Text="7 ngày"></asp:ListItem>
                                                <asp:ListItem Value="30days" Text="30 ngày"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="col-xs-6">
                                                <div class="row">
                                                    <a href="javascript:;" onclick="searchProduct()" class="btn primary-btn h45-btn"><i class="fa fa-search"></i> Tìm kiếm</a>
                                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn primary-btn h45-btn" OnClick="btnSearch_Click" Style="display: none" />
                                                </div>
                                            </div>
                                            <div class="col-xs-6">
                                                <div class="row">
                                                    <a href="/sp" class="btn primary-btn h45-btn download-btn"><i class="fa fa-times" aria-hidden="true"></i> Làm lại</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
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
        
                <script type="text/javascript">
                    function searchProduct() {
                        $("#<%= btnSearch.ClientID%>").click();
                    }

                </script>
            </main>

            <a href="javascript:;" class="scroll-top-link" id="scroll-top"><i class="fa fa-angle-up"></i></a>
            <a href="javascript:;" class="scroll-bottom-link" id="scroll-bottom"><i class="fa fa-angle-down"></i></a>
            
            <script src="/App_Themes/Ann/js/bootstrap.min.js"></script>
            <script src="/App_Themes/Ann/js/bootstrap-table/bootstrap-table.js"></script>
            <script src="/App_Themes/NewUI/js/select2/select2.min.js"></script>
            <script src="/App_Themes/Ann/js/master.js?v=2011"></script>
            <script src="/App_Themes/Ann/js/copy-product-info.js?v=2011"></script>
            <script src="/App_Themes/Ann/js/download-product-image.js?v=2011"></script>
            <script type="text/javascript">

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
        </div>
    </form>
</body>
</html>
