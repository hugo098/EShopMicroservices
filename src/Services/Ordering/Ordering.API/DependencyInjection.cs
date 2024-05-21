namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        Assembly assembly = typeof(Program).Assembly;
        string applicationName = Assembly.GetExecutingAssembly().GetName().Name!;

        services
            .AddSwaggerApiVersioning()
            .Configure<ApplicationOptions>(options => options.ApplicationName = applicationName)
            .ConfigureOptions<ConfigureSwaggerGenOptions>();

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app) 
    {


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
