using System;

namespace IM_PJ.Models
{
    public class CustomerResponseModel
    {
        public int ID { get; set; }
        public string Nick { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
        public int? CustomerLevelID { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsHidden { get; set; }
        public string Zalo { get; set; }
        public string Facebook { get; set; }
        public string Note { get; set; }
        public int? ProvinceID { get; set; }
        public string Avatar { get; set; }
        public int? ShippingType { get; set; }
        public int? PaymentType { get; set; }
        public int? TransportCompanyID { get; set; }
        public int? TransportCompanySubID { get; set; }
        public string CustomerPhone2 { get; set; }
        public string CustomerPhoneBackup { get; set; }
        public string UnSignedNick { get; set; }
        public string UnSignedName { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public int? SendSMSIntroApp { get; set; }

        #region Mapper
        public static CustomerResponseModel map (tbl_Customer source)
        {
            var result = new CustomerResponseModel()
            {
               ID = source.ID,
                Nick = source.Nick,
                CustomerName = source.CustomerName,
                CustomerPhone = source.CustomerPhone,
                CustomerAddress = source.CustomerAddress,
                CustomerEmail = source.CustomerEmail,
                CustomerLevelID = source.CustomerLevelID,
                Status = source.Status,
                CreatedDate = source.CreatedDate,
                CreatedBy = source.CreatedBy,
                ModifiedDate = source.ModifiedDate,
                ModifiedBy = source.ModifiedBy,
                IsHidden = source.IsHidden,
                Zalo = source.Zalo,
                Facebook = source.Facebook,
                Note = source.Note,
                ProvinceID = source.ProvinceID,
                Avatar = source.Avatar,
                ShippingType = source.ShippingType,
                PaymentType = source.PaymentType,
                TransportCompanyID = source.TransportCompanyID,
                TransportCompanySubID = source.TransportCompanySubID,
                CustomerPhone2 = source.CustomerPhone2,
                CustomerPhoneBackup = source.CustomerPhoneBackup,
                UnSignedNick = source.UnSignedNick,
                UnSignedName = source.UnSignedName,
                DistrictId = source.DistrictId,
                WardId = source.WardId,
                SendSMSIntroApp = source.SendSMSIntroApp,
            };

            return result;
        }
        #endregion
    }
}