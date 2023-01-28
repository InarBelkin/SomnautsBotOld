using System.ComponentModel.DataAnnotations.Schema;
using BookServices.Models;

namespace DAL.Entities;

public class Book
{
    public int Id { get; set; }
    public required string ContainingFolder { get; set; }
    public required bool IsVisibleToUsers { get; set; }
    [Column(TypeName = "jsonb")] public required BookDescriptionModel BookDescription { get; set; }
}