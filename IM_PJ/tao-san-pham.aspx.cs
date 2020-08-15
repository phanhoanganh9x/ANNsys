﻿using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Utils;
using MB.Extensions;
using Newtonsoft.Json;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace IM_PJ
{
    public partial class tao_san_pham : System.Web.UI.Page
    {

        public static string htmlAll = "";
        public static int element = 0;
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
                        hdfUserRole.Value = acc.RoleID.ToString();
                        if (acc.RoleID == 0 || acc.RoleID == 1 || acc.Username == "nhom_zalo502")
                        {
                            LoadSupplier();
                            LoadPDW();
                            LoadCategory();
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
        public void LoadPDW()
        {
            var variablename = VariableController.GetAllIsHidden(false);
            if (variablename.Count > 0)
            {
                ddlVariablename.Items.Clear();
                ddlVariablename.Items.Insert(0, new ListItem("Chọn thuộc tính", "0"));
                foreach (var p in variablename)
                {
                    ListItem listitem = new ListItem(p.VariableName, p.ID.ToString());
                    ddlVariablename.Items.Add(listitem);
                }
                ddlVariablename.DataBind();

            }
            ddlVariableValue.Items.Clear();
            ddlVariableValue.Items.Insert(0, new ListItem("Chọn giá trị", "0"));
        }

        public void BindVariableValue(int VariableID)
        {
            ddlVariableValue.Items.Clear();
            ddlVariableValue.Items.Insert(0, new ListItem("Chọn giá trị", "0"));
            if (VariableID > 0)
            {
                var variableValue = VariableValueController.GetByVariableID(VariableID);
                if (variableValue.Count > 0)
                {
                    foreach (var p in variableValue)
                    {
                        ListItem listitem = new ListItem(p.VariableValue, p.ID.ToString());
                        ddlVariableValue.Items.Add(listitem);
                    }
                }
                ddlVariableValue.DataBind();
            }
        }
        protected void ddlVariablename_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindVariableValue(ddlVariablename.SelectedValue.ToInt(0));
        }

        public void LoadSupplier()
        {
            var supplier = SupplierController.GetAllWithIsHidden(false);
            ddlSupplier.Items.Clear();
            ddlSupplier.Items.Insert(0, new ListItem("Chọn nhà cung cấp", "0"));
            if (supplier.Count > 0)
            {
                foreach (var p in supplier)
                {
                    ListItem listitem = new ListItem(p.SupplierName, p.ID.ToString());
                    ddlSupplier.Items.Add(listitem);
                }
                ddlSupplier.DataBind();
            }
        }

        public void LoadCategory()
        {
            var category = CategoryController.GetAllWithIsHidden(false);
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new ListItem("Chọn danh mục", "0"));
            if (category.Count > 0)
            {
                addItemCategory(0, "");
                ddlCategory.DataBind();
            }
        }

        public void addItemCategory(int id, string h = "")
        {
            var categories = CategoryController.GetByParentID("", id);
            
            if (categories.Count > 0)
            {
                foreach (var c in categories)
                {
                    ListItem listitem = new ListItem(h + c.CategoryName, c.ID.ToString());
                    ddlCategory.Items.Add(listitem);

                    addItemCategory(c.ID, h + "---");
                }
            }
        }
        public static void DeQuyCongTu(int el, int final, string r, List<Variable> listObject)
        {
            var currentElement = listObject[el];
            var name = currentElement.VariableName;
            var childrens = currentElement.Value;
            foreach (var item in childrens)
            {
                var variableID = VariableValueController.GetByID(Convert.ToInt32(item.Value)).VariableID;
                string rprev = r;
                int leng = el + 1;
                var skutext = VariableValueController.GetByID(Convert.ToInt32(item.Value)) != null ?
                        VariableValueController.GetByID(Convert.ToInt32(item.Value)).SKUText : "";
                if (leng < final)
                {
                    rprev += variableID + "*" + name + ":" + item.Value + "," + item.Name + "," + skutext + "-";
                    DeQuyCongTu(leng, listObject.Count, rprev, listObject);
                }
                else
                {
                    string a = r;
                    a += variableID + "*" + name + ":" + item.Value + "," + item.Name + "," + skutext + "|";
                    htmlAll += a;
                }
            }
        }

        [WebMethod]
        public static string getVariable(string list)
        {
            List<Variable> listparent = new List<Variable>();
            List<VariableGetOut> vg = new List<VariableGetOut>();
            string[] value = list.Split('|');
            for (int i = 0; i < value.Length - 1; i++)
            {
                Variable vr = new Variable();
                List<VariableValue> vl = new List<VariableValue>();
                string[] t = value[i].Split(':');
                vr.VariableName = t[0];
                string[] vl1 = t[1].Split(';');
                for (int k = 0; k < vl1.Length - 1; k++)
                {
                    string[] vl2 = vl1[k].Split('-');
                    VariableValue vvl = new VariableValue();
                    //vvl.ID = vl2[0].ToInt();
                    vvl.Value = vl2[0];
                    vvl.Name = vl2[1];
                    vl.Add(vvl);
                }
                vr.Value = vl;
                listparent.Add(vr);
            }
            htmlAll = "";
            DeQuyCongTu(element, listparent.Count, "", listparent);
            string[] item = htmlAll.Split('|');
            if (item.Count() > 0)
            {

                for (int i = 0; i < item.Length - 1; i++)
                {

                    string listvalue = "";
                    string namelist = "";
                    string variablevalue = "";
                    string valuename = "";
                    string varisku = "";
                    string productvariable = "";
                    string ProductVariableName = "";
                    string[] temp = item[i].Split('-');
                    for (int j = 0; j < temp.Length; j++)
                    {
                        string[] vl1 = temp[j].Split('*');
                        listvalue += vl1[0].Trim() + "|";
                        string[] vl2 = vl1[1].Split(':');
                        namelist += vl2[0].Trim() + "|";
                        string[] vl3 = vl2[1].Split(',');
                        variablevalue += vl3[0].Trim() + "|";
                        valuename += vl3[1].Trim() + "|";
                        varisku += vl3[2].Trim();
                        productvariable += vl1[0] + ":" + vl3[0] + "|";
                        ProductVariableName += vl2[0] + ": " + vl3[1] + "|";
                    }
                    VariableGetOut v = new VariableGetOut();
                    v.VariableListValue = listvalue;
                    v.VariableNameList = namelist;
                    v.VariableValue = variablevalue;
                    v.VariableValueName = valuename;
                    v.VariableSKUText = varisku;
                    v.ProductVariable = productvariable;
                    v.ProductVariableName = ProductVariableName;
                    vg.Add(v);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(vg);
        }

        [WebMethod]
        public static string CheckSKU(string SKU)
        {
            string ProductSKU = SKU.Trim().ToUpper();

            var productcheck = ProductController.GetBySKU(ProductSKU);
            if (productcheck != null)
            {
                return "null";
            }
            else
            {
                return "ok";

            }

        }

        [WebMethod]
        public static string getParent(int parent)
        {
            List<GetOutCategory> gc = new List<GetOutCategory>();
            if (parent != 0)
            {
                var parentlist = CategoryController.API_GetByParentID(parent);
                if (parentlist != null)
                {

                    for (int i = 0; i < parentlist.Count; i++)
                    {
                        GetOutCategory go = new GetOutCategory();
                        go.ID = parentlist[i].ID;
                        go.CategoryName = parentlist[i].CategoryName;
                        go.CategoryLevel = parentlist[i].CategoryLevel.ToString();
                        gc.Add(go);
                    }
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(gc);
        }

        public class GetOutCategory
        {
            public int ID { get; set; }
            public string CategoryName { get; set; }
            public string CategoryLevel { get; set; }
        }

        public class Variable
        {
            public string VariableName { get; set; }
            public List<VariableValue> Value { get; set; }
        }
        public class VariableValue
        {
            //public int ID { get; set; }
            public string Value { get; set; }
            public string Name { get; set; }
        }

        public class VariableGetOut
        {
            public string VariableListValue { get; set; }
            public string VariableNameList { get; set; }
            public string VariableValue { get; set; }
            public string VariableValueName { get; set; }
            public string VariableSKUText { get; set; }
            public string ProductVariable { get; set; }
            public string ProductVariableName { get; set; }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["usernameLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            if (acc != null)
            {
                if (acc.RoleID == 0 || acc.RoleID == 1 || acc.Username == "nhom_zalo502")
                {
                    int cateID = hdfParentID.Value.ToInt();
                    if (cateID > 0)
                    {
                        string ProductSKU = txtProductSKU.Text.Trim().ToUpper();
                        var check = true;
                        var productcheck = ProductController.GetBySKU(ProductSKU);
                        if (productcheck != null)
                        {
                            check = false;
                        }
                        else
                        {
                            var productvariable = ProductVariableController.GetBySKU(ProductSKU);
                            if (productvariable != null)
                                check = false;
                        }

                        if (check == false)
                        {
                            PJUtils.ShowMessageBoxSwAlert("Mã sản phẩm đã tồn tại, hãy kiểm tra lại", "e", false, Page);
                        }
                        else
                        {
                            string ProductTitle = Regex.Replace(txtProductTitle.Text.Trim(), @"\s*\,\s*|\s*\;\s*", " - ");
                            string ProductContent = pContent.Content.ToString();

                            double ProductStock = 0;
                            int StockStatus = 3;
                            double Regular_Price = Convert.ToDouble(pRegular_Price.Text);
                            double CostOfGood = Convert.ToDouble(pCostOfGood.Text);
                            double Retail_Price = Convert.ToDouble(pRetailPrice.Text);
                            int supplierID = ddlSupplier.SelectedValue.ToInt(0);
                            string supplierName = ddlSupplier.SelectedItem.ToString();
                            string mainColor = ddlColor.SelectedValue.Trim();
                            int a = 1;
                            var preOrder = ddlPreOrder.SelectedValue == "1" ? true : false;
                            double Old_Price = String.IsNullOrEmpty(pOld_Price.Text) ? 0 : Convert.ToDouble(pOld_Price.Text);

                            double MinimumInventoryLevel = pMinimumInventoryLevel.Text.ToInt(0);
                            double MaximumInventoryLevel = pMaximumInventoryLevel.Text.ToInt(0);

                            if (hdfsetStyle.Value == "2")
                            {
                                MinimumInventoryLevel = 0;
                                MaximumInventoryLevel = 0;
                                a = hdfsetStyle.Value.ToInt();
                            }

                            int ShowHomePage = ddlShowHomePage.SelectedValue.ToInt(0);

                            var prodNew = new tbl_Product()
                            {
                                CategoryID = cateID,
                                ProductOldID = 0,
                                ProductTitle = ProductTitle,
                                ProductContent = ProductContent,
                                ProductSKU = ProductSKU,
                                ProductStock = ProductStock,
                                StockStatus = StockStatus,
                                ManageStock = true,
                                Regular_Price = Regular_Price,
                                CostOfGood = CostOfGood,
                                Retail_Price = Retail_Price,
                                ProductImage = String.Empty,
                                ProductType = a,
                                IsHidden = false,
                                CreatedDate = currentDate,
                                CreatedBy = username,
                                SupplierID = supplierID,
                                SupplierName = supplierName,
                                Materials = txtMaterials.Text,
                                MinimumInventoryLevel = MinimumInventoryLevel,
                                MaximumInventoryLevel = MaximumInventoryLevel,
                                ProductStyle = a,
                                ShowHomePage = ShowHomePage,
                                Color = mainColor,
                                PreOrder = preOrder,
                                Old_Price = Old_Price
                            };

                            string kq = ProductController.Insert(prodNew);
                            prodNew.ID = Convert.ToInt32(kq);

                            if (!String.IsNullOrEmpty(hdfTags.Value))
                            {
                                var tagList = JsonConvert.DeserializeObject<List<TagModel>>(hdfTags.Value);

                                if (tagList.Count > 0)
                                {
                                    // Get tag new
                                    var tagNew = TagController.insert(tagList, acc);

                                    var productTag = tagList
                                        .GroupJoin(
                                            tagNew,
                                            t => t.name.ToLower(),
                                            n => n.Name.ToLower(),
                                            (t, n) => new { t, n }
                                        )
                                        .SelectMany(
                                            x => x.n.DefaultIfEmpty(),
                                            (parent, child) => new ProductTag
                                            {
                                                TagID = child != null ? child.ID : parent.t.id,
                                                ProductID = prodNew.ID,
                                                ProductVariableID = 0,
                                                SKU = prodNew.ProductSKU,
                                                CreatedBy = acc.ID,
                                                CreatedDate = currentDate
                                            }
                                        )
                                        .ToList();

                                    ProductTagController.insert(productTag);
                                }
                            }

                            //Phần thêm ảnh đại diện sản phẩm
                            string path = "/uploads/images/";
                            string ProductImage = "";
                            if (ProductThumbnailImage.UploadedFiles.Count > 0)
                            {
                                foreach (UploadedFile f in ProductThumbnailImage.UploadedFiles)
                                {
                                    var o = path + kq + '-' + Slug.ConvertToSlug(Path.GetFileName(f.FileName), isFile: true);
                                    if (!File.Exists(Server.MapPath(o)))
                                    {
                                        f.SaveAs(Server.MapPath(o));
                                        // Thumbnail
                                        Thumbnail.create(Server.MapPath(o), 85, 113);
                                        Thumbnail.create(Server.MapPath(o), 159, 212);
                                        Thumbnail.create(Server.MapPath(o), 240, 320);
                                        Thumbnail.create(Server.MapPath(o), 350, 467);
                                        Thumbnail.create(Server.MapPath(o), 600, 0);
                                    }

                                    ProductImage = Path.GetFileName(Server.MapPath(o));
                                }
                            }
                            string updateImage = ProductController.UpdateImage(kq.ToInt(), ProductImage);

                            //Phần thêm ảnh đại diện sản phẩm sạch không có đóng dấu
                            string ProductImageClean = "";
                            if (ProductThumbnailImageClean.UploadedFiles.Count > 0)
                            {
                                foreach (UploadedFile f in ProductThumbnailImageClean.UploadedFiles)
                                {
                                    var o = path + kq + "-clean-" + Slug.ConvertToSlug(Path.GetFileName(f.FileName), isFile: true);
                                    if (!File.Exists(Server.MapPath(o)))
                                    {
                                        f.SaveAs(Server.MapPath(o));
                                        // Thumbnail
                                        Thumbnail.create(Server.MapPath(o), 85, 113);
                                        Thumbnail.create(Server.MapPath(o), 159, 212);
                                        Thumbnail.create(Server.MapPath(o), 240, 320);
                                        Thumbnail.create(Server.MapPath(o), 350, 467);
                                        Thumbnail.create(Server.MapPath(o), 600, 0);
                                    }

                                    ProductImageClean = Path.GetFileName(Server.MapPath(o));
                                }
                            }
                            string updateImageClean = ProductController.UpdateImageClean(kq.ToInt(), ProductImageClean);

                            //Phần thêm thư viện ảnh sản phẩm
                            string IMG = "";
                            if (hinhDaiDien.UploadedFiles.Count > 0)
                            {
                                foreach (UploadedFile f in hinhDaiDien.UploadedFiles)
                                {
                                    var o = path + kq + '-' + Slug.ConvertToSlug(Path.GetFileName(f.FileName), isFile: true);
                                    if (!File.Exists(Server.MapPath(o)))
                                    {
                                        f.SaveAs(Server.MapPath(o));
                                        // Thumbnail
                                        Thumbnail.create(Server.MapPath(o), 85, 113);
                                        Thumbnail.create(Server.MapPath(o), 159, 212);
                                        Thumbnail.create(Server.MapPath(o), 240, 320);
                                        Thumbnail.create(Server.MapPath(o), 350, 467);
                                        Thumbnail.create(Server.MapPath(o), 600, 0);
                                    }

                                    IMG = Path.GetFileName(Server.MapPath(o));
                                    ProductImageController.Insert(kq.ToInt(), IMG, false, currentDate, username);
                                }
                            }


                            if (kq.ToInt(0) > 0)
                            {
                                int ProductID = kq.ToInt(0);

                                string variable = hdfVariableListInsert.Value;
                                if (!string.IsNullOrEmpty(variable))
                                {
                                    string[] items = variable.Split(',');
                                    for (int i = 0; i < items.Length - 1; i++)
                                    {
                                        string item = items[i];
                                        string[] itemElement = item.Split(';');

                                        string datanameid = itemElement[0];
                                        string[] datavalueid = itemElement[1].Split('|');
                                        string datanametext = itemElement[2];
                                        string datavaluetext = itemElement[3];
                                        string productvariablesku = itemElement[4].Trim().ToUpper();
                                        string regularprice = itemElement[5];
                                        string costofgood = itemElement[6];
                                        string retailprice = itemElement[7];
                                        string[] datanamevalue = itemElement[8].Split('|');
                                        string imageUpload = itemElement[4];
                                        int _MaximumInventoryLevel = itemElement[9].ToInt(0);
                                        int _MinimumInventoryLevel = itemElement[10].ToInt(0);

                                        int stockstatus = itemElement[11].ToInt();

                                        HttpPostedFile postedFile = Request.Files["" + imageUpload + ""];
                                        string image = "";
                                        if (postedFile != null && postedFile.ContentLength > 0)
                                        {
                                            var o = path + kq + '-' + Slug.ConvertToSlug(Path.GetFileName(postedFile.FileName), isFile: true);
                                            if (!File.Exists(Server.MapPath(o)))
                                            {
                                                postedFile.SaveAs(Server.MapPath(o));
                                                // Thumbnail
                                                Thumbnail.create(Server.MapPath(o), 85, 113);
                                                Thumbnail.create(Server.MapPath(o), 159, 212);
                                                Thumbnail.create(Server.MapPath(o), 240, 320);
                                                Thumbnail.create(Server.MapPath(o), 350, 467);
                                                Thumbnail.create(Server.MapPath(o), 600, 0);
                                            }

                                            image = Path.GetFileName(Server.MapPath(o));
                                        }

                                        string kq1 = ProductVariableController.Insert(ProductID, ProductSKU, productvariablesku, 0, stockstatus, Convert.ToDouble(regularprice),
                                            Convert.ToDouble(costofgood), Convert.ToDouble(retailprice), image, true, false, currentDate, username,
                                            supplierID, supplierName, _MinimumInventoryLevel, _MaximumInventoryLevel);

                                        string color = "";
                                        string size = "";
                                        int ProductVariableID = 0;

                                        if (kq1.ToInt(0) > 0)
                                        {
                                            ProductVariableID = kq1.ToInt(0);
                                            color = datavalueid[0];
                                            size = datavalueid[1];
                                            string[] Data = datanametext.Split('|');
                                            string[] DataValue = datavaluetext.Split('|');
                                            for (int k = 0; k < Data.Length - 2; k++)
                                            {
                                                int variablevalueID = datavalueid[k].ToInt();
                                                string variableName = Data[k];
                                                string variableValueName = DataValue[k];
                                                ProductVariableValueController.Insert(ProductVariableID, productvariablesku, variablevalueID,
                                                        variableName, variableValueName, false, currentDate, username);
                                            }
                                        }
                                        ProductVariableController.UpdateColorSize(ProductVariableID, color, size);
                                    }
                                }

                                PJUtils.ShowMessageBoxSwAlertCallFunction("Tạo sản phẩm thành công", "s", true, "redirectTo(" + kq + ", '" + ProductSKU  + "')", Page);
                            }
                        }

                    }

                }
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true, XmlSerializeString = false)]
        public static List<TagModel> GetTags(string tagName)
        {
            if (!String.IsNullOrEmpty(tagName) && tagName.IndexOf(',') >= 0)
            {
                return null;
            }

            var now = DateTime.Now;
            var textInfo = new CultureInfo("vi-VN", false).TextInfo;
            var tags = new List<TagModel>();
            var tagData = TagController.get(tagName);

            if (tagData.Where(x => x.name == textInfo.ToLower(tagName)).Count() > 0)
            {
                tags.AddRange(tagData);
            }
            else
            {
                tags.Add(new TagModel()
                {
                    id = 0,
                    name = textInfo.ToLower(tagName),
                    slug = String.Format("tag-new-{0:yyyyMMddhhmmss}", now)
                });
                tags.AddRange(tagData);
            }

            return tags;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true, XmlSerializeString = false)]
        public static List<TagModel> GetTagList(string tagName)
        {
            if (!String.IsNullOrEmpty(tagName) && tagName.IndexOf(',') >= 0)
            {
                return null;
            }

            var now = DateTime.Now;
            var textInfo = new CultureInfo("vi-VN", false).TextInfo;
            var tags = new List<TagModel>();
            var tagData = TagController.get(tagName);

            tags.AddRange(tagData);

            return tags;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true, XmlSerializeString = false)]
        public static List<TagModel> GetTagListByNameList([FromUri]string[] tagNameList)
        {
            if (tagNameList == null || tagNameList.Length == 0)
                return null;

            return TagController.get(tagNameList.ToList());
        }
    }
}