using BuildingBlocks.Pagination;
using Ordering.Application.Orders.Queries.GetOrders;

namespace Ordering.API.Endpoints.v1;

//- Accepts pagination parameters.
//- Constructs a GetOrdersQuery with these parameters.
//- Retrieves the data and returns it in a paginated format.

//public record GetOrdersRequest(PaginationRequest PaginationRequest);

public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

public class GetOrders : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(pattern: "orders/", handler: async ([AsParameters] PaginationRequest request, ISender sender) =>
        {
            GetOrdersQuery query = new(request);

            GetOrdersResult result = await sender.Send(query);

            GetOrdersResponse response = result.Adapt<GetOrdersResponse>();

            return response;
        })
        .WithTags("Orders")
        .WithName("GetOrders")
        .WithSummary("Get Orders")
        .WithDescription("Get Orders")
        .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}