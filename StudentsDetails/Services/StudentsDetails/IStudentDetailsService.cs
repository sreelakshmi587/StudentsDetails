using Microsoft.AspNetCore.Mvc;
using StudentsDetails.Model;
using System.Collections.Generic;

namespace StudentsDetails.Services.StudentsDetails
{
    public interface IStudentDetailsService
    {
        ActionResult<List<StudentDetails>> GetAllStudents();
        ActionResult<StudentDetails> GetStudentById(int id);
    }
}
