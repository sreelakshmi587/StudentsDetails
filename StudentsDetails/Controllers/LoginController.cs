using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentsDetails.CrossCuttingConcerns.Constants;
using StudentsDetails.Infrastructure.ViewModels;
using StudentsDetails.Services.StudentsDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace StudentsDetails.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private IMapper Mapper { get; }
        private IStudentDetailsUsingEfService StudentDetailsUsingEfService { get; }
        public LoginController(IConfiguration config
            , IMapper mapper
            , IStudentDetailsUsingEfService studentDetailsUsingEfService)
        {
            _config = config;
            Mapper = mapper;
            StudentDetailsUsingEfService = studentDetailsUsingEfService;
        }

        [AllowAnonymous]
        [HttpPost("register-users")]
        [SwaggerOperation(Summary = "Register")]

        public ActionResult<UserViewModel> Register(UserViewModel user)
        {
            var registeredUser = StudentDetailsUsingEfService.RegisterUser(user);
            if (registeredUser == null)
            {
                return Conflict(SwaggerConstants.RegisteredUser);
            }

            return Ok(Mapper.Map<UserViewModel>(registeredUser));
        }

        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(Summary = "Login")]
        public IActionResult Login(UserViewModel userModel)
        {
            var user = new UserViewModel()
            {
                UserName = userModel.UserName,
                Password = userModel.Password
            };
            var loggedUser = StudentDetailsUsingEfService.Authenticate(user);
            if (loggedUser != null)
            {
                var token = StudentDetailsUsingEfService.Generate(loggedUser);
                return Ok(token);
            }
            return NotFound(SwaggerConstants.UserNotFound);
        }
    }
}
