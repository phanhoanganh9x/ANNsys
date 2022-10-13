<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="print-shipping-note.aspx.cs" Inherits="IM_PJ.print_shipping_note" %>
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <title>Phiếu gửi hàng</title>
    <script src="/App_Themes/Ann/js/jquery-2.1.3.min.js"></script>
    <link href="/App_Themes/NewUI/js/sweet/sweet-alert.css" rel="stylesheet" type="text/css" />
  <style>
    
    body {
        font-size: 18px;
        font-family: Tahoma,sans-serif;
        margin-left: 0;
        margin-top: 0;
    }
    p {
        line-height: 1.5;
        margin-top: 4px;
        margin-bottom: 4px;
    }
    .logo-ghtk {
        width: 200px;
        float: left;
    }
    .table {
        display: block;
        width: 175mm;
    }
    .table table, th, td {
        padding: 15px;
        border: 1px solid;
        border-collapse: collapse;
    }
    .table-ghtk {
        width: 175mm;
    }
    .table-ghtk .top-right {
        right: 32mm;
    }
    .table-ghtk .bottom-right {
        right: 32mm;
    }
    .table-ghtk .destination {
        text-align: center;
        font-size: 42px;
    }
    .table-ghtk .delivery {
        text-align: center;
        font-size: 25px;
        letter-spacing: 2px;
    }
    .table-note {
        display: block;
        width: 200mm;
        height: 70mm;
        position: relative;
    }
    .table-note h2 {
        font-size: 3em;
        margin-bottom: 10px;
        margin-top: 5px;
    }
    .table-fragile-goods {
        display: block;
        width: 150mm;
        height: 70mm;
        position: relative;
        text-align: center;
    }
    .table-fragile-goods h2 {
        font-size: 4.7em;
        margin-bottom: 0;
        margin-top: 5px;
        line-height: 1.3em;
    }
    .td-logo {
        float: left;
        width: 35%;
    }
    .td-delivery-info {
        float: left;
        width: 65%;
        text-align: center;
    }
    .top-left {
        position: absolute;
        top: 0;
        left: 2mm;
        width: 65mm;
    }
    .top-right {
        position: absolute;
        top: 1mm;
        right: 12mm;
        width: 90mm;
        text-align: right;
    }
    .top-right img {
        max-width: 88%;
    }
    .bottom-left {
        position: absolute;
        bottom: 1mm;
        left: 2mm;
        width: 55mm;
    }
    .bottom-left img {
        width: 40mm;
        height: 10mm;
    }
    .bottom-right {
        position: absolute;
        bottom: 1mm;
        right: 12mm;
        width: 90mm;
    }
    .cod {
        font-size: 24px;
        font-weight: bold;
    }
    .address {
        text-transform: capitalize;
    }
    .web {
        text-decoration: underline;
    }
    .delivery {
        margin-top: 0;
        text-transform: uppercase;
        font-size: 23px;
    }
    .shipto {
        text-transform: uppercase;
        font-size: 20px;
    }
    .sender {
        margin-top: 10px;
        margin-bottom: 15px;
        text-decoration: underline;
        font-weight: bold;
    }
    .sender-name {
        font-size: 26px;
        text-transform: uppercase;
        font-weight: bold;
    }
    .receiver {
        margin-top: 10px;
        margin-bottom: 15px;
        text-decoration: underline;
        font-weight: bold;
    }
    .receiver-name {
        font-size: 26px;
        text-transform: uppercase;
        font-weight: bold;
    }
    .phone {
        font-size: 22px;
        font-weight: bold;
    }
    .img {
        width: 170px;
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
    .footer {
        font-size: 16px;
    }
    /*.rotated {
        transform: rotate(-90deg);
        width: 72mm;
        position: absolute;
        text-align: center;
        top: 31mm;
        left: 114mm;
        font-size: 30px;
        font-weight: bold;
        border-top: dashed 2px #000;
        padding-top: 0;
        letter-spacing: 6px;
    }*/
    .margin-left-ghtk {
        left: 126mm;
    }
    .ghtk {
        top: 30.5mm;
        left: 115mm;
        padding-top: 0;
        text-align: center;
        font-size: 33px;
    }
    .btn-blue {
        background-color: #2ea2cc!important;
    }
    .btn-green {
        background-color: #73a724!important;
    }
    .btn-violet {
        background-color: #8000d0!important
    }
    .btn:hover, .btn:active {
        background-color: #585858!important;
    }
    #previewNoteImage{
        margin-top: 30px;
    }
    .barcode-image {
        width: 65%;
        height: 50%;
    }
    
    @media print { 
        /*body {
            -ms-transform:rotate(-90deg);
            -o-transform:rotate(-90deg);
            transform:rotate(-90deg);
            margin-top: 110mm;
            margin-left: 4mm;
        }*/
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
    <br />
    <div id="previewNoteImage"></div>
    <div id="previewFragileGoodsImage"></div>

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
            $(".table-note").hide();
            $(".table-fragile-goods").hide();
        }

        function replacePhone() {
            var phone = $(".replace-phone").text();
            var replacePhone = phone.replace(/[^\d]/g, "");
            replacePhone = replacePhone.substr(0, 3) + "***" + replacePhone.substr(6, replacePhone.length - 6);
            $(".replace-phone").text(replacePhone);
        }

        function printIt() {
            $("#previewImage").hide();
            $("#previewNoteImage").hide();
            $("#previewFragileGoodsImage").hide();
            $(".table").show();
            $(".table-note").hide();
            $(".table-fragile-goods").hide();
            $(".print-it").hide();
            $(".h2guide").hide();
            $(".pguide").hide();
            $(".btn-violet").hide();
            $(".show-transport-info").hide();
            $(".sweet-alert").hide().empty();
            $(".sweet-overlay").hide().empty();
            replacePhone();
            window.print();
            window.close();
        }

        function printNote() {
            $("#previewImage").hide();
            $("#previewNoteImage").hide();
            $("#previewFragileGoodsImage").hide();
            $(".table").hide();
            $(".table-note").show();
            $(".table-fragile-goods").hide();
            $(".print-it").hide();
            $(".h2guide").hide();
            $(".pguide").hide();
            $(".btn-violet").hide();
            $(".show-transport-info").hide();
            $(".sweet-alert").hide().empty();
            $(".sweet-overlay").hide().empty();
            window.print();
            location.reload();
        }

        function printFragileGoods() {
            $("#previewImage").hide();
            $("#previewNoteImage").hide();
            $("#previewFragileGoodsImage").hide();
            $(".table").hide();
            $(".table-note").hide();
            $(".table-fragile-goods").show();
            $(".print-it").hide();
            $(".h2guide").hide();
            $(".pguide").hide();
            $(".btn-violet").hide();
            $(".show-transport-info").hide();
            $(".sweet-alert").hide().empty();
            $(".sweet-overlay").hide().empty();
            window.print();
            location.reload();
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
