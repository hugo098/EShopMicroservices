namespace Catalog.API.Features.Products.UpdateProduct.v1;

public record UpdateProductRequest(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price
);

public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(pattern: "/products", handler: async (UpdateProductRequest request, ISender sender) =>
        {
            UpdateProductCommand command = request.Adapt<UpdateProductCommand>();

            UpdateProductResult result = await sender.Send(command);

            UpdateProductResponse response = result.Adapt<UpdateProductResponse>();

            return Results.Ok(response);
        })
        .WithTags("Products")
        .WithName("UpdateProduct")
        .WithSummary("Update Product")
        .WithDescription("Update Product")
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}