using System;
using System.Threading.Tasks;
using library_volunteering_enquiry_service.Extensions;
using library_volunteering_enquiry_service.Models;
using Newtonsoft.Json;
using StockportGovUK.NetStandard.Gateways.MailingServiceGateway;
using StockportGovUK.NetStandard.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.Enums;
using StockportGovUK.NetStandard.Models.Mail;

namespace library_volunteering_enquiry_service.Services
{
    public class LibraryVolunteeringEnquiryService : ILibraryVolunteeringEnquiryService
    {
        private readonly IVerintServiceGateway _verintServiceGateway;
        private readonly IMailingServiceGateway _mailingServiceGateway;

        public LibraryVolunteeringEnquiryService(IVerintServiceGateway verintServiceGateway, IMailingServiceGateway mailingServiceGateway)
        {
            _verintServiceGateway = verintServiceGateway;
            _mailingServiceGateway = mailingServiceGateway;
        }
        public async Task<string> CreateCase(LibraryVolunteeringEnquiry enquiry)
        {
            var response = await _verintServiceGateway.CreateCase(enquiry.MapToCase());

            if (!response.IsSuccessStatusCode)
                throw new Exception("LibraryVolunteeringEnquiryService.CreateCase: VerintServiceGateway status code indicated the case was not created.");

            if(!string.IsNullOrEmpty(enquiry.Email))
                _ = _mailingServiceGateway.Send(new Mail
                {
                    Template = EMailTemplate.LibraryVolunteeringEnquiry,
                    Payload = JsonConvert.SerializeObject(new
                    {
                        enquiry.FirstName,
                        Reference = response.ResponseContent,
                        RecipientAddress = enquiry.Email,
                        Subject = "Library Volunteering Enquiry"
                    })
                });

            return response.ResponseContent;
        }
    }
}