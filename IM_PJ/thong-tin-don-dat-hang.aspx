<%@ Page Title="Thông tin đơn đặt hàng" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="thong-tin-don-dat-hang.aspx.cs" Inherits="IM_PJ.thong_tin_don_dat_hang" EnableSessionState="ReadOnly" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="App_Themes/Ann/css/pages/thong-tin-don-dat-hang/thong-tin-don-dat-hang.css?v=202101081451" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="parent" runat="server">
        <main id="main-wrap">
            <div class="container">
                <div id="infor-order" class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 id="pageTitle" class="page-title left not-margin-bot"></h3>
                            </div>
                            <div class="panel-body">
                                <div class="row pad">
                                    <div class="col-md-3">
                                        <label class="left pad10">Loại đơn: </label>
                                        <div id="orderType"></div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Nhân viên: </label>
                                        <div id="createBy"></div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Ngày tạo: </label>
                                        <div id="createDate"></div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Hoàn tất: </label>
                                    </div>
                                </div>
                                <div class="row pad">
                                    <div class="col-md-3">
                                        <label class="left pad10">Số lượng: </label>
                                        <div id="totalQuantityHeader"></div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Tổng tiền: </label>
                                        <div id="totalHeader"></div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Trạng thái: </label>
                                        <div id="orderStatus"></div>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="left pad10">Ghi chú: </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="infor-customer" class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Thông tin khách hàng</h3>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Họ tên</label>
                                            <input type="text" id="txtFullname" class="form-control capitalize" placeholder="Họ tên thật của khách (F2)" disabled="disabled"/>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Điện thoại</label>
                                            <input type="text" id="txtPhone" class="form-control" placeholder="Số điện thoại khách hàng" disabled="disabled"/>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Nick đặt hàng</label>
                                            <input type="text" id="txtNick" class="form-control capitalize" placeholder="Tên nick đặt hàng" disabled="disabled"/>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Facebook</label>
                                            <div class="row" id="divFacebook">
                                                <div class="col-md-12 fb">
                                                    <input type="text" class="form-control" placeholder="Đường link chat Facebook" disabled="disabled" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div id="divCustomerView" class="col-md-12 view-detail">
                                    </div>
                                </div>
                                <div class="row">
                                    <div id="discountInfo" class="col-md-12 discount-info"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="deliveryAddress" class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Thông tin nhận hàng</h3>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Họ tên</label>
                                            <input type="text" id="txtRecipientFullName" class="form-control capitalize" placeholder="Họ tên người nhận hàng" disabled="disabled"/>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Điện thoại</label>
                                            <input type="text" id="txtRecipientPhone" class="form-control" placeholder="Số điện thoại người nhận hàng" disabled="disabled"/>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Địa chỉ</label>
                                            <input type="text" id="txtRecipientAddress" class="form-control capitalize" placeholder="Địa chỉ khách hàng" disabled="disabled"/>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Tỉnh thành</label>
                                            <select id="ddlRecipientProvince" class="form-control" disabled="disabled">
                                                <option value="" title="Chọn Tỉnh Thành">Chọn tỉnh Thành</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Quận huyện</label>
                                            <select id="ddlRecipientDistrict" class="form-control" disabled="disabled">
                                                <option value="" title="Chọn quận huyện">Chọn quận huyện</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>Phường xã</label>
                                            <select id="ddlRecipientWard" class="form-control" disabled="disabled">
                                                <option value="" title="Chọn phường xã">Chọn phường xã</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="detail" class="row disable">
                    <div class="col-md-12">
                        <div class="panel-post">
                            <asp:DropDownList ID="ddlCustomerType" runat="server" CssClass="form-control customer-type">
                                <asp:ListItem Value="2" Text="Khách mua sỉ"></asp:ListItem>
                            </asp:DropDownList>
                            <div class="post-body search-product-content clear">
                                <div class="search-product-content">
                                    <table class="table table-checkable table-product table-sale-order shop_table_responsive">
                                        <thead>
                                            <tr>
                                                <th class="order-item">#</th>
                                                <th class="image-item">Ảnh</th>
                                                <th class="name-item">Sản phẩm</th>
                                                <th class="sku-item">Mã</th>
                                                <th class="variable-item">Thuộc tính</th>
                                                <th class="price-item">Giá</th>
                                                <th class="discount-item">Chiết khấu</th>
                                                <th class="quantity-item">Kho</th>
                                                <th class="quantity-item">Số lượng</th>
                                                <th class="total-item">Thành tiền</th>
                                            </tr>
                                        </thead>
                                        <tbody id="preOrderTable" class="content-product"></tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="post-footer">
                                <div class="post-row clear">
                                    <div class="left">Số lượng</div>
                                    <div id="totalQuantityFooter" class="right"></div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">Thành tiền</div>
                                    <div id="totalPrice" class="right totalpriceorder"></div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">Chiết khấu</div>
                                    <div id="discount" class="right totalDiscount"></div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">Sau chiết khấu</div>
                                    <div id="totalAfterDiscount" class="right priceafterchietkhau"></div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">Phí vận chuyển</div>
                                    <div id="feeShip" class="right totalDiscount"></div>
                                </div>
                                <div class="post-row clear coupon">
                                    <div class="left">Mã giảm giá</div>
                                    <div id="coupon" class="right totalDiscount"></div>
                                </div>
                                <div class="post-row clear">
                                    <div class="left">
                                        <strong>TỔNG TIỀN</strong> (đơn hàng <strong id="preOrderId"></strong>)
                                    </div>
                                    <div id='totalFooter' class="right totalpriceorderall price-red"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="status" class="row">
                    <div class="col-md-12">
                        <div class="panel panelborderheading">
                            <div class="panel-heading clear">
                                <h3 class="page-title left not-margin-bot">Trạng thái đơn hàng</h3>
                            </div>
                            <div class="panel-body">
                                <div id="row-excute-status" class="form-row">
                                    <div class="row-left">
                                        Trạng thái xử lý
                                    </div>
                                    <div class="row-right">
                                        <select id="ddlExcuteStatus" class="form-control" disabled="disabled">
                                            <option value="" title="Trạng thái xử lý">Trạng thái xử lý</option>
                                        </select>
                                    </div>
                                </div>
                                <div id="row-payment-status" class="form-row">
                                    <div class="row-left">
                                        Trạng thái thanh toán
                                    </div>
                                    <div class="row-right">
                                        <select id="ddlPaymentStatus" class="form-control" disabled="disabled">
                                            <option value="1" title="Chưa thanh toán">Chưa thanh toán</option>
                                        </select>
                                    </div>
                                </div>
                                <div id="row-payment-type" class="form-row">
                                    <div class="row-left">
                                        Phương thức thanh toán
                                    </div>
                                    <div class="row-right">
                                        <select id="ddlPaymentType" class="form-control" disabled="disabled">
                                            <option value="" title="Phương thức thanh toán">Phương thức thanh toán</option>
                                        </select>
                                    </div>
                                </div>
                                <div id="row-shipping-type" class="form-row">
                                    <div class="row-left">
                                        Phương thức giao hàng
                                    </div>
                                    <div class="row-right">
                                        <select id="ddlShippingType" class="form-control" disabled="disabled">
                                            <option value="" title="Phương thức giao hàng">Phương thức giao hàng</option>
                                        </select>
                                    </div>
                                </div>
                                <div id="row-order-note" class="form-row">
                                    <div class="row-left">
                                        Ghi chú đơn hàng
                                    </div>
                                    <div class="row-right">
                                        <asp:TextBox ID="txtOrderNote" runat="server" CssClass="form-control" placeholder="Ghi chú" disabled="disabled"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="row-createdby" class="form-row">
                                    <div class="row-left">
                                        Nhân viên phụ trách
                                    </div>
                                    <div class="row-right">
                                        <asp:DropDownList ID="ddlCreatedBy" runat="server" CssClass="form-control createdby"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="panel-post">
                                    <div class="post-table-links clear">
                                        <a href="javascript:;" class="btn link-btn btn-create-order hidden" style="background-color: #f87703; float: right" title="Tạo đơn hàng" onclick="createOrder()"><i class="fa fa-floppy-o"></i> Tạo đơn hàng</a>
                                        <a href="javascript:;" class="btn link-btn btn-cancel-preorder hidden" style="background-color: #ffad00; float: right" title="Hủy đơn hàng" onclick="cancelPreOrder()"><i class="fa fa-remove"></i> Hủy đơn hàng</a>
                                        <a href="javascript:;" class="btn link-btn btn-recovery-preorder hidden" style="background-color: #f87703; float: right" title="Phục hồi đơn hàng" onclick="recoveryPreOrder()"><i class="fa fa-reply"></i> Phục hồi đơn hàng</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="buttonbar">
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel-buttonbar">
                            <div class="panel-post">
                                <div class="post-table-links clear">
                                    <a href="javascript:;" class="btn link-btn btn-create-order hidden" style="background-color: #f87703; float: right" title="Tạo đơn hàng" onclick="createOrder()"><i class="fa fa-floppy-o"></i> Tạo đơn hàng</a>
                                    <a href="javascript:;" class="btn link-btn btn-cancel-preorder hidden" style="background-color: #ffad00; float: right" title="Hủy đơn hàng" onclick="cancelPreOrder()"><i class="fa fa-remove"></i> Hủy đơn hàng</a>
                                    <a href="javascript:;" class="btn link-btn btn-recovery-preorder hidden" style="background-color: #f87703; float: right" title="Phục hồi đơn hàng" onclick="recoveryPreOrder()"><i class="fa fa-reply"></i> Phục hồi đơn hàng</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Checking Old Order Modal -->
            <div class="modal fade" id="oldOrderModal" role="dialog" data-backdrop="false">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Thông báo</h4>
                        </div>
                        <div class="modal-body">
                            <h4 id="txtOrder" class="hide"></h4>
                            <h4 id="txtRefundGoods" class="hide">Khách hàng này đang có đơn hàng đổi trả chưa trừ tiền!</h4>
                        </div>
                        <div class="modal-footer">
                            <button id="btnCloseOrderOld" type="button" class="btn btn-default">Vẫn tiếp tục</button>
                            <button id="btnOpenOrder" type="button" class="btn btn-primary">Xem đơn</button>
                            <button id="btnOpenRefundGoods" type="button" class="btn btn-primary">Xem đơn đổi trả</button>
                        </div>
                    </div>
                </div>
            </div>

            <asp:HiddenField ID="hdRole" runat="server" />
            <asp:HiddenField ID="hdfCustomer" runat="server" />
        </main>
    </asp:Panel>

    <telerik:RadScriptBlock ID="sc" runat="server">
        <script type="text/javascript" src="/App_Themes/Ann/js/utils/string-format.js?v=202109302253"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/common/utils-service.js?v=202101081451"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/common/order-service.js?v=202109271901"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/services/thong-tin-don-dat-hang/thong-tin-don-dat-hang-service.js?v=202110132304"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/controllers/thong-tin-don-dat-hang/thong-tin-don-dat-hang-controller.js?v=202110132304"></script>
        <script type="text/javascript" src="/App_Themes/Ann/js/pages/thong-tin-don-dat-hang/thong-tin-don-dat-hang.js?v=202110141258"></script>
    </telerik:RadScriptBlock>
</asp:Content>
