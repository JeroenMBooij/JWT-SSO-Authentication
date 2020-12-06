using AuthenticationServer.Common.Interfaces.Domain;
using AuthenticationServer.Common.Models.ResponseModels;
using AuthenticationServer.Common.Models.ResponseModels.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public class JwtConfigurationRepository : IJwtConfigurationRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IConfiguration _config;

        public JwtConfigurationRepository(ISqlDataAccess _db, IConfiguration config)
        {
            this._db = _db;
            _config = config;
        }

        public Task CreateJwtConfiguration(JwtConfiguration tenant, int domainId)
        {
            throw new NotImplementedException();
        }

        public Task Delete<T>(T data)
        {
            throw new NotImplementedException();
        }

        public async Task<JwtConfiguration> Get<JwtConfiguration>(string id)
        {
            string sql = $"SELECT * FROM jwt_configuration WHERE id = {id}";

            return await _db.GetData<JwtConfiguration, dynamic>(sql, new { });
        }

        public Task<List<JwtConfiguration>> GetAll<JwtConfiguration>()
        {
            throw new NotImplementedException();
        }

        public JwtConfiguration GetJwtConfigurationFromAccount(Account account)
        {
            JwtConfiguration jwtContainerModel = _config.GetSection("JWTSecurity").Get<JwtConfiguration>();
            jwtContainerModel.Claims = account.Claims;

            return jwtContainerModel;
        }

        public Task<JwtConfiguration> GetJwtConfigurationFromUser(Account account)
        {
            throw new NotImplementedException();
        }

        public Task Insert<JwtConfiguration>(JwtConfiguration user)
        {
            string sql = @"insert into dbo.Users (Name, Email)
                            values (@Name, @Email);";

            return _db.SaveData(sql, user);
        }

        public Task Update<T>(T data)
        {
            throw new NotImplementedException();
        }
    }
}
