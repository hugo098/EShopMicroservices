namespace Catalog.API.Features.Products.DeleteProduct.v1;

//public record DeleteProductRequest(Guid Id);
public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(pattern: "/products/{productId}", handler: async (Guid productId, ISender sender) =>
        {
            DeleteProductResult result = await sender.Send(new DeleteProductCommand(productId));

            DeleteProductResponse response = result.Adapt<DeleteProductResponse>();

            return Results.Ok(response);
        })
        .WithTags("Products")
        .WithName("DeleteProduct")
        .WithSummary("Delete Product")
        .WithDescription("Delete Product")
        .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}