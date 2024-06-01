namespace Shopping.Web.Extensions;

public static class ApiClientsExtensions
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddRefitClient<ICatalogService>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]!);
            });

        services
            .AddRefitClient<IBasketService>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]!);
            });

        services
            .AddRefitClient<IOrderingService>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]!);
            });

        return services;
    }
}
