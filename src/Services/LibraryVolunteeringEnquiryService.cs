using System;
using System.Text;
using System.Threading.Tasks;
using library_volunteering_enquiry_service.Models;
using StockportGovUK.NetStandard.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.Verint;

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
            var crmCase = CreateCrmObject(libraryVolunteeringEnquiry);

            try
            {
                var response = await _VerintServiceGateway.CreateCase(crmCase);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Status code not successful");
                }

                return response.ResponseContent;
            }
            catch (Exception ex)
            {
                throw new Exception($"CRMService CreateLibraryVolunteeringEnquiry an exception has occured while creating the case in verint service", ex);
            }
        }

        private Case CreateCrmObject(LibraryVolunteeringEnquiry libraryVolunteeringEnquiry)
        {
            var crmCase = new Case
            {
                EventCode = 4000031,
                EventTitle = "Volunteering",
                Description = GenerateDescription(libraryVolunteeringEnquiry),
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
            return crmCase;
        }

        private string GenerateDescription(LibraryVolunteeringEnquiry libraryVolunteeringEnquiry)
        {
            StringBuilder description = new StringBuilder();

            if (libraryVolunteeringEnquiry.InterestList.Count > 0)
            {
                description.Append("Selected Interests: ");
                foreach (var interest in libraryVolunteeringEnquiry.InterestList)
                {
                    description.Append($"{interest}, ");
                }
                description.Append("\n");
            }

            if (libraryVolunteeringEnquiry.PreferredLocationList.Count > 0)
            {
                description.Append("Selected Locations: ");
                foreach (var location in libraryVolunteeringEnquiry.PreferredLocationList)
                {
                    description.Append($"{location}, ");
                }
                description.Append("\n");
            }

            if (!string.IsNullOrEmpty(libraryVolunteeringEnquiry.NumberOfHours))
            {
                description.Append($"Hours: {libraryVolunteeringEnquiry.NumberOfHours}\n");
            }

            if (libraryVolunteeringEnquiry.NotAvailableList.Count > 0)
            {
                description.Append("Days can't work: ");
                foreach (var notAvailable in libraryVolunteeringEnquiry.NotAvailableList)
                {
                    description.Append($"{notAvailable}, ");
                }
                description.Append("\n");
            }

            if (!string.IsNullOrEmpty(libraryVolunteeringEnquiry.AdditionalInfo))
            {
                description.Append($"Extra Information: {libraryVolunteeringEnquiry.AdditionalInfo}");
            }

            return description.ToString();
        }
    }
}