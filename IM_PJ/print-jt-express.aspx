<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="print-jt-express.aspx.cs" Inherits="IM_PJ.print_jt_express" %>

<!DOCTYPE html public "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>In vận đơn J&T Express</title>
    <link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Roboto&display=swap" rel="stylesheet">
    <style type="text/css">
        @import url('https://fonts.googleapis.com/css2?family=Roboto&display=swap');
        body {
            font-family: 'Roboto', sans-serif;
            font-size: 9px;
        }
        .table {
            display: block;
            width: 384px; 
            height: 562px; 
            background: url('https://simyphamonline.com/wp-content/uploads/bgjt-ann-384px-4.png'); 
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
    </style>
</head>
<body>
    
    
<div class="table">
    <div class="row" style="height: 14%;">
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
    <div class="row" style="height: 12%;">
        <div class="column" style="width: 50%;">
            <div class="sender-address">
                Người gởi :
                                    <asp:Literal ID="ltSenderName" runat="server"></asp:Literal><br />
                <asp:Literal ID="ltSenderPhone" runat="server"></asp:Literal><br />
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
            <asp:Literal ID="ltReceiverPhone" runat="server"></asp:Literal><br />
            Địa chỉ :
                                    <asp:Literal ID="ltReceiverAddressLine1" runat="server"></asp:Literal>
            <asp:Literal ID="ltReceiverAddressLine2" runat="server"></asp:Literal>
            <asp:Literal ID="ltReceiverAddressLine3" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <div class="row" style="height: 6.4%;">
        <div class="column" style="width: 20%;"><div class="postal-code"><asp:Literal ID="ltPostalCode" runat="server"></asp:Literal></div></div>
        <div class="column" style="width: 66%;"><div class="customer-order">Mã đơn KH</div></div>
        <div class="column" style="width: 14%;"><div class="item-number">Số kiện<br /><asp:Literal ID="ltItemNumber" runat="server"></asp:Literal></div></div>
    </div>
    <div class="row" style="height: 38.6%;">
        <div class="column" style="width: 20.2%; height: 100%">
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
            </div>
            <div class="column" style="width: 100%; height: 16.8%;">
                <div class="weight-label">Trọng lượng</div>
                <div class="weight-value">
                     <span class="weight-value-left"><asp:Literal ID="ltWeight" runat="server"></asp:Literal></span>
                    <span class="weight-value-right">KG</span>
                </div>
            </div>
        </div>
        <div class="column" style="width: 79.8%; height:100%">
            <div class="content-label">Nội dung</div>
            <div class="content-value"><asp:Literal ID="ltItemName" runat="server"></asp:Literal></div>
        </div>
    </div>
    <div class="row" style="height: 9.7%;">
        <div class="column" style="width: 50%;">
            <div class="sender-sig">
                Người gởi ký tên
            </div>
        </div>
        <div class="column" style="width: 50%">
            <div class="receiver-sig">
            Người nhận ký tên
            </div>
            <div class="receiver-confirm">
                Xác nhận đã nhận được bưu kiện trong tình trạng tốt
            </div>
        </div>
    </div>
    <div class="row" style="height: 6.7%;">
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
            <div class="footer">
                Express Your Online Business - <span class="red-text">www.jtexpress.vn </span>- Hotline: <span class="red-text">1900 1088</span>
            </div>
        </div>
    </div>
</div>
    <script type="text/javascript">
        //window.onload = setTimeout(function () {
        //    window.print();
        //    setTimeout(function () { window.close(); }, 1);
        //}, 1000);
    </script>
</body>
</html>
