namespace LuckyExpenses.Shared.Utils
{
    public static class StringExtensions
    {
        public static string ToLowerEachProperty(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return string
                .Join('.', value.Split('.')
                    .Select(segment =>
                        string.IsNullOrEmpty(segment)
                            ? segment
                            : char.ToLowerInvariant(segment[0]) + segment[1..]));
        }
    }
}
