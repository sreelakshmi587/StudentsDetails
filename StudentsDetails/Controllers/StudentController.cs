using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentsDetails.CrossCuttingConcerns.Constants;
using StudentsDetails.Model;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace StudentsDetails.Controllers
{
    [ApiController]
    [Route("student")]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _config;
        public string cnn = " ";

        public StudentController(IConfiguration config)
        {
            _config = config;
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build();
            cnn = builder.GetSection("ConnectionStrings:StudentsConnectionString").Value;
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
            List<StudentDetails> students = new();
            try
            {
                using (SqlConnection con = new SqlConnection(cnn))
                {
                    string query = "Select * from StudentDetails";
                    SqlCommand cm = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cm.ExecuteReader();
                    while (reader.Read())
                    {
                        students.Add(new StudentDetails()
                        {
                            Id = int.Parse(reader["Id"].ToString()),
                            AdmissionNo = reader["AdmissionNo"].ToString(),
                            Name = reader["Name"].ToString(),
                            Class = int.Parse(reader["Class"].ToString()),
                            Address = reader["Address"].ToString()
                        }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            };

            return students;
        }

        [HttpGet("get-student-details-by-id")]
        [SwaggerOperation(SwaggerConstants.ReturnsStudentDetailsById)]
        [SwaggerResponse(StatusCodes.Status200OK, SwaggerConstants.StudentDetailsByIdReturned)]
        [SwaggerResponse(StatusCodes.Status404NotFound, SwaggerConstants.StudentDetailsByIdNotFound)]
        public ActionResult<StudentDetails> GetStudentById(int id)
        {

            using (SqlConnection connection = new SqlConnection(cnn))
            {
                string query = "SELECT * FROM StudentDetails WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    StudentDetails student = new StudentDetails
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        AdmissionNo = reader["AdmissionNo"].ToString(),
                        Name = reader["Name"].ToString(),
                        Class = int.Parse(reader["Class"].ToString()),
                        Address = reader["Address"].ToString()
                    };

                    return student;
                }
                else
                {
                    return null;
                }
            }

        }

    }



}
