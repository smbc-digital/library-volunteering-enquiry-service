using Amazon.Runtime.Internal.Util;
using library_volunteering_enquiry_service.Controllers;
using library_volunteering_enquiry_service.Models;
using library_volunteering_enquiry_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StockportGovUK.NetStandard.Models.Addresses;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace library_volunteering_enquiry_service_tests.Controllers
{
    public class HomeControllerTest
    {
        private readonly HomeController _controller;
        private readonly Mock<ILogger<HomeController>> _mockLogger = new Mock<ILogger<HomeController>>();
        private readonly Mock<ILibraryVolunteeringEnquiryService> _mockCrmService = new Mock<ILibraryVolunteeringEnquiryService>();

        public HomeControllerTest()
        {
            _controller = new HomeController(_mockLogger.Object, _mockCrmService.Object);
        }

        [Fact]
        public async void PostCrmCase_ShouldCallLibraryVolunteeringEnquiryService()
        {
            //Arrange
                _mockCrmService
                    .Setup(_ => _.CreateCase(It.IsAny<LibraryVolunteeringEnquiry>()))
                    .ReturnsAsync(It.IsAny<string>());

            var Data = new LibraryVolunteeringEnquiry
            {
                InterestList = new List<string> { "Code Clubs", "Duke of Edinburgh" },
                PreferredLocationList = new List<string> { "No preference" },
                NumberOfHours = "36",
                NotAvailableList = new List<string> { "Monday" },
                AdditionalInfo = "",
                FirstName = "Joe",
                LastName = "Bloggs",
                Email = "joe@test.com",
                Phone = "0161 123 1234",
                CustomersAddress = new Address
                {
                    SelectedAddress = "Test",
                    AddressLine1 ="100 Green road",
                    AddressLine2 = "",
                    Town = "Stockport",
                    Postcode = "SK2 9FT",
                    PlaceRef = "",
                }
            };

            // Act
            await _controller.Post(Data);

            // Assert
            _mockCrmService.Verify(_ => _.CreateCase(It.IsAny<LibraryVolunteeringEnquiry>()), Times.Once);
        }       
    }
}
