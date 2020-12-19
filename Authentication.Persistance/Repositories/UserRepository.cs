using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMainSqlDataAccess _db;

        public UserRepository(IMainSqlDataAccess _db)
        {
            this._db = _db;
        }

        public Task Delete(UserEntity data)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<JwtConfigurationEntity> GetUserJwtConfiguration(UserEntity userEntity)
        {
            string sql = $"SELECT * FROM jwt_configuration WHERE id = -1";//{userEntity.Domains;

            JwtConfigurationEntity jwtConfigurationDTO = await _db.GetData<JwtConfigurationEntity, dynamic>(sql, new { });

            return jwtConfigurationDTO;

        }

        public Task<JwtConfigurationEntity> GetUserJwtConfiguration(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task Insert<UserEntity>(UserEntity user)
        {
            string sql = @"INSERT INTO dbo.Users (Email, HashedPassword, Firstname, Middlename, Lastname)
                            values (@Email, @HashedPassword, @Firstname, @Middlename, @Lastname);";

            return _db.SaveData(sql, user);
        }

        public Task Update(UserEntity data)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDatamodelSchemaFromEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> GetUserIdFromUserEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<UserEntity> Get(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
