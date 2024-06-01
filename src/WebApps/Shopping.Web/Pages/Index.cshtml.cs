namespace Shopping.Web.Pages;
public class IndexModel(
    ICatalogService catalogService,
    IBasketService basketService,
    ILogger<IndexModel> logger)
    : PageModel
{
    public IEnumerable<ProductModel> ProductList { get; set; } = new List<ProductModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogInformation("Index page visited");
        GetProductsResponse result = await catalogService.GetProducts();
        ProductList = result.Products;
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

        return RedirectToPage("Cart");
    }
}