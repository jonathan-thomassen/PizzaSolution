namespace PizzaPlace
{
    public static class EnumerableExtension
    {
        public static ComparableList<T> ToComparableList<T>(this IEnumerable<T> values) =>
            new(values);
    }
}
