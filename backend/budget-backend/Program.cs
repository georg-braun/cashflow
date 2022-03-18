using budget_backend.application;
using budget_backend.Controllers;
using budget_backend.Controllers.apiDto;
using budget_backend.data;
using budget_backend.endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddDbContext<DataContext>(optionsBuilder => optionsBuilder.UseNpgsql(
    builder.Configuration["ConnectionStrings:Database"]));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline (middleware)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseAuthorization();

app.MapGet(Routes.GetAll, AccountEndpoints.GetAll);
app.MapGet(Routes.GetAllAccounts, AccountEndpoints.GetAllAccounts);
app.MapGet(Routes.GetAccountEntriesOfAccount, AccountEndpoints.GetAccountEntriesOfAccount);
app.MapGet(Routes.GetSpendings, AccountEndpoints.GetSpendings);
app.MapPost(Routes.AddIncome, AccountEndpoints.AddIncome);
app.MapPost(Routes.AddSpending, AccountEndpoints.AddSpending);
app.MapPost(Routes.AddAccount, AccountEndpoints.AddAccount);

app.MapPost(Routes.AddBudgetaryItem, BudgetEndpoints.AddBudgetaryItem);
app.MapGet(Routes.GetAllBudgetaryItems, BudgetEndpoints.GetAllBudgetaryItems);
app.MapGet(Routes.GetBudgetChanges, BudgetEndpoints.GetBudgetChanges);
app.MapPost(Routes.SetBudgetEntry, BudgetEndpoints.AddBudgetEntry);

app.Run();

public partial class Program
{
} /* use for integration tests */