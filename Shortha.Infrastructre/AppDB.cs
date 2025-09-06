using Microsoft.EntityFrameworkCore;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces;
using Shortha.Infrastructre.Audit;

namespace Shortha.Infrastructre
{
    public class AppDb(DbContextOptions<AppDb> options, ICurrentSessionProvider currentSessionProvider)
        : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; } = null!;
        public DbSet<Url> Urls { get; set; } = null!;
        public DbSet<Visit> Visits { get; set; } = null!;

        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Subscription> Subscriptions { get; set; } = null!;

        public DbSet<Package> Packages { get; set; } = null!;
        public DbSet<AuditTrail> AuditTrails { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var userId = currentSessionProvider.GetUserId();

            SetAuditableProperties(userId);

            var auditEntries = HandleAuditingBeforeSaveChanges(userId).ToList();
            if (auditEntries.Count > 0)
            {
                await AuditTrails.AddRangeAsync(auditEntries, cancellationToken);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private List<AuditTrail> HandleAuditingBeforeSaveChanges(string? userId)
        {
            var auditableEntries = ChangeTracker.Entries<IBase>()
                .Where(x => x.State is EntityState.Added or EntityState.Deleted
                    or EntityState.Modified)
                .Select(x => AuditConfig.CreateTrailEntry(userId, x))
                .ToList();

            return auditableEntries;
        }


        /// <summary>
        /// Sets auditable properties for entities that are inherited from <see cref="IAuditableEntity"/>
        /// </summary>
        /// <param name="userId">User identifier that performed an action</param>
        /// <returns>Collection of auditable entities</returns>
        private void SetAuditableProperties(string? userId)
        {
            const string systemSource = "system";
            foreach (var entry in ChangeTracker.Entries<IBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy = userId?.ToString() ?? systemSource;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = userId?.ToString() ?? systemSource;
                        break;
                }
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDb).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}