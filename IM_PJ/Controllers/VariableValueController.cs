using IM_PJ.Models;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Controllers
{
    public class VariableValueController
    {
        #region CRUD
        public static string Insert(int VariableID, string VariableName, string VariableValue, bool IsHidden,
            DateTime CreatedDate, string CreatedBy, string SKUText)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_VariableValue ui = new tbl_VariableValue();
                ui.VariableID = VariableID;
                ui.VariableName = VariableName;
                ui.VariableValue = VariableValue;
                ui.IsHidden = IsHidden;
                ui.CreatedDate = CreatedDate;
                ui.CreatedBy = CreatedBy;
                ui.SKUText = SKUText;
                dbe.tbl_VariableValue.Add(ui);
                int kq = dbe.SaveChanges();
                return kq.ToString();
            }
        }
        public static string Update(int ID, int VariableID, string VariableName, string VariableValue, bool IsHidden,
            DateTime ModifiedDate, string ModifiedBy,string SKUText)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_VariableValue ui = dbe.tbl_VariableValue.Where(a => a.ID == ID).SingleOrDefault();
                if (ui != null)
                {
                    ui.VariableID = VariableID;
                    ui.VariableName = VariableName;
                    ui.VariableValue = VariableValue;
                    ui.IsHidden = IsHidden;
                    ui.ModifiedBy = ModifiedBy;
                    ui.ModifiedDate = ModifiedDate;
                    ui.SKUText = SKUText;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        public static string UpdateVariableValueText(int ID, string VariableValueText)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_VariableValue ui = dbe.tbl_VariableValue.Where(a => a.ID == ID).SingleOrDefault();
                if (ui != null)
                {                    
                    ui.VariableValueText = VariableValueText;                    
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        #endregion
        #region Select
        public static tbl_VariableValue GetByID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_VariableValue ai = dbe.tbl_VariableValue.Where(a => a.ID == ID).FirstOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }
        public static tbl_VariableValue GetEnNameByName(string variableValue)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_VariableValue ai = dbe.tbl_VariableValue.Where(a => a.VariableValue == variableValue).FirstOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }
        public static tbl_VariableValue GetByNameAndValue(string variableName, string variableValue)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_VariableValue ai = dbe.tbl_VariableValue.Where(a => a.VariableName == variableName && a.VariableValue == variableValue).FirstOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }

        public static tbl_VariableValue GetByValueAndSKUText(int variableName, string variableValue, string skutext)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_VariableValue ai = dbe.tbl_VariableValue.Where(a => a.SKUText.Trim() == skutext && a.VariableID == variableName || a.VariableValue.Trim() == variableValue && a.VariableID == variableName).FirstOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }
        public static List<tbl_VariableValue> GetAll(string s)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_VariableValue> ags = new List<tbl_VariableValue>();
                ags = dbe.tbl_VariableValue.Where(v => v.VariableName.Contains(s)).OrderBy(o => o.ID).ToList();
                return ags;
            }
        }
        public static List<tbl_VariableValue> GetByVariableID(int VariableID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_VariableValue> ags = new List<tbl_VariableValue>();
                ags = dbe.tbl_VariableValue.Where(v => v.VariableID == VariableID).OrderBy(o => o.ID).ToList();
                return ags;
            }
        }
        public static List<tbl_VariableValue> GetByVariableIDIsHidden(int VariableID, bool IsHidden)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                List<tbl_VariableValue> ags = new List<tbl_VariableValue>();
                ags = dbe.tbl_VariableValue.Where(v => v.VariableID == VariableID && v.IsHidden == IsHidden).OrderBy(o => o.ID).ToList();
                return ags;
            }
        }
        #endregion
    }
}