using library_volunteering_enquiry_service.Builders;
using library_volunteering_enquiry_service.Models;
using StockportGovUK.NetStandard.Models.Verint;
using System.Diagnostics.CodeAnalysis;

namespace library_volunteering_enquiry_service.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class LibraryVolunteeringEnquiryExtensions
    {
        public static Case MapToCase(this LibraryVolunteeringEnquiry model, int eventCode, string classification)
        {
            var description = new DescriptionBuilder()
                .Add("Selected Interests: ", model.InterestList, ", ")
                .Add("Selected Locations: ", model.PreferredLocationList, ", ")
                .Add("Hours: ", model.NumberOfHours.ToString())
                .Add("Days can't work: ", model.NotAvailableList, ", ")
                .Add("Extra Information: ", model.AdditionalInfo)
                .Build();

            return new Case
            {
                EventCode = eventCode,
                Classification = classification,
                EventTitle = "Volunteering",
                Description = description,
                AssociatedWithBehaviour = AssociatedWithBehaviourEnum.Individual,
                Customer = new Customer
                {
                    Forename = model.FirstName,
                    Surname = model.LastName,
                    Email = model.Email,
                    Telephone = model.Phone,
                    Address = new Address
                    {
                        AddressLine1 = model.CustomersAddress.AddressLine1,
                        AddressLine2 = model.CustomersAddress.AddressLine2,
                        AddressLine3 = model.CustomersAddress.Town,
                        Postcode = model.CustomersAddress.Postcode,
                        Reference = model.CustomersAddress.PlaceRef,
                        UPRN = model.CustomersAddress.PlaceRef
                    }
                }
            };
        }
    }
}
