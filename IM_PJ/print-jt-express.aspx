<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="print-jt-express.aspx.cs" Inherits="IM_PJ.print_jt_express" %>

<!DOCTYPE html public "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Roboto&display=swap" rel="stylesheet">
    <link type="text/css" rel="stylesheet" href="App_Themes/Ann/css/pages/print-jt-express/print-jt-express.css?v=202110191556" />

    <title>In vận đơn J&T Express</title>
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

    <script src="/App_Themes/Ann/js/jquery-2.1.3.min.js"></script>
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
