﻿using IM_PJ.Models;
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

                    connect.SaveChanges();
                }
            }
            return company.ID;
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
                        .Where(x => x.ID == ID && x.SubID == SubID)
                        .SingleOrDefault();
            }
        }

        public static List<tbl_TransportCompany> Filter(string TextSearch)
        {
            var list = new List<tbl_TransportCompany>();
            var sql = new StringBuilder();

            sql.AppendLine(String.Format("SELECT * "));
            sql.AppendLine(String.Format("FROM tbl_TransportCompany"));
            sql.AppendLine(String.Format("WHERE SubID = 0"));

            if (TextSearch != "")
            {
                sql.AppendLine(String.Format("	AND ( (CompanyName LIKE N'%{0}%') OR (CompanyAddress LIKE N'%{0}%') OR (ShipTo LIKE N'%{0}%') OR (Address LIKE N'%{0}%') OR (CompanyPhone = '{0}') )", TextSearch));
            }

            sql.AppendLine(String.Format("ORDER BY CompanyName"));

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql.ToString());
            while (reader.Read())
            {
                var entity = new tbl_TransportCompany();

                entity.ID = Convert.ToInt32(reader["ID"]);
                entity.SubID = Convert.ToInt32(reader["ID"]);
                entity.CompanyName = reader["CompanyName"].ToString();
                entity.CompanyPhone = reader["CompanyPhone"].ToString();
                entity.CompanyAddress = reader["CompanyAddress"].ToString();
                entity.ShipTo = reader["ShipTo"].ToString();
                entity.Address = reader["Address"].ToString();
                entity.Prepay = Convert.ToBoolean(reader["Prepay"]);
                entity.COD = Convert.ToBoolean(reader["COD"]);
                entity.Note = reader["Note"].ToString();
                entity.CreatedBy = reader["CreatedBy"].ToString();
                entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);

                list.Add(entity);
            }
            reader.Close();
            return list;
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
                return connect.tbl_TransportCompany.Where(x => x.SubID == 0).OrderBy(x => x.CompanyName).ToList();
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
                return connect.tbl_TransportCompany.Where(x => x.ID == ID && x.SubID != 0).ToList();
            }
        }
        #endregion
    }
}