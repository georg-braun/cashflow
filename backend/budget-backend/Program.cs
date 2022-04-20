using budget_backend.application;
using budget_backend.Controllers;
using budget_backend.data;
using budget_backend.endpoints;
using budget_backend.middleware;
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

if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(audience))
    Console.WriteLine("WARNING. Domain or audience is empty.");

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

builder.Services.AddCors();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<DataContext>(optionsBuilder => optionsBuilder.UseNpgsql(
    builder.Configuration["ConnectionStrings:Database"]));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDebugAllRequestsMiddleware();

// Configure the HTTP request pipeline (middleware)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet(Routes.GetAll, AccountEndpoints.GetAll);


app.MapGet(Routes.GetAllAccounts, AccountEndpoints.GetAllAccounts);
app.MapPost(Routes.AddAccount, AccountEndpoints.AddAccount);
app.MapPost(Routes.DeleteAccount, AccountEndpoints.DeleteAccount);

app.MapPost(Routes.AddAccountEntry, AccountEndpoints.AddAccountEntry);
app.MapGet(Routes.GetAccountEntriesOfAccount, AccountEndpoints.GetAccountEntriesOfAccount);
app.MapPost(Routes.DeleteAccountEntry, AccountEndpoints.DeleteAccountEntry);

app.MapPost(Routes.AddBudgetaryItem, BudgetEndpoints.AddBudgetaryItem);
app.MapPost(Routes.DeleteBudgetryItem, AccountEndpoints.DeleteBudgetaryItem);
app.MapGet(Routes.GetAllBudgetaryItems, BudgetEndpoints.GetAllBudgetaryItems);
app.MapGet(Routes.GetBudgetChanges, BudgetEndpoints.GetBudgetChanges);
app.MapPost(Routes.SetBudgetEntry, BudgetEndpoints.AddBudgetEntry);

app.Run();

public partial class Program
{
} /* use for integration tests */