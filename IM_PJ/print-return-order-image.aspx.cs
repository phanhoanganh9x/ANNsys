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
    public partial class print_return_order_image : System.Web.UI.Page
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
                LoadData();
            }
        }
        public void printItemList(ref int ID, ref double TotalQuantity, ref double TotalOrder, ref string Print, ref double TotalFee)
        {
            var orderdetails = RefundGoodDetailController.GetByRefundGoodsID(ID);
            if (orderdetails.Count > 0)
            {
                int t = 0;
                int print = 1;
                foreach (var item in orderdetails)
                {
                    TotalQuantity += Convert.ToDouble(item.Quantity);

                    int ProductType = Convert.ToInt32(item.ProductType);
                    double ItemPrice = Convert.ToDouble(item.GiavonPerProduct);
                    double SoldPrice = Convert.ToDouble(item.SoldPricePerProduct);
                    double RefundFee = Convert.ToDouble(item.RefundFeePerProduct);
                    string SKU = item.SKU;
                    string ProductName = "";
                    string ProductImage = "";
                    int SubTotal = (Convert.ToInt32(SoldPrice) - Convert.ToInt32(item.RefundFeePerProduct)) * Convert.ToInt32(item.Quantity);

                    t++;
                    Print += "<tr>";
                    Print += "<td>" + t + "</td>";

                    if (ProductType == 1)
                    {
                        var product = ProductController.GetBySKU(SKU);
                        if (product != null)
                        {
                            ProductName = product.ProductTitle;
                            if (!string.IsNullOrEmpty(product.ProductImage))
                            {
                                ProductImage = product.ProductImage;
                            }
                            Print += "<td><image src='" + Thumbnail.getURL(ProductImage, Thumbnail.Size.Large) + "' /></td> ";
                            Print += "<td><strong>" + SKU + "</strong> - " + ProductName + "</td> ";
                        }
                    }
                    else
                    {
                        var productvariable = ProductVariableController.GetBySKU(SKU);
                        if (productvariable != null)
                        {
                            var parent_product = ProductController.GetByID(Convert.ToInt32(productvariable.ProductID));
                            if (parent_product != null)
                            {
                                ProductName = parent_product.ProductTitle;

                                if (string.IsNullOrEmpty(productvariable.Image))
                                {
                                    ProductImage = parent_product.ProductImage;
                                }
                                else
                                {
                                    ProductImage = productvariable.Image;
                                }
                            }
                            
                            Print += "<td><image src='" + Thumbnail.getURL(ProductImage, Thumbnail.Size.Large) + "' /></td>";
                            Print += "<td><p><strong>" + SKU + "</strong> - " + ProductName + "</p></td> ";
                        }
                    }

                    Print += "<td>" + item.Quantity + "</td>";
                    Print += "<td>" + string.Format("{0:N0}", ItemPrice) + "</td>";
                    Print += "<td>" + string.Format("{0:N0}", SoldPrice) + "</td>";
                    Print += "<td>" + string.Format("{0:N0}", RefundFee) + "</td>";
                    Print += "<td>" + string.Format("{0:N0}", SubTotal) + "</td>";
                    Print += "</tr>";
                    TotalFee += Convert.ToInt32(item.RefundFeePerProduct) * Convert.ToInt32(item.Quantity);
                    TotalOrder += SubTotal;
                    
                    if(t % 10 == 0)
                    {
                        if(t == print * 10)
                        {
                            continue;
                        }
                        Print += "</tbody>";
                        Print += "</table>";
                        Print += "</div>";
                        Print += "</div>";
                        Print += "</div>";
                        Print += "</div>";

                        Print += "<div class=\"print-order-image\">";
                        Print += "<div class=\"all print print-" + print + "\">";
                        Print += "<div class=\"body\">";
                        Print += "<div class=\"table-2\">";
                        Print += "<table>";
                        Print += "<colgroup>";
                        Print += "<col class=\"order-item\" />";
                        Print += "<col class=\"image\" />";
                        Print += "<col class=\"name\" />";
                        Print += "<col class=\"quantity\" />";
                        Print += "<col class=\"price\" />";
                        Print += "<col class=\"sold-price\" />";
                        Print += "<col class=\"fee\" />";
                        Print += "<col class=\"subtotal\"/>";
                        Print += "</colgroup>";
                        Print += "<thead>";
                        Print += "<th>#</th>";
                        Print += "<th>Hình ảnh</th>";
                        Print += "<th>Sản phẩm</th>";
                        Print += "<th>SL</th>";
                        Print += "<th>Giá niêm yết</th>";
                        Print += "<th>Giá đã bán</th>";
                        Print += "<th>Phí đổi hàng</th>";
                        Print += "<th>Tổng</th>";
                        Print += "</thead>";
                        Print += "<tbody>";
                        print++;
                    }
                }
            }
        }
        public void LoadData()
        {
            int ID = Request.QueryString["id"].ToInt(0);
            
            if (ID > 0)
            {
                var order = RefundGoodController.GetByID(ID);
                
                if (order != null)
                {
                    string error = "";
                    string Print = "";

                    double TotalQuantity = 0;
                    double TotalOrder = 0;
                    double TotalFee = 0;
                    var orderdetails = RefundGoodDetailController.GetByRefundGoodsID(ID);

                    if (orderdetails.Count > 0)
                    {
                        printItemList(ref ID, ref TotalQuantity, ref TotalOrder, ref Print, ref TotalFee);

                        string productPrint = "";
                        string shtml = "";

                        productPrint += "<div class=\"body\">";
                        productPrint += "<div class=\"table-1\">";
                        productPrint += "<h1>ĐƠN ĐỔI TRẢ #" + order.ID + "</h1>";
                        productPrint += "<div class=\"note\">";
                        productPrint += "<p>- Miễn phí đổi size/màu hoặc đổi hàng lỗi cùng mẫu.</p>";
                        productPrint += "<p>- Tính phí 15.000đ/cái nếu đổi sang sản phẩm khác.</p>";
                        productPrint += "<p>- Giá đã bán nếu nhỏ hơn giá niêm yết là do có trừ chiết khấu.</p>";
                        productPrint += "<p>- Đơn đổi trả sẽ được trừ vào đơn mua hàng kế tiếp.</p>";
                        productPrint += "<p>- Chúng tôi chỉ hoàn tiền mặt nếu đơn đổi trả dưới 50.000đ.</p>";
                        productPrint += "<p>- Lưu ý, hình ảnh sản phẩm có thể hiển thị không đúng màu.</p>";
                        productPrint += "<p>- Nếu có sai sót, quý khách vui lòng thông báo cho nhân viên.</p>";
                        productPrint += "</div>";
                        productPrint += "<table>";
                        productPrint += "<colgroup >";
                        productPrint += "<col class=\"col-left\"/>";
                        productPrint += "<col class=\"col-right\"/>";
                        productPrint += "</colgroup>";
                        productPrint += "<tbody>";
                        productPrint += "<tr>";
                        productPrint += "<td>Khách hàng</td>";
                        productPrint += "<td class=\"customer-name\">" + order.CustomerName + "</td>";
                        productPrint += "</tr>";
                        productPrint += "<tr>";
                        productPrint += "<td>Điện thoại</td>";
                        productPrint += "<td>" + order.CustomerPhone + "</td>";
                        productPrint += "</tr>";
                        productPrint += "<tr>";
                        productPrint += "<td>Ngày tạo</td>";
                        string date = string.Format("{0:dd/MM/yyyy HH:mm}", order.CreatedDate);
                        productPrint += "<td>" + date + "</td>";
                        productPrint += "</tr>";
                        
                            
                        productPrint += "</tr>";
                        productPrint += "<tr>";
                        productPrint += "<td>Nhân viên</td>";
                        productPrint += "<td>" + order.CreatedBy + "</td>";
                        productPrint += "</tr>";

                        if (!string.IsNullOrEmpty(order.RefundNote)) {
                            productPrint += "<tr>";
                            productPrint += "<td>Ghi chú</td>";
                            productPrint += "<td>" + order.RefundNote + "</td>";
                            productPrint += "</tr>";
                        }

                        productPrint += "</tbody>";
                        productPrint += "</table>";
                        productPrint += "</div>";
                        productPrint += "<div class=\"table-2\">";
                        productPrint += "<table>";
                        productPrint += "<colgroup>";
                        productPrint += "<col class=\"order-item\" />";
                        productPrint += "<col class=\"image\" />";
                        productPrint += "<col class=\"name\" />";
                        productPrint += "<col class=\"quantity\" />";
                        productPrint += "<col class=\"price\" />";
                        productPrint += "<col class=\"sold-price\" />";
                        productPrint += "<col class=\"fee\" />";
                        productPrint += "<col class=\"subtotal\"/>";
                        productPrint += "</colgroup>";
                        productPrint += "<thead>";
                        productPrint += "<th>#</th>";
                        productPrint += "<th>Hình ảnh</th>";
                        productPrint += "<th>Sản phẩm</th>";
                        productPrint += "<th>SL</th>";
                        productPrint += "<th>Giá niêm yết</th>";
                        productPrint += "<th>Giá đã bán</th>";
                        productPrint += "<th>Phí đổi hàng</th>";
                        productPrint += "<th>Tổng</th>";
                        productPrint += "</thead>";
                        productPrint += "<tbody>";
                        productPrint += Print;
                        productPrint += "<tr>";
                        productPrint += "<td colspan=\"7\" class=\"align-right\">Số lượng</td>";
                        productPrint += "<td>" + TotalQuantity + "</td>";
                        productPrint += "</tr>";
                        productPrint += "<td colspan=\"7\" class=\"align-right\">Phí đổi hàng</td>";
                        productPrint += "<td>" + string.Format("{0:N0}", TotalFee) + "</td>";
                        productPrint += "</tr>";

                        double TotalPrice = TotalOrder;

                        if (TotalPrice != Convert.ToDouble(order.TotalPrice))
                        {
                            error += "Đơn hàng tính sai tổng tiền";
                        }

                        productPrint += "<tr>";
                        productPrint += "<td class=\"strong align-right\" colspan=\"7\">TỔNG TIỀN (Đã trừ phí)</td>";
                        productPrint += "<td class=\"strong\">" + string.Format("{0:N0}", TotalOrder) + "</td>";
                        productPrint += "</tr>";
                        
                        productPrint += "</tbody>";
                        productPrint += "</table>";
                        productPrint += "</div>";
                        productPrint += "</div>";

                        string dateOrder = string.Format("{0:dd/MM/yyyy HH:mm}", order.CreatedDate);

                        shtml += "<div class=\"print-order-image\">";
                        shtml += "<div class=\"all print print-0\">";
                        


                        shtml += "<div class=\"head\">";
                        string address = "";
                        string phone = "";
                        var agent = AgentController.GetByID(Convert.ToInt32(order.AgentID));
                        if (agent != null)
                        {
                            address = agent.AgentAddress;
                            phone = agent.AgentPhone;
                        }
                        shtml += "<div class=\"logo\"><img src=\"App_Themes/Ann/image/logo.png\" /></div>";
                        shtml += "<div class=\"info\">";

                        shtml += "<div class=\"ct\">";
                        shtml += "<div class=\"ct-title\"></div>";
                        shtml += "<div class=\"ct-detail\"> " + address + "</div>";
                        shtml += "</div>";

                        shtml += "<div class=\"ct\">";
                        shtml += "<div class=\"ct-title\"> </div>";
                        shtml += "<div class=\"ct-detail\"> " + phone + "</div>";
                        shtml += "</div>";

                        shtml += "</div>";
                        shtml += "</div>";

                        

                        shtml += productPrint;

                        shtml += "</div>";
                        shtml += "</div>";

                        if (error != "")
                        {
                            ltrPrintInvoice.Text = "Xảy ra lỗi: " + error;
                        }
                        else
                        {
                            ltrPrintInvoice.Text = shtml;
                        }
                    }
                }
                else
                {
                    ltrPrintInvoice.Text = "Không tìm thấy đơn hàng " + ID;
                }
            }
            else
            {
                ltrPrintInvoice.Text = "Xảy ra lỗi!!!";
            }
        }
    }
}