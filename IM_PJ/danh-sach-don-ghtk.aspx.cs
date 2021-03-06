﻿#region NetFramewrok
using System;
using System.Web;
#endregion

#region ANN Shop
using MB.Extensions;

// Controllers
using IM_PJ.Controllers;
#endregion

namespace IM_PJ
{
    public partial class danh_sach_don_ghtk : System.Web.UI.Page
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
                        hdfStaff.Value = acc.Username;
                        _initPage();
                    }
                    else
                        Response.Redirect("/dang-nhap");
                }
                else
                    Response.Redirect("/dang-nhap");
            }
        }

        #region Private
        /// <summary>
        /// Cài đặt ban đầu với text tìm kiếm đơn hàng
        /// </summary>
        private void _initSearch()
        {
            var search = Request.QueryString["search"];

            if (!String.IsNullOrEmpty(search))
                txtSearch.Text = HttpUtility.UrlDecode(search);
        }

        /// <summary>
        /// Cài đặt ban đầu với datetime picker về khoảng thời gian
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


            dpFromDate.SelectedDate = fromDate;
            dpFromDate.MinDate = dateConfig;
            dpFromDate.MaxDate = DateTime.Now;
            #endregion

            #region Cài đặt toDate
            var toDate = DateTime.Now;
            var queryToDate = Request.QueryString["toDate"];

            if (!String.IsNullOrEmpty(queryToDate))
                toDate = Convert.ToDateTime(queryToDate).AddDays(1).AddMinutes(-1);
            else
                query += String.Format("&toDate={0:MM/dd/yyyy}", toDate);

            dpToDate.SelectedDate = toDate;
            dpToDate.MinDate = dateConfig;
            dpToDate.MaxDate = DateTime.Now;
            #endregion

            if (String.IsNullOrEmpty(queryFromDate) || String.IsNullOrEmpty(queryToDate))
                Response.Redirect(url.AbsolutePath + query);
        }


        private void _initPage()
        {
            _initSearch();
            _initDatetimePicker();
        }
        #endregion
    }
}