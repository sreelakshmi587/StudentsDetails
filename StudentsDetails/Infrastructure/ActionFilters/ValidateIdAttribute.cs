﻿using Microsoft.AspNetCore.Mvc.Filters;
using StudentsDetails.CrossCuttingConcerns.Constants;
using StudentsDetails.Infrastructure.Exceptions;
using StudentsDetails.Services.StudentsDetails;
using System;

namespace StudentsDetails.Infrastructure.ActionFilters
{
    public class ValidateIdAttribute : Attribute, IActionFilter
    {
        private readonly IStudentDetailsUsingEfService _studentDetailsUsingEfService;
        public ValidateIdAttribute(IStudentDetailsUsingEfService studentDetailsUsingEfService)
        {
            _studentDetailsUsingEfService = studentDetailsUsingEfService;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!int.TryParse(context.RouteData.Values["id"]?.ToString(), out int id))
            {
                throw new BadRequestException(SwaggerConstants.InvalidId);
                //context.Result = new BadRequestObjectResult(SwaggerConstants.InvalidId);
            }
            var student = _studentDetailsUsingEfService.GetStudentDetailsById(id);
            if (student == null)
            {
                throw new NotFoundException(SwaggerConstants.StudentDetailsByIdNotFound);
                //context.Result = new NotFoundObjectResult(SwaggerConstants.StudentDetailsByIdNotFound);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }


    }
}
