<%@ Page Title="Thông kê giao hàng tiết kiệm" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="thong-ke-ghtk.aspx.cs" Inherits="IM_PJ.thong_ke_ghtk" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/moment.min.js"></script>
    <script src="/Scripts/moment-with-locales.min.js"></script>
    <script src="/Scripts/bootstrap-datetimepicker.min.js"></script>
    <link rel="stylesheet" href="App_Themes/Ann/css/pages/thong-ke-ghtk/thong-ke-ghtk.css?v=202106061418" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title left">Duyệt đơn hàng GHTK <span id="spanReport"></span></h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-3 col-xs-12">
                                    <input  type="text" id="txtFileName" class="form-control" placeholder="Tên file" onchange="onChangeFileName($(this).val())" disabled/>
                                </div>
                                <div class="col-md-3 col-xs-6">
                                    <input type="file" id="fUpload" accept=".xls" onchange="onChangeUpload($(this))" />
                                </div>
                                <div class="col-md-2 col-xs-4">
                                    <a id="btnUpload" href="javascript:;" class="btn primary-btn fw-btn width-100" onclick="onClickUpload($('#txtFileName').val(), $('#fUpload'))">
                                        <i class="fa fa-cloud-upload" aria-hidden="true"></i> Upload
                                    </a>
                                </div>
                                <div class="col-md-4 col-xs-12">
                                    <asp:Label id="lbUploadStatus" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-3 col-xs-6">
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Tìm đơn hàng" autocomplete="off" onKeyUp="onKeyUpSearch(event)"></asp:TextBox>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlFeeStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Tình trạng phí vận chuyển"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Có lời"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Lỗ vốn"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <label>Từ ngày</label>
                                    <telerik:RadDatePicker RenderMode="Lightweight" ID="dpFromDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate">
                                        <DateInput DisplayDateFormat="dd/MM/yyyy" runat="server">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <label>Đến ngày</label>
                                    <telerik:RadDatePicker RenderMode="Lightweight" ID="dpToDate" ShowPopupOnFocus="true" Width="100%" runat="server" DateInput-CssClass="radPreventDecorate">
                                        <DateInput DisplayDateFormat="dd/MM/yyyy" runat="server">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </div>
                                <div class="col-md-1 col-xs-6 search-button">
                                    <a href="javascript:;" onclick="onClickSearch()" class="btn primary-btn h45-btn"><i class="fa fa-search"></i></a>
                                    <a href="/thong-ke-ghtk" class="btn primary-btn h45-btn"><i class="fa fa-times" aria-hidden="true"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-3">
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlOrderStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Trạng thái tìm đơn"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Tìm thấy đơn"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Không tìm thấy đơn"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlGhtkStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Trạng thái GHTK"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Đã đối soát"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Công nợ trả hàng"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Khác"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-xs-6">
                                    <asp:DropDownList ID="ddlReviewStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Trạng thái duyệt"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Chưa duyệt"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Đã duyệt"></asp:ListItem>
                                    </asp:DropDownList>
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
                            <table id="tbReport" class="table shop_table_responsive table-checkable table-product table-new-product">
                            </table>
                        </div>
                        <div class="panel-footer clear">
                            <div class="pagination">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Check Order Old Modal -->
        <div class="modal fade" id="approveModal" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Duyệt đơn hàng Bưu Điện</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtOrderId" CssClass="form-control" placeholder="Nhập mã đơn hàng cần duyệt"/>
                            <asp:HiddenField ID="hdfdeliverySaveId" runat="server" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="closeApprove" type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                        <button id="commitApprove" type="button" class="btn btn-primary" onclick="onClickApprove()">Duyệt đơn</button>
                    </div>
                </div>
            </div>
        </div>

        <script type="text/javascript" src="/App_Themes/Ann/js/utils/string-format.js?v=202106061418"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/common/utils-service.js?v=202106061418"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/thong-ke-ghtk/thong-ke-ghtk-service.js?v=202106061418"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/controllers/thong-ke-ghtk/thong-ke-ghtk-controller.js?v=202106061418"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/pages/thong-ke-ghtk/thong-ke-ghtk.js?v=202106061418"></script>
    </main>
</asp:Content>
