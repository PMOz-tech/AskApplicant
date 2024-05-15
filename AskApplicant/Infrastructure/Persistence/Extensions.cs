using AskApplicant.Core.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AskApplicant.Infrastructure.Persistence
{
    public static class Extensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AskApplicantDbSettings>(configuration.GetSection(nameof(AskApplicantDbSettings)));

            services.AddSingleton<AskApplicantDbContext>(serviceProvider =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<AskApplicantDbSettings>>().Value;
                return new AskApplicantDbContext(settings.ConnectionString, settings.DatabaseName);
            });

            return services;
        }
    }
}
