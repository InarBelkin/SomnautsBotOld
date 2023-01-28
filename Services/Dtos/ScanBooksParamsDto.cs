namespace Services.Dtos;

public class ScanBooksParamsDto
{
    public required DoWithNotExistingBooksEnum DoWithNotExistingBooks { get; init; }
}