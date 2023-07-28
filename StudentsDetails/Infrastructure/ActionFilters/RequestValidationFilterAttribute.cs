using Microsoft.AspNetCore.Mvc.Filters;
using StudentsDetails.CrossCuttingConcerns.Constants;
using StudentsDetails.Infrastructure.Exceptions;
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
                throw new UnprocessableEntityObjectException(SwaggerConstants.InvalidModel, context.ModelState);
                //context.Result = new UnprocessableEntityObjectResult(context.ModelState);
                //return;
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
                throw new BadRequestException(SwaggerConstants.InvalidStudentData);

                //context.Result = new BadRequestObjectResult(SwaggerConstants.InvalidStudentData);
                //return;
            }

            var existingStudent = _studentDetailsUsingEfService.GetStudentDetailsById(studentModel.Id);

            if (string.Equals(httpMethod, "PUT", StringComparison.OrdinalIgnoreCase) && existingStudent == null)
            {
                //context.Result = new NotFoundObjectResult(SwaggerConstants.InvalidId);
                //return;
                throw new NotFoundException(SwaggerConstants.InvalidId);
            }

            if (existingStudent != null)
            {
                if (string.Equals(httpMethod, "POST", StringComparison.OrdinalIgnoreCase) &&
                    existingStudent.AdmissionNo == studentModel.AdmissionNo)
                {
                    throw new ConflictObjectException(SwaggerConstants.StudentAlreadyExists);
                    // context.Result = new ConflictObjectResult(SwaggerConstants.StudentAlreadyExists);
                }

            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }


    }
}
