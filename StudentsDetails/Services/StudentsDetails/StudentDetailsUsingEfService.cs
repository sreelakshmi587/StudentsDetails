using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentsDetails.Model;
using StudentsDetails.Persistence.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace StudentsDetails.Services.StudentsDetails
{
    public class StudentDetailsUsingEfService : IStudentDetailsUsingEfService
    {
        private readonly IConfiguration _config;
        private readonly StudentsDbContext Context;
        public StudentDetailsUsingEfService(IConfiguration config
            , StudentsDbContext context)
        {
            _config = config;
            Context = context;
        }
        public List<StudentDetails> GetAllStudentsDetails()
        {
            var studentList = Context.StudentDetails.AsNoTracking().ToList();

            return studentList;
        }

        public StudentDetails GetStudentDetailsById(int id)
        {
            var student = Context.StudentDetails.FirstOrDefault(s => s.Id == id);

            return student;

        }

        public StudentDetails AddStudentDetail(StudentDetails studentDetails)
        {
            var existingStudent = Context.StudentDetails.Where(s => s.Id == studentDetails.Id && s.AdmissionNo == studentDetails.AdmissionNo).FirstOrDefault();
            if (existingStudent == null)
            {
                Context.StudentDetails.Add(studentDetails);
                Context.SaveChanges();
            }
            else
            {
                return null;
            }

            return studentDetails;
        }

        public StudentDetails UpdateStudentDetails(int id, StudentDetails details)
        {
            var student = Context.StudentDetails.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                student.AdmissionNo = details.AdmissionNo;
                student.Name = details.Name;
                student.Class = details.Class;
                student.Address = details.Address;

                Context.StudentDetails.Update(student);
                Context.SaveChanges();
            }

            return student;
        }

        public StudentDetails DeleteStudent(int id)
        {
            var student = Context.StudentDetails.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                Context.StudentDetails.Remove(student);
                Context.SaveChanges();
            }
            return null;
        }

        public string Generate(UserModel model)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, model.UserName),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role, model.Role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"]
                , _config["Jwt:Audience"]
                , claims
                , expires: DateTime.Now.AddMinutes(15)
                , signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public UserModel Authenticate(UserLogin login)
        {
            var currentUser = Context.UserModels.FirstOrDefault(o => o.UserName.ToLower() == login.UserName.ToLower()
            && o.Password == login.Password);
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }
    }
}
