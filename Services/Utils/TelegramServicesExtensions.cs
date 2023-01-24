using System.Text.Json;
using DAL.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Services.Configuration;
using Services.Services;
using Services.Telegram;
using Services.Telegram.Handlers;
using Telegram.Bot;

namespace Services.Utils;

public static class TelegramServicesExtensions
{
    public static IServiceCollection AddTelegramServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelegramOptions>(configuration.GetSection("TelegramOptions"));
        services.AddLocalizations();

        services.AddDal(configuration);

        services.AddScoped<IUsersService, UsersService>();

        services.AddScoped<ITgUpdateHandler, TgUpdateHandler>()
            .AddScoped<ITgCommandsHandler, TgCommandsHandler>()
            .AddScoped<ITgButtonsHandler, TgButtonsHandler>();

        services.AddSingleton<ITelegramBotClient>(provider =>
            new TelegramBotClient(provider.GetRequiredService<IOptions<TelegramOptions>>().Value.Token));
        services.AddHostedService<TgBotHostedService>();
        return services;
    }

    private static IServiceCollection AddLocalizations(this IServiceCollection services)
    {
        using var fs = new FileStream("UiLocalizations.json", FileMode.Open);
        var localization = JsonSerializer.Deserialize<UiLocalization>(fs,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        services.AddSingleton<UiLocalization>(localization);

        return services;
    }
}