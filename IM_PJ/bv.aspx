<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bv.aspx.cs" Inherits="IM_PJ.bv" EnableSessionState="ReadOnly" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Bài viết ANN</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1, user-scalable=yes">
    <meta name="format-detection" content="telephone=no">
    <meta name="robots" content="noindex, nofollow">
    <link rel="stylesheet" href="/App_Themes/Ann/css/style.css?v=19102020" media="all">
    <link rel="stylesheet" href="/App_Themes/Ann/css/style-P.css?v=19102020" media="all">
    <link href="/App_Themes/NewUI/js/select2/select2.css" rel="stylesheet" />
    <link rel="stylesheet" href="/App_Themes/Ann/css/style-sp.css?v=19102020" media="all">
    <script type="text/javascript" src="/App_Themes/Ann/js/jquery-2.1.3.min.js"></script>
</head>
<body>
    <form id="form12" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager runat="server" ID="scr">
        </asp:ScriptManager>
        <div>
            <main>
                <div class="container">
                    <div class="row">
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="/sp" class="btn btn-menu primary-btn h45-btn btn-product"><i class="fa fa-sign-in" aria-hidden="true"></i> Sản phẩm</a>
                            </div>
                        </div>
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="/bv" class="btn btn-menu primary-btn h45-btn btn-post"><i class="fa fa-sign-in" aria-hidden="true"></i> Bài viết</a>
                            </div>
                        </div>
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="/dang-ky-nhap-hang" class="btn btn-menu primary-btn h45-btn btn-order"><i class="fa fa-cart-plus" aria-hidden="true"></i> Đặt hàng</a>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="/kh" class="btn primary-btn h45-btn btn-customer"><i class="fa fa-sign-in" aria-hidden="true"></i> Khách hàng</a>
                            </div>
                        </div>
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="/bc" class="btn primary-btn h45-btn btn-report"><i class="fa fa-sign-in" aria-hidden="true"></i> Báo cáo</a>
                            </div>
                        </div>
                        <div class="col-xs-4">
                            <div class="row">
                                <a href="/nhan-vien-dat-hang" class="btn primary-btn h45-btn btn-list-order"><i class="fa fa-cart-plus" aria-hidden="true"></i> DS đặt hàng</a>
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
                                                <div class="col-md-6 col-xs-12 margin-bottom-15">
                                                    <asp:TextBox ID="txtSearchPost" runat="server" CssClass="form-control" placeholder="Tìm bài viết" autocomplete="off"></asp:TextBox>
                                                </div>
                                                <div class="col-md-3 col-xs-6 margin-bottom-15">
                                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-3 col-xs-6 margin-bottom-15">
                                                    <asp:DropDownList ID="ddlCreatedDate" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="" Text="Thời gian"></asp:ListItem>
                                                        <asp:ListItem Value="today" Text="Hôm nay"></asp:ListItem>
                                                        <asp:ListItem Value="yesterday" Text="Hôm qua"></asp:ListItem>
                                                        <asp:ListItem Value="beforeyesterday" Text="Hôm kia"></asp:ListItem>
                                                        <asp:ListItem Value="week" Text="Tuần này"></asp:ListItem>
                                                        <asp:ListItem Value="thismonth" Text="Tháng này"></asp:ListItem>
                                                        <asp:ListItem Value="7days" Text="7 ngày"></asp:ListItem>
                                                        <asp:ListItem Value="30days" Text="30 ngày"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-xs-12">
                                            <div class="row">
                                                <div class="col-xs-6">
                                                    <a href="javascript:;" onclick="searchPost()" class="btn primary-btn h45-btn"><i class="fa fa-search"></i> Tìm kiếm</a>
                                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn primary-btn h45-btn" OnClick="btnSearch_Click" Style="display: none" />
                                                </div>
                                                <div class="col-xs-6">
                                                    <a href="/bv" class="btn primary-btn h45-btn download-btn"><i class="fa fa-times" aria-hidden="true"></i> Làm lại</a>
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
                            <h3>Danh sách bài viết</h3>
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

            <a href="javascript:;" class="scroll-top-link" id="scroll-top"><i class="fa fa-angle-up"></i></a>
            <a href="javascript:;" class="scroll-bottom-link" id="scroll-bottom"><i class="fa fa-angle-down"></i></a>
            
            <script src="/App_Themes/Ann/js/bootstrap.min.js"></script>
            <script src="/App_Themes/Ann/js/bootstrap-table/bootstrap-table.js"></script>
            <script src="/App_Themes/NewUI/js/select2/select2.min.js"></script>
            <script src="/App_Themes/Ann/js/master.js?v=19102020"></script>
            <script src="/App_Themes/Ann/js/copy-post-info.js?v=19102020"></script>
            <script src="/App_Themes/Ann/js/download-post-image.js?v=19102020"></script>

            <script type="text/javascript">

                $("#<%=txtSearchPost.ClientID%>").keyup(function (e) {
                    if (e.keyCode == 13)
                    {
                        $("#<%= btnSearch.ClientID%>").click();
                    }
                });

                function searchPost() {
                    $("#<%= btnSearch.ClientID%>").click();
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
        </div>
    </form>
</body>
</html>
