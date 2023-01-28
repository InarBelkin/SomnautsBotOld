using System.Text.Json;
using BookServices.Models;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services.Configuration;
using Services.Dtos;
using Services.Models;
using Utils;

namespace Services.Services;

public interface IBooksService
{
    Task<ScanBooksResultDto> ScanAvailableBooks(ScanBooksParamsDto paramsDto);
}

public class BooksService : IBooksService
{
    private readonly SomnContext _db;
    private readonly BooksOptions _options;

    public BooksService(IOptions<BooksOptions> options, SomnContext db)
    {
        _db = db;
        _options = options.Value;
    }

    public async Task<ScanBooksResultDto> ScanAvailableBooks(ScanBooksParamsDto paramsDto)
    {
        var books = await SearchInDirectory(_options.Path).ToListAsync();
        var result = new ScanBooksResultDto
        {
            FoundBooks = books.Select(b =>
                new SearchResult(b.ContainingFolderPath,
                    b.BookDescription != null ? "Success" : $"Fail:{b.Exception!.Message}")).ToList(),
            Message = ""
        };
        var correctBooks = books.Where(b => b.BookDescription != null)
            .Select(b => (path: b.ContainingFolderPath, bookDescription: b.BookDescription!)).ToList();

        var repeatedCodes = correctBooks
            .GroupBy(b => b.bookDescription.GenId)
            .Where(g => g.Count() > 1)
            .ToList();

        if (repeatedCodes.Count != 0)
        {
            result.Message += "Error: there are books with the same generated id: \n" +
                              JsonSerializer.Serialize(repeatedCodes) + "/n";
            return result;
        }

        await UpdateBooksDb(paramsDto, correctBooks);


        return result;
    }

    private async IAsyncEnumerable<BookSearchModel> SearchInDirectory(string directory)
    {
        var files = Directory.GetFiles(directory);
        var projFile = files.FirstOrDefault(f =>
            string.Equals(Path.GetFileName(f), _options.ProjectFileName, StringComparison.CurrentCultureIgnoreCase));
        if (projFile != null)
        {
            await using var fs = new FileStream(projFile, FileMode.Open);
            BookDescriptionModel? bookDescription = null;
            Exception? exception = null;
            try
            {
                bookDescription = await JsonSerializer.DeserializeAsync<BookDescriptionModel>(fs,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception e)
            {
                Console.WriteLine($"somn project file {projFile} is incorrect:\n{e.Message}");
                exception = e;
            }

            if (bookDescription != null)
            {
                yield return new BookSearchModel(directory, bookDescription, null);
                yield break;
            }
            else
            {
                yield return new BookSearchModel(directory, null,
                    exception ?? new NullReferenceException("Book model was null"));
                yield break;
            }
        }

        foreach (var innerDirectory in Directory.GetDirectories(directory))
        {
            await foreach (var bookDescription in SearchInDirectory(innerDirectory))
            {
                yield return bookDescription;
            }
        }
    }

    private async Task UpdateBooksDb(ScanBooksParamsDto paramsDto,
        List<(string path, BookDescriptionModel bookDescription)> scannedBooks)
    {
        var existingBooks = await _db.Books.ToListAsync();
        foreach (var existingBook in existingBooks)
        {
            if (scannedBooks.Where(b => b.bookDescription.GenId == existingBook.BookDescription.GenId)
                .TryGetFirst(out var scannedBook))
            {
                existingBook.ContainingFolder = scannedBook.path;
                existingBook.BookDescription = scannedBook.bookDescription;
                scannedBooks.Remove(scannedBook);
            }
            else
            {
                switch (paramsDto.DoWithNotExistingBooks.Name)
                {
                    case nameof(DoWithNotExistingBooksEnum.Nothing):
                        break;
                    case nameof(DoWithNotExistingBooksEnum.Delete):
                        _db.Books.Remove(existingBook);
                        break;
                    case nameof(DoWithNotExistingBooksEnum.Invisible):
                        existingBook.IsVisibleToUsers = false;
                        break;
                }
            }
        }

        await _db.SaveChangesAsync();

        foreach (var scannedBook in scannedBooks)
        {
            var newBook = new Book()
            {
                IsVisibleToUsers = true,
                ContainingFolder = scannedBook.path,
                BookDescription = scannedBook.bookDescription,
            };
            _db.Books.Add(newBook);
        }

        await _db.SaveChangesAsync();
    }
}