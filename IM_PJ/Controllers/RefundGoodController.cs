using IM_PJ.Models;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using WebUI.Business;

namespace IM_PJ.Controllers
{
    public class RefundGoodController
    {
        #region CRUD
        public static int Insert(int AgentID, string TotalPrice, int Status, int CustomerID, double TotalQuantity, string TotalRefundFee,
            string AgentName, string CustomerName, string CustomerPhone, DateTime CreatedDate, string CreatedBy, string RefundNote)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_RefundGoods a = new tbl_RefundGoods();
                a.AgentID = AgentID;
                a.TotalPrice = TotalPrice;
                a.Status = Status;
                a.CustomerID = CustomerID;
                a.TotalQuantity = TotalQuantity;
                a.TotalRefundFee = TotalRefundFee;
                a.AgentName = AgentName;
                a.CustomerName = CustomerName;
                a.CustomerPhone = CustomerPhone;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                a.RefundNote = RefundNote;
                dbe.tbl_RefundGoods.Add(a);
                dbe.SaveChanges();
                int kq = a.ID;
                return kq;
            }
        }

        public static int Insert(tbl_RefundGoods refund)
        {
            using (var con = new inventorymanagementEntities())
            {
                con.tbl_RefundGoods.Add(refund);
                con.SaveChanges();
                int kq = refund.ID;
                return kq;
            }
        }

        public static string updatestatus(int ID, int status, DateTime ModifiedDate, string ModifiedBy, string Note)
        {
            using (var dbe = new inventorymanagementEntities())
            {

                var a = dbe.tbl_RefundGoods.Where(ac => ac.ID == ID).SingleOrDefault();
                if (a != null)
                {
                    a.Status = status;
                    a.RefundNote = Note;
                    a.ModifiedBy = ModifiedBy;
                    a.ModifiedDate = ModifiedDate;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static string UpdateCustomerPhone(int ID, string CustomerPhone)
        {
            using (var dbe = new inventorymanagementEntities())
            {

                var a = dbe.tbl_RefundGoods.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.RefundNote = a.RefundNote + ". Số điện thoại cũ của khách hàng này là " + a.CustomerPhone;
                    a.CustomerPhone = CustomerPhone;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static bool DeleteByID(int ID)
        {
            using (var con = new inventorymanagementEntities())
            {
                var item = con.tbl_RefundGoods.Where(x => x.ID == ID).SingleOrDefault();

                if (item != null)
                {
                    con.tbl_RefundGoods.Remove(item);
                    con.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        #endregion
        #region Select        
        public static List<tbl_RefundGoods> GetAll(string s)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_RefundGoods> las = new List<tbl_RefundGoods>();
                las = dbe.tbl_RefundGoods.OrderByDescending(r => r.CreatedDate).ToList();
                return las;
            }
        }
        public static List<tbl_RefundGoods> GetByCustomerID(int CustomerID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_RefundGoods> las = new List<tbl_RefundGoods>();
                las = dbe.tbl_RefundGoods.Where(r => r.CustomerID == CustomerID).OrderByDescending(r => r.CreatedDate).ToList();
                return las;
            }
        }

        public static List<tbl_RefundGoods> GetByAgentIDCustomerIDFromDateToDate(int AgentID, int CustomerID, DateTime FromDate, DateTime ToDate)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_RefundGoods> las = new List<tbl_RefundGoods>();
                las = dbe.tbl_RefundGoods.Where(r => r.AgentID == AgentID && r.CustomerID == CustomerID && r.CreatedDate >= FromDate && r.CreatedDate < ToDate).OrderByDescending(r => r.CreatedDate).ToList();
                return las;
            }
        }
        public static List<tbl_RefundGoods> GetByAgentID(int AgentID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_RefundGoods> las = new List<tbl_RefundGoods>();
                las = dbe.tbl_RefundGoods.Where(r => r.AgentID == AgentID).OrderByDescending(r => r.CreatedDate).ToList();
                return las;
            }
        }
        public static List<tbl_RefundGoods> GetByAgent(int AgentID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_RefundGoods> las = new List<tbl_RefundGoods>();

                las = dbe.tbl_RefundGoods.Where(r => r.AgentID == AgentID).OrderByDescending(r => r.CreatedDate).ToList();
                var a = las.GroupBy(r => r.CreatedBy).ToList();
                return las;
            }
        }
        public static tbl_RefundGoods GetByID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_RefundGoods acc = dbe.tbl_RefundGoods.Where(a => a.ID == ID).FirstOrDefault();
                if (acc != null)
                    return acc;
                else
                    return null;
            }
        }

        public static tbl_RefundGoods GetOrderByID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_RefundGoods acc = dbe.tbl_RefundGoods.Where(a => a.ID == ID && a.Status == 1).FirstOrDefault();
                if (acc != null)
                    return acc;
                else
                    return null;
            }
        }
        public static tbl_RefundGoods GetByIDAndAgentID(int ID, int AgentID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                var las = dbe.tbl_RefundGoods.Where(r => r.AgentID == AgentID && r.ID == ID).FirstOrDefault();
                if (las != null)
                    return las;
                else return null;
            }
        }

        public static int UpdateStatus(int ID, string createdby, int status, int orderSale)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                var las = dbe.tbl_RefundGoods.Where(r => r.ID == ID).FirstOrDefault();
                if (las != null)
                {
                    las.Status = status;
                    if(orderSale > 0)
                    {
                        las.RefundNote = "Đã trừ tiền trong đơn " + orderSale.ToString();
                    }
                    else
                    {
                        las.RefundNote = "";
                    }
                    las.OrderSaleID = orderSale;
                    las.ModifiedBy = createdby;
                    las.ModifiedDate = DateTime.Now;
                    int i = dbe.SaveChanges();
                    return i;
                }
                return 0;
            }
        }

        public static List<RefundOrder> Filter(string TextSearch, int Status, string RefundFee, string CreatedBy, string CreatedDate)
        {
            var list = new List<RefundOrder>();
            var sql = new StringBuilder();

            sql.AppendLine(String.Format("SELECT Ord.ID, Ord.CustomerName, Ord.CustomerPhone, Customer.Nick, Ord.CustomerID, Ord.Status, Ord.TotalPrice, Ord.TotalRefundFee, Ord.CreatedBy, Ord.CreatedDate, Ord.RefundNote, Ord.OrderSaleID, SUM(ISNULL(OrdDetail.Quantity, 0)) AS Quantity "));
            sql.AppendLine(String.Format("FROM tbl_RefundGoods AS Ord"));
            sql.AppendLine(String.Format("INNER JOIN tbl_RefundGoodsDetails AS OrdDetail"));
            sql.AppendLine(String.Format("ON 	Ord.ID = OrdDetail.RefundGoodsID"));
            sql.AppendLine(String.Format("INNER JOIN tbl_Customer AS Customer"));
            sql.AppendLine(String.Format("ON 	Ord.CustomerID = Customer.ID"));
            sql.AppendLine(String.Format("WHERE 1 = 1"));

            if (Status > 0)
            {
                sql.AppendLine(String.Format("	AND Ord.Status = {0}", Status));
            }

            if (RefundFee != "")
            {
                if(RefundFee == "yes")
                {
                    sql.AppendLine(String.Format("	AND Ord.TotalRefundFee > 0"));
                }
                else
                {
                    sql.AppendLine(String.Format("	AND Ord.TotalRefundFee = 0"));
                }
            }

            if (TextSearch != "")
            {
                string TextSearchName = '"' + TextSearch + '"';
                sql.AppendLine(String.Format("	AND (  CONTAINS(Ord.CustomerName, '{1}') OR CONTAINS(Customer.Nick, '{1}') OR (Ord.CustomerPhone = '{0}') OR (convert(nvarchar, Ord.ID) = '{0}') OR (convert(nvarchar, Ord.OrderSaleID) = '{0}') OR (OrdDetail.SKU LIKE '{0}%')  )", TextSearch, TextSearchName));
            }

            if (CreatedBy != "")
            {
                sql.AppendLine(String.Format("	AND Ord.CreatedBy = '{0}'", CreatedBy));
            }

            if (CreatedDate != "")
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
                    case "lastmonth":
                        var thismonth = new DateTime(fromdate.Year, fromdate.Month, 1);
                        fromdate = thismonth.AddMonths(-1);
                        todate = thismonth;
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
                sql.AppendLine(String.Format("	AND	(CONVERT(datetime, Ord.CreatedDate, 121) BETWEEN CONVERT(datetime, '{0}', 121) AND CONVERT(datetime, '{1}', 121))", fromdate.ToString(), todate.ToString()));
            }

            sql.AppendLine(String.Format("GROUP BY Ord.ID, Ord.CustomerName, Ord.CustomerPhone, Customer.Nick, Ord.CustomerID, Ord.Status, Ord.TotalPrice, Ord.TotalRefundFee, Ord.CreatedBy, Ord.CreatedDate, Ord.RefundNote, Ord.OrderSaleID"));
            sql.AppendLine(String.Format("ORDER BY Ord.ID DESC"));

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql.ToString());
            while (reader.Read())
            {
                var entity = new RefundOrder();

                entity.ID = Convert.ToInt32(reader["ID"]);
                entity.CustomerName = reader["CustomerName"].ToString();
                entity.CustomerPhone = reader["CustomerPhone"].ToString();
                entity.CustomerID = Convert.ToInt32(reader["CustomerID"]);
                entity.Nick = reader["Nick"].ToString();
                entity.Status = Convert.ToInt32(reader["Status"]);
                entity.TotalPrice = Convert.ToInt32(reader["TotalPrice"]);
                entity.TotalRefundFee = Convert.ToInt32(reader["TotalRefundFee"]);
                entity.CreatedBy = reader["CreatedBy"].ToString();
                entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                if (reader["RefundNote"] != DBNull.Value)
                    entity.RefundNote = reader["RefundNote"].ToString();
                if (reader["OrderSaleID"] != DBNull.Value)
                    entity.OrderSaleID = Convert.ToInt32(reader["OrderSaleID"]);
                entity.Quantity = Convert.ToInt32(reader["Quantity"]);

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static RefundProductReportModel getRefundProductReport(string SKU, DateTime fromDate, DateTime toDate)
        {
            var list = new List<RefundReport>();
            var sql = new StringBuilder();

            sql.AppendLine(String.Format("SELECT Ord.ID, SUM(ISNULL(OrdDetail.Quantity, 0)) AS Quantity, SUM(OrdDetail.Quantity * ISNULL(Product.CostOfGood, Variable.CostOfGood)) AS TotalCost, SUM(OrdDetail.Quantity * OrdDetail.SoldPricePerProduct) AS TotalRevenue, SUM(OrdDetail.Quantity * OrdDetail.RefundFeePerProduct) AS TotalRefundFee"));
            sql.AppendLine(String.Format("FROM tbl_RefundGoods AS Ord"));
            sql.AppendLine(String.Format("INNER JOIN tbl_RefundGoodsDetails AS OrdDetail"));
            sql.AppendLine(String.Format("ON 	Ord.ID = OrdDetail.RefundGoodsID"));
            sql.AppendLine(String.Format("LEFT JOIN tbl_ProductVariable AS Variable"));
            sql.AppendLine(String.Format("ON 	OrdDetail.SKU = Variable.SKU"));
            sql.AppendLine(String.Format("LEFT JOIN tbl_Product AS Product"));
            sql.AppendLine(String.Format("ON 	OrdDetail.SKU = Product.ProductSKU"));
            sql.AppendLine(String.Format("WHERE 1 = 1"));
            sql.AppendLine(String.Format("	AND OrdDetail.SKU LIKE '{0}%'", SKU));
            sql.AppendLine(String.Format("	AND	(CONVERT(datetime, Ord.CreatedDate, 121) BETWEEN CONVERT(datetime, '{0}', 121) AND CONVERT(datetime, '{1}', 121))", fromDate.ToString(), toDate.ToString()));
            sql.AppendLine(String.Format("GROUP BY Ord.ID, OrdDetail.RefundFeePerProduct"));

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql.ToString());
            while (reader.Read())
            {
                var entity = new RefundReport();

                entity.ID = Convert.ToInt32(reader["ID"]);
                entity.Quantity = Convert.ToInt32(reader["Quantity"]);
                entity.TotalCost = Convert.ToDouble(reader["TotalCost"]);
                entity.TotalRevenue = Convert.ToDouble(reader["TotalRevenue"]);
                entity.TotalRefundFee = Convert.ToInt32(reader["TotalRefundFee"]);
                list.Add(entity);
            }
            reader.Close();

            return new RefundProductReportModel()
            {
                totalRefund = list.Sum(x => x.Quantity),
                totalRevenue = list.Sum(x => x.TotalRevenue),
                totalCost = list.Sum(x => x.TotalCost),
                totalRefundFee = list.Sum(x => x.TotalRefundFee),
            };
        }

        public class RefundProductReportModel
        {
            public int totalRefund { get; set; }
            public double totalCost { get; set; }
            public double totalRevenue { get; set; }
            public double totalRefundFee { get; set; }
        }

        public static List<tbl_RefundGoods> Search(string s, int n, int status, string by)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_RefundGoods> ags = new List<tbl_RefundGoods>();
                if (!string.IsNullOrEmpty(s))
                {
                    if (n > 0)
                    {
                        if (status > 0)
                        {
                            if (!string.IsNullOrEmpty(by))
                            {
                                ags = dbe.tbl_RefundGoods.Where(x => x.CustomerName.Contains(s) && x.AgentID == n && x.Status == status && x.CreatedBy == by || x.CreatedBy == by && x.CustomerPhone.Contains(s) && x.AgentID == n && x.Status == status || x.ID.ToString().Contains(s) && x.CreatedBy == by && x.AgentID == n && x.Status == status).ToList();
                            }
                            else
                            {
                                ags = dbe.tbl_RefundGoods.Where(x => x.CustomerName.Contains(s) && x.AgentID == n && x.Status == status || x.CustomerPhone.Contains(s) && x.AgentID == n && x.Status == status || x.ID.ToString().Contains(s) && x.AgentID == n && x.Status == status).ToList();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(by))
                            {
                                ags = dbe.tbl_RefundGoods.Where(x => x.CustomerName.Contains(s) && x.AgentID == n && x.CreatedBy == by || x.CreatedBy == by && x.CustomerPhone.Contains(s) && x.AgentID == n || x.ID.ToString().Contains(s) && x.AgentID == n && x.CreatedBy == by).ToList();

                            }
                            else
                            {
                                ags = dbe.tbl_RefundGoods.Where(x => x.CustomerName.Contains(s) && x.AgentID == n || x.CustomerPhone.Contains(s) && x.AgentID == n || x.ID.ToString().Contains(s) && x.AgentID == n).ToList();
                            }
                        }
                    }
                    else
                    {
                        if (status > 0)
                        {

                            ags = dbe.tbl_RefundGoods.Where(x => x.CustomerName.Contains(s) && x.AgentID == n && x.Status == status || x.CustomerPhone.Contains(s) && x.AgentID == n && x.Status == status || x.ID.ToString().Contains(s) && x.AgentID == n && x.Status == status).ToList();
                        }
                        else
                        {
                            ags = dbe.tbl_RefundGoods.Where(x => x.CustomerName.Contains(s) || x.CustomerPhone.Contains(s) || x.ID.ToString().Contains(s)).ToList();
                        }

                    }
                }
                else
                {
                    if (n > 0)
                    {
                        if (status > 0)
                        {
                            if (!string.IsNullOrEmpty(by))
                            {
                                ags = dbe.tbl_RefundGoods.Where(x => x.AgentID == n && x.Status == status && x.CreatedBy == by).ToList();
                            }
                            else
                            {
                                ags = dbe.tbl_RefundGoods.Where(x => x.AgentID == n && x.Status == status).ToList();
                            }

                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(by))
                            {
                                ags = dbe.tbl_RefundGoods.Where(x => x.AgentID == n && x.CreatedBy == by).ToList();
                            }
                            else
                            {
                                ags = dbe.tbl_RefundGoods.Where(x => x.AgentID == n).ToList();
                            }

                        }
                    }
                    else
                    {
                        if (status > 0)
                        {
                            if (!string.IsNullOrEmpty(by))
                            {
                                ags = dbe.tbl_RefundGoods.Where(x => x.Status == status && x.CreatedBy == by).ToList();
                            }
                            else
                            {
                                ags = dbe.tbl_RefundGoods.Where(x => x.Status == status).ToList();
                            }

                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(by))
                            {
                                ags = dbe.tbl_RefundGoods.Where(x => x.CreatedBy == by).ToList();
                            }
                            else
                            {
                                ags = dbe.tbl_RefundGoods.ToList();
                            }

                        }

                    }
                }
                return ags.OrderByDescending(x => x.CreatedDate).ToList();
            }
        }

        public static List<tbl_RefundGoods> TotalRefund(string fromdate, string todate)
        {
            using (var db = new inventorymanagementEntities())
            {
                List<tbl_RefundGoods> or = new List<tbl_RefundGoods>();
                if (!string.IsNullOrEmpty(fromdate))
                {
                    if (!string.IsNullOrEmpty(todate))
                    {
                        DateTime fd = Convert.ToDateTime(fromdate);
                        DateTime td = Convert.ToDateTime(todate);
                        or = db.tbl_RefundGoods
                            .Where(r => r.CreatedDate >= fd && r.CreatedDate <= td)
                            .OrderByDescending(r => r.ID).ToList();
                    }
                    else
                    {
                        DateTime fd = Convert.ToDateTime(fromdate);
                        or = db.tbl_RefundGoods
                            .Where(r => r.CreatedDate >= fd)
                            .OrderByDescending(r => r.ID).ToList();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(todate))
                    {
                        DateTime td = Convert.ToDateTime(todate);
                        or = db.tbl_RefundGoods
                            .Where(r => r.CreatedDate <= td)
                            .OrderByDescending(r => r.ID).ToList();
                    }
                    else
                    {
                        or = db.tbl_RefundGoods.ToList();
                    }
                }
                return or;
            }
        }

        public static int GetTotalRefundByAccount(string accountName, DateTime fromdate, DateTime todate)
        {
            using (var db = new inventorymanagementEntities())
            {
                List<tbl_RefundGoods> or = new List<tbl_RefundGoods>();
                or = db.tbl_RefundGoods
                            .Where(x => (fromdate <= x.CreatedDate && x.CreatedDate <= todate)
                                        && x.CreatedBy.Trim().ToUpper() == accountName.Trim().ToUpper())
                            .ToList();
                int tongdoitra = 0;
                if (or != null)
                {
                    foreach (var item in or)
                    {
                        var oddetail = RefundGoodDetailController.GetByRefundGoodsID(item.ID);
                        if (oddetail != null)
                        {
                            foreach (var temp in oddetail)
                            {
                                tongdoitra += Convert.ToInt32(temp.Quantity);
                            }
                        }
                    }
                }
                return tongdoitra;
            }
        }

        public static string getOrderReturnJSON(int customerID)
        {
            using (var con = new inventorymanagementEntities())
            {
                var orders = con.tbl_RefundGoods
                    .Where(x => x.CustomerID == customerID)
                    .Where(x => x.Status == 1)
                    .OrderByDescending(o => o.CreatedDate)
                    .ToArray();

                var serializer = new JavaScriptSerializer();
                return serializer.Serialize(orders);
            }
        }

        #endregion
        public class RefundReport
        {
            public int ID { get; set; }
            public int Quantity { get; set; }
            public double TotalRevenue { get; set; }
            public double TotalCost { get; set; }
            public double TotalRefundFee { get; set; }
        }
        public class RefundOrder
        {
            public int ID { get; set; }
            public int CustomerID { get; set; }
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public string Nick { get; set; }
            public int Status { get; set; }
            public int Quantity { get; set; }
            public double TotalPrice { get; set; }
            public double TotalRefundFee { get; set; }
            public DateTime CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public string RefundNote { get; set; }
            public Nullable<int> OrderSaleID { get; set; }
        }
    }
}