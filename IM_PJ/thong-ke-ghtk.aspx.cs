#region NetFramewrok
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
    public partial class thong_ke_ghtk : System.Web.UI.Page
    {
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
                        if (acc.RoleID != 0)
                            Response.Redirect("/trang-chu");
                        else
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
        /// Cài đặt ban đầu với drop down list tình trạng phí vận chuyển
        /// </summary>
        private void _initFeeStatus()
        {
            var feeStatus = Request.QueryString["feeStatus"];

            if (!String.IsNullOrEmpty(feeStatus))
                ddlFeeStatus.SelectedValue = feeStatus.ToString();
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

        /// <summary>
        /// Cài đặt ban đầu với drop down list tình trạng đơn hàng
        /// </summary>
        private void _initOrderStatus()
        {
            var orderStatus = Request.QueryString["orderStatus"];

            if (!String.IsNullOrEmpty(orderStatus))
                ddlOrderStatus.SelectedValue = orderStatus.ToString();
        }

        /// <summary>
        /// Cài đặt ban đầu với drop down list tình trạng đơn GHTK
        /// </summary>
        private void _initGhtkStatus()
        {
            var ghtkStatus = Request.QueryString["ghtkStatus"];

            if (!String.IsNullOrEmpty(ghtkStatus))
                ddlGhtkStatus.SelectedValue = ghtkStatus.ToString();
        }

        /// <summary>
        /// Cài đặt ban đầu với drop down list tình trạng duyệt
        /// </summary>
        private void _initReviewStatus()
        {
            var reviewStatus = Request.QueryString["reviewStatus"];

            if (!String.IsNullOrEmpty(reviewStatus))
                ddlReviewStatus.SelectedValue = reviewStatus.ToString();
        }

        private void _initPage()
        {
            _initSearch();
            _initFeeStatus();
            _initDatetimePicker();
            _initOrderStatus();
            _initGhtkStatus();
            _initReviewStatus();
        }
        #endregion
    }
}