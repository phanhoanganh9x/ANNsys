<%@ Page Title="Sửa bài viết App" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="sua-bai-viet-app.aspx.cs" Inherits="IM_PJ.sua_bai_viet_app" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .width {
            width: calc(100% - 100px);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-9">
                    <div class="panel panelborderheading">
                        <div class="panel-heading clear">
                            <h3 class="page-title left not-margin-bot">Sửa bài viết App</h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-row">
                                <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Tiêu đề
                                </div>
                                <div class="row-right">
                                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="Tiêu đề bài viết" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Kiểu bài viết
                                </div>
                                <div class="row-right">
                                    <asp:DropDownList ID="ddlAction" AppendDataBoundItems="true" runat="server" class="form-control" onchange="changeAction()">
                                        <asp:ListItem Text="Chọn kiểu" Value="" />
                                        <asp:ListItem Text="Bài nội bộ" Value="view_more" />
                                        <asp:ListItem Text="Link ngoài" Value="show_web" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="input-link form-row hide">
                                <div class="row-left">
                                    Link ngoài
                                </div>
                                <div class="row-right">
                                    <asp:TextBox ID="txtLink" runat="server" CssClass="form-control" placeholder="Nhập link" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="input-slug form-row hide">
                                <div class="row-left">
                                    Slug
                                </div>
                                <div class="row-right">
                                    <asp:TextBox ID="txtSlug" runat="server" CssClass="form-control" placeholder="Slug" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Danh mục
                                </div>
                                <div class="row-right parent">
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control slparent" date-name="parentID" data-level="1" onchange="selectCategory($(this))">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Chính sách
                                </div>
                                <div class="row-right">
                                    <asp:DropDownList ID="ddlIsPolicy" AppendDataBoundItems="true" runat="server" class="form-control">
                                        <asp:ListItem Text="Không" Value="False" />
                                        <asp:ListItem Text="Có" Value="True" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Trang chủ
                                </div>
                                <div class="row-right">
                                    <asp:DropDownList ID="ddlAtHome" AppendDataBoundItems="true" runat="server" class="form-control">
                                        <asp:ListItem Text="Ẩn" Value="False" />
                                        <asp:ListItem Text="Hiện" Value="True" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Ảnh đại diện
                                </div>
                                <div class="row-right">
                                    <telerik:RadAsyncUpload Skin="Metro" runat="server" ID="PostPublicThumbnailImage" ChunkSize="0" Localization-Select="Chọn ảnh" AllowedFileExtensions=".jpeg,.jpg,.png" MultipleFileSelection="Disabled" OnClientFileSelected="OnClientFileSelected1" MaxFileInputsCount="1"  OnClientFileUploadRemoved="OnClientFileUploadRemoved">
                                    </telerik:RadAsyncUpload>
                                    <asp:Image runat="server" ID="PostPublicThumbnail" Width="200" />
                                    <asp:HiddenField runat="server" ID="ListPostPublicThumbnail" ClientIDMode="Static" />
                                    <div class="hidPostPublicThumbnail"></div>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Youtube
                                </div>
                                <div class="row-right">
                                    <div class="form-row">
                                        <asp:TextBox ID="txtYoutubeUrl" runat="server"  CssClass="form-control" placeholder="Url Youtube của sản phẩm" onchange="onChangeYoutubeUrl($(this).val())"></asp:TextBox>
                                    </div>
                                    <div class="form-row">
                                        <asp:RadioButtonList ID="rdbActiveVideo" CssClass="RadioButtonList" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="true" Selected="True">Hiện</asp:ListItem>
                                            <asp:ListItem Value="false">Ẩn</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div id="divYoutube" class="form-row hidden">
                                    </div>
                                </div>
                            </div>
                            <div class="input-summary form-row">
                                <div class="row-left">
                                    Mô tả ngắn
                                </div>
                                <div class="row-right">
                                    <telerik:RadEditor runat="server" ID="pSummary" Width="100%" Height="150px" ToolsFile="~/FilesResources/ToolContent.xml" Skin="Metro" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd" AutoResizeHeight="False" ContentFilters="MakeUrlsAbsolute">
                                        <ImageManager ViewPaths="~/uploads/images/posts" UploadPaths="~/uploads/images/posts" DeletePaths="~/uploads/images/posts" />
                                    </telerik:RadEditor>
                                </div>
                            </div>
                            <div class="input-content form-row">
                                <div class="row-left">
                                    Nội dung
                                </div>
                                <div class="row-right">
                                    <telerik:RadEditor runat="server" ID="pContent" Width="100%" Height="700px" ToolsFile="~/FilesResources/ToolContent.xml" Skin="Metro" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd" AutoResizeHeight="False" ContentFilters="MakeUrlsAbsolute">
                                        <ImageManager ViewPaths="~/uploads/images/posts" UploadPaths="~/uploads/images/posts" DeletePaths="~/uploads/images/posts" />
                                    </telerik:RadEditor>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Thư viện ảnh
                                </div>
                                <div class="row-right">
                                    <asp:FileUpload runat="server" ID="UploadImages" name="uploadImageGallery" onchange='showImageGallery(this,$(this));' AllowMultiple="true" />
                                    <asp:Literal ID="imageGallery" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="form-row post-variants hide">
                                <div class="row-left">
                                    Biến thể
                                </div>
                                <div class="row-right post-variant-list">
                                </div>
                            </div>
                            <div class="form-row">
                                <a href="javascript:;" class="btn primary-btn fw-btn not-fullwidth" onclick="updatePost()"><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Cập nhật</a>
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn primary-btn fw-btn not-fullwidth" Text="Cập nhật" OnClick="btnSubmit_Click" Style="display: none" />
                                <asp:Literal ID="ltrBack" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="panel panelborderheading">
                        <div class="panel-heading clear">
                            <h3 class="page-title left not-margin-bot">Thông tin</h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-row">
                                <asp:Literal ID="ltrPostInfo" runat="server"></asp:Literal>
                                <a href="javascript:;" class="btn primary-btn fw-btn not-fullwidth" onclick="updatePost()"><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Cập nhật</a>
                                <asp:Literal ID="ltrBack2" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdfUploadGallery" runat="server" />
        <asp:HiddenField ID="hdfDeleteImageGallery" runat="server" />
        <asp:HiddenField ID="hdfAction" runat="server" />
        <asp:HiddenField ID="hdfParentID" runat="server" />
        <asp:HiddenField ID="hdfPostVariants" runat="server" />
        <asp:HiddenField ID="hdfPostId" runat="server" />
        <asp:HiddenField ID="hdfNewVideoId" runat="server" />
        <asp:HiddenField ID="hdfOldVideoId" runat="server" />
    </main>

    <telerik:RadCodeBlock runat="server">
        <script>
            class Variant {
                constructor(Web, Title) {
                    this.Web = Web;
                    this.Title = Title;
                }

                stringJSON() {
                    return JSON.stringify(this);
                }
            }

            var variants = [];

            function initVariants() {

                // Load Variants List
                let obj = JSON.parse($("#<%=hdfPostVariants.ClientID%>").val());
                if ($.isArray(obj))
                {
                    $(".post-variants").removeClass("hide");

                    obj.forEach((item) => {
                        let variant = new Variant(
                            item.Web,
                            item.Title
                        );

                        variants.push(variant);
                        let htmlInput = "<div class='form-row' id='" + item.Web + "'><label>" + item.Web + "</label><input class='form-control' name='" + item.Web + "' value='" + item.Title + "'></div>";
                        $(".post-variant-list").append(htmlInput);
                    });
                }
            }

            function initFocusInput() {
                let queryString = window.location.search;
                let urlParams = new URLSearchParams(queryString);
                let web = urlParams.get('web');
                if (web != "") {
                    $("input[name='" + web + "']").focus().select();
                }
            }

            $(document).ready(function () {

                initVariants();
                initFocusInput();

                var action = $("#<%=hdfAction.ClientID%>").val();
                if (action == "show_web") {
                    $(".input-link").removeClass("hide");
                    $(".input-slug").addClass("hide");
                }
                else if (action == "view_more") {
                    $(".input-slug").removeClass("hide");
                    $(".input-link").addClass("hide");
                }

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

                            let $txtYoutubeUrl = $("#<%=txtYoutubeUrl.ClientID%>");
                            let $rdbActiveVideo = $("#<%=rdbActiveVideo.ClientID%>");
                            let $hdfOldVideoId = $("#<%=hdfOldVideoId.ClientID%>");
                            let $divYoutube = $("#divYoutube");

                            if (xhr.status == 200) {
                                if (response) {
                                    url = "https://www.youtube.com/watch?v=" + response.videoId;

                                    $hdfOldVideoId.val(response.videoId);
                                    $txtYoutubeUrl.val(url);

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

                                    $divYoutube.removeClass('hidden');
                                    $divYoutube.html(iframe);
                                }
                            }
                            else if (xhr.status == 204) {
                                $hdfOldVideoId.val('');
                                $txtYoutubeUrl.text('');

                                $rdbActiveVideo.find("input[value='true']").prop("checked", false);
                                $rdbActiveVideo.find("input[value='false']").prop("checked", false);
                                $divYoutube.addClass('hidden');
                                $divYoutube.html('');
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

            function handlePostVariant() {
                let checkVariant = $(".post-variant-list").html();
                if (checkVariant == "") {
                    return;
                }
                variants.forEach((item) => {
                    let title = $("input[name='" + item.Web + "']").val();
                    item.Title = title;
                });
                $("#<%=hdfPostVariants.ClientID%>").val(JSON.stringify(variants));
            }

            function redirectTo(ID) {
                window.location.href = "/xem-bai-viet-app?id=" + ID;
            }

            var storedFiles = [];

            function isBlank(str) {
                return (!str || /^\s*$/.test(str));
            }

            function changeAction() {
                var action = $("#<%=ddlAction.ClientID%>").val();
                if (action == "show_web") {
                    $(".input-link").removeClass("hide");
                    $(".input-slug").addClass("hide");
                    $(".input-summary").removeClass("hide");
                    $(".input-content").addClass("hide");
                }
                else if (action == "view_more") {
                    $(".input-slug").removeClass("hide");
                    $(".input-link").addClass("hide");
                    $(".input-summary").removeClass("hide");
                    $(".input-content").removeClass("hide");
                }
                else {
                    $(".input-slug").addClass("hide");
                    $(".input-link").addClass("hide");
                    $(".input-summary").addClass("hide");
                    $(".input-content").addClass("hide");
                }

                var slug = $("#<%=txtSlug.ClientID%>").val();
                if (slug == "") {
                    ChangeToSlug();
                }
            }

            function ChangeToSlug() {
                var title, slug;

                //Lấy text từ thẻ input title
                title = $("#<%=txtTitle.ClientID%>").val();

                //Đổi chữ hoa thành chữ thường
                slug = title.toLowerCase();

                //Đổi ký tự có dấu thành không dấu
                slug = slug.replace(/á|à|ả|ạ|ã|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ/gi, 'a');
                slug = slug.replace(/é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ/gi, 'e');
                slug = slug.replace(/i|í|ì|ỉ|ĩ|ị/gi, 'i');
                slug = slug.replace(/ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ/gi, 'o');
                slug = slug.replace(/ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự/gi, 'u');
                slug = slug.replace(/ý|ỳ|ỷ|ỹ|ỵ/gi, 'y');
                slug = slug.replace(/đ/gi, 'd');
                //Xóa các ký tự đặt biệt
                slug = slug.replace(/\`|\~|\!|\@|\#|\||\$|\%|\^|\&|\*|\(|\)|\+|\=|\,|\.|\/|\?|\>|\<|\'|\"|\:|\;|_/gi, '');
                //Đổi khoảng trắng thành ký tự gạch ngang
                slug = slug.replace(/ /gi, "-");
                //Đổi nhiều ký tự gạch ngang liên tiếp thành 1 ký tự gạch ngang
                //Phòng trường hợp người nhập vào quá nhiều ký tự trắng
                slug = slug.replace(/\-\-\-\-\-/gi, '-');
                slug = slug.replace(/\-\-\-\-/gi, '-');
                slug = slug.replace(/\-\-\-/gi, '-');
                slug = slug.replace(/\-\-/gi, '-');
                //Xóa các ký tự gạch ngang ở đầu và cuối
                slug = '@' + slug + '@';
                slug = slug.replace(/\@\-|\-\@|\@/gi, '');
                //In slug ra textbox có id “slug”
                $("#<%=txtSlug.ClientID%>").val(slug);
            }

            function selectCategory(obj) {
                var parentID = obj.val();
                $("#<%=hdfParentID.ClientID%>").val(parentID);
                var lv = parseFloat(obj.attr('data-level'));
                var level = lv + 1;
                $(".slparent").each(function () {
                    var lev = $(this).attr('data-level');
                    if (lv < lev) {
                        $(this).remove();
                    }
                });

                $.ajax({
                    type: "POST",
                    url: "/tao-bai-viet-app.aspx/getParent",
                    data: "{parent:'" + parentID + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = JSON.parse(msg.d);
                        var html = "";
                        //var sl = "";
                        if (data.length > 0) {
                            html += "<select class='form-control slparent' style='margin-top:15px;' data-level='" + level + "' onchange='chooseParent($(this))'>";
                            html += "<option  value='0'>Chọn danh mục</option>";
                            for (var i = 0; i < data.length; i++) {
                                html += "<option value='" + data[i].ID + "'>" + data[i].CategoryName + "</option>";
                            }
                            html += "</select>";
                        }
                        $(".parent").append(html);
                    }
                });
            }

            function showImageGallery(input, obj) {
                if (input.files) {
                    base64 = "";
                    fileSize = 0;
                    var allSizes = "";
                    for (i = 0; i < input.files.length ; i++) {

                        if (!input.files[i].type.match("image.*")) {
                            return;
                        }

                        storedFiles.push(input.files[i]);
                        fileSize += input.files[i].size; // total files size
                        allSizes = allSizes + input.files[i].size + ",";

                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $(".image-gallery").append("<li><img src='" + e.target.result + "' /><a href='javascript:;' data-image-id='' onclick='deleteImageGallery($(this))' class='btn-delete'><i class='fa fa-times' aria-hidden='true'></i> Xóa hình</a></li>")
                            base64 += e.target.result + "|";
                        }
                        reader.readAsDataURL(input.files[i]);
                    }
                    allSizes = allSizes.substring(0, allSizes.length - 1);

                    $("#<%=hdfUploadGallery.ClientID%>").val(allSizes);
                }
            }

            function updatePost() {
                // #region Kiểm tra url Video
                let $youtubeUrl = $("#<%=txtYoutubeUrl.ClientID%>");

                if ($youtubeUrl.val()) {
                    if (!_checkYoutubeUrl($youtubeUrl.val())) {
                        $("#<%=txtYoutubeUrl.ClientID%>").focus();
                        return swal("Thông báo", "Url Youtube không đúng", "error");
                    }
                }
                else {
                    $("#<%=hdfNewVideoId.ClientID%>").val("");
                }
                // #endregion

                var action = $("#<%=ddlAction.ClientID%>").val();
                var category = $("#<%=ddlCategory.ClientID%>").val();
                var title = $("#<%=txtTitle.ClientID%>").val();
                var slug = $("#<%=txtSlug.ClientID%>").val();
                var link = $("#<%=txtLink.ClientID%>").val();

                // tạo slug cho trường hợp chưa nhập
                if (action == "view_more" && slug == "") {
                    ChangeToSlug();
                }

                if (title == "") {
                    $("#<%=txtTitle.ClientID%>").focus();
                    swal("Thông báo", "Chưa nhập tiêu đề bài viết", "error");
                }
                else if (action == "") {
                    $("#<%=ddlAction.ClientID%>").focus();
                    swal("Thông báo", "Chưa chọn kiểu bài viết", "error");
                }
                else if (action == "show_web" && link == "") {
                    $("#<%=txtLink.ClientID%>").focus();
                    swal("Thông báo", "Chưa nhập link", "error");
                }
                else if (category == "0") {
                    $("#<%=ddlCategory.ClientID%>").focus();
                    swal("Thông báo", "Chưa chọn danh mục bài viết", "error");
                }
                else {
                    handlePostVariant();
                    HoldOn.open();
                    $("#<%=btnSubmit.ClientID%>").click();
                }
            }

            function deleteImageGallery(obj) {
                swal({
                    title: "Xác nhận",
                    text: "Cưng có chắc xóa hình này?",
                    type: "warning",
                    showCancelButton: true,
                    closeOnConfirm: true,
                    cancelButtonText: "Đợi em xem tí!",
                    confirmButtonText: "Chắc chắn sếp ơi..",
                }, function (isConfirm) {
                    if (isConfirm) {
                        if (obj.attr("data-image-id") != "") {
                            var deletelist = $("#<%=hdfDeleteImageGallery.ClientID%>").val();
                            $("#<%=hdfDeleteImageGallery.ClientID%>").val(deletelist + obj.attr("data-image-id") + ",");
                        }
                        obj.parent().addClass("hide");
                    }
                });
            }

            function OnClientFileSelected1(sender, args) {
                if ($telerik.isIE) return;
                else {
                    truncateName(args);
                    var file = args.get_fileInputField().files.item(args.get_rowIndex());
                    showThumbnail(file, args);
                    $("#<%= PostPublicThumbnail.ClientID %>").hide();
                }
            }

            function OnClientFileUploadRemoved(sender, args) {
                    $("#<%= PostPublicThumbnail.ClientID %>").show();
            }

            function _checkYoutubeUrl(youtubeUrl) {
                let expression = /^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/gm;
                let regex = new RegExp(expression);

                if (!youtubeUrl.match(regex))
                    return false;

                let url = new URL(youtubeUrl);
                let urlParams = new URLSearchParams(url.search);
                let videoId = urlParams.get('v') || '';

                $("#<%=hdfNewVideoId.ClientID%>").val(videoId);

                return videoId ? true : false;
            }

            function onChangeYoutubeUrl(url) {
                let $divYoutube = $("#divYoutube");

                if (!_checkYoutubeUrl(url)) {
                    $divYoutube.addClass('hidden');
                    $divYoutube.html('');

                    return;
                }

                let videoId = $("#<%=hdfNewVideoId.ClientID%>").val();
                let iframe = '';

                iframe += '<iframe ';
                iframe += '  src="https://www.youtube.com/embed/' + videoId + '" ';
                iframe += '</iframe>';

                $divYoutube.removeClass('hidden');
                $divYoutube.html(iframe);
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
