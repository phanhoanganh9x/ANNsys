using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.Services;
using static IM_PJ.Controllers.ProductController;
using IM_PJ.Utils;
using System.Drawing;
using System.Web.Hosting;
using System.IO;

namespace IM_PJ
{
    public partial class tat_ca_san_pham : System.Web.UI.Page
    {
        private static tbl_Account acc;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem"].Value;
                    acc = AccountController.GetByUsername(username);
                    if (acc != null)
                    {
                        LoadData();
                        LoadCategory();
                        LoadTag();
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
            }
        }
        public void LoadTag()
        {
            var tag = TagController.getAll();
            ddlTag.Items.Clear();
            ddlTag.Items.Insert(0, new ListItem("Tag", "0"));
            if (tag.Count > 0)
            {
                foreach (var t in tag)
                {
                    ListItem item = new ListItem(t.Name, t.ID.ToString());
                    ddlTag.Items.Add(item);
                }
                ddlTag.DataBind();
            }
        }
        public void LoadCategory()
        {
            var category = CategoryController.GetAllWithIsHidden(false);
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new ListItem("Danh mục", "0"));
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
        public void LoadData()
        {
            if (acc != null)
            {
                if (acc.RoleID == 0 || acc.RoleID == 1 || acc.Username == "nhom_zalo502")
                {
                    ltrAddProduct.Text = "<a href='/tao-san-pham' class='h45-btn btn primary-btn'>Thêm mới</a>";
                }

                DateTime year = new DateTime(2018, 6, 22);

                var config = ConfigController.GetByTop1();
                if (config.ViewAllReports == 0)
                {
                    year = DateTime.Now.AddMonths(-2);
                }

                DateTime DateConfig = year;

                DateTime fromDate = DateConfig;
                DateTime toDate = DateTime.Now;

                if (!String.IsNullOrEmpty(Request.QueryString["fromdate"]))
                {
                    fromDate = Convert.ToDateTime(Request.QueryString["fromdate"]);
                }

                if (!String.IsNullOrEmpty(Request.QueryString["todate"]))
                {
                    toDate = Convert.ToDateTime(Request.QueryString["todate"]).AddDays(1).AddMinutes(-1);
                }

                rFromDate.SelectedDate = fromDate;
                rFromDate.MinDate = DateConfig;
                rFromDate.MaxDate = DateTime.Now;

                rToDate.SelectedDate = toDate;
                rToDate.MinDate = DateConfig;
                rToDate.MaxDate = DateTime.Now;

                string TextSearch = String.Empty;
                int CategoryID = 0;
                int StockStatus = 0;
                string ShowHomePage = String.Empty;
                string WebPublish = String.Empty;
                string strColor = String.Empty;
                string strSize = String.Empty;
                int Page = 1;
                // add filter quantity
                string Quantity = String.Empty;
                int QuantityFrom = 0;
                int QuantityTo = 0;
                // add filter preOrder
                var preOrder = String.Empty;
                string orderBy = String.Empty;
                int tag = 0;
                // add filter warehouse
                var warehouse = 0;

                if (Request.QueryString["textsearch"] != null)
                    TextSearch = Request.QueryString["textsearch"].Trim();
                if (Request.QueryString["stockstatus"] != null)
                    StockStatus = Request.QueryString["stockstatus"].ToInt();
                if (Request.QueryString["categoryid"] != null)
                    CategoryID = Request.QueryString["categoryid"].ToInt();
                if (Request.QueryString["showhomepage"] != null)
                    ShowHomePage = Request.QueryString["showhomepage"];
                if (Request.QueryString["webpublish"] != null)
                    WebPublish = Request.QueryString["webpublish"];
                // Add filter valiable value
                if (Request.QueryString["color"] != null)
                    strColor = Request.QueryString["color"].Trim();
                if (Request.QueryString["size"] != null)
                    strSize = Request.QueryString["size"].Trim();
                if (Request.QueryString["Page"] != null)
                {
                    Page = Request.QueryString["Page"].ToInt();
                }
                // add filter quantity
                if (Request.QueryString["quantityfilter"] != null)
                {
                    Quantity = Request.QueryString["quantityfilter"];

                    if (Quantity == "greaterthan")
                    {
                        QuantityFrom = Request.QueryString["quantity"].ToInt();
                    }
                    else if (Quantity == "lessthan")
                    {
                        QuantityTo = Request.QueryString["quantity"].ToInt();
                    }
                    else if (Quantity == "between")
                    {
                        QuantityFrom = Request.QueryString["quantitymin"].ToInt();
                        QuantityTo = Request.QueryString["quantitymax"].ToInt();
                    }
                }
                if (Request.QueryString["orderby"] != null)
                {
                    orderBy = Request.QueryString["orderby"];
                }
                if (Request.QueryString["tag"] != null)
                {
                    tag = Request.QueryString["tag"].ToInt();
                }

                txtSearchProduct.Text = TextSearch;
                ddlCategory.SelectedValue = CategoryID.ToString();
                ddlStockStatus.SelectedValue = StockStatus.ToString();
                ddlShowHomePage.SelectedValue = ShowHomePage.ToString();
                ddlWebPublish.SelectedValue = WebPublish.ToString();
                // add filter quantity
                ddlQuantityFilter.SelectedValue = Quantity.ToString();
                if (Quantity == "greaterthan")
                {
                    txtQuantity.Text = QuantityFrom.ToString();
                    txtQuantityMin.Text = "0";
                    txtQuantityMax.Text = "0";
                }
                else if (Quantity == "lessthan")
                {
                    txtQuantity.Text = QuantityTo.ToString();
                    txtQuantityMin.Text = "0";
                    txtQuantityMax.Text = "0";
                }
                else if (Quantity == "between")
                {
                    txtQuantity.Text = "0";
                    txtQuantityMin.Text = QuantityFrom.ToString();
                    txtQuantityMax.Text = QuantityTo.ToString();
                }

                // Add filter valiable value
                ddlColor.SelectedValue = strColor;
                ddlSize.SelectedValue = strSize;
                // Add filter preOrder
                if (!String.IsNullOrEmpty(Request.QueryString["preOrder"]))
                    preOrder = Request.QueryString["preOrder"];
                ddlPreOrder.SelectedValue = preOrder;

                ddlTag.SelectedValue = tag.ToString();
                ddlOrderBy.SelectedValue = orderBy;
                // Add filter warehouse
                if (!String.IsNullOrEmpty(Request.QueryString["warehouse"]))
                    warehouse = Request.QueryString["warehouse"].ToInt();

                // Add Warehouse
                if (!String.IsNullOrEmpty(Request.QueryString["warehouse"]))
                {
                    warehouse = Request.QueryString["warehouse"].ToInt();
                    ddlWarehouse.SelectedValue = Request.QueryString["warehouse"];
                }

                // Create order filter
                var filter = new ProductFilterModel()
                {
                    category = CategoryID,
                    search = TextSearch,
                    color = strColor,
                    size = strSize,
                    stockStatus = StockStatus,
                    quantity = Quantity,
                    quantityFrom = QuantityFrom,
                    quantityTo = QuantityTo,
                    fromDate = fromDate,
                    toDate = toDate,
                    showHomePage = ShowHomePage,
                    webPublish = WebPublish,
                    preOrder = preOrder,
                    tag = tag,
                    orderBy = orderBy,
                    warehouse = warehouse
                };
                // Create pagination
                var page = new PaginationMetadataModel()
                {
                    currentPage = Page
                };

                List<ProductSQL> a = new List<ProductSQL>();
                a = ProductController.GetAllSql(filter, ref page);

                pagingall(a, page);

                ltrNumberOfProduct.Text = page.totalCount.ToString();

                if (acc.RoleID != 0)
                {
                    ddlShowHomePage.Enabled = false;
                    ddlWebPublish.Enabled = false;
                }
            }
        }
        [WebMethod]
        public static string updateHotProduct(int productID)
        {
            var product = ProductController.GetByID(productID);
            if (product == null)
            {
                return "productNotfound";
            }

            var hotTag = new ProductTag() {
                TagID = 8,
                ProductID = product.ID,
                ProductVariableID = 0,
                SKU = product.ProductSKU,
                CreatedBy = acc.ID,
                CreatedDate = DateTime.Now
            };
            var result = ProductTagController.checkAndUnCheck(hotTag);

            if (result == CheckTagStatus.@checked)
            {
                return "hot";
            }
            else
            {
                return "noHot";
            }
        }
        [WebMethod]
        public static string getAllProductImage1MB(string sku)
        {
            List<string> result = new List<string>();

            string rootPath = HostingEnvironment.ApplicationPhysicalPath;
            string uploadsImagesPath = rootPath + "/uploads/images/";
            var product = ProductController.GetBySKU(sku);

            if (product != null)
            {
                List<string> images = new List<string>();

                // lấy ảnh đại diện
                string imgAdd = product.ProductImage;
                if (File.Exists(uploadsImagesPath + imgAdd))
                {
                    images.Add(imgAdd);
                }

                // lấy ảnh sản phẩm từ thư viện
                var imageProduct = ProductImageController.GetByProductID(product.ID);
                if (imageProduct != null)
                {
                    foreach (var img in imageProduct)
                    {
                        imgAdd = img.ProductImage;
                        if (File.Exists(uploadsImagesPath + imgAdd))
                        {
                            images.Add(imgAdd);
                        }
                    }
                }

                // lấy ảnh sản phẩm từ biến thể
                var variable = ProductVariableController.GetByParentSKU(product.ProductSKU);
                if (variable != null)
                {
                    foreach (var v in variable)
                    {
                        if (!String.IsNullOrEmpty(v.Image))
                        {
                            imgAdd = v.Image;
                            if (File.Exists(uploadsImagesPath + imgAdd))
                            {
                                images.Add(imgAdd);
                            }
                        }
                    }
                }

                // lọc hình trùng lặp
                images = images.Distinct().ToList();

                if (images.Count() > 0)
                {
                    for (int i = 0; i < images.Count - 1; i++)
                    {
                        for (int y = i + 1; y < images.Count; y++)
                        {
                            string img1 = Thumbnail.getURL(images[i], Thumbnail.Size.Micro);
                            string img2 = Thumbnail.getURL(images[y], Thumbnail.Size.Micro);

                            // so sánh 2 hình và lọc hình trùng lặp
                            Bitmap bmp1 = (Bitmap)Bitmap.FromFile(rootPath + img1);
                            Bitmap bmp2 = (Bitmap)Bitmap.FromFile(rootPath + img2);
                            if (CompareBitmapsLazy(bmp1, bmp2))
                            {
                                images.RemoveAt(y);
                                y--;
                            }
                        }
                    }
                }

                // lấy hình dưới 1MB
                for (int i = 0; i < images.Count; i++)
                {
                    string item = uploadsImagesPath + images[i];
                    var size = new System.IO.FileInfo(item).Length;
                    if (size > 1000000)
                    {
                        // kiểm tra size hình rộng 600px
                        item = uploadsImagesPath + "600/" + images[i];
                        if (File.Exists(item))
                        {
                            size = new System.IO.FileInfo(item).Length;
                            if (size > 1000000)
                            {
                                result.Add(Thumbnail.getURL(images[i], Thumbnail.Size.Large));
                            }
                            else
                            {
                                result.Add(Thumbnail.getURL(images[i], Thumbnail.Size.XLarge));
                            }
                        }
                        else
                        {
                            result.Add(Thumbnail.getURL(images[i], Thumbnail.Size.Large));
                        }
                    }
                    else
                    {
                        result.Add(Thumbnail.getURL(images[i], Thumbnail.Size.Source));
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(result.ToList());
        }
        [WebMethod]
        public static string getAllProductImage(string sku)
        {
            List<string> result = new List<string>();

            string rootPath = HostingEnvironment.ApplicationPhysicalPath;
            string uploadsImagesPath = rootPath + "/uploads/images/";
            var product = ProductController.GetBySKU(sku);

            if (product != null)
            {
                List<string> images = new List<string>();

                // lấy ảnh đại diện
                string imgAdd = product.ProductImage;
                if (File.Exists(uploadsImagesPath + imgAdd))
                {
                    images.Add(imgAdd);
                }

                // lấy ảnh sản phẩm từ thư viện
                var imageProduct = ProductImageController.GetByProductID(product.ID);
                if (imageProduct != null)
                {
                    foreach (var img in imageProduct)
                    {
                        imgAdd = img.ProductImage;
                        if (File.Exists(uploadsImagesPath + imgAdd))
                        {
                            images.Add(imgAdd);
                        }
                    }
                }

                // lấy ảnh sản phẩm từ biến thể
                var variable = ProductVariableController.GetByParentSKU(product.ProductSKU);
                if (variable != null)
                {
                    foreach (var v in variable)
                    {
                        if (!String.IsNullOrEmpty(v.Image))
                        {
                            imgAdd = v.Image;
                            if (File.Exists(uploadsImagesPath + imgAdd))
                            {
                                images.Add(imgAdd);
                            }
                        }
                    }
                }

                // lọc hình trùng lặp
                images = images.Distinct().ToList();

                if (images.Count() > 0)
                {
                    for (int i = 0; i < images.Count - 1; i++)
                    {
                        for (int y = i + 1; y < images.Count; y++)
                        {
                            string img1 = Thumbnail.getURL(images[i], Thumbnail.Size.Micro);
                            string img2 = Thumbnail.getURL(images[y], Thumbnail.Size.Micro);

                            // so sánh 2 hình và lọc hình trùng lặp
                            Bitmap bmp1 = (Bitmap)Bitmap.FromFile(rootPath + img1);
                            Bitmap bmp2 = (Bitmap)Bitmap.FromFile(rootPath + img2);
                            if (CompareBitmapsLazy(bmp1, bmp2))
                            {
                                images.RemoveAt(y);
                                y--;
                            }
                        }
                    }
                }

                // lấy hình gốc
                for (int i = 0; i < images.Count; i++)
                {
                    result.Add(Thumbnail.getURL(images[i], Thumbnail.Size.Source));
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(result.ToList());
        }
        public static bool CompareBitmapsLazy(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1 == null || bmp2 == null)
                return false;
            if (object.Equals(bmp1, bmp2))
                return true;
            if (!bmp1.Size.Equals(bmp2.Size) || !bmp1.PixelFormat.Equals(bmp2.PixelFormat))
                return false;

            //Compare bitmaps using GetPixel method
            for (int column = 0; column < bmp1.Width; column++)
            {
                for (int row = 0; row < bmp1.Height; row++)
                {
                    if (!bmp1.GetPixel(column, row).Equals(bmp2.GetPixel(column, row)))
                        return false;
                }
            }

            return true;
        }
        [WebMethod]
        public static string updateShowHomePage(int id, int value)
        {
            // Kiểm tra hành động đang cho ẩn hay hiện
            if (value == 0)
            {
                // Đang cho ẩn thì thực hiện luôn (không cần kiểm tra sản phẩm)
                string update = ProductController.updateShowHomePage(id, value);

                if (update != null)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
            else
            {
                // Đang cho hiện thì kiểm tra sản phẩm
                var product = ProductController.GetByID(id);

                if (String.IsNullOrEmpty(product.ProductImage) || String.IsNullOrEmpty(product.ProductImageClean))
                {
                    return "nocleanimage";
                }
                else
                {
                    // so sánh 2 hình ảnh đại diện
                    var rootPath = HostingEnvironment.ApplicationPhysicalPath;
                    string img1 = Thumbnail.getURL(product.ProductImage, Thumbnail.Size.Source);
                    string img2 = Thumbnail.getURL(product.ProductImageClean, Thumbnail.Size.Source);

                    if (File.Exists(rootPath + img1) && File.Exists(rootPath + img2))
                    {
                        Bitmap bmp1 = (Bitmap)Bitmap.FromFile(rootPath + img1);
                        Bitmap bmp2 = (Bitmap)Bitmap.FromFile(rootPath + img2);

                        if (CompareBitmapsLazy(bmp1, bmp2))
                        {
                            // Nếu giống nhau
                            return "sameimage";
                        }
                        else
                        {
                            // Nếu khác nhau thì tiến hành update
                            string update = ProductController.updateShowHomePage(id, value);

                            if (update != null)
                            {
                                return "true";
                            }
                            else
                            {
                                return "false";
                            }
                        }
                    }
                    else
                    {
                        return "false";
                    }

                    
                }
            }
            
        }
        [WebMethod]
        public static string updateWebPublish(int id, bool value)
        {
            string update = ProductController.updateWebPublish(id, value);
            if (update != null)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }
        [WebMethod]
        public static string upTopWebUpdate(int id)
        {
            string update = ProductController.upTopWebUpdate(id);
            if (update != null)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }
        [WebMethod]
        public static string deleteProduct(int id)
        {
            var product = ProductController.GetByID(id);

            if(product != null)
            {
                // Kiểm tra sản phẩm này đã bán chưa?
                if(product.ProductStyle == 1)
                {
                    var order = OrderDetailController.GetByProductID(product.ID);
                    if (order.Count() > 0)
                    {
                        return "orderfound";
                    }
                }
                else
                {
                    // Kiểm tra biến thể của sản phẩm này đã bán chưa?
                    var variables = ProductVariableController.GetAllByParentID(product.ID);

                    foreach (var item in variables)
                    {
                        var order = OrderDetailController.GetByProductVariableID(item.ID);
                        if (order.Count() > 0)
                        {
                            return "orderfound";
                        }
                    }
                }

                // Xóa biến thể
                if (product.ProductStyle == 2)
                {
                    // Xóa giá trị biến thể
                    var variables = ProductVariableController.GetAllByParentID(product.ID);
                    foreach(var item in variables)
                    {
                        var removeVariableValue = ProductVariableValueController.DeleteByProductVariableID(item.ID);
                    }

                    // Xóa biến thể
                    var removeVariable = ProductVariableController.deleteVariable(product.ID);
                }

                // Xóa hình sản phẩm trong database
                var removeProductImage = ProductImageController.deleteAll(product.ID);

                // Xóa nhập kho
                var removeStock = StockManagerController.deleteAll(product.ID);

                // Xóa sản phẩm cha
                string delete = ProductController.deleteProduct(product.ID);
                if (delete != null)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }

            return "false";
        }
        [WebMethod]
        public static string updateProductSKU(string oldSKU, string newSKU)
        {
            oldSKU = oldSKU.Trim().ToUpper();
            newSKU = newSKU.Trim().ToUpper();

            var product = ProductController.GetBySKU(oldSKU);

            if (product != null)
            {
                // Kiểm tra sản phẩm này đã bán chưa?
                if (product.ProductStyle == 1)
                {
                    var order = OrderDetailController.GetByProductID(product.ID);
                    if (order.Count() > 0)
                    {
                        return "orderfound";
                    }
                }
                else
                {
                    // Kiểm tra biến thể của sản phẩm này đã bán hoặc nhập kho chưa?
                    var variables = ProductVariableController.GetAllByParentID(product.ID);

                    foreach (var item in variables)
                    {
                        var order = OrderDetailController.GetByProductVariableID(item.ID);
                        if (order.Count() > 0)
                        {
                            return "orderfound";
                        }
                    }
                }

                // Kiểm tra sản phẩm đã nhập kho chưa?

                var stock = StockManagerController.GetByParentID(product.ID);

                if (stock.Count() > 0)
                {
                    return "stockfound";
                }

                // Kiểm tra SKU mới có tồn tại chưa?
                var checkNewSKU = ProductController.GetBySKU(newSKU);

                if(checkNewSKU != null)
                {
                    return "newskuexist";
                }

                // cập nhật SKU biến thể
                if (product.ProductStyle == 2)
                {
                    // Cập nhật sku trong giá trị biến thể
                    var variables = ProductVariableController.GetAllByParentID(product.ID);
                    foreach (var item in variables)
                    {
                        ProductVariableValueController.updateSKU(item.ID, product.ProductSKU, newSKU);
                    }

                    // cập nhật SKU biến thể gốc
                    ProductVariableController.updateSKU(product.ID, newSKU);
                }

                // Cập nhật SKU sản phẩm cha
                string update = ProductController.updateSKU(product.ID, newSKU);

                if (update != null)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }

            return "false";
        }
        [WebMethod]
        public static string copyProductInfo(int id)
        {
            var product = ProductController.GetByID(id);
            StringBuilder html = new StringBuilder();
            if (product != null)
            {
                html.AppendLine("<p>" + product.ProductSKU + " - Sỉ " + product.ProductTitle + "</p>\r\n");
                html.AppendLine("<p>📌 Giá sỉ: " + (product.Regular_Price).ToString() + "</p>\r\n");
                html.AppendLine("<p>📌 Giá lẻ: " + (product.Retail_Price).ToString() + "</p>\r\n");

                if (!string.IsNullOrEmpty(product.Materials))
                {
                    html.AppendLine("<p>🔖 " + (product.CategoryID == 44 ? "" : "Chất liệu: ")  + product.Materials + "</p>\r\n");
                }

                if (!string.IsNullOrEmpty(product.ProductContent))
                {
                    string content = Regex.Replace(product.ProductContent, @"<img\s[^>]*>(?:\s*?</img>)?", "").ToString();
                    html.AppendLine("<p>🔖 " + content + "</p>\r\n");
                }

                // liệt kê thuộc tính sản phẩm

                List<ProductVariable> variableTemp = new List<ProductVariable>();

                List<tbl_ProductVariable> v = new List<tbl_ProductVariable>();

                v = ProductVariableController.SearchProductID(id, "");

                string Variable = "";
                if (v.Count > 0)
                {
                    for (int i = 0; i < v.Count; i++)
                    {
                        var item = v[i];
                        var value = ProductVariableValueController.GetByProductVariableIDSortByName(item.ID);
                        if (value != null)
                        {
                            for (int j = 0; j < value.Count; j++)
                            {
                                variableTemp.Add(new ProductVariable() { VariableName = value[j].VariableName, VariableValue = value[j].VariableValue });

                            }
                        }
                    }
                    var vari = variableTemp
                                .GroupBy(x => new { x.VariableName, x.VariableValue })
                                .Select(x => new { VariableName = x.Key.VariableName, VariableValue = x.Key.VariableValue })
                                .OrderBy(x => x.VariableName)
                                .ToList();

                    string stringVariable = vari[0].VariableName;

                    Variable = "<p><strong>📚 " + vari[0].VariableName + "</strong>: ";

                    int count = 0;

                    for (int y = 0; y < vari.Count; y++)
                    {
                        if (stringVariable == vari[y].VariableName)
                        {
                            if (vari[y].VariableValue.IndexOf("Mẫu") >= 0)
                            {
                                count = count + 1;

                                if (y == (vari.Count - 1))
                                {
                                    Variable += count.ToString() + " mẫu khác nhau";
                                }
                            }
                            else
                            {
                                Variable += vari[y].VariableValue + "; ";
                            }
                        }
                        else
                        {
                            if (count > 0)
                            {
                                Variable += count.ToString() + " mẫu khác nhau";
                            }
                            Variable += "</p>\r\n";
                            Variable += "<p><strong>📐 " + vari[y].VariableName + "</strong>: " + vari[y].VariableValue + "; ";
                            stringVariable = vari[y].VariableName;
                        }
                    }
                    
                    html.AppendLine(Variable);
                }

                if (id % 3 == 0)
                {
                    // thông tin liên hệ
                    string categoryName = "";
                    var category = CategoryController.GetByID(product.CategoryID.Value);
                    if (category != null)
                    {
                        categoryName = " " + category.CategoryName.ToUpper();
                    }
                    html.AppendLine("\r\n");
                    html.AppendLine("<p>--------------------------</p>\r\n");
                    html.AppendLine("<p>⚡⚡ KHO SỈ" + categoryName + " ANN ⚡⚡</p>\r\n");
                    html.AppendLine("<p>🏭 68 Đường C12, P.13, Tân Bình, TP.HCM</p>\r\n");
                    html.AppendLine("<p>⭐ Web: https://ann.com.vn </p>\r\n");
                }
                
            }

            return html.ToString();
        }
        public class ProductVariable
        {
            public string VariableName { get; set; }
            public string VariableValue { get; set; }
        }
        #region Paging
        public void pagingall(List<ProductSQL> acs, PaginationMetadataModel page)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<thead>");
            html.AppendLine("<tr>");
            html.AppendLine("    <th class='image-column'>Ảnh</th>");
            html.AppendLine("    <th class='name-column'>Sản phẩm</th>");
            html.AppendLine("    <th class='sku-column'>Mã</th>");
            html.AppendLine("    <th class='wholesale-price-column'>Giá sỉ</th>");
            if (acc.RoleID == 0)
            {
                html.AppendLine("    <th class='cost-price-column'>Giá vốn</th> ");
            }
            html.AppendLine("    <th class='retail-price-column'>Giá lẻ</th>");
            html.AppendLine("    <th class='stock-column'>SL</th>");
            html.AppendLine("    <th class='stock-status-column'>Kho</th>");
            html.AppendLine("    <th class='category-column'>Danh mục</th>");
            html.AppendLine("    <th class='date-column'>Ngày tạo</th>");
            if (acc.RoleID == 0 || acc.Username == "nhom_zalo502")
            {
                html.AppendLine("    <th class='show-homepage-column'>Trang chủ</th>");
                html.AppendLine("    <th class='show-webpublish-column'>Xem hàng</th>");
            }
            html.AppendLine("    <th class='action-column'></th>");
            html.AppendLine("</tr>");
            html.AppendLine("</thead>");

            html.AppendLine("<tbody>");
            if (acs.Count > 0)
            {
                PageCount = page.totalPages;
                Int32 Page = page.currentPage;

                foreach (var item in acs)
                {
                    html.AppendLine("<tr data-id='" + item.ID + "' data-home-page='" + item.ShowHomePage.ToString() + "' data-stock-quantity='" + item.TotalProductInstockQuantityLeft + "'>");
                    html.AppendLine("<td>");
                    html.AppendLine("   <a target='_blank' href='/xem-san-pham?id=" + item.ID + "'><img src='" + Thumbnail.getURL(item.ProductImage, Thumbnail.Size.Normal) + "'></a>");
                    html.AppendLine("   <a href='javascript:;' onclick='copyProductInfo(" + item.ID + ")' class='btn download-btn h45-btn'><i class='fa fa-files-o'></i> Copy</a>");
                    html.AppendLine("</td>");
                    html.AppendLine("   <td>");
                    if (!String.IsNullOrEmpty(item.Tags) && item.Tags.Contains("hot"))
                    {
                        html.AppendLine("       <a class='update-hot-product' href='javascript:;' data-id='" + item.ID + "' onclick='updateHotProduct($(this))'><i class='fa fa-star' aria-hidden='true'></i></a>");
                    }
                    else
                    {
                        html.AppendLine("       <a class='update-hot-product' href='javascript:;' data-id='" + item.ID + "' onclick='updateHotProduct($(this))'><i class='fa fa-star-o' aria-hidden='true'></i></a>");
                    }
                    html.AppendLine("       <a target='_blank' class='customer-name-link' href='/xem-san-pham?id=" + item.ID + "'>" + (item.OldPrice > 0 ? "<span class='sale-icon'>SALE</span> " : "") + item.ProductTitle + "</a>");
                    html.AppendLine("       <p class='p-paterials'><strong>Chất liệu:</strong> " + item.Materials + "<p>");

                    if (!String.IsNullOrEmpty(item.Tags))
                    {
                        var tagList = item.Tags.Split(',').Select(x => x.Trim()).ToList();

                        foreach (var tag in tagList)
                        {
                            if (tag != "hot")
                            {
                                html.AppendLine(String.Format("       <span class='tag-blue'>{0}</span>", tag.ToLower()));
                            }
                        }
                    }
                    html.AppendLine("   </td>");

                    html.AppendLine("   <td data-title='Mã'>");
                    html.AppendLine("   <span class='customer-name-link'>" + item.ProductSKU + "</span>");
                    if (acc.RoleID == 0)
                    {
                        html.AppendLine("   <br>(" + item.ID + ")");
                    }
                    html.AppendLine("   </td>");
                    html.AppendLine("   <td data-title='Giá sỉ'>" + string.Format("{0:N0}", item.RegularPrice) + (item.OldPrice > 0 ? " <span class='old-price'>" + string.Format("{0:N0}", item.OldPrice) + "</span>" : "") + "</td>");
                    if (acc.RoleID == 0)
                    {
                        html.AppendLine("   <td data-title='Giá vốn'>" + string.Format("{0:N0}", item.CostOfGood) + "</td>");
                    }
                    html.AppendLine("   <td data-title='Giá lẻ'>" + string.Format("{0:N0}", item.RetailPrice) + "</td>");

                    var strStock2Quantity = String.Empty;
                    if (item.HasStock2)
                        strStock2Quantity = String.Format("<br>(Kho 2: {0:N0})", item.Stock2Quantity);
                    html.AppendLine(String.Format("   <td data-title='Số lượng'><span><a target='_blank' title='Xem thống kê sản phẩm' href='/thong-ke-san-pham?SKU={0}'>{1:N0}</a></span>{2}</td>", item.ProductSKU, item.TotalProductInstockQuantityLeft, strStock2Quantity));
                    html.AppendLine("   <td data-title='Kho'>" + item.ProductInstockStatus + "</td>");
                    html.AppendLine("   <td data-title='Danh mục'>" + item.CategoryName + "</td>");
                    string date = string.Format("<strong>{0:dd/MM/yyyy}</strong><br>{0:HH:mm}", item.CreatedDate);
                    html.AppendLine("   <td data-title='Ngày tạo'>" + date + "</td>");
                    if (acc.RoleID == 0 || acc.RoleID == 1 || acc.Username == "nhom_zalo502")
                    {
                        if (item.ShowHomePage == 0)
                        {
                            html.AppendLine("   <td data-title='Trang chủ'><span id='showHomePage_" + item.ID + "'><a href='javascript:;' data-product-id='" + item.ID + "' data-update='1' class='bg-black bg-button' onclick='updateShowHomePage($(this))'>Đang ẩn</a></span></td>");
                        }
                        else
                        {
                            html.AppendLine("   <td data-title='Trang chủ'><span id='showHomePage_" + item.ID + "'><a href='javascript:;' data-product-id='" + item.ID + "' data-update='0' class='bg-green bg-button' onclick='updateShowHomePage($(this))'>Đang hiện</a></span></td>");
                        }

                        if (item.WebPublish == false)
                        {
                            html.AppendLine("   <td data-title='Trang xem hàng'><span id='showWebPublish_" + item.ID + "'><a href='javascript:;' data-product-id='" + item.ID + "' data-update='true' class='bg-black bg-button' onclick='updateShowWebPublish($(this))'>Đang ẩn</a></span></td>");
                        }
                        else
                        {
                            html.AppendLine("   <td data-title='Trang xem hàng'><span id='showWebPublish_" + item.ID + "'><a href='javascript:;' data-product-id='" + item.ID + "' data-update='false' class='bg-red bg-button' onclick='updateShowWebPublish($(this))'>Đang hiện</a></span></td>");
                        }
                    }

                    html.AppendLine("   <td class='update-button'>");
                    html.AppendLine("       <a href='javascript:;' title='Download tất cả hình sản phẩm này' class='btn primary-btn h45-btn' onclick='getAllProductImage(`" + item.ProductSKU + "`);'><i class='fa fa-file-image-o' aria-hidden='true'></i></a>");
                    html.AppendLine("       <a target='_blank' href='https://www.facebook.com/search/posts/?q=" + item.ProductSKU + "&filters_rp_author=%7B%22name%22%3A%22author%22%2C%22args%22%3A%22100012594165130%22%7D&filters_rp_chrono_sort=%7B%22name%22%3A%22chronosort%22%2C%22args%22%3A%22%22%7D' title='Tìm trên facebook' class='btn primary-btn btn-black h45-btn'><i class='fa fa-facebook-official' aria-hidden='true'></i></a>");

                    if (acc.RoleID == 0 || acc.Username == "nhom_zalo502")
                    {
                        html.AppendLine("       <a href='javascript:;' title='Up sản phẩm lên đầu trang' class='btn primary-btn btn-violet h45-btn' data-id='" + item.ID + "' onclick='upTopWebUpdate($(this));'><i class='fa fa-arrow-up' aria-hidden='true'></i></a>");
                        html.AppendLine("       <a target='_blank' href='/thong-tin-san-pham?id=" + item.ID + "' title='Sửa sản phẩm' class='btn btn-blue primary-btn h45-btn'><i class='fa fa-pencil-square-o' aria-hidden='true'></i></a>");
                        html.AppendLine("       <a href='javascript:;' title='Đồng bộ sản phẩm' class='up-product-" + item.ID + " btn btn-green primary-btn h45-btn " + (item.ShowHomePage == 1 ? "" : "hide") + "' onclick='showProductSyncModal(`" + item.ProductSKU + "`, " + item.ID + ", " + item.CategoryID + ");'><i class='fa fa-refresh' aria-hidden='true'></i></a>");
                        if (item.TotalProductInstockQuantityLeft > 0)
                        {
                            html.AppendLine("       <a href='javascript:;' title='Xả hết kho' class='liquidation-product-" + item.ID + " btn primary-btn btn-red h45-btn' onclick='liquidateProduct(" + item.CategoryID + ", " + item.ID + ", `" + item.ProductSKU + "`);'><i class='glyphicon glyphicon-trash' aria-hidden='true'></i></a>");
                        }
                        else if (item.Liquidated)
                        {
                            html.AppendLine("       <a href='javascript:;' title='Phục hồi xả kho' class='recover-liquidation-product-" + item.ID + " btn primary-btn btn-green h45-btn' onclick='recoverLiquidatedProduct(" + item.CategoryID + ", " + item.ID + ", `" + item.ProductSKU + "`);'><i class='glyphicon glyphicon-repeat' aria-hidden='true'></i></a>");
                        }
                    }

                    html.AppendLine("  </td>");
                    html.AppendLine("</tr>");
                }

            }
            else
            {
                html.AppendLine("<tr><td colspan='13'>Không tìm thấy sản phẩm...</td></tr>");
            }
            html.AppendLine("</tbody>");

            ltrList.Text = html.ToString();
        }
        public static Int32 GetIntFromQueryString(String key)
        {
            Int32 returnValue = -1;
            String queryStringValue = HttpContext.Current.Request.QueryString[key];
            try
            {
                if (queryStringValue == null)
                    return returnValue;
                if (queryStringValue.IndexOf("#") > 0)
                    queryStringValue = queryStringValue.Substring(0, queryStringValue.IndexOf("#"));
                returnValue = Convert.ToInt32(queryStringValue);
            }
            catch
            { }
            return returnValue;
        }
        private int PageCount;
        protected void DisplayHtmlStringPaging1()
        {

            Int32 CurrentPage = Convert.ToInt32(Request.QueryString["Page"]);
            if (CurrentPage == -1) CurrentPage = 1;
            string[] strText = new string[4] { "Trang đầu", "Trang cuối", "Trang sau", "Trang trước" };
            if (PageCount > 1)
                Response.Write(GetHtmlPagingAdvanced(6, CurrentPage, PageCount, Context.Request.RawUrl, strText));

        }
        private static string GetPageUrl(int currentPage, string pageUrl)
        {
            pageUrl = Regex.Replace(pageUrl, "(\\?|\\&)*" + "Page=" + currentPage, "");
            if (pageUrl.IndexOf("?") > 0)
            {
                pageUrl += "&Page={0}";
            }
            else
            {
                pageUrl += "?Page={0}";
            }
            return pageUrl;
        }
        public static string GetHtmlPagingAdvanced(int pagesToOutput, int currentPage, int pageCount, string currentPageUrl, string[] strText)
        {
            //Nếu Số trang hiển thị là số lẻ thì tăng thêm 1 thành chẵn
            if (pagesToOutput % 2 != 0)
            {
                pagesToOutput++;
            }

            //Một nửa số trang để đầu ra, đây là số lượng hai bên.
            int pagesToOutputHalfed = pagesToOutput / 2;

            //Url của trang
            string pageUrl = GetPageUrl(currentPage, currentPageUrl);


            //Trang đầu tiên
            int startPageNumbersFrom = currentPage - pagesToOutputHalfed; ;

            //Trang cuối cùng
            int stopPageNumbersAt = currentPage + pagesToOutputHalfed; ;

            StringBuilder output = new StringBuilder();

            //Nối chuỗi phân trang
            //output.Append("<div class=\"paging\">");
            output.Append("<ul>");

            //Link First(Trang đầu) và Previous(Trang trước)
            if (currentPage > 1)
            {
                output.Append("<li><a title=\"" + strText[0] + "\" href=\"" + string.Format(pageUrl, 1) + "\">Trang đầu</a></li>");
                output.Append("<li><a title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\">Trang trước</a></li>");
                //output.Append("<li class=\"UnselectedPrev\" ><a title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\"><i class=\"fa fa-angle-left\"></i></a></li>");

                //output.Append("<span class=\"Unselect_prev\"><a href=\"" + string.Format(pageUrl, currentPage - 1) + "\"></a></span>");
            }

            /******************Xác định startPageNumbersFrom & stopPageNumbersAt**********************/
            if (startPageNumbersFrom < 1)
            {
                startPageNumbersFrom = 1;

                //As page numbers are starting at one, output an even number of pages.  
                stopPageNumbersAt = pagesToOutput;
            }

            if (stopPageNumbersAt > pageCount)
            {
                stopPageNumbersAt = pageCount;
            }

            if ((stopPageNumbersAt - startPageNumbersFrom) < pagesToOutput)
            {
                startPageNumbersFrom = stopPageNumbersAt - pagesToOutput;
                if (startPageNumbersFrom < 1)
                {
                    startPageNumbersFrom = 1;
                }
            }
            /******************End: Xác định startPageNumbersFrom & stopPageNumbersAt**********************/

            //Các dấu ... chỉ những trang phía trước  
            if (startPageNumbersFrom > 1)
            {
                output.Append("<li><a href=\"" + string.Format(GetPageUrl(currentPage - 1, pageUrl), startPageNumbersFrom - 1) + "\">&hellip;</a></li>");
            }

            //Duyệt vòng for hiển thị các trang
            for (int i = startPageNumbersFrom; i <= stopPageNumbersAt; i++)
            {
                if (currentPage == i)
                {
                    output.Append("<li class=\"current\" ><a >" + i.ToString() + "</a> </li>");
                }
                else
                {
                    output.Append("<li><a href=\"" + string.Format(pageUrl, i) + "\">" + i.ToString() + "</a> </li>");
                }
            }

            //Các dấu ... chỉ những trang tiếp theo  
            if (stopPageNumbersAt < pageCount)
            {
                output.Append("<li><a href=\"" + string.Format(pageUrl, stopPageNumbersAt + 1) + "\">&hellip;</a></li>");
            }

            //Link Next(Trang tiếp) và Last(Trang cuối)
            if (currentPage != pageCount)
            {
                //output.Append("<span class=\"Unselect_next\"><a href=\"" + string.Format(pageUrl, currentPage + 1) + "\"></a></span>");
                //output.Append("<li class=\"UnselectedNext\" ><a title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\"><i class=\"fa fa-angle-right\"></i></a></li>");
                output.Append("<li><a title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\">Trang sau</a></li>");
                output.Append("<li><a title=\"" + strText[3] + "\" href=\"" + string.Format(pageUrl, pageCount) + "\">Trang cuối</a></li>");
            }
            output.Append("</ul>");
            //output.Append("</div>");
            return output.ToString();
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearchProduct.Text;
            string request = "/tat-ca-san-pham?";

            if (!String.IsNullOrEmpty(search))
            {
                request += "&textsearch=" + search;
            }

            if (!String.IsNullOrEmpty(ddlStockStatus.SelectedValue))
            {
                request += "&stockstatus=" + ddlStockStatus.SelectedValue;
            }

            if (ddlCategory.SelectedValue != "0")
            {
                request += "&categoryid=" + ddlCategory.SelectedValue;
            }

            if (rFromDate.SelectedDate.HasValue)
            {
                request += "&fromdate=" + rFromDate.SelectedDate.ToString();
            }

            if (rToDate.SelectedDate.HasValue)
            {
                request += "&todate=" + rToDate.SelectedDate.ToString();
            }

            if (!String.IsNullOrEmpty(ddlShowHomePage.SelectedValue))
            {
                request += "&showhomepage=" + ddlShowHomePage.SelectedValue;
            }

            if (!String.IsNullOrEmpty(ddlWebPublish.SelectedValue))
            {
                request += "&webpublish=" + ddlWebPublish.SelectedValue;
            }

            if (!String.IsNullOrEmpty(ddlQuantityFilter.SelectedValue))
            {
                if (ddlQuantityFilter.SelectedValue == "greaterthan" || ddlQuantityFilter.SelectedValue == "lessthan")
                {
                    request += "&quantityfilter=" + ddlQuantityFilter.SelectedValue + "&quantity=" + txtQuantity.Text;
                }

                if (ddlQuantityFilter.SelectedValue == "between")
                {
                    request += "&quantityfilter=" + ddlQuantityFilter.SelectedValue + "&quantitymin=" + txtQuantityMin.Text + "&quantitymax=" + txtQuantityMax.Text;
                }
            }

            // Add filter valiable value
            if (!String.IsNullOrEmpty(ddlColor.SelectedValue))
            {
                request += "&color=" + ddlColor.SelectedValue;
            }
            if (!String.IsNullOrEmpty(ddlSize.SelectedValue))
            {
                request += "&size=" + ddlSize.SelectedValue;
            }

            // Add filter preOrder
            if (!String.IsNullOrEmpty(ddlPreOrder.SelectedValue))
                request += "&preOrder=" + ddlPreOrder.SelectedValue;

            // Add filter tag
            if (ddlTag.SelectedValue != "0")
            {
                request += "&tag=" + ddlTag.SelectedValue;
            }

            // Add filter order by
            if (!String.IsNullOrEmpty(ddlOrderBy.SelectedValue))
            {
                request += "&orderby=" + ddlOrderBy.SelectedValue;
            }

            // Add filter warehouse
            if (!String.IsNullOrEmpty(ddlWarehouse.SelectedValue))
            {
                request += "&warehouse=" + ddlWarehouse.SelectedValue;
            }

            Response.Redirect(request);
        }
        public class danhmuccon1
        {
            public tbl_Category cate1 { get; set; }
            public string parentName { get; set; }
        }

        [WebMethod]
        public static bool liquidateProduct(int productID)
        {
            var loginHiddenPage = HttpContext.Current.Request.Cookies["loginHiddenPage"];
            var usernameLoginSystem = HttpContext.Current.Request.Cookies["usernameLoginSystem"];

            if (loginHiddenPage != null)
                acc = AccountController.GetByUsername(loginHiddenPage.Value);
            else if (usernameLoginSystem != null)
                acc = AccountController.GetByUsername(usernameLoginSystem.Value);
            else
                throw new Exception("Có vấn đề trong việc lấy thông tin User");

            return StockManagerController.liquidateProduct(acc, productID);
        }

        [WebMethod]
        public static ProductSQL recoverLiquidatedProduct(int productID, string sku)
        {
            var loginHiddenPage = HttpContext.Current.Request.Cookies["loginHiddenPage"];
            var usernameLoginSystem = HttpContext.Current.Request.Cookies["usernameLoginSystem"];

            if (loginHiddenPage != null)
                acc = AccountController.GetByUsername(loginHiddenPage.Value);
            else if (usernameLoginSystem != null)
                acc = AccountController.GetByUsername(usernameLoginSystem.Value);
            else
                throw new Exception("Có vấn đề trong việc lấy thông tin User");

            var recover = StockManagerController.recoverLiquidatedProduct(acc, productID);

            if (!recover)
                throw new Exception("Lỗi phục hồi lại sản phẩm xả hàng");

            var filter = new ProductFilterModel() { search = sku };
            var page = new PaginationMetadataModel() { currentPage = 1 };

            var result = ProductController.GetAllSql(filter, ref page)
                .Where(x => x.ID == productID)
                .FirstOrDefault();

            return result;
        }
    }
}