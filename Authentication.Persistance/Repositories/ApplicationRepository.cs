using AuthenticationServer.Common.Constants.StoredProcedures;
using AuthenticationServer.Common.Extentions;
using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Domain.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly IMainSqlDataAccess _db;

        public ApplicationRepository(IMainSqlDataAccess db)
        {
            _db = db;
        }

        public async Task AddTenantToAdmin(Guid tenantId, string adminId)
        {
            var parametersToStoredProcedure = new Dictionary<DynamicParameters, string>();

            var tenantLanguageParameters = new DynamicParameters();
            tenantLanguageParameters.Add("TenantId", tenantId);
            tenantLanguageParameters.Add("AdmintId", adminId);
            parametersToStoredProcedure.Add(tenantLanguageParameters, SPName.AddTenant);

            await _db.ExecuteStoredProcedures(parametersToStoredProcedure);
        }

        public Task Delete(ApplicationEntity domain)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationEntity> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ApplicationEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationEntity> GetDomainFromName(string name)
        {
            string sql = $"SELECT * FROM dbo.Domains where name LIKE @Name";
            var parameters = new { Name = $"{name}%" };

            return await _db.GetData<ApplicationEntity, dynamic>(sql, parameters);
        }

        public async Task<ApplicationEntity> GetDomainFromUrl(string url)
        {
            string sql = $"SELECT * FROM dbo.Domains where url LIKE @Url";
            var parameters = new { Url = $"{url}%" };

            return await _db.GetData<ApplicationEntity, dynamic>(sql, parameters);
        }

        public async Task<List<ApplicationEntity>> GetDomainsFromAdminId(string adminId)
        {
            string sql = $"SELECT * FROM dbo.Domains where AdminId = @AdminId";
            var parameters = new { AdminId = adminId };

            return await _db.GetData< List<ApplicationEntity>, dynamic>(sql, parameters);
        }

        public async Task Insert(ApplicationEntity domain)
        {
            var parametersToStoredProcedure = new Dictionary<DynamicParameters, string>();

            var tenantLanguageParameters = new DynamicParameters();
            tenantLanguageParameters.AddParametersFromProperties(domain);
            parametersToStoredProcedure.Add(tenantLanguageParameters, SPName.InsertApplication);

            await _db.ExecuteStoredProcedures(parametersToStoredProcedure);
        }

        public Task Update(ApplicationEntity domain)
        {
            throw new NotImplementedException();
        }
    }
}
