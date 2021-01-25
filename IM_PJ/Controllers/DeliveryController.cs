using IM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IM_PJ.Controllers
{
    public class DeliveryController
    {
        public static bool Update(Delivery delivery)
        {
            using (var con = new inventorymanagementEntities())
            {
                Delivery old = null;
                var deliveries = con.Deliveries
                    .Where(x => x.OrderID == delivery.OrderID)
                    .OrderByDescending(o => o.StartAt)
                    .ToList();

                if (deliveries.Count() >= 1)
                {
                    old = deliveries.FirstOrDefault();
                    deliveries = deliveries.Where(x => x.UUID != old.UUID).ToList();

                    if (deliveries.Count > 0)
                    {
                        con.Deliveries.RemoveRange(deliveries);
                        con.SaveChanges();
                    }

                }

                if (old != null)
                { 
                    old.ShipperID = delivery.ShipperID;
                    old.Status = delivery.Status;
                    old.Image = delivery.Image;
                    old.COD = delivery.COD;
                    old.COO = delivery.COO;
                    old.ShipNote = delivery.ShipNote;
                    old.StartAt = delivery.StartAt;
                    old.ModifiedBy = delivery.ModifiedBy;
                    old.ModifiedDate = delivery.ModifiedDate;
                    old.Times = delivery.Times;
                    con.SaveChanges();
                }
                else
                {
                    con.Deliveries.Add(delivery);
                    con.SaveChanges();
                }
            }

            return true;
        }

        public static TransportReport getTransportReport(List<int> orders)
        {
            using (var con = new inventorymanagementEntities())
            {
                var header = con.tbl_Order
                    .Where(x => x.ExcuteStatus == 2) // Đơn đã hoàn tất
                    .Where(x => x.ShippingType == 4) // Chuyển tới nhà xe
                    .Where(x => orders.Contains(x.ID))
                    .OrderBy(o => o.ID);

                var transport = con.tbl_TransportCompany
                    .Join(
                        header,
                        tran => new { tranID = tran.ID, tranSubID = tran.SubID },
                        ord => new { tranID = ord.TransportCompanyID.Value, tranSubID = ord.TransportCompanySubID.Value },
                        (tran, ord) => new { tran, ord }
                     )
                     .Select(x => new
                     {
                         OrderID = x.ord.ID,
                         TransportID = x.tran.ID,
                         TransportName = x.tran.CompanyName
                     })
                     .OrderBy(o => o.OrderID);

                var customer = con.tbl_Customer
                    .Join(
                        header,
                        c => c.ID,
                        o => o.CustomerID,
                        (c, o) => c
                    )
                    .Distinct()
                    .OrderBy(o => o.ID);

                var refund = con.tbl_RefundGoods
                    .Join(
                        header,
                        r => r.ID,
                        o => o.RefundsGoodsID,
                        (r, o) => r
                    )
                    .OrderBy(o => o.ID);

                var delivery = con.Deliveries
                    .Join(
                        header.Where(x => x.PaymentType == (int)PaymentType.CashCollection),
                        d => d.OrderID,
                        h => h.ID,
                        (d, h) => d
                    )
                    .OrderBy(o => o.OrderID);

                var data = header
                    .GroupJoin(
                        refund,
                        o => o.RefundsGoodsID,
                        r => r.ID,
                        (o, r) => new { o, r }
                    )
                    .SelectMany(
                        x => x.r.DefaultIfEmpty(),
                        (parent, child) => new
                        {
                            order = parent.o,
                            moneyRefund = child != null ? child.TotalPrice : "0"
                        }
                    )
                    .Join(
                        customer,
                        tem1 => tem1.order.CustomerID,
                        c => c.ID,
                        (tem1, c) => new {
                            order = tem1.order,
                            customer = c,
                            moneyRefund = tem1.moneyRefund
                        }
                    )
                    .Join(
                        transport,
                        tem2 => tem2.order.ID,
                        t => t.OrderID,
                        (tem2, t) => new
                        {
                            order = tem2.order,
                            customer = tem2.customer,
                            moneyRefund = tem2.moneyRefund,
                            transport = t
                        }
                    )
                    .GroupJoin(
                        delivery,
                        tem3 => tem3.order.ID,
                        d => d.OrderID,
                        (tem3, d) => new {tem3 , d}
                    )
                    .SelectMany(
                        x => x.d.DefaultIfEmpty(),
                        (parent, child) => new
                        {
                            OrderID = parent.tem3.order.ID,
                            TransportID = parent.tem3.transport.TransportID,
                            TransportName = parent.tem3.transport.TransportName,
                            CustomerID = parent.tem3.order.CustomerID.Value,
                            CustomerName = parent.tem3.customer.CustomerName,
                            Quantity = 1,
                            Collection = parent.tem3.order.PaymentType == (int)PaymentType.CashCollection ? 1 : 0,
                            Payment = parent.tem3.order.TotalPrice,
                            MoneyRefund = parent.tem3.moneyRefund,
                            MoneyCollection = child != null ? child.COO : 0 
                        }
                    )
                    .ToList();

                var report = new TransportReport()
                {
                    Transports = data
                        .GroupBy(x => x.TransportID)
                        .Select(g => new TransportInfo
                        {
                            TransportID = g.Key,
                            TransportName = g.Max(x => x.TransportName),
                            Quantity = g.Sum(x => x.Quantity),
                            Collection = g.Sum(x => x.Collection),
                        })
                        .OrderBy(o => o.TransportName)
                        .ToList(),
                    Collections = data.Where(x => x.Collection == 1)
                        .Select(x => new CollectionInfo()
                        {
                            OrderID = x.OrderID,
                            TransportID = x.TransportID,
                            TransportName = x.TransportName,
                            CustomerID = x.CustomerID,
                            CustomerName = x.CustomerName,
                            Collection = Convert.ToDecimal(x.Payment) - Convert.ToDecimal(x.MoneyRefund),
                            MoneyReceived = x.MoneyCollection
                        })
                        .OrderByDescending(
                            o =>  o,
                            new CollectionInfoComparer()
                        )
                        .ToList()
                }; 

                return report;
            }
        }

        public static List<ShipperReport> getShipperReport(List<int> orders)
        {
            using (var con = new inventorymanagementEntities())
            {
                var delivery = con.Deliveries
                    .Where(x => orders.Contains(x.OrderID))
                    .OrderBy(o => o.OrderID);

                var order = con.tbl_Order
                     .Where(x => x.ExcuteStatus == 2) // Đơn đã hoàn tất
                     .Where(x => x.ShippingType == 5) // Nhân viên giao hàng
                     .Where(x => orders.Contains(x.ID))
                     .OrderBy(o => o.ID);

                var refund = con.tbl_RefundGoods
                    .Join(
                        order,
                        r => r.ID,
                        o => o.RefundsGoodsID,
                        (r, o) => r
                    )
                    .OrderBy(o => o.ID);

                var data = order
                    .GroupJoin(
                        refund,
                        o => o.RefundsGoodsID.Value,
                        r => r.ID,
                        (o, r) => new { o, r }
                    )
                    .SelectMany(
                        x => x.r.DefaultIfEmpty(),
                        (parent, child) => new
                        {
                            order = parent.o,
                            moneyRefund = child != null ? child.TotalPrice : "0"
                        }
                    )
                    .GroupJoin(
                        delivery,
                        tem1 => tem1.order.ID,
                        del => del.OrderID,
                        (tem1, del) => new { tem1, del }
                    )
                    .SelectMany(
                        x => x.del.DefaultIfEmpty(),
                        (parent, child) => new {
                            order = parent.tem1.order,
                            delivery = child,
                            moneyRefund = parent.tem1.moneyRefund
                        }
                    )
                    .ToList();

                var report = data
                    .Select(x =>
                    {
                        var item = new ShipperReport();

                        item.OrderID = x.order.ID;
                        item.CustomerName = x.order.CustomerName;
                        item.Payment = Convert.ToDecimal(x.order.TotalPrice) - Convert.ToDecimal(x.moneyRefund);
                        item.MoneyCollection = 0;
                        item.Price = 0;

                        if (x.delivery != null)
                        {
                            // Thu hộ
                            if (x.order.PaymentType == 3)
                            {
                                item.MoneyCollection = x.delivery.COO;
                            }
                            item.Price = x.delivery.COD;
                        }
                        else
                        {
                            // Thu hộ
                            if (x.order.PaymentType == 3)
                            {
                                item.MoneyCollection = item.Payment;
                            }
                            item.Price = Convert.ToDecimal(x.order.FeeShipping);
                        }

                        return item;
                    })
                    .ToList();

                return report;
            }
        }

        public static void udpateAfterPrint(int shiperID, List<int> orders, int user, int deliveryTimes)
        {
            using (var con = new inventorymanagementEntities())
            {
                foreach (var id in orders)
                {
                    var now = DateTime.Now;
                    var old = con.Deliveries.Where(x => x.OrderID == id).SingleOrDefault();
                    if (old != null)
                    {
                        // Tránh trường hợp record đã được cập nhật trường hợp khác
                        if (old.Status == 2 || old.Status == 3)
                        {
                            old.ShipperID = shiperID;
                            old.Status = 3;
                            old.ModifiedBy = user;
                            old.ModifiedDate = now;
                            old.Times = deliveryTimes;
                            con.SaveChanges();
                        }
                    }
                    else
                    {
                        var delivery = new Delivery()
                        {
                            UUID = Guid.NewGuid(),
                            OrderID = id,
                            Status = 3,
                            StartAt = now,
                            ShipperID = shiperID,
                            COO = 0,
                            COD = 0,
                            ShipNote = String.Empty,
                            CreatedBy = user,
                            CreatedDate = now,
                            ModifiedBy = user,
                            ModifiedDate = now,
                            Times = deliveryTimes
                        };

                        var order = con.tbl_Order.Where(x => x.ID == id).FirstOrDefault();

                        if (order != null)
                        {
                            // Thu hộ
                            if (order.PaymentType == 3)
                            {
                                decimal COD = Convert.ToDecimal(order.TotalPrice);
                                if (order.RefundsGoodsID > 0)
                                {
                                    var refund = con.tbl_RefundGoods.Where(x => x.ID == order.RefundsGoodsID).FirstOrDefault();
                                    if (refund != null)
                                    {
                                        COD = COD - Convert.ToDecimal(refund.TotalPrice);
                                    }
                                }
                                delivery.COO = COD;
                            }
                            
                            delivery.COD = Convert.ToDecimal(order.FeeShipping);
                        }
                        con.Deliveries.Add(delivery);
                        con.SaveChanges();
                    }
                }
            }
        }

        public static string getDeliveryLast(int customerID)
        {
            using (var con = new inventorymanagementEntities())
            {
                var last = con.tbl_Customer
                    .Where(x => x.ID == customerID)
                    .Where(x => x.ShippingType == 4) // Hình thức nhà xe
                    .Join(
                        con.tbl_TransportCompany,
                        cus => new { tranID = cus.TransportCompanyID.Value, tranSubID = cus.TransportCompanySubID.Value },
                        tran => new { tranID = tran.ID, tranSubID = tran.SubID },
                        (cus, tran) => new
                        {
                            tranID = tran.ID,
                            tranName = tran.CompanyName,
                            tranSubID = tran.SubID,
                            tranSubName = tran.ShipTo
                        }
                    )
                    .SingleOrDefault();

                var serializer = new JavaScriptSerializer();

                if (last == null)
                {
                    last = con.tbl_Order
                        .Where(x => x.CustomerID == customerID)
                        .Where(x => x.ShippingType == 4) // Hình thức nhà xe
                        .OrderByDescending(o => o.DateDone)
                        .Join(
                            con.tbl_TransportCompany,
                            cus => new { tranID = cus.TransportCompanyID.Value, tranSubID = cus.TransportCompanySubID.Value },
                            tran => new { tranID = tran.ID, tranSubID = tran.SubID },
                            (cus, tran) => new
                            {
                                tranID = tran.ID,
                                tranName = tran.CompanyName,
                                tranSubID = tran.SubID,
                                tranSubName = tran.ShipTo
                            }
                        )
                        .FirstOrDefault();
                }

                return serializer.Serialize(last);
            }
        }
        
        public static void updateDelivery(tbl_Account acc, List<DeliverySession> session)
        {
            using (var con = new inventorymanagementEntities())
            {
                var now = DateTime.Now;

                foreach (var item in session)
                {
                    var data = con.Deliveries
                    .Where(x => x.OrderID == item.OrderID)
                    .FirstOrDefault();

                    if (data != null)
                    {
                        data.ShipperID = item.ShipperID;
                        data.Times = item.DeliveryTimes;
                        data.Status = item.DeliveryStatus;
                        data.COD = Convert.ToDecimal(item.ShippingFee);
                        data.COO = Convert.ToDecimal(item.COD);
                        data.ModifiedBy = acc.ID;
                        data.ModifiedDate = now;

                        con.SaveChanges();
                    }
                    else
                    {
                        var delivery = new Delivery()
                        {
                            UUID = Guid.NewGuid(),
                            OrderID = item.OrderID,
                            ShipperID = item.ShipperID,
                            Status = item.DeliveryStatus,
                            Image = String.Empty,
                            COD = Convert.ToDecimal(item.ShippingFee),
                            COO = Convert.ToDecimal(item.COD),
                            ShipNote = String.Empty,
                            StartAt = now,
                            Note = String.Empty,
                            CreatedBy = acc.ID,
                            CreatedDate = now,
                            ModifiedBy = acc.ID,
                            ModifiedDate = now,
                            Times = item.DeliveryTimes
                        };

                        con.Deliveries.Add(delivery);
                        con.SaveChanges();
                    }

                }
            }
        }

        public static DeliveryAddress getDeliveryAddressById(long deliveryAddressId)
        {
            using (var con = new inventorymanagementEntities())
            {
                var deliveryAddress = con.DeliveryAddresses
                    .Where(x => x.Id == deliveryAddressId)
                    .SingleOrDefault();

                return deliveryAddress;
            }
        }

        public class TransportInfo
        {
            public int TransportID { get; set; }
            public string TransportName { get; set; }
            public double Quantity { get; set; }
            // Số lượng thu hộ
            public double Collection { get; set; }
        }

        public class CollectionInfo
        {
            public int OrderID { get; set; }
            public int TransportID { get; set; }
            public string TransportName { get; set; }
            public int CustomerID { get; set; }
            public string CustomerName { get; set; }
            // Số tiền thu hộ
            public decimal Collection { get; set; }
            // Số tiền đã nhận
            public decimal MoneyReceived { get; set; }
        }

        public class CollectionInfoComparer : IComparer<CollectionInfo>
        {
            public int Compare(CollectionInfo item, CollectionInfo other)
            {
                if (item.TransportName == other.TransportName)
                {
                    return item.OrderID.CompareTo(other.OrderID);
                }

                return item.TransportName.CompareTo(other.TransportName);
            }
        }

        public class TransportReport
        {
            public List<TransportInfo> Transports { get; set; }
            public List<CollectionInfo> Collections { get; set; }
        }

        public class ShipperReport
        {
            public int OrderID { get; set; }
            public string CustomerName { get; set; }
            public decimal Payment { get; set; }
            public decimal MoneyCollection { get; set; }
            public decimal Price { get; set; }
        }
    }
}