using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class SomnContext : DbContext
{
    public SomnContext(DbContextOptions<SomnContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<BookSave> BookSaves => Set<BookSave>();
}