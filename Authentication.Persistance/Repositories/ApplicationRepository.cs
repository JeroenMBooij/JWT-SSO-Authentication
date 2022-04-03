using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Extentions;
using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Domain.Entities;
using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private IMainSqlDataAccess _db;
        private readonly ILogger<ApplicationRepository> _logger;

        public ApplicationRepository(IMainSqlDataAccess db, ILogger<ApplicationRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<ApplicationEntity> GetApplicationFromName(string name)
        {
            string sql = $"SELECT * FROM Applications where name LIKE @Name";
            var parameters = new { Name = $"{name}%" };

            return await _db.GetData<ApplicationEntity, dynamic>(sql, parameters);
        }

        public async Task<ApplicationEntity> GetApplicationFromHostname(string url)
        {
            string sql = $"SELECT * FROM Applications where url LIKE @Url";
            var parameters = new { Url = $"{url}%" };

            return await _db.GetData<ApplicationEntity, dynamic>(sql, parameters);
        }

        public async Task<List<ApplicationEntity>> GetApplicationsFromAdminId(Guid adminId)
        {
            string sql = $"SELECT * FROM Applications where AdminId = @AdminId";
            var parameters = new { AdminId = adminId };

            return await _db.GetData<List<ApplicationEntity>, dynamic>(sql, parameters);
        }

        public async Task<AccountRole?> GetAccountRole(string email)
        {
            string sql = $@"SELECT {nameof(ApplicationUserEntity.AuthenticationRole)} 
                            FROM ApplicationUsers where {nameof(ApplicationUserEntity.Email)} = @Email";

            var parameters = new { Email = email };

            var result = await _db.GetData<string, dynamic>(sql, parameters);

            if (result is null)
                return null;

            return Enum.Parse<AccountRole>(result);
        }

        public async Task<AccountRole?> GetAccountRole(Guid id)
        {
            string sql = $@"SELECT {nameof(ApplicationUserEntity.AuthenticationRole)} 
                            FROM {typeof(ApplicationUserEntity).GetTableName()} 
                            where {nameof(ApplicationUserEntity.Id)} = @Id";

            var parameters = new { Id = id.ToString() };

            var result = await _db.GetData<string, dynamic>(sql, parameters);

            if (result is null)
                return null;

            return Enum.Parse<AccountRole>(result);
        }

        public async Task<Guid> Insert(ApplicationEntity applicationEntity, string data = "")
        {
            string sql = $@"INSERT INTO {typeof(ApplicationEntity).GetTableName()}
                            VALUES (@ApplicationsId, @Name, @AdminId, @MultimediaUUID, @Created, @LastModified);";

            var parameters = new DynamicParameters();
            parameters.AddColumnParameters(applicationEntity);

            await _db.SaveData(sql, parameters);

            return applicationEntity.Id;
        }

        public async Task<ApplicationEntity> Get(Guid? adminId, Guid id)
        {
            string sql = $@"SELECT app.*, jwt.*, dn.* 
                            FROM {typeof(ApplicationEntity).GetTableName()} app
                            LEFT JOIN {typeof(JwtTenantConfigEntity).GetTableName()} jwt
                                ON app.{nameof(ApplicationEntity.Id)} = jwt.{nameof(JwtTenantConfigEntity.ApplicationId)}
                            LEFT JOIN {typeof(DomainNameEntity).GetTableName()} dn
                                ON app.{nameof(ApplicationEntity.Id)} = dn.{nameof(DomainNameEntity.ApplicationId)}
                            WHERE app.{nameof(ApplicationEntity.Id)} = @Id;";

            var parameters = new { Id = id };


            _logger.LogInformation(adminId.Value.ToString());
            _logger.LogInformation(JsonSerializer.Serialize(_db));

            if (_db is null)
                _logger.LogInformation("WHAT THE FUCK WHY");

            var application = await _db.GetData<ApplicationEntity,
                JwtTenantConfigEntity, DomainNameEntity, ApplicationEntity, dynamic>(sql, parameters,
                    (application, jwtConfigs, domains) =>
                    {
                        _logger.LogInformation(JsonSerializer.Serialize(application));
                        application.JwtTenantConfigurations.Add(jwtConfigs);
                        _logger.LogInformation("nope");
                        application.Domains.Add(domains);

                        return application;
                    });

            if (application.AdminId != adminId.Value)
                throw new AuthenticationApiException("Application", "UNAUTHORIZED", 403);

            return application;
        }

        public async Task<List<ApplicationEntity>> GetAll(Guid adminId)
        {
            string sql = $@"SELECT app.*, jwt.*, dn.* 
                            FROM {typeof(ApplicationEntity).GetTableName()} app
                            LEFT JOIN {typeof(JwtTenantConfigEntity).GetTableName()} jwt
                                ON app.{nameof(ApplicationEntity.Id)} = jwt.{nameof(JwtTenantConfigEntity.ApplicationId)}
                            LEFT JOIN {typeof(DomainNameEntity).GetTableName()} dn
                                ON app.{nameof(ApplicationEntity.Id)} = dn.{nameof(DomainNameEntity.ApplicationId)}
                            WHERE {nameof(ApplicationUserEntity.AdminId)} = @AdminId";

            var parameters = new { AdminId = adminId };

            var applications = await _db.GetAllData<ApplicationEntity,
                JwtTenantConfigEntity, DomainNameEntity, ApplicationEntity, dynamic>(sql, parameters,
                    (application, jwtConfigs, domains) =>
                    {
                        application.JwtTenantConfigurations.Add(jwtConfigs);
                        application.Domains.Add(domains);

                        return application;
                    });

            return applications;
        }

        public async Task Update(Guid adminId, Guid id, ApplicationEntity applicationEntity)
        {
            await Get(adminId, id);

            // TODO AUTO set LastModified
            string sql = $@"UPDATE {typeof(ApplicationEntity).GetTableName()}
                            SET 
                                {nameof(ApplicationEntity.Name)} = @Name,
                                {nameof(ApplicationEntity.MultimediaUUID)} = @MultimediaUUID,
                                {nameof(ApplicationEntity.LastModified)} = @LastModified
                            WHERE {nameof(ApplicationEntity.Id)} = @ApplicationsId;";

            var parameters = new DynamicParameters();
            parameters.AddColumnParameters(applicationEntity, nameof(ApplicationEntity.AdminId), nameof(ApplicationEntity.Created));

            await _db.SaveData(sql, parameters);
        }

        public async Task Delete(Guid adminId, Guid id)
        {
            await Get(adminId, id);

            string sql = $@"DELETE {typeof(ApplicationEntity).GetTableName()}
                            WHERE {nameof(ApplicationUserEntity.Id)} = @Id";

            var parameters = new { Id = id };

            await _db.SaveData(sql, parameters);
        }

        public async Task<Guid> GetApplicationIconUUID(Guid applicationId)
        {
            string sql = $@"SELECT {nameof(ApplicationEntity.MultimediaUUID)} 
                            FROM {typeof(ApplicationEntity).GetTableName()} 
                            WHERE {nameof(ApplicationUserEntity.Id)} = @Id";

            var parameters = new { Id = applicationId };

            var iconUUID = await _db.GetData<string, dynamic>(sql, parameters);

            return Guid.Parse(iconUUID);
        }
    }
}
