using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using ThreadboxApi.Domain.Entities;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Infrastructure.Persistence
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<RegistrationKey> RegistrationKeys { get; set; }
        public DbSet<Domain.Entities.FileInfo> FileInfos { get; set; }
        public DbSet<DbFile> DbFiles { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Domain.Entities.Thread> Threads { get; set; }
        public DbSet<ThreadImage> ThreadImages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}