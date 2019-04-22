using IM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace IM_PJ.Controllers
{
    public class FeeController
    {
        public static string getFeesJSON(int orderID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var result = String.Empty;

            using (var con = new inventorymanagementEntities())
            {
                var fees = con.Fees
                    .Where(x => x.OrderID == orderID)
                    .Join(
                        con.FeeTypes,
                        fee => fee.FeeTypeID,
                        feetype => feetype.ID,
                        (fee, feetype) => new {
                            UUID = fee.UUID,
                            FeeTypeID = fee.FeeTypeID,
                            FeeTypeName = feetype.Name,
                            FeePrice = fee.FeePrice
                        })
                    .ToArray();
                result = serializer.Serialize(fees);
            }

            return result;
        }

        public static List<FeeInfo> getFeeInfo(int orderID)
        {
            using (var con = new inventorymanagementEntities())
            {
                var fees = con.Fees
                    .Where(x => x.OrderID == orderID)
                    .Join(
                        con.FeeTypes,
                        fee => fee.FeeTypeID,
                        feetype => feetype.ID,
                        (fee, feetype) => new FeeInfo() {
                            Name = feetype.Name,
                            Price = fee.FeePrice
                        })
                    .ToList();

                return fees;
            }
        }

        public static bool Update(int orderID, List<Fee> fees)
        {
            using (var con = new inventorymanagementEntities())
            {
                // Remove fee if it exist in table
                var old = con.Fees.Where(x => x.OrderID == orderID);
                con.Fees.RemoveRange(old);
                con.SaveChanges();

                con.Fees.AddRange(fees);
                con.SaveChanges();
            }

            return true;
        }

        public static bool deleteAll(int orderID)
        {
            using (var con = new inventorymanagementEntities())
            {
                var fees = con.Fees.Where(x => x.OrderID == orderID);
                con.Fees.RemoveRange(fees);
                con.SaveChanges();
            }

            return true;
        }
    }

    public class FeeInfo
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}