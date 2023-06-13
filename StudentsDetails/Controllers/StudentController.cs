using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentsDetails.CrossCuttingConcerns.Constants;
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
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _config;
        private IMapper Mapper { get; }
        private IStudentDetailsService StudentDetailsService { get; }
        private IStudentDetailsUsingEfService StudentDetailsUsingEfService { get; }
        private readonly StudentsDbContext Context;

        public StudentController(IConfiguration config
            , IMapper mapper
            , IStudentDetailsService studentDetailsService
            , IStudentDetailsUsingEfService studentDetailsUsingEfService
            , StudentsDbContext context)
        {
            _config = config;
            Mapper = mapper;
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
        [SwaggerOperation(SwaggerConstants.ReturnsStudentDetails)]
        [SwaggerResponse(StatusCodes.Status200OK, SwaggerConstants.StudentDetailsReturned)]
        [SwaggerResponse(StatusCodes.Status404NotFound, SwaggerConstants.StudentDetailsNotFound)]
        public ActionResult<List<StudentDetailsResponse>> GetAllStudentsDetails()
        {
            var studentList = StudentDetailsUsingEfService.GetAllStudentsDetails();
            return Mapper.Map<List<StudentDetailsResponse>>(studentList);
        }

        [HttpGet("get-student-data-by-id/{id}")]
        [SwaggerOperation(SwaggerConstants.ReturnsStudentDetailsById)]
        [SwaggerResponse(StatusCodes.Status200OK, SwaggerConstants.StudentDetailsByIdReturned)]
        [SwaggerResponse(StatusCodes.Status404NotFound, SwaggerConstants.StudentDetailsByIdNotFound)]
        public ActionResult<StudentDetailsResponse> GetStudentDetailsById(int id)
        {
            var student = StudentDetailsUsingEfService.GetStudentDetailsById(id);

            if (student != null)
            {
                return Mapper.Map<StudentDetailsResponse>(student);
            }

            return NotFound(SwaggerConstants.StudentDetailsByIdNotFound);
        }

        [HttpPost("add-student-details")]
        [SwaggerOperation(SwaggerConstants.AddsStudentDetails)]
        [SwaggerResponse(StatusCodes.Status200OK, SwaggerConstants.StudentDetailsAdded)]
        [SwaggerResponse(StatusCodes.Status409Conflict, SwaggerConstants.StudentDetailsByIdNotFound)]
        public ActionResult<StudentDetailsResponse> AddStudentDetails(StudentDetails studentDetails)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var newStudent = StudentDetailsUsingEfService.AddStudentDetail(studentDetails);

            if (newStudent != null)
            {
                return Ok(Mapper.Map<StudentDetailsResponse>(newStudent));
            }

            return Conflict(SwaggerConstants.StudentAlreadyExists);
        }

        [HttpPut("update-student-details/{id}")]
        [SwaggerOperation(SwaggerConstants.UpdateStudentDetails)]
        [SwaggerResponse(StatusCodes.Status200OK, SwaggerConstants.StudentDetailsUpdated)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, SwaggerConstants.BadRequestMessage)]
        public ActionResult<StudentDetailsResponse> UpdateStudentDetails(int id, StudentDetails studentDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = StudentDetailsUsingEfService.UpdateStudentDetails(id, studentDetails);

            if (student != null)
            {
                return Ok(Mapper.Map<StudentDetailsResponse>(student));
            }

            return BadRequest(SwaggerConstants.BadRequestMessage);
        }

        [HttpDelete("delete-student-details/{id}")]
        [SwaggerOperation(SwaggerConstants.DeletesStudentDetails)]
        [SwaggerResponse(StatusCodes.Status204NoContent, SwaggerConstants.StudentDetailsDeleted)]
        [SwaggerResponse(StatusCodes.Status404NotFound, SwaggerConstants.StudentDetailsByIdNotFound)]
        public ActionResult<StudentDetailsResponse> DeleteStudentDetails(int id)
        {
            var student = StudentDetailsUsingEfService.GetStudentDetailsById(id);
            if (student != null)
            {
                StudentDetailsUsingEfService.DeleteStudent(id);

                return NoContent();
            }
            return NotFound(SwaggerConstants.StudentDetailsByIdNotFound);

        }
    }
}
