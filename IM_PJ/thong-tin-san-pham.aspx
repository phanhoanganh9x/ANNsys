<%@ Page Title="Thông tin sản phẩm" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="thong-tin-san-pham.aspx.cs" Inherits="IM_PJ.thong_tin_san_pham" EnableSessionState="ReadOnly" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="Content/bootstrap-tagsinput.css" />
    <link type="text/css" rel="stylesheet" href="Content/bootstrap-tagsinput-typeahead.css" />
    <link type="text/css" rel="stylesheet" href="Content/typeahead.css" />
    <style>
        .generat-variable-content {
            float: left;
            width: 100%;
            margin: 20px 0;
        }

        .generat-variable-content .item-var-gen {
                float: left;
                width: 100%;
                margin: 15px 0;
                border: dotted 1px #ccc;
                padding: 15px 0;
        }

        .variable-label {
            padding-top: 10px;
            padding-left: 0px;
            padding-right: 0px;
        }

        .variable-removal {
            display: block;
        }

        .variable-removal > a {
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
        .img-product {
            width: 200px;
        }

        .bootstrap-tagsinput {
            width: 100%;
        }

        .bootstrap-tagsinput .label {
            font-size: 100%;
        }

        .bootstrap-tagsinput .twitter-typeahead input {
            margin-top: 5px;
        }

        .bootstrap-tagsinput input {
            width: 100%
        }

        @media (max-width: 769px) {
            .RadUpload .ruInputs li {
                width: 100%;
            }
            .img-product {
                width: 100%;
            }
            ul.image-gallery li {
                width: 100%;
            }
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
                            <h3 class="page-title left not-margin-bot">Sửa sản phẩm</h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-row">
                                <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                            </div>
                            
                            <div class="form-row">
                                <div class="row-left">
                                    Tên sản phẩm
                                    <asp:RequiredFieldValidator ID="rq" runat="server" ControlToValidate="txtProductTitle" ForeColor="Red" SetFocusOnError="true" ErrorMessage="(*)" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="row-right">
                                    <asp:TextBox ID="txtProductTitle" runat="server" CssClass="form-control" placeholder="Tên sản phẩm"></asp:TextBox>
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
                                    Đồng bộ lên KiotViet
                                </div>
                                <div class="row-right">
                                    <asp:RadioButtonList ID="rdbSyncKiotViet" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="true" Selected="True">Có</asp:ListItem>
                                        <asp:ListItem Value="false">Không</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Mã sản phẩm
                                </div>
                                <div class="row-right">
                                    <asp:TextBox ID="txtProductSKU" runat="server" CssClass="form-control sku-input" placeholder="Mã sản phẩm" OnTextChanged="txtProductSKU_Changed" AutoPostBack="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Chất liệu
                                </div>
                                <div class="row-right">
                                    <asp:TextBox ID="txtMaterials" runat="server" CssClass="form-control" placeholder="Chất liệu"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row main-color">
                                <div class="row-left">
                                    Màu chủ đạo
                                </div>
                                <div class="row-right">
                                    <asp:DropDownList ID="ddlColor" runat="server" CssClass="form-control select2">
                                        <asp:ListItem Value="" Text="Chọn màu chủ đạo"></asp:ListItem>
                                        <asp:ListItem Value="cam" Text="Cam"></asp:ListItem>
                                        <asp:ListItem Value="cam tươi" Text="Cam tươi"></asp:ListItem>
                                        <asp:ListItem Value="cam đất" Text="Cam đất"></asp:ListItem>
                                        <asp:ListItem Value="cam sữa" Text="Cam sữa"></asp:ListItem>
                                        <asp:ListItem Value="caro" Text="Caro"></asp:ListItem>
                                        <asp:ListItem Value="da bò" Text="Da bò"></asp:ListItem>
                                        <asp:ListItem Value="đen" Text="Đen"></asp:ListItem>
                                        <asp:ListItem Value="đỏ" Text="Đỏ"></asp:ListItem>
                                        <asp:ListItem Value="đỏ đô" Text="Đỏ đô"></asp:ListItem>
                                        <asp:ListItem Value="đỏ tươi" Text="Đỏ tươi"></asp:ListItem>
                                        <asp:ListItem Value="dưa cải" Text="Dưa cải"></asp:ListItem>
                                        <asp:ListItem Value="gạch tôm" Text="Gạch tôm"></asp:ListItem>
                                        <asp:ListItem Value="hồng" Text="Hồng"></asp:ListItem>
                                        <asp:ListItem Value="hồng cam" Text="Hồng cam"></asp:ListItem>
                                        <asp:ListItem Value="hồng da" Text="Hồng da"></asp:ListItem>
                                        <asp:ListItem Value="hồng dâu" Text="Hồng dâu"></asp:ListItem>
                                        <asp:ListItem Value="hồng phấn" Text="Hồng phấn"></asp:ListItem>
                                        <asp:ListItem Value="hồng ruốc" Text="Hồng ruốc"></asp:ListItem>
                                        <asp:ListItem Value="hồng sen" Text="Hồng sen"></asp:ListItem>
                                        <asp:ListItem Value="kem" Text="Kem"></asp:ListItem>
                                        <asp:ListItem Value="kem tươi" Text="Kem tươi"></asp:ListItem>
                                        <asp:ListItem Value="kem đậm" Text="Kem đậm"></asp:ListItem>
                                        <asp:ListItem Value="kem nhạt" Text="Kem nhạt"></asp:ListItem>
                                        <asp:ListItem Value="nâu" Text="Nâu"></asp:ListItem>
                                        <asp:ListItem Value="nho" Text="Nho"></asp:ListItem>
                                        <asp:ListItem Value="rạch tôm" Text="Rạch tôm"></asp:ListItem>
                                        <asp:ListItem Value="sọc" Text="Sọc"></asp:ListItem>
                                        <asp:ListItem Value="tím" Text="Tím"></asp:ListItem>
                                        <asp:ListItem Value="tím cà" Text="Tím cà"></asp:ListItem>
                                        <asp:ListItem Value="tím đậm" Text="Tím đậm"></asp:ListItem>
                                        <asp:ListItem Value="tím xiêm" Text="Tím xiêm"></asp:ListItem>
                                        <asp:ListItem Value="trắng" Text="Trắng"></asp:ListItem>
                                        <asp:ListItem Value="trắng-đen" Text="Trắng-đen"></asp:ListItem>
                                        <asp:ListItem Value="trắng-đỏ" Text="Trắng-đỏ"></asp:ListItem>
                                        <asp:ListItem Value="trắng-xanh" Text="Trắng-xanh"></asp:ListItem>
                                        <asp:ListItem Value="vàng" Text="Vàng"></asp:ListItem>
                                        <asp:ListItem Value="vàng tươi" Text="Vàng tươi"></asp:ListItem>
                                        <asp:ListItem Value="vàng bò" Text="Vàng bò"></asp:ListItem>
                                        <asp:ListItem Value="vàng nghệ" Text="Vàng nghệ"></asp:ListItem>
                                        <asp:ListItem Value="vàng nhạt" Text="Vàng nhạt"></asp:ListItem>
                                        <asp:ListItem Value="xanh vỏ đậu" Text="Xanh vỏ đậu"></asp:ListItem>
                                        <asp:ListItem Value="xám" Text="Xám"></asp:ListItem>
                                        <asp:ListItem Value="xám chì" Text="Xám chì"></asp:ListItem>
                                        <asp:ListItem Value="xám chuột" Text="Xám chuột"></asp:ListItem>
                                        <asp:ListItem Value="xám nhạt" Text="Xám nhạt"></asp:ListItem>
                                        <asp:ListItem Value="xám tiêu" Text="Xám tiêu"></asp:ListItem>
                                        <asp:ListItem Value="xám xanh" Text="Xám xanh"></asp:ListItem>
                                        <asp:ListItem Value="xanh biển" Text="Xanh biển"></asp:ListItem>
                                        <asp:ListItem Value="xanh biển đậm" Text="Xanh biển đậm"></asp:ListItem>
                                        <asp:ListItem Value="xanh lá chuối" Text="Xanh lá chuối"></asp:ListItem>
                                        <asp:ListItem Value="xanh cổ vịt" Text="Xanh cổ vịt"></asp:ListItem>
                                        <asp:ListItem Value="xanh coban" Text="Xanh coban"></asp:ListItem>
                                        <asp:ListItem Value="xanh da" Text="Xanh da"></asp:ListItem>
                                        <asp:ListItem Value="xanh dạ quang" Text="Xanh dạ quang"></asp:ListItem>
                                        <asp:ListItem Value="xanh đen" Text="Xanh đen"></asp:ListItem>
                                        <asp:ListItem Value="xanh jean" Text="Xanh jean"></asp:ListItem>
                                        <asp:ListItem Value="xanh lá" Text="Xanh lá"></asp:ListItem>
                                        <asp:ListItem Value="xanh lá mạ" Text="Xanh lá mạ"></asp:ListItem>
                                        <asp:ListItem Value="xanh lính" Text="Xanh lính"></asp:ListItem>
                                        <asp:ListItem Value="xanh lông công" Text="Xanh lông công"></asp:ListItem>
                                        <asp:ListItem Value="xanh môn" Text="Xanh môn"></asp:ListItem>
                                        <asp:ListItem Value="xanh ngọc" Text="Xanh ngọc"></asp:ListItem>
                                        <asp:ListItem Value="xanh rêu" Text="Xanh rêu"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Loại hàng
                                </div>
                                <div class="row-right">
                                    <asp:DropDownList ID="ddlPreOrder" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Hàng có sẵn" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Hàng order" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Tồn kho ít nhất
                                </div>
                                <div class="row-right">
                                    <asp:TextBox type="number" min="0" ID="pMinimumInventoryLevel" runat="server" CssClass="form-control" placeholder="Số lượng tồn kho ít nhất"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Tồn kho nhiều nhất
                                </div>
                                <div class="row-right">
                                    <asp:TextBox type="number" min="0" ID="pMaximumInventoryLevel" runat="server" CssClass="form-control" placeholder="Số lượng tồn kho nhiều nhất"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Nhà cung cấp
                                </div>
                                <div class="row-right">
                                    <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Giá cũ chưa sale
                                </div>
                                <div class="row-right">
                                    <asp:TextBox type="number" min="0" autocomplete="off" ID="pOld_Price" runat="server" CssClass="form-control" placeholder="Giá sỉ cũ chưa sale"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Giá sỉ
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="pRegular_Price" ForeColor="Red" ErrorMessage="(*)" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="row-right">
                                    <asp:TextBox type="number" min="0" autocomplete="off" ID="pRegular_Price" runat="server" CssClass="form-control" placeholder="Giá sỉ"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row cost-of-goods">
                                <div class="row-left">
                                    Giá vốn
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="pCostOfGood" ForeColor="Red" ErrorMessage="(*)" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="row-right">
                                    <asp:TextBox type="number" min="0" autocomplete="off" ID="pCostOfGood" runat="server" CssClass="form-control cost-price" placeholder="Giá vốn"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Giá lẻ
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="pRetailPrice" ForeColor="Red" ErrorMessage="(*)" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="row-right">
                                    <asp:TextBox type="number" min="0" autocomplete="off" ID="pRetailPrice" runat="server" CssClass="form-control" placeholder="Giá lẻ"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Ảnh đại diện
                                </div>
                                <div class="row-right">
                                    <telerik:RadAsyncUpload Skin="Metro" runat="server" ID="uploadProductImage" ChunkSize="0"
                                        Localization-Select="Chọn ảnh" AllowedFileExtensions=".jpeg,.jpg,.png"
                                        MultipleFileSelection="Disabled" OnClientFileSelected="OnClientFileSelected1" 
                                        MaxFileInputsCount="1" OnClientFileUploadRemoved="OnClientFileUploadRemoved1">
                                    </telerik:RadAsyncUpload>
                                    <asp:Image runat="server" ID="imgProductImage" class="img-product" />
                                    <asp:HiddenField runat="server" ID="hdfProductImage" ClientIDMode="Static" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Tags
                                </div>
                                <div class="row-right">
                                    <input type="text" id="txtTag" class="typeahead" data-role="tagsinput" />
                                    <div id="tagList"></div>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Nội dung
                                </div>
                                <div class="row-right">
                                    <telerik:RadEditor runat="server" ID="pContent" Width="100%" Height="600px" ToolsFile="~/FilesResources/ToolContent.xml" Skin="Metro" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd" AutoResizeHeight="False">
                                        <ImageManager ViewPaths="~/uploads/images" UploadPaths="~/uploads/images" DeletePaths="~/uploads/images" />
                                    </telerik:RadEditor>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Thư viện ảnh
                                </div>
                                <div class="row-right">
                                    <asp:FileUpload runat="server" ID="uploadImageGallery" name="uploadImageGallery" onchange='showImageGallery(this,$(this));' AllowMultiple="true" />  
                                    
                                    <asp:Literal ID="imageGallery" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="row-left">
                                    Ảnh đại diện sạch
                                </div>
                                <div class="row-right">
                                    <telerik:RadAsyncUpload Skin="Metro" runat="server" ID="uploadProductImageClean" ChunkSize="0"
                                        Localization-Select="Chọn ảnh" AllowedFileExtensions=".jpeg,.jpg,.png"
                                        MultipleFileSelection="Disabled" OnClientFileSelected="OnClientFileSelected2" 
                                        MaxFileInputsCount="1"  OnClientFileUploadRemoved="OnClientFileUploadRemoved2">
                                    </telerik:RadAsyncUpload>
                                    <asp:Image runat="server" ID="imgProductImageClean" Width="200" />
                                    <asp:HiddenField runat="server" ID="hdfProductImageClean" ClientIDMode="Static" />
                                </div>
                            </div>
                            <div class="form-row variable">
                                <div class="row-left">
                                    Biến thể
                                </div>
                                <div class="row-right">
                                    <div class="generat-variable-content">
                                        <div class="row">
                                            <div class="col-md-9">
                                                <h3>Danh sách biến thể</h3>
                                            </div>
                                            <div class="col-md-3">
                                                <a href="javascript:;" onclick="addVariant()" id="delete" class="btn primary-btn fw-btn not-fullwidth"><i class="fa fa-file-o" aria-hidden="true"></i> Thêm biến thể</a>
                                            </div>
                                        </div>
                                        <div class="row list-item-genred">
                                            <div class="col-md-12">
                                                <asp:Literal ID="ltrVariables" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-row">
                                <a href="javascript:;" class="btn primary-btn fw-btn not-fullwidth" onclick="updateProduct()"><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Cập nhật</a>
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
                                <asp:Literal ID="ltrProductInfo" runat="server"></asp:Literal>
                                <a href="javascript:;" class="btn primary-btn fw-btn not-fullwidth" onclick="updateProduct()"><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Cập nhật</a>
                                <asp:Literal ID="ltrBack2" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdfTempVariable" runat="server" />
        <asp:HiddenField ID="hdfUploadGallery" runat="server" />
        <asp:HiddenField ID="hdfDeleteImageGallery" runat="server" />
        <asp:HiddenField ID="hdfsetStyle" runat="server" />
        <asp:HiddenField ID="hdfVariableFull" runat="server" />
        <asp:HiddenField ID="hdfVariationRemovalList" runat="server" />
        <asp:HiddenField ID="hdfVariableListInsert" runat="server" />
        <asp:HiddenField ID="hdfParentID" runat="server" />
        <asp:HiddenField ID="hdfUserRole" runat="server" />
        <asp:HiddenField ID="hdfTags" runat="server" />
        <asp:FileUpload runat="server" ID="uploadVariationImage" name="uploadVariationImage" AllowMultiple="true" style="display: none"/>
    </main>

    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript" src="Scripts/bootstrap-tagsinput.min.js"></script>
        <script type="text/javascript" src="Scripts/typeahead.bundle.min.js"></script>
        <script type="text/javascript" src="Scripts/typeahead.jquery.js"></script>
        <script>
            let _productID = 0;
            let _variation = [];
            let _variationSKURemoval = [];
            let _variationImageFiles = [];

            // init Input Tag
            let tags = new Bloodhound({
                datumTokenizer: (tag) => {
                    return Bloodhound.tokenizers.whitespace(tag.name);
                },
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                remote: {
                    url: '/thong-tin-san-pham.aspx/GetTags?tagName="%QUERY"',
                    filter: (response) => {
                        if (!response.d)
                            return {};

                        return $.map(response.d, function (item) {
                            return {
                                id: item.id,
                                name: item.name,
                                slug: item.slug,
                            };
                        });
                    },
                    ajax: {
                        type: "GET",
                        contentType: "application/json; charset=utf-8"
                    }
                }
            });
            tags.initialize();

            let txtTagDOM = $('#txtTag');
            txtTagDOM.tagsinput({
                itemValue: 'slug',
                itemText: 'name',
                trimValue: true,
                typeaheadjs: {
                    name: 'tags',
                    displayKey: 'name',
                    source: tags.ttAdapter()
                }
            });

            var storedFiles = [];

            $(document).ready(function () {
                let queryParams = new URLSearchParams(window.location.search);
                _productID = queryParams.get('id') || 0;

                var userRole = $("#<%=hdfUserRole.ClientID%>").val();
                if (userRole != "0") {
                    $(".cost-of-goods").addClass("hide");
                }

                var productStyle = $("#<%=hdfsetStyle.ClientID%>").val();
                if (productStyle == 1) {
                    $(".variable").addClass("hide");
                }

                let hdfTags = $("#<%=hdfTags.ClientID%>").val();
                let tags = hdfTags ? (JSON.parse(hdfTags) || []) : [];

                tags.forEach((item) => txtTagDOM.tagsinput('add', { id: item.id, name: item.name, slug: item.slug }));


                txtTagDOM.on('beforeItemAdd', function (event) {
                    // event.item: contains the item
                    // event.cancel: set to true to prevent the item getting added
                    let items = txtTagDOM.tagsinput('items') || [];
                    let exist = false;

                    items.forEach((tag) => {
                        if (tag.name === event.item.name) {
                            exist = true;
                            return false;
                        }
                    })

                    if (exist)
                        event.cancel = true;
                });

                $(".bootstrap-tagsinput").find(".tt-input").keypress((event) => {
                    if (event.which === 13 || event.which === 44) {
                        if (event.which === 13)
                            event.preventDefault();

                        let target = event.target;
                        let tagName = target.value || "";

                        if (!tagName)
                            return;

                        let tagNameList = tagName.split(',');
                        let url = "";

                        if (tagNameList.length > 1) {
                            url += "/thong-tin-san-pham.aspx/GetTagListByNameList?";
                            url += `&tagNameList=${JSON.stringify(tagNameList)}`;
                        }
                        else {
                            url += `/thong-tin-san-pham.aspx/GetTags?tagName=${JSON.stringify(tagNameList[0])}`;
                        }

                        $.ajax({
                            headers: {
                                Accept: "application/json, text/javascript, */*; q=0.01",
                                "Content-Type": "application/json; charset=utf-8"
                            },
                            type: "GET",
                            url: url,
                            success: function (response) {
                                let data = response.d || [];

                                data.forEach((item) => {
                                    txtTagDOM.tagsinput('add', { id: item.id, name: item.name, slug: item.slug })
                                })

                                target.value = "";
                            }
                        })
                    }
                });

                var categoryID = $("#<%=ddlCategory.ClientID%>").val();
                if (categoryID != "0") {
                    getTagList(categoryID);
                }
                
                // Variation
                _initVariation();
            });

            function _initVariation() {
                _initDropDownListVariableValue();
                _initVariationRegularPrice();
                _initVariationCostOfGood();
                _initVariationRetailPrice();
            }

            function _initDropDownListVariableValue() {
                let $ddlVariableValue = $("select[name='ddlVariableValue']");
                
                $.each($ddlVariableValue, function() {
                    $(this).attr("data-pre", $(this).val())
                });
            }

            function _initVariationRegularPrice() {
                $(".item-var-gen").find(".regularprice").blur(function () {
                    let $price = $(this);

                    if (!$price.val())
                        return swal({
                            title: "Thông báo",
                            text: "Chưa nhập giá sỉ",
                            type: "error"
                        }, function () {
                            $price.focus();
                            $price.select();
                        });
                    else
                        _checkVariationInputPrice($price);
                });
            }

            function _initVariationCostOfGood() {
                $(".item-var-gen").find(".costofgood").blur(function () {
                    let $price = $(this);

                    if (!$price.val())
                        return swal({
                            title: "Thông báo",
                            text: "Chưa nhập giá vốn",
                            type: "error"
                        }, function () {
                            $price.focus();
                            $price.select();
                        });
                    else
                        _checkVariationInputPrice($price);
                });
            }

            function _initVariationRetailPrice() {
                $(".item-var-gen").find(".retailprice").blur(function () {
                    let $price = $(this);

                    if (!$price.val())
                        return swal({
                            title: "Thông báo",
                            text: "Chưa nhập giá lẻ",
                            type: "error"
                        }, function () {
                            $price.focus();
                            $price.select();
                        });
                    else
                        _checkVariationInputPrice($price);
                });
            }

            function _checkVariationInputPrice($price) {
                let $variation = $price.closest(".item-var-gen");

                // Kiểm tra về giá sỉ
                let $regularPrice = $variation.find(".regularprice");
                let giasi = $regularPrice.val() || "";

                // Kiểm tra về giá vốn
                let $costOfGood = $variation.find(".costofgood");
                let giavon = $costOfGood.val() || "";

                // Kiểm tra về giá lẻ
                let $retailPrice = $variation.find(".retailprice");
                let giale = $retailPrice.val() || "";

                if (parseFloat(giasi) < parseFloat(giavon)) {
                    return swal({
                        title: "Thông báo",
                        text: "Gía sỉ không được thấp hơn giá vốn",
                        type: "error"
                    }, function () {
                        $regularPrice.focus();
                        $regularPrice.select();
                    });
                }
                else if (parseFloat(giasi) > parseFloat(giale)) {
                    return swal({
                        title: "Thông báo",
                        text: "Giá lẻ không được thấp hơn giá sỉ",
                        type: "error"
                    }, function () {
                        $retailPrice.focus();
                        $retailPrice.select();
                    });
                }
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
                })

                getKvCategory(+parentID || 0);

                $.ajax({
                    type: "POST",
                    url: "/tao-san-pham.aspx/getParent",
                    data: "{parent:'" + parentID + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = JSON.parse(msg.d);
                        var html = "";
                        //var sl = "";
                        if (data.length > 0) {
                            html += "<select class='form-control slparent' style='margin-top:15px;' data-level='" + level + "' onchange='selectCategory($(this))'>";
                            html += "<option  value='0'>Chọn danh mục</option>";
                            for (var i = 0; i < data.length; i++) {
                                html += "<option value='" + data[i].ID + "'>" + data[i].CategoryName + "</option>";
                            }
                            html += "</select>";
                        }
                        $(".parent").append(html);
                    }
                });

                getTagList(parentID);
            }

            // Cập nhật lại tất cả SKU của biến thể sau khi thay đổi SKU cha
            function updateVariationSKUA(skuOld, skuNew) {
                $(".item-var-gen").each(function () {
                    let $variationLabel = $(this).find(".variable-label");
                    let htmlTitle = $variationLabel.html().trim();

                    htmlTitle = htmlTitle.replace(skuOld, skuNew);
                    $variationLabel.html(htmlTitle);

                    let $variationSKU = $(this).find(".productvariablesku");
                    let variationSKU = $variationSKU.val();

                    _variationSKURemoval.push(variationSKU);
                    variationSKU = variationSKU.replace(skuOld, skuNew);
                    $variationSKU.val(variationSKU).trigger("change");
                })
            }

            function getTagList(categoryID) {
                // clear old value
                $("#tagList").html("");

                var search = "";
                if (categoryID == 18) {
                    search = "đồ bộ";
                }
                else if (categoryID == 17) {
                    search = "đầm";
                }
                else if (categoryID == 3 || categoryID == 4 || categoryID == 5 || categoryID == 6) {
                    search = "áo thun nam";
                }
                else if (categoryID == 19) {
                    search = "áo thun nữ";
                }

                if (search != "") {
                    var url = `/tao-san-pham.aspx/GetTagList?tagName=${JSON.stringify(search)}`;

                    $.ajax({
                        headers: {
                            Accept: "application/json, text/javascript, */*; q=0.01",
                            "Content-Type": "application/json; charset=utf-8"
                        },
                        type: "GET",
                        url: url,
                        success: function (response) {
                            let data = response.d || [];

                            data.forEach((item) => {
                                $("#tagList").append("<span onclick='clickTagList(`" + item.name + "`)' class='tag-blue-click'>" + item.name + "</span>");
                            })
                        }
                    });
                }
            }

            function clickTagList(tagName) {

                let url = `/tao-san-pham.aspx/GetTags?tagName=${JSON.stringify(tagName)}`;

                $.ajax({
                    headers: {
                        Accept: "application/json, text/javascript, */*; q=0.01",
                        "Content-Type": "application/json; charset=utf-8"
                    },
                    type: "GET",
                    url: url,
                    success: function (response) {
                        let data = response.d || [];

                        data.forEach((item) => {
                            txtTagDOM.tagsinput('add', { id: item.id, name: item.name, slug: item.slug })
                        })
                    }
                })
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
                            $(".image-gallery").append("<li><img src='" + e.target.result + "' /><a href='javascript:;' data-image-id='' onclick='deleteImageGallery($(this))' class='btn-delete'><i class=\"fa fa-times\" aria-hidden=\"true\"></i> Xóa hình</a></li>")
                            base64 += e.target.result + "|";
                        }
                        reader.readAsDataURL(input.files[i]);
                    }
                    allSizes = allSizes.substring(0, allSizes.length - 1);

                    $("#<%=hdfUploadGallery.ClientID%>").val(allSizes);
                }
            }

            function addVariant() {
                $(".item-var-gen:eq('0')").clone().prependTo(".list-item-genred > .col-md-12");

                let parentSKU = $("#<%=txtProductSKU.ClientID%>").val();
                let $variation = $(".item-var-gen");
                let $variationNew = $variation.first();
                let $ddlVariationValue = $variationNew.find("select[name='ddlVariableValue']");

                // Cài đặt màu background default cho div biến thể
                $variationNew.css("background-color", "#fff");
                $variationNew.attr("data-index", $variation.length);
                $variationNew.attr("data-product-variation-id", 0);
                // Thể hiện chi tiết biến thể
                $variationNew.find(".variable-content").show();
                // Image
                $variationNew.find(".productVariableImage").val("");
                $variationNew.find(".imgpreview")
                    .attr("src", "/App_Themes/Ann/image/placeholder.png")
                    .attr("data-file-name", "/App_Themes/Ann/image/placeholder.png");
                $variationNew.find(".btn-delete").addClass("hide");
                // Drop down list biến thể
                $.each($ddlVariationValue, function () {
                    $(this).parent().find(".select2-container--default").remove();
                    $(this).attr("data-pre", "");
                    $(this).select2().val("").trigger("change");
                });
                // Input Variation SKU
                $variationNew.find(".productvariablesku").val(parentSKU);
            }

            function showVariableContent($self) {
                let $variation = $self.closest(".item-var-gen");
                let $title = $self.parent();
                let $content = $variation.find(".variable-content");
                
                if ($content.is(":hidden")) {
                    $title.addClass("margin-bottom-15");
                    $content.show();
                }
                else {
                    $title.removeClass("margin-bottom-15");
                    $content.hide();
                }
            }

            function deleteVariableItem($self) {
                if ($(".item-var-gen").length == 1)
                    return swal({
                        title: "Thông báo",
                        text: "Đây là sản phẩm biến thể.<br> Bạn không thể xóa hết các biển thể được.",
                        type: "error",
                        html: true
                    });

                let $variation = $self.closest(".item-var-gen");
                let variationSKU = $variation.find(".productvariablesku").val() || "";

                return swal({
                    title: "Thông báo",
                    text: "Bạn muốn xóa biến thể này?",
                    showCancelButton: true,
                    closeOnConfirm: true,
                    cancelButtonText: "Đợi em xem tí!",
                    confirmButtonText: "Chắc chắn sếp ơi..",
                }, function (isConfirm) {
                    if (isConfirm) {
                        _variationSKURemoval.push(variationSKU)
                        $variation.remove();

                        _updateVariationIndex();
                    }
                });
            }

            function _updateVariationIndex() {
                let $variation = $(".item-var-gen");
                let numberVariation = $variation.length;

                $.each($variation, function (index) {
                    $(this).attr("data-index", numberVariation - index);
                    _updateVariationTitle($(this));
                });
            }

            function onChangeVariationImage(input, $self) {
                let file = input.files[0];

                if (file) {
                    let fileName = file.name;
                    let reader = new FileReader();

                    reader.onload = function (e) {
                        let $img = $self.parent().find(".imgpreview");
                        let $btnDelete = $self.parent().find(".btn-delete");

                        // img tag
                        $img.attr("src", e.target.result);
                        $img.attr("data-file-name", "/uploads/images/" + _productID + "-" + fileName);
                        $img.attr("data-changed", true);
                        // a tag
                        $btnDelete.removeClass("hide");
                    }

                    reader.readAsDataURL(file);
                    _variationImageFiles.push(file)
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

            function onChangeVariationValue($select) {
                let preData = $select.attr("data-pre") || "";

                // Trường hợp là biến thể củ
                if (!$select.val() && preData)
                    return swal({
                        title: "Thông báo",
                        text: "Vui lòng chọn giá trị biến thể",
                        type: "error"
                    }, function () {
                        $select.val(preData).trigger("change");
                        $select.select2("open");
                    });

                let $parent = $select.closest(".item-var-gen");
                let variationID = "";
                let variationValueID = "";
                let variationName = "";
                let variationValue = "";
                let variation = "";
                let variationSKUOld = $parent.find(".productvariablesku").val() || "";
                let variableSKU = $("#<%=txtProductSKU.ClientID%>").val().trim().toUpperCase();

                $parent.find("select option:selected").each(function () {
                    let $option = $(this);

                    if ($option.val()) {
                        variationID += $option.parent().attr("data-name-id") + "|";
                        variationValueID += $option.val() + "|";
                        variationName += $option.parent().attr("data-name-text") + "|";
                        variationValue += $option.text() + "|";
                        variation += $option.parent().attr("data-name-id") + ":" + $option.val() + "|";
                        variableSKU += $option.attr("data-sku-text");
                    }
                });

                if (!_checkDuplicateVariationSKU(variableSKU)) {
                    $parent.attr("data-name-id", variationID);
                    $parent.attr("data-value-id", variationValueID);
                    $parent.attr("data-name-text", variationName);
                    $parent.attr("data-value-text", variationValue);
                    $parent.attr("data-name-value", variation);
                    $parent.find(".productVariableImage").attr("name", variableSKU);
                    $parent.find(".productvariablesku").val(variableSKU);
                    $select.attr("data-pre", $select.val());

                    _updateVariationTitle($parent);

                    // Cập nhật lại biến thể bị xóa
                    if ($select.val()) {
                        if (preData)
                            _variationSKURemoval.push(variationSKUOld);
                        _variationSKURemoval = _variationSKURemoval.filter(sku => sku != variableSKU);
                    }
                }
                else {
                    let message = "Biến thể " + $select.data("name-text") + " : " + $select.children("option:selected").text() + " đã tồn tại.";

                    return swal({
                        title: "Thông báo",
                        text: message,
                        type: "error"
                    }, function () {
                        $select.val(preData).trigger("change");
                        $select.select2("open");
                    });
                }
            }

            function _checkDuplicateVariationSKU(value) {
                let $variation = $(".item-var-gen");
                let duplicated = false;

                $variation.each(function () {
                    let variationSKU = $(this).find(".productvariablesku").val() || "";

                    if (variationSKU == value) {
                        duplicated = true;
                        return false;
                    }
                });

                return duplicated;
            }

            function _updateVariationTitle($self) {
                let variationIndex = $self.attr("data-index") || "";
                let variationName = $self.attr("data-name-text") || "";
                let variationValue = $self.attr("data-value-text") || "";
                let variationSKU = $self.find(".productvariablesku").val() || "";
                let title = "<strong>#" + variationIndex + "</strong> - ";

                variationName = variationName.split('|').filter(item => item != "");
                variationValue = variationValue.split('|').filter(item => item != "");

                $.each(variationName, function (index, item) {
                    title += variationName[index] + ": " + variationValue[index] + " - "
                });

                title += variationSKU

                $self.find(".variable-label").html(title);
            }


            function updateProduct() {
                HoldOn.open();

                let productStyle = +$("#<%=hdfsetStyle.ClientID%>").val() || 1;

                // Sản phẩm có biến thể
                if (productStyle == 2) {
                    if (!_checkProductVariation())
                        return;

                    let variations = []

                    $(".item-var-gen").each(function () {
                        variations.push({
                            productVariationID: $(this).attr("data-product-variation-id") || 0,
                            variationID: $(this).attr("data-name-id") || "",
                            variationValueID: $(this).attr("data-value-id") || "",
                            variationName: $(this).attr("data-name-text") || "",
                            variationValueName: $(this).attr("data-value-text") || "",
                            sku: $(this).find(".productvariablesku").val() || "",
                            regularPrice: +$(this).find(".regularprice").val() || 0,
                            costOfGood: +$(this).find(".costofgood").val() || 0,
                            retailPrice: +$(this).find(".retailprice").val() || 0,
                            variationValue: $(this).attr("data-name-value") || "",
                            maximumInventoryLevel: +$("#<%=pMaximumInventoryLevel.ClientID%>").val() || 0,
                            minimumInventoryLevel: +$("#<%=pMinimumInventoryLevel.ClientID%>").val() || 0,
                            stockStatus: 3,
                            checked: true,
                            image: $(this).find(".imgpreview").attr("data-file-name") || ""
                        });
                    });

                    if (variations.length != 0) {
                        // Variation removal
                        $("#<%=hdfVariationRemovalList.ClientID%>").val(JSON.stringify(_variationSKURemoval));
                        // Variation update and insert
                        $("#<%=hdfVariableListInsert.ClientID%>").val(JSON.stringify(variations.reverse()));
                        // Tags
                        $("#<%=hdfTags.ClientID%>").val(JSON.stringify(txtTagDOM.tagsinput('items')));
                        // Upload Variation Image
                        let $uploadVariationImage = $("#<%= uploadVariationImage.ClientID %>").get(0);
                        $uploadVariationImage.files = new FileListItem(_variationImageFiles);

                        $("#<%=btnSubmit.ClientID%>").click();
                    }
                    else
                    {
                        HoldOn.close();

                        return swal({
                            title: "Lỗi",
                            text: "Hãy kiểm tra thông tin biến thể",
                            type: "error"
                        });
                    }
                }
                else {
                    if (!_checkProduct())
                        return;

                    // Tags
                    $("#<%=hdfTags.ClientID%>").val(JSON.stringify(txtTagDOM.tagsinput('items')));
                    $("#<%=btnSubmit.ClientID%>").click();
                }
            }

            function _checkValidation() {
                // Kiểm tra tên sản phẩm
                let title = $("#<%=txtProductTitle.ClientID%>").val() || "";

                if (title == "") {
                    HoldOn.close();
                    $("#<%=txtProductTitle.ClientID%>").focus();

                    swal({
                        title: "Thông báo",
                        text: "Chưa nhập tên sản phẩm",
                        type: "error"
                    });

                    return false;
                }

                // Kiểm tra mã sản phẩm
                let SKU = $("#<%=txtProductSKU.ClientID%>").val() || "";

                if (SKU == "") {
                    HoldOn.close();
                    $("#<%=txtProductSKU.ClientID%>").focus();

                    swal({
                        title: "Thông báo",
                        text: "Chưa nhập mã sản phẩm",
                        type: "error"
                    });

                    return false;
                }

                // Kiểm tra chất liệu sản phẩm
                let materials = $("#<%=txtMaterials.ClientID%>").val() || "";

                if (materials == "") {
                    HoldOn.close();
                    $("#<%=txtMaterials.ClientID%>").focus();

                    swal({
                        title: "Thông báo",
                        text: "Chưa nhập chất liệu sản phẩm",
                        type: "error"
                    });

                    return false;
                }

                // Kiểm tra về giá sỉ
                let giasi = $("#<%=pRegular_Price.ClientID%>").val() || "";
                    
                if (giasi == "") {
                    HoldOn.close();
                    $("#<%=pRegular_Price.ClientID%>").focus();

                    swal({
                        title: "Thông báo",
                        text: "Chưa nhập giá sỉ",
                        type: "error"
                    });

                    return false;
                }

                // Kiểm tra về giá vốn
                let giavon = $("#<%=pCostOfGood.ClientID%>").val() || "";

                if (giavon == "") {
                    HoldOn.close();
                    $("#<%=pCostOfGood.ClientID%>").focus();

                    swal({
                        title: "Thông báo",
                        text: "Chưa nhập giá vốn",
                        type: "error"
                    });

                    return false;
                }

                // Kiểm tra về giá lẻ
                let giale = $("#<%=pRetailPrice.ClientID%>").val() || "";

                if (giale == "") {
                    HoldOn.close();
                    $("#<%=pRetailPrice.ClientID%>").focus();


                    swal({
                        title: "Thông báo",
                        text: "Chưa nhập giá lẻ",
                        type: "error"
                    });

                    return false;
                }

                // Kiểm tra logic về giá
                let giacu = $("#<%=pOld_Price.ClientID%>").val() || 0;
                    
                if (parseFloat(giasi) < parseFloat(giavon)) {
                    HoldOn.close();
                    $("#<%=pRegular_Price.ClientID%>").focus();

                    swal({
                        title: "Thông báo",
                        text: "Gía sỉ không được thấp hơn giá vốn",
                        type: "error"
                    });

                    return false;
                }
                else if (giacu > 0 && giacu < parseFloat(giasi)) {
                    HoldOn.close();
                    $("#<%=pOld_Price.ClientID%>").focus();

                    swal({
                        title: "Thông báo",
                        text: "Giá cũ chưa sale không được thấp hơn giá sỉ",
                        type: "error"
                    });

                    return false;
                }
                else if (parseFloat(giasi) > parseFloat(giale)) {
                    HoldOn.close();
                    $("#<%=pRetailPrice.ClientID%>").focus();

                    swal({
                        title: "Thông báo",
                        text: "Giá lẻ không được thấp hơn giá sỉ",
                        type: "error"
                    });

                    return false;
                }

                return true;
            }

            function _checkProduct() {
                if (!_checkValidation())
                    return false;

                // Kiểm tra màu chủ đạo
                let maincolor = $("#<%=ddlColor.ClientID%>").val() || "";

                if (maincolor == "") {
                    HoldOn.close();
                    $("#<%=ddlColor.ClientID%>").focus();

                    swal({
                        title: "Thông báo",
                        text: "Chưa chọn màu chủ đạo",
                        type: "error"
                    });

                    return false;
                }

                return true;
            }

            function _checkProductVariation() {
                if (!_checkValidation())
                    return false;

                // Kiểm tra danh mục sản phẩm
                let categoryID = +$("#<%=hdfParentID.ClientID%>").val() || 0;

                if (categoryID == 0) {
                    HoldOn.close();
                    $("#<%=ddlCategory.ClientID%>").focus();

                    swal({
                        title: "Thông báo",
                        text: "Chưa chọn danh mục sản phẩm",
                        type: "error"
                    });

                    return false;
                }

                // Kiểm tra các biến thể generate
                let $variation = $(".item-var-gen");

                if ($variation.length == 0) {
                    HoldOn.close();

                    swal({
                        title: "Lỗi",
                        text: "Chưa thiếp lập biến thể sản phẩm",
                        type: "error"
                    });

                    return false;
                }

                let errorPosition = null;
                let arraySKU = [];

                $.each($variation, function (index, value) {
                    let sku = $(this).find(".productvariablesku").val() || "";
                    let regularPrice = $(this).find(".regularprice").val() || "";
                    let costOfGood = $(this).find(".costofgood").val();
                    let retailPrice = $(this).find(".retailprice").val();
                    let selectedVariation = true;

                    $(this).find("select").each(function () {
                        let variation = $(this).val() || "";

                        if ($(this).val() == "") {
                            selectedVariation = false;
                            return false;
                        }
                    });

                    // check null value
                    if (!selectedVariation || isBlank(sku) || isBlank(regularPrice) || isBlank(costOfGood) || isBlank(retailPrice)) {
                        errorPosition = errorPosition == null ? $(this).offset().top : errorPosition;
                        $(this).css("background-color", "#fff0c5");
                    }
                    else {
                        arraySKU.push(sku);
                        $(this).css("background-color", "#fff");
                    }
                });

                if (errorPosition != null) {
                    HoldOn.close();

                    swal({
                        title: "Thông báo",
                        text: "Hãy nhập đầy đủ thông tin các biến thể!",
                        type: "warning",
                        showCancelButton: false,
                        closeOnConfirm: true,
                        confirmButtonText: "Để em xem lại...",
                    }, function () {
                        $('html, body').animate({
                            scrollTop: errorPosition - 150
                        }, 500);
                    });

                    return false;
                }

                return true;
            }

            function openUploadImage(obj) {
                obj.parent().find(".productVariableImage").click();
            }

            function deleteImageVariable(obj) {
                let $productVariableImage = obj.parent().find(".productVariableImage").get(0);
                let $imgpreview = obj.parent().find(".imgpreview").get(0);
                let hasChanged = false;
                let imageName = "";

                // input[type='file'] tag
                if ($productVariableImage)
                    $productVariableImage.value = "";
                // img tag
                hasChanged = ($imgpreview.dataset["changed"] || false) == "true";
                if (hasChanged)
                    imageName = $imgpreview.dataset["fileName"];
                $imgpreview.setAttribute("src", "/App_Themes/Ann/image/placeholder.png");
                $imgpreview.setAttribute("data-file-name", "/App_Themes/Ann/image/placeholder.png");
                // a tag
                obj.addClass("hide");

                if (hasChanged)
                    _variationImageFiles = _variationImageFiles.filter(item => "/uploads/images/" + _productID + "-" + item.name != imageName);
            }

            function OnClientFileSelected1(sender, args) {
                if ($telerik.isIE) return;
                else {
                    truncateName(args);
                    var file = args.get_fileInputField().files.item(args.get_rowIndex());
                    showThumbnail(file, args);
                    $("#<%= imgProductImage.ClientID %>").hide();
                }
            }

            function OnClientFileSelected2(sender, args) {
                if ($telerik.isIE) return;
                else {
                    truncateName(args);
                    var file = args.get_fileInputField().files.item(args.get_rowIndex());
                    showThumbnail(file, args);
                    $("#<%= imgProductImageClean.ClientID %>").hide();
                }
            }

            function OnClientFileUploadRemoved1(sender, args) {
                    $("#<%= imgProductImage.ClientID %>").show();
            }

            function OnClientFileUploadRemoved2(sender, args) {
                    $("#<%= imgProductImageClean.ClientID %>").show();
            }

            // Used for creating a new FileList in a round-about way
            function FileListItem(a) {
                a = [].slice.call(Array.isArray(a) ? a : arguments)
                for (var c, b = c = a.length, d = !0; b-- && d;) d = a[b] instanceof File
                if (!d) throw new TypeError("expected argument to FileList is File or array of File objects")
                for (b = (new ClipboardEvent("")).clipboardData || new DataTransfer; c--;) b.items.add(a[c])
                return b.files
            }

            // Kiểm tra giá trị string is null or empty
            function isBlank(str) {
                return (!str || /^\s*$/.test(str));
            }

            // #region KiotViet
            let _kvUser = "admin";
            let _kvPassword = "0914615407";
            let _retailerName = "iwillgiaminh"
            // Kiểm tra xem danh mục có đang theo dõi không
            function getKvCategory(categoryId) {
                if (categoryId == 0)
                    return;

                let titleAlert = "Kiểm tra danh mục";

                $.ajax({
                    beforeSend: function () {
                        HoldOn.open();
                    },
                    url: "/api/v1/kiotviet/category/ann-shop/" + categoryId,
                    headers: {
                        "Authorization": "Basic " + btoa(_kvUser + ":" + _kvPassword),
                        "retailerName": _retailerName
                    },
                    contentType: 'application/json',
                    method: 'GET',
                    success: (response, textStatus, xhr) => {
                        HoldOn.close();

                        if (xhr.status == 200) {
                            if (!response.cronJob)
                            {
                                let message = "Danh mục đang không được theo dõi.<br>Bạn vẫn muốn đang lên KiotViet";

                                swal({
                                    title: titleAlert,
                                    text: message,
                                    type: "warning",
                                    showCancelButton: true,
                                    cancelButtonText: "Để em xem lại...",
                                    confirmButtonText: "Đúng rồi",
                                    html: true
                                }, function (isConfirm) {
                                    let selected = isConfirm ? 'true' : 'false'; 
                                    let $rdbSyncKiotViet = $("#<%=rdbSyncKiotViet.ClientID%>").find("input[type='radio']");

                                    $.each($rdbSyncKiotViet, function (index, element) {
                                        if ($(this).val() == selected)
                                            $(this).prop('checked', true)
                                        else
                                            $(this).prop('checked', false)
                                    });
                                });
                            }
                        } else {
                            _alterError(titleAlert);
                        }
                    },
                    error: (xhr, textStatus, error) => {
                        HoldOn.close();
                        _alterError(titleAlert, xhr.responseJSON);
                    }
                });
            }
            // #endregion

            // #region Swal
            function _alterError(title, responseJSON) {
                let message = '';
                title = (typeof title !== 'undefined') ? title : 'Thông báo lỗi';

                if (responseJSON === undefined || responseJSON === null) {
                    message = 'Đẫ có lỗi xãy ra.';
                }
                else {
                    if (responseJSON.message)
                        message += responseJSON.message;
                }

                return swal({
                    title: title,
                    text: message,
                    type: "error",
                    html: true
                });
            }
            // #endregion
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
