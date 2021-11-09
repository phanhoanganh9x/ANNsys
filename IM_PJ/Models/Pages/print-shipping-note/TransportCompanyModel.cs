using MB.Extensions;

namespace IM_PJ.Models.Pages.print_shipping_note
{
    public class TransportCompanyModel
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string shipTo { get; set; }
        public string note { get; set; }

        #region Mapper
        public static TransportCompanyModel map(tbl_TransportCompany source, tbl_TransportCompany transportSub)
        {
            if (source == null || transportSub == null)
                return null;

            var transport = new TransportCompanyModel()
            {
                name = source.CompanyName.ToTitleCase(),
                phone = source.CompanyPhone,
                address = source.CompanyAddress.ToTitleCase(),
                shipTo = transportSub.ShipTo.ToTitleCase(),
                note = source.Note.ToTitleCase()
            };

            return transport;
        }
        #endregion
    }
}