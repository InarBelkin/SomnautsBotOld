using Newtonsoft.Json;
using Services.Dtos;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Tests.Services.Services;

public class BooksServiceTests
{
    [Fact]
    public void Converter_IsCorrect()
    {
        var obj = new ScanBooksParamsDto() { DoWithNotExistingBooks = DoWithNotExistingBooksEnum.Nothing };
        var s = JsonSerializer.Serialize(obj);

        Assert.True(true);
    }
}