using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Utils;
using MB.Extensions;
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
using Newtonsoft.Json;

namespace IM_PJ
{
    public partial class chuyen_kho : System.Web.UI.Page
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
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
                
            }
        }

        private static ProductGetOut _getProduct(int warehouse, tbl_Product product) {
            var quantity = PJUtils.TotalProductQuantityInstock(1, product.ProductSKU);
            var quantity2 = StockManagerController.getQuantityStock2BySKU(product.ProductSKU);

            if (warehouse == 1 && quantity == 0)
                return null;

            if (warehouse == 2 && (!quantity2.HasValue && quantity2.Value == 0))
                return null;

            var result = new ProductGetOut() {
                ID = product.ID,
                ProductName = product.ProductTitle,
                ProductVariable = String.Empty,
                ProductVariableName = String.Empty,
                ProductVariableValue = String.Empty,
                ProductStyle = 1,
                ProductImage = Thumbnail.getURL(product.ProductImage, Thumbnail.Size.Normal),
                ProductImageOrigin = Thumbnail.getURL(product.ProductImage, Thumbnail.Size.Source),
                SKU = product.ProductSKU.Trim().ToUpper(),
                WarehouseQuantity = String.Format("{0:N0}", quantity),
                WarehouseQuantity2 = String.Format("{0:N0}", quantity2),
                ParentID = product.ID,
                ParentSKU = product.ProductSKU
            };

            return result;
        }

        private static List<ProductGetOut> _getProductVariation(int warehouse, List<tbl_ProductVariable> variations, tbl_Product product = null) {
            var result = new List<ProductGetOut>();

            if (variations == null || variations.Count() == 0)
                return result;
            
            if (product == null)
                product = ProductController.GetBySKU(variations.FirstOrDefault().ParentSKU);

            foreach (var variation in variations)
            {
                var quantity = PJUtils.TotalProductQuantityInstock(1, variation.SKU);
                var quantity2 = StockManagerController.getQuantityStock2BySKU(variation.SKU);

                if (warehouse == 1 && quantity == 0)
                    continue;

                if (warehouse == 2 && (!quantity2.HasValue || quantity2.Value == 0))
                    continue;

                var attributes = ProductVariableValueController.GetByProductVariableSKU(variation.SKU);

                if (attributes.Count == 0)
                    continue;

                var variable = String.Empty;
                var variablename = String.Empty;
                var variablevalue = String.Empty;

                foreach (var attribute in attributes)
                {
                    variable += attribute.VariableName.Trim() + ": " + attribute.VariableValue.Trim() + "|";
                    variablename += attribute.VariableName.Trim() + "|";
                    variablevalue += attribute.VariableValue.Trim() + "|";
                }

                var item = new ProductGetOut(){
                    ID = variation.ID,
                    ProductName = product.ProductTitle,
                    ProductVariable = variable,
                    ProductVariableName = variablename,
                    ProductVariableValue = variablevalue,
                    ProductStyle = 2,
                    ProductImage = Thumbnail.getURL(variation.Image, Thumbnail.Size.Normal),
                    ProductImageOrigin = Thumbnail.getURL(variation.Image, Thumbnail.Size.Source),
                    SKU = variation.SKU.Trim().ToUpper(),
                    WarehouseQuantity = String.Format("{0:N0}", quantity),
                    WarehouseQuantity2 = String.Format("{0:N0}", quantity2),
                    ParentID = product.ID,
                    ParentSKU = product.ProductSKU
                };
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Lấy thông tin về sản phẩm và số lượng tại các kho
        /// </summary>
        /// <param name="transfer">1: Kho 1 sang kho 2 | 2: Kho 2 sang kho 1</param>
        /// <param name="textsearch"></param>
        /// <returns></returns>
        [WebMethod]
        public static string getProduct(int transfer, string textsearch)
        {
            List<ProductGetOut> result = new List<ProductGetOut>();
            string username = HttpContext.Current.Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            
            if (acc != null)
            {
                int AgentID = Convert.ToInt32(acc.AgentID);
                var product = ProductController.GetBySKU(transfer, textsearch.Trim().ToUpper());

                if (product != null)
                {
                    if (product.ProductStyle == 1) {
                        var data = _getProduct(transfer, product);

                        if (data != null)
                            result.Add(data);
                    }
                    else {
                        var productvariable = ProductVariableController.GetByParentSKU(product.ProductSKU);
                        var data = _getProductVariation(transfer, productvariable, product);

                        if (data != null && data.Count > 0)
                            result.AddRange(data);
                    }
                }
                else
                {
                    var productvariable = ProductVariableController.GetAllBySKU(textsearch.Trim().ToUpper());
                    var data = _getProductVariation(transfer, productvariable);

                    if (data != null && data.Count > 0)
                        result.AddRange(data);
                }
            }
            
            return JsonConvert.SerializeObject(result);
        }

        public class ProductGetOut
        {
            public int ID { get; set; }
            public string ProductName { get; set; }
            public string ProductVariable { get; set; }
            public string ProductVariableName { get; set; }
            public string ProductVariableValue { get; set; }
            public int ProductStyle { get; set; }
            public string ProductImage { get; set; }
            public string ProductImageOrigin { get; set; }
            public string SKU { get; set; }
            public string WarehouseQuantity { get; set; }
            public string WarehouseQuantity2 { get; set; }
            public int ParentID { get; set; }
            public string ParentSKU { get; set; }
        }

        public class WarehousePostModel {
            public int productStyle { get; set; }
            public int productID { get; set; }
            public int productVariableID { get; set; }
            public string sku { get; set; }
            public string parentSKU { get; set; }
            public int quantity { get; set; }
            public int stock1 { get; set; }
            public int stock2 { get; set; }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (hdfWarehouseTransfer.Value == "0")
            {
                PJUtils.ShowMessageBoxSwAlert("Vui lòng chọn kho muốn chuyển hàng tới", "i", false, Page);
                return;
            }

            DateTime currentDate = DateTime.Now;
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                int AgentID = Convert.ToInt32(acc.AgentID);
                string note = String.Empty;
                var data = String.IsNullOrEmpty(hdfvalue.Value) ? 
                    new List<WarehousePostModel>() : 
                    JsonConvert.DeserializeObject<List<WarehousePostModel>>(hdfvalue.Value);

                if (hdfWarehouseTransfer.Value == "1")
                {
                    foreach (var item in data)
                    {
                        StockManagerController.warehousing1(new tbl_StockManager()
                        {
                            AgentID = AgentID,
                            ProductID = item.productVariableID == 0 ? item.productID : 0,
                            ProductVariableID = item.productVariableID,
                            SKU = item.sku,
                            Type = 2,
                            Quantity = item.quantity,
                            Status = 18,
                            NoteID = "Chuyển hàng sang kho 2 bằng chức năng chuyển kho",
                            CreatedDate = currentDate,
                            CreatedBy = username,
                            ModifiedDate = currentDate,
                            ModifiedBy = username,
                            OrderID = 0,
                            MoveProID = 0,
                            ParentID = item.productID
                        });
                        StockManagerController.warehousing2(new StockManager2()
                        {
                            AgentID = AgentID,
                            ProductID = item.productID,
                            ProductVariableID = item.productVariableID,
                            SKU = item.sku,
                            ParentSKU = item.parentSKU,
                            Type = 1,
                            Quantity = item.quantity,
                            Status = 20,
                            Note = "Nhận hàng từ kho 1 bằng chức năng chuyển kho",
                            CreatedDate = currentDate,
                            CreatedBy = username,
                            ModifiedDate = currentDate,
                            ModifiedBy = username,
                        });
                    }
                }
                else
                {
                    foreach (var item in data)
                    {
                        StockManagerController.warehousing1(new tbl_StockManager()
                        {
                            AgentID = AgentID,
                            ProductID = item.productVariableID == 0 ? item.productID : 0,
                            ProductVariableID = item.productVariableID,
                            SKU = item.sku,
                            Type = 1,
                            Quantity = item.quantity,
                            Status = 20,
                            NoteID = "Nhận hàng từ kho 2 bằng chức năng chuyển kho",
                            CreatedDate = currentDate,
                            CreatedBy = username,
                            ModifiedDate = currentDate,
                            ModifiedBy = username,
                            OrderID = 0,
                            MoveProID = 0,
                            ParentID = item.productID
                        });
                        StockManagerController.warehousing2(new StockManager2()
                        {
                            AgentID = AgentID,
                            ProductID = item.productID,
                            ProductVariableID = item.productVariableID,
                            SKU = item.sku,
                            ParentSKU = item.parentSKU,
                            Type = 2,
                            Quantity = item.quantity,
                            Status = 18,
                            Note = "Chuyển hàng sang kho 1 bằng chức năng chuyển kho",
                            CreatedDate = currentDate,
                            CreatedBy = username,
                            ModifiedDate = currentDate,
                            ModifiedBy = username,
                        });
                    }
                }
                

                PJUtils.ShowMessageBoxSwAlert("Chuyển kho thành công!", "s", true, Page);
            }
        }
    }
}