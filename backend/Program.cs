using Microsoft.EntityFrameworkCore;
using backend.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Expense Tracker працює!");

app.MapGet("/expenses", async (AppDbContext db) =>
    await db.Expenses.ToListAsync());

app.Run();