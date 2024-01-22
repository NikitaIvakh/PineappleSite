namespace PineappleSite.Presentation.Models.Paginated
{
    public class PaginatedList<Type> : List<Type>
    {
        public PaginatedList(List<Type> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)(Math.Ceiling(count / (double)pageSize));

            AddRange(items);
        }

        public static PaginatedList<Type> Create(IQueryable<Type> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            if (pageIndex > totalPages)
                pageIndex = totalPages;

            else if (pageIndex <= 1)
                pageIndex = 1;

            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<Type>(items, count, pageIndex, pageSize);
        }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviosPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;
    }
}