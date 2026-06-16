using ApiPulseHQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ApiPulseHQ.Infrastructure.Persistence
{
    public class ApiPulseDbContext:DbContext
    {
        public ApiPulseDbContext(DbContextOptions<ApiPulseDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<ServiceEndpoint> ServiceEndpoints => Set<ServiceEndpoint>();
        public DbSet<ServiceCheckLog> ServiceCheckLogs => Set<ServiceCheckLog>();
        public DbSet<AlertRule> AlertRules => Set<AlertRule>();
        public DbSet<StatusPage> StatusPages => Set<StatusPage>();
        public DbSet<StatusPageService> StatusPageServices => Set<StatusPageService>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .HasIndex(u => u.Email)
               .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiPulseDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
