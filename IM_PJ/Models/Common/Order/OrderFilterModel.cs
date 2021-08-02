using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Models
{
    public class OrderFilterModel
    {
        // Thể loại tìm kiếm ( 1: tìm đơn hàng | 2: tìm sản phẩm)
        public int searchType { get; set; } = 0;
        // Sản phâm: SKU, tên product, màu
        // Khách hàng: Tên khách hàng, nick, số điện thoại
        // Delivery: Mã code dịch vụ shipping
        public string search { get; set; } = String.Empty;
        // Thể loại đơn hàng( 1: Lẻ | 2: Sỉ)
        public int orderType { get; set; } = 0;
        // chiết khấu ( 'yes': Có | 'no': Không)
        public string discount { get; set; } = String.Empty;
        // Phí khác ( 'yes': Có | 'no': Không)
        public string otherFee { get; set; } = String.Empty;
        // Số lượng ( 'greaterthan': Lớn hơn | 'lessthan': nhỏ hơn | 'between': Trong khoản)
        public string quantity { get; set; } = String.Empty;
        public int quantityFrom { get; set; } = 0;
        public int quantityTo { get; set; } = 0;
        // Xử lý ( 1: Đang xử lý | 2: Đã hoàn tất | 3: Đã hủy )
        public List<int> excuteStatus { get; set; } = new List<int>();
        // Kiểu thanh toán ( 1: Tiền mặt | 2: Chuyển khoản | 3: Thu hộ | 4: Công nợ)
        public int paymentType { get; set; } = 0;
        // Trạng thái thanh toán ( 1: Chưa thanh toán | 2: Thanh toán thiếu | 3: Đã thanh toán)
        public int paymentStatus { get; set; } = 0;
        // Danh sách ngân hàng nhận tiền
        public int bankReceive { get; set; } = 0;
        // Trạng thái chuyển khoản ( 1: Đã nhận tiền | 2: Chưa nhận tiền)
        public int transferStatus { get; set; } = 0;
        // Kiểu giao hàng ( 1: Lấy hàng trực tiếp | 2: Chuyển Bưu Điện | 3: Dịch vụ Proship | 4: Chuyển xe | 5: Nhân viên giao hàng
        // 6: Giao hàng tiết kiệm | 7: Viettel | 8: Grab | 9: AhaMove | 10: J&T)
        public List<int> shippingType { get; set; } = new List<int>();
        // Chành xe
        public int transportCompany { get; set; } = 0;
        // Người giao hàng
        public int shipper { get; set; } = 0;
        // Thời gian hàng ( 'today': Hôm nay | 'yesterday': Hôm qua | 'beforeyesterday': Hôm kia | 'week': Tuần nay |
        // '7days': 7 ngày | 'thismonth': Tháng này | 'lastmonth': Tháng trước | 'beforelastmonth': Tháng trước nữa | '30days': 30 ngày) 
        public string deliveryStart { get; set; } = String.Empty;
        // Trạng thái giao hàng (1: Đã giao hàng | 2: Chưa giao hàng | 3: Đang giao hàng)
        public int deliveryStatus { get; set; } = 0;
        // Đợt giao hàng
        public int deliveryTimes { get; set; } = 0;
        // Trạng thái biên nhận (1: Có | 2: Không)
        public int invoiceStatus { get; set; } = 0;
        public string orderCreatedBy { get; set; } = String.Empty;
        // Thời gian đơn hàng
        public DateTime? orderFromDate { get; set; }
        public DateTime? orderToDate { get; set; }
        // Thời gian chuyển khoản ngân hàng
        public DateTime? transferFromDate { get; set; }
        public DateTime? transferToDate { get; set; }
        public string orderNote { get; set; } = String.Empty;
        // Bộ lọc đã chon cho in report
        public bool selected { get; set; } = false;
        // Người đang thực hiện filter
        public tbl_Account account { get; set; }
        // Đơn hàng có mã khuyến mãi hay không
        public int couponStatus { get; set; }
    }
}