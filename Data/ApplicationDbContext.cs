using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mundial.Models;
using System.Text.RegularExpressions;

namespace Mundial.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUsers>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Grupa> Groups { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<GameCountry> GameCountry { get; set; }
        public DbSet<AppUsers> AppUsers { get; set; }
        public DbSet<Mundial.Models.Bettings> Bettings { get; set; }
    }
}