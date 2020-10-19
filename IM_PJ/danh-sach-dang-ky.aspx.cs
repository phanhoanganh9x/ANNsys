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
using static IM_PJ.Controllers.RegisterController;

namespace IM_PJ
{
    public partial class danh_sach_dang_ky : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["usernameLoginSystem"] != null)
                {
                    string username = Request.Cookies["usernameLoginSystem"].Value;
                    var acc = AccountController.GetByUsername(username);
                    int agent = acc.AgentID.ToString().ToInt();

                    if (acc != null)
                    {
                        if (acc.RoleID == 0 || acc.Username == "hotline")
                        {
                            LoadUser();
                        }
                        else if (acc.RoleID == 2)
                        {
                            LoadUser(acc);
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
                LoadProvince();
            }
        }
        public void LoadUser(tbl_Account acc = null)
        {
            //if (acc != null)
            //{
            //    ddlUser.Items.Clear();
            //    ddlUser.Items.Insert(0, new ListItem(acc.Username, acc.ID.ToString()));
            //}
            //else
            //{

            //}

            var CreateBy = AccountController.GetAllByRoleID(2);
            ddlUser.Items.Clear();
            ddlUser.Items.Insert(0, new ListItem("Nhân viên phụ trách", ""));
            // drop down list at update register modal
            ddlUserModal.Items.Clear();
            ddlUserModal.Items.Insert(0, new ListItem("Chọn nhân viên", "0"));
            ddlUserCreateModal.Items.Insert(0, new ListItem("Chọn nhân viên", "0"));
            if (CreateBy.Count > 0)
            {
                foreach (var p in CreateBy)
                {
                    if (p.Username != "hotline")
                    {
                        ListItem listitem = new ListItem(p.Username, p.ID.ToString());
                        ddlUser.Items.Add(listitem);
                        ddlUserModal.Items.Add(listitem);
                        ddlUserCreateModal.Items.Add(listitem);
                    }
                }
                ddlUser.DataBind();
                ddlUserModal.DataBind();
            }
        }
        public void LoadProvince()
        {
            var pro = ProvinceController.GetAll();
            ddlProvince.Items.Clear();
            ddlProvince.Items.Insert(0, new ListItem("Tỉnh thành", "0"));
            if (pro.Count > 0)
            {
                foreach (var p in pro)
                {
                    ListItem listitem = new ListItem(p.Name, p.ID.ToString());
                    ddlProvince.Items.Add(listitem);
                }
                ddlProvince.DataBind();
            }
        }
        public void LoadData()
        {
            string username = Request.Cookies["usernameLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if(acc.RoleID == 0 || acc.Username == "hotline")
                {
                    ltrCreateButton.Text = "<button type='button' class='h45-btn primary-btn btn' data-toggle='modal' data-target='#CreateRegisterModal' data-backdrop='static' data-keyboard='false' title='Cập nhật thông tin đăng ký'>Thêm mới</button>";
                }
                int Status = 0;
                string TextSearch = "";
                string CreatedDate = "";
                int UserID = 0;
                int ProvinceID = 0;
                string Referer = "";
                string Category = "";

                if (Request.QueryString["textsearch"] != null)
                {
                    TextSearch = Request.QueryString["textsearch"].Trim();
                }
                if (Request.QueryString["status"] != null)
                {
                    Status = Request.QueryString["status"].ToInt(0);
                }
                if (Request.QueryString["createddate"] != null)
                {
                    CreatedDate = Request.QueryString["createddate"];
                }
                if (Request.QueryString["userid"] != null)
                {
                    UserID = Request.QueryString["userid"].ToInt(0);
                }
                if (Request.QueryString["provinceid"] != "0")
                {
                    ProvinceID = Request.QueryString["provinceid"].ToInt(0);
                }
                if (Request.QueryString["referer"] != null)
                {
                    Referer = Request.QueryString["referer"];
                }
                if (Request.QueryString["category"] != null)
                {
                    Category = Request.QueryString["category"];
                }

                txtSearch.Text = TextSearch;
                ddlStatus.SelectedValue = Status.ToString();
                ddlUser.SelectedValue = UserID.ToString();
                ddlCreatedDate.SelectedValue = CreatedDate.ToString();
                ddlProvince.SelectedValue = ProvinceID.ToString();
                ddlReferer.SelectedValue = Referer;
                ddlProductCategory.SelectedValue = Category;

                var rs = RegisterController.Filter(TextSearch, ProvinceID, UserID, Status, Referer, Category, CreatedDate);

                //if(acc.RoleID != 0 && acc.Username != "hotline")
                //{
                //    rs = rs.Where(x => x.UserID == acc.ID).ToList();
                //}

                pagingall(rs);

                ltrNumberOfOrder.Text = rs.Count().ToString();

            }
        }


        #region Paging
        public void pagingall(List<RegisterOut> acs)
        {
            string username = Request.Cookies["usernameLoginSystem"].Value;
            var acc = AccountController.GetByUsername(username);

            int PageSize = 30;

            StringBuilder html = new StringBuilder();
            html.Append("<thead>");
            html.Append("<tr>");
            html.Append("    <th>#</th>");
            html.Append("    <th>Tên</th>");
            html.Append("    <th>Điện thoại</th>");
            html.Append("    <th>Địa chỉ</th>");
            html.Append("    <th>Tỉnh</th>");
            html.Append("    <th>Quan tâm</th>");
            html.Append("    <th>Nguồn</th>");
            html.Append("    <th>Trạng thái</th>");
            html.Append("    <th>Phụ trách</th>");
            html.Append("    <th>Ngày đăng ký</th>");
            html.Append("    <th></th>");
            html.Append("</tr>");
            html.Append("</thead>");

            html.Append("<tbody>");
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

                    var TrTag = new StringBuilder();
                    TrTag.AppendLine("<tr ");
                    TrTag.AppendLine(String.Format("data-registerid='{0}' ", item.ID));
                    TrTag.AppendLine(String.Format("data-userid='{0}' ", item.UserID));
                    TrTag.AppendLine("/>");

                    html.Append(TrTag.ToString());
                    var message = "";
                    if (!string.IsNullOrEmpty(item.Note))
                    {
                        message = "<a href='javascript:;' class='view-message' data-id='" + item.ID + "' data-toggle='modal' data-target='#ViewMessageModal' data-backdrop='static' data-keyboard='false'>(tin nhắn)</a>";
                    }
                    html.Append("   <td data-title='#'>" + (i + 1).ToString() + "</td>");
                    html.Append("   <td data-title='Tên'><strong>" + item.Name + "</strong> " + message + "</td>");

                    string phone = "<a href='https://zalo.me/" + item.Phone + "' target='_blank'>" + item.Phone + "</a>";
                    if (item.Status == 1)
                    {
                        phone = "<a href='javascript:;' data-id='" + item.ID + "' data-phone='" + item.Phone + "' data-update='2' onclick='getPhone($(this))'>xem</a>";
                    }
                    if (item.Status == 3 && acc.RoleID != 0 && acc.Username != "hotline")
                    {
                        phone = "<a href='javascript:;' data-id='" + item.ID + "' data-phone='" + item.Phone + "' data-update='4' onclick='getPhone($(this))'>tiếp nhận</a>";
                    }
                    html.Append("   <td id='phone' data-title='Điện thoại'>" + phone + "</td>");

                    html.Append("   <td data-title='Địa chỉ'>" + item.Address + "</td>");
                    html.Append("   <td data-title='Tỉnh'>" + item.ProvinceName + "</td>");
                    html.Append("   <td data-title='Quan tâm'>" + item.ProductCategory + "</td>");
                    html.Append("   <td data-title='Nguồn'>" + item.Referer + "</td>");

                    string status = "";
                    if (item.Status == 1)
                    {
                        status = "<span class='bg-black'>Chưa xem</span>";
                    }
                    else if (item.Status == 2)
                    {
                        status = "<span class='bg-yellow'>Đã xem</span>";
                    }
                    else if (item.Status == 3)
                    {
                        status = "<span class='bg-blue'>Đã bàn giao</span>";
                    }
                    else if (item.Status == 4)
                    {
                        status = "<span class='bg-green'>Đã tiếp nhận</span>";
                    }
                    else if (item.Status == 5)
                    {
                        status = "<span class='bg-green'>Khách hàng</span>";
                    }
                    html.Append("   <td id='status' data-title='Trạng thái'>" + status + "</td>");

                    html.Append("   <td id='user' data-title='Phụ trách'>" + item.User + "</td>");
                    string date = string.Format("{0:dd/MM HH:mm}", item.CreatedDate);
                    html.Append("   <td data-title='Ngày đăng ký'>" + date + "</td>");

                    html.Append("   <td data-title='Thao tác' class='update-button' id='updateButton'>");
                    if(acc.RoleID == 0 || acc.Username == "hotline")
                    {
                        html.Append("       <button type='button' class='btn primary-btn h45-btn' data-toggle='modal' data-target='#UpdateRegisterModal' data-backdrop='static' data-keyboard='false' title='Cập nhật thông tin đăng ký'><span class='glyphicon glyphicon-edit'></span></button>");
                    }
                    if (acc.RoleID == 0)
                    {
                        html.Append("       <a href='javascript:;' class='btn primary-btn h45-btn btn-red' title='Xóa thông tin đăng ký này' onclick='deleteRegister(" + item.ID + ")'><i class='fa fa-times' aria-hidden='true'></i></a>");
                    }
                    html.Append("   </td>");
                    html.Append("</tr>");
                }
                
            }
            else
            {
                html.Append("<tr><td colspan='12'>Không tìm thấy đăng ký mua sỉ...</td></tr>");
            }

            html.Append("</tbody>");

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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();
            string request = "/danh-sach-dang-ky?";

            if (search != "")
            {
                request += "&textsearch=" + search;
            }

            if(ddlStatus.SelectedValue != "")
            {
                request += "&status=" + ddlStatus.SelectedValue;
            }

            if (ddlUser.SelectedValue != "")
            {
                request += "&userid=" + ddlUser.SelectedValue;
            }

            if (ddlProvince.SelectedValue != "0")
            {
                request += "&provinceid=" + ddlProvince.SelectedValue;
            }

            if (ddlReferer.SelectedValue != "")
            {
                request += "&referer=" + ddlReferer.SelectedValue;
            }

            if (ddlProductCategory.SelectedValue != "")
            {
                request += "&category=" + ddlProductCategory.SelectedValue;
            }

            if (ddlCreatedDate.SelectedValue != "")
            {
                request += "&createddate=" + ddlCreatedDate.SelectedValue;
            }

            Response.Redirect(request);
        }
        [WebMethod]
        public static string updateStatus(int id, int value)
        {
            int ID = RegisterController.UpdateStatus(id, value);

            if (ID != 0)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }
        [WebMethod]
        public static string deleteRegister(int id)
        {
            string register = RegisterController.deleteRegister(id);

            if (register != null)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }
        [WebMethod]
        public static string updateUser(int id, int userid)
        {
            int ID = RegisterController.UpdateUser(id, userid);

            if (ID != 0)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }
        [WebMethod]
        public static string createRegister(Register data)
        {
            Register register = new Register();
            register.Name = data.Name;
            register.Phone = data.Phone;
            register.UserID = data.UserID;
            register.Status = 3;
            register.Note = "";
            register.Address = "";
            register.ProductCategory = "";
            register.ProvinceID = 0;
            register.Referer = "user";

            int ID = RegisterController.Insert(register);

            if (ID != 0)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }
        [WebMethod]
        public static string getRegisterMessage(int id)
        {
            var register = RegisterController.GetByID(id);

            if (register != null)
            {
                return register.Note;
            }
            else
            {
                return "false";
            }
        }
    }
}