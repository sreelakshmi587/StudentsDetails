using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentsDetails.CrossCuttingConcerns.Constants;
using StudentsDetails.Model;
using StudentsDetails.Services.StudentsDetails;

namespace StudentsDetails.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private IStudentDetailsUsingEfService StudentDetailsUsingEfService { get; }
        public LoginController(IConfiguration config
            , IStudentDetailsUsingEfService studentDetailsUsingEfService)
        {
            _config = config;
            StudentDetailsUsingEfService = studentDetailsUsingEfService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(UserLogin userLogin)
        {
            var user = StudentDetailsUsingEfService.Authenticate(userLogin);
            if (user != null)
            {
                var token = StudentDetailsUsingEfService.Generate(user);
                return Ok(token);
            }
            return NotFound(SwaggerConstants.UserNotFound);
        }
    }
}
