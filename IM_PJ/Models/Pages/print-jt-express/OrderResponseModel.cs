using System;

namespace IM_PJ.Models.Pages.print_jt_express
{
    public class OrderResponseModel
    {
        public string groupOrderCode { get; set; }
        public int? orderId { get; set; }
        public string code { get; set; }
        public OrderAddressResponseModel sender { get; set; }
        public OrderAddressResponseModel receiver { get; set; }
        public string postalCode { get; set; }
        public string postalBranchCode { get; set; }
        public decimal cod { get; set; }
        public OrderItemResponseModel item { get; set; }
        public float weight { get; set; }
        public string note { get; set; }
        public DateTime sentDate { get; set; }
    }
}