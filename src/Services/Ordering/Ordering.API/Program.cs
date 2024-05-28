using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});

builder.Services
       .AddApplicationServices()
       .AddInfrastructureServices(builder.Configuration)
       .AddApiServices(builder.Configuration);

WebApplication app = builder.Build();

// Configura the HTTP request pipeline.
app.UseSerilogRequestLogging();
app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.Run();