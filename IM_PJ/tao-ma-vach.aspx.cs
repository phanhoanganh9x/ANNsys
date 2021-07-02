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
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static IM_PJ.Controllers.DiscountCustomerController;

namespace IM_PJ
{
    public partial class tao_ma_vach : System.Web.UI.Page
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
                        if (acc.RoleID == 0 || acc.RoleID == 1)
                        {
                            var p = ConfigController.GetByTop1();
                            if (p != null)
                            {
                                hdfCSSPrintBarcode.Value = p.CSSPrintBarcode;
                            }
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

        [WebMethod]
        public static string getProduct(string textsearch, int gettotal)
        {
            List<ProductGetOut> ps = new List<ProductGetOut>();
            string username = HttpContext.Current.Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                int AgentID = Convert.ToInt32(acc.AgentID);
                var product = ProductController.GetBySKU(textsearch.Trim().ToUpper());

                // Kiểm tra sản phẩm có trong table Product không?
                if (product != null) // Nếu sản phẩm có trong table Product thì...
                {
                    var productvariable = ProductVariableController.GetByParentSKU(product.ProductSKU);

                    // Kiểm tra sản phẩm cha là variable hay simple?
                    if (productvariable.Count > 0) // Nếu sản phẩm cha là variable thì...
                    {
                        foreach (var pv in productvariable)
                        {
                            string SKU = pv.SKU.Trim().ToUpper();

                            var variables = ProductVariableValueController.GetByProductVariableSKU(pv.SKU);

                            if (variables.Count > 0)
                            {
                                string variablename = "";
                                string variablevalue = "";
                                string variable = "";
                                string variablesave = "";
                                foreach (var v in variables)
                                {
                                    variable += v.VariableName.Trim() + ": " + v.VariableValue.Trim() + "<br/>";
                                    variablesave += v.VariableName.Trim() + ": " + v.VariableValue.Trim() + "|";
                                    variablename += v.VariableName.Trim() + "|";
                                    variablevalue += v.VariableValue.Trim() + "|";
                                }

                                ProductGetOut p = new ProductGetOut();
                                p.ID = pv.ID;
                                p.ProductName = product.ProductTitle;
                                p.ProductVariable = variable;
                                p.ProductVariableSave = variablesave;
                                p.ProductVariableName = variablename;
                                p.ProductVariableValue = variablevalue;
                                p.ProductType = 2;

                                if (!string.IsNullOrEmpty(pv.Image))
                                {
                                    p.ProductImage = "<img src='" + Thumbnail.getURL(pv.Image, Thumbnail.Size.Normal) + "'>";
                                    p.ProductImageOrigin = Thumbnail.getURL(pv.Image, Thumbnail.Size.Source);
                                }
                                else if (!string.IsNullOrEmpty(product.ProductImage))
                                {
                                    p.ProductImage = "<img src='" + Thumbnail.getURL(product.ProductImage, Thumbnail.Size.Normal) + "'>";
                                    p.ProductImageOrigin = Thumbnail.getURL(product.ProductImage, Thumbnail.Size.Source);
                                }

                                if (gettotal == 1)
                                {
                                    double total = PJUtils.TotalProductQuantityInstock(AgentID, SKU);
                                    var mainstock = PJUtils.TotalProductQuantityInstock(1, SKU);
                                    p.QuantityMainInstock = mainstock;
                                    p.QuantityMainInstockString = string.Format("{0:N0}", mainstock);
                                    p.QuantityInstock = total;
                                    p.QuantityInstockString = string.Format("{0:N0}", total);
                                }

                                p.SKU = SKU;
                                p.Giabansi = Convert.ToDouble(pv.Regular_Price);
                                p.stringGiabansi = string.Format("{0:N0}", pv.Regular_Price);
                                p.Giabanle = Convert.ToDouble(pv.RetailPrice);
                                p.stringGiabanle = string.Format("{0:N0}", pv.RetailPrice);
                                ps.Add(p);
                            }
                        }
                    }
                    else // Nếu sản phẩm cha là simple thì...
                    {
                        string SKU = product.ProductSKU.Trim().ToUpper();
                        double mainstock = PJUtils.TotalProductQuantityInstock(1, SKU);

                        ProductGetOut p = new ProductGetOut();
                        p.ID = product.ID;
                        p.ProductName = product.ProductTitle;
                        p.ProductVariable = "";
                        p.ProductVariableSave = "";
                        p.ProductVariableName = "";
                        p.ProductVariableValue = "";
                        p.ProductType = 1;
                        p.ProductImage = "<img src='" + Thumbnail.getURL(product.ProductImage, Thumbnail.Size.Normal) + "'>";
                        p.ProductImageOrigin = Thumbnail.getURL(product.ProductImage, Thumbnail.Size.Source);
                        p.SKU = SKU;
                        p.QuantityMainInstock = mainstock;
                        p.QuantityMainInstockString = string.Format("{0:N0}", mainstock);
                        p.QuantityInstock = mainstock;
                        p.QuantityInstockString = string.Format("{0:N0}", mainstock);
                        p.Giabansi = Convert.ToDouble(product.Regular_Price);
                        p.stringGiabansi = string.Format("{0:N0}", product.Regular_Price);
                        p.Giabanle = Convert.ToDouble(product.Retail_Price);
                        p.stringGiabanle = string.Format("{0:N0}", product.Retail_Price);
                        ps.Add(p);
                    }
                }
                else // Nếu không nằm trong table Product thì...
                {
                    var productvariable = ProductVariableController.GetBySKU(textsearch.Trim().ToUpper());

                    // Nếu sản phẩm là con (nằm trong table ProductVariable) thì...
                    if (productvariable != null)
                    {
                        string SKU = productvariable.SKU.Trim().ToUpper();

                        var variables = ProductVariableValueController.GetByProductVariableSKU(SKU);

                        if (variables.Count > 0)
                        {
                            string variablename = "";
                            string variablevalue = "";
                            string variable = "";
                            string variablesave = "";

                            foreach (var v in variables)
                            {
                                variable += v.VariableName + ": " + v.VariableValue + "<br/>";
                                variablesave += v.VariableName.Trim() + ": " + v.VariableValue.Trim() + "|";
                                variablename += v.VariableName + "|";
                                variablevalue += v.VariableValue + "|";
                            }

                            double mainstock = PJUtils.TotalProductQuantityInstock(1, SKU);

                            ProductGetOut p = new ProductGetOut();
                            p.ID = productvariable.ID;

                            var _product = ProductController.GetBySKU(productvariable.ParentSKU);
                            if (_product != null)
                                p.ProductName = _product.ProductTitle;

                            p.ProductVariable = variable;
                            p.ProductVariableSave = variablesave;
                            p.ProductVariableName = variablename;
                            p.ProductVariableValue = variablevalue;
                            p.ProductType = 2;

                            if (!string.IsNullOrEmpty(productvariable.Image))
                            {
                                p.ProductImage = "<img src='" + Thumbnail.getURL(productvariable.Image, Thumbnail.Size.Normal) + "'>";
                                p.ProductImageOrigin = Thumbnail.getURL(productvariable.Image, Thumbnail.Size.Source);
                            }
                            else if (_product != null && !string.IsNullOrEmpty(_product.ProductImage))
                            {
                                p.ProductImage = "<img src='" + Thumbnail.getURL(_product.ProductImage, Thumbnail.Size.Normal) + "'>";
                                p.ProductImageOrigin = Thumbnail.getURL(_product.ProductImage, Thumbnail.Size.Source);
                            }

                            p.SKU = SKU;
                            p.QuantityMainInstock = mainstock;
                            p.QuantityMainInstockString = string.Format("{0:N0}", mainstock);
                            p.QuantityInstock = mainstock;
                            p.QuantityInstockString = string.Format("{0:N0}", mainstock);
                            p.Giabansi = Convert.ToDouble(productvariable.Regular_Price);
                            p.stringGiabansi = string.Format("{0:N0}", productvariable.Regular_Price);
                            p.Giabanle = Convert.ToDouble(productvariable.Regular_Price);
                            p.stringGiabanle = string.Format("{0:N0}", productvariable.Regular_Price);
                            ps.Add(p);
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
            public string ProductVariableSave { get; set; }
            public string ProductVariableName { get; set; }
            public string ProductVariableValue { get; set; }
            public int ProductType { get; set; }
            public string ProductImage { get; set; }
            public string ProductImageOrigin { get; set; }
            public string QuantityMainInstockString { get; set; }
            public double QuantityMainInstock { get; set; }
            public string QuantityInstockString { get; set; }
            public double QuantityInstock { get; set; }
            public string SKU { get; set; }
            public double Giabanle { get; set; }
            public string stringGiabanle { get; set; }
            public double Giabansi { get; set; }
            public string stringGiabansi { get; set; }
        }

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            string barcodeValue = "";
            string productPrint = "";
            string barcodeImage = "";
            string[] list = hdfListProduct.Value.Split(';');
            if (list.Count() > 0)
            {
                var temps = new List<String>();
                productPrint += "<div class='qcode {0}'>";
                int[] categoryIDs = new int[] { 44 };
                bool isCol4 = true;
                for (int i = 0; i < list.Length - 1; i++)
                {
                  
                    string[] list2 = list[i].Split(',');
                    int quantity = list2[2].ToInt();
                    for (int j = 0; j < quantity; j++)
                    {
                        var variable = ProductVariableController.GetBySKU(list2[1]);
                        if (variable != null)
                        {
                            var product = ProductController.GetByID(variable.ProductID.Value);
                            isCol4 = categoryIDs.Contains(product.CategoryID.Value);
                        }
                        else
                        {
                            var product = ProductController.GetBySKU(list2[1]);
                            isCol4 = categoryIDs.Contains(product.CategoryID.Value);
                        }

                        barcodeValue = list2[1];
                        var imageName = String.Format("{0}{1}.png", DateTime.UtcNow.ToString("yyyyMMddHHmmss"), Guid.NewGuid());
                        barcodeImage = "/uploads/barcodes/" + imageName;

                        System.Drawing.Image barCode = PJUtils.MakeBarcodeImage(barcodeValue, 2,true);

                        barCode.Save(HttpContext.Current.Server.MapPath("" + barcodeImage + ""), ImageFormat.Png);

                        productPrint += "<div class='item'>";
                        productPrint += "<div class='img'><img src='data:image/png;base64, " + Convert.ToBase64String(File.ReadAllBytes(Server.MapPath("" + barcodeImage + ""))) + "'></div>";
                        productPrint += "<div><h1>" + barcodeValue + "</h1></div>";
                        productPrint += "</div>";

                        temps.Add(imageName);
                    }
                }
                productPrint += "</div>";
                productPrint = String.Format(productPrint, isCol4 ? "col-4" : "");
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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { printBarcode() });", true);
            }
        }
    }
}