using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Models
{
    public enum SearchType
    {
        Order = 1,        // Liên quan đến đơn hàng
        Product = 2       // Liên quan đến sản phẩm

    }
    public enum SaleType
    {
        Retail = 1,         // Lẻ
        Wholesale = 2       // Sỉ
    }

    public enum ExcuteStatus
    {
        Doing = 1,          // Đang xử lý
        Done = 2,           // Đã hoàn tất
        Cancel = 3,         // Đã hủy
        Return = 4,         // Chuyển hoàn
        Sent = 5,           // Đã gửi đi
    }

    public enum PaymentType
    {
        Cash = 1,               // Tiền mặt
        Bank = 2,               // Chuyển khoản
        CashCollection = 3,     // Thu hộ tiền mặt
        Debit = 4               // Ghi nợ
    }

    public enum PaymentStatus
    {
        Waitting = 1,       // Chưa thanh toán
        NotEnough = 2,      // Thanh toán thiếu
        Done = 3,           // Đã hoàn tất
        Approve = 4         // Đã duyệt (Trạng thái chỉ thể hiện với các đơn hàng bưu điện)
    }

    public enum TransferStatus
    {
        Done = 1,           // Đã nhận tiền
        Waitting = 2        // Chưa nhận tiền
    }

    public enum DeliveryType
    {
        Face = 1,                   // Lấy trực tiếp
        PostOffice = 2,             // Chuyển bưu điện
        Proship = 3,                // Dịch vụ Proship
        TransferStation = 4,        // Chuyển xe
        Shipper = 5,                // Nhân viên giao
        DeliverySave = 6,           // Giao hàng tiết kiệm
        Viettel = 7,                // Viettel
        Grab = 8,                   // Grab
        AhaMove = 9,                // AhaMove
        JT = 10,                    // J&T
        GHN = 11                    // Giao hang nhanh
    }

    public enum DeliveryStatus
    {
        Done = 1,           // Đã giao hàng
        Waiting = 2,        // Chưa giao hàng
        Shipping = 3        // Đang giao hàng
    }

    public enum InvoiceStatus
    {
        Yes = 1,    // Có hóa biên nhận
        No = 2      // Không có biên nhận
    }

    public enum StockStatus
    {
        stocking = 1,       // Còn hàng
        stockOut = 2,       // Hết hàng
        stockIn = 3         // Nhập hàng
    }

    public enum ShelfLevel
    {
        Floor = 1,          // Tầng
        Row = 2,            // Hàng
        Shelf = 3,          // Kệ
        FloorShelf = 4      // Tần của kệ
    }

    public enum RegisterProductStatus
    {
        NoApprove = 1,      // Chưa được duyệt
        Approve = 2,        // Đã được duyệt
        Ordering = 3,       // Đã đặt hàng
        Done = 4            // Hàng về
    }

    public enum DeliveryPostOfficeReview
    {
        NoApprove = 1,      // Chưa được duyệt
        Approve = 2         // Đã được duyệt
    }

    public enum PostOfficeFeeStatus
    {
        Profitable = 1, // Trạng thái tiền phí lơn hơn hoặc bằng phí của bưu điện
        Losses = 2
    }

    public enum OrderStatus
    {
        Exist = 1,      // Tồn tại
        NoExist = 2,    // Không tồn tại
        Spam = 3,       // Rac
    }

    public enum DeliveryProshipReview
    {
        NoApprove = 1,      // Chưa được duyệt
        Approve = 2         // Đã được duyệt
    }

    public enum ProshipFeeStatus
    {
        Profitable = 1, // Trạng thái tiền phí lơn hơn hoặc bằng phí của bưu điện
        Losses = 2
    }

    public enum CronJobStatus
    {
        Scheduled = 1, // Trạng thái chờ đợi để chạy
        Start = 2, // Trạng thái đang thao tác
        Pause = 3, // Trạng thái dự lại đợi chờ j đó
        Done = 4, // Trạng thái hoàn thành
        Fail = 5, // Trạng thái thất bại
        Continue = 6 // Trạng thái bỏ qua khi không có sản phẩm
    }

    public enum CouponStatus
    {
        Yes = 1, // Trạng thái có mã giảm giá
        No = 2 // Trạng thái không có mã giảm giá
    }

    public class ProductOrderBy
    {
        // Mới nhất trên app
        public const string latestOnApp = "latestOnApp";
        // Mới nhất trên hệ thống
        public const string latestOnSystem = "latestOnSystem";
        // Kho giảm dần
        public const string stockDesc = "stockDesc";
        // Kho tăng dần
        public const string stockAsc = "stockAsc";
    }

    public enum CheckTagStatus
    {
        @checked = 1,
        unChecked = 2
    }

    public enum Warehouse
    {
        One = 1,
        Two = 2
    }
}