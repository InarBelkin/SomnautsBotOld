namespace Services.Configuration;

public class UiLocalization
{
    public required Dictionary<string, string> Help { get; init; }
    public required Dictionary<string, string> Load { get; init; }
    public required Dictionary<string, string> SelectUiLang { get; init; }
    public required Dictionary<string, string> SelectBookLang { get; init; }
    public required Dictionary<string, string> SelectBookLangNoCurrentBook { get; init; }
}