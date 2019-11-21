﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="print-shipping-note.aspx.cs" Inherits="IM_PJ.print_shipping_note" %>
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <title>Phiếu gửi hàng</title>
    <script src="/App_Themes/Ann/js/jquery-2.1.3.min.js"></script>
    <link href="/App_Themes/NewUI/js/sweet/sweet-alert.css" rel="stylesheet" type="text/css" />
  <style>
    
    body {
        font-size: 17px;
        font-family: Tahoma,sans-serif;
        margin-left: 0;
        margin-top: 0;
    }
    p {
        line-height: 1.5;
        margin-top: 5px;
        margin-bottom: 5px;
    }
    .table {
        display: block;
        width: 220mm;
        height: 79mm;
        position: relative;
        border-left: dashed 2px #000;
    }
    .top-left {
        position: absolute;
        top: 1mm;
        left: 3mm;
        width: 90mm;
    }
    .top-right {
        position: absolute;
        top: 2.5mm;
        right: 16mm;
        width: 120mm;
        text-align: right;
    }
    .bottom-left {
        position: absolute;
        bottom: 0;
        left: 3mm;
        width: 70mm;
    }
    .bottom-right {
        position: absolute;
        bottom: 0;
        right: 16mm;
        width: 130mm;
    }
    .cod {
        font-size: 19px;
        font-weight: bold;
    }
    .address {
        text-transform: capitalize;
    }
    .agent-address {
        font-size: 16px;
    }
    .web {
        text-decoration: underline;
    }
    .delivery {
        margin-top: 0;
        text-transform: uppercase;
    }
    .name {
        font-size: 24px;
        text-transform: uppercase;
        font-weight: bold;
    }
    .phone {
        font-size: 22px;
        font-weight: bold;
    }
    .img {
        margin-top: 5px;
        margin-bottom: 5px;
        width: 27%;
    }
    .btn {
        display: inline-block;
        appearance: none;
        -webkit-appearance: none;
        -moz-appearance: none;
        -ms-appearance: none;
        -o-appearance: none;
        border: none;
        color: #fff;
        line-height: 20px;
        background-color: #f87703;
        -webkit-transition: all 0.3s ease-in-out;
        -moz-transition: all 0.3s ease-in-out;
        -o-transition: all 0.3s ease-in-out;
        -ms-transition: all 0.3s ease-in-out;
        transition: all 0.3s ease-in-out;
        padding: 10px 15px;
        border-radius: 2px;
        text-align: center;
        text-decoration: none;
        margin-right: 30px;
        float: left;
    }
    .btn-black {
        background-color: #000;
    }
    .transport-info {
        display: none;
        font-size: 15px;
    }
    .capitalize {
        text-transform: capitalize;
    }
    .h2-guide {
        text-align: center!important;
        font-size: 18px!important;
        background: #00BCD4!important;
        color: #fff!important;
        padding: 6px!important;
        margin-top: 0;
        margin-bottom: 0;
    }
    .p-guide {
        text-align: center!important;
        font-size: 18px!important;
        margin-bottom: 15px!important;
        background: #000!important;
        color: #fff!important;
        padding: 3px!important;
        margin-top: 0;
    }
    .rotated {
        transform: rotate(-90deg);
        width: 79mm;
        position: absolute;
        text-align: left;
        top: 33.7mm;
        left: 173mm;
        font-size: 26.8px;
        font-weight: bold;
        border-top: dashed 2px #000;
        padding-top: 2mm;
        padding-left: 2.5mm;
        letter-spacing: 2px;
    }
    .btn-violet {
        background-color: #8000d0!important
    }
    .btn-violet:hover, .btn-violet:active {
        background-color: #585858!important;
    }
    @media print { 
        body {
            -ms-transform:rotate(-90deg);
            -o-transform:rotate(-90deg);
            transform:rotate(-90deg);
            margin-top: 142mm;
            margin-left: 0;
        }
    }
  </style>
    <asp:Literal ID="ltrDisablePrint"  runat="server"></asp:Literal>
</head>

<body class="receipt">
    <h2 class="h2guide" style="display:none">Gửi phiếu này cho khách xem để xác nhận thông tin!</h2>
    <p class="pguide" style="display:none">Click chuột phải vào ảnh -> Chọn Sao chép hình ảnh -> Dán vào Zalo hoặc Facebook</p>
    <div id="previewImage"></div>
    <asp:Literal ID="ltrShippingNote" runat="server"></asp:Literal>
    <asp:Literal ID="ltrPrintButton"  runat="server"></asp:Literal>
    <a href="javascript:;" onclick="copyNote()" title="Copy link hóa đơn" class="btn btn-violet h45-btn">Copy câu kiểm tra</a>
    <script src="/App_Themes/NewUI/js/sweet/sweet-alert.js" type="text/javascript"></script>
    <script src="/App_Themes/Ann/js/html2canvas.js"></script>
    <script type="text/javascript">

        $(document).ready(printImage());

        function printImage () {
            html2canvas(document.querySelector(".table"), {
                allowTaint: true,
                logging: false
            }).then(canvas => {
                $("#previewImage").append(canvas);
                $(".table").hide();
                $(".h2guide").addClass("h2-guide").show();
                $(".pguide").addClass("p-guide").show();
            });
        }

        function printIt() {
            swal({
                title: "Coi lại lần cuối nè",
                text: "Phiếu gửi hàng đúng thông tin hết chưa và có gửi cho khách xem chưa?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Đúng rồi sếp! In sếp ơi..",
                cancelButtonText: "Để em coi lại lần nữa..",
                closeOnConfirm: true,
                html: false
            }, function () {
                removeDiv();
            });
        }

        function printError(shippingType) {
            swal({
                title: "Không in được",
                text: "Đơn này gửi <strong>" + shippingType + "</strong> nhưng <strong>Không thu hộ</strong>.<br><br>Nếu khách đã chuyển khoản thì nhờ chị Ngọc in nhé!",
                type: "warning",
                confirmButtonColor: "#000000",
                confirmButtonText: "OK sếp ơi..",
                closeOnConfirm: true,
                html: true
            });
        }
        function removeDiv() {
            $("#previewImage").hide();
            $(".table").show();
            $(".print-it").hide();
            $(".h2guide").hide();
            $(".pguide").hide();
            $(".show-transport-info").hide();
            $(".sweet-alert").hide().empty();
            $(".sweet-overlay").hide().empty();
            window.print();
            window.close();
        }

        function showTransportInfo() {
            $("#previewImage").html("");
            $(".table").show();
            if ($(".transport-info").is(":hidden")) {
                $(".transport-info").show();
                $(".show-transport-info").html("Ẩn thông tin nhà xe");
            }
            else {
                $(".transport-info").hide();
                $(".show-transport-info").html("Hiện thông tin nhà xe");
            }
            printImage();
        }

        window.Clipboard = (function (window, document, navigator) {
            var textArea,
                copy;

            function isOS() {
                return navigator.userAgent.match(/ipad|iphone/i);
            }

            function createTextArea(text) {
                textArea = document.createElement('textArea');
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

        function copyNote(orderID, customerID) {
            var copyText = "kiểm tra thông tin trên phiếu gửi hàng giúp em nha!";
            Clipboard.copy(copyText);
        }
    </script> 
</body>
</html>
