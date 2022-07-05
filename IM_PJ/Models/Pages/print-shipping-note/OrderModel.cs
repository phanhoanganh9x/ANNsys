using System;
using System.Collections.Generic;
using System.Linq;

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
        private static string _createDestination (string shippingCode)
        {
            var destination = String.Empty;
            var codes = shippingCode
                .Split('.')
                .Where(x => !String.IsNullOrEmpty(x))
                .ToList();

            if (codes.Count == 6)
                destination = String.Format("{0}.{1}.{2}.{3}", codes[1], codes[2], codes[3], codes[4]);
            else if (codes.Count == 5)
                destination = String.Format("{0}.{1}.{2}", codes[1], codes[2], codes[3]);
            else if (codes.Count == 4)
                destination = String.Format("{0}.{1}", codes[1], codes[2]);
            else if (codes.Count == 3)
                destination = String.Format("{0}", codes[1]);

            return destination;
        }

        public static OrderModel map(
            GroupOrder source,
            AddressModel sender,
            AddressModel receiver,
            TransportCompanyModel transport
        )
        {
            var order = new OrderModel()
            {
                code = source.Code,
                status = source.OrderStatusId,
                paymentMethod = source.PaymentMethodId,
                deliveryMethod = source.DeliveryMethodId,
                postalDeliveryMethod = 0,
                cod = 0,
                shopFee = source.ShippingFee,
                staff = source.CreatedBy,
                sender = sender,
                receiver = receiver,
                transport = transport
            };

            // Mã vận đơn
            if (!String.IsNullOrEmpty(source.ShippingCode))
            {
                order.shippingCode = source.ShippingCode;

                if (order.deliveryMethod == (int)DeliveryType.DeliverySave)
                    order.destination = _createDestination(order.shippingCode);
            }

            // COD
            if (order.paymentMethod == (int)PaymentType.CashCollection)
                order.cod = source.Cod;

            return order;
        }

        public static OrderModel map(
            tbl_Order source,
            tbl_RefundGoods refund,
            AddressModel sender,
            AddressModel receiver,
            TransportCompanyModel transport
        ) {
            var order = new OrderModel()
            {
                code = source.ID.ToString(),
                status = source.ExcuteStatus.HasValue ? source.ExcuteStatus.Value : (int)ExcuteStatus.Doing,
                paymentMethod = source.PaymentType.HasValue ? source.PaymentType.Value : (int)PaymentType.Cash,
                deliveryMethod = source.ShippingType.HasValue ? source.ShippingType.Value : (int)DeliveryType.Face,
                postalDeliveryMethod = source.PostalDeliveryType.HasValue ? source.PostalDeliveryType.Value : 0,
                cod = 0,
                shopFee = Convert.ToInt32(source.FeeShipping),
                staff = source.CreatedBy,
                sender = sender,
                receiver = receiver,
                transport = transport
            };

            // Mã vận đơn
            if (!String.IsNullOrEmpty(source.ShippingCode))
            {
                order.shippingCode = source.ShippingCode;

                if (order.deliveryMethod == (int)DeliveryType.DeliverySave)
                    order.destination = _createDestination(order.shippingCode);
            }

            // COD
            if (order.paymentMethod == (int)PaymentType.CashCollection)
            {
                order.cod += Convert.ToDecimal(source.TotalPrice);

                if (refund != null)
                    order.cod -= Convert.ToDecimal(refund.TotalPrice);
            }

            return order;
        }
        #endregion
    }
}