<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="print-jt-express.aspx.cs" Inherits="IM_PJ.print_jt_express" %>

<!DOCTYPE html public "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>In vận đơn J&T Express</title>
    <link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Roboto&display=swap" rel="stylesheet">
    <script src="/App_Themes/Ann/js/jquery-2.1.3.min.js"></script>
    <style type="text/css">
        @import url('https://fonts.googleapis.com/css2?family=Roboto&display=swap');
        body {
            font-family: 'Roboto', sans-serif;
            font-size: 9px;
        }
        .table {
            display: block;
            float: left;
            width: 384px; 
            height: 543px; 
            background: url('https://simyphamonline.com/wp-content/uploads/bgjt-ann-384px-5.png'); 
        }

        .row {
            width: 100%;
            display: block;
            float: left;
        }

        .column {
            display: block;
            vertical-align: top;
            float: left;
        }
        .barcode-row {
            margin-top: 6.5px;
            text-align: center;
        }
        .barcode-img {
            width: 180px;
            height: 32.5px;
        }
        .barcode-text {
            font-weight: bold;
    font-size: 10px;
    margin-top: 2.5px;
        }
        .order-id-label {
                margin-top: 5.5px;
    margin-left: 15px;
    font-size: 10px;
    font-weight: bold;
    float: left;
        }
        .order-id-value {
                margin-top: 6px;
    margin-left: 65.5px;
    font-size: 9px;
    font-weight: bold;
    float: left;
        }
        .order-date {
                margin-top: 60px;
    margin-left: 28px;
    font-size: 10.5px;
    font-weight: bold;
        }
        .sender-address {
                 margin-top: 3.5px;
    margin-left: 10px;
    font-size: 8.5px;
    font-weight: bold;
    line-height: 1.6;
        }
        .receiver-address {
                margin-top: 3.5px;
    margin-left: 8px;
    font-size: 8.5px;
    font-weight: bold;
    line-height: 1.6;
        }
        .postal-code {
                    margin-top: 1.5px;
    margin-left: 18px;
    font-size: 21.5px;
    font-weight: bold;
    line-height: 1.5;
        }
        .customer-order {
                margin-top: 10px;
    margin-left: 37px;
    font-size: 9.5px;
    font-weight: bold;
    line-height: 1.5;
        }
        .item-number {
            margin-top: 2.5px;
    margin-left: 0;
    font-size: 9.5px;
    font-weight: bold;
    line-height: 1.5;
    text-align: center;
        }
        .postal-branch-code-1 {
            margin-top: 0;
    margin-left: 0;
    font-size: 14.5px;
    font-weight: bold;
    text-align: center;
        }
        .postal-branch-code-2 {
           margin-top: 0.5px;
    margin-left: 0;
    font-size: 14.5px;
    font-weight: bold;
    text-align: center;
        }
        .delivery-type-label {
           margin-top: 4px;
    margin-left: 0;
    font-size: 9px;
    text-align: center;
        }
        .delivery-type-value {
                margin-top: 5.5px;
    margin-left: 0;
    font-size: 9.5px;
    font-weight: bold;
    text-align: center;
        }
        .pttt-label {
            margin-top: 4px;
    margin-left: 0;
    font-size: 9px;
    text-align: center;
        }
        .pttt-value {
            margin-top: 6px;
            margin-left: 0;
            font-size: 9.5px;
            font-weight: bold;
            text-align: center;
        }
        .cod-value {
            margin-top: 6px;
            margin-left: 0;
            font-size: 9.5px;
            font-weight: bold;
            text-align: center;
        }
        .fee-shipping {
            margin-top: 2.5px;
    margin-left: 0;
    font-size: 9px;
    text-align: center;
        }
        .cod {
        margin-top: 1px;
    margin-left: 0;
    font-size: 9.5px;
    text-align: center;
        }
        .weight-label {
                margin-top: 1px;
    margin-left: -2px;
    font-size: 9px;
    font-weight: bold;
    text-align: center;
        }
        .weight-value {
            margin-top: 6px;
            margin-left: 0;
            font-size: 9.5px;
            font-weight: bold;
        }
        .weight-value-left {
            padding-left: 17.5px;
        }
        .weight-value-right {
            padding-left: 20px;
        }
        .sender-sig {
                margin-top: 3px;
    margin-left: 52px;
    font-size: 9.5px;
    font-weight: bold;
    line-height: 1.5;
        }
        .receiver-sig {
                margin-top: 3px;
    margin-left: 54px;
    font-size: 9.5px;
    font-weight: bold;
    line-height: 1.5;
        }
        .receiver-confirm {
                margin-top: 25px;
    margin-left: 21px;
    font-size: 6px;
    font-weight: bold;
    line-height: 1.5;
        }
        .note-label {
                       margin-top: 3.5px;
    margin-left: 172.5px;
    font-size: 9.5px;
    font-weight: bold;
        }
        .note-value {
                        margin-top: 0;
    margin-left: 3.5px;
    font-size: 8px;
        }
        .content-label {
            margin-top: 6.5px;
    margin-left: 115px;
    font-size: 9.5px;
    font-weight: bold;
        }
        .content-value {
            margin-top: 4px;
    margin-left: 4.5px;
    font-size: 9.5px;
        }
        .footer {
                margin-top: 6.5px;
    margin-left: 55px;
    font-size: 8.3px;
    font-weight: bold;
        }
        .red-text {
            color: #ff0000;
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
        font-size: 14px;
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
    </style>
</head>
<body>
    <h2 class="h2guide" style="display:none">Gửi phiếu này cho khách xem để xác nhận thông tin!</h2>
    <p class="pguide" style="display:none">Click chuột phải vào ảnh -> Chọn Sao chép hình ảnh -> Dán vào Zalo hoặc Facebook</p>
    
    <div id="previewImage"></div>

    <div class="table">
        <div class="row" style="height: 14.5%;">
            <div class="column" style="width: 50%;">
                <div class="barcode-row">
                    <asp:Literal ID="ltBarcode" runat="server"></asp:Literal>
                    <div class="barcode-text"><asp:Literal ID="ltJtCode" runat="server"></asp:Literal></div>
                </div>
                <div class="order-id-label">
                    Mã đơn đặt
                </div>
                <div class="order-id-value">
                    <asp:Literal ID="ltOrderIdHeader" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="column" style="width: 50%;">
                <div class="order-date">
                    Ngày gửi:
                    <asp:Literal ID="ltSentDate" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="row" style="height: 12.5%;">
            <div class="column" style="width: 50%;">
                <div class="sender-address">
                    Người gởi :
                    <asp:Literal ID="ltSenderName" runat="server"></asp:Literal><br />
                    <span class="replace-phone"><asp:Literal ID="ltSenderPhone" runat="server"></asp:Literal></span><br />
                    Địa chỉ :
                    <asp:Literal ID="ltSenderAddressLine1" runat="server"></asp:Literal>
                    <asp:Literal ID="ltSenderAddressLine2" runat="server"></asp:Literal>
                    <asp:Literal ID="ltSenderAddressLine3" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="column" style="width: 50%;">
                <div class="receiver-address">
                    Người nhận :
                    <asp:Literal ID="ltReceiverName" runat="server"></asp:Literal><br />
                    <span class="replace-phone"><asp:Literal ID="ltReceiverPhone" runat="server"></asp:Literal></span><br />
                    Địa chỉ :
                    <asp:Literal ID="ltReceiverAddressLine1" runat="server"></asp:Literal>
                    <asp:Literal ID="ltReceiverAddressLine2" runat="server"></asp:Literal>
                    <asp:Literal ID="ltReceiverAddressLine3" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="row" style="height: 6.9%;">
            <div class="column" style="width: 20%;">
                <div class="postal-code"><asp:Literal ID="ltPostalCode" runat="server"></asp:Literal></div>
            </div>
            <div class="column" style="width: 66%;"><div class="customer-order">Mã đơn KH</div></div>
            <div class="column" style="width: 14%;">
                <div class="item-number">
                    Số kiện<br />
                    <asp:Literal ID="ltItemNumber" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="row" style="height: 39.5%;">
            <div class="column" style="width: 20.2%; height: 100%;">
                <div class="column" style="width: 100%; height: 16.8%;">
                    <div class="postal-branch-code-1"><asp:Literal ID="ltPostalBranchCodeLine1" runat="server"></asp:Literal></div>
                    <div class="postal-branch-code-2"><asp:Literal ID="ltPostalBranchCodeLine2" runat="server"></asp:Literal></div>
                </div>
                <div class="column" style="width: 100%; height: 16.8%;">
                    <div class="delivery-type-label">Loại vận chuyển</div>
                    <div class="delivery-type-value">ET</div>
                </div>
                <div class="column" style="width: 100%; height: 16.8%;">
                    <div class="pttt-label">PTTT</div>
                    <div class="pttt-value">PP_PM</div>
                </div>
                <div class="column" style="width: 100%; height: 16.8%;">
                    <div class="fee-shipping">Vận phí</div>
                </div>
                <div class="column" style="width: 100%; height: 16%;">
                    <div class="cod">Thu hộ COD</div>
                    <div class="cod-value"><asp:Literal ID="ltCODvalue" runat="server"></asp:Literal></div>
                </div>
                <div class="column" style="width: 100%; height: 16.8%;">
                    <div class="weight-label">Trọng lượng</div>
                    <div class="weight-value">
                        <span class="weight-value-left"><asp:Literal ID="ltWeight" runat="server"></asp:Literal></span>
                        <span class="weight-value-right">KG</span>
                    </div>
                </div>
            </div>
            <div class="column" style="width: 79.8%; height: 100%;">
                <div class="content-label">Nội dung</div>
                <div class="content-value"><asp:Literal ID="ltItemName" runat="server"></asp:Literal></div>
            </div>
        </div>
        <div class="row" style="height: 10.2%;">
            <div class="column" style="width: 50%;">
                <div class="sender-sig">
                    Người gởi ký tên
                </div>
            </div>
            <div class="column" style="width: 50%;">
                <div class="receiver-sig">
                    Người nhận ký tên
                </div>
                <div class="receiver-confirm">
                    Xác nhận đã nhận được bưu kiện trong tình trạng tốt
                </div>
            </div>
        </div>
        <div class="row" style="height: 6.9%;">
            <div class="column" style="width: 100%;">
                <div class="note-label">
                    Ghi chú
                </div>
                <div class="note-value">
                    <asp:Literal ID="ltNote" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="row" style="height: 4.3%;">
            <div class="column" style="width: 100%;">
                <div class="footer">Express Your Online Business - <span class="red-text">www.jtexpress.vn </span>- Hotline: <span class="red-text">1900 1088</span></div>
            </div>
        </div>
    </div>
    
    <a class="btn print-it" href="javascript:;" onclick="printIt()">In phiếu gửi hàng</a>

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
            $(".replace-phone").each(function () {
                var phone = $(this).text();
                var replacePhone = phone.replace(/[^\d]/g, "");
                replacePhone = "******" + replacePhone.substr(6, replacePhone.length - 6);
                $(this).text(replacePhone);
            });
        }

        function printIt() {
            $("#previewImage").hide();
            $(".table").show();
            $(".print-it").hide();
            $(".h2guide").hide();
            $(".pguide").hide();
            $(".cod-value").hide();
            replacePhone();
            window.print();
            window.close();
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
    </script> 
</body>
</html>
