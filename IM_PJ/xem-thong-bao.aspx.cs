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

namespace IM_PJ
{
    public partial class xem_thong_bao : System.Web.UI.Page
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
            string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
            var acc = AccountController.GetByUsername(username);

            int id = Request.QueryString["id"].ToInt(0);
            if (id > 0)
            {
                var p = NotifyController.GetByID(id);
                if (p == null)
                {
                    PJUtils.ShowMessageBoxSwAlertError("Không tìm thấy bài viết " + id, "e", true, "/danh-sach-thong-bao", Page);
                }
                else
                {
                    ltrThumbnail.Text = "<img src='" + p.Thumbnail + "'>";
                    ltrSummary.Text = p.Summary;
                    if (p.Action == "show_web")
                    {
                        ltrLink.Text = "<p><strong>Link:</strong> <a href='" + p.ActionValue + "' target='_blank'>" + p.ActionValue + "</a></p>";
                    }
                    else
                    {
                        ltrContent.Text = p.Content;
                    }
                    this.Title = String.Format("{0} - Thông báo", p.Title.ToTitleCase());
                    ltrEditTop.Text = "";
                    if (acc.RoleID == 0 || acc.Username == "nhom_zalo502")
                    {
                        ltrEditTop.Text += "<a href='/sua-thong-bao?id=" + p.ID + "' class='btn primary-btn fw-btn not-fullwidth'><i class='fa fa-pencil-square-o' aria-hidden='true'></i> Chỉnh sửa</a>";
                        ltrEditTop.Text += "<a href='/tao-thong-bao' class='btn primary-btn fw-btn not-fullwidth print-invoice-merged'><i class='fa fa-file-text-o' aria-hidden='true'></i> Thêm mới</a>";
                    }
                    ltrEditBottom.Text = ltrEditTop.Text;
                    ltrTitle.Text = p.Title;
                    
                    string Action = "";
                    if (p.Action == "show_web")
                    {
                        Action = "Link ngoài";
                    }
                    else
                    {
                        Action = "Bài nội bộ";
                    }
                    string PostInfo = "<p><strong>Kiểu bài viết</strong>: " + Action + "</p>";
                    PostInfo += "<p><strong>Ngày tạo</strong>: " + p.CreatedDate + "</p>";
                    PostInfo += "<p><strong>Người viết</strong>: " + p.CreatedBy + "</p>";
                    PostInfo += "<p><strong>Ngày cập nhật</strong>: " + p.ModifiedDate + "</p>";
                    PostInfo += "<p><strong>Người cập nhật</strong>: " + p.ModifiedBy + "</p>";
                    ltrPostInfo.Text = PostInfo;
                }
            }
        }
    }
}