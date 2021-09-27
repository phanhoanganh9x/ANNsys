<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kh.aspx.cs" Inherits="IM_PJ.kh" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Khách hàng</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1, user-scalable=yes">
    <meta name="format-detection" content="telephone=no">
    <meta name="robots" content="noindex, nofollow">
    <link rel="stylesheet" href="/App_Themes/Ann/css/style.css?v=04092021" media="all">
    <link rel="stylesheet" href="/App_Themes/Ann/css/style-P.css?v=202109271901" media="all">
    <link rel="stylesheet" href="/App_Themes/Ann/css/style-sp.css?v=04092021" media="all">
    <link href="/App_Themes/NewUI/js/sweet/sweet-alert.css" rel="stylesheet" />
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
                        <div class="col-md-6 col-md-offset-3 col-xs-12">
                            <div class="filter-above-wrap clear">
                                <div class="filter-control">
                                    <div class="row">
                                        <div class="col-md-6 col-xs-12">
                                            <div class="row">
                                                <div class="col-md-12 margin-bottom-15">
                                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Tìm khách hàng" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6 col-xs-12">
                                            <div class="row">
                                                <div class="col-xs-6">
                                                    <a href="javascript:;" onclick="search()" class="btn primary-btn h45-btn"><i class="fa fa-search"></i> Tìm kiếm</a>
                                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn primary-btn h45-btn" OnClick="btnSearch_Click" Style="display: none" />
                                                </div>
                                                <div class="col-xs-6">
                                                    <a href="/kh" class="btn primary-btn h45-btn download-btn"><i class="fa fa-times" aria-hidden="true"></i> Làm lại</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 col-md-offset-3 col-xs-12">
                            <h3><asp:Literal ID="ltrHeading" runat="server" EnableViewState="false"></asp:Literal></h3>
                            <p><asp:Literal ID="ltrAccount" runat="server" EnableViewState="false"></asp:Literal></p>
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
            <script src="/App_Themes/Ann/js/master.js?v=04092021"></script>
            <script src="/App_Themes/NewUI/js/sweet/sweet-alert.js?v=04092021" type="text/javascript"></script>

            <script type="text/javascript">

                $("#<%=txtSearch.ClientID%>").keyup(function (e) {
                    if (e.keyCode == 13) {
                        $("#<%= btnSearch.ClientID%>").click();
                    }
                });

                function search() {
                    $("#<%= btnSearch.ClientID%>").click();
                }

                // Format số lượng
                var formatThousands = function (n, dp) {
                    var s = '' + (Math.floor(n)), d = n % 1, i = s.length, r = '';
                    while ((i -= 3) > 0) { r = ',' + s.substr(i, 3) + r; }
                    return s.substr(0, i + 3) + r +
                        (d ? '.' + Math.round(d * Math.pow(10, dp || 2)) : '');
                };
            </script>
        </div>
    </form>
</body>
</html>
