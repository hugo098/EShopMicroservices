using Ordering.Application.Orders.Queries.GetOrdersByName;

namespace Ordering.API.Endpoints.v1;

//- Accepts a name parameter.
//- Constructs a GetOrdersByNameQuery.
//- Retrieves and returns matching orders.

//public record GetOrdersByNameRequest(string name);

public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);

public class GetOrdersByName : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(pattern: "/orders/{orderName}", handler: async (string orderName, ISender sender) =>
        {
            GetOrdersByNameQuery query = new(orderName);

            GetOrdersByNameResult result = await sender.Send(query);

            GetOrdersByNameResponse response = result.Adapt<GetOrdersByNameResponse>();

            return Results.Ok(response);
        })
        .WithTags("Orders")
        .WithName("GetOrdersByName")
        .WithSummary("Get Orders by Name")
        .WithDescription("Get Orders by Name")
        .Produces<GetOrdersByNameResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}