<%@ Page Language="C#" Title="Báo cáo - Thống kê" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="bao-cao.aspx.cs" Inherits="IM_PJ.bao_cao" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main-wrap">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title left">Báo cáo - Thống kê</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel-table clear">
                        <div class="responsive-table">
                            <table class="table table-checkable table-product center-txt">
                                <tbody>
                                    <tr>
                                        <th>Loại báo cáo - thống kê</th>
                                        <th>Thao tác</th>
                                    </tr>
                                    <tr>
                                        <td><a href="thong-ke-loi-nhuan">Lợi nhuận</a></td>
                                        <td><a href="thong-ke-loi-nhuan" class="btn primary-btn h45-btn">Chi tiết</a></td>
                                    </tr>
                                    <tr>
                                        <td><a href="thong-ke-doanh-thu">Doanh thu</a></td>
                                        <td><a href="thong-ke-doanh-thu" class="btn primary-btn h45-btn">Chi tiết</a></td>
                                    </tr>
                                    <tr>
                                        <td><a href="thong-ke-san-luong">Sản lượng</a></td>
                                        <td><a href="thong-ke-san-luong" class="btn primary-btn h45-btn">Chi tiết</a></td>
                                    </tr>
                                    <tr>
                                        <td><a href="thong-ke-nhan-vien">Nhân viên</a></td>
                                        <td><a href="thong-ke-nhan-vien" class="btn primary-btn h45-btn">Chi tiết</a></td>
                                    </tr>
                                    <tr>
                                        <td><a href="thong-ke-san-pham">Sản phẩm</a></td>
                                        <td><a href="thong-ke-san-pham" class="btn primary-btn h45-btn">Chi tiết</a></td>
                                    </tr>
                                     <tr>
                                        <td><a href="thong-ke-so-luong-ton-kho-theo-danh-muc">Tồn kho</a></td>
                                        <td><a href="thong-ke-so-luong-ton-kho-theo-danh-muc" class="btn primary-btn h45-btn">Chi tiết</a></td>
                                    </tr>
                                     <tr>
                                        <td><a href="thong-ke-buu-dien">Thống kê bưu điện</a></td>
                                        <td><a href="thong-ke-buu-dien" class="btn primary-btn h45-btn">Chi tiết</a></td>
                                    </tr>
                                    <tr>
                                        <td><a href="thong-ke-ghtk">Thống kê GHTK</a></td>
                                        <td><a href="thong-ke-ghtk" class="btn primary-btn h45-btn">Chi tiết</a></td>
                                    </tr>
                                    <tr>
                                        <td><a href="thong-ke-khach-hang">Thống kê khách hàng</a></td>
                                        <td><a href="thong-ke-khach-hang" class="btn primary-btn h45-btn">Chi tiết</a></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
</asp:Content>
