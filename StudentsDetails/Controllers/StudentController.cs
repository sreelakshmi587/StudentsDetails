using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentsDetails.CrossCuttingConcerns.Constants;
using StudentsDetails.Model;
using StudentsDetails.Services.StudentsDetails;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace StudentsDetails.Controllers
{
    [ApiController]
    [Route("student")]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _config;
        private IStudentDetailsService StudentDetailsService { get; }

        public StudentController(IConfiguration config
            , IStudentDetailsService studentDetailsService)
        {
            _config = config;
            StudentDetailsService = studentDetailsService;
        }

        [HttpGet("get-student-names")]
        [SwaggerOperation(SwaggerConstants.ReturnsAllStudents)]
        [SwaggerResponse(StatusCodes.Status200OK, SwaggerConstants.StudentsListReturned)]
        [SwaggerResponse(StatusCodes.Status404NotFound, SwaggerConstants.StudentsListNotFound)]
        public ActionResult<List<StudentNames>> GetStudentNames()
        {
            StudentNames studentNames = new();
            _config.GetSection("Students").Bind(studentNames);

            return Ok(studentNames.Name);
        }

        [HttpGet("get-all-students")]
        [SwaggerOperation(SwaggerConstants.ReturnsStudentDetails)]
        [SwaggerResponse(StatusCodes.Status200OK, SwaggerConstants.StudentDetailsReturned)]
        [SwaggerResponse(StatusCodes.Status404NotFound, SwaggerConstants.StudentDetailsNotFound)]
        public ActionResult<List<StudentDetails>> GetAllStudents()
        {
            var studentList = StudentDetailsService.GetAllStudents();

            return studentList;
        }

        [HttpGet("get-student-details-by-id")]
        [SwaggerOperation(SwaggerConstants.ReturnsStudentDetailsById)]
        [SwaggerResponse(StatusCodes.Status200OK, SwaggerConstants.StudentDetailsByIdReturned)]
        [SwaggerResponse(StatusCodes.Status404NotFound, SwaggerConstants.StudentDetailsByIdNotFound)]
        public ActionResult<StudentDetails> GetStudentById(int id)
        {
            var student = StudentDetailsService.GetStudentById(id);

            if (student != null)
            {
                return student;
            }

            return NotFound(SwaggerConstants.StudentDetailsByIdNotFound);
        }
    }
}
