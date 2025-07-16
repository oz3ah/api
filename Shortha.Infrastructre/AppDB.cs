using Microsoft.EntityFrameworkCore;
using Shortha.Domain.Entites;
using Shortha.Infrastructre.Configuration;

namespace Shortha.Infrastructre
{
    public class AppDb : DbContext
    {
        public DbSet<AppUser> Users { get; set; } = null!;
        public DbSet<Url> Urls { get; set; } = null!;
        public DbSet<Visit> Visits { get; set; } = null!;

        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Subscription> Subscriptions { get; set; } = null!;

        public DbSet<Package> Packages { get; set; } = null!;

        public AppDb(DbContextOptions<AppDb> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UrlConfiguration());
            modelBuilder.ApplyConfiguration(new VisitConfiguration());
            modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new PackageConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}