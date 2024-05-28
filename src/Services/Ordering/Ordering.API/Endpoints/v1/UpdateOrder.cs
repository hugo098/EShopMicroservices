using Ordering.Application.Orders.Commands.UpdateOrder;

namespace Ordering.API.Endpoints.v1;

//- Accepts a UpdateOrderRequest.
//- Maps the request to an UpdateOrderCommand.
//- Sends the command for processing.
//- Returns a success or error response based on the outcome.

public record UpdateOrderRequest(OrderDto Order);
public record UpdateOrderResponse(bool IsSuccess);
public class UpdateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(pattern: "/orders", handler: async (UpdateOrderRequest request, ISender sender) =>
        {
            UpdateOrderCommand command = request.Adapt<UpdateOrderCommand>();

            UpdateOrderResult result = await sender.Send(command);

            UpdateOrderResponse response = result.Adapt<UpdateOrderResponse>();

            return Results.Ok(response);
        })
        .WithTags("Orders")
        .WithName("UpdateOrder")
        .WithSummary("Update Order")
        .WithDescription("Update Order")
        .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}