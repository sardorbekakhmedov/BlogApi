namespace BlogApi.HelperEntities;

public class Pagination
{
    public int CurrentPage { get; }
    public int TotalCollectionCount { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public bool HasPreviousPage => CurrentPage < 1;
    public bool HasNextPage => CurrentPage > TotalPages;

    public Pagination(int totalCollectionCount, int pageSize, int pageCount)
    {
        CurrentPage = pageCount;
        PageSize = pageSize;
        TotalCollectionCount = totalCollectionCount;
        TotalPages = (int)Math.Ceiling(totalCollectionCount / (double)pageSize);
    }

}