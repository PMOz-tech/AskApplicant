using FluentValidation.AspNetCore;
using FluentValidation;
using MongoDB.Driver.Core.Authentication;
using System.Data.Common;
using AskApplicant.Core.Application.Implementation;
using AskApplicant.Core.Application.Services;
using AskApplicant.Core.Models.Requests;
using System.Reflection;

namespace AskApplicant.Core.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmployerService, EmployerService>();
            services.AddScoped<IApplicantService, ApplicantService>();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<CreateApplicationForm>();
            services.AddValidatorsFromAssemblyContaining<EditApplicationForm>();

            return services;
        }
    }
}
