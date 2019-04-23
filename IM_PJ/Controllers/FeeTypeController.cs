using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IM_PJ.Models;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

namespace IM_PJ.Controllers
{
    public class FeeTypeController
    {
        public static List<ListItem> getDropDownList()
        {
            var data = new List<ListItem>();
            data.Add(new ListItem(String.Empty, "0"));
            using (var con = new inventorymanagementEntities())
            {
                foreach (var freeType in con.FeeTypes.ToList())
                {
                    data.Add(new ListItem(freeType.Name, freeType.ID.ToString()));
                }
            }

            return data;
        }

        public static string getFeeTypeJSON()
        {
            using (var con = new inventorymanagementEntities())
            {
                var typeList = con.FeeTypes.ToArray();
                foreach(var item in typeList)
                {
                    if (!item.IsNegativeFee.HasValue)
                        item.IsNegativeFee = false;
                }
                var json = new JavaScriptSerializer();
                return json.Serialize(typeList);
            }
        }
    }
}