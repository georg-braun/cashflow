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

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtOptions =>
{
    // how the token looks like
    jwtOptions.SaveToken = true;
    jwtOptions.TokenValidationParameters = new TokenValidationParameters()
    {
        // validate the hash with the secret
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = false,
        ValidateLifetime = true
    };
});


builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddDbContext<DataContext>(optionsBuilder => optionsBuilder.UseNpgsql(
    builder.Configuration["ConnectionStrings:Database"]));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo(){Title = "Budget API", Version = "v1"});

    var securityScheme = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };
    
    x.AddSecurityDefinition("Bearer", securityScheme);

    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, Array.Empty<string>()}
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline (middleware)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

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