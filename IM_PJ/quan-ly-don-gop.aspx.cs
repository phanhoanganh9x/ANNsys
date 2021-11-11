using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Models.Pages.quan_ly_don_gop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class quan_ly_don_gop : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem_ANN123"] == null)
                    Response.Redirect("/dang-nhap");

                string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                var acc = AccountController.GetByUsername(username);

                if (acc == null)
                    Response.Redirect("/dang-nhap");

                if (acc.RoleID != 0)
                    Response.Redirect("/trang-chu");

                _initPage();
            }
        }

        #region Private
        private void _loadStatus()
        {
            #region Khởi tạo API
            var api = "http://ann-shop-dotnet-core.com/api/v1/order/statuses?page=quan-ly-don-gop";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(api);

            httpWebRequest.Method = "GET";
            #endregion

            try
            {
                // Thực thi API
                var response = (HttpWebResponse)httpWebRequest.GetResponse();

                ddlStatus.Items.Clear();
                ddlStatus.Items.Add(new ListItem("Trạng thái", "0"));

                if (response.StatusCode == HttpStatusCode.OK)
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var statuses = JsonConvert.DeserializeObject<IList<KeyValueModel>>(reader.ReadToEnd());
                        var listItems = statuses
                            .Select(x => new ListItem(x.value, x.key.ToString()))
                            .ToArray();

                        ddlStatus.Items.AddRange(listItems);
                    }

                ddlStatus.DataBind();
            }
            catch (WebException we)
            {
                throw we;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Cài đặt ban đầu với text tìm kiếm đơn hàng
        /// </summary>
        private void _initSearch()
        {
            var code = Request.QueryString["search"];

            if (!String.IsNullOrEmpty(code))
                txtSearch.Text = HttpUtility.UrlDecode(code);
        }

        private void _initOrderType()
        {
            var orderType = Request.QueryString["orderType"];

            if (!String.IsNullOrEmpty(orderType))
                ddlOrderType.SelectedValue = orderType;
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
                    query += String.Format("&fromDate={0:MM/dd/yyyy HH:mm}", fromDate);
                else
                    query += String.Format("?fromDate={0:MM/dd/yyyy HH:mm}", fromDate);
            }

            dpFromDate.SelectedDate = fromDate;
            dpFromDate.MinDate = dateConfig;
            dpFromDate.MaxDate = DateTime.Now;
            #endregion

            #region Cài đặt toDate
            var toDate = DateTime.Now;
            var queryToDate = Request.QueryString["toDate"];

            if (!String.IsNullOrEmpty(queryToDate))
                toDate = Convert.ToDateTime(queryToDate);
            else
                query += String.Format("&toDate={0:MM/dd/yyyy HH:mm}", toDate);

            dpToDate.SelectedDate = toDate;
            dpToDate.MinDate = dateConfig;
            dpToDate.MaxDate = DateTime.Now;
            #endregion

            if (String.IsNullOrEmpty(queryFromDate) || String.IsNullOrEmpty(queryToDate))
                Response.Redirect(url.AbsolutePath + query);
        }

        private void _initStatus()
        {
            var status = Request.QueryString["status"];

            if (!String.IsNullOrEmpty(status))
                ddlStatus.SelectedValue = status;
        }

        private void _initPage()
        {
            _loadStatus();

            _initSearch();
            _initDatetimePicker();
            _initStatus();
        }
        #endregion
    }
}