using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ThreadboxApi.Models;
using Thread = ThreadboxApi.Models.Thread;

namespace ThreadboxApi.Configuration
{
    public class ThreadboxDbContext : DbContext
    {
        public ThreadboxDbContext(DbContextOptions<ThreadboxDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Board> Boards { get; set; } = null!;
        public DbSet<Thread> Threads { get; set; } = null!;
        public DbSet<ThreadImage> ThreadImages { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<PostImage> PostImages { get; set; } = null!;
    }
}