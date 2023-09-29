using ApplicationY.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Data
{
    public class Context : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public Context(DbContextOptions<Context> Options) : base(Options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<TemporaryCode> TemporaryCodes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}
