using Discount.Grpc.Data;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddDbContext<DiscountContext>(opts =>
{
    opts.UseSqlite(builder.Configuration.GetConnectionString("Database"));
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMigration();
app.MapGrpcService<DiscountService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

if (app.Environment.IsDevelopment())
    app.MapGrpcReflectionService();


app.Run();
