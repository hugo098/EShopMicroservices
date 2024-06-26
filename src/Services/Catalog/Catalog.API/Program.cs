WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Assembly assembly = typeof(Program).Assembly;
string posgresConnectionString = builder.Configuration.GetConnectionString("Database")!;
string applicationName = Assembly.GetExecutingAssembly().GetName().Name!;

// Add services to the container

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
    .Configure<ApplicationOptions>(options => options.ApplicationName = applicationName)
    .ConfigureOptions<ConfigureSwaggerGenOptions>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services
    .AddHealthChecks()
    .AddNpgSql(posgresConnectionString);


WebApplication app = builder.Build();

// Configure the http request pipeline
ApiVersionSet apiVersionSet = app
            .NewApiVersionSet()
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