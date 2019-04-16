using IM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}