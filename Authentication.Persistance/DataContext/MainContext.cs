using AuthenticationServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationServer.Domain.DataAccess.DataContext
{
    public class MainContext : DbContext
    {
        public MainContext() { }
        public MainContext(DbContextOptions<MainContext> options) : base(options) { }

        public virtual DbSet<DomainEntity> Domains { get; set; }
        public virtual DbSet<JwtConfigurationEntity> JwtConfigurations { get; set; }
        public virtual DbSet<TenantEntity> Tenants { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }
    }
}
