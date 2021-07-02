using IM_PJ.Controllers;
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
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class sua_phien_kiem_kho : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                    var acc = AccountController.GetByUsername(username);
                    if (acc != null)
                    {
                        if (acc.RoleID == 0)
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

        #region Private
        private void _deleteCheckWarehouseDetail(int checkID, IList<int> productIDList)
        {
            using (var con = new inventorymanagementEntities())
            {
                var products = con.CheckWarehouseDetails
                    .Where(x => x.CheckWarehouseID == checkID)
                    .Where(x => productIDList.Contains(x.ProductID))
                    .ToList();

                var totalRemovals = (int)Math.Ceiling(products.Count / 100D);

                for (int i = 0; i < totalRemovals; i++)
                {
                    var details = products.Skip(i * 100).Take(100).ToList();
                    con.CheckWarehouseDetails.RemoveRange(details);
                    con.SaveChanges();
                }
            }
        }

        private void _updateCheckWarehouseDetail(int checkID, List<CheckWarehouseDetail> data)
        {
            using (var con = new inventorymanagementEntities())
            {
                var productOld = con.CheckWarehouseDetails
                    .Where(x => x.CheckWarehouseID == checkID)
                    .ToList()
                    .Join(
                        data,
                        cwd => cwd.ProductSKU,
                        d => d.ProductSKU,
                        (cwd, d) => d
                    );

                var productNew = data.Except(productOld).ToList();

                var totalInserts = (int)Math.Ceiling(productNew.Count / 100D);

                for (int i = 0; i < totalInserts; i++)
                {
                    var details = productNew.Skip(i * 100).Take(100).ToList();
                    con.CheckWarehouseDetails.AddRange(details);
                    con.SaveChanges();
                }
            }
        }
        #endregion

        private void LoadCategory()
        {
            var categories = CategoryController.GetAll();
            StringBuilder html = new StringBuilder();

            html.Append("<select id='ddlCategory' class='form-control' disabled='disabled' readonly>");
            html.Append("<option value=''>Tất cả các danh mục</option>");

            foreach (var c in categories)
            {
                html.Append("<option value='" + c.ID + "'>" + c.CategoryName + "</option>");
            }

            html.Append("</select>");
            ltrCategory.Text = html.ToString();
        } 

        private void LoadCheckWarehouse()
        {
            var checkID = 0;

            if (!String.IsNullOrEmpty(Request.QueryString["checkID"]))
                checkID = Request.QueryString["checkID"].ToInt(0);
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Không tìm thấy ID của phiên kiểm kho", "e", true, Page);
                return;
            }

            var checkWarehouse = CheckWarehouseController.get(checkID);

            if (checkWarehouse != null)
            {
                if (!checkWarehouse.Active)
                {
                    var message = String.Format("Phiên kiểm kho #ID({0}) đã kết thúc", checkID);
                    PJUtils.ShowMessageBoxSwAlert(message, "e", true, Page);
                    return;
                }

                txtTestName.Text = checkWarehouse.Name;
                ddlStock.SelectedValue = checkWarehouse.Stock.ToString();
                hdfCheckID.Value = checkID.ToString();
            }
            else
            {
                var message = String.Format("Không tìm thấy thông tin phiên kiểm kho #ID({0})", checkID);
                PJUtils.ShowMessageBoxSwAlert(message, "e", true, Page);
                return;
            }
        }

        public void LoadData()
        {
            LoadCheckWarehouse();
            LoadCategory();
        }

        [WebMethod]
        public static string getProduct(string sku, int stock, int? categoryID, int? stockStatus)
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
                var stockFilter1 = con.tbl_StockManager
                    .Join(
                        source,
                        s => s.ParentID,
                        d => d.productID,
                        (s, d) => s
                    )
                    .Select(x => new
                    {
                        ParentID = x.ParentID.HasValue ? x.ParentID.Value : 0,
                        ProductVariableID = x.ProductVariableID.HasValue ? x.ProductVariableID.Value : 0,
                        CreatedDate = x.CreatedDate.HasValue ? x.CreatedDate.Value : DateTime.Now,
                        Quantity = x.Quantity.HasValue ? (int)x.Quantity.Value : 0,
                        QuantityCurrent = x.QuantityCurrent.HasValue ? (int)x.QuantityCurrent.Value : 0,
                        Type = x.Type.HasValue ? x.Type.Value : 0
                    })
                    .OrderBy(o => o.ParentID)
                    .ThenBy(o => o.ProductVariableID)
                    .ThenBy(o => o.CreatedDate);

                var stockFilter2 = con.StockManager2
                    .Join(
                        source,
                        s => s.ProductID,
                        d => d.productID,
                        (s, d) => s
                    )
                    .Select(x => new
                    {
                        ParentID = x.ProductID,
                        ProductVariableID = x.ProductVariableID.HasValue ? x.ProductVariableID.Value : 0,
                        CreatedDate = x.CreatedDate,
                        Quantity = x.Quantity,
                        QuantityCurrent = x.QuantityCurrent,
                        Type = x.Type
                    })
                    .OrderBy(o => o.ParentID)
                    .ThenBy(o => o.ProductVariableID)
                    .ThenBy(o => o.CreatedDate);

                var stockFilter = stock == 1 ? stockFilter1 : stockFilter2;
                // Lấy dòng stock cuối cùng
                var stockLast = stockFilter
                    .Select(x => new
                    {
                        parentID = x.ParentID,
                        productVariableID = x.ProductVariableID,
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
                            parentID = rec.ParentID,
                            productVariableID = rec.ProductVariableID,
                            doneAt = rec.CreatedDate
                        },
                        (last, rec) => new
                        {
                            parentID = last.parentID,
                            quantity = rec.Quantity,
                            quantityCurrent = rec.QuantityCurrent,
                            type = rec.Type
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

        [WebMethod]
        public static string getProductByCheckID(int checkID)
        {
            var products = CheckWarehouseController.getProductByCheckID(checkID);

            if (products == null)
                return null;

            using (var con = new inventorymanagementEntities())
            {
                var result = products
                    .GroupBy(g => g.ProductID)
                    .Select(x => new
                    {
                        productID = x.Key,
                        quantity = x.Sum(s => s.QuantityOld),
                        modifiedDate = x.Max(m => m.ModifiedDate)
                    })
                    .OrderByDescending(o => o.modifiedDate)
                    .Join(
                        con.tbl_Product,
                        f => f.productID,
                        p => p.ID,
                        (f, p) => new ProductGetOut()
                        {
                            ID = f.productID,
                            SKU = p.ProductSKU,
                            ProductStyle = p.ProductStyle.HasValue ? p.ProductStyle.Value : 0,
                            ProductName = p.ProductTitle,
                            WarehouseQuantity = f.quantity.ToString()
                        }
                    );
                JavaScriptSerializer serializer = new JavaScriptSerializer();

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

        public class ProductVariation
        {
            public int productID { get; set; }
            public int productVariableID { get; set; }
            public string sku { get; set; }
            public double calQuantity { get; set; }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var checkID = hdfCheckID.Value.ToInt(0);
            var acc = AccountController.GetByUsername(username);

            if (acc == null)
                return;

            #region Loại bỏ những sản phẩm muốn xóa
            var productRemovalList = JsonConvert.DeserializeObject<List<int>>(hdfProductRemoval.Value);
            productRemovalList = productRemovalList.Distinct().ToList();
            _deleteCheckWarehouseDetail(checkID, productRemovalList);
            #endregion

            #region Thêm những sản phẩm mới
            var data = JsonConvert.DeserializeObject<List<ProductGetOut>>(hdfvalue.Value);
            var details = new List<CheckWarehouseDetail>();

            #region Xử lý với sản phẩm không có biến thể
            var products = data.Where(x => x.ProductStyle == 1).ToList();

            foreach (var item in products)
            {
                var temp = new CheckWarehouseDetail()
                {
                    CheckWarehouseID = checkID,
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
            }

            if (details.Count > 0)
            {
                _updateCheckWarehouseDetail(checkID, details);
                details.Clear();
            }
            #endregion

            #region Xử lý với sản phẩm có biến thể
            IList<ProductVariation> variations;

            #region Lấy thông tin  stock
            using (var con = new inventorymanagementEntities())
            {
                var productIDs = data.Except(products).Select(x => x.ID).Distinct().ToList();
                var productVariations = con.tbl_ProductVariable
                    .Where(x => x.ProductID.HasValue)
                    .Where(x => productIDs.Contains(x.ProductID.Value))
                    .Select(x => new {
                        productID = x.ProductID.Value,
                        productVariableID = x.ID,
                        sku = x.SKU
                    })
                    .OrderBy(o => o.productID)
                    .ThenBy(o => o.productVariableID);

                // Lọc ra những dòng stock cần lấy
                var stockFilter = con.tbl_StockManager
                    .Join(
                        productVariations,
                        s => s.SKU,
                        v => v.sku,
                        (s, v) => s
                    )
                    .Select(x => new
                    {
                        ParentID = x.ParentID,
                        ProductVariableID = x.ProductVariableID,
                        SKU = x.SKU,
                        CreatedDate = x.CreatedDate,
                        Quantity = x.Quantity,
                        QuantityCurrent = x.QuantityCurrent,
                        Type = x.Type
                    })
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
                var stocks = stockFilter
                    .Join(
                        stockLast,
                        rec => new
                        {
                            parentID = rec.ParentID.Value,
                            productVariableID = rec.ProductVariableID.Value,
                            doneAt = rec.CreatedDate
                        },
                        last => new
                        {
                            parentID = last.parentID,
                            productVariableID = last.productVariableID,
                            doneAt = last.doneAt
                        },
                        (rec, last) => new
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
                        calQuantity = x.quantityCurrent + x.quantity * (x.type == 1 ? 1 : -1)
                    })
                    .OrderBy(o => o.productID)
                    .ThenBy(o => o.productVariableID);

                variations = productVariations
                    .GroupJoin(
                        stocks,
                        v => v.sku,
                        s => s.sku,
                        (v, s) => new { variation = v, stock = s }
                    )
                    .SelectMany(
                        x => x.stock.DefaultIfEmpty(),
                        (parent, child) => new { variation = parent.variation, stock = child }
                    )
                    .Select(x => new ProductVariation()
                    {
                        productID = x.variation.productID,
                        productVariableID = x.variation.productVariableID,
                        sku = x.variation.sku,
                        calQuantity = x.stock != null ? x.stock.calQuantity : 0D
                    })
                    .ToList();
            }
            #endregion

            foreach (var item in variations)
            {
                var temp = new CheckWarehouseDetail()
                {
                    CheckWarehouseID = checkID,
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
            }

            if (details.Count > 0)
            {
                _updateCheckWarehouseDetail(checkID, details);
                details.Clear();
            }
            #endregion
            #endregion

            PJUtils.ShowMessageBoxSwAlert("Sửa phiên kiểm kho thành công!", "s", true, Page);
        }
    }
}