using IM_PJ.Models;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using WebUI.Business;

namespace IM_PJ.Controllers
{
    public class PostPublicController
    {
        internal static string checkSlug(string slug, int ID = 0)
        {
            using (var con = new inventorymanagementEntities())
            {
                var product = con.PostPublics.Where(x => x.ActionValue == slug && x.ID != ID).FirstOrDefault();
                if (product != null)
                {
                    return checkSlug(slug + "-1", ID);
                }
                else
                {
                    return slug;
                }
            }
        }
        #region CRUD
        public static PostPublic Insert(PostPublic data)
        {
            using (var con = new inventorymanagementEntities())
            {
                con.PostPublics.Add(data);
                con.SaveChanges();

                return data;
            }
        }
        public static PostPublic Update(PostPublic data)
        {
            using (var con = new inventorymanagementEntities())
            {
                var post = con.PostPublics.Where(o => o.ID == data.ID).FirstOrDefault();
                if (post != null)
                {
                    post.Action = data.Action;
                    post.ActionValue = data.ActionValue;
                    post.AtHome = data.AtHome;
                    post.CategoryID = data.CategoryID;
                    post.CategorySlug = data.CategorySlug;
                    post.Content = data.Content;
                    post.CreatedBy = data.CreatedBy;
                    post.CreatedDate = data.CreatedDate;
                    post.IsPolicy = data.IsPolicy;
                    post.ModifiedBy = data.ModifiedBy;
                    post.ModifiedDate = data.ModifiedDate;
                    post.Summary = data.Summary;
                    post.Thumbnail = data.Thumbnail;
                    post.Title = data.Title;
                    con.SaveChanges();

                    return post;
                }
                return null;
            }
        }

