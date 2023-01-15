using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Services.Telegram;

public sealed class TgBotHostedService : IHostedService, IDisposable
{
    private readonly ITelegramBotClient bot;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TgBotHostedService> _logger;
    private readonly CancellationTokenSource _serviceCancellationTokenSource;


    public TgBotHostedService(ITelegramBotClient bot, IServiceScopeFactory scopeFactory,
        ILogger<TgBotHostedService> logger)
    {
        this.bot = bot;
        _scopeFactory = scopeFactory;
        _logger = logger;
        _serviceCancellationTokenSource = new();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        bot.StartReceiving(UpdateHandlerAsync, ErrorHandlerAsync,
            cancellationToken: _serviceCancellationTokenSource.Token);
        return Task.CompletedTask;
    }

    private async Task UpdateHandlerAsync(ITelegramBotClient _, Update update, CancellationToken token)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var updateHandler = scope.ServiceProvider.GetRequiredService<ITgUpdateHandler>();
        try
        {
            await updateHandler.InvokeAsync(update, token);
        }
        catch (Exception e) when (e is OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error when handle telegram update");
        }
    }

    private Task ErrorHandlerAsync(ITelegramBotClient client, Exception exception, CancellationToken arg3)
    {
        _logger.LogError(exception, "Telegram service is stopped with error");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _serviceCancellationTokenSource.Cancel();
        _logger.LogInformation("Telegram service is stopped");
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _serviceCancellationTokenSource.Dispose();
    }
}