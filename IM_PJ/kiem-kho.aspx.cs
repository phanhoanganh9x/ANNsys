using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Utils;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class kiem_kho : System.Web.UI.Page
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
                LoadData();
            }
        }
        public void LoadData()
        {
            var supplier = SupplierController.GetAllWithIsHidden(false);
            StringBuilder html = new StringBuilder();
            //html.Append("<select id=\"supplierList\" class=\"select2 form-control\" style=\"width: 20%; float: left; margin-right: 10px\">");
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
        }

        [WebMethod]
        public static string getProduct(string textsearch, int typeinout)
        {
            List<ProductGetOut> ps = new List<ProductGetOut>();
            string username = HttpContext.Current.Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                int AgentID = Convert.ToInt32(acc.AgentID);
                if (typeinout == 1)
                {
                    var products = ProductController.GetByTextSearchIsHidden(textsearch.Trim(), false);
                    if (products.Count > 0)
                    {
                        foreach (var item in products)
                        {
                            var productvariable = ProductVariableController.GetProductID(item.ID);
                            if (productvariable.Count > 0)
                            {
                                foreach (var pv in productvariable)
                                {
                                    string SKU = pv.SKU.Trim().ToUpper();
                                    var check = StockManagerController.GetBySKU(AgentID, SKU);
                                    if (check.Count > 0)
                                    {
                                        double total = PJUtils.TotalProductQuantityInstock(AgentID, SKU);
                                        if (total > 0)
                                        {
                                            var variables = ProductVariableValueController.GetByProductVariableID(pv.ID);
                                            if (variables.Count > 0)
                                            {
                                                string variablename = "";
                                                string variablevalue = "";
                                                string variable = "";

                                                foreach (var v in variables)
                                                {
                                                    variable += v.VariableName.Trim() + ":" + v.VariableValue.Trim() + "|";
                                                    variablename += v.VariableName.Trim() + "|";
                                                    variablevalue += v.VariableValue.Trim() + "|";
                                                }

                                                ProductGetOut p = new ProductGetOut();
                                                p.ID = pv.ID;
                                                p.ProductName = item.ProductTitle;
                                                p.ProductVariable = variable;
                                                p.ProductVariableName = variablename;
                                                p.ProductVariableValue = variablevalue;
                                                p.ProductType = 2;
                                                p.ProductImage = "<img src=\"" + Thumbnail.getURL(pv.Image, Thumbnail.Size.Small) + "\" style=\"width: 50px\" />";
                                                p.ProductImageOrigin = Thumbnail.getURL(pv.Image, Thumbnail.Size.Source);
                                                p.QuantityInstock = total;
                                                p.QuantityInstockString = string.Format("{0:N0}", total);
                                                p.SKU = SKU;
                                                int supplierID = 0;
                                                if (pv.SupplierID != null)
                                                    supplierID = pv.SupplierID.ToString().ToInt(0);
                                                p.SupplierID = supplierID;
                                                string supplierName = "";
                                                if (!string.IsNullOrEmpty(pv.SupplierName))
                                                    supplierName = pv.SupplierName;
                                                p.SupplierName = supplierName;
                                                ps.Add(p);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string SKU = item.ProductSKU.Trim().ToUpper();
                                var check = StockManagerController.GetBySKU(AgentID, SKU);
                                if (check.Count > 0)
                                {
                                    double total = PJUtils.TotalProductQuantityInstock(AgentID, SKU);
                                    if (total > 0)
                                    {
                                        string variablename = "";
                                        string variablevalue = "";
                                        string variable = "";

                                        ProductGetOut p = new ProductGetOut();
                                        p.ID = item.ID;
                                        p.ProductName = item.ProductTitle;
                                        p.ProductVariable = variable;
                                        p.ProductVariableName = variablename;
                                        p.ProductVariableValue = variablevalue;
                                        p.ProductType = 1;
                                        var img = ProductImageController.GetFirstByProductID(item.ID);
                                        p.ProductImage = "<img src=\"" + Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Small) + "\" style=\"width: 50px\"  />";
                                        p.ProductImageOrigin = Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Source);
                                        p.QuantityInstock = total;
                                        p.QuantityInstockString = string.Format("{0:N0}", total);
                                        p.SKU = SKU;
                                        int supplierID = 0;
                                        if (item.SupplierID != null)
                                            supplierID = item.SupplierID.ToString().ToInt(0);
                                        p.SupplierID = supplierID;
                                        string supplierName = "";
                                        if (!string.IsNullOrEmpty(item.SupplierName))
                                            supplierName = item.SupplierName;
                                        p.SupplierName = supplierName;
                                        ps.Add(p);
                                    }
                                }

                            }
                        }
                    }
                }
                else
                {
                    var products = ProductController.GetBySKU(textsearch);
                    if (products != null)
                    {
                        var productvariable = ProductVariableController.GetProductID(products.ID);
                        if (productvariable.Count > 0)
                        {
                            foreach (var pv in productvariable)
                            {
                                string SKU = pv.SKU.Trim().ToUpper();
                                var check = StockManagerController.GetBySKU(AgentID, SKU);
                                if (check.Count > 0)
                                {
                                    double total = PJUtils.TotalProductQuantityInstock(AgentID, SKU);

                                    if (total > 0)
                                    {
                                        var variables = ProductVariableValueController.GetByProductVariableID(pv.ID);
                                        if (variables.Count > 0)
                                        {
                                            string variablename = "";
                                            string variablevalue = "";
                                            string variable = "";

                                            foreach (var v in variables)
                                            {
                                                variable += v.VariableName.Trim() + ":" + v.VariableValue.Trim() + "|";
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
                                            p.ProductImage = "<img src=\"" + Thumbnail.getURL(pv.Image, Thumbnail.Size.Small) + "\" style=\"width: 50px\"  />";
                                            p.ProductImageOrigin = Thumbnail.getURL(pv.Image, Thumbnail.Size.Source);
                                            p.QuantityInstock = total;
                                            p.QuantityInstockString = string.Format("{0:N0}", total);
                                            p.SKU = SKU;
                                            int supplierID = 0;
                                            if (pv.SupplierID != null)
                                                supplierID = pv.SupplierID.ToString().ToInt(0);
                                            p.SupplierID = supplierID;
                                            string supplierName = "";
                                            if (!string.IsNullOrEmpty(pv.SupplierName))
                                                supplierName = pv.SupplierName;
                                            p.SupplierName = supplierName;
                                            ps.Add(p);
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            string SKU = products.ProductSKU.Trim().ToUpper();
                            var check = StockManagerController.GetBySKU(AgentID, SKU);
                            if (check.Count > 0)
                            {
                                double total = PJUtils.TotalProductQuantityInstock(AgentID, SKU);
                                if (total > 0)
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
                                    var img = ProductImageController.GetFirstByProductID(products.ID);
                                    p.ProductImage = "<img src=\"" + Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Small) + "\" style=\"width: 50px\" />";
                                    p.ProductImageOrigin = Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Source);
                                    p.SKU = SKU;
                                    p.QuantityInstock = total;
                                    p.QuantityInstockString = string.Format("{0:N0}", total);
                                    int supplierID = 0;
                                    if (products.SupplierID != null)
                                        supplierID = products.SupplierID.ToString().ToInt(0);
                                    p.SupplierID = supplierID;
                                    string supplierName = "";
                                    if (!string.IsNullOrEmpty(products.SupplierName))
                                        supplierName = products.SupplierName;
                                    p.SupplierName = supplierName;
                                    ps.Add(p);
                                }
                            }

                        }
                    }
                    else
                    {

                        var productvariable = ProductVariableController.GetAllBySKU(textsearch);
                        if (productvariable != null)
                        {
                            foreach (var value in productvariable)
                            {

                                string SKU = value.SKU.Trim().ToUpper();
                                var check = StockManagerController.GetBySKU(AgentID, SKU);
                                if (check.Count > 0)
                                {
                                    double total = PJUtils.TotalProductQuantityInstock(AgentID, SKU);
                                    if (total > 0)
                                    {
                                        var variables = ProductVariableValueController.GetByProductVariableID(value.ID);

                                        if (variables.Count > 0)
                                        {
                                            string variablename = "";
                                            string variablevalue = "";
                                            string variable = "";

                                            foreach (var v in variables)
                                            {
                                                variable += v.VariableName + ":" + v.VariableValue + "|";
                                                variablename += v.VariableName + "|";
                                                variablevalue += v.VariableValue + "|";
                                            }

                                            ProductGetOut p = new ProductGetOut();
                                            p.ID = value.ID;
                                            var product = ProductController.GetByID(Convert.ToInt32(value.ProductID));
                                            if (product != null)
                                                p.ProductName = product.ProductTitle;
                                            p.ProductVariable = variable;
                                            p.ProductVariableName = variablename;
                                            p.ProductVariableValue = variablevalue;
                                            p.ProductType = 2;
                                            p.ProductImage = "<img src=\"" + Thumbnail.getURL(value.Image, Thumbnail.Size.Small) + "\" style=\"width:50px;\" />";
                                            p.ProductImageOrigin = Thumbnail.getURL(value.Image, Thumbnail.Size.Small);
                                            p.SKU = value.SKU.Trim().ToUpper();
                                            p.QuantityInstock = total;
                                            p.QuantityInstockString = string.Format("{0:N0}", total);
                                            int supplierID = 0;
                                            if (value.SupplierID != null)
                                                supplierID = value.SupplierID.ToString().ToInt(0);
                                            p.SupplierID = supplierID;
                                            string supplierName = "";
                                            if (!string.IsNullOrEmpty(value.SupplierName))
                                                supplierName = value.SupplierName;
                                            p.SupplierName = supplierName;
                                            ps.Add(p);
                                        }
                                    }
                                }
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
            public string QuantityInstockString { get; set; }
            public double QuantityInstock { get; set; }
            public string SKU { get; set; }
            public int SupplierID { get; set; }
            public string SupplierName { get; set; }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0 || acc.RoleID == 1)
                {
                    int AgentID = Convert.ToInt32(acc.AgentID);
                    string list = hdfvalue.Value;
                    string note = hdfNote.Value;
                    string[] items = list.Split(';');
                    if (items.Length - 1 > 0)
                    {
                        int SessionInOutID = SessionInOutController.Insert(currentDate, note, AgentID, 2, currentDate, username).ToInt(0);
                        if (SessionInOutID > 0)
                        {
                            for (int i = 0; i < items.Length - 1; i++)
                            {
                                //list += id + "," + sku + "," + producttype + "," + productnariablename + "," + productvariablevalue + "," + quantity + "|";
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

                                string noteeach = itemValue[9];
                                double QuantityInStock = Convert.ToDouble(itemValue[10]);

                                if (!string.IsNullOrEmpty(noteeach))
                                {
                                    noteeach = "Cập nhật số lượng sản phẩm khi kiểm kho ngày: " + string.Format("0:dd/MM/yyyy", currentDate);
                                }


                                if (Quantity > QuantityInStock)
                                {
                                    double slIn = Quantity - QuantityInStock;
                                    if (producttype == 1)
                                    {
                                        StockManagerController.Insert(
                                            new tbl_StockManager {
                                                AgentID = AgentID,
                                                ProductID = ID,
                                                ProductVariableID = 0,
                                                Quantity = slIn,
                                                QuantityCurrent = 0,
                                                Type = 1,
                                                NoteID = note,
                                                OrderID = 0,
                                                Status = 1,
                                                SKU = SKU,
                                                CreatedDate = currentDate,
                                                CreatedBy = username,
                                                ModifiedDate = currentDate,
                                                ModifiedBy = username,
                                                MoveProID = 0,
                                                ParentID = ID
                                            });
                                    }
                                    else
                                    {
                                        int parentID = 0;
                                        string parentSKU = "";
                                        var productV = ProductVariableController.GetByID(ID);
                                        if (productV != null)
                                            parentSKU = productV.ParentSKU;
                                        if (!string.IsNullOrEmpty(parentSKU))
                                        {
                                            var product = ProductController.GetBySKU(parentSKU);
                                            if (product != null)
                                                parentID = product.ID;
                                        }
                                        StockManagerController.Insert(
                                            new tbl_StockManager {
                                                AgentID = AgentID,
                                                ProductID = 0,
                                                ProductVariableID = ID,
                                                Quantity = slIn,
                                                QuantityCurrent = 0,
                                                Type = 1,
                                                NoteID = note,
                                                OrderID = 0,
                                                Status = 1,
                                                SKU = SKU,
                                                CreatedDate = currentDate,
                                                CreatedBy = username,
                                                ModifiedDate = currentDate,
                                                ModifiedBy = username,
                                                MoveProID = 0,
                                                ParentID = parentID
                                            });
                                    }
                                }
                                else if (Quantity < QuantityInStock)
                                {
                                    double slOut = QuantityInStock - Quantity;
                                    if (producttype == 1)
                                    {
                                        StockManagerController.Insert(
                                            new tbl_StockManager {
                                                AgentID = AgentID,
                                                ProductID = ID,
                                                ProductVariableID = 0,
                                                Quantity = slOut,
                                                QuantityCurrent = 0,
                                                Type = 2,
                                                NoteID = noteeach,
                                                OrderID = 0,
                                                Status = 2,
                                                SKU = SKU,
                                                CreatedDate = currentDate,
                                                CreatedBy = username,
                                                ModifiedDate = currentDate,
                                                ModifiedBy = username,
                                                MoveProID = 0,
                                                ParentID = ID
                                            });
                                    }
                                    else
                                    {
                                        int parentID = 0;
                                        string parentSKU = "";
                                        var productV = ProductVariableController.GetByID(ID);
                                        if (productV != null)
                                            parentSKU = productV.ParentSKU;
                                        if (!string.IsNullOrEmpty(parentSKU))
                                        {
                                            var product = ProductController.GetBySKU(parentSKU);
                                            if (product != null)
                                                parentID = product.ID;
                                        }
                                        StockManagerController.Insert(
                                            new tbl_StockManager {
                                                AgentID = AgentID,
                                                ProductID = 0,
                                                ProductVariableID = ID,
                                                Quantity = slOut,
                                                QuantityCurrent = 0,
                                                Type = 2,
                                                NoteID = noteeach,
                                                OrderID = 0,
                                                Status = 2,
                                                SKU = SKU,
                                                CreatedDate = currentDate,
                                                CreatedBy = username,
                                                ModifiedDate = currentDate,
                                                ModifiedBy = username,
                                                MoveProID = 0,
                                                ParentID = parentID
                                            });
                                    }
                                }
                            }
                            PJUtils.ShowMessageBoxSwAlert("Kiểm kho thành công", "s", true, Page);
                        }
                    }
                }
            }
        }
    }
}