namespace IM_PJ.Models.Pages.print_order_image
{
    public class OrderDetailModel : IOrderDetailModel
    {
        public string avatar { get; set; }
        public int productType { get; set; }
        public string sku { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public decimal oldPrice { get; set; }
        public decimal discount { get; set; }
    }
}