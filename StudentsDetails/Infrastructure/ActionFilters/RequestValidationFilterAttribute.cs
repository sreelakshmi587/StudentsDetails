using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudentsDetails.CrossCuttingConcerns.Constants;
using StudentsDetails.Model;
using StudentsDetails.Services.StudentsDetails;
using System;
using System.Linq;

namespace StudentsDetails.Infrastructure.ActionFilters
{
    public class RequestValidationFilterAttribute : Attribute, IActionFilter
    {
        private readonly IStudentDetailsUsingEfService _studentDetailsUsingEfService;
        public RequestValidationFilterAttribute(IStudentDetailsUsingEfService studentDetailsUsingEfService)
        {
            _studentDetailsUsingEfService = studentDetailsUsingEfService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
                return;
            }

            StudentDetails studentModel = (StudentDetails)context.ActionArguments.Values.FirstOrDefault();

            if (studentModel == null)
            {
                context.Result = new BadRequestObjectResult(SwaggerConstants.InvalidStudentData);
                return;
            }

            var existingStudent = _studentDetailsUsingEfService.GetStudentDetailsById(studentModel.Id);

            if (existingStudent != null)
            {
                context.Result = new ConflictObjectResult(SwaggerConstants.StudentAlreadyExists);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }


    }
}
