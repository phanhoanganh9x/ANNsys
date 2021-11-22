using IM_PJ.Models;
using MB.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebUI.Business;
using System.Text.RegularExpressions;
using System.Data.Entity.Validation;
using System.Data.Entity;
using IM_PJ.Models.Common;
using System.Text;

namespace IM_PJ.Controllers
{
    public class CustomerController
    {
        #region CRUD
        public static string Insert(string CustomerName, string CustomerPhone, string CustomerAddress, string CustomerEmail, int CustomerLevelID, int Status,
            DateTime CreatedDate, string CreatedBy, bool IsHidden, string Zalo, string Facebook, string Note, string Nick, string Avatar = "", int ShippingType = 0, int PaymentType = 0, int TransportCompanyID = 0, int TransportCompanySubID = 0, string CustomerPhone2 = "", int ProvinceID = 0, int DistrictID = 0, int WardID = 0)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Customer ui = new tbl_Customer();
                ui.CustomerName = CustomerName;
                ui.UnSignedName = UnSign.convert(CustomerName);
                ui.CustomerPhone = CustomerPhone;
                ui.CustomerAddress = CustomerAddress;
                ui.CustomerEmail = CustomerEmail;
                ui.CustomerLevelID = CustomerLevelID;
                ui.Status = Status;
                ui.CreatedDate = CreatedDate;
                ui.CreatedBy = CreatedBy;
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
                ui.ProvinceID = ProvinceID;
                ui.DistrictId = DistrictID;
                ui.WardId = WardID;

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
           string CreatedBy, DateTime ModifiedDate, string ModifiedBy, bool IsHidden, string Zalo, string Facebook, string Note, string Nick, string Avatar, int ShippingType, int PaymentType, int TransportCompanyID, int TransportCompanySubID, string CustomerPhone2, int ProvinceID, int DistrictID, int WardID)
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
                    ui.ProvinceID = ProvinceID;
                    ui.DistrictId = DistrictID;
                    ui.WardId = WardID;

                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        public static string UpdateSendSMSIntroApp(string customerPhone)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Customer ui = dbe.tbl_Customer.Where(a => a.CustomerPhone == customerPhone).FirstOrDefault();
                if (ui != null)
                {
                    ui.SendSMSIntroApp = 1;

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

        /// <summary>
        /// Tìm thông tin khách hàng
        /// </summary>
        /// <param name="text">Chuỗi tìm kiếm</param>
        /// <param name="createdby">Nhân viên</param>
        /// <returns></returns>
        public static List<CustomerOut> Find(string text, string createdby = "")
        {
            #region Khởi tạo SQL
            var sql = new StringBuilder();

            sql.AppendLine("SELECT");
            sql.AppendLine("    C.ID");
            sql.AppendLine(",   C.CustomerName");
            sql.AppendLine(",   C.Nick");
            sql.AppendLine(",   C.CustomerPhone");
            sql.AppendLine(",   C.CustomerPhone2");
            sql.AppendLine(",   C.Zalo");
            sql.AppendLine(",   C.Facebook");
            sql.AppendLine(",   C.CustomerAddress");
            sql.AppendLine(",   C.ProvinceID as Province");
            sql.AppendLine(",   C.CreatedBy");
            sql.AppendLine("FROM");
            sql.AppendLine("    tbl_Customer AS C");
            sql.AppendLine("WHERE");
            sql.AppendLine("    1 = 1");

            // Lọc theo nhân viên quản lý
            createdby = !String.IsNullOrEmpty(createdby) ? createdby.Trim().ToUpper() : String.Empty;

            if (!String.IsNullOrEmpty(createdby))
                sql.AppendLine(String.Format("    And UPPER(C.CreatedBy) =  = N'{0}'", createdby));

            // Lọc theo từ tìm kiếm
            text = !String.IsNullOrEmpty(text) ? text.Trim().ToUpper() : String.Empty;

            if (!String.IsNullOrEmpty(text))
            {
                var numberReg = new Regex(@"^\d+$");

                if (numberReg.IsMatch(text))
                {
                    sql.AppendLine("    And (");
                    sql.AppendLine(String.Format("        C.CustomerPhone LIKE '%{0}%'", text));
                    sql.AppendLine(String.Format("        OR C.CustomerPhone2 LIKE '%{0}%'", text));
                    sql.AppendLine(String.Format("        OR C.CustomerPhoneBackup LIKE '%{0}%'", text));
                    sql.AppendLine(String.Format("        OR C.Zalo LIKE '%{0}%'", text));
                    sql.AppendLine("    )");
                }
                else {
                    sql.AppendLine("    And (");
                    sql.AppendLine(String.Format("        UPPER(C.CustomerName) LIKE N'%{0}%'", text));
                    sql.AppendLine(String.Format("        OR UPPER(C.Nick) LIKE N'%{0}%'", text));
                    sql.AppendLine("    )");
                }
            }

            sql.AppendLine("ORDER BY");
            sql.AppendLine("    C.ID DESC");
            #endregion

            #region Thực thi SQL
            var customers = new List<CustomerOut>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql.ToString());

            while (reader.Read())
            {
                var item = new CustomerOut();

                // ID khách hàng
                if (reader["ID"] != DBNull.Value)
                    item.ID = reader["ID"].ToString().ToInt(0);
                // Tên khách hàng
                if (reader["CustomerName"] != DBNull.Value)
                    item.CustomerName = reader["CustomerName"].ToString().ToTitleCase();
                // SĐT
                if (reader["CustomerPhone"] != DBNull.Value)
                    item.CustomerPhone = reader["CustomerPhone"].ToString();
                // SĐT 2
                if (reader["CustomerPhone2"] != DBNull.Value)
                    item.CustomerPhone2 = reader["CustomerPhone2"].ToString();
                // Nhân viên khởi tạo
                if (reader["CreatedBy"] != DBNull.Value)
                    item.CreatedBy = reader["CreatedBy"].ToString();
                // Địa chỉ khách hàng
                if (reader["CustomerAddress"] != DBNull.Value)
                    item.CustomerAddress = reader["CustomerAddress"].ToString().ToTitleCase();
                // Zalo
                if (reader["Zalo"] != DBNull.Value)
                    item.Zalo = reader["Zalo"].ToString();
                // Facebook
                if (reader["Facebook"] != DBNull.Value)
                    item.Facebook = reader["Facebook"].ToString();
                // Nick
                if (reader["Nick"] != DBNull.Value)
                    item.Nick = reader["Nick"].ToString().ToTitleCase();
                // ID tỉnh / thành phố
                if (reader["Province"] != DBNull.Value)
                {
                    var provinceID = reader["Province"].ToString().ToInt();
                    var province = ProvinceController.GetByID(provinceID);

                    item.Province = province != null ? province.Name : String.Empty;
                }

                customers.Add(item);
            }

            reader.Close();
            #endregion

            return customers;
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

        public static List<CustomerOut> Filter(string text, string by, int Province, string Sort, DateTime FromDate, DateTime ToDate)
        {
            var result = new List<CustomerOut>();

            using (var dbe = new inventorymanagementEntities())
            {
                var customers = dbe.tbl_Customer
                    .Where(
                        x =>
                        string.IsNullOrEmpty(text) ||
                        (
                            !string.IsNullOrEmpty(text) &&
                            (
                                x.CustomerName.Contains(text) ||
                                x.Nick.Contains(text) ||
                                x.UnSignedName.Contains(text) ||
                                x.UnSignedNick.Contains(text) ||
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
                    .Where(x => (string.IsNullOrEmpty(FromDate.ToString()) && string.IsNullOrEmpty(ToDate.ToString())) || (!string.IsNullOrEmpty(FromDate.ToString()) && !string.IsNullOrEmpty(ToDate.ToString()) && x.CreatedDate >= FromDate && x.CreatedDate <= ToDate))
                    .OrderBy(x => x.ID);

                // Get customer of province
                result = customers
                    .GroupJoin(
                        dbe.DeliverySaveAddresses,
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
                            CustomerAddress = parent.customer.CustomerAddress,
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
                            ProvinceName = child != null ? child.Name : String.Empty,
                            SendSMSIntroApp = parent.customer.SendSMSIntroApp.HasValue ? parent.customer.SendSMSIntroApp.Value : 0
                        }
                    )
                    .OrderBy(x => x.ID)
                    .ToList();

                // Get info order
                var orderInfo = dbe.tbl_Order.Where(x => x.ExcuteStatus == 2)
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

                result.GroupJoin(
                        dbe.Users,
                        customer => customer.CustomerPhone,
                        user => user.Phone,
                        (customer, user) => new { customer, user }
                    )
                    .SelectMany(
                        x => x.user.DefaultIfEmpty(),
                        (parent, child) =>
                        {
                            parent.customer.InApp = child != null ? true : false;

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
        public static List<tbl_Customer> Report(string CreatedBy, DateTime fromDate, DateTime toDate)
        {
            var result = new List<tbl_Customer>();

            using (var dbe = new inventorymanagementEntities())
            {
                result = dbe.tbl_Customer
                    .Where(x => string.IsNullOrEmpty(CreatedBy) || (!string.IsNullOrEmpty(CreatedBy) && x.CreatedBy == CreatedBy))
                    .Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate)
                    .OrderBy(x => x.CreatedDate)
                    .ToList();
            }
            return result;
        }
        public static List<CustomerModel> GetPotentialCustomers(int discountGroupID, string staffName = "")
        {
            using (var con = new inventorymanagementEntities())
            {
                #region Lọc ra các khách hàng tìm năng do nhân viên đó khởi tạo
                var customerInGroup = con.tbl_Customer
                    .Join(
                        con.tbl_DiscountCustomer.Where(x => x.DiscountGroupID.Value == discountGroupID),
                        c => c.ID,
                        dc => dc.UID,
                        (c, dc) => c
                    );
                var potentialCustomer = con.tbl_Customer
                    .Except(customerInGroup)
                    .OrderByDescending(o => o.ID)
                    .ToList();

                if (!String.IsNullOrEmpty(staffName) && staffName != "admin")
                {
                    potentialCustomer = potentialCustomer.Where(x => x.CreatedBy == staffName).ToList();
                }
                #endregion

                if (!String.IsNullOrEmpty(staffName) && staffName != "admin")
                {
                    #region Lấy ra những đơn hàng mà khách hạng đạt tiêu chuẩn để join vô group
                    var quanlityOrder = OrderController.getOrderQualifiedOfDiscountGroup(discountGroupID);
                    if (quanlityOrder == null)
                        return null;

                    var customerOrdered = quanlityOrder
                        .GroupBy(g => g.CustomerID)
                        .Select(x => new { customerID = x.Key })
                        .OrderByDescending(o => o.customerID)
                        .ToList();
                    #endregion

                    potentialCustomer = potentialCustomer
                        .Join(
                            customerOrdered,
                            pc => pc.ID,
                            co => co.customerID,
                            (pc, co) => pc
                        )
                        .ToList();
                }

                var discount = con.tbl_DiscountCustomer
                    .Join(
                        con.tbl_DiscountGroup,
                        dc => dc.DiscountGroupID,
                        dg => dg.ID,
                        (dc, dg) => new
                        {
                            customerID = dc.UID,
                            discountGroupID = dg.ID,
                            discountGroupName = dg.DiscountName
                        }
                    )
                    .ToList();

                var data = potentialCustomer
                    .GroupJoin(
                        discount,
                        c => c.ID,
                        d => d.customerID,
                        (c, d) => new { customer = c, discount = d}
                    )
                    .SelectMany(
                        x => x.discount.DefaultIfEmpty(),
                        (parent, child) => new CustomerModel()
                        {
                            ID = parent.customer.ID,
                            FullName = parent.customer.CustomerName,
                            Nick = parent.customer.Nick,
                            Phone = !String.IsNullOrEmpty(parent.customer.CustomerPhone) ?
                                parent.customer.CustomerPhone :
                                !String.IsNullOrEmpty(parent.customer.CustomerPhone2) ? parent.customer.CustomerPhone2 : parent.customer.CustomerPhoneBackup,
                            Address = parent.customer.CustomerAddress,
                            Zalo = parent.customer.Zalo,
                            Facebook = parent.customer.Facebook,
                            StaffName = parent.customer.CreatedBy,
                            DiscountGroup = child != null ?
                                new DiscountGroupModel() { ID = child.discountGroupID, Name = child.discountGroupName } : null
                        }
                    )
                    .OrderByDescending(o => o.ID)
                    .ToList();

                return data;
            }
        }
        public static List<CustomerModel> getByDiscountGroupID(int discountGroupID, string CreatedBy)
        {
            using (var con = new inventorymanagementEntities())
            {
                #region Lọc ra khách hàng trong group
                var customer = con.tbl_Customer.Where(x => 1 == 1);
                if (!string.IsNullOrEmpty(CreatedBy))
                {
                    customer = customer.Where(x => x.CreatedBy == CreatedBy);
                }
                var customerInGroup = customer
                    .Join(
                        con.tbl_DiscountCustomer.Where(x => x.DiscountGroupID.Value == discountGroupID),
                        c => c.ID,
                        dc => dc.UID,
                        (c, dc) => c
                    );
                #endregion

                #region Lấy thông tin nhóm chiết khấu
                var discount = con.tbl_DiscountGroup.Where(x => x.ID == discountGroupID);
                #endregion

                var data = con.tbl_DiscountCustomer
                    .Where(x => x.DiscountGroupID.Value == discountGroupID)
                    .Join(
                        customerInGroup,
                        dc => dc.UID,
                        c => c.ID,
                        (dc, c) => new { customer = c, disCustomer = dc }
                    )
                    .Join(
                        discount,
                        tem => tem.disCustomer.DiscountGroupID.Value,
                        d => d.ID,
                        (tem, d) => new { customer = tem.customer, discount = d, disCustomer = tem.disCustomer }
                    )
                    .Select(x => new CustomerModel()
                    {
                        ID = x.customer.ID,
                        FullName = x.customer.CustomerName,
                        Nick = x.customer.Nick,
                        Phone = !String.IsNullOrEmpty(x.customer.CustomerPhone) ?
                                x.customer.CustomerPhone :
                                !String.IsNullOrEmpty(x.customer.CustomerPhone2) ? x.customer.CustomerPhone2 : x.customer.CustomerPhoneBackup,
                        Address = x.customer.CustomerAddress,
                        Zalo = x.customer.Zalo,
                        Facebook = x.customer.Facebook,
                        StaffName = x.customer.CreatedBy,
                        DiscountGroup = new DiscountGroupModel()
                        {
                            ID = x.discount.ID,
                            Name = x.discount.DiscountName,
                            DateJoined = x.disCustomer.CreatedDate.Value,
                            StaffName = x.disCustomer.CreatedBy,
                            VerifyOrder = x.disCustomer.VerifyOrder.HasValue ? x.disCustomer.VerifyOrder.Value : 0
                        }
                    })
                    .ToList();

                return data;
            }
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

        public static List<RefundInfo> getRefundQuantity(int? customerID, DateTime? fromDate, DateTime? toDate)
        {
            using (var con = new inventorymanagementEntities())
            {
                var refund = con.tbl_RefundGoodsDetails.Where(x => 1 == 1);

                if(customerID.HasValue)
                    refund = refund.Where(x => x.CustomerID == customerID);

                if (fromDate.HasValue)
                    refund = refund.Where(x => DbFunctions.TruncateTime(x.CreatedDate) >= fromDate);

                if (toDate.HasValue)
                    refund = refund.Where(x => DbFunctions.TruncateTime(x.CreatedDate) <= toDate);

                return refund.GroupBy(g => g.CustomerID.Value)
                    .Select(x => new RefundInfo()
                    {
                        customerID = x.Key,
                        refundNoFeeQuantity = x.Sum(s =>
                            (s.TotalRefundFee == "0" && s.RefundType == 2) ? (s.Quantity.HasValue ? s.Quantity.Value : 0) : 0
                        ),
                        refundFeeQuantity = x.Sum(s =>
                            !(s.TotalRefundFee == "0" && s.RefundType == 2) ? (s.Quantity.HasValue ? s.Quantity.Value : 0) : 0
                        )
                    })
                    .OrderBy(o => o.customerID)
                    .ToList();
            }
        }

        #endregion

        #region Class
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

            public bool InApp { get; set; }
            public int SendSMSIntroApp { get; set; }
        }

        public class RefundInfo
        {
            public int customerID { get; set; }
            public double refundNoFeeQuantity { get; set; }
            public double refundFeeQuantity { get; set; }
        }
        #endregion
    }
}