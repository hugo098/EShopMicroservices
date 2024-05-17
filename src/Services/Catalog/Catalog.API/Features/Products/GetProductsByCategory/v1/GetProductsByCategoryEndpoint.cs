namespace Catalog.API.Features.Products.GetProductsByCategory.v1;

//public record GetProductsByCategoryRequest();
public record GetProductsByCategoryResponse(IEnumerable<Product> Products);

public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(pattern: "/products/category/{category}", handler: async (string category, ISender sender) =>
        {
            GetProductsByCategoryResult result = await sender.Send(new GetProductsByCategoryQuery(category));

            GetProductsByCategoryResponse response = result.Adapt<GetProductsByCategoryResponse>();

            return Results.Ok(response);
        })
        .WithTags("Products")
        .WithName("GetProductsByCategory")
        .WithSummary("Get Product by category")
        .WithDescription("Get Product by category")
        .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}
