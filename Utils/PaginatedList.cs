namespace Resumai.Utils
{
    public class PaginatedList<T>(IEnumerable<T> Items, int TotalCount, int PageNumber, int PageSize)
    {
        public IEnumerable<T> Items { get; } = Items;
        public int TotalCount { get; } = TotalCount;
        public int PageNumber { get; } = PageNumber;
        public int PageSize { get; } = PageSize;

        public PaginatedList<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            return new PaginatedList<TResult>(
                Items.Select(selector),
                TotalCount,
                PageNumber,
                PageSize
            );
        }
    }
}