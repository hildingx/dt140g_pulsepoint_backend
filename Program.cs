using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PulsePoint.Data;
using PulsePoint.Models;
using PulsePoint.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

LoadEnvFile();

// Registrera controllers
builder.Services.AddControllers();

// Konfigurera databaskoppling mot MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PulsePointDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Registrera Identity med anpassad User-modell och roller (int som nyckeltyp)
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<PulsePointDbContext>()
    .AddDefaultTokenProviders();

// Registrera JWT-tjänst för token-generering 
builder.Services.AddScoped<JwtService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    var keyString = builder.Configuration["Jwt:Key"]
        ?? throw new InvalidOperationException("Jwt:Key saknas i appsettings.json");

    options.RequireHttpsMetadata = false; // Tillåt HTTP i utveckling

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

builder.Configuration
    .SetBasePath(app.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>() // Läs user secrets automatiskt
    .AddEnvironmentVariables();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Mappa controllers till endpoints (API-rutter)
app.MapControllers();

// Skapa roller i databasen om de inte redan finns
await SeedRoles(app);

app.Run();

static async Task SeedRoles(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

    string[] roles = { "admin", "user", "manager" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<int>(role));
        }
    }
}

void LoadEnvFile(string path = ".env")
{
    if (!File.Exists(path)) return;

    foreach (var line in File.ReadAllLines(path))
    {
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;

        var parts = line.Split('=', 2);
        if (parts.Length != 2) continue;

        var key = parts[0].Trim();
        var value = parts[1].Trim().Trim('"');

        Environment.SetEnvironmentVariable(key, value);
    }
}
