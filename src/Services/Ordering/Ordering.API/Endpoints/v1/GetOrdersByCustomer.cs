using Ordering.Application.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.API.Endpoints.v1;

//- Accepts a customer ID.
//- Uses a GetOrdersByCustomerQuery to fetch orders.
//- Returns the list of orders for that customer.

//public record GetOrdersByCustomerRequest(string name);

public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);

public class GetOrdersByCustomer : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(pattern: "/orders/customer/{customerId}", handler: async (Guid customerId, ISender sender) =>
        {
            GetOrdersByCustomerQuery query = new(customerId);

            GetOrdersByCustomerResult result = await sender.Send(query);

            GetOrdersByCustomerResponse response = result.Adapt<GetOrdersByCustomerResponse>();

            return Results.Ok(response);
        })
        .WithTags("Orders")
        .WithName("GetOrdersByCustomer")
        .WithSummary("Get Orders by Customer")
        .WithDescription("Get Orders by Customer")
        .Produces<GetOrdersByCustomerResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}