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
		{
			//var databaseExists = Database.GetAppliedMigrations().Any();

			//if (!databaseExists)
			//{
			//	Database.Migrate();
			//}

			//Database.EnsureCreated();

			//var apliedMigrationsd = Database.GetAppliedMigrations();

			/// Note that this API does **not * *use migrations to create the database.In addition, the database that is
			///         created cannot be later updated using migrations. If you are targeting a relational database and using migrations,
			///         then you can use <see cref="M:Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.Migrate" />
			///         to ensure the database is created using migrations and that all migrations have been applied.
			// We create database according to applied migrations but with no migrations applied

			// This method does not use migrations to create the database
		}

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