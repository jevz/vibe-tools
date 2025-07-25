namespace VibeTools.Api.Extensions;

public static class StringExtensions
{
    public static string PascalToCamelCase(this string value)
    {
        if (string.IsNullOrEmpty(value) || char.IsLower(value[0])) return value;
        return char.ToLowerInvariant(value[0]) + value[1..];
    }
}