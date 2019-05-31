using IM_PJ.Models;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using WebUI.Business;

namespace IM_PJ.Controllers
{
    public class OrderController
    {
        #region CRUD
        public static tbl_Order Insert(int AgentID, int OrderType, string AdditionFee, string DisCount, int CustomerID, string CustomerName, string CustomerPhone,
            string CustomerAddress, string CustomerEmail, string TotalPrice, string TotalPriceNotDiscount, int PaymentStatus, int ExcuteStatus, bool IsHidden, int WayIn,
            DateTime CreatedDate, string CreatedBy, double DiscountPerProduct, double TotalDiscount, string FeeShipping, int PaymentType, int ShippingType, string DateDone, double GuestPaid, double GuestChange, int TransportCompanyID, int TransportCompanySubID, string OtherFeeName, double OtherFeeValue, int PostalDeliveryType)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Order ui = new tbl_Order();
                ui.AgentID = AgentID;
                ui.OrderType = OrderType;
                ui.AdditionFee = AdditionFee;
                ui.DisCount = DisCount;
                ui.CustomerID = CustomerID;
                ui.CustomerName = CustomerName;
                ui.CustomerPhone = CustomerPhone;
                ui.CustomerAddress = CustomerAddress;
                if (!string.IsNullOrEmpty(CustomerEmail))
                    ui.CustomerEmail = CustomerEmail;
                ui.TotalPrice = TotalPrice;
                ui.TotalPriceNotDiscount = TotalPriceNotDiscount;
                ui.PaymentStatus = PaymentStatus;
                ui.ExcuteStatus = ExcuteStatus;
                ui.IsHidden = IsHidden;
                ui.WayIn = WayIn;
                ui.IsHidden = IsHidden;
                ui.DiscountPerProduct = DiscountPerProduct;
                ui.TotalDiscount = TotalDiscount;
                ui.FeeShipping = FeeShipping;
                ui.CreatedDate = CreatedDate;
                ui.CreatedBy = CreatedBy;
                ui.PaymentType = PaymentType;
                ui.ShippingType = ShippingType;
                ui.GuestPaid = GuestPaid;
                ui.GuestChange = GuestChange;
                ui.TransportCompanyID = TransportCompanyID;
                ui.TransportCompanySubID = TransportCompanySubID;
                ui.OtherFeeName = OtherFeeName;
                ui.OtherFeeValue = OtherFeeValue;
                ui.PostalDeliveryType = PostalDeliveryType;
                if (DateDone != "")
                {
                    ui.DateDone = Convert.ToDateTime(DateDone);
                }

