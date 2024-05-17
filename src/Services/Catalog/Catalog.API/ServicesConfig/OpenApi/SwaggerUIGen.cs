using Asp.Versioning.ApiExplorer;

namespace Catalog.API.ServicesConfig.OpenApi;

public static class SwaggerUIGen
{
    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            IApiVersionDescriptionProvider provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
            {
                string url = $"/swagger/{description.GroupName}/swagger.json";
                string name = description.GroupName.ToUpperInvariant();

                options.SwaggerEndpoint(url, name);
            }
        });

        return app;
    }
}