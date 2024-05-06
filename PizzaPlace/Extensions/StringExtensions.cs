namespace PizzaPlace.Extensions;

public static class StringExtensions
{
    public static T? TryGetEnum<T>(this string? value) where T : struct, Enum =>
        Enum.TryParse<T>(value, true, out var enumValue) ? enumValue : null;
}
