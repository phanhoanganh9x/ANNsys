<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="print-invoice.aspx.cs" Inherits="IM_PJ.print_invoice" EnableSessionState="ReadOnly" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8" />
    <link rel="stylesheet" href="/App_Themes/Ann/css/print-invoice.css?v=26042021" type="text/css"/>
    <link rel="stylesheet" href="/App_Themes/Ann/barcode/style.css" type="text/css"/>
    <link rel="stylesheet" href="/App_Themes/Ann/css/responsive.css" type="text/css"/>
    <script src="/App_Themes/Ann/js/jquery-2.1.3.min.js"></script>
    <title>Hóa đơn mua hàng</title>
</head>
<body>
    <asp:Literal ID="ltrPrintInvoice" runat="server"></asp:Literal>
    <asp:Literal ID="ltrPrintEnable"  runat="server"></asp:Literal>
    <script type="text/javascript">
        window.onload = setTimeout(function () {
            if ($(".print-enable").hasClass("true")) {
                window.print();
                setTimeout(function () { window.close(); }, 1);
            }
        }, 1500);
    </script> 
</body>
</html>