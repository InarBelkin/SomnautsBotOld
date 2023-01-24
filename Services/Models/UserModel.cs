using Utils.Language;

namespace Services.Models;

public class UserModel
{
    public required string UserName { get; init; }
    public required LangEnum InterfaceLang { get; init; }
    public required long TelegramId { get; set; }
}