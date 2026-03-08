using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TransmetroConecta.Auth.Application.Interfaces;
using TransmetroConecta.Auth.Application.Services;
using TransmetroConecta.Auth.Domain.Interfaces;
using TransmetroConecta.Auth.Infrastructure.Data;
using TransmetroConecta.Auth.Infrastructure.Repositories;
using TransmetroConecta.Auth.Infrastructure.Security;
using TransmetroConecta.Auth.API.Extensions;
using TransmetroConecta.Auth.API.Middlewares;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using TransmetroConecta.Auth.API.Filters;
using TransmetroConecta.Auth.Application.Validators;
using TransmetroConecta.Auth.Infrastructure.Integration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilterAttribute>();
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestDtoValidator>();

var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret es nulo");
var key = Encoding.UTF8.GetBytes(jwtSecret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpClient<IWalletIntegrationService, WalletIntegrationService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["WalletService:BaseUrl"] ?? "http://localhost:3002");
});

builder.Services.AddHttpClient<IWalletIntegrationService, WalletIntegrationService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["WalletService:BaseUrl"] ?? "http://localhost:3002");
    client.DefaultRequestHeaders.Add("x-internal-secret", "SuperSecretS2S_Transmetro2026");
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.ApplyPendingMigrationsAsync();

app.Run();