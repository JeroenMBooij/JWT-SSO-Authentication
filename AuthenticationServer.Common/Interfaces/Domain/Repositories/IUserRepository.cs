using AuthenticationServer.Domain.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface IUserRepository
    {
        Task Insert<UserEntity>(UserEntity user);
        Task<UserEntity> Get(Guid userId);
        Task<List<UserEntity>> GetAll();
        Task Update(UserEntity user);
        Task Delete(UserEntity user);


        Task<JwtConfigurationEntity> GetUserJwtConfiguration(Guid userId);
        Task<Guid> GetUserIdFromUserEmail(string email);
        Task<string> GetDatamodelSchemaFromEmail(string email);
    }
}
