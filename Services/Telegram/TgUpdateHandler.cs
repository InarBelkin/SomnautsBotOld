using Services.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Services.Telegram;

public interface ITgUpdateHandler
{
    Task InvokeAsync(Update update, CancellationToken token);
}

public class TgUpdateHandler : ITgUpdateHandler
{
    private readonly ITgCommandsHandler _commandsHandler;
    private readonly IUsersService _usersService;

    public TgUpdateHandler(ITgCommandsHandler commandsHandler, IUsersService usersService)
    {
        _commandsHandler = commandsHandler;
        _usersService = usersService;
    }

    public async Task InvokeAsync(Update update, CancellationToken token)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                await HandleMessage(update.Message!);
                break;
            case UpdateType.CallbackQuery:
                await HandleButton(update.CallbackQuery!);
                break;
        }
    }

    private async Task HandleMessage(Message msg)
    {
        if (msg.From is null || msg.Text is null) return;
        var user = await _usersService.GetOrCreateUser(msg.From);
        if (msg.Text.StartsWith("/") && msg.Text.Length > 1)
        {
            await _commandsHandler.InvokeAsync(user, msg.Text);
        }
    }

    private async Task HandleButton(CallbackQuery updateCallbackQuery)
    {
        throw new NotImplementedException();
    }
}