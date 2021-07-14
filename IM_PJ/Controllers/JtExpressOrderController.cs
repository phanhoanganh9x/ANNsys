using IM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Controllers
{
    public class JtExpressOrderController
    {
        public static JtExpressOrder getOrder(int orderId)
        {
            using (var con = new inventorymanagementEntities())
            {
                var data = con.JtExpressOrders
                    .Where(x => x.OrderId == orderId)
                    .SingleOrDefault();

                return data;
            }
        }
    }
}