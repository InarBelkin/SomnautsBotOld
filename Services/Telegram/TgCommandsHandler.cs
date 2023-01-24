using Services.Configuration;
using Services.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Services.Telegram;

public interface ITgCommandsHandler
{
    Task InvokeAsync(UserModel user, string command);
}

public class TgCommandsHandler : ITgCommandsHandler
{
    private readonly UiLocalization _localization;
    private readonly ITelegramBotClient _telegramBotClient;
    
    public TgCommandsHandler(UiLocalization localization, ITelegramBotClient telegramBotClient)
    {
        _localization = localization;
        _telegramBotClient = telegramBotClient;
    }

    public async Task InvokeAsync(UserModel user, string command)
    {
        switch (command[1..])
        {
            case "start" or "help":
                await _telegramBotClient.SendTextMessageAsync(user.TelegramId,
                    _localization.Help[user.InterfaceLang]);
                break;
        }
    }
}