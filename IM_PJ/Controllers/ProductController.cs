﻿using IM_PJ.Models;
using MB.Extensions;
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
    public class ProductController
    {
        static string checkSlug(string slug)
        {
            using (var con = new inventorymanagementEntities())
            {
                var product = con.tbl_Product.Where(x => x.Slug == slug).FirstOrDefault();
                if (product != null)
                {
                    return checkSlug(slug + "-1");
                }
                else
                {
                    return slug;
                }
            }
        }

        #region CRUD
        public static string Insert(int CategoryID, int ProductOldID, string ProductTitle, string ProductContent, string ProductSKU, double ProductStock,
            int StockStatus, bool ManageStock, double Regular_Price, double CostOfGood, double Retail_Price, string ProductImage, int ProductType,
            bool IsHidden, DateTime CreatedDate, string CreatedBy, int SupplierID, string SupplierName, string Materials,
            double MinimumInventoryLevel, double MaximumInventoryLevel, int ProductStyle, int ShowHomePage, string mainColor)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Product ui = new tbl_Product();
                ui.CategoryID = CategoryID;
                ui.ProductOldID = ProductOldID;
                ui.ProductTitle = ProductTitle;
                ui.ProductContent = ProductContent;
                ui.ProductSKU = ProductSKU;
                ui.ProductStock = ProductStock;
                ui.StockStatus = StockStatus;
                ui.ManageStock = ManageStock;
                ui.Regular_Price = Regular_Price;
                ui.CostOfGood = CostOfGood;
                ui.Retail_Price = Retail_Price;
                ui.ProductImage = ProductImage;
                ui.ProductType = ProductType;
                ui.IsHidden = IsHidden;
                ui.CreatedDate = CreatedDate;
                ui.CreatedBy = CreatedBy;
                ui.SupplierID = SupplierID;
                ui.SupplierName = SupplierName;
                ui.Materials = Materials;
                ui.MinimumInventoryLevel = MinimumInventoryLevel;
                ui.MaximumInventoryLevel = MaximumInventoryLevel;
                ui.ProductStyle = ProductStyle;
                ui.ShowHomePage = ShowHomePage;
                ui.WebPublish = true;
                ui.WebUpdate = CreatedDate;
                ui.UnSignedTitle = UnSign.convert(ProductTitle);
                ui.Slug = checkSlug(Slug.ConvertToSlug(ProductTitle));
                ui.Color = mainColor;

                dbe.tbl_Product.Add(ui);
                dbe.SaveChanges();
                int kq = ui.ID;
                return kq.ToString();
            }
        }
        public static string Update(int ID, int CategoryID, int ProductOldID, string ProductTitle, string ProductContent, string ProductSKU, double ProductStock,
            int StockStatus, bool ManageStock, double Regular_Price, double CostOfGood, double Retail_Price, string ProductImage, int ProductType,
            bool IsHidden, DateTime ModifiedDate, string ModifiedBy, int SupplierID, string SupplierName, string Materials,
            double MinimumInventoryLevel, double MaximumInventoryLevel, string ProductImageClean, string mainColor)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Product ui = dbe.tbl_Product.Where(a => a.ID == ID).SingleOrDefault();
                if (ui != null)
                {
                    ui.CategoryID = CategoryID;
                    ui.ProductOldID = ProductOldID;
                    ui.ProductTitle = ProductTitle;
                    ui.ProductContent = ProductContent;
                    ui.ProductSKU = ProductSKU;
                    if (ProductStock > 0)
                        ui.ProductStock = ProductStock;
                    if (StockStatus > 0)
                        ui.StockStatus = StockStatus;
                    ui.ManageStock = ManageStock;
                    ui.Regular_Price = Regular_Price;
                    ui.CostOfGood = CostOfGood;
                    ui.Retail_Price = Retail_Price;
                    ui.ProductImage = ProductImage;
                    ui.ProductType = ProductType;
                    ui.IsHidden = IsHidden;
                    ui.ModifiedBy = ModifiedBy;
                    ui.ModifiedDate = ModifiedDate;
                    ui.SupplierID = SupplierID;
                    ui.SupplierName = SupplierName;
                    ui.Materials = Materials;
                    ui.MinimumInventoryLevel = MinimumInventoryLevel;
                    ui.MaximumInventoryLevel = MaximumInventoryLevel;
                    ui.ProductImageClean = ProductImageClean;
                    ui.UnSignedTitle = UnSign.convert(ProductTitle);
                    ui.Color = mainColor;

                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        public static string updateSKU(int ID, string newSKU)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Product ui = dbe.tbl_Product.Where(a => a.ID == ID).SingleOrDefault();
                if (ui != null)
                {
                    ui.ProductSKU = newSKU;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        public static string UpdateImage(int ID, string ProductImage)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Product ui = dbe.tbl_Product.Where(a => a.ID == ID).SingleOrDefault();
                if (ui != null)
                {
                    ui.ProductImage = ProductImage;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }

        public static string UpdateImageClean(int ID, string ProductImageClean)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Product ui = dbe.tbl_Product.Where(a => a.ID == ID).SingleOrDefault();
                if (ui != null)
                {
                    ui.ProductImageClean = ProductImageClean;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }

        public static string UpdateStockStatus(string SKU, int StockStatus, bool IsHidden, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Product ui = dbe.tbl_Product.Where(a => a.ProductSKU == SKU).SingleOrDefault();
                if (ui != null)
                {

                    ui.StockStatus = StockStatus;
                    ui.IsHidden = IsHidden;
                    ui.ModifiedBy = ModifiedBy;
                    ui.ModifiedDate = ModifiedDate;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        public static string updateShowHomePage(int id, int value)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Product ui = dbe.tbl_Product.Where(a => a.ID == id).SingleOrDefault();
                if (ui != null)
                {
                    ui.ShowHomePage = value;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        public static string updateWebPublish(int id, bool value)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Product ui = dbe.tbl_Product.Where(a => a.ID == id).SingleOrDefault();
                if (ui != null)
                {
                    ui.WebPublish = value;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        public static string updateWebUpdate(int id)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Product ui = dbe.tbl_Product.Where(a => a.ID == id).SingleOrDefault();
                if (ui != null)
                {
                    ui.WebUpdate = DateTime.Now;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        public static string deleteProduct(int id)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Product ui = dbe.tbl_Product.Where(a => a.ID == id).SingleOrDefault();
                if (ui != null)
                {
                    dbe.tbl_Product.Remove(ui);
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        #endregion
        #region Select
        public static List<tbl_Product> GetByTextSearchIsHidden(string s, bool IsHidden)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_Product> ags = new List<tbl_Product>();
                ags = dbe.tbl_Product.Where(p => p.ProductTitle.Contains(s) && p.IsHidden == IsHidden).OrderByDescending(o => o.ID).ToList();
                return ags;
            }
        }
        public static tbl_Product GetByID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Product ai = dbe.tbl_Product.Where(a => a.ID == ID).SingleOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }
        public static tbl_Product GetByVariableID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_ProductVariable variable = dbe.tbl_ProductVariable.Where(a => a.ID == ID).SingleOrDefault();
                tbl_Product ai = dbe.tbl_Product.Where(a => a.ID == variable.ProductID).SingleOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }
        public static tbl_Product GetByVariableSKU(string SKU)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_ProductVariable variable = dbe.tbl_ProductVariable.Where(a => a.SKU == SKU).SingleOrDefault();
                tbl_Product ai = dbe.tbl_Product.Where(a => a.ID == variable.ProductID).SingleOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }
        public static tbl_Product GetBySKU(string SKU)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Product ai = dbe.tbl_Product.Where(a => a.ProductSKU == SKU).SingleOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }
        public static List<tbl_Product> GetAll(string s)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_Product> ags = new List<tbl_Product>();
                ags = dbe.tbl_Product.Where(p => p.ProductTitle.Contains(s)).OrderByDescending(o => o.ID).ToList();
                return ags;
            }
        }
        public static List<tbl_Product> GetByCategoryID(int CategoryID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_Product> ags = new List<tbl_Product>();
                ags = dbe.tbl_Product.Where(p => p.CategoryID == CategoryID).OrderByDescending(o => o.ID).ToList();
                return ags;
            }
        }
        
        public static List<ProductSQL> GetProductReport(int categoryID)
        {
            var list = new List<ProductSQL>();
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("BEGIN");

            if (categoryID > 0)
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
                sql.AppendLine("    AND     ID = " + categoryID);
                sql.AppendLine(")");
                sql.AppendLine("SELECT");
                sql.AppendLine("        ID");
                sql.AppendLine(",       CategoryName");
                sql.AppendLine(",       ParentID");
                sql.AppendLine("INTO #category");
                sql.AppendLine("FROM category");
            }

            sql.AppendLine(String.Empty);
            sql.AppendLine("    SELECT");
            sql.AppendLine("            PRD.*");
            sql.AppendLine("    INTO #Product");
            sql.AppendLine("    FROM");
            sql.AppendLine("            tbl_Product AS PRD");
            sql.AppendLine("    WHERE");
            sql.AppendLine("            1 = 1");

            if (categoryID > 0)
            {
                sql.AppendLine("    AND EXISTS(");
                sql.AppendLine("            SELECT");
                sql.AppendLine("                    NULL AS DUMMY");
                sql.AppendLine("            FROM");
                sql.AppendLine("                    #category");
                sql.AppendLine("            WHERE");
                sql.AppendLine("                    ID = PRD.CategoryID");
                sql.AppendLine("    )");
            }

            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             PRD.ProductStyle");
            sql.AppendLine("     ,       PRD.ID");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #Product([ProductStyle], [ID])");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             STM.ProductID");
            sql.AppendLine("     ,       STM.Quantity");
            sql.AppendLine("     ,       STM.QuantityCurrent");
            sql.AppendLine("     ,       STM.Type");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ,       STM.ParentID");
            sql.AppendLine("     INTO #StockProduct");
            sql.AppendLine("     FROM");
            sql.AppendLine("             #Product AS PRD");
            sql.AppendLine("     INNER JOIN tbl_StockManager AS STM");
            sql.AppendLine("         ON  PRD.ProductStyle = 1");
            sql.AppendLine("         AND PRD.ID = STM.ParentID");
            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             STM.ProductID");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #StockProduct(");
            sql.AppendLine("             [ProductID] ASC");
            sql.AppendLine("     ,       [CreatedDate] DESC");
            sql.AppendLine("     )");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             STM.ProductVariableID");
            sql.AppendLine("     ,       STM.Quantity");
            sql.AppendLine("     ,       STM.QuantityCurrent");
            sql.AppendLine("     ,       STM.Type");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ,       STM.ParentID");
            sql.AppendLine("     INTO #StockProductVariable");
            sql.AppendLine("     FROM");
            sql.AppendLine("             #Product AS PRD");
            sql.AppendLine("     INNER JOIN tbl_StockManager AS STM");
            sql.AppendLine("         ON  PRD.ProductStyle = 2");
            sql.AppendLine("         AND PRD.ID = STM.ParentID");
            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             STM.ProductVariableID");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #StockProductVariable(");
            sql.AppendLine("             [ProductVariableID] ASC");
            sql.AppendLine("     ,       [CreatedDate] DESC");
            sql.AppendLine("     )");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             PRQ.ProductStyle");
            sql.AppendLine("     ,       PRQ.ParentID");
            sql.AppendLine("     ,       SUM(ISNULL(PRQ.QuantityLeft, 0)) AS QuantityLeft");
            sql.AppendLine("     INTO #ProductQuantity");
            sql.AppendLine("     FROM (");
            sql.AppendLine("         SELECT");
            sql.AppendLine("                 1 AS ProductStyle");
            sql.AppendLine("         ,       STP.ParentID");
            sql.AppendLine("         ,       (");
            sql.AppendLine("                     CASE STP.Type");
            sql.AppendLine("                         WHEN 1");
            sql.AppendLine("                             THEN STP.QuantityCurrent + STP.Quantity");
            sql.AppendLine("                         WHEN 2");
            sql.AppendLine("                             THEN STP.QuantityCurrent - STP.Quantity");
            sql.AppendLine("                         ELSE");
            sql.AppendLine("                             0");
            sql.AppendLine("                     END");
            sql.AppendLine("                 ) AS QuantityLeft");
            sql.AppendLine("         FROM ");
            sql.AppendLine("                 #StockProduct AS STP");
            sql.AppendLine("         INNER JOIN (");
            sql.AppendLine("                 SELECT");
            sql.AppendLine("                         ProductID");
            sql.AppendLine("                 ,       MAX(CreatedDate) AS CreatedDate");
            sql.AppendLine("                 FROM");
            sql.AppendLine("                         #StockProduct");
            sql.AppendLine("                 GROUP BY");
            sql.AppendLine("                         ProductID");
            sql.AppendLine("             ) AS SPM");
            sql.AppendLine("             ON  STP.ProductID = SPM.ProductID");
            sql.AppendLine("             AND STP.CreatedDate = SPM.CreatedDate");
            sql.AppendLine(String.Empty);
            sql.AppendLine("         UNION ALL");
            sql.AppendLine(String.Empty);
            sql.AppendLine("         SELECT");
            sql.AppendLine("                 2 AS ProductStyle");
            sql.AppendLine("         ,       STP.ParentID");
            sql.AppendLine("         ,       (");
            sql.AppendLine("                     CASE STP.Type");
            sql.AppendLine("                         WHEN 1");
            sql.AppendLine("                             THEN STP.QuantityCurrent + STP.Quantity");
            sql.AppendLine("                         WHEN 2");
            sql.AppendLine("                             THEN STP.QuantityCurrent - STP.Quantity");
            sql.AppendLine("                         ELSE");
            sql.AppendLine("                             0");
            sql.AppendLine("                     END");
            sql.AppendLine("                 ) AS QuantityLeft");
            sql.AppendLine("         FROM ");
            sql.AppendLine("                 #StockProductVariable AS STP");
            sql.AppendLine("         INNER JOIN (");
            sql.AppendLine("                 SELECT");
            sql.AppendLine("                         ProductVariableID");
            sql.AppendLine("                 ,       MAX(CreatedDate) AS CreatedDate");
            sql.AppendLine("                 FROM");
            sql.AppendLine("                         #StockProductVariable");
            sql.AppendLine("                 GROUP BY");
            sql.AppendLine("                         ProductVariableID");
            sql.AppendLine("             ) AS SPM");
            sql.AppendLine("             ON  STP.ProductVariableID = SPM.ProductVariableID");
            sql.AppendLine("             AND STP.CreatedDate = SPM.CreatedDate");
            sql.AppendLine("     ) AS PRQ");
            sql.AppendLine("     GROUP BY");
            sql.AppendLine("             PRQ.ProductStyle");
            sql.AppendLine("     ,       PRQ.ParentID");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #ProductQuantity([ProductStyle], [ParentID])");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             p.ProductStyle AS ProductStyle");
            sql.AppendLine("     ,       c.CategoryName");
            sql.AppendLine("     ,       p.*");
            sql.AppendLine("     ,       PRQ.QuantityLeft");
            sql.AppendLine("     FROM");
            sql.AppendLine("             #Product AS p");
            sql.AppendLine("     LEFT JOIN #ProductQuantity AS PRQ");
            sql.AppendLine("         ON  p.ProductStyle = PRQ.ProductStyle");
            sql.AppendLine("         AND p.ID = PRQ.ParentID");
            sql.AppendLine("     LEFT JOIN (");
            sql.AppendLine("             SELECT");
            sql.AppendLine("                     ID");
            sql.AppendLine("             ,       CategoryName");
            sql.AppendLine("             FROM");
            sql.AppendLine("                     dbo.tbl_Category");
            sql.AppendLine("     ) AS c");
            sql.AppendLine("     ON c.ID = p.CategoryID");
            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             p.ID");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine(" END");

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql.ToString());

            while (reader.Read())
            {
                double quantityLeft = 0;

                var entity = new ProductSQL();

                entity.ID = Convert.ToInt32(reader["ID"]);

                if (reader["QuantityLeft"] != DBNull.Value)
                {
                    quantityLeft = Convert.ToDouble(reader["QuantityLeft"]);
                }

                entity.TotalProductInstockQuantityLeft = quantityLeft;

                if (reader["Regular_Price"] != DBNull.Value)
                    entity.RegularPrice = Convert.ToDouble(reader["Regular_Price"].ToString());
                if (reader["CostOfGood"] != DBNull.Value)
                    entity.CostOfGood = Convert.ToDouble(reader["CostOfGood"].ToString());

                list.Add(entity);
            }

            reader.Close();
            return list.ToList();
        }
        public static List<ProductSQL> GetProductAPI(int categoryID, int limit, int showHomePage, int minQuantity, int changeProductName)
        {
            var list = new List<ProductSQL>();
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("BEGIN");

            if (categoryID > 0)
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
                sql.AppendLine("    AND     ID = " + categoryID);
                sql.AppendLine(")");
                sql.AppendLine("SELECT");
                sql.AppendLine("        ID");
                sql.AppendLine(",       CategoryName");
                sql.AppendLine(",       ParentID");
                sql.AppendLine("INTO #category");
                sql.AppendLine("FROM category");
            }

            sql.AppendLine(String.Empty);
            sql.AppendLine("    SELECT");
            sql.AppendLine("            PRD.*");
            sql.AppendLine("    INTO #Product");
            sql.AppendLine("    FROM");
            sql.AppendLine("            tbl_product AS PRD");
            sql.AppendLine("    WHERE");
            sql.AppendLine("            1 = 1");

            if (categoryID > 0)
            {
                sql.AppendLine("    AND EXISTS(");
                sql.AppendLine("            SELECT");
                sql.AppendLine("                    NULL AS DUMMY");
                sql.AppendLine("            FROM");
                sql.AppendLine("                    #category");
                sql.AppendLine("            WHERE");
                sql.AppendLine("                    ID = PRD.CategoryID");
                sql.AppendLine("    )");
            }

            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             PRD.ProductStyle");
            sql.AppendLine("     ,       PRD.ID");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #Product([ProductStyle], [ID])");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             STM.ProductID");
            sql.AppendLine("     ,       STM.Quantity");
            sql.AppendLine("     ,       STM.QuantityCurrent");
            sql.AppendLine("     ,       STM.Type");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ,       STM.ParentID");
            sql.AppendLine("     INTO #StockProduct");
            sql.AppendLine("     FROM");
            sql.AppendLine("             #Product AS PRD");
            sql.AppendLine("     INNER JOIN tbl_StockManager AS STM");
            sql.AppendLine("         ON  PRD.ProductStyle = 1");
            sql.AppendLine("         AND PRD.ID = STM.ParentID");
            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             STM.ProductID");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #StockProduct(");
            sql.AppendLine("             [ProductID] ASC");
            sql.AppendLine("     ,       [CreatedDate] DESC");
            sql.AppendLine("     )");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             STM.ProductVariableID");
            sql.AppendLine("     ,       STM.Quantity");
            sql.AppendLine("     ,       STM.QuantityCurrent");
            sql.AppendLine("     ,       STM.Type");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ,       STM.ParentID");
            sql.AppendLine("     INTO #StockProductVariable");
            sql.AppendLine("     FROM");
            sql.AppendLine("             #Product AS PRD");
            sql.AppendLine("     INNER JOIN tbl_StockManager AS STM");
            sql.AppendLine("         ON  PRD.ProductStyle = 2");
            sql.AppendLine("         AND PRD.ID = STM.ParentID");
            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             STM.ProductVariableID");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #StockProductVariable(");
            sql.AppendLine("             [ProductVariableID] ASC");
            sql.AppendLine("     ,       [CreatedDate] DESC");
            sql.AppendLine("     )");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             PRQ.ProductStyle");
            sql.AppendLine("     ,       PRQ.ParentID");
            sql.AppendLine("     ,       SUM(ISNULL(PRQ.QuantityLeft, 0)) AS QuantityLeft");
            sql.AppendLine("     INTO #ProductQuantity");
            sql.AppendLine("     FROM (");
            sql.AppendLine("         SELECT");
            sql.AppendLine("                 1 AS ProductStyle");
            sql.AppendLine("         ,       STP.ParentID");
            sql.AppendLine("         ,       (");
            sql.AppendLine("                     CASE STP.Type");
            sql.AppendLine("                         WHEN 1");
            sql.AppendLine("                             THEN STP.QuantityCurrent + STP.Quantity");
            sql.AppendLine("                         WHEN 2");
            sql.AppendLine("                             THEN STP.QuantityCurrent - STP.Quantity");
            sql.AppendLine("                         ELSE");
            sql.AppendLine("                             0");
            sql.AppendLine("                     END");
            sql.AppendLine("                 ) AS QuantityLeft");
            sql.AppendLine("         FROM ");
            sql.AppendLine("                 #StockProduct AS STP");
            sql.AppendLine("         INNER JOIN (");
            sql.AppendLine("                 SELECT");
            sql.AppendLine("                         ProductID");
            sql.AppendLine("                 ,       MAX(CreatedDate) AS CreatedDate");
            sql.AppendLine("                 FROM");
            sql.AppendLine("                         #StockProduct");
            sql.AppendLine("                 GROUP BY");
            sql.AppendLine("                         ProductID");
            sql.AppendLine("             ) AS SPM");
            sql.AppendLine("             ON  STP.ProductID = SPM.ProductID");
            sql.AppendLine("             AND STP.CreatedDate = SPM.CreatedDate");
            sql.AppendLine(String.Empty);
            sql.AppendLine("         UNION ALL");
            sql.AppendLine(String.Empty);
            sql.AppendLine("         SELECT");
            sql.AppendLine("                 2 AS ProductStyle");
            sql.AppendLine("         ,       STP.ParentID");
            sql.AppendLine("         ,       (");
            sql.AppendLine("                     CASE STP.Type");
            sql.AppendLine("                         WHEN 1");
            sql.AppendLine("                             THEN STP.QuantityCurrent + STP.Quantity");
            sql.AppendLine("                         WHEN 2");
            sql.AppendLine("                             THEN STP.QuantityCurrent - STP.Quantity");
            sql.AppendLine("                         ELSE");
            sql.AppendLine("                             0");
            sql.AppendLine("                     END");
            sql.AppendLine("                 ) AS QuantityLeft");
            sql.AppendLine("         FROM ");
            sql.AppendLine("                 #StockProductVariable AS STP");
            sql.AppendLine("         INNER JOIN (");
            sql.AppendLine("                 SELECT");
            sql.AppendLine("                         ProductVariableID");
            sql.AppendLine("                 ,       MAX(CreatedDate) AS CreatedDate");
            sql.AppendLine("                 FROM");
            sql.AppendLine("                         #StockProductVariable");
            sql.AppendLine("                 GROUP BY");
            sql.AppendLine("                         ProductVariableID");
            sql.AppendLine("             ) AS SPM");
            sql.AppendLine("             ON  STP.ProductVariableID = SPM.ProductVariableID");
            sql.AppendLine("             AND STP.CreatedDate = SPM.CreatedDate");
            sql.AppendLine("     ) AS PRQ");
            sql.AppendLine("     GROUP BY");
            sql.AppendLine("             PRQ.ProductStyle");
            sql.AppendLine("     ,       PRQ.ParentID");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #ProductQuantity([ProductStyle], [ParentID])");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             p.ProductStyle AS ProductStyle");
            sql.AppendLine("     ,       c.CategoryName");
            sql.AppendLine("     ,       p.*");
            sql.AppendLine("     ,       PRQ.QuantityLeft");
            sql.AppendLine("     FROM");
            sql.AppendLine("             #Product AS p");
            sql.AppendLine("     LEFT JOIN #ProductQuantity AS PRQ");
            sql.AppendLine("         ON  p.ProductStyle = PRQ.ProductStyle");
            sql.AppendLine("         AND p.ID = PRQ.ParentID");
            sql.AppendLine("     LEFT JOIN (");
            sql.AppendLine("             SELECT");
            sql.AppendLine("                     ID");
            sql.AppendLine("             ,       CategoryName");
            sql.AppendLine("             FROM");
            sql.AppendLine("                     dbo.tbl_Category");
            sql.AppendLine("     ) AS c");
            sql.AppendLine("     ON c.ID = p.CategoryID");
            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             p.ID");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine(" END");

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql.ToString());

            while (reader.Read())
            {
                double quantityLeft = 0;

                var entity = new ProductSQL();

                entity.ID = Convert.ToInt32(reader["ID"]);

                bool check = true;

                // check min stock

                if (reader["QuantityLeft"] != DBNull.Value)
                {
                    quantityLeft = Convert.ToDouble(reader["QuantityLeft"]);

                    if (quantityLeft >= minQuantity)
                    {
                        // check show homepage

                        if (showHomePage == 1)
                        {
                            if (string.IsNullOrEmpty(reader["ProductImage"].ToString()) || reader["ShowHomePage"].ToString().ToInt(0) == 0)
                            {
                                check = false;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(reader["ProductImage"].ToString()))
                            {
                                check = false;
                            }
                        }
                    }
                    else
                    {
                        check = false;
                    }
                }
                else
                {
                    
                    // check show homepage

                    if (showHomePage == 1)
                    {
                        if (string.IsNullOrEmpty(reader["ProductImage"].ToString()) || reader["ShowHomePage"].ToString().ToInt(0) == 0)
                        {
                            check = false;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(reader["ProductImage"].ToString()))
                        {
                            check = false;
                        }
                    }
                }
                

                if (check == true)
                {
                    entity.ProductImage = reader["ProductImage"].ToString();

                    if (!string.IsNullOrEmpty(reader["ProductImageClean"].ToString()))
                    {
                        entity.ProductImageClean = reader["ProductImageClean"].ToString();
                    }

                    if (changeProductName == 1)
                    {
                        if (reader["CategoryName"] != DBNull.Value)
                        {
                            entity.ProductTitle = reader["CategoryName"].ToString() + ' ' + reader["ProductSKU"].ToString();
                        }
                        else
                        {
                            if (reader["ProductTitle"] != DBNull.Value)
                            {
                                entity.ProductTitle = reader["ProductTitle"].ToString() + ' ' + reader["ProductSKU"].ToString();
                            }
                        }
                            
                    }
                    else
                    {
                        if (reader["ProductTitle"] != DBNull.Value)
                        {
                            entity.ProductTitle = reader["ProductTitle"].ToString();
                        }
                    }
                    
                    // get SKU
                    if (reader["ProductSKU"] != DBNull.Value)
                        entity.ProductSKU = reader["ProductSKU"].ToString();

                    // sản phẩm đã nhập kho
                    if (reader["QuantityLeft"] != DBNull.Value)
                    {
                        quantityLeft = Convert.ToDouble(reader["QuantityLeft"]);

                        if (quantityLeft > 0)
                        {
                            entity.StockStatus = 1;
                        }
                        else
                        {
                            entity.StockStatus = 2;
                        }
                    }
                    else
                    {
                        // Sản phẩm chưa nhập kho thì cho kho = 1 để khi đồng bộ sản phẩm thì trạng thái là còn hàng
                        quantityLeft = 1;
                        entity.StockStatus = 1;
                    }

                    entity.TotalProductInstockQuantityLeft = quantityLeft;

                    if (reader["Regular_Price"] != DBNull.Value)
                        entity.RegularPrice = Convert.ToDouble(reader["Regular_Price"].ToString());
                    if (reader["CostOfGood"] != DBNull.Value)
                        entity.CostOfGood = Convert.ToDouble(reader["CostOfGood"].ToString());
                    if (reader["Retail_Price"] != DBNull.Value)
                        entity.RetailPrice = Convert.ToDouble(reader["Retail_Price"].ToString());
                    if (reader["CategoryName"] != DBNull.Value)
                        entity.CategoryName = reader["CategoryName"].ToString();
                    if (reader["CategoryID"] != DBNull.Value)
                        entity.CategoryID = reader["CategoryID"].ToString().ToInt(0);
                    if (reader["CreatedDate"] != DBNull.Value)
                        entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    if (reader["ProductContent"] != DBNull.Value)
                        entity.ProductContent = reader["ProductContent"].ToString();
                    if (reader["ProductStyle"] != DBNull.Value)
                        entity.ProductStyle = reader["ProductStyle"].ToString().ToInt(0);

                    list.Add(entity);
                }
            }

            reader.Close();
            return list.OrderByDescending(x => x.ID).Take(limit).ToList();
        }

        public static List<ProductSQL> GetAllSql(ProductFilterModel filter, ref PaginationMetadataModel page)
        {
            var list = new List<ProductSQL>();
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("BEGIN");
            #region Khai báo biến phân trang
            sql.AppendLine(String.Empty);
            sql.AppendLine("    DECLARE @totalCount int = 0;");
            sql.AppendLine(String.Format("    DECLARE @pageSize int = {0};", page.pageSize));
            sql.AppendLine(String.Format("    DECLARE @currentPage int = {0};", page.currentPage));
            sql.AppendLine("    DECLARE @totalPages int = 0;");
            #endregion
            #region Lấy id category (gồm của category cha and con)
            sql.AppendLine(String.Empty);
            if (filter.category > 0)
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
                sql.AppendLine("    AND     ID = " + filter.category);
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
            #endregion
            
            #region Lấy id màu mà có chứa từ khóa
            // Filter by color product
            if (!String.IsNullOrEmpty(filter.color))
            {
                sql.AppendLine(String.Empty);
                sql.AppendLine("    With VariableValue AS (");
                sql.AppendLine("        SELECT");
                sql.AppendLine("            *");
                sql.AppendLine("        FROM");
                sql.AppendLine("            tbl_VariableValue");
                sql.AppendLine("        WHERE");
                sql.AppendLine(String.Format("        VariableID = 1 AND  LOWER(VariableValue) LIKE N'%{0}%'", filter.color.ToLower()));
                sql.AppendLine("    )");
                sql.AppendLine("    SELECT");
                sql.AppendLine("        PVA.ProductID AS ID");
                sql.AppendLine("    INTO #ProductColor");
                sql.AppendLine("    FROM ");
                sql.AppendLine("    (");
                sql.AppendLine("        SELECT");
                sql.AppendLine("            PVV.ProductVariableID");
                sql.AppendLine("        FROM");
                sql.AppendLine("            VariableValue AS VAV");
                sql.AppendLine("        INNER JOIN tbl_ProductVariableValue AS PVV");
                sql.AppendLine("        ON  VAV.ID = PVV.VariableValueID");
                sql.AppendLine("        GROUP BY PVV.ProductVariableID");
                sql.AppendLine("    ) AS PVF");
                sql.AppendLine("    INNER JOIN tbl_ProductVariable AS PVA");
                sql.AppendLine("    ON  PVF.ProductVariableID = PVA.ID");
                sql.AppendLine("    GROUP BY PVA.ProductID;");
            }
            #endregion

            #region Lấy ra id size mà có chứa từ khóa
            // Filter by size product
            if (!String.IsNullOrEmpty(filter.size))
            {
                sql.AppendLine(String.Empty);
                sql.AppendLine("    With VariableValue AS (");
                sql.AppendLine("        SELECT");
                sql.AppendLine("            *");
                sql.AppendLine("        FROM");
                sql.AppendLine("            tbl_VariableValue");
                sql.AppendLine("        WHERE");
                sql.AppendLine(String.Format("        VariableID = 2 AND  LOWER(VariableValue) LIKE N'%{0}%'", filter.size.ToLower()));
                sql.AppendLine("    )");
                sql.AppendLine("    SELECT");
                sql.AppendLine("        PVA.ProductID AS ID");
                sql.AppendLine("    INTO #ProductSize");
                sql.AppendLine("    FROM ");
                sql.AppendLine("    (");
                sql.AppendLine("        SELECT");
                sql.AppendLine("            PVV.ProductVariableID");
                sql.AppendLine("        FROM");
                sql.AppendLine("            VariableValue AS VAV");
                sql.AppendLine("        INNER JOIN tbl_ProductVariableValue AS PVV");
                sql.AppendLine("        ON  VAV.ID = PVV.VariableValueID");
                sql.AppendLine("        GROUP BY PVV.ProductVariableID");
                sql.AppendLine("    ) AS PVF");
                sql.AppendLine("    INNER JOIN tbl_ProductVariable AS PVA");
                sql.AppendLine("    ON  PVF.ProductVariableID = PVA.ID");
                sql.AppendLine("    GROUP BY PVA.ProductID;");
            }
            #endregion

            #region Thực thi lấy dữ liệu
            #region Trích xuất dữ liệu chính
            sql.AppendLine(String.Empty);
            sql.AppendLine("    SELECT");
            sql.AppendLine("            PRD.*");
            sql.AppendLine("    INTO #Product");
            sql.AppendLine("    FROM");
            sql.AppendLine("            tbl_Product AS PRD");
            sql.AppendLine("    WHERE");
            sql.AppendLine("            1 = 1");

            #region Loc theo từ khóa (Tên sản phẩm, SKU, màu)
            if (!string.IsNullOrEmpty(filter.search))
            {
                sql.AppendLine("    AND (");
                sql.AppendLine(String.Format("        PRD.ProductSKU like N'%{0}%'", filter.search));
                sql.AppendLine(String.Format("        OR PRD.ProductTitle like N'%{0}%'", filter.search));
                sql.AppendLine(String.Format("        OR PRD.UnSignedTitle like N'%{0}%'", filter.search));
                sql.AppendLine(String.Format("        OR PRD.Materials like N'%{0}%'", filter.search));
                sql.AppendLine("    )");
            }
            #endregion

            #region Tìm sản phẩm có giá bằng ...
            if (filter.price > 0)
            {
                sql.AppendLine("    AND (");
                sql.AppendLine(String.Format("        PRD.Regular_Price = {0}", filter.price));
                sql.AppendLine("    )");
            }
            #endregion

            #region Lọc sản phẩm show lên home page
            if (!string.IsNullOrEmpty(filter.showHomePage))
            {
                sql.AppendLine("    AND (");
                sql.AppendLine(String.Format("        PRD.ShowHomePage = {0}", filter.showHomePage));
                sql.AppendLine("    )");
            }
            #endregion

            #region Lọc sản phẩm hiển thị tại trang quảng cáo
            if (!string.IsNullOrEmpty(filter.webPublish))
            {
                sql.AppendLine("    AND (");
                sql.AppendLine(String.Format("        PRD.WebPublish = {0}", filter.webPublish));
                sql.AppendLine("    )");
            }
            #endregion

            #region Lọc sản phẩm theo ngày khởi tạo
            if (!string.IsNullOrEmpty(filter.productDate))
            {
                DateTime fromdate = DateTime.Today;
                DateTime todate = DateTime.Now;
                switch (filter.productDate)
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
                sql.AppendLine(String.Format("	AND	(CONVERT(datetime, PRD.CreatedDate, 121) BETWEEN CONVERT(datetime, '{0}', 121) AND CONVERT(datetime, '{1}', 121))", fromdate.ToString(), todate.ToString()));
            }

            if (filter.fromDate.HasValue && filter.toDate.HasValue)
            {
                sql.AppendLine(String.Format("	AND	(CONVERT(datetime, PRD.CreatedDate, 121) BETWEEN CONVERT(datetime, '{0}', 121) AND CONVERT(datetime, '{1}', 121))", filter.fromDate.ToString(), filter.toDate.ToString()));
            }
            #endregion

            #region Lọc sản phẩm theo màu
            if (!String.IsNullOrEmpty(filter.color))
            {
                sql.AppendLine("    AND (");
                sql.AppendLine(String.Format("        LOWER(PRD.ProductTitle) like N'%{0}%'", filter.color.ToLower()));
                sql.AppendLine(String.Format("        OR LOWER(PRD.Color) like N'%{0}%'", filter.color.ToLower()));
                sql.AppendLine("        OR EXISTS (");
                sql.AppendLine("            SELECT");
                sql.AppendLine("                NULL AS DUMMY");
                sql.AppendLine("            FROM");
                sql.AppendLine("                #ProductColor AS PRC");
                sql.AppendLine("            WHERE");
                sql.AppendLine("                PRD.ID = PRC.ID");
                sql.AppendLine("        )");
                sql.AppendLine("    )");
            }
            #endregion

            #region Lọc sản phẩm theo size
            if (!String.IsNullOrEmpty(filter.size))
            {
                sql.AppendLine("    AND EXISTS (");
                sql.AppendLine("        SELECT");
                sql.AppendLine("            NULL AS DUMMY");
                sql.AppendLine("        FROM");
                sql.AppendLine("            #ProductSize AS PRS");
                sql.AppendLine("        WHERE");
                sql.AppendLine("            PRD.ID = PRS.ID");
                sql.AppendLine("    )");
            }
            #endregion

            #region Lọc sản phẩm theo nhanh cateory (gồm cha và con)
            if (filter.category > 0)
            {
                sql.AppendLine("    AND EXISTS(");
                sql.AppendLine("            SELECT");
                sql.AppendLine("                    NULL AS DUMMY");
                sql.AppendLine("            FROM");
                sql.AppendLine("                    #category");
                sql.AppendLine("            WHERE");
                sql.AppendLine("                    ID = PRD.CategoryID");
                sql.AppendLine("    )");
            }
            #endregion

            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             PRD.ProductStyle");
            sql.AppendLine("     ,       PRD.ID");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #Product([ProductStyle], [ID])");
            #endregion

            #region Trích xuất thông tin kho
            #region Thông tin kho vơi sản phẩm thường
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             STM.ProductID");
            sql.AppendLine("     ,       STM.Quantity");
            sql.AppendLine("     ,       STM.QuantityCurrent");
            sql.AppendLine("     ,       STM.Type");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ,       STM.ParentID");
            sql.AppendLine("     INTO #StockProduct");
            sql.AppendLine("     FROM");
            sql.AppendLine("             #Product AS PRD");
            sql.AppendLine("     INNER JOIN tbl_StockManager AS STM");
            sql.AppendLine("         ON  PRD.ProductStyle = 1");
            sql.AppendLine("         AND PRD.ID = STM.ParentID");
            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             STM.ProductID");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #StockProduct(");
            sql.AppendLine("             [ProductID] ASC");
            sql.AppendLine("     ,       [CreatedDate] DESC");
            sql.AppendLine("     )");
            #endregion

            #region Thông tin kho với sản phẩm biến thể
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             STM.ProductVariableID");
            sql.AppendLine("     ,       STM.Quantity");
            sql.AppendLine("     ,       STM.QuantityCurrent");
            sql.AppendLine("     ,       STM.Type");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ,       STM.ParentID");
            sql.AppendLine("     INTO #StockProductVariable");
            sql.AppendLine("     FROM");
            sql.AppendLine("             #Product AS PRD");
            sql.AppendLine("     INNER JOIN tbl_StockManager AS STM");
            sql.AppendLine("         ON  PRD.ProductStyle = 2");
            sql.AppendLine("         AND PRD.ID = STM.ParentID");
            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             STM.ProductVariableID");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #StockProductVariable(");
            sql.AppendLine("             [ProductVariableID] ASC");
            sql.AppendLine("     ,       [CreatedDate] DESC");
            sql.AppendLine("     )");
            #endregion

            #region Tính thông tin kho của tất cả sản phẩm ( sản phẩm thường và sản phẩm biến thể)
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             PRQ.ProductStyle");
            sql.AppendLine("     ,       PRQ.ParentID");
            sql.AppendLine("     ,       SUM(ISNULL(PRQ.QuantityLeft, 0)) AS QuantityLeft");
            sql.AppendLine("     INTO #ProductQuantity");
            sql.AppendLine("     FROM (");
            sql.AppendLine("         SELECT");
            sql.AppendLine("                 1 AS ProductStyle");
            sql.AppendLine("         ,       STP.ParentID");
            sql.AppendLine("         ,       (");
            sql.AppendLine("                     CASE STP.Type");
            sql.AppendLine("                         WHEN 1");
            sql.AppendLine("                             THEN STP.QuantityCurrent + STP.Quantity");
            sql.AppendLine("                         WHEN 2");
            sql.AppendLine("                             THEN STP.QuantityCurrent - STP.Quantity");
            sql.AppendLine("                         ELSE");
            sql.AppendLine("                             0");
            sql.AppendLine("                     END");
            sql.AppendLine("                 ) AS QuantityLeft");
            sql.AppendLine("         FROM ");
            sql.AppendLine("                 #StockProduct AS STP");
            sql.AppendLine("         INNER JOIN (");
            sql.AppendLine("                 SELECT");
            sql.AppendLine("                         ProductID");
            sql.AppendLine("                 ,       MAX(CreatedDate) AS CreatedDate");
            sql.AppendLine("                 FROM");
            sql.AppendLine("                         #StockProduct");
            sql.AppendLine("                 GROUP BY");
            sql.AppendLine("                         ProductID");
            sql.AppendLine("             ) AS SPM");
            sql.AppendLine("             ON  STP.ProductID = SPM.ProductID");
            sql.AppendLine("             AND STP.CreatedDate = SPM.CreatedDate");
            sql.AppendLine(String.Empty);
            sql.AppendLine("         UNION ALL");
            sql.AppendLine(String.Empty);
            sql.AppendLine("         SELECT");
            sql.AppendLine("                 2 AS ProductStyle");
            sql.AppendLine("         ,       STP.ParentID");
            sql.AppendLine("         ,       (");
            sql.AppendLine("                     CASE STP.Type");
            sql.AppendLine("                         WHEN 1");
            sql.AppendLine("                             THEN STP.QuantityCurrent + STP.Quantity");
            sql.AppendLine("                         WHEN 2");
            sql.AppendLine("                             THEN STP.QuantityCurrent - STP.Quantity");
            sql.AppendLine("                         ELSE");
            sql.AppendLine("                             0");
            sql.AppendLine("                     END");
            sql.AppendLine("                 ) AS QuantityLeft");
            sql.AppendLine("         FROM ");
            sql.AppendLine("                 #StockProductVariable AS STP");
            sql.AppendLine("         INNER JOIN (");
            sql.AppendLine("                 SELECT");
            sql.AppendLine("                         ProductVariableID");
            sql.AppendLine("                 ,       MAX(CreatedDate) AS CreatedDate");
            sql.AppendLine("                 FROM");
            sql.AppendLine("                         #StockProductVariable");
            sql.AppendLine("                 GROUP BY");
            sql.AppendLine("                         ProductVariableID");
            sql.AppendLine("             ) AS SPM");
            sql.AppendLine("             ON  STP.ProductVariableID = SPM.ProductVariableID");
            sql.AppendLine("             AND STP.CreatedDate = SPM.CreatedDate");
            sql.AppendLine("     ) AS PRQ");
            sql.AppendLine("     GROUP BY");
            sql.AppendLine("             PRQ.ProductStyle");
            sql.AppendLine("     ,       PRQ.ParentID");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #ProductQuantity([ProductStyle], [ParentID])");
            #endregion
            #endregion

            #region Lọc lại dữ liệu liên quan đến kho
            if (filter.stockStatus != 0 || !String.IsNullOrEmpty(filter.quantity))
            {
                sql.AppendLine(String.Empty);
                sql.AppendLine("DELETE #Product");
                sql.AppendLine("WHERE ID NOT IN (");
                sql.AppendLine("    SELECT");
                sql.AppendLine("        p.ID AS ProductID");
                sql.AppendLine("    FROM");
                sql.AppendLine("        #Product AS p");
                sql.AppendLine("    LEFT JOIN #ProductQuantity AS PRQ");
                sql.AppendLine("    ON  p.ProductStyle = PRQ.ProductStyle");
                sql.AppendLine("    AND p.ID = PRQ.ParentID");
                sql.AppendLine("    WHERE 1 = 1");
                if (filter.stockStatus == (int)StockStatus.stocking)
                    sql.AppendLine("    AND PRQ.QuantityLeft > 0");
                else if (filter.stockStatus == (int)StockStatus.stockOut)
                    sql.AppendLine("    AND PRQ.QuantityLeft <= 0");
                else if (filter.stockStatus == (int)StockStatus.stockIn)
                    sql.AppendLine("    AND PRQ.QuantityLeft IS NULL");

                if (filter.quantity.Equals("greaterthan"))
                    sql.AppendLine(String.Format("  AND PRQ.QuantityLeft >= {0}", filter.quantityFrom));
                else if (filter.quantity.Equals("lessthan"))
                    sql.AppendLine(String.Format("  AND PRQ.QuantityLeft <= {0}", filter.quantityTo));
                else if (filter.quantity.Equals("between"))
                    sql.AppendLine(String.Format("  AND PRQ.QuantityLeft BETWEEN {0} AND {1}", filter.quantityFrom, filter.quantityTo));
                sql.AppendLine(");");

                sql.AppendLine("DELETE #ProductQuantity");
                sql.AppendLine("WHERE NOT EXISTS (");
                sql.AppendLine("    SELECT");
                sql.AppendLine("        NULL AS DUMMY");
                sql.AppendLine("    FROM");
                sql.AppendLine("        #Product AS p");
                sql.AppendLine("    WHERE");
                sql.AppendLine("        p.ProductStyle = ProductStyle");
                sql.AppendLine("    AND p.ID = ParentID");
                sql.AppendLine(");");
            }
            #endregion

            #region Tính toán phân trang
            sql.AppendLine(String.Empty);
            sql.AppendLine("SELECT");
            sql.AppendLine("    @totalCount = COUNT(*),");
            sql.AppendLine("    @totalPages = CEILING(COUNT(*) / (@pageSize * 1.0))");
            sql.AppendLine("FROM");
            sql.AppendLine("    #Product AS p;");
            #endregion
            #region Kết thúc
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             @totalCount AS TotalCount");
            sql.AppendLine("     ,       @totalPages AS TotalPages");
            sql.AppendLine("     ,       p.ProductStyle AS ProductStyle");
            sql.AppendLine("     ,       c.CategoryName");
            sql.AppendLine("     ,       p.*");
            sql.AppendLine("     ,       PRQ.QuantityLeft");
            sql.AppendLine("     FROM");
            sql.AppendLine("             #Product AS p");
            sql.AppendLine("     LEFT JOIN #ProductQuantity AS PRQ");
            sql.AppendLine("         ON  p.ProductStyle = PRQ.ProductStyle");
            sql.AppendLine("         AND p.ID = PRQ.ParentID");
            sql.AppendLine("     LEFT JOIN (");
            sql.AppendLine("             SELECT");
            sql.AppendLine("                     ID");
            sql.AppendLine("             ,       CategoryName");
            sql.AppendLine("             FROM");
            sql.AppendLine("                     dbo.tbl_Category");
            sql.AppendLine("     ) AS c");
            sql.AppendLine("     ON c.ID = p.CategoryID");
            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             p.ID DESC");
            sql.AppendLine("     OFFSET @pageSize * (@currentPage - 1) ROWS");
            sql.AppendLine("     FETCH NEXT @pageSize ROWS ONLY");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            #endregion
            #endregion
            sql.AppendLine(" END");

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql.ToString());

            while (reader.Read())
            {
                double quantityLeft = 0;

                var entity = new ProductSQL();

                entity.ID = Convert.ToInt32(reader["ID"]);

                entity.ProductImage = reader["ProductImage"].ToString();

                if (!string.IsNullOrEmpty(reader["ProductImageClean"].ToString()))
                {
                    entity.ProductImageClean = reader["ProductImageClean"].ToString();
                }

                if (reader["ProductTitle"] != DBNull.Value)
                    entity.ProductTitle = reader["ProductTitle"].ToString();
                if (reader["ProductSKU"] != DBNull.Value)
                    entity.ProductSKU = reader["ProductSKU"].ToString();


                if (reader["QuantityLeft"] != DBNull.Value)
                {
                    quantityLeft = Convert.ToDouble(reader["QuantityLeft"]);

                    if (quantityLeft > 0)
                    {
                        entity.ProductInstockStatus = "<span class='bg-green'>Còn hàng</span>";
                        entity.StockStatus = 1;
                    }
                    else
                    {
                        entity.ProductInstockStatus = "<span class='bg-red'>Hết hàng</span>";
                        entity.StockStatus = 2;
                    }
                }
                else
                {
                    entity.ProductInstockStatus = "<span class='bg-yellow'>Nhập hàng</span>";
                    entity.StockStatus = 3;
                }

                entity.TotalProductInstockQuantityLeft = quantityLeft;

                if (reader["Regular_Price"] != DBNull.Value)
                    entity.RegularPrice = Convert.ToDouble(reader["Regular_Price"].ToString());
                if (reader["CostOfGood"] != DBNull.Value)
                    entity.CostOfGood = Convert.ToDouble(reader["CostOfGood"].ToString());
                if (reader["Retail_Price"] != DBNull.Value)
                    entity.RetailPrice = Convert.ToDouble(reader["Retail_Price"].ToString());
                if (reader["CategoryName"] != DBNull.Value)
                    entity.CategoryName = reader["CategoryName"].ToString();
                if (reader["CategoryID"] != DBNull.Value)
                    entity.CategoryID = reader["CategoryID"].ToString().ToInt(0);
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                if (reader["ProductContent"] != DBNull.Value)
                    entity.ProductContent = reader["ProductContent"].ToString();
                if (reader["ProductStyle"] != DBNull.Value)
                    entity.ProductStyle = reader["ProductStyle"].ToString().ToInt(0);
                if (reader["ShowHomePage"] != DBNull.Value)
                    entity.ShowHomePage = reader["ShowHomePage"].ToString().ToInt(0);
                if (reader["Materials"] != DBNull.Value)
                    entity.Materials = reader["Materials"].ToString();
                if (reader["WebPublish"] != DBNull.Value)
                    entity.WebPublish = reader["WebPublish"].ToString().ToBool();
                if (reader["WebUpdate"] != DBNull.Value)
                    entity.WebUpdate = Convert.ToDateTime(reader["WebUpdate"]);
                // get infe page header
                if (reader["TotalCount"] != DBNull.Value)
                    page.totalCount = reader["TotalCount"].ToString().ToInt(0);
                if (reader["TotalPages"] != DBNull.Value)
                    page.totalPages = reader["TotalPages"].ToString().ToInt(0);
                list.Add(entity);
            }
            reader.Close();
            return list.OrderByDescending(x => x.ID).ToList();
        }

        public static ProductStockReport getProductStockReport(string SKU, int CategoryID)
        {
            var list = new List<ProductStockReport>();
            StringBuilder sql = new StringBuilder();

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

            sql.AppendLine(String.Empty);
            sql.AppendLine("    SELECT");
            sql.AppendLine("            PRD.*");
            sql.AppendLine("    INTO #Product");
            sql.AppendLine("    FROM");
            sql.AppendLine("            tbl_Product AS PRD");
            sql.AppendLine("    WHERE");
            sql.AppendLine("            1 = 1");
            sql.AppendLine("    AND (PRD.ProductSKU like '" + SKU + "%')");

            if (CategoryID > 0)
            {
                sql.AppendLine("    AND EXISTS(");
                sql.AppendLine("            SELECT");
                sql.AppendLine("                    NULL AS DUMMY");
                sql.AppendLine("            FROM");
                sql.AppendLine("                    #category");
                sql.AppendLine("            WHERE");
                sql.AppendLine("                    ID = PRD.CategoryID");
                sql.AppendLine("    )");
            }

            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             PRD.ProductStyle");
            sql.AppendLine("     ,       PRD.ID");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #Product([ProductStyle], [ID])");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             STM.ProductID");
            sql.AppendLine("     ,       STM.Quantity");
            sql.AppendLine("     ,       STM.QuantityCurrent");
            sql.AppendLine("     ,       STM.Type");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ,       STM.ParentID");
            sql.AppendLine("     INTO #StockProduct");
            sql.AppendLine("     FROM");
            sql.AppendLine("             #Product AS PRD");
            sql.AppendLine("     INNER JOIN tbl_StockManager AS STM");
            sql.AppendLine("         ON  PRD.ProductStyle = 1");
            sql.AppendLine("         AND PRD.ID = STM.ParentID");
            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             STM.ProductID");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #StockProduct(");
            sql.AppendLine("             [ProductID] ASC");
            sql.AppendLine("     ,       [CreatedDate] DESC");
            sql.AppendLine("     )");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             STM.ProductVariableID");
            sql.AppendLine("     ,       STM.Quantity");
            sql.AppendLine("     ,       STM.QuantityCurrent");
            sql.AppendLine("     ,       STM.Type");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ,       STM.ParentID");
            sql.AppendLine("     INTO #StockProductVariable");
            sql.AppendLine("     FROM");
            sql.AppendLine("             #Product AS PRD");
            sql.AppendLine("     INNER JOIN tbl_StockManager AS STM");
            sql.AppendLine("         ON  PRD.ProductStyle = 2");
            sql.AppendLine("         AND PRD.ID = STM.ParentID");
            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             STM.ProductVariableID");
            sql.AppendLine("     ,       STM.CreatedDate");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #StockProductVariable(");
            sql.AppendLine("             [ProductVariableID] ASC");
            sql.AppendLine("     ,       [CreatedDate] DESC");
            sql.AppendLine("     )");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             PRQ.ProductStyle");
            sql.AppendLine("     ,       PRQ.ParentID");
            sql.AppendLine("     ,       SUM(ISNULL(PRQ.QuantityLeft, 0)) AS QuantityLeft");
            sql.AppendLine("     INTO #ProductQuantity");
            sql.AppendLine("     FROM (");
            sql.AppendLine("         SELECT");
            sql.AppendLine("                 1 AS ProductStyle");
            sql.AppendLine("         ,       STP.ParentID");
            sql.AppendLine("         ,       (");
            sql.AppendLine("                     CASE STP.Type");
            sql.AppendLine("                         WHEN 1");
            sql.AppendLine("                             THEN STP.QuantityCurrent + STP.Quantity");
            sql.AppendLine("                         WHEN 2");
            sql.AppendLine("                             THEN STP.QuantityCurrent - STP.Quantity");
            sql.AppendLine("                         ELSE");
            sql.AppendLine("                             0");
            sql.AppendLine("                     END");
            sql.AppendLine("                 ) AS QuantityLeft");
            sql.AppendLine("         FROM ");
            sql.AppendLine("                 #StockProduct AS STP");
            sql.AppendLine("         INNER JOIN (");
            sql.AppendLine("                 SELECT");
            sql.AppendLine("                         ProductID");
            sql.AppendLine("                 ,       MAX(CreatedDate) AS CreatedDate");
            sql.AppendLine("                 FROM");
            sql.AppendLine("                         #StockProduct");
            sql.AppendLine("                 GROUP BY");
            sql.AppendLine("                         ProductID");
            sql.AppendLine("             ) AS SPM");
            sql.AppendLine("             ON  STP.ProductID = SPM.ProductID");
            sql.AppendLine("             AND STP.CreatedDate = SPM.CreatedDate");
            sql.AppendLine(String.Empty);
            sql.AppendLine("         UNION ALL");
            sql.AppendLine(String.Empty);
            sql.AppendLine("         SELECT");
            sql.AppendLine("                 2 AS ProductStyle");
            sql.AppendLine("         ,       STP.ParentID");
            sql.AppendLine("         ,       (");
            sql.AppendLine("                     CASE STP.Type");
            sql.AppendLine("                         WHEN 1");
            sql.AppendLine("                             THEN STP.QuantityCurrent + STP.Quantity");
            sql.AppendLine("                         WHEN 2");
            sql.AppendLine("                             THEN STP.QuantityCurrent - STP.Quantity");
            sql.AppendLine("                         ELSE");
            sql.AppendLine("                             0");
            sql.AppendLine("                     END");
            sql.AppendLine("                 ) AS QuantityLeft");
            sql.AppendLine("         FROM ");
            sql.AppendLine("                 #StockProductVariable AS STP");
            sql.AppendLine("         INNER JOIN (");
            sql.AppendLine("                 SELECT");
            sql.AppendLine("                         ProductVariableID");
            sql.AppendLine("                 ,       MAX(CreatedDate) AS CreatedDate");
            sql.AppendLine("                 FROM");
            sql.AppendLine("                         #StockProductVariable");
            sql.AppendLine("                 GROUP BY");
            sql.AppendLine("                         ProductVariableID");
            sql.AppendLine("             ) AS SPM");
            sql.AppendLine("             ON  STP.ProductVariableID = SPM.ProductVariableID");
            sql.AppendLine("             AND STP.CreatedDate = SPM.CreatedDate");
            sql.AppendLine("     ) AS PRQ");
            sql.AppendLine("     GROUP BY");
            sql.AppendLine("             PRQ.ProductStyle");
            sql.AppendLine("     ,       PRQ.ParentID");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     CREATE INDEX [ID_PROCDUCT] ON #ProductQuantity([ProductStyle], [ParentID])");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             p.ProductStyle AS ProductStyle");
            sql.AppendLine("     ,       p.*");
            sql.AppendLine("     ,       PRQ.QuantityLeft");
            sql.AppendLine("     FROM");
            sql.AppendLine("             #Product AS p");
            sql.AppendLine("     LEFT JOIN #ProductQuantity AS PRQ");
            sql.AppendLine("         ON  p.ProductStyle = PRQ.ProductStyle");
            sql.AppendLine("         AND p.ID = PRQ.ParentID");
            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             p.ID");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine(" END");

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql.ToString());
            

            while (reader.Read())
            {
                var entity = new ProductStockReport();

                double quantityLeft = 0;

                if (reader["QuantityLeft"] != DBNull.Value)
                {
                    quantityLeft = Convert.ToDouble(reader["QuantityLeft"]);
                }

                entity.totalStock = Convert.ToInt32(quantityLeft);

                if (reader["CostOfGood"] != DBNull.Value)
                {
                    entity.totalStockValue = quantityLeft *  Convert.ToDouble(reader["CostOfGood"]);
                }
                list.Add(entity);
            }
            reader.Close();

            return new ProductStockReport()
            {
                totalStock = list.Sum(x => x.totalStock),
                totalStockValue = list.Sum(x => x.totalStockValue),
            };
        }


        public static ProductStock GetStock(int ProductVariableID)
        {
            var entity = new ProductStock();
            var sql = @"select t.ID,t.SKU,t.ID, d.quantiyIN as InProduct, k.quantiyIN as OutProduct, (d.quantiyIN - k.quantiyIN) as leftProduct from tbl_ProductVariable as t";
            sql += " left outer join (select  ProductVariableID, sum(quantity) as quantiyIN from tbl_StockManager where [Type]= 1 group by ProductVariableID) as d ON t.ID = d.ProductVariableID ";
            sql += " left outer join (select ProductVariableID, sum(quantity) as quantiyIN from tbl_StockManager where [Type]= 2 group by ProductVariableID) as k ON t.ID = k.ProductVariableID ";
            if (ProductVariableID > 0)
            {
                sql += " Where t.ID = " + ProductVariableID;
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);

            while (reader.Read())
            {
                int quantityIn = reader["InProduct"].ToString().ToInt(0);
                int quantityOut = reader["OutProduct"].ToString().ToInt(0);
                int quantityLeft = quantityIn - quantityOut;


                if (quantityIn > 0)
                {
                    if (quantityLeft > 0)
                    {
                        entity.ProductInstockStatus = "<span class=\"bg-green\">Còn hàng</span>";

                    }
                    else
                    {
                        entity.ProductInstockStatus = "<span class=\"bg-red\">Hết hàng</span>";
                    }
                }
                else
                {
                    entity.ProductInstockStatus = "<span class=\"bg-yellow\">Nhập hàng</span>";

                }
                entity.quantityLeft = quantityLeft;
            }
            reader.Close();
            return entity;
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

        public static List<ProductShelf> GetProductShelf(ProductFilterModel filter, ref PaginationMetadataModel page)
        {
            using (var con = new inventorymanagementEntities())
            {
                var product = con.tbl_Product
                    .GroupJoin(
                        con.tbl_ProductVariable,
                        p => new { productID = p.ID, productStyle = p.ProductStyle.Value },
                        v => new { productID = v.ProductID.Value, productStyle = 2 },
                        (p, v) => new { p, v }
                    )
                    .SelectMany(
                        x => x.v.DefaultIfEmpty(),
                        (parent, child) => new
                        {
                            product = parent.p,
                            variable = child
                        }
                    );


                #region Lọc với text search
                if (!String.IsNullOrEmpty(filter.search))
                {
                    var search = filter.search.Trim().ToLower();
                    product = product
                        .Where(x =>
                            x.product.ProductSKU.Trim().ToLower().Contains(search) ||
                            x.variable.SKU.Trim().ToLower().Contains(search) ||
                            x.product.UnSignedTitle.Trim().ToLower().Contains(search)
                        );
                }
                #endregion

                #region Lọc với danh mục
                if (filter.category > 0)
                {
                    var parentCatogory = con.tbl_Category.Where(x => x.ID == filter.category).FirstOrDefault();
                    var catogoryFilter = CategoryController.getCategoryChild(parentCatogory).Select(x => x.ID).ToList();

                    product = product
                        .Where(x =>
                            catogoryFilter.Contains(
                                x.product.CategoryID.HasValue ? x.product.CategoryID.Value : 0
                                )
                        );
                }
                #endregion

                #region Lọc với số lượng còn hay hết dựa theo kiểm kệ gần nhất
                if (filter.stockStatus > 0)
                {
                    product = product
                        .GroupJoin(
                            con.ShelfManagers.Where(x => x.ProductVariableID == 0),
                            p => new
                            {
                                productID = p.product.ID,
                                variableID = p.variable != null ? p.variable.ID : 0
                            },
                            s => new
                            {
                                productID = s.ProductID,
                                variableID = s.ProductVariableID
                            },
                            (p, s) => new { p, s }
                        )
                        .SelectMany(
                            x => x.s.DefaultIfEmpty(),
                            (parent, child) => new
                            {
                                product = parent.p,
                                quantity = child != null ? child.Quantity : 0
                            }
                        )
                        .Where(x =>
                            (filter.stockStatus == (int)StockStatus.stocking && x.quantity > 0) ||
                            (filter.stockStatus == (int)StockStatus.stockOut && x.quantity <= 0)
                        )
                        .Select(x => x.product);
                }
                #endregion

                #region Lọc với thời gian khởi tạo sản phẩm
                if (!String.IsNullOrEmpty(filter.productDate))
                {
                    DateTime fromdate = DateTime.Today;
                    DateTime todate = DateTime.Now;
                    CalDate(filter.productDate, ref fromdate, ref todate);

                    product = product
                        .Where(x =>
                            (x.product.CreatedDate >= fromdate && x.product.CreatedDate <= todate) ||
                            (
                                x.variable != null &&
                                (
                                    x.variable.CreatedDate >= fromdate &&
                                    x.variable.CreatedDate <= todate
                                )
                            )
                        );
                }
                #endregion

                #region Lọc với màu
                if (!String.IsNullOrEmpty(filter.color))
                {
                    product = product
                            .Where(x => x.product.ProductStyle == 2)
                            .Where(x => x.variable.color == filter.color.Trim().ToLower());
                }
                #endregion

                #region Lọc với size
                if (!String.IsNullOrEmpty(filter.size))
                {
                    product = product
                            .Where(x => x.product.ProductStyle == 2)
                            .Where(x => x.variable.size == filter.size.Trim().ToLower());
                }
                #endregion

                #region Lọc với sắp xếp kệ
                if (filter.floor > 0)
                {
                    var temp = product
                        .Join(
                            con.ShelfManagers.Where(x => x.ProductVariableID == 0),
                            p => new
                            {
                                productID = p.product.ID,
                                variableID = p.variable != null ? p.variable.ID : 0
                            },
                            s => new
                            {
                                productID = s.ProductID,
                                variableID = s.ProductVariableID
                            },
                            (p, s) => new { p, s }
                        )
                        .Where(x => x.s.Floor == filter.floor);

                    if (filter.row > 0)
                    {
                        temp = temp.Where(x => x.s.Row == filter.row);

                        if (filter.shelf > 0)
                        {
                            temp = temp.Where(x => x.s.Shelf == filter.shelf);

                            if (filter.floorShelf > 0)
                            {
                                temp = temp.Where(x => x.s.FloorShelf == filter.floorShelf);
                            }
                        }
                    }

                    product = product
                        .Join(
                            temp.Select(x => x.p),
                            p => new
                            {
                                productID = p.product.ID,
                                variableID = p.variable != null ? p.variable.ID : 0
                            },
                            t => new
                            {
                                productID = t.product.ID,
                                variableID = t.variable != null ? t.variable.ID : 0
                            },
                            (p, t) => p
                        );
                }
                #endregion

                #region Lấy những thông tin cần thiết để tiếp tục phân trang
                var dataPagination = product
                    .Select(x => new
                    {
                        categoryID = x.product.CategoryID.Value,
                        productID = x.product.ID,
                        productVariableID = x.variable != null ? x.variable.ID : 0,
                        sku = x.variable != null ? x.variable.SKU : x.product.ProductSKU,
                        title = x.product.ProductTitle,
                        image = x.variable != null ? x.variable.Image : x.product.ProductImage,
                        materials = x.product.Materials,
                        content = x.product.ProductContent,
                        costOfGood = x.variable != null ? x.variable.CostOfGood.Value : x.product.CostOfGood.Value,
                        regularPrice = x.variable != null ? x.variable.Regular_Price.Value : x.product.Regular_Price.Value,
                        retailPrice = x.variable != null ? x.variable.RetailPrice.Value : x.product.Retail_Price.Value,
                        craeteDate = x.variable != null ? x.variable.CreatedDate.Value : x.product.CreatedDate.Value
                    }
                    );
                #endregion

                #region Tính toán phân trang
                // Calculate pagination
                page.totalCount = dataPagination.Count();
                page.totalPages = (int)Math.Ceiling(page.totalCount / (double)page.pageSize);

                dataPagination = dataPagination
                   .OrderByDescending(o => new { o.productID, o.productVariableID })
                   .Skip((page.currentPage - 1) * page.pageSize)
                   .Take(page.pageSize);
                #endregion

                #region Lấy thông tin kệ
                var shelf = con.ShelfManagers
                    .Join(
                        con.CategoryShelves.Where(x => x.Level == 1),
                        s => s.Floor,
                        c => c.ID,
                        (s, c) => new
                        {
                            shelf = s,
                            floorName = c.Name
                        }
                    )
                    .Join(
                        con.CategoryShelves.Where(x => x.Level == 2),
                        t1 => t1.shelf.Row,
                        c => c.ID,
                        (t1, c) => new
                        {
                            shelf = t1.shelf,
                            floorName = t1.floorName,
                            rowName = c.Name
                        }
                    )
                    .Join(
                        con.CategoryShelves.Where(x => x.Level == 3),
                        t2 => t2.shelf.Shelf,
                        c => c.ID,
                        (t2, c) => new
                        {
                            shelf = t2.shelf,
                            floorName = t2.floorName,
                            rowName = t2.rowName,
                            shelfName = c.Name
                        }
                    )
                    .Join(
                        con.CategoryShelves.Where(x => x.Level == 4),
                        t3 => t3.shelf.FloorShelf,
                        c => c.ID,
                        (t3, c) => new
                        {
                            shelf = t3.shelf,
                            floorName = t3.floorName,
                            rowName = t3.rowName,
                            shelfName = t3.shelfName,
                            floorShelfName = c.Name
                        }
                    );
                #endregion

                var result = dataPagination
                    .Join(
                        con.tbl_Category,
                        d => d.categoryID,
                        c => c.ID,
                        (d, c) => new { d, c }
                    )
                    .GroupJoin(
                        shelf,
                        t1 => new { productID = t1.d.productID, productVariable = t1.d.productVariableID },
                        s => new { productID = s.shelf.ProductID, productVariable = s.shelf.ProductVariableID },
                        (t1, s) => new { t1, s }
                    )
                    .SelectMany(
                        x => x.s.DefaultIfEmpty(),
                        (parent, child) => new ProductShelf
                        {
                            CategoryID = parent.t1.d.categoryID,
                            CategoryName = parent.t1.c.CategoryName,
                            ProductID = parent.t1.d.productID,
                            ProductVariable = parent.t1.d.productVariableID,
                            SKU = parent.t1.d.sku,
                            Title = parent.t1.d.title,
                            Image = parent.t1.d.image,
                            Materials = parent.t1.d.materials,
                            Content = parent.t1.d.content,
                            Quantity = child != null ? child.shelf.Quantity : 0,
                            CostOfGood = parent.t1.d.costOfGood,
                            RegularPrice = parent.t1.d.regularPrice,
                            RetailPrice = parent.t1.d.retailPrice,
                            CreatedDate = parent.t1.d.craeteDate,
                            Floor = child != null ? child.shelf.Floor : 0,
                            FloorName = child != null ? child.floorName : String.Empty,
                            Row = child != null ? child.shelf.Row : 0,
                            RowName = child != null ? child.rowName : String.Empty,
                            Shelf = child != null ? child.shelf.Shelf : 0,
                            ShelfName = child != null ? child.shelfName : String.Empty,
                            FloorShelf = child != null ? child.shelf.FloorShelf : 0,
                            FloorShelfName = child != null ? child.floorShelfName : String.Empty
                        }
                    )
                    .OrderByDescending(o => new { o.ProductID, o.ProductVariable })
                    .ToList();

                return result;
            }
        }

        public static List<Product> GetProductShelf(ProductFilterModel filter, bool useChangeShelf = false)
        {
            using (var con = new inventorymanagementEntities())
            {
                var productFilter = con.tbl_Product
                    .Where(x =>
                        x.ProductSKU.ToLower() == filter.search.Trim().ToLower()
                    )
                    .FirstOrDefault();

                var productVariableFilter = con.tbl_ProductVariable
                    .Where(x =>
                        x.SKU.ToLower().Contains(filter.search.Trim().ToLower())
                    )
                    .FirstOrDefault();

                if (productFilter == null && productVariableFilter == null)
                    return null;

                #region Lấy thông tin sản phẩm
                Product data;
                if (productVariableFilter == null)
                {
                    // Trường hợp sản phẩm không có biến thể
                    data = new Product()
                    {
                        productID = productFilter.ID,
                        variableID = 0,
                        sku = productFilter.ProductSKU,
                        title = productFilter.ProductTitle,
                        image = productFilter.ProductImage,
                        color = String.Empty,
                        size = String.Empty,
                    };
                }
                else
                {
                    // Trường hợp có biến thể
                    var color = con.tbl_ProductVariableValue.Where(x => x.ProductVariableID == productVariableFilter.ID)
                        .Join(
                            con.tbl_VariableValue.Where(x => x.VariableID == 1), // Chỉ lấy màu
                            p => p.VariableValueID,
                            v => v.ID,
                            (p, v) => v.VariableValue
                        )
                        .FirstOrDefault();

                    var size = con.tbl_ProductVariableValue.Where(x => x.ProductVariableID == productVariableFilter.ID)
                        .Join(
                            con.tbl_VariableValue.Where(x => x.VariableID == 2), // Chỉ lấy size
                            p => p.VariableValueID,
                            v => v.ID,
                            (p, v) => v.VariableValue
                        )
                        .FirstOrDefault();

                    var title = con.tbl_Product
                        .Where(x => x.ID == productVariableFilter.ProductID)
                        .Select(x => x.ProductTitle)
                        .FirstOrDefault();

                    data = new Product()
                    {
                        productID = productVariableFilter.ProductID.Value,
                        variableID = productVariableFilter.ID,
                        sku = productVariableFilter.SKU,
                        title = title,
                        image = productVariableFilter.Image,
                        color = color,
                        size = size,
                    };
                }
                #endregion

                #region Lấy thông tin kệ
                var shelf = con.ShelfManagers
                    .Where(x => x.ProductID == data.productID && x.ProductVariableID == data.variableID)
                    .Join(
                        con.CategoryShelves.Where(x => x.Level == 1),
                        s => s.Floor,
                        c => c.ID,
                        (s, c) => new
                        {
                            shelf = s,
                            floorName = c.Name
                        }
                    )
                    .Join(
                        con.CategoryShelves.Where(x => x.Level == 2),
                        t1 => t1.shelf.Row,
                        c => c.ID,
                        (t1, c) => new
                        {
                            shelf = t1.shelf,
                            floorName = t1.floorName,
                            rowName = c.Name
                        }
                    )
                    .Join(
                        con.CategoryShelves.Where(x => x.Level == 3),
                        t2 => t2.shelf.Shelf,
                        c => c.ID,
                        (t2, c) => new
                        {
                            shelf = t2.shelf,
                            floorName = t2.floorName,
                            rowName = t2.rowName,
                            shelfName = c.Name
                        }
                    )
                    .Join(
                        con.CategoryShelves.Where(x => x.Level == 4),
                        t3 => t3.shelf.FloorShelf,
                        c => c.ID,
                        (t3, c) => new
                        {
                            shelf = t3.shelf,
                            floorName = t3.floorName,
                            rowName = t3.rowName,
                            shelfName = t3.shelfName,
                            floorShelfName = c.Name
                        }
                    )
                    .ToList();
                #endregion

                var result = new List<Product>();
                var existShelf = false;
                foreach (var item in shelf)
                {
                    if (
                        item.shelf.Floor == filter.floor &&
                        item.shelf.Row == filter.row &&
                        item.shelf.Shelf == filter.shelf &&
                        item.shelf.FloorShelf == filter.floorShelf
                    )
                    {
                        if (!useChangeShelf)
                        {
                            existShelf = true;
                        }
                        else
                        {
                            if (
                                item.shelf.ProductID == data.productID &&
                                item.shelf.ProductVariableID == data.variableID
                                )
                            {
                                continue;
                            }
                        }
                    }

                    result.Add(new Product()
                    {
                        productID = data.productID,
                        variableID = data.variableID,
                        sku = data.sku,
                        title = data.title,
                        image = data.image,
                        color = data.color,
                        size = data.size,
                        quantity = item.shelf.Quantity,
                        floor = item.shelf.Floor,
                        floorName = item.floorName,
                        row = item.shelf.Row,
                        rowName = item.rowName,
                        shelf = item.shelf.Shelf,
                        shelfName = item.shelfName,
                        floorShelf = item.shelf.FloorShelf,
                        floorShelfName = item.floorShelfName
                    });
                }

                if (!useChangeShelf && !existShelf)
                {
                    var floorName = con.CategoryShelves
                        .Where(x => x.Level == 1 && x.ID == filter.floor)
                        .Select(x => x.Name)
                        .FirstOrDefault();
                    var rowName = con.CategoryShelves
                        .Where(x => x.Level == 2 && x.ID == filter.row)
                        .Select(x => x.Name)
                        .FirstOrDefault();
                    var shelfName = con.CategoryShelves
                        .Where(x => x.Level == 3 && x.ID == filter.shelf)
                        .Select(x => x.Name)
                        .FirstOrDefault();
                    var floorShelfName = con.CategoryShelves
                        .Where(x => x.Level == 4 && x.ID == filter.floorShelf)
                        .Select(x => x.Name)
                        .FirstOrDefault();

                    result.Add(new Product()
                    {
                        productID = data.productID,
                        variableID = data.variableID,
                        sku = data.sku,
                        title = data.title,
                        image = data.image,
                        color = data.color,
                        size = data.size,
                        quantity = 0,
                        floor = filter.floor,
                        floorName = floorName,
                        row = filter.row,
                        rowName = rowName,
                        shelf = filter.shelf,
                        shelfName = shelfName,
                        floorShelf = filter.floorShelf,
                        floorShelfName = floorShelfName
                    });
                }

                return result;
            }
        }

        public static void updateProductQuantityInShelf(List<Product> productShelf, int requester)
        {
            using (var con = new inventorymanagementEntities())
            {
                foreach (var item in productShelf)
                {
                    var prodOld = con.ShelfManagers
                        .Where(x => x.Floor == item.floor)
                        .Where(x => x.Row == item.row)
                        .Where(x => x.Shelf == item.shelf)
                        .Where(x => x.FloorShelf == item.floorShelf)
                        .Where(x => x.ProductID == item.productID)
                        .Where(x => x.ProductVariableID == item.variableID)
                        .FirstOrDefault();

                    var now = DateTime.Now;

                    if (prodOld != null)
                    {
                        prodOld.Quantity = Convert.ToInt32(item.quantity);
                        prodOld.ModifiedBy = requester;
                        prodOld.ModifiedDate = now;
                        con.SaveChanges();
                    }
                    else
                    {
                        con.ShelfManagers.Add(new ShelfManager()
                        {
                            Floor = item.floor,
                            Row = item.row,
                            Shelf = item.shelf,
                            FloorShelf = item.floorShelf,
                            ProductID = item.productID,
                            ProductVariableID = item.variableID,
                            Quantity = Convert.ToInt32(item.quantity),
                            CreatedBy = requester,
                            CreatedDate = now,
                            ModifiedBy = requester,
                            ModifiedDate = now
                        });
                        con.SaveChanges();
                    }
                }
            }
        }

        public static void updateChangeShelf(List<Product> productShelf, int floor, int row, int shelf, int floorShelf, int requester)
        {
            using (var con = new inventorymanagementEntities())
            {
                foreach (var item in productShelf)
                {
                    var prod = con.ShelfManagers
                        .Where(x => x.Floor == item.floor)
                        .Where(x => x.Row == item.row)
                        .Where(x => x.Shelf == item.shelf)
                        .Where(x => x.FloorShelf == item.floorShelf)
                        .Where(x => x.ProductID == item.productID)
                        .Where(x => x.ProductVariableID == item.variableID)
                        .FirstOrDefault();

                    var now = DateTime.Now;

                    if (prod != null)
                    {
                        var prodChage = con.ShelfManagers
                            .Where(x => x.Floor == floor)
                            .Where(x => x.Row == row)
                            .Where(x => x.Shelf == shelf)
                            .Where(x => x.FloorShelf == floorShelf)
                            .Where(x => x.ProductID == item.productID)
                            .Where(x => x.ProductVariableID == item.variableID)
                            .FirstOrDefault();

                        if (prodChage != null)
                        {
                            prodChage.Quantity += Convert.ToInt32(item.quantity);
                            prodChage.ModifiedBy = requester;
                            prodChage.ModifiedDate = now;
                        }
                        else
                        {
                            con.ShelfManagers.Add(new ShelfManager()
                            {
                                Floor = floor,
                                Row = row,
                                Shelf = shelf,
                                FloorShelf = floorShelf,
                                ProductID = item.productID,
                                ProductVariableID = item.variableID,
                                Quantity = Convert.ToInt32(item.quantity),
                                CreatedBy = requester,
                                CreatedDate = now,
                                ModifiedBy = requester,
                                ModifiedDate = now
                            });
                        }

                        con.ShelfManagers.Remove(prod);
                        con.SaveChanges();
                    }
                }
            }
        }

        public static List<Product> GetAllProduct(ProductFilterModel filter, ref PaginationMetadataModel page)
        {
            using (var con = new inventorymanagementEntities())
            {
                var product = con.tbl_Product
                    .GroupJoin(
                        con.tbl_ProductVariable,
                        p => new { productID = p.ID, productStyle = p.ProductStyle.Value },
                        v => new { productID = v.ProductID.Value, productStyle = 2 },
                        (p, v) => new { p, v }
                    )
                    .SelectMany(
                        x => x.v.DefaultIfEmpty(),
                        (parent, child) => new
                        {
                            product = parent.p,
                            variable = child
                        }
                    );

                #region Thực thi triết xuất dữ liệu
                #region Lọc với text search
                if (!String.IsNullOrEmpty(filter.search))
                {
                    var search = filter.search.Trim().ToLower();
                    product = product
                        .Where(x =>
                            x.product.ProductSKU.Trim().ToLower().Contains(search) ||
                            x.variable.SKU.Trim().ToLower().Contains(search) ||
                            x.product.UnSignedTitle.Trim().ToLower().Contains(search)
                        );
                }
                #endregion

                #region Lọc với danh mục
                if (filter.category > 0)
                {
                    var parentCatogory = con.tbl_Category.Where(x => x.ID == filter.category).FirstOrDefault();
                    var catogoryFilter = CategoryController.getCategoryChild(parentCatogory).Select(x => x.ID).ToList();

                    product = product
                        .Where(x =>
                            catogoryFilter.Contains(
                                x.product.CategoryID.HasValue ? x.product.CategoryID.Value : 0
                                )
                        );
                }
                #endregion

                //#region Lọc với số lượng còn hay hết dựa theo kiểm kệ gần nhất
                //if (filter.stockStatus > 0)
                //{
                //    product = product
                //        .GroupJoin(
                //            con.ShelfManagers.Where(x => x.ProductVariableID == 0),
                //            p => new
                //            {
                //                productID = p.product.ID,
                //                variableID = p.variable != null ? p.variable.ID : 0
                //            },
                //            s => new
                //            {
                //                productID = s.ProductID,
                //                variableID = s.ProductVariableID
                //            },
                //            (p, s) => new { p, s }
                //        )
                //        .SelectMany(
                //            x => x.s.DefaultIfEmpty(),
                //            (parent, child) => new
                //            {
                //                product = parent.p,
                //                quantity = child != null ? child.Quantity : 0
                //            }
                //        )
                //        .Where(x =>
                //            (filter.stockStatus == (int)StockStatus.stocking && x.quantity > 0) ||
                //            (filter.stockStatus == (int)StockStatus.stockOut && x.quantity <= 0)
                //        )
                //        .Select(x => x.product)
                //        .Distinct();
                //}
                //#endregion

                #region Lọc với thời gian khởi tạo sản phẩm
                if (!String.IsNullOrEmpty(filter.productDate))
                {
                    DateTime fromdate = DateTime.Today;
                    DateTime todate = DateTime.Now;
                    CalDate(filter.productDate, ref fromdate, ref todate);

                    product = product
                        .Where(x =>
                            (x.product.CreatedDate >= fromdate && x.product.CreatedDate <= todate) ||
                            (
                                x.variable != null &&
                                (
                                    x.variable.CreatedDate >= fromdate &&
                                    x.variable.CreatedDate <= todate
                                )
                            )
                        );
                }
                #endregion

                #region Lọc với màu
                if (!String.IsNullOrEmpty(filter.color))
                {
                    var productColor = con.tbl_VariableValue
                        .Where(x => x.VariableID == 1)
                        .Where(x => x.VariableValue.ToLower().Contains(filter.color.Trim().ToLower()))
                        .Join(
                            con.tbl_ProductVariableValue,
                            vv => vv.ID,
                            pvv => pvv.VariableValueID,
                            (vv, pvv) => pvv
                        );

                    product = product
                            .Where(x => x.product.ProductStyle == 2)
                            .Join(
                                productColor,
                                p => p.variable.ID,
                                c => c.ProductVariableID,
                                (p, c) => p
                            );
                }
                #endregion

                #region Lọc với size
                if (!String.IsNullOrEmpty(filter.size))
                {
                    var productSize = con.tbl_VariableValue
                        .Where(x => x.VariableID == 2)
                        .Where(x => x.VariableValue.ToLower().Contains(filter.size.Trim().ToLower()))
                        .Join(
                            con.tbl_ProductVariableValue,
                            vv => vv.ID,
                            pvv => pvv.VariableValueID,
                            (vv, pvv) => pvv
                        );

                    product = product
                            .Where(x => x.product.ProductStyle == 2)
                            .Join(
                                productSize,
                                p => p.variable.ID,
                                c => c.ProductVariableID,
                                (p, c) => p
                            );
                }
                #endregion

                //#region Lọc với sắp xếp kệ
                //if (filter.floor > 0)
                //{
                //    var temp = product
                //        .Join(
                //            con.ShelfManagers.Where(x => x.ProductVariableID == 0),
                //            p => new
                //            {
                //                productID = p.product.ID,
                //                variableID = p.variable != null ? p.variable.ID : 0
                //            },
                //            s => new
                //            {
                //                productID = s.ProductID,
                //                variableID = s.ProductVariableID
                //            },
                //            (p, s) => new { p, s }
                //        )
                //        .Where(x => x.s.Floor == filter.floor);

                //    if (filter.row > 0)
                //    {
                //        temp = temp.Where(x => x.s.Row == filter.row);

                //        if (filter.shelf > 0)
                //        {
                //            temp = temp.Where(x => x.s.Shelf == filter.shelf);

                //            if (filter.floorShelf > 0)
                //            {
                //                temp = temp.Where(x => x.s.FloorShelf == filter.floorShelf);
                //            }
                //        }
                //    }

                //    product = product
                //        .Join(
                //            temp.Select(x => x.p),
                //            p => new
                //            {
                //                productID = p.product.ID,
                //                variableID = p.variable != null ? p.variable.ID : 0
                //            },
                //            t => new
                //            {
                //                productID = t.product.ID,
                //                variableID = t.variable != null ? t.variable.ID : 0
                //            },
                //            (p, t) => p
                //        );
                //}
                //#endregion

                #region Lấy những sẩn phẩm được yêu cầu nhập hàng theo người khởi tạo
                if (filter.createdBy > 0)
                {
                    var registerProduct = con.RegisterProducts.Where(x => x.CreatedBy == filter.createdBy);

                    product = product
                        .Join(
                            registerProduct,
                            p => new
                            {
                                productID = p.product.ID,
                                variableID = p.variable != null ? p.variable.ID : 0
                            },
                            r => new
                            {
                                productID = r.ProductID,
                                variableID = r.VariableID
                            },
                            (p, r) => p
                        );
                }
                #endregion
                #endregion

                #region Lấy những thông tin cần thiết để tiếp tục phân trang
                var data = product
                    .Select(x => new
                    {
                        categoryID = x.product.CategoryID.Value,
                        productID = x.product.ID,
                        variableID = x.variable != null ? x.variable.ID : 0,
                        sku = x.variable != null ? x.variable.SKU : x.product.ProductSKU,
                        title = x.product.ProductTitle,
                        image = x.variable != null ? x.variable.Image : x.product.ProductImage,
                        materials = x.product.Materials,
                        content = x.product.ProductContent,
                        costOfGood = x.variable != null ? x.variable.CostOfGood.Value : x.product.CostOfGood.Value,
                        regularPrice = x.variable != null ? x.variable.Regular_Price.Value : x.product.Regular_Price.Value,
                        retailPrice = x.variable != null ? x.variable.RetailPrice.Value : x.product.Retail_Price.Value,
                        craeteDate = x.variable != null ? x.variable.CreatedDate.Value : x.product.CreatedDate.Value,
                        productType = x.product.ProductStyle,
                        orderIndex = x.product.ProductStyle == 1 ? 1 : 2, // Dùng để sắp xếp cho sản phẩm con
                        numberChild = 1, // Số lượng biết thể con
                    }
                    );

                var variable = data.Where(x => x.productType == 2)
                    .Select(x => new { productID = x.productID, numberChild = x.numberChild })
                    .GroupBy(g => g.productID)
                    .Select(x => new { productID = x.Key, numberChild = x.Sum(s => s.numberChild) });
                #endregion

                #region Add thêm dòng biến thể cha của các sản phẩm con
                data = data.Union(
                    product.Where(x => x.product.ProductStyle == 2)
                        .Distinct()
                        .Select(x => new
                        {
                            categoryID = x.product.CategoryID.Value,
                            productID = x.product.ID,
                            variableID = 0,
                            sku = x.product.ProductSKU,
                            title = x.product.ProductTitle,
                            image = x.product.ProductImage,
                            materials = x.product.Materials,
                            content = x.product.ProductContent,
                            costOfGood = x.product.CostOfGood.Value,
                            regularPrice = x.product.Regular_Price.Value,
                            retailPrice = x.product.Retail_Price.Value,
                            craeteDate = x.product.CreatedDate.Value,
                            productType = x.product.ProductStyle,
                            orderIndex = 1,
                            numberChild = 0
                        })
                );
                #endregion
                #region Tính toán phân trang
                // Calculate pagination
                page.totalCount = data.Count();
                page.totalPages = (int)Math.Ceiling(page.totalCount / (double)page.pageSize);

                var dataPagination = data
                   .OrderByDescending(o => o.productID)
                   .ThenBy(o => o.orderIndex)
                   .ThenByDescending(o => o.variableID)
                   .Skip((page.currentPage - 1) * page.pageSize)
                   .Take(page.pageSize);

                variable = variable
                    .Join(
                        dataPagination.Where(x => x.variableID == 0 && x.productType == 2),
                        v => v.productID,
                        d => d.productID,
                        (v, d) => v
                    );
                #endregion

                #region Lấy thông tin của biến thể
                var colors = con.tbl_ProductVariableValue
                    .Join(
                        data.Where(x => x.variableID > 0),
                        pvv => pvv.ProductVariableID,
                        d => d.variableID,
                        (pvv, d) => pvv
                    )
                    .Join(
                        con.tbl_VariableValue.Where(x => x.VariableID == 1),
                        pvv => pvv.VariableValueID,
                        vv => vv.ID,
                        (pvv, vv) => new
                        {
                            variableID = pvv.ProductVariableID,
                            color = vv.VariableValue
                        }
                    )
                    .ToList();

                var sizes = con.tbl_ProductVariableValue
                    .Join(
                        data.Where(x => x.variableID > 0),
                        pvv => pvv.ProductVariableID,
                        d => d.variableID,
                        (pvv, d) => pvv
                    )
                    .Join(
                        con.tbl_VariableValue.Where(x => x.VariableID == 2),
                        pvv => pvv.VariableValueID,
                        vv => vv.ID,
                        (pvv, vv) => new
                        {
                            variableID = pvv.ProductVariableID,
                            size = vv.VariableValue
                        }
                    )
                    .ToList();
                #endregion

                var result = dataPagination
                    .GroupJoin(
                        variable,
                        d => new { productID = d.productID, variableID = d.variableID, productType = d.productType.Value },
                        v => new { productID = v.productID, variableID = 0, productType = 2 },
                        (d, v) => new { d, v}
                    )
                    .SelectMany(
                        x => x.v.DefaultIfEmpty(),
                        (parent, child) => new {
                            categoryID = parent.d.categoryID,
                            productID = parent.d.productID,
                            variableID = parent.d.variableID,
                            sku = parent.d.sku,
                            title = parent.d.title,
                            image = parent.d.image,
                            materials = parent.d.materials,
                            content = parent.d.content,
                            costOfGood = parent.d.costOfGood,
                            regularPrice = parent.d.regularPrice,
                            retailPrice = parent.d.retailPrice,
                            craeteDate = parent.d.craeteDate,
                            productType = parent.d.productType,
                            orderIndex = parent.d.orderIndex,
                            numberChild = child != null ? child.numberChild : parent.d.numberChild
                        }
                    )
                    .ToList()
                    .GroupJoin(
                        colors,
                        d => d.variableID,
                        c => c.variableID,
                        (d, c) => new { product = d, color = c }
                    )
                    .SelectMany(
                        x => x.color.DefaultIfEmpty(),
                        (parent, child) => new { product = parent.product, color = child != null ? child.color : String.Empty }
                    )
                    .GroupJoin(
                        sizes,
                        t1 => t1.product.variableID,
                        s => s.variableID,
                        (t1, s) => new { product = t1.product, color = t1.color, size = s }
                    )
                    .SelectMany(
                        x => x.size.DefaultIfEmpty(),
                        (parent, child) => new { product = parent.product, color = parent.color, size = child != null ? child.size : String.Empty }
                    )
                    .Select(x => {
                        var stock = StockManagerController.getStock(x.product.productID, x.product.variableID);
                        double quantity = 0;

                        if (stock.Count > 0)
                        {
                            quantity = stock
                            .Select(s => s.Type == 2 ? (s.QuantityCurrent.Value - s.Quantity.Value) : (s.QuantityCurrent.Value + s.Quantity.Value))
                            .Sum(s => s);
                        }

                        return new Product()
                        {
                            productID = x.product.productID,
                            variableID = x.product.variableID,
                            sku = x.product.sku,
                            title = x.product.title,
                            image = x.product.image,
                            color = x.color,
                            size = x.size,
                            materials = x.product.materials,
                            content = x.product.content,
                            quantity = quantity,
                            costOfGood = x.product.costOfGood,
                            regularPrice = x.product.regularPrice,
                            retailPrice = x.product.retailPrice,
                            createdDate = x.product.craeteDate,
                            productStyle = x.product.productType.Value,
                            numberChild = x.product.numberChild
                        };
                    })
                    .ToList();

                return result;
            }
        }
        #endregion
        #region Class
        public class Product
        {
            public int productID { get; set; }
            public int variableID { get; set; }
            public string sku { get; set; }
            public string title { get; set; }
            public string image { get; set; }
            public string color { get; set; }
            public string size { get; set; }
            public double quantity { get; set; }
            public string materials { get; set; }
            public string content { get; set; }
            public double costOfGood { get; set; }
            public double regularPrice { get; set; }
            public double retailPrice { get; set; }
            public int floor { get; set; }
            public string floorName { get; set; }
            public int row { get; set; }
            public string rowName { get; set; }
            public int shelf { get; set; }
            public string shelfName { get; set; }
            public int floorShelf { get; set; }
            public string floorShelfName { get; set; }
            public DateTime createdDate { get; set; }
            public int productStyle { get; set; }
            // Dùng để tính toán tổng sản phẩm dăng ký với ký đủ màu, đủ size
            public int numberChild { get; set; }

        }
        public class ProductShelf
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; }
            public int ProductID { get; set; }
            public int ProductVariable { get; set; }
            public string SKU { get; set; }
            public string Title { get; set; }
            public string Image { get; set; }
            public string Materials { get; set; }
            public string Content { get; set; }
            public double Quantity { get; set; }
            public double CostOfGood { get; set; }
            public double RegularPrice { get; set; }
            public double RetailPrice { get; set; }
            public DateTime CreatedDate { get; set; }
            public int Floor { get; set; }
            public string FloorName { get; set; }
            public int Row { get; set; }
            public string RowName { get; set; }
            public int Shelf { get; set; }
            public string ShelfName { get; set; }
            public int FloorShelf { get; set; }
            public string FloorShelfName { get; set; }

        }
        public class RegisterProduct
        {
            public string customer { get; set; }
            public int productID { get; set; }
            public int variableID { get; set; }
            public string sku { get; set; }
            public string title { get; set; }
            public string image { get; set; }
            public string color { get; set; }
            public string size { get; set; }
            public double quantity { get; set; }
            // Trạng thái đơn yêu cầu nhập hàng
            public int status { get; set; }
            // Ngày bên xưởng giao hàng
            public int expectedDate { get; set; }
            public string note { get; set; }
            // Người tạo yêu cầu nhập hàng
            public int createdBy { get; set; }
            // Ngày tạo yêu cầu nhập hàng
            public DateTime createdDate { get; set; }
            // Người chỉnh yêu cầu nhập hàng
            public int modifiedBy { get; set; }
            // Ngày chỉnh yêu cầu nhập hàng
            public DateTime modifiedDate { get; set; }
        }
        public class ProductSQL
        {
            public int ID { get; set; }
            public string ProductImage { get; set; }
            public string ProductTitle { get; set; }
            public string ProductSKU { get; set; }
            public string ProductInstockStatus { get; set; }
            public double TotalProductInstockQuantityLeft { get; set; }
            public string ProductContent { get; set; }
            public double RegularPrice { get; set; }
            public double CostOfGood { get; set; }
            public double RetailPrice { get; set; }
            public int CategoryID { get; set; }
            public string CategoryName { get; set; }
            public DateTime CreatedDate { get; set; }
            public int StockStatus { get; set; }
            public int ProductStyle { get; set; }
            public int ShowHomePage { get; set; }
            public string Materials { get; set; }
            public string ProductImageClean { get; set; }
            public bool WebPublish { get; set; }
            public DateTime WebUpdate { get; set; }
        }

        public class ProductStockReport
        {
            public int totalStock { get; set; }
            public double totalStockValue { get; set; }
        }

        public class ProductStock
        {
            public string ProductInstockStatus { get; set; }
            public int quantityLeft { get; set; }
        }
        #endregion
    }
}