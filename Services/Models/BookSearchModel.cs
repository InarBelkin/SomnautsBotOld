using BookServices.Models;

namespace Services.Models;

public record BookSearchModel(string ContainingFolderPath, BookDescriptionModel? BookDescription, Exception? Exception);