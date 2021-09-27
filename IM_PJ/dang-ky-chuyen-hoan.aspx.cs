using IM_PJ.Controllers;
using IM_PJ.Models.Pages.dang_ky_chuyen_hoan;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class dang_ky_chuyen_hoan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var userName = Request.Cookies["usernameLoginSystem_ANN123"] != null
                    ? Request.Cookies["usernameLoginSystem_ANN123"].Value
                    : null;

                if (!String.IsNullOrEmpty(userName))
                {
                    var acc = AccountController.GetByUsername(userName);

                    if (acc == null)
                        Response.Redirect("/dang-nhap");
                    else
                        hdfStaff.Value = userName;
                }
                else
                    Response.Redirect("/dang-nhap");

                _loadData();
            }
        }

        #region Private
        private void _loadOrderType()
        {
            #region Khởi tạo API
            var api = "http://ann-shop-dotnet-core.com/api/v1/order/types";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(api);

            httpWebRequest.Method = "GET";
            #endregion

            try
            {
                // Thực thi API
                var response = (HttpWebResponse)httpWebRequest.GetResponse();

                ddlOrderType.Items.Clear();


                if (response.StatusCode == HttpStatusCode.OK)
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var orderTypes = JsonConvert.DeserializeObject<IList<KeyValueModel>>(reader.ReadToEnd());
                        var listItems = orderTypes
                            .Select(x => new ListItem(x.value, x.key.ToString()))
                            .ToArray();

                        ddlOrderType.Items.AddRange(listItems);
                        ddlOrderType.DataBind();
                    }
                else
                {
                    ddlOrderType.Items.Add(new ListItem("Loại đơn hàng", "0"));
                    ddlOrderType.DataBind();
                }
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

        private void _loadData()
        {
            // Loại đơn hàng
            _loadOrderType();
            // Ngày gửi
            rRefundDate.SelectedDate = DateTime.Now;
        }
        #endregion
    }
}