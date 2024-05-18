using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BuildingBlocks.OpenApi;

public class ConfigureSwaggerGenOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly ApplicationOptions _applicationOptions;


    public ConfigureSwaggerGenOptions(
        IApiVersionDescriptionProvider provider, IOptions<ApplicationOptions> applicationOptions)
    {
        _provider = provider;
        _applicationOptions = applicationOptions.Value;
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
        {
            OpenApiInfo openApiInfo = new()
            {
                Title = $"{_applicationOptions.ApplicationName} v{description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
            };

            options.SwaggerDoc(description.GroupName, openApiInfo);
        }
    }

    public void Configure(SwaggerGenOptions options)
    {
        Configure(options);
    }
}