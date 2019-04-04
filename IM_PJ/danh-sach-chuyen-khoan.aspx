<%@ Page Title="Danh sách đơn hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="danh-sach-chuyen-khoan.aspx.cs" Inherits="IM_PJ.danh_sach_chuyen_khoan" EnableSessionState="ReadOnly" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="/Scripts/moment.min.js"></script>
    <script src="/Scripts/moment-with-locales.min.js"></script>
    <script src="/Scripts/bootstrap-datetimepicker.min.js"></script>

    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title left">Danh sách đơn hàng 
                        <span>(<asp:Literal ID="ltrNumberOfOrder" runat="server" EnableViewState="false"></asp:Literal>đơn)
                        </span>
                    </h3>
                    <div class="right above-list-btn">
                        <a href="/them-moi-don-hang" class="h45-btn primary-btn btn">Thêm mới</a>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-5">
                                    <asp:TextBox ID="txtSearchOrder" runat="server" CssClass="form-control" placeholder="Tìm đơn hàng" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlCreatedDate" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Tất cả thời gian"></asp:ListItem>
                                        <asp:ListItem Value="today" Text="Hôm nay"></asp:ListItem>
                                        <asp:ListItem Value="yesterday" Text="Hôm qua"></asp:ListItem>
                                        <asp:ListItem Value="beforeyesterday" Text="Hôm kia"></asp:ListItem>
                                        <asp:ListItem Value="week" Text="Tuần này"></asp:ListItem>
                                        <asp:ListItem Value="7days" Text="7 ngày"></asp:ListItem>
                                        <asp:ListItem Value="thismonth" Text="Tháng này"></asp:ListItem>
                                        <asp:ListItem Value="lastmonth" Text="Tháng trước"></asp:ListItem>
                                        <asp:ListItem Value="beforelastmonth" Text="Tháng trước nữa"></asp:ListItem>
                                        <asp:ListItem Value="30days" Text="30 ngày"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlCreatedBy" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-1">
                                    <a href="javascript:;" onclick="searchOrder()" class="btn primary-btn h45-btn"><i class="fa fa-search"></i></a>
                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn primary-btn h45-btn" OnClick="btnSearch_Click" Style="display: none" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-5">
                                    <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Loại"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Lẻ"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Sỉ"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlPaymentStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Thanh toán"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Chưa thanh toán"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Thanh toán thiếu"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Đã thanh toán"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-1">
                                    <a href="/danh-sach-chuyen-khoan" class="btn primary-btn h45-btn"><i class="fa fa-times" aria-hidden="true"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-table clear">
                        <div class="panel-footer clear">
                            <div class="pagination">
                                <%this.DisplayHtmlStringPaging1();%>
                            </div>
                        </div>
                        <div class="responsive-table">
                            <table class="table table-checkable table-product table-new-product">
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

        <!-- Modal -->
        <div class="modal fade" id="TransferBankModal" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Cập nhật thông tin chuyển khoản</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-3">
                                <p>Chuyển từ</p>
                            </div>
                            <div class="col-xs-9">
                                <asp:DropDownList ID="ddlCustomerBank" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Tất cả thời gian"></asp:ListItem>
                                    <asp:ListItem Value="today" Text="Hôm nay"></asp:ListItem>
                                    <asp:ListItem Value="yesterday" Text="Hôm qua"></asp:ListItem>
                                    <asp:ListItem Value="beforeyesterday" Text="Hôm kia"></asp:ListItem>
                                    <asp:ListItem Value="week" Text="Tuần này"></asp:ListItem>
                                    <asp:ListItem Value="7days" Text="7 ngày"></asp:ListItem>
                                    <asp:ListItem Value="thismonth" Text="Tháng này"></asp:ListItem>
                                    <asp:ListItem Value="lastmonth" Text="Tháng trước"></asp:ListItem>
                                    <asp:ListItem Value="beforelastmonth" Text="Tháng trước nữa"></asp:ListItem>
                                    <asp:ListItem Value="30days" Text="30 ngày"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-3">
                                <p>Tài khoản nhận</p>
                            </div>
                            <div class="col-xs-9">
                                <asp:DropDownList ID="ddlAccoutBank" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Tất cả thời gian"></asp:ListItem>
                                    <asp:ListItem Value="today" Text="Hôm nay"></asp:ListItem>
                                    <asp:ListItem Value="yesterday" Text="Hôm qua"></asp:ListItem>
                                    <asp:ListItem Value="beforeyesterday" Text="Hôm kia"></asp:ListItem>
                                    <asp:ListItem Value="week" Text="Tuần này"></asp:ListItem>
                                    <asp:ListItem Value="7days" Text="7 ngày"></asp:ListItem>
                                    <asp:ListItem Value="thismonth" Text="Tháng này"></asp:ListItem>
                                    <asp:ListItem Value="lastmonth" Text="Tháng trước"></asp:ListItem>
                                    <asp:ListItem Value="beforelastmonth" Text="Tháng trước nữa"></asp:ListItem>
                                    <asp:ListItem Value="30days" Text="30 ngày"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-3">
                                <p>Trạng Thái</p>
                            </div>
                            <div class="col-xs-9">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Tất cả thời gian"></asp:ListItem>
                                    <asp:ListItem Value="today" Text="Hôm nay"></asp:ListItem>
                                    <asp:ListItem Value="yesterday" Text="Hôm qua"></asp:ListItem>
                                    <asp:ListItem Value="beforeyesterday" Text="Hôm kia"></asp:ListItem>
                                    <asp:ListItem Value="week" Text="Tuần này"></asp:ListItem>
                                    <asp:ListItem Value="7days" Text="7 ngày"></asp:ListItem>
                                    <asp:ListItem Value="thismonth" Text="Tháng này"></asp:ListItem>
                                    <asp:ListItem Value="lastmonth" Text="Tháng trước"></asp:ListItem>
                                    <asp:ListItem Value="beforelastmonth" Text="Tháng trước nữa"></asp:ListItem>
                                    <asp:ListItem Value="30days" Text="30 ngày"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-3">
                                <p>Tổng đơn</p>
                            </div>
                            <div class="col-xs-9 text-right">
                                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control text-align-right" value="10,000,000" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-3">
                                <p>Đã nhận</p>
                            </div>
                            <div class="col-xs-9">
                                <asp:TextBox ID="txtNumber" runat="server" CssClass="form-control text-right" placeholder="Số tiền khách đã chuyển"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-3">
                                <p>Thời gian chuyển</p>
                            </div>
                            <div class="col-xs-9">
                                <div class="form-group">
                                    <div class="input-group date" id="datetimepicker">
                                        <input type="text" class="form-control" />
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <button type="button" class="btn btn-primary">Save changes</button>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdfcreate" runat="server" />
        <script type="text/javascript">
            $(document).ready(() => {
                $("button[data-toggle='modal']").click(e => {
                    let picker = $('#datetimepicker');
                    picker.datetimepicker({
                        format: 'DD/MM/YYYY hh:mm:ss',
                        setDate: new Date(),
                        autoclose: true
                    });
                    
                    //picker.children('input').val(now.format('dd/mm/yyyy'));
                    console.log($(this).parent());
                    console.log(e);
                })
            })
            // Parse URL Queries
            function url_query(query) {
                query = query.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
                var expr = "[\\?&]" + query + "=([^&#]*)";
                var regex = new RegExp(expr);
                var results = regex.exec(window.location.href);
                if (results !== null) {
                    return results[1];
                } else {
                    return false;
                }
            }

            var url_param = url_query('quantityfilter');
            if (url_param) {
                if (url_param == "greaterthan" || url_param == "lessthan") {
                    $(".greaterthan").removeClass("hide");
                    $(".between").addClass("hide");
                }
                else if (url_param == "between") {
                    $(".between").removeClass("hide");
                    $(".greaterthan").addClass("hide");
                }
            }

            function searchOrder() {
                $("#<%= btnSearch.ClientID%>").click();
            }

            var formatThousands = function (n, dp) {
                var s = '' + (Math.floor(n)), d = n % 1, i = s.length, r = '';
                while ((i -= 3) > 0) { r = ',' + s.substr(i, 3) + r; }
                return s.substr(0, i + 3) + r +
                    (d ? '.' + Math.round(d * Math.pow(10, dp || 2)) : '');
            };
        </script>
    </main>
</asp:Content>
