using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentsDetails.CrossCuttingConcerns.Constants;
using StudentsDetails.Model;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

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
        public ActionResult<List<StudentDetails>> GetStudentNames()
        {
            StudentDetails studentDetails = new();
           _config.GetSection("Students").Bind(studentDetails);

           return Ok(studentDetails.Name);

        }

    }



}
