using AuthenticationServer.Common.Extentions;
using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Domain.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public class JwtTenantConfigRepository : IJwtTenantConfigRepository
    {
        private readonly IMainSqlDataAccess _db;

        public JwtTenantConfigRepository(IMainSqlDataAccess db)
        {
            _db = db;
        }

        public Task CreateJwtConfiguration(JwtTenantConfigEntity jwtConfigurationEntity)
        {
            throw new NotImplementedException();
        }

        public async Task<JwtTenantConfigEntity> GetFromApplicationId(Guid applicationId)
        {
            string sql = $"SELECT * FROM JwtTenantConfig WHERE applicationId = {applicationId}";

            return await _db.GetData<JwtTenantConfigEntity, dynamic>(sql, new { });
        }


        public async Task<Guid> Insert(JwtTenantConfigEntity JwtTenantConfigEntity, string data = "")
        {
            string sql = $@"INSERT INTO {typeof(JwtTenantConfigEntity).GetTableName()}
                            VALUES (@JwtTenantConfigId, @SecretKey, @Claims, @ExpireMinutes, @RefreshExpireMinutes, @Algorithm, @ApplicationId, @Created, @LastModified);";

            var parameters = new DynamicParameters();
            parameters.AddColumnParameters(JwtTenantConfigEntity, null);

            await _db.SaveData(sql, parameters);

            return JwtTenantConfigEntity.Id;
        }

        public async Task<JwtTenantConfigEntity> Get(Guid? adminId, Guid id)
        {
            string sql = $"SELECT * FROM JwtTenantConfig WHERE id = @Id";

            return await _db.GetData<JwtTenantConfigEntity, object>(sql, new { Id = id });
        }

        public Task<List<JwtTenantConfigEntity>> GetAll(Guid adminId)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Guid adminId, Guid id, JwtTenantConfigEntity jwtTenantConfigEntity)
        {
            string sql = $@"UPDATE {typeof(JwtTenantConfigEntity).GetTableName()}
                            SET 
                                {nameof(jwtTenantConfigEntity.SecretKey)} = @SecretKey,
                                {nameof(jwtTenantConfigEntity.Claims)} = @Claims,
                                {nameof(jwtTenantConfigEntity.ExpireMinutes)} = @ExpireMinutes
                                {nameof(jwtTenantConfigEntity.Algorithm)} = @Algorithm
                                {nameof(jwtTenantConfigEntity.LastModified)} = @LastModified
                            WHERE {nameof(jwtTenantConfigEntity.Id)} = @Id;";

            var parameters = new DynamicParameters();
            parameters.AddColumnParameters(jwtTenantConfigEntity, nameof(jwtTenantConfigEntity.Created));

            await _db.SaveData(sql, parameters);
        }

        public Task Delete(Guid adminId, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
