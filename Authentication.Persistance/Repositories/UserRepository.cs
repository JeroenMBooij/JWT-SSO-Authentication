using AuthenticationServer.Common.Interfaces.Domain;
using AuthenticationServer.Common.Interfaces.Domain.Repository;
using AuthenticationServer.Common.Models.ResponseModels;
using AuthenticationServer.Common.Models.ResponseModels.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Persistance.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ISqlDataAccess _db;

        public UserRepository(ISqlDataAccess _db)
        {
            this._db = _db;
        }

        public Task<JwtConfiguration> CreateEntity<Account>(Account account)
        {
            throw new NotImplementedException();
        }

        public Task<JwtConfiguration> CreateTenant(Tenant tenant)
        {
            throw new NotImplementedException();
        }

        public Task<JwtConfiguration> CreateUser<User>(User data)
        {
            throw new NotImplementedException();
        }

        public Task Delete<T>(T data)
        {
            throw new NotImplementedException();
        }

        public Task<T> Get<T>(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDTO>> GetAll<UserDTO>()
        {
            throw new NotImplementedException();
        }

        public Task Insert<UserDTO>(UserDTO user)
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
