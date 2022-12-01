<%@ Page Language="C#" Title="Xem bài viết" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeBehind="xem-bai-viet-app.aspx.cs" Inherits="IM_PJ.xem_bai_viet_app" %>

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
        img {
            width: inherit;
            margin: 10px auto 0 auto;
            max-width: 100%;
            height: auto;
        }
        .image-gallery img {
            width: 100%;
            margin: 0;
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
                    <div class="panel panelborderheading">
                        <div class="panel-heading clear">
                            <h3 class="page-title left not-margin-bot">Tóm tắt</h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-row">
                                <asp:Literal ID="ltrThumbnail" runat="server"></asp:Literal>
                                <asp:Literal ID="ltrSummary" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="panel panelborderheading">
                        <div class="panel-heading clear">
                            <h3 class="page-title left not-margin-bot"><asp:Literal ID="ltrTitle" runat="server"></asp:Literal></h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-row post-content">
                                <asp:Literal ID="ltrLink" runat="server"></asp:Literal>
                                <asp:Literal ID="ltrContent" runat="server"></asp:Literal>
                            </div>
                            <div id="divYoutube" class="form-row hidden">
                                <hr style="border-top: 1px solid #eeeeee"/>
                                <div class="form-row">
                                    <h4>Youtube:</h4>
                                    <span id="spanYoutubeUrl"></span>
                                </div>
                                <div class="form-row">
                                    <asp:RadioButtonList ID="rdbActiveVideo" CssClass="RadioButtonList" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="true" Enabled="false">Hiện</asp:ListItem>
                                        <asp:ListItem Value="false" Enabled="false">Ẩn</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div id="divIframeYoutube" class="form-row"></div>
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
                    <div class="panel panelborderheading">
                        <div class="panel-heading clear">
                            <h3 class="page-title left not-margin-bot">Thao tác</h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-row">
                                <asp:Literal ID="ltrPostInfo" runat="server"></asp:Literal>
                                <asp:Literal ID="ltrEditTop" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdfPostId" runat="server" />
    </main>
    <script src="/App_Themes/Ann/js/sync-post.js?v=01122022"></script>
    <script src="/App_Themes/Ann/js/copy-post-app-info.js?v=01122022"></script>
    <script src="/App_Themes/Ann/js/download-post-app-image.js?v=01122022"></script>

    <script>
        $(document).ready(function () {
            _initVideo();
        });

        function _initVideo() {
            let postId = +$("#<%=hdfPostId.ClientID%>").val() || null;

            if (postId) {
                $.ajax({
                    type: "GET",
                    url: "/api/v1/post/" + postId + "/video",
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    success: function (response, textStatus, xhr) {
                        HoldOn.close();

                        let $divYoutube = $("#divYoutube");

                        let $spanYoutubeUrl = $("#spanYoutubeUrl");
                        let $rdbActiveVideo = $("#<%=rdbActiveVideo.ClientID%>");
                        let $divIframeYoutube = $("#divIframeYoutube")

                        if (xhr.status == 200) {
                            if (response) {
                                $divYoutube.removeClass('hidden');

                                url = "https://www.youtube.com/watch?v=" + response.videoId;
                                $spanYoutubeUrl.html(url);

                                if (response.isActive) {
                                    $rdbActiveVideo.find("input[value='true']").prop("checked", true);
                                    $rdbActiveVideo.find("input[value='false']").prop("checked", false);
                                }
                                else {
                                    $rdbActiveVideo.find("input[value='true']").prop("checked", false);
                                    $rdbActiveVideo.find("input[value='false']").prop("checked", true);
                                }

                                let iframe = '';

                                iframe += '<iframe ';
                                iframe += '  src="' + response.url + '" ';
                                iframe += '</iframe>';

                                $divIframeYoutube.html(iframe);
                            }
                        }
                        else if (xhr.status == 204) {
                            $divYoutube.addClass('hidden');

                            $spanYoutubeUrl.html('');
                            $rdbActiveVideo.find("input[value='true']").prop("checked", false);
                            $rdbActiveVideo.find("input[value='false']").prop("checked", false);
                            $divIframeYoutube.html('');
                        }
                        else
                        {
                            swal("Thông báo", "Đã có lỗi trong quá trình lấy thông tin video", "error");
                        }
                    },
                    error: function (xhr, textStatus, error) {
                        HoldOn.close();

                        swal("Thông báo", "Đã có lỗi trong quá trình lấy thông tin video", "error");
                    }
                });
            }
        }
    </script>
</asp:Content>
