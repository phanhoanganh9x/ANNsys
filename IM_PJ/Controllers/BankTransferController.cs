using IM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IM_PJ.Controllers
{
    public class BankTransferController
    {
        public static TransferLast getTransferLast(int orderID, int cusID)
        {
            using (var con = new inventorymanagementEntities())
            {
                var last = con.tbl_Order
                    .Where(x => x.OrderType == 2 && x.CustomerID == cusID)
                    .Where(x => x.ID != orderID)
                    .Join(
                        con.BankTransfers,
                        order => order.ID,
                        tran => tran.OrderID,
                        (order, tran) => new { tran.CusBankID, tran.AccBankID, order.CreatedDate }
                    ).Join(
                        con.Banks,
                        tran => tran.CusBankID,
                        cusBank => cusBank.ID,
                        (tran, cusBank) => new
                        {
                            CusBankID = tran.CusBankID,
                            CusBankName = cusBank.BankName,
                            AccBankID = tran.AccBankID,
                            OrderCreateDate = tran.CreatedDate
                        }
                    ).Join(
                        con.BankAccounts,
                        tran => tran.AccBankID,
                        accBank => accBank.ID,
                        (tran, accBank) => new
                        {
                            CusBankID = tran.CusBankID,
                            CusBankName = tran.CusBankName,
                            AccBankID = tran.AccBankID,
                            AccBankName = accBank.BankName,
                            OrderCreateDate = tran.OrderCreateDate
                        }
                    ).OrderByDescending(
                        o => o.OrderCreateDate
                    ).Select(x => new TransferLast()
                    {
                        CusBankID = x.CusBankID,
                        CusBankName = x.CusBankName,
                        AccBankID = x.AccBankID,
                        AccBankName = x.AccBankName
                    }).FirstOrDefault();
                return last;
            }
        }
    }

    [Serializable]
    public class TransferLast
    {
        public int CusBankID { get; set; }
        public string CusBankName { get; set; }
        public int AccBankID { get; set; }
        public string AccBankName { get; set; }
    }
}