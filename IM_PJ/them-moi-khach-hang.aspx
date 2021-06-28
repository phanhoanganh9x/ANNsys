<%@ Page Title="Thêm mới khách hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="them-moi-khach-hang.aspx.cs" Inherits="IM_PJ.them_moi_khach_hang" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panelborderheading">
                        <div class="panel-heading clear">
                            <h3 class="page-title left not-margin-bot">Thêm khách hàng</h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-row">
                                <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Họ tên
                                    <asp:RequiredFieldValidator ID="rq" runat="server" ControlToValidate="txtCustomerName" ForeColor="Red" SetFocusOnError="true" ErrorMessage="(*)" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="row-right">
                                    <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" placeholder="Họ tên khách hàng" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                              <div class="form-row">
                                <div class="row-left">
                                    Nick đặt hàng
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNick" ForeColor="Red" SetFocusOnError="true" ErrorMessage="(*)" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="row-right">
                                    <asp:TextBox ID="txtNick" runat="server" CssClass="form-control" placeholder="Nick đặt hàng" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Điện thoại
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCustomerPhone" ForeColor="Red" SetFocusOnError="true" ErrorMessage="(*)" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="row-right">
                                    <asp:TextBox ID="txtCustomerPhone" runat="server" CssClass="form-control" placeholder="Số điện thoại" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Điện thoại 2
                                </div>
                                <div class="row-right">
                                    <asp:TextBox ID="txtCustomerPhone2" runat="server" CssClass="form-control" placeholder="Số điện thoại 2 nếu có" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Zalo
                                </div>
                                <div class="row-right">
                                    <asp:TextBox ID="txtZalo" runat="server" CssClass="form-control" placeholder="Số điện thoại Zalo" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Facebook
                                </div>
                                <div class="row-right">
                                    <asp:TextBox ID="txtFacebook" runat="server" CssClass="form-control" placeholder="Link Facebook" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Địa chỉ
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAddress" ForeColor="Red" SetFocusOnError="true" ErrorMessage="(*)" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="row-right">
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Địa chỉ" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Tỉnh thành
                                </div>
                                <div class="row-right">
                                    <asp:DropDownList ID="ddlProvince" runat="server" CssClass="form-control" AutoPostBack="False"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Phương thức thanh toán
                                </div>
                                <div class="row-right">
                                    <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0" Text="Chọn phương thức thanh toán mặc định"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Tiền mặt"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Chuyển khoản"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Thu hộ"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Công nợ"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Phương thức giao hàng
                                </div>
                                <div class="row-right">
                                    <asp:DropDownList ID="ddlShippingType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0" Text="Chọn phương thức giao hàng mặc định"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Lấy trực tiếp"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Bưu điện"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Proship"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Chuyển xe"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="Nhân viên giao"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="GHTK"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="Viettel"></asp:ListItem>
                                        <asp:ListItem Value="8" Text="Grab"></asp:ListItem>
                                        <asp:ListItem Value="9" Text="AhaMove"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="J&T"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-row">
                                <asp:UpdatePanel ID="up" runat="server">
                                    <ContentTemplate>
                                            <div class="form-row transport-company">
                                                <div class="row-left">
                                                    Chành xe
                                                </div>
                                                <div class="row-right">
                                                    <asp:DropDownList ID="ddlTransportCompanyID" DataTextField="TransportCompanyID" DataValueField="ID" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddlTransportCompanyID_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-row transport-company">
                                                <div class="row-left">
                                                    Nơi nhận
                                                </div>
                                                <div class="row-right">
                                                    <asp:DropDownList ID="ddlTransportCompanySubID" DataTextField="TransportCompanySubID" DataValueField="ID" AppendDataBoundItems="True" AutoPostBack="false" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Ghi chú
                                </div>
                                <div class="row-right">
                                    <asp:TextBox ID="txtNote" runat="server" CssClass="form-control" placeholder="Ghi chú" TextMode="MultiLine" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Ảnh đại diện
                                </div>
                                <div class="row-right">
                                    <telerik:RadAsyncUpload Skin="Metro" runat="server" ID="UploadAvatarImage" ChunkSize="0"
                                        Localization-Select="Chọn ảnh" AllowedFileExtensions=".jpeg,.jpg,.png"
                                        MultipleFileSelection="Disabled" OnClientFileSelected="OnClientFileSelected1" MaxFileInputsCount="1">
                                    </telerik:RadAsyncUpload>
                                    <asp:Image runat="server" ID="AvatarThumbnail" Width="200" />
                                    <asp:HiddenField runat="server" ID="ListAvatarImage" ClientIDMode="Static" />
                                    <div class="hidAvatarImage"></div>
                                </div>
                            </div>
                            <div class="form-row">
                                <a href="javascript:;" class="btn primary-btn fw-btn not-fullwidth" onclick="addCustomer()">Tạo mới</a>
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn primary-btn fw-btn not-fullwidth" Text="Tạo mới" OnClick="btnSubmit_Click" Style="display: none" />
                                <a href="/danh-sach-khach-hang" class="btn primary-btn fw-btn not-fullwidth">Trở về</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
    <script>
        function OnClientFileSelected1(sender, args) {
            if ($telerik.isIE) return;
            else {
                truncateName(args);
                var file = args.get_fileInputField().files.item(args.get_rowIndex());
                showThumbnail(file, args);
            }
        }
        function addCustomer() {
            var CustomerName = $("#<%=txtCustomerName.ClientID%>").val();
            var Nick = $("#<%=txtNick.ClientID%>").val();
            var CustomerPhone = $("#<%=txtCustomerPhone.ClientID%>").val();
            var Address = $("#<%=txtAddress.ClientID%>").val();

            if (CustomerName == "") {
                $("#<%=txtCustomerName.ClientID%>").focus();
                swal("Thông báo", "Chưa nhập họ tên khách hàng", "error");
            }
            else if (Nick == "") {
                $("#<%=txtNick.ClientID%>").focus();
                swal("Thông báo", "Chưa nhập nick đặt hàng", "error");
            }
            else if (CustomerPhone == "") {
                $("#<%=txtCustomerPhone.ClientID%>").focus();
                swal("Thông báo", "Chưa nhập số điện thoại", "error");
            }
            else if (Address == "") {
                $("#<%=txtAddress.ClientID%>").focus();
                swal("Thông báo", "Chưa nhập địa chỉ", "error");
            }
            else {
                HoldOn.open();
                $("#<%=btnSubmit.ClientID%>").click();
            }
        }

        $(document).ready(function () {

            $("#<%=txtCustomerPhone.ClientID%>").keyup(function (e) {
                if (/\D/g.test(this.value)) {
                    // Filter non-digits from input value.
                    this.value = this.value.replace(/\D/g, '');
                }
            });

            $("#<%=txtZalo.ClientID%>").keyup(function (e) {
                if (/\D/g.test(this.value)) {
                    // Filter non-digits from input value.
                    this.value = this.value.replace(/\D/g, '');
                }
            });

            if ($("#<%=ddlShippingType.ClientID%>").find(":selected").val() == 4) {
                $(".transport-company").removeClass("hide");
            }
            else {
                $(".transport-company").addClass("hide");
            }

            $("#<%=ddlShippingType.ClientID%>").change(function () {
                var selected = $(this).find(":selected").val();
                switch(selected) {
                    case "1":
                        $(".transport-company").addClass("hide");
                        $("#<%=ddlTransportCompanyID.ClientID%>").val(0);
                        $("#<%=ddlTransportCompanySubID.ClientID%>").val(0);
                        break;
                    case "2":
                        $(".transport-company").addClass("hide");
                        $("#<%=ddlTransportCompanyID.ClientID%>").val(0);
                        $("#<%=ddlTransportCompanySubID.ClientID%>").val(0);
                        break;
                    case "3":
                        $(".transport-company").addClass("hide");
                        $("#<%=ddlTransportCompanyID.ClientID%>").val(0);
                        $("#<%=ddlTransportCompanySubID.ClientID%>").val(0);
                        break;
                    case "4":
                        $(".transport-company").removeClass("hide");
                        break;
                    case "5":
                        $(".transport-company").addClass("hide");
                        $("#<%=ddlTransportCompanyID.ClientID%>").val(0);
                        $("#<%=ddlTransportCompanySubID.ClientID%>").val(0);
                        break;
                }
            });
        });
    </script>
</asp:Content>