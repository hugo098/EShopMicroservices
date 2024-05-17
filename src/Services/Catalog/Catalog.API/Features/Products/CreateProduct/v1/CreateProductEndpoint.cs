namespace Catalog.API.Features.Products.CreateProduct.v1;

public record CreateProductRequest(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price
);

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(pattern: "/products", handler: async (CreateProductRequest request, ISender sender) =>
        {
            CreateProductCommand command = request.Adapt<CreateProductCommand>();

            CreateProductResult result = await sender.Send(command);

            CreateProductResponse response = result.Adapt<CreateProductResponse>();

            return Results.Created($"/products/{response.Id}", response);
        })
        .WithTags("Products")
        .WithName("CreateProduct")
        .WithSummary("Create Product")
        .WithDescription("Create Product")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}