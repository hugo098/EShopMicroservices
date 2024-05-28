using Asp.Versioning;
using Asp.Versioning.Builder;
using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        Assembly assembly = typeof(Program).Assembly;
        string applicationName = Assembly.GetExecutingAssembly().GetName().Name!;

        services
            .AddSwaggerApiVersioning()
            .Configure<ApplicationOptions>(options => options.ApplicationName = applicationName)
            .ConfigureOptions<ConfigureSwaggerGenOptions>();

        services.AddCarter();

        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("Database")!);

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        ApiVersionSet apiVersionSet = app
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder versionGroup = app
            .MapGroup("api/v{apiVersion:apiVersion}")
            .WithApiVersionSet(apiVersionSet);

        versionGroup.MapCarter();

        app.UseExceptionHandler(options => { });

        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
           

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

        return app;
    }
}