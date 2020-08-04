using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Models
{
    /// <summary>
    /// Lớp Product Variation Cost Of Good Sale
    /// </summary>
    public class ProductVariationCOGSModel
    {
        public int id { get; set; }
        public string sku { get; set; }
        public double cogs { get; set; }
    }
}