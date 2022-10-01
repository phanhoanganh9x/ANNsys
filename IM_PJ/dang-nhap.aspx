<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dang-nhap.aspx.cs" Inherits="IM_PJ.dang_nhap" %>

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
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Tên đăng nhập"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="req" runat="server" ControlToValidate="txtUsername" ErrorMessage="Không để trống"
                        ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <div class="form-row">
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Mật khẩu" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword" ErrorMessage="Không để trống"
                        ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <div class="form-row">
                    <a href="javascript:;" class="btn primary-btn fw-btn" onclick="showSecurityCode()">Đăng nhập</a>
                    <asp:Button ID="btnLogin" runat="server" CssClass="btn primary-btn fw-btn" Text="Đăng nhập" OnClick="btnLogin_Click" Style="display: none"/>
                </div>
            </div>
        </div>


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

            $('#txtPassword').keydown(function (event) {
                if (event.which === 13) {
                    var user = $('#txtUsername').val();
                    var pass = $("#txtPassword").val();

                    if (isBlank(user) || isBlank(pass)) {
                        swal("Thông báo", "Hãy nhập đầy đủ thông tin đăng nhập", "error");
                    }
                    else {
                        showSecurityCode();
                    }

                    event.preventDefault();
                    return false;
                }
            });

            $('#txtUsername').keydown(function (event) {
                if (event.which === 13) {
                    var user = $('#txtUsername').val();
                    var pass = $("#txtPassword").val();

                    if (isBlank(user) || isBlank(pass)) {
                        swal("Thông báo", "Hãy nhập đầy đủ thông tin đăng nhập", "error");
                    }
                    else {
                        showSecurityCode();
                    }

                    event.preventDefault();
                    return false;
                }
            });

            function showSecurityCode() {
                var html = "";
                html += "<div class=\"form-group\">";
                html += "<label>Nhập mã bảo mật: </label>";
                html += "<input id=\"txtSecurityCode\" class=\"form-control fjx\" type=\"password\"></input>";
                html += "<a href=\"javascript: ;\" class=\"btn link-btn\" style=\"background-color:#f87703;float:right;color:#fff;\" onclick=\"checkSecurityCode()\">OK</a>";
                html += "</div>";
                showPopup(html, 3);
                $("#txtSecurityCode").focus();
                $('#txtSecurityCode').keydown(function (event) {
                    if (event.which === 13) {
                        checkSecurityCode();
                        event.preventDefault();
                        return false;
                    }
                });
            }

            function checkSecurityCode() {
                HoldOn.open();
                var code = $("#txtSecurityCode").val();
                $.ajax({
                    type: "POST",
                    url: "/dang-nhap.aspx/CheckSecurityCode",
                    data: "{code:'" + code + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d == "ok") {
                            $("#<%=btnLogin.ClientID%>").click();
                            closePopup();
                        }
                        else {
                            HoldOn.close();
                            swal("Thông báo", "Mã bảo mật không đúng!!!", "error");
                            $("#txtSecurityCode").select();
                        }
                    }
                });
            }
        </script>
    </form>
</body>
</html>
