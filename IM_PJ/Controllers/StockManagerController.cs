#region .NET Framework
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
#endregion

#region Utils
using WebUI.Business;
#endregion

#region ANN Shop
using IM_PJ.Models;
using IM_PJ.Models.Pages.thong_ke_nhap_kho;
using IM_PJ.Models.Pages.thong_ke_chuyen_kho;
#endregion

namespace IM_PJ.Controllers
{
    public class StockManagerController
    {
        #region Quản lý kho 1

        #region Create
        public static tbl_StockManager Insert(tbl_StockManager data)
        {
            #region Kiểm tra xem sản phẩm đã nhập vô kho chưa
            if (hasExistedProduct(data.ParentID.Value, data.ProductVariableID))
                return Update(data);
            #endregion

            #region Khởi tạo thông tin kho với sản phẩm mới
            try
            {
                using (var context = new inventorymanagementEntities())
                {
                    if (data.Type == 2) // Output SKU
                    {
                        data.Type = 1;
                        data.Quantity = 0;
                    }

                    data.QuantityCurrent = 0;
                    context.Entry(data).State = System.Data.Entity.EntityState.Added;
                    context.tbl_StockManager.Add(data);
                    context.SaveChanges();

                    return data;
                }
            }
            catch (OptimisticConcurrencyException)
            {
                Thread.Sleep(500);
                return Insert(data);
            }
            catch (Exception e)
            {
                throw e;
            }
            #endregion
        }

        public static List<tbl_StockManager> Insert(List<tbl_StockManager> data)
        {

            var stocks = new List<tbl_StockManager>();

            foreach (var item in data)
                stocks.Add(Insert(item));

            return stocks;
        }

        public static tbl_StockManager warehousing1(tbl_StockManager data)
        {
            return Insert(data);
        }

        // Thực hiện quản lý xuất / nhập sản phẩm kho 2
        public static tbl_StockManager warehousing1(StockManager2 data)
        {
            if (hasExistedProduct(data.ProductID, data.ProductVariableID))
                return updateStock1(data);

            #region Thực hiện tạo stock 1 mới cho sản phẩm
            var stock1 = new tbl_StockManager()
            {
                AgentID = data.AgentID,
                ProductID = data.ProductVariableID == 0 ? data.ProductID : 0,
                ProductVariableID = data.ProductVariableID,
                Quantity = data.QuantityCurrent + data.Quantity * (data.Type == 1 ? 1 : -1),
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

            return Insert(stock1);
            #endregion
        }
        #endregion

        #region Update
        public static tbl_StockManager Update(tbl_StockManager data)
        {
            using (var context = new inventorymanagementEntities())
            {
                try
                {
                    #region Lock dữ liệu để tính toán
                    var stock = context.tbl_StockManager
                        .Where(x => x.ParentID == data.ParentID)
                        .Where(x => x.ProductVariableID == data.ProductVariableID)
                        .OrderByDescending(x => x.CreatedDate)
                        .FirstOrDefault();

                    context.Entry(stock).State = System.Data.Entity.EntityState.Modified;
                    #endregion

                    #region Tính số lượng kho
                    // Số lượng hiện tại trong kho
                    var quantityCurrent = stock.QuantityCurrent.Value + stock.Quantity.Value * (stock.Type == 1 ? 1 : -1);
                    quantityCurrent = quantityCurrent < 0 ? 0 : quantityCurrent;
                    // Số lượng kho sau khi update
                    var newQuantityCurrent = quantityCurrent + data.Quantity.Value * (data.Type == 1 ? 1 : -1);
                    // Trường hợp là số lượng sản phẩm âm
                    // Type = 2 thì cói như số lượng hiện tại bằng số lượng xuất kho
                    newQuantityCurrent = newQuantityCurrent < 0 ? data.Quantity.Value : quantityCurrent;
                    #endregion

                    #region Cập nhật thông tin kho
                    stock.Type = data.Type;
                    stock.Quantity = data.Quantity;
                    stock.QuantityCurrent = newQuantityCurrent;
                    stock.NoteID = data.NoteID;
                    stock.OrderID = data.OrderID;
                    stock.Status = data.Status;
                    stock.ModifiedDate = data.ModifiedDate;
                    stock.ModifiedBy = data.ModifiedBy;
                    context.SaveChanges();
                    #endregion

                    return stock;
                }
                catch (OptimisticConcurrencyException)
                {
                    Thread.Sleep(500);
                    return Update(data);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static tbl_StockManager updateStock1(StockManager2 data)
        {
            using (var context = new inventorymanagementEntities())
            {
                try
                {
                    #region Lock dữ liệu để tính toán
                    var stock = context.tbl_StockManager
                        .Where(x => x.ParentID == data.ProductID)
                        .Where(x => x.ProductVariableID == data.ProductVariableID)
                        .OrderByDescending(x => x.CreatedDate)
                        .FirstOrDefault();

                    context.Entry(stock).State = System.Data.Entity.EntityState.Modified;
                    #endregion

                    #region Thông tin dữ liệu củ về tồn kho của sản phẩm
                    var quantityCurrent = stock.QuantityCurrent.HasValue ? stock.QuantityCurrent.Value : 0;
                    var type = stock.Type.HasValue ? stock.Type.Value : 1;
                    var quantity = stock.Quantity.HasValue ? stock.Quantity.Value : 0;
                    #endregion

                    #region Tính số lượng kho
                    var newQuantityCurrent = quantityCurrent + quantity * (type == 1 ? 1 : -1);
                    newQuantityCurrent = newQuantityCurrent < 0 ? 0 : newQuantityCurrent;
                    var newQuantity = data.QuantityCurrent + data.Quantity * (data.Type == 1 ? 1 : -1) - newQuantityCurrent;
                    #endregion

                    #region Cập nhật thông tin kho
                    stock.Type = newQuantity > 0 ? 1 : 2;
                    stock.Quantity = Math.Abs(newQuantity);
                    stock.QuantityCurrent = newQuantityCurrent;
                    stock.NoteID = data.Note;
                    stock.Status = data.Status;
                    stock.ModifiedDate = data.ModifiedDate;
                    stock.ModifiedBy = data.ModifiedBy;
                    context.SaveChanges();
                    #endregion

                    return stock;
                }
                catch (OptimisticConcurrencyException)
                {
                    Thread.Sleep(500);
                    return updateStock1(data);
                }
                catch (Exception e)
                {
                    throw e;
                }
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
        #endregion

        #region Delete
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

        public static bool deleteStock1(int productId, int? variationId)
        {
            #region Kiểm tra xem sản phẩm đã nhập vô kho chưa
            if (!hasExistedProduct(productId, variationId))
                return true;
            #endregion

            try
            {
                using (var context = new inventorymanagementEntities())
                {
                    #region Lock dữ liệu để tính toán
                    var stock = context.tbl_StockManager
                        .Where(x => x.ParentID == productId)
                        .Where(x => x.ProductVariableID == variationId)
                        .OrderByDescending(x => x.CreatedDate)
                        .FirstOrDefault();

                    context.Entry(stock).State = System.Data.Entity.EntityState.Deleted;
                    #endregion

                    #region Thực hiện xóa stock
                    context.tbl_StockManager.Remove(stock);
                    context.SaveChanges();
                    #endregion

                    return true;
                }
            }
            catch (OptimisticConcurrencyException)
            {
                Thread.Sleep(500);
                return deleteStock1(productId, variationId);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Get
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

        /// <summary>
        /// Lấy stock của những sản phẩm con theo SKU Parent
        /// </summary>
        /// <param name="parentSKU">SKU parent</param>
        /// <returns></returns>
        public static List<tbl_StockManager> warehousing1ByParentSKU(string parentSKU)
        {
            using (var con = new inventorymanagementEntities())
            {
                var stock = con.tbl_Product
                    .Where(x => x.ProductSKU == parentSKU)
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

        public static List<tbl_StockManager> GetByProductVariableID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_StockManager> ags = new List<tbl_StockManager>();
                ags = dbe.tbl_StockManager.Where(i => i.ProductVariableID == ID).ToList();
                return ags;
            }
        }

        public static tbl_StockManager getVariationById(int productID, int variationId)
        {
            using (var context = new inventorymanagementEntities())
            {
                var stock = context.tbl_StockManager
                    .Where(x => x.ParentID == productID)
                    .Where(x => x.ProductVariableID == variationId)
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefault();

                return stock;
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

        /// <summary>
        /// Kiểm tra xem sản phẩm đã nhập vô kho chưa
        /// </summary>
        /// <param name="productId">Parent ID</param>
        /// <param name="variationId">Variation ID</param>
        /// <returns></returns>
        public static bool hasExistedProduct(int productId, int? variationId)
        {
            using (var context = new inventorymanagementEntities())
            {
                return context.tbl_StockManager
                    .Where(x => x.ParentID == productId)
                    .Where(x => x.ProductVariableID == variationId)
                    .Any();
            }
        }

        /// <summary>
        /// Lấy thông tin chuyển kho
        /// </summary>
        /// <param name="filter">Thôi tin lọc sản phẩm</param>
        /// <param name="page">Thông tin phân trang</param>
        /// <param name="totalQuantityInput">Tổng số lượng nhập kho</param>
        /// <returns></returns>
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
        public static bool liquidateProduct(string userName, int productId, int variationId)
        {
            using (var context = new inventorymanagementEntities())
            {
                try
                {
                    var stock = getVariationById(productId, variationId);

                    if (stock == null)
                        return false;

                    context.Entry(stock).State = System.Data.Entity.EntityState.Modified;
                    var quantity = stock.QuantityCurrent + stock.Quantity * (stock.Type == 1 ? 1 : -1);

                    stock.QuantityCurrent = quantity.HasValue ? Math.Abs(quantity.Value) : 0;
                    stock.Quantity = quantity.HasValue ? Math.Abs(quantity.Value) : 0;
                    stock.Type = 2;
                    stock.Status = 14;
                    stock.NoteID = "Xuất hết kho";
                    stock.ModifiedBy = userName;
                    stock.ModifiedDate = DateTime.Now;
                    context.SaveChanges();

                    return true;
                }
                catch (OptimisticConcurrencyException)
                {
                    Thread.Sleep(500);
                    return liquidateProduct(userName, productId, variationId);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool liquidateProduct(tbl_Account acc, int productID)
        {
            var success = true;
            var stocks = getStock(productID, 0);

            if (stocks.Count == 0)
                return true;

            foreach (var item in stocks)
                if (!liquidateProduct(acc.Username, item.ParentID.Value, item.ProductVariableID.HasValue ? item.ProductVariableID.Value : 0))
                    success = false;

            return success;
        }
        #endregion

        #region Phục hồi lại các sản phẩm đã xã kho
        public static bool recoverLiquidatedProduct(string userName, int productId, int variationId)
        {
            using (var context = new inventorymanagementEntities())
            {
                try
                {
                    var stock = getVariationById(productId, variationId);

                    if (stock == null)
                        return false;

                    if (stock.Status != 14)
                        return false;

                    context.Entry(stock).State = System.Data.Entity.EntityState.Modified;
                    stock.Type = 1;
                    stock.QuantityCurrent = 0;
                    stock.Quantity = stock.Quantity.HasValue ? Math.Abs(stock.Quantity.Value) : 0;
                    stock.Status = 15;
                    stock.NoteID = "Phục hồi xuất hết kho";
                    stock.ModifiedBy = userName;
                    stock.ModifiedDate = DateTime.Now;
                    context.SaveChanges();

                    return true;
                }
                catch (OptimisticConcurrencyException)
                {
                    Thread.Sleep(500);
                    return recoverLiquidatedProduct(userName, productId, variationId);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public static bool recoverLiquidatedProduct(tbl_Account acc, int productID)
        {
            var success = true;
            // Nếu mà không phải là status 14 thì không thực thi phục hồi lại với sản phẩm đó
            var stocks = getStock(productID, 0).Where(x => x.Status == 14).ToList();

            if (stocks.Count == 0)
                throw new Exception("Sản phẩn không thể phục hồi xả hàng.");

            foreach (var item in stocks)
                if (!recoverLiquidatedProduct(acc.Username, item.ParentID.Value, item.ProductVariableID.HasValue ? item.ProductVariableID.Value : 0))
                    success = false;

            return success;
        }
        #endregion
        #endregion

        #region Quản lý kho 2
        #region Create
        public static StockManager2 warehousing2(StockManager2 data)
        {
            #region Kiểm tra xem sản phẩm đã nhập vô kho 2 chưa
            if (hasExistedProductStock2(data.ProductID, data.ProductVariableID))
                return UpdateStock2(data);
            #endregion

            #region Khởi tạo thông tin kho với sản phẩm mới
            try
            {
                using (var context = new inventorymanagementEntities())
                {

                    if (data.Type == 2) // Output SKU
                    {
                        data.Type = 1;
                        data.Quantity = 0;
                    }

                    data.QuantityCurrent = 0;
                    context.Entry(data).State = System.Data.Entity.EntityState.Added;
                    context.StockManager2.Add(data);
                    context.SaveChanges();

                    return data;
                }
            }
            catch(OptimisticConcurrencyException)
            {
                Thread.Sleep(500);
                return warehousing2(data);
            }
            catch(Exception e)
            {
                throw e;
            }
            #endregion
        }
        #endregion

        #region Update
        /// <summary>
        /// Cập nhật thông tin kho 2
        /// </summary>
        /// <param name="data">Thông tin cập nhật số lượng sản phẩm</param>
        /// <returns></returns>
        public static StockManager2 UpdateStock2(StockManager2 data)
        {
            using (var context = new inventorymanagementEntities())
            {
                try
                {
                    #region Lock dữ liệu để tính toán
                    var stock = context.StockManager2
                        .Where(x => x.ProductID == data.ProductID)
                        .Where(x => x.ProductVariableID == data.ProductVariableID)
                        .OrderByDescending(x => x.CreatedDate)
                        .FirstOrDefault();

                    context.Entry(stock).State = System.Data.Entity.EntityState.Modified;
                    #endregion

                    #region Tính số lượng kho
                    // Số lượng hiện tại trong kho
                    var quantityCurrent = stock.QuantityCurrent + stock.Quantity * (stock.Type == 1 ? 1 : -1);
                    quantityCurrent = quantityCurrent < 0 ? 0 : quantityCurrent;
                    // Số lượng kho sau khi update
                    var newQuantityCurrent = quantityCurrent + data.Quantity * (data.Type == 1 ? 1 : -1);
                    // Trường hợp là số lượng sản phẩm âm
                    // Type = 2 thì cói như số lượng hiện tại bằng số lượng xuất kho
                    newQuantityCurrent = newQuantityCurrent < 0 ? data.Quantity : quantityCurrent;
                    #endregion

                    #region Cập nhật thông tin kho
                    stock.Quantity = data.Quantity;
                    stock.QuantityCurrent = newQuantityCurrent;
                    stock.Type = data.Type;
                    stock.Note = data.Note;
                    stock.Status = data.Status;
                    stock.ModifiedDate = data.ModifiedDate;
                    stock.ModifiedBy = data.ModifiedBy;
                    #endregion

                    context.SaveChanges();

                    return stock;
                }
                catch (OptimisticConcurrencyException)
                {
                    Thread.Sleep(500);
                    return UpdateStock2(data);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        #endregion

        #region Get
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

        /// <summary>
        /// Lấy thông tin chuyển kho
        /// </summary>
        /// <param name="filter">Thông tin lọc sản phẩm</param>
        /// <param name="page">Thông tin phân trang</param>
        /// <param name="totalQuantityInput">Tổng số lượng nhập kho</param>
        /// <returns></returns>
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

        /// <summary>
        /// Kiểm tra xem sản phẩm đã nhập vô kho 2 chưa
        /// </summary>
        /// <param name="productId">Parent ID</param>
        /// <param name="variationId">Variation ID</param>
        /// <returns></returns>
        public static bool hasExistedProductStock2(int productId, int? variationId)
        {
            using (var context = new inventorymanagementEntities())
            {
                return context.StockManager2
                    .Where(x => x.ProductID == productId)
                    .Where(x => x.ProductVariableID == variationId)
                    .Any();
            }
        }
        #endregion
        #endregion
    }
}