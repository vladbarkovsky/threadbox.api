﻿using Microsoft.EntityFrameworkCore;
using ThreadboxAPI.Models;
using System.Reflection;

namespace ThreadboxAPI
{
    public class ThreadboxContext : DbContext
    {
        public ThreadboxContext(DbContextOptions<ThreadboxContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Section> Sections { get; set; } = null!;
    }
}
