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
    
    public partial class Delivery
    {
        public System.Guid UUID { get; set; }
        public int OrderID { get; set; }
        public int ShipperID { get; set; }
        public int ShippingType { get; set; }
        public int Status { get; set; }
        public string Image { get; set; }
        public decimal COD { get; set; }
        public decimal COO { get; set; }
        public string ShipNote { get; set; }
        public System.DateTime StartAt { get; set; }
        public string Note { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<int> Times { get; set; }
    }
}
