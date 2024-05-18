namespace Basket.API.Features.Basket.GetBasket.v1;

public record GetBasketQuery(string Username) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart Cart);

public class GetBasketQueryHandler(IBasketRepository repository)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        ShoppingCart basket = await repository.GetBasket(query.Username, cancellationToken);

        return new GetBasketResult(basket);
    }
}