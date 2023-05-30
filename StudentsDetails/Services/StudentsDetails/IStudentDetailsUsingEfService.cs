using StudentsDetails.Model;
using System.Collections.Generic;

namespace StudentsDetails.Services.StudentsDetails
{
    public interface IStudentDetailsUsingEfService
    {
        List<StudentDetails> GetAllStudentsDetails();
        StudentDetails GetStudentDetailsById(int id);
        StudentDetails AddStudentDetail(StudentDetails studentDetails);
        StudentDetails UpdateStudentDetails(int id, StudentDetails details);
        StudentDetails DeleteStudent(int id);
    }
}
