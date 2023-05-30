using AutoMapper;
using StudentsDetails.Infrastructure.ViewModels;
using StudentsDetails.Model;

namespace StudentsDetails.Controllers
{
    public class StudentsModelMapping : Profile
    {
        public StudentsModelMapping()
        {
            CreateMap<StudentDetails, StudentDetailsResponse>().ReverseMap();
        }
    }
}
