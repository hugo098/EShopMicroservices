namespace Catalog.API.Features.Products.GetProducts.v1;

public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);
public record GetProductsResponse(
    IEnumerable<Product> Products,
    long Count,
    long TotalItemCount,
    long PageNumber,
    long PageSize,
    long PageCount,
    bool HasNextPage,
    bool HasPreviousPage,
    bool IsFirtsPage,
    bool IsLastPage
);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(pattern: "/products", handler: async ([AsParameters] GetProductsRequest request, ISender sender) =>
        {
            GetProductsQuery query = request.Adapt<GetProductsQuery>();

            GetProductsResult result = await sender.Send(query);

            GetProductsResponse response = result.Adapt<GetProductsResponse>();

            return Results.Ok(response);
        })
        .WithTags("Products")
        .WithName("GetProducts")
        .WithSummary("Get Products")
        .WithDescription("Get Products")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}