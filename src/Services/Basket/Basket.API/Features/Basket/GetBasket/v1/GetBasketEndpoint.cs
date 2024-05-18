namespace Basket.API.Features.Basket.GetBasket.v1;

//public record GetBasketRequest(string UserName);
public record GetBasketResponse(ShoppingCart Cart);

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(pattern: "/basket/{username}", handler: async (string username, ISender sender) =>
        {
            GetBasketResult result = await sender.Send(new GetBasketQuery(username));

            GetBasketResponse response = result.Adapt<GetBasketResponse>();

            return Results.Ok(response);
        })
        .WithTags("Basket")
        .WithName("GetBasket")
        .WithSummary("Get Basket")
        .WithDescription("Get Basket")
        .Produces<GetBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}