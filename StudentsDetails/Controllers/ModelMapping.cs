using AutoMapper;
using StudentsDetails.Infrastructure.ViewModels;
using StudentsDetails.Model;

namespace StudentsDetails.Controllers
{
    public class ModelMapping : Profile
    {
        public ModelMapping()
        {
            CreateMap<StudentDetails, StudentDetailsResponse>().ReverseMap();
            CreateMap<UserModel, UserViewModel>().ReverseMap();
        }
    }
}
