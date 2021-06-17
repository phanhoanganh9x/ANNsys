using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Models
{
    public class ProductModel
    {
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int ProductVariableID { get; set; }
        public int ProductStyle { get; set; }
        public string ProductImage { get; set; }
        public string ProductTitle { get; set; }
        public string VariableValue { get; set; }
        public string ParentSKU { get; set; }
        public string ChildSKU { get; set; }
        public double RegularPrice { get; set; }
        public double CostOfGood { get; set; }
        public double RetailPrice { get; set; }
        public double QuantityCurrent { get; set; }
        public double OldPrice { get; set; }
        public double Price10 { get; set; }
        public double BestPrice { get; set; }
    }
}