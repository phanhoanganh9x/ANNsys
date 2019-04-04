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
            if (Request.Cookies["userLoginSystem"] != null)
            {
                string username = Request.Cookies["userLoginSystem"].Value;
                var acc = AccountController.GetByUsername(username);
                if (acc != null)
                {
                    var accountInfo = AccountInfoController.GetByUserID(acc.ID);

                    hdfUserID.Value = acc.ID.ToString();

                    txtNote.Content = accountInfo.Note;

                    ltruserInfor.Text += "<a href=\"javascript:;\" class=\"user-name\" style=\"display:inline-block\">Xin chào, " + acc.Username + "</a> | ";
                    ltruserInfor.Text += "<a href=\"/dang-xuat\" class=\"user-name\" style=\"display:inline-block\">Thoát</a>";
                    int role = Convert.ToInt32(acc.RoleID);
                    if (role == 0) //Admin
                    {
                        ltrMenu.Text = "";
                        ltrMenu.Text += "<li><a href=\"/trang-chu\"><span class=\"icon-menu icon-home\"></span>Trang chủ</a></li>";
                        ltrMenu.Text += "<li><a href=\"/pos\"><span class=\"icon-menu icon-order\"></span>Máy tính tiền</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-don-hang\"><span class=\"icon-menu icon-order\"></span>Đơn hàng</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-chuyen-khoan\"><span class=\"icon-menu icon-order\"></span>Chuyển khoản</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-don-tra-hang\"><span class=\"icon-menu icon-order\"></span>Đổi trả hàng</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-don-hang-chuyen-hoan\"><span class=\"icon-menu icon-order\"></span>Chuyển hoàn</a></li>";
                        ltrMenu.Text += "<li><a href=\"/tat-ca-san-pham\"><span class=\"icon-menu icon-lib\"></span>Sản phẩm</a></li>";
                        ltrMenu.Text += "<li><a href=\"/sp\" target=\"_blank\"><span class=\"icon-menu icon-lib\"></span>SP mở rộng</a></li>";
                        ltrMenu.Text += "<li><a href=\"/bv\" target=\"_blank\"><span class=\"icon-menu icon-lib\"></span>Bài viết</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-bai-viet\"><span class=\"icon-menu icon-lib\"></span>QL bài viết</a></li>";
                        ltrMenu.Text += "<li><a href=\"/quan-ly-danh-muc-san-pham\"><span class=\"icon-menu icon-product\"></span>Danh mục SP</a></li>";
                        ltrMenu.Text += "<li><a href=\"/quan-ly-danh-muc-thuoc-tinh\"><span class=\"icon-menu icon-product\"></span>Thuộc tính</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-khach-hang\"><span class=\"icon-menu icon-product\"></span>Khách hàng</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-nhom-khach-hang\"><span class=\"icon-menu icon-product\"></span>Nhóm KH</a></li>";
                        ltrMenu.Text += "<li><a href=\"/tao-ma-vach\"><span class=\"icon-menu icon-product\"></span>In mã vạch</a></li>";
                        ltrMenu.Text += "<li><a href=\"/quan-ly-nhap-kho\"><span class=\"icon-menu icon-product\"></span>Nhập kho</a></li>";
                        ltrMenu.Text += "<li><a href=\"/quan-ly-xuat-kho\"><span class=\"icon-menu icon-product\"></span>Xuất kho</a></li>";
                        ltrMenu.Text += "<li><a href=\"/kiem-kho\"><span class=\"icon-menu icon-product\"></span>Kiểm kho</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-chuyen-hang\"><span class=\"icon-menu icon-product\"></span>Chuyển kho</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-nhan-hang\"><span class=\"icon-menu icon-product\"></span>Nhận hàng</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-nha-xe\"><span class=\"icon-menu icon-product\"></span>Nhà xe</a></li>";
                        ltrMenu.Text += "<li><a href=\"/quan-ly-dai-ly\"><span class=\"icon-menu icon-lib\"></span>Chi nhánh</a></li>";
                        ltrMenu.Text += "<li><a href=\"/tat-ca-nhan-vien\"><span class=\"icon-menu icon-lib\"></span>Nhân viên</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-nha-cung-cap\"><span class=\"icon-menu icon-product\"></span>Nhà cung cấp</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-chiet-khau\"><span class=\"icon-menu icon-product\"></span>Chiết khấu</a></li>";
                        ltrMenu.Text += "<li><a href=\"/bao-cao\"><span class=\"icon-menu icon-product\"></span>Báo cáo</a></li>";
                        ltrMenu.Text += "<li><a href=\"/cai-dat\"><span class=\"icon-menu icon-product\"></span>Cài đặt</a></li>";
                    }
                    else if (role == 1) //Nhân viên kho
                    {
                        ltrMenu.Text += "<li><a href=\"/trang-chu\"><span class=\"icon-menu icon-home\"></span>Trang chủ</a></li>";
                        ltrMenu.Text += "<li><a href=\"/tat-ca-san-pham\"><span class=\"icon-menu icon-lib\"></span>Sản phẩm</a></li>";
                        ltrMenu.Text += "<li><a href=\"/quan-ly-danh-muc-thuoc-tinh\"><span class=\"icon-menu icon-product\"></span>Thuộc tính</a></li>";
                        ltrMenu.Text += "<li><a href=\"/tao-ma-vach\"><span class=\"icon-menu icon-product\"></span>In mã vạch</a></li>";
                        ltrMenu.Text += "<li><a href=\"/quan-ly-nhap-kho\"><span class=\"icon-menu icon-product\"></span>Nhập kho</a></li>";
                        ltrMenu.Text += "<li><a href=\"/quan-ly-xuat-kho\"><span class=\"icon-menu icon-product\"></span>Xuất kho </a></li>";
                        ltrMenu.Text += "<li><a href=\"/kiem-kho\"><span class=\"icon-menu icon-product\"></span>Kiểm kho</a></li>";
                    }
                    else //Nhân viên bán hàng
                    {
                        ltrMenu.Text += "<li><a href=\"/trang-chu\"><span class=\"icon-menu icon-home\"></span>Trang chủ</a></li>";
                        ltrMenu.Text += "<li><a href=\"/pos\"><span class=\"icon-menu icon-order\"></span>Máy tính tiền</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-don-hang\"><span class=\"icon-menu icon-order\"></span>Đơn hàng</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-chuyen-khoan\"><span class=\"icon-menu icon-order\"></span>Chuyển khoản</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-don-tra-hang\"><span class=\"icon-menu icon-order\"></span>Đổi trả hàng</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-don-hang-chuyen-hoan\"><span class=\"icon-menu icon-order\"></span>Chuyển hoàn</a></li>";
                        ltrMenu.Text += "<li><a href=\"/tat-ca-san-pham\"><span class=\"icon-menu icon-lib\"></span>Sản phẩm</a></li>";
                        ltrMenu.Text += "<li><a href=\"/sp\" target=\"_blank\"><span class=\"icon-menu icon-lib\"></span>SP mở rộng</a></li>";
                        ltrMenu.Text += "<li><a href=\"/bv\" target=\"_blank\"><span class=\"icon-menu icon-lib\"></span>Bài viết</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-khach-hang\"><span class=\"icon-menu icon-product\"></span>Khách hàng</a></li>";
                        ltrMenu.Text += "<li><a href=\"/quan-ly-nhap-kho\"><span class=\"icon-menu icon-product\"></span>Nhập kho</a></li>";
                        ltrMenu.Text += "<li><a href=\"/danh-sach-nha-xe\"><span class=\"icon-menu icon-product\"></span>Nhà xe</a></li>";
                    }
                }
            }
        }
    }
}