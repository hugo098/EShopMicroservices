using Asp.Versioning;
using Asp.Versioning.Builder;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
Assembly? assembly = typeof(Program).Assembly;

string posgresConnectionString = builder.Configuration.GetConnectionString("Database")!;

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(opts =>
{
    opts.Connection(posgresConnectionString);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services
    .AddSwaggerApiVersioning()
    .ConfigureOptions<ConfigureSwaggerGenOptions>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(posgresConnectionString);


WebApplication app = builder.Build();

// Configure the http request pipeline
ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

RouteGroupBuilder versionGroup = app
    .MapGroup("api/v{apiVersion:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

versionGroup.MapCarter();

app.UseSerilogRequestLogging();

app.UseExceptionHandler(options => { });

if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();    
}

app.UseHealthChecks(path: "/health",
    new HealthCheckOptions()
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();