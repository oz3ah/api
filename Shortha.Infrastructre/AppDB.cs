using Microsoft.EntityFrameworkCore;
using Shortha.Domain.Entites;
using Shortha.Infrastructre.Configuration;
using Shortha.Infrastructre.Interceptors;

namespace Shortha.Infrastructre
{
    public class AppDb(DbContextOptions<AppDb> options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; } = null!;
        public DbSet<Url> Urls { get; set; } = null!;
        public DbSet<Visit> Visits { get; set; } = null!;

        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Subscription> Subscriptions { get; set; } = null!;

        public DbSet<Package> Packages { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDb).Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}