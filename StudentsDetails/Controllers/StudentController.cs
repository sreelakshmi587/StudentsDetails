using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StudentsDetails.CrossCuttingConcerns.Constants;
using StudentsDetails.Infrastructure.ActionFilters;
using StudentsDetails.Infrastructure.ViewModels;
using StudentsDetails.Model;
using StudentsDetails.Persistence.Context;
using StudentsDetails.Services.StudentsDetails;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace StudentsDetails.Controllers
{
    [ApiController]
    [Route("student")]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _config;
        private IMapper Mapper { get; }
        private readonly ILogger<StudentController> _logger;
        private IStudentDetailsService StudentDetailsService { get; }
        private IStudentDetailsUsingEfService StudentDetailsUsingEfService { get; }
        private readonly StudentsDbContext Context;

        public StudentController(IConfiguration config
            , IMapper mapper
            , ILogger<StudentController> logger
            , IStudentDetailsService studentDetailsService
            , IStudentDetailsUsingEfService studentDetailsUsingEfService
            , StudentsDbContext context)
        {
            _config = config;
            Mapper = mapper;
            _logger = logger;
            StudentDetailsService = studentDetailsService;
            StudentDetailsUsingEfService = studentDetailsUsingEfService;
            Context = context;
        }

        //Fetches data from appsettings class

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

        // Using ADO .Net

        [HttpGet("get-all-students")]
        [SwaggerOperation(SwaggerConstants.ReturnsStudentDetails)]
        [SwaggerResponse(StatusCodes.Status200OK, SwaggerConstants.StudentDetailsReturned)]
        [SwaggerResponse(StatusCodes.Status404NotFound, SwaggerConstants.StudentDetailsNotFound)]
        public ActionResult<List<StudentDetails>> GetAllStudents()
        {
            var studentList = StudentDetailsService.GetAllStudents();

            return studentList;
        }

        [HttpGet("get-student-details-by-id/{id}")]
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

        //Using EF Core

        [HttpGet("get-all-students-details")]
        [Authorize(Roles = "Admin, Student")]
        [SwaggerOperation(SwaggerConstants.ReturnsStudentDetails)]
        [SwaggerResponse(StatusCodes.Status200OK, SwaggerConstants.StudentDetailsReturned)]
        [SwaggerResponse(StatusCodes.Status404NotFound, SwaggerConstants.StudentDetailsNotFound)]
        public ActionResult<List<StudentDetailsResponse>> GetAllStudentsDetails()
        {
            _logger.LogInformation("Beginning to access all students details...");
            var studentList = StudentDetailsUsingEfService.GetAllStudentsDetails();
            _logger.LogInformation($"Accessed all details of {studentList.Count} students");

            return Mapper.Map<List<StudentDetailsResponse>>(studentList);
        }

        [HttpGet("get-student-data-by-id/{id}")]
        [Authorize(Roles ="Admin, Student")]
        [ServiceFilter(typeof(ValidateIdAttribute))]
        [SwaggerOperation(SwaggerConstants.ReturnsStudentDetailsById)]
        [SwaggerResponse(StatusCodes.Status200OK, SwaggerConstants.StudentDetailsByIdReturned)]
        [SwaggerResponse(StatusCodes.Status404NotFound, SwaggerConstants.StudentDetailsByIdNotFound)]
        public ActionResult<StudentDetailsResponse> GetStudentDetailsById(int id)
        {
            var student = StudentDetailsUsingEfService.GetStudentDetailsById(id);

            return Mapper.Map<StudentDetailsResponse>(student);
        }

        [HttpPost("add-student-details")]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(RequestValidationFilterAttribute))]
        [SwaggerOperation(SwaggerConstants.AddsStudentDetails)]
        [SwaggerResponse(StatusCodes.Status200OK, SwaggerConstants.StudentDetailsAdded)]
        [SwaggerResponse(StatusCodes.Status409Conflict, SwaggerConstants.StudentDetailsByIdNotFound)]
        public ActionResult<StudentDetailsResponse> AddStudentDetails(StudentDetails studentDetails)
        {
            var newStudent = StudentDetailsUsingEfService.AddStudentDetail(studentDetails);

            return Ok(Mapper.Map<StudentDetailsResponse>(newStudent));
        }

        [HttpPut("update-student-details/{id}")]
        [Authorize(Policy = "AdminOnly")]
        [ServiceFilter(typeof(RequestValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateIdAttribute))]
        [SwaggerOperation(SwaggerConstants.UpdateStudentDetails)]
        [SwaggerResponse(StatusCodes.Status200OK, SwaggerConstants.StudentDetailsUpdated)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, SwaggerConstants.BadRequestMessage)]
        public ActionResult<StudentDetailsResponse> UpdateStudentDetails([FromBody] StudentDetails studentDetails)
        {

            var student = StudentDetailsUsingEfService.UpdateStudentDetails(studentDetails);

            return Ok(Mapper.Map<StudentDetailsResponse>(student));
        }

        [HttpDelete("delete-student-details/{id}")]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidateIdAttribute))]
        [SwaggerOperation(SwaggerConstants.DeletesStudentDetails)]
        [SwaggerResponse(StatusCodes.Status204NoContent, SwaggerConstants.StudentDetailsDeleted)]
        [SwaggerResponse(StatusCodes.Status404NotFound, SwaggerConstants.StudentDetailsByIdNotFound)]
        public ActionResult<StudentDetailsResponse> DeleteStudentDetails(int id)
        {
            StudentDetailsUsingEfService.DeleteStudent(id);

            return NoContent();
        }
    }
}
