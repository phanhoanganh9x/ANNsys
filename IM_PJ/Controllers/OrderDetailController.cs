using IM_PJ.Models;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using WebUI.Business;

namespace IM_PJ.Controllers
{
    public class OrderDetailController
    {
        #region CRUD
        public static string Insert(int AgentID, int OrderID, string SKU, int ProductID, int ProductVariableID, string ProductVariableDescrition, double Quantity,
            double Price, int Status, double DiscountPrice, int ProductType, DateTime CreatedDate, string CreatedBy, bool IsCount)
        {
            using (var con = new inventorymanagementEntities())
            {
                #region Cập nhật thôn tin giá vốn
                Nullable<double> cogs = null;

                #region Sản phẩm đơn gian
                if (ProductType == 1)
                {
                    cogs = con.tbl_Product
                        .Where(x => x.ID == ProductID)
                        .Select(x => x.CostOfGood.HasValue ? x.CostOfGood.Value : 0)
                        .SingleOrDefault();
                }
                #endregion

                #region Sản phẩm biến thể
                if (ProductType == 2)
                {
                    cogs = con.tbl_ProductVariable
                        .Where(x => x.ID == ProductVariableID)
                        .Select(x => x.CostOfGood.HasValue ? x.CostOfGood.Value : 0)
                        .SingleOrDefault();
                }
                #endregion
                #endregion

                #region Khởi tạo chi tiết đơn hàng
                tbl_OrderDetail ui = new tbl_OrderDetail();
                ui.AgentID = AgentID;
                ui.OrderID = OrderID;
                ui.SKU = SKU;
                ui.ProductID = ProductID;
                ui.ProductVariableID = ProductVariableID;
                ui.ProductVariableDescrition = ProductVariableDescrition;
                ui.Quantity = Quantity;
                ui.Price = Convert.ToDouble(Price);
                ui.Status = Status;
                ui.DiscountPrice = DiscountPrice;
                ui.ProductType = ProductType;
                ui.CreatedDate = CreatedDate;
                ui.CreatedBy = CreatedBy;
                ui.ModifiedDate = CreatedDate;
                ui.ModifiedBy = CreatedBy;
                ui.IsCount = IsCount;
                ui.CostOfGood = cogs.HasValue ? Convert.ToDecimal(cogs.Value) : 0;

                con.Entry(ui).State = System.Data.Entity.EntityState.Added;
                con.tbl_OrderDetail.Add(ui);
                var kq = con.SaveChanges();
                #endregion

                return kq.ToString();
            }
        }

        public static void Insert(tbl_OrderDetail orderDetail)
        {
            using (var con = new inventorymanagementEntities())
            {
                #region Cập nhật thôn tin giá vốn
                Nullable<double> cogs = null;

                #region Sản phẩm đơn gian
                if (orderDetail.ProductType == 1)
                {
                    cogs = con.tbl_Product
                        .Where(x => x.ID == orderDetail.ProductID)
                        .Select(x => x.CostOfGood.HasValue ? x.CostOfGood.Value : 0)
                        .SingleOrDefault();
                }
                #endregion

                #region Sản phẩm biến thể
                if (orderDetail.ProductType == 2)
                {
                    cogs = con.tbl_ProductVariable
                        .Where(x => x.ID == orderDetail.ProductVariableID)
                        .Select(x => x.CostOfGood.HasValue ? x.CostOfGood.Value : 0)
                        .SingleOrDefault();
                }
                #endregion
                #endregion

                #region Khởi tạo chi tiết đơn hàng
                tbl_OrderDetail ui = new tbl_OrderDetail();
                ui.AgentID = orderDetail.AgentID;
                ui.OrderID = orderDetail.OrderID;
                ui.SKU = orderDetail.SKU;
                ui.ProductID = orderDetail.ProductID;
                ui.ProductVariableID = orderDetail.ProductVariableID;
                ui.ProductVariableDescrition = orderDetail.ProductVariableDescrition;
                ui.Quantity = orderDetail.Quantity;
                ui.Price = orderDetail.Price;
                ui.Status = orderDetail.Status;
                ui.DiscountPrice = orderDetail.DiscountPrice;
                ui.ProductType = orderDetail.ProductType;
                ui.CreatedDate = orderDetail.CreatedDate;
                ui.CreatedBy = orderDetail.CreatedBy;
                ui.ModifiedDate = orderDetail.ModifiedDate;
                ui.ModifiedBy = orderDetail.ModifiedBy;
                ui.IsCount = orderDetail.IsCount;
                ui.CostOfGood = cogs.HasValue ? Convert.ToDecimal(cogs.Value) : 0;

                con.Entry(ui).State = System.Data.Entity.EntityState.Added;
                con.tbl_OrderDetail.Add(ui);
                con.SaveChanges();
                #endregion
            }
        }

        /// <summary>
        /// Khởi tạo chi tiết đơn hàng
        /// </summary>
        /// <param name="orderDetails">Danh sách các sản phẩm</param>
        public static List<tbl_OrderDetail> Insert(List<tbl_OrderDetail> orderDetails)
        {
            using (var con = new inventorymanagementEntities())
            {
                if (orderDetails == null && orderDetails.Count == 0)
                    return null;

                #region Cập nhật thôn tin giá vốn
                List<ProductCOGSModel> product = new List<ProductCOGSModel>();
                List<ProductVariationCOGSModel> variation = new List<ProductVariationCOGSModel>();

                #region Sản phẩm đơn gian
                var productFilter = orderDetails
                    .Where(x => x.ProductType == 1)
                    .Where(x => x.ProductID.HasValue)
                    .Select(x => x.ProductID.Value)
                    .Distinct()
                    .ToList();

                if (productFilter != null && productFilter.Count > 0)
                {
                    product = con.tbl_Product
                        .Where(x =>
                            productFilter.Contains(x.ID)
                        )
                        .Select(x => new ProductCOGSModel()
                        {
                            id = x.ID,
                            cogs = x.CostOfGood.HasValue ? x.CostOfGood.Value : 0
                        })
                        .OrderBy(o => o.id)
                        .ToList();
                }
                #endregion

                #region Sản phẩm biến thể
                var variationFilter = orderDetails
                    .Where(x => x.ProductType == 2)
                    .Where(x => x.ProductVariableID.HasValue)
                    .Select(x => x.ProductVariableID.Value)
                    .Distinct()
                    .ToList();

                if (variationFilter != null && variationFilter.Count > 0)
                {
                    variation = con.tbl_ProductVariable
                        .Where(x =>
                            variationFilter.Contains(x.ID)
                        )
                        .Select(x => new ProductVariationCOGSModel()
                        {
                            id = x.ID,
                            cogs = x.CostOfGood.HasValue ? x.CostOfGood.Value : 0
                        })
                        .OrderBy(o => o.id)
                        .ToList();
                }
                #endregion
                #endregion

                #region Cập nhật chi tiết đơn hàng
                orderDetails = orderDetails
                    .Where(x => x.ProductType.HasValue)
                    .Where(x => x.ProductID.HasValue)
                    .Where(x => x.ProductVariableID.HasValue)
                    .GroupJoin(
                        product,
                        d => new { productStyle = d.ProductType.Value, productID = d.ProductID.Value },
                        p => new { productStyle = 1, productID = p.id },
                        (d, p) => new { order = d, product = p }
                    )
                    .SelectMany(
                        x => x.product.DefaultIfEmpty(),
                        (parent, child) => new { order = parent.order, product = child }
                    )
                    .GroupJoin(
                        variation,
                        temp => new { productStyle = temp.order.ProductType.Value, variationID = temp.order.ProductVariableID.Value },
                        v => new { productStyle = 2, variationID = v.id },
                        (temp, v) => new { order = temp.order, product = temp.product, variation = v }
                    )
                    .SelectMany(
                        x => x.variation.DefaultIfEmpty(),
                        (parent, child) => new { order = parent.order, product = parent.product, variation = child }
                    )
                    .Select(x =>
                    {
                        var item = x.order;

                        item.ModifiedDate = item.CreatedDate;
                        item.ModifiedBy = item.ModifiedBy;

                        // Cập nhật số tiền gốc
                        if (item.ProductType == 1 && x.product != null)
                            item.CostOfGood += Convert.ToDecimal(x.product.cogs);
                        if (item.ProductType == 2 && x.variation != null)
                            item.CostOfGood += Convert.ToDecimal(x.variation.cogs);

                        return item;
                    })
                    .ToList();
                #endregion

                #region Khởi tạo chi tiết đơn hàng
                var size = 100;
                var counts = Math.Ceiling(orderDetails.Count / (double)size);

                for (int index = 0; index < counts; index++)
                {
                    var insertedData = orderDetails
                        .Skip(index * size)
                        .Take(size)
                        .ToList();
                    con.tbl_OrderDetail.AddRange(insertedData);
                    con.SaveChanges();
                }
                #endregion

                return orderDetails;
            }
        }

        public static string UpdateQuantity(int id, double quantity, double price, double discount, DateTime modifiedDate, string modifiedBy)
        {
            using (var con = new inventorymanagementEntities())
            {
                var orderDetail = con.tbl_OrderDetail
                    .Where(o => o.ID == id)
                    .SingleOrDefault();

                if (orderDetail == null)
                    return "0";

                con.Entry(orderDetail).State = System.Data.Entity.EntityState.Modified;
                orderDetail.Quantity = quantity;
                orderDetail.Price = price;
                orderDetail.DiscountPrice = discount;
                orderDetail.ModifiedDate = modifiedDate;
                orderDetail.ModifiedBy = modifiedBy;
                var kq = con.SaveChanges();

                return kq.ToString();
            }
        }

        public static string UpdateIsCount(int ID, bool isCount)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                var ui = dbe.tbl_OrderDetail.Where(o => o.ID == ID).FirstOrDefault();
                if (ui != null)
                {
                    ui.IsCount = isCount;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                return "0";
            }
        }
        public static string Delete(int ID,string SKU)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                var ui = dbe.tbl_OrderDetail
                    .Where(o => o.ID == ID && o.SKU == SKU)
                    .SingleOrDefault();

                if (ui == null)
                    return "0";

                dbe.Entry(ui).State = System.Data.Entity.EntityState.Deleted;
                dbe.tbl_OrderDetail.Remove(ui);
                var kq = dbe.SaveChanges();

                return kq.ToString();
            }
        }
        #endregion
        #region Select
        public static tbl_OrderDetail GetByID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_OrderDetail ai = dbe.tbl_OrderDetail.Where(a => a.ID == ID).SingleOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }
        public static List<tbl_OrderDetail> GetAll()
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_OrderDetail> ags = new List<tbl_OrderDetail>();
                ags = dbe.tbl_OrderDetail.OrderByDescending(o => o.ID).ToList();
                return ags;
            }
        }
        public static List<tbl_OrderDetail> GetAllByAgentID(int AgentID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_OrderDetail> ags = new List<tbl_OrderDetail>();
                ags = dbe.tbl_OrderDetail.Where(o => o.AgentID == AgentID).OrderByDescending(o => o.ID).ToList();
                return ags;
            }
        }
        public static List<tbl_OrderDetail> GetByOrderID(int OrderID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_OrderDetail> ags = new List<tbl_OrderDetail>();
                ags = dbe.tbl_OrderDetail.Where(o => o.OrderID == OrderID).OrderBy(o => o.ID).ToList();
                return ags;
            }
        }
        public static List<tbl_OrderDetail> GetByIDSortBySKU(int OrderID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_OrderDetail> ags = new List<tbl_OrderDetail>();
                ags = dbe.tbl_OrderDetail.Where(o => o.OrderID == OrderID).OrderBy(o => o.SKU).ThenByDescending(o => o.Price).ToList();
                return ags;
            }
        }
        public static List<tbl_OrderDetail> GetByProductID(int ProductID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_OrderDetail> ags = new List<tbl_OrderDetail>();
                ags = dbe.tbl_OrderDetail.Where(o => o.ProductID == ProductID).ToList();
                return ags;
            }
        }
        public static List<tbl_OrderDetail> GetByProductVariableID(int ProductVariableID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_OrderDetail> ags = new List<tbl_OrderDetail>();
                ags = dbe.tbl_OrderDetail.Where(o => o.ProductVariableID == ProductVariableID && o.ProductID == 0).ToList();
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
        #endregion
        public static List<tbl_OrderDetail> GetAllToday()
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_OrderDetail> ags = new List<tbl_OrderDetail>();
                ags = dbe.tbl_OrderDetail.OrderByDescending(o => o.ID).ToList();
                return ags;
            }
        }

        public static List<orderReport> Report(string fromdate, string todate)
        {

            var list = new List<orderReport>();
            var sql = new StringBuilder();

            sql.AppendLine(String.Format("SELECT"));
            sql.AppendLine(String.Format("		ODD.ID"));
            sql.AppendLine(String.Format(",		ODD.SKU"));
            sql.AppendLine(String.Format(",		SUM(ISNULL(ODD.Quantity, 0)) AS Quantity"));
            sql.AppendLine(String.Format(",		SUM(ISNULL(ODD.Price, 0)) AS Price"));
            sql.AppendLine(String.Format(",		2 AS ExcuteStatus"));
            sql.AppendLine(String.Format(",		3 AS PaymentStatus"));
            sql.AppendLine(String.Format("FROM"));
            sql.AppendLine(String.Format("		tbl_Order AS ORD"));
            sql.AppendLine(String.Format("INNER JOIN tbl_OrderDetail AS ODD"));
            sql.AppendLine(String.Format("	ON 	ORD.ID = ODD.OrderID"));
            sql.AppendLine(String.Format("WHERE"));
            sql.AppendLine(String.Format("		CONVERT(NVARCHAR(10), ORD.DateDone, 121) BETWEEN CONVERT(NVARCHAR(10), '{0:yyyy-MM-dd}', 121) AND CONVERT(NVARCHAR(10), '{1:yyyy-MM-dd}', 121)", fromdate, todate));
            sql.AppendLine(String.Format("	AND ORD.ExcuteStatus = 2"));
            sql.AppendLine(String.Format("	AND (ORD.PaymentStatus = 2 OR ORD.PaymentStatus = 3 OR Ord.PaymentStatus = 4)"));
            sql.AppendLine(String.Format("GROUP BY"));
            sql.AppendLine(String.Format("		ODD.ID"));
            sql.AppendLine(String.Format(",		ODD.SKU"));
            sql.AppendLine(String.Format(";"));

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql.ToString());
            while (reader.Read())
            {
                var entity = new orderReport();

                entity.ID = Convert.ToInt32(reader["ID"]);

                entity.SKU = reader["SKU"].ToString();

                entity.Quantity = Convert.ToDouble(reader["Quantity"]);

                entity.Price = Convert.ToDouble(reader["Price"]);

                entity.ExcuteStatus = Convert.ToInt32(reader["ExcuteStatus"]);

                entity.ExcuteStatus = Convert.ToInt32(reader["PaymentStatus"]);

                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public class orderReport
        {
            public int ID { get; set; }
            public string SKU { get; set; }
            public double Quantity { get; set; }
            public double Price { get; set; }
            public DateTime DateDone { get; set; }
            public int ExcuteStatus { get; set; }
            public int PaymentStatus { get; set; }

        }
    }
}