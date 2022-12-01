<%@ Page Language="C#" Title="Xem bài viết" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeBehind="xem-bai-viet.aspx.cs" Inherits="IM_PJ.xem_bai_viet" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        .post-content {
            font-size: 18px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-3">
                </div>
                <div class="col-md-6">
                    <div class="panel panelborderheading">
                        <div class="panel-heading clear">
                            <h3 class="page-title left not-margin-bot"><asp:Literal ID="ltrTitle" runat="server"></asp:Literal></h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-row">
                                <asp:Literal ID="ltrEditTop" runat="server"></asp:Literal>
                            </div>
                            <div class="form-row post-content">
                                <asp:Literal ID="ltrContent" runat="server"></asp:Literal>
                            </div>
                            <div class="form-row">
                                <asp:Literal ID="imageGallery" runat="server"></asp:Literal>
                            </div>
                            <div class="form-row">
                                <asp:Literal ID="ltrEditBottom" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                </div>
            </div>
        </div>
    </main>
    <script src="/App_Themes/Ann/js/copy-post-to-app.js?v=01122022"></script>
    <script src="/App_Themes/Ann/js/copy-post-info.js?v=01122022"></script>
    <script src="/App_Themes/Ann/js/download-post-image.js?v=01122022"></script>
</asp:Content>
