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
        public DbSet<Message> Messages { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<LikedPost> LikedPosts { get; set; }
        public DbSet<Like> Likes { get; set; }  
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<VerificationQueue> VerificationQueues { get; set; }
        public DbSet<Update> Updates { get; set; }
        public DbSet<Subscribtion> Subscribtions { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Audio> Audios { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Purge> Purges { get; set; }
        public DbSet<DisabledAccount> DisabledAccounts { get; set; }
        public DbSet<DisabledProject> DisabledProjects { get; set; }
        public DbSet<Mention> Mentions { get; set; }
    }
}
