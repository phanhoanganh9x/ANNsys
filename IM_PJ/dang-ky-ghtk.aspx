<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dang-ky-ghtk.aspx.cs" Inherits="IM_PJ.dang_ky_ghtk" EnableSessionState="ReadOnly" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1, user-scalable=yes">
    <meta name="format-detection" content="telephone=no">
    <meta name="robots" content="noindex, nofollow">

    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css"
        integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous">

    <!-- Add icon library -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">

    <!-- Add Select2 library -->
    <!-- https://select2.org/getting-started/installation -->
    <link href="https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/css/select2.min.css" rel="stylesheet" />
    <link href="/App_Themes/Ann/css/HoldOn.css?v=28112021" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="App_Themes/Ann/css/pages/dang-ky-ghtk/dang-ky-ghtk.css?v=202106081515" />

    <title>Tạo đơn GHTK</title>
</head>
<body class="bg07">
    <form id="form12" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager runat="server" ID="scr">
        </asp:ScriptManager>
        <div>
            <main>
                <div class="container container-bg">
                    <div class="row top-bar">
                        <img width="250" src="/uploads/images/logo-ghtk.png">
                    </div>
                    <div class="row">
                        <div class="col-12 col-xl-6">
                            <div class="form-group">
                                <h4>Thông tin người nhận</h4>
                            </div>
                            <div class="form-group">
                                <div class="input-group mb-3">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text" id="inputGroup-tel"><i class="fa fa-phone"></i></span>
                                    </div>
                                    <input type="text" id="tel" class="form-control" aria-label="Sizing example input"
                                        aria-describedby="inputGroup-tel" placeholder="Nhập SĐT">
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="input-group mb-3">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text" id="inputGroup-name"><i class="fa fa-user"></i></span>
                                    </div>
                                    <input type="text" id="name" class="form-control" aria-label="Sizing example input"
                                        aria-describedby="inputGroup-name" placeholder="Tên khách hàng">
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="input-group mb-3">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text" id="inputGroup-address"><i class="fa fa-home"></i></span>
                                    </div>
                                    <input type="text" id="address" class="form-control" aria-label="Sizing example input"
                                        aria-describedby="inputGroup-address" placeholder="Địa chỉ chi tiết (nhà/ngõ/đường...)">
                                </div>
                            </div>
                            <div class="form-group">
                                <select id="ddlProvince" class="form-control">
                                </select>
                            </div>
                            <div class="form-group">
                                <select id="ddlDistrict" class="form-control">
                                </select>
                            </div>
                            <div class="form-group">
                                <select id="ddlWard" class="form-control">
                                </select>
                            </div>
                            <br />
                            <div class="form-group">
                                <h4>Thông tin lấy hàng</h4>
                            </div>
                            <div class="form-group row">
                                <div class="col-xl-4">
                                    <label>Hình thức gửi hàng</label>
                                </div>
                                <div class="col-xl-8">
                                    <div class="custom-control custom-radio custom-control-inline">
                                        <input type="radio" id="shipment_shop" name="shipment_type" class="custom-control-input" value="shop" checked="checked">
                                        <label class="custom-control-label" for="shipment_shop">Lấy hàng tận nơi</label>
                                    </div>
                                    <div class="custom-control custom-radio custom-control-inline">
                                        <input type="radio" id="shipment_post_office" name="shipment_type" class="custom-control-input" value="post_office">
                                        <label class="custom-control-label" for="shipment_post_office">Gửi hàng bưu cục</label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-xl-4">
                                    <label>Địa chỉ lấy hàng</label>
                                </div>
                                <div class="col-xl-8">
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text" id="inputGroup-pick-address"><i class="fa fa-home"></i></span>
                                        </div>
                                        <input type="text" id="pick_address" class="form-control" aria-label="Sizing example input"
                                            aria-describedby="inputGroup-pick-address" disabled="disabled" readonly>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="pick_work_shift" class="col-5 col-xl-4 col-form-label">Dự kiến lấy hàng</label>
                                <div class="col-7 col-xl-8">
                                    <select id="pick_work_shift" class="form-control"></select>
                                </div>
                            </div>
                        </div>
                        <div class="col-12 col-xl-6">
                            <div class="form-group">
                                <h4>Thông tin hàng hoá</h4>
                            </div>
                            <div class="form-group">
                                <table class="table table-bordered">
                                    <tbody>
                                        <tr class="odd">
                                            <td class="index">Tên hàng</td>
                                            <td class="product-name">
                                                <select id="ddlProduct" class="form-control"></select>
                                            </td>
                                        </tr>
                                        <tr class="odd">
                                            <td class="label-weight">Khối lượng (kg)</td>
                                            <td class="input-weight">
                                                <input type="number" min="0" step="0.1" id="weight" class="form-control text-right">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="form-group row">
                                <div class="col-4 col-xl-4">
                                    <label>Hình thức giao hàng</label>
                                </div>
                                <div class="col-8 col-xl-8">
                                    <div class="row">
                                        <div class="col-12 col-xl-12">
                                            <div class="custom-control custom-checkbox">
                                                <input type="checkbox" id="part-delivery" class="custom-control-input" value="1">
                                                <label class="custom-control-label" for="part-delivery">Giao hàng một phần</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divFee" class="form-group row hide">
                                <div class="col-4 col-xl-4">
                                    <label>Phí GHTK tính</label>
                                </div>
                                <div class="col-8 col-xl-8">
                                    <div class="row">
                                        <div class="col-5 col-xl-3">
                                            <label id="feeship">0</label>
                                        </div>
                                        <div class="col-7 col-xl-9">
                                            <div class="custom-control custom-radio custom-control-inline">
                                                <input type="radio" id="feeship_shop" name="feeship" class="custom-control-input" value="1">
                                                <label class="custom-control-label" for="feeship_shop">Lấy phí GHTK (update phí này vào đơn hàng)</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divFeeShop" class="form-group row">
                                <div class="col-4 col-xl-4">
                                    <label id="labelShopFeeTitle">Phí nhân viên tính</label>
                                </div>
                                <div class="col-8 col-xl-8">
                                    <div class="row">
                                        <div class="col-5 col-xl-3">
                                            <label id="labelFeeShop">0</label>
                                        </div>
                                        <div class="col-7 col-xl-9">
                                            <div class="custom-control custom-radio custom-control-inline">
                                                <input type="radio" id="fee_entered" name="feeship" class="custom-control-input" value="2">
                                                <label class="custom-control-label" for="fee_entered">Lấy phí nhân viên tính</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="pick_money" class="col-4 col-xl-4 col-form-label">Tiền thu hộ</label>
                                <div class="input-group col-8 col-xl-8">
                                    <input type="text" id="pick_money" class="form-control text-right" value="0" disabled="disabled" readonly>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="input-group col-xl-12">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text">Mã đơn hàng</span>
                                    </div>
                                    <input type="text" id="client_id" class="form-control" disabled="disabled" readonly>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="input-group col-xl-12">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text"><i class="fa fa-file-o"></i></span>
                                    </div>
                                    <input type="text" id="note" class="form-control" maxlength="120" value="Vui lòng cho khách kiểm tra hàng và nhận hàng về nếu có" placeholder="Hãy thêm thông tin để GHTK phục vụ tốt hơn.">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xl-12">
                            <button id="btnRegister" class="form-control btn-primary btn-submit" type="button" onclick="_checkSubmit()" disabled="disabled">Đơn hàng chưa hoàn tất...</button>
                        </div>
                    </div>
                </div>
            </main>

            <!-- Optional JavaScript -->
            <!-- jQuery first, then Popper.js, then Bootstrap JS -->
            <!-- <script src="https://code.jquery.com/jquery-3.4.1.slim.min.js"
                integrity="sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n"
                crossorigin="anonymous"></script> -->
            <!-- Fix bug: Select2 chạy được trên jquery 3.3.1 -->
            <script src="https://code.jquery.com/jquery-3.3.1.min.js"
                integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8="
                crossorigin="anonymous"></script>
            <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js"
                integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo"
                crossorigin="anonymous"></script>
            <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js"
                integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6"
                crossorigin="anonymous"></script>
            <!-- Sweet Alert -->
            <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
            <!-- Select2 -->
            <script src="https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/js/select2.min.js"></script>
            <script src="/App_Themes/Ann/js/HoldOn.js?v=28112021"></script>

            <script type="text/javascript" src="/App_Themes/Ann/js/services/common/utils-service.js?v=202111091752"></script>
            <script type="text/javascript" src="App_Themes/Ann/js/pages/dang-ky-ghtk/dang-ky-ghtk.js?v=202110141312"></script>
        </div>
    </form>
</body>
</html>
