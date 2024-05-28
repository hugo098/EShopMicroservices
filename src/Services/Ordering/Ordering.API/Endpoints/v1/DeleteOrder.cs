using Ordering.Application.Orders.Commands.DeleteOrder;

namespace Ordering.API.Endpoints.v1;

//- Accepts the order ID as a parameter.
//- Constructs a DeleteOrderCommand.
//- Sends the command using MediatR.
//- Returns a success or not found response.

//public record DeleteOrderRequest(Guid Id);

public record DeleteOrderResponse(bool IsSuccess);
public class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(pattern: "/orders/{orderId}", handler: async (Guid orderId, ISender sender) =>
        {
            DeleteOrderCommand command = new(orderId);

            DeleteOrderResult result = await sender.Send(command);

            DeleteOrderResponse response = result.Adapt<DeleteOrderResponse>();

            return Results.Ok(response);
        })
        .WithTags("Orders")
        .WithName("DeleteOrder")
        .WithSummary("Delete Order")
        .WithDescription("Delete Order")
        .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}