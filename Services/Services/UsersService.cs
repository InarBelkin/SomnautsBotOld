using DAL;
using Microsoft.EntityFrameworkCore;
using Services.Models;
using Telegram.Bot.Types;

namespace Services.Services;

public interface IUsersService
{
    Task<UserModel> GetOrCreateUser(User tgUser);
}

public class UsersService : IUsersService
{
    private readonly SomnContext _context;

    public UsersService(SomnContext context)
    {
        _context = context;
    }

    public async Task<UserModel> GetOrCreateUser(User tgUser)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.TelegramId == tgUser.Id);
        if (user == null)
        {
            user = _context.Users.Add(
                new DAL.Entities.User()
                {
                    UserName = tgUser.Username ?? tgUser.FirstName,
                    InterfaceLang = tgUser.LanguageCode,
                    TelegramId = tgUser.Id,
                }).Entity;
            await _context.SaveChangesAsync();
        }

        var userModel = new UserModel()
        {
            UserName = tgUser.Username ?? user.UserName,
            InterfaceLang = user.InterfaceLang ?? tgUser.LanguageCode ?? "en",
            TelegramId = tgUser.Id
        };
        return userModel;
    }
}