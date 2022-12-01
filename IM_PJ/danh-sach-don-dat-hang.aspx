<%@ Page Title="Danh sách đơn đặt hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="danh-sach-don-dat-hang.aspx.cs" Inherits="IM_PJ.danh_sach_don_dat_hang" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="App_Themes/Ann/css/pages/danh-sach-don-dat-hang/danh-sach-don-dat-hang.css?v=202109271901" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title left">Đơn đặt hàng <span id="spanNumber"></span></h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-3 col-xs-12 search-box-type">
                                    <div class="col-1">
                                        <asp:TextBox ID="txtSearchOrder" runat="server" CssClass="form-control" placeholder="Tìm đơn đặt hàng" autocomplete="off" onKeyUp="onKeyUp_txtSearchOrder(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-2">
                                        <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="3" Text="Đơn đặt hàng"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Sản phẩm"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlExcuteStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0|1" Text="Xử lý"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Chờ tiếp nhận" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Đang xử lý"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Đã hủy"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlDiscount" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Chiết khấu"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Có chiết khấu"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Không chiết khấu"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <label>Từ ngày</label>
                                    <telerik:RadDatePicker RenderMode="Lightweight" ID="rOrderFromDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate">
                                        <DateInput DisplayDateFormat="dd/MM/yyyy" runat="server">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <label>Đến ngày</label>
                                    <telerik:RadDatePicker RenderMode="Lightweight" ID="rOrderToDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate">
                                        <DateInput DisplayDateFormat="dd/MM/yyyy" runat="server">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </div>
                                <div class="col-md-1 col-xs-6 search-button">
                                    <a href="javascript:;" onclick="onClickSearchPreOrder()" class="btn primary-btn h45-btn"><i class="fa fa-search"></i></a>
                                    <a href="/danh-sach-don-dat-hang" class="btn primary-btn h45-btn"><i class="fa fa-times" aria-hidden="true"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-1 col-xs-6">
                                    <asp:DropDownList ID="ddlCouponStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Coupon"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Có"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Không"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Kiểu thanh toán"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Tiền mặt"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Chuyển khoản"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Thu hộ"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlShippingType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Kiểu giao hàng"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Lấy trực tiếp"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Chuyển xe"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="GHTK"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlQuantityFilter" runat="server" CssClass="form-control" onchange="onChange_ddlQuantityFilter(this)">
                                        <asp:ListItem Value="" Text="Số lượng mua"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Số lượng lớn hơn"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Số lượng nhỏ hơn"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Số lượng trong khoảng"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6 greaterthan lessthan">
                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" placeholder="Số lượng" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-md-2 col-xs-6 between hide">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtQuantityMin" runat="server" CssClass="form-control" placeholder="Min" autocomplete="off"></asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtQuantityMax" runat="server" CssClass="form-control" placeholder="Max" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlCreatedBy" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-table clear">
                        <div class="panel-footer clear">
                            <div class="pagination">
                            </div>
                        </div>
                        <div class="responsive-table">
                            <table id="tbPreOrder" class="table table-checkable table-product shop_table_responsive"></table>
                        </div>
                        <div class="panel-footer clear">
                            <div class="pagination">
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <asp:HiddenField ID="hdRole" runat="server"/>
        </div>

        <script type="text/javascript" src="/App_Themes/Ann/js/utils/string-format.js?v=202109302253"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/common/utils-service.js?v=01122022"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/common/order-service.js?v=202109271901"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/danh-sach-don-dat-hang/danh-sach-don-dat-hang-service.js?v=202101051418"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/controllers/danh-sach-don-dat-hang/danh-sach-don-dat-hang-controller.js?v=202101051418"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/pages/danh-sach-don-dat-hang/danh-sach-don-dat-hang.js?v=202110011616"></script>
    </main>
</asp:Content>
