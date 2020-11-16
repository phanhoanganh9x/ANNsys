using IM_PJ.Models;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using WebUI.Business;

namespace IM_PJ.Controllers
{
    public class UserController
    {
        #region Select
        public static User getByPhone(string Phone)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                User user = dbe.Users.Where(a => a.Phone == Phone).FirstOrDefault();
                if (user != null)
                    return user;
                else
                    return null;
            }
        }
        public static List<AppUserOut> Filter(string TextSearch, string Gender, string City, int Status, string CreatedDate)
        {
            var result = new List<AppUserOut>();

            using (var con = new inventorymanagementEntities())
            {
                DateTime fromdate = DateTime.Today;
                DateTime todate = DateTime.Now;

                switch (CreatedDate)
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
                    case "7days":
                        fromdate = DateTime.Today.AddDays(-6);
                        todate = DateTime.Now;
                        break;
                    case "30days":
                        fromdate = DateTime.Today.AddDays(-29);
                        todate = DateTime.Now;
                        break;
                }

                var users = con.Users
                    .Where(x =>
                        string.IsNullOrEmpty(TextSearch) ||
                        (
                            !string.IsNullOrEmpty(TextSearch) &&
                            (
                                x.FullName.Contains(TextSearch) ||
                                x.Address.Contains(TextSearch) ||
                                x.Phone == TextSearch
                            )
                        )
                    )
                    .Where(x => string.IsNullOrEmpty(Gender) || (!string.IsNullOrEmpty(Gender) && x.Gender == Gender))
                    .Where(x => string.IsNullOrEmpty(City) || (!string.IsNullOrEmpty(City) && x.City == City))
                    .Where(x => string.IsNullOrEmpty(CreatedDate) || (!string.IsNullOrEmpty(CreatedDate) && x.CreatedDate >= fromdate && x.CreatedDate <= todate));



                // Kiểm tra user đã trở thành khách hàng thân thân thiết chưa
                result = users
                    .GroupJoin(
                        con.tbl_Customer,
                        reg => reg.Phone,
                        cus => cus.CustomerPhone,
                        (reg, cus) => new { reg, cus }
                    )
                    .SelectMany(
                        x => x.cus.DefaultIfEmpty(),
                        (parent, child) => new AppUserOut()
                        {
                            ID = parent.reg.ID,
                            Phone = parent.reg.Phone,
                            FullName = parent.reg.FullName,
                            Gender = parent.reg.Gender,
                            Birthday = parent.reg.BirthDay,
                            Address = parent.reg.Address,
                            City = parent.reg.City,
                            Status = child != null ? 3 : parent.reg.ViewStatus.Value,
                            CreatedDate = parent.reg.CreatedDate,
                        }
                    )
                    .ToList();

                if (Status > 0)
                {
                    result = result.Where(x => x.Status == Status).ToList();
                }
            }

            return result.OrderByDescending(x => x.ID).ToList();
        }
        public static int UpdateStatus(int id, int status)
        {
            using (var connect = new inventorymanagementEntities())
            {
                User target = connect.Users
                    .Where(x => x.ID == id)
                    .SingleOrDefault();

                if (target != null)
                {
                    target.ViewStatus = status;

                    connect.SaveChanges();
                }

                return target.ID;
            }

        }
        public class AppUserOut
        {
            public int ID { get; set; }
            public string Phone { get; set; }
            public string FullName { get; set; }
            public string Gender { get; set; }
            public DateTime? Birthday { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public int Status { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        public static bool checkExists(int userID)
        {
            using (var con = new inventorymanagementEntities())
            {
                var customer = con.tbl_Customer.Where(x => x.ID == userID).SingleOrDefault();

                if (customer == null)
                    return false;

                var exists = con.Users.Where(x =>
                    (x.Phone == customer.CustomerPhone) ||
                    (x.Phone == customer.CustomerPhone2)
                ).Any();

                return exists;
            }
        }
        #endregion

    }
}