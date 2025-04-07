using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PulsePoint.Models;

namespace PulsePoint.Data
{
    // Databaskontext som hanterar Identity + applikationens modeller
    public class PulsePointDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public PulsePointDbContext(DbContextOptions<PulsePointDbContext> options) : base(options) { }

        // Tabell för arbetsplatser
        public DbSet<Workplace> Workplaces { get; set; }

        // Tabell för användarnas hälsoregistreringar
        public DbSet<HealthEntry> HealthEntries { get; set; }
    }
}