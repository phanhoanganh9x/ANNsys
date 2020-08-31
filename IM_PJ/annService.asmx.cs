using Bnails.Bussiness;
using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Utils;
using MB.Extensions;
using Newtonsoft.Json;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using static IM_PJ.Controllers.ProductController;

namespace IM_PJ
{
    /// <summary>
    /// Summary description for annService
    /// </summary>
    [WebService(Namespace = "http://hethongann.com")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class annService : System.Web.Services.WebService
    {

        public bool Login(string username, string password)
        {
            var user = AccountController.Login(username, password);
            if (user != null && user.RoleID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GetRootCategory()
        {
            var rs = new ResponseClass();
            var category = CategoryController.API_GetRootCategory();
            if (category.Count > 0)
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.SUCCESS);
                rs.Status = APIUtils.ResponseMessage.Success.ToString();
                rs.Category = category;
            }
            else
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                rs.Status = APIUtils.ResponseMessage.Error.ToString();
                rs.Message = APIUtils.OBJ_DNTEXIST;
            }
            Context.Response.ContentType = "application/json";
            Context.Response.Write(JsonConvert.SerializeObject(rs, Formatting.Indented));
            Context.Response.Flush();
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public void GetAllCategory(string username, string password)
        {
            var rs = new ResponseClass();
            if (Login(username, password))
            {
                var category = CategoryController.API_GetAllCategory();
                if (category.Count > 0)
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.SUCCESS);
                    rs.Status = APIUtils.ResponseMessage.Success.ToString();
                    rs.Category = category;
                }
                else
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                    rs.Status = APIUtils.ResponseMessage.Error.ToString();
                    rs.Message = APIUtils.OBJ_DNTEXIST;
                }
            }
            else
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.FAILED);
                rs.Status = APIUtils.ResponseMessage.Fail.ToString();
            }

            Context.Response.ContentType = "application/json";
            Context.Response.Write(JsonConvert.SerializeObject(rs, Formatting.Indented));
            Context.Response.Flush();
            Context.Response.End();
        }

        public void GetCategoryByParentID(int ParentID)
        {
            var rs = new ResponseClass();
            var category = CategoryController.API_GetByParentID(ParentID);
            if (category.Count > 0)
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.SUCCESS);
                rs.Status = APIUtils.ResponseMessage.Success.ToString();
                rs.Category = category;
            }
            else
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                rs.Status = APIUtils.ResponseMessage.Error.ToString();
                rs.Message = APIUtils.OBJ_DNTEXIST;
            }
            Context.Response.ContentType = "application/json";
            Context.Response.Write(JsonConvert.SerializeObject(rs, Formatting.Indented));
            Context.Response.Flush();
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public void GetProductBySKU(string SKU, string username, string password)
        {
            var rs = new ResponseClass();
            if (Login(username, password))
            {
                // Create order fileter
                var filter = new ProductFilterModel() { search = SKU};
                // Create pagination
                var page = new PaginationMetadataModel();
                var Product = ProductController.GetAllSql(filter, ref page);

                if (Product.Count > 0)
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.SUCCESS);
                    rs.Status = APIUtils.ResponseMessage.Success.ToString();

                    foreach (var item in Product)
                    {
                        item.ProductContent = String.Format("<h3>{0}</h3>", item.ProductTitle) + item.ProductContent;

                        string featuredImage = "";
                        if (!string.IsNullOrEmpty(item.ProductImage))
                        {
                            featuredImage = item.ProductImage;
                            item.ProductContent += String.Format("<p><img src='/wp-content/uploads/{0}' alt='{1}'/></p>", item.ProductImage, item.ProductTitle);
                            item.ProductImage = Thumbnail.getURL(item.ProductImage, Thumbnail.Size.Source);
                        }

                        if (!string.IsNullOrEmpty(item.ProductImageClean))
                        {
                            item.ProductImageClean = Thumbnail.getURL(item.ProductImageClean, Thumbnail.Size.Source);
                            item.ProductImage = item.ProductImageClean + "|" + item.ProductImage;
                        }

                        var productImage = ProductImageController.GetByProductID(item.ID);

                        if (productImage.Count() > 0)
                        {
                            foreach (var image in productImage)
                            {
                                if (image.ProductImage != featuredImage)
                                {
                                    item.ProductImage += "|" + Thumbnail.getURL(image.ProductImage, Thumbnail.Size.Source);
                                    item.ProductContent += String.Format("<p><img src='/wp-content/uploads/{0}' alt='{1}'/></p>", image.ProductImage, item.ProductTitle);
                                }
                            }
                        }
                    }
                    rs.Product = Product;
                }
                else
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                    rs.Status = APIUtils.ResponseMessage.Error.ToString();
                    rs.Message = APIUtils.OBJ_DNTEXIST;
                }
            }
            else
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.FAILED);
                rs.Status = APIUtils.ResponseMessage.Fail.ToString();
            }

            Context.Response.ContentType = "application/json";
            Context.Response.Write(JsonConvert.SerializeObject(rs, Formatting.Indented));
            Context.Response.Flush();
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public void GetProductByCategory(int CategoryID, int limit, string username, string password, int showHomePage, int minQuantity, int changeProductName)
        {
            var rs = new ResponseClass();
            if (Login(username, password))
            {
                var Product = ProductController.GetProductAPI(CategoryID, limit, showHomePage, minQuantity, changeProductName);

                if (Product.Count > 0)
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.SUCCESS);
                    rs.Status = APIUtils.ResponseMessage.Success.ToString();

                    foreach (var item in Product)
                    {
                        item.ProductContent = String.Format("<h3>{0}</h3>", item.ProductTitle) + item.ProductContent;

                        string featuredImage = "";
                        if (!string.IsNullOrEmpty(item.ProductImage))
                        {
                            featuredImage = item.ProductImage;
                            item.ProductContent += String.Format("<p><img src='/wp-content/uploads/{0}' alt='{1}'/></p>", item.ProductImage, item.ProductTitle);
                            item.ProductImage = Thumbnail.getURL(item.ProductImage, Thumbnail.Size.Source);
                        }

                        if (!string.IsNullOrEmpty(item.ProductImageClean))
                        {
                            item.ProductImageClean = Thumbnail.getURL(item.ProductImageClean, Thumbnail.Size.Source);
                            item.ProductImage = item.ProductImageClean + "|" + item.ProductImage;
                        }

                        var productImage = ProductImageController.GetByProductID(item.ID);

                        if (productImage.Count() > 0)
                        {
                            foreach (var image in productImage)
                            {
                                if (image.ProductImage != featuredImage)
                                {
                                    item.ProductImage += "|" + Thumbnail.getURL(image.ProductImage, Thumbnail.Size.Source);
                                    item.ProductContent += String.Format("<p><img src='/wp-content/uploads/{0}' alt='{1}'/></p>", image.ProductImage, item.ProductTitle);
                                }
                            }
                        }
                    }
                    rs.Product = Product;
                }
                else
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                    rs.Status = APIUtils.ResponseMessage.Error.ToString();
                    rs.Message = APIUtils.OBJ_DNTEXIST;
                }
            }
            else
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.FAILED);
                rs.Status = APIUtils.ResponseMessage.Fail.ToString();
            }

            Context.Response.ContentType = "application/json";
            Context.Response.Write(JsonConvert.SerializeObject(rs, Formatting.Indented));
            Context.Response.Flush();
            Context.Response.End();
        }

        public void GetProductImageByProductID(int ProductID, string username, string password)
        {
            var rs = new ResponseClass();
            if (Login(username, password))
            {
                var ProductImage = ProductImageController.GetByProductID(ProductID);
                if (ProductImage.Count > 0)
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.SUCCESS);
                    rs.Status = APIUtils.ResponseMessage.Success.ToString();
                    foreach(var item in ProductImage)
                    {
                        item.ProductImage = Thumbnail.getURL(item.ProductImage, Thumbnail.Size.Source);
                    }
                    rs.ProductImage = ProductImage;
                }
                else
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                    rs.Status = APIUtils.ResponseMessage.Error.ToString();
                    rs.Message = APIUtils.OBJ_DNTEXIST;
                }
            }
            else
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.FAILED);
                rs.Status = APIUtils.ResponseMessage.Fail.ToString();
            }
            Context.Response.ContentType = "application/json";
            Context.Response.Write(JsonConvert.SerializeObject(rs, Formatting.Indented));
            Context.Response.Flush();
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public void GetProductVariableByProductID(int ProductID, string username, string password)
        {
            var rs = new ResponseClass();
            if (Login(username, password))
            {
                var ProductVariable = ProductVariableController.GetProductID(ProductID);
                if (ProductVariable.Count > 0)
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.SUCCESS);
                    rs.Status = APIUtils.ResponseMessage.Success.ToString();

                    foreach (var item in ProductVariable)
                    {
                        item.Stock = PJUtils.GetSotckProduct(1, item.SKU);

                        if (item.Stock <= 0)
                        {
                            item.Stock = 1;
                        }

                        item.StockStatus = 1;

                        item.Image = Thumbnail.getURL(item.Image, Thumbnail.Size.Source);
                    }

                    rs.ProductVariable = ProductVariable;
                }
                else
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                    rs.Status = APIUtils.ResponseMessage.Error.ToString();
                    rs.Message = APIUtils.OBJ_DNTEXIST;
                }
            }
            else
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.FAILED);
                rs.Status = APIUtils.ResponseMessage.Fail.ToString();
            }
            Context.Response.ContentType = "application/json";
            Context.Response.Write(JsonConvert.SerializeObject(rs, Formatting.Indented));
            Context.Response.Flush();
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public void GetProductVariableValueByProductVariableID(int ProductVariableID, string username, string password)
        {
            var rs = new ResponseClass();
            if (Login(username, password))
            {
                var ProductVariableValue = ProductVariableValueController.GetByProductVariableID(ProductVariableID);
                if (ProductVariableValue.Count > 0)
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.SUCCESS);
                    rs.Status = APIUtils.ResponseMessage.Success.ToString();
                    rs.ProductVariableValue = ProductVariableValue;
                }
                else
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                    rs.Status = APIUtils.ResponseMessage.Error.ToString();
                    rs.Message = APIUtils.OBJ_DNTEXIST;
                }
            }
            else
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.FAILED);
                rs.Status = APIUtils.ResponseMessage.Fail.ToString();
            }

            Context.Response.ContentType = "application/json";
            Context.Response.Write(JsonConvert.SerializeObject(rs, Formatting.Indented));
            Context.Response.Flush();
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public void LoginSystem(string username, string password)
        {
            var rs = new ResponseClass();
            var user = AccountController.Login(username, password);
            if (user != null)
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.SUCCESS);
                rs.Status = APIUtils.ResponseMessage.Success.ToString();
                rs.User = user;
            }
            else
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                rs.Status = APIUtils.ResponseMessage.Error.ToString();
                rs.Message = APIUtils.OBJ_DNTEXIST;
            }
            Context.Response.ContentType = "application/json";
            Context.Response.Write(JsonConvert.SerializeObject(rs, Formatting.Indented));
            Context.Response.Flush();
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public void GetAgentCode(int AgentID, string username, string password)
        {
            var rs = new ResponseClass();
            if (Login(username, password))
            {
                var agent = AgentController.GetByID(AgentID);
                if (agent != null)
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.SUCCESS);
                    rs.Status = APIUtils.ResponseMessage.Success.ToString();
                    rs.Agent = agent;
                }
                else
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                    rs.Status = APIUtils.ResponseMessage.Error.ToString();
                    rs.Message = APIUtils.OBJ_DNTEXIST;
                }
            }
            else
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.FAILED);
                rs.Status = APIUtils.ResponseMessage.Fail.ToString();
            }
            Context.Response.ContentType = "application/json";
            Context.Response.Write(JsonConvert.SerializeObject(rs, Formatting.Indented));
            Context.Response.Flush();
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public void InserOrder1(string AgentAPIID, string AgentAPICode, int OrderType, string CustomerName, string CustomerPhone, string CustomerEmail,
            string CustomerAddress, double TotalPrice, int PaymentStatus, int ExcuteStatus, List<ProductOrder> ListProduct, string CreatedBy, string username, string password)
        {
            var rs = new ResponseClass();
            if (Login(username, password))
            {
                DateTime currentDate = DateTime.Now;
                var agent = AgentController.GetByAPICodeID(AgentAPIID, AgentAPICode);
                if (agent != null)
                {
                    int AgentID = agent.ID;
                    int CustomerID = 0;
                    string AdditionFee = "0";
                    string DisCount = "0";
                    var checkphone = CustomerController.GetByPhone(CustomerPhone);
                    if (checkphone != null)
                    {
                        CustomerID = checkphone.ID;
                    }
                    else
                    {
                        string kq = CustomerController.Insert(CustomerName, CustomerPhone, CustomerAddress, CustomerEmail, 0, 0, currentDate, CreatedBy, false, "", "", "", "", "");
                        if (kq.ToInt(0) > 0)
                        {
                            CustomerID = kq.ToInt(0);
                        }
                    }
                    bool IsHidden = false;
                    int Wayin = 2;

                    var orderNew = new tbl_Order() {
                        AgentID = AgentID,
                        OrderType = OrderType,
                        AdditionFee = AdditionFee,
                        DisCount = DisCount,
                        CustomerID = CustomerID,
                        CustomerName = CustomerName,
                        CustomerPhone = CustomerPhone,
                        CustomerAddress = CustomerAddress,
                        CustomerEmail = CustomerEmail,
                        TotalPrice = TotalPrice.ToString(),
                        TotalPriceNotDiscount = TotalPrice.ToString(),
                        PaymentStatus = PaymentStatus,
                        ExcuteStatus = ExcuteStatus,
                        IsHidden = IsHidden,
                        WayIn = Wayin,
                        CreatedDate = currentDate,
                        CreatedBy = CreatedBy,
                        DiscountPerProduct = 0,
                        TotalDiscount = 0,
                        FeeShipping = "0",
                        PaymentType = 0,
                        ShippingType = 0,
                        DateDone = DateTime.Now,
                        GuestPaid = 0,
                        GuestChange = 0,
                        TransportCompanyID = 0,
                        TransportCompanySubID = 0,
                        OtherFeeName = String.Empty,
                        OtherFeeValue = 0,
                        PostalDeliveryType = 1,
                    };

                    var ret = OrderController.Insert(orderNew);
                    int OrderID = ret.ID;
                    if (OrderID > 0)
                    {
                        if (ListProduct.Count > 0)
                        {
                            foreach (var p in ListProduct)
                            {

                                int ProductID = 0;
                                int ProductVariableID = 0;

                                int ID = p.ID;
                                string SKU = p.SKU;
                                int producttype = p.ProductType;
                                if (producttype == 1)
                                {
                                    ProductID = ID;
                                    ProductVariableID = 0;
                                }
                                else
                                {
                                    ProductID = 0;
                                    ProductVariableID = ID;
                                }

                                string ProductVariableName = p.ProductVariableName;
                                string ProductVariableValue = p.ProductVariableValue;
                                double Quantity = Convert.ToDouble(p.Quantity);
                                string ProductName = p.ProductName;
                                string ProductImageOrigin = p.ProductImageOrigin;
                                double ProductPrice = p.Price;
                                string ProductVariableSave = p.ProductVariableDescription;


                                if (ExcuteStatus == 2 && PaymentStatus == 3)
                                {
                                    OrderDetailController.Insert(AgentID, OrderID, SKU, ProductID, ProductVariableID, ProductVariableSave, Quantity,
                                    ProductPrice, 1, 0, producttype, currentDate, CreatedBy, true);
                                    if (producttype == 1)
                                    {
                                        StockManagerController.Insert(
                                            new tbl_StockManager()
                                            {
                                                AgentID = AgentID,
                                                ProductID = ProductID,
                                                ProductVariableID = 0,
                                                Quantity = Quantity,
                                                QuantityCurrent = 0,
                                                Type = 2,
                                                NoteID = String.Empty,
                                                OrderID = OrderID,
                                                Status = 3,
                                                SKU = SKU,
                                                CreatedDate = currentDate,
                                                CreatedBy = CreatedBy,
                                                ModifiedDate = currentDate,
                                                ModifiedBy = CreatedBy,
                                                MoveProID = 0,
                                                ParentID = ProductID
                                            });
                                    }
                                    else
                                    {
                                        int parentID = 0;
                                        string parentSKU = "";
                                        var productV = ProductVariableController.GetByID(ProductVariableID);
                                        if (productV != null)
                                            parentSKU = productV.ParentSKU;
                                        if (!string.IsNullOrEmpty(parentSKU))
                                        {
                                            var product = ProductController.GetBySKU(parentSKU);
                                            if (product != null)
                                                parentID = product.ID;
                                        }
                                        StockManagerController.Insert(
                                            new tbl_StockManager
                                            {
                                                AgentID = AgentID,
                                                ProductID = 0,
                                                ProductVariableID = ProductVariableID,
                                                Quantity = Quantity,
                                                QuantityCurrent = 0,
                                                Type = 2,
                                                NoteID = String.Empty,
                                                OrderID = OrderID,
                                                Status = 3,
                                                SKU = SKU,
                                                CreatedDate = currentDate,
                                                CreatedBy = CreatedBy,
                                                ModifiedDate = currentDate,
                                                ModifiedBy = CreatedBy,
                                                MoveProID = 0,
                                                ParentID = parentID,
                                            });
                                    }
                                }
                                else
                                {
                                    OrderDetailController.Insert(AgentID, OrderID, SKU, ProductID, ProductVariableID, ProductVariableSave, Quantity,
                                    ProductPrice, 1, 0, producttype, currentDate, CreatedBy, false);
                                }
                            }
                        }
                        rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.SUCCESS);
                        rs.Status = APIUtils.ResponseMessage.Success.ToString();
                        rs.Message = "Tạo mới đơn hàng thành công";
                    }
                    else
                    {
                        rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                        rs.Status = APIUtils.ResponseMessage.Error.ToString();
                        rs.Message = "Có lỗi trong quá trình tạo mới đơn hàng, vui lòng thử lại sau.";
                    }
                }
                else
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                    rs.Status = APIUtils.ResponseMessage.Error.ToString();
                    rs.Message = "Không tồn tại thông tin đại lý";
                }
            }
            else
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.FAILED);
                rs.Status = APIUtils.ResponseMessage.Fail.ToString();
            }

            Context.Response.ContentType = "application/json";
            Context.Response.Write(JsonConvert.SerializeObject(rs, Formatting.Indented));
            Context.Response.Flush();
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public void InserOrder(string AgentAPIID, string AgentAPICode, string OrderType, string CustomerName, string CustomerPhone, string CustomerEmail,
            string CustomerAddress, string TotalPrice, string PaymentStatus, string ExcuteStatus, string CreatedBy, string productquantity,
            string FeeShipping, int PaymentType, int ShippingType, string username, string password)
        {
            var rs = new ResponseClass();
            if (Login(username, password))
            {
                DateTime currentDate = DateTime.Now;
                var agent = AgentController.GetByAPICodeID(AgentAPIID, AgentAPICode);
                if (agent != null)
                {
                    int AgentID = agent.ID;
                    int CustomerID = 0;
                    string AdditionFee = "0";
                    string DisCount = "0";
                    var checkphone = CustomerController.GetByPhone(CustomerPhone);
                    if (checkphone != null)
                    {
                        CustomerID = checkphone.ID;
                    }
                    else
                    {
                        string kq = CustomerController.Insert(CustomerName, CustomerPhone, CustomerAddress, CustomerEmail, 0, 0, currentDate, CreatedBy, false, "", "", "", "", "");
                        if (kq.ToInt(0) > 0)
                        {
                            CustomerID = kq.ToInt(0);
                        }
                    }
                    bool IsHidden = false;
                    int Wayin = 2;

                    double amount = 0;
                    double totalDiscount = 0;
                    double totalleft = 0;

                    var d = DiscountCustomerController.getbyCustID(CustomerID);
                    if (d.Count > 0)
                    {
                        amount = d[0].DiscountAmount;
                    }
                    int pquantity = productquantity.ToInt(0);
                    if (amount == 0)
                    {
                        if (pquantity > 29 && pquantity <= 49)
                        {
                            amount = 3000;
                        }
                        else if (pquantity > 49 && pquantity <= 99)
                        {
                            amount = 5000;
                        }
                        else if (pquantity > 99 && pquantity <= 199)
                        {
                            amount = 7000;
                        }
                        else if (pquantity > 199)
                        {
                            amount = 8000;
                        }
                        else
                        {
                            amount = 0;
                        }
                    }
                    if (amount > 0)
                    {
                        totalDiscount = amount * pquantity;
                        totalleft = Convert.ToDouble(TotalPrice) - totalDiscount;

                    }
                    else
                    {
                        totalDiscount = 0;
                        totalleft = Convert.ToDouble(TotalPrice);
                    }

                    var orderNew = new tbl_Order()
                    {
                        AgentID = AgentID,
                        OrderType = OrderType.ToInt(1),
                        AdditionFee = AdditionFee,
                        DisCount = DisCount,
                        CustomerID = CustomerID,
                        CustomerName = CustomerName,
                        CustomerPhone = CustomerPhone,
                        CustomerAddress = CustomerAddress,
                        CustomerEmail = CustomerEmail,
                        TotalPrice = totalleft.ToString(),
                        TotalPriceNotDiscount = TotalPrice.ToString(),
                        PaymentStatus = PaymentStatus.ToInt(0),
                        ExcuteStatus = ExcuteStatus.ToInt(0),
                        IsHidden = IsHidden,
                        WayIn = Wayin,
                        CreatedDate = currentDate,
                        CreatedBy = CreatedBy,
                        DiscountPerProduct = amount,
                        TotalDiscount = totalDiscount,
                        FeeShipping = FeeShipping.ToString(),
                        PaymentType = PaymentType,
                        ShippingType = ShippingType,
                        DateDone = DateTime.Now,
                        GuestPaid = 0,
                        GuestChange = 0,
                        TransportCompanyID = 0,
                        TransportCompanySubID = 0,
                        OtherFeeName = String.Empty,
                        OtherFeeValue = 0,
                        PostalDeliveryType = 1,
                    };

                    var ret = OrderController.Insert(orderNew);
                    int OrderID = ret.ID;
                    if (OrderID > 0)
                    {

                        rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.SUCCESS);
                        rs.Status = APIUtils.ResponseMessage.Success.ToString();
                        rs.OrderID = OrderID;
                        rs.Message = "Tạo mới đơn hàng thành công";
                    }
                    else
                    {
                        rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                        rs.Status = APIUtils.ResponseMessage.Error.ToString();
                        rs.Message = "Có lỗi trong quá trình tạo mới đơn hàng, vui lòng thử lại sau.";
                    }
                }
                else
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                    rs.Status = APIUtils.ResponseMessage.Error.ToString();
                    rs.Message = "Không tồn tại thông tin đại lý";
                }
            }
            else
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.FAILED);
                rs.Status = APIUtils.ResponseMessage.Fail.ToString();
            }

            Context.Response.ContentType = "application/json";
            Context.Response.Write(JsonConvert.SerializeObject(rs, Formatting.Indented));
            Context.Response.Flush();
            Context.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public void InserOrderDetail(string AgentAPIID, string AgentAPICode, int OrderID, int ID, string SKU, int ProductType,
            string ProductVariableName, string ProductVariableValue, double Quantity, string ProductName, string ProductImageOrigin,
            double ProductPrice, string ProductVariableSave, int ExcuteStatus, int PaymentStatus, string CreatedBy, string username, string password)
        {
            var rs = new ResponseClass();
            if (Login(username, password))
            {
                DateTime currentDate = DateTime.Now;
                var agent = AgentController.GetByAPICodeID(AgentAPIID, AgentAPICode);
                if (agent != null)
                {
                    int AgentID = agent.ID;
                    int ProductID = 0;
                    int ProductVariableID = 0;
                    if (ProductType == 1)
                    {
                        ProductID = ID;
                        ProductVariableID = 0;
                    }
                    else
                    {
                        ProductID = 0;
                        ProductVariableID = ID;
                    }

                    if (ExcuteStatus == 2 && PaymentStatus == 3)
                    {
                        OrderDetailController.Insert(AgentID, OrderID, SKU, ProductID, ProductVariableID, ProductVariableSave, Quantity,
                        ProductPrice, 1, 0, ProductType, currentDate, CreatedBy, true);
                        if (ProductType == 1)
                        {
                            StockManagerController.Insert(
                                new tbl_StockManager
                                {
                                    AgentID = AgentID,
                                    ProductID = ProductID,
                                    ProductVariableID = 0,
                                    Quantity = Quantity,
                                    QuantityCurrent = 0,
                                    Type = 2,
                                    NoteID = String.Empty,
                                    OrderID = OrderID,
                                    Status = 3,
                                    SKU = SKU,
                                    CreatedDate = currentDate,
                                    CreatedBy = CreatedBy,
                                    ModifiedDate = currentDate,
                                    ModifiedBy = CreatedBy,
                                    MoveProID = 0,
                                    ParentID = ProductID
                                });
                        }
                        else
                        {
                            int parentID = 0;
                            string parentSKU = "";
                            var productV = ProductVariableController.GetByID(ProductVariableID);
                            if (productV != null)
                                parentSKU = productV.ParentSKU;
                            if (!string.IsNullOrEmpty(parentSKU))
                            {
                                var product = ProductController.GetBySKU(parentSKU);
                                if (product != null)
                                    parentID = product.ID;
                            }
                            StockManagerController.Insert(
                                new tbl_StockManager
                                {
                                    AgentID = AgentID,
                                    ProductID = 0,
                                    ProductVariableID = ProductVariableID,
                                    Quantity = Quantity,
                                    QuantityCurrent = 0,
                                    Type = 2,
                                    NoteID = String.Empty,
                                    OrderID = OrderID,
                                    Status = 3,
                                    SKU = SKU,
                                    CreatedDate = currentDate,
                                    CreatedBy = CreatedBy,
                                    ModifiedDate = currentDate,
                                    ModifiedBy = CreatedBy,
                                    MoveProID = 0,
                                    ParentID = parentID
                                });
                        }
                    }
                    else
                    {
                        OrderDetailController.Insert(AgentID, OrderID, SKU, ProductID, ProductVariableID, ProductVariableSave, Quantity,
                        ProductPrice, 1, 0, ProductType, currentDate, CreatedBy, false);
                    }
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.SUCCESS);
                    rs.Status = APIUtils.ResponseMessage.Success.ToString();
                    rs.Message = "Tạo mới đơn hàng thành công";
                }
                else
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                    rs.Status = APIUtils.ResponseMessage.Error.ToString();
                    rs.Message = "Không tồn tại thông tin đại lý";
                }
            }
            else
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.FAILED);
                rs.Status = APIUtils.ResponseMessage.Fail.ToString();
            }

            Context.Response.ContentType = "application/json";
            Context.Response.Write(JsonConvert.SerializeObject(rs, Formatting.Indented));
            Context.Response.Flush();
            Context.Response.End();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = false)]
        public void GetStockAll(string username, string password)
        {
            if (Login(username, password))
            {
                var StockManager = StockManagerController.GetStockAll();

                if (StockManager.Count > 0)
                {
                    var dataToExportToCSV = StockManager.Select(x =>
                    {
                        var quantityCurrent = 0D;

                        if (x.Type == 1)
                        {
                            quantityCurrent = x.QuantityCurrent.Value + x.Quantity.Value;
                        }
                        else
                        {
                            quantityCurrent = x.QuantityCurrent.Value - x.Quantity.Value;
                        }

                        return new StockManager()
                        {
                            SKU = x.SKU,
                            Quantity = quantityCurrent,
                        };
                    }).ToList();

                    string attachment = String.Format("attachment; filename={0}_stock-all.csv", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.AddHeader("content-disposition", attachment);
                    HttpContext.Current.Response.ContentType = "text/csv";
                    HttpContext.Current.Response.AddHeader("Pragma", "public");
                    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;

                    var sb = new StringBuilder();
                    sb.AppendLine("parent_sku, sku, stock, stock_status");
                    foreach (var line in dataToExportToCSV)
                    {
                        var parentProduct = ProductVariableController.GetBySKU(line.SKU);
                        string parentSKU = "";
                        if (parentProduct != null)
                        {
                            parentSKU = parentProduct.ParentSKU;
                        }
                        sb.AppendLine(String.Format("{0}, {1}, {2}, {3}", parentSKU, line.SKU, line.Quantity, line.Quantity > 0 ? "instock" : "outofstock"));
                    }

                    HttpContext.Current.Response.Write(sb.ToString());
                }
            }
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = false)]
        public void GetStockVariable(string username, string password)
        {
            if (Login(username, password))
            {
                var StockManager = StockManagerController.GetStockAll();

                if (StockManager.Count > 0)
                {
                    var dataToExportToCSV = StockManager.Select(x =>
                    {
                        var quantityCurrent = 0D;

                        if (x.Type == 1)
                        {
                            quantityCurrent = x.QuantityCurrent.Value + x.Quantity.Value;
                        }
                        else
                        {
                            quantityCurrent = x.QuantityCurrent.Value - x.Quantity.Value;
                        }

                        return new StockManager()
                        {
                            SKU = x.SKU,
                            Quantity = quantityCurrent,
                        };
                    }).ToList();

                    string attachment = String.Format("attachment; filename={0}_stock-variable.csv", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.AddHeader("content-disposition", attachment);
                    HttpContext.Current.Response.ContentType = "text/csv";
                    HttpContext.Current.Response.AddHeader("Pragma", "public");
                    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;

                    var sb = new StringBuilder();
                    sb.AppendLine("parent_sku, sku, stock, stock_status, manage_stock");
                    foreach (var line in dataToExportToCSV)
                    {
                        var parentProduct = ProductVariableController.GetBySKU(line.SKU);
                        string parentSKU = "";
                        if (parentProduct != null)
                        {
                            parentSKU = parentProduct.ParentSKU;
                            sb.AppendLine(String.Format("{0}, {1}, {2}, {3}, {4}", parentSKU, line.SKU, line.Quantity, line.Quantity > 0 ? "instock" : "outofstock", "yes"));
                        }
                    }

                    HttpContext.Current.Response.Write(sb.ToString());
                }
            }
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = false)]
        public void GetStockProduct(string username, string password)
        {
            if (Login(username, password))
            {
                var StockManager = StockManagerController.GetStockAll();

                if (StockManager.Count > 0)
                {
                    var dataToExportToCSV = StockManager.Select(x =>
                    {
                        var quantityCurrent = 0D;

                        if (x.Type == 1)
                        {
                            quantityCurrent = x.QuantityCurrent.Value + x.Quantity.Value;
                        }
                        else
                        {
                            quantityCurrent = x.QuantityCurrent.Value - x.Quantity.Value;
                        }

                        return new StockManager()
                        {
                            SKU = x.SKU,
                            Quantity = quantityCurrent,
                        };
                    }).ToList();

                    string attachment = String.Format("attachment; filename={0}_stock-product.csv", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.AddHeader("content-disposition", attachment);
                    HttpContext.Current.Response.ContentType = "text/csv";
                    HttpContext.Current.Response.AddHeader("Pragma", "public");
                    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;

                    var sb = new StringBuilder();
                    sb.AppendLine("sku, stock, stock_status, manage_stock");
                    foreach (var line in dataToExportToCSV)
                    {
                        var Product = ProductController.GetBySKU(line.SKU);
                        string parentSKU = "";
                        if (Product != null)
                        {
                            parentSKU = Product.ProductSKU;
                            sb.AppendLine(String.Format("{0}, {1}, {2}, {3}", line.SKU, line.Quantity, line.Quantity > 0 ? "instock" : "outofstock", "yes"));
                        }
                    }

                    HttpContext.Current.Response.Write(sb.ToString());
                }
            }
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = false)]
        public void GetStockToDay(string username, string password)
        {
            if (Login(username, password))
            {
                var StockManager = StockManagerController.GetStockToDay();

                if (StockManager.Count > 0)
                {
                    var dataToExportToCSV = StockManager.Select(x =>
                    {
                        var quantityCurrent = 0D;

                        if (x.Type == 1)
                        {
                            quantityCurrent = x.QuantityCurrent.Value + x.Quantity.Value;
                        }
                        else
                        {
                            quantityCurrent = x.QuantityCurrent.Value - x.Quantity.Value;
                        }

                        return new StockManager()
                        {
                            SKU = x.SKU,
                            Quantity = quantityCurrent,
                        };
                    }).ToList();

                    string attachment = String.Format("attachment; filename={0}_stock-today.csv", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.AddHeader("content-disposition", attachment);
                    HttpContext.Current.Response.ContentType = "text/csv";
                    HttpContext.Current.Response.AddHeader("Pragma", "public");
                    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;

                    var sb = new StringBuilder();
                    sb.AppendLine("SKU, Stock, StockStatus");
                    foreach (var line in dataToExportToCSV)
                    {
                        sb.AppendLine(String.Format("{0}, {1}", line.SKU, line.Quantity, line.Quantity > 0 ? "instock" : "outofstock"));
                    }

                    HttpContext.Current.Response.Write(sb.ToString());
                }
            }
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public void insertRegister(string name, string phone, string address, int province, string productcategory, string note, string referer, string username, string password)
        {
            var rs = new ResponseClass();
            if (username == "register" && password == "register@ann")
            {
                int ID = 0;

                Regex digitsOnly = new Regex(@"[^\d]");
                string numberphone = digitsOnly.Replace(phone, "");

                if (numberphone.Length >= 9)
                {
                    var checkRegister = RegisterController.GetByPhone(numberphone);

                    Register register = new Register();

                    if (checkRegister != null)
                    {
                        register.ID = checkRegister.ID;
                        register.Name = name;
                        register.Phone = checkRegister.Phone;
                        register.UserID = checkRegister.UserID;
                        register.Status = checkRegister.Status;
                        register.Note = note;
                        register.Address = address;
                        register.ProductCategory = productcategory;
                        register.ProvinceID = province;
                        register.CreatedDate = DateTime.Now;
                        register.Referer = referer;

                        ID = RegisterController.Update(register);
                    }
                    else
                    {
                        register.Name = name;
                        register.Phone = numberphone;
                        register.UserID = null;
                        register.Status = 1;
                        register.Note = note;
                        register.Address = address;
                        register.ProductCategory = productcategory;
                        register.ProvinceID = province;
                        register.Referer = referer;

                        ID = RegisterController.Insert(register);
                    }
                }

                if(ID != 0)
                {
                    rs.OrderID = ID;
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.SUCCESS);
                    rs.Status = APIUtils.ResponseMessage.Success.ToString();
                    rs.Message = "Tạo mới đăng ký thành công.";
                }
                else
                {
                    rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.NotFound);
                    rs.Status = APIUtils.ResponseMessage.Error.ToString();
                    rs.Message = "Có lỗi trong quá trình tạo đăng ký.";
                }
            }
            else
            {
                rs.Code = APIUtils.GetResponseCode(APIUtils.ResponseCode.FAILED);
                rs.Status = APIUtils.ResponseMessage.Fail.ToString();
            }

            Context.Response.ContentType = "application/json";
            Context.Response.Write(JsonConvert.SerializeObject(rs, Formatting.Indented));
            Context.Response.Flush();
            Context.Response.End();
        }
        public class ResponseClass
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Code { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Status { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Message { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<tbl_Category> Category { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<ProductSQL> Product { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<tbl_ProductImage> ProductImage { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<tbl_ProductVariable> ProductVariable { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<tbl_ProductVariableValue> ProductVariableValue { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<StockManager> StockManager { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public tbl_Account User { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public tbl_Agent Agent { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int OrderID { get; set; }
        }
        public class ProductOrder
        {
            public int ProductType { get; set; }
            public int ID { get; set; }
            public string SKU { get; set; }
            public string ProductName { get; set; }
            public string ProductImageOrigin { get; set; }
            public string ProductVariableDescription { get; set; }
            public string ProductVariableName { get; set; }
            public string ProductVariableValue { get; set; }
            public double Quantity { get; set; }
            public double Price { get; set; }
        }

        public class StockManager
        {
            public string SKU { get; set; }
            public string ProductType { get; set; }
            public double Quantity { get; set; }
        }
    }
}
