namespace IM_PJ.Models.Pages.print_order_image_english
{
    public class OrderMergerDetailModel : IOrderDetailModel
    {
        public string name { get; set; }
        public decimal price { get; set; }
        public decimal totalQuantity { get; set; }
        public decimal totalDiscount { get; set; }
        public decimal total { get; set; }
    }
}