                dbe.tbl_Order.Add(ui);
                dbe.SaveChanges();
                //int kq = ui.ID;
                return ui;
            }
        }
        public static tbl_Order InsertOnSystem(int AgentID, int OrderType, string AdditionFee, string DisCount, int CustomerID, string CustomerName, string CustomerPhone,
           string CustomerAddress, string CustomerEmail, string TotalPrice, string TotalPriceNotDiscount, int PaymentStatus, int ExcuteStatus, bool IsHidden, int WayIn,
           DateTime CreatedDate, string CreatedBy, double DiscountPerProduct, double TotalDiscount, string FeeShipping, double GuestPaid, double GuestChange, int PaymentType, int ShippingType, string OrderNote, DateTime DateDone, string OtherFeeName, double OtherFeeValue, int PostalDeliveryType)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Order ui = new tbl_Order();
                ui.AgentID = AgentID;
                ui.OrderType = OrderType;
                ui.AdditionFee = AdditionFee;
                ui.DisCount = DisCount;
                ui.CustomerID = CustomerID;
                ui.CustomerName = CustomerName;
                ui.CustomerPhone = CustomerPhone;
                ui.CustomerAddress = CustomerAddress;
                ui.CustomerEmail = CustomerEmail;
                ui.TotalPrice = TotalPrice;
                ui.TotalPriceNotDiscount = TotalPriceNotDiscount;
                ui.PaymentStatus = PaymentStatus;
                ui.ExcuteStatus = ExcuteStatus;
                ui.IsHidden = IsHidden;
                ui.WayIn = WayIn;
                ui.IsHidden = IsHidden;
                ui.DiscountPerProduct = DiscountPerProduct;
                ui.TotalDiscount = TotalDiscount;
                ui.FeeShipping = FeeShipping;
                ui.CreatedDate = CreatedDate;
                ui.CreatedBy = CreatedBy;
                ui.GuestPaid = GuestPaid;
                ui.GuestChange = GuestChange;
                ui.PaymentType = PaymentType;
                ui.ShippingType = ShippingType;
                ui.OrderNote = OrderNote;
                ui.DateDone = DateDone;
                ui.OtherFeeName = OtherFeeName;
                ui.OtherFeeValue = OtherFeeValue;
                ui.PostalDeliveryType = PostalDeliveryType;
                dbe.tbl_Order.Add(ui);
                dbe.SaveChanges();

                return ui;
            }
        }

        public static int UpdateRefund(int ID, int? RefundsGoodsID, string created)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                var ui = dbe.tbl_Order.Where(x => x.ID == ID).FirstOrDefault();
                if (ui != null)
                {
                    ui.RefundsGoodsID = RefundsGoodsID;
                    ui.ModifiedDate = DateTime.Now;
                    ui.ModifiedBy = created;
                    int i = dbe.SaveChanges();
                    return i;
                }
                return 0;
            }
        }
        public static int DeleteOrderRefund(int RefundsGoodsID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                var ui = dbe.tbl_Order.Where(x => x.RefundsGoodsID == RefundsGoodsID).FirstOrDefault();
                if (ui != null)
                {
                    ui.RefundsGoodsID = null;
                    ui.ModifiedDate = DateTime.Now;
                    int i = dbe.SaveChanges();
                    return ui.ID;
                }
                return 0;
            }
        }
        public static string Update(int ID, int OrderType, string AdditionFee, string DisCount, int CustomerID, string CustomerName,
            string CustomerPhone, string CustomerAddress, string CustomerEmail, string TotalPrice, string TotalPriceNotDiscount, int PaymentStatus,
            int ExcuteStatus, DateTime ModifiedDate, string ModifiedBy, double DiscountPerProduct, double TotalDiscount,
            string FeeShipping)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                var ui = dbe.tbl_Order.Where(o => o.ID == ID).FirstOrDefault();
                if (ui != null)
                {
                    ui.OrderType = OrderType;
                    ui.AdditionFee = AdditionFee;
                    ui.DisCount = DisCount;
                    ui.CustomerID = CustomerID;
                    ui.CustomerName = CustomerName;
                    ui.CustomerPhone = CustomerPhone;
                    ui.CustomerAddress = CustomerAddress;
                    ui.CustomerEmail = CustomerEmail;
                    ui.TotalPrice = TotalPrice;
                    ui.TotalPriceNotDiscount = TotalPriceNotDiscount;
                    ui.PaymentStatus = PaymentStatus;
                    ui.ExcuteStatus = ExcuteStatus;
                    ui.DiscountPerProduct = DiscountPerProduct;
                    ui.TotalDiscount = TotalDiscount;
                    ui.FeeShipping = FeeShipping;
                    ui.ModifiedDate = ModifiedDate;
                    ui.ModifiedBy = ModifiedBy;

                    dbe.SaveChanges();
                    int kq = ui.ID;
                    return kq.ToString();
                }
                return null;
            }
        }
        public static string UpdateCustomerPhone(int ID, string CustomerPhone)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                var ui = dbe.tbl_Order.Where(o => o.ID == ID).FirstOrDefault();
                if (ui != null)
                {
                    ui.OrderNote = ui.OrderNote + ". Số điện thoại cũ của khách hàng này là " + ui.CustomerPhone;
                    ui.CustomerPhone = CustomerPhone;
                    dbe.SaveChanges();
                    int kq = ui.ID;
                    return kq.ToString();
                }
                return null;
            }
        }
        public static string UpdateOnSystem(int ID, int OrderType, string AdditionFee, string DisCount, int CustomerID, string CustomerName,
            string CustomerPhone, string CustomerAddress, string CustomerEmail, string TotalPrice, string TotalPriceNotDiscount, int PaymentStatus,
            int ExcuteStatus, DateTime ModifiedDate, string ModifiedBy, double DiscountPerProduct, double TotalDiscount,
            string FeeShipping, double GuestPaid, double GuestChange, int PaymentType, int ShippingType, string OrderNote, string DateDone, int RefundsGoodsID = 0, string ShippingCode = null, int TransportCompanyID = 0, int TransportCompanySubID = 0, string OtherFeeName = "", double OtherFeeValue = 0, int PostalDeliveryType = 1)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                var ui = dbe.tbl_Order.Where(o => o.ID == ID).FirstOrDefault();
                if (ui != null)
                {
                    ui.OrderType = OrderType;
                    ui.AdditionFee = AdditionFee;
                    ui.DisCount = DisCount;
                    ui.CustomerID = CustomerID;
                    ui.CustomerName = CustomerName;
                    ui.CustomerPhone = CustomerPhone;
                    ui.CustomerAddress = CustomerAddress;
                    ui.CustomerEmail = CustomerEmail;
                    ui.TotalPrice = TotalPrice;
                    ui.TotalPriceNotDiscount = TotalPriceNotDiscount;
                    ui.PaymentStatus = PaymentStatus;
                    ui.ExcuteStatus = ExcuteStatus;
                    ui.DiscountPerProduct = DiscountPerProduct;
                    ui.TotalDiscount = TotalDiscount;
                    ui.FeeShipping = FeeShipping;
                    ui.GuestPaid = GuestPaid;
                    ui.GuestChange = GuestChange;
                    ui.ModifiedDate = ModifiedDate;
                    ui.ModifiedBy = ModifiedBy;
                    ui.PaymentType = PaymentType;
                    ui.ShippingType = ShippingType;
                    ui.OrderNote = OrderNote;
                    ui.DateDone = null;
                    if (DateDone != "")
                    {
                        ui.DateDone = Convert.ToDateTime(DateDone);
                    }
                    ui.ShippingCode = ShippingCode;
                    ui.TransportCompanyID = TransportCompanyID;
                    ui.TransportCompanySubID = TransportCompanySubID;
                    ui.OtherFeeName = OtherFeeName;
                    ui.OtherFeeValue = OtherFeeValue;
                    ui.PostalDeliveryType = PostalDeliveryType;
                    dbe.SaveChanges();
                    int kq = ui.ID;
                    return kq.ToString();
                }
                return null;
            }
        }

        public static int UpdateExcuteStatus(int ID, string CreatedBy)
        {
            using (var db = new inventorymanagementEntities())
            {
                var ui = db.tbl_Order.Where(x => x.ID == ID).FirstOrDefault();
                if (ui != null)
                {
                    ui.ExcuteStatus = 3;
                    ui.TotalPrice = "0";
                    ui.TotalDiscount = 0;
                    ui.TotalPriceNotDiscount = "0";
                    ui.GuestChange = 0;
                    ui.ModifiedDate = DateTime.Now;
                    ui.ModifiedBy = CreatedBy;
                    int i = db.SaveChanges();
                    return i;
                }
                return 0;
            }
        }

        public static bool UpdateExcuteStatus4(int ID, string CreatedBy, string OrderNote)
        {
            using (var db = new inventorymanagementEntities())
            {
                var ui = db.tbl_Order.Where(x => x.ID == ID).FirstOrDefault();
                if (ui != null)
                {
                    if (ui.ExcuteStatus != 4)
                    {
                        ui.ModifiedDate = DateTime.Now;
                    }
                    ui.ModifiedBy = CreatedBy;
                    ui.PaymentStatus = 1;
                    ui.ExcuteStatus = 4;
                    ui.OrderNote = OrderNote;
                    int i = db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        #endregion
        #region Select
        public static tbl_Order GetByID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Order ai = dbe.tbl_Order.Where(a => a.ID == ID).SingleOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }
        public static List<tbl_Order> GetAll()
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_Order> ags = new List<tbl_Order>();
                ags = dbe.tbl_Order.OrderByDescending(o => o.ID).ToList();
                return ags;
            }
        }
        public static List<tbl_Order> GetAllByAgentID(int AgentID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_Order> ags = new List<tbl_Order>();
                ags = dbe.tbl_Order.Where(o => o.AgentID == AgentID).OrderByDescending(o => o.ID).ToList();
                return ags;
            }
        }
        public static List<tbl_Order> GetAllByOrderType(int OrderType)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_Order> ags = new List<tbl_Order>();
                ags = dbe.tbl_Order.Where(o => o.OrderType == OrderType).OrderByDescending(o => o.ID).ToList();
                return ags;
            }
        }
        public static List<tbl_Order> GetByPaymentStatus(int PaymentStatus)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_Order> ags = new List<tbl_Order>();
                ags = dbe.tbl_Order.Where(o => o.PaymentStatus == PaymentStatus).OrderByDescending(o => o.ID).ToList();
                return ags;
            }
        }
        public static List<tbl_Order> GetByExcuteStatus(int ExcuteStatus)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_Order> ags = new List<tbl_Order>();
                ags = dbe.tbl_Order.Where(o => o.ExcuteStatus == ExcuteStatus).OrderByDescending(o => o.ID).ToList();
                return ags;
            }
        }
        public static List<tbl_Order> GetByCustomerIDFromDateToDate(int AgentID, int CustomerID, DateTime FromDate, DateTime ToDate)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_Order> ags = new List<tbl_Order>();
                ags = dbe.tbl_Order.Where(o => o.AgentID == AgentID && o.CustomerID == CustomerID && o.CreatedDate >= FromDate && o.CreatedDate < ToDate)
                    .OrderByDescending(o => o.ID).ToList();
                return ags;
            }
        }

        private static void CalDate(string strDate, ref DateTime fromdate, ref DateTime todate)
        {
            switch (strDate)
            {
                case "today":
                    fromdate = DateTime.Today;
                    todate = DateTime.Now;
                    break;
                case "yesterday":
                    fromdate = fromdate.AddDays(-1);
                    todate = DateTime.Today;
                    break;
                case "beforeyesterday":
                    fromdate = DateTime.Today.AddDays(-2);
                    todate = DateTime.Today.AddDays(-1);
                    break;
                case "week":
                    int days = DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.Today.DayOfWeek;
                    fromdate = fromdate.AddDays(-days + 1);
                    todate = DateTime.Now;
                    break;
                case "thismonth":
                    fromdate = new DateTime(fromdate.Year, fromdate.Month, 1);
                    todate = DateTime.Now;
                    break;
                case "lastmonth":
                    var thismonth = new DateTime(fromdate.Year, fromdate.Month, 1);
                    fromdate = thismonth.AddMonths(-1);
                    todate = thismonth;
                    break;
                case "beforelastmonth":
                    thismonth = new DateTime(fromdate.Year, fromdate.Month, 1);
                    fromdate = thismonth.AddMonths(-2);
                    todate = thismonth.AddMonths(-1);
                    break;
                case "7days":
                    fromdate = fromdate.AddDays(-6);
                    todate = DateTime.Now;
                    break;
                case "30days":
                    fromdate = fromdate.AddDays(-29);
                    todate = DateTime.Now;
                    break;
            }
        }

        public static List<OrderList> Filter(string TextSearch, int OrderType, int ExcuteStatus, int PaymentStatus, int TransferStatus, int PaymentType, int ShippingType, string Discount, string OtherFee, string CreatedBy, string CreatedDate, string TransferDoneAt, int TransportCompany, string DeliveryStartAt)
        {
            using (var con = new inventorymanagementEntities())
            {
                var orders = con.tbl_Order
                    .Select(x => new
                    {
                        ID = x.ID,
                        CustomerID = x.CustomerID,
                        CustomerNewPhone = x.CustomerNewPhone,
                        OrderType = x.OrderType,
                        PaymentType = x.PaymentType,
                        PaymentStatus = x.PaymentStatus,
                        ShippingType = x.ShippingType,
                        ShippingCode = x.ShippingCode,
                        TransportCompanyID = x.TransportCompanyID,
                        ExcuteStatus = x.ExcuteStatus,
                        DateDone = x.DateDone,
                        CreatedDate = x.CreatedDate,
                        TotalDiscount = x.TotalDiscount,
                        CreatedBy = x.CreatedBy,
                        OrderNote = x.OrderNote,
                        RefundsGoodsID = x.RefundsGoodsID,
                    })
                    .Where(x => 1 == 1);

                // Filter Order Type
                if (OrderType > 0)
                {
                    orders = orders.Where(x =>
                        x.OrderType == OrderType
                    );
                }
                // Filter Payment Type
                if (PaymentType > 0)
                {
                    orders = orders.Where(x =>
                        x.PaymentType == PaymentType
                    );
                }
                // Filter Payment Status
                if (PaymentStatus > 0)
                {
                    orders = orders.Where(x =>
                        x.PaymentStatus == PaymentStatus
                    );
                }
                // Filter Excute Status
                if (ExcuteStatus > 0)
                {
                    orders = orders.Where(x =>
                        x.ExcuteStatus == ExcuteStatus
                    );
                }
                // Filter Shipping Type
                if (ShippingType > 0)
                {
                    orders = orders.Where(x =>
                       x.ShippingType == ShippingType
                    );
                }

                // Filter Transport Company
                if (TransportCompany > 0)
                {
                    orders = orders.Where(x =>
                       x.TransportCompanyID == TransportCompany
                    );
                }

                // Filter Discount
                if (!String.IsNullOrEmpty(Discount))
                {
                    if (Discount.Equals("yes"))
                    {
                        orders = orders.Where(x =>
                           x.TotalDiscount > 0
                        );
                    }
                    else
                    {
                        orders = orders.Where(x =>
                           x.TotalDiscount == 0
                        );
                    }
                }

                // Filter Created By
                if (!String.IsNullOrEmpty(CreatedBy))
                {
                    orders = orders.Where(x =>
                        x.CreatedBy == CreatedBy
                    );
                }

                // Filter Created Date
                if (!String.IsNullOrEmpty(CreatedDate))
                {
                    DateTime fromdate = DateTime.Today;
                    DateTime todate = DateTime.Now;
                    CalDate(CreatedDate, ref fromdate, ref todate);

                    if (ExcuteStatus == 2)
                    {
                        orders = orders.Where(x =>
                            x.DateDone >= fromdate &&
                            x.DateDone <= todate
                        );
                    }
                    else
                    {
                        orders = orders.Where(x =>
                            x.CreatedDate >= fromdate &&
                            x.CreatedDate <= todate
                        );
                    }
                }

                var orderFilter = orders.Select(x => new {
                    ID = x.ID,
                    CustomerID = x.CustomerID,
                    RefundsGoodsID = x.RefundsGoodsID
                });

                // Filter orderid or customername or customerphone or nick or shipcode
                if (!String.IsNullOrEmpty(TextSearch))
                {
                    string search = Regex.Replace(TextSearch.Trim(), @"[^0-9a-zA-Z\s_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+", "").Trim().ToLower();
                    search = UnSign.convert(search);

                    var number = Regex.IsMatch(search, @"^\d+$");
                    var textNumber = Regex.IsMatch(search, @"^\w+\d+$");

                    if (number)
                    {
                        orderFilter = orders
                            .Join(
                                con.tbl_Customer,
                                h => h.CustomerID,
                                c => c.ID,
                                (h, c) => new
                                {
                                    ID = h.ID,
                                    CustomerID = h.CustomerID,
                                    RefundsGoodsID = h.RefundsGoodsID,
                                    // Customer
                                    CustomerPhone = c.CustomerPhone,
                                    CustomerNewPhone = h.CustomerNewPhone
                                }
                            )
                            .Where(x =>
                                x.ID.ToString() == search ||
                                x.CustomerPhone == search ||
                                x.CustomerNewPhone == search
                            )
                            .Select(x => new {
                                ID = x.ID,
                                CustomerID = x.CustomerID,
                                RefundsGoodsID = x.RefundsGoodsID
                            });
                    }
                    else if (textNumber)
                    {
                        orderFilter = orders
                            .Join(
                                con.tbl_OrderDetail,
                                h => h.ID,
                                d => d.OrderID,
                                (h, d) => new
                                {
                                    ID = h.ID,
                                    CustomerID = h.CustomerID,
                                    ShippingCode = h.ShippingCode,
                                    RefundsGoodsID = h.RefundsGoodsID,
                                    // Order detail
                                    SKU = d.SKU,
                                }
                            )
                            .Where(x =>
                                x.SKU.ToLower().StartsWith(search) ||
                                x.ShippingCode.ToLower() == search
                            )
                            .Select(x => new {
                                ID = x.ID,
                                CustomerID = x.CustomerID,
                                RefundsGoodsID = x.RefundsGoodsID
                            });
                    }
                    else
                    {
                        orderFilter = orders
                            .Join(
                                con.tbl_Customer,
                                h => h.CustomerID,
                                c => c.ID,
                                (h, c) => new
                                {
                                    ID = h.ID,
                                    CustomerID = h.CustomerID,
                                    ShippingCode = h.ShippingCode,
                                    RefundsGoodsID = h.RefundsGoodsID,
                                    // Customer
                                    UnSignedName = c.UnSignedName,
                                    UnSignedNick = c.UnSignedNick
                                }
                            )
                            .Where(x =>
                                x.UnSignedName.ToLower().Contains(search) ||
                                x.UnSignedNick.ToLower().Contains(search) ||
                                x.ShippingCode == search
                            )
                            .Select(x => new {
                                ID = x.ID,
                                CustomerID = x.CustomerID,
                                RefundsGoodsID = x.RefundsGoodsID
                            });
                    }
                }

                // Filter Delivery Start At
                if (!String.IsNullOrEmpty(DeliveryStartAt))
                {
                    DateTime fromdate = DateTime.Today;
                    DateTime todate = DateTime.Now;
                    CalDate(DeliveryStartAt, ref fromdate, ref todate);

                    orderFilter = orders
                        .GroupJoin(
                            con.Deliveries,
                            h => h.ID,
                            d => d.OrderID,
                            (h, d) => new { h, d }
                        )
                        .SelectMany(
                            x => x.d.DefaultIfEmpty(),
                            (parent, child) => new
                            {
                                ID = parent.h.ID,
                                CustomerID = parent.h.CustomerID,
                                RefundsGoodsID = parent.h.RefundsGoodsID,
                                // Delivery
                                DelStartAt = child.StartAt
                            }
                        )
                        .Where(x =>
                            x.DelStartAt >= fromdate &&
                            x.DelStartAt <= todate
                        )
                        .Select(x => new {
                            ID = x.ID,
                            CustomerID = x.CustomerID,
                            RefundsGoodsID = x.RefundsGoodsID
                        });
                }


                // Filter Transfer Status or DoneAt
                if (TransferStatus > 0 || !String.IsNullOrEmpty(TransferDoneAt))
                {
                    DateTime fromdate = DateTime.Today;
                    DateTime todate = DateTime.Now;
                    CalDate(TransferDoneAt, ref fromdate, ref todate);

                    var tempTrans = orders
                        .GroupJoin(
                            con.BankTransfers,
                            h => h.ID,
                            b => b.OrderID,
                            (h, b) => new { h, b }
                        )
                        .SelectMany(
                            x => x.b.DefaultIfEmpty(),
                            (parent, child) => new
                            {
                                ID = parent.h.ID,
                                CustomerID = parent.h.CustomerID,
                                RefundsGoodsID = parent.h.RefundsGoodsID,
                                // Bank Transfers
                                TransStatus = child.Status,
                                TransDoneAt = child.DoneAt,
                            }
                        )
                        .Where(x => 1 == 1);

                    if (TransferStatus == 1)
                    {
                        tempTrans = tempTrans.Where(x =>
                            x.TransStatus == 1
                        );
                    }
                    else if (TransferStatus == 2)
                    {
                        tempTrans = tempTrans.Where(x =>
                            x.TransStatus != 1
                        );
                    }

                    if (!String.IsNullOrEmpty(TransferDoneAt))
                    {

                        tempTrans = tempTrans.Where(x =>
                            x.TransDoneAt >= fromdate &&
                            x.TransDoneAt <= todate
                        );
                    }

                    orderFilter = tempTrans.Select(x => new {
                        ID = x.ID,
                        CustomerID = x.CustomerID,
                        RefundsGoodsID = x.RefundsGoodsID
                    });
                }

                // Get info main
                var header = orderFilter
                    .Join(
                        con.tbl_Order,
                        h => h.ID,
                        o => o.ID,
                        (h, o) => o
                    )
                    .OrderByDescending(o => o.ID)
                    .ToList();

                // Get info quantiy
                var body = orderFilter
                    .Join(
                        con.tbl_OrderDetail,
                        h => h.ID,
                        od => od.OrderID.Value,
                        (h, od) => new
                        {
                            OrderID = h.ID,
                            Quantity = od.Quantity.HasValue ? od.Quantity.Value : 0
                        }
                    )
                    .GroupBy(x => x.OrderID)
                    .Select(g => new
                    {
                        OrderID = g.Key,
                        Quantity = g.Sum(x => x.Quantity)
                    })
                    .OrderByDescending(o => o.OrderID)
                    .ToList();

                // Get info refunds
                var refunds = orderFilter.Where(x => x.RefundsGoodsID.HasValue)
                    .Join(
                        con.tbl_RefundGoods,
                        h => h.RefundsGoodsID.Value,
                        r => r.ID,
                        (h, r) => new
                        {
                            OrderID = h.ID,
                            RefundsGoodsID = h.RefundsGoodsID,
                            TotalPrice = r.TotalPrice
                        });

                // Get info customer
                var customer = orderFilter
                    .Join(
                        con.tbl_Customer,
                        h => h.CustomerID,
                        c => c.ID,
                        (h, c) => new
                        {
                            OrderID = h.ID,
                            CustomerID = c.ID,
                            CustomerName = c.CustomerName,
                            Nick = c.Nick,
                            CustomerPhone = c.CustomerPhone
                        })
                    .OrderByDescending(o => o.OrderID)
                    .ToList();

                // Get info fee
                var fee = orderFilter
                    .Join(
                        con.Fees,
                        h => h.ID,
                        f => f.OrderID,
                        (h, f) => new
                        {
                            OrderID = h.ID,
                            FeeTypeID = f.FeeTypeID,
                            FeePrice = f.FeePrice
                        }
                    )
                    .Join(
                        con.FeeTypes,
                        d => d.FeeTypeID,
                        t => t.ID,
                        (d, t) => new
                        {
                            OrderID = d.OrderID,
                            FeeTypeName = t.Name,
                            FeePrice = d.FeePrice
                        }
                    )
                    .GroupBy(x => x.OrderID)
                    .Select(g => new
                    {
                        OrderID = g.Key,
                        OtherFeeName = g.Count() > 1 ? "Nhiều phí khác" : g.Max(x => x.FeeTypeName),
                        OtherFeeValue = g.Sum(x => x.FeePrice)
                    })
                    .OrderByDescending(o => o.OrderID)
                    .ToList();

                // Get info transfer bank
                var trans = orderFilter
                    .Join(
                        con.BankTransfers,
                        h => h.ID,
                        t => t.OrderID,
                        (h, t) => new
                        {
                            OrderID = h.ID,
                            CusBankID = t.CusBankID,
                            AccBankID = t.AccBankID,
                            Money = t.Money,
                            Status = t.Status,
                            DoneAt = t.DoneAt,
                            Note = t.Note
                        }
                    )
                    .Join(
                        con.Banks,
                        h => h.CusBankID,
                        c => c.ID,
                        (h, c) => new
                        {
                            OrderID = h.OrderID,
                            CusBankID = h.CusBankID,
                            AccBankID = h.AccBankID,
                            Money = h.Money,
                            Status = h.Status,
                            DoneAt = h.DoneAt,
                            Note = h.Note,
                            // Bank
                            CusBankName = c.BankName
                        }
                    )
                    .Join(
                        con.BankAccounts,
                        h => h.AccBankID,
                        a => a.ID,
                        (h, a) => new
                        {
                            OrderID = h.OrderID,
                            CusBankID = h.CusBankID,
                            AccBankID = h.AccBankID,
                            Money = h.Money,
                            Status = h.Status,
                            DoneAt = h.DoneAt,
                            Note = h.Note,
                            // Bank
                            CusBankName = h.CusBankName,
                            // Bank Account
                            AccBankName = a.BankName
                        }
                    )
                    .OrderByDescending(o => o.OrderID)
                    .ToList();

                // Get info delivery
                var deliveries = orderFilter
                    .Join(
                        con.Deliveries,
                        h => h.ID,
                        t => t.OrderID,
                        (h, t) => new
                        {
                            OrderID = h.ID,
                            StartAt = t.StartAt,
                            Status = t.Status,
                            ShipperID = t.ShipperID,
                            COD = t.COD,
                            COO = t.COO,
                            ShipNote = t.ShipNote,
                            Image = t.Image,
                        }
                    )
                    .Join(
                        con.Shippers,
                        h => h.ShipperID,
                        c => c.ID,
                        (h, c) => new
                        {
                            OrderID = h.OrderID,
                            StartAt = h.StartAt,
                            Status = h.Status,
                            ShipperID = h.ShipperID,
                            COD = h.COD,
                            COO = h.COO,
                            ShipNote = h.ShipNote,
                            Image = h.Image,
                            // Shipper
                            ShipperName = c.Name
                        }
                    )
                    .OrderByDescending(o => o.OrderID)
                    .ToList();

                var data = header
                    .Join(
                        body,
                        h => h.ID,
                        b => b.OrderID,
                        (h, b) => new OrderList()
                        {
                            ID = h.ID,
                            CustomerID = h.CustomerID.HasValue ? h.CustomerID.Value : 0,
                            OrderType = h.OrderType.HasValue ? h.OrderType.Value : 0,
                            ExcuteStatus = h.ExcuteStatus.HasValue ? h.ExcuteStatus.Value : 0,
                            PaymentStatus = h.PaymentStatus.HasValue ? h.PaymentStatus.Value : 0,
                            PaymentType = h.PaymentType.HasValue ? h.PaymentType.Value : 0,
                            ShippingType = h.ShippingType.HasValue ? h.ShippingType.Value : 0,
                            TotalPrice = Convert.ToDouble(h.TotalPrice),
                            TotalDiscount = h.TotalDiscount.Value,
                            FeeShipping = Convert.ToDouble(h.FeeShipping),
                            CreatedBy = h.CreatedBy,
                            CreatedDate = h.CreatedDate.HasValue ? h.CreatedDate.Value : DateTime.Now,
                            DateDone = h.DateDone,
                            OrderNote = h.OrderNote,
                            RefundsGoodsID = h.RefundsGoodsID,
                            ShippingCode = h.ShippingCode,
                            TransportCompanyID = h.TransportCompanyID,
                            TransportCompanySubID = h.TransportCompanySubID,
                            PostalDeliveryType = h.PostalDeliveryType.HasValue ? h.PostalDeliveryType.Value : 0,
                            // Order Detail
                            Quantity = Convert.ToInt32(b.Quantity),
                        }
                    )
                    .Join(
                        customer,
                        h => new { OrderID = h.ID, CustomerID = h.CustomerID },
                        c => new { OrderID = c.OrderID, CustomerID = c.CustomerID },
                        (h, c) => new OrderList()
                        {
                            ID = h.ID,
                            CustomerID = h.CustomerID,
                            OrderType = h.OrderType,
                            ExcuteStatus = h.ExcuteStatus,
                            PaymentStatus = h.PaymentStatus,
                            PaymentType = h.PaymentType,
                            ShippingType = h.ShippingType,
                            TotalPrice = h.TotalPrice,
                            TotalDiscount = h.TotalDiscount,
                            FeeShipping = h.FeeShipping,
                            CreatedBy = h.CreatedBy,
                            CreatedDate = h.CreatedDate,
                            DateDone = h.DateDone,
                            OrderNote = h.OrderNote,
                            RefundsGoodsID = h.RefundsGoodsID,
                            ShippingCode = h.ShippingCode,
                            TransportCompanyID = h.TransportCompanyID,
                            TransportCompanySubID = h.TransportCompanySubID,
                            PostalDeliveryType = h.PostalDeliveryType,
                            // Order Detail
                            Quantity = h.Quantity,
                            // Customer
                            CustomerName = c.CustomerName,
                            Nick = c.Nick,
                            CustomerPhone = c.CustomerPhone
                        }
                    )
                    .GroupJoin(
                        refunds,
                        h => new { OrderID = h.ID, RefundsGoodsID = h.RefundsGoodsID },
                        rf => new { OrderID = rf.OrderID, RefundsGoodsID = rf.RefundsGoodsID },
                        (h, rf) => new { h, rf }
                    )
                    .SelectMany(
                        x => x.rf.DefaultIfEmpty(),
                        (parent, child) => new OrderList()
                        {
                            ID = parent.h.ID,
                            CustomerID = parent.h.CustomerID,
                            OrderType = parent.h.OrderType,
                            ExcuteStatus = parent.h.ExcuteStatus,
                            PaymentStatus = parent.h.PaymentStatus,
                            PaymentType = parent.h.PaymentType,
                            ShippingType = parent.h.ShippingType,
                            TotalPrice = parent.h.TotalPrice,
                            TotalDiscount = parent.h.TotalDiscount,
                            FeeShipping = parent.h.FeeShipping,
                            CreatedBy = parent.h.CreatedBy,
                            CreatedDate = parent.h.CreatedDate,
                            DateDone = parent.h.DateDone,
                            OrderNote = parent.h.OrderNote,
                            RefundsGoodsID = parent.h.RefundsGoodsID,
                            ShippingCode = parent.h.ShippingCode,
                            TransportCompanyID = parent.h.TransportCompanyID,
                            TransportCompanySubID = parent.h.TransportCompanySubID,
                            PostalDeliveryType = parent.h.PostalDeliveryType,
                            // Order Detail
                            Quantity = parent.h.Quantity,
                            // Customer
                            CustomerName = parent.h.CustomerName,
                            Nick = parent.h.Nick,
                            CustomerPhone = parent.h.CustomerPhone,
                            // Refunds
                            TotalRefund = child != null ? Convert.ToDouble(child.TotalPrice) : 0
                        }
                    )
                    .GroupJoin(
                        fee,
                        h => h.ID,
                        f => f.OrderID,
                        (h, f) => new { h, f }
                    )
                    .SelectMany(
                        x => x.f.DefaultIfEmpty(),
                        (parent, child) => new OrderList()
                        {
                            ID = parent.h.ID,
                            CustomerID = parent.h.CustomerID,
                            OrderType = parent.h.OrderType,
                            ExcuteStatus = parent.h.ExcuteStatus,
                            PaymentStatus = parent.h.PaymentStatus,
                            PaymentType = parent.h.PaymentType,
                            ShippingType = parent.h.ShippingType,
                            TotalPrice = parent.h.TotalPrice,
                            TotalDiscount = parent.h.TotalDiscount,
                            FeeShipping = parent.h.FeeShipping,
                            CreatedBy = parent.h.CreatedBy,
                            CreatedDate = parent.h.CreatedDate,
                            DateDone = parent.h.DateDone,
                            OrderNote = parent.h.OrderNote,
                            RefundsGoodsID = parent.h.RefundsGoodsID,
                            ShippingCode = parent.h.ShippingCode,
                            TransportCompanyID = parent.h.TransportCompanyID,
                            TransportCompanySubID = parent.h.TransportCompanySubID,
                            PostalDeliveryType = parent.h.PostalDeliveryType,
                            // Order Detail
                            Quantity = parent.h.Quantity,
                            // Customer
                            CustomerName = parent.h.CustomerName,
                            Nick = parent.h.Nick,
                            CustomerPhone = parent.h.CustomerPhone,
                            // Refunds
                            TotalRefund = parent.h.TotalRefund,
                            // Fee Other
                            OtherFeeName = child != null ? child.OtherFeeName : String.Empty,
                            OtherFeeValue = child != null ? Convert.ToDouble(child.OtherFeeValue) : 0
                        }
                    )
                    .GroupJoin(
                        trans,
                        h => h.ID,
                        t => t.OrderID,
                        (h, t) => new { h, t }
                    )
                    .SelectMany(
                        x => x.t.DefaultIfEmpty(),
                        (parent, child) => {
                            var result = new OrderList()
                            {
                                ID = parent.h.ID,
                                CustomerID = parent.h.CustomerID,
                                OrderType = parent.h.OrderType,
                                ExcuteStatus = parent.h.ExcuteStatus,
                                PaymentStatus = parent.h.PaymentStatus,
                                PaymentType = parent.h.PaymentType,
                                ShippingType = parent.h.ShippingType,
                                TotalPrice = parent.h.TotalPrice,
                                TotalDiscount = parent.h.TotalDiscount,
                                FeeShipping = parent.h.FeeShipping,
                                CreatedBy = parent.h.CreatedBy,
                                CreatedDate = parent.h.CreatedDate,
                                DateDone = parent.h.DateDone,
                                OrderNote = parent.h.OrderNote,
                                RefundsGoodsID = parent.h.RefundsGoodsID,
                                ShippingCode = parent.h.ShippingCode,
                                TransportCompanyID = parent.h.TransportCompanyID,
                                TransportCompanySubID = parent.h.TransportCompanySubID,
                                PostalDeliveryType = parent.h.PostalDeliveryType,
                                // Order Detail
                                Quantity = parent.h.Quantity,
                                // Customer
                                CustomerName = parent.h.CustomerName,
                                Nick = parent.h.Nick,
                                CustomerPhone = parent.h.CustomerPhone,
                                // Refunds
                                TotalRefund = parent.h.TotalRefund,
                                // Fee Other
                                OtherFeeName = parent.h.OtherFeeName,
                                OtherFeeValue = parent.h.OtherFeeValue,
                            };

                            if (child != null)
                            {
                                // Transfer Bank
                                result.CusBankID = child.CusBankID;
                                result.CusBankName = child.CusBankName;
                                result.AccBankID = child.AccBankID;
                                result.AccBankName = child.AccBankName;
                                result.MoneyReceive = child.Money;
                                result.TransferStatus = child.Status;
                                result.StatusName = child.Status == 1 ? "Đã nhận tiền" : "Chưa nhận tiền";
                                result.DoneAt = child.DoneAt;
                                result.TransferNote = child.Note;
                            }

                            return result;
                        }
                    )
                    .GroupJoin(
                        deliveries,
                        h => h.ID,
                        d => d.OrderID,
                        (h, d) => new { h, d }
                    )
                    .SelectMany(
                        x => x.d.DefaultIfEmpty(),
                        (parent, child) => {
                            var result = new OrderList()
                            {
                                ID = parent.h.ID,
                                CustomerID = parent.h.CustomerID,
                                OrderType = parent.h.OrderType,
                                ExcuteStatus = parent.h.ExcuteStatus,
                                PaymentStatus = parent.h.PaymentStatus,
                                PaymentType = parent.h.PaymentType,
                                ShippingType = parent.h.ShippingType,
                                TotalPrice = parent.h.TotalPrice,
                                TotalDiscount = parent.h.TotalDiscount,
                                FeeShipping = parent.h.FeeShipping,
                                CreatedBy = parent.h.CreatedBy,
                                CreatedDate = parent.h.CreatedDate,
                                DateDone = parent.h.DateDone,
                                OrderNote = parent.h.OrderNote,
                                RefundsGoodsID = parent.h.RefundsGoodsID,
                                ShippingCode = parent.h.ShippingCode,
                                TransportCompanyID = parent.h.TransportCompanyID,
                                TransportCompanySubID = parent.h.TransportCompanySubID,
                                PostalDeliveryType = parent.h.PostalDeliveryType,
                                // Order Detail
                                Quantity = parent.h.Quantity,
                                // Customer
                                CustomerName = parent.h.CustomerName,
                                Nick = parent.h.Nick,
                                CustomerPhone = parent.h.CustomerPhone,
                                // Refunds
                                TotalRefund = parent.h.TotalRefund,
                                // Fee Other
                                OtherFeeName = parent.h.OtherFeeName,
                                OtherFeeValue = parent.h.OtherFeeValue,
                                // Transfer Bank
                                CusBankID = parent.h.CusBankID,
                                CusBankName = parent.h.CusBankName,
                                AccBankID = parent.h.AccBankID,
                                AccBankName = parent.h.AccBankName,
                                MoneyReceive = parent.h.MoneyReceive,
                                TransferStatus = parent.h.TransferStatus,
                                StatusName = parent.h.StatusName,
                                DoneAt = parent.h.DoneAt,
                                TransferNote = parent.h.TransferNote,
                            };

                            if (child != null)
                            {
                                // Delivery
                                result.DeliveryDate = child.StartAt;
                                result.DeliveryStatus = child.Status;
                                result.ShipperID = child.ShipperID;
                                result.CostOfDelivery = child.COD;
                                result.CollectionOfOrder = child.COO;
                                result.ShipNote = child.ShipNote;
                                result.InvoiceImage = child.Image;
                                result.ShipperName = child.ShipperName;
                            }
                            else
                            {
                                result.DeliveryStatus = 2;
                            }

                            return result;
                        }
                    )
                    .Where(x => 1 == 1);

                if (!String.IsNullOrEmpty(OtherFee))
                {
                    if (OtherFee.Equals("yes"))
                    {
                        data = data.Where(x => x.OtherFeeValue != 0);
                    }
                    else
                    {
                        data = data.Where(x => x.OtherFeeValue == 0);
                    }
                }

                var list = data.OrderByDescending(o => o.ID).ToList();

                return list;
            }
        }

        public static List<tbl_Order> SearchByStatical(int orderType, int PaymentStatus, int ExcuteStatus, string s, int agentID, int PaymentType, int ShippingType, string sku)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_Order> rs = new List<tbl_Order>();
                List<tbl_Order> end = new List<tbl_Order>();
                if (!string.IsNullOrEmpty(s))
                {
                    if (orderType > 0)
                    {
                        if (PaymentStatus > 0)
                        {
                            if (ExcuteStatus > 0)
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                            else
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (ExcuteStatus > 0)
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                            else
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentType == PaymentType && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.PaymentType == PaymentType && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.PaymentType == PaymentType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.ID.ToString().Contains(s)
                                    || r.OrderType == orderType && r.CustomerName.Contains(s)
                                    || r.OrderType == orderType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (PaymentStatus > 0)
                        {
                            if (ExcuteStatus > 0)
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ID.ToString().Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.CustomerName.Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.ID.ToString().Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.CustomerName.Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                            else
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType && r.ID.ToString().Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType && r.CustomerName.Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus && r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus && r.ID.ToString().Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.CustomerName.Contains(s)
                                    || r.PaymentStatus == PaymentStatus && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (ExcuteStatus > 0)
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ID.ToString().Contains(s)
                                    || r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.CustomerName.Contains(s)
                                    || r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.ExcuteStatus == ExcuteStatus && r.ID.ToString().Contains(s)
                                    || r.ExcuteStatus == ExcuteStatus && r.CustomerName.Contains(s)
                                    || r.ExcuteStatus == ExcuteStatus && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                            else
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.PaymentType == PaymentType && r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentType == PaymentType && r.ID.ToString().Contains(s)
                                    || r.PaymentType == PaymentType && r.CustomerName.Contains(s)
                                    || r.PaymentType == PaymentType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.ShippingType == ShippingType && r.ID.ToString().Contains(s)
                                    || r.ShippingType == ShippingType && r.CustomerName.Contains(s)
                                    || r.ShippingType == ShippingType && r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.ID.ToString().Contains(s)
                                    || r.CustomerName.Contains(s)
                                    || r.CustomerPhone.Contains(s))
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (orderType > 0)
                    {
                        if (PaymentStatus > 0)
                        {
                            if (ExcuteStatus > 0)
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                            else
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus && r.ShippingType == ShippingType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentStatus == PaymentStatus)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (ExcuteStatus > 0)
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.ExcuteStatus == ExcuteStatus)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                            else
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentType == PaymentType && r.ShippingType == ShippingType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.PaymentType == PaymentType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType && r.ShippingType == ShippingType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.OrderType == orderType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (PaymentStatus > 0)
                        {
                            if (ExcuteStatus > 0)
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus && r.ExcuteStatus == ExcuteStatus)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                            else
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus && r.PaymentType == PaymentType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus && r.ShippingType == ShippingType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentStatus == PaymentStatus)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (ExcuteStatus > 0)
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType && r.ShippingType == ShippingType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.ExcuteStatus == ExcuteStatus && r.PaymentType == PaymentType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.ExcuteStatus == ExcuteStatus && r.ShippingType == ShippingType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.ExcuteStatus == ExcuteStatus)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                            else
                            {
                                if (PaymentType > 0)
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentType == PaymentType && r.ShippingType == ShippingType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.PaymentType == PaymentType)
                                    .OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                                else
                                {
                                    if (ShippingType > 0)
                                    {
                                        rs = dbe.tbl_Order
                                    .Where(r => r.ShippingType == ShippingType).OrderByDescending(r => r.ID).ToList();
                                    }
                                    else
                                    {
                                        rs = dbe.tbl_Order.OrderByDescending(r => r.ID).ToList();
                                    }
                                }
                            }
                        }
                    }
                }


                if (!string.IsNullOrEmpty(sku))
                {
                    var product = dbe.tbl_OrderDetail.Where(x => x.SKU == sku).ToList();
                    if (product != null)
                    {
                        for (int i = 0; i < product.Count(); i++)
                        {
                            for (int j = 0; j < rs.Count(); j++)
                            {
                                if (product[i].OrderID == rs[j].ID)
                                {
                                    end.Add(rs[j]);
                                }
                            }
                        }
                    }
                    return end.Where(r => r.AgentID == agentID).OrderByDescending(r => r.ID).ToList();
                }
                else
                {
                    return rs.Where(r => r.AgentID == agentID).OrderByDescending(r => r.ID).ToList();
                }
            }

        }

        public static List<tbl_Order> GetByCustomerID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_Order> las = new List<tbl_Order>();
                las = dbe.tbl_Order.Where(x => x.CustomerID == ID).OrderByDescending(x => x.ID).ToList();
                return las;
            }
        }

        public static List<tbl_Order> GetByCustomerID(int ID, int ExcuteStatus)
        {
            using (var con = new inventorymanagementEntities())
            {
                var las = con.tbl_Order
                    .Where(x => x.CustomerID == ID)
                    .Where(x => x.ExcuteStatus == ExcuteStatus)
                    .OrderByDescending(x => x.ID)
                    .ToList();
                return las;
            }
        }
        public static List<OrderReportHomePage> ReportHomePage(DateTime fromDate, DateTime toDate)
        {
            var result = new List<OrderReportHomePage>();

            using (var con = new inventorymanagementEntities())
            {

                var customers = con.tbl_Customer.OrderBy(x => x.ID);

                result = customers
                    .Select(
                        x => new OrderReportHomePage()
                        {
                            ID = x.ID,
                            CustomerName = x.CustomerName,
                            Nick = x.Nick,
                            CreatedBy = x.CreatedBy,
                        }
                    )
                    .OrderBy(x => x.ID)
                    .ToList();

                // Get info order
                var orderInfo = con.tbl_Order
                    .Where(x => (x.DateDone >= fromDate && x.DateDone <= toDate)
                                && x.ExcuteStatus == 2
                                && x.PaymentStatus == 3)
                    .Join(
                        customers,
                        order => order.CustomerID,
                        customer => customer.ID,
                        (order, customer) => order
                    )
                    .Join(
                        con.tbl_OrderDetail,
                        order => order.ID,
                        detail => detail.OrderID,
                        (order, detail) => new
                        {
                            CustomerID = order.CustomerID,
                            Order = order.ID,
                            Quantity = detail.Quantity.Value
                        }
                    )
                    .GroupBy(x => x.CustomerID)
                    .Select(g => new
                    {
                        CustomerID = g.Key.Value,
                        Quantity = g.Sum(x => x.Quantity)
                    }
                    )
                    .OrderBy(x => x.CustomerID)
                    .ToList();

                result.GroupJoin(
                        orderInfo,
                        customer => customer.ID,
                        order => order.CustomerID,
                        (customer, order) => new { customer, order }
                    )
                    .SelectMany(
                        x => x.order.DefaultIfEmpty(),
                        (parent, child) =>
                        {
                            parent.customer.Quantity = child != null ? child.Quantity : 0;

                            return parent.customer;
                        }
                    )
                    .OrderBy(x => x.ID)
                    .ToList();
            }

            result = result.Where(x => x.Quantity > 0).OrderByDescending(x => x.Quantity).Take(10).ToList();

            return result;
        }

        public class OrderReportHomePage
        {
            public int ID { get; set; }
            public string CustomerName { get; set; }
            public string Nick { get; set; }
            public double Quantity { get; set; }
            public string CreatedBy { get; set; }

        }
        public static List<tbl_Order> Report(string fromdate, string todate)
        {
            using (var db = new inventorymanagementEntities())
            {
                List<tbl_Order> or = new List<tbl_Order>();
                if (!string.IsNullOrEmpty(fromdate))
                {
                    if (!string.IsNullOrEmpty(todate))
                    {
                        DateTime fd = Convert.ToDateTime(fromdate);
                        DateTime td = Convert.ToDateTime(todate);
                        or = db.tbl_Order
                            .Where(r => r.DateDone >= fd && r.DateDone <= td)
                            .Where(r => r.ExcuteStatus == 2)
                            .Where(r => r.PaymentStatus == 3)
                            .ToList();
                    }
                    else
                    {
                        DateTime fd = Convert.ToDateTime(fromdate);
                        or = db.tbl_Order
                            .Where(r => r.CreatedDate >= fd)
                            .Where(r => r.ExcuteStatus == 2)
                            .Where(r => r.PaymentStatus == 3)
                            .ToList();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(todate))
                    {
                        DateTime td = Convert.ToDateTime(todate);
                        or = db.tbl_Order
                            .Where(r => r.CreatedDate <= td)
                            .Where(r => r.ExcuteStatus == 2)
                            .Where(r => r.PaymentStatus == 3)
                            .ToList();
                    }
                    else
                    {
                        or = db.tbl_Order
                            .Where(r => r.ExcuteStatus == 2)
                            .Where(r => r.PaymentStatus == 3)
                            .ToList();
                    }
                }
                return or;
            }
        }

        public static long GetTotalPriceByAccount(string accountName, DateTime fromdate, DateTime todate)
        {
            using (var con = new inventorymanagementEntities())
            {
                return con.tbl_Order
                    .Where(x => (fromdate <= x.DateDone && x.DateDone <= todate)
                                && x.CreatedBy.Trim().ToUpper() == accountName.Trim().ToUpper()
                                && (x.ExcuteStatus == 2 && x.PaymentStatus == 3)
                           )
                     .ToList()
                     .Sum(x => Convert.ToInt64(x.TotalPrice));
            }
        }

        public static int GetTotalProductSalesByAccount(string accountName, DateTime fromdate, DateTime todate)
        {
            using (var con = new inventorymanagementEntities())
            {
                List<tbl_Order> or = new List<tbl_Order>();
                or = con.tbl_Order
                    .Where(x => (fromdate <= x.DateDone && x.DateDone <= todate)
                                && x.CreatedBy.Trim().ToUpper() == accountName.Trim().ToUpper()
                                && (x.ExcuteStatus == 2 && x.PaymentStatus == 3)
                           )
                     .ToList();
                int tongbanra = 0;
                if (or != null)
                {
                    foreach (var item in or)
                    {
                        var oddetail = OrderDetailController.GetByOrderID(item.ID);
                        if (oddetail != null)
                        {
                            foreach (var temp in oddetail)
                            {
                                tongbanra += Convert.ToInt32(temp.Quantity);
                            }
                        }
                    }
                }
                return tongbanra;
            }
        }

        #endregion

        public class OrderReport
        {
            public int ID { get; set; }
            public int Quantity { get; set; }
            public double TotalRevenue { get; set; }
            public double TotalCost { get; set; }
        }
        public class OrderList
        {
            public int ID { get; set; }
            public int OrderType { get; set; }
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public string Nick { get; set; }
            public int CustomerID { get; set; }
            public double TotalPrice { get; set; }
            public double TotalDiscount { get; set; }
            public double TotalRefund { get; set; }
            public double FeeShipping { get; set; }
            public string OtherFeeName { get; set; }
            public double OtherFeeValue { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public Nullable<System.DateTime> DateDone { get; set; }
            public int PaymentStatus { get; set; }
            public int ExcuteStatus { get; set; }
            public int PaymentType { get; set; }
            public int ShippingType { get; set; }
            public Nullable<int> RefundsGoodsID { get; set; }
            public int Quantity { get; set; }
            public string ShippingCode { get; set; }
            public Nullable<int> TransportCompanyID { get; set; }
            public Nullable<int> TransportCompanySubID { get; set; }
            public string OrderNote { get; set; }
            public int PostalDeliveryType { get; set; }

            // Custom Transfer Bank
            public int? CusBankID { get; set; }
            public string CusBankName { get; set; }
            public int? AccBankID { get; set; }
            public string AccBankName { get; set; }
            public Decimal? MoneyReceive { get; set; }
            public int? TransferStatus { get; set; }
            public string StatusName { get; set; }
            public DateTime? DoneAt { get; set; }
            public string TransferNote { get; set; }

            // Custom Delivery
            public DateTime? DeliveryDate { get; set; }
            public int? DeliveryStatus { get; set; }
            public int? ShipperID { get; set; }
            public Decimal? CostOfDelivery { get; set; }
            public Decimal? CollectionOfOrder { get; set; }
            public string ShipNote { get; set; }
            public string ShipperName { get; set; }
            public string InvoiceImage { get; set; }
            public Boolean? CheckDelivery { get; set; }
        }

        public class OrderSQL
        {
            public int ID { get; set; }
            public int OrderType { get; set; }
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public int TotalPrice { get; set; }
            public string CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public string DateDone { get; set; }

            public int PaymentStatus { get; set; }
            public int ExcuteStatus { get; set; }
            public int PaymentType { get; set; }
            public int ShippingType { get; set; }

        }

        public static ProfitReportModel GetProfitReport(DateTime fromDate, DateTime toDate)
        {
            using (var con = new inventorymanagementEntities())
            {
                int TotalNumberOfOrder = 0;
                double TotalSalePrice = 0;
                double TotalSaleCost = 0;
                double TotalSaleDiscount = 0;
                double TotalOtherFee = 0;
                double TotalShippingFee = 0;

                double TotalRefundPrice = 0;
                double TotalRefundFee = 0;
                double TotalRefundCost = 0;

                // Get all infor product
                var productTarget = con.tbl_Product
                    .GroupJoin(
                        con.tbl_ProductVariable,
                        product => new
                        {
                            ProductStyle = product.ProductStyle.Value,
                            ProductID = product.ID,
                        },
                        productVariable => new
                        {
                            ProductStyle = 2,
                            ProductID = productVariable.ProductID.Value,
                        },
                        (product, productVariable) => new
                        {
                            product,
                            productVariable
                        })
                    .SelectMany(x => x.productVariable.DefaultIfEmpty(),
                                (parent, child) => new
                                {
                                    CategoryID = parent.product.CategoryID.Value,
                                    ProductID = parent.product.ID,
                                    ProductVariableID = child != null ? child.ID : 0,
                                    ProductStyle = parent.product.ProductStyle.Value,
                                    ProductImage = child != null ? child.Image : parent.product.ProductImage,
                                    ProductTitle = parent.product.ProductTitle,
                                    VariableValue = String.Empty,
                                    SKU = child != null ? child.SKU : parent.product.ProductSKU,
                                    RegularPrice = child != null ? child.Regular_Price.Value : parent.product.Regular_Price.Value,
                                    CostOfGood = child != null ? child.CostOfGood.Value : parent.product.CostOfGood.Value,
                                    RetailPrice = child != null ? child.RetailPrice.Value : parent.product.Retail_Price.Value
                                })
                    .OrderBy(x => x.SKU);

                // Choose order in date target
                var orderTarget = con.tbl_Order
                    .Where(x => (x.DateDone >= fromDate && x.DateDone <= toDate)
                                && x.ExcuteStatus == 2
                                && x.PaymentStatus == 3)
                    .OrderBy(x => x.ID);

                var orderDetailTarget = con.tbl_OrderDetail.OrderBy(x => x.OrderID).ThenBy(x => x.ID);

                // Get info order
                var inforOrder = orderTarget
                    .Join(
                        orderDetailTarget,
                        order => order.ID,
                        detail => detail.OrderID,
                        (order, detail) => new
                        {
                            OrderID = order.ID,
                            ProductID = detail.ProductID.Value,
                            ProductVariableID = detail.ProductVariableID.Value,
                            ProductStyle = detail.ProductType.Value,
                            SKU = detail.SKU,
                            Price = detail.Price,
                            Quantity = detail.Quantity.Value,
                            TotalPrice = order.TotalPrice,
                            TotalDiscount = order.TotalDiscount,
                            TotalOtherFee = order.OtherFeeValue,
                            TotalShippingFee = order.FeeShipping
                        })
                        .OrderBy(x => x.SKU)
                    .Join(
                        productTarget,
                        order => order.SKU,
                        product => product.SKU,
                        (order, product) => new
                        {
                            OrderID = order.OrderID,
                            ProductID = order.ProductID,
                            ProductVariableID = order.ProductVariableID,
                            ProductStyle = order.ProductStyle,
                            SKU = order.SKU,
                            Price = order.Price,
                            Quantity = order.Quantity,
                            CostOfGood = product.CostOfGood,
                            TotalPrice = order.TotalPrice,
                            TotalDiscount = order.TotalDiscount,
                            TotalOtherFee = order.TotalOtherFee,
                            TotalShippingFee = order.TotalShippingFee
                        })
                    .ToList();

                // Choose refund good in date target
                var refundTarget = con.tbl_RefundGoods
                    .Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate)
                    .OrderBy(x => x.ID);

                var refundDetailTarget = con.tbl_RefundGoodsDetails.OrderBy(x => x.RefundGoodsID).ThenBy(x => x.ID);

                var infoRefund = refundTarget
                    .Join(
                        refundDetailTarget,
                        refund => refund.ID,
                        detail => detail.RefundGoodsID,
                        (refund, detail) => new
                        {
                            RefundGoodsID = refund.ID,
                            ProductStyle = detail.ProductType.Value,
                            SKU = detail.SKU,
                            SoldPrice = detail.SoldPricePerProduct,
                            Quantity = detail.Quantity.Value,
                            TotalRefundPrice = refund.TotalPrice,
                            TotalRefundFee = refund.TotalRefundFee
                        })
                    .OrderBy(x => x.SKU)
                    .Join(
                        productTarget,
                        refund => refund.SKU,
                        product => product.SKU,
                        (refund, product) => new
                        {
                            RefundGoodsID = refund.RefundGoodsID,
                            ProductStyle = refund.ProductStyle,
                            SKU = refund.SKU,
                            Quantity = refund.Quantity,
                            SoldPrice = refund.SoldPrice,
                            CostOfGood = product.CostOfGood,
                            TotalRefundPrice = refund.TotalRefundPrice,
                            TotalRefundFee = refund.TotalRefundFee
                        })
                      .ToList();

                TotalNumberOfOrder = inforOrder.GroupBy(x => x.OrderID).Count();

                TotalSalePrice = inforOrder
                    .Select(x => new { TotalSalePrice = x.Quantity * Convert.ToDouble(x.Price) })
                    .Sum(x => x.TotalSalePrice);

                TotalSaleCost = inforOrder
                    .Select(x => new { TotalSaleCost = x.Quantity * Convert.ToDouble(x.CostOfGood) })
                    .Sum(x => x.TotalSaleCost);

                TotalSaleDiscount = inforOrder
                    .GroupBy(x => x.OrderID)
                    .Select(g => new { TotalSaleDiscount = g.Max(x => Convert.ToDouble(x.TotalDiscount)) })
                    .Sum(x => x.TotalSaleDiscount);

                TotalOtherFee = inforOrder
                    .GroupBy(x => x.OrderID)
                    .Select(g => new { TotalOtherFee = g.Max(x => Convert.ToDouble(x.TotalOtherFee)) })
                    .Sum(x => x.TotalOtherFee);

                TotalShippingFee = inforOrder
                    .GroupBy(x => x.OrderID)
                    .Select(g => new { TotalShippingFee = g.Max(x => Convert.ToDouble(x.TotalShippingFee)) })
                    .Sum(x => x.TotalShippingFee);

                TotalRefundPrice = infoRefund
                    .Select(x => new { TotalRefundPrice = x.Quantity * Convert.ToDouble(x.SoldPrice) })
                    .Sum(x => x.TotalRefundPrice);

                TotalRefundCost = infoRefund
                    .Select(x => new { TotalRefundCost = x.Quantity * Convert.ToDouble(x.CostOfGood) })
                    .Sum(x => x.TotalRefundCost);

                TotalRefundFee = infoRefund
                    .GroupBy(x => x.RefundGoodsID)
                    .Select(g => new { TotalRefundFee = g.Max(x => Convert.ToDouble(x.TotalRefundFee)) })
                    .Sum(x => x.TotalRefundFee);

                return new ProfitReportModel()
                {
                    TotalNumberOfOrder = TotalNumberOfOrder,
                    TotalSalePrice = TotalSalePrice,
                    TotalSaleCost = TotalSaleCost,
                    TotalSaleDiscount = TotalSaleDiscount,
                    TotalOtherFee = TotalOtherFee,
                    TotalShippingFee = TotalShippingFee,
                    TotalRefundPrice = TotalRefundPrice,
                    TotalRefundCost = TotalRefundCost,
                    TotalRefundFee = TotalRefundFee
                };
            }
        }

        public static ProductReportModel getProductReport(string SKU, DateTime fromDate, DateTime toDate)
        {
            var list = new List<OrderReport>();
            var sql = new StringBuilder();

            sql.AppendLine(String.Format("SELECT Ord.ID, SUM(ISNULL(OrdDetail.Quantity, 0)) AS Quantity, SUM(OrdDetail.Quantity * ISNULL(Product.CostOfGood, Variable.CostOfGood)) AS TotalCost, SUM(OrdDetail.Quantity * (OrdDetail.Price - Ord.DiscountPerProduct)) AS TotalRevenue"));
            sql.AppendLine(String.Format("FROM tbl_Order AS Ord"));
            sql.AppendLine(String.Format("INNER JOIN tbl_OrderDetail AS OrdDetail"));
            sql.AppendLine(String.Format("ON     Ord.ID = OrdDetail.OrderID"));
            sql.AppendLine(String.Format("LEFT JOIN tbl_ProductVariable AS Variable"));
            sql.AppendLine(String.Format("ON     OrdDetail.SKU = Variable.SKU"));
            sql.AppendLine(String.Format("LEFT JOIN tbl_Product AS Product"));
            sql.AppendLine(String.Format("ON     OrdDetail.SKU = Product.ProductSKU"));
            sql.AppendLine(String.Format("WHERE 1 = 1"));
            sql.AppendLine(String.Format("    AND Ord.ExcuteStatus = 2"));
            sql.AppendLine(String.Format("    AND Ord.PaymentStatus = 3"));
            sql.AppendLine(String.Format("    AND OrdDetail.SKU LIKE '{0}%'", SKU));
            sql.AppendLine(String.Format("    AND    CONVERT(datetime, Ord.DateDone, 121) BETWEEN CONVERT(datetime, '{0}', 121) AND CONVERT(datetime, '{1}', 121)", fromDate.ToString(), toDate.ToString()));
            sql.AppendLine(String.Format("GROUP BY Ord.ID"));

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql.ToString());
            while (reader.Read())
            {
                var entity = new OrderReport();

                entity.ID = Convert.ToInt32(reader["ID"]);
                entity.Quantity = Convert.ToInt32(reader["Quantity"]);
                entity.TotalRevenue = Convert.ToDouble(reader["TotalRevenue"]);
                entity.TotalCost = Convert.ToDouble(reader["TotalCost"]);
                list.Add(entity);
            }
            reader.Close();

            return new ProductReportModel()
            {
                totalSold = list.Sum(x => x.Quantity),
                totalRevenue = list.Sum(x => x.TotalRevenue),
                totalCost = list.Sum(x => x.TotalCost)
            };
        }

        public class ProductReportModel
        {
            public int totalSold { get; set; }
            public double totalRevenue { get; set; }
            public double totalCost { get; set; }
        }

        public static string getLastJSON(int customerID)
        {
            using (var con = new inventorymanagementEntities())
            {
                var serializer = new JavaScriptSerializer();
                var orderLast = con.tbl_Order
                    .Where(x => x.CustomerID == customerID)
                    .OrderByDescending(o => o.ID)
                    .FirstOrDefault();

                if (orderLast != null)
                {
                    var result = new OrderLast()
                    {
                        payType = orderLast.PaymentType.HasValue ? orderLast.PaymentType.Value : 0,
                        bankID = 0,
                        bankName = String.Empty,
                        shipType = orderLast.ShippingType.HasValue ? orderLast.ShippingType.Value : 0,
                        tranID = 0,
                        tranName = String.Empty,
                        tranSubID = 0,
                        tranSubName = String.Empty
                    };

                    // Lấy thông tin ngân hàng
                    if (orderLast.PaymentType == 2)
                    {
                        var bankLast = con.BankTransfers
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
                         .Select(x => x.bank)
                         .FirstOrDefault();

                        if (bankLast != null)
                        {
                            result.bankID = bankLast.ID;
                            result.bankName = bankLast.BankName;
                        }
                    }

                    // Lấy thông tin chành xe và nơi tới
                    if (orderLast.ShippingType == 4)
                    {
                        var tranLast = con.tbl_Order
                           .Where(x => x.ShippingType == 4) // Hình thức nhà xe
                           .Where(x => x.CustomerID == customerID)
                           .Join(
                               con.tbl_TransportCompany,
                               ord => new { tranID = ord.TransportCompanyID.Value, tranSubID = ord.TransportCompanySubID.Value },
                               tran => new { tranID = tran.ID, tranSubID = tran.SubID },
                               (ord, tran) => new { ord, tran }
                           )
                           .OrderByDescending(o => o.ord.ID)
                           .Select(x => x.tran)
                           .FirstOrDefault();

                        if (tranLast != null)
                        {
                            result.tranID = tranLast.ID;
                            result.tranName = tranLast.CompanyName;
                            result.tranSubID = tranLast.SubID;
                            result.tranSubName = tranLast.ShipTo;
                        }
                    }

                    return serializer.Serialize(result);
                }

                return serializer.Serialize(null);
            }
        }

        public class OrderLast
        {
            public int payType { get; set; }
            public int bankID { get; set; }
            public string bankName { get; set; }
            public int shipType { get; set; }
            public int tranID { get; set; }
            public string tranName { get; set; }
            public int tranSubID { get; set; }
            public string tranSubName { get; set; }
        }
    }
}