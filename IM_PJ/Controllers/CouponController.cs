using IM_PJ.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace IM_PJ.Controllers
{
    public class CouponController
    {
        public static string getCoupon(int customerID, string code, int productNumber, decimal price)
        {
            using (var con = new inventorymanagementEntities())
            {
                var now = DateTime.Now;
                var coupon = con.Coupons
                    .Where(x => x.Code.Trim().ToUpper() == code.Trim().ToUpper())
                    .OrderByDescending(x => x.Value)
                    .FirstOrDefault();

                if (coupon != null)
                {
                    if (!(now.Date >= coupon.CreatedDate.Date && now.Date <= coupon.EndDate.Date))
                        return JsonConvert.SerializeObject(new
                        {
                            status = false,
                            message = String.Format("Mã {0} hết hiệu lực ({1:dd/MM/yyyy} - {2:dd/MM/yyyy})", code, coupon.CreatedDate, coupon.EndDate),
                            couponID = 0,
                            value = 0,
                            productNumber = 0,
                            priceMin = 0
                        });

                    if (!coupon.Active)
                        return JsonConvert.SerializeObject(new {
                            status = false,
                            message = String.Format("Mã {0} hết hiệu lực", code),
                            couponID = 0,
                            value = 0,
                            productNumber = 0,
                            priceMin = 0
                        });

                    if (!(productNumber >= coupon.ProductNumber && price >= coupon.PriceMin))
                        return JsonConvert.SerializeObject(new
                        {
                            status = false,
                            message = String.Format("Mã {0} chỉ áp dụng với đơn hàng số lượng từ {1:N0} và trị giá từ {2:N0}", code, coupon.ProductNumber, coupon.PriceMin),
                            couponID = 0,
                            value = 0,
                            productNumber = 0,
                            priceMin = 0
                        });

                    var user = CustomerController.GetByID(customerID);

                    var customerCouponList = con.CustomerCoupons
                        .Where(x => x.CustomerID == customerID || x.Phone == user.CustomerPhone)
                        .Where(x => x.CouponID == coupon.ID)
                        .ToList();

                    if (customerCouponList.Count == 0)
                    {
                        var textInfo = new CultureInfo("vi-VN", false).TextInfo;

                        return JsonConvert.SerializeObject(new
                        {
                            status = false,
                            message = String.Format("Mã {0} không áp dụng cho khách hàng {1}", code, textInfo.ToTitleCase(user.CustomerName)),
                            couponID = 0,
                            value = 0,
                            productNumber = 0,
                            priceMin = 0
                        });
                    }

                    var customerCouponActives = customerCouponList.Where(x => x.Active == true).ToList();

                    if (customerCouponActives.Count == 0)
                    {
                        var textInfo = new CultureInfo("vi-VN", false).TextInfo;

                        if (customerCouponList.Count > 1)
                            return JsonConvert.SerializeObject(new
                            {
                                status = false,
                                message = String.Format("Mã {0} đã được khách hàng {1} sử dụng {2} lần", code, textInfo.ToTitleCase(user.CustomerName), customerCouponList.Count),
                                couponID = 0,
                                value = 0,
                                productNumber = 0,
                                priceMin = 0
                            });
                        else
                            return JsonConvert.SerializeObject(new
                            {
                                status = false,
                                message = String.Format("Khách hàng {0} đã sử dụng mã {1}", textInfo.ToTitleCase(user.CustomerName), code),
                                couponID = 0,
                                value = 0,
                                productNumber = 0,
                                priceMin = 0
                            });
                    }
                    else
                    {
                        var customerCoupon = customerCouponActives.OrderByDescending(o => o.ID).FirstOrDefault();

                        if (!(now.Date >= customerCoupon.StartDate.Date && now.Date <= customerCoupon.EndDate.Date))
                        {
                            var textInfo = new CultureInfo("vi-VN", false).TextInfo;

                            return JsonConvert.SerializeObject(new
                            {
                                status = false,
                                message = String.Format("Khách hàng {0} sử dụng mã {1} không đúng thời gian ({2:dd/MM/yyyy} - {3:dd/MM/yyyy})", textInfo.ToTitleCase(user.CustomerName), code, customerCoupon.StartDate, customerCoupon.EndDate),
                                couponID = 0,
                                value = 0,
                                productNumber = 0,
                                priceMin = 0
                            });
                        }
                    }

                    return JsonConvert.SerializeObject(new
                    {
                        status = true,
                        message = String.Empty,
                        couponID = coupon.ID,
                        value = coupon.Value,
                        productNumber = coupon.ProductNumber,
                        priceMin = coupon.PriceMin
                    });
                }

                return JsonConvert.SerializeObject(new {
                    status = false,
                    message = String.Format("Mã {0} không tồn tại", code),
                    couponID = 0,
                    value = 0,
                    productNumber = 0,
                    priceMin = 0
                });
            }
        }

        public static string checkCouponForCustomer(int customerID, string code)
        {
            using (var con = new inventorymanagementEntities())
            {
                var now = DateTime.Now;
                var coupon = con.Coupons
                    .Where(x => x.Code.Trim().ToUpper() == code.Trim().ToUpper())
                    .OrderByDescending(x => x.Value)
                    .FirstOrDefault();

                if (coupon != null)
                {
                    if (!(now.Date >= coupon.CreatedDate.Date && now.Date <= coupon.EndDate.Date))
                        return String.Format("Mã {0} hết hiệu lực ({1:dd/MM/yyyy} - {2:dd/MM/yyyy})", code, coupon.CreatedDate, coupon.EndDate);

                    if (!coupon.Active)
                        return String.Format("Mã {0} hết hiệu lực", code);

                    var user = CustomerController.GetByID(customerID);

                    var customerCouponList = con.CustomerCoupons
                        .Where(x => x.CustomerID == customerID || x.Phone == user.CustomerPhone)
                        .Where(x => x.CouponID == coupon.ID)
                        .ToList();

                    if (customerCouponList.Count == 0)
                    {
                        var textInfo = new CultureInfo("vi-VN", false).TextInfo;

                        return String.Format("Mã {0} chưa tạo cho khách hàng {1}", code, textInfo.ToTitleCase(user.CustomerName));
                    }

                    var customerCouponActives = customerCouponList.Where(x => x.Active == true).ToList();

                    if (customerCouponActives.Count == 0)
                    {
                        var textInfo = new CultureInfo("vi-VN", false).TextInfo;

                        if (customerCouponList.Count > 1)
                            return String.Format("Mã {0} đã được khách hàng {1} sử dụng {2} lần", code, textInfo.ToTitleCase(user.CustomerName), customerCouponList.Count);
                        else
                            return String.Format("Khách hàng {0} đã sử dụng mã {1}", textInfo.ToTitleCase(user.CustomerName), code);
                    }
                    else
                    {
                        var customerCoupon = customerCouponActives.OrderByDescending(o => o.ID).FirstOrDefault();

                        if (!(now.Date >= customerCoupon.StartDate.Date && now.Date <= customerCoupon.EndDate.Date))
                        {
                            var textInfo = new CultureInfo("vi-VN", false).TextInfo;

                            return String.Format("Khách hàng {0} sử dụng mã {1} không đúng thời gian ({2:dd/MM/yyyy} - {3:dd/MM/yyyy})", textInfo.ToTitleCase(user.CustomerName), code, customerCoupon.StartDate, customerCoupon.EndDate);
                        }
                    }

                    return String.Format("Khách hàng đủ điều kiện sử dụng mã {0}", code);
                }

                return String.Format("Mã {0} không tồn tại", code);
            }
        }

        public static Coupon getCoupon(int id)
        {
            using (var con = new inventorymanagementEntities())
            {
                return con.Coupons.Where(x => x.ID == id).FirstOrDefault();
            }
        }
        public static List<CustomerCoupon> getCouponByCustomer(int couponID, int customerID, string customerPhone)
        {
            using (var con = new inventorymanagementEntities())
            {
                return con.CustomerCoupons.Where(x => x.CouponID == couponID && (x.CustomerID == customerID || x.Phone == customerPhone)).ToList();
            }
        }
        public static Coupon getByName(string couponCode)
        {
            using (var con = new inventorymanagementEntities())
            {
                return con.Coupons.Where(x => x.Code == couponCode).FirstOrDefault();
            }
        }

        public static CustomerCoupon updateStatusCouponCustomer(int customerID, int couponID, bool active)
        {
            using (var con = new inventorymanagementEntities())
            {
                var customer = CustomerController.GetByID(customerID);

                var customerCoupon = con.CustomerCoupons
                    .Where(x => x.CustomerID == customerID || x.Phone == customer.CustomerPhone)
                    .Where(x => x.CouponID == couponID)
                    .Where(x => x.Active == !active)
                    .FirstOrDefault();

                if (customerCoupon == null)
                    return null;

                customerCoupon.Active = active;
                con.SaveChanges();

                return customerCoupon;
            }
        }

        public static CustomerCoupon insertCustomerCoupon(int customerID, int couponID)
        {
            using (var con = new inventorymanagementEntities())
            {
                var coupon = CouponController.getCoupon(couponID);
                if (coupon != null)
                {
                    var cc = new CustomerCoupon()
                    {
                        CouponID = couponID,
                        CustomerID = customerID,
                        StartDate = coupon.StartDate,
                        EndDate = coupon.EndDate,
                        Active = coupon.Active,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Phone = "",
                    };
                    con.CustomerCoupons.Add(cc);
                    con.SaveChanges();

                    return cc;
                }
                return null;
            }
        }
    }
}