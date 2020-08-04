using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Models.Pages.thong_ke_doanh_thu_khach_hang
{
    public class OrderInfoModel
    {
        public DateTime dateDone { get; set; }
        public int quantityOrder { get; set; }
        public int quantityProduct { get; set; }
        public Double costOfGoods { get; set; }
        public Double price { get; set; }
        public Double discount { get; set; }
        public Double coupon { get; set; }
        // Phí giao hàng dùng để tham khảo
        public Double feeShipping { get; set; }
        public int quantityRefund { get; set; }
        public int quantityProductRefund { get; set; }
        public Double refundCapital { get; set; }
        // Tiền hoàn trả đã bao gồm phí hoàn trả
        public Double refundMoney { get; set; }
        // Phí hoàn trả dùng để tham khảo
        public Double refundFee { get; set; }
    }
}