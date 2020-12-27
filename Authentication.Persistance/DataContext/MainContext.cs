using AuthenticationServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationServer.Domain.DataAccess.DataContext
{
    public class MainContext : DbContext
    {
        public MainContext() { }
        public MainContext(DbContextOptions<MainContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public virtual DbSet<DashboardEntity> Dashboards { get; set; }
        public virtual DbSet<DomainEntity> Domains { get; set; }
        public virtual DbSet<JwtConfigurationEntity> JwtConfigurations { get; set; }
        public virtual DbSet<LanguageEntity> Languages { get; set; }
        public virtual DbSet<RoleEntity> Roles { get; set; }
        public virtual DbSet<TenantEntity> Tenants { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<UserModelEntity> UserModels { get; set; }
        public virtual DbSet<UserSchemaEntity> UserSchemas { get; set; }
    }
}
