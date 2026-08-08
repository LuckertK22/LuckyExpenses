namespace LuckyExpenses.Application.Common
{
    public class PagedResponse<T>
    {
        public IReadOnlyList<T> Items { get; set; } = [];

        public int TotalItems { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }
    }
}
