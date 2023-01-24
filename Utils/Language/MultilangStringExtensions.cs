namespace Utils.Language;

public static class MultilangStringExtensions
{
    public static string WithErrorString(this Dictionary<string, string> dictionary, LangEnum key)
    {
        return dictionary.TryGetValue(key.Value, out var fromDict) ? fromDict : key.ErrorMessage;
    }
}