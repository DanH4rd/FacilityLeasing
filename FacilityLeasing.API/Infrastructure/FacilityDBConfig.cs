using Microsoft.EntityFrameworkCore;

namespace FacilityLeasing.API.Infrastructure
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFacilityDbContext(this IServiceCollection services)
        {
            // don't register db context for testing env, it will be registered from tests project
            if (!(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "").Equals("Testing"))
            {
                var dbName = Environment.GetEnvironmentVariable("DB_NAME");
                var dbUser = Environment.GetEnvironmentVariable("DB_USER");
                var dbPass = Environment.GetEnvironmentVariable("DB_PASSWORD");
                var dbHost = Environment.GetEnvironmentVariable("DB_SERVER");
                var dbPort = Environment.GetEnvironmentVariable("DB_PORT");

                // compose connection string
                var connectionString = $"Server={dbHost},{dbPort};Database={dbName};User Id={dbUser};Password={dbPass};TrustServerCertificate=True;Pooling=True;";

                // register DbContext with connection string
                services.AddDbContext<FacilityDBContext>(options => options.UseSqlServer(connectionString));
            }

            return services;
        }
    }
}
