using Microsoft.OpenApi.Models;

namespace FacilityLeasing.API.Infrastructure
{
    /// <summary>
    /// Swagger config to allow Authorization header.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("CustomHeader", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = "X-AUTH",
                    In = ParameterLocation.Header,
                    Description = "Please enter your X-AUTH header",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "CustomHeader"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }
    }
}
