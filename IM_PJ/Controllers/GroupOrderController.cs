using IM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace IM_PJ.Controllers
{
    public class GroupOrderController
    {
        /// <summary>
        /// Lấy thông tin đơn hàng gộp
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static GroupOrder getByCode(string code)
        {
            using (var con = new inventorymanagementEntities())
            {
                var groupOrder = con.GroupOrders
                    .Where(x => x.Code == code)
                    .SingleOrDefault();

                return groupOrder;
            }
        }

        /// <summary>
        /// Lấy danh sách ID đơn đổi trả
        /// </summary>
        /// <param name="groupCode">Mã đơn gộp</param>
        /// <returns></returns>
        public static IList<int> getRefundIds(string groupCode)
        {
            using (var con = new inventorymanagementEntities())
            {
                var refundIds = con.tbl_Order
                    .Where(x => x.GroupCode == groupCode)
                    .Where(x => x.RefundsGoodsID.HasValue)
                    .Select(x => x.RefundsGoodsID.Value)
                    .ToList();

                return refundIds;
            }
        }
    }
}