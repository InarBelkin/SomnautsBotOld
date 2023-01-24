using System.Text.Json;
using System.Text.Json.Serialization;
using Utils.Language;

namespace Tests.Utils;

public class LangEnumTests
{
    [Fact]
    public void Converter_IsCorrect()
    {
        var l = LangEnum.En;
        var s = JsonSerializer.Serialize(l);
        var lBack = JsonSerializer.Deserialize<LangEnum>(s);
        Assert.Equal(l, lBack);
    }
}