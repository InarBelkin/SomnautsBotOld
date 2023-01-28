﻿using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;

namespace DAL;

public class SomnContext : DbContext
{
    public SomnContext(DbContextOptions<SomnContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<BookSave> BookSaves => Set<BookSave>();
    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureSmartEnum();
        modelBuilder.Entity<User>()
            .HasOne(u => u.CurrentSave)
            .WithOne()
            .HasForeignKey<User>(u => u.CurrentSaveId)
            .OnDelete(deleteBehavior: DeleteBehavior.SetNull);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Saves)
            .WithOne(s => s.User);

        modelBuilder.Entity<BookSave>().HasOne(bs => bs.Book)
            .WithMany()
            .OnDelete(deleteBehavior: DeleteBehavior.SetNull);
    }
}