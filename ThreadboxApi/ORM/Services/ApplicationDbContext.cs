using AutoMapper.Internal;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.Json;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Entities.Interfaces;

namespace ThreadboxApi.ORM.Services
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ApplicationContext _appContext;

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<RegistrationKey> RegistrationKeys { get; set; }
        public DbSet<Entities.FileInfo> FileInfos { get; set; }
        public DbSet<DbFile> DbFiles { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Entities.Thread> Threads { get; set; }
        public DbSet<ThreadImage> ThreadImages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<Tripcode> Tripcodes { get; set; }
        public DbSet<BffSession> BffSessions { get; set; }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            IDateTimeService dateTimeService,
            ApplicationContext appContext) : base(options, operationalStoreOptions)
        {
            _dateTimeService = dateTimeService;
            _appContext = appContext;
        }

        public override int SaveChanges()
        {
            AddAuditInfo();
            CreateAuditLogs();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AddAuditInfo();
            CreateAuditLogs();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddAuditInfo();
            CreateAuditLogs();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAuditInfo();
            CreateAuditLogs();
            return base.SaveChangesAsync(cancellationToken);
        }

        public void SetOriginalRowVersion<TEntity>(TEntity entity, uint? rowVersion)
            where TEntity : class, IConsistent
        {
            if (!rowVersion.HasValue)
            {
                throw new InvalidOperationException(
                    "RowVersion is null. This should never happen. Please find the author of this joke.");
            }

            PropertyEntry<TEntity, uint> propertyEntry = Entry(entity).Property(uint (TEntity entity) => entity.RowVersion);
            propertyEntry.OriginalValue = rowVersion.Value;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            ConfigureDeleteFilters(modelBuilder);
        }

        private void AddAuditInfo()
        {
            DateTimeOffset utcNow = _dateTimeService.UtcNow;

            ChangeTracker
                .Entries<IAuditable>()
                .Where(bool (EntityEntry<IAuditable> entry) => entry.State == EntityState.Added || entry.State == EntityState.Modified)
                .AsParallel()
                .ForAll(void (EntityEntry<IAuditable> entry) =>
                {
                    IAuditable entity = entry.Entity;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = utcNow;
                        entity.CreatedById = _appContext.CurrentUserId;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        entity.UpdatedAt = utcNow;
                        entity.UpdatedById = _appContext.CurrentUserId;
                    }
                });
        }

        private void CreateAuditLogs()
        {
            EntityEntry[] entries = ChangeTracker
                .Entries()
                .Where(bool (EntityEntry entry) =>
                    entry.Entity is not AuditLog &&
                    entry.State != EntityState.Detached &&
                    entry.State != EntityState.Unchanged)
                .ToArray();

            DateTimeOffset utcNow = _dateTimeService.UtcNow;

            foreach (EntityEntry entry in entries)
            {
                var auditLog = new AuditLog()
                {
                    EntityName = entry.Entity.GetType().Name,
                    CreatedAt = utcNow,
                    UserId = _appContext.CurrentUserId
                };

                var primaryKeys = new Dictionary<string, string>();
                var oldValues = new Dictionary<string, object>();
                var newValues = new Dictionary<string, object>();
                var affectedProperties = new List<string>();

                foreach (PropertyEntry propertyEntry in entry.Properties)
                {
                    string propertyName = propertyEntry.Metadata.Name;

                    if (propertyEntry.Metadata.IsPrimaryKey())
                    {
                        primaryKeys[propertyName] = propertyEntry.CurrentValue.ToString();
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditLog.Operation = AuditLogOperation.Create;
                            newValues[propertyName] = propertyEntry.CurrentValue;
                            break;

                        case EntityState.Modified:
                            if (!propertyEntry.IsModified)
                            {
                                break;
                            }

                            auditLog.Operation = AuditLogOperation.Update;
                            oldValues[propertyName] = propertyEntry.OriginalValue;
                            newValues.Add(propertyName, propertyEntry.CurrentValue);
                            affectedProperties.Add(propertyName);
                            break;

                        case EntityState.Deleted:
                            auditLog.Operation = AuditLogOperation.Delete;
                            oldValues[propertyName] = propertyEntry.OriginalValue;
                            break;
                    }
                }

                auditLog.EntityPrimaryKeys = primaryKeys.Any() ? JsonSerializer.Serialize(primaryKeys) : null;
                auditLog.OldValues = oldValues.Any() ? JsonSerializer.Serialize(oldValues) : null;
                auditLog.NewValues = newValues.Any() ? JsonSerializer.Serialize(newValues) : null;
                auditLog.AffectedProperties = affectedProperties.Any() ? JsonSerializer.Serialize(affectedProperties) : null;

                AuditLogs.Add(auditLog);
            }
        }

        public void ConfigureDeleteFilter<TEntity>(ModelBuilder modelBuilder)
            where TEntity : class, IDeletable
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(bool (TEntity entity) => !entity.Deleted);
        }

        private void ConfigureDeleteFilters(ModelBuilder modelBuilder)
        {
            IEnumerable<Type> entityTypes = modelBuilder.Model.GetEntityTypes().Select(Type (IMutableEntityType type) => type.ClrType);
            IEnumerable<Type> deletableEntityTypes = entityTypes.Where(bool (Type type) => typeof(IDeletable).IsAssignableFrom(type));
            MethodInfo methodInfo = typeof(ApplicationDbContext).GetMethod(nameof(ConfigureDeleteFilter));

            foreach (Type entityType in deletableEntityTypes)
            {
                MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(entityType);
                genericMethodInfo.Invoke(this, new object[] { modelBuilder });
            }
        }
    }
}