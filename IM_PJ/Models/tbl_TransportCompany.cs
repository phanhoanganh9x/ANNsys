
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
    
public partial class tbl_TransportCompany
{

    public int ID { get; set; }

    public int SubID { get; set; }

    public string CompanyName { get; set; }

    public string CompanyPhone { get; set; }

    public string CompanyAddress { get; set; }

    public string ShipTo { get; set; }

    public string Address { get; set; }

    public bool Prepay { get; set; }

    public bool COD { get; set; }

    public string Note { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<System.DateTime> ModifiedDate { get; set; }

    public string ModifiedBy { get; set; }

    public Nullable<int> Status { get; set; }

}

}
