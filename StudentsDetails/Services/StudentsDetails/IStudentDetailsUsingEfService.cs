using StudentsDetails.Infrastructure.ViewModels;
using StudentsDetails.Model;
using System.Collections.Generic;

namespace StudentsDetails.Services.StudentsDetails
{
    public interface IStudentDetailsUsingEfService
    {
        List<StudentDetails> GetAllStudentsDetails();
        StudentDetails GetStudentDetailsById(int id);
        StudentDetails AddStudentDetail(StudentDetails studentDetails);
        StudentDetails UpdateStudentDetails(StudentDetails details);
        StudentDetails DeleteStudent(int id);
        UserModel RegisterUser(UserViewModel user);
        string Generate(UserViewModel model);
        UserViewModel Authenticate(UserViewModel login);
    }
}
