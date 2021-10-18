using IM_PJ.Models;
using IM_PJ.Models.Pages.thong_ke_doanh_thu_khach_hang;
using Newtonsoft.Json;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.SqlServer;
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
        #region Private
        private static ViewOrder _createViewOrderByOrder(tbl_Order order)
        {
            using (var con = new inventorymanagementEntities())
            {
                var now = DateTime.Now;
                var user = con.tbl_Customer
                    .Where(x => x.ID == order.CustomerID)
                    .SingleOrDefault();
                var item = new ViewOrder()
                {
                    Phone = user.CustomerPhone,
                    Status = order.ExcuteStatus.HasValue ? order.ExcuteStatus.Value : (int)ExcuteStatus.Doing,
                    OrderId = order.ID,
                    CreatedBy = order.CreatedBy,
                    CreatedDate = now,
                    ModifiedBy = order.CreatedBy,
                    ModifiedDate = now
                };

                con.Entry(item).State = System.Data.Entity.EntityState.Added;
                con.ViewOrders.Add(item);
                con.SaveChanges();

                return item;
            }
        }

        private static ViewOrder _updateViewOrderByOrder(tbl_Order order)
        {
            using (var con = new inventorymanagementEntities())
            {
                var item = con.ViewOrders
                    .Where(x => x.OrderId.HasValue)
                    .Where(x => x.OrderId == order.ID)
                    .SingleOrDefault();

                if (item == null)
                    return null;

                con.Entry(item).State = System.Data.Entity.EntityState.Modified;
                item.Status = order.ExcuteStatus.HasValue ? order.ExcuteStatus.Value : (int)ExcuteStatus.Doing;
                item.ModifiedBy = String.IsNullOrEmpty(order.ModifiedBy) ? order.CreatedBy : order.ModifiedBy;
                item.ModifiedDate = DateTime.Now;
                con.SaveChanges();

                return item;
            }
        }
        #endregion

        #region CRUD
        public static tbl_Order Insert(tbl_Order data)
        {
            using (var con = new inventorymanagementEntities())
            {
                #region Khởi tạo thông tin đơn hàng
                con.Entry(data).State = System.Data.Entity.EntityState.Added;
                con.tbl_Order.Add(data);
                con.SaveChanges();
                #endregion

                #region Khởi tạo thông tin danh sách đơn hàng
                _createViewOrderByOrder(data);
                #endregion

                return data;
            }
        }

        public static tbl_Order InsertOnSystem(tbl_Order data)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                #region Khởi tạo thông tin đơn hàng
                dbe.Entry(data).State = System.Data.Entity.EntityState.Added;
                dbe.tbl_Order.Add(data);
                dbe.SaveChanges();
                #endregion

                #region Khởi tạo thông tin danh sách đơn hàng
                _createViewOrderByOrder(data);
                #endregion

                return data;
            }
        }

        #region Xử lý với Refund
        public static int UpdateRefund(int ID, int? RefundsGoodsID, string created)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                var ui = dbe.tbl_Order
                    .Where(x => x.ID == ID)
                    .SingleOrDefault();

                if (ui == null)
                    return 0;

                dbe.Entry(ui).State = System.Data.Entity.EntityState.Modified;
                ui.RefundsGoodsID = RefundsGoodsID;
                ui.ModifiedDate = DateTime.Now;
                ui.ModifiedBy = created;
                var i = dbe.SaveChanges();

                return i;
            }
        }

        public static int DeleteOrderRefund(int RefundsGoodsID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                var ui = dbe.tbl_Order
                    .Where(x => x.RefundsGoodsID == RefundsGoodsID)
                    .FirstOrDefault();

                if (ui == null)
                    return 0;

                dbe.Entry(ui).State = System.Data.Entity.EntityState.Modified;
                ui.RefundsGoodsID = null;
                ui.ModifiedDate = DateTime.Now;
                var i = dbe.SaveChanges();

                return ui.ID;
            }
        }
        #endregion

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

        public static tbl_Order UpdateOnSystem(tbl_Order data)
        {
            using (var con = new inventorymanagementEntities())
            {
                var orderOld = con.tbl_Order
                    .Where(o => o.ID == data.ID)
                    .SingleOrDefault();

                if (orderOld == null)
                    return null;

                #region Cập thông tin đơn hàng
                con.Entry(orderOld).State = System.Data.Entity.EntityState.Modified;

                orderOld.OrderType = data.OrderType;
                orderOld.AdditionFee = data.AdditionFee;
                orderOld.DisCount = data.DisCount;
                orderOld.CustomerID = data.CustomerID;
                orderOld.CustomerName = data.CustomerName;
                orderOld.CustomerPhone = data.CustomerPhone;
                orderOld.CustomerAddress = data.CustomerAddress;
                orderOld.CustomerEmail = data.CustomerEmail;
                orderOld.TotalPrice = data.TotalPrice;
                orderOld.TotalPriceNotDiscount = data.TotalPriceNotDiscount;
                orderOld.PaymentStatus = data.PaymentStatus;
                orderOld.ExcuteStatus = data.ExcuteStatus;
                orderOld.DiscountPerProduct = data.DiscountPerProduct;
                orderOld.TotalDiscount = data.TotalDiscount;
                orderOld.FeeShipping = data.FeeShipping;
                orderOld.GuestPaid = data.GuestPaid;
                orderOld.GuestChange = data.GuestChange;
                orderOld.ModifiedDate = data.ModifiedDate;
                orderOld.CreatedBy = data.CreatedBy;
                orderOld.ModifiedBy = data.ModifiedBy;
                orderOld.PaymentType = data.PaymentType;
                orderOld.ShippingType = data.ShippingType;
                orderOld.OrderNote = data.OrderNote;
                orderOld.DateDone = data.DateDone;
                orderOld.ShippingCode = data.ShippingCode;
                orderOld.TransportCompanyID = data.TransportCompanyID;
                orderOld.TransportCompanySubID = data.TransportCompanySubID;
                orderOld.OtherFeeName = data.OtherFeeName;
                orderOld.OtherFeeValue = data.OtherFeeValue;
                orderOld.PostalDeliveryType = data.PostalDeliveryType;
                orderOld.CouponID = data.CouponID;
                orderOld.CouponValue = data.CouponValue;
                orderOld.Weight = data.Weight;
                orderOld.DeliveryAddressId = data.DeliveryAddressId;
                con.SaveChanges();
                #endregion

                #region Cập nhật thông tin danh sách đơn hàng
                _updateViewOrderByOrder(orderOld);
                #endregion

                return orderOld;
            }
        }

        #region Xử lý ExcuteStatus
        public static tbl_Order UpdateExcuteStatus(int orderID, int status, string CreatedBy)
        {
            tbl_Order order;

            using (var con = new inventorymanagementEntities())
            {
                #region Cập nhật thông tin đơn hàng
                order = con.tbl_Order
                    .Where(x => x.ID == orderID)
                    .SingleOrDefault();

                if (order == null)
                    return null;

                con.Entry(order).State = System.Data.Entity.EntityState.Modified;
                order.ExcuteStatus = status;
                order.ModifiedDate = DateTime.Now;
                order.ModifiedBy = CreatedBy;
                con.SaveChanges();
                #endregion
            }

            #region Cập nhật thông tin danh sách đơn hàng
            _updateViewOrderByOrder(order);
            #endregion

            return order;
        }

        public static int UpdateExcuteStatus(int ID, string CreatedBy)
        {
            using (var db = new inventorymanagementEntities())
            {
                #region Cập nhật thông tin đơn hàng
                var ui = db.tbl_Order
                    .Where(x => x.ID == ID)
                    .SingleOrDefault();

                if (ui == null)
                    return 0;

                db.Entry(ui).State = System.Data.Entity.EntityState.Modified;
                ui.ExcuteStatus = 3;
                ui.TotalPrice = "0";
                ui.TotalDiscount = 0;
                ui.TotalPriceNotDiscount = "0";
                ui.GuestChange = 0;
                ui.ModifiedDate = DateTime.Now;
                ui.ModifiedBy = CreatedBy;
                int i = db.SaveChanges();
                #endregion

                #region Cập nhật thông tin danh sách đơn hàng
                _updateViewOrderByOrder(ui);
                #endregion

                return i;
            }
        }

        public static bool UpdateExcuteStatus4(int ID, string CreatedBy, string OrderNote)
        {
            using (var db = new inventorymanagementEntities())
            {
                #region Cập nhật thông tin đơn hàng
                var ui = db.tbl_Order
                    .Where(x => x.ID == ID)
                    .SingleOrDefault();

                if (ui == null)
                    return false;

                db.Entry(ui).State = System.Data.Entity.EntityState.Modified;
                ui.ModifiedBy = CreatedBy;
                ui.PaymentStatus = 1;
                ui.ExcuteStatus = 4;
                ui.OrderNote = OrderNote;

                if (ui.ExcuteStatus != 4)
                    ui.ModifiedDate = DateTime.Now;

                db.SaveChanges();
                #endregion

                #region Cập nhật thông tin danh sách đơn hàng
                _updateViewOrderByOrder(ui);
                #endregion

                return true;
            }
        }
        #endregion

        public static tbl_Order updateAvatarQuantityCOGS(int orderID, string avatar = "")
        {
            using (var con = new inventorymanagementEntities())
            {
                var order = con.tbl_Order
                    .Where(x => x.ID == orderID)
                    .SingleOrDefault();

                if (order == null)
                    return null;

                var updatedData = con.tbl_OrderDetail
                    .Where(x => x.OrderID.HasValue)
                    .Where(x => x.OrderID.Value == orderID)
                    .Select(x => new
                    {
                        Quantity = x.Quantity.HasValue ? x.Quantity.Value : 0,
                        TotalCostOfGood = x.TotalCostOfGood.HasValue ? x.TotalCostOfGood.Value : 0
                    })
                    .AsEnumerable()
                    .GroupBy(g => 1)
                    .Select(x => new
                    {
                        totalQuantity = x.Sum(s => Convert.ToInt32(s.Quantity)),
                        totalCOGS = x.Sum(s => s.TotalCostOfGood)
                    })
                    .SingleOrDefault();

                con.Entry(order).State = System.Data.Entity.EntityState.Modified;
                order.Avatar = String.IsNullOrEmpty(avatar) ? null : avatar;
                order.TotalQuantity = updatedData == null ? 0 : updatedData.totalQuantity;
                order.TotalCostOfGood = updatedData == null ? 0 : Convert.ToDecimal(updatedData.totalCOGS);
                con.SaveChanges();

                return order;
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

        public static List<OrderList> Filter(OrderFilterModel filter, ref PaginationMetadataModel page)
        {
            using (var con = new inventorymanagementEntities())
            {
                #region Loại bớt data chỉ lấy những dữ liệu trong 2019-02-15
                // ẩn sản phẩm theo thời gian
                DateTime year = new DateTime(2019, 12, 15);

                var config = ConfigController.GetByTop1();

                if (config.ViewAllOrders == 1)
                    year = new DateTime(2018, 6, 22);

                if (config.ViewAllReports == 0)
                    year = DateTime.Now.AddMonths(-2);

                #region Table Order
                var orders = con.tbl_Order
                    .Where(x => x.CreatedDate.HasValue && x.CreatedDate >= year)
                    .Select(x => new
                    {
                        ID = x.ID,
                        CustomerID = x.CustomerID,
                        OrderType = x.OrderType,
                        ExcuteStatus = x.ExcuteStatus,
                        PaymentStatus = x.PaymentStatus,
                        PaymentType = x.PaymentType,
                        ShippingType = x.ShippingType,
                        TotalPrice = String.IsNullOrEmpty(x.TotalPrice) ? "0" : x.TotalPrice,
                        TotalDiscount = x.TotalDiscount.HasValue ? x.TotalDiscount.Value : 0D,
                        FeeShipping = String.IsNullOrEmpty(x.FeeShipping) ? "0" : x.FeeShipping,
                        CreatedDate = x.CreatedDate,
                        CreatedBy = x.CreatedBy,
                        DateDone = x.DateDone,
                        OrderNote = x.OrderNote,
                        RefundsGoodsID = x.RefundsGoodsID,
                        ShippingCode = x.ShippingCode,
                        TransportCompanyID = x.TransportCompanyID,
                        TransportCompanySubID = x.TransportCompanySubID,
                        PostalDeliveryType = x.PostalDeliveryType,
                        CouponID = x.CouponID,
                        CouponValue = x.CouponValue.HasValue ? x.CouponValue.Value : 0,
                        TotalQuantity = x.TotalQuantity,
                        TotalCostOfGood = x.TotalCostOfGood,
                        TotalPriceNotDiscount = String.IsNullOrEmpty(x.TotalPriceNotDiscount) ? "0" : x.TotalPriceNotDiscount,
                        DeliveryAddressId = x.DeliveryAddressId
                    });
                #endregion

                #region Table Bank Transfer
                var bankTransfers = con.BankTransfers
                    .Where(x => x.CreatedDate >= year)
                    .Select(x => new {
                        OrderID = x.OrderID,
                        CusBankID = x.CusBankID,
                        AccBankID = x.AccBankID,
                        Money = x.Money,
                        Status = x.Status,
                        DoneAt = x.DoneAt,
                        Note = x.Note,
                        CreatedDate = x.CreatedDate
                    });
                #endregion

                #region Table Delivery
                var deliveries = con.Deliveries
                    .Where(x => x.CreatedDate >= year)
                    .Select(x => new
                    {
                        OrderID = x.OrderID,
                        StartAt = x.StartAt,
                        Status = x.Status,
                        ShipperID = x.ShipperID,
                        COD = x.COD,
                        COO = x.COO,
                        ShipNote = x.ShipNote,
                        Image = x.Image,
                        Times = x.Times,
                        CreatedDate = x.CreatedDate
                    });
                #endregion

                #region Table Fee
                var fees = con.Fees
                    .Where(x => x.CreatedDate >= year)
                    .Join(
                        con.FeeTypes,
                        f => f.FeeTypeID,
                        t => t.ID,
                        (f, t) => new { fee = f, type = t }
                    )
                    .Select(x => new
                    {
                        OrderID = x.fee.OrderID,
                        FeeName = x.type.Name,
                        FeePrice = x.fee.FeePrice,
                        CreatedDate = x.fee.CreatedDate
                    });
                #endregion

                #region Table Coupons
                var coupons = con.Coupons
                    .Where(x => x.CreatedDate >= year)
                    .Select(x => new
                    {
                        OrderID = x.ID,
                        CouponID = x.ID,
                        CouponCode = x.Code,
                        CreatedDate = x.CreatedDate
                    });
                #endregion

                #region Table Refunds
                var refundGoods = con.tbl_RefundGoods
                    .Where(x => x.CreatedDate.HasValue && x.CreatedDate >= year)
                    .Select(x => new
                    {
                        RefundsGoodsID = x.ID,
                        TotalQuantity = x.TotalQuantity,
                        TotalCostOfGood = x.TotalCostOfGood,
                        TotalPrice = x.TotalPrice,
                        TotalRefundFee = String.IsNullOrEmpty(x.TotalRefundFee) ? "0" : x.TotalRefundFee,
                        CreatedDate = x.CreatedDate
                    });
                #endregion
                #endregion

                #region Các filter trức tiếp trên bản tbl_Order
                // Filter Created By
                if (!String.IsNullOrEmpty(filter.orderCreatedBy))
                    orders = orders.Where(x => x.CreatedBy == filter.orderCreatedBy);

                // Filter Order Type
                if (filter.orderType > 0)
                    orders = orders.Where(x => x.OrderType == filter.orderType);

                // Filter Payment Type
                if (filter.paymentType > 0)
                    orders = orders.Where(x => x.PaymentType == filter.paymentType);

                // Filter Payment Status
                if (filter.paymentStatus > 0)
                    orders = orders.Where(x => x.PaymentStatus == filter.paymentStatus);

                // Filter Excute Status
                if (filter.excuteStatus.Count() > 0)
                    orders = orders.Where(x => x.ExcuteStatus.HasValue && filter.excuteStatus.Contains(x.ExcuteStatus.Value));

                // Filter Shipping Type
                if (filter.shippingType.Count() > 0)
                    orders = orders.Where(x => x.ShippingType.HasValue && filter.shippingType.Contains(x.ShippingType.Value));

                // Filter Transport Company
                if (filter.transportCompany > 0)
                    orders = orders.Where(x => x.TransportCompanyID == filter.transportCompany);

                // Filter Discount
                if (!String.IsNullOrEmpty(filter.discount))
                {
                    if (filter.discount.Equals("yes"))
                        orders = orders.Where(x => x.TotalDiscount > 0);
                    else
                        orders = orders.Where(x => x.TotalDiscount == 0);
                }

                // Filter Order Created Date
                if (filter.orderFromDate.HasValue && filter.orderToDate.HasValue)
                {
                    if (filter.excuteStatus.Count() == 1 && filter.excuteStatus.Contains(2))
                        orders = orders
                            .Where(x =>
                                x.DateDone.HasValue &&
                                x.DateDone >= filter.orderFromDate &&
                                x.DateDone <= filter.orderToDate
                            );
                    else
                    {
                        orders = orders.Where(x =>
                            x.CreatedDate.HasValue &&
                            x.CreatedDate >= filter.orderFromDate &&
                            x.CreatedDate <= filter.orderToDate
                        );
                    }
                }

                // Filter Order Note
                if (!String.IsNullOrEmpty(filter.orderNote))
                {
                    if (filter.orderNote.Equals("yes"))
                        orders = orders.Where(x => !String.IsNullOrEmpty(x.OrderNote));
                    else
                        orders = orders.Where(x => String.IsNullOrEmpty(x.OrderNote));
                }

                // Filter những order được chọn để in report
                if (filter.selected && filter.account != null)
                {
                    var deliverySession = SessionController.getDeliverySession(filter.account);
                    var orderSelected = deliverySession.Select(x => x.OrderID).ToList();
                    // Chỉ lấy những order đã check
                    orders = orders.Where(x => orderSelected.Contains(x.ID));
                }
                #endregion

                #region Lấy dữ liệu cần thiết cho việc filter liên kết bảng
                var orderFilter = orders.Select(x => new
                {
                    ID = x.ID,
                    CustomerID = x.CustomerID,
                    TotalQuantiy = x.TotalQuantity,
                    TotalCostOfGood = x.TotalCostOfGood,
                    RefundsGoodsID = x.RefundsGoodsID,
                    CouponID = x.CouponID,
                    ShippingCode = x.ShippingCode,
                    DeliveryAddressId = x.DeliveryAddressId
                })
                .ToList();
                #endregion

                #region Các filter cần liên kết bản
                #region Tìm kiếm theo Customer, Order Detail
                var customerFilter = con.tbl_Customer
                    .Join(
                        orders.Where(x => x.CustomerID.HasValue)
                            .GroupBy(g => g.CustomerID)
                            .Select(x => new { CustomerID = x.Key }),
                        c => c.ID,
                        h => h.CustomerID,
                        (c, h) => c
                    )
                    .Select(x => new {
                        ID = x.ID,
                        CustomerName = x.CustomerName,
                        Nick = x.Nick,
                        Zalo = x.Zalo,
                        CustomerPhone = x.CustomerPhone,
                        CustomerPhone2 = x.CustomerPhone2,
                        UnSignedName = x.UnSignedName,
                        UnSignedNick = x.UnSignedNick
                    })
                    .ToList();

                #region Tìm kiếm theo từ khóa
                // Filter orderid or customername or customerphone or nick or shipcode
                if (!String.IsNullOrEmpty(filter.search))
                {
                    string search = Regex.Replace(filter.search.Trim(), @"[^0-9a-zA-Z\.\s_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+", "").Trim().ToLower();
                    search = UnSign.convert(search);

                    if (filter.searchType == (int)SearchType.Order)
                    {
                        // Trường hợp là mã giao hàng tiết kiệm
                        if (Regex.IsMatch(search, @"^s19100253\."))
                            orderFilter = orderFilter.Where(x => x.ShippingCode == filter.search).ToList();
                        // Trường hợp bắt đầu là sô
                        else if (Regex.IsMatch(search, @"^\d+$"))
                        {
                            if (search.Length <= 6)
                                orderFilter = orderFilter.Where(x => x.ID.ToString() == search).ToList();
                            else
                            {
                                var deliveryAddressFilter = con.DeliveryAddresses
                                    .Join(
                                        orders
                                            .Where(x => x.DeliveryAddressId.HasValue)
                                            .Select(x => new { deliveryAddressId = x.DeliveryAddressId.Value })
                                            .GroupBy(g => g.deliveryAddressId)
                                            .Select(x => new { deliveryAddressId = x.Key }),
                                        da => da.Id,
                                        f => f.deliveryAddressId,
                                        (da, f) => new
                                        {
                                            id = da.Id,
                                            phone = da.Phone
                                        }
                                    )
                                    .ToList();

                                orderFilter = orderFilter
                                    .Join(
                                        customerFilter,
                                        h => h.CustomerID,
                                        c => c.ID,
                                        (h, c) => new {
                                            order = h,
                                            customer = c
                                        }
                                    )
                                    .GroupJoin(
                                        deliveryAddressFilter,
                                        temp => temp.order.DeliveryAddressId,
                                        da => da.id,
                                        (temp, da) => new
                                        {
                                            order = temp.order,
                                            customer = temp.customer,
                                            deliveryAddress = da
                                        }
                                    )
                                    .SelectMany(
                                        x => x.deliveryAddress.DefaultIfEmpty(),
                                        (parent, child) => new
                                        {
                                            order = parent.order,
                                            customer = parent.customer,
                                            deliveryAddress = child
                                        }
                                    )
                                    .Where(x =>
                                        x.customer.CustomerPhone == search
                                        || x.customer.CustomerPhone2 == search
                                        || x.customer.Zalo == search
                                        || x.order.ShippingCode == search
                                        || (x.deliveryAddress != null && x.deliveryAddress.phone == search)
                                    )
                                    .Select(x => x.order)
                                    .ToList();

                                customerFilter = customerFilter
                                    .Join(
                                        orderFilter
                                            .Where(x => x.CustomerID.HasValue)
                                            .GroupBy(g => g.CustomerID)
                                            .Select(x => new { CustomerID = x.Key }),
                                        c => c.ID,
                                        h => h.CustomerID,
                                        (c, h) => c
                                    )
                                    .ToList();
                            }
                        }
                        else
                        {
                            orderFilter = orderFilter
                                .Join(
                                    customerFilter,
                                    h => h.CustomerID,
                                    c => c.ID,
                                    (h, c) => new {
                                        order = h,
                                        customer = c
                                    }
                                )
                                .Where(x =>
                                    (!String.IsNullOrEmpty(x.customer.UnSignedName) && x.customer.UnSignedName.ToLower().Contains(search)) ||
                                    (!String.IsNullOrEmpty(x.customer.UnSignedNick) && x.customer.UnSignedNick.ToLower().Contains(search)) ||
                                    (!String.IsNullOrEmpty(x.order.ShippingCode) && x.order.ShippingCode.ToLower() == search)
                                )
                                .Select(x => x.order)
                                .ToList();

                            customerFilter = customerFilter
                                .Join(
                                    orderFilter
                                        .Where(x => x.CustomerID.HasValue)
                                        .GroupBy(g => g.CustomerID)
                                        .Select(x => new { CustomerID = x.Key }),
                                    c => c.ID,
                                    h => h.CustomerID,
                                    (c, h) => c
                                )
                                .ToList();
                        }

                    }
                    else if (filter.searchType == (int)SearchType.Product)
                    {
                        var orderDetail = con.tbl_OrderDetail
                            .Where(x => x.CreatedDate >= year)
                            .Where(x => x.SKU.ToUpper().StartsWith(search))
                            .Select(x => new
                            {
                                OrderID = x.OrderID.HasValue ? x.OrderID.Value : 0
                            })
                            .Distinct()
                            .ToList();
                        var orderDetailFilter = orderDetail
                            .Join(
                                orderFilter,
                                d => d.OrderID,
                                h => h.ID,
                                (d, h) => d
                            )
                            .Select(x => new
                            {
                                OrderID = x.OrderID
                            })
                            .ToList();

                        orderFilter = orderFilter
                            .Join(
                                orderDetailFilter,
                                h => h.ID,
                                d => d.OrderID,
                                (h, d) => h
                            )
                            .ToList();
                    }
                }
                #endregion

                #region Tìm kiếm theo số lượng đơn hàng
                // Get info quantiy
                // Filter quantity
                if (!String.IsNullOrEmpty(filter.quantity))
                {
                    if (filter.quantity.Equals("greaterthan"))
                    {
                        orderFilter = orderFilter
                            .Where(x => x.TotalQuantiy >= filter.quantityFrom)
                            .ToList();
                    }
                    else if (filter.quantity.Equals("lessthan"))
                    {
                        orderFilter = orderFilter
                            .Where(x => x.TotalQuantiy <= filter.quantityTo)
                            .ToList();
                    }
                    else if (filter.quantity.Equals("between"))
                    {
                        orderFilter = orderFilter
                            .Where(x => x.TotalQuantiy >= filter.quantityFrom)
                            .Where(x => x.TotalQuantiy <= filter.quantityTo)
                            .ToList();
                    }
                }
                #endregion
                #endregion

                #region Tìm kiếm theo Bank, Transfers
                var transferFilter = bankTransfers
                    .ToList()
                    .Join(
                        orderFilter,
                        b => b.OrderID,
                        h => h.ID,
                        (b, h) => b
                    )
                    .Select(x => new {
                        OrderID = x.OrderID,
                        CusBankID = x.CusBankID,
                        AccBankID = x.AccBankID,
                        Money = x.Money,
                        Status = x.Status,
                        DoneAt = x.DoneAt,
                        Note = x.Note
                    })
                    .ToList();

                #region Tìm kiếm trạng thái chuyển khoản hoặc ngày kết kết thúc chuyển khoản
                // Filter Transfer Status or DoneAt
                if (filter.transferStatus > 0 || (filter.transferFromDate.HasValue && filter.transferToDate.HasValue))
                {
                    // Transfer Status
                    if (filter.transferStatus == (int)TransferStatus.Done)
                    {
                        transferFilter = transferFilter.Where(x => x.Status == 1).ToList();
                    }
                    else if (filter.transferStatus == (int)TransferStatus.Waitting)
                    {
                        transferFilter = transferFilter.Where(x => x.Status != 1).ToList();
                    }

                    // Transfer Date
                    if (filter.transferFromDate.HasValue && filter.transferToDate.HasValue)
                    {
                        transferFilter = transferFilter.Where(x =>
                            x.DoneAt >= filter.transferFromDate &&
                            x.DoneAt <= filter.transferToDate
                        )
                        .ToList();
                    }

                    orderFilter = orderFilter
                        .Join(
                            transferFilter,
                            h => h.ID,
                            b => b.OrderID,
                            (h, b) => h
                        )
                        .ToList();
                }
                #endregion

                #region Tìm kiếm theo tài khoản ngân hàng nhận tiền
                if (filter.bankReceive != 0)
                {
                    transferFilter = transferFilter.Where(x => x.AccBankID == filter.bankReceive).ToList();
                    orderFilter = orderFilter
                        .Join(
                            transferFilter,
                            o => o.ID,
                            b => b.OrderID,
                            (o, b) => o
                        )
                        .ToList();
                }
                #endregion
                #endregion

                #region Tìm kiếm theo Delivery
                var deliveryFilter = deliveries
                    .ToList()
                    .Join(
                        orderFilter,
                        d => d.OrderID,
                        h => h.ID,
                        (d, h) => d
                    )
                    .Select(x => new
                    {
                        OrderID = x.OrderID,
                        StartAt = x.StartAt,
                        Status = x.Status,
                        ShipperID = x.ShipperID,
                        COD = x.COD,
                        COO = x.COO,
                        ShipNote = x.ShipNote,
                        Image = x.Image,
                        Times = x.Times,
                    })
                    .ToList();

                #region Tìm kiếm theo ngày giao hàng
                // Filter Delivery Start At
                if (!String.IsNullOrEmpty(filter.deliveryStart))
                {
                    DateTime fromdate = DateTime.Today;
                    DateTime todate = DateTime.Now;
                    CalDate(filter.deliveryStart, ref fromdate, ref todate);

                    deliveryFilter = deliveryFilter
                        .Where(x =>
                            x.StartAt >= fromdate &&
                            x.StartAt <= todate
                        )
                        .ToList();

                    orderFilter = orderFilter
                        .Join(
                            deliveryFilter,
                            h => h.ID,
                            d => d.OrderID,
                            (h, d) => h
                        )
                        .ToList();
                }
                #endregion

                #region Tìm kiếm đợt giao hàng
                // Filter Delivery times
                if (filter.deliveryTimes > 0)
                {
                    deliveryFilter = deliveryFilter.Where(x => x.Times == filter.deliveryTimes).ToList();
                    orderFilter = orderFilter
                        .Join(
                            deliveryFilter,
                            h => h.ID,
                            d => d.OrderID,
                            (h, d) => h
                        )
                        .ToList();
                }
                #endregion

                #region Tìm kiếm theo shipper
                // Filter Shipper
                if (filter.shipper > 0)
                {
                    deliveryFilter = deliveryFilter.Where(x => x.ShipperID == filter.shipper).ToList();
                    orderFilter = orderFilter
                        .Join(
                            deliveryFilter,
                            h => h.ID,
                            d => d.OrderID,
                            (h, d) => h
                        )
                        .ToList();
                }
                #endregion

                #region Tìm kiếm theo phiếu giao hàng
                if (filter.deliveryStatus != 0)
                {
                    deliveryFilter = deliveryFilter.Where(x => x.Status == filter.deliveryStatus).ToList();
                    orderFilter = orderFilter
                        .Join(
                            deliveryFilter,
                            o => o.ID,
                            b => b.OrderID,
                            (o, b) => o
                        )
                        .ToList();
                }

                switch (filter.invoiceStatus)
                {
                    case (int)InvoiceStatus.Yes:
                        deliveryFilter = deliveryFilter.Where(x => !String.IsNullOrEmpty(x.Image)).ToList();

                        orderFilter = orderFilter
                            .Join(
                                deliveryFilter,
                                o => o.ID,
                                b => b.OrderID,
                                (o, b) => o
                            )
                            .ToList();
                        break;
                    case (int)InvoiceStatus.No:
                        deliveryFilter = deliveryFilter.Where(x => String.IsNullOrEmpty(x.Image)).ToList();
                        orderFilter = orderFilter
                            .Join(
                                deliveryFilter,
                                o => o.ID,
                                b => b.OrderID,
                                (o, b) => o
                            )
                            .ToList();
                        break;
                    default:
                        break;
                }
                #endregion
                #endregion

                #region Tìm kiếm theo Fee
                // Get fee of product
                var feeFilter = fees
                    .ToList()
                    .Join(
                        orderFilter,
                        f => f.OrderID,
                        h => h.ID,
                        (f, h) => new
                        {
                            OrderID = h.ID,
                            FeeName = f.FeeName,
                            FeePrice = f.FeePrice
                        }
                    )
                    .GroupBy(x => x.OrderID)
                    .Select(g => new
                    {
                        OrderID = g.Key,
                        OtherFeeName = g.Count() > 1 ? "Nhiều phí khác" : g.Max(x => x.FeeName),
                        OtherFeeValue = g.Sum(x => x.FeePrice)
                    })
                    .ToList();

                #region Tìm kiếm theo phí sản phẩm
                // Filter fee of product
                if (!String.IsNullOrEmpty(filter.otherFee))
                {
                    if (filter.otherFee.Equals("yes"))
                    {
                        feeFilter = feeFilter.Where(x => x.OtherFeeValue != 0).ToList();
                        orderFilter = orderFilter
                            .Join(
                                feeFilter,
                                o => o.ID,
                                b => b.OrderID,
                                (o, b) => o
                            )
                            .ToList();
                    }
                    else
                    {
                        feeFilter = feeFilter.Where(x => x.OtherFeeValue == 0).ToList();
                        orderFilter = orderFilter
                            .Join(
                                feeFilter,
                                o => o.ID,
                                b => b.OrderID,
                                (o, b) => o
                            )
                            .ToList();
                    }
                }
                #endregion
                #endregion

                #region Lọc theo mã giảm giá
                var couponFilter = con.Coupons
                    .ToList()
                    .Join(
                        orderFilter,
                        c => c.ID,
                        o => o.CouponID,
                        (c, o) => new
                        {
                            orderID = o.ID,
                            couponID = c.ID,
                            couponCode = c.Code
                        }
                    )
                    .ToList();

                if (filter.couponStatus != 0)
                {
                    var orderCoupon = orderFilter
                       .Where(x => x.CouponID.HasValue)
                       .Join(
                           couponFilter,
                           o => o.ID,
                           c => c.orderID,
                           (o, c) => o
                       )
                       .ToList();
                    if (filter.couponStatus == (int)CouponStatus.Yes)
                        orderFilter = orderCoupon.ToList();
                    if (filter.couponStatus == (int)CouponStatus.No)
                        orderFilter = orderFilter.Except(orderCoupon).ToList();
                }
                #endregion
                #endregion

                #region Tính toán phân trang
                // Calculate pagination
                page.totalCount = orderFilter.Count();
                page.totalPages = (int)Math.Ceiling(page.totalCount / (double)page.pageSize);

                orderFilter = orderFilter
                    .OrderByDescending(x => x.ID)
                    .Skip((page.currentPage - 1) * page.pageSize)
                    .Take(page.pageSize)
                    .ToList();
                #endregion

                #region Xuất dữ liệu
                #region Xuất thông tin chính
                // Get info main
                var orderIDList = orderFilter.Select(y => y.ID).ToList();
                var header = orders
                    .Where(x => orderIDList.Contains(x.ID))
                    .OrderByDescending(o => o.ID)
                    .ToList();
                #endregion

                #region Xuất thông tin về tra hàng
                // Get info refunds
                var refunds = refundGoods
                    .ToList()
                    .Join(
                        orderFilter.Where(x => x.RefundsGoodsID.HasValue),
                        r => r.RefundsGoodsID,
                        h => h.RefundsGoodsID.Value,
                        (r, h) => new
                        {
                            OrderID = h.ID,
                            RefundsGoodsID = h.RefundsGoodsID,
                            TotalQuantity = r.TotalQuantity,
                            TotalCostOfGood = r.TotalCostOfGood,
                            TotalPrice = r.TotalPrice,
                            TotalRefundFee = r.TotalRefundFee
                        })
                    .ToList();
                #endregion

                #region Xuất thông tin khách hàng
                // Get info customer
                var customer = orderFilter
                    .Join(
                        customerFilter,
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
                #endregion

                #region Xuất thông tin phí của đơn hàng
                // Get info fee
                var fee = feeFilter
                    .Join(
                        orderFilter,
                        f => f.OrderID,
                        o => o.ID,
                        (f, o) => f
                    )
                    .OrderByDescending(o => o.OrderID)
                    .ToList();
                #endregion

                #region Xuất thông tin giao dich ngân hàng
                // Các giao dịch của shop và khách
                transferFilter = transferFilter
                    .Join(
                        orderFilter,
                        t => t.OrderID,
                        h => h.ID,
                        (t, h) => t
                    )
                    .ToList();

                // Ngân hàng của khách
                var cusBankIDList = transferFilter
                    .GroupBy(g => g.CusBankID)
                    .Select(x => x.Key)
                    .ToList();
                var banks = con.Banks
                    .Where(x => cusBankIDList.Contains(x.ID))
                    .Select(x => new
                    {
                        ID = x.ID,
                        BankName = x.BankName
                    })
                    .ToList();
                // Ngân hàng của shop
                var shopBankIDList = transferFilter
                    .GroupBy(g => g.AccBankID)
                    .Select(x => x.Key)
                    .ToList();
                var backAccounts = con.BankAccounts
                    .Where(x => shopBankIDList.Contains(x.ID))
                    .Select(x => new
                    {
                        ID = x.ID,
                        BankName = x.BankName
                    })
                    .ToList();

                // Tổng hợp thông tin giao dịch
                var transfers = transferFilter
                    .Join(
                        banks,
                        h => h.CusBankID,
                        c => c.ID,
                        (h, c) => new
                        {
                            transfer = h,
                            bank = new { CusBankName = c.BankName }
                        }
                    )
                    .Join(
                        backAccounts,
                        h => h.transfer.AccBankID,
                        a => a.ID,
                        (h, a) => new
                        {
                            transfer = h.transfer,
                            bank = h.bank,
                            account = new { AccBankName = a.BankName }
                        }
                    )
                    .Select(x => new
                    {
                        OrderID = x.transfer.OrderID,
                        CusBankID = x.transfer.CusBankID,
                        CusBankName = x.bank.CusBankName,
                        AccBankID = x.transfer.AccBankID,
                        AccBankName = x.account.AccBankName,
                        Money = x.transfer.Money,
                        Status = x.transfer.Status,
                        StatusName = x.transfer.Status,
                        DoneAt = x.transfer.DoneAt,
                        Note = x.transfer.Note,
                    })
                    .OrderByDescending(o => o.OrderID)
                    .ToList();
                #endregion

                #region Xuất thông tin giao hàng
                // Thông tin giao hàng
                deliveryFilter = deliveryFilter
                    .Join(
                        orderFilter,
                        d => d.OrderID,
                        o => o.ID,
                        (d, o) => d
                    )
                    .ToList();
                // Shipper
                var skipperIDList = deliveryFilter
                    .GroupBy(g => g.ShipperID)
                    .Select(x => x.Key)
                    .ToList();
                var shipper = con.Shippers
                    .Where(x => skipperIDList.Contains(x.ID))
                    .ToList();

                //Thông tin về giao hàng
                var shipments = deliveryFilter
                    .GroupJoin(
                        shipper,
                        d => d.ShipperID,
                        s => s.ID,
                        (d, s) => new { d, s }
                    )
                    .SelectMany(
                        x => x.s.DefaultIfEmpty(),
                        (parent, child) => new
                        {
                            delivery = parent.d,
                            shipper = child
                        }
                    )
                    .Select(x => new {
                        OrderID = x.delivery.OrderID,
                        StartAt = x.delivery.StartAt,
                        Status = x.delivery.Status,
                        ShipperID = x.delivery.ShipperID,
                        COD = x.delivery.COD,
                        COO = x.delivery.COO,
                        ShipNote = x.delivery.ShipNote,
                        Image = x.delivery.Image,
                        DeliveryTimes = x.delivery.Times,
                        // Shipper
                        ShipperName = x.shipper != null ? x.shipper.Name : String.Empty
                    })
                    .OrderByDescending(o => o.OrderID)
                    .ToList();

                // Fix bug duble in database
                shipments = shipments
                    .Join(
                        shipments
                            .GroupBy(g => g.OrderID)
                            .Select(x => new {
                                OrderID = x.Key,
                                StartAt = x.Max(m => m.StartAt)
                            }),
                        d => new { OrderID = d.OrderID, StartAt = d.StartAt },
                        f => new { OrderID = f.OrderID, StartAt = f.StartAt },
                        (d, f) => d
                    )
                    .ToList();
                #endregion

                #region Xuất thông phiếu giảm giá
                // Lấy thông tin phiếu giảm giá
                couponFilter = couponFilter
                    .Join(
                        orderFilter,
                        c => c.orderID,
                        o => o.ID,
                        (c, o) => c
                    )
                    .OrderByDescending(o => o.orderID)
                    .ToList();
                #endregion
                #endregion

                #region Kêt thúc: Tổng hợp lại các thông tin
                var data = header
                    .Join(
                        customer,
                        o => new {
                            OrderID = o.ID,
                            CustomerID = o.CustomerID.HasValue ? o.CustomerID.Value : 0
                        },
                        c => new { OrderID = c.OrderID, CustomerID = c.CustomerID },
                        (o, c) => new {
                            order = o,
                            customer = c
                        }
                    )
                    .GroupJoin(
                        refunds,
                        temp => new { OrderID = temp.order.ID, RefundsGoodsID = temp.order.RefundsGoodsID },
                        rf => new { OrderID = rf.OrderID, RefundsGoodsID = rf.RefundsGoodsID },
                        (temp, rf) => new {
                            order = temp.order,
                            customer = temp.customer,
                            refund = rf
                        }
                    )
                    .SelectMany(
                        x => x.refund.DefaultIfEmpty(),
                        (parent, child) => new {
                            order = parent.order,
                            customer = parent.customer,
                            refund = child
                        }
                    )
                    .GroupJoin(
                        fee,
                        temp => temp.order.ID,
                        f => f.OrderID,
                        (temp, f) => new {
                            order = temp.order,
                            customer = temp.customer,
                            refund = temp.refund,
                            fee = f
                        }
                    )
                    .SelectMany(
                        x => x.fee.DefaultIfEmpty(),
                        (parent, child) => new {
                            order = parent.order,
                            customer = parent.customer,
                            refund = parent.refund,
                            fee = child
                        }
                    )
                    .GroupJoin(
                        transfers,
                        temp => temp.order.ID,
                        t => t.OrderID,
                        (temp, t) => new {
                            order = temp.order,
                            customer = temp.customer,
                            refund = temp.refund,
                            fee = temp.fee,
                            trans = t
                        }
                    )
                    .SelectMany(
                        x => x.trans.DefaultIfEmpty(),
                        (parent, child) => new {
                            order = parent.order,
                            customer = parent.customer,
                            refund = parent.refund,
                            fee = parent.fee,
                            trans = child
                        }
                    )
                    .GroupJoin(
                        shipments,
                        temp => temp.order.ID,
                        d => d.OrderID,
                        (temp, d) => new {
                            order = temp.order,
                            customer = temp.customer,
                            refund = temp.refund,
                            fee = temp.fee,
                            trans = temp.trans,
                            delivery = d
                        }
                    )
                    .SelectMany(
                        x => x.delivery.DefaultIfEmpty(),
                        (parent, child) => new {
                            order = parent.order,
                            customer = parent.customer,
                            refund = parent.refund,
                            fee = parent.fee,
                            trans = parent.trans,
                            delivery = child
                        }
                    )
                    .GroupJoin(
                        couponFilter,
                        temp => temp.order.ID,
                        c => c.orderID,
                        (temp, c) => new {
                            order = temp.order,
                            customer = temp.customer,
                            refund = temp.refund,
                            fee = temp.fee,
                            trans = temp.trans,
                            delivery = temp.delivery,
                            coupon = c
                        }
                    )
                    .SelectMany(
                        x => x.coupon.DefaultIfEmpty(),
                        (parent, child) => new {
                            order = parent.order,
                            customer = parent.customer,
                            refund = parent.refund,
                            fee = parent.fee,
                            trans = parent.trans,
                            delivery = parent.delivery,
                            coupon = child
                        }
                    )
                    .Select(temp => {
                        var result = new OrderList();
                        var totalCOGS = Convert.ToDouble(temp.order.TotalCostOfGood);
                        var totalProfit = 0D;

                        // Tính lợi nhuận
                        totalProfit = Convert.ToDouble(temp.order.TotalPriceNotDiscount) - Convert.ToDouble(temp.order.TotalDiscount) - totalCOGS;

                        // OrderID
                        result.ID = temp.order.ID;
                        result.CustomerID = temp.order.CustomerID.HasValue ? temp.order.CustomerID.Value : 0;
                        result.OrderType = temp.order.OrderType.HasValue ? temp.order.OrderType.Value : 0;
                        result.ExcuteStatus = temp.order.ExcuteStatus.HasValue ? temp.order.ExcuteStatus.Value : 0;
                        result.PaymentStatus = temp.order.PaymentStatus.HasValue ? temp.order.PaymentStatus.Value : 0;
                        result.PaymentType = temp.order.PaymentType.HasValue ? temp.order.PaymentType.Value : 0;
                        result.ShippingType = temp.order.ShippingType.HasValue ? temp.order.ShippingType.Value : 0;
                        result.Quantity = temp.order.TotalQuantity;
                        result.TotalProfit = totalProfit;
                        result.TotalPrice = Convert.ToDouble(temp.order.TotalPrice);
                        result.TotalDiscount = temp.order.TotalDiscount;
                        result.FeeShipping = Convert.ToDouble(temp.order.FeeShipping);
                        result.CreatedBy = temp.order.CreatedBy;
                        result.CreatedDate = temp.order.CreatedDate.HasValue ? temp.order.CreatedDate.Value : DateTime.Now;
                        result.DateDone = temp.order.DateDone;
                        result.OrderNote = temp.order.OrderNote;
                        result.RefundsGoodsID = temp.order.RefundsGoodsID;
                        result.ShippingCode = temp.order.ShippingCode;
                        result.TransportCompanyID = temp.order.TransportCompanyID;
                        result.TransportCompanySubID = temp.order.TransportCompanySubID;
                        result.PostalDeliveryType = temp.order.PostalDeliveryType.HasValue ? temp.order.PostalDeliveryType.Value : 0;
                        // Customer
                        result.CustomerName = temp.customer.CustomerName;
                        result.Nick = temp.customer.Nick;
                        result.CustomerPhone = temp.customer.CustomerPhone;

                        // Phiếm giảm giắ
                        if (temp.coupon != null)
                        {
                            result.TotalProfit = result.TotalProfit - Convert.ToDouble(temp.order.CouponValue);
                            result.CouponCode = temp.coupon.couponCode;
                            result.CouponValue = temp.order.CouponValue;
                        }

                        // Refunds
                        if (temp.refund != null)
                        {
                            var TotalRefundPrice = Convert.ToDouble(temp.refund.TotalPrice);
                            var totalRefundCOGS = temp.refund.TotalCostOfGood;

                            result.TotalProfit = result.TotalProfit - (TotalRefundPrice - Convert.ToDouble(totalRefundCOGS));
                            result.TotalRefund = TotalRefundPrice;
                        }
                        // Fee Other
                        if (temp.fee != null)
                        {
                            result.OtherFeeName = temp.fee.OtherFeeName;
                            result.OtherFeeValue = Convert.ToDouble(temp.fee.OtherFeeValue);
                        }
                        else
                        {
                            result.OtherFeeName = String.Empty;
                            result.OtherFeeValue = 0;
                        }
                        // Transfer Bank
                        if (temp.trans != null)
                        {

                            result.CusBankID = temp.trans.CusBankID;
                            result.CusBankName = temp.trans.CusBankName;
                            result.AccBankID = temp.trans.AccBankID;
                            result.AccBankName = temp.trans.AccBankName;
                            result.MoneyReceive = temp.trans.Money;
                            result.TransferStatus = temp.trans.Status;
                            result.StatusName = temp.trans.Status == 1 ? "Đã nhận tiền" : "Chưa nhận tiền";
                            result.DoneAt = temp.trans.DoneAt;
                            result.TransferNote = temp.trans.Note;
                        }
                        // Delivery
                        if (temp.delivery != null)
                        {

                            result.DeliveryDate = temp.delivery.StartAt;
                            result.DeliveryStatus = temp.delivery.Status;
                            result.ShipperID = temp.delivery.ShipperID;
                            result.CostOfDelivery = temp.delivery.COD;
                            result.CollectionOfOrder = temp.delivery.COO;
                            result.ShipNote = temp.delivery.ShipNote;
                            result.InvoiceImage = temp.delivery.Image;
                            result.ShipperName = temp.delivery.ShipperName;
                            result.DeliveryTimes = temp.delivery.DeliveryTimes.HasValue ? temp.delivery.DeliveryTimes.Value : 0;
                        }
                        else
                        {
                            result.DeliveryStatus = (int)DeliveryStatus.Waiting;
                        }

                        return result;
                    })
                    .OrderByDescending(x => x.ID)
                    .ToList();
                #endregion

                #region Sắp xếp lại đơn hàng theo thứ tự đã chọn
                if (filter.selected && filter.account != null)
                {
                    var deliverySession = SessionController.getDeliverySession(filter.account);
                    var orderSelected = deliverySession
                        .OrderByDescending(o => o.ModifiedDate)
                        .Select(x => new { orderID = x.OrderID })
                        .ToList();
                    // Chỉ lấy những order đã check
                    data = orderSelected
                        .Join(
                            data,
                            s => s.orderID,
                            d => d.ID,
                            (s, d) => d
                        )
                        .ToList();
                }
                #endregion

                return data;
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
        public static List<tbl_Order> GetAllNoteByCustomerID(int ID, int currentOrderID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_Order> las = new List<tbl_Order>();
                las = dbe.tbl_Order.Where(x => x.CustomerID == ID && x.ID < currentOrderID && !String.IsNullOrEmpty(x.OrderNote)).OrderByDescending(x => x.ID).Take(5).ToList();
                return las;
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

        public static PreOrder getLastPreOrder(string customerPhone, int statuses)
        {
            using (var con = new inventorymanagementEntities())
            {
                var lastPreOrder = con.PreOrders
                    .Where(x => x.CreatedBy == customerPhone)
                    .Where(x => x.Status == statuses)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                return lastPreOrder;
            }
        }

        public static tbl_Order getLastOrderOfDay(DateTime createDate, int customerId, IList<int> statuses)
        {
            using (var con = new inventorymanagementEntities())
            {
                var lastOrder = con.tbl_Order
                    .Where(x => x.CreatedDate >= createDate.Date)
                    .Where(x => x.CustomerID == customerId)
                    .Where(x => x.ExcuteStatus.HasValue && statuses.Contains(x.ExcuteStatus.Value))
                    .OrderByDescending(x => x.ID)
                    .FirstOrDefault();

                return lastOrder;
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
                                && x.PaymentStatus != 1)
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

            result = result.Where(x => x.Quantity > 0).OrderByDescending(x => x.Quantity).Take(15).ToList();

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
                            .Where(r => r.PaymentStatus != 1)
                            .ToList();
                    }
                    else
                    {
                        DateTime fd = Convert.ToDateTime(fromdate);
                        or = db.tbl_Order
                            .Where(r => r.CreatedDate >= fd)
                            .Where(r => r.ExcuteStatus == 2)
                            .Where(r => r.PaymentStatus != 1)
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
                            .Where(r => r.PaymentStatus != 1)
                            .ToList();
                    }
                    else
                    {
                        or = db.tbl_Order
                            .Where(r => r.ExcuteStatus == 2)
                            .Where(r => r.PaymentStatus != 1)
                            .ToList();
                    }
                }
                return or;
            }
        }

        #endregion

        public class OrderReport
        {
            public DateTime DateDone { get; set; }
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
            public double TotalProfit { get; set; }
            public double TotalPrice { get; set; }
            public double TotalDiscount { get; set; }
            public double TotalCostOfGood { get; set; }
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
            public int DeliveryTimes { get; set; }
            // Mã giảm giá
            public string CouponCode { get; set; }
            public decimal CouponValue { get; set; }
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

        public static List<ProfitReportModel> GetProfitReport(DateTime fromDate, DateTime toDate)
        {
            using (var con = new inventorymanagementEntities())
            {
                #region Lọc dữ liệu chính
                #region Order
                var orders = con.tbl_Order
                    .Where(x => x.ExcuteStatus == 2)
                    .Where(x => x.PaymentStatus != 1)
                    .Where(x => x.DateDone.HasValue)
                    .Where(x => x.DateDone >= fromDate && x.DateDone <= toDate)
                    .Select(x => new
                    {
                        DateDone = x.DateDone.Value,
                        TotalQuantity = x.TotalQuantity,
                        TotalCostOfGood = x.TotalCostOfGood,
                        TotalPrice = String.IsNullOrEmpty(x.TotalPriceNotDiscount) ? "0" : x.TotalPriceNotDiscount,
                        TotalDiscount = x.TotalDiscount.HasValue ? x.TotalDiscount.Value : 0,
                        TotalCoupon = x.CouponValue.HasValue ? x.CouponValue.Value : 0,
                        // Phí giao hàng chỉ dùng để tham khảo
                        TotalShippingFee = String.IsNullOrEmpty(x.FeeShipping) ? "0" : x.FeeShipping,
                        // Phí khác chỉ dùng để tham khảo
                        TotalOtherFee = x.OtherFeeValue.HasValue ? x.OtherFeeValue.Value : 0
                    })
                    .OrderBy(x => x.DateDone);
                #endregion

                #region Refund
                var refundTarget = con.tbl_RefundGoods
                    .Where(x => x.CreatedDate.HasValue)
                    .Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate)
                    .Select(x => new
                    {
                        DateDone = x.CreatedDate.Value,
                        TotalQuantity = x.TotalQuantity,
                        TotalCostOfGood = x.TotalCostOfGood,
                        // Tiền hoàn trả đã bao gồm phí hoàn trả
                        TotalRefundPrice = String.IsNullOrEmpty(x.TotalPrice) ? "0" : x.TotalPrice,
                        TotalRefundFee = String.IsNullOrEmpty(x.TotalRefundFee) ? "0" : x.TotalRefundFee
                    })
                    .OrderBy(x => x.DateDone);
                #endregion
                #endregion

                #region Tính toán số liệu báo cáo
                #region Tính toán theo ngày với từng đơn hàng
                var orderDate = orders
                    .ToList()
                    .GroupBy(g => g.DateDone.ToString("yyyy-MM-dd"))
                    .Select(x => new
                    {
                        DateDone = Convert.ToDateTime(x.Key),
                        TotalNumberOfOrder = x.Count(),
                        TotalSoldQuantity = x.Sum(s => s.TotalQuantity),
                        TotalSaleCost = x.Sum(s => Convert.ToDouble(s.TotalCostOfGood)),
                        TotalSalePrice = x.Sum(s => Convert.ToDouble(s.TotalPrice)),
                        TotalSaleDiscount = x.Sum(s => s.TotalDiscount),
                        TotalCoupon = x.Sum(s => Convert.ToDouble(s.TotalCoupon)),
                        // Phí giao hàng chỉ dùng để tham khảo
                        TotalShippingFee = x.Sum(s => Convert.ToDouble(s.TotalShippingFee)),
                        // Phí khác chỉ dùng để tham khảo
                        TotalOtherFee = x.Sum(s => s.TotalOtherFee)
                    })
                    .ToList();
                #endregion

                #region Tính toán theo ngày với từng đơn hàng đổi trả
                var refundDate = refundTarget
                    .ToList()
                    .GroupBy(g => g.DateDone.ToString("yyyy-MM-dd"))
                    .Select(x => new
                    {
                        DateDone = Convert.ToDateTime(x.Key),
                        TotalRefundQuantity = x.Sum(s => s.TotalQuantity),
                        TotalRefundCost = x.Sum(s => Convert.ToDouble(s.TotalCostOfGood)),
                        // Tiền hoàn trả đã bao gồm phí hoàn trả
                        TotalRefundPrice = x.Sum(s => Convert.ToDouble(s.TotalRefundPrice)),
                        TotalRefundFee = x.Sum(s => Convert.ToDouble(s.TotalRefundFee))
                    })
                    .ToList();
                #endregion
                #endregion

                var result = orderDate
                    .GroupJoin(
                        refundDate,
                        o => o.DateDone,
                        r => r.DateDone,
                        (o, r) => new { o, r }
                    )
                    .SelectMany(
                        x => x.r.DefaultIfEmpty(),
                        (parent, child) =>
                        {
                            var item = new ProfitReportModel()
                            {
                                DateDone = parent.o.DateDone,
                                TotalNumberOfOrder = parent.o.TotalNumberOfOrder,
                                TotalSoldQuantity = parent.o.TotalSoldQuantity,
                                TotalSalePrice = parent.o.TotalSalePrice,
                                TotalSaleCost = parent.o.TotalSaleCost,
                                TotalSaleDiscount = parent.o.TotalSaleDiscount,
                                TotalCoupon = parent.o.TotalCoupon,
                                // Phí giao hàng chỉ dùng để tham khảo
                                TotalShippingFee = parent.o.TotalShippingFee,
                                // Phí khác chỉ dùng để tham khảo
                                TotalOtherFee = parent.o.TotalOtherFee
                            };

                            if (child != null)
                            {
                                item.TotalRefundQuantity = child.TotalRefundQuantity;
                                item.TotalRefundCost = child.TotalRefundCost;
                                // Tiền hoàn trả đã bao gồm phí hoàn trả
                                item.TotalRefundPrice = child.TotalRefundPrice + child.TotalRefundFee;
                                item.TotalRefundFee = child.TotalRefundFee;
                            }

                            return item;
                        }
                    )
                    .OrderBy(x => x.DateDone)
                    .ToList();

                return result;
            }
        }

        public static UserReportModel getUserReport(string CreatedBy, DateTime fromDate, DateTime toDate)
        {
            var list = new List<OrderReport>();
            var sql = new StringBuilder();

            sql.AppendLine("SELECT");
            sql.AppendLine("    Ord.ID");
            sql.AppendLine(",   SUM(ISNULL(OrdDetail.Quantity, 0)) AS Quantity");
            sql.AppendLine(",   SUM(ISNULL(OrdDetail.TotalCostOfGood, 0)) AS TotalCost");
            sql.AppendLine(",   SUM(");
            sql.AppendLine("        IIF(");
            sql.AppendLine("            ISNULL(OrdDetail.DiscountPrice, 0) > 0");
            sql.AppendLine("        ,   (ISNULL(OrdDetail.Price, 0) - ISNULL(OrdDetail.DiscountPrice, 0)) * ISNULL(OrdDetail.Quantity, 0)");
            sql.AppendLine("        ,   (ISNULL(OrdDetail.Price, 0) - ISNULL(Ord.DiscountPerProduct, 0)) * ISNULL(OrdDetail.Quantity, 0)");
            sql.AppendLine("        )");
            sql.AppendLine("    ) AS TotalRevenue");
            sql.AppendLine("FROM tbl_Order AS Ord");
            sql.AppendLine("INNER JOIN tbl_OrderDetail AS OrdDetail");
            sql.AppendLine("ON     Ord.ID = OrdDetail.OrderID");
            sql.AppendLine("WHERE 1 = 1");
            if (!String.IsNullOrEmpty(CreatedBy))
            {
                sql.AppendLine(String.Format("    AND Ord.CreatedBy = '{0}'", CreatedBy));
            }
            sql.AppendLine("    AND Ord.ExcuteStatus = 2");
            sql.AppendLine("    AND (Ord.PaymentStatus = 2 OR Ord.PaymentStatus = 3 OR Ord.PaymentStatus = 4)");
            sql.AppendLine(String.Format("    AND CONVERT(NVARCHAR(10), Ord.DateDone, 121) BETWEEN CONVERT(NVARCHAR(10), '{0:yyyy-MM-dd}', 121) AND CONVERT(NVARCHAR(10), '{1:yyyy-MM-dd}', 121)", fromDate, toDate));
            sql.AppendLine("GROUP BY Ord.ID");

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

            return new UserReportModel()
            {
                totalSoldQuantity = list.Sum(x => x.Quantity),
                totalRevenue = list.Sum(x => x.TotalRevenue),
                totalCost = list.Sum(x => x.TotalCost)
            };
        }

        public class UserReportModel
        {
            public int totalSoldQuantity { get; set; }
            public double totalRevenue { get; set; }
            public double totalCost { get; set; }
        }

        public static List<ProductReportModel> getProductReport(string SKU, int CategoryID, string CreatedBy, DateTime fromDate, DateTime toDate)
        {
            var list = new List<OrderReport>();
            var sql = new StringBuilder();

            sql.AppendLine("BEGIN");

            if (CategoryID > 0)
            {
                sql.AppendLine(String.Empty);
                sql.AppendLine("WITH category AS(");
                sql.AppendLine("    SELECT");
                sql.AppendLine("            ID");
                sql.AppendLine("    ,       CategoryName");
                sql.AppendLine("    ,       ParentID");
                sql.AppendLine("    FROM");
                sql.AppendLine("            tbl_Category");
                sql.AppendLine("    WHERE");
                sql.AppendLine("            1 = 1");
                sql.AppendLine("    AND     ID = " + CategoryID);
                sql.AppendLine("");
                sql.AppendLine("    UNION ALL");
                sql.AppendLine("");
                sql.AppendLine("    SELECT");
                sql.AppendLine("            CHI.ID");
                sql.AppendLine("    ,       CHI.CategoryName");
                sql.AppendLine("    ,       CHI.ParentID");
                sql.AppendLine("    FROM");
                sql.AppendLine("            category AS PAR");
                sql.AppendLine("    INNER JOIN tbl_Category AS CHI");
                sql.AppendLine("        ON PAR.ID = CHI.ParentID");
                sql.AppendLine(")");
                sql.AppendLine("SELECT");
                sql.AppendLine("        ID");
                sql.AppendLine(",       CategoryName");
                sql.AppendLine(",       ParentID");
                sql.AppendLine("INTO #category");
                sql.AppendLine("FROM category;");
            }

            sql.AppendLine("SELECT");
            sql.AppendLine("    CONVERT(VARCHAR(10), Ord.DateDone, 121) AS DateDone");
            sql.AppendLine(",   Ord.ID");
            sql.AppendLine(",   OrdDetail.SKU");
            sql.AppendLine(",   OrdDetail.Quantity");
            sql.AppendLine(",   OrdDetail.Price");
            sql.AppendLine(",   OrdDetail.DiscountPrice");
            sql.AppendLine(",   OrdDetail.TotalCostOfGood");
            sql.AppendLine("INTO #data");
            sql.AppendLine("FROM tbl_Order AS Ord");
            sql.AppendLine("INNER JOIN tbl_OrderDetail AS OrdDetail");
            sql.AppendLine("ON     Ord.ID = OrdDetail.OrderID");
            sql.AppendLine("WHERE 1 = 1");
            sql.AppendLine("    AND Ord.ExcuteStatus = 2");
            sql.AppendLine("    AND (Ord.PaymentStatus = 2 OR Ord.PaymentStatus = 3 OR Ord.PaymentStatus = 4)");

            if (!String.IsNullOrEmpty(CreatedBy))
            {
                sql.AppendLine(String.Format("    AND Ord.CreatedBy = '{0}'", CreatedBy));
            }

            if (!String.IsNullOrEmpty(SKU))
            {
                sql.AppendLine(String.Format("    AND OrdDetail.SKU LIKE '{0}%'", SKU));
            }

            sql.AppendLine(String.Format("    AND    CONVERT(NVARCHAR(10), Ord.DateDone, 121) BETWEEN CONVERT(NVARCHAR(10), '{0:yyyy-MM-dd}', 121) AND CONVERT(NVARCHAR(10), '{1:yyyy-MM-dd}', 121);", fromDate, toDate));

            sql.AppendLine("SELECT");
            sql.AppendLine("    DAT.DateDone");
            sql.AppendLine(",   DAT.ID");
            sql.AppendLine(",   SUM(ISNULL(DAT.Quantity, 0)) AS Quantity");
            sql.AppendLine(",   SUM(DAT.TotalCostOfGood) AS TotalCost");
            sql.AppendLine(",   SUM(DAT.Quantity * (DAT.Price - DAT.DiscountPrice)) AS TotalRevenue");
            sql.AppendLine("FROM #data AS DAT");
            sql.AppendLine("GROUP BY");
            sql.AppendLine("    DAT.DateDone");
            sql.AppendLine(",   DAT.ID");

            sql.AppendLine(" END");

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql.ToString());
            while (reader.Read())
            {
                var entity = new OrderReport();
                entity.DateDone = Convert.ToDateTime(reader["DateDone"]);
                entity.ID = Convert.ToInt32(reader["ID"]);
                entity.Quantity = Convert.ToInt32(reader["Quantity"]);
                entity.TotalRevenue = Convert.ToDouble(reader["TotalRevenue"]);
                entity.TotalCost = Convert.ToDouble(reader["TotalCost"]);
                list.Add(entity);
            }
            reader.Close();

            var result = list.GroupBy(g => g.DateDone)
                .Select(x => new ProductReportModel()
                {
                    reportDate = x.Key,
                    totalOrder = x.Count(),
                    totalSold = x.Sum(s => s.Quantity),
                    totalRevenue = x.Sum(s => s.TotalRevenue),
                    totalCost = x.Sum(s => s.TotalCost)
                })
                .OrderBy(o => o.reportDate)
                .ToList();

            return result;
        }
        public static ReportModel getReport(string SKU, int CategoryID, string user, DateTime fromdate, DateTime todate)
        {
            int day = Convert.ToInt32((todate - fromdate).TotalDays);

            var userReport = OrderController.getProductReport(SKU, CategoryID, user, fromdate, todate);

            int totalSoldQuantity = userReport.Sum(x => x.totalSold);
            int averageSoldQuantity = totalSoldQuantity / day;

            var userRefundReport = RefundGoodController.getRefundProductReport(SKU, CategoryID, user, fromdate, todate);

            int totalRefundQuantity = userRefundReport.Sum(x => x.totalRefund);
            int averageRefundQuantity = totalRefundQuantity / day;

            double totalRevenue = userReport.Sum(x => x.totalRevenue);
            double totalCost = userReport.Sum(x => x.totalCost);
            double totalRefundRevenue = userRefundReport.Sum(x => x.totalRevenue);
            double totalRefundCost = userRefundReport.Sum(x => x.totalCost);
            double totalRefundFee = userRefundReport.Sum(x => x.totalRefundFee);
            double totalProfit = (totalRevenue - totalCost) - (totalRefundRevenue - totalRefundCost) + totalRefundFee;

            int totalSaleOrder = userReport.Sum(x => x.totalOrder);
            int averageSaleOrder = totalSaleOrder / day;

            var newCustomer = CustomerController.Report(user, fromdate, todate);

            return new ReportModel()
            {
                totalSaleOrder = totalSaleOrder,
                averageSaleOrder = averageSaleOrder,
                totalSoldQuantity = totalSoldQuantity,
                averageSoldQuantity = averageSoldQuantity,
                totalRefundQuantity = totalRefundQuantity,
                averageRefundQuantity = averageRefundQuantity,
                totalRemainQuantity = totalSoldQuantity - totalRefundQuantity,
                averageRemainQuantity = (totalSoldQuantity - totalRefundQuantity) / day,
                totalNewCustomer = newCustomer.Count(),
                totalProfit = totalProfit
            };
        }
        public class ReportModel
        {
            public int totalSaleOrder { get; set; }
            public int averageSaleOrder { get; set; }
            public int totalSoldQuantity { get; set; }
            public int averageSoldQuantity { get; set; }
            public int totalRefundQuantity { get; set; }
            public int averageRefundQuantity { get; set; }
            public int totalRemainQuantity { get; set; }
            public int averageRemainQuantity { get; set; }
            public int totalNewCustomer { get; set; }
            public double totalProfit { get; set; }
        }
        public class ProductReportModel
        {
            public DateTime reportDate { get; set; }
            public int totalOrder { get; set; }
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
                        tranSubName = String.Empty,
                        shippingFee = orderLast.FeeShipping
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

                    // Trường hợp là GHTK
                    if (orderLast.ShippingType == 6)
                        result.shippingFee = "0";

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
            public string shippingFee { get; set; }
        }

        public static ReportProfitCustomerModel reportProfitByCustomer(string phone, DateTime fromDate, DateTime toDate)
        {
            using (var con = new inventorymanagementEntities())
            {
                #region Trích xuất dữ liệu cần dùng

                #region Lọc ra khách hàng đang cần xem
                if (String.IsNullOrEmpty(phone))
                    return null;

                var customer = con.tbl_Customer
                    .Where(x =>
                        x.CustomerPhone.Equals(phone)
                    )
                    .FirstOrDefault();

                if (customer == null)
                    return null;
                #endregion

                #region Kiếm những hóa đơn mà khách hàng đã mua trong khoảng thời gian
                var orders = con.tbl_Order
                    .Where(x => x.CustomerID == customer.ID)
                    .Where(x => x.ExcuteStatus == (int)ExcuteStatus.Done)
                    .Where(x => x.DateDone.HasValue)
                    .Where(x => x.DateDone >= fromDate)
                    .Where(x => x.DateDone <= toDate)
                    .Select(x => new
                    {
                        customerID = x.CustomerID,
                        orderID = x.ID,
                        dateDone = x.DateDone.Value,
                        totalQuantity = x.TotalQuantity,
                        totalCostOfGood = x.TotalCostOfGood,
                        totalPrice = String.IsNullOrEmpty(x.TotalPriceNotDiscount) ? "0" : x.TotalPriceNotDiscount,
                        totalDiscount = x.TotalDiscount.HasValue ? x.TotalDiscount.Value : 0,
                        coupon = x.CouponValue.HasValue ? x.CouponValue.Value : 0,
                        // Phí giao hàng chỉ dùng để tham khảo
                        feeShipping = String.IsNullOrEmpty(x.FeeShipping) ? "0" : x.FeeShipping,
                        refundsGoodsID = x.RefundsGoodsID
                    })
                    .OrderBy(o => o.orderID);
                #endregion

                #region Kiếm những hóa đơn mà khách hàng đã đổi trả trong khoảng thời gian
                var refunds = con.tbl_RefundGoods
                    .Where(x => x.CustomerID == customer.ID)
                    .Where(x => x.CreatedDate.HasValue)
                    .Join(
                        orders.Where(x => x.refundsGoodsID.HasValue),
                        rf => rf.ID,
                        o => o.refundsGoodsID,
                        (rf, o) => rf
                    )
                    .Select(x => new {
                        customerID = x.CustomerID,
                        refundID = x.ID,
                        dateDone = x.CreatedDate.Value,
                        totalQuantity = x.TotalQuantity,
                        // Tiền hoàn trả đã bao gồm phí hoàn trả
                        totalPrice = String.IsNullOrEmpty(x.TotalPrice) ? "0" : x.TotalPrice,
                        totalCostOfGood = x.TotalCostOfGood,
                        totalRefundFee = String.IsNullOrEmpty(x.TotalRefundFee) ? "0" : x.TotalRefundFee
                    })
                    .OrderBy(o => o.refundID);
                #endregion

                #endregion

                #region Lấy thông tin cho việc tính toán lợi nhuận trên từng đơn hàng
                var profitInfo = orders
                    .ToList()
                    .GroupBy(g => new { customerID = g.customerID, dateDone = g.dateDone.ToString("yyyy-MM-dd") })
                    .Select(x => new
                    {
                        customerID = x.Key.customerID,
                        dateDone = x.Key.dateDone,
                        quantityOrder = x.Count(),
                        quantityProduct = x.Sum(s => s.totalQuantity),
                        costOfGoods = x.Sum(s => Convert.ToDouble(s.totalCostOfGood)),
                        price = x.Sum(s => Convert.ToDouble(s.totalPrice)),
                        discount = x.Sum(s => s.totalDiscount),
                        coupon = x.Sum(s => Convert.ToDouble(s.coupon)),
                        // Phí giao hàng chỉ dùng để tham khảo
                        feeShipping = x.Sum(s => Convert.ToDouble(s.feeShipping))
                    })
                    .OrderBy(o => o.dateDone)
                    .ToList();
                #endregion

                #region Thực thi lấy dữ liệu từ dưới database lên
                #region Thực thi lấy thông tin dữ liệu đỗi trả
                var refundInfo = refunds
                    .ToList()
                    .GroupBy(g => new { customerID = g.customerID, dateDone = g.dateDone.ToString("yyyy-MM-dd") })
                    .Select(x => new {
                        customerID = x.Key.customerID,
                        dateDone = x.Key.dateDone,
                        quantityRefund = x.Count(),
                        quantityProduct = x.Sum(s => s.totalQuantity),
                        refundCapital = x.Sum(s => Convert.ToDouble(s.totalCostOfGood)),
                        // Tiền hoàn trả đã bao gồm phí hoàn trả
                        refundMoney = x.Sum(s => Convert.ToDouble(s.totalPrice) + Convert.ToDouble(s.totalRefundFee)),
                        refundFee = x.Sum(s => Convert.ToDouble(s.totalRefundFee))
                    })
                    .OrderBy(o => o.dateDone)
                    .ToList();
                #endregion

                #region Tổng hợp các dữ liệu cần lấy
                var reportData = new List<OrderInfoModel>();

                var date = fromDate.Date;
                while (date <= toDate.Date)
                {
                    reportData.Add(new OrderInfoModel() { dateDone = date });
                    date = date.AddDays(1);
                }

                reportData = reportData
                    .GroupJoin(
                        profitInfo,
                        d => d.dateDone.ToString("yyyy-MM-dd"),
                        p => p.dateDone,
                        (d, p) => new
                        {
                            data = d,
                            profit = p
                        }
                    )
                    .SelectMany(
                        x => x.profit.DefaultIfEmpty(),
                        (parent, child) => new
                        {
                            data = parent.data,
                            profit = child
                        }
                    )
                    .GroupJoin(
                        refundInfo,
                        temp => temp.data.dateDone.ToString("yyyy-MM-dd"),
                        rf => rf.dateDone,
                        (tem, rf) => new
                        {
                            data = tem.data,
                            profit = tem.profit,
                            refund = rf
                        }
                    )
                    .SelectMany(
                        x => x.refund.DefaultIfEmpty(),
                        (parent, child) => new
                        {
                            data = parent.data,
                            profit = parent.profit,
                            refund = child
                        }
                    )
                    .Where(x => x.profit != null || x.refund != null)
                    .Select(x => {
                        var item = new OrderInfoModel() { dateDone = x.data.dateDone };

                        if (x.profit != null)
                        {
                            item.quantityOrder = x.profit != null ? x.profit.quantityOrder : 0;
                            item.quantityProduct = x.profit != null ? Convert.ToInt32(x.profit.quantityProduct) : 0;
                            item.costOfGoods = x.profit != null ? x.profit.costOfGoods : 0;
                            item.price = x.profit != null ? x.profit.price : 0;
                            item.discount = x.profit != null ? x.profit.discount : 0;
                            item.coupon = x.profit != null ? x.profit.coupon : 0;
                            item.feeShipping = x.profit != null ? x.profit.feeShipping : 0;
                        }

                        if (x.refund != null)
                        {
                            item.quantityRefund = x.refund != null ? x.refund.quantityRefund : 0;
                            item.quantityProductRefund = x.refund != null ? Convert.ToInt32(x.refund.quantityProduct) : 0;
                            item.refundCapital = x.refund != null ? x.refund.refundCapital : 0;
                            // Tiền hoàn trả đã bao gồm phí hoàn trả
                            item.refundMoney = x.refund != null ? x.refund.refundMoney : 0;
                            item.refundFee = x.refund != null ? x.refund.refundFee : 0;
                        }

                        return item;
                    })
                    .OrderBy(o => o.dateDone)
                    .ToList();
                #endregion

                #endregion

                #region Xuất data cho báo cáo
                var result = new ReportProfitCustomerModel()
                {
                    customer = new CustomerModel()
                    {
                        id = customer.ID,
                        name = customer.CustomerName,
                        nick = customer.Nick,
                        phone = !String.IsNullOrEmpty(customer.CustomerPhone) ? customer.CustomerPhone :
                            !String.IsNullOrEmpty(customer.CustomerPhone2) ? customer.CustomerPhone2 : customer.CustomerPhoneBackup,
                        address = customer.CustomerAddress,
                        zalo = customer.Zalo,
                        facebook = customer.Facebook
                    },

                    data = reportData
                };
                #endregion

                return result;
            }
        }

        public static List<Models.Common.OrderModel> getOrderQualifiedOfDiscountGroup(int discountGroupID, int customerID = 0)
        {
            using (var con = new inventorymanagementEntities())
            {
                #region Lọc ra đơn khác hàng đã mua và tính số lượng mua
                var order = con.tbl_Order.Where(x => x.ExcuteStatus == (int)ExcuteStatus.Done);

                if (customerID > 0)
                    order = order
                        .Where(x => x.CustomerID.HasValue)
                        .Where(x => x.CustomerID == customerID);
                var orderDetail = order
                    .Join(
                        con.tbl_OrderDetail,
                        o => o.ID,
                        d => d.OrderID.Value,
                        (o, d) => new { order = o, detail = d }
                    )
                    .GroupBy(g => new { customerID = g.order.CustomerID.Value, orderID = g.order.ID })
                    .Select(x => new
                    {
                        customerID = x.Key.customerID,
                        orderID = x.Key.orderID,
                        quantityProduct = x.Sum(s => s.detail.Quantity.HasValue ? s.detail.Quantity.Value : 0)
                    });
                #endregion

                #region Lọc những đơn hàng đủ điều kiện để join vào discount group
                var discount = DiscountGroupController.GetByID(discountGroupID);
                if (discount == null)
                    return null;

                orderDetail = orderDetail.Where(x => x.quantityProduct >= discount.QuantityRequired);

                var data = order
                    .Join(
                        orderDetail,
                        o => o.ID,
                        d => d.orderID,
                        (o, d) => new { order = o, orderDetail = d }
                    )
                    .OrderByDescending(o => o.order.ID)
                    .ToList();
                #endregion

                var result = data.Select(x => new Models.Common.OrderModel() {
                    ID = x.order.ID,
                    CustomerID = x.order.CustomerID.HasValue ? x.order.CustomerID.Value : 0,
                    QuantityProduct = Convert.ToInt32(x.orderDetail.quantityProduct),
                    Price = Convert.ToDouble(x.order.TotalPrice),
                    Discount = Convert.ToDouble(x.order.TotalDiscount),
                    FeeShipping = Convert.ToDouble(x.order.FeeShipping),
                    StaffName = x.order.CreatedBy,
                    DateDone = x.order.DateDone
                })
                .ToList();

                return result;
            }
        }

        public static List<Models.Common.OrderModel> get(int customerID)
        {
            using (var con = new inventorymanagementEntities())
            {
                #region Lọc ra đơn khác hàng đã mua và tính số lượng mua
                var order = con.tbl_Order
                    .Where(x => x.ExcuteStatus == (int)ExcuteStatus.Done)
                    .Where(x => x.CustomerID.HasValue)
                    .Where(x => x.CustomerID == customerID);

                var orderDetail = order
                    .Join(
                        con.tbl_OrderDetail,
                        o => o.ID,
                        d => d.OrderID.Value,
                        (o, d) => new { order = o, detail = d }
                    )
                    .GroupBy(g => new { customerID = g.order.CustomerID.Value, orderID = g.order.ID })
                    .Select(x => new
                    {
                        customerID = x.Key.customerID,
                        orderID = x.Key.orderID,
                        quantityProduct = x.Sum(s => s.detail.Quantity.HasValue ? s.detail.Quantity.Value : 0)
                    });
                #endregion

                var data = order
                    .Join(
                        orderDetail,
                        o => o.ID,
                        d => d.orderID,
                        (o, d) => new { order = o, orderDetail = d }
                    )
                    .OrderByDescending(o => o.order.ID)
                    .ToList();

                var result = data.Select(x => new Models.Common.OrderModel()
                {
                    ID = x.order.ID,
                    CustomerID = x.order.CustomerID.HasValue ? x.order.CustomerID.Value : 0,
                    QuantityProduct = Convert.ToInt32(x.orderDetail.quantityProduct),
                    Price = Convert.ToDouble(x.order.TotalPrice),
                    Discount = Convert.ToDouble(x.order.TotalDiscount),
                    FeeShipping = Convert.ToDouble(x.order.FeeShipping),
                    StaffName = x.order.CreatedBy,
                    DateDone = x.order.DateDone
                })
                .ToList();

                return result;
            }
        }
    }
}