using System.Collections.Generic;
using StockportGovUK.NetStandard.Models.Addresses;

namespace library_volunteering_enquiry_service.Models
{
    public class LibraryVolunteeringEnquiry
    {
        public List<string> InterestList { get; set; }
        public List<string> PreferredLocationList { get; set; }
        public int NumberOfHours { get; set; }
        public List<string> NotAvailableList { get; set; }
        public string AdditionalInfo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Address CustomersAddress { get; set; }
    }
}
