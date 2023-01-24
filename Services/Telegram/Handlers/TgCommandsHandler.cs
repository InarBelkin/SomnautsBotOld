using Services.Configuration;
using Services.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Utils.Language;

namespace Services.Telegram.Handlers;

public interface ITgCommandsHandler
{
    Task InvokeAsync(UserModel user, string command);
}

public class TgCommandsHandler : ITgCommandsHandler
{
    private readonly UiLocalization _uiResources;
    private readonly ITelegramBotClient _telegramBotClient;

    public TgCommandsHandler(UiLocalization uiResources, ITelegramBotClient telegramBotClient)
    {
        _uiResources = uiResources;
        _telegramBotClient = telegramBotClient;
    }

    public async Task InvokeAsync(UserModel user, string command)
    {
        switch (command[1..].ToLower())
        {
            case "start" or "help":
                await _telegramBotClient.SendTextMessageAsync(user.TelegramId,
                    _uiResources.Help.WithErrorString(user.InterfaceLang), entities: new MessageEntity[] { });
                break;
            case "interfacelang":
                await _telegramBotClient.SendTextMessageAsync(user.TelegramId,
                    _uiResources.SelectUiLang.WithErrorString(user.InterfaceLang),
                    replyMarkup: new InlineKeyboardMarkup(LangEnum.List.Select(l =>
                            InlineKeyboardButton.WithCallbackData(l.LangName, $"#interfacelang_{l.Value}"))
                        .Select(b => new[] { b }))
                );
                break;
            case "books":
                
                break;
        }
    }
}