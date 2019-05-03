﻿using IM_PJ.Models;
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
                var old = con.Deliveries.Where(x => x.OrderID == delivery.OrderID).SingleOrDefault();
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

        public static List<DeliveryReport> getDeliveryReport(List<int> orders)
        {
            using (var con = new inventorymanagementEntities())
            {
                // Get order id list which didn't shipped to transport
                var delivered = con.Deliveries
                    .Where(x => x.Status == 1) // loại bỏ đơn đã giao
                    .Where(x => orders.Contains(x.OrderID))
                    .Select(x => x.OrderID)
                    .OrderBy(o => o)
                    .ToList();
                orders = orders.Where(x => !delivered.Contains(x)).ToList();

                var header = con.tbl_Order
                    .Where(x => x.ExcuteStatus == 2)
                    .Where(x => x.ShippingType == 4)
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

                var report = header
                    .Join(
                        transport,
                        h => h.ID,
                        t => t.OrderID,
                        (h, t) => new
                        {
                            TransportID = t.TransportID,
                            TransportName = t.TransportName,
                            Quantity = 1,
                            Collection = h.PaymentType == 3 ? 1 : 0
                        }
                    )
                    .GroupBy(x => x.TransportID)
                    .Select(g => new DeliveryReport
                    {
                        TransportID = g.Key,
                        TransportName = g.Max(x => x.TransportName),
                        Quantity = g.Sum(x => x.Quantity),
                        Collection = g.Sum(x => x.Collection)
                    })
                    .OrderBy(o => o.TransportName)
                    .ToList();

                return report;
            }
        }

        public static void udpateAfterPrint(int shiperID, List<int> orders, int user)
        {
            using (var con = new inventorymanagementEntities())
            {
                foreach (var id in orders)
                {
                    var now = DateTime.Now;
                    var old = con.Deliveries.Where(x => x.OrderID == id).SingleOrDefault();
                    if (old != null)
                    {
                        // Tránh trường hợp recorde đã được cập nhật trường hợp khác
                        if (old.Status == 2 || old.Status == 3)
                        {
                            old.ShipperID = shiperID;
                            old.Status = 3;
                            old.ModifiedBy = user;
                            old.ModifiedDate = now;
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
                            ModifiedDate = now
                        };
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

        public class DeliveryReport
        {
            public int TransportID { get; set; }
            public string TransportName { get; set; }
            public double Quantity { get; set; }
            // Số lượng thu hộ
            public double Collection { get; set; }
        }
    }
}