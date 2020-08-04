using IM_PJ.Models;
using IM_PJ.Models.Pages.thong_ke_chuyen_kho;
using IM_PJ.Models.Pages.thong_ke_nhap_kho;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using WebUI.Business;

namespace IM_PJ.Controllers
{
    public class StockManagerController
    {
        #region Quản lý kho 1

        #region CRUD
        public static void Insert(tbl_StockManager stock)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                var stockOld = dbe.tbl_StockManager
                    .Where(x => x.AgentID == stock.AgentID
                                && x.ProductID == stock.ProductID
                                && x.ProductVariableID == stock.ProductVariableID)
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefault();

                // Check exists record stock old
                if (stockOld != null)
                {
                    // Calculator quantity current stock with SKU target
                    var quantityCurrent = 0D;

                    if (stockOld.Type == 1)
                    {
                        quantityCurrent = stockOld.QuantityCurrent.Value + stockOld.Quantity.Value;
                    }
                    else
                    {
                        quantityCurrent = stockOld.QuantityCurrent.Value - stockOld.Quantity.Value;
                    }

                    // Calculator quantity current stock when input SKU target
                    var quantityCurrentNew = 0D;

                    if (stock.Type == 2) // Ouput SKU
                    {
                        quantityCurrentNew = quantityCurrent - stock.Quantity.Value;

                        // Check quantity < 0 when output SKU target
                        if (quantityCurrentNew < 0)
                        {
                            tbl_StockManager stockBalance = new tbl_StockManager();
                            stockBalance.AgentID = stock.AgentID;
                            stockBalance.ProductID = stock.ProductID;
                            stockBalance.ProductVariableID = stock.ProductVariableID;
                            stockBalance.Quantity = quantityCurrentNew * (-1);
                            stockBalance.QuantityCurrent = quantityCurrent;
                            stockBalance.Type = 1;
                            stockBalance.NoteID = "Nhập lệch kho";
                            stockBalance.OrderID = stock.OrderID;
                            stockBalance.Status = stock.Status;
                            stockBalance.SKU = stock.SKU;
                            stockBalance.ParentID = stock.ParentID;
                            stockBalance.CreatedDate = stock.CreatedDate.Value.AddMilliseconds(-10);
                            stockBalance.CreatedBy = stock.CreatedBy;
                            stockBalance.ModifiedDate = stock.CreatedDate.Value.AddMilliseconds(-10);
                            stockBalance.ModifiedBy = stock.CreatedBy;
                            stockBalance.MoveProID = stock.MoveProID;

                            dbe.tbl_StockManager.Add(stockBalance);

                            // Handling balance quantity stock with SKU target
                            stock.QuantityCurrent = stock.Quantity;
                            dbe.tbl_StockManager.Add(stock);
                        }
                        else
                        {
                            // Update quantity stock with SKU target
                            stock.QuantityCurrent = quantityCurrent;
                            dbe.tbl_StockManager.Add(stock);
                        }
                    }
                    else // Input SKU
                    {
                        // Update quantity stock with SKU target
                        stock.QuantityCurrent = quantityCurrent;
                        dbe.tbl_StockManager.Add(stock);
                    }
                }
                else // New SKU 
                {
                    if(stock.Type == 2) // Output SKU
                    {
                        tbl_StockManager stockBalance = new tbl_StockManager();
                        stockBalance.AgentID = stock.AgentID;
                        stockBalance.ProductID = stock.ProductID;
                        stockBalance.ProductVariableID = stock.ProductVariableID;
                        stockBalance.Quantity = stock.Quantity;
                        stockBalance.QuantityCurrent = 0;
                        stockBalance.Type = 1;
                        stockBalance.NoteID = "Nhập kho sản phẩm mới";
                        stockBalance.OrderID = stock.OrderID;
                        stockBalance.Status = stock.Status;
                        stockBalance.SKU = stock.SKU;
                        stockBalance.ParentID = stock.ParentID;
                        stockBalance.CreatedDate = stock.CreatedDate.Value.AddMilliseconds(-10);
                        stockBalance.CreatedBy = stock.CreatedBy;
                        stockBalance.ModifiedDate = stock.CreatedDate.Value.AddMilliseconds(-10);
                        stockBalance.ModifiedBy = stock.CreatedBy;
                        stockBalance.MoveProID = stock.MoveProID;

                        dbe.tbl_StockManager.Add(stockBalance);

                        stock.QuantityCurrent = stockBalance.Quantity;
                        dbe.tbl_StockManager.Add(stock);
                    }
                    else // Input SKU
                    {
                        stock.QuantityCurrent = 0;
                        dbe.tbl_StockManager.Add(stock);
                    }
                }
 
                dbe.SaveChanges();
            }
        }

        public static void Insert(List<tbl_StockManager> stockManagers)
        {
            if (stockManagers.Count == 0)
            {
                return;
            }

            using (var con = new inventorymanagementEntities())
            {
                var stockManagersOrder = stockManagers.OrderBy(x => x.AgentID)
                                                      .ThenBy(x => x.ProductID)
                                                      .ThenBy(x => x.ProductVariableID)
                                                      .ToList();

                StringBuilder sqlString = new StringBuilder();

                sqlString.AppendLine("SELECT");
                sqlString.AppendLine("        DAT.[AgentID]");
                sqlString.AppendLine(",       DAT.[ProductID]");
                sqlString.AppendLine(",       DAT.[ProductVariableID]");
                sqlString.AppendLine("INTO #StockTarget");
                sqlString.AppendLine("FROM (");
                for (var i = 0; i < stockManagersOrder.Count(); i++)
                {
                    var sqlSub = String.Empty;

                    if (i != 0)
                    {
                        sqlSub += String.Format("UNION\n");
                    }

                    sqlSub += String.Format("SELECT\n");
                    sqlSub += String.Format("        {0} AS AgentID\n", stockManagersOrder[i].AgentID.Value);
                    sqlSub += String.Format(",       {0} AS ProductID\n", stockManagersOrder[i].ProductID.Value);
                    sqlSub += String.Format(",       {0} AS ProductVariableID\n", stockManagersOrder[i].ProductVariableID.Value);

                    sqlString.AppendLine(sqlSub);

                }
                sqlString.AppendLine(") AS DAT");
                sqlString.AppendLine(";");
                sqlString.AppendLine("");
                sqlString.AppendLine("SELECT");
                sqlString.AppendLine("        STM.[ID]");
                sqlString.AppendLine(",       STM.[AgentID]");
                sqlString.AppendLine(",       STM.[ProductID]");
                sqlString.AppendLine(",       STM.[ProductVariableID]");
                sqlString.AppendLine(",       STM.[Quantity]");
                sqlString.AppendLine(",       STM.[QuantityCurrent]");
                sqlString.AppendLine(",       STM.[Type]");
                sqlString.AppendLine(",       STM.[CreatedDate]");
                sqlString.AppendLine(",       STM.[CreatedBy]");
                sqlString.AppendLine(",       STM.[ModifiedDate]");
                sqlString.AppendLine(",       STM.[ModifiedBy]");
                sqlString.AppendLine(",       STM.[NoteID]");
                sqlString.AppendLine(",       STM.[OrderID]");
                sqlString.AppendLine(",       STM.[Status]");
                sqlString.AppendLine(",       STM.[SKU]");
                sqlString.AppendLine(",       STM.[MoveProID]");
                sqlString.AppendLine(",       STM.[ParentID]");
                sqlString.AppendLine("FROM");
                sqlString.AppendLine("        tbl_StockManager AS STM");
                sqlString.AppendLine("INNER JOIN #StockTarget AS TAG");
                sqlString.AppendLine("    ON  TAG.[AgentID]= STM.[AgentID]");
                sqlString.AppendLine("    AND TAG.[ProductID] = STM.[ProductID]");
                sqlString.AppendLine("    AND TAG.[ProductVariableID] = STM.[ProductVariableID]");
                sqlString.AppendLine("WHERE");
                sqlString.AppendLine("        CreatedDate = (");
                sqlString.AppendLine("                        SELECT");
                sqlString.AppendLine("                                MAX([CreatedDate])");
                sqlString.AppendLine("                        FROM");
                sqlString.AppendLine("                                tbl_StockManager AS SMM");
                sqlString.AppendLine("                        WHERE");
                sqlString.AppendLine("                                SMM.[AgentID] = STM.[AgentID]");
                sqlString.AppendLine("                        AND     SMM.[ProductID] = STM.[ProductID]");
                sqlString.AppendLine("                        AND     SMM.[ProductVariableID] = STM.[ProductVariableID]");
                sqlString.AppendLine("                    )");
                sqlString.AppendLine("ORDER BY");
                sqlString.AppendLine("        [AgentID]");
                sqlString.AppendLine(",       [ProductID]");
                sqlString.AppendLine(",       [ProductVariableID]");
                sqlString.AppendLine(";");

                var stockOlds = new List<tbl_StockManager>();

                var reader = (IDataReader)SqlHelper.ExecuteDataReader(sqlString.ToString());
                while (reader.Read())
                {
                    var stock = new tbl_StockManager();

                    stock.ID = Convert.ToInt32(reader["ID"]);

                    // AgentID
                    if (reader["AgentID"] != DBNull.Value)
                    {
                        stock.AgentID = Convert.ToInt32(reader["AgentID"]);
                    }

                    // ProductID
                    if (reader["ProductID"] != DBNull.Value)
                    {
                        stock.ProductID = Convert.ToInt32(reader["ProductID"]);
                    }

                    // ProductVariableID
                    if (reader["ProductVariableID"] != DBNull.Value)
                    {
                        stock.ProductVariableID = Convert.ToInt32(reader["ProductVariableID"]);
                    }

                    // Quantity
                    if (reader["Quantity"] != DBNull.Value)
                    {
                        stock.Quantity = Convert.ToDouble(reader["Quantity"]);
                    }

                    // Quantity
                    if (reader["QuantityCurrent"] != DBNull.Value)
                    {
                        stock.QuantityCurrent = Convert.ToDouble(reader["QuantityCurrent"]);
                    }

                    // Type
                    if (reader["Type"] != DBNull.Value)
                    {
                        stock.Type = Convert.ToInt32(reader["Type"]);
                    }

                    // CreatedDate
                    if (reader["CreatedDate"] != DBNull.Value)
                    {
                        stock.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    }

                    // CreatedBy
                    if (reader["CreatedBy"] != DBNull.Value)
                    {
                        stock.CreatedBy = reader["CreatedBy"].ToString();
                    }

                    // ModifiedDate
                    if (reader["ModifiedDate"] != DBNull.Value)
                    {
                        stock.ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"]);
                    }

                    // ModifiedBy
                    if (reader["ModifiedBy"] != DBNull.Value)
                    {
                        stock.ModifiedBy = reader["ModifiedBy"].ToString();
                    }

                    // NoteID
                    if (reader["NoteID"] != DBNull.Value)
                    {
                        stock.NoteID = reader["NoteID"].ToString();
                    }

                    // OrderID
                    if (reader["OrderID"] != DBNull.Value)
                    {
                        stock.OrderID = Convert.ToInt32(reader["OrderID"]);
                    }

                    // Status
                    if (reader["Status"] != DBNull.Value)
                    {
                        stock.Status = Convert.ToInt32(reader["Status"]);
                    }

                    // SKU
                    if (reader["SKU"] != DBNull.Value)
                    {
                        stock.SKU = reader["SKU"].ToString();
                    }

                    // MoveProID
                    if (reader["MoveProID"] != DBNull.Value)
                    {
                        stock.MoveProID = Convert.ToInt32(reader["MoveProID"]);
                    }

                    // ParentID
                    if (reader["ParentID"] != DBNull.Value)
                    {
                        stock.ParentID = Convert.ToInt32(reader["ParentID"]);
                    }

                    stockOlds.Add(stock);
                }

                var stockInput = stockManagersOrder
                    .Where(x => x.Type == 1)
                    .GroupJoin(
                        stockOlds,
                        stockNew => new { stockNew.AgentID, stockNew.ProductID, stockNew.ProductVariableID },
                        stockOld => new { AgentID = stockOld.AgentID, ProductID = stockOld.ProductID, ProductVariableID = stockOld.ProductVariableID },
                        (stockNew, stockOld) =>
                        {
                            var quantityCurrent = 0D;

                            if (stockOld.Count() > 0)
                            {
                                if (stockOld.FirstOrDefault().Type == 1)
                                {
                                    quantityCurrent = stockOld.FirstOrDefault().QuantityCurrent.Value + stockOld.FirstOrDefault().Quantity.Value;
                                }
                                else
                                {
                                    quantityCurrent = stockOld.FirstOrDefault().QuantityCurrent.Value - stockOld.FirstOrDefault().Quantity.Value;
                                }
                            }

                            return new tbl_StockManager()
                            {
                                AgentID = stockNew.AgentID,
                                ProductID = stockNew.ProductID,
                                ProductVariableID = stockNew.ProductVariableID,
                                Quantity = stockNew.Quantity,
                                QuantityCurrent = quantityCurrent,
                                Type = stockNew.Type,
                                NoteID = stockNew.NoteID,
                                OrderID = stockNew.OrderID,
                                Status = stockNew.Status,
                                SKU = stockNew.SKU,
                                ParentID = stockNew.ParentID,
                                CreatedDate = stockNew.CreatedDate,
                                CreatedBy = stockNew.CreatedBy,
                                ModifiedDate = stockNew.CreatedDate,
                                ModifiedBy = stockNew.CreatedBy,
                                MoveProID = stockNew.MoveProID
                            };
                         })
                    .ToList();

                var stockBalance = new List<tbl_StockManager>();
                var stockOut = stockManagersOrder
                    .Where(x => x.Type == 2)
                    .GroupJoin(
                        stockOlds,
                        stockNew => new { stockNew.AgentID, stockNew.ProductID, stockNew.ProductVariableID },
                        stockOld => new { AgentID = stockOld.AgentID, ProductID = stockOld.ProductID, ProductVariableID = stockOld.ProductVariableID },
                        (stockNew, stockOld) =>
                        {
                            // Check exists record stock old
                            if (stockOld.Count() > 0)
                            {
                                var quantityCurrent = 0D;

                                // Calculator quantity current stock with SKU target
                                if (stockOld.FirstOrDefault().Type == 1)
                                {
                                    quantityCurrent = stockOld.FirstOrDefault().QuantityCurrent.Value + stockOld.FirstOrDefault().Quantity.Value;
                                }
                                else
                                {
                                    quantityCurrent = stockOld.FirstOrDefault().QuantityCurrent.Value - stockOld.FirstOrDefault().Quantity.Value;
                                }

                                // Calculator quantity current stock when input SKU target
                                var quantityCurrentNew = 0D;

                                quantityCurrentNew = quantityCurrent - stockNew.Quantity.Value;

                                if (quantityCurrentNew < 0)
                                {
                                    stockBalance.Add(
                                        new tbl_StockManager()
                                        {
                                            AgentID = stockNew.AgentID,
                                            ProductID = stockNew.ProductID,
                                            ProductVariableID = stockNew.ProductVariableID,
                                            Quantity = quantityCurrentNew * (-1),
                                            QuantityCurrent = quantityCurrent,
                                            Type = 1,
                                            NoteID = "Nhập lệch kho",
                                            OrderID = stockNew.OrderID,
                                            Status = stockNew.Status,
                                            SKU = stockNew.SKU,
                                            ParentID = stockNew.ParentID,
                                            CreatedDate = stockNew.CreatedDate.Value.AddMilliseconds(-10),
                                            CreatedBy = stockNew.CreatedBy,
                                            ModifiedDate = stockNew.CreatedDate.Value.AddMilliseconds(-10),
                                            ModifiedBy = stockNew.CreatedBy,
                                            MoveProID = stockNew.MoveProID
                                        });

                                    return new tbl_StockManager()
                                    {
                                        AgentID = stockNew.AgentID,
                                        ProductID = stockNew.ProductID,
                                        ProductVariableID = stockNew.ProductVariableID,
                                        Quantity = stockNew.Quantity,
                                        QuantityCurrent = stockNew.Quantity,
                                        Type = stockNew.Type,
                                        NoteID = stockNew.NoteID,
                                        OrderID = stockNew.OrderID,
                                        Status = stockNew.Status,
                                        SKU = stockNew.SKU,
                                        ParentID = stockNew.ParentID,
                                        CreatedDate = stockNew.CreatedDate,
                                        CreatedBy = stockNew.CreatedBy,
                                        ModifiedDate = stockNew.CreatedDate,
                                        ModifiedBy = stockNew.CreatedBy,
                                        MoveProID = stockNew.MoveProID
                                    };
                                }
                                else
                                {
                                    return new tbl_StockManager()
                                    {
                                        AgentID = stockNew.AgentID,
                                        ProductID = stockNew.ProductID,
                                        ProductVariableID = stockNew.ProductVariableID,
                                        Quantity = stockNew.Quantity,
                                        QuantityCurrent = quantityCurrent,
                                        Type = stockNew.Type,
                                        NoteID = stockNew.NoteID,
                                        OrderID = stockNew.OrderID,
                                        Status = stockNew.Status,
                                        SKU = stockNew.SKU,
                                        ParentID = stockNew.ParentID,
                                        CreatedDate = stockNew.CreatedDate,
                                        CreatedBy = stockNew.CreatedBy,
                                        ModifiedDate = stockNew.CreatedDate,
                                        ModifiedBy = stockNew.CreatedBy,
                                        MoveProID = stockNew.MoveProID
                                    };
                                }
                            }
                            else
                            {
                                stockBalance.Add(
                                    new tbl_StockManager()
                                    {
                                        AgentID = stockNew.AgentID,
                                        ProductID = stockNew.ProductID,
                                        ProductVariableID = stockNew.ProductVariableID,
                                        Quantity = stockNew.Quantity,
                                        QuantityCurrent = 0,
                                        Type = 1,
                                        NoteID = "Nhập kho sản phẩm mới",
                                        OrderID = stockNew.OrderID,
                                        Status = stockNew.Status,
                                        SKU = stockNew.SKU,
                                        ParentID = stockNew.ParentID,
                                        CreatedDate = stockNew.CreatedDate.Value.AddMilliseconds(-10),
                                        CreatedBy = stockNew.CreatedBy,
                                        ModifiedDate = stockNew.CreatedDate.Value.AddMilliseconds(-10),
                                        ModifiedBy = stockNew.CreatedBy,
                                        MoveProID = stockNew.MoveProID
                                    });

                                return new tbl_StockManager()
                                {
                                    AgentID = stockNew.AgentID,
                                    ProductID = stockNew.ProductID,
                                    ProductVariableID = stockNew.ProductVariableID,
                                    Quantity = stockNew.Quantity,
                                    QuantityCurrent = stockNew.Quantity,
                                    Type = stockNew.Type,
                                    NoteID = stockNew.NoteID,
                                    OrderID = stockNew.OrderID,
                                    Status = stockNew.Status,
                                    SKU = stockNew.SKU,
                                    ParentID = stockNew.ParentID,
                                    CreatedDate = stockNew.CreatedDate,
                                    CreatedBy = stockNew.CreatedBy,
                                    ModifiedDate = stockNew.CreatedDate,
                                    ModifiedBy = stockNew.CreatedBy,
                                    MoveProID = stockNew.MoveProID
                                };
                            }
                        })
                    .ToList();

                var stockInsert = new List<tbl_StockManager>();
                stockInsert.AddRange(stockInput);
                stockInsert.AddRange(stockBalance);
                stockInsert.AddRange(stockOut);

                var index = 1;

                foreach (tbl_StockManager stock in stockInsert)
                {
                    if (index >= 100)
                    {
                        index = 1;
                        con.tbl_StockManager.Add(stock);
                        con.SaveChanges();
                    }
                    else
                    {
                        con.tbl_StockManager.Add(stock);
                    }

                    index++;
                }

                con.SaveChanges();

            }
        }

        public static string Update(tbl_StockManager stock)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_StockManager ui = dbe.tbl_StockManager.Where(a => a.ID == stock.ID).SingleOrDefault();
                if (ui != null)
                {
                    ui.AgentID = stock.AgentID;
                    ui.ProductVariableID = stock.ProductVariableID;
                    ui.Quantity = stock.Quantity;
                    ui.QuantityCurrent = stock.QuantityCurrent;
                    ui.Type = stock.Type;
                    ui.NoteID = stock.NoteID;
                    ui.OrderID = stock.OrderID;
                    ui.Status = stock.Status;
                    ui.SKU = stock.SKU;
                    ui.CreatedBy = stock.CreatedBy;
                    ui.ModifiedBy = stock.ModifiedBy;
                    ui.ModifiedDate = stock.ModifiedDate;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        public static void updateCreatedByOrderID(int OrderID, string createdBy)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                var ui = dbe.tbl_StockManager.Where(a => a.OrderID == OrderID).ToList();
                ui.ForEach(a =>
                {
                    a.CreatedBy = createdBy;
                    a.ModifiedDate = DateTime.Now;
                });

                dbe.SaveChanges();

            }
        }
        public static string deleteAll(int ProductID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_StockManager> ui = dbe.tbl_StockManager.Where(o => o.ParentID == ProductID).ToList();
                if (ui != null)
                {
                    dbe.tbl_StockManager.RemoveRange(ui);
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                return "0";
            }
        }
        #endregion
        #region Select
        public static tbl_StockManager GetByID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_StockManager ai = dbe.tbl_StockManager.Where(a => a.ID == ID).SingleOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }
        public static List<tbl_StockManager> GetAll()
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_StockManager> ags = new List<tbl_StockManager>();
                ags = dbe.tbl_StockManager.ToList();
                return ags;
            }
        }
        //public static List<tbl_StockManager> GetByProductName(int AgentID,string ProductName)
        //{
        //    using (var dbe = new inventorymanagementEntities())
        //    {
        //        List<tbl_StockManager> ags = new List<tbl_StockManager>();
        //        ags = dbe.tbl_StockManager.Where(i => i.AgentID == AgentID && i.ProductName.Contains(ProductName)).ToList();
        //        return ags;
        //    }
        //}
        public static List<tbl_StockManager> GetBySKU(int AgentID, string SKU)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_StockManager> ags = new List<tbl_StockManager>();
                ags = dbe.tbl_StockManager.Where(i => i.AgentID == AgentID && i.SKU == SKU).ToList();
                return ags;
            }
        }

        public static List<tbl_StockManager> GetBySKU(string SKU)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_StockManager> ags = new List<tbl_StockManager>();
                ags = dbe.tbl_StockManager.Where(i => i.SKU == SKU).ToList();
                return ags;
            }
        }

        public static List<tbl_StockManager> GetByProductID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_StockManager> ags = new List<tbl_StockManager>();
                ags = dbe.tbl_StockManager.Where(i => i.ProductID == ID).ToList();
                return ags;
            }
        }

        public static List<tbl_StockManager> GetByParentID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_StockManager> ags = new List<tbl_StockManager>();
                ags = dbe.tbl_StockManager.Where(i => i.ParentID == ID).ToList();
                return ags;
            }
        }

        public static List<tbl_StockManager> GetByProductVariableID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_StockManager> ags = new List<tbl_StockManager>();
                ags = dbe.tbl_StockManager.Where(i => i.ProductVariableID == ID).ToList();
                return ags;
            }
        }

        public static tbl_StockManager GetStockID(Nullable<int> productID = null, Nullable<int> productVariableID = null) {
            tbl_StockManager stock = new tbl_StockManager();
            bool exists = false;
            String sql = String.Empty;

            sql += " SELECT TOP 1 ";
            sql += "         ID ";
            sql += " ,       AgentID ";
            sql += " ,       ProductID ";
            sql += " ,       ProductVariableID ";
            sql += " ,       Quantity ";
            sql += " ,       QuantityCurrent ";
            sql += " ,       Type ";
            sql += " FROM ";
            sql += "         tbl_StockManager ";
            sql += " WHERE ";
            sql += "         1 = 1 ";

            if (productID != null)
            {
                sql += "         AND ProductID = " + productID.Value.ToString();
            }

            if (productVariableID != null)
            {
                sql += "         AND ProductVariableID = " + productVariableID.Value.ToString();
            }

            sql += " ORDER BY ";
            sql += "     CreatedDate DESC ";

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);

            while (reader.Read()) {
                exists = true;

                stock.ID = Convert.ToInt32(reader["ID"]);

                if(reader["AgentID"] != DBNull.Value) {
                    stock.AgentID = Convert.ToInt32(reader["AgentID"]);
                }

                if (reader["ProductID"] != DBNull.Value)
                {
                    stock.ProductID = Convert.ToInt32(reader["ProductID"]);
                }

                if (reader["ProductVariableID"] != DBNull.Value)
                {
                    stock.ProductVariableID = Convert.ToInt32(reader["ProductVariableID"]);
                }

                if (reader["Quantity"] != DBNull.Value)
                {
                    stock.Quantity = Convert.ToDouble(reader["Quantity"]);
                }

                if (reader["QuantityCurrent"] != DBNull.Value)
                {
                    stock.QuantityCurrent = Convert.ToDouble(reader["QuantityCurrent"]);
                }

                if (reader["Type"] != DBNull.Value)
                {
                    stock.Type = Convert.ToInt32(reader["Type"]);
                }
            }

            reader.Close();

            if (exists)
            {
                return stock;
            }
            else
            {
                return null;
            }
        }
        public static List<tbl_StockManager> GetStockAll()
        {
            using (var con = new inventorymanagementEntities())
            {

                var stockToDay = con.tbl_StockManager
                    .GroupBy(x => new { x.SKU })
                    .Select(x => new
                    {
                        SKU = x.Key.SKU,
                        CreatedDate = x.Max(row => row.CreatedDate)
                    });

                return con.tbl_StockManager
                            .Join(
                                stockToDay,
                                stock => new { stock.SKU, stock.CreatedDate },
                                stock_max => new { stock_max.SKU, stock_max.CreatedDate },
                                (stock, stock_max) => stock)
                             .ToList();
            }
        }
        public static List<tbl_StockManager> GetStockToDay()
        {
            using (var con = new inventorymanagementEntities())
            {
                var today = DateTime.Today.AddDays(-1);
                var tomorrow = today.AddDays(2);

                var stockToDay = con.tbl_StockManager
                    .Where(x => today <= x.CreatedDate && x.CreatedDate < tomorrow)
                    .GroupBy(x => new {x.SKU})
                    .Select(x => new
                    {
                        SKU = x.Key.SKU,
                        CreatedDate = x.Max(row => row.CreatedDate)
                    });

                return con.tbl_StockManager
                            .Join(
                                stockToDay,
                                stock => new { stock.SKU, stock.CreatedDate },
                                stock_max => new { stock_max.SKU, stock_max.CreatedDate },
                                (stock, stock_max) => stock)
                             .ToList();
            }
        }
        public static List<tbl_StockManager> GetStockProduct()
        {
            using (var con = new inventorymanagementEntities())
            {

                var stockAll = con.tbl_StockManager
                    .Where(x => x.ProductID != 0 && x.ProductVariableID == 0)
                    .GroupBy(x => new { x.ProductID, x.ProductVariableID })
                    .Select(x => new
                    {
                        ProductID = x.Key.ProductID,
                        ProductVariableID = x.Key.ProductVariableID,
                        CreatedDate = x.Max(row => row.CreatedDate)
                    });

                return con.tbl_StockManager
                            .Join(
                                stockAll,
                                stock => new {stock.ProductID, stock.ProductVariableID, stock.CreatedDate },
                                stock_max => new { stock_max.ProductID, stock_max.ProductVariableID, stock_max.CreatedDate },
                                (stock, stock_max) => stock)
                             .ToList();
            }
        }
        public static int getTotalProductsSold(int ID)
        {
            using (var con = new inventorymanagementEntities())
            {

                return Convert.ToInt32(con.tbl_StockManager
                    .Where(x => x.ProductID == ID && x.ProductVariableID == 0 && x.Type == 2)
                    .Sum(x => x.Quantity));
            }
        }
        public static int getTotalProductsRefund(int ID)
        {
            using (var con = new inventorymanagementEntities())
            {

                return Convert.ToInt32(con.tbl_StockManager
                    .Where(x => x.ProductID == ID && x.ProductVariableID == 0 && x.Type == 1)
                    .Sum(x => x.Quantity));
            }
        }
        public static List<tbl_StockManager> GetStockVariable()
        {
            using (var con = new inventorymanagementEntities())
            {

                var stockAll = con.tbl_StockManager
                    .Where(x => x.ProductID == 0 && x.ProductVariableID != 0)
                    .GroupBy(x => new {x.ProductID, x.ProductVariableID })
                    .Select(x => new
                    {
                        ProductID = x.Key.ProductID,
                        ProductVariableID = x.Key.ProductVariableID,
                        CreatedDate = x.Max(row => row.CreatedDate)
                    });

                return con.tbl_StockManager
                            .Join(
                                stockAll,
                                stock => new {stock.ProductID, stock.ProductVariableID, stock.CreatedDate },
                                stock_max => new { stock_max.ProductID, stock_max.ProductVariableID, stock_max.CreatedDate },
                                (stock, stock_max) => stock)
                             .ToList();
            }
        }

        public static List<tbl_StockManager> getStock(int productID, int variableID)
        {
            using (var con = new inventorymanagementEntities())
            {
                var stockLast = con.tbl_StockManager
                    .Where(x => x.ParentID == productID)
                    .Where(x => variableID == 0 || x.ProductVariableID == variableID)
                    .GroupBy(g => new { productID = g.ParentID.Value, variableID = g.ProductVariableID.Value })
                    .Select(x => 
                        new {
                            productID = x.Key.productID,
                            variableID = x.Key.variableID,
                            last = x.Max(m => m.ID)
                        }
                    );

                var result = con.tbl_StockManager
                    .Join(
                        stockLast,
                        s => new
                        {
                            productID = s.ParentID.Value,
                            variableID = s.ProductVariableID.Value,
                            last = s.ID
                        },
                        l => new
                        {
                            productID = l.productID,
                            variableID = l.variableID,
                            last = l.last
                        },
                        (s, l) => s
                    )
                    .ToList();

                return result;
            }
        }
        #endregion

        #region Lấy thông tin báo cáo nhập kho
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

        public static List<GoodsReceiptReport> getGoodsReceiptReport(ProductFilterModel filter, 
                                                                     ref PaginationMetadataModel page,
                                                                     ref int totalQuantityInput)
        {
            using (var con = new inventorymanagementEntities())
            {
                var stock = con.tbl_StockManager.Where(x => x.Status == 1);

                #region Thực thi lấy dữ liệu
                #region Loc theo từ khóa SKU
                if (!String.IsNullOrEmpty(filter.search))
                {
                    var sku = filter.search.Trim().ToLower();
                    stock = stock.Where(x => x.SKU.ToLower().Contains(sku));
                }
                #endregion

                #region Lọc sản phẩm theo ngày nhập hàng
                if (filter.fromDate.HasValue && filter.toDate.HasValue)
                {
                    stock = stock.Where(x => x.CreatedDate >= filter.fromDate && x.CreatedDate <= filter.toDate);
                }
                #endregion

                #region Lọc sản phẩm theo danh mục
                if (filter.category > 0)
                {
                    var category = con.tbl_Category.Where(x => x.ID == filter.category).FirstOrDefault();

                    if (category != null)
                    {
                        var categoryID = CategoryController.getCategoryChild(category)
                            .Select(x => x.ID)
                            .OrderBy(o => o)
                            .ToList();

                        var product = con.tbl_Product
                            .GroupJoin(
                                con.tbl_ProductVariable,
                                p => p.ID,
                                v => v.ProductID,
                                (p, v) => new { product = p, variable = v }
                            )
                            .SelectMany(
                                x => x.variable.DefaultIfEmpty(),
                                (parent, child) => new { product = parent.product, variable = child }
                            )
                            .Where(x => categoryID.Contains(x.product.CategoryID.Value))
                            .Select(x => new
                            {
                                productID = x.product.ID,
                                variableID = x.variable != null ? x.variable.ID : 0
                            });

                        stock = stock
                            .Join(
                                product,
                                s => new
                                {
                                    productID = s.ParentID.Value,
                                    variableID = s.ProductVariableID.Value
                                },
                                p => new
                                {
                                    productID = p.productID,
                                    variableID = p.variableID
                                },
                                (s, p) => s
                            );
                    }
                }
                #endregion

                #region Lọc sản phẩm theo màu
                if (!String.IsNullOrEmpty(filter.color))
                {
                    var colors = con.tbl_VariableValue
                        .Where(x => x.VariableID == 1)
                        .Where(x => x.VariableValue.ToLower().Contains(filter.color.ToLower()))
                        .Join(
                            con.tbl_ProductVariableValue,
                            vv => vv.ID,
                            pvv => pvv.VariableValueID,
                            (vv, pvv) => new { variableID = pvv.ProductVariableID }
                        );

                    stock = stock
                        .Join(
                            colors,
                            s => s.ProductVariableID,
                            c => c.variableID,
                            (s, c) => s
                        );
                }
                #endregion

                #region Lọc sản phẩm theo size
                if (!String.IsNullOrEmpty(filter.size))
                {
                    var sizes = con.tbl_VariableValue
                        .Where(x => x.VariableID == 2)
                        .Where(x => x.VariableValue.ToLower().Contains(filter.size.ToLower()))
                        .Join(
                            con.tbl_ProductVariableValue,
                            vv => vv.ID,
                            pvv => pvv.VariableValueID,
                            (vv, pvv) => new { variableID = pvv.ProductVariableID }
                        );

                    stock = stock
                        .Join(
                            sizes,
                            s => s.ProductVariableID,
                            c => c.variableID,
                            (s, c) => s
                        );
                }
                #endregion
                #endregion

                #region Tính toán phân trang
                var productFilter = stock
                    .GroupBy(x => x.ParentID)
                    .Select(x => new {
                        productID = x.Key,
                        quantityInput = x.Sum(s => s.Quantity.HasValue ? s.Quantity.Value : 0),
                        goodsReceiptDate = x.Max(m => m.CreatedDate.Value)
                    });

                if (productFilter.Count() > 0)
                {
                    totalQuantityInput = Convert.ToInt32(productFilter.Sum(x => x.quantityInput));
                }

                // Calculate pagination
                page.totalCount = productFilter.Count();
                page.totalPages = (int)Math.Ceiling(page.totalCount / (double)page.pageSize);
                productFilter = productFilter
                    .OrderByDescending(o => o.goodsReceiptDate)
                    .Skip((page.currentPage - 1) * page.pageSize)
                    .Take(page.pageSize);
                #endregion

                #region Kêt thúc: xuất ra dữ liệu
                var data = productFilter
                    .Join(
                        con.tbl_Product,
                        f => f.productID,
                        p => p.ID,
                        (f, p) => new {
                            product = p,
                            quantityInput = f.quantityInput,
                            goodsReceiptDate = f.goodsReceiptDate
                        }
                    )
                    .Join(
                        con.tbl_Category,
                        p => p.product.CategoryID,
                        c => c.ID,
                        (p, c) => new
                        {
                            categoryID = c.ID,
                            categoryName = c.CategoryName,
                            productID = p.product.ID,
                            variableID = 0,
                            sku = p.product.ProductSKU,
                            title = p.product.ProductTitle,
                            image = p.product.ProductImage,
                            quantityInput = p.quantityInput,
                            goodsReceiptDate = p.goodsReceiptDate,
                            isVariable = p.product.ProductStyle == 2 ? true : false,
                            showHomePage = p.product.ShowHomePage,
                            webPublish = p.product.WebPublish
                        }
                    )
                    .ToList();

                var result = data
                    .Select(x =>
                    {
                        var productStock = getStock(x.productID, x.variableID);
                        var quantityStock = productStock
                            .Select(y => new
                            {
                                quantityCurrent = y.QuantityCurrent.HasValue ? y.QuantityCurrent.Value : 0,
                                quantity = y.Quantity.HasValue ? y.Quantity.Value : 0,
                                type = y.Type
                            })
                            .Sum(s => s.quantityCurrent + (s.quantity * (s.type == 1 ? 1 : -1)));

                        return new GoodsReceiptReport()
                        {
                            categoryID = x.categoryID,
                            categoryName = x.categoryName,
                            productID = x.productID,
                            variableID = x.variableID,
                            sku = x.sku,
                            title = x.title,
                            image = x.image,
                            quantityInput = Convert.ToInt32(x.quantityInput),
                            quantityStock = Convert.ToInt32(quantityStock),
                            goodsReceiptDate= x.goodsReceiptDate,
                            isVariable = x.isVariable,
                            showHomePage = Convert.ToInt32(x.showHomePage),
                            webPublish = Convert.ToInt32(x.webPublish)
                        };
                    })
                    .OrderByDescending(x => x.goodsReceiptDate)
                    .ToList();
                #endregion

                return result;
            }
        }

        public static List<GoodsReceiptReport> getSubGoodsReceipt(ProductFilterModel filter, GoodsReceiptReport main)
        {
            using (var con = new inventorymanagementEntities())
            {
                var stock = con.tbl_StockManager
                    .Where(x => x.ParentID == main.productID)
                    .Where(x => x.Status == 1);

                #region Lọc sản phẩm theo ngày nhập hàng
                if (filter.fromDate.HasValue && filter.toDate.HasValue)
                {
                    stock = stock.Where(x => x.CreatedDate >= filter.fromDate && x.CreatedDate <= filter.toDate);
                }
                #endregion

                #region Kêt thúc: xuất ra dữ liệu
                var variableFilter = stock
                    .GroupBy(x => new { x.ParentID, x.ProductVariableID })
                    .Select(x => new {
                        productID = x.Key.ParentID,
                        variableID = x.Key.ProductVariableID,
                        quantityInput = x.Sum(s => s.Quantity.HasValue ? s.Quantity.Value : 0),
                        goodsReceiptDate = x.Max(m => m.CreatedDate.Value)
                    });

                var data = variableFilter
                    .Join(
                        con.tbl_ProductVariable.Where(x => x.ProductID.Value == main.productID),
                        f => new { productID = f.productID.Value, variableID = f.variableID.Value },
                        v => new { productID = v.ProductID.Value, variableID = v.ID },
                        (f, v) => new {
                            categoryID = main.categoryID,
                            categoryName = main.categoryName,
                            productID = v.ProductID.Value,
                            variableID = v.ID,
                            sku = v.SKU,
                            title = main.title,
                            image = v.Image,
                            quantityInput = f.quantityInput,
                            goodsReceiptDate = f.goodsReceiptDate,
                            isVariable = true
                        }
                    )
                    .ToList();

                var result = data
                    .Select(x =>
                    {
                        var productStock = getStock(x.productID, x.variableID);
                        var quantityStock = productStock
                            .Select(y => new
                            {
                                quantityCurrent = y.QuantityCurrent.HasValue ? y.QuantityCurrent.Value : 0,
                                quantity = y.Quantity.HasValue ? y.Quantity.Value : 0,
                                type = y.Type
                            })
                            .Sum(s => s.quantityCurrent + (s.quantity * (s.type == 1 ? 1 : -1)));

                        return new GoodsReceiptReport()
                        {
                            categoryID = x.categoryID,
                            categoryName = x.categoryName,
                            productID = x.productID,
                            variableID = x.variableID,
                            sku = x.sku,
                            title = x.title,
                            image = x.image,
                            quantityInput = Convert.ToInt32(x.quantityInput),
                            quantityStock = Convert.ToInt32(quantityStock),
                            goodsReceiptDate = x.goodsReceiptDate,
                            isVariable = x.isVariable
                        };
                    })
                    .ToList();
                #endregion

                return result;
            }
        }
        #endregion

        #region Thực hiện xả kho với các sản phẩm tồn
        public static bool liquidateProduct(tbl_Account acc, int productID)
        {
            using (var con = new inventorymanagementEntities())
            {
                var now = DateTime.Now;
                var stocks = getStock(productID, 0);
                var index = 0;
                var dataInsert = new List<tbl_StockManager>();

                foreach (var item in stocks)
                {
                    // Trường hợp bằng 1 làm nhập kho nên số lượng hiện tại là Số lượng có sản + số lượng thêm vào
                    if (item.Type == 1)
                        item.Quantity = item.QuantityCurrent + item.Quantity;
                    else
                        item.Quantity = item.QuantityCurrent - item.Quantity;

                    item.QuantityCurrent = item.Quantity;
                    // Xã hàng ra khỏi kho
                    item.Type = 2;
                    item.Status = 14;
                    item.NoteID = "Xuất hết kho";
                    item.CreatedBy = acc.Username;
                    item.CreatedDate = now;
                    item.ModifiedBy = acc.Username;
                    item.ModifiedDate = now;

                    index += 1;
                    dataInsert.Add(item);

                    if (dataInsert.Count >= 100)
                    {
                        try
                        {
                            con.tbl_StockManager.AddRange(dataInsert);
                            con.SaveChanges();
                            // Sau khi commit xong clear
                            dataInsert.Clear();
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                }

                if (dataInsert.Count >= 0)
                {
                    try
                    {
                        con.tbl_StockManager.AddRange(dataInsert);
                        con.SaveChanges();
                        // Sau khi commit xong clear
                        dataInsert.Clear();
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        #endregion

        #region Phục hồi lại các sản phẩm đã xã kho
        public static bool recoverLiquidatedProduct(tbl_Account acc, int productID)
        {
            using (var con = new inventorymanagementEntities())
            {
                var now = DateTime.Now;
                // Lấy dòng mới nhất của sản phẩn trong stock ra
                // Nếu mà không phải là status 14 thì không thực thi phục hồi lại với sản phẩm đó
                var stocks = getStock(productID, 0).Where(x => x.Status == 14);
                var index = 0;
                var dataInsert = new List<tbl_StockManager>();

                if (stocks.Count() == 0)
                    throw new Exception("Sản phẩn không thể phục hồi xả hàng.");

                foreach (var item in stocks)
                {
                    item.Type = 1;
                    item.QuantityCurrent = 0;
                    item.Status = 15;
                    item.NoteID = "Phục hồi xuất hết kho";
                    item.CreatedBy = acc.Username;
                    item.CreatedDate = now;
                    item.ModifiedBy = acc.Username;
                    item.ModifiedDate = now;

                    index += 1;
                    dataInsert.Add(item);

                    if (dataInsert.Count >= 100)
                    {
                        try
                        {
                            con.tbl_StockManager.AddRange(dataInsert);
                            con.SaveChanges();
                            // Sau khi commit xong clear
                            dataInsert.Clear();
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                }

                if (dataInsert.Count >= 0)
                {
                    try
                    {
                        con.tbl_StockManager.AddRange(dataInsert);
                        con.SaveChanges();
                        // Sau khi commit xong clear
                        dataInsert.Clear();
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        #endregion


        public static tbl_StockManager warehousing1(tbl_StockManager data)
        {
            using (var con = new inventorymanagementEntities())
            {
                con.tbl_StockManager.Add(data);
                con.SaveChanges();

                return data;
            }
        }

        public static tbl_StockManager warehousing1(StockManager2 data)
        {
            using (var con = new inventorymanagementEntities())
            {
                var stock1 = new tbl_StockManager()
                {
                    AgentID = data.AgentID,
                    ProductID = data.ProductVariableID == 0 ? data.ProductID : 0,
                    ProductVariableID = data.ProductVariableID,
                    Quantity = data.Quantity,
                    QuantityCurrent = data.QuantityCurrent,
                    Type = data.Type,
                    CreatedDate = data.CreatedDate,
                    CreatedBy = data.CreatedBy,
                    ModifiedDate = data.ModifiedDate,
                    ModifiedBy = data.ModifiedBy,
                    NoteID = data.Note,
                    OrderID = 0,
                    Status = data.Status,
                    SKU = data.SKU,
                    MoveProID = 0,
                    ParentID = data.ProductID
                };
                con.tbl_StockManager.Add(stock1);
                con.SaveChanges();

                return stock1;
            }
        }

        public static List<tbl_StockManager> warehousing1ByParentSKU(string parentSKU)
        {
            using (var con = new inventorymanagementEntities())
            {
                var stock = con.tbl_Product.Where(x => x.ProductSKU == parentSKU)
                    .Join(
                        con.tbl_StockManager,
                        p => p.ID,
                        s => s.ParentID,
                        (p, s) => s
                    )
                    .ToList();

                return stock;
            }
        }

        #region Lấy thông tin chuyển kho
        public static List<StockTransferReport> getStock1TransferReport(ProductFilterModel filter,
                                                                        ref PaginationMetadataModel page,
                                                                        ref int totalQuantityInput)
        {
            using (var con = new inventorymanagementEntities())
            {
                // 20: Nhận hàng từ kho khác bằng chức năng chuyển kho
                var stockTransfer = con.tbl_StockManager.Where(x => x.Status == 20);

                #region Thực thi lấy dữ liệu
                #region Lọc sản phẩm theo ngày nhập hàng
                if (filter.fromDate.HasValue && filter.toDate.HasValue)
                {
                    stockTransfer = stockTransfer.Where(x => x.CreatedDate >= filter.fromDate && x.CreatedDate <= filter.toDate);
                }
                #endregion

                #region Loc theo từ khóa SKU
                if (!String.IsNullOrEmpty(filter.search))
                {
                    var sku = filter.search.Trim().ToLower();
                    stockTransfer = stockTransfer.Where(x => x.SKU.ToLower().Contains(sku));
                }
                #endregion

                #region Lọc sản phẩm theo danh mục
                if (filter.category > 0)
                {
                    var category = con.tbl_Category.Where(x => x.ID == filter.category).FirstOrDefault();

                    if (category != null)
                    {
                        var categoryID = CategoryController.getCategoryChild(category)
                            .Select(x => x.ID)
                            .OrderBy(o => o)
                            .ToList();

                        var product = con.tbl_Product
                            .GroupJoin(
                                con.tbl_ProductVariable,
                                p => p.ID,
                                v => v.ProductID,
                                (p, v) => new { product = p, variable = v }
                            )
                            .SelectMany(
                                x => x.variable.DefaultIfEmpty(),
                                (parent, child) => new { product = parent.product, variable = child }
                            )
                            .Where(x => categoryID.Contains(x.product.CategoryID.Value))
                            .Select(x => new
                            {
                                productID = x.product.ID,
                                variableID = x.variable != null ? x.variable.ID : 0
                            });

                        stockTransfer = stockTransfer
                            .Join(
                                product,
                                s => new
                                {
                                    productID = s.ParentID.Value,
                                    variableID = s.ProductVariableID.Value
                                },
                                p => new
                                {
                                    productID = p.productID,
                                    variableID = p.variableID
                                },
                                (s, p) => s
                            );
                    }
                }
                #endregion
                #endregion

                #region Tính toán phân trang
                var transferFilter = stockTransfer
                    .GroupBy(x => new { productID = x.ParentID, transferDate = x.CreatedDate.Value })
                    .Select(x => new {
                        productID = x.Key.productID,
                        quantityTransfer = x.Sum(s => s.Quantity.HasValue ? s.Quantity.Value : 0),
                        transferDate = x.Key.transferDate
                    });

                if (transferFilter.Count() > 0)
                    totalQuantityInput = Convert.ToInt32(transferFilter.Sum(x => x.quantityTransfer));

                // Calculate pagination
                page.totalCount = transferFilter.Count();
                page.totalPages = (int)Math.Ceiling(page.totalCount / (double)page.pageSize);
                transferFilter = transferFilter
                    .OrderByDescending(o => o.transferDate)
                    .Skip((page.currentPage - 1) * page.pageSize)
                    .Take(page.pageSize);
                #endregion

                #region Kêt thúc: xuất ra dữ liệu
                var productFilter = transferFilter.Select(x => new { productID = x.productID }).Distinct();

                #region Lấy dữ liệu để tính số lượng hiện tại của sản phẩm trong kho
                var stock = con.tbl_StockManager
                    .Join(
                        productFilter,
                        s => s.ParentID,
                        f => f.productID,
                        (s, f) => s
                    );

                var stockLast = stock
                    .Join(
                        productFilter,
                        s => s.ParentID,
                        f => f.productID,
                        (s, f) => s
                    )
                    .GroupBy(g => new { productID = g.ParentID, productVariationID = g.ProductVariableID })
                    .Select(x => new {
                        productID = x.Key.productID,
                        productVariationID = x.Key.productVariationID,
                        lastDate = x.Max(m => m.CreatedDate.Value)
                    });

                stock = stock
                    .Join(
                        stockLast,
                        s => new
                        {
                            productID = s.ParentID,
                            productVariationID = s.ProductVariableID,
                            lastDate = s.CreatedDate.Value
                        },
                        l => new
                        {
                            productID = l.productID,
                            productVariationID = l.productVariationID,
                            lastDate = l.lastDate
                        },
                        (s, l) => s
                    );

                var productStock = stock
                    .Select(x => new {
                        productID = x.ParentID,
                        productVariationID = x.ProductVariableID,
                        quantityAvailable =
                                    (x.QuantityCurrent.HasValue ? x.QuantityCurrent.Value : 0) +
                                    (x.Quantity.HasValue ? x.Quantity.Value : 0) * ((x.Type.HasValue ? x.Type.Value : 1) == 1 ? 1 : -1)
                    })
                    .GroupBy(g => g.productID)
                    .Select(x => new {
                        productID = x.Key,
                        quantityAvailable = x.Sum(s => s.quantityAvailable)
                    });
                var productVariationStock = stock
                    .Select(x => new {
                        productID = x.ParentID,
                        productVariationID = x.ProductVariableID,
                        quantityAvailable =
                                    (x.QuantityCurrent.HasValue ? x.QuantityCurrent.Value : 0) +
                                    (x.Quantity.HasValue ? x.Quantity.Value : 0) * ((x.Type.HasValue ? x.Type.Value : 1) == 1 ? 1 : -1)
                    });
                #endregion

                var data = transferFilter
                    .Join(
                        con.tbl_Product,
                        f => f.productID,
                        p => p.ID,
                        (f, p) => new {
                            product = p,
                            quantityTransfer = f.quantityTransfer,
                            transferDate = f.transferDate
                        }
                    )
                    .Join(
                        con.tbl_Category,
                        temp => temp.product.CategoryID,
                        c => c.ID,
                        (temp, c) => new
                        {
                            category = c,
                            product = temp.product,
                            quantityTransfer = temp.quantityTransfer,
                            transferDate = temp.transferDate
                        }
                    )
                    .Join(
                        productStock,
                        temp2 => temp2.product.ID,
                        s => s.productID,
                        (temp2, s) => new
                        {
                            category = temp2.category,
                            product = temp2.product,
                            quantityTransfer = temp2.quantityTransfer,
                            quantityAvailable = s.quantityAvailable,
                            transferDate = temp2.transferDate
                        }
                    )
                    .Select(x => new
                        {
                            categoryID = x.category.ID,
                            categoryName = x.category.CategoryName,
                            productID = x.product.ID,
                            variableID = 0,
                            sku = x.product.ProductSKU,
                            title = x.product.ProductTitle,
                            image = x.product.ProductImage,
                            quantityTransfer = x.quantityTransfer,
                            quantityAvailable = x.quantityAvailable,
                            transferDate = x.transferDate,
                            isVariable = x.product.ProductStyle == 2 ? true : false
                        }
                    );
                var subData = transferFilter
                    .Join(
                        con.tbl_ProductVariable,
                        f => f.productID,
                        p => p.ProductID,
                        (f, p) => new
                        {
                            productID = p.ProductID.HasValue ? p.ProductID.Value : 0,
                            productVariationID = p.ID,
                            sku = p.SKU,
                            image = p.Image,
                            quantityTransfer = 0D,
                            quantityAvailable = 0D,
                            transferDate = f.transferDate,
                        }
                    ).ToList();
                
                // Lấy thông tin số lượng nhận sản phẩm
                subData = subData
                    .Join(
                        stockTransfer.ToList(),
                        p => new {
                            productID = p.productID,
                            productVariationID = p.productVariationID,
                            transferDate = p.transferDate
                        },
                        s => new {
                            productID = s.ParentID.HasValue ? s.ParentID.Value : 0,
                            productVariationID = s.ProductVariableID.HasValue ? s.ProductVariableID.Value : 0,
                            transferDate = s.CreatedDate.HasValue ? s.CreatedDate.Value : DateTime.Now
                        },
                        (p, s) => new
                        {
                            productID = p.productID,
                            productVariationID = p.productVariationID,
                            sku = p.sku,
                            image = p.image,
                            quantityTransfer = s.Quantity.HasValue ? s.Quantity.Value : 0D,
                            quantityAvailable = 0D,
                            transferDate = p.transferDate
                        }
                    )
                    .ToList();
                
                // Lấy thông tin số lượng hiện tại của sản phẩm
                subData = subData
                    .Join(
                        productVariationStock.ToList(),
                        p => new {
                            productID = p.productID,
                            productVariationID = p.productVariationID
                        },
                        s => new {
                            productID = s.productID.HasValue ? s.productID.Value : 0,
                            productVariationID = s.productVariationID.HasValue ? s.productVariationID.Value : 0
                        },
                        (p, s) => new
                        {
                            productID = p.productID,
                            productVariationID = p.productVariationID,
                            sku = p.sku,
                            image = p.image,
                            quantityTransfer = p.quantityTransfer,
                            quantityAvailable = s.quantityAvailable,
                            transferDate = p.transferDate
                        }
                    )
                    .ToList();

                var result = data.ToList()
                    .Select(x =>
                    {
                        var children = subData
                            .Where(y => y.productID == x.productID)
                            .Where(y => y.transferDate == x.transferDate)
                            .Select(y => new SubStockTransferReport()
                            {
                                sku = y.sku,
                                image = y.image,
                                quantityTransfer = Convert.ToInt32(y.quantityTransfer),
                                quantityAvailable = Convert.ToInt32(y.quantityAvailable),
                                transferDate = y.transferDate
                            })
                            .ToList();

                        return new StockTransferReport()
                        {
                            categoryID = x.categoryID,
                            categoryName = x.categoryName,
                            productID = x.productID,
                            variableID = x.variableID,
                            sku = x.sku,
                            title = x.title,
                            image = x.image,
                            quantityTransfer = Convert.ToInt32(x.quantityTransfer),
                            quantityAvailable = Convert.ToInt32(x.quantityAvailable),
                            transferDate = x.transferDate,
                            isVariable = x.isVariable,
                            children = children
                        };
                    })
                    .OrderByDescending(x => x.transferDate)
                    .ToList();
                #endregion

                return result;
            }
        }
        #endregion
        #endregion

        #region Quản lý kho 2
        public static int? getQuantityStock2BySKU(string sku)
        {
            using (var con = new inventorymanagementEntities())
            {
                var stock2 = con.StockManager2
                            .Where(x => x.SKU == sku)
                            .OrderByDescending(o => o.ID)
                            .FirstOrDefault();

                if (stock2 == null)
                    return null;
                else
                    return stock2.QuantityCurrent + stock2.Quantity * (stock2.Type == 1 ? 1 : -1);
            }
        }

        public static StockManager2 warehousing2(StockManager2 data)
        {
            using (var con = new inventorymanagementEntities())
            {
                con.StockManager2.Add(data);
                con.SaveChanges();

                return data;
            }
        }

        #region Lấy thông tin chuyển kho
        public static List<StockTransferReport> getStock2TransferReport(ProductFilterModel filter,
                                                                        ref PaginationMetadataModel page,
                                                                        ref int totalQuantityInput)
        {
            using (var con = new inventorymanagementEntities())
            {
                // 20: Nhận hàng từ kho khác bằng chức năng chuyển kho
                var stockTransfer = con.StockManager2.Where(x => x.Status == 20);

                #region Thực thi lấy dữ liệu
                #region Lọc sản phẩm theo ngày nhập hàng
                if (filter.fromDate.HasValue && filter.toDate.HasValue)
                {
                    stockTransfer = stockTransfer.Where(x => x.CreatedDate >= filter.fromDate && x.CreatedDate <= filter.toDate);
                }
                #endregion

                #region Loc theo từ khóa SKU
                if (!String.IsNullOrEmpty(filter.search))
                {
                    var sku = filter.search.Trim().ToLower();
                    stockTransfer = stockTransfer.Where(x => x.SKU.ToLower().Contains(sku));
                }
                #endregion

                #region Lọc sản phẩm theo danh mục
                if (filter.category > 0)
                {
                    var category = con.tbl_Category.Where(x => x.ID == filter.category).FirstOrDefault();

                    if (category != null)
                    {
                        var categoryID = CategoryController.getCategoryChild(category)
                            .Select(x => x.ID)
                            .OrderBy(o => o)
                            .ToList();

                        var product = con.tbl_Product
                            .GroupJoin(
                                con.tbl_ProductVariable,
                                p => p.ID,
                                v => v.ProductID,
                                (p, v) => new { product = p, variable = v }
                            )
                            .SelectMany(
                                x => x.variable.DefaultIfEmpty(),
                                (parent, child) => new { product = parent.product, variable = child }
                            )
                            .Where(x => categoryID.Contains(x.product.CategoryID.Value))
                            .Select(x => new
                            {
                                productID = x.product.ID,
                                variableID = x.variable != null ? x.variable.ID : 0
                            });

                        stockTransfer = stockTransfer
                            .Join(
                                product,
                                s => new
                                {
                                    productID = s.ProductID,
                                    variableID = s.ProductVariableID.Value
                                },
                                p => new
                                {
                                    productID = p.productID,
                                    variableID = p.variableID
                                },
                                (s, p) => s
                            );
                    }
                }
                #endregion
                #endregion

                #region Tính toán phân trang
                var transferFilter = stockTransfer
                    .GroupBy(x => new { productID = x.ProductID, transferDate = x.CreatedDate })
                    .Select(x => new {
                        productID = x.Key.productID,
                        quantityTransfer = x.Sum(s => s.Quantity),
                        transferDate = x.Key.transferDate
                    });

                if (transferFilter.Count() > 0)
                    totalQuantityInput = Convert.ToInt32(transferFilter.Sum(x => x.quantityTransfer));

                // Calculate pagination
                page.totalCount = transferFilter.Count();
                page.totalPages = (int)Math.Ceiling(page.totalCount / (double)page.pageSize);
                transferFilter = transferFilter
                    .OrderByDescending(o => o.transferDate)
                    .Skip((page.currentPage - 1) * page.pageSize)
                    .Take(page.pageSize);
                #endregion

                #region Kêt thúc: xuất ra dữ liệu
                var productFilter = transferFilter.Select(x => new { productID = x.productID }).Distinct();

                #region Lấy dữ liệu để tính số lượng hiện tại của sản phẩm trong kho
                var stock = con.StockManager2
                    .Join(
                        productFilter,
                        s => s.ProductID,
                        f => f.productID,
                        (s, f) => s
                    );

                var stockLast = stock
                    .Join(
                        productFilter,
                        s => s.ProductID,
                        f => f.productID,
                        (s, f) => s
                    )
                    .GroupBy(g => new { productID = g.ProductID, productVariationID = g.ProductVariableID })
                    .Select(x => new {
                        productID = x.Key.productID,
                        productVariationID = x.Key.productVariationID,
                        lastDate = x.Max(m => m.CreatedDate)
                    });

                stock = stock
                    .Join(
                        stockLast,
                        s => new
                        {
                            productID = s.ProductID,
                            productVariationID = s.ProductVariableID,
                            lastDate = s.CreatedDate
                        },
                        l => new
                        {
                            productID = l.productID,
                            productVariationID = l.productVariationID,
                            lastDate = l.lastDate
                        },
                        (s, l) => s
                    );

                var productStock = stock
                    .Select(x => new {
                        productID = x.ProductID,
                        productVariationID = x.ProductVariableID,
                        quantityAvailable = x.QuantityCurrent + x.Quantity * (x.Type == 1 ? 1 : -1)
                    })
                    .GroupBy(g => g.productID)
                    .Select(x => new {
                        productID = x.Key,
                        quantityAvailable = x.Sum(s => s.quantityAvailable)
                    });
                var productVariationStock = stock
                    .Select(x => new {
                        productID = x.ProductID,
                        productVariationID = x.ProductVariableID,
                        quantityAvailable = x.QuantityCurrent + x.Quantity * (x.Type == 1 ? 1 : -1)
                    });
                #endregion

                var data = transferFilter
                    .Join(
                        con.tbl_Product,
                        f => f.productID,
                        p => p.ID,
                        (f, p) => new {
                            product = p,
                            quantityTransfer = f.quantityTransfer,
                            transferDate = f.transferDate
                        }
                    )
                    .Join(
                        con.tbl_Category,
                        temp => temp.product.CategoryID,
                        c => c.ID,
                        (temp, c) => new
                        {
                            category = c,
                            product = temp.product,
                            quantityTransfer = temp.quantityTransfer,
                            transferDate = temp.transferDate
                        }
                    )
                    .Join(
                        productStock,
                        temp2 => temp2.product.ID,
                        s => s.productID,
                        (temp2, s) => new
                        {
                            category = temp2.category,
                            product = temp2.product,
                            quantityTransfer = temp2.quantityTransfer,
                            quantityAvailable = s.quantityAvailable,
                            transferDate = temp2.transferDate
                        }
                    )
                    .Select(x => new
                    {
                        categoryID = x.category.ID,
                        categoryName = x.category.CategoryName,
                        productID = x.product.ID,
                        variableID = 0,
                        sku = x.product.ProductSKU,
                        title = x.product.ProductTitle,
                        image = x.product.ProductImage,
                        quantityTransfer = x.quantityTransfer,
                        quantityAvailable = x.quantityAvailable,
                        transferDate = x.transferDate,
                        isVariable = x.product.ProductStyle == 2 ? true : false
                    }
                    );
                var subData = transferFilter
                    .Join(
                        con.tbl_ProductVariable,
                        f => f.productID,
                        p => p.ProductID,
                        (f, p) => new
                        {
                            productID = p.ProductID.HasValue ? p.ProductID.Value : 0,
                            productVariationID = p.ID,
                            sku = p.SKU,
                            image = p.Image,
                            quantityTransfer = 0,
                            quantityAvailable = 0,
                            transferDate = f.transferDate,
                        }
                    ).ToList();
                
                // Lấy thông tin số lượng nhận sản phẩm
                subData = subData
                    .Join(
                        stockTransfer.ToList(),
                        p => new {
                            productID = p.productID,
                            productVariationID = p.productVariationID,
                            transferDate = p.transferDate
                        },
                        s => new {
                            productID = s.ProductID,
                            productVariationID = s.ProductVariableID.HasValue ? s.ProductVariableID.Value : 0,
                            transferDate = s.CreatedDate
                        },
                        (p, s) => new
                        {
                            productID = p.productID,
                            productVariationID = p.productVariationID,
                            sku = p.sku,
                            image = p.image,
                            quantityTransfer = s.Quantity,
                            quantityAvailable = 0,
                            transferDate = p.transferDate
                        }
                    )
                    .ToList();
                
                // Lấy thông tin số lượng hiện tại của sản phẩm
                subData = subData
                    .Join(
                        productVariationStock.ToList(),
                        p => new {
                            productID = p.productID,
                            productVariationID = p.productVariationID
                        },
                        s => new {
                            productID = s.productID,
                            productVariationID = s.productVariationID.HasValue ? s.productVariationID.Value : 0
                        },
                        (p, s) => new
                        {
                            productID = p.productID,
                            productVariationID = p.productVariationID,
                            sku = p.sku,
                            image = p.image,
                            quantityTransfer = p.quantityTransfer,
                            quantityAvailable = s.quantityAvailable,
                            transferDate = p.transferDate
                        }
                    )
                    .ToList();

                var result = data.ToList()
                    .Select(x =>
                    {
                        var children = subData
                            .Where(y => y.productID == x.productID)
                            .Where(y => y.transferDate == x.transferDate)
                            .Select(y => new SubStockTransferReport()
                            {
                                sku = y.sku,
                                image = y.image,
                                quantityTransfer = Convert.ToInt32(y.quantityTransfer),
                                quantityAvailable = Convert.ToInt32(y.quantityAvailable),
                                transferDate = y.transferDate
                            })
                            .ToList();

                        return new StockTransferReport()
                        {
                            categoryID = x.categoryID,
                            categoryName = x.categoryName,
                            productID = x.productID,
                            variableID = x.variableID,
                            sku = x.sku,
                            title = x.title,
                            image = x.image,
                            quantityTransfer = Convert.ToInt32(x.quantityTransfer),
                            quantityAvailable = Convert.ToInt32(x.quantityAvailable),
                            transferDate = x.transferDate,
                            isVariable = x.isVariable,
                            children = children
                        };
                    })
                    .OrderByDescending(x => x.transferDate)
                    .ToList();
                #endregion

                return result;
            }
        }
        #endregion
        #endregion
    }
}