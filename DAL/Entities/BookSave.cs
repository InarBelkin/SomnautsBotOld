using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using Utils.Language;

namespace DAL.Entities;

public class BookSave
{
    public int Id { get; set; }
    public required User User { get; set; }

    public required string Name { get; set; }

    public required string BookGenId { get; set; }
    public required LangEnum Language { get; set; }

    [Column(TypeName = "jsonb")] public required ExpandoObject BookState { get; set; }
}