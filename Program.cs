using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PulsePoint.Models;
using PulsePoint.Data;

var builder = WebApplication.CreateBuilder(args);

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

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
