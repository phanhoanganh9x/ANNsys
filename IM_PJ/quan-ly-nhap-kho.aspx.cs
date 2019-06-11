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

namespace IM_PJ
{
    public partial class quan_ly_nhap_kho : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["userLoginSystem"] != null)
                {
                    string username = Request.Cookies["userLoginSystem"].Value;
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
            var supplier = SupplierController.GetAllWithIsHidden(false);
            StringBuilder html = new StringBuilder();

            html.Append("<select id=\"supplierList\" class=\"form-control\" style=\"width: 20%; float: left; margin-right: 10px\">");
            html.Append("<option value=\"0\">Tất cả nhà cung cấp</option>");
            if (supplier.Count > 0)
            {
                foreach (var s in supplier)
                {
                    html.Append("<option value=\"" + s.ID + "\">" + s.SupplierName + "</option>");
                }
            }
            html.Append("</select>");
            ltrSupplier.Text = html.ToString();

            // Lấy css print barcode từ cài đặt

            var p = ConfigController.GetByTop1();
            if (p != null)
            {
                hdfCSSPrintBarcode.Value = p.CSSPrintBarcode;
            }
        }

        [WebMethod]
        public static string getProduct(string textsearch)
        {
            List<ProductGetOut> ps = new List<ProductGetOut>();
            string username = HttpContext.Current.Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                int AgentID = Convert.ToInt32(acc.AgentID);
                var products = ProductController.GetBySKU(textsearch.Trim().ToUpper());

                if (products != null)
                {
                    var productvariable = ProductVariableController.GetByParentSKU(products.ProductSKU);

                    if (productvariable.Count > 0)
                    {
                        foreach (var pv in productvariable)
                        {
                            var variables = ProductVariableValueController.GetByProductVariableSKU(pv.SKU);

                            if (variables.Count > 0)
                            {
                                string variablename = "";
                                string variablevalue = "";
                                string variable = "";

                                foreach (var v in variables)
                                {
                                    variable += v.VariableName.Trim() + ": " + v.VariableValue.Trim() + "|";
                                    variablename += v.VariableName.Trim() + "|";
                                    variablevalue += v.VariableValue.Trim() + "|";
                                }

                                ProductGetOut p = new ProductGetOut();
                                p.ID = pv.ID;
                                p.ProductName = products.ProductTitle;
                                p.ProductVariable = variable;
                                p.ProductVariableName = variablename;
                                p.ProductVariableValue = variablevalue;
                                p.ProductType = 2;

                                if (!string.IsNullOrEmpty(pv.Image))
                                {
                                    p.ProductImage = "<img onclick='openImage($(this))' src=\"" + Thumbnail.getURL(pv.Image, Thumbnail.Size.Small) + "\" />";
                                    p.ProductImageOrigin = pv.Image;
                                }
                                else
                                {
                                    p.ProductImage = "<img src=\"/App_Themes/Ann/image/placeholder.png\" />";
                                    p.ProductImageOrigin = "";
                                }

                                p.SKU = pv.SKU.Trim().ToUpper();

                                int supplierID = 0;
                                if (pv.SupplierID != null)
                                    supplierID = pv.SupplierID.ToString().ToInt(0);
                                p.SupplierID = supplierID;

                                string supplierName = "";

                                if (!string.IsNullOrEmpty(pv.SupplierName))
                                    supplierName = pv.SupplierName;
                                p.SupplierName = supplierName;

                                double total = PJUtils.TotalProductQuantityInstock(AgentID, pv.SKU);
                                p.WarehouseQuantity = string.Format("{0:N0}", total);
                                ps.Add(p);
                            }
                        }
                    }
                    else
                    {
                        string variablename = "";
                        string variablevalue = "";
                        string variable = "";

                        ProductGetOut p = new ProductGetOut();
                        p.ID = products.ID;
                        p.ProductName = products.ProductTitle;
                        p.ProductVariable = variable;
                        p.ProductVariableName = variablename;
                        p.ProductVariableValue = variablevalue;
                        p.ProductType = 1;

                        if (!string.IsNullOrEmpty(products.ProductImage))
                        {
                            p.ProductImage = "<img onclick='openImage($(this))' src=\"" + Thumbnail.getURL(products.ProductImage, Thumbnail.Size.Small) + "\" />";
                            p.ProductImageOrigin = Thumbnail.getURL(products.ProductImage, Thumbnail.Size.Small);
                        }
                        else
                        {
                            p.ProductImage = "<img src=\"/App_Themes/Ann/image/placeholder.png\" />";
                            p.ProductImageOrigin = "";
                        }

                        p.SKU = products.ProductSKU.Trim().ToUpper();
                        int supplierID = 0;
                        if (products.SupplierID != null)
                            supplierID = products.SupplierID.ToString().ToInt(0);
                        p.SupplierID = supplierID;
                        string supplierName = "";
                        if (!string.IsNullOrEmpty(products.SupplierName))
                            supplierName = products.SupplierName;
                        p.SupplierName = supplierName;

                        double total = PJUtils.TotalProductQuantityInstock(AgentID, products.ProductSKU);

                        p.WarehouseQuantity = string.Format("{0:N0}", total);
                        ps.Add(p);
                    }
                }
                else
                {

                    var productvariable = ProductVariableController.GetAllBySKU(textsearch.Trim().ToUpper());

                    if (productvariable != null)
                    {
                        foreach (var value in productvariable)
                        {
                            var variables = ProductVariableValueController.GetByProductVariableSKU(value.SKU);

                            if (variables.Count > 0)
                            {
                                string variablename = "";
                                string variablevalue = "";
                                string variable = "";

                                foreach (var v in variables)
                                {
                                    variable += v.VariableName + ": " + v.VariableValue + "|";
                                    variablename += v.VariableName + "|";
                                    variablevalue += v.VariableValue + "|";
                                }

                                ProductGetOut p = new ProductGetOut();
                                p.ID = value.ID;

                                var product = ProductController.GetBySKU(value.ParentSKU);
                                if (product != null)
                                    p.ProductName = product.ProductTitle;

                                p.ProductVariable = variable;
                                p.ProductVariableName = variablename;
                                p.ProductVariableValue = variablevalue;
                                p.ProductType = 2;

                                if (!string.IsNullOrEmpty(value.Image))
                                {
                                    p.ProductImage = "<img onclick='openImage($(this))' src=\"" + Thumbnail.getURL(value.Image, Thumbnail.Size.Small) + "\" />";
                                    p.ProductImageOrigin = Thumbnail.getURL(value.Image, Thumbnail.Size.Small);
                                }
                                else
                                {
                                    p.ProductImage = "<img src=\"/App_Themes/Ann/image/placeholder.png\" />";
                                    p.ProductImageOrigin = "";
                                }

                                p.SKU = value.SKU.Trim().ToUpper();
                                int supplierID = 0;
                                if (value.SupplierID != null)
                                    supplierID = value.SupplierID.ToString().ToInt(0);
                                p.SupplierID = supplierID;
                                string supplierName = "";
                                if (!string.IsNullOrEmpty(value.SupplierName))
                                    supplierName = value.SupplierName;
                                p.SupplierName = supplierName;

                                double total = PJUtils.TotalProductQuantityInstock(AgentID, value.SKU);

                                p.WarehouseQuantity = string.Format("{0:N0}", total);
                                ps.Add(p);
                            }
                        }

                    }
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(ps);
        }

        public class ProductGetOut
        {
            public int ID { get; set; }
            public string ProductName { get; set; }
            public string ProductVariable { get; set; }
            public string ProductVariableName { get; set; }
            public string ProductVariableValue { get; set; }
            public int ProductType { get; set; }
            public string ProductImage { get; set; }
            public string ProductImageOrigin { get; set; }
            public string SKU { get; set; }
            public int SupplierID { get; set; }
            public string SupplierName { get; set; }
            public string WarehouseQuantity { get; set; }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                int AgentID = Convert.ToInt32(acc.AgentID);
                string list = hdfvalue.Value;
                string note = "Nhập kho bằng chức năng nhập kho";
                if(hdfNote.Value != "")
                {
                    note = hdfNote.Value;
                }

                string[] items = list.Split(';');
                if (items.Length - 1 > 0)
                {
                    for (int i = 0; i < items.Length - 1; i++)
                    {
                        var item = items[i];
                        string[] itemValue = item.Split(',');
                        int ID = itemValue[0].ToInt();
                        string SKU = itemValue[1];
                        int producttype = itemValue[2].ToInt();
                        string ProductVariableName = itemValue[3];
                        string ProductVariableValue = itemValue[4];
                        double Quantity = Convert.ToDouble(itemValue[5]);
                        string ProductName = itemValue[6];
                        string ProductImageOrigin = itemValue[7];
                        string ProductVariable = itemValue[8];
                        var productV = ProductVariableController.GetByID(ID);
                        string parentSKU = "";
                        parentSKU = productV.ParentSKU;

                        if (producttype == 1)
                        {
                            ProductController.UpdateStockStatus(parentSKU, 1, false, currentDate, username);

                            StockManagerController.Insert(
                                new tbl_StockManager
                                {
                                    AgentID = AgentID,
                                    ProductID = ID,
                                    ProductVariableID = 0,
                                    Quantity = Quantity,
                                    QuantityCurrent = 0,
                                    Type = 1,
                                    NoteID = note,
                                    OrderID = 0,
                                    Status = 1,
                                    SKU = SKU,
                                    CreatedDate = currentDate,
                                    CreatedBy = username,
                                    MoveProID = 0,
                                    ParentID = ID
                                });
                        }
                        else
                        {
                            int parentID = 0;
                            var variable = ProductVariableController.GetByID(ID);
                            if (variable != null)
                            {
                                parentID = Convert.ToInt32(variable.ProductID);
                            }

                            StockManagerController.Insert(
                                new tbl_StockManager
                                {
                                    AgentID = AgentID,
                                    ProductID = 0,
                                    ProductVariableID = ID,
                                    Quantity = Quantity,
                                    QuantityCurrent = 0,
                                    Type = 1,
                                    NoteID = note,
                                    OrderID = 0,
                                    Status = 1,
                                    SKU = SKU,
                                    CreatedDate = currentDate,
                                    CreatedBy = username,
                                    MoveProID = 0,
                                    ParentID = parentID
                                });
                        }

                        ProductVariableController.UpdateStockStatus(ID, 1, false, currentDate, username);

                        double total = PJUtils.TotalProductQuantityInstock(AgentID, itemValue[1]);
                    }

                    // IN MÃ VẠCH

                    string barcodeValue = "";
                    string productPrint = "";
                    string barcodeImage = "";

                    string[] value = hdfBarcode.Value.Split(';');
                    if (value.Count() > 1)
                    {
                        var temps = new List<String>();
                        productPrint += "<div class=\"qcode\">";
                        for (int i = 0; i < value.Length - 1; i++)
                        {

                            string[] list2 = value[i].Split(',');
                            int quantity = list2[1].ToInt();

                            for (int j = 0; j < quantity; j++)
                            {
                                barcodeValue = list2[0];
                                var imageName = String.Format("{0}{1}.png", DateTime.UtcNow.ToString("yyyyMMddHHmmss"), Guid.NewGuid());
                                barcodeImage = "/uploads/barcodes/" + imageName;
                                System.Drawing.Image barCode = PJUtils.MakeBarcodeImage(barcodeValue, 2, true);

                                barCode.Save(HttpContext.Current.Server.MapPath("" + barcodeImage + ""), ImageFormat.Png);

                                productPrint += "<div class=\"item\">";
                                productPrint += "<div class=\"img\"><img src=\"data:image/png;base64, " + Convert.ToBase64String(File.ReadAllBytes(Server.MapPath("" + barcodeImage + ""))) + "\"></div>";
                                productPrint += "<div><h1>" + barcodeValue + "</h1></div>";
                                productPrint += "</div>";

                                temps.Add(imageName);
                            }
                        }
                        productPrint += "</div>";
                        string html = "";
                        html += productPrint;
                        ltrprint.Text = html;

                        // Delete barcode image after print
                        string[] filePaths = Directory.GetFiles(Server.MapPath("/uploads/barcodes/"));
                        foreach (string filePath in filePaths)
                        {
                            foreach (var item in temps)
                            {
                                if (filePath.EndsWith(item))
                                {
                                    File.Delete(filePath);
                                }
                            }
                        }

                        // in mã vạch

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { printBarcode() });", true);
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Nhập kho thành công!", "s", true, Page);
                    }
                }
            }
        }
    }
}