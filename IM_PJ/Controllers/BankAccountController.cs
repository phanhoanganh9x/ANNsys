using IM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace IM_PJ.Controllers
{
    public class BankAccountController
    {
        public static List<ListItem> getDropDownList()
        {
            var data = new List<ListItem>();
            data.Add(new ListItem(String.Empty, "0"));
            using (var con = new inventorymanagementEntities())
            {
                foreach (var bank in con.BankAccounts.ToList())
                {
                    data.Add(new ListItem(bank.BankName, bank.ID.ToString()));
                }
            }

            return data;
        }
    }
}