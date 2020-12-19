using AuthenticationServer.Common.Extentions;
using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public class TenantRepository : AbstractRepository, ITenantRepository
    {
        private readonly IMainSqlDataAccess _db;

        public TenantRepository(IMainSqlDataAccess _db)
        {
            this._db = _db;
        }

        public Task Delete(TenantEntity tenantEntity)
        {
            throw new System.NotImplementedException();
        }

        public Task<TenantEntity> Get(Guid Id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TenantEntity>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<JwtConfigurationDto> GetTenantJwtConfiguration(TenantEntity tenantEntity)
        {
            throw new System.NotImplementedException();
        }

        public Task Insert(TenantEntity tenantEntity)
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

            return _db.ExecuteStoredProcedures(parametersToStoredProcedure);
        }

        public Task Update(TenantEntity tenantEntity)
        {
            throw new System.NotImplementedException();
        }

    }
}
