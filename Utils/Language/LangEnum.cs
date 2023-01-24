using System.Text.Json.Serialization;
using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;

namespace Utils.Language;

[JsonConverter(typeof(SmartEnumValueConverter<LangEnum, string>))]
public sealed class LangEnum : SmartEnum<LangEnum, string>
{
    public static readonly LangEnum En = new LangEnum(nameof(En), "en", "eng", "english")
        { ErrorMessage = "String resource not found" };

    public static readonly LangEnum Ru = new LangEnum(nameof(Ru), "ru", "rus", "russian")
        { ErrorMessage = "Строка не найдена" };

    public static readonly LangEnum De = new LangEnum(nameof(De), "de") { ErrorMessage = "" };

    private LangEnum(string name, string value, params string[] aliases) : base(name, value)
    {
        _aliases = new[] { value }.Concat(aliases).ToArray();
    }

    private readonly string[] _aliases;
    public IEnumerable<string> Aliases => _aliases;

    public required string ErrorMessage { get; init; }

    public static LangEnum GetNearestLang(string languageCode)
    {
        LangEnum.TryFromName(languageCode, true, out var result);
        result ??= En;
        return result;
    }

    public static LangEnum GetAvailableLang(LangEnum neededLang, LangEnum[] availableLangs) =>
        availableLangs.Contains(neededLang) ? neededLang : availableLangs.OrderBy(l => l.Value).First();
}