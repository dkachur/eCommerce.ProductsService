namespace eCommerce.ProductsService.API.Helpers;

public static class EnumHelper
{
    /// <summary>
    /// Creates a human readable string that lists all names from the specified Enum separated by commas,
    /// with the last element separated by <paramref name="lastSeparator"/>.
    /// </summary>
    /// <typeparam name="TEnum">The Enum type which names to list.</typeparam>
    /// <param name="lastSeparator">The last separator for the string. Default value is "or".</param>
    /// <returns>The created string.</returns>
    public static string GetOptionsString<TEnum>(string lastSeparator = "or") where TEnum : struct, Enum
    {
        var names = Enum.GetNames<TEnum>();

        if (names.Length == 0) return string.Empty;
        if (names.Length == 1) return names.First();

        return string.Join(", ", names.Take(names.Length - 1))
            + $" {lastSeparator} " + names.Last();
    }
}
