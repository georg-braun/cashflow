using budget_backend.application;
using budget_backend.Controllers;
using budget_backend.data;
using budget_backend.endpoints;
using budget_backend.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var jwtSettings = new JwtSettings();
builder.Configuration.Bind(nameof(jwtSettings), jwtSettings);
builder.Services.AddSingleton(jwtSettings);

var domain = builder.Configuration["Auth0:Domain"];
var audience = builder.Configuration["Auth0:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
    {
        jwtOptions.Authority = domain;
        jwtOptions.Audience = audience;
        jwtOptions.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization(o =>
{
    o.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});


builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();

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