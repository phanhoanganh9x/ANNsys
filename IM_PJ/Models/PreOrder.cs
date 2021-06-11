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
    
    public partial class PreOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PreOrder()
        {
            this.PreOrderDetails = new HashSet<PreOrderDetail>();
            this.ViewOrders = new HashSet<ViewOrder>();
        }
    
        public long Id { get; set; }
        public int Status { get; set; }
        public string Avatar { get; set; }
        public long DeliveryAddressId { get; set; }
        public int DeliveryMethod { get; set; }
        public int PaymentMethod { get; set; }
        public Nullable<int> CouponId { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalCostOfGoods { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal CouponPrice { get; set; }
        public decimal Total { get; set; }
        public string SourceOrdering { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] Timestamp { get; set; }
        public decimal ShippingFee { get; set; }
    
        public virtual Coupon Coupon { get; set; }
        public virtual DeliveryAddress DeliveryAddress { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PreOrderDetail> PreOrderDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ViewOrder> ViewOrders { get; set; }
    }
}
