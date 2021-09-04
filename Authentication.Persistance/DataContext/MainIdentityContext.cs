using AuthenticationServer.Domain.Common;
using AuthenticationServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Authentication.Persistance.DataContext
{
    public class MainIdentityContext : IdentityDbContext<ApplicationUserEntity, RoleEntity, Guid>
    {
        public MainIdentityContext(DbContextOptions<MainIdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUserEntity>()
                        .ToTable("ApplicationUsers");

            modelBuilder.Entity<ApplicationUserEntity>()
                        .HasOne(x => x.Admin)
                        .WithMany(x => x.Tenants)
                        .HasForeignKey(x => x.AdminId)
                        .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ApplicationUserEntity>()
                        .HasMany(x => x.Assets)
                        .WithOne(x => x.Admin)
                        .HasForeignKey(x => x.AdminId);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public virtual DbSet<DashboardEntity> Dashboards { get; set; }
        public virtual DbSet<ApplicationEntity> Domains { get; set; }
        public virtual DbSet<JwtConfigurationEntity> JwtConfigurations { get; set; }
        public virtual DbSet<LanguageEntity> Languages { get; set; }
        public virtual DbSet<ApplicationUserEntity> Tenants { get; set; }


    }
}
