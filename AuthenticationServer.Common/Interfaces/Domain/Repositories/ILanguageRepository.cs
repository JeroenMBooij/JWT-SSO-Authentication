using AuthenticationServer.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface ILanguageRepository
    {
        Task Insert(LanguageEntity tenant);
        Task<LanguageEntity> Get(string Id);
        Task<List<LanguageEntity>> GetAll();
        Task Update(LanguageEntity tenant);
        Task Delete(LanguageEntity tenant);
        Task<LanguageEntity> GetLanguageByName(string languageName);
    }
}
