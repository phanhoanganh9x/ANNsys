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
    public partial class chi_tiet_nhan_hang : System.Web.UI.Page
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
            int ID = Request.QueryString["id"].ToInt(0);
            if (ID > 0)
            {
                string username = Request.Cookies["userLoginSystem"].Value;
                var acc = AccountController.GetByUsername(username);
                if (acc != null)
                {
                    int agentIDReceive = Convert.ToInt32(acc.AgentID);
                    var mp = MoveProController.GetByIDAndAgentIDReceive(ID, agentIDReceive);
                    if (mp != null)
                    {
                        ViewState["ID"] = ID;
                        int agentIDSend = Convert.ToInt32(mp.AgentIDSend);
                        string agentSendname = "";
                        string agentReceivename = "";

                        var agentSend = AgentController.GetByID(agentIDSend);
                        if (agentSend != null)
                        {
                            agentSendname = agentSend.AgentName;
                        }
                        var agentReceive = AgentController.GetByID(agentIDReceive);
                        if (agentReceive != null)
                        {
                            agentReceivename = agentReceive.AgentName;
                        }
                        ltrAgentReceive.Text = agentReceivename;
                        ltrAgentSend.Text = agentSendname;



                        int mpStatus = Convert.ToInt32(mp.Status);
                        ltrNote.Text = mp.Note;

                        if (mpStatus < 3)
                        {
                            ltrbutton1.Text = "<a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth\" onclick=\"inProduct()\">Cập nhật</a>";
                            ltrbutton2.Text = "<div class=\"float-right\"><a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth\" onclick=\"inProduct()\">Cập nhật</a></div>";
                        }
                        #region LoadDDL
                        //Lấy trạng thái
                        StringBuilder htmlStatus = new StringBuilder();
                        htmlStatus.Append("<select id=\"moveProStatus\" class=\"form-control\" style=\"width: 20%; display: inline-block\">");
                        if (mpStatus == 2)
                        {
                            htmlStatus.Append("<option value=\"2\" selected>Đã chuyển</option>");
                            htmlStatus.Append("<option value=\"3\">Đã hoàn tất</option>");
                        }
                        else if (mpStatus == 3)
                        {
                            htmlStatus.Append("<option value=\"2\">Đã chuyển</option>");
                            htmlStatus.Append("<option value=\"3\" selected>Đã hoàn tất</option>");
                        }
                        htmlStatus.Append("</select>");
                        ltrMoProStatus.Text = htmlStatus.ToString();

                        //Lấy nhà cung cấp
                        //var supplier = SupplierController.GetAllWithIsHidden(false);
                        //StringBuilder htmlSupplier = new StringBuilder();
                        //htmlSupplier.Append("<select id=\"supplierList\" class=\"form-control\" style=\"width: 20%; float: left; margin-right: 10px\">");
                        //htmlSupplier.Append("<option value=\"0\">Tất cả nhà cung cấp</option>");
                        //if (supplier.Count > 0)
                        //{
                        //    foreach (var s in supplier)
                        //    {
                        //        htmlSupplier.Append("<option value=\"" + s.ID + "\">" + s.SupplierName + "</option>");
                        //    }
                        //}
                        //htmlSupplier.Append("</select>");
                        //ltrSupplier.Text = htmlSupplier.ToString();
                        #endregion
                        #region Lấy danh sách sản phẩm
                        var mpDetail = MoveProDetailController.GetByMoveProID(ID);
                        string html = "";
                        if (mpDetail.Count > 0)
                        {
                            string listSKU = "";
                            foreach (var item in mpDetail)
                            {
                                listSKU += item.SKU + "|";
                                int ProductType = Convert.ToInt32(item.ProductType);
                                int ProductID = Convert.ToInt32(item.ProductID);
                                int ProductVariableID = Convert.ToInt32(item.ProductVariableID);

                                string SKU = item.SKU;
                                double QuantityInstock = 0;
                                string ProductImageOrigin = "";
                                string ProductVariable = "";
                                string ProductName = "";
                                int PID = 0;
                                string ProductVariableName = "";
                                string ProductVariableValue = "";
                                string ProductVariableSave = "";
                                double QuantityMainInstock = 0;
                                string ProductImage = "";
                                string QuantityMainInstockString = "";
                                string QuantityInstockString = "";
                                string productVariableDescription = item.ProductVariableDescription;
                                string SupplierName = item.SupplierName;
                                if (ProductType == 1)
                                {
                                    PID = ProductID;
                                    var product = ProductController.GetBySKU(SKU);
                                    if (product != null)
                                    {
                                        double total = PJUtils.TotalProductQuantityInstock(agentIDSend, SKU);

                                        string variablename = "";
                                        string variablevalue = "";
                                        string variable = "";

                                        double mainstock = PJUtils.TotalProductQuantityInstock(1, SKU);

                                        QuantityInstock = total;
                                        QuantityInstockString = string.Format("{0:N0}", total);

                                        var img = ProductImageController.GetFirstByProductID(product.ID);
                                        if (img != null)
                                        {
                                            ProductImage = "<img src=\"" + Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Small) + "\" alt=\"\" style=\"width: 50px\"  />";
                                            ProductImageOrigin = Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Small);
                                        }
                                        else
                                        {
                                            ProductImage = "";
                                            ProductImageOrigin = "";
                                        }


                                        ProductVariable = variable;
                                        ProductName = product.ProductTitle;

                                        QuantityMainInstock = mainstock;
                                        QuantityMainInstockString = string.Format("{0:N0}", mainstock);
                                        ProductVariableSave = item.ProductVariableDescription;

                                        ProductVariableName = variablename;
                                        ProductVariableValue = variablevalue;
                                    }
                                }
                                else
                                {
                                    PID = ProductVariableID;
                                    var productvariable = ProductVariableController.GetBySKU(SKU);
                                    if (productvariable != null)
                                    {
                                        SKU = productvariable.SKU.Trim().ToUpper();
                                        double total = PJUtils.TotalProductQuantityInstock(agentIDSend, SKU);


                                        string variablename = "";
                                        string variablevalue = "";
                                        string variable = "";

                                        string[] vs = productVariableDescription.Split('|');
                                        if (vs.Length - 1 > 0)
                                        {
                                            for (int i = 0; i < vs.Length - 1; i++)
                                            {
                                                string[] items = vs[i].Split(':');
                                                variable += items[0] + ":" + items[1] + "<br/>";
                                                variablename += items[0] + "|";
                                                variablevalue += items[1] + "|";
                                            }
                                        }

                                        double mainstock = PJUtils.TotalProductQuantityInstock(1, SKU);

                                        QuantityInstock = total;
                                        QuantityInstockString = string.Format("{0:N0}", total);
                                        ProductImage = "<img src=\"" + Thumbnail.getURL(productvariable.Image, Thumbnail.Size.Small) + "\" alt=\"\" style=\"width:50px;\" />";
                                        ProductImageOrigin = Thumbnail.getURL(productvariable.Image, Thumbnail.Size.Small);

                                        ProductVariable = variable;
                                        var product1 = ProductController.GetByID(Convert.ToInt32(productvariable.ProductID));
                                        if (product1 != null)
                                            ProductName = product1.ProductTitle;

                                        QuantityMainInstock = mainstock;
                                        QuantityMainInstockString = string.Format("{0:N0}", mainstock);
                                        ProductVariableSave = item.ProductVariableDescription;

                                        ProductVariableName = variablename;
                                        ProductVariableValue = variablevalue;
                                    }
                                }
                                html += "<tr class=\"product-result\" data-mpdid=\"" + item.ID + "\" data-quantityinstock=\"" + QuantityInstock
                                    + "\" data-productimageorigin=\"" + ProductImageOrigin
                                    + "\" data-productvariable=\"" + ProductVariableSave
                                    + "\" data-productname=\"" + ProductName
                                    + "\" data-sku=\"" + SKU + "\" data-producttype=\"" + ProductType
                                    + "\" data-id=\"" + item.ID + "\" data-productnariablename=\"" + ProductVariableName
                                    + "\" data-productvariablevalue =\"" + ProductVariableValue + "\">";
                                html += "   <td>" + ProductImage + "";
                                html += "   <td>" + ProductName + "</td>";
                                html += "   <td>" + SKU + "</td>";
                                html += "   <td>" + SupplierName + "</td>";
                                html += "   <td>" + ProductVariable + "</td>";
                                html += "   <td>" + item.QuantiySend + "</td>";
                                html += "   <td style=\"width:20%;\"><input type=\"text\" min=\"0\" class=\"form-control in-quanlity\" style=\"width: 40%;margin: 0 auto;\" value=\"" + item.QuantityReceive + "\"  onkeyup=\"checkQuantiy($(this))\" onkeypress='return event.charCode >= 48 && event.charCode <= 57'/></td>";
                                //html += "   <td><a href=\"javascript:;\" onclick=\"deleteRow($(this))\">Xóa</a></td>";
                                //html += "   <td></td>";
                                html += "</tr>";

                            }
                            ltrProducts.Text = html;
                            hdfListProductSend.Value = listSKU;
                        }
                        #endregion
                    }
                }

            }
        }
        [WebMethod]
        public static string getProduct(string textsearch, int typeinout)
        {
            List<ProductGetOut> ps = new List<ProductGetOut>();
            string username = HttpContext.Current.Request.Cookies["userLoginSystem"].Value;
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
                                                p.ProductImage = "<img src=\"" + Thumbnail.getURL(pv.Image, Thumbnail.Size.Small) + "\" alt=\"\" style=\"width: 50px\" />";
                                                p.ProductImageOrigin = Thumbnail.getURL(pv.Image, Thumbnail.Size.Small);
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
                                        if (img != null)
                                        {
                                            p.ProductImage = "<img src=\"" + Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Small) + "\" alt=\"\" style=\"width: 50px\"  />";
                                            p.ProductImageOrigin = Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Small);
                                        }
                                        else
                                        {
                                            p.ProductImage = "";
                                            p.ProductImageOrigin = "";
                                        }
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
                                            p.ProductImage = "<img src=\"" + Thumbnail.getURL(pv.Image, Thumbnail.Size.Small) + "\" alt=\"\" style=\"width: 50px\"  />";
                                            p.ProductImageOrigin = Thumbnail.getURL(pv.Image, Thumbnail.Size.Small);
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
                                    if (img != null)
                                    {
                                        p.ProductImage = "<img src=\"" + Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Small) + "\" alt=\"\" style=\"width: 50px\"  />";
                                        p.ProductImageOrigin = Thumbnail.getURL(img.ProductImage, Thumbnail.Size.Small);
                                    }
                                    else
                                    {
                                        p.ProductImage = "";
                                        p.ProductImageOrigin = "";
                                    }
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

                        var productvariable = ProductVariableController.GetBySKU(textsearch);
                        if (productvariable != null)
                        {
                            string SKU = productvariable.SKU.Trim().ToUpper();
                            var check = StockManagerController.GetBySKU(AgentID, SKU);
                            if (check.Count > 0)
                            {
                                double total = PJUtils.TotalProductQuantityInstock(AgentID, SKU);
                                if (total > 0)
                                {
                                    var variables = ProductVariableValueController.GetByProductVariableID(productvariable.ID);

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
                                        p.ID = productvariable.ID;
                                        var product = ProductController.GetByID(Convert.ToInt32(productvariable.ProductID));
                                        if (product != null)
                                            p.ProductName = product.ProductTitle;
                                        p.ProductVariable = variable;
                                        p.ProductVariableName = variablename;
                                        p.ProductVariableValue = variablevalue;
                                        p.ProductType = 2;
                                        p.ProductImage = "<img src=\"" + Thumbnail.getURL(productvariable.Image, Thumbnail.Size.Small) + "\" alt=\"\" style=\"width:50px;\" />";
                                        p.ProductImageOrigin = Thumbnail.getURL(productvariable.Image, Thumbnail.Size.Small);
                                        p.SKU = productvariable.SKU.Trim().ToUpper();
                                        p.QuantityInstock = total;
                                        p.QuantityInstockString = string.Format("{0:N0}", total);
                                        int supplierID = 0;
                                        if (productvariable.SupplierID != null)
                                            supplierID = productvariable.SupplierID.ToString().ToInt(0);
                                        p.SupplierID = supplierID;
                                        string supplierName = "";
                                        if (!string.IsNullOrEmpty(productvariable.SupplierName))
                                            supplierName = productvariable.SupplierName;
                                        p.SupplierName = supplierName;
                                        ps.Add(p);
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
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0 || acc.RoleID == 1)
                {
                    int mpID = ViewState["ID"].ToString().ToInt(0);
                    var mp = MoveProController.GetByID(mpID);
                    if (mp != null)
                    {
                        int AgentID = Convert.ToInt32(acc.AgentID);
                        int AgentIDReceive = hdfAgentID.Value.ToInt(0);
                        string agentSendname = "";
                        string agentReceivename = "";
                        var agentSend = AgentController.GetByID(AgentID);
                        if (agentSend != null)
                        {
                            agentSendname = agentSend.AgentName;
                        }
                        var agentReceive = AgentController.GetByID(AgentIDReceive);
                        if (agentReceive != null)
                        {
                            agentReceivename = agentReceive.AgentName;
                        }
                        string list = hdfvalue.Value;
                        string note = hdfNote.Value;

                        int moveProStatus = hdfStatus.Value.ToInt(0);
                        MoveProController.UpdateStatus(mpID, moveProStatus, currentDate, username);
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
                                int mpdid = itemValue[9].ToInt(0);

                                if (mpdid > 0)
                                {
                                    var mpdetail = MoveProDetailController.GetByID(mpdid);
                                    if (mpdetail != null)
                                    {
                                        if (moveProStatus == 3)
                                        {
                                            if (producttype == 1)
                                            {
                                                StockManagerController.Insert(
                                                    new tbl_StockManager {
                                                        AgentID = AgentID,
                                                        ProductID = ID,
                                                        ProductVariableID = 0,
                                                        Quantity = Quantity,
                                                        QuantityCurrent = 0,
                                                        Type = 1,
                                                        NoteID = note,
                                                        OrderID = 0,
                                                        Status = 7,
                                                        SKU = SKU,
                                                        CreatedDate = currentDate,
                                                        CreatedBy = username,
                                                        MoveProID = mpID,
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
                                                        Quantity = Quantity,
                                                        QuantityCurrent = 0,
                                                        Type = 1,
                                                        NoteID = note,
                                                        OrderID = 0,
                                                        Status = 7,
                                                        SKU = SKU,
                                                        CreatedDate = currentDate,
                                                        CreatedBy = username,
                                                        MoveProID = mpID,
                                                        ParentID = parentID
                                                    });
                                            }
                                            MoveProDetailController.UpdateQuantityReceive(mpdid, Quantity, currentDate, username);
                                        }
                                        else
                                        {
                                            MoveProDetailController.UpdateQuantityReceive(mpdid, Quantity, currentDate, username);
                                        }
                                    }
                                }
                            }

                        }
                        PJUtils.ShowMessageBoxSwAlert("Cập nhật nhận hàng thành công", "s", true, Page);
                    }
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0 || acc.RoleID == 1)
                {
                    int AgentID = Convert.ToInt32(acc.AgentID);
                    int ID = hdfMPDID.Value.ToInt(0);
                    if (ID > 0)
                    {
                        var mpd = MoveProDetailController.GetByID(ID);
                        if (mpd != null)
                        {
                            if (mpd.IsCount == true)
                            {
                                if (mpd.ProductType == 1)
                                {
                                    StockManagerController.Insert(
                                        new tbl_StockManager {
                                            AgentID = AgentID,
                                            ProductID = ID,
                                            ProductVariableID = 0,
                                            Quantity = Convert.ToDouble(mpd.QuantiySend),
                                            QuantityCurrent = 0,
                                            Type = 1,
                                            NoteID = "Nhập lại sản phẩm khi xóa khỏi đợt chuyển hàng",
                                            OrderID = 0,
                                            Status = 6,
                                            SKU = mpd.SKU,
                                            CreatedDate = currentDate,
                                            CreatedBy = username,
                                            MoveProID = Convert.ToInt32(mpd.MoveProID),
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
                                            Quantity = Convert.ToDouble(mpd.QuantiySend),
                                            QuantityCurrent = 0,
                                            Type = 1,
                                            NoteID = "Nhập lại sản phẩm khi xóa khỏi đợt chuyển hàng",
                                            OrderID = 0,
                                            Status = 6,
                                            SKU = mpd.SKU,
                                            CreatedDate = currentDate,
                                            CreatedBy = username,
                                            MoveProID = Convert.ToInt32(mpd.MoveProID),
                                            ParentID = parentID
                                        });
                                }
                            }
                            MoveProDetailController.Delete(ID);
                            PJUtils.ShowMessageBoxSwAlert("Xóa thành công", "s", true, Page);
                        }
                    }
                }
            }
        }
    }
}