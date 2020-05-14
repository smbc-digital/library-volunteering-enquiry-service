using library_volunteering_enquiry_service.Models;
using StockportGovUK.NetStandard.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.Verint;
using System;
using System.Threading.Tasks;

namespace library_volunteering_enquiry_service.Services
{
    public class LibraryVolunteeringEnquiryService : ILibraryVolunteeringEnquiryService
    {
        private readonly IVerintServiceGateway _VerintServiceGateway;

        public LibraryVolunteeringEnquiryService(IVerintServiceGateway verintServiceGateway)
        {
            _VerintServiceGateway = verintServiceGateway;
        }
        public async Task<string> CreateCase(LibraryVolunteeringEnquiry libraryVolunteeringEnquiry)
        {
            var description = string.Empty;

            if (libraryVolunteeringEnquiry.InterestList.Count > 0)
            {
                description += "Selected Interests: ";
                foreach (var interest in libraryVolunteeringEnquiry.InterestList)
                {
                    description += $"{interest}, ";
                }
            }

            if (libraryVolunteeringEnquiry.PreferredLocationList.Count > 0)
            {
                description += "Selected Locations: ";
                foreach (var location in libraryVolunteeringEnquiry.PreferredLocationList)
                {
                    description += $"{location}, ";
                }
            }

            description += $@"HowManyHours: {libraryVolunteeringEnquiry.NumberOfHours}
                            ";

            if (libraryVolunteeringEnquiry.NotAvailableList.Count > 0)
            {
                description += "Days can't work: ";
                foreach (var notAvailable in libraryVolunteeringEnquiry.NotAvailableList)
                {
                    description += $"{notAvailable}, ";
                }
            }

            description += $@"AdditionalInformation: {libraryVolunteeringEnquiry.AdditionalInfo}
                           ";

            var crmCase = new Case
            {
                EventCode = 4000031,
                EventTitle = "Volunteering",
                Description = description,
                AssociatedWithBehaviour = AssociatedWithBehaviourEnum.Individual
            };

            if (!string.IsNullOrEmpty(libraryVolunteeringEnquiry.FirstName) && !string.IsNullOrEmpty(libraryVolunteeringEnquiry.LastName))
            {
                crmCase.Customer = new Customer
                {
                    Forename = libraryVolunteeringEnquiry.FirstName,
                    Surname = libraryVolunteeringEnquiry.LastName
                };

                if (!string.IsNullOrEmpty(libraryVolunteeringEnquiry.Email))
                {
                    crmCase.Customer.Email = libraryVolunteeringEnquiry.Email;
                }

                if (!string.IsNullOrEmpty(libraryVolunteeringEnquiry.Phone))
                {
                    crmCase.Customer.Telephone = libraryVolunteeringEnquiry.Phone;
                }

                if (string.IsNullOrEmpty(libraryVolunteeringEnquiry.CustomersAddress.PlaceRef))
                {
                    crmCase.Customer.Address = new Address
                    {
                        AddressLine1 = libraryVolunteeringEnquiry.CustomersAddress.AddressLine1,
                        AddressLine2 = libraryVolunteeringEnquiry.CustomersAddress.AddressLine2,
                        AddressLine3 = libraryVolunteeringEnquiry.CustomersAddress.Town,
                        Postcode = libraryVolunteeringEnquiry.CustomersAddress.Postcode,
                    };
                }
                else
                {
                    crmCase.Customer.Address = new Address
                    {
                        Reference = libraryVolunteeringEnquiry.CustomersAddress.PlaceRef,
                        UPRN = libraryVolunteeringEnquiry.CustomersAddress.PlaceRef
                    };
                }
            }

            try
            {
                var response = await _VerintServiceGateway.CreateCase(crmCase);
                return response.ResponseContent;
            }
            catch (Exception ex)
            {
                throw new Exception($"CRMService CreateLibraryVolunteeringEnquiry an exception has occured while creating the case in verint service", ex);
            }
        }
    }
}