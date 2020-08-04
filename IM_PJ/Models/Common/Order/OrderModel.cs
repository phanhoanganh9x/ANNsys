using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Models.Common
{
    public class OrderModel
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int QuantityProduct { get; set; }
        public double Price { get; set;  }
        public double Discount { get; set; }
        public double FeeShipping { get; set; }
        public string StaffName { get; set; }
        public DateTime? DateDone { get; set; }
    }
}