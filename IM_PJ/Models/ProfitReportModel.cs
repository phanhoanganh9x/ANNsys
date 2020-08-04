using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Models
{
    public class ProfitReportModel
    {
        public DateTime DateDone { get; set; }
        #region Order
        public int TotalNumberOfOrder { get; set; }
        public int TotalSoldQuantity { get; set; }
        public double TotalSaleCost { get; set; }
        public double TotalSalePrice { get; set; }
        public double TotalSaleDiscount { get; set; }
        public double TotalCoupon { get; set; }
        // Phí giao hàng chỉ dùng để tham khảo
        public double TotalShippingFee { get; set; }
        // Phí khác chỉ dùng để tham khảo
        public double TotalOtherFee { get; set; }
        #endregion
        
        #region Refund
        public int TotalRefundQuantity { get; set; }
        public double TotalRefundCost { get; set; }
        public double TotalRefundPrice { get; set; }
        public double TotalRefundFee { get; set; }
        #endregion
    }
}