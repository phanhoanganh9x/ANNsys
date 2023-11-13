using IM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Controllers
{
    public class CommonController
    {
        private static CommonController _common = new CommonController();

        private CommonController() { }

        public static CommonController getInstance()
        {
            return _common;
        }

        public List<ProductModel> GetProduct(string SKU, bool isStock)
        {
            using (var con = new inventorymanagementEntities())
            {
                var productTarget = con.tbl_Product
                    .GroupJoin(
                        con.tbl_ProductVariable,
                        product => new
                        {
                            ProductStyle = product.ProductStyle.Value,
                            ProductID = product.ID,
                        },
                        productVariable => new
                        {
                            ProductStyle = 2,
                            ProductID = productVariable.ProductID.Value,
                        },
                        (product, productVariable) => new
                        {
                            product,
                            productVariable
                        })
                    .SelectMany(x => x.productVariable.DefaultIfEmpty(),
                                (parent, child) => new ProductModel
                                {
                                    CategoryID = parent.product.CategoryID.Value,
                                    ProductID = parent.product.ID,
                                    ProductVariableID = child != null ? child.ID : 0,
                                    ProductStyle = parent.product.ProductStyle.Value,
                                    ProductImage = child != null ? (!string.IsNullOrEmpty(child.Image) ? child.Image : (!string.IsNullOrEmpty(parent.product.ProductImage) ? parent.product.ProductImage : "/App_Themes/Ann/image/placeholder.png" )) : (!string.IsNullOrEmpty(parent.product.ProductImage) ? parent.product.ProductImage : "/App_Themes/Ann/image/placeholder.png"),
                                    ProductTitle = parent.product.ProductTitle,
                                    VariableValue = String.Empty,
                                    ParentSKU = parent.product.ProductSKU,
                                    ChildSKU = child != null ? child.SKU : String.Empty,
                                    RegularPrice = child != null ? child.Regular_Price.Value : parent.product.Regular_Price.Value,
                                    CostOfGood = child != null ? child.CostOfGood.Value : parent.product.CostOfGood.Value,
                                    RetailPrice = child != null ? child.RetailPrice.Value : parent.product.Retail_Price.Value,
                                    OldPrice = parent.product.Old_Price.Value > 0 ? parent.product.Old_Price.Value : 0,
                                    Price10 = parent.product.Price10.Value > 0 ? parent.product.Price10.Value : 0,
                                    BestPrice = parent.product.BestPrice.Value > 0 ? parent.product.BestPrice.Value : 0,
                                    LastPrice = parent.product.LastPrice.Value > 0 ? parent.product.LastPrice.Value : 0
                                })
                    .Where(x => x.ParentSKU == SKU.Trim().ToUpper() || x.ChildSKU == SKU.Trim().ToUpper())
                    .OrderBy(x => x.ProductID)
                    .ThenBy(x => x.ProductVariableID)
                    .ToList();

                if (productTarget.Count > 1 || (productTarget.Count == 1 && productTarget[0].ProductStyle == 2))
                {
                    var variableValue = con.tbl_ProductVariableValue
                    .Where(x => x.ProductvariableSKU.Contains(SKU.Trim().ToUpper()))
                    .OrderBy(x => x.ProductVariableID)
                    .ThenBy(x => x.ID)
                    .ToList();

                    productTarget = productTarget.Select(x =>
                    {
                        string properties = String.Empty;

                        variableValue
                            .Where(y => y.ProductVariableID == x.ProductVariableID)
                            .Select(y =>
                            {
                                properties += String.Format("{0}: {1}<br/>", y.VariableName, y.VariableValue);
                                return y;
                            })
                            .ToList();

                        return new ProductModel
                        {
                            CategoryID = x.CategoryID,
                            ProductID = x.ProductVariableID == 0 ? x.ProductID : 0,
                            ProductVariableID = x.ProductVariableID,
                            ProductStyle = x.ProductStyle,
                            ProductImage = x.ProductImage,
                            ProductTitle = x.ProductTitle,
                            VariableValue = properties,
                            ParentSKU = x.ParentSKU,
                            ChildSKU = x.ChildSKU,
                            RegularPrice = x.RegularPrice,
                            CostOfGood = x.CostOfGood,
                            RetailPrice = x.RetailPrice,
                            OldPrice = x.OldPrice,
                            Price10 = x.Price10,
                            BestPrice = x.BestPrice,
                            LastPrice = x.LastPrice,
                        };
                    })
                    .ToList();
                }

                if (isStock)
                {
                    var stockMax = con.tbl_StockManager
                        .GroupBy(x => new { x.AgentID, x.ProductID, x.ProductVariableID })
                        .Select(g => new
                        {
                            AgentID = g.Key.AgentID,
                            ProductID = g.Key.ProductID,
                            ProductVariableID = g.Key.ProductVariableID,
                            CreatedDate = g.Max(x => x.CreatedDate)
                        });

                    var stockCurrent = con.tbl_StockManager
                        .Join(
                            stockMax,
                            stock => new
                            {
                                AgentID = stock.AgentID,
                                ProductID = stock.ProductID,
                                ProductVariableID = stock.ProductVariableID,
                                CreatedDate = stock.CreatedDate
                            },
                            max => new
                            {
                                AgentID = max.AgentID,
                                ProductID = max.ProductID,
                                ProductVariableID = max.ProductVariableID,
                                CreatedDate = max.CreatedDate
                            },
                            (stock, max) => new
                            {
                                AgentID = stock.AgentID.Value,
                                ProductID = stock.ProductID.Value,
                                ProductVariableID = stock.ProductVariableID.Value,
                                Quantity = stock.Type == 1 ? stock.QuantityCurrent.Value + stock.Quantity.Value
                                                                    : stock.QuantityCurrent.Value - stock.Quantity.Value
                            });

                    productTarget = productTarget
                        .GroupJoin(
                            stockCurrent,
                            product => new
                            {
                                ProductID = product.ProductID,
                                ProductVariableID = product.ProductVariableID
                            },
                            stock => new
                            {
                                ProductID = stock.ProductID,
                                ProductVariableID = stock.ProductVariableID
                            },
                            (product, stock) => new { product, stock }
                        )
                        .SelectMany(x => x.stock.DefaultIfEmpty(),
                            (parent, child) => new ProductModel
                            {
                                CategoryID = parent.product.CategoryID,
                                ProductID = parent.product.ProductID,
                                ProductVariableID = parent.product.ProductVariableID,
                                ProductStyle = parent.product.ProductStyle,
                                ProductImage = parent.product.ProductImage,
                                ProductTitle = parent.product.ProductTitle,
                                VariableValue = parent.product.VariableValue,
                                ParentSKU = parent.product.ParentSKU,
                                ChildSKU = parent.product.ChildSKU,
                                RegularPrice = parent.product.RegularPrice,
                                CostOfGood = parent.product.CostOfGood,
                                RetailPrice = parent.product.RetailPrice,
                                QuantityCurrent = child != null ? child.Quantity : 0,
                                OldPrice = parent.product.OldPrice,
                                Price10 = parent.product.Price10,
                                BestPrice = parent.product.BestPrice,
                                LastPrice = parent.product.LastPrice
                            })
                        .ToList();
                }

                return productTarget;
            }
        }
    }
}