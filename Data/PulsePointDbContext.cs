using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PulsePoint.Models;

namespace PulsePoint.Data
{
    /// <summary>
    /// Databaskontext för PulsePoint-applikationen.
    /// Ärver från IdentityDbContext för att hantera användare, roller och autentisering via Identity.
    /// Innehåller även DbSets för applikationens egna modeller: Workplaces och HealthEntries.
    /// </summary>
    public class PulsePointDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public PulsePointDbContext(DbContextOptions<PulsePointDbContext> options) : base(options) { }

        /// <summary>
        /// Representerar tabellen för arbetsplatser.
        /// </summary>
        public DbSet<Workplace> Workplaces { get; set; }

        /// <summary>
        /// Representerar tabellen för användarnas hälsoregistreringar.
        /// </summary>
        public DbSet<HealthEntry> HealthEntries { get; set; }
    }
}
