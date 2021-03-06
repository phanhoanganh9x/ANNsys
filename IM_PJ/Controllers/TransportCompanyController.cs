﻿using IM_PJ.Models;
using MB.Extensions;
using NHST.Bussiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using WebUI.Business;

namespace IM_PJ.Controllers
{
    public class TransportCompanyController
    {
        #region CRUD
        /// <summary>
        /// Insert transport company new
        /// </summary>
        /// <param name="company"></param>
        public static int InsertTransportCompany(tbl_TransportCompany company)
        {
            using (var connect = new inventorymanagementEntities())
            {
                tbl_TransportCompany companyNew = new tbl_TransportCompany();

                companyNew.ID = GetIDNew();
                companyNew.SubID = 0;
                companyNew.CompanyName = company.CompanyName;
                companyNew.CompanyPhone = company.CompanyPhone;
                companyNew.CompanyAddress = company.CompanyAddress;
                companyNew.ShipTo = null;
                companyNew.Address = null;
                companyNew.Prepay = company.Prepay;
                companyNew.COD = company.COD;
                companyNew.Note = company.Note;
                companyNew.CreatedDate = DateTime.Now;
                companyNew.CreatedBy = company.CreatedBy;
                companyNew.ModifiedDate = null;
                companyNew.ModifiedBy = null;
                companyNew.Status = company.Status;

                connect.tbl_TransportCompany.Add(companyNew);
                connect.SaveChanges();
                return companyNew.ID;
            }
        }

        /// <summary>
        /// Insert receive place new of transport company
        /// </summary>
        /// <param name="company"></param>
        public static void InsertReceivePlace(tbl_TransportCompany company)
        {
            using (var connect = new inventorymanagementEntities())
            {
                tbl_TransportCompany companyNew = new tbl_TransportCompany();

                companyNew.ID = company.ID;
                companyNew.SubID = GetSubIDNew(company.ID);
                companyNew.CompanyName = company.CompanyName;
                companyNew.CompanyPhone = company.CompanyPhone;
                companyNew.CompanyAddress = company.CompanyAddress;
                companyNew.ShipTo = company.ShipTo;
                companyNew.Address = company.Address;
                companyNew.Prepay = company.Prepay;
                companyNew.COD = company.COD;
                companyNew.Note = company.Note;
                companyNew.CreatedDate = DateTime.Now;
                companyNew.CreatedBy = company.CreatedBy;
                companyNew.ModifiedDate = null;
                companyNew.ModifiedBy = null;
                companyNew.Status = company.Status;

                connect.tbl_TransportCompany.Add(companyNew);
                connect.SaveChanges();
            }
        }

        /// <summary>
        /// update info of transport company
        /// </summary>
        /// <param name="company"></param>
        public static int UpdateTransportCompany(tbl_TransportCompany company)
        {
            using (var connect = new inventorymanagementEntities())
            {
                tbl_TransportCompany target = connect.tbl_TransportCompany
                    .Where(x => x.ID == company.ID && x.SubID == 0)
                    .SingleOrDefault();

                if (target != null)
                {
                    target.CompanyName = company.CompanyName;
                    target.CompanyPhone = company.CompanyPhone;
                    target.CompanyAddress = company.CompanyAddress;
                    target.Note = company.Note;
                    target.Prepay = company.Prepay;
                    target.COD = company.COD;
                    target.ModifiedDate = DateTime.Now;
                    target.ModifiedBy = company.ModifiedBy;
                    target.Status = target.Status;

                    connect.SaveChanges();
                }
            }
            return company.ID;
        }
        public static string UpdateStatus(int ID, int SubID)
        {
            using (var connect = new inventorymanagementEntities())
            {
                tbl_TransportCompany target = connect.tbl_TransportCompany
                    .Where(x => x.ID == ID && x.SubID == SubID)
                    .SingleOrDefault();

                if (target != null)
                {
                    target.Status = target.Status == 1 ? 0 : 1;
                    target.ModifiedDate = DateTime.Now;

                    connect.SaveChanges();
                    return "true";
                }
            }
            return "false";
        }
        /// <summary>
        /// update receive place of transport company
        /// </summary>
        /// <param name="company"></param>
        public static void UpdateReceivePlace(tbl_TransportCompany company)
        {
            using (var connect = new inventorymanagementEntities())
            {
                tbl_TransportCompany target = connect.tbl_TransportCompany
                    .Where(x => x.ID == company.ID && x.SubID == company.SubID)
                    .SingleOrDefault();

                if (target != null)
                {
                    target.ShipTo = company.ShipTo;
                    target.Address = company.Address;
                    target.Prepay = company.Prepay;
                    target.COD = company.COD;
                    target.Note = company.Note;
                    target.ModifiedDate = DateTime.Now;
                    target.ModifiedBy = company.ModifiedBy;
                    target.Status = target.Status;

                    connect.SaveChanges();
                }
            }
        }
        #endregion

        #region Select
        /// <summary>
        /// Get id new which will use to transport company new
        /// </summary>
        /// <returns></returns>
        private static int GetIDNew()
        {
            using (var connect = new inventorymanagementEntities())
            {
                try
                {
                    int idNew = connect.tbl_TransportCompany
                    .Where(x => x.SubID == 0)
                    .OrderByDescending(x => x.ID)
                    .FirstOrDefault()
                    .ID;

                    return idNew + 1;
                }
                catch (Exception)
                {
                    return 1;
                }

            }
        }

        /// <summary>
        /// Get sub id new which will use to receive place new of transport company
        /// </summary>
        /// <returns></returns>
        private static int GetSubIDNew(int ID)
        {
            using (var connect = new inventorymanagementEntities())
            {
                int idNew = connect.tbl_TransportCompany
                    .Where(x => x.ID == ID)
                    .OrderByDescending(x => x.SubID)
                    .FirstOrDefault()
                    .SubID;

                return idNew + 1;

            }
        }

        /// <summary>
        /// get transport company by id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static tbl_TransportCompany GetTransportCompanyByID(int ID)
        {
            using (var connect = new inventorymanagementEntities())
            {
                return connect.tbl_TransportCompany
                        .Where(x => x.ID == ID && x.SubID == 0 && x.Status == 1)
                        .SingleOrDefault();
            }
        }
        public static tbl_TransportCompany GetTransportCompanyForOrderList(int ID)
        {
            using (var connect = new inventorymanagementEntities())
            {
                return connect.tbl_TransportCompany
                        .Where(x => x.ID == ID && x.SubID == 0)
                        .SingleOrDefault();
            }
        }
        public static tbl_TransportCompany GetAllTransportCompanyByID(int ID)
        {
            using (var connect = new inventorymanagementEntities())
            {
                return connect.tbl_TransportCompany
                        .Where(x => x.ID == ID && x.SubID == 0)
                        .SingleOrDefault();
            }
        }
        /// <summary>
        /// get receive place by id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static tbl_TransportCompany GetReceivePlaceByID(int ID, int SubID)
        {
            using (var connect = new inventorymanagementEntities())
            {
                return connect.tbl_TransportCompany
                        .Where(x => x.ID == ID && x.SubID == SubID && x.Status == 1)
                        .SingleOrDefault();
            }
        }
        public static tbl_TransportCompany GetReceivePlaceForOrderList(int ID, int SubID)
        {
            using (var connect = new inventorymanagementEntities())
            {
                return connect.tbl_TransportCompany
                        .Where(x => x.ID == ID && x.SubID == SubID)
                        .SingleOrDefault();
            }
        }
        public static tbl_TransportCompany GetAllReceivePlaceByID(int ID, int SubID)
        {
            using (var connect = new inventorymanagementEntities())
            {
                return connect.tbl_TransportCompany
                        .Where(x => x.ID == ID && x.SubID == SubID)
                        .SingleOrDefault();
            }
        }
        public static List<tbl_TransportCompany> Filter(string TextSearch)
        {
            using (var con = new inventorymanagementEntities())
            {
                if (!String.IsNullOrEmpty(TextSearch))
                {
                    var unsignTextSearch = UnSign.convert(TextSearch);

                    var tran = con.tbl_TransportCompany
                        .Where(x => x.SubID == 0)
                        .OrderBy(x => x.CompanyName)
                        .ToList();

                    var tranSub = con.tbl_TransportCompany
                        .Where(x => x.SubID != 0)
                        .OrderBy(x => x.CompanyName)
                        .ToList();

                    var data = tran
                        .GroupJoin(
                            tranSub,
                            t => t.ID,
                            tb => tb.ID,
                            (t, tb) => new { t, tb }
                        )
                        .SelectMany(
                            x => x.tb.DefaultIfEmpty(),
                            (parent, child) => new {
                                transfor = parent.t,
                                CompanyName = parent.t.CompanyName,
                                CompanyAddress = parent.t.CompanyAddress,
                                CompanyPhone = parent.t.CompanyPhone,
                                ShipTo = child != null? child.ShipTo : String.Empty
                            }
                        )
                        .Where(x =>
                            UnSign.convert(x.CompanyName).Contains(unsignTextSearch) ||
                            UnSign.convert(x.CompanyAddress).Contains(unsignTextSearch) ||
                            UnSign.convert(x.ShipTo).Contains(unsignTextSearch) ||
                            x.CompanyPhone == unsignTextSearch
                        )
                        .Select(x => x.transfor)
                        .OrderBy(x => x.CompanyName)
                        .ToList();

                    return data;
                }
                else
                {
                    return con.tbl_TransportCompany
                        .Where(x => x.SubID == 0)
                        .OrderBy(x => x.CompanyName)
                        .ToList();
                }
            }
        }

        /// <summary>
        /// Get list transport company
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static List<tbl_TransportCompany> GetTransportCompany()
        {
            using (var connect = new inventorymanagementEntities())
            {
                return connect.tbl_TransportCompany.Where(x => x.SubID == 0 && x.Status == 1).OrderBy(x => x.CompanyName).ToList();
            }
        }

        /// <summary>
        /// Get list of receive places
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static List<tbl_TransportCompany> GetReceivePlace(int ID)
        {
            using (var connect = new inventorymanagementEntities())
            {
                return connect.tbl_TransportCompany.Where(x => x.ID == ID && x.SubID != 0 && x.Status == 1).OrderByDescending(x => x.CompanyName).ToList();
            }
        }
        public static List<tbl_TransportCompany> GetAllReceivePlace(int ID)
        {
            using (var connect = new inventorymanagementEntities())
            {
                return connect.tbl_TransportCompany.Where(x => x.ID == ID && x.SubID != 0).OrderByDescending(x => x.CompanyName).ToList();
            }
        }
        #endregion

        public static List<ListItem> getDropDownListTrans()
        {
            var data = new List<ListItem>();
            data.Add(new ListItem(String.Empty, "0"));
            using (var con = new inventorymanagementEntities())
            {
                foreach (var tran in con.tbl_TransportCompany.Where(x => x.SubID == 0 && x.Status == 1).OrderByDescending(x => x.CompanyName).ToList())
                {
                    data.Add(new ListItem(tran.CompanyName, tran.ID.ToString()));
                }
            }

            return data;
        }
    }
}