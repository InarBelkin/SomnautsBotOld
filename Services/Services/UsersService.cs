using DAL;
using Microsoft.EntityFrameworkCore;
using Services.Models;
using Telegram.Bot.Types;
using Utils.Language;

namespace Services.Services;

public interface IUsersService
{
    Task<UserModel> GetOrCreateUser(User tgUser);
    Task UpdateLang(int id, LangEnum lang);
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
                    InterfaceLang = LangEnum.DefineLanguageOrEng(tgUser.LanguageCode),
                    TelegramId = tgUser.Id,
                }).Entity;
            await _context.SaveChangesAsync();
        }

        var userModel = new UserModel()
        {
            Id = user.Id,
            UserName = tgUser.Username ?? user.UserName,
            InterfaceLang = user.InterfaceLang,
            TelegramId = tgUser.Id
        };
        return userModel;
    }

    public async Task UpdateLang(int id, LangEnum lang)
    {
        await _context.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(p => p.SetProperty(u => u.InterfaceLang, lang));
    }
}