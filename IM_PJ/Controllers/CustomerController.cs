using IM_PJ.Models;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebUI.Business;
using System.Text.RegularExpressions;
using System.Data.Entity.Validation;

namespace IM_PJ.Controllers
{
    public class CustomerController
    {
        #region CRUD
        public static string Insert(string CustomerName, string CustomerPhone, string CustomerAddress, string CustomerEmail, int CustomerLevelID, int Status,
            DateTime CreatedDate, string CreatedBy, bool IsHidden, string Zalo, string Facebook, string Note, string Province, string Nick, string Avatar = "", int ShippingType = 0, int PaymentType = 0, int TransportCompanyID = 0, int TransportCompanySubID = 0, string CustomerPhone2 = "")
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Customer ui = new tbl_Customer();
                ui.CustomerName = CustomerName;
                ui.UnSignedName = UnSign.convert(CustomerName);
                ui.CustomerPhone = CustomerPhone;
                ui.CustomerAddress = CustomerAddress;
                if (!string.IsNullOrEmpty(CustomerEmail))
                    ui.CustomerEmail = CustomerEmail;
                ui.CustomerLevelID = CustomerLevelID;
                ui.Status = Status;
                ui.CreatedDate = CreatedDate;
                ui.CreatedBy = CreatedBy;
                ui.IsHidden = IsHidden;

                if (!string.IsNullOrEmpty(Zalo))
                    ui.Zalo = Zalo;
                if (!string.IsNullOrEmpty(Facebook))
                    ui.Facebook = Facebook;
                if (!string.IsNullOrEmpty(Note))
                    ui.Note = Note;
                if (!string.IsNullOrEmpty(Province))
                    ui.ProvinceID = Province.ToInt();

                ui.Nick = Nick;
                ui.UnSignedNick = UnSign.convert(Nick);

                if (!string.IsNullOrEmpty(Avatar))
                    ui.Avatar = Avatar;

                ui.ShippingType = ShippingType;
                ui.PaymentType = PaymentType;
                ui.TransportCompanyID = TransportCompanyID;
                ui.TransportCompanySubID = TransportCompanySubID;
                ui.CustomerPhone2 = CustomerPhone2;
                try
                {
                    dbe.tbl_Customer.Add(ui);
                    dbe.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                int kq = ui.ID;
                return kq.ToString();
            }
        }
        public static string Update(int ID, string CustomerName, string CustomerPhone, string CustomerAddress, string CustomerEmail, int CustomerLevelID, int Status,
           string CreatedBy, DateTime ModifiedDate, string ModifiedBy, bool IsHidden, string Zalo, string Facebook, string Note, string Province, string Nick, string Avatar, int ShippingType, int PaymentType, int TransportCompanyID, int TransportCompanySubID, string CustomerPhone2)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Customer ui = dbe.tbl_Customer.Where(a => a.ID == ID).SingleOrDefault();
                if (ui != null)
                {
                    ui.CustomerName = CustomerName;
                    ui.UnSignedName = UnSign.convert(CustomerName);
                    ui.CustomerPhone = CustomerPhone;
                    ui.CustomerAddress = CustomerAddress;
                    ui.CustomerEmail = CustomerEmail;
                    ui.CustomerLevelID = CustomerLevelID;
                    ui.Status = Status;
                    ui.CreatedBy = CreatedBy;
                    ui.ModifiedBy = ModifiedBy;
                    ui.ModifiedDate = ModifiedDate;
                    ui.IsHidden = IsHidden;
                    ui.Zalo = Zalo;
                    ui.Facebook = Facebook;
                    ui.Note = Note;
                    ui.Nick = Nick;
                    ui.UnSignedNick = UnSign.convert(Nick);
                    ui.Avatar = Avatar;
                    ui.ShippingType = ShippingType;
                    ui.PaymentType = PaymentType;
                    ui.TransportCompanyID = TransportCompanyID;
                    ui.TransportCompanySubID = TransportCompanySubID;
                    ui.CustomerPhone2 = CustomerPhone2;

                    if (!string.IsNullOrEmpty(Province))
                        ui.ProvinceID = Province.ToInt();
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        #endregion
        #region Select
        public static tbl_Customer GetByPhone(string CustomerPhone)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Customer ai = dbe.tbl_Customer.Where(a => a.CustomerPhone == CustomerPhone || a.CustomerPhone2 == CustomerPhone || a.CustomerPhoneBackup == CustomerPhone).FirstOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }
        public static string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public static List<tbl_Customer> Find2(string text)
        {
            string text2 = convertToUnSign(text);
            using (var dbe = new inventorymanagementEntities())
            {
                var a = dbe.tbl_Customer.Where(delegate (tbl_Customer x)
                {
                    if (convertToUnSign(x.CustomerName).IndexOf(text2, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        return true;
                    else
                        return false;
                }).ToList();
                var i = dbe.tbl_Customer.Where(x => x.CustomerName.Contains(text2) || x.CustomerName.Contains(text) || x.Nick.Contains(text) || x.Nick.Contains(text2) || x.CustomerPhone.Contains(text) || x.Zalo.Contains(text)).ToList();
                var ai = a.Concat(i).ToList();
                if (ai.Count() > 0)
                    return ai;
                return null;
            }
        }
        public static List<CustomerOut> Find(string text, string createdby = "")
        {
            string textsearch = '"' + text + '"';
            var list = new List<CustomerOut>();
            var sql = @"select c.ID, c.CustomerName, c.Nick, c.CustomerPhone, c.CustomerPhone2, c.Zalo, c.Facebook, c.CustomerAddress, c.CreatedBy, c.ProvinceID as Province
                        from tbl_Customer c
                         WHERE (CONTAINS(c.CustomerName,'" + textsearch + "')  OR CONTAINS(c.Nick,'" + textsearch + "') OR c.CustomerPhone like '%" + text + "%' OR c.CustomerPhone2 like '%" + text + "%' OR c.CustomerPhoneBackup like '%" + text + "%' OR c.Facebook like '%" + text + "%' OR c.Zalo like '%" + text + "%')";
            if (createdby != "")
            {
                sql += " And c.CreatedBy = N'" + createdby + "'";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new CustomerOut();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["CustomerName"] != DBNull.Value)
                    entity.CustomerName = reader["CustomerName"].ToString().ToTitleCase();
                if (reader["CustomerPhone"] != DBNull.Value)
                    entity.CustomerPhone = reader["CustomerPhone"].ToString();
                if (reader["CustomerPhone2"] != DBNull.Value)
                    entity.CustomerPhone2 = reader["CustomerPhone2"].ToString();
                if (reader["CreatedBy"] != DBNull.Value)
                    entity.CreatedBy = reader["CreatedBy"].ToString();
                if (reader["CustomerAddress"] != DBNull.Value)
                    entity.CustomerAddress = reader["CustomerAddress"].ToString().ToTitleCase();
                if (reader["Zalo"] != DBNull.Value)
                    entity.Zalo = reader["Zalo"].ToString();
                if (reader["Facebook"] != DBNull.Value)
                    entity.Facebook = reader["Facebook"].ToString();
                if (reader["Nick"] != DBNull.Value)
                    entity.Nick = reader["Nick"].ToString().ToTitleCase();
                if (reader["Province"] != DBNull.Value)
                {
                    var provinceID = reader["Province"].ToString().ToInt();
                    var pro = ProvinceController.GetByID(provinceID);
                    if (pro != null)
                    {
                        entity.Province = pro.ProvinceName;
                    }
                    else
                    {
                        entity.Province = "";
                    }

                }

                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public static tbl_Customer GetByID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Customer ai = dbe.tbl_Customer.Where(a => a.ID == ID).FirstOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }
        public static List<tbl_Customer> GetAll(string s)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_Customer> ags = new List<tbl_Customer>();
                ags = dbe.tbl_Customer.Where(c => c.CustomerName.Contains(s) || c.CustomerPhone.Contains(s) || c.Zalo.Contains(s) || c.Facebook.Contains(s)).ToList();
                return ags;
            }
        }

        public static List<CustomerOut> Get(string text, int Discount, string by, int Provice)
        {
            string textsearch = '"' + text + '"';
            var list = new List<CustomerOut>();
            var sql = @"select c.ID, c.CustomerName, c.Nick, c.CustomerPhone, c.Zalo, c.Facebook, c.CreatedBy, c.CreatedDate, dg.DiscountName, dg.ID as DiscountID, c.ProvinceID as Province
                        from tbl_DiscountCustomer dc, tbl_DiscountGroup dg, tbl_Customer c
                        where dc.DiscountGroupID = dg.ID and c.ID = dc.UID";
            if (!string.IsNullOrEmpty(textsearch))
            {
                sql += " And (CONTAINS(c.CustomerName,'" + textsearch + "')  OR CONTAINS(c.Nick,'" + textsearch + "') OR c.CustomerPhone like '%" + text + "%' OR c.Facebook like '%" + text + "%' OR c.Zalo like '%" + text + "%')";
            }
            if (Discount > 0)
            {
                sql += " AND dg.ID  = " + Discount;
            }
            if (Provice > 0)
            {
                sql += " AND c.ProvinceID  = " + Provice;
            }
            if (!string.IsNullOrEmpty(by))
            {
                sql += " And c.CreatedBy = N'" + by + "'";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new CustomerOut();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["CustomerName"] != DBNull.Value)
                    entity.CustomerName = reader["CustomerName"].ToString();
                if (reader["Nick"] != DBNull.Value)
                    entity.Nick = reader["Nick"].ToString();
                if (reader["CustomerPhone"] != DBNull.Value)
                    entity.CustomerPhone = reader["CustomerPhone"].ToString();
                if (reader["Zalo"] != DBNull.Value)
                    entity.Zalo = reader["Zalo"].ToString();
                if (reader["Facebook"] != DBNull.Value)
                    entity.Facebook = reader["Facebook"].ToString();
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                if (reader["CreatedBy"] != DBNull.Value)
                    entity.CreatedBy = reader["CreatedBy"].ToString();
                if (reader["DiscountName"] != DBNull.Value)
                    entity.DiscountName = reader["DiscountName"].ToString();
                if (reader["Province"] != DBNull.Value)
                    entity.Province = reader["Province"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static List<CustomerOut> Filter(string text, string by, int Province, string CreatedDate, string Sort)
        {
            var result = new List<CustomerOut>();

            using (var dbe = new inventorymanagementEntities())
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
                    case "month":
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

                var customers = dbe.tbl_Customer
                    .Where(
                        x =>
                        string.IsNullOrEmpty(text) ||
                        (
                            !string.IsNullOrEmpty(text) &&
                            (
                                x.CustomerName.Contains(text) ||
                                x.Nick.Contains(text) ||
                                x.CustomerPhone.Contains(text) ||
                                x.CustomerPhone2.Contains(text) ||
                                x.CustomerPhoneBackup.Contains(text) ||
                                x.Facebook.Contains(text) ||
                                x.Zalo.Contains(text)
                            )
                        )
                    )
                    .Where(x => Province <= 0 || (Province > 0 && x.ProvinceID == Province))
                    .Where(x => string.IsNullOrEmpty(by) || (!string.IsNullOrEmpty(by) && x.CreatedBy == by))
                    .Where(x => string.IsNullOrEmpty(CreatedDate) || (!string.IsNullOrEmpty(CreatedDate) && x.CreatedDate >= fromdate && x.CreatedDate <= todate))
                    .OrderBy(x => x.ID);

                // Get customer of province
                result = customers
                    .GroupJoin(
                        dbe.tbl_Province,
                        cus => cus.ProvinceID,
                        pro => pro.ID,
                        (customer, province) => new { customer, province }
                    )
                    .SelectMany(
                        x => x.province.DefaultIfEmpty(),
                        (parent, child) => new CustomerOut()
                        {
                            ID = parent.customer.ID,
                            CustomerName = parent.customer.CustomerName,
                            CustomerPhone = parent.customer.CustomerPhone,
                            CustomerPhone2 = parent.customer.CustomerPhone2,
                            Zalo = parent.customer.Zalo,
                            Facebook = parent.customer.Facebook,
                            CreatedDate = parent.customer.CreatedDate.HasValue ? parent.customer.CreatedDate.Value : DateTime.Now,
                            CreatedBy = parent.customer.CreatedBy,
                            Nick = parent.customer.Nick,
                            Province = parent.customer.ProvinceID.HasValue ? parent.customer.ProvinceID.Value.ToString() : String.Empty,
                            Avatar = !string.IsNullOrEmpty(parent.customer.Avatar) ? parent.customer.Avatar : "/uploads/avatars/no-avatar.png",
                            ShippingType = parent.customer.ShippingType.HasValue ? parent.customer.ShippingType.Value : 0,
                            PaymentType = parent.customer.PaymentType.HasValue ? parent.customer.PaymentType.Value : 0,
                            TransportCompanyID = parent.customer.TransportCompanyID.HasValue ? parent.customer.TransportCompanyID.Value : 0,
                            TransportCompanySubID = parent.customer.TransportCompanySubID.HasValue ? parent.customer.TransportCompanySubID.Value : 0,
                            ProvinceName = child != null ? child.ProvinceName : String.Empty
                        }
                    )
                    .OrderBy(x => x.ID)
                    .ToList();

                // Get info order
                var orderInfo = dbe.tbl_Order
                    .Join(
                        customers,
                        order => order.CustomerID,
                        customer => customer.ID,
                        (order, customer) => order
                    )
                    .Join(
                        dbe.tbl_OrderDetail,
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
                            TotalOrder = g.Select(x => x.Order).Distinct().Count(),
                            TotalQuantity = g.Sum(x => x.Quantity)
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
                            parent.customer.TotalOrder = child != null ? child.TotalOrder : 0;
                            parent.customer.TotalQuantity = child != null ? child.TotalQuantity : 0;

                            return parent.customer;
                        }
                    )
                    .OrderBy(x => x.ID)
                    .ToList();

                // Get discount customer
                var discounts = dbe.tbl_DiscountCustomer
                    .Join(
                        customers,
                        discount => discount.UID,
                        customer => customer.ID,
                        (discount, customer) => discount
                    )
                    .Where(x => x.IsHidden == false)
                    .GroupJoin(
                        dbe.tbl_DiscountGroup,
                        cus => cus.DiscountGroupID,
                        grp => grp.ID,
                        (cus, grp) => new { cus, grp }
                    )
                    .SelectMany(
                        x => x.grp.DefaultIfEmpty(),
                        (parent, child) => new
                        {
                            CustomerID = parent.cus.UID,
                            DiscountGroupID = parent.cus.DiscountGroupID.HasValue ? parent.cus.DiscountGroupID.Value.ToString() : String.Empty,
                            DiscountName = child != null ? child.DiscountName : String.Empty
                        }
                    )
                    .AsEnumerable()
                    .GroupBy(x => x.CustomerID)
                    .Select(g => new
                        {
                            CustomerID = g.Key.Value,
                            DiscountGroupID = string.Join("|", g.Select(i => i.DiscountGroupID)),
                            DiscountName = string.Join("|", g.Select(i => i.DiscountName))
                        }
                    )
                    .OrderBy(x => x.CustomerID)
                    .ToList();

                result.GroupJoin(
                        discounts,
                        cus => cus.ID,
                        dis => dis.CustomerID,
                        (customer, discount) => new { customer, discount }
                    )
                    .SelectMany(
                        x => x.discount.DefaultIfEmpty(),
                        (parent, child) =>
                        {
                            parent.customer.DisID = child != null ? child.DiscountGroupID : String.Empty;
                            parent.customer.DiscountName = child != null ? child.DiscountName : String.Empty;

                            return parent.customer;
                        }
                    )
                    .ToList();
            }

            result = result.OrderByDescending(x => x.ID).ToList();

            if (!string.IsNullOrEmpty(Sort))
            {
                switch (Sort)
                {
                    case "latest":
                        result = result.OrderByDescending(x => x.CreatedDate).ToList();
                        break;
                    case "oldest":
                        result = result.OrderBy(x => x.CreatedDate).ToList();
                        break;
                    case "orderdesc":
                        result = result.OrderByDescending(x => x.TotalOrder).ToList();
                        break;
                    case "orderasc":
                        result = result.OrderBy(x => x.TotalOrder).ToList();
                        break;
                    case "itemdesc":
                        result = result.OrderByDescending(x => x.TotalQuantity).ToList();
                        break;
                    case "itemasc":
                        result = result.OrderBy(x => x.TotalQuantity).ToList();
                        break;
                }
            }

            return result;
        }

        public static List<CustomerGet> GetNotInGroupByGroupID(int GroupID)
        {
            var list = new List<CustomerGet>();
            var sql = @"SELECT  l.ID, l.CustomerName, l.CustomerPhone, l.CreatedBy FROM tbl_Customer l";
            sql += " LEFT JOIN (Select UID, CustomerName from tbl_DiscountCustomer where DiscountGroupID = " + GroupID + " ) as r ON  r.UID = l.ID";
            sql += " WHERE r.UID IS NULL";

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new CustomerGet();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["CustomerName"] != DBNull.Value)
                    entity.CustomerName = reader["CustomerName"].ToString();
                if (reader["CustomerPhone"] != DBNull.Value)
                    entity.CustomerPhone = reader["CustomerPhone"].ToString();
                if (reader["CreatedBy"] != DBNull.Value)
                    entity.CreatedBy = reader["CreatedBy"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public static List<tbl_Customer> GetInGroupByGroupID(int GroupID)
        {
            var list = new List<tbl_Customer>();
            var sql = @"SELECT  l.ID, l.CustomerName, l.CustomerPhone, l.Nick, l.CreatedBy, r.CreatedDate as CreatedDate FROM tbl_Customer l";
            sql += " LEFT JOIN (Select UID, CustomerName, CreatedDate from tbl_DiscountCustomer where DiscountGroupID = " + GroupID + " ) as r ON  r.UID = l.ID";
            sql += " WHERE r.UID IS NOT NULL";

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new tbl_Customer();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["Nick"] != DBNull.Value)
                    entity.Nick = reader["Nick"].ToString();
                if (reader["CustomerName"] != DBNull.Value)
                    entity.CustomerName = reader["CustomerName"].ToString();
                if (reader["CustomerPhone"] != DBNull.Value)
                    entity.CustomerPhone = reader["CustomerPhone"].ToString();
                if (reader["CreatedBy"] != DBNull.Value)
                    entity.CreatedBy = reader["CreatedBy"].ToString();
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public static List<tbl_Customer> GetBylevelID(int LevelID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_Customer> ags = new List<tbl_Customer>();
                ags = dbe.tbl_Customer.Where(a => a.CustomerLevelID == LevelID).ToList();
                return ags;
            }
        }

        public static List<tbl_Customer> TotalCustomer(string fromdate, string todate)
        {
            using (var db = new inventorymanagementEntities())
            {
                List<tbl_Customer> or = new List<tbl_Customer>();
                if (!string.IsNullOrEmpty(fromdate))
                {
                    if (!string.IsNullOrEmpty(todate))
                    {
                        DateTime fd = Convert.ToDateTime(fromdate);
                        DateTime td = Convert.ToDateTime(todate);
                        or = db.tbl_Customer
                            .Where(r => r.CreatedDate >= fd && r.CreatedDate <= td)
                            .OrderByDescending(r => r.ID).ToList();
                    }
                    else
                    {
                        DateTime fd = Convert.ToDateTime(fromdate);
                        or = db.tbl_Customer
                            .Where(r => r.CreatedDate >= fd)
                            .OrderByDescending(r => r.ID).ToList();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(todate))
                    {
                        DateTime td = Convert.ToDateTime(todate);
                        or = db.tbl_Customer
                            .Where(r => r.CreatedDate <= td)
                            .OrderByDescending(r => r.ID).ToList();
                    }
                    else
                    {
                        or = db.tbl_Customer.ToList();
                    }
                }
                return or;
            }
        }
        #endregion
        #region Class
        public class CustomerGet
        {
            public int ID { get; set; }
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public string CreatedBy { get; set; }
        }

        public class CustomerOut
        {
            public int ID { get; set; }
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public string CustomerAddress { get; set; }
            public string Zalo { get; set; }
            public string Facebook { get; set; }
            public string CreatedBy { get; set; }
            public string DiscountName { get; set; }
            public DateTime CreatedDate { get; set; }
            public string IsHidden { get; set; }
            public string Province { get; set; }

            public string DisID { get; set; }
            public string Nick { get; set; }
            public string Avatar { get; set; }
            public int ShippingType { get; set; }
            public int PaymentType { get; set; }
            public int TransportCompanyID { get; set; }
            public int TransportCompanySubID { get; set; }
            public string CustomerPhone2 { get; set; }

            public int TotalOrder { get; set; }
            public double TotalQuantity { get; set; }
            public string ProvinceName { get; set; }
        }
        #endregion
    }
}