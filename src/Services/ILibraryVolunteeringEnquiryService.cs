using library_volunteering_enquiry_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library_volunteering_enquiry_service.Services
{
    public interface ILibraryVolunteeringEnquiryService
    {
        Task<string> CreateCase(LibraryVolunteeringEnquiry libraryVolunteeringEnquiry);
    }
}
