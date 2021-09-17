using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;

namespace Authentication.Persistance.Repositories
{
    public class Repository : IRepository
    {
        public IMainSqlDataAccess Database { get; }
        public IAdminAccountRepository AdminAccount { get; }
        public ITenantAccountRepository TenantAccount { get; }
        public IApplicationRepository Application { get; }
        public IDomainNameRepository DomainName { get; }
        public IJwtTenantConfigRepository JwtTenantConfig { get; }


        public Repository(IMainSqlDataAccess database, IAdminAccountRepository adminAccountRepo, ITenantAccountRepository tenantAccountRepoy,
            IApplicationRepository applicationRepo, IDomainNameRepository domainNameRepo, IJwtTenantConfigRepository jwtTenantConfigRepo)
        {
            AdminAccount = adminAccountRepo;
            TenantAccount = tenantAccountRepoy;
            Application = applicationRepo;
            DomainName = domainNameRepo;
            JwtTenantConfig = jwtTenantConfigRepo;
            Database = database;
        }



    }
}
