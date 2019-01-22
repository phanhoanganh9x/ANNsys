﻿using IM_PJ.Models;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Controllers
{
    public class PostCategoryController
    {
        #region CRUD
        public static int Insert(string Title, int ParentID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_PostCategory ui = new tbl_PostCategory();
                ui.Title = Title;
                ui.ParentID = ParentID;
                dbe.tbl_PostCategory.Add(ui);
                int kq = dbe.SaveChanges();
                return ui.ID;
            }
        }
        public static string Update(int ID, string Title, int ParentID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_PostCategory ui = dbe.tbl_PostCategory.Where(a => a.ID == ID).SingleOrDefault();
                if (ui != null)
                {
                    ui.Title = Title;
                    ui.ParentID = ParentID;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        #endregion
        #region Select
        public static tbl_PostCategory GetByID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_PostCategory ai = dbe.tbl_PostCategory.Where(a => a.ID == ID).SingleOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }
        public static List<tbl_PostCategory> GetAll()
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_PostCategory> ags = new List<tbl_PostCategory>();
                ags = dbe.tbl_PostCategory.ToList();
                return ags;
            }
        }
        public static List<tbl_PostCategory> GetAll(string s)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_PostCategory> ags = new List<tbl_PostCategory>();
                ags = dbe.tbl_PostCategory.Where(c => c.Title.Contains(s)).ToList();
                return ags;
            }
        }

        public static List<tbl_PostCategory> GetByParentID(string s, int ParentID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_PostCategory> ags = new List<tbl_PostCategory>();
                ags = dbe.tbl_PostCategory.Where(a => a.Title.Contains(s) && a.ParentID == ParentID).ToList();
                return ags;
            }
        }
        #endregion
        #region API
        public static List<tbl_PostCategory> API_GetAllCategory()
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_PostCategory> ags = new List<tbl_PostCategory>();
                ags = dbe.tbl_PostCategory.ToList();
                return ags;
            }
        }
        public static List<tbl_PostCategory> API_GetRootCategory()
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_PostCategory> ags = new List<tbl_PostCategory>();
                ags = dbe.tbl_PostCategory.Where(a => a.ParentID == 0).ToList();
                return ags;
            }
        }
        public static List<tbl_PostCategory> API_GetByParentID(int ParentID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_PostCategory> ags = new List<tbl_PostCategory>();
                ags = dbe.tbl_PostCategory.Where(a => a.ParentID == ParentID).ToList();
                return ags;
            }
        }
        #endregion
    }
}