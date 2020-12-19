using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly IMainSqlDataAccess _db;

        public LanguageRepository(IMainSqlDataAccess _db)
        {
            this._db = _db;
        }

        public Task Delete(LanguageEntity tenant)
        {
            throw new NotImplementedException();
        }

        public Task<LanguageEntity> Get(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<LanguageEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<LanguageEntity> GetLanguageByName(string languageName)
        {
            var p = new
            {
                LanguageName = languageName
            };

            string sql = $"SELECT * FROM languages WHERE name = @LanguageName";

            LanguageEntity languageEntity = await _db.GetData<LanguageEntity, dynamic>(sql, p);

            return languageEntity;
        }

        public Task Insert(LanguageEntity tenant)
        {
            throw new NotImplementedException();
        }

        public Task Update(LanguageEntity tenant)
        {
            throw new NotImplementedException();
        }
    }
}
