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
    
    public partial class tbl_Product
    {
        public int ID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> ProductOldID { get; set; }
        public string ProductTitle { get; set; }
        public string ProductContent { get; set; }
        public string ProductSKU { get; set; }
        public Nullable<double> ProductStock { get; set; }
        public Nullable<int> StockStatus { get; set; }
        public Nullable<bool> ManageStock { get; set; }
        public Nullable<double> Regular_Price { get; set; }
        public Nullable<double> CostOfGood { get; set; }
        public Nullable<double> Retail_Price { get; set; }
        public string ProductImage { get; set; }
        public Nullable<int> ProductType { get; set; }
        public Nullable<bool> IsHidden { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Materials { get; set; }
        public Nullable<double> MinimumInventoryLevel { get; set; }
        public Nullable<double> MaximumInventoryLevel { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string SupplierName { get; set; }
        public Nullable<int> ProductStyle { get; set; }
        public Nullable<int> ShowHomePage { get; set; }
        public string ProductImageClean { get; set; }
        public Nullable<bool> WebPublish { get; set; }
        public Nullable<System.DateTime> WebUpdate { get; set; }
        public string UnSignedTitle { get; set; }
        public string Slug { get; set; }
        public string Color { get; set; }
        public bool PreOrder { get; set; }
        public Nullable<double> Old_Price { get; set; }
        public bool SyncKiotViet { get; set; }
    }
}
