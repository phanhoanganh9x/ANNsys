using System;

namespace IM_PJ.Models.Pages.print_shipping_note
{
    public class OrderModel
    {
        public string code { get; set; }
        public int status { get; set; }        
        public int paymentMethod { get; set;}
        public int deliveryMethod { get; set; }
        public int postalDeliveryMethod { get; set;}
        public string shippingCode { get; set; }
        public string destination { get; set;}
        public decimal cod { get; set; }
        public decimal shopFee { get; set; }
        public string staff { get; set; }

        public AddressModel sender { get; set; }
        public AddressModel receiver { get; set; }
        public TransportCompanyModel transport { get; set; }

        #region Mapper
        public static OrderModel map(tbl_Order source, tbl_RefundGoods refund, AddressModel sender, AddressModel receiver, TransportCompanyModel transport)
        {
            var order = new OrderModel()
            {
                code = source.ID.ToString(),
                status = source.ExcuteStatus,
                paymentMethod = source.PaymentType,
                deliveryMethod = source.ShippingType,
                postalDeliveryMethod = source.PostDeliveryType,
                shopFee = Convert.ToInt32(source.FeeShipping),
                staff = source.CreatedBy,
                this.sender = sender,
                this.receiver = receiver,
                this.transport = transport
            }

            if (!String.IsNullOrEmpty(source.ShippingCode))
            {
                order.shippingCode = source.ShippingCode;

                if (order.deliveryMethod == (int)DeliveryType.DeliverySave)
                {
                    var codes = order.ShippingCode
                        .Split('.')
                        .Where(x => !String.IsNullOrEmpty(x))
                        .ToList();
                        
                    if (codes.Length == 6)
                        order.destination = String.Format("{0}.{1}.{2}.{3}", codes[1], codes[2], codes[3], codes[4]);
                    else if (codes.Length == 5)
                        order.destination = String.Format("{0}.{1}.{2}", codes[1], codes[2], codes[3]);
                    else if (codes.Length == 4)
                        order.destination = String.Format("{0}.{1}", codes[1], codes[2]);
                    else if (codes.Length == 3)
                        order.destination = String.Format("{0}", codes[1]);
                }
            }
        }
        #endregion
    }
}