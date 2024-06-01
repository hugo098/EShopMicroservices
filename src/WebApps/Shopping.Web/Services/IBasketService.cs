namespace Shopping.Web.Services;

public interface IBasketService
{
    [Get("/basket-service/api/v1/basket/{userName}")]
    Task<GetBasketResponse> GetBasket(string userName);

    [Post("/basket-service/api/v1/basket")]
    Task<StoreBasketResponse> StoreBasket(StoreBasketRequest request);

    [Delete("/basket-service/api/v1/basket/{userName}")]
    Task<DeleteBasketResponse> DeleteBasket(string userName);

    [Post("/basket-service/api/v1/basket/checkout")]
    Task<CheckoutBasketResponse> CheckoutBasket(CheckoutBasketRequest request);

    public async Task<ShoppingCartModel> LoadUserBasket()
    {
        var username = "hugo";
        ShoppingCartModel basket;

        try
        {
            GetBasketResponse getBasketResponse = await GetBasket(username);

            basket = getBasketResponse.Cart;
        }
        catch (Exception)
        {
            basket = new ShoppingCartModel
            {
                Username = username,
                Items = []
            };
        }

        return basket;
    }
}