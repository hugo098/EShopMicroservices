namespace Catalog.API.Features.Products.GetProducts.v1;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
public record GetProductsResult(
    IEnumerable<Product> Products,
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

internal class GetProductsQueryHandler(IDocumentSession session) 
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        IPagedList<Product> products = await session
            .Query<Product>()
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        return new GetProductsResult(
            Products: products,
            Count: products.Count,
            TotalItemCount: products.TotalItemCount,
            PageNumber: products.PageNumber,
            PageSize: products.PageSize,
            PageCount: products.PageCount,
            HasNextPage: products.HasNextPage,
            HasPreviousPage: products.HasPreviousPage,
            IsFirtsPage: products.IsFirstPage,
            IsLastPage: products.IsLastPage
          );
    }
}