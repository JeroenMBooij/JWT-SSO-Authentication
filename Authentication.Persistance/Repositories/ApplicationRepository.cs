using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly IMainSqlDataAccess _db;

        public ApplicationRepository(IMainSqlDataAccess _db)
        {
            this._db = _db;
        }

        public async Task<ApplicationEntity> GetApplicationFromName(string name)
        {
            string sql = $"SELECT * FROM dbo.Applications where name LIKE @Name";
            var parameters = new { Name = $"{name}%" };

            return await _db.GetData<ApplicationEntity, dynamic>(sql, parameters);
        }

        public async Task<ApplicationEntity> GetApplicationFromHostname(string url)
        {
            string sql = $"SELECT * FROM dbo.Applications where url LIKE @Url";
            var parameters = new { Url = $"{url}%" };

            return await _db.GetData<ApplicationEntity, dynamic>(sql, parameters);
        }

        public async Task<List<ApplicationEntity>> GetApplicationsFromAdminId(Guid adminId)
        {
            string sql = $"SELECT * FROM dbo.Applications where AdminId = @AdminId";
            var parameters = new { AdminId = adminId };

            return await _db.GetData<List<ApplicationEntity>, dynamic>(sql, parameters);
        }

        public async Task<AccountRole> GetAccountRoleFromEmail(string email)
        {
            string sql = $@"SELECT {nameof(ApplicationUserEntity.AuthenticationRole)} 
                            FROM dbo.Applications where {nameof(ApplicationUserEntity.Email)} = @Email";

            var parameters = new { Email = email };

            var result = await _db.GetData<string, dynamic>(sql, parameters);

            return Enum.Parse<AccountRole>(result);
        }

        public async Task<AccountRole> GetAccountRoleFromId(Guid id)
        {
            string sql = $@"SELECT {nameof(ApplicationUserEntity.AuthenticationRole)} 
                            FROM dbo.Applications where {nameof(ApplicationUserEntity.Id)} = @Id";

            var parameters = new { Id = id.ToString() };

            var result = await _db.GetData<string, dynamic>(sql, parameters);

            return Enum.Parse<AccountRole>(result);
        }

        public Task Insert(ApplicationEntity entity, string data = "")
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

        public Task Update(Guid id, ApplicationEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(ApplicationEntity entity)
        {
            throw new NotImplementedException();
        }

    }
}
