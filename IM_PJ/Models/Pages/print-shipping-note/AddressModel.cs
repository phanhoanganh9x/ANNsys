using MB.Extensions;
using System;
using System.Linq;

namespace IM_PJ.Models.Pages.print_shipping_note
{
    public class AddressModel
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string phone2 { get; set; }
        public string provinceName { get; set;}
        public string districtName { get; set;}
        public string wardName { get; set;}
        public string address { get; set; }

        #region Mapper
        public static AddressModel map(tbl_Agent source)
        {
            var phones = source.AgentPhone
                .Split('-')
                .Where(x => !String.IsNullOrEmpty(x))
                .Select(x => x.Trim())
                .ToList();

            var address = new AddressModel() {
                name = source.AgentLeader,
                phone = phones.FirstOrDefault(),
                phone2 = phones.Count > 1 ? phones.ElementAt(1) : null,
                address = source.AgentAddress
            };

            return address;
        }

        public static AddressModel map(
            DeliveryAddress source,
            DeliverySaveAddress province,
            DeliverySaveAddress district,
            DeliverySaveAddress ward
        ) {
            var address = new AddressModel() {
                name = source.FullName.ToTitleCase(),
                address = source.Address.ToTitleCase()
            };

            if (!String.IsNullOrEmpty(source.Phone))
            {
                address.phone = source.Phone;

                if (source.Phone.Length == 10)
                {
                    var phone1 = source.Phone.Substring(0, 4);
                    var phone2 = source.Phone.Substring(4, 3);
                    var phone3 = source.Phone.Substring(7);

                    address.phone = String.Format("{0}.{1}.{2}", phone1, phone2, phone3);
                }
            }

            if (province != null)
                address.provinceName = province.Name;

            if (district != null)
                address.districtName = district.Name;

            if (ward != null)
                address.wardName = ward.Name;

            return address;
        }

        public static AddressModel map(
            tbl_Customer source,
            DeliverySaveAddress province,
            DeliverySaveAddress district,
            DeliverySaveAddress ward
        ) {
            var address = new AddressModel() {
                name = source.CustomerName.ToTitleCase(),
                address = source.CustomerAddress.ToTitleCase()
            };

            if (!String.IsNullOrEmpty(source.CustomerPhone))
            {
                address.phone = source.CustomerPhone;

                if (source.CustomerPhone.Length == 10)
                {
                    var phone1 = source.CustomerPhone.Substring(0, 4);
                    var phone2 = source.CustomerPhone.Substring(4, 3);
                    var phone3 = source.CustomerPhone.Substring(7);

                    address.phone = String.Format("{0}.{1}.{2}", phone1, phone2, phone3);
                }
            }

            if (!String.IsNullOrEmpty(source.CustomerPhone2))
            {
                address.phone = source.CustomerPhone2;

                if (source.CustomerPhone2.Length == 10)
                {
                    var phone1 = source.CustomerPhone2.Substring(0, 4);
                    var phone2 = source.CustomerPhone2.Substring(4, 3);
                    var phone3 = source.CustomerPhone2.Substring(7);

                    address.phone2 = String.Format("{0}.{1}.{2}", phone1, phone2, phone3);
                }
            }

            if (province != null)
                address.provinceName = province.Name;

            if (district != null)
                address.districtName = district.Name;

            if (ward != null)
                address.wardName = ward.Name;

            return address;
        }
        #endregion
    }
}