using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentsDetails.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace StudentsDetails.Services.StudentsDetails
{
    public class StudentDetailsService : IStudentDetailsService
    {
        private readonly IConfiguration _config;
        public string cnn = " ";
        public StudentDetailsService(IConfiguration config)
        {
            _config = config;
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build();
            cnn = builder.GetSection("ConnectionStrings:StudentsConnectionString").Value;
        }
        public ActionResult<List<StudentDetails>> GetAllStudents()
        {
            List<StudentDetails> students = new();
            try
            {
                using (SqlConnection connection = new(cnn))
                {
                    SqlCommand command = new("spGetAllStudentDetails", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
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

        public ActionResult<StudentDetails> GetStudentById(int id)
        {
            using (SqlConnection connection = new(cnn))
            {
                SqlCommand command = new("spGetStudentDetailsById", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    StudentDetails student = new()
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
