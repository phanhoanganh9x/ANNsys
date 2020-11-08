using IM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IM_PJ.Controllers
{
    public class CurrencyController
    {
        #region Publich
        public static ExchangeRate getByCode(string code)
        {
            using (var con = new inventorymanagementEntities())
            {
                var currency = con.ExchangeRates
                    .Where(x => x.CurrencyCode.ToUpper() == code.ToUpper())
                    .FirstOrDefault();

                return currency;
            }
        }
        #endregion
    }
}