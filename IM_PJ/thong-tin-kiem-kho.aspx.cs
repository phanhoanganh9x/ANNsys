using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Models.Pages.thuc_hien_kiem_kho;
using IM_PJ.Utils;
using MB.Extensions;
using Newtonsoft.Json;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class thong_tin_kiem_kho : System.Web.UI.Page
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
                        if (acc.RoleID != 0 && acc.RoleID != 1 && acc.RoleID != 3)
                        {
                            Response.Redirect("/trang-chu");
                        }
                        hdfRoleID.Value = acc.RoleID.ToString();
                        LoadData();
                    }
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
                
            }
        }

        public void LoadData()
        {
            var checkWarehouse = CheckWarehouseController.getAll();
            ddlCheckWarehouse.Items.Clear();
            ddlCheckWarehouse.Items.Insert(0, new ListItem("Chọn phiên kiểm kho", ""));

            foreach (var cw in checkWarehouse)
            {
                ListItem listitem = new ListItem(String.Format("{0} - {1}", cw.ID, cw.Name), cw.ID.ToString());
                ddlCheckWarehouse.Items.Add(listitem);
            }

            if (checkWarehouse.Count > 0)
            {
                ddlCheckWarehouse.DataBind();

                for (int i = 0; i < ddlCheckWarehouse.Items.Count; i++)
                {
                    var item = ddlCheckWarehouse.Items[i];

                    if (!String.IsNullOrEmpty(item.Value))
                    {
                        var cw = checkWarehouse.Where(x => x.ID.ToString() == item.Value).SingleOrDefault();
                        item.Attributes.Add("data-finished", cw.Active ? "false" : "true");
                    }
                }
            }

            if (!String.IsNullOrEmpty(Request.QueryString["checkID"]))
                ddlCheckWarehouse.SelectedValue = Request.QueryString["checkID"];
        }

        [WebMethod]
        public static string getCheckWarehouse(int id)
        {
            using (var con = new inventorymanagementEntities())
            {
                var productCheck = con.CheckWarehouseDetails.Where(x => x.CheckWarehouseID == id);

                if (productCheck.Count() == 0)
                    return null;

                var data = productCheck
                    .Join(
                        con.tbl_Product,
                        c => c.ProductID,
                        p => p.ID,
                        (c, p) => new
                        {
                            ProductName = p.ProductTitle,
                            SKU = c.ProductSKU,
                            QuantityOld = c.QuantityOld,
                            QuantityNew = c.QuantityNew,
                            CreatedDate = c.CreatedDate,
                            CreatedBy = c.CreatedBy,
                            ModifiedDate = c.ModifiedDate,
                            ModifiedBy = c.ModifiedBy
                        }
                    )
                    .OrderByDescending(o => o.ModifiedDate)
                    .ToList();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var result = data.Select(x => new CheckInfo()
                {
                    ProductName = x.ProductName,
                    SKU = x.SKU,
                    QuantityOld = String.Format("{0:N0}", x.QuantityOld),
                    QuantityNew = x.QuantityNew.HasValue ? String.Format("{0:N0}", x.QuantityNew.Value) : String.Empty,
                    ModifiedDate = x.CreatedDate == x.ModifiedDate ? String.Empty : x.ModifiedDate.ToString(),
                    //ModifiedDate = x.CreatedDate == x.ModifiedDate ? String.Empty : x.ModifiedDate.ToString("yyyy-MM-dd hh:mm:ss"),
                    ModifiedBy = x.CreatedBy == x.ModifiedBy ? String.Empty : x.ModifiedBy
                }).ToList();

                return serializer.Serialize(result);
            }
        }


        public class CheckInfo
        {
            public string ProductName { get; set; }
            public string SKU { get; set; }
            public string QuantityOld { get; set; }
            public string QuantityNew { get; set; }
            public string ModifiedDate { get; set; }
            public string ModifiedBy { get; set; }
        }

        /// <summary>
        /// Cập nhật số lượng 0 với những sản phẩm chưa được kiểm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateQuantity_Click(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var checkID = hdfCheckHouseID.Value.ToInt(0);
            var acc = AccountController.GetByUsername(username);

            if (acc == null || acc.RoleID != 0)
                return;

            var products = CheckWarehouseController.getProductRemainingByCheckID(checkID);

            if (products == null)
                PJUtils.ShowMessageBoxSwAlert("Không có sản phẩm nào chưa kiểm tra!", "i", false, Page);
            else
            {
                var productUpdate = products
                    .Select(x => new UpdateQuantityModel()
                    {
                        checkID = x.CheckWarehouseID,
                        sku = x.ProductSKU,
                        quantity = 0,
                        updateDate = now,
                        staff = username
                    })
                    .ToList();
                var result = CheckWarehouseController.updateQuantity(productUpdate);

                if (result)
                    PJUtils.ShowMessageBoxSwAlert("Thành cập nhật số lượng các sản phẩm chưa kiểm!", "s", true, Page);
                else
                    PJUtils.ShowMessageBoxSwAlert("Thất bại trong việc cập nhật số lượng các sản phẩm chưa kiểm!", "e", false, Page);
            }
        }

        protected void btnCloseCheckWareHouse_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

            if (acc == null || acc.RoleID != 0)
                return;

            var result = CheckWarehouseController.closeCheckHouse(hdfCheckHouseID.Value.ToInt(0), DateTime.Now, acc.Username);

            if (result)
                PJUtils.ShowMessageBoxSwAlert("Thành công trong việc đóng lại hoạt động kiểm tra kho!", "s", true, Page);
            else
                PJUtils.ShowMessageBoxSwAlert("Thất bại trong việc đóng lại hoạt động kiểm tra kho!", "e", true, Page);
        }
    }
}