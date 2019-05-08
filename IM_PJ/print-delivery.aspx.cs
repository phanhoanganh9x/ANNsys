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
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static IM_PJ.Controllers.DeliveryController;

namespace IM_PJ
{
    public partial class print_delivery : System.Web.UI.Page
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

        public void LoadData()
        {
            var shipperID = Request.QueryString.Get("shipperid").ToInt(0);
            var shippingType = Request.QueryString.Get("shippingtype").ToInt(0);

            if (shipperID == 0)
            {
                ltrPrintDelivery.Text = String.Format("ShipperID #{0} truyền vào có vấn đề", shipperID);
            }
            else if (shippingType == 0)
            {
                ltrPrintDelivery.Text = String.Format("ShippingType #{0} truyền vào có vấn đề", shippingType);
            }
            else
            {
                string username = Request.Cookies["userLoginSystem"].Value;
                var acc = AccountController.GetByUsername(username);

                if (shippingType == 4) // Báo cáo chuyển hàng tới nhà xe
                {
                    var orders = SessionController.getDeliverySession(acc)
                        .Where(x => x.ShippingType == shippingType)
                        .Select(x => x.OrderID)
                        .OrderBy(o => o)
                        .ToList();

                    var data = DeliveryController.getTransportReport(orders);
                    if (data.Count == 0)
                    {
                        ltrPrintDelivery.Text = "Không tìm thấy dữ liệu để xuất hóa đơn giao hàng";
                    }
                    else
                    {
                        ltrPrintDelivery.Text = getTransportReportHTML(data, shipperID);
                        ltrPrintEnable.Text = "<div class='print-enable true'></div>";

                        // Update Delivery
                        DeliveryController.udpateAfterPrint(shipperID, orders, acc.ID);
                    }
                }
                else if (shippingType == 5) // Báo cáo chuyển hàng của shipper
                {
                    var orders = SessionController.getDeliverySession(acc)
                        .Where(x => x.ShippingType == shippingType)
                        .Select(x => x.OrderID)
                        .OrderBy(o => o)
                        .ToList();

                    var data = DeliveryController.getShipperReport(orders);
                    if (data.Count == 0)
                    {
                        ltrPrintDelivery.Text = "Không tìm thấy dữ liệu để xuất hóa đơn giao hàng";
                    }
                    else
                    {
                        ltrPrintDelivery.Text = getShipperReportHTML(data, shipperID);
                        ltrPrintEnable.Text = "<div class='print-enable true'></div>";

                        // Update Delivery
                        DeliveryController.udpateAfterPrint(shipperID, orders, acc.ID);
                    }
                }
                else
                {
                    ltrPrintDelivery.Text = String.Format("Không có báo có vần chuyển nào với ShippintType #{0} này.", shippingType);
                }
            }
        }

        public string getTransportReportHTML(List<TransportReport> data, int shipperID)
        {
            var html = new StringBuilder();
            int index = 0;
            double totalQuantity = 0;
            double totalCollection = 0;
            string shipperName = ShipperController.getShipperNameByID(shipperID);

            html.AppendLine("<h1>PHIẾU GỬI HÀNG</h1>");
            
            html.AppendLine("<div class='delivery'>");
            html.AppendLine("    <div class='all'>");
            html.AppendLine("        <div class='body'>");
            html.AppendLine("            <div class='table-2'>");
            html.AppendLine("               <div class='info'>");
            html.AppendLine(String.Format(" <p>Ngày giao: {0}</p>", string.Format("{0:dd/MM HH:mm}", DateTime.Now)));
            html.AppendLine(String.Format(" <p>Người giao: {0}</p>", shipperName));
            html.AppendLine("               </div>");
            html.AppendLine("                <table>");
            html.AppendLine("                    <colgroup>");
            html.AppendLine("                        <col />");
            html.AppendLine("                        <col />");
            html.AppendLine("                        <col />");
            html.AppendLine("                    </colgroup>");
            html.AppendLine("                    <thead>");
            html.AppendLine("                        <th>Nhà xe</th>");
            html.AppendLine("                        <th>SL</th>");
            html.AppendLine("                        <th>Thu hộ</th>");
            html.AppendLine("                    </thead>");
            html.AppendLine("                    <tbody>");
            foreach(var item in data)
            {
                index += 1;
                totalQuantity += item.Quantity;
                totalCollection += item.Collection;

                html.AppendLine("                        <tr>");
                html.AppendLine(String.Format("                            <td><strong>{0}</strong></td>", item.TransportName));
                html.AppendLine(String.Format("                            <td>{0:#,###}</td>", item.Quantity));
                html.AppendLine(String.Format("                            <td>{0:#,###}</td>", item.Collection));
                html.AppendLine("                        </tr>");
            }
            html.AppendLine("                        <tr>");
            html.AppendLine("                            <td colspan='1' style='text-align: right'>Tổng số đơn</td>");
            html.AppendLine(String.Format("                            <td colspan='2'>{0:#,###}</td>", totalQuantity));
            html.AppendLine("                        </tr>");
            if(totalCollection > 0)
            {
                html.AppendLine("                        <tr>");
                html.AppendLine("                            <td colspan='1'  style='text-align: right'>Thu hộ</td>");
                html.AppendLine(String.Format("                            <td colspan='2'>{0:#,###}</td>", totalCollection));
                html.AppendLine("                        </tr>");
            }
            html.AppendLine("                    </tbody>");
            html.AppendLine("                </table>");
            html.AppendLine("            </div>");
            html.AppendLine("        </div>");
            html.AppendLine("    </div>");
            html.AppendLine("</div>");

            return html.ToString();
        }

        public string getShipperReportHTML(List<ShipperReport> data, int shipperID)
        {
            var html = new StringBuilder();
            decimal totalPayment = 0;
            decimal totalMoneyCollection = 0;
            decimal totalPrice = 0;
            string shipperName = ShipperController.getShipperNameByID(shipperID);

            html.AppendLine("<h1>PHIẾU GỬI HÀNG</h1>");

            html.AppendLine("<div class='delivery'>");
            html.AppendLine("    <div class='all'>");
            html.AppendLine("        <div class='body'>");
            html.AppendLine("            <div class='table-2'>");
            html.AppendLine("               <div class='info'>");
            html.AppendLine(String.Format(" <p>Ngày giao: {0}</p>", string.Format("{0:dd/MM HH:mm}", DateTime.Now)));
            html.AppendLine(String.Format(" <p>Người giao: {0}</p>", shipperName));
            html.AppendLine("               </div>");
            html.AppendLine("                <table>");
            html.AppendLine("                    <colgroup>");
            html.AppendLine("                        <col />");
            html.AppendLine("                        <col />");
            html.AppendLine("                        <col />");
            html.AppendLine("                        <col />");
            html.AppendLine("                        <col />");
            html.AppendLine("                    </colgroup>");
            html.AppendLine("                    <thead>");
            html.AppendLine("                        <th>Mã</th>");
            html.AppendLine("                        <th>Tên khách kàng</th>");
            html.AppendLine("                        <th>Số tiền</th>");
            html.AppendLine("                        <th>Thu hộ</th>");
            html.AppendLine("                        <th>Phí</th>");
            html.AppendLine("                    </thead>");
            html.AppendLine("                    <tbody>");
            foreach (var item in data)
            {
                totalPayment += item.Payment;
                totalMoneyCollection += item.MoneyCollection;
                totalPrice += item.Price;

                html.AppendLine("                        <tr>");
                html.AppendLine(String.Format("                            <td>{0}</td>", item.OrderID));
                html.AppendLine(String.Format("                            <td><strong>{0}</strong></td>", item.CustomerName));
                html.AppendLine(String.Format("                            <td>{0:#,###}</td>", item.Payment));
                html.AppendLine(String.Format("                            <td>{0:#,###}</td>", item.MoneyCollection));
                html.AppendLine(String.Format("                            <td>{0:#,###}</td>", item.Price));
                html.AppendLine("                        </tr>");
            }
            html.AppendLine("                        <tr>");
            html.AppendLine("                            <td colspan='2' style='text-align: right'>Tổng đơn</td>");
            html.AppendLine(String.Format("                            <td colspan='3'>{0:#,###}</td>", totalPayment));
            html.AppendLine("                        </tr>");
            if (totalMoneyCollection > 0)
            {
                html.AppendLine("                        <tr>");
                html.AppendLine("                            <td colspan='2'  style='text-align: right'>Tổng thu hộ</td>");
                html.AppendLine(String.Format("                            <td colspan='3'>{0:#,###}</td>", totalMoneyCollection));
                html.AppendLine("                        </tr>");
            }
            if (totalPrice > 0)
            {
                html.AppendLine("                        <tr>");
                html.AppendLine("                            <td colspan='2'  style='text-align: right'>Tổng phí</td>");
                html.AppendLine(String.Format("                            <td colspan='3'>{0:#,###}</td>", totalPrice));
                html.AppendLine("                        </tr>");
            }
            html.AppendLine("                    </tbody>");
            html.AppendLine("                </table>");
            html.AppendLine("            </div>");
            html.AppendLine("        </div>");
            html.AppendLine("    </div>");
            html.AppendLine("</div>");

            return html.ToString();
        }

    }
}