using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public class JwtConfigurationRepository : IJwtConfigurationRepository
    {
        private readonly IMainSqlDataAccess _db;
        private readonly IConfiguration _config;

        public JwtConfigurationRepository(IMainSqlDataAccess _db, IConfiguration config)
        {
            this._db = _db;
            _config = config;
        }

        public Task CreateJwtConfiguration(JwtConfigurationEntity jwtConfigurationEntity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(JwtConfigurationEntity jwtConfigurationEntity)
        {
            throw new NotImplementedException();
        }

        public async Task<JwtConfigurationEntity> Get(string id)
        {
            string sql = $"SELECT * FROM jwt_configuration WHERE id = {id}";

            return await _db.GetData<JwtConfigurationEntity, dynamic>(sql, new { });
        }

        public Task<List<JwtConfigurationEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<JwtConfigurationEntity> GetJWTContainerModel(int currentDomainId)
        {
            throw new NotImplementedException();
        }

        public Task Insert(JwtConfigurationEntity user)
        {
            string sql = @"insert into dbo.Users (Name, Email)
                            values (@Name, @Email);";

            return _db.SaveData(sql, user);
        }

        public Task Update(JwtConfigurationEntity jwtConfigurationEntity)
        {
            throw new NotImplementedException();
        }
    }
}
