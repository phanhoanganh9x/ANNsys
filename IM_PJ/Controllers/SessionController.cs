using IM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace IM_PJ.Controllers
{
    public class SessionController
    {
        public static List<DeliverySession> getDeliverySession(tbl_Account acc, string page = "danh-sach-van-chuyen")
        {
            using (var con = new inventorymanagementEntities())
            {
                var session = con.Sessions
                    .Where(x => x.AccountID == acc.ID)
                    .Where(x => x.Page.ToLower() == page.ToLower())
                    .FirstOrDefault();

                if (session != null)
                    return JsonConvert.DeserializeObject<List<DeliverySession>>(session.Value);
                else
                    return new List<DeliverySession>();
            }
        }
        public static string addDeliverySession(tbl_Account acc, List<DeliverySession> deliverySession, string page = "danh-sach-van-chuyen")
        {
            using (var con = new inventorymanagementEntities())
            {
                var session = con.Sessions
                    .Where(x => x.AccountID == acc.ID)
                    .Where(x => x.Page.ToLower() == page.ToLower())
                    .FirstOrDefault();

                if (session != null)
                {
                    var oldDdeliveries = JsonConvert.DeserializeObject<List<DeliverySession>>(session.Value);
                    var data = deliverySession
                        .Except(
                            deliverySession.Join(
                                    oldDdeliveries,
                                    news => news.OrderID,
                                    olds => olds.OrderID,
                                    (news, olds) => news
                                )
                         )
                         .ToList();

                    if (data.Count > 0)
                    {
                        oldDdeliveries.AddRange(data);
                        oldDdeliveries = oldDdeliveries.OrderByDescending(o => o.OrderID).ToList();
                        session.Value = JsonConvert.SerializeObject(oldDdeliveries);
                        session.ModifiedDate = DateTime.Now;

                        con.SaveChanges();
                    }

                    return session.Value;
                }
                else
                {
                    var now = DateTime.Now;
                    var data = new Session()
                    {
                        AccountID = acc.ID,
                        Page = page.ToLower(),
                        Value = JsonConvert.SerializeObject(deliverySession),
                        CreatedBy = acc.ID,
                        CreatedDate = now,
                        ModifiedBy = acc.ID,
                        ModifiedDate = now
                    };

                    con.Sessions.Add(data);
                    con.SaveChanges();

                    return data.Value;
                }
            }
        }

        public static string deleteDeliverySession(tbl_Account acc, List<DeliverySession> deliverySession, string page = "danh-sach-van-chuyen")
        {
            using (var con = new inventorymanagementEntities())
            {
                var session = con.Sessions
                    .Where(x => x.AccountID == acc.ID)
                    .Where(x => x.Page.ToLower() == page.ToLower())
                    .FirstOrDefault();

                if (session != null)
                {
                    var oldDdeliveries = JsonConvert.DeserializeObject<List<DeliverySession>>(session.Value);

                    oldDdeliveries = oldDdeliveries
                        .Except(
                            oldDdeliveries.Join(
                                    deliverySession,
                                    olds => olds.OrderID,
                                    news => news.OrderID,
                                    (olds, news) => olds
                                )
                        )
                        .OrderByDescending(o => o.OrderID)
                        .ToList();
                    session.Value = JsonConvert.SerializeObject(oldDdeliveries);
                    session.ModifiedDate = DateTime.Now;

                    con.SaveChanges();

                    return session.Value;
                }

                return JsonConvert.SerializeObject(new List<DeliverySession>());
            }
        }

        public static void deleteDeliverySession(tbl_Account acc, string page = "danh-sach-van-chuyen")
        {
            using (var con = new inventorymanagementEntities())
            {
                var session = con.Sessions
                    .Where(x => x.AccountID == acc.ID)
                    .Where(x => x.Page.ToLower() == page.ToLower());

                if (session.Count() > 0)
                {
                    con.Sessions.RemoveRange(session);
                    con.SaveChanges();
                }
            }
        }
    }

    public class DeliverySession
    {
        public int OrderID { get; set; }
        public int ShippingType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}