namespace Basket.API.Features.Basket.StoreBasket.v1;

public record StoreBasketRequest(ShoppingCart Cart);
public record StoreBasketResponse(string Username);

public class StoreBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(pattern: "/basket", handler: async (StoreBasketRequest request, ISender sender) =>
        {
            StoreBasketCommand command = request.Adapt<StoreBasketCommand>();

            StoreBasketResult result = await sender.Send(command);

            StoreBasketResponse response = result.Adapt<StoreBasketResponse>();

            return Results.Created($"/basket/{response.Username}", response);
        })
        .WithTags("Basket")
        .WithName("StoreBasket")
        .WithSummary("Store Basket")
        .WithDescription("Store Basket")
        .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}