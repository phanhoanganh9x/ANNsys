using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Models
{
    public class RefundGoodModel
    {
        public int RefundGoodsID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerNick { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerZalo { get; set; }
        public string CustomerFacebook { get; set; }
        public List<RefundDetailModel> RefundDetails { get; set; }
        public double TotalPrice { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalFreeRefund { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        public string CreateBy { get; set; }
        public int OrderSaleID { get; set; }
    }
}