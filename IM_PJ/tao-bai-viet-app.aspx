<%@ Page Title="Thêm bài viết App" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="tao-bai-viet-app.aspx.cs" Inherits="IM_PJ.tao_bai_viet_app" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .convert-case {
            float: right;
        }
        .variableselect {
            float: left;
            width: 100%;
            clear: both;
            margin: 10px 0;
        }

        .variable-select {
            float: left;
            width: 30%;
            margin-bottom: 10px;
            border: solid 1px #4a4a4a;
            margin-left: 10px;
        }

        .variable-select .variablename {
            float: left;
            width: 100%;
            margin-right: 10px;
            background: blue;
            color: #fff;
            text-align: center;
            padding: 10px 0;
            line-height: 40px;
        }

        .variable-select .variablevalue {
            float: left;
            width: 100%;
            padding: 10px;
        }

        .variable-select .variablevalue .variablevalue-item {
            float: left;
            width: 100%;
            clear: both;
            margin-bottom: 10px;
            border-bottom: solid 1px #ccc;
            padding-bottom: 5px;
        }

        .variable-select .variablevalue .variablevalue-item:last-child {
            border: none;
        }

        .variable-select .variablevalue .variablevalue-item .v-value {
            float: left;
            width: 78%;
            line-height: 40px;
        }

        .variable-select .variablevalue .variablevalue-item .v-delete {
            float: left;
            width: 20%;
        }

        #selectvariabletitle {
            float: left;
            width: 70%;
            clear: both;
            font-weight: bold;
            margin: 20px 0;
            display: none;
        }

        #generateVariable {
            float: right;
            display: block;
        }

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
                            <h3 class="page-title left not-margin-bot">Thêm bài viết App</h3>
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
                                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="Tiêu đề bài viết" autocomplete="off" onkeyup="ChangeToSlug();"></asp:TextBox>
                                    <a href="javascript:;" class="convert-case" onclick="convertCase()"><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Convert</a>
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
                                    <select id="ddlCategory" date-name="parentID" runat="server" class="form-control slparent" data-level="1" onchange="selectCategory($(this))"></select>
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
                                    <telerik:RadAsyncUpload Skin="Metro" runat="server" ID="PostPublicThumbnailImage" ChunkSize="0"
                                        Localization-Select="Chọn ảnh" AllowedFileExtensions=".jpeg,.jpg,.png"
                                        MultipleFileSelection="Disabled" OnClientFileSelected="OnClientFileSelected1" MaxFileInputsCount="1">
                                    </telerik:RadAsyncUpload>
                                    <asp:Image runat="server" ID="PostPublicThumbnail" Width="200" />
                                    <asp:HiddenField runat="server" ID="ListPostPublicThumbnail" ClientIDMode="Static" />
                                    <div class="hidPostPuclicThumbnail"></div>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Youtube
                                </div>
                                <div class="row-right">
                                    <div class="form-row">
                                        <asp:TextBox ID="txtYoutubeUrl" runat="server"  CssClass="form-control" placeholder="Url Youtube của sản phẩm"  onchange="onChangeYoutubeUrl($(this).val())"></asp:TextBox>
                                    </div>
                                    <div class="form-row">
                                        <asp:TextBox ID="txtLinkDownload" runat="server"  CssClass="form-control" placeholder="Link tải sản phẩm"></asp:TextBox>
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
                                    <telerik:RadEditor runat="server" ID="pSummary" Width="100%"
                                        Height="150px" ToolsFile="~/FilesResources/ToolContent.xml" Skin="Metro"
                                        DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd" AutoResizeHeight="False" ContentFilters="MakeUrlsAbsolute">
                                        <ImageManager ViewPaths="~/uploads/images/posts" UploadPaths="~/uploads/images/posts" DeletePaths="~/uploads/images/posts" />
                                    </telerik:RadEditor>
                                </div>
                            </div>
                            <div class="input-content form-row">
                                <div class="row-left">
                                    Nội dung
                                </div>
                                <div class="row-right">
                                    <telerik:RadEditor runat="server" ID="pContent" Width="100%"
                                        Height="700px" ToolsFile="~/FilesResources/ToolContent.xml" Skin="Metro"
                                        DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd" AutoResizeHeight="False" ContentFilters="MakeUrlsAbsolute">
                                        <ImageManager MaxUploadFileSize="710000000" ViewPaths="~/uploads/images/posts" UploadPaths="~/uploads/images/posts" DeletePaths="~/uploads/images/posts" />
                                    </telerik:RadEditor>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Thư viện ảnh
                                </div>
                                <div class="row-right">
                                    <telerik:RadAsyncUpload Skin="Metro" runat="server" ID="ImageGallery" ChunkSize="0"
                                        Localization-Select="Chọn ảnh" AllowedFileExtensions=".jpeg,.jpg,.png"
                                        MultipleFileSelection="Automatic" OnClientFileSelected="OnClientFileSelected1">
                                    </telerik:RadAsyncUpload>
                                    <asp:Image runat="server" ID="imgGallery" Width="200" />
                                    <asp:HiddenField runat="server" ID="listImg" ClientIDMode="Static" />
                                    <div class="hidImage"></div>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Copy vào hệ thống gốc
                                </div>
                                <div class="row-right">
                                    <asp:DropDownList ID="ddlCopyToSystem" AppendDataBoundItems="true" runat="server" class="form-control">
                                        <asp:ListItem Text="Có" Value="True" />
                                        <asp:ListItem Text="Không" Value="False" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Tạo biến thể Wordpress
                                </div>
                                <div class="row row-right">
                                    <div class="col-md-5">
                                        <asp:DropDownList ID="ddlWordpress" AppendDataBoundItems="true" runat="server" class="form-control">
                                        <asp:ListItem Text="Thêm tất cả" Value="all" />
                                        <asp:ListItem Text="ann.com.vn" Value="ann.com.vn" />
                                        <asp:ListItem Text="khohangsiann.com" Value="khohangsiann.com" />
                                        <asp:ListItem Text="bosiquanao.net" Value="bosiquanao.net" />
                                        <asp:ListItem Text="quanaogiaxuong.com" Value="quanaogiaxuong.com" />
                                        <asp:ListItem Text="panpan.vn" Value="panpan.vn" />
                                        <asp:ListItem Text="bansithoitrang.net" Value="bansithoitrang.net" />
                                        <asp:ListItem Text="annshop.vn" Value="annshop.vn" />
                                        <asp:ListItem Text="quanaoxuongmay.com" Value="quanaoxuongmay.com" />
                                        <asp:ListItem Text="nhapsionline.com" Value="nhapsionline.com" />
                                        <asp:ListItem Text="thoitrangann.com" Value="thoitrangann.com" />
                                    </asp:DropDownList>
                                    </div>
                                    <div class="col-md-7">
                                        <a href="javascript:;" class="btn primary-btn fw-btn not-fullwidth" onclick="addClone()"><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Thêm</a>
                                        <a href="javascript:;" class="btn primary-btn fw-btn not-fullwidth copy-title-to-clone hide" onclick="copyTitleToClone()"><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Copy tiêu đề</a>
                                        <a href="javascript:;" class="btn primary-btn fw-btn not-fullwidth copy-title-to-clone hide" onclick="deleteClone()"><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Xóa tất cả</a>
                                    </div>
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
                                <a href="javascript:;" class="btn primary-btn fw-btn not-fullwidth" onclick="addNewPost()"><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Xuất bản</a>
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn primary-btn fw-btn not-fullwidth" Text="Xuất bản" OnClick="btnSubmit_Click" Style="display: none" />
                                <a href="/danh-sach-bai-viet-app" class="btn primary-btn fw-btn not-fullwidth"><i class="fa fa-arrow-left" aria-hidden="true"></i> Trở về</a>
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
                                <a href="javascript:;" class="btn primary-btn fw-btn not-fullwidth" onclick="addNewPost()"><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Xuất bản</a>
                                <a href="/danh-sach-bai-viet-app" class="btn primary-btn fw-btn not-fullwidth"><i class="fa fa-arrow-left" aria-hidden="true"></i> Trở về</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdfParentID" runat="server" />
        <asp:HiddenField ID="hdfPostVariants" runat="server" />
        <asp:HiddenField ID="hdfVideoId" runat="server" />
    </main>

    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript">

            $(document).on("keypress", 'form', function (e) {
                var code = e.keyCode || e.which;
                if (code == 13) {
                    e.preventDefault();
                    return false;
                }
            });

            // FeeModel
            class Variant {
                constructor(Web, Title) {
                    this.Web = Web;
                    this.Web = Web;
                }

                stringJSON() {
                    return JSON.stringify(this);
                }
            }

            var variants = [];

            function addClone() {
                let title = $("#<%=txtTitle.ClientID%>").val();
                if (title == "") {
                    return swal("Thông báo", "Chưa nhập tiêu đề bài viết", "warning");
                }
                let action = $("#<%=ddlAction.ClientID%>").val();
                if (action != "view_more") {
                    return swal("Thông báo", "Bài viết phải là dạng nội bộ", "warning");
                }
                $(".copy-title-to-clone").removeClass("hide");
                $(".post-variants").removeClass("hide");
                let wordpress = $("#<%=ddlWordpress.ClientID%>").val();
                if (wordpress == "all") {
                    $("#<%=ddlWordpress.ClientID%> option").each(function () {
                        let web = $(this).val();
                        let checkWeb = $("input[name='" + web + "']").length;
                        if (web != "all" && !checkWeb) {
                            let htmlInput = "<div class='form-row'><label>" + web + "</label><input class='form-control' name='" + web + "' value='" + title + "'></div>";
                            $(".post-variant-list").append(htmlInput);
                            variants.push(new Variant(web, title));
                        }
                    });
                }
                else {
                    let checkWeb = $("input[name='" + wordpress + "']").length;
                    if (checkWeb) {
                        return swal("Thông báo", "Đã tạo biến thể này rồi", "warning");
                    }

                    let htmlInput = "<div class='form-row'><label>" + wordpress + "</label><input class='form-control' name='" + wordpress + "' value='" + title + "'></div>";
                    $(".post-variant-list").append(htmlInput);
                    variants.push(new Variant(wordpress, title));
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

            function copyTitleToClone() {
                let title = $("#<%=txtTitle.ClientID%>").val();
                if (title == "") {
                    return swal("Thông báo", "Chưa nhập tiêu đề bài viết", "warning");
                }
                let checkVariant = $(".post-variant-list").html();
                if (checkVariant == "") {
                    return swal("Thông báo", "Chưa tạo biến thể bài viết", "warning");
                }
                variants.forEach((item) => {
                    $("input[name='" + item.Web + "']").val(title);
                    item.Title = title;
                });
                $("#<%=hdfPostVariants.ClientID%>").val(JSON.stringify(variants));
            }

            function deleteClone() {
                $(".post-variant-list").html("");
                variants = [];
                $(".post-variants").addClass("hide");
            }

            function changeAction() {
                var action = $("#<%=ddlAction.ClientID%>").val();
                if (action == "show_web") {
                    $(".input-link").removeClass("hide");
                    $(".input-slug").addClass("hide");
                    $(".input-summary").removeClass("hide");
                    $(".input-content").addClass("hide");
                    $(".post-variant-list").html("");
                    variants = [];
                    $(".post-variants").addClass("hide");
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

            function redirectTo(ID) {
                window.location.href = "/xem-bai-viet-app?id=" +ID;
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
                            html += "<select class='form-control slparent' style='margin-top:15px;' data-level=" + level + " onchange='selectCategory($(this))'>";
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

            function imagepreview(input, obj) {
                if (input.files && input.files[0]) {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        obj.parent().find(".imgpreview").attr("src", e.target.result);
                        obj.parent().find(".imgpreview").attr("data-file-name", obj.parent().find("input:file").val());
                        obj.parent().find(".btn-delete").removeClass("hide");
                    }

                    reader.readAsDataURL(input.files[0]);
                }
            }

            function addNewPost() {
                // #region Kiểm tra url video
                let $youtubeUrl = $("#<%=txtYoutubeUrl.ClientID%>");

                if ($youtubeUrl.val()) {
                     if (!_checkYoutubeUrl($youtubeUrl.val())) {
                        $("#<%=txtYoutubeUrl.ClientID%>").focus();
                        return swal("Thông báo", "Url Youtube không đúng<br> Url mẫu: https://www.youtube.com/watch?v={videoId}", "error");
                    }
                }
                else {
                    $("#<%=hdfVideoId.ClientID%>").val("");
                }
                // #endregion

                var action = $("#<%=ddlAction.ClientID%>").val();
                var category = $("#<%=hdfParentID.ClientID%>").val();
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
                else if (category == "") {
                    $("#<%=ddlCategory.ClientID%>").focus();
                    swal("Thông báo", "Chưa chọn danh mục bài viết", "error");
                }
                else {
                    handlePostVariant();
                    HoldOn.open();
                    $("#<%=btnSubmit.ClientID%>").click();
                }
            }

            function _checkYoutubeUrl(youtubeUrl) {
                let expression = /^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/gm;
                let regex = new RegExp(expression);

                if (!youtubeUrl.match(regex))
                    return false;

                let url = new URL(youtubeUrl);
                let urlParams = new URLSearchParams(url.search);
                let videoId = urlParams.get('v') || '';

                $("#<%=hdfVideoId.ClientID%>").val(videoId);

                return videoId ? true : false;
            }

            function onChangeYoutubeUrl(url) {
                let $divYoutube = $("#divYoutube");

                if (!_checkYoutubeUrl(url)) {
                    $divYoutube.addClass('hidden');
                    $divYoutube.html('');

                    return;
                }

                let videoId = $("#<%=hdfVideoId.ClientID%>").val();
                let iframe = '';

                iframe += '<iframe ';
                iframe += '  src="https://www.youtube.com/embed/' + videoId + '" ';
                iframe += '</iframe>';

                $divYoutube.removeClass('hidden');
                $divYoutube.html(iframe);
            }

            function convertCase() {
                let title = $("#<%=txtTitle.ClientID%>").val();
                title = title.charAt(0).toUpperCase() + title.slice(1).toLowerCase();
                $("#<%=txtTitle.ClientID%>").val(title);
            }

            function isBlank(str) {
                return (!str || /^\s*$/.test(str));
            }

            function OnClientFileSelected1(sender, args) {
                if ($telerik.isIE) return;
                else {
                    truncateName(args);
                    var file = args.get_fileInputField().files.item(args.get_rowIndex());
                    //var file = args.get_fileInputField().files.item(0);
                    showThumbnail(file, args);
                }
            }

            function DelRow(that, link) {

                $(that).parent().parent().remove();
                var myHidden = $("#<%= listImg.ClientID %>");
                var tempF = myHidden.value;
                myHidden.value = tempF.replace(link, '');
            }
            (function (global, undefined) {
                var textBox = null;

                function textBoxLoad(sender) {
                    textBox = sender;
                }

                function OpenFileExplorerDialog() {
                    global.radopen("/Dialogs/Dialog.aspx", "ExplorerWindow");
                }

                //This function is called from a code declared on the Explorer.aspx page
                function OnFileSelected(fileSelected) {
                    if (textBox) {
                        {
                            var myHidden = document.getElementById('<%= listImg.ClientID %>');
                            var tempF = myHidden.value;

                            tempF = tempF + '#' + fileSelected;
                            myHidden.value = tempF;

                            $('.hidImage').append('<tr><td><img height="100px" src="' + fileSelected + '"/></td><td style="text-align:center"><a class="btn btn-success" onclick="DelRow(this,\'' + fileSelected + '\')">Xóa</a></td></li>');
                            //alert(fileSelected);
                            textBox.set_value(fileSelected);
                        }
                    }
                }

                global.OpenFileExplorerDialog = OpenFileExplorerDialog;
                global.OnFileSelected = OnFileSelected;
                global.textBoxLoad = textBoxLoad;
            })(window);

        </script>
    </telerik:RadCodeBlock>
</asp:Content>
