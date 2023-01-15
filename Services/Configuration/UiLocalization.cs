using BookServices;

namespace Services.Configuration;

public class UiLocalization
{
    public required Dictionary<string, string> Help { get; init; }
    public required MultilangString Load { get; init; }
    public required MultilangString SelectUiLang { get; init; }
    public required MultilangString SelectBookLang { get; init; }
    public required MultilangString SelectBookLangNoCurrentBook { get; init; }
}