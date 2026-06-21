using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>().HasData(
            new Expense
            {
                Id = 1,
                Category = "Продукти",
                Amount = 850,
                Description = "Покупки в супермаркеті",
                ExpenseDate = new DateTime(2026, 6, 1)
            },
            new Expense
            {
                Id = 2,
                Category = "Транспорт",
                Amount = 200,
                Description = "Проїзд",
                ExpenseDate = new DateTime(2026, 6, 2)
            }
        );
    }
}