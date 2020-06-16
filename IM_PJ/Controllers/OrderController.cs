﻿using IM_PJ.Models;
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
        #region CRUD
        public static tbl_Order Insert(tbl_Order data)
        {
            using (var con = new inventorymanagementEntities())
            {
                con.tbl_Order.Add(data);
                con.SaveChanges();

                return data;
            }
        }
        public static tbl_Order InsertOnSystem(tbl_Order data)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                dbe.tbl_Order.Add(data);
                dbe.SaveChanges();

                return data;
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
        public static tbl_Order UpdateOnSystem(tbl_Order data)
        {
            using (var con = new inventorymanagementEntities())
            {
                var orderOld = con.tbl_Order.Where(o => o.ID == data.ID).FirstOrDefault();
                if (orderOld != null)
                {
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
                    con.SaveChanges();

                    return orderOld;
                }
                return null;
            }
        }

        public static tbl_Order UpdateExcuteStatus(int orderID, int status, string CreatedBy)
        {
            using (var con = new inventorymanagementEntities())
            {
                var order = con.tbl_Order.Where(x => x.ID == orderID).FirstOrDefault();

                if (order != null)
                {
                    order.ExcuteStatus = status;
                    order.ModifiedDate = DateTime.Now;
                    order.ModifiedBy = CreatedBy;

                    con.SaveChanges();
                }

                return order;
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

        public static List<OrderList> Filter(OrderFilterModel filter, ref PaginationMetadataModel page)
        {
            using (var con = new inventorymanagementEntities())
            {
                #region Loại bớt data chỉ lấy những dữ liệu tỏng 2019-02-15
                // ẩn sản phẩm theo thời gian
                DateTime year = new DateTime(2019, 12, 15);

                var config = ConfigController.GetByTop1();

                if (config.ViewAllOrders == 1)
                {
                    year = new DateTime(2018, 6, 22);
                }

                if (config.ViewAllReports == 0)
                {
                    year = DateTime.Now.AddMonths(-2);
                }

                var orders = con.tbl_Order
                    .Select(x => new
                    {
                        ID = x.ID,
                        CustomerID = x.CustomerID,
                        OrderType = x.OrderType,
                        ExcuteStatus = x.ExcuteStatus,
                        PaymentStatus = x.PaymentStatus,
                        PaymentType = x.PaymentType,
                        ShippingType = x.ShippingType,
                        TotalPrice = x.TotalPrice,
                        TotalDiscount = x.TotalDiscount,
                        FeeShipping = x.FeeShipping,
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
                        CouponValue = x.CouponValue
                    })
                    .Where(x => x.CreatedDate >= year);
                #endregion

                #region Các filter trức tiếp trên bản tbl_Order
                // Filter Created By
                if (!String.IsNullOrEmpty(filter.orderCreatedBy))
                {
                    orders = orders.Where(x =>
                        x.CreatedBy == filter.orderCreatedBy
                    );
                }
                // Filter Order Type
                if (filter.orderType > 0)
                {
                    orders = orders.Where(x =>
                        x.OrderType == filter.orderType
                    );
                }
                // Filter Payment Type
                if (filter.paymentType > 0)
                {
                    orders = orders.Where(x =>
                        x.PaymentType == filter.paymentType
                    );
                }
                // Filter Payment Status
                if (filter.paymentStatus > 0)
                {
                    orders = orders.Where(x =>
                        x.PaymentStatus == filter.paymentStatus
                    );
                }
                // Filter Excute Status
                if (filter.excuteStatus.Count() > 0)
                {
                    orders = orders.Where(x =>
                       filter.excuteStatus.Contains(x.ExcuteStatus.HasValue ? x.ExcuteStatus.Value : 0)
                    );
                }
                // Filter Shipping Type
                if (filter.shippingType.Count() > 0)
                {
                    orders = orders.Where(x =>
                       filter.shippingType.Contains(x.ShippingType.HasValue ? x.ShippingType.Value : 0)
                    );
                }
                // Filter Transport Company
                if (filter.transportCompany > 0)
                {
                    orders = orders.Where(x =>
                       x.TransportCompanyID == filter.transportCompany
                    );
                }
                // Filter Discount
                if (!String.IsNullOrEmpty(filter.discount))
                {
                    if (filter.discount.Equals("yes"))
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
                // Filter Order Created Date
                if (filter.orderFromDate.HasValue && filter.orderToDate.HasValue)
                {
                    if (filter.excuteStatus.Count() == 1 && filter.excuteStatus.Contains(2))
                    {
                        orders = orders.Where(x =>
                            x.DateDone >= filter.orderFromDate &&
                            x.DateDone <= filter.orderToDate
                        );
                    }
                    else
                    {
                        orders = orders.Where(x =>
                            x.CreatedDate >= filter.orderFromDate &&
                            x.CreatedDate <= filter.orderToDate
                        );
                    }
                }
                // Filter Order Note
                if (!String.IsNullOrEmpty(filter.orderNote))
                {
                    if (filter.orderNote.Equals("yes"))
                    {
                        orders = orders.Where(x =>
                            !String.IsNullOrEmpty(x.OrderNote)
                        );
                    }
                    else
                    {
                        orders = orders.Where(x =>
                            String.IsNullOrEmpty(x.OrderNote)
                        );
                    }
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
                    RefundsGoodsID = x.RefundsGoodsID,
                    CouponID = x.CouponID,
                    ShippingCode = x.ShippingCode
                });
                #endregion

                #region Các filter cần liên kết bản
                #region Tìm kiếm theo Customer, Order Detail
                var orderDetailFilter = con.tbl_OrderDetail
                    .Join(
                        orderFilter,
                        d => d.OrderID,
                        h => h.ID,
                        (d, h) => d
                    )
                    .Select(x => new {
                        OrderID = x.OrderID,
                        SKU = x.SKU,
                        Quantity = x.Quantity.HasValue ? x.Quantity.Value : 0
                    });

                var customerFilter = con.tbl_Customer
                    .Join(
                        orderFilter
                            .Where(x => x.CustomerID.HasValue)
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
                    });

                #region Tìm kiếm theo từ khóa
                // Filter orderid or customername or customerphone or nick or shipcode
                if (!String.IsNullOrEmpty(filter.search))
                {
                    string search = Regex.Replace(filter.search.Trim(), @"[^0-9a-zA-Z\s_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+", "").Trim().ToLower();
                    search = UnSign.convert(search);

                    var number = Regex.IsMatch(search, @"^\d+$");

                    if (filter.searchType == (int)SearchType.Order)
                    {
                        if (number)
                        {
                            if (search.Length <= 6)
                            {
                                orderFilter = orderFilter.Where(x => x.ID.ToString() == search);
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
                                        x.customer.CustomerPhone == search ||
                                        x.customer.CustomerPhone2 == search ||
                                        x.customer.Zalo == search ||
                                        x.order.ShippingCode == search
                                    )
                                    .Select(x => x.order);

                                customerFilter = customerFilter
                                    .Join(
                                        orderFilter
                                            .Where(x => x.CustomerID.HasValue)
                                            .GroupBy(g => g.CustomerID)
                                            .Select(x => new { CustomerID = x.Key }),
                                        c => c.ID,
                                        h => h.CustomerID,
                                        (c, h) => c
                                    );
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
                                    x.customer.UnSignedName.ToLower().Contains(search) ||
                                    x.customer.UnSignedNick.ToLower().Contains(search) ||
                                    x.order.ShippingCode.ToLower() == search
                                )
                                .Select(x => x.order);

                            customerFilter = customerFilter
                                .Join(
                                    orderFilter
                                        .Where(x => x.CustomerID.HasValue)
                                        .GroupBy(g => g.CustomerID)
                                        .Select(x => new { CustomerID = x.Key }),
                                    c => c.ID,
                                    h => h.CustomerID,
                                    (c, h) => c
                                );
                        }

                    }
                    else if (filter.searchType == (int)SearchType.Product)
                    {
                        orderDetailFilter = orderDetailFilter.Where(x => x.SKU.ToUpper().StartsWith(search));
                        orderFilter = orderFilter
                            .Join(
                                orderDetailFilter,
                                h => h.ID,
                                d => d.OrderID,
                                (h, d) => h
                            );
                    }
                }
                #endregion

                #region Tìm kiếm theo số lượng đơn hàng
                // Get info quantiy
                var orderQuantity = orderFilter
                    .Join(
                        orderDetailFilter,
                        h => h.ID,
                        od => od.OrderID,
                        (h, od) => new
                        {
                            OrderID = h.ID,
                            Quantity = od.Quantity
                        }
                    )
                    .GroupBy(x => x.OrderID)
                    .Select(g => new
                    {
                        OrderID = g.Key,
                        Quantity = g.Sum(x => x.Quantity)
                    });

                // Filter quantity
                if (!String.IsNullOrEmpty(filter.quantity))
                {
                    if (filter.quantity.Equals("greaterthan"))
                    {
                        orderFilter = orderFilter
                            .Join(
                                orderQuantity.Where(p => p.Quantity >= filter.quantityFrom),
                                o => o.ID,
                                b => b.OrderID,
                                (o, b) => o
                            );
                    }
                    else if (filter.quantity.Equals("lessthan"))
                    {
                        orderFilter = orderFilter
                            .Join(
                                orderQuantity.Where(p => p.Quantity <= filter.quantityTo),
                                o => o.ID,
                                b => b.OrderID,
                                (o, b) => o
                            );
                    }
                    else if (filter.quantity.Equals("between"))
                    {
                        orderFilter = orderFilter
                            .Join(
                                orderQuantity.Where(p => p.Quantity >= filter.quantityFrom && p.Quantity <= filter.quantityTo),
                                o => o.ID,
                                b => b.OrderID,
                                (o, b) => o
                            );
                    }
                }
                #endregion
                #endregion

                #region Tìm kiếm theo Bank, Transfers
                var transferFilter = con.BankTransfers
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
                    });

                #region Tìm kiếm trạng thái chuyển khoản hoặc ngày kết kết thúc chuyển khoản
                // Filter Transfer Status or DoneAt
                if (filter.transferStatus > 0 || (filter.transferFromDate.HasValue && filter.transferToDate.HasValue))
                {
                    var bankTransfers = con.BankTransfers.Where(x => 1 == 1);

                    if (filter.transferStatus == 1)
                    {
                        bankTransfers = bankTransfers.Where(x => x.Status == 1);
                    }
                    else if (filter.transferStatus == 2)
                    {
                        bankTransfers = bankTransfers.Where(x => x.Status != 1);
                    }

                    if (filter.transferFromDate.HasValue && filter.transferToDate.HasValue)
                    {

                        bankTransfers = bankTransfers.Where(x =>
                            x.DoneAt >= filter.transferFromDate &&
                            x.DoneAt <= filter.transferToDate
                        );
                    }

                    orderFilter = orderFilter
                        .Join(
                            bankTransfers,
                            h => h.ID,
                            b => b.OrderID,
                            (h, b) => h
                        );
                }
                #endregion

                #region Tìm kiếm theo tài khoản ngân hàng nhận tiền
                if (filter.bankReceive != 0)
                {
                    transferFilter = transferFilter.Where(x => x.AccBankID == filter.bankReceive);
                    orderFilter = orderFilter
                        .Join(
                            transferFilter,
                            o => o.ID,
                            b => b.OrderID,
                            (o, b) => o
                        );
                }
                #endregion
                #endregion

                #region Tìm kiếm theo Delivery
                var deliveryFilter = con.Deliveries
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
                    });

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
                        );

                    orderFilter = orderFilter
                        .Join(
                            deliveryFilter,
                            h => h.ID,
                            d => d.OrderID,
                            (h, d) => h
                        );
                }
                #endregion

                #region Tìm kiếm đợt giao hàng
                // Filter Delivery times
                if (filter.deliveryTimes > 0)
                {
                    deliveryFilter = deliveryFilter.Where(x => x.Times == filter.deliveryTimes);
                    orderFilter = orderFilter
                        .Join(
                            deliveryFilter,
                            h => h.ID,
                            d => d.OrderID,
                            (h, d) => h
                        );
                }
                #endregion

                #region Tìm kiếm theo shipper
                // Filter Shipper
                if (filter.shipper > 0)
                {
                    deliveryFilter = deliveryFilter.Where(x => x.ShipperID == filter.shipper);
                    orderFilter = orderFilter
                        .Join(
                            deliveryFilter,
                            h => h.ID,
                            d => d.OrderID,
                            (h, d) => h
                        );
                }
                #endregion

                #region Tìm kiếm theo phiếu giao hàng
                if (filter.deliveryStatus != 0)
                {
                    deliveryFilter = deliveryFilter.Where(x => x.Status == filter.deliveryStatus);
                    orderFilter = orderFilter
                        .Join(
                            deliveryFilter,
                            o => o.ID,
                            b => b.OrderID,
                            (o, b) => o
                        );
                }

                switch (filter.invoiceStatus)
                {
                    case (int)InvoiceStatus.Yes:
                        deliveryFilter = deliveryFilter.Where(x => !String.IsNullOrEmpty(x.Image));

                        orderFilter = orderFilter
                            .Join(
                                deliveryFilter,
                                o => o.ID,
                                b => b.OrderID,
                                (o, b) => o
                            );
                        break;
                    case (int)InvoiceStatus.No:
                        deliveryFilter = deliveryFilter.Where(x => String.IsNullOrEmpty(x.Image));
                        orderFilter = orderFilter
                            .Join(
                                deliveryFilter,
                                o => o.ID,
                                b => b.OrderID,
                                (o, b) => o
                            );
                        break;
                    default:
                        break;
                }
                #endregion
                #endregion

                #region Tìm kiếm theo Fee
                // Get fee of product
                var feeFilter = con.Fees
                    .Join(
                        orderFilter,
                        f => f.OrderID,
                        h => h.ID,
                        (f, h) => new
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
                    });

                #region Tìm kiếm theo phí sản phẩm
                // Filter fee of product
                if (!String.IsNullOrEmpty(filter.otherFee))
                {
                    if (filter.otherFee.Equals("yes"))
                    {
                        feeFilter = feeFilter.Where(x => x.OtherFeeValue != 0);
                        orderFilter = orderFilter
                            .Join(
                                feeFilter,
                                o => o.ID,
                                b => b.OrderID,
                                (o, b) => o
                            );
                    }
                    else
                    {
                        feeFilter = feeFilter.Where(x => x.OtherFeeValue == 0);
                        orderFilter = orderFilter
                            .Join(
                                feeFilter,
                                o => o.ID,
                                b => b.OrderID,
                                (o, b) => o
                            );
                    }
                }
                #endregion
                #endregion

                #region Lọc theo mã giảm giá
                var couponFilter = con.Coupons
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
                    );

                if (filter.couponStatus != 0)
                {
                    var orderCoupon = orderFilter
                       .Where(x => x.CouponID.HasValue)
                       .Join(
                           couponFilter,
                           o => o.ID,
                           c => c.orderID,
                           (o, c) => o
                       );
                    if (filter.couponStatus == (int)CouponStatus.Yes)
                        orderFilter = orderCoupon;
                    if (filter.couponStatus == (int)CouponStatus.No)
                        orderFilter = orderFilter.Except(orderCoupon);
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
                    .Take(page.pageSize);
                #endregion

                #region Xuất dữ liệu
                #region Xuất thông tin chính
                // Get info main
                var header = orders
                    .Join(
                        orderFilter,
                        o => o.ID,
                        h => h.ID,
                        (o, h) => o
                    )
                    .OrderByDescending(o => o.ID)
                    .ToList();
                #endregion

                #region Xuất thông tin về số lượng order
                // Get info quantiy
                var body = orderQuantity
                    .Join(
                        orderFilter,
                        b => b.OrderID,
                        o => o.ID,
                        (b, o) => b
                    )
                    .OrderByDescending(o => o.OrderID)
                    .ToList();
                #endregion

                #region Xuất thông tin về tra hàng
                // Get info refunds
                var refunds = con.tbl_RefundGoods
                    .Join(
                        orderFilter.Where(x => x.RefundsGoodsID.HasValue),
                        r => r.ID,
                        h => h.RefundsGoodsID.Value,
                        (r, h) => new
                        {
                            OrderID = h.ID,
                            RefundsGoodsID = h.RefundsGoodsID,
                            TotalPrice = r.TotalPrice
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
                // Get info transfer bank
                var transfers = transferFilter
                    .Join(
                        orderFilter,
                        t => t.OrderID,
                        h => h.ID,
                        (t, h) => t
                    )
                    .Join(
                        con.Banks,
                        h => h.CusBankID,
                        c => c.ID,
                        (h, c) => new
                        {
                            transfer = h,
                            bank = new { CusBankName = c.BankName }
                        }
                    )
                    .Join(
                        con.BankAccounts,
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
                // Get info delivery
                var deliveries = deliveryFilter
                    .Join(
                        orderFilter,
                        d => d.OrderID,
                        o => o.ID,
                        (d, o) => d
                    )
                    .GroupJoin(
                        con.Shippers,
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
                deliveries = deliveries
                    .Join(
                        deliveries
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
                var coupons = couponFilter
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
                        body,
                        h => h.ID,
                        b => b.OrderID,
                        (h, b) => new {
                            header = h,
                            body = b
                        }
                    )
                    .Join(
                        customer,
                        temp => new {
                            OrderID = temp.header.ID,
                            CustomerID = temp.header.CustomerID.HasValue ? temp.header.CustomerID.Value : 0
                        },
                        c => new { OrderID = c.OrderID, CustomerID = c.CustomerID },
                        (temp, c) => new {
                            header = temp.header,
                            body = temp.body,
                            customer = c
                        }
                    )
                    .GroupJoin(
                        refunds,
                        temp => new { OrderID = temp.header.ID, RefundsGoodsID = temp.header.RefundsGoodsID },
                        rf => new { OrderID = rf.OrderID, RefundsGoodsID = rf.RefundsGoodsID },
                        (temp, rf) => new {
                            header = temp.header,
                            body = temp.body,
                            customer = temp.customer,
                            refund = rf
                        }
                    )
                    .SelectMany(
                        x => x.refund.DefaultIfEmpty(),
                        (parent, child) => new {
                            header = parent.header,
                            body = parent.body,
                            customer = parent.customer,
                            refund = child
                        }
                    )
                    .GroupJoin(
                        fee,
                        temp => temp.header.ID,
                        f => f.OrderID,
                        (temp, f) => new {
                            header = temp.header,
                            body = temp.body,
                            customer = temp.customer,
                            refund = temp.refund,
                            fee = f
                        }
                    )
                    .SelectMany(
                        x => x.fee.DefaultIfEmpty(),
                        (parent, child) => new {
                            header = parent.header,
                            body = parent.body,
                            customer = parent.customer,
                            refund = parent.refund,
                            fee = child
                        }
                    )
                    .GroupJoin(
                        transfers,
                        temp => temp.header.ID,
                        t => t.OrderID,
                        (temp, t) => new {
                            header = temp.header,
                            body = temp.body,
                            customer = temp.customer,
                            refund = temp.refund,
                            fee = temp.fee,
                            trans = t
                        }
                    )
                    .SelectMany(
                        x => x.trans.DefaultIfEmpty(),
                        (parent, child) => new {
                            header = parent.header,
                            body = parent.body,
                            customer = parent.customer,
                            refund = parent.refund,
                            fee = parent.fee,
                            trans = child
                        }
                    )
                    .GroupJoin(
                        deliveries,
                        temp => temp.header.ID,
                        d => d.OrderID,
                        (temp, d) => new {
                            header = temp.header,
                            body = temp.body,
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
                            header = parent.header,
                            body = parent.body,
                            customer = parent.customer,
                            refund = parent.refund,
                            fee = parent.fee,
                            trans = parent.trans,
                            delivery = child
                        }
                    )
                    .GroupJoin(
                        coupons,
                        temp => temp.header.ID,
                        c => c.orderID,
                        (temp, c) => new {
                            header = temp.header,
                            body = temp.body,
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
                            header = parent.header,
                            body = parent.body,
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

                        // OrderID
                        result.ID = temp.header.ID;
                        result.CustomerID = temp.header.CustomerID.HasValue ? temp.header.CustomerID.Value : 0;
                        result.OrderType = temp.header.OrderType.HasValue ? temp.header.OrderType.Value : 0;
                        result.ExcuteStatus = temp.header.ExcuteStatus.HasValue ? temp.header.ExcuteStatus.Value : 0;
                        result.PaymentStatus = temp.header.PaymentStatus.HasValue ? temp.header.PaymentStatus.Value : 0;
                        result.PaymentType = temp.header.PaymentType.HasValue ? temp.header.PaymentType.Value : 0;
                        result.ShippingType = temp.header.ShippingType.HasValue ? temp.header.ShippingType.Value : 0;
                        result.TotalPrice = Convert.ToDouble(temp.header.TotalPrice);
                        result.TotalDiscount = temp.header.TotalDiscount.Value;
                        result.FeeShipping = Convert.ToDouble(temp.header.FeeShipping);
                        result.CreatedBy = temp.header.CreatedBy;
                        result.CreatedDate = temp.header.CreatedDate.HasValue ? temp.header.CreatedDate.Value : DateTime.Now;
                        result.DateDone = temp.header.DateDone;
                        result.OrderNote = temp.header.OrderNote;
                        result.RefundsGoodsID = temp.header.RefundsGoodsID;
                        result.ShippingCode = temp.header.ShippingCode;
                        result.TransportCompanyID = temp.header.TransportCompanyID;
                        result.TransportCompanySubID = temp.header.TransportCompanySubID;
                        result.PostalDeliveryType = temp.header.PostalDeliveryType.HasValue ? temp.header.PostalDeliveryType.Value : 0;
                        // Order Detail
                        result.Quantity = Convert.ToInt32(temp.body.Quantity);
                        // Customer
                        result.CustomerName = temp.customer.CustomerName;
                        result.Nick = temp.customer.Nick;
                        result.CustomerPhone = temp.customer.CustomerPhone;
                        // Refunds
                        result.TotalRefund = temp.refund != null ? Convert.ToDouble(temp.refund.TotalPrice) : 0;
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
                        // Phiếm giảm giắ
                        if (temp.coupon != null)
                        {
                            result.CouponCode = temp.coupon.couponCode;
                            result.CouponValue = temp.header.CouponValue.HasValue ? temp.header.CouponValue.Value : 0;
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
                                && x.PaymentStatus != 1)
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
                            TotalShippingFee = order.FeeShipping,
                            DateDone = order.DateDone,
                            TotalCouponValue = order.CouponValue
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
                            TotalShippingFee = order.TotalShippingFee,
                            DateDone = order.DateDone.Value,
                            TotalCouponValue = order.TotalCouponValue
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
                            TotalRefundFee = refund.TotalRefundFee,
                            DateDone = refund.CreatedDate
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
                            TotalRefundFee = refund.TotalRefundFee,
                            DateDone = refund.DateDone.Value
                        })
                      .ToList();
                #region Tính toán số liệu báo cáo
                #region Tính toán theo ngày với từng đơn hàng
                var orderDate = inforOrder
                    .Select(x => new
                    {
                        DateDone = x.DateDone.Date,
                        OrderID = x.OrderID,
                        SoldQuantity = Convert.ToInt32(x.Quantity),
                        SalePrice = x.Quantity * Convert.ToDouble(x.Price.HasValue ? x.Price.Value : 0),
                        SaleCost = x.Quantity * Convert.ToDouble(x.CostOfGood),
                        TotalDiscount = Convert.ToDouble(x.TotalDiscount.HasValue ? x.TotalDiscount.Value : 0),
                        TotalOtherFee = Convert.ToDouble(x.TotalOtherFee.HasValue ? x.TotalOtherFee.Value : 0),
                        TotalShippingFee = Convert.ToDouble(String.IsNullOrEmpty(x.TotalShippingFee) ? "0" : x.TotalShippingFee),
                        TotalCouponValue = Convert.ToDouble(x.TotalCouponValue.HasValue ? x.TotalCouponValue.Value : 0),
                    }
                    )
                    .GroupBy(g => new { DateDone = g.DateDone, OrderID = g.OrderID })
                    .Select(x => new
                    {
                        DateDone = x.Key.DateDone,
                        OrderID = x.Key.OrderID,
                        Number = 1,
                        TotalSoldQuantity = x.Sum(s => s.SoldQuantity),
                        TotalSalePrice = x.Sum(s => s.SalePrice),
                        TotalSaleCost = x.Sum(s => s.SaleCost),
                        TotalSaleDiscount = x.Max(m => m.TotalDiscount),
                        TotalOtherFee = x.Max(m => m.TotalOtherFee),
                        TotalShippingFee = x.Max(m => m.TotalShippingFee),
                        TotalCouponValue = x.Max(m => m.TotalCouponValue)
                    }
                    )
                    .GroupBy(g => g.DateDone)
                    .Select(x => new
                    {
                        DateDone = x.Key,
                        TotalNumberOfOrder = x.Sum(s => s.Number),
                        TotalSoldQuantity = x.Sum(s => s.TotalSoldQuantity),
                        TotalSalePrice = x.Sum(s => s.TotalSalePrice),
                        TotalSaleCost = x.Sum(s => s.TotalSaleCost),
                        TotalSaleDiscount = x.Sum(s => s.TotalSaleDiscount),
                        TotalOtherFee = x.Sum(s => s.TotalOtherFee),
                        TotalShippingFee = x.Sum(s => s.TotalShippingFee),
                        TotalCouponValue = x.Sum(s => s.TotalCouponValue),
                    })
                    .ToList();
                #endregion

                #region Tính toán theo ngày với từng đơn hàng đổi trả
                var refundDate = infoRefund
                    .Select(x => new
                    {
                        DateDone = x.DateDone.Date,
                        RefundGoodsID = x.RefundGoodsID,
                        RefundQuantity = Convert.ToInt32(x.Quantity),
                        RefundPrice = x.Quantity * Convert.ToDouble(String.IsNullOrEmpty(x.SoldPrice) ? "0" : x.SoldPrice),
                        RefundCost = x.Quantity * Convert.ToDouble(x.CostOfGood),
                        TotalRefundFee = Convert.ToDouble(String.IsNullOrEmpty(x.TotalRefundFee) ? "0" : x.TotalRefundFee)
                    })
                    .GroupBy(g => new { DateDone = g.DateDone, RefundGoodsID = g.RefundGoodsID })
                    .Select(x => new
                    {
                        DateDone = x.Key.DateDone,
                        RefundGoodsID = x.Key.RefundGoodsID,
                        TotalRefundQuantity = x.Sum(s => s.RefundQuantity),
                        TotalRefundPrice = x.Sum(s => s.RefundPrice),
                        TotalRefundCost = x.Sum(s => s.RefundCost),
                        TotalRefundFee = x.Max(m => m.TotalRefundFee)
                    })
                    .GroupBy(g => g.DateDone)
                    .Select(x => new
                    {
                        DateDone = x.Key,
                        TotalRefundQuantity = x.Sum(s => s.TotalRefundQuantity),
                        TotalRefundPrice = x.Sum(s => s.TotalRefundPrice),
                        TotalRefundCost = x.Sum(s => s.TotalRefundCost),
                        TotalRefundFee = x.Sum(s => s.TotalRefundFee)
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
                                TotalOtherFee = parent.o.TotalOtherFee,
                                TotalShippingFee = parent.o.TotalShippingFee,
                                TotalCouponValue = parent.o.TotalCouponValue
                            };

                            if (child != null)
                            {
                                item.TotalRefundQuantity = child.TotalRefundQuantity;
                                item.TotalRefundPrice = child.TotalRefundPrice;
                                item.TotalRefundCost = child.TotalRefundCost;
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

            sql.AppendLine(String.Format("SELECT Ord.ID, SUM(ISNULL(OrdDetail.Quantity, 0)) AS Quantity, SUM(OrdDetail.Quantity * ISNULL(Product.CostOfGood, Variable.CostOfGood)) AS TotalCost, SUM(OrdDetail.Quantity * (OrdDetail.Price - Ord.DiscountPerProduct)) AS TotalRevenue"));
            sql.AppendLine(String.Format("FROM tbl_Order AS Ord"));
            sql.AppendLine(String.Format("INNER JOIN tbl_OrderDetail AS OrdDetail"));
            sql.AppendLine(String.Format("ON     Ord.ID = OrdDetail.OrderID"));
            sql.AppendLine(String.Format("LEFT JOIN tbl_ProductVariable AS Variable"));
            sql.AppendLine(String.Format("ON     OrdDetail.SKU = Variable.SKU"));
            sql.AppendLine(String.Format("LEFT JOIN tbl_Product AS Product"));
            sql.AppendLine(String.Format("ON     OrdDetail.SKU = Product.ProductSKU"));
            sql.AppendLine(String.Format("WHERE 1 = 1"));
            if (!String.IsNullOrEmpty(CreatedBy))
            {
                sql.AppendLine(String.Format("    AND Ord.CreatedBy = '{0}'", CreatedBy));
            }
            sql.AppendLine(String.Format("    AND Ord.ExcuteStatus = 2"));
            sql.AppendLine(String.Format("    AND (Ord.PaymentStatus = 2 OR Ord.PaymentStatus = 3 OR Ord.PaymentStatus = 4)"));
            sql.AppendLine(String.Format("    AND    CONVERT(NVARCHAR(10), Ord.DateDone, 121) BETWEEN CONVERT(NVARCHAR(10), '{0:yyyy-MM-dd}', 121) AND CONVERT(NVARCHAR(10), '{1:yyyy-MM-dd}', 121)", fromDate, toDate));
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
            sql.AppendLine(",   Ord.DiscountPerProduct");
            sql.AppendLine(",   OrdDetail.SKU");
            sql.AppendLine(",   OrdDetail.Quantity");
            sql.AppendLine(",   OrdDetail.Price");
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
            sql.AppendLine(",   SUM(DAT.Quantity * ISNULL(PRO.CostOfGood, 0)) AS TotalCost");
            sql.AppendLine(",   SUM(DAT.Quantity * (DAT.Price - DAT.DiscountPerProduct)) AS TotalRevenue");
            sql.AppendLine("FROM #data AS DAT");
            sql.AppendLine("INNER JOIN (");
            sql.AppendLine("    SELECT");
            sql.AppendLine("        Product.CategoryID,");
            sql.AppendLine("        (");
            sql.AppendLine("            CASE Product.ProductStyle");
            sql.AppendLine("                WHEN 1 THEN Product.ProductSKU");
            sql.AppendLine("                ELSE Variable.SKU");
            sql.AppendLine("            END");
            sql.AppendLine("        ) AS SKU,");
            sql.AppendLine("        (");
            sql.AppendLine("            CASE Product.ProductStyle");
            sql.AppendLine("                WHEN 1 THEN Product.CostOfGood");
            sql.AppendLine("                ELSE Variable.CostOfGood");
            sql.AppendLine("            END");
            sql.AppendLine("        ) AS CostOfGood");
            sql.AppendLine("    FROM tbl_Product AS Product");
            sql.AppendLine("    LEFT JOIN tbl_ProductVariable AS Variable");
            sql.AppendLine("    ON Product.ID = Variable.ProductID");
            sql.AppendLine("    WHERE 1 = 1");
            if (CategoryID > 0)
            {
                sql.AppendLine("    AND EXISTS(");
                sql.AppendLine("            SELECT");
                sql.AppendLine("                    NULL AS DUMMY");
                sql.AppendLine("            FROM");
                sql.AppendLine("                    #category");
                sql.AppendLine("            WHERE");
                sql.AppendLine("                    ID = Product.CategoryID");
                sql.AppendLine("    )");
            }
            sql.AppendLine(") AS PRO");
            sql.AppendLine("ON     DAT.SKU = PRO.SKU");
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
                        x.CustomerPhone.Equals(phone) ||
                        x.CustomerPhone2.Equals(phone) ||
                        x.CustomerPhoneBackup.Equals(phone)
                    )
                    .FirstOrDefault();

                if (customer == null)
                    return null;
                #endregion

                #region Kiếm những hóa đơn mà khách hàng đã mua trong khoảng thời gian
                var orders = con.tbl_Order
                    .Where(x => x.CustomerID == customer.ID)
                    .Where(x => x.ExcuteStatus == (int)ExcuteStatus.Done)
                    .Where(x => x.DateDone >= fromDate)
                    .Where(x => x.DateDone <= toDate)
                    .OrderBy(o => o.ID);
                #endregion

                #region Kiếm những hóa đơn mà khách hàng đã đổi trả trong khoảng thời gian
                var refunds = con.tbl_RefundGoods
                    .Where(x => x.CustomerID == customer.ID)
                    .Where(x => x.CreatedDate >= fromDate)
                    .Where(x => x.CreatedDate <= toDate)
                    .OrderBy(o => o.ID);
                #endregion

                #endregion

                #region Lấy các thông tin cho xuất báo cáo

                #region Lấy các sản phẩm mà khách hàng đã mua
                var orderDetials = con.tbl_OrderDetail
                    .Join(
                        orders,
                        det => det.OrderID,
                        ord => ord.ID,
                        (det, ord) => det
                    )
                    .OrderBy(o => o.ProductID)
                    .ThenBy(o => o.ProductVariableID);

                var productOrdered = con.tbl_Product
                    .Join(
                        orderDetials.Where(x => x.ProductVariableID == 0),
                        p => p.ID,
                        det => det.ProductID.Value,
                        (p, det) => new { orderDetial = det, product = p }
                    )
                    .GroupBy(g => g.orderDetial.OrderID.Value)
                    .Select(x => new
                    {
                        orderID = x.Key,
                        quantityProduct = x.Count(),
                        costOfGoods = x.Sum(s => s.product.CostOfGood.HasValue ? s.product.CostOfGood.Value : 0),
                        price = x.Sum(s => s.orderDetial.Price.HasValue ? s.orderDetial.Price.Value : 0)
                    })
                    .OrderBy(o => o.orderID);

                var variableOrdered = con.tbl_ProductVariable
                    .Join(
                        orderDetials.Where(x => x.ProductVariableID != 0),
                        v => v.ID,
                        det => det.ProductVariableID.Value,
                        (v, det) => new { orderDetial = det, variable = v }
                    )
                    .GroupBy(g => g.orderDetial.OrderID.Value)
                    .Select(x => new
                    {
                        orderID = x.Key,
                        quantityProduct = x.Count(),
                        costOfGoods = x.Sum(s => s.variable.CostOfGood.HasValue ? s.variable.CostOfGood.Value : 0),
                        price = x.Sum(s => s.orderDetial.Price.HasValue ? s.orderDetial.Price.Value : 0)
                    })
                    .OrderBy(o => o.orderID);
                #endregion

                #endregion

                #region Dữ liệu tính toán xuất ra báo cáo

                #region Lấy thông tin cho việc tính toán lợi nhuận trên từng đơn hàng
                var profitInfo = orders
                    .GroupJoin(
                        productOrdered,
                        o => o.ID,
                        po => po.orderID,
                        (o, po) => new
                        {
                            order = o,
                            productOrdered = po
                        }
                    )
                    .SelectMany(
                        x => x.productOrdered.DefaultIfEmpty(),
                        (parent, child) => new
                        {
                            order = parent.order,
                            quantityProduct = child != null ? child.quantityProduct : 0,
                            costOfGoods = child != null ? child.costOfGoods : 0,
                            price = child != null ? child.price : 0
                        }
                    )
                    .GroupJoin(
                        variableOrdered,
                        temp => temp.order.ID,
                        vo => vo.orderID,
                        (temp, vo) => new
                        {
                            order = temp.order,
                            quantityProduct = temp.quantityProduct,
                            costOfGoods = temp.costOfGoods,
                            price = temp.price,
                            variableOrdered = vo
                        }
                    )
                    .SelectMany(
                        x => x.variableOrdered.DefaultIfEmpty(),
                        (parent, child) => new
                        {
                            customerID = parent.order.ID,
                            dateDone = parent.order.DateDone.Value,
                            orderID = parent.order.ID,
                            quantityProduct = parent.quantityProduct + (child != null ? child.quantityProduct : 0),
                            costOfGoods = parent.costOfGoods + (child != null ? child.costOfGoods : 0),
                            price = parent.price + (child != null ? child.price : 0),
                            discount = parent.order.TotalDiscount.HasValue ? parent.order.TotalDiscount.Value : 0,
                            feeShipping = parent.order.FeeShipping
                        }
                    )
                    .ToList()
                    .GroupBy(g => new { customerID = g.customerID, dateDone = g.dateDone, orderID = g.orderID })
                    .Select(x => new
                    {
                        customerID = x.Key.customerID,
                        dateDone = x.Key.dateDone,
                        quantityProduct = x.Sum(s => s.quantityProduct),
                        costOfGoods = x.Sum(s => s.costOfGoods),
                        price = x.Sum(s => s.price),
                        discount = x.Sum(s => s.discount),
                        feeShipping = x.Sum(s => !String.IsNullOrEmpty(s.feeShipping) ? Convert.ToDouble(s.feeShipping) : 0)
                    }
                    )
                    .OrderBy(o => o.dateDone);
                #endregion

                #region Lấy thông tin các hàng đổi trả
                var refundDetail = refunds
                    .Join(
                        con.tbl_RefundGoodsDetails,
                        h => h.ID,
                        b => b.RefundGoodsID,
                        (h, b) => b
                    );

                var productRefund = con.tbl_Product
                    .Join(
                        refundDetail.Where(x => x.ProductType == 1),
                        p => p.ProductSKU,
                        rfd => rfd.SKU,
                        (p, rfd) => new { refundDetail = rfd, product = p }
                    )
                    .ToList()
                    .GroupBy(g => g.refundDetail.RefundGoodsID.Value)
                    .Select(x => new
                    {
                        refundID = x.Key,
                        quantityProduct = x.Count(),
                        refundCapital = x.Sum(s => s.product.CostOfGood.HasValue ? s.product.CostOfGood.Value : 0),
                        refundMoney = x.Sum(s => Convert.ToInt32(s.refundDetail.TotalPriceRow))
                    })
                    .OrderBy(o => o.refundID);

                var variableRefund = con.tbl_ProductVariable
                    .Join(
                        refundDetail.Where(x => x.ProductType == 2),
                        v => v.SKU,
                        rfd => rfd.SKU,
                        (v, rfd) => new { refundDetail = rfd, variable = v }
                    )
                    .ToList()
                    .GroupBy(g => g.refundDetail.RefundGoodsID.Value)
                    .Select(x => new
                    {
                        refundID = x.Key,
                        quantityProduct = x.Count(),
                        refundCapital = x.Sum(s => s.variable.CostOfGood.HasValue ? s.variable.CostOfGood.Value : 0),
                        refundMoney = x.Sum(s => Convert.ToInt32(s.refundDetail.TotalPriceRow))
                    })
                    .OrderBy(o => o.refundID);
                #endregion

                #endregion

                #region Thực thi lấy dữ liệu từ dưới database lên
                var refundFilter = refunds.ToList();

                #region Thực thi lấy thông tin dữ liệu đỗi trả
                var refundInfo = refundFilter
                    .GroupJoin(
                        productRefund,
                        rf => rf.ID,
                        p => p.refundID,
                        (rf, p) => new
                        {
                            refund = rf,
                            productRefund = p
                        }
                    )
                    .SelectMany(
                        x => x.productRefund.DefaultIfEmpty(),
                        (parent, child) => new
                        {
                            refund = parent.refund,
                            productRefund = child
                        }
                    )
                    .GroupJoin(
                        variableRefund,
                        temp => temp.refund.ID,
                        v => v.refundID,
                        (temp, v) => new
                        {
                            refund = temp.refund,
                            productRefund = temp.productRefund,
                            variableRefund = v
                        }
                    )
                    .SelectMany(
                        x => x.variableRefund.DefaultIfEmpty(),
                        (parent, child) => new
                        {
                            refund = parent.refund,
                            productRefund = parent.productRefund,
                            variableRefund = child
                        }
                    )
                    .Select(x => new {
                        customerID = x.refund.CustomerID,
                        dateDone = x.refund.CreatedDate.Value,
                        refundID = x.refund.ID,
                        quantityProduct = (x.productRefund != null ? x.productRefund.quantityProduct : 0) + (x.variableRefund != null ? x.variableRefund.quantityProduct : 0),
                        refundCapital = (x.productRefund != null ? x.productRefund.refundCapital : 0) + (x.variableRefund != null ? x.variableRefund.refundCapital : 0),
                        refundMoney = (x.productRefund != null ? x.productRefund.refundMoney : 0) + (x.variableRefund != null ? x.variableRefund.refundMoney : 0),
                        refundFee = x.refund != null ? Convert.ToInt32(x.refund.TotalRefundFee) : 0,
                    })
                    .GroupBy(g => new { customerID = g.customerID, dateDone = g.dateDone, refundID = g.refundID })
                    .Select(x => new {
                        customerID = x.Key.customerID,
                        dateDone = x.Key.dateDone,
                        quantityProduct = x.Sum(s => s.quantityProduct),
                        refundCapital = x.Sum(s => s.refundCapital),
                        refundMoney = x.Sum(s => s.refundMoney),
                        refundFee = x.Sum(s => s.refundFee)
                    })
                    .OrderBy(o => o.dateDone);
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
                        p => p.dateDone.ToString("yyyy-MM-dd"),
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
                        rf => rf.dateDone.ToString("yyyy-MM-dd"),
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
                    .GroupBy(g => g.data.dateDone.ToString("yyyy-MM-dd"))
                    .Select(x => new OrderInfoModel()
                    {
                        dateDone = DateTime.Parse(x.Key),
                        quantityOrder = x.Sum(s => s.profit != null ? 1 : 0),
                        quantityProduct = x.Sum(s => s.profit != null ? s.profit.quantityProduct : 0),
                        costOfGoods = x.Sum(s => s.profit != null ? s.profit.costOfGoods : 0),
                        price = x.Sum(s => s.profit != null ? s.profit.price : 0),
                        discount = x.Sum(s => s.profit != null ? s.profit.discount : 0),
                        feeShipping = x.Sum(s => s.profit != null ? s.profit.feeShipping : 0),
                        quantityRefund = x.Sum(s => s.refund != null ? s.refund.quantityProduct : 0),
                        quantityProductRefund = x.Sum(s => s.refund != null ?  s.refund.quantityProduct : 0),
                        refundCapital = x.Sum(s => s.refund != null ? s.refund.refundCapital : 0),
                        refundMoney = x.Sum(s => s.refund != null ? s.refund.refundMoney : 0),
                        refundFee = x.Sum(s => s.refund != null ? s.refund.refundFee : 0)
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