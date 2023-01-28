namespace Utils;

public static class UtilsExtensions
{
    public static bool TryGetFirst<T>(this IEnumerable<T> collection, out T value)
    {
        try
        {
            value = collection.First();
        }
        catch (InvalidOperationException e)
        {
            value = default;
            return false;
        }

        return true;
    }
}