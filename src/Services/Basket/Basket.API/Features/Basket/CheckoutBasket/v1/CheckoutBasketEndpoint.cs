namespace Basket.API.Features.Basket.CheckoutBasket.v1;

public record CheckoutBasketRequest(BasketCheckoutDto BasketCheckoutDto);
public record CheckoutBasketResponse(bool IsSuccess);

public class CheckoutBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            pattern: "/basket/checkout",
            handler: async (CheckoutBasketRequest request, ISender sender) =>
        {
            CheckoutBasketCommand command = request.Adapt<CheckoutBasketCommand>();

            CheckoutBasketResult result = await sender.Send(command);

            CheckoutBasketResponse response = result.Adapt<CheckoutBasketResponse>();

            return Results.Ok(response);
        })
        .WithTags("Basket")
        .WithName("CheckoutBasket")
        .WithSummary("Checkout Basket")
        .WithDescription("Checkout Basket")
        .Produces<CheckoutBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}