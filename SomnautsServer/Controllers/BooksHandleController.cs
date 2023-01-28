using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Services;

namespace SomnautsServer.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksHandleController : ControllerBase
{
    private readonly IBooksService _booksService;

    public BooksHandleController(IBooksService booksService)
    {
        _booksService = booksService;
    }

    [HttpPost("scan")]
    public async Task<ActionResult<ScanBooksResultDto>> Scan(ScanBooksParamsDto dto)
    {
        var result = await _booksService.ScanAvailableBooks(dto);
        return result;
    }
}