namespace Resumai.Utils
{
    public static class IQueryableExtensions
    {
        public static PaginatedList<T> Paginate<T>(this IQueryable<T> query, PageRequest pageRequest)
        {
            var totalCount = query.Count();
            var items = query
                .Skip((pageRequest.PageNumber - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToList();

            return new PaginatedList<T>(items, totalCount, pageRequest.PageNumber, pageRequest.PageSize);
        }
    }
}