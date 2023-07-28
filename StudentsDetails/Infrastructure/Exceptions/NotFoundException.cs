using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace StudentsDetails.Infrastructure.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }

    public class UnprocessableEntityObjectException : Exception
    {
        public ModelStateDictionary ModelState { get; }

        public UnprocessableEntityObjectException(string message, ModelStateDictionary modelState)
            : base(message)
        {
            ModelState = modelState;
        }
    }

    public class ConflictObjectException : Exception
    {
        public ConflictObjectException(string message) : base(message) { }
    }
}
