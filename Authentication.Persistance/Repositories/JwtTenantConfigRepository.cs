using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Domain.Entities;
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

            /*JwtTenantConfigEntity jwtConfigurationEntity = _config.GetSection("TenantJwtConfiguration")
                                                                .Get<JwtTenantConfigEntity>();

            return jwtConfigurationEntity;*/
        }


        public Task Insert(JwtTenantConfigEntity entity, string data = "")
        {

            string sql = @"insert into dbo.Users (Name, Email)
                            values (@Name, @Email);";

            return _db.SaveData<JwtTenantConfigEntity, dynamic>(sql, entity);
        }

        public async Task<JwtTenantConfigEntity> Get(Guid id)
        {
            string sql = $"SELECT * FROM JwtTenantConfig WHERE id = {id}";

            return await _db.GetData<JwtTenantConfigEntity, dynamic>(sql, new { });
        }

        public Task<List<JwtTenantConfigEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Update(Guid id, JwtTenantConfigEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(JwtTenantConfigEntity jwtConfigurationEntity)
        {
            throw new NotImplementedException();
        }
    }
}
