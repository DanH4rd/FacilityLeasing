using Microsoft.EntityFrameworkCore;
using FacilityLeasing.API.Infrastructure;
using FacilityLeasing.API.Presentation;
using FacilityLeasing.API.Abstract;
using FluentValidation;
using FacilityLeasing.API.Infrastructure.BakgroundProcessing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer(); // include routes to OpenAPI spec
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CustomExceptionHandler>(); // register error custom handler
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddSwagger();
builder.Services.AddFacilityDbContext();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IContractProcessing, ContractProcessing>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateContractValidator>();

// register background processing service
builder.Services.AddSingleton<ContractTaskQueue>();
builder.Services.AddHostedService<ContractProcessingService>();

builder.Logging.ClearProviders().AddConsole();


var app = builder.Build();

// apply db changes (migrations) automatically
if (!(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "").Equals("Testing"))
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<FacilityDBContext>();
        dbContext.Database.Migrate();
    }

//if (app.Environment.IsDevelopment())
//{
    app.MapOpenApi(); // url to see the doc: /openapi/v1.json 
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseExceptionHandler(); // Converts unhandled exceptions into Problem Details responses
app.UseStatusCodePages(); // Returns the Problem Details response for (empty) non-successful responses

app.ConfigureRoutes();

app.Run();

// define class to make tests available
namespace FacilityLeasing.API
{
    public partial class Program { }
}