        public static string UpdateImage(int ID, string Thumbnail)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                PostPublic ui = dbe.PostPublics.Where(a => a.ID == ID).SingleOrDefault();
                if (ui != null)
                {
                    ui.Thumbnail = Thumbnail;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        public static string upTopPost(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                PostPublic ui = dbe.PostPublics.Where(a => a.ID == ID).SingleOrDefault();
                if (ui != null)
                {
                    ui.ModifiedDate = DateTime.Now;
                    ui.CreatedDate = DateTime.Now;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        public static string Delete(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                PostPublic ui = dbe.PostPublics.Where(o => o.ID == ID).FirstOrDefault();
                if (ui != null)
                {
                    dbe.PostPublics.Remove(ui);
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        #endregion
        #region Select
        public static PostPublic GetByID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                PostPublic ai = dbe.PostPublics.Where(a => a.ID == ID).SingleOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }
        public static List<PostPublic> GetAll(string s)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<PostPublic> ags = new List<PostPublic>();
                ags = dbe.PostPublics.Where(p => p.Title.Contains(s)).OrderByDescending(o => o.ID).ToList();
                return ags;
            }
        }
        public static List<PostPublic> GetByCategoryID(int CategoryID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<PostPublic> ags = new List<PostPublic>();
                ags = dbe.PostPublics.Where(p => p.CategoryID == CategoryID).OrderByDescending(o => o.ID).ToList();
                return ags;
            }
        }
        public static List<PostPublicSQL> GetAllSql(int categoryID, string textSearch, string AtHome, string CreatedDate, string CreatedBy)
        {
            var list = new List<PostPublicSQL>();
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("BEGIN");

            if (categoryID > 0)
            {
                sql.AppendLine(String.Empty);
                sql.AppendLine("WITH category AS(");
                sql.AppendLine("    SELECT");
                sql.AppendLine("            ID");
                sql.AppendLine("    ,       Name");
                sql.AppendLine("    ,       ParentID");
                sql.AppendLine("    FROM");
                sql.AppendLine("            PostCategory");
                sql.AppendLine("    WHERE");
                sql.AppendLine("            1 = 1");
                sql.AppendLine("    AND     ID = " + categoryID);
                sql.AppendLine("");
                sql.AppendLine("    UNION ALL");
                sql.AppendLine("");
                sql.AppendLine("    SELECT");
                sql.AppendLine("            CHI.ID");
                sql.AppendLine("    ,       CHI.Name");
                sql.AppendLine("    ,       CHI.ParentID");
                sql.AppendLine("    FROM");
                sql.AppendLine("            category AS PAR");
                sql.AppendLine("    INNER JOIN PostCategory AS CHI");
                sql.AppendLine("        ON PAR.ID = CHI.ParentID");
                sql.AppendLine(")");
                sql.AppendLine("SELECT");
                sql.AppendLine("        ID");
                sql.AppendLine(",       Name");
                sql.AppendLine(",       ParentID");
                sql.AppendLine("INTO #category");
                sql.AppendLine("FROM category");
            }

            sql.AppendLine(String.Empty);
            sql.AppendLine("    SELECT");
            sql.AppendLine("            POS.*");
            sql.AppendLine("    INTO #Post");
            sql.AppendLine("    FROM");
            sql.AppendLine("            PostPublic AS POS");
            sql.AppendLine("    WHERE");
            sql.AppendLine("            1 = 1");

            if (!string.IsNullOrEmpty(textSearch))
            {
                sql.AppendLine("    AND (POS.Title like N'%" + textSearch + "%')");
            }

            if (!string.IsNullOrEmpty(AtHome))
            {
                sql.AppendLine("    AND (POS.AtHome = '" + AtHome + "')");
            }

            if (!string.IsNullOrEmpty(CreatedBy))
            {
                sql.AppendLine("    AND (POS.CreatedBy = '" + CreatedBy + "')");
            }

            if (!string.IsNullOrEmpty(CreatedDate))
            {
                DateTime fromdate = DateTime.Today;
                DateTime todate = DateTime.Now;
                switch (CreatedDate)
                {
                    case "today":
                        fromdate = DateTime.Today;
                        todate = DateTime.Now;
                        break;
                    case "yesterday":
                        fromdate = fromdate.AddDays(-1);
                        todate = DateTime.Today;
                        break;
                    case "beforeyesterday":
                        fromdate = DateTime.Today.AddDays(-2);
                        todate = DateTime.Today.AddDays(-1);
                        break;
                    case "week":
                        int days = DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.Today.DayOfWeek;
                        fromdate = fromdate.AddDays(-days + 1);
                        todate = DateTime.Now;
                        break;
                    case "thismonth":
                        fromdate = new DateTime(fromdate.Year, fromdate.Month, 1);
                        todate = DateTime.Now;
                        break;
                    case "lastmonth":
                        var thismonth = new DateTime(fromdate.Year, fromdate.Month, 1);
                        fromdate = thismonth.AddMonths(-1);
                        todate = thismonth;
                        break;
                    case "7days":
                        fromdate = DateTime.Today.AddDays(-6);
                        todate = DateTime.Now;
                        break;
                    case "30days":
                        fromdate = DateTime.Today.AddDays(-29);
                        todate = DateTime.Now;
                        break;
                }
                sql.AppendLine(String.Format("	AND	(CONVERT(NVARCHAR(10), POS.CreatedDate, 121) BETWEEN CONVERT(NVARCHAR(10), '{0:yyyy-MM-dd}', 121) AND CONVERT(NVARCHAR(10), '{1:yyyy-MM-dd}', 121))", fromdate, todate));
            }

            if (categoryID > 0)
            {
                sql.AppendLine("    AND EXISTS(");
                sql.AppendLine("            SELECT");
                sql.AppendLine("                    NULL AS DUMMY");
                sql.AppendLine("            FROM");
                sql.AppendLine("                    #category");
                sql.AppendLine("            WHERE");
                sql.AppendLine("                    ID = POS.CategoryID");
                sql.AppendLine("    )");
            }

            sql.AppendLine("     ORDER BY");
            sql.AppendLine("            POS.ID");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine("     SELECT");
            sql.AppendLine("             c.Name AS CategoryName");
            sql.AppendLine("     ,       p.*");
            sql.AppendLine("     FROM");
            sql.AppendLine("             #Post AS p");
            sql.AppendLine("     LEFT JOIN (");
            sql.AppendLine("             SELECT");
            sql.AppendLine("                     ID");
            sql.AppendLine("             ,       Name");
            sql.AppendLine("             FROM");
            sql.AppendLine("                     dbo.PostCategory");
            sql.AppendLine("     ) AS c");
            sql.AppendLine("     ON c.ID = p.CategoryID");
            sql.AppendLine("     ORDER BY");
            sql.AppendLine("             p.ID");
            sql.AppendLine("     ;");
            sql.AppendLine(String.Empty);
            sql.AppendLine(" END");

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql.ToString());

            while (reader.Read())
            {

                var entity = new PostPublicSQL();

                entity.ID = Convert.ToInt32(reader["ID"]);

                if (!string.IsNullOrEmpty(reader["Thumbnail"].ToString()))
                {
                    entity.Thumbnail = reader["Thumbnail"].ToString();
                }
                else
                {
                    entity.Thumbnail = "/App_Themes/Ann/image/placeholder.png";
                }

                if (reader["Title"] != DBNull.Value)
                    entity.Title = reader["Title"].ToString();
                if (reader["Content"] != DBNull.Value)
                    entity.Content = reader["Content"].ToString();
                if (reader["CategoryName"] != DBNull.Value)
                    entity.CategoryName = reader["CategoryName"].ToString();
                if (reader["CategoryID"] != DBNull.Value)
                    entity.CategoryID = reader["CategoryID"].ToString().ToInt(0);
                if (reader["CreatedBy"] != DBNull.Value)
                    entity.CreatedBy = reader["CreatedBy"].ToString();
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                if (reader["AtHome"] != DBNull.Value)
                    entity.AtHome = reader["AtHome"].ToString().ToBool();
                list.Add(entity);
            }
            reader.Close();

            return list.OrderByDescending(x => x.CreatedDate).ToList();
        }

        public static string updateAtHome(int id, bool value)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                PostPublic ui = dbe.PostPublics.Where(a => a.ID == id).SingleOrDefault();
                if (ui != null)
                {
                    ui.AtHome = value;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        #endregion
        #region Class
        public class PostPublicSQL
        {
            public int ID { get; set; }
            public Nullable<int> CategoryID { get; set; }
            public string CategorySlug { get; set; }
            public string CategoryName { get; set; }
            public string Title { get; set; }
            public string Thumbnail { get; set; }
            public string Summary { get; set; }
            public string Content { get; set; }
            public string Action { get; set; }
            public string ActionValue { get; set; }
            public bool AtHome { get; set; }
            public bool IsPolicy { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<System.DateTime> CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public Nullable<System.DateTime> ModifiedDate { get; set; }
        }

        #endregion
    }
}