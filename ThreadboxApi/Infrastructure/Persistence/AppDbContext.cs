﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ThreadboxApi.Domain.Entities;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<
        AppUser,
        IdentityRole<Guid>,
        Guid,
        IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<RegistrationKey> RegistrationKeys { get; set; }
        public DbSet<Domain.Entities.FileInfo> FileInfos { get; set; }
        public DbSet<DbFile> DbFiles { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Domain.Entities.Thread> Threads { get; set; }
        public DbSet<ThreadImage> ThreadImages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
    }
}