
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false; // Disable the server header
});
// Add dependencies
builder.Services.AddDependencies(builder.Configuration).GetAwaiter();
// Add Configuration and Environment
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables();
// Add Custom Services
builder.Services.AddCustomServices().GetAwaiter();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Add Custom Validations
builder.Services.AddCustomValidators();
builder.Services.AddEndpointsApiExplorer();
// Add Custom Swagger Settings
builder.Services.AddCustomSwgger(builder.Configuration).GetAwaiter();
builder.Services.AddHttpContextAccessor();
// Add Custom CORS Policies
builder.Services.AddCustomCors(builder.Configuration).GetAwaiter();
// Add Custom Exception Handler
//builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); // TODO: Implement Custom Exception Handler
builder.Services.AddProblemDetails();
// Add API Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.AddInMemoryRateLimiting();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Limit = 100,
            Period = "1m"
        }
    };
});
// CSRF
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});
//builder.Services.AddSwaggerGen();

var app = builder.Build();

//Seeding the Database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ShipmentTrackerAppDBContext>();
    DbSeeder.Seed(dbContext);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        string openApiVersion = builder.Configuration.GetValue<string>("OpenApi:Version")!;
        options.SwaggerEndpoint($"/swagger/{openApiVersion}/swagger.json", $"API {openApiVersion}");
    });
}

app.UseIpRateLimiting();
app.UseCors("CorsPolicy");
app.UseExceptionHandler();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
