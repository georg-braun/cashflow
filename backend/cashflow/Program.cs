using budget_backend.application;
using budget_backend.Controllers;
using budget_backend.data;
using budget_backend.middleware;
using budget_backend.Options;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(typeof(Program));

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
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddDbContext<CashflowDataContext>(optionsBuilder => optionsBuilder.UseNpgsql(
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

app.MapGet(Routes.GetAll, MoneyMovementEndpoints.GetAll);

app.MapGet(Routes.GetCategories, CategoryEndpoints.GetCategories);
app.MapPost(Routes.AddCategory, CategoryEndpoints.AddCategory);
app.MapPost(Routes.DeleteCategory, CategoryEndpoints.DeleteCategory);

app.MapGet(Routes.GetMoneyMovements, MoneyMovementEndpoints.GetMoneyMovements);
app.MapPost(Routes.AddMoneyMovement, MoneyMovementEndpoints.AddMoneyMovement);
app.MapPost(Routes.DeleteMoneyMovement, MoneyMovementEndpoints.DeleteMoneyMovement);

app.MapGet(Routes.GetTemplates, TemplateEndpoints.GetTemplates);
app.MapPost(Routes.AddTemplate, TemplateEndpoints.AddTemplate);

app.Run();

public partial class Program
{
} /* use for integration tests */