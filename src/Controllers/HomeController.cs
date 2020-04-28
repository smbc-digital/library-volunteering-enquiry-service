using library_volunteering_enquiry_service.Models;
using library_volunteering_enquiry_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportGovUK.AspNetCore.Attributes.TokenAuthentication;
using System;
using System.Text.Json;
using System.Threading.Tasks;

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

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LibraryVolunteeringEnquiry libraryVolunteeringEnquiry)
        {
            _logger.LogDebug(JsonSerializer.Serialize(libraryVolunteeringEnquiry));

            try
            {
                var result = await _libraryVolunteeringEnquiryService.CreateCase(libraryVolunteeringEnquiry);
                _logger.LogWarning($"Case result: { result }");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Case an exception has occurred while calling CreateCase, ex: {ex}");
                return StatusCode(500, ex);
            }
        }
    }
}