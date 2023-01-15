namespace DAL.Entities;

public class User
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public string? InterfaceLang { get; set; }

    public int? CurrentSaveId { get; set; } //TODO: cyclic dependency
    public int? LastMessageId { get; set; }
    public long? TelegramId { get; set; }
    public List<BookSave> Saves { get; set; } = new();
}