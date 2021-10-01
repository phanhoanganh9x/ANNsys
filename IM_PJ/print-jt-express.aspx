<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="print-jt-express.aspx.cs" Inherits="IM_PJ.print_jt_express" %>

<!DOCTYPE html public "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>In vận đơn J&T Express</title>
    <link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Roboto&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="App_Themes/Ann/css/pages/print-jt-express/print-jt-express.css?v=202109291533" />
    <style type="text/css">
        @import url('https://fonts.googleapis.com/css2?family=Roboto&display=swap');
        body {
            font-family: 'Roboto', sans-serif;
            font-size: 18px;
        }
        .table {
            display: block;
            width: 768px; 
            height: 1126px; 
            background: url('https://simyphamonline.com/wp-content/uploads/bgjt-11-1.png'); 
        }

        .row {
            display: block;
            float: left;
        }

        .column {
            display: block;
    vertical-align: top;
    float: left;
        }
        .barcode-row {
            margin-top: 13px;
            text-align: center;
        }
        .barcode-img {
            width: 370px;
            height: 63px;
        }
        .barcode-text {
            font-weight: bold;
    font-size: 20px;
    margin-top: 5px;
        }
        .order-id-label {
                margin-top: 11px;
    margin-left: 30px;
    font-size: 20px;
    font-weight: bold;
    float: left;
        }
        .order-id-value {
                margin-top: 12px;
    margin-left: 131px;
    font-size: 18px;
    font-weight: bold;
    float: left;
        }
        .order-date {
                margin-top: 120px;
    margin-left: 56px;
    font-size: 21px;
    font-weight: bold;
        }
        .sender-address {
                 margin-top: 7px;
    margin-left: 20px;
    font-size: 17px;
    font-weight: bold;
    line-height: 1.5;
        }
        .receiver-address {
                margin-top: 7px;
    margin-left: 18px;
    font-size: 17px;
    font-weight: bold;
    line-height: 1.5;
        }
        .postal-code {
                    margin-top: 3px;
    margin-left: 36px;
    font-size: 43px;
    font-weight: bold;
    line-height: 1.5;
        }
        .customer-order {
                margin-top: 20px;
    margin-left: 64px;
    font-size: 19px;
    font-weight: bold;
    line-height: 1.5;
        }
        .item-number {
            margin-top: 5px;
    margin-left: 0;
    font-size: 19px;
    font-weight: bold;
    line-height: 1.5;
    text-align: center;
        }
        .postal-branch-code-1 {
            margin-top: 0;
    margin-left: 0;
    font-size: 29px;
    font-weight: bold;
    text-align: center;
        }
        .postal-branch-code-2 {
           margin-top: 1px;
    margin-left: 0;
    font-size: 29px;
    font-weight: bold;
    text-align: center;
        }
        .delivery-type-label {
           margin-top: 8px;
    margin-left: 0;
    font-size: 18px;
    text-align: center;
        }
        .delivery-type-value {
                margin-top: 11px;
    margin-left: 0;
    font-size: 19px;
    font-weight: bold;
    text-align: center;
        }
        .pttt-label {
            margin-top: 8px;
    margin-left: 0;
    font-size: 18px;
    text-align: center;
        }
        .pttt-value {
            margin-top: 12px;
            margin-left: 0;
            font-size: 19px;
            font-weight: bold;
            text-align: center;
        }
        .fee-shipping {
            margin-top: 5px;
    margin-left: 0;
    font-size: 18px;
    text-align: center;
        }
        .cod {
        margin-top: 2px;
    margin-left: 0;
    font-size: 19px;
    text-align: center;
        }
        .weight-label {
                margin-top: 2px;
    margin-left: -4px;
    font-size: 18px;
    font-weight: bold;
    text-align: center;
        }
        .weight-value {
            margin-top: 12px;
            margin-left: 0;
            font-size: 19px;
            font-weight: bold;
        }
        .weight-value-left {
            padding-left: 35px;
        }
        .weight-value-right {
            padding-left: 40px;
        }
        .sender-sig {
                margin-top: 6px;
    margin-left: 104px;
    font-size: 19px;
    font-weight: bold;
    line-height: 1.5;
        }
        .receiver-sig {
                margin-top: 6px;
    margin-left: 108px;
    font-size: 19px;
    font-weight: bold;
    line-height: 1.5;
        }
        .receiver-confirm {
                margin-top: 50px;
    margin-left: 42px;
    font-size: 12px;
    font-weight: bold;
    line-height: 1.5;
        }
        .note-label {
                       margin-top: 7px;
    margin-left: 345px;
    font-size: 19px;
    font-weight: bold;
        }
        .note-value {
                        margin-top: 0;
    margin-left: 7px;
    font-size: 16.34px;
        }
        .content-label {
            margin-top: 13px;
    margin-left: 230px;
    font-size: 19px;
    font-weight: bold;
        }
        .content-value {
            margin-top: 8px;
    margin-left: 9px;
    font-size: 19px;
        }
        .footer {
                margin-top: 13px;
    margin-left: 110px;
    font-size: 16.6px;
    font-weight: bold;
        }
        .red-text {
            color: #ff0000;
        }
    </style>
</head>
<body>
    
    
<div class="table">
    <div>
        <div class="column" style="width: 384px; height: 158px;">
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
        <div class="column" style="width: 384px; height: 158px;">
            <div class="order-date">
                Ngày gửi:
                <asp:Literal ID="ltSentDate" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <div>
        <div class="column" style="width: 384px; height: 136px;">
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
        <div class="column" style="width: 384px; height: 136px;">
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
    <div class="row">
        <div class="column" style="width: 156px; height: 72px;"><div class="postal-code"><asp:Literal ID="ltPostalCode" runat="server"></asp:Literal></div></div>
        <div class="column" style="width: 505px; height: 72px;"><div class="customer-order">Mã đơn KH</div></div>
        <div class="column" style="width: 106px; height: 72px;"><div class="item-number">Số kiện<br /><asp:Literal ID="ltItemNumber" runat="server"></asp:Literal></div></div>
    </div>
    <div class="row">
        <div class="column" style="width: 156px; height: 432px;">
            <div class="column" style="width: 156px; height: 73px;">
                <div class="postal-branch-code-1"><asp:Literal ID="ltPostalBranchCodeLine1" runat="server"></asp:Literal></div>
                <div class="postal-branch-code-2"><asp:Literal ID="ltPostalBranchCodeLine2" runat="server"></asp:Literal></div>
            </div>
            <div class="column" style="width: 156px; height: 73px;">
                <div class="delivery-type-label">Loại vận chuyển</div>
                <div class="delivery-type-value">ET</div>
            </div>
            <div class="column" style="width: 156px; height: 73px;">
                <div class="pttt-label">PTTT</div>
                <div class="pttt-value">PP_PM</div>
            </div>
            <div class="column" style="width: 156px; height: 73px;">
                <div class="fee-shipping">Vận phí</div>
            </div>
            <div class="column" style="width: 156px; height: 73px;">
                <div class="cod">Thu hộ COD</div>
            </div>
            <div class="column" style="width: 156px; height: 73px;">
                <div class="weight-label">Trọng lượng</div>
                <div class="weight-value">
                     <span class="weight-value-left"><asp:Literal ID="ltWeight" runat="server"></asp:Literal></span>
                    <span class="weight-value-right">KG</span>
                </div>
            </div>
        </div>
        <div class="column" style="width: 611px; height: 432px;">
            <div class="content-label">Nội dung</div>
            <div class="content-value"><asp:Literal ID="ltItemName" runat="server"></asp:Literal></div>
        </div>
    </div>
    <div class="row">
        <div class="column" style="width: 384px; height: 110px;">
            <div class="sender-sig">
                Người gởi ký tên
            </div>
        </div>
        <div class="column" style="width: 384px; height: 110px;">
            <div class="receiver-sig">
            Người nhận ký tên
            </div>
            <div class="receiver-confirm">
                Xác nhận đã nhận được bưu kiện trong tình trạng tốt
            </div>
        </div>
    </div>
    <div class="row">
        <div class="column" style="width: 768px; height: 170px;">
            <div class="note-label">
                Ghi chú
            </div>
            <div class="note-value">
                <asp:Literal ID="ltNote" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="column" style="width: 768px; height: 50px;">
            <div class="footer">
                Express Your Online Business - <span class="red-text">www.jtexpress.vn </span>- Hotline: <span class="red-text">1900 1088</span>
            </div>
        </div>
    </div>
</div>
</body>
</html>
