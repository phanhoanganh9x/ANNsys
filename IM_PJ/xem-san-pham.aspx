<%@ Page Language="C#" Title="Xem sản phẩm" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeBehind="xem-san-pham.aspx.cs" Inherits="IM_PJ.xem_san_pham" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="Content/bootstrap-tagsinput.css" />

    <style>
        .btn.download-btn {
            background-color: #000;
            color: #fff;
            border-radius: 0;
            font-size: 16px;
            text-transform: uppercase;
            width: 100%;
        }
        .btn.down-btn {
            background-color: #E91E63;
            color: #fff;
        }

        .bootstrap-tagsinput {
            width: 100%;
        }
        
        .bootstrap-tagsinput .label {
            font-size: 100%;
        }

        @media (max-width: 769px) {
            ul.image-gallery li {
                width: 100%;
            }
            .btn {
                width: 100%!important;
                float: left;
                margin-bottom: 10px;
                margin-left: 0;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panelborderheading">
                        <div class="panel-heading clear">
                            <h3 class="page-title left not-margin-bot">Thông tin sản phẩm</h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-row">
                                <asp:Literal ID="ltrEdit1" runat="server"></asp:Literal>
                            </div>
                            <div class="form-row">
                                <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Tên sản phẩm
                                </div>
                                <div class="row-right">
                                    <asp:Label ID="lbProductTitle" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Danh mục
                                </div>
                                <div class="row-right parent">
                                    <asp:DropDownList ID="ddlCategory" Enabled="false" runat="server" CssClass="form-control slparent" date-name="parentID" data-level="1" onchange="chooseParent($(this))">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Đồng bộ lên KiotViet
                                </div>
                                <div class="row-right">
                                    <asp:RadioButtonList ID="rdbSyncKiotViet" Enabled="false" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="true" Selected="True">Có</asp:ListItem>
                                        <asp:ListItem Value="false">Không</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Mã sản phẩm
                                </div>
                                <div class="row-right">
                                    <asp:Label ID="lblSKU" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Chất liệu                                    
                                </div>
                                <div class="row-right">
                                    <asp:Label ID="lbMaterials" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                            <div class="form-row main-color">
                                <div class="row-left">
                                    Màu chủ đạo
                                </div>
                                <div class="row-right">
                                    <asp:Label ID="lbColor" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Số lượng
                                </div>
                                <div class="row-right">
                                    <asp:Label ID="lbProductStock" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Nhà cung cấp
                                </div>
                                <div class="row-right">
                                    <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control" Enabled="False">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Trạng thái                                    
                                </div>
                                <div class="row-right">
                                    <asp:DropDownList ID="ddlStockStatus" runat="server" CssClass="form-control" Enabled="False">
                                        <asp:ListItem Text="Còn hàng" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Hết hàng" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Đang nhập hàng" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Giá cũ chưa sale
                                </div>
                                <div class="row-right">
                                    <asp:Label ID="lbOldPrice" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Giá sỉ
                                </div>
                                <div class="row-right">
                                    <asp:Label ID="lbRegularPrice" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                            <asp:Literal ID="ltrCostOfGood" runat="server"></asp:Literal>
                            <div class="form-row">
                                <div class="row-left">
                                    Giá lẻ
                                </div>
                                <div class="row-right">
                                    <asp:Label ID="lbRetailPrice" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Hàng Order
                                </div>
                                <div class="row-right">
                                    <asp:DropDownList ID="ddlPreOrder" runat="server" CssClass="form-control" Enabled="False">
                                        <asp:ListItem Text="Khồng cần đặt hàng" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Phải đặt hàng" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Tags
                                </div>
                                <div class="row-right">
                                    <input type="text" id="txtTag" data-role="tagsinput" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Nội dung
                                </div>
                                <div class="row-right">
                                    <div class="content-box">
                                        <asp:Literal ID="pContent" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Thư viện ảnh
                                </div>
                                <div class="row-right">
                                    <asp:Literal ID="imageGallery" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="form-row">
                                <asp:Literal ID="ltrEdit2" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row tableview">
                <div class="col-md-12">
                    <div class="panel-table clear">
                        <div class="responsive-table">
                            <table class="table table-checkable table-product all-product-table">
                                <tbody>
                                    <asp:Literal ID="ltrList" runat="server" EnableViewState="false"></asp:Literal>
                                </tbody>
                            </table>
                        </div>
                        <div class="panel-footer clear">
                            <div class="pagination">
                                <%this.DisplayHtmlStringPaging1();%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Create Register -->
        <div class="modal fade" id="modalUpdateProductSKU" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Sửa mã sản phẩm</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row form-group">
                            <div class="col-md-3 col-xs-4">
                                <p>Mã cũ</p>
                            </div>
                            <div class="col-md-9 col-xs-8">
                                <asp:TextBox ID="txtOldSKU" runat="server" CssClass="form-control text-right" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-3 col-xs-4">
                                <p>Mã mới</p>
                            </div>
                            <div class="col-md-9 col-xs-8">
                                <asp:TextBox ID="txtNewSKU" runat="server" CssClass="form-control text-right"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="closeUpdateProductSKU" type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                        <button id="updateProductSKU" type="button" class="btn btn-primary">Xác nhận</button>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdfTempVariable" runat="server" />
        <asp:HiddenField ID="hdfVariableFull" runat="server" />
        <asp:HiddenField ID="hdfTable" runat="server" />
        <asp:HiddenField ID="hdfTags" runat="server" />
    </main>

    <script type="text/javascript" src="Scripts/bootstrap-tagsinput.min.js"></script>

    <script src="/App_Themes/Ann/js/sync-product-v2.js?v=11072020"></script>
    <script src="/App_Themes/Ann/js/copy-product-info.js?v=11072020"></script>pt>
    <script src="/App_Themes/Ann/js/download-product-image.js?v=11072020"></script>
    <script>
        // init Input Tag
        let txtTagDOM = $('#txtTag');

        $(document).ready(() => {
            $("a[data-target='#modalUpdateProductSKU']").click(e => {
                $("#<%=txtNewSKU.ClientID%>").focus();
            });

            $("#updateProductSKU").click(e => {
                let oldSKU = $("#<%=txtOldSKU.ClientID%>").val();
                let newSKU = $("#<%=txtNewSKU.ClientID%>").val();

                $.ajax({
                    type: "POST",
                    url: "/tat-ca-san-pham.aspx/updateProductSKU",
                    data: "{oldSKU: '" + oldSKU.toUpperCase() + "', newSKU: '" + newSKU.toUpperCase() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    success: function (msg) {
                        if (msg.d == "orderfound")
                        {
                            $("#closeUpdateProductSKU").click();
                            swal("Thông báo", "Sản phẩm đã được bán vì vậy không được sửa!", "error");
                        }
                        else if (msg.d == "stockfound")
                        {
                            $("#closeUpdateProductSKU").click();
                            swal("Thông báo", "Sản phẩm đã được nhập kho vì vậy không được sửa!", "error");
                        }
                        else if (msg.d == "newskuexist")
                        {
                            $("#<%=txtNewSKU.ClientID%>").focus();
                            swal("Thông báo", "Mã sản phẩm mới đã được tạo cho sản phẩm khác!", "error");
                        }
                        else if (msg.d == "true")
                        {
                            swal({
                                title: 'Thông báo',
                                text: 'Đã cập nhật mã sản phẩm thành công!',
                                type: 'success',
                                showCancelButton: false,
                                closeOnConfirm: true,
                                confirmButtonText: "OK",
                            }, function (confirm) {
                                if (confirm) location.reload();
                            });
                        }
                        else
                        {
                            alert("Lỗi");
                        }
                    },
                    complete: function () {
                        HoldOn.close();
                    }
                });
            });

            // Init Tags
            txtTagDOM.tagsinput('add', $("#<%=hdfTags.ClientID%>").val());
            let divBootstrapTagsinput = $('.bootstrap-tagsinput');
            divBootstrapTagsinput.find('span[data-role="remove"]').remove();
            divBootstrapTagsinput.find('input[type="text"]').remove();
        });

        function deleteProduct(id)
        {
            swal({
                title: 'Thông báo',
                text: 'Bạn có muốn xóa sản phẩm này?',
                type: 'info',
                showCancelButton: true,
                closeOnConfirm: false,
                closeOnCancel: true,
                cancelButtonText: "Để em kiểm tra lại..",
                confirmButtonText: "Xóa luôn..",
                html: true,
            }, function (isconfirm) {
                if (isconfirm)
                {
                    ajaxDeleteProduct(id);
                }
            });
        }

        function ajaxDeleteProduct(id)
        {
            $.ajax({
                type: "POST",
                url: "/tat-ca-san-pham.aspx/deleteProduct",
                data: "{id: " + id + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    HoldOn.open();
                },
                success: function (msg) {
                    if (msg.d === "orderfound")
                    {
                        swal("Thông báo", "Sản phẩm đã được bán vì vậy không được xóa!", "error");
                    }
                    else if (msg.d === "true")
                    {
                        swal({
                            title: 'Thông báo',
                            text: 'Đã xóa sản phẩm thành công!',
                            type: "success",
                            showCancelButton: false,
                            closeOnConfirm: false,
                            confirmButtonText: "OK",
                        }, function (isconfirm) {
                            if (isconfirm) window.location.replace("/tat-ca-san-pham");
                        });
                    }
                    else
                    {
                        alert("Lỗi");
                    }
                },
                complete: function () {
                    HoldOn.close();
                }
            });
        }
    </script>
</asp:Content>
