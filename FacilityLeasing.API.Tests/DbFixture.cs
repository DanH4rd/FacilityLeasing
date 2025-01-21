using FacilityLeasing.API.Infrastructure;
using FacilityLeasing.API.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FacilityLeasing.API.Tests
{
    public class DBFixture : IDisposable
    {
        public WebApplicationFactory<FacilityLeasing.API.Program> Factory { get; private set; } = null!;
        public HttpClient Client { get; private set; } = null!;
        private string _dbName;

        public DBFixture()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
            Environment.SetEnvironmentVariable("X_AUTH_HEADER", "a8ee3b24-2cf1-49fd-95cd-61874d340734");

            // unique test db name
            _dbName = $"Db_{Guid.NewGuid()}";

            // create factory for api
            Factory = CreateFactory<FacilityDBContext, FacilityLeasing.API.Program>(_dbName);

            // create http client
            Client = Factory.CreateClient();
        }

        private WebApplicationFactory<TProgram> CreateFactory<TContext, TProgram>(string databaseName)
            where TContext : DbContext
            where TProgram : class
        {
            return new WebApplicationFactory<TProgram>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // register InMemory Database instead of a real one
                        services.AddDbContext<TContext>(options =>
                            options.UseInMemoryDatabase(databaseName));

                        // init db
                        using var scope = services.BuildServiceProvider().CreateScope();
                        var context = scope.ServiceProvider.GetRequiredService<TContext>();
                        context.Database.EnsureCreated(); // create db

                        // add initial data
                        (context as FacilityDBContext)!.ProductionFacilities.Add(new ProductionFacility
                        {
                            Code = "FAC001",
                            Name = "Central Manufacturing Unit",
                            StandardArea = 10000,
                            CreatedDate = DateTime.UtcNow,
                            IsActive = true
                        });

                        (context as FacilityDBContext)!.ProcessEquipment.Add(new ProcessEquipment
                        {
                            Code = "EQP001",
                            Name = "CNC Milling Machine",
                            Area = 25,
                            CreatedDate = DateTime.UtcNow,
                            IsActive = true
                        });

                        context.SaveChanges();
                    });
                });
        }

        public void Dispose()
        {
            // remove db
            using (var scope = Factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FacilityDBContext>();
                context.Database.EnsureDeleted();
            }

            Factory.Dispose();
        }
    }
}
