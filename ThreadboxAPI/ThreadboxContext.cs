using Microsoft.EntityFrameworkCore;
using ThreadboxAPI.Models;

namespace ThreadboxAPI
{
    public class ThreadboxContext : DbContext
    {
        public ThreadboxContext(DbContextOptions<ThreadboxContext> options)
            : base(options)
        { }

        public DbSet<Section> Sections { get; set; } = null!;
    }
}
