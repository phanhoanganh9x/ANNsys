﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="print-order-image.aspx.cs" Inherits="IM_PJ.print_order_image" EnableSessionState="ReadOnly" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8" />
    <link rel="stylesheet" href="/App_Themes/Ann/css/print-order-image.css?v=24082019" type="text/css"/>
    <link rel="stylesheet" href="/App_Themes/Ann/barcode/style.css" type="text/css"/>
    <link rel="stylesheet" href="/App_Themes/Ann/css/responsive.css" type="text/css"/>
    <script type="text/javascript" src="/App_Themes/Ann/js/jquery-2.1.3.min.js"></script>
    <script type="text/javascript" src="/App_Themes/Ann/js/html2canvas.js"></script>
    <script type="text/javascript" src="/App_Themes/Ann/js/copy-invoice-url.js?v=26042020"></script>
    <title>Lấy ảnh đơn hàng</title>    
    <style>
        #old-order-note {
            background: #707070;
            color: #fff;
            padding: 15px;
        }
        #old-order-note ul {
            padding-left: 15px;
        }
        .btn {
            display: inline-block;
            margin-bottom: 0;
            font-weight: normal;
            text-align: center;
            vertical-align: middle;
            cursor: pointer;
            background-image: none;
            border: 1px solid transparent;
            white-space: nowrap;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.42857;
            border-radius: 0px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }
        .btn {
            appearance: none;
            -webkit-appearance: none;
            -moz-appearance: none;
            -ms-appearance: none;
            -o-appearance: none;
            border: none;
            color: #fff;
            height: 40px;
            line-height: 20px;
            background-color: transparent;
            -webkit-transition: all 0.3s ease-in-out;
            -moz-transition: all 0.3s ease-in-out;
            -o-transition: all 0.3s ease-in-out;
            -ms-transition: all 0.3s ease-in-out;
            transition: all 0.3s ease-in-out;
            padding: 10px 15px;
            border-radius: 2px;
            text-align: center;
        }
        .btn-violet {
            background-color: #8000d0!important;
        }
        .copy-invoice-url {
            margin-bottom: 15px;
        }
    </style>
</head>
<body>
    <asp:Literal ID="ltrOldOrderNote" runat="server"></asp:Literal>
    <h2 class="guide">Click chuột phải vào ảnh -> Chọn Sao chép hình ảnh -> Dán vào Zalo hoặc Facebook (làm từng ảnh theo thứ tự trong đơn hàng)</h2>
    <div class="copy-invoice-url"><asp:Literal ID="ltrCopyInvoiceURL" runat="server"></asp:Literal></div>
    <div id="previewImage"></div>
    <asp:Literal ID="ltrPrintInvoice" runat="server"></asp:Literal>
    <asp:Literal ID="ltrPrintEnable"  runat="server"></asp:Literal>
    <script>
        $(document).ready(function () {
            var print = $(".print");
            for (var i = 0; i < print.length; i++) {
                html2canvas(document.querySelector(".print-" + i), {
                    allowTaint: true,
                    logging: false
                }).then(canvas => {
                    $("#previewImage").append(canvas);
                    $(".print-order-image").hide();
                    $(".guide").addClass("p-guide");
                });
            }
        });
    </script>
</body>
</html>