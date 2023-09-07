using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StudentsDetails.Infrastructure.ViewModels;
using StudentsDetails.Model;
using StudentsDetails.Persistence.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
            var existingStudent = Context.StudentDetails.Where(s => s.AdmissionNo == studentDetails.AdmissionNo).FirstOrDefault();
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

        public StudentDetails UpdateStudentDetails(StudentDetails details)
        {
            var student = Context.StudentDetails.FirstOrDefault(s => s.Id == details.Id);
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

        //For Login Controller

        public UserModel RegisterUser(UserModelResponse user)
        {
            var salt = GenerateSalt();
            var hashedPassword = HashPassword(user.Password, salt);

            var registeredUser = new UserModel()
            {
                UserName = user.UserName,
                Password = hashedPassword,
                Salt = salt,
                Email = user.Email,
                Roles  = user.Roles
            };

            var existingUser = Context.UserModels.FirstOrDefault(u => u.Email == registeredUser.Email && u.Roles == registeredUser.Roles);

            if (existingUser == null)
            {
                Context.UserModels.Add(registeredUser);
                Context.SaveChanges();
            }
            else
            {
                return null;
            }

            return registeredUser;
        }
        

        private static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16]; 
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPassword(string password, string salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 10000))
            {
                byte[] hashBytes = pbkdf2.GetBytes(256 / 8); 
                return Convert.ToBase64String(hashBytes);
            }
        }

        private static bool VerifyPassword(string password, string passwordHash, string salt)
        {
            var hashedPassword = HashPassword(password, salt);
            return hashedPassword == passwordHash;
        }

        public string Generate(UserModel model)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

           var roleClaims = model.Roles.Split(',').Select(role => new Claim(ClaimTypes.Role, role));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, model.UserName),
                new Claim(ClaimTypes.Email, model.Email),
            };
            claims.AddRange(roleClaims);
            

            var token = new JwtSecurityToken(_config["Jwt:Issuer"]
                , _config["Jwt:Audience"]
                , claims.ToArray()
                , expires: DateTime.Now.AddMinutes(15)
                , signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);    

        }

        public UserModel Authenticate(UserModel login)
        {
            var user = Context.UserModels.FirstOrDefault(u => u.UserName.ToLower() == login.UserName.ToLower());

            if (user != null && VerifyPassword(login.Password, user.Password, user.Salt))
            {
                return user;
            }

            return null;
        }
    }
}
