using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

var currentAssembly = typeof(Program).Assembly;

// Add services to the container.
builder.Services.AddFeaturesAssembly(currentAssembly)
    .AddMediatRPipelines()
    .AddControllers()
     .AddJsonOptions(configure =>
     {
         var options = configure.JsonSerializerOptions;
         options.PropertyNameCaseInsensitive = true;
         options.WriteIndented = false;
         options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
     }).AddApplicationPart(currentAssembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddTransient<ISlotAvailabilityService, SlotAvailabilityService>();
builder.Services.AddDocPlannerAPIClient(configure =>
{
    configure.UserName = AppSettingsProvider.DocPlannerOptions.User;
    configure.Password = AppSettingsProvider.DocPlannerOptions.Password;
    configure.BaseUrl = AppSettingsProvider.DocPlannerOptions.BaseUrl;
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = ApiVersion.Default;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();

}).AddApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/status", () =>
{
    return Results.Ok();
});

app.Run();
