<%@ Page Title="Thông tin trả hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="thong-tin-tra-hang.aspx.cs" Inherits="IM_PJ.thong_tin_tra_hang" %>

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
                                <h3 class="page-title left not-margin-bot">Thông tin đơn hàng đổi trả #<asp:Literal ID="ltrOrderID" runat="server"></asp:Literal></h3>
                            </div>
                            <div class="panel-body">
                                <div class="row pad">
                                    <div class="col-md-4">
                                        <label class="left pad10">Nhân viên: </label>
                                        <div class="ordercreateby">
                                            <asp:Literal ID="ltrCreateBy" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <label class="left pad10">Ngày tạo: </label>
                                        <div class="ordercreatedate">
                                            <asp:Literal ID="ltrCreateDate" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <label class="left pad10"></label>
                                        <div class="orderstatus">
                                            <asp:Literal ID="ltrOrderStatus" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="row pad">
                                    <div class="col-md-4">
                                        <label class="left pad10">Số lượng: </label>
                                        <div class="orderquantity">
                                            <asp:Literal ID="ltrOrderQuantity" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <label class="left pad10">Tổng tiền: </label>
                                        <div class="ordertotalprice">
                                            <asp:Literal ID="ltrOrderTotalPrice" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <label class="left pad10">Phí đổi hàng: </label>
                                        <div class="ordernote">
                                            <asp:Literal ID="ltrTotalRefundFee" runat="server"></asp:Literal>
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
                                <table class="table table-checkable table-product custom-font-size-12">
                                    <thead>
                                        <tr>
                                            <th>Mã đơn hàng</th>
                                            <th>Tên sản phẩm</th>
                                            <th>SKU</th>
                                            <th>Giá gốc</th>
                                            <th>Giá đã bán</th>
                                            <th>Đổi tối đa</th>
                                            <th>Cần đổi</th>
                                            <th>Hình thức</th>
                                            <th>Phí đổi hàng</th>
                                            <th>Thành tiền</th>
                                        </tr>
                                    </thead>
                                    <tbody class="content-product">
                                        <asp:Literal ID="ltrList" runat="server"></asp:Literal>
                                    </tbody>
                                </table>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Tổng cộng</div>
                                <div class="right totalpriceorder">
                                    <asp:Literal ID="ltrTotal" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="post-row clear">
                                <div class="left">Tổng số lượng sản phẩm</div>
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
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Thông tin trạng thái đơn</h3>
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
                                <div class="post-table-links clear">
                                    <a href="javascript:;" class="btn link-btn" style="background-color: #f87703; float: right; color: #fff;" onclick="Update()">Cập nhật</a>
                                    <asp:Button ID="btnCreate" runat="server" CssClass="btn primary-btn fw-btn" Text="Tạo" OnClick="btnCreate_Click" Style="display: none" />
                                    <a href="/danh-sach-don-tra-hang" class="btn link-btn" style="background-color: #f87703; float: right; color: #fff; margin-right: 10px;" >Trở về</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdfCustomerID" runat="server" />
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
            height: 350px;
            background: #fff;
            overflow-y: scroll;
            padding: 5px;
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

    <script>
        function Update() {
            $("#<%=btnCreate.ClientID%>").click();
        }
    </script>
</asp:Content>
