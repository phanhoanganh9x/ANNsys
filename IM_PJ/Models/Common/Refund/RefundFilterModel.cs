using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ.Models
{
    public class RefundFilterModel
    {
        // Refund: Mã code
        // Order: Mã code
        // Khách hàng: Tên khách hàng, nick, số điện thoại
        // Sản phâm: SKU
        public string search { get; set; } = String.Empty;
        // Trạng thái đơn đổi trả
        public int status { get; set; }
        // Trạng thái phí đổi trả
        public string feeStatus { get; set; }
        // Thời gian hàng ( 'today': Hôm nay | 'yesterday': Hôm qua | 'beforeyesterday': Hôm kia | 'week': Tuần nay |
        // '7days': 7 ngày | 'thismonth': Tháng này | 'lastmonth': Tháng trước | 'beforelastmonth': Tháng trước nữa | '30days': 30 ngày) 
        public string dateTimePicker { get; set; }
        // Nhân viên tạo đơn
        public string staff { get; set; }
    }
}