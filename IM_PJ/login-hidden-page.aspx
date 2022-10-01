<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login-hidden-page.aspx.cs" Inherits="IM_PJ.login_hidden_page" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1, user-scalable=yes" />
    <meta name="format-detection" content="telephone=no" />
    <meta name="robots" content="noindex, nofollow" />
    <title>Đăng nhập hệ thống</title>
    <link rel="stylesheet" href="/App_Themes/Ann/css/style.css" media="all" />
    <link rel="stylesheet" href="/App_Themes/Ann/css/style-P.css?v=02102022" media="all" />
    <link href="/App_Themes/NewUI/js/sweet/sweet-alert.css" rel="stylesheet" type="text/css" />
    <link href="/App_Themes/NewUI/js/select2/select2.css" rel="stylesheet" />
    <script type="text/javascript" src="/App_Themes/Ann/js/jquery-2.1.3.min.js"></script>

    <script type="text/javascript" src="/App_Themes/Ann/js/popup.js"></script>

    <link href="/App_Themes/Ann/css/HoldOn.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="side-full-cont">
            <div class="logo">
                <a href="javascrip:;">
                    <img src="/App_Themes/Ann/image/logo.png" alt="THỜI TRANG GIÁ SỈ ANN" /></a>
            </div>
            <div class="form form-login">
                <div class="form-row">
                    <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                </div>
                <div class="form-row">
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Tài khoản"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtUsername" ErrorMessage="Không để trống"
                        ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <div class="form-row">
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Mật khẩu" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword" ErrorMessage="Không để trống"
                        ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <div class="form-row">
                    <a href="javascript:;" class="btn primary-btn fw-btn" onclick="login()">Đăng nhập</a>
                    <asp:Button ID="btnLogin" runat="server" CssClass="btn primary-btn fw-btn" Text="Đăng nhập" OnClick="btnLogin_Click" Style="display: none"/>
                </div>
            </div>
        </div>


        <a href="javascript:;" class="scroll-top-link" id="scroll-top"><i class="fa fa-angle-up"></i></a>
        <script src="/App_Themes/Ann/js/bootstrap.min.js"></script>
        <script src="/App_Themes/Ann/js/bootstrap-table/bootstrap-table.js"></script>
        <script src="/App_Themes/NewUI/js/sweet/sweet-alert.js" type="text/javascript"></script>
        <script src="/App_Themes/NewUI/js/select2/select2.min.js"></script>
        <script src="/App_Themes/Ann/js/chartjs.min.js"></script>
        <script src="/App_Themes/Ann/js/master.js"></script>
        <script src="/App_Themes/Ann/js/HoldOn.js"></script>
        <script>
            function isBlank(str) {
                return (!str || /^\s*$/.test(str));
            }

            function login() {
                var password = $("#txtPassword").val();

                if (isBlank(password)) {
                    swal("Thông báo", "Hãy nhập mật khẩu đăng nhập", "error");
                }
                else {
                    $("#<%=btnLogin.ClientID%>").click();
                }
            }

            $('#txtPassword').keydown(function (event) {
                if (event.which === 13) {

                    login();

                    event.preventDefault();
                    return false;
                }
            });
        </script>
    </form>
</body>
</html>
