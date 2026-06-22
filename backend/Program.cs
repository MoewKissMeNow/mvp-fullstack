using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Expense Tracker API",
        Version = "v1",
        Description = "API для обліку витрат"
    });
});

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Expense Tracker API v1");
    options.DocumentTitle = "Expense Tracker API";
});

app.MapGet("/", () => "Expense Tracker працює!");

// READ: отримати всі записи
app.MapGet("/expenses", async (AppDbContext db) =>
    await db.Expenses.ToListAsync()).WithTags("Expenses"); 

// READ: отримати запис за Id
app.MapGet("/expenses/{id}", async (int id, AppDbContext db) =>
    await db.Expenses.FindAsync(id) is Expense expense
        ? Results.Ok(expense)
        : Results.NotFound()).WithTags("Expenses");

// CREATE
app.MapPost("/expenses", async (Expense expense, AppDbContext db) =>
{
    db.Expenses.Add(expense);
    await db.SaveChangesAsync();

    return Results.Created($"/expenses/{expense.Id}", expense);
});


// UPDATE
app.MapPut("/expenses/{id}", async (int id, Expense input, AppDbContext db) =>
{
    var expense = await db.Expenses.FindAsync(id);

    if (expense is null)
        return Results.NotFound();

    expense.Category = input.Category;
    expense.Amount = input.Amount;
    expense.Description = input.Description;
    expense.ExpenseDate = input.ExpenseDate;

    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithTags("Expenses");

// DELETE
app.MapDelete("/expenses/{id}", async (int id, AppDbContext db) =>
{
    var expense = await db.Expenses.FindAsync(id);

    if (expense is null)
        return Results.NotFound();

    db.Expenses.Remove(expense);

    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithTags("Expenses");

app.Run();