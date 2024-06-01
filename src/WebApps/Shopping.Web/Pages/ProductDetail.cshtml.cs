namespace Shopping.Web.Pages;

public class ProductDetailModel(
    ICatalogService catalogService,
    IBasketService basketService,
    ILogger<ProductDetailModel> logger) 
    : PageModel
{
    public ProductModel Product { get; set; } = default!;

    [BindProperty]
    public string Color { get; set; } = default!;

    [BindProperty]
    public int Quantity { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid productId)
    {
        GetProductByIdResponse response = await catalogService.GetProduct(productId);

        Product = response.Product;

        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
    {
        logger.LogInformation("Add to cart button clicked");
        GetProductByIdResponse productResponse = await catalogService.GetProduct(productId);

        ShoppingCartModel basket = await basketService.LoadUserBasket();

        basket.Items.Add(new ShoppingCartItemModel
        {
            ProductId = productId,
            ProductName = productResponse.Product.Name,
            Price = productResponse.Product.Price,
            Quantity = 1,
            Color = "Black"
        });

        await basketService.StoreBasket(new StoreBasketRequest(basket));

        return RedirectToPage("Cart");
    }
}
