﻿using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Utils;
using MB.Extensions;
using Newtonsoft.Json;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class tao_phien_kiem_kho : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);
                    if (acc != null)
                    {
                        if (acc.RoleID == 0)
                        {

                        }
                        else if (acc.RoleID == 1)
                        {

                        }
                        else if (acc.RoleID == 2)
                        {

                        }
                        else
                        {
                            Response.Redirect("/trang-chu");
                        }
                        LoadData();
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
                
            }
        }
        public void LoadData()
        {
            var categories = CategoryController.GetAll();
            StringBuilder html = new StringBuilder();

            html.Append("<select id='ddlCategory' class='form-control' style='width: 20%; float: left; margin-right: 10px'>");
            html.Append("<option value=''>Tất cả các danh mục</option>");

            foreach (var c in categories)
            {
                html.Append("<option value='" + c.ID + "'>" + c.CategoryName + "</option>");
            }

            html.Append("</select>");
            ltrCategory.Text = html.ToString();
        }

        [WebMethod]
        public static string getProduct(string sku, int? categoryID, int? stockStatus)
        {
            using (var con = new inventorymanagementEntities())
            {
                var source = con.tbl_Product
                    .Select(x => new
                    {
                        categoryID = x.CategoryID.HasValue ? x.CategoryID.Value : 0,
                        productID = x.ID,
                        sku = x.ProductSKU,
                        productStyle = x.ProductStyle.HasValue ? x.ProductStyle.Value : 1,
                        title = x.ProductTitle,
                        quantity = 0D
                    });

                #region Lọc sản phẩm theo text search
                if (!String.IsNullOrEmpty(sku))
                {
                    sku = sku.Trim().ToLower();

                    source = source
                        .Where(x =>
                            (
                                x.sku.Trim().Length >= sku.Length &&
                                x.sku.Trim().ToLower().StartsWith(sku)
                            ) ||
                            (
                                x.sku.Trim().Length < sku.Length &&
                                sku.StartsWith(x.sku.Trim().ToLower())
                            )
                        );

                }
                #endregion

                #region Lấy theo category ID
                if (categoryID.HasValue)
                {
                    source = source.Where(x => x.categoryID == categoryID.Value);
                }
                #endregion

                #region Lấy thông tin sản phẩm và stock
                // Lọc ra những dòng stock cần lấy
                var stockFilter = con.tbl_StockManager
                    .Join(
                        source,
                        s => s.ParentID,
                        d => d.productID,
                        (s, d) => s
                    )
                    .OrderBy(o => o.ParentID)
                    .ThenBy(o => o.ProductVariableID)
                    .ThenBy(o => o.CreatedDate);

                // Lấy dòng stock cuối cùng
                var stockLast = stockFilter
                    .Select(x => new
                    {
                        parentID = x.ParentID.Value,
                        productVariableID = x.ProductVariableID.Value,
                        createDate = x.CreatedDate
                    })
                    .GroupBy(x => new { x.parentID, x.productVariableID })
                    .Select(g => new
                    {
                        parentID = g.Key.parentID,
                        productVariableID = g.Key.productVariableID,
                        doneAt = g.Max(x => x.createDate)
                    });

                // Thông tin stock
                var stocks = stockLast
                    .Join(
                        stockFilter,
                        last => new
                        {
                            parentID = last.parentID,
                            productVariableID = last.productVariableID,
                            doneAt = last.doneAt
                        },
                        rec => new
                        {
                            parentID = rec.ParentID.Value,
                            productVariableID = rec.ProductVariableID.Value,
                            doneAt = rec.CreatedDate
                        },
                        (last, rec) => new
                        {
                            parentID = last.parentID,
                            quantity = rec.Quantity.HasValue ? rec.Quantity.Value : 0,
                            quantityCurrent = rec.QuantityCurrent.HasValue ? rec.QuantityCurrent.Value : 0,
                            type = rec.Type.HasValue ? rec.Type.Value : 0
                        }
                    )
                    .Select(x => new
                    {
                        parentID = x.parentID,
                        calQuantity = x.type == 1 ?
                            (x.quantityCurrent + x.quantity) :
                            (x.type == 2 ? x.quantityCurrent - x.quantity : 0D)
                    })
                    .GroupBy(x => x.parentID)
                    .Select(g => new
                    {
                        productID = g.Key,
                        quantity = g.Sum(x => x.calQuantity)
                    })
                    .OrderBy(x => x.productID);

                source = source
                    .GroupJoin(
                        stocks,
                        s => s.productID,
                        st => st.productID,
                        (s, st) => new { product = s, stock = st }
                    )
                    .SelectMany(
                        x => x.stock.DefaultIfEmpty(),
                        (parent, child) => new {
                            categoryID = parent.product.categoryID,
                            productID = parent.product.productID,
                            sku = parent.product.sku,
                            productStyle = parent.product.productStyle,
                            title = parent.product.title,
                            quantity = child != null ? child.quantity : parent.product.quantity
                        });
                #endregion

                #region lọc sản phẩm theo trạng thái kho
                if (String.IsNullOrEmpty(sku) && stockStatus.HasValue)
                {
                    switch (stockStatus.Value)
                    {
                        case (int)StockStatus.stocking:
                            source = source.Where(x => x.quantity > 0);
                            break;
                        case (int)StockStatus.stockOut:
                            source = source.Where(x => x.quantity == 0);
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var result = source.Select(x => new ProductGetOut()
                {
                    ID = x.productID,
                    SKU = x.sku,
                    ProductStyle = x.productStyle,
                    ProductName = x.title,
                    WarehouseQuantity = x.quantity.ToString()
                }).ToList();

                return serializer.Serialize(result);
            }
        }



        public class ProductGetOut
        {
            public int ID { get; set; }
            public string SKU { get; set; }
            public int ProductStyle { get; set; }
            public string ProductName { get; set; }
            public string WarehouseQuantity { get; set; }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Request.Cookies["usernameLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc == null)
                return;

            using (var con = new inventorymanagementEntities())
            {
                var now = DateTime.Now;
                var checkWarehouse = new CheckWarehouse()
                {
                    Name = String.Format("{0}-{1:yyyy/MM/dd hh:mm:ss}", txtTestName.Text.Trim(), now),
                    Active = true,
                    CreatedDate = now,
                    CreatedBy = acc.Username,
                    ModifiedDate = now,
                    ModifiedBy = acc.Username
                };

                con.CheckWarehouses.Add(checkWarehouse);
                con.SaveChanges();

                var data = JsonConvert.DeserializeObject<List<ProductGetOut>>(hdfvalue.Value);
                var details = new List<CheckWarehouseDetail>();

                #region Xử lý với sản phẩm không có biến thể
                var products = data.Where(x => x.ProductStyle == 1).ToList();

                foreach (var item in products)
                {
                    var temp = new CheckWarehouseDetail()
                    {
                        CheckWarehouseID = checkWarehouse.ID,
                        ProductID = item.ID,
                        ProductVariableID = 0,
                        ProductSKU = item.SKU,
                        QuantityOld = Convert.ToInt32(item.WarehouseQuantity),
                        CreatedDate = now,
                        CreatedBy = acc.Username,
                        ModifiedDate = now,
                        ModifiedBy = acc.Username
                    };
                    details.Add(temp);

                    if (details.Count == 100)
                    {
                        con.CheckWarehouseDetails.AddRange(details);
                        con.SaveChanges();

                        details.Clear();
                    }
                }

                if (details.Count > 0)
                {
                    con.CheckWarehouseDetails.AddRange(details);
                    con.SaveChanges();

                    details.Clear();
                }
                #endregion

                #region Xử lý với sản phẩm có biến thể
                var productVariables = data.Except(products).ToList();

                #region Lấy thông tin  stock
                var productIDs = productVariables.Select(x => x.ID).Distinct().ToList();

                // Lọc ra những dòng stock cần lấy
                var stockFilter = con.tbl_StockManager.Where(x => productIDs.Contains(x.ParentID.Value))
                    .OrderBy(o => o.ParentID)
                    .ThenBy(o => o.ProductVariableID)
                    .ThenBy(o => o.CreatedDate);

                // Lấy dòng stock cuối cùng
                var stockLast = stockFilter
                    .Select(x => new
                    {
                        parentID = x.ParentID.Value,
                        productVariableID = x.ProductVariableID.Value,
                        createDate = x.CreatedDate
                    })
                    .GroupBy(x => new { x.parentID, x.productVariableID })
                    .Select(g => new
                    {
                        parentID = g.Key.parentID,
                        productVariableID = g.Key.productVariableID,
                        doneAt = g.Max(x => x.createDate)
                    });

                // Thông tin stock
                var stocks = stockLast
                    .Join(
                        stockFilter,
                        last => new
                        {
                            parentID = last.parentID,
                            productVariableID = last.productVariableID,
                            doneAt = last.doneAt
                        },
                        rec => new
                        {
                            parentID = rec.ParentID.Value,
                            productVariableID = rec.ProductVariableID.Value,
                            doneAt = rec.CreatedDate
                        },
                        (last, rec) => new
                        {
                            parentID = last.parentID,
                            productVariableID = last.productVariableID,
                            sku = rec.SKU,
                            quantity = rec.Quantity.HasValue ? rec.Quantity.Value : 0,
                            quantityCurrent = rec.QuantityCurrent.HasValue ? rec.QuantityCurrent.Value : 0,
                            type = rec.Type.HasValue ? rec.Type.Value : 0
                        }
                    )
                    .Select(x => new
                    {
                        productID = x.parentID,
                        productVariableID = x.productVariableID,
                        sku = x.sku,
                        calQuantity = x.type == 1 ?
                            (x.quantityCurrent + x.quantity) :
                            (x.type == 2 ? x.quantityCurrent - x.quantity : 0D)
                    })
                    .OrderBy(o => o.productID)
                    .ThenBy(o => o.productVariableID);
                #endregion

                foreach (var item in stocks)
                {
                    var temp = new CheckWarehouseDetail()
                    {
                        CheckWarehouseID = checkWarehouse.ID,
                        ProductID = item.productID,
                        ProductVariableID = item.productVariableID,
                        ProductSKU = item.sku,
                        QuantityOld = Convert.ToInt32(item.calQuantity),
                        CreatedDate = now,
                        CreatedBy = acc.Username,
                        ModifiedDate = now,
                        ModifiedBy = acc.Username
                    };
                    details.Add(temp);

                    if (details.Count == 100)
                    {
                        con.CheckWarehouseDetails.AddRange(details);
                        con.SaveChanges();

                        details.Clear();
                    }
                }

                if (details.Count > 0)
                {
                    con.CheckWarehouseDetails.AddRange(details);
                    con.SaveChanges();

                    details.Clear();
                }
                #endregion

                PJUtils.ShowMessageBoxSwAlert("Nhập kho thành công!", "s", true, Page);
            }
        }
    }
}