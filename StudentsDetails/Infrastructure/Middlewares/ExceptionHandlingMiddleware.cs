using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StudentsDetails.Infrastructure.Exceptions;
using StudentsDetails.Infrastructure.ViewModels;
using System;
using System.Net;
using System.Threading.Tasks;

namespace StudentsDetails.Infrastructure.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);

                await HandleException(context, ex);
            }
        }

        public static Task HandleException(HttpContext context, Exception ex)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;
            switch (ex)
            {
                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    break;

                case BadRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case UnprocessableEntityObjectException:
                    statusCode = (int)HttpStatusCode.UnprocessableEntity;
                    break;

                case ConflictObjectException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    break;

            }
            var error = new ErrorResponse()
            {
                Status = statusCode,
                Messsage = ex.Message
            }.ToString();

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(error);
        }


    }



    public static class ExceptionHandlerExtension
    {
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
