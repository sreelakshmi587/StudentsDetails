using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentsDetails.CrossCuttingConcerns.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace StudentsDetails.Controllers
{
    [ApiController]
    [Route("student")]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _config;

        public StudentController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("get-all-students")]
        [SwaggerOperation(SwaggerConstants.ReturnsAllStudents)]
        [SwaggerResponse(StatusCodes.Status200OK, SwaggerConstants.StudentsListReturned)]
        [SwaggerResponse(StatusCodes.Status404NotFound, SwaggerConstants.StudentsListNotFound)]
        public ActionResult<string[]> GetStudentNames()
        {
            string[] studentNames = _config.GetSection("Students").Get<string[]>();

            return Ok(studentNames);
        }
    }



}
