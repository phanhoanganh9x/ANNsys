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
using System.Web.Services;
using System.Web.UI.WebControls;
using static IM_PJ.Controllers.OrderController;

namespace IM_PJ
{
    public partial class danh_sach_don_dat_hang : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem"] != null)
                {
                    var username = Request.Cookies["usernameLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);

                    if (acc != null)
                    {
                        if (acc.RoleID == 0 || acc.RoleID == 2)
                            _initPage(acc);
                        else
                            Response.Redirect("/trang-chu");
                    }
                    else
                        Response.Redirect("/dang-nhap");
                }
                else
                    Response.Redirect("/dang-nhap");
            }
        }

        #region Private
        private void _initSearch()
        {
            var search = Request.QueryString["search"];

            if (!String.IsNullOrEmpty(search))
            {
                txtSearchOrder.Text = HttpUtility.UrlDecode(search);

                var searchType = Request.QueryString["searchType"];

                if (!String.IsNullOrEmpty(searchType))
                    for (int i = 0; i < ddlSearchType.Items.Count; i++)
                        if (ddlSearchType.Items[i].Value == searchType)
                            ddlSearchType.SelectedIndex = i;
            }
        }

        /// <summary>
        /// Cài đặt bộ lọc trạng thái đơn hàng
        /// </summary>
        private void _initOrderStatus()
        {
            var orderStatus = Request.QueryString["orderStatus"];

            if (!String.IsNullOrEmpty(orderStatus))
            {
                for (int i = 0; i < ddlExcuteStatus.Items.Count; i++)
                    if (ddlExcuteStatus.Items[i].Value == orderStatus)
                        ddlExcuteStatus.SelectedIndex = i;
            }
            else
            {
                var url = HttpContext.Current.Request.Url;
                var query = url.Query;

                if (!String.IsNullOrEmpty(query))
                    query += "&orderStatus=0";
                else
                    query += "?orderStatus=0";

                Response.Redirect(url.AbsolutePath + query);
            }
        }

        /// <summary>
        /// Cài đặt bộ lọc triết khấu
        /// </summary>
        private void _initDiscount()
        {
            var hasDiscount = Request.QueryString["hasDiscount"];

            if (!String.IsNullOrEmpty(hasDiscount))
                for (int i = 0; i < ddlDiscount.Items.Count; i++)
                    if (ddlDiscount.Items[i].Value == hasDiscount)
                        ddlDiscount.SelectedIndex = i;
        }

        /// <summary>
        /// Cài đặt datetime picker fromDate và toDate
        /// </summary>
        private void _initDatetimePicker()
        {
            var url = HttpContext.Current.Request.Url;
            var query = url.Query;

            // ẩn sản phẩm theo thời gian
            var dateConfig = new DateTime(2019, 12, 15);
            var config = ConfigController.GetByTop1();

            if (config.ViewAllOrders == 1)
                dateConfig = new DateTime(2018, 6, 22);
            else if (config.ViewAllReports == 0)
                dateConfig = DateTime.Now.AddMonths(-2);

            #region Cài đặt fromDate
            var fromDate = dateConfig;
            var queryFromDate = Request.QueryString["fromDate"];

            if (!String.IsNullOrEmpty(queryFromDate))
                fromDate = Convert.ToDateTime(queryFromDate);
            else
            {
                if (!String.IsNullOrEmpty(query))
                    query += String.Format("&fromDate={0:MM/dd/yyyy}", fromDate);
                else
                    query += String.Format("?fromDate={0:MM/dd/yyyy}", fromDate);
            }


            rOrderFromDate.SelectedDate = fromDate;
            rOrderFromDate.MinDate = dateConfig;
            rOrderFromDate.MaxDate = DateTime.Now;
            #endregion

            #region Cài đặt toDate
            var toDate = DateTime.Now;
            var queryToDate = Request.QueryString["toDate"];

            if (!String.IsNullOrEmpty(queryToDate))
                toDate = Convert.ToDateTime(queryToDate).AddDays(1).AddMinutes(-1);
            else
                query += String.Format("&toDate={0:MM/dd/yyyy}", toDate);

            rOrderToDate.SelectedDate = toDate;
            rOrderToDate.MinDate = dateConfig;
            rOrderToDate.MaxDate = DateTime.Now;
            #endregion

            if (String.IsNullOrEmpty(queryFromDate) || String.IsNullOrEmpty(queryToDate))
                Response.Redirect(url.AbsolutePath + query);
        }

        /// <summary>
        /// Cài đặt bộ lọc khuyến mãi
        /// </summary>
        private void _initCoupon()
        {
            var hasCoupon = Request.QueryString["hasCoupon"];

            if (!String.IsNullOrEmpty(hasCoupon))
                for (int i = 0; i < ddlCouponStatus.Items.Count; i++)
                    if (ddlCouponStatus.Items[i].Value == hasCoupon)
                        ddlCouponStatus.SelectedIndex = i;
        }

        /// <summary>
        /// Cài đặt bộ lọc hình thức thanh toán
        /// </summary>
        private void _initPaymentMethod()
        {
            var paymentMethod = Request.QueryString["paymentMethod"];

            if (!String.IsNullOrEmpty(paymentMethod))
                ddlPaymentType.SelectedIndex = Convert.ToInt32(paymentMethod);
        }

        /// <summary>
        /// Cài đặt bộ lọc hình thức nhận hàng
        /// </summary>
        private void _initDeliveryMethod()
        {
            var deliveryMethod = Request.QueryString["deliveryMethod"];

            if (!String.IsNullOrEmpty(deliveryMethod))
                for (int i = 0; i < ddlShippingType.Items.Count; i++)
                    if (ddlShippingType.Items[i].Value == deliveryMethod)
                        ddlShippingType.SelectedIndex = i;
        }

        /// <summary>
        /// Cài đặt bộ lọc hình thức nhận hàng
        /// </summary>
        private void _initQuantity()
        {
            var quantityFilter = Request.QueryString["quantityFilter"];

            if (!String.IsNullOrEmpty(quantityFilter))
            {
                if (quantityFilter == "1" || quantityFilter == "2")
                {
                    var quantity = Request.QueryString["quantity"];

                    if (!String.IsNullOrEmpty(quantity))
                    {
                        txtQuantity.Text = quantity;

                        for (int i = 0; i < ddlQuantityFilter.Items.Count; i++)
                            if (ddlQuantityFilter.Items[i].Value == quantityFilter)
                                ddlQuantityFilter.SelectedIndex = i;
                    }
                }
                else if (Convert.ToInt32(quantityFilter) == 3)
                {
                    var minQuantity = Request.QueryString["minQuantity"];

                    if (!String.IsNullOrEmpty(minQuantity))
                        txtQuantityMin.Text = Convert.ToInt32(minQuantity).ToString();

                    var maxQuantity = Request.QueryString["maxQuantity"];

                    if (!String.IsNullOrEmpty(maxQuantity))
                        txtQuantityMax.Text = Convert.ToInt32(maxQuantity).ToString();

                    if (!String.IsNullOrEmpty(minQuantity) || !String.IsNullOrEmpty(maxQuantity))
                        for (int i = 0; i < ddlQuantityFilter.Items.Count; i++)
                            if (ddlQuantityFilter.Items[i].Value == quantityFilter)
                                ddlQuantityFilter.SelectedIndex = i;
                }
            }
        }

        /// <summary>
        /// Cài đặt bộ lọc nhân viên phụ trách
        /// </summary>
        /// <param name="acc"></param>
        private void _initCreatedBy(tbl_Account acc = null)
        {
            hdRole.Value = acc == null ? "0" : acc.RoleID.ToString();

            if (acc != null)
            {
                ddlCreatedBy.Enabled = false;
                ddlCreatedBy.Items.Clear();
                ddlCreatedBy.Items.Insert(0, new ListItem(acc.Username, acc.Username));

                
                var staff = Request.QueryString["staff"];

                if (String.IsNullOrEmpty(staff))
                {
                    var url = HttpContext.Current.Request.Url;
                    var query = url.Query;

                    if (!String.IsNullOrEmpty(query))
                        query += "&staff=" + acc.Username;
                    else
                        query += "?staff=0" + acc.Username;

                    Response.Redirect(url.AbsolutePath + query);
                }
                else if (staff != acc.Username)
                {
                    var url = HttpContext.Current.Request.Url;
                    var query = url.Query;

                    query = query.Replace("staff=" + staff, "staff=" + acc.Username);
                    Response.Redirect(url.AbsolutePath + query);
                }
            }
            else
            {
                var items = AccountController
                    .GetAllNotSearch()
                    .Where(x => x.RoleID == 0 || x.RoleID == 2)
                    .Select(x => new ListItem(x.Username, x.Username))
                    .ToArray();

                ddlCreatedBy.Enabled = true;
                ddlCreatedBy.Items.Clear();
                ddlCreatedBy.Items.Insert(0, new ListItem("Nhân viên tạo đơn", ""));
                ddlCreatedBy.Items.AddRange(items);
                ddlCreatedBy.DataBind();


                var staff = Request.QueryString["staff"];
                if (!String.IsNullOrEmpty(staff))
                    for (int i = 0; i < ddlCreatedBy.Items.Count; i++)
                        if (ddlCreatedBy.Items[i].Value == staff)
                            ddlCreatedBy.SelectedIndex = i;
            }
        }

        private void _initPage(tbl_Account acc)
        {
            _initSearch();
            _initDatetimePicker();
            _initOrderStatus();
            _initDiscount();
            _initCoupon();
            _initPaymentMethod();
            _initDeliveryMethod();
            _initQuantity();
            _initCreatedBy(acc.RoleID == 2 ? acc : null);
        }
        #endregion
    }
}
