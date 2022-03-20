using System.Text;
using budget_backend.application;
using budget_backend.Controllers;
using budget_backend.data;
using budget_backend.endpoints;
using budget_backend.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var jwtSettings = new JwtSettings();
builder.Configuration.Bind(nameof(jwtSettings), jwtSettings);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
{
    jwtOptions.Authority = builder.Configuration["Auth0:Domain"];
    jwtOptions.Audience = builder.Configuration["Auth0:Audience"];
});

const string readWritePolicy = "budget:read-write";
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy(readWritePolicy, p =>
    {
        p.RequireAuthenticatedUser().RequireClaim("scope", "budget:read-write");
    });
});


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
app.UseAuthentication();
app.UseAuthorization();

app.MapGet(Routes.GetAll, AccountEndpoints.GetAll).RequireAuthorization(readWritePolicy);
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