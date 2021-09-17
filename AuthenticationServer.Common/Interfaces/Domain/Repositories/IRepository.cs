using AuthenticationServer.Common.Interfaces.Domain.DataAccess;

namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface IRepository
    {
        IMainSqlDataAccess Database { get; }
        IAdminAccountRepository AdminAccount { get; }
        ITenantAccountRepository TenantAccount { get; }
        IApplicationRepository Application { get; }
        IDomainNameRepository DomainName { get; }
        IJwtTenantConfigRepository JwtTenantConfig { get; }
    }
}