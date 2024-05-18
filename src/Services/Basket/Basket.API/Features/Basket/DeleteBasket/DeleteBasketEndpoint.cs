namespace Basket.API.Features.Basket.DeleteBasket;

//public record DeleteBasketRequest(string Username);
public record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(pattern: "/basket/{username}", handler: async (string username, ISender sender) =>
        {
            DeleteBasketResult result = await sender.Send(new DeleteBasketCommand(username));

            DeleteBasketResponse response = result.Adapt<DeleteBasketResponse>();

            return Results.Ok(response);
        })
        .WithTags("Basket")
        .WithName("DeleteBasket")
        .WithSummary("Delete Basket")
        .WithDescription("Delete Basket")
        .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}