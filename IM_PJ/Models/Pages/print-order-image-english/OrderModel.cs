using System;
using System.Collections.Generic;

namespace IM_PJ.Models.Pages.print_order_image_english
{
    public interface IOrderDetailModel
    {
    }

    public class OrderModel
    {
        // false: Ảnh chi tiết | true: Ảnh gộp
        public bool merger { get; set; }
        public int id { get; set; }
        public StaffModel staff { get; set; }
        public CustomerModel customer { get; set; }
        // Ngày tạo đơn hàng
        public DateTime createdDate { get; set; }
        public string note { get; set; }
        public int totalQuantity { get; set; }
        // Tỷ giá tiền tệ
        public decimal currencyRate { get; set; }
        public decimal totalPrice { get; set; }
        // Dùng cho các đơn hàng củ không có triết khấu từng dòng
        public decimal defaultDiscount { get; set; }
        public decimal totalDiscount { get; set; }
        public decimal shippingFee { get; set; }
        public IList<OtherFeeModel> otherFees { get; set; }
        public CouponModel coupon { get; set; }
        public RefundOrderModel refundOrder { get; set; }
        public int oldOrderNumber { get; set; }

        public IList<IOrderDetailModel> details { get; set; }
    }
}