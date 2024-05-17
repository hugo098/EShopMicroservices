namespace Catalog.API.Features.Products.GetProductById.v1;

//public record GetProductByIdRequest();
public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(pattern: "/products/{productId}", handler: async (Guid productId, ISender sender) =>
        {
            GetProductByIdResult result = await sender.Send(new GetProductByIdQuery(productId));

            GetProductByIdResponse response = result.Adapt<GetProductByIdResponse>();

            return Results.Ok(response);
        })
        .WithTags("Products")
        .WithName("GetProductById")
        .WithSummary("Get Product by id")
        .WithDescription("Get Product by id")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}