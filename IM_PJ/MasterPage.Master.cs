using IM_PJ.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        public void LoadData()
        {
            if (Request.Cookies["usernameLoginSystem"] != null)
            {
                string username = Request.Cookies["usernameLoginSystem"].Value;
                var acc = AccountController.GetByUsername(username);
                if (acc != null)
                {
                    var accountInfo = AccountInfoController.GetByUserID(acc.ID);

                    hdfUserID.Value = acc.ID.ToString();

                    txtNote.Content = accountInfo.Note;

                    ltruserInfor.Text += "<a href='javascript:;' class='user-name' style='display:inline-block'>Xin chào, " + acc.Username + "</a> | ";
                    ltruserInfor.Text += "<a href='/dang-xuat' class='user-name' style='display:inline-block'>Thoát</a>";

                    var config = ConfigController.GetByTop1();

                    int role = Convert.ToInt32(acc.RoleID);
                    if (role == 0) //Admin
                    {
                        ltrMenu.Text = "";
                        ltrMenu.Text += "<li><a href='/trang-chu'><span class='icon-menu icon-home'></span>Trang chủ</a></li>";
                        ltrMenu.Text += "<li><a target='_blank' href='/pos'><span class='icon-menu icon-order'></span>Máy tính tiền</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-don-hang'><span class='icon-menu icon-order'></span>Đơn hàng</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-don-dat-hang'><span class='icon-menu icon-order'></span>Đơn online</a></li>";
                        if (config.ViewAllReports == 1)
                        {
                            ltrMenu.Text += "<li><a href='/danh-sach-chuyen-khoan'><span class='icon-menu icon-order'></span>Chuyển khoản</a></li>";
                        }
                        ltrMenu.Text += "<li><a href='/danh-sach-van-chuyen'><span class='icon-menu icon-order'></span>Giao hàng</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-don-ghtk'><span class='icon-menu icon-order'></span>GHTK</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-don-tra-hang'><span class='icon-menu icon-order'></span>Đổi trả hàng</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-don-hang-chuyen-hoan'><span class='icon-menu icon-order'></span>Chuyển hoàn</a></li>";
                        ltrMenu.Text += "<li><a href='/tat-ca-san-pham'><span class='icon-menu icon-lib'></span>Sản phẩm</a></li>";
                        //ltrMenu.Text += "<li><a href='/san-pham-theo-ke'><span class='icon-menu icon-lib'></span>Sản phẩm kệ</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-bai-viet'><span class='icon-menu icon-lib'></span>Bài viết</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-bai-viet-app'><span class='icon-menu icon-lib'></span>Bài viết App</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-thong-bao'><span class='icon-menu icon-lib'></span>Thông báo</a></li>";
                        ltrMenu.Text += "<li><a href='/quan-ly-danh-muc-san-pham'><span class='icon-menu icon-product'></span>Danh mục</a></li>";
                        ltrMenu.Text += "<li><a href='/quan-ly-danh-muc-thuoc-tinh'><span class='icon-menu icon-product'></span>Thuộc tính</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-khach-hang'><span class='icon-menu icon-product'></span>Khách hàng</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-nhom-khach-hang'><span class='icon-menu icon-product'></span>Nhóm KH</a></li>";
                        //ltrMenu.Text += "<li><a href='/tao-ma-vach'><span class='icon-menu icon-product'></span>In mã vạch</a></li>";
                        //ltrMenu.Text += "<li><a href='/kiem-ke'><span class='icon-menu icon-product'></span>Kiểm kệ</a></li>";
                        //ltrMenu.Text += "<li><a href='/chuyen-ke'><span class='icon-menu icon-product'></span>Chuyển kệ</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-nhap-hang'><span class='icon-menu icon-product'></span>QL đặt hàng</a></li>";
                        ltrMenu.Text += "<li><a href='/chuyen-kho'><span class='icon-menu icon-product'></span>Chuyển kho</a></li>";
                        ltrMenu.Text += "<li><a href='/thong-ke-chuyen-kho'><span class='icon-menu icon-product'></span>TK chuyển kho</a></li>";
                        ltrMenu.Text += "<li><a href='/thong-ke-nhap-kho'><span class='icon-menu icon-product'></span>TK nhập kho</a></li>";
                        ltrMenu.Text += "<li><a href='/tao-phien-kiem-kho'><span class='icon-menu icon-product'></span>Phiên kiểm kho</a></li>";
                        ltrMenu.Text += "<li><a href='/thong-tin-kiem-kho'><span class='icon-menu icon-product'></span>Xem kiểm kho</a></li>";
                        ltrMenu.Text += "<li><a href='/quan-ly-nhap-kho'><span class='icon-menu icon-product'></span>Nhập kho 1</a></li>";
                        ltrMenu.Text += "<li><a href='/quan-ly-xuat-kho'><span class='icon-menu icon-product'></span>Xuất kho 1</a></li>";
                        ltrMenu.Text += "<li><a href='/quan-ly-nhap-kho-2'><span class='icon-menu icon-product'></span>Nhập kho 2</a></li>";
                        ltrMenu.Text += "<li><a href='/quan-ly-xuat-kho-2'><span class='icon-menu icon-product'></span>Xuất kho 2</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-nha-xe'><span class='icon-menu icon-product'></span>Nhà xe</a></li>";
                        ltrMenu.Text += "<li><a href='/tat-ca-nhan-vien'><span class='icon-menu icon-lib'></span>Nhân viên</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-nha-cung-cap'><span class='icon-menu icon-product'></span>Nhà cung cấp</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-chiet-khau'><span class='icon-menu icon-product'></span>Chiết khấu</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-dang-ky'><span class='icon-menu icon-order'></span>Đăng ký Web</a></li>";
                        ltrMenu.Text += "<li><a href='/tai-khoan-app'><span class='icon-menu icon-order'></span>Đăng ký App</a></li>";
                        if (config.ViewAllReports == 1)
                        {
                            ltrMenu.Text += "<li><a href='/bao-cao'><span class='icon-menu icon-product'></span>Báo cáo</a></li>";
                        }
                        ltrMenu.Text += "<li class='visible-sm'><a href='/cai-dat'><span class='icon-menu icon-product'></span>Cài đặt</a></li>";
                        ltrMenu.Text += "<li><a href='/cron-job-product-status'><span class='icon-menu icon-product'></span>Cron Job</a></li>";
                    }
                    else if (role == 1) //Nhân viên kho
                    {
                        ltrMenu.Text += "<li><a href='/trang-chu'><span class='icon-menu icon-home'></span>Trang chủ</a></li>";
                        ltrMenu.Text += "<li><a href='/tat-ca-san-pham'><span class='icon-menu icon-lib'></span>Sản phẩm</a></li>";
                        ltrMenu.Text += "<li><a href='/san-pham-theo-ke'><span class='icon-menu icon-lib'></span>Sản phẩm kệ</a></li>";
                        ltrMenu.Text += "<li><a href='/tao-ma-vach'><span class='icon-menu icon-product'></span>In mã vạch</a></li>";
                        ltrMenu.Text += "<li><a href='/dang-ky-nhap-hang' target='_blank'><span class='icon-menu icon-lib'></span>Đặt hàng</a></li>";
                        ltrMenu.Text += "<li><a href='/nhan-vien-dat-hang' target='_blank'><span class='icon-menu icon-lib'></span>DS đặt hàng</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-nhap-hang'><span class='icon-menu icon-product'></span>QL đặt hàng</a></li>";
                        ltrMenu.Text += "<li><a href='/thong-ke-nhap-kho'><span class='icon-menu icon-product'></span>TK nhập kho</a></li>";
                        ltrMenu.Text += "<li><a href='/quan-ly-nhap-kho'><span class='icon-menu icon-product'></span>Nhập kho 1</a></li>";
                        ltrMenu.Text += "<li><a href='/quan-ly-xuat-kho'><span class='icon-menu icon-product'></span>Xuất kho 1</a></li>";
                        ltrMenu.Text += "<li><a href='/quan-ly-nhap-kho-2'><span class='icon-menu icon-product'></span>Nhập kho 2</a></li>";
                        ltrMenu.Text += "<li><a href='/quan-ly-xuat-kho-2'><span class='icon-menu icon-product'></span>Xuất kho 2</a></li>";
                        ltrMenu.Text += "<li><a href='/chuyen-kho'><span class='icon-menu icon-product'></span>Chuyển kho</a></li>";
                        ltrMenu.Text += "<li><a href='/thong-ke-chuyen-kho'><span class='icon-menu icon-product'></span>TK chuyển kho</a></li>";
                        ltrMenu.Text += "<li><a href='/thuc-hien-kiem-kho'><span class='icon-menu icon-product'></span>Kiểm kho</a></li>";
                        ltrMenu.Text += "<li><a href='/thong-tin-kiem-kho'><span class='icon-menu icon-product'></span>Xem kiểm kho</a></li>";
                    }
                    else if (role == 2) //Nhân viên bán hàng
                    {
                        ltrMenu.Text += "<li><a href='/trang-chu'><span class='icon-menu icon-home'></span>Trang chủ</a></li>";
                        ltrMenu.Text += "<li><a target='_blank' href='/pos'><span class='icon-menu icon-order'></span>Máy tính tiền</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-don-hang'><span class='icon-menu icon-order'></span>Đơn hàng</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-don-dat-hang'><span class='icon-menu icon-order'></span>Đơn online</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-don-ghtk'><span class='icon-menu icon-order'></span>GHTK</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-don-tra-hang'><span class='icon-menu icon-order'></span>Đổi trả hàng</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-don-hang-chuyen-hoan'><span class='icon-menu icon-order'></span>Chuyển hoàn</a></li>";
                        ltrMenu.Text += "<li><a href='/tat-ca-san-pham'><span class='icon-menu icon-lib'></span>Sản phẩm</a></li>";
                        ltrMenu.Text += "<li><a href='/sp' target='_blank'><span class='icon-menu icon-lib'></span>Xem sản phẩm</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-khach-hang'><span class='icon-menu icon-product'></span>Khách hàng</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-nhom-khach-hang'><span class='icon-menu icon-product'></span>Nhóm KH</a></li>";
                        ltrMenu.Text += "<li><a href='/dang-ky-nhap-hang' target='_blank'><span class='icon-menu icon-lib'></span>Đặt hàng</a></li>";
                        ltrMenu.Text += "<li><a href='/nhan-vien-dat-hang' target='_blank'><span class='icon-menu icon-lib'></span>DS đặt hàng</a></li>";
                        if (acc.Username == "nhom_zalo502")
                        {
                            ltrMenu.Text += "<li><a href='/danh-sach-bai-viet'><span class='icon-menu icon-lib'></span>Bài viết</a></li>";
                            ltrMenu.Text += "<li><a href='/danh-sach-bai-viet-app'><span class='icon-menu icon-lib'></span>Bài viết App</a></li>";
                            ltrMenu.Text += "<li><a href='/danh-sach-thong-bao'><span class='icon-menu icon-lib'></span>Thông báo</a></li>";
                        }
                        ltrMenu.Text += "<li><a target='_blank' href='/bv'><span class='icon-menu icon-lib'></span>Xem bài viết</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-bai-viet-app'><span class='icon-menu icon-lib'></span>Bài viết App</a></li>";
                        ltrMenu.Text += "<li><a href='/danh-sach-nha-xe'><span class='icon-menu icon-product'></span>Nhà xe</a></li>";
                        ltrMenu.Text += "<li><a href='/thong-ke-nhap-kho'><span class='icon-menu icon-product'></span>TK nhập kho</a></li>";
                        if (acc.Username == "nhom_zalo406" || acc.Username == "nhom_zalo409")
                        {
                            ltrMenu.Text += "<li><a href='/chuyen-kho'><span class='icon-menu icon-product'></span>Chuyển kho</a></li>";
                            ltrMenu.Text += "<li><a href='/thong-ke-chuyen-kho'><span class='icon-menu icon-product'></span>TK chuyển kho</a></li>";
                        }
                        if (acc.Username == "nhom_zalo406" || acc.Username == "hotline")
                        {
                            ltrMenu.Text += "<li><a href='/danh-sach-van-chuyen'><span class='icon-menu icon-order'></span>Giao hàng</a></li>";
                        }
                        ltrMenu.Text += "<li><a href='/danh-sach-dang-ky'><span class='icon-menu icon-order'></span>Đăng ký Web</a></li>";
                        ltrMenu.Text += "<li><a href='/tai-khoan-app'><span class='icon-menu icon-order'></span>Đăng ký App</a></li>";
                        if (config.ViewAllReports == 1)
                        {
                            ltrMenu.Text += "<li><a href='/bao-cao-nhan-vien'><span class='icon-menu icon-order'></span>Báo cáo</a></li>";
                        }
                    }
                    else if (role == 3) //Nhân viên kiểm kho
                    {
                        ltrMenu.Text += "<li><a href='/thuc-hien-kiem-kho'><span class='icon-menu icon-product'></span>Kiểm kho</a></li>";
                        ltrMenu.Text += "<li><a href='/thong-tin-kiem-kho'><span class='icon-menu icon-product'></span>Xem kiểm kho</a></li>";
                    }
                    else
                    {
                        ltrMenu.Text += "<li><a href='/trang-chu'><span class='icon-menu icon-home'></span>Trang chủ</a></li>";
                    }
                }
            }
        }
    }
}