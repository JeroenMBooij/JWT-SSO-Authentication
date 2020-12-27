using AuthenticationServer.Common.Extentions;
using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public class TenantRepository : AbstractRepository, ITenantRepository
    {
        private readonly IMainSqlDataAccess _db;
        private readonly IConfiguration _config;

        public TenantRepository(IMainSqlDataAccess _db, IConfiguration config)
        {
            this._db = _db;
            _config = config;
        }

        public Task Delete(TenantEntity tenantEntity)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TenantEntity> Get(Guid id)
        {
            string sql = $"SELECT * FROM dbo.Tenants where Id = @Id";
            var parameters = new { Id = id.ToString() };

            return await _db.GetData<TenantEntity, dynamic>(sql, parameters);
        }

        public Task<List<TenantEntity>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<JwtConfigurationDto> GetTenantJwtConfiguration(TenantEntity tenantEntity)
        {
            throw new System.NotImplementedException();
        }

        public async Task Insert(TenantEntity tenantEntity)
        {
            var parametersToStoredProcedure = new Dictionary<object, string>();

            Dictionary<string, object> tenantParameters = GetEntityParamaters(tenantEntity);
            tenantParameters.AddRangeParameters(GetEntityParamaters(tenantEntity.DashboardModel, tenantParameters));
            tenantParameters.AddRangeParameters(GetEntityParamaters(tenantEntity.UserSchema, tenantParameters));
            tenantParameters.AddRangeParameters(GetEntityParamaters(tenantEntity.UsersJwtConfiguration, tenantParameters));

            parametersToStoredProcedure.Add(tenantParameters, "sp_insert_tenant_with_1:1_attributes");

            foreach (DomainEntity domainEntity in tenantEntity.Domains)
            {
                Dictionary<string, object> tenantDomainParameters = GetEntityParamaters(domainEntity);
                parametersToStoredProcedure.Add(tenantDomainParameters, "sp_insert_tenant_domain");
            }

            foreach(LanguageEntity languageEntity in tenantEntity.Languages)
            {
                var tenantLanguageParameters = new DynamicParameters();
                tenantLanguageParameters.Add("LanguageId", languageEntity.Id);
                tenantLanguageParameters.Add("TenantId", tenantEntity.Id);

                parametersToStoredProcedure.Add(tenantLanguageParameters, "sp_insert_tenant_language");
            }

            await _db.ExecuteStoredProcedures(parametersToStoredProcedure);
        }

        public async Task SetVerified(Guid id)
        {
            string sql = $"UPDATE dbo.Tenants SET EmailVerified = '1' where Id = @Id";
            var parameters = new { Id = id.ToString() };

            await _db.GetData<TenantEntity, dynamic>(sql, parameters);
        }

        public Task Update(TenantEntity tenantEntity)
        {
            throw new System.NotImplementedException();
        }

    }
}
