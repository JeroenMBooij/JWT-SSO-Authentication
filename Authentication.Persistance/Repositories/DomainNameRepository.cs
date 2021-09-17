using AuthenticationServer.Common.Extentions;
using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Domain.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public class DomainNameRepository : IDomainNameRepository
    {
        public IMainSqlDataAccess Database { get; private set; }

        public DomainNameRepository(IMainSqlDataAccess database)
        {
            Database = database;
        }

        public async Task<Guid> Insert(DomainNameEntity domainNameEntity, string data = "")
        {
            string sql = $@"INSERT INTO dbo.{typeof(DomainNameEntity).GetTableName()}
                            VALUES (@DomainNameId, @Name, @Url, @ApplicationId, @Created, @LastModified);";

            var parameters = new DynamicParameters();
            parameters.AddColumnParameters(domainNameEntity, null);

            await Database.SaveData(sql, parameters);

            return domainNameEntity.Id;
        }

        public Task<DomainNameEntity> Get(Guid? adminId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DomainNameEntity>> GetAll(Guid adminId)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Guid adminId, Guid id, DomainNameEntity domainNameEntity)
        {
            string sql = $@"UPDATE dbo.{typeof(DomainNameEntity).GetTableName()}
                            SET Name = @Name, 
                            Url = @Url,
                            LastModified = @LastModified;
                        WHERE Id = @ DomainNameId";

            var parameters = new DynamicParameters();
            parameters.AddColumnParameters(domainNameEntity, nameof(DomainNameEntity.ApplicationId), nameof(DomainNameEntity.Created));

            await Database.SaveData(sql, parameters);
        }

        public Task Delete(Guid adminId, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
