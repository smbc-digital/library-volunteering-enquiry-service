using System.Collections.Generic;
using System.Threading.Tasks;
using library_volunteering_enquiry_service.Controllers;
using library_volunteering_enquiry_service.Models;
using library_volunteering_enquiry_service.Services;
using Microsoft.Extensions.Logging;
using Moq;
using StockportGovUK.NetStandard.Models.Addresses;
using Xunit;

namespace library_volunteering_enquiry_service_tests.Controllers
{
    public class HomeControllerTest
    {
        private readonly HomeController _homeController;
        private readonly Mock<ILibraryVolunteeringEnquiryService> _mockLibraryVolunteeringEnquiryService = new Mock<ILibraryVolunteeringEnquiryService>();

        public HomeControllerTest()
        {
            _homeController = new HomeController(Mock.Of<ILogger<HomeController>>(), _mockLibraryVolunteeringEnquiryService.Object);
        }

        [Fact]
        public async Task Post_ShouldCallCreateCase()
        {
            _mockLibraryVolunteeringEnquiryService
                .Setup(_ => _.CreateCase(It.IsAny<LibraryVolunteeringEnquiry>()))
                .ReturnsAsync("test");

            var result = await _homeController.Post(null);

            _mockLibraryVolunteeringEnquiryService
                .Verify(_ => _.CreateCase(null), Times.Once);
        }

        [Fact]
        public async Task Post_ReturnOkActionResult()
        {
            _mockLibraryVolunteeringEnquiryService
                .Setup(_ => _.CreateCase(It.IsAny<LibraryVolunteeringEnquiry>()))
                .ReturnsAsync("test");

            var result = await _homeController.Post(null);

            Assert.Equal("OkObjectResult", result.GetType().Name);
        }
    }
}
