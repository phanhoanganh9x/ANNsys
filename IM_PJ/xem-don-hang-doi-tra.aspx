<%@ Page Title="Thông tin trả hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="xem-don-hang-doi-tra.aspx.cs" Inherits="IM_PJ.xem_don_hang_doi_tra" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/App_Themes/Ann/js/search-customer.js?v=202108040205"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="parent" runat="server">
        <main id="main-wrap">
            <div class="container">
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot"><asp:Literal ID="ltrHeading" runat="server"></asp:Literal></h3>
                            </div>
                            <div class="panel-body">
                                <div class="row pad">
                                    <div class="col-md-3">
                                        <label class="left pad10">Nhân viên: </label>
                                        <div class="ordercreateby">
                                            <asp:Literal ID="ltrCreateBy" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Ngày tạo: </label>
                                        <div class="ordercreatedate">
                                            <asp:Literal ID="ltrCreateDate" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10"></label>
                                        <div class="orderstatus">
                                            <asp:Literal ID="ltrOrderStatus" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Đơn hàng trừ tiền: </label>
                                        <div class="orderstatus">
                                            <asp:Literal ID="ltrOrderSaleID" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="row pad">
                                    <div class="col-md-3">
                                        <label class="left pad10">Số lượng: </label>
                                        <div class="orderquantity">
                                            <asp:Literal ID="ltrOrderQuantity" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Tổng tiền: </label>
                                        <div class="ordertotalprice">
                                            <asp:Literal ID="ltrOrderTotalPrice" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Phí đổi hàng: </label>
                                        <div class="ordernote">
                                            <asp:Literal ID="ltrTotalRefundFee" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Ghi chú: </label>
                                        <div class="ordernote">
                                            <asp:Literal ID="ltrRefundNote" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Literal ID="ltrPrint" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Thông tin khách hàng</h3>
                            </div>
                            <div class="panel-body">
                                <asp:Literal ID="ltrInfo" runat="server"></asp:Literal>
                                <div class="row">
                                    <div class="col-md-12 view-detail">
                                        <asp:Literal ID="ltrViewDetail" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel-post">
                            <div class="post-body search-product-content clear">
                                <table class="table table-checkable table-product table-return-order">
                                    <thead>
                                        <tr>
                                            <th class="order-column">#</th>
                                            <th class="image-column">Ảnh</th>
                                            <th class="name-column">Sản phẩm</th>
                                            <th class="sku-column">Mã</th>
                                            <th class="price-column">Giá niêm yết</th>
                                            <th class="sold-price-column">Giá đã bán</th>
                                            <th class="quantity-column">Cần đổi</th>
                                            <th class="type-column">Hình thức</th>
                                            <th class="fee-column">Phí đổi hàng</th>
                                            <th class="total-column">Thành tiền</th>
                                        </tr>
                                    </thead>
                                    <tbody class="content-product">
                                        <asp:Literal ID="ltrList" runat="server"></asp:Literal>
                                    </tbody>
                                </table>
                            </div>
                            <div class="post-footer">
                                <div class="post-row clear">
                                    <div class="left">Số lượng</div>
                                    <div class="right totalproductQuantity">
                                        <asp:Literal ID="ltrQuantity" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">Phí đổi hàng</div>
                                    <div class="right totalrefund">
                                        <asp:Literal ID="ltrRefund" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">Tổng tiền (đã trừ phí)</div>
                                    <div class="right totalpriceorder">
                                        <asp:Literal ID="ltrTotal" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Trạng thái đơn hàng</h3>
                            </div>
                            <div class="panel-body">
                                <div class="form-row">
                                    <div class="row-left">
                                        Trạng thái
                                    </div>
                                    <div class="row-right">
                                        <asp:DropDownList ID="ddlRefundStatus" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="1" Text="Chưa trừ tiền"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Đã trừ tiền"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="row-left">
                                        Ghi chú đơn hàng
                                    </div>
                                    <div class="row-right">
                                        <asp:TextBox ID="txtRefundsNote" runat="server" CssClass="form-control" placeholder="Ghi chú"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="panel-post">
                                    <div class="post-table-links clear">
                                        <a href="javascript:;" class="btn link-btn" style="background-color: #f87703; float: right;" onclick="Update()"><i class="fa fa-floppy-o"></i> Xác nhận</a>
                                        <asp:Button ID="btnCreate" runat="server" OnClick="btnCreate_Click" Style="display: none" />
                                        <a href="javascript:;" class="btn link-btn" style="background-color: #F44336; float: right;" onclick="Delete()"><i class="fa fa-times" aria-hidden="true"></i> Làm lại đơn hàng này</a>
                                        <asp:Button ID="btnDelete" runat="server" CssClass="btn primary-btn fw-btn" Text="Tạo" OnClick="btnDelete_Click" Style="display: none" />
                                        <a href="/danh-sach-don-tra-hang" class="btn link-btn" style="background-color: #f87703; float: right;" ><i class="fa fa-reply" aria-hidden="true"></i> Trở về danh sách</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="buttonbar">
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel-buttonbar">
                            <div class="panel-post">
                                <div class="post-table-links clear">
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #f87703; float: right;" onclick="Update()"><i class="fa fa-floppy-o"></i> Xác nhận</a>
                                        <a href="javascript:;" class="btn link-btn" style="background-color: #F44336; float: right;" onclick="Delete()"><i class="fa fa-times" aria-hidden="true"></i> Làm lại đơn hàng này</a>
                                        <a href="/danh-sach-don-tra-hang" class="btn link-btn" style="background-color: #f87703; float: right;" ><i class="fa fa-reply" aria-hidden="true"></i> Trở về danh sách</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdfCustomerID" runat="server" />
            <asp:HiddenField ID="hdfCustomerPhone" runat="server" />
        </main>
    </asp:Panel>
    <style>
        .pad {
            padding-bottom: 15px;
        }
        .pad10 {
            padding-right: 10px;
        }
        .search-product-content {
            background: #fff;
            min-height: 400px;
        }
    </style>
    <telerik:RadAjaxManager ID="rAjax" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnOrder">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="parent" LoadingPanelID="rxLoading"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <script src="https://code.jquery.com/jquery-3.3.1.min.js" integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8=" crossorigin="anonymous"></script>

    <script>
        "use strict";

        $(document).ready(function () {

            _initReceiverAddress();
            _onChangeReceiverAddress();
            _getCustomerAddress($("#<%=hdfCustomerPhone.ClientID%>").val());
            $("select[id$='_ddlProvince']").attr('disabled', true);
            $("select[id$='_ddlProvince']").prop('readonly', true);
            $("select[id$='_ddlDistrict']").attr('disabled', true);
            $("select[id$='_ddlDistrict']").prop('readonly', true);
            $("select[id$='_ddlWard']").attr('disabled', true);
            $("select[id$='_ddlWard']").prop('readonly', true);
        });

        function clickrow(obj) {
            if (!obj.find("td").eq(1).hasClass("checked")) {
                obj.find("td").addClass("checked");
            }
            else {
                obj.find("td").removeClass("checked");
            }
        }

        function Update() {
            $("#<%=btnCreate.ClientID%>").click();
        }

        function Delete() {
            swal({
                title: "Cơ hội cuối nha!",
                text: "Đơn hàng trả này sẽ được xóa và tạo lại. <br><br><strong>Nhớ làm cho đúng á!!</strong>",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "OK sếp !!",
                cancelButtonText: "Để em coi lại..",
                closeOnConfirm: true,
                html: true
            }, function () {
                HoldOn.open();
                $("#<%=btnDelete.ClientID%>").click();
            });
        }

        // format price
        function formatThousands(n, dp) {
            var s = '' + (Math.floor(n)),
                d = n % 1,
                i = s.length,
                r = '';
            while ((i -= 3) > 0) {
                r = ',' + s.substr(i, 3) + r;
            }
            return s.substr(0, i + 3) + r +
                (d ? '.' + Math.round(d * Math.pow(10, dp || 2)) : '');
        };
    </script>
</asp:Content>
