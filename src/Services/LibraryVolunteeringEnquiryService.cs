using System;
using System.Threading.Tasks;
using library_volunteering_enquiry_service.Config;
using library_volunteering_enquiry_service.Extensions;
using library_volunteering_enquiry_service.Models;
using Microsoft.Extensions.Options;
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
        private readonly VerintConfiguration _verintConfiguration;

        public LibraryVolunteeringEnquiryService(
            IVerintServiceGateway verintServiceGateway,
            IMailingServiceGateway mailingServiceGateway,
            IOptions<VerintConfiguration> verintConfiguration)
        {
            _verintServiceGateway = verintServiceGateway;
            _mailingServiceGateway = mailingServiceGateway;
            _verintConfiguration = verintConfiguration.Value;
        }

        public async Task<string> CreateCase(LibraryVolunteeringEnquiry enquiry)
        {
            var response = await _verintServiceGateway.CreateCase(enquiry.MapToCase(
                _verintConfiguration.EventCode,
                _verintConfiguration.Classification));

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