//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IM_PJ.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_RefundGoodsDetails
    {
        public int ID { get; set; }
        public Nullable<int> RefundGoodsID { get; set; }
        public Nullable<int> AgentID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string SKU { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> QuantityMax { get; set; }
        public string PriceNotFeeRefund { get; set; }
        public Nullable<int> ProductType { get; set; }
        public Nullable<bool> IsCount { get; set; }
        public Nullable<int> RefundType { get; set; }
        public string RefundFeePerProduct { get; set; }
        public string TotalRefundFee { get; set; }
        public string GiavonPerProduct { get; set; }
        public string DiscountPricePerProduct { get; set; }
        public string SoldPricePerProduct { get; set; }
        public string TotalPriceRow { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int CostOfGood { get; set; }
        public double TotalCostOfGood { get; set; }
    }
}
