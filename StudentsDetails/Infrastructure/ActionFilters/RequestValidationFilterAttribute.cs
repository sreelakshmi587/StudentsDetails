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

            var httpMethod = context.HttpContext.Request.Method;
            var isHttpPostOrPut = httpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase) ||
                                  httpMethod.Equals("PUT", StringComparison.OrdinalIgnoreCase);

            if (!isHttpPostOrPut)
            {
                return;
            }

            StudentDetails studentModel = (StudentDetails)context.ActionArguments.Values.FirstOrDefault();

            if (studentModel == null)
            {
                context.Result = new BadRequestObjectResult(SwaggerConstants.InvalidStudentData);
                return;
            }


            var existingStudent = _studentDetailsUsingEfService.GetStudentDetailsById(studentModel.Id);
            if (string.Equals(httpMethod, "PUT", StringComparison.OrdinalIgnoreCase) && existingStudent == null)
            {
                context.Result = new NotFoundObjectResult(SwaggerConstants.InvalidId);
                return;
            }

            if (existingStudent != null)
            {
                if (string.Equals(httpMethod, "POST", StringComparison.OrdinalIgnoreCase) &&
                    existingStudent.AdmissionNo == studentModel.AdmissionNo)
                {
                    context.Result = new ConflictObjectResult(SwaggerConstants.StudentAlreadyExists);
                }

            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }


    }
}
