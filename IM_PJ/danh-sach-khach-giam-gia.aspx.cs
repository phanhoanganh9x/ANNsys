using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Models.Common;
using MB.Extensions;
using Newtonsoft.Json;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using static IM_PJ.Controllers.CustomerController;

namespace IM_PJ
{
    public partial class danh_sach_khach_giam_gia : System.Web.UI.Page
    {
        private static int discountGroupID = 0;
        private static tbl_Account acc;

        protected void Page_Load(object sender, EventArgs e)
        {
            discountGroupID = Request.QueryString["id"].ToInt(0);

            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                    acc = AccountController.GetByUsername(username);
                    if (!AccountController.isPermittedLoading(acc, "danh-sach-khach-giam-gia", discountGroupID))
                    {
                        Response.Redirect("/trang-chu");
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
            if (discountGroupID > 0)
            {
                string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                acc = AccountController.GetByUsername(username);

                var discount = DiscountGroupController.GetByID(discountGroupID);
                if (discount != null)
                {
                    hdfDiscountGroupID.Value = discountGroupID.ToString();
                    

                    // Hiển thị thông tin của discount group
                    ltrGroupName.Text = discount.DiscountName;
                    var numberCustomer = DiscountCustomerController.GetByGroupID(discount.ID).Count;
                    if(numberCustomer > 0)
                        ltrNumberCustomer.Text = String.Format("{0:N0} khách", numberCustomer);
                    if(discount.DiscountAmount.HasValue && discount.DiscountAmount.Value > 0)
                        ltrDiscount.Text = String.Format("{0:N0}đ/cái", discount.DiscountAmount);
                    if (discount.QuantityRequired.HasValue && discount.QuantityRequired.Value > 0)
                        ltrQuantityRequired.Text = String.Format("{0:N0} cái", discount.QuantityRequired);
                    if (discount.QuantityProduct.HasValue && discount.QuantityProduct.Value > 0)
                        ltrQuantityProduct.Text = String.Format("{0:N0} cái", discount.QuantityProduct);
                    if (discount.FeeRefund.HasValue && discount.FeeRefund.Value > 0)
                        ltrFeeRefund.Text = String.Format("{0:N0}đ/cái", discount.FeeRefund);
                    else
                        ltrFeeRefund.Text = "miễn phí";
                    if (discount.NumOfDateToChangeProduct.HasValue && discount.NumOfDateToChangeProduct.Value > 0)
                        ltrNumberOfDateToChnageProduct.Text = String.Format("{0:N0} ngày", discount.NumOfDateToChangeProduct);
                    if (discount.NumOfProductCanChange.HasValue && discount.NumOfProductCanChange.Value > 0)
                        ltrNumberOfProductCanChange.Text = String.Format("{0:N0} cái/{1} ngày", discount.NumOfProductCanChange, discount.NumOfDateToChangeProduct);
                    if (discount.RefundQuantityNoFee.HasValue && discount.RefundQuantityNoFee.Value > 0)
                        ltrRefundQuantityNoFee.Text = String.Format("{0:N0} cái", discount.RefundQuantityNoFee);

                    string createdBy = "";
                    if (acc.RoleID == 2)
                    {
                        createdBy = acc.Username;
                    }
                    // Hiển thị thông tin khách hàng được hưởng chiết khấu
                    var customer = CustomerController.getByDiscountGroupID(discountGroupID, createdBy)
                        .OrderBy(o => o.FullName)
                        .ToList();

                    if (customer.Count > 0)
                        pagingall(customer);
                }
            }
        }
        #region Paging
        public void pagingall(List<CustomerModel> acs)
        {
            int PageSize = 15;
            StringBuilder html = new StringBuilder();
            if (acs.Count > 0)
            {
                int TotalItems = acs.Count;
                if (TotalItems % PageSize == 0)
                    PageCount = TotalItems / PageSize;
                else
                    PageCount = TotalItems / PageSize + 1;

                Int32 Page = GetIntFromQueryString("Page");

                if (Page == -1) Page = 1;
                int FromRow = (Page - 1) * PageSize;
                int ToRow = Page * PageSize - 1;
                if (ToRow >= TotalItems)
                    ToRow = TotalItems - 1;
                for (int i = FromRow; i < ToRow + 1; i++)
                {
                    var item = acs[i];
                    html.Append("<tr data-id='" + item.ID + "'>");
                    html.Append("   <td class='customer-name-link'><a href='/chi-tiet-khach-hang?id=" + item.ID + "'>" + item.FullName.ToTitleCase() + "</a></td>");
                    html.Append("   <td class='customer-name-link'>" + item.Nick.ToTitleCase() + "</td>");
                    html.Append("   <td>" + item.Phone + "</td>");
                    html.Append("   <td>" + item.StaffName + "</td>");
                    html.Append("   <td>" + String.Format("{0:dd/MM/yyyy}", item.DiscountGroup.DateJoined) + "</td>");

                    html.Append("   <td>");
                    if (acc.Username == "admin" )
                        html.Append("       <a href='javascript:;' onclick='deletecustomer(" + JsonConvert.SerializeObject(item) + ")' class='btn primary-btn btn-red h45-btn'><i class='fa fa-times' aria-hidden='true'></i> Xóa</a>");
                    if (item.DiscountGroup.VerifyOrder > 0)
                        html.Append(String.Format("       <a href='/thong-tin-don-hang?id={0}' class='btn primary-btn h45-btn' target='_blank'><i class='fa fa-file-text-o' aria-hidden='true'></i> Đơn xác nhận</a>", item.DiscountGroup.VerifyOrder));
                    html.Append("       <a href='/thong-ke-khach-hang?textsearch=" + item.Phone + "' title='Xem thống kê khách hàng' class='btn primary-btn btn-blue h45-btn' target='_blank'><i class='fa fa-line-chart' aria-hidden='true'></i> Thống kê</a>");
                    html.Append("   </td>");
                    html.Append("</tr>");
                }
            }
            ltrList.Text = html.ToString();
        }
        public static Int32 GetIntFromQueryString(String key)
        {
            Int32 returnValue = -1;
            String queryStringValue = HttpContext.Current.Request.QueryString[key];
            try
            {
                if (queryStringValue == null)
                    return returnValue;
                if (queryStringValue.IndexOf("#") > 0)
                    queryStringValue = queryStringValue.Substring(0, queryStringValue.IndexOf("#"));
                returnValue = Convert.ToInt32(queryStringValue);
            }
            catch
            { }
            return returnValue;
        }
        private int PageCount;
        protected void DisplayHtmlStringPaging1()
        {

            Int32 CurrentPage = Convert.ToInt32(Request.QueryString["Page"]);
            if (CurrentPage == -1) CurrentPage = 1;
            string[] strText = new string[4] { "Trang đầu", "Trang cuối", "Trang sau", "Trang trước" };
            if (PageCount > 1)
                Response.Write(GetHtmlPagingAdvanced(6, CurrentPage, PageCount, Context.Request.RawUrl, strText));

        }
        private static string GetPageUrl(int currentPage, string pageUrl)
        {
            pageUrl = Regex.Replace(pageUrl, "(\\?|\\&)*" + "Page=" + currentPage, "");
            if (pageUrl.IndexOf("?") > 0)
            {
                pageUrl += "&Page={0}";
            }
            else
            {
                pageUrl += "?Page={0}";
            }
            return pageUrl;
        }
        public static string GetHtmlPagingAdvanced(int pagesToOutput, int currentPage, int pageCount, string currentPageUrl, string[] strText)
        {
            //Nếu Số trang hiển thị là số lẻ thì tăng thêm 1 thành chẵn
            if (pagesToOutput % 2 != 0)
            {
                pagesToOutput++;
            }

            //Một nửa số trang để đầu ra, đây là số lượng hai bên.
            int pagesToOutputHalfed = pagesToOutput / 2;

            //Url của trang
            string pageUrl = GetPageUrl(currentPage, currentPageUrl);


            //Trang đầu tiên
            int startPageNumbersFrom = currentPage - pagesToOutputHalfed; ;

            //Trang cuối cùng
            int stopPageNumbersAt = currentPage + pagesToOutputHalfed; ;

            StringBuilder output = new StringBuilder();

            //Nối chuỗi phân trang
            //output.Append("<div class=\"paging\">");
            output.Append("<ul>");

            //Link First(Trang đầu) và Previous(Trang trước)
            if (currentPage > 1)
            {
                output.Append("<li><a title=\"" + strText[0] + "\" href=\"" + string.Format(pageUrl, 1) + "\">Trang đầu</a></li>");
                output.Append("<li><a title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\">Trang trước</a></li>");
                //output.Append("<li class=\"UnselectedPrev\" ><a title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\"><i class=\"fa fa-angle-left\"></i></a></li>");

                //output.Append("<span class=\"Unselect_prev\"><a href=\"" + string.Format(pageUrl, currentPage - 1) + "\"></a></span>");
            }

            /******************Xác định startPageNumbersFrom & stopPageNumbersAt**********************/
            if (startPageNumbersFrom < 1)
            {
                startPageNumbersFrom = 1;

                //As page numbers are starting at one, output an even number of pages.  
                stopPageNumbersAt = pagesToOutput;
            }

            if (stopPageNumbersAt > pageCount)
            {
                stopPageNumbersAt = pageCount;
            }

            if ((stopPageNumbersAt - startPageNumbersFrom) < pagesToOutput)
            {
                startPageNumbersFrom = stopPageNumbersAt - pagesToOutput;
                if (startPageNumbersFrom < 1)
                {
                    startPageNumbersFrom = 1;
                }
            }
            /******************End: Xác định startPageNumbersFrom & stopPageNumbersAt**********************/

            //Các dấu ... chỉ những trang phía trước  
            if (startPageNumbersFrom > 1)
            {
                output.Append("<li><a href=\"" + string.Format(GetPageUrl(currentPage - 1, pageUrl), startPageNumbersFrom - 1) + "\">&hellip;</a></li>");
            }

            //Duyệt vòng for hiển thị các trang
            for (int i = startPageNumbersFrom; i <= stopPageNumbersAt; i++)
            {
                if (currentPage == i)
                {
                    output.Append("<li class=\"current\" ><a >" + i.ToString() + "</a> </li>");
                }
                else
                {
                    output.Append("<li><a href=\"" + string.Format(pageUrl, i) + "\">" + i.ToString() + "</a> </li>");
                }
            }

            //Các dấu ... chỉ những trang tiếp theo  
            if (stopPageNumbersAt < pageCount)
            {
                output.Append("<li><a href=\"" + string.Format(pageUrl, stopPageNumbersAt + 1) + "\">&hellip;</a></li>");
            }

            //Link Next(Trang tiếp) và Last(Trang cuối)
            if (currentPage != pageCount)
            {
                //output.Append("<span class=\"Unselect_next\"><a href=\"" + string.Format(pageUrl, currentPage + 1) + "\"></a></span>");
                //output.Append("<li class=\"UnselectedNext\" ><a title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\"><i class=\"fa fa-angle-right\"></i></a></li>");
                output.Append("<li><a title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\">Trang sau</a></li>");
                output.Append("<li><a title=\"" + strText[3] + "\" href=\"" + string.Format(pageUrl, pageCount) + "\">Trang cuối</a></li>");
            }
            output.Append("</ul>");
            //output.Append("</div>");
            return output.ToString();
        }
        #endregion
        protected void btnAddCustomer_Click(object sender, EventArgs e)
        {
            if (discountGroupID != 0 && acc != null)
            {
                var customer = JsonConvert.DeserializeObject<CustomerModel>(hdfCustomer.Value);
                var order = JsonConvert.DeserializeObject<OrderModel>(hdfOrder.Value);

                if (customer != null)
                {
                    var now = DateTime.Now;
                    var data = new tbl_DiscountCustomer()
                    {
                        DiscountGroupID = discountGroupID,
                        UID = customer.ID,
                        CustomerName = customer.FullName,
                        CustomerPhone = customer.Phone,
                        IsHidden = false,
                        CreatedDate = now,
                        CreatedBy = acc.Username,
                        ModifiedDate = now,
                        ModifiedBy = acc.Username,
                        VerifyOrder = order != null ? order.ID : 0
                    };

                    DiscountCustomerController.Insert(data);
                    PJUtils.ShowMessageBoxSwAlert("Thêm khách hàng vào nhóm thành công", "s", true, Page);
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Vui lòng chọn khách hàng cần thêm", "e", true, Page);
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (acc != null && acc.RoleID == 0)
            {
                var customer = JsonConvert.DeserializeObject<CustomerModel>(hdfCustomer.Value);

                if (customer != null)
                {
                    DiscountCustomerController.Delete(customer.ID);
                    PJUtils.ShowMessageBoxSwAlert("Xóa khách hàng ra khỏi nhóm thành công", "s", true, Page);
                }
            }
        }

        [WebMethod]
        public static List<CustomerModel> getPotentialCustomer(int discountGroupID, string search)
        {
            // Trường hợp là admin thì không cần filter theo người khởi tạo
            // chỉ cần khách hàng đạt chuẩn của mức chiết khấu là đc
            var staffName = HttpContext.Current.Request.Cookies["usernameLoginSystem_ANN123"].Value;
            if (String.IsNullOrEmpty(staffName))
                return null;

            // Lấy ra danh sách các khách hàng tìm năng
            var data = CustomerController.GetPotentialCustomers(discountGroupID, staffName);

            // Lọc lại khách hàng theo bộ lọc
            var result = data;
            search = search.Trim().ToUpper();

            if (!String.IsNullOrEmpty(search))
                result = result.Where(x =>
                    x.FullName.Trim().ToUpper().Contains(search) ||
                    x.Nick.Trim().ToUpper().Contains(search) ||
                    x.Phone.Trim().ToUpper().Contains(search)
                ).ToList();

            return result;
        }

        [WebMethod]
        public static List<OrderModel> getOrderQualifiedOfDiscountGroup(int discountGroupID, int customerID)
        {
            // Trường hợp là admin thì không cần filter theo người khởi tạo
            // chỉ cần khách hàng đạt chuẩn của mức chiết khấu là đc
            var staffName = HttpContext.Current.Request.Cookies["usernameLoginSystem_ANN123"].Value;
            if (String.IsNullOrEmpty(staffName))
                return null;

            if (staffName == "admin")
                // Lấy những đơn khách đã mua
                return OrderController.get(customerID);
            else
                // Lấy ra đơn đủ điều kiện để khách hàng có thể join vô group
                return OrderController.getOrderQualifiedOfDiscountGroup(discountGroupID, customerID);
        }
    }
}