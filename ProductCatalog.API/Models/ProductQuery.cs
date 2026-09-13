namespace ProductCatalog.API.Models;

public enum ProductSortBy { CreatedAt, Name, Price }
public enum SortDirection { Asc, Desc }

public class ProductQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public ProductSortBy SortBy { get; set; } = ProductSortBy.CreatedAt;
    public SortDirection SortDir { get; set; } = SortDirection.Desc;
    public string? Search { get; set; }
}
