using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Models
{
    public class RegisterProductFilterModel
    {
        // Tìm kiếm theo title
        // Tìm kiếm theo tên khách hàng
        public string search { get; set; } = String.Empty;
        // Danh mục sản phẩm
        public int category { get; set; } = 0;
        // Trạng thái đơn đăng ký nhập hàng
        public int status { get; set; } = 0;
        // Nhận viên khởi tạo
        public string createdBy { get; set; } = String.Empty;
        // Thời gian tạo đơn đăng ký nhập hàng
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        // Tìm kiếu theo một số màu mặc định
        public string color { get; set; } = String.Empty;
        // Tìm kiếm theo một số size mặc định
        public string size { get; set; } = String.Empty;
        // Chọn các dữ liệu đã đánh dấu
        public bool selected { get; set; } = false;
        // Người đang thực hiện filter
        public tbl_Account account { get; set; }
    }
}