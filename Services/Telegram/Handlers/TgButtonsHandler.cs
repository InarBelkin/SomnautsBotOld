using Services.Configuration;
using Services.Models;
using Services.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Utils.Language;

namespace Services.Telegram.Handlers;

public interface ITgButtonsHandler
{
    Task InvokeAsync(UserModel user, CallbackQuery updateCallbackQuery);
}

public class TgButtonsHandler : ITgButtonsHandler
{
    private readonly IUsersService _usersService;
    private readonly ITelegramBotClient _bot;
    private readonly UiLocalization _resources;

    public TgButtonsHandler(IUsersService usersService, ITelegramBotClient bot, UiLocalization resources)
    {
        _usersService = usersService;
        _bot = bot;
        _resources = resources;
    }

    public async Task InvokeAsync(UserModel user, CallbackQuery updateCallbackQuery)
    {
        var query = updateCallbackQuery.Data;
        if (query == null) return;
        var queryWords = query.Split('_');
        switch (queryWords)
        {
            case ["#interfacelang", { } langCode]:
                if (LangEnum.TryFromValue(langCode, out var lang))
                {
                    await _usersService.UpdateLang(user.Id, lang);
                    await _bot.AnswerCallbackQueryAsync(updateCallbackQuery.Id,
                        _resources.UiLangSelected.WithErrorString(lang));
                }
                break;
        }
    }
}