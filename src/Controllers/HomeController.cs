using System.Threading.Tasks;
using library_volunteering_enquiry_service.Models;
using library_volunteering_enquiry_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportGovUK.AspNetCore.Attributes.TokenAuthentication;

namespace library_volunteering_enquiry_service.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[Controller]")]
    [ApiController]
    [TokenAuthentication]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILibraryVolunteeringEnquiryService _libraryVolunteeringEnquiryService;

        public HomeController(ILogger<HomeController> logger, ILibraryVolunteeringEnquiryService libraryVolunteeringEnquiryService)
        {
            _logger = logger;
            _libraryVolunteeringEnquiryService = libraryVolunteeringEnquiryService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LibraryVolunteeringEnquiry libraryVolunteeringEnquiry) =>
            Ok(await _libraryVolunteeringEnquiryService.CreateCase(libraryVolunteeringEnquiry));
    }
}