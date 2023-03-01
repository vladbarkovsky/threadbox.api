using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ThreadboxApi.Models;

namespace ThreadboxApi.Configuration
{
	public class ThreadboxDbContext : IdentityDbContext<
		User,
		IdentityRole<Guid>,
		Guid,
		IdentityUserClaim<Guid>,
		IdentityUserRole<Guid>,
		IdentityUserLogin<Guid>,
		IdentityRoleClaim<Guid>,
		IdentityUserToken<Guid>>
	{
		public ThreadboxDbContext(DbContextOptions<ThreadboxDbContext> options)
			: base(options)
		{ }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}

		public DbSet<RegistrationKey> RegistrationKeys { get; set; }
		public DbSet<Board> Boards { get; set; } = null!;
		public DbSet<ThreadModel> Threads { get; set; } = null!;
		public DbSet<ThreadImage> ThreadImages { get; set; } = null!;
		public DbSet<Post> Posts { get; set; } = null!;
		public DbSet<PostImage> PostImages { get; set; } = null!;
	}
}