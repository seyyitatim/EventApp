using EventAppMVC.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace EventAppMVC.Contexts
{
    public class MySqlDbContext : IdentityDbContext<ApplicationUser>
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
    }
}
