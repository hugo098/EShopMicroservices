namespace Shopping.Web.Services;

public interface ICatalogService
{
    [Get("/catalog-service/api/v1/products?pageNumber={pageNumber}&pageSize={pageSize}")]
    Task<GetProductsResponse> GetProducts(int? pageNumber = 1, int? pageSize = 10);

    [Get("/catalog-service/api/v1/products/category/{category}")]
    Task<GetProductsByCategoryResponse> GetProductsByCategory(string category);

    [Get("/catalog-service/api/v1/products/{id}")]
    Task<GetProductByIdResponse> GetProduct(Guid id);
}