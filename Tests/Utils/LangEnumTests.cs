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

    [Fact]
    public void Object_Converter_IsCorrect()
    {
        var o = new LangHolder() { Language = LangEnum.En };
        var s = JsonSerializer.Serialize(o);
        var oBack = JsonSerializer.Deserialize<LangHolder>(s);
        Assert.Equal(o, oBack);
    }

    [Fact]
    public void MultilangString_Converter_IsCorrect2()
    {
        var mstring = new Dictionary<LangEnum, string>()
        {
            { LangEnum.En, "Hello" },
            { LangEnum.Ru, "Здорова" },
        };
        var json = JsonSerializer.Serialize(mstring);
        var deserialized = JsonSerializer.Deserialize<Dictionary<LangEnum, string>>(json);
        Assert.Equal(mstring, deserialized);
    }

    private class LangHolder
    {
        public required LangEnum Language { get; set; }
    }
}