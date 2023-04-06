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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.Services;
using static IM_PJ.Controllers.PostController;
using System.IO;

namespace IM_PJ
{
    public partial class danh_sach_bai_viet : System.Web.UI.Page
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
                        if (acc.RoleID == 0 || acc.Username == "baiviet")
                        {
                            LoadData();
                            LoadCreatedBy();
                            LoadCategory();
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
            }
        }
        public void LoadCreatedBy(tbl_Account acc = null)
        {
            if (acc != null)
            {
                ddlCreatedBy.Items.Clear();
                ddlCreatedBy.Items.Insert(0, new ListItem(acc.Username, acc.Username));
            }
            else
            {
                var CreateBy = AccountController.GetAllNotSearch().Where(x => x.RoleID == 0 || x.RoleID == 2).ToList();
                ddlCreatedBy.Items.Clear();
                ddlCreatedBy.Items.Insert(0, new ListItem("Người viết bài", ""));
                if (CreateBy.Count > 0)
                {
                    foreach (var p in CreateBy)
                    {
                        ListItem listitem = new ListItem(p.Username, p.Username);
                        ddlCreatedBy.Items.Add(listitem);
                    }
                    ddlCreatedBy.DataBind();
                }
            }
        }
        public void LoadCategory()
        {
            var category = PostCategoryController.GetAll();
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new ListItem("Danh mục bài viết", "0"));
            if (category.Count > 0)
            {
                addItemCategory(0, "");
                ddlCategory.DataBind();
            }
        }

        public void addItemCategory(int id, string h = "")
        {
            var categories = PostCategoryController.GetByParentID("", id);

            if (categories.Count > 0)
            {
                foreach (var c in categories)
                {
                    ListItem listitem = new ListItem(h + c.Title, c.ID.ToString());
                    ddlCategory.Items.Add(listitem);

                    addItemCategory(c.ID, h + "---");
                }
            }
        }
        public void LoadData()
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                if (acc.RoleID == 0 || acc.Username == "baiviet")
                {
                    ltrAddPost.Text = "<a href='/tao-bai-viet' class='h45-btn btn primary-btn'>Thêm mới</a>";
                }
            }
            
            string TextSearch = "";
            string CreatedDate = "";
            string CreatedBy = "";
            int CategoryID = 0;
            string Status = "";
            string WebPublish = "";

            if (Request.QueryString["textsearch"] != null)
                TextSearch = Request.QueryString["textsearch"].Trim();
            if (Request.QueryString["status"] != null)
                Status = Request.QueryString["status"];
            if (Request.QueryString["categoryid"] != null)
                CategoryID = Request.QueryString["categoryid"].ToInt();
            if (Request.QueryString["createddate"] != null)
                CreatedDate = Request.QueryString["createddate"];
            if (Request.QueryString["webpublish"] != null)
                WebPublish = Request.QueryString["webpublish"];
            if (Request.QueryString["createdby"] != null)
                CreatedBy = Request.QueryString["createdby"];

            txtSearchPost.Text = TextSearch;
            ddlCategory.SelectedValue = CategoryID.ToString();
            ddlCreatedDate.SelectedValue = CreatedDate.ToString();
            ddlStatus.SelectedValue = Status.ToString();
            ddlWebPublish.SelectedValue = WebPublish.ToString();
            ddlCreatedBy.SelectedValue = CreatedBy;

            List<PostSQL> a = new List<PostSQL>();
            a = PostController.GetAllSql(CategoryID, TextSearch, Status, WebPublish, CreatedDate, CreatedBy);

            pagingall(a);

            ltrNumberOfPost.Text = a.Count().ToString();
        }
        [WebMethod]
        public static string getAllPostImage(int id)
        {
            var post = PostController.GetByID(id);
            List<string> images = new List<string>();
            if (post != null)
            {
                images.Add(post.Image);

                // lấy ảnh sản phẩm từ thư viện

                var imagePost = PostImageController.GetByPostID(post.ID);

                if(imagePost != null)
                {
                    foreach (var img in imagePost)
                    {
                        images.Add(img.Image);
                    }
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(images.Distinct().ToList());
        }
        [WebMethod]
        public static string updateWebPublish(int id, bool value)
        {
            string update = PostController.updateWebPublish(id, value);
            if (update != null)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }
        [WebMethod]
        public static string deletePost(int id)
        {
            var post = PostController.GetByID(id);
            string result = "";

            if (post != null)
            {
                // Delete image gallery
                var postImage = PostImageController.GetByPostID(post.ID);
                if (postImage.Count > 0)
                {
                    foreach (var img in postImage)
                    {
                        if (!string.IsNullOrEmpty(img.Image))
                        {
                            // Delete in database
                            string deletePostImage = PostImageController.Delete(img.ID);
                        }
                    }
                }

                string deletePost = PostController.Delete(id);

                if (!string.IsNullOrEmpty(deletePost))
                {
                    result = "success";
                }
                else
                {
                    result = "failed";
                }
            }
            else
            {
                result = "notfound";
            }

            return result;
        }
        [WebMethod]
        public static string copyPostInfo(int id)
        {
            var post = PostController.GetByID(id);
            StringBuilder html = new StringBuilder();
            if (post != null)
            {
                html.Append("<p>" + post.Content + "</p>\r\n");
                html.Append("<p></p>\r\n");
            }

            return html.ToString();
        }

        [WebMethod]
        public static string copyPostToApp(int id)
        {
            var post = PostController.GetByID(id);
            if (post != null)
            {
                var postCategory = PostCategoryController.GetByID(post.CategoryID.Value);
                PostCategory postPublicCategory = new PostCategory();
                if (postCategory != null)
                {
                    postPublicCategory = PostPublicCategoryController.GetByName(postCategory.Title);
                }

                var newPostPublic = new PostPublic()
                {
                    CategoryID = postPublicCategory != null ? postPublicCategory.ID : 0,
                    CategorySlug = postPublicCategory != null ? postPublicCategory.Slug : "",
                    Title = post.Title,
                    Thumbnail = post.Image,
                    Summary = "",
                    Content = post.Content,
                    Action = "view_more",
                    ActionValue = PostPublicController.checkSlug(post.Slug),
                    AtHome = false,
                    IsPolicy = false,
                    CreatedDate = DateTime.Now,
                    CreatedBy = post.CreatedBy,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = post.CreatedBy
                };

                var newpost = PostPublicController.Insert(newPostPublic);

                if (newpost != null)
                {
                    // Copy image gallery
                    var postImage = PostImageController.GetToCopyByPostID(post.ID);
                    if (postImage.Count > 0)
                    {
                        foreach (var img in postImage)
                        {
                            if (!string.IsNullOrEmpty(img.Image))
                            {
                                string newImage = PostPublicImageController.Insert(newpost.ID, img.Image, newpost.CreatedBy, DateTime.Now);
                            }
                        }
                    }

                    return newpost.ID.ToString();
                }
            }

            return "false";
        }

        #region Paging
        public void pagingall(List<PostSQL> acs)
        {
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

            int PageSize = 30;
            StringBuilder html = new StringBuilder();
            html.Append("<tr>");
            html.Append("    <th class='image-column'>Ảnh</th>");
            html.Append("    <th class='name-column'>Tiêu đề</th>");
            html.Append("    <th class='sku-column'>Nổi bật</th>");
            html.Append("    <th class='stock-status-column'>Trang nội bộ</th>");
            html.Append("    <th class='category-column'>Danh mục</th>");
            html.Append("    <th class='category-column'>Trang xem hàng</th>");
            html.Append("    <th class='date-column'>Người viết</th>");
            html.Append("    <th class='date-column'>Ngày tạo</th>");
            html.Append("    <th class='action-column'></th>");
            html.Append("</tr>");

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
                    html.Append("<tr class='item-" + item.ID + "'>");
                    html.Append("<td>");
                    html.Append("   <a target='_blank' href='/xem-bai-viet?id=" + item.ID + "'><img src='" + item.Image + "'/></a>");
                    html.Append("   <a href='javascript:;' onclick='copyPostInfo(" + item.ID + ")' class='btn download-btn h45-btn'><i class='fa fa-files-o'></i> Copy</a>");
                    html.Append("</td>");
                    html.Append("   <td class='customer-name-link'><a target='_blank' href='/xem-bai-viet?id=" + item.ID + "'>" + item.Title + "</a></td>");
                    if (item.Featured == 1)
                    {
                        html.Append("   <td><span class='bg-blue'>Nổi bật</span></td>");
                    }
                    else
                    {
                        html.Append("   <td></td>");
                    }
                    if (item.Status == 1)
                    {
                        html.Append("   <td>Đang hiện</td>");
                    }
                    else
                    {
                        html.Append("   <td>Đang ẩn</td>");
                    }
                    html.Append("   <td>" + item.CategoryName + "</td>");
                    if (item.WebPublish == false)
                    {
                        html.Append("   <td data-title='Trang xem hàng'><span id='showWebPublish_" + item.ID + "'><a href='javascript:;' data-post-id='" + item.ID + "' data-update='true' class='bg-black bg-button' onclick='updateShowWebPublish($(this))'>Đang ẩn</a></span></td>");
                    }
                    else
                    {
                        html.Append("   <td data-title='Trang xem hàng'><span id='showWebPublish_" + item.ID + "'><a href='javascript:;' data-post-id='" + item.ID + "' data-update='false' class='bg-green bg-button' onclick='updateShowWebPublish($(this))'>Đang hiện</a></span></td>");
                    }
                    html.Append("   <td>" + item.CreatedBy + "</td>");
                    string date = string.Format("{0:dd/MM/yyyy}", item.CreatedDate);
                    html.Append("   <td>" + date + "</td>");
                    html.Append("   <td>");
                    html.Append("       <a href='javascript:;' title='Xóa bài này' class='btn primary-btn btn-red h45-btn' onclick='deletePost(" + item.ID + ");'><i class='fa fa-times' aria-hidden='true'></i> Xóa</a>");
                    html.Append("       <a target='_blank' href='/sua-bai-viet?id=" + item.ID + "' title='Sửa bài này' class='btn primary-btn btn-blue h45-btn'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Sửa</a>");
                    html.Append("  </td>");
                    html.Append("</tr>");

                }

            }
            else
            {
                html.Append("<tr><td colspan='9'>Không tìm thấy bài viết...</td></tr>");
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearchPost.Text;
            string request = "/danh-sach-bai-viet?";

            if (search != "")
            {
                request += "&textsearch=" + search;
            }

            if (ddlStatus.SelectedValue != "")
            {
                request += "&status=" + ddlStatus.SelectedValue;
            }

            if (ddlCategory.SelectedValue != "0")
            {
                request += "&categoryid=" + ddlCategory.SelectedValue;
            }

            if (ddlWebPublish.SelectedValue != "")
            {
                request += "&webpublish=" + ddlWebPublish.SelectedValue;
            }

            if (ddlCreatedDate.SelectedValue != "")
            {
                request += "&createddate=" + ddlCreatedDate.SelectedValue;
            }

            if (ddlCreatedBy.SelectedValue != "")
            {
                request += "&createdby=" + ddlCreatedBy.SelectedValue;
            }

            Response.Redirect(request);
        }
    }
}