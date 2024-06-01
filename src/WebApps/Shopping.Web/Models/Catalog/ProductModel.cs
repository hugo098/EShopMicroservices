namespace Shopping.Web.Models.Catalog;

public class ProductModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<string> Category { get; set; } = new();
    public string Description { get; set; } = default!;
    public string ImageFile { get; set; } = default!;
    public decimal Price { get; set; }
}

public record GetProductsResponse(
    IEnumerable<ProductModel> Products,
    long Count,
    long TotalItemCount,
    long PageNumber,
    long PageSize,
    long PageCount,
    bool HasNextPage,
    bool HasPreviousPage,
    bool IsFirtsPage,
    bool IsLastPage
);

public record GetProductsByCategoryResponse(IEnumerable<ProductModel> Products);

public record GetProductByIdResponse(ProductModel Product);