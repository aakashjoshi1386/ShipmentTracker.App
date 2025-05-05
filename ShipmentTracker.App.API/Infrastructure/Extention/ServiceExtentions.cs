namespace ShipmentTracker.App.API.Infrastructure.Extentions;
public static class ServiceExtentions
{
    public static async Task AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlLiteSettings = configuration.GetSection("SQLLiteSettings").Get<SQLLiteSettings>();

        services.AddApplication()
                .AddInfrastructure(sqlLiteSettings)
                .GetAwaiter();
    }
    public static async Task<IServiceCollection> AddCustomServices(this IServiceCollection services)
    {
        // Singelton Services
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        // Scoped Services
        services.AddScoped<IExceptionLoggerService, ExceptionLoggerService>();
        services.AddScoped<IShipmentService, ShipmentService>();
        services.AddScoped<ICarrierService, CarrierService>();
        return services;
    }
    public static void AddCustomValidators(this IServiceCollection services)
    {
        // Fluent Validation Init
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<GetShipmentValidator>();
        services.AddValidatorsFromAssemblyContaining<AddShipmentValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateShipmentValidator>();
    }
    public static async Task AddCustomSwgger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc($"{configuration.GetValue<string>("OpenApi:Version")!}",
                new OpenApiInfo
                {
                    Title = "Shipment Tracker APIs",
                    Description = "Shipment Tracker API description",
                    Contact = new OpenApiContact
                    {
                        Name = "Aakash Joshi",
                        Email = "aakashjoshi1386@gmail.com",
                        Url = new Uri("https://test.api.apache-licence.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT",
                        Url = new Uri("https://test.api.apache-licence.com")
                    },
                    Version = $"{configuration.GetValue<string>("OpenApi:Version")}"
                });
            #region TODO: Securing APIs implementing JWT MSAL tokens
            //options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            //{
            //    Description = "JWT Authorization header using the Bearer scheme.Example: \"Authorization: Bearer {token}\"",
            //    Name = "Authorization",
            //    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            //    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            //    Scheme = "Bearer"
            //});
            //options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            //{
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference
            //            {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = "Bearer"
            //            },
            //            Scheme = "oauth2",
            //            Name = "Bearer",
            //            In = ParameterLocation.Header
            //        },
            //        new List<string> ()
            //    }
            //}); 
            #endregion
        });
    }
    public static async Task AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSettings = configuration.GetSection("CORSSettings").Get<CORSSettings>();
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.WithOrigins(corsSettings.AllowedUrls)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials()
                      .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
            });
        });
    }
}
