using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.Endpoints.v1;

//- Accepts a CreateOrderRequest object.
//- Maps the request to a CreateOrderCommand.
//- Uses MediatR to send the command to the corresponding handler.
//- Returns a response with the created order's ID.

public record CreateOrderRequest(OrderDto Order);
public record CreateOrderResponse(Guid Id);
public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(pattern: "/orders", handler: async (CreateOrderRequest request, ISender sender) =>
        {
            CreateOrderCommand command = request.Adapt<CreateOrderCommand>();

            CreateOrderResult result = await sender.Send(command);

            CreateOrderResponse response = result.Adapt<CreateOrderResponse>();

            return Results.Created(uri: $"/orders/{response.Id}", value: response);
        })
        .WithTags("Orders")
        .WithName("CreateOrder")
        .WithSummary("Create Order")
        .WithDescription("Create Order")
        .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}