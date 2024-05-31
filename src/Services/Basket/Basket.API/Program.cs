using BuildingBlocks.Messaging.MassTransit;
using Discount.Grpc;
using System.Net.Security;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Assembly assembly = typeof(Program).Assembly;
string posgresConnectionString = builder.Configuration.GetConnectionString("Database")!;
string redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;
string applicationName = Assembly.GetExecutingAssembly().GetName().Name!;

// Add services to container.

// Application Services
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddCarter();

// Data Services
builder.Services.AddMarten(opts =>
{
    opts.Connection(posgresConnectionString);
    opts.Schema.For<ShoppingCart>().Identity(x => x.Username);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    //options.InstanceName = "Basket";
});

// Grpc Service
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    HttpClientHandler handler = new()
    {
        ServerCertificateCustomValidationCallback = builder.Environment.IsDevelopment() ? HttpClientHandler.DangerousAcceptAnyServerCertificateValidator : null,
    };

    return handler;
});

//Async Communication services
builder.Services.AddMessageBroker(builder.Configuration);

// Cross-Cutting Services
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services
    .AddSwaggerApiVersioning()
    .Configure<ApplicationOptions>(options => options.ApplicationName = applicationName)
    .ConfigureOptions<ConfigureSwaggerGenOptions>();

builder.Services
    .AddHealthChecks()
    .AddNpgSql(posgresConnectionString)
    .AddRedis(redisConnectionString);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
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
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

        foreach (ApiVersionDescription description in descriptions)
        {
            string url = $"/swagger/{description.GroupName}/swagger.json";
            string name = description.GroupName;

            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseHealthChecks(
    path: "/health",
    options: new HealthCheckOptions()
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();