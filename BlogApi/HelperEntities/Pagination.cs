namespace BlogApi.HelperEntities;

public class Pagination
{
    public int CurrentPage { get; }
    public int TotalCollectionCount { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public bool HasPreviousPage => CurrentPage < 1;
    public bool HasNextPage => CurrentPage > TotalPages;
}