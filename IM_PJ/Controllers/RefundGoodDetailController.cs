using IM_PJ.Models;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebUI.Business;


namespace IM_PJ.Controllers
{
    public class RefundGoodDetailController
    {
        #region CRUD
        public static int Insert(int RefundGoodsID, int AgentID, int OrderID, string ProductName, int CustomerID, string SKU, double Quantity, double QuantityMax,
            string PriceNotFeeRefund, int ProductType, bool IsCount, int RefundType, string RefundFeePerProduct, string TotalRefundFee, string GiavonPerProduct,
            string DiscountPricePerProduct, string SoldPricePerProduct, string TotalPriceRow, DateTime CreatedDate, string CreatedBy)
        {
            using (var con = new inventorymanagementEntities())
            {
                #region Cập nhật thôn tin giá vốn
                Nullable<double> cogs = null;

                #region Sản phẩm đơn gian
                if (ProductType == 1)
                {
                    cogs = con.tbl_Product
                        .Where(x => x.ProductSKU == SKU)
                        .Select(x => x.CostOfGood.HasValue ? x.CostOfGood.Value : 0)
                        .SingleOrDefault();
                }
                #endregion

                #region Sản phẩm biến thể
                if (ProductType == 2)
                {
                    cogs = con.tbl_ProductVariable
                        .Where(x => x.SKU == SKU)
                        .Select(x => x.CostOfGood.HasValue ? x.CostOfGood.Value : 0)
                        .SingleOrDefault();
                }
                #endregion
                #endregion

                tbl_RefundGoodsDetails a = new tbl_RefundGoodsDetails();
                a.RefundGoodsID = RefundGoodsID;
                a.AgentID = AgentID;
                a.OrderID = OrderID;
                a.ProductName = ProductName;
                a.CustomerID = CustomerID;
                a.SKU = SKU;
                a.Quantity = Quantity;
                a.QuantityMax = QuantityMax;
                a.PriceNotFeeRefund = PriceNotFeeRefund;
                a.ProductType = ProductType;
                a.IsCount = true;
                a.RefundType = RefundType;
                a.RefundFeePerProduct = RefundFeePerProduct;
                a.TotalRefundFee = TotalRefundFee;
                a.GiavonPerProduct = GiavonPerProduct;
                a.DiscountPricePerProduct = DiscountPricePerProduct;
                a.SoldPricePerProduct = SoldPricePerProduct;
                a.TotalPriceRow = TotalPriceRow;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                a.ModifiedDate = CreatedDate;
                a.ModifiedBy = CreatedBy;
                a.CostOfGood = cogs.HasValue ? Convert.ToDecimal(cogs.Value) : 0;
                con.tbl_RefundGoodsDetails.Add(a);
                con.SaveChanges();
                int kq = a.ID;
                return kq;
            }
        }

        public static List<tbl_RefundGoodsDetails> Insert(List<tbl_RefundGoodsDetails> refundDetails)
        {
            using (var con = new inventorymanagementEntities())
            {
                if (refundDetails == null && refundDetails.Count == 0)
                    return null;

                #region Cập nhật thôn tin giá vốn
                List<ProductCOGSModel> product = new List<ProductCOGSModel>();
                List<ProductVariationCOGSModel> variation = new List<ProductVariationCOGSModel>();

                #region Sản phẩm đơn gian
                var productFilter = refundDetails
                    .Where(x => x.ProductType == 1)
                    .Where(x => !String.IsNullOrEmpty(x.SKU))
                    .Select(x => x.SKU)
                    .Distinct()
                    .ToList();

                if (productFilter != null && productFilter.Count > 0)
                {
                    product = con.tbl_Product
                        .Where(x => !String.IsNullOrEmpty(x.ProductSKU))
                        .Where(x => productFilter.Contains(x.ProductSKU))
                        .Select(x => new ProductCOGSModel()
                        {
                            id = x.ID,
                            sku = x.ProductSKU,
                            cogs = x.CostOfGood.HasValue ? x.CostOfGood.Value : 0
                        })
                        .OrderBy(o => o.id)
                        .ToList();
                }
                #endregion

                #region Sản phẩm biến thể
                var variationFilter = refundDetails
                    .Where(x => x.ProductType == 2)
                    .Where(x => !String.IsNullOrEmpty(x.SKU))
                    .Select(x => x.SKU)
                    .Distinct()
                    .ToList();

                if (variationFilter != null && variationFilter.Count > 0)
                {
                    variation = con.tbl_ProductVariable
                        .Where(x => !String.IsNullOrEmpty(x.SKU))
                        .Where(x => variationFilter.Contains(x.SKU))
                        .Select(x => new ProductVariationCOGSModel()
                        {
                            id = x.ID,
                            sku = x.SKU,
                            cogs = x.CostOfGood.HasValue ? x.CostOfGood.Value : 0
                        })
                        .OrderBy(o => o.id)
                        .ToList();
                }
                #endregion
                #endregion

                #region Cập nhật chi tiết đơn hàng
                refundDetails = refundDetails
                    .Where(x => x.ProductType.HasValue)
                    .Where(x => !String.IsNullOrEmpty(x.SKU))
                    .GroupJoin(
                        product,
                        d => new { productStyle = d.ProductType.Value, productSKU = d.SKU },
                        p => new { productStyle = 1, productSKU = p.sku },
                        (d, p) => new { refund = d, product = p }
                    )
                    .SelectMany(
                        x => x.product.DefaultIfEmpty(),
                        (parent, child) => new { refund = parent.refund, product = child }
                    )
                    .GroupJoin(
                        variation,
                        temp => new { productStyle = temp.refund.ProductType.Value, variationSKU = temp.refund.SKU },
                        v => new { productStyle = 2, variationSKU = v.sku },
                        (temp, v) => new { order = temp.refund, product = temp.product, variation = v }
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
                var counts = Math.Ceiling(refundDetails.Count / (double)size);

                for (int index = 0; index < counts; index++)
                {
                    var insertedData = refundDetails
                        .Skip(index * size)
                        .Take(size)
                        .ToList();
                    con.tbl_RefundGoodsDetails.AddRange(insertedData);
                    con.SaveChanges();
                }
                #endregion

                return refundDetails;
            }
        }

        public static bool DeleteByRefundGoodsID(int RefundGoodsID)
        {
            using (var con = new inventorymanagementEntities())
            {
                var listDelete = con.tbl_RefundGoodsDetails.Where(x => x.RefundGoodsID == RefundGoodsID);

                if (listDelete != null)
                {
                    con.tbl_RefundGoodsDetails.RemoveRange(listDelete);
                    con.SaveChanges();
                    return true;
                }

                return false;
            }
        }
        #endregion
        #region Select        
        public static List<tbl_RefundGoodsDetails> GetAll(string s)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_RefundGoodsDetails> las = new List<tbl_RefundGoodsDetails>();
                las = dbe.tbl_RefundGoodsDetails.OrderByDescending(r => r.CreatedDate).ToList();
                return las;
            }
        }
        public static List<tbl_RefundGoodsDetails> GetByRefundGoodsID(int RefundGoodsID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_RefundGoodsDetails> las = new List<tbl_RefundGoodsDetails>();
                las = dbe.tbl_RefundGoodsDetails.Where(r => r.RefundGoodsID == RefundGoodsID).OrderByDescending(r => r.CreatedDate).ToList();
                return las;
            }
        }

        public static List<RefundDetailModel> GetInfoShowRefundDetail(int RefundGoodsID, double FeeRefundDefault)
        {
            using (var con = new inventorymanagementEntities())
            {

                var products = con.tbl_Product
                    .GroupJoin(
                        con.tbl_ProductVariable,
                        product => product.ID,
                        variable => variable.ProductID,
                        (product, variable) => new { product, variable })
                    .SelectMany(x => x.variable.DefaultIfEmpty(),
                                (parent, child) => new
                                {
                                    ProductID = parent.product.ID,
                                    ProductVariableID = parent.product.ProductStyle == 2 ? child.ID : 0,
                                    ProductStyle = parent.product.ProductStyle,
                                    ProductImage = parent.product.ProductStyle == 2 ? child.Image : parent.product.ProductImage,
                                    ProductTitle = parent.product.ProductTitle,
                                    ParentSKU = parent.product.ProductSKU,
                                    ChildSKU = parent.product.ProductStyle == 2 ? child.SKU : String.Empty
                                });

                var refundDetail = con.tbl_RefundGoodsDetails
                            .Where(x => x.RefundGoodsID == RefundGoodsID)
                            .OrderByDescending(r => r.CreatedDate)
                            .Join(
                                products,
                                refund => refund.SKU,
                                product => product.ProductStyle == 2 ? product.ChildSKU : product.ParentSKU,
                                (refund, product) => new
                                {
                                    RefundGoodsID = refund.RefundGoodsID.Value,
                                    RefundDetailID = refund.ID,
                                    OrderID = refund.OrderID.HasValue? refund.OrderID.Value : 0,
                                    ProductID = product.ProductID,
                                    ProductVariableID = product.ProductVariableID,
                                    ProductStyle = product.ProductStyle.Value,
                                    ProductImage = product.ProductImage,
                                    ProductTitle = product.ProductTitle,
                                    ParentSKU = product.ParentSKU,
                                    ChildSKU = product.ChildSKU,
                                    Price = refund.GiavonPerProduct,
                                    ReducedPrice = refund.SoldPricePerProduct,
                                    QuantityRefund = refund.Quantity.Value,
                                    ChangeType = refund.RefundType.Value,
                                    FeeRefund = refund.RefundFeePerProduct,
                                    TotalFeeRefund = refund.TotalPriceRow
                                }
                            )
                            .ToList();

                return refundDetail.Select(x => new RefundDetailModel
                            {
                                RefundGoodsID = x.RefundGoodsID,
                                RefundDetailID = x.RefundDetailID,
                                OrderID = x.OrderID,
                                ProductID = x.ProductID,
                                ProductVariableID = x.ProductVariableID,
                                ProductStyle = x.ProductStyle,
                                ProductImage = x.ProductImage,
                                ProductTitle = x.ProductTitle,
                                ParentSKU = x.ParentSKU,
                                ChildSKU = x.ChildSKU,
                                Price = Convert.ToDouble(x.Price),
                                ReducedPrice = Convert.ToDouble(x.ReducedPrice),
                                QuantityRefund = x.QuantityRefund,
                                ChangeType = x.ChangeType,
                                FeeRefund = Convert.ToDouble(x.FeeRefund),
                                FeeRefundDefault = FeeRefundDefault,
                                TotalFeeRefund = Convert.ToDouble(x.TotalFeeRefund)
                            })
                            .ToList();
            }
        }
        public static List<tbl_RefundGoodsDetails> GetQuantityMax(int CustomerID, int OrderID, string SKU)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_RefundGoodsDetails> las = new List<tbl_RefundGoodsDetails>();
                las = dbe.tbl_RefundGoodsDetails.Where(x => x.CustomerID == CustomerID && x.OrderID == OrderID && x.SKU == SKU).ToList();
                return las;
            }
        }

      
        #endregion
    }
}