<%@ Page Title="Danh sách đơn hàng chuyển hoàn" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="danh-sach-don-hang-chuyen-hoan.aspx.cs" Inherits="IM_PJ.danh_sach_don_hang_chuyen_hoan" EnableSessionState="ReadOnly" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title left">Chuyển hoàn <span>(<asp:Literal ID="ltrNumberOfOrder" runat="server" EnableViewState="false"></asp:Literal>)</span></h3>
                    <div class="right above-list-btn">
                        <a href="javascript:;" onclick="addOrder()" class="h45-btn primary-btn btn">Thêm mới</a>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="filter-above-wrap clear">
                        <div class="filter-control">
                            <div class="row">
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtAgentName" runat="server" CssClass="form-control" placeholder="Tìm đơn hàng" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0" Text="Loại"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Lẻ"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Sỉ"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlPaymentStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0" Text="Thanh toán"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Chưa thanh toán"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Thanh toán thiếu"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Đã thanh toán"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0" Text="Kiểu thanh toán"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Tiền mặt"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Chuyển khoản"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Thu hộ"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Công nợ"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlShippingType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0" Text="Giao hàng"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Lấy trực tiếp"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Bưu điện"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Proship"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Chuyển xe"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="Nhân viên giao"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="GHTK"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="Viettel"></asp:ListItem>
                                        <asp:ListItem Value="8" Text="Grab"></asp:ListItem>
                                        <asp:ListItem Value="9" Text="AhaMove"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlCreateBy" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-1">
                                    <a href="javascript:;" onclick="searchAgent()" class="btn primary-btn h45-btn"><i class="fa fa-search"></i></a>
                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn primary-btn h45-btn" OnClick="btnSearch_Click" Style="display: none" />
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
                            <table class="table table-checkable table-product">
                                <tbody>
                                    <tr>
                                        <th>Mã</th>
                                        <th>Loại</th>
                                        <th>Điện thoại</th>
                                        <th>Khách hàng</th>
                                        <th>Mua</th>
                                        <th>Thanh toán</th>
                                        <th>Xử lý</th>
                                        <th>Kiểu thanh toán</th>
                                        <th>Giao hàng</th>
                                        <th>Tổng tiền</th>
                                        <th>Hoàn tất</th>
                                        <th>Chuyển hoàn</th>
                                        <th></th>
                                    </tr>
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
        <asp:HiddenField ID="hdfcreate" runat="server" />
        <script type="text/javascript">
            $("#<%=txtAgentName.ClientID%>").keyup(function (e) {
                if (e.keyCode == 13)
                {
                    $("#<%= btnSearch.ClientID%>").click();
                }
            });

            function searchAgent() {
                $("#<%= btnSearch.ClientID%>").click();
            }

            function addOrder() {
                swal({
                    title: "Tìm đơn hàng",
                    text: "Nhập mã đơn hàng cần chuyển hoàn vào đây:",
                    type: "input",
                    showCancelButton: true,
                    closeOnConfirm: false,
                    cancelButtonText: "Để em xem lại..",
                    confirmButtonText: "Tới đi sếp ơi!",
                    html: true,
                }, function (ID) {
                    if (ID != "" && $.isNumeric(ID)) {
                        window.location.href = "/thong-tin-don-hang-chuyen-hoan?id=" + ID;
                    }
                    else {
                        swal({
                            title: "Có gì đó sai sai à nghen",
                            text: "Xem lại mã đơn hàng coi nhập đúng chưa (phải nhập số):",
                            type: "input",
                            showCancelButton: true,
                            closeOnConfirm: true,
                            cancelButtonText: "Để em xem lại..",
                            confirmButtonText: "Lần này đúng rồi sếp!",
                            html: true,
                        }, function (ID) {
                            if (ID != "" && $.isNumeric(ID)) {
                                window.location.href = "/thong-tin-don-hang-chuyen-hoan?id=" + ID;
                            }
                            else {

                            }
                        });
                    }
                });
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
