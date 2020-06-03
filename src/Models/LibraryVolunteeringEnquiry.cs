using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StockportGovUK.NetStandard.Models.Addresses;

namespace library_volunteering_enquiry_service.Models
{
    public class LibraryVolunteeringEnquiry
    {
        [Required]
        public List<string> InterestList { get; set; }

        [Required]
        public List<string> PreferredLocationList { get; set; }

        [Required]
        public int NumberOfHours { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public Address CustomersAddress { get; set; }

        public List<string> NotAvailableList { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
