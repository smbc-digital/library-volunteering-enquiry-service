﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using library_volunteering_enquiry_service.Models;
using library_volunteering_enquiry_service.Services;
using StockportGovUK.NetStandard.Gateways.Response;
using StockportGovUK.NetStandard.Gateways.MailingServiceGateway;
using StockportGovUK.NetStandard.Gateways.VerintServiceGateway;
using StockportGovUK.NetStandard.Models.Verint;
using Xunit;
using StockportGovUK.NetStandard.Models.Mail;
using library_volunteering_enquiry_service.Config;
using Microsoft.Extensions.Options;

namespace library_volunteering_enquiry_service_tests.Services
{
    public class LibraryVolunteeringEnquiryServiceTests
    {
        private Mock<IVerintServiceGateway> _mockVerintServiceGateway = new Mock<IVerintServiceGateway>();
        private Mock<IMailingServiceGateway> _mockMailingServiceGateway = new Mock<IMailingServiceGateway>();
        private Mock<IOptions<VerintConfiguration>> _mockIOptionsConfig = new Mock<IOptions<VerintConfiguration>>();
        private LibraryVolunteeringEnquiryService _service;

        LibraryVolunteeringEnquiry _libraryVolunteeringEnquiryData = new LibraryVolunteeringEnquiry
        {
            InterestList = new List<string> { "Code Clubs", "Duke of Edinburgh" },
            PreferredLocationList = new List<string> { "No preference" },
            NumberOfHours = 36,
            NotAvailableList = new List<string> { "Monday" },
            AdditionalInfo = "",
            FirstName = "Joe",
            LastName = "Bloggs",
            Email = "joe@test.com",
            Phone = "0161 123 1234",
            CustomersAddress = new StockportGovUK.NetStandard.Models.Addresses.Address
            {
                AddressLine1 = "100 Green road",
                AddressLine2 = "",
                Postcode = "SK2 9FT",
            }
        };

        public LibraryVolunteeringEnquiryServiceTests()
        {
            _mockIOptionsConfig
                .SetupGet(_ => _.Value)
                .Returns(new VerintConfiguration
                {
                    EventCode = 0,
                    Classification = "test classification"
                });

            _service = new LibraryVolunteeringEnquiryService(
                _mockVerintServiceGateway.Object,
                _mockMailingServiceGateway.Object,
                _mockIOptionsConfig.Object);
        }

        [Fact]
        public async Task CreateCase_ShouldThrowException_WhenIsNotSuccessStatusCode()
        {
            _mockVerintServiceGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .ReturnsAsync(new HttpResponse<string>
                {
                    IsSuccessStatusCode = false
                });


            _ = await Assert.ThrowsAsync<Exception>(() => _service.CreateCase(_libraryVolunteeringEnquiryData));
        }

        [Fact]
        public async Task CreateCase_ShouldReturnResponseContent()
        {
            _mockVerintServiceGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .ReturnsAsync(new HttpResponse<string>
                {
                    IsSuccessStatusCode = true,
                    ResponseContent = "test"
                });

            var result = await _service.CreateCase(_libraryVolunteeringEnquiryData);

            Assert.Contains("test", result);
        }

        [Fact]
        public async Task CreateCase_ShouldCallVerintGatewayWithCRMCase()
        {
            Case crmCaseParameter = null;

            _mockVerintServiceGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .Callback<Case>(_ => crmCaseParameter = _)
                .ReturnsAsync(new HttpResponse<string>
                {
                    IsSuccessStatusCode = true,
                    ResponseContent = "test"
                });

            _ = await _service.CreateCase(_libraryVolunteeringEnquiryData);
            
            _mockVerintServiceGateway.Verify(_ => _.CreateCase(It.IsAny<Case>()), Times.Once);

            Assert.NotNull(crmCaseParameter);
            Assert.Equal(_libraryVolunteeringEnquiryData.CustomersAddress.AddressLine1, crmCaseParameter.Customer.Address.AddressLine1);
            Assert.Equal(_libraryVolunteeringEnquiryData.CustomersAddress.AddressLine2, crmCaseParameter.Customer.Address.AddressLine2);
            Assert.Equal(_libraryVolunteeringEnquiryData.CustomersAddress.Town, crmCaseParameter.Customer.Address.AddressLine3);
            Assert.Equal(_libraryVolunteeringEnquiryData.CustomersAddress.Postcode, crmCaseParameter.Customer.Address.Postcode);
            Assert.Null(crmCaseParameter.Customer.Address.UPRN);
            Assert.Null(crmCaseParameter.Customer.Address.Reference);
        }

        [Fact]
        public async Task GenerateDescription_ShouldCallVerintGatewayAndGenerateDescription()
        {
            var interestList = _libraryVolunteeringEnquiryData.InterestList;
            var preferredLocationList = _libraryVolunteeringEnquiryData.PreferredLocationList;
            var numberOfHours = _libraryVolunteeringEnquiryData.NumberOfHours;
            var notAvailableList = _libraryVolunteeringEnquiryData.NotAvailableList;
            var additionalInfo = _libraryVolunteeringEnquiryData.AdditionalInfo;

            Case crmCaseParameter = null;

            _mockVerintServiceGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .Callback<Case>(_ => crmCaseParameter = _)
                .ReturnsAsync(new HttpResponse<string>
                {
                    IsSuccessStatusCode = true,
                    ResponseContent = "test"
                });

            _ = await _service.CreateCase(_libraryVolunteeringEnquiryData);

            _mockVerintServiceGateway.Verify(_ => _.CreateCase(It.IsAny<Case>()), Times.Once);

            Assert.NotNull(crmCaseParameter);

            Assert.Contains(interestList[0], crmCaseParameter.Description);
            Assert.Contains(interestList[1], crmCaseParameter.Description);
            Assert.Contains(preferredLocationList[0], crmCaseParameter.Description);
            Assert.Contains(numberOfHours.ToString(), crmCaseParameter.Description);
            Assert.Contains(notAvailableList[0], crmCaseParameter.Description);
            Assert.Contains(additionalInfo, crmCaseParameter.Description);            
        }

        [Fact]
        public async Task CreateCase_ShouldCallMailingService()
        {
            _mockVerintServiceGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .ReturnsAsync(new HttpResponse<string>
                {
                    IsSuccessStatusCode = true,
                    ResponseContent = "test"
                });

            await _service.CreateCase(_libraryVolunteeringEnquiryData);

            _mockMailingServiceGateway
                .Verify(_ => _.Send(It.IsAny<Mail>()), Times.Once);
        }

        [Fact]
        public async Task CreateCase_ShouldNotCallMailingService()
        {
            var enquiryData = _libraryVolunteeringEnquiryData;

            enquiryData.Email = string.Empty;

            _mockVerintServiceGateway
                .Setup(_ => _.CreateCase(It.IsAny<Case>()))
                .ReturnsAsync(new HttpResponse<string>
                {
                    IsSuccessStatusCode = true,
                    ResponseContent = "test"
                });

            await _service.CreateCase(_libraryVolunteeringEnquiryData);

            _mockMailingServiceGateway
                .VerifyNoOtherCalls();
        }
    }
}

