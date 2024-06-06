namespace PizzaPlace.Extensions
{
    public static class StringExtensions
    {
        public static T? TryGetEnum<T>(this string? value) where T : struct, Enum =>
            Enum.TryParse(value, true, out T enumValue) ? enumValue : null;
    }
}
