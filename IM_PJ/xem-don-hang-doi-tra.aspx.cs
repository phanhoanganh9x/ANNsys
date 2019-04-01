using IM_PJ.Controllers;
using IM_PJ.Models;
using MB.Extensions;
using Newtonsoft.Json;
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
using static IM_PJ.Controllers.DiscountCustomerController;

namespace IM_PJ
{
    public partial class xem_don_hang_doi_tra : System.Web.UI.Page
    {
        private static RefundGoodModel _refundGood;

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
                        if (_refundGood == null)
                        {
                            _refundGood = new RefundGoodModel();
                            _refundGood.CreateBy = acc.Username;
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
            string username = Request.Cookies["userLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {

                int AgentID = Convert.ToInt32(acc.AgentID);
                int ID = Request.QueryString["id"].ToInt(0);
                if (ID > 0)
                {
                    ViewState["ID"] = ID;
                    var r = RefundGoodController.GetByIDAndAgentID(ID, AgentID);
                    if (r == null)
                    {
                        PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy đơn hàng " + ID, "e", true, "/danh-sach-don-tra-hang", Page);
                    }
                    else
                    {
                        if (acc.RoleID != 0)
                        {
                            if (r.CreatedBy != acc.Username)
                            {
                                PJUtils.ShowMessageBoxSwAlertError("Đơn hàng này không phải của bạn", "e", true, "/danh-sach-don-tra-hang", Page);
                            }
                        }
                        ltrOrderID.Text = ID.ToString();
                        ltrCreateBy.Text = r.CreatedBy;
                        ltrCreateDate.Text = r.CreatedDate.ToString();
                        ltrOrderStatus.Text = PJUtils.RefundStatus(Convert.ToInt32(r.Status));
                        if(r.OrderSaleID > 0)
                        {
                            ltrOrderSaleID.Text = "<td><a class=\"customer-name-link\" target=\"_blank\" title=\"Bấm vào xem đơn hàng trừ tiền\" href=\"/thong-tin-don-hang?id=" + r.OrderSaleID + "\">" + r.OrderSaleID + " (Xem đơn)</a>";
                        }
                        else
                        {
                            ltrOrderSaleID.Text = "";
                        }
                        ltrOrderQuantity.Text = r.TotalQuantity.ToString();
                        ltrOrderTotalPrice.Text = string.Format("{0:N0}", (Convert.ToDouble(r.TotalPrice)));
                        ltrTotalRefundFee.Text = string.Format("{0:N0}", (Convert.ToDouble(r.TotalRefundFee)));
                        ltrRefundNote.Text = r.RefundNote;

                        ltrInfo.Text += "<div class=\"row\">";
                        ltrInfo.Text += "<div class=\"col-md-6\">";
                        ltrInfo.Text += "<div class=\"form-group\">";
                        ltrInfo.Text += "<label>Họ tên</label>";
                        ltrInfo.Text += "<span class=\"form-control input-disabled\">" + r.CustomerName + "</span>";
                        ltrInfo.Text += "</div>";
                        ltrInfo.Text += "</div>";
                        ltrInfo.Text += "<div class=\"col-md-6\">";
                        ltrInfo.Text += "<div class=\"form-group\">";
                        ltrInfo.Text += "<label>Điện thoại</label>";
                        ltrInfo.Text += "<span class=\"form-control input-disabled\">" + r.CustomerPhone + "</span>";
                        ltrInfo.Text += "</div>";
                        ltrInfo.Text += "</div>";
                        ltrInfo.Text += "</div>";

                        var cus = CustomerController.GetByID(r.CustomerID.Value);
                        if (cus != null)
                        {
                            ltrInfo.Text += "<div class=\"row\">";
                            ltrInfo.Text += "<div class=\"col-md-6\">";
                            ltrInfo.Text += "<div class=\"form-group\">";
                            ltrInfo.Text += "<label>Nick đặt hàng</label>";
                            ltrInfo.Text += "<span class=\"form-control input-disabled\">" + cus.Nick + "</span>";
                            ltrInfo.Text += "</div>";
                            ltrInfo.Text += "</div>";
                            ltrInfo.Text += "<div class=\"col-md-6\">";
                            ltrInfo.Text += "<div class=\"form-group\">";
                            ltrInfo.Text += "<label>Địa chỉ</label>";
                            ltrInfo.Text += "<span class=\"form-control input-disabled\">" + cus.CustomerAddress + "</span>";
                            ltrInfo.Text += "</div>";
                            ltrInfo.Text += "</div>";
                            ltrInfo.Text += "</div>";

                            ltrInfo.Text += "<div class=\"row\">";
                            ltrInfo.Text += "<div class=\"col-md-6\">";
                            ltrInfo.Text += "<div class=\"form-group\">";
                            ltrInfo.Text += "<label>Zalo</label>";
                            ltrInfo.Text += "<span class=\"form-control input-disabled\">" + cus.Zalo + "</span>";
                            ltrInfo.Text += "</div>";
                            ltrInfo.Text += "</div>";
                            ltrInfo.Text += "<div class=\"col-md-6\">";
                            ltrInfo.Text += "<div class=\"form-group\">";
                            ltrInfo.Text += "<label>Facebook</label>";
                            ltrInfo.Text += "<span class=\"form-control input-disabled\">" + cus.Facebook + "</span>";
                            ltrInfo.Text += "</div>";
                            ltrInfo.Text += "</div>";
                            ltrInfo.Text += "</div>";
                            ltrInfo.Text += "<div class=\"form-row view-detail\">";
                            ltrInfo.Text += "    <a href=\"javascript:;\" class=\"btn primary-btn fw-btn not-fullwidth\" onclick=\"viewCustomerDetail('" + cus.ID + "')\"><i class=\"fa fa-address-card-o\" aria-hidden=\"true\"></i> Xem chi tiết</a>";
                            ltrInfo.Text += "</div>";

                            var discount = DiscountCustomerController.getbyCustID(cus.ID).FirstOrDefault();
                            if (discount != null)
                            {

                                ltrInfo.Text += "<div class='form-row discount-info'>";
                                ltrInfo.Text += String.Format("<strong>* Chiết khấu của khách: {0:0,0}đ/cái.</strong>", discount.DiscountAmount);
                                ltrInfo.Text += "</div>";
                                ltrInfo.Text += "<div class='form-row refund-info'>";
                                if (discount.FeeRefund == 0)
                                {
                                    ltrInfo.Text += "<strong>* Miễn phí đổi hàng</strong>";
                                }
                                else
                                {
                                    ltrInfo.Text += String.Format("<strong>* Phí đổi hàng của khách: {0:0,0}đ/cái.</strong>", discount.FeeRefund);
                                }
                                ltrInfo.Text += "</div>";
                            }
                        }

                        ltrTotal.Text = string.Format("{0:N0}", Convert.ToDouble(r.TotalPrice));
                        ltrQuantity.Text = string.Format("{0:N0}", Convert.ToDouble(r.TotalQuantity));
                        ltrRefund.Text = string.Format("{0:N0}", Convert.ToDouble(r.TotalRefundFee));

                        // get info tranfor page tao-don-hang-doi-tra.aspx
                        _refundGood.RefundGoodsID = ID;
                        _refundGood.CustomerID = r.CustomerID.Value;
                        _refundGood.CustomerName = r.CustomerName;
                        _refundGood.CustomerPhone = r.CustomerPhone;
                        _refundGood.CustomerNick = cus != null? cus.Nick : String.Empty;
                        _refundGood.CustomerAddress = cus != null ? cus.CustomerAddress : String.Empty;
                        _refundGood.CustomerZalo = cus != null ? cus.Zalo : String.Empty;
                        _refundGood.CustomerFacebook = cus != null ? cus.Facebook : String.Empty;
                        _refundGood.RefundDetails = RefundGoodDetailController.GetInfoShowRefundDetail(ID);
                        _refundGood.TotalPrice = Convert.ToDouble(r.TotalPrice);
                        _refundGood.TotalQuantity = Convert.ToDouble(r.TotalQuantity);
                        _refundGood.TotalFreeRefund = Convert.ToDouble(r.TotalRefundFee);
                        _refundGood.Status = r.Status.Value;
                        _refundGood.Note = txtRefundsNote.Text;
                        _refundGood.CreateBy = r.CreatedBy;

                        var rds = RefundGoodDetailController.GetByRefundGoodsID(ID);

                        var product = _refundGood.RefundDetails
                            .Join(
                                rds,
                                p1 => new {
                                    RefundGoodsID = p1.RefundGoodsID,
                                    RefundDetailID = p1.RefundDetailID
                                },
                                p2 => new {
                                    RefundGoodsID = p2.RefundGoodsID.Value,
                                    RefundDetailID = p2.ID },
                                (p1, p2) => new { p1, p2 })
                            .Select(x => new {
                                SKU = x.p2.SKU,
                                OrderID = x.p2.OrderID,
                                ProductName = x.p2.ProductName,
                                ProductType = x.p2.ProductType,
                                GiavonPerProduct = x.p2.GiavonPerProduct,
                                SoldPricePerProduct = x.p2.SoldPricePerProduct,
                                DiscountPricePerProduct = x.p2.DiscountPricePerProduct,
                                Quantity = x.p2.Quantity,
                                QuantityMax = x.p2.QuantityMax,
                                RefundType = x.p2.RefundType,
                                RefundFeePerProduct = x.p2.RefundFeePerProduct,
                                TotalPriceRow = x.p2.TotalPriceRow,
                                ProductImage = x.p1.ProductImage
                            })
                            .ToList();

                        if (product.Count > 0)
                        {
                            string html = "";
                            int t = 0;
                            foreach (var item in product)
                            {
                                
                                var variables = ProductVariableValueController.GetByProductVariableSKU(item.SKU);
                                string variable = "";
                                if (variables.Count > 0)
                                {
                                    variable += "<br><br>";
                                    foreach (var v in variables)
                                    {
                                        variable += v.VariableName.Trim() + ": " + v.VariableValue.Trim() + "<br>";
                                    }
                                }
                                t++;
                                html += "<tr ondblclick=\"clickrow($(this))\" class=\"product-result\" data-sku=\"" + item.SKU + "\" data-orderID=\"" + item.OrderID
                                                    + "\" data-ProductName=\"" + item.ProductName
                                                    + "\" data-ProductType=\"" + item.ProductType + "\" data-Giagoc=\"" + item.GiavonPerProduct
                                                    + "\" data-Giadaban=\"" + item.SoldPricePerProduct
                                                    + "\" data-TienGiam=\"" + item.DiscountPricePerProduct
                                                    + "\" data-Soluongtoida=\"" + item.QuantityMax + "\" data-RefundFee=\"" + item.RefundFeePerProduct + "\"  >";
                                html += "   <td>" + t + "</td>";
                                html += "   <td><img src='" + item.ProductImage + "'></td>";
                                html += "   <td>" + item.ProductName + variable + "</td>";
                                html += "   <td>" + item.SKU + "</td>";
                                html += "   <td class=\"giagoc\" data-giagoc=\"" + item.GiavonPerProduct + "\">" + string.Format("{0:N0}", Convert.ToDouble(item.GiavonPerProduct)) + "</td>";
                                html += "   <td class=\"giadaban\" data-giadaban=\"" + item.SoldPricePerProduct + "\"><strong>" + string.Format("{0:N0}", Convert.ToDouble(item.SoldPricePerProduct)) + "</strong><br>(CK: " + string.Format("{0:N0}", Convert.ToDouble(item.DiscountPricePerProduct)) + ")</td>";
                                html += "   <td class=\"slcandoi\">" + item.Quantity + "</td>";
                                html += "   <td>";
                                int refundType = Convert.ToInt32(item.RefundType);
                                string refuntTypeName = "";
                                if (item.RefundType == 1)
                                    refuntTypeName = "Đổi size";
                                else if (item.RefundType == 2)
                                    refuntTypeName = "Đổi sản phẩm khác";
                                else
                                    refuntTypeName = "Đổi hàng lỗi";
                                html += refuntTypeName;
                                html += "    </td>";
                                html += "   <td class=\"phidoihang\">" + string.Format("{0:N0}", Convert.ToDouble(item.RefundFeePerProduct)) + "</td>";
                                html += "   <td class=\"thanhtien\">" + string.Format("{0:N0}", Convert.ToDouble(item.TotalPriceRow)) + "</td>";
                                html += "</tr>";

                            }

                            ddlRefundStatus.SelectedValue = r.Status.ToString();
                            txtRefundsNote.Text = r.RefundNote;
                            ltrList.Text = html;
                        }
                        ltrPrint.Text = "<a href=\"/print-invoice-return?id=" + ID + "\" target=\"_blank\" class=\"btn primary-btn fw-btn not-fullwidth\"><i class=\"fa fa-print\" aria-hidden=\"true\"></i> In hóa đơn</a>";
                        ltrPrint.Text += "<a href=\"/print-return-order-image?id=" + ID + "\" target=\"_blank\" class=\"btn primary-btn fw-btn not-fullwidth print-invoice-merged\"><i class=\"fa fa-picture-o\" aria-hidden=\"true\"></i> Lấy ảnh đơn hàng</a>";
                    }
                }
            }

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            string username = Request.Cookies["userLoginSystem"].Value;
            var a = AccountController.GetByUsername(username);
            if (a != null)
            {
                var re = RefundGoodController.updatestatus((ViewState["ID"].ToString()).ToInt(), Convert.ToInt32(ddlRefundStatus.SelectedValue), DateTime.Now, a.Username,txtRefundsNote.Text);
                if(re != null)
                {
                    PJUtils.ShowMessageBoxSwAlert("Cập nhật đơn hàng đổi trả thành công", "s", true, Page);
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Thất bại", "e", false, Page);
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                RefundGoodDetailController.DeleteByRefundGoodsID(_refundGood.RefundGoodsID);
                RefundGoodController.DeleteByID(_refundGood.RefundGoodsID);
                int OrderSaleID = OrderController.DeleteOrderRefund(_refundGood.RefundGoodsID);
                if(OrderSaleID > 0)
                {
                    _refundGood.OrderSaleID = OrderSaleID;
                }

                foreach (var product in _refundGood.RefundDetails)
                {
                    if (!string.IsNullOrEmpty(product.ChildSKU))
                    {
                        var ProductVariable = ProductVariableValueController.GetByProductVariableSKU(product.ChildSKU);
                        string VariableValue = "";
                        if (ProductVariable.Count > 0)
                        {
                            foreach (var v in ProductVariable)
                            {
                                VariableValue += v.VariableName.Trim() + ": " + v.VariableValue.Trim() + "|";
                            }
                        }
                        product.VariableValue = VariableValue;
                    }
                    
                    if (product.ChangeType != 3) // Change product error
                    {
                        StockManagerController.Insert(new tbl_StockManager()
                        {
                            AgentID = 1,
                            ProductID = product.ProductStyle == 1 ? product.ProductID : 0,
                            ProductVariableID = product.ProductVariableID,
                            Quantity = product.QuantityRefund,
                            QuantityCurrent = 0,
                            Type = 1,
                            NoteID = "Xuất kho do làm lại đơn hàng đổi trả",
                            OrderID = product.OrderID,
                            Status = 11,
                            SKU = product.ProductStyle == 1 ? product.ParentSKU : product.ChildSKU,
                            CreatedDate = DateTime.Now,
                            CreatedBy = _refundGood.CreateBy,
                            MoveProID = 0,
                            ParentID = product.ProductID,
                        });
                    }
                }

                HttpContext.Current.Items.Add("xem-don-hang-doi-tra", JsonConvert.SerializeObject(_refundGood));
                Server.Transfer("tao-don-hang-doi-tra.aspx");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}