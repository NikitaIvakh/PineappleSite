namespace PineappleSite.Presentation.Models.Paginated;

public sealed class PaginatedList<T> : List<T>
{
    private PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)(Math.Ceiling(count / (double)pageSize));

        AddRange(items);
    }

    public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = source.Count();
        var totalPages = (int)Math.Ceiling(count / (double)pageSize);

        if (pageIndex > totalPages)
            pageIndex = totalPages;

        else if (pageIndex <= 1)
            pageIndex = 1;

        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }

    public int PageIndex { get; init; }

    public int TotalPages { get; init; }

    public bool HasPreviosPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;
}