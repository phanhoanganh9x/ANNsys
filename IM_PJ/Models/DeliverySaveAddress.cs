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
    
    public partial class DeliverySaveAddress
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliverySaveAddress()
        {
            this.DeliveryAddresses = new HashSet<DeliveryAddress>();
            this.DeliveryAddresses1 = new HashSet<DeliveryAddress>();
            this.DeliveryAddresses2 = new HashSet<DeliveryAddress>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> PID { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<int> Region { get; set; }
        public string Alias { get; set; }
        public bool IsPicked { get; set; }
        public bool IsDelivered { get; set; }
        public string NameJT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryAddress> DeliveryAddresses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryAddress> DeliveryAddresses1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryAddress> DeliveryAddresses2 { get; set; }
    }
}
