using Services.Configuration;
using Services.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Utils.Language;

namespace Services.Telegram;

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
        switch (command[1..])
        {
            case "start" or "help":
                await _telegramBotClient.SendTextMessageAsync(user.TelegramId,
                    _uiResources.Help.WithErrorString(user.InterfaceLang));
                break;
        }
    }
}