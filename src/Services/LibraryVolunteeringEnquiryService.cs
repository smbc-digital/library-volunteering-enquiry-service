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
            
            var description = $@"FirstName: {libraryVolunteeringEnquiry.FirstName}
                                LastName: { libraryVolunteeringEnquiry.LastName}
                                Email: {libraryVolunteeringEnquiry.Email}
                                Phone: {libraryVolunteeringEnquiry.Phone}
                                HowManyHours: {libraryVolunteeringEnquiry.NumberOfHours}
                                AdditionalInformation: {libraryVolunteeringEnquiry.AdditionalInfo}";

            if (libraryVolunteeringEnquiry.InterestList.Count > 0)
            {
                description += $@"CheckBoxSelectInterests: {libraryVolunteeringEnquiry.InterestList[0]}
                                CheckBoxPreferredLocation: {libraryVolunteeringEnquiry.PreferredLocationList[0]}
                                CheckBoxDaysNotAvailable: {libraryVolunteeringEnquiry.NotAvailableList[0]}
                                ";
            }

            if (libraryVolunteeringEnquiry.CustomersAddress != null)
            {
                description += $@"AddressLine1: {libraryVolunteeringEnquiry.CustomersAddress.AddressLine1}
                                AddressLine2: {libraryVolunteeringEnquiry.CustomersAddress.AddressLine2}
                                Town: {libraryVolunteeringEnquiry.CustomersAddress.Town}
                                Postcode: {libraryVolunteeringEnquiry.CustomersAddress.Postcode}
                                SelectedAddress: {libraryVolunteeringEnquiry.CustomersAddress.SelectedAddress}";
            }

            var crmCase = new Case
            {
                EventCode = 4000031,
                EventTitle = "Basic Verint Case",
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