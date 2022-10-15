using Microsoft.EntityFrameworkCore;
using ThreadboxAPI.Models;
using System.Reflection;

namespace ThreadboxApi.Configuration
{
    public class ThreadboxDbContext : DbContext
    {
        public ThreadboxDbContext(DbContextOptions<ThreadboxDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Section> Sections { get; set; } = null!;
    }
}
