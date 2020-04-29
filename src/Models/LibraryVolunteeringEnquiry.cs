﻿using System.Collections.Generic;

namespace library_volunteering_enquiry_service.Models
{
    public class LibraryVolunteeringEnquiry
    {
        public List<string> InterestList { get; set; }
        public List<string> PreferredLocationList { get; set; }
        public string NumberOfHours { get; set; }
        public List<string> NotAvailableList { get; set; }
        public string AdditionalInfo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public StockportGovUK.NetStandard.Models.Addresses.Address CustomersAddress { get; set; }
    }
}