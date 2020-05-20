using System.Threading.Tasks;
using library_volunteering_enquiry_service.Models;


namespace library_volunteering_enquiry_service.Services
{
    public interface ILibraryVolunteeringEnquiryService
    {
        Task<string> CreateCase(LibraryVolunteeringEnquiry libraryVolunteeringEnquiry);
    }
}
