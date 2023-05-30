using Microsoft.Extensions.DependencyInjection;
using StudentsDetails.Controllers;
using System;

namespace StudentsDetails.Infrastructure.Extensions.Automapper
{
    public static partial class AutoMapperProfile
    {
        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddAutoMapper(x => x.AddProfile(new StudentsModelMapping()));

            return services;
        }
    }
}
