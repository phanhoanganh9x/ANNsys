using IM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
                        (order, tran) => new { tran.CusBankID, tran.AccBankID, order.CreatedDate, tran.Note }
                    ).Join(
                        con.Banks,
                        tran => tran.CusBankID,
                        cusBank => cusBank.ID,
                        (tran, cusBank) => new
                        {
                            CusBankID = tran.CusBankID,
                            CusBankName = cusBank.BankName,
                            AccBankID = tran.AccBankID,
                            OrderCreateDate = tran.CreatedDate,
                            Note = tran.Note
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
                            OrderCreateDate = tran.OrderCreateDate,
                            Note = tran.Note
                        }
                    ).OrderByDescending(
                        o => o.OrderCreateDate
                    ).Select(x => new TransferLast()
                    {
                        CusBankID = x.CusBankID,
                        CusBankName = x.CusBankName,
                        AccBankID = x.AccBankID,
                        AccBankName = x.AccBankName,
                        Note = x.Note
                    }).FirstOrDefault();
                return last;
            }
        }

        /// <summary>
        /// Lấy ID ngân hàng nhận tiền của đơn hàng
        /// </summary>
        /// <param name="orderId">ID đơn hàng</param>
        /// <returns></returns>
        public static int getAccBankId(int orderId)
        {
            using (var con = new inventorymanagementEntities())
            {
                var bank = con.BankTransfers
                    .Where(x => x.OrderID == orderId)
                    .FirstOrDefault();

                return bank != null ?  bank.AccBankID : 0;
            }
        }

        /// <summary>
        /// Khởi tạo thông tin chuyển khoản của đơn hàng
        /// </summary>
        /// <param name="orderId">ID đơn hàng</param>
        /// <param name="accBankId">ID ngân hàng nhận</param>
        /// <param name="staff">Nhân viên khởi tạo đơn hàng</param>
        /// <returns></returns>
        public static bool createBankTransfer(int orderId, int accBankId, int staff)
        {
            try
            {
                using (var con = new inventorymanagementEntities())
                {
                    var now = DateTime.Now;
                    // Lấy thông tin mới về ngân hàng chuyển
                    var cusBankId = con.Banks
                        .Join(
                            con.BankAccounts.Where(x => x.ID == accBankId),
                            b => b.ID,
                            ab => ab.BankID,
                            (b, ab) => b
                        )
                        .Select(x => x.ID)
                        .SingleOrDefault();
                    var transfer = new BankTransfer()
                    {
                        UUID = Guid.NewGuid(),
                        OrderID = orderId,
                        CusBankID = cusBankId,
                        AccBankID = accBankId,
                        DoneAt = now,
                        Money = 0,
                        Status = 2, // Chưa nhận tiền
                        CreatedBy = staff,
                        CreatedDate = now,
                        ModifiedBy = staff,
                        ModifiedDate = now
                    };

                    con.BankTransfers.Add(transfer);
                    con.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Update(BankTransfer transfer)
        {
            using (var con = new inventorymanagementEntities())
            {
                var old = con.BankTransfers.Where(x => x.OrderID == transfer.OrderID).SingleOrDefault();
                if (old != null)
                {
                    old.CusBankID = transfer.CusBankID;
                    old.AccBankID = transfer.AccBankID;
                    old.Money = transfer.Money;
                    old.DoneAt = transfer.DoneAt;
                    old.Status = transfer.Status;
                    old.Note = transfer.Note;
                    old.ModifiedBy = transfer.ModifiedBy;
                    old.ModifiedDate = transfer.ModifiedDate;
                    con.SaveChanges();
                }
                else
                {
                    con.BankTransfers.Add(transfer);
                    con.SaveChanges();
                }
            }

            return true;
        }

        /// <summary>
        /// Cập nhật lại ngân hàng nhận tiền của đơn hàng
        /// </summary>
        /// <param name="orderId">ID đơn hàng</param>
        /// <param name="newAccBankId">ID nhân hàng mới</param>
        /// <returns></returns>
        public static bool updateAccBank(int orderId, int newAccBankId, int staff)
        {
            try
            {
                using (var con = new inventorymanagementEntities())
                {
                    // Lấy thông tin mới về ngân hàng chuyển
                    var newCusBankId = con.Banks
                        .Join(
                            con.BankAccounts.Where(x => x.ID == newAccBankId),
                            b => b.ID,
                            ab => ab.BankID,
                            (b, ab) => b
                        )
                        .Select(x => x.ID)
                        .SingleOrDefault();
                    // Lây dòng dữ liệu thông tin chuyển khoản của đơn hàng
                    var transfer = con.BankTransfers
                        .Where(x => x.OrderID == orderId)
                        .SingleOrDefault();

                    if (transfer != null)
                    {
                        transfer.CusBankID = newCusBankId;
                        transfer.AccBankID = newAccBankId;
                        transfer.ModifiedDate = DateTime.Now;
                        transfer.ModifiedBy = staff;

                        con.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string getTransferLastJSON(int customerID)
        {
            using (var con = new inventorymanagementEntities())
            {
                var last = con.BankTransfers
                    .Join(
                        con.tbl_Order.Where(x => x.CustomerID == customerID),
                        trans => trans.OrderID,
                        ord => ord.ID,
                        (trans, ord) => trans
                    )
                    .Join(
                        con.Banks,
                        tran => tran.CusBankID,
                        bank => bank.ID,
                        (tran, bank) => new { tran, bank }
                     )
                     .OrderByDescending(o => o.tran.DoneAt)
                     .Select(x => new
                     {
                         value = x.bank.ID,
                         text = x.bank.BankName
                     })
                     .FirstOrDefault();

                //if (last == null)
                //{
                //    last = con.Banks
                //        .Select(x => new
                //        {
                //            value = x.ID,
                //            text = x.BankName
                //        })
                //        .FirstOrDefault();
                //}

                var serializer = new JavaScriptSerializer();
                return serializer.Serialize(last);
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
        public string Note { get; set; }
    }
}