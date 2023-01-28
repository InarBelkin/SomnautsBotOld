using BookServices.Models;
using Services.Models;

namespace Services.Dtos;

public record SearchResult(string Path, string Result);

public class ScanBooksResultDto
{
    public List<SearchResult> FoundBooks { get; set; } = new();
    public required string Message { get; set; }
}