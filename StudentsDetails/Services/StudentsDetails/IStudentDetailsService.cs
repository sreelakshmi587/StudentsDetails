using Microsoft.AspNetCore.Mvc;
using StudentsDetails.Model;
using System.Collections.Generic;

namespace StudentsDetails.Services.StudentsDetails
{
    public interface IStudentDetailsService
    {
        List<StudentDetails> GetAllStudents();
        StudentDetails GetStudentById(int id);
    }
}
