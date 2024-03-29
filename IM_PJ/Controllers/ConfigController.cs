﻿using IM_PJ.Models;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Controllers
{
    public class ConfigController
    {
        #region CRUD
        public static string Update(
            int ID, 
            double NumOfDateToChangeProduct, 
            double NumOfProductCanChange, 
            double FeeChangeProduct, 
            double FeeDiscountPerProduct,
            string ChangeGoodsRule, 
            string RetailReturnRule, 
            DateTime ModifiedDate, 
            string ModifiedBy, 
            string CSSPrintBarcode, 
            int HideProduct, 
            int ViewAllOrders, 
            int ViewAllReports,
            string WPNewsClothes,
            string WPNewsCosmetics,
            string WPZaloClothes,
            string WPZaloCosmetics,
            string WPFBClothes,
            string WPFBCosmetics,
            string WPHotlineClothes,
            string WPHotlineCosmetics,
            string WPImageClothes,
            string WPImageCosmetics,
            string WPNewsLixi,
            string WPHotlineLixi,
            string WPZaloLixi,
            string WPCategoryCosmetics,
            string WPCategoryPerfume,
            string WPVideoClothes,
            string WPVideoCosmetics,
            string WPBannerTop,
            string WPBannerProduct,
            string WPTime
            )
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Config ui = dbe.tbl_Config.Where(a => a.ID == ID).SingleOrDefault();
                if (ui != null)
                {
                    ui.NumOfDateToChangeProduct = NumOfDateToChangeProduct;
                    ui.NumOfProductCanChange = NumOfProductCanChange;
                    ui.FeeChangeProduct = FeeChangeProduct;
                    ui.FeeDiscountPerProduct = FeeDiscountPerProduct;
                    ui.ChangeGoodsRule = ChangeGoodsRule;
                    ui.RetailReturnRule = RetailReturnRule;
                    ui.ModifiedBy = ModifiedBy;
                    ui.ModifiedDate = ModifiedDate;
                    ui.CSSPrintBarcode = CSSPrintBarcode;
                    ui.HideProduct = HideProduct;
                    ui.ViewAllOrders = ViewAllOrders;
                    ui.ViewAllReports = ViewAllReports;

                    ui.WPFBClothes = WPFBClothes;
                    ui.WPFBCosmetics = WPFBCosmetics;
                    ui.WPNewsClothes = WPNewsClothes;
                    ui.WPNewsCosmetics = WPNewsCosmetics;
                    ui.WPZaloClothes = WPZaloClothes;
                    ui.WPZaloCosmetics = WPZaloCosmetics;
                    ui.WPHotlineClothes = WPHotlineClothes;
                    ui.WPHotlineCosmetics = WPHotlineCosmetics;
                    ui.WPImageClothes = WPImageClothes;
                    ui.WPImageCosmetics = WPImageCosmetics;
                    ui.WPNewsLixi = WPNewsLixi;
                    ui.WPHotlineLixi = WPHotlineLixi;
                    ui.WPZaloLixi = WPZaloLixi;
                    ui.WPCategoryCosmetics = WPCategoryCosmetics;
                    ui.WPCategoryPerfume = WPCategoryPerfume;
                    ui.WPVideoClothes = WPVideoClothes;
                    ui.WPVideoCosmetics = WPVideoCosmetics;
                    ui.WPBannerTop = WPBannerTop;
                    ui.WPBannerProduct = WPBannerProduct;
                    ui.WPTime = WPTime;

                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }
        public static string UpdateSecurityCode(string SecurityCode)
        {
            using (var dbe = new inventorymanagementEntities())
            {

                var a = dbe.tbl_Config.Where(ac => ac.ID == 1).SingleOrDefault();
                if (a != null)
                {
                    a.SecurityCode = PJUtils.Encrypt("scode", SecurityCode);
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                return null;
            }
        }
        #endregion
        #region Select
        public static tbl_Config GetByID(int ID)
        {
            using (var dbe = new inventorymanagementEntities())
            {
                tbl_Config ai = dbe.tbl_Config.Where(a => a.ID == ID).SingleOrDefault();
                if (ai != null)
                {
                    return ai;
                }
                else return null;

            }
        }
        public static tbl_Config GetByTop1()
        {
            using (var dbe = new inventorymanagementEntities())
            {
                var conf = dbe.tbl_Config.Where(c => c.ID == 1).FirstOrDefault();
                if (conf != null)
                {
                    return conf;
                }
                else
                    return null;
            }
        }
        #endregion
    }
}