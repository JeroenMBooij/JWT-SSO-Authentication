using AuthenticationServer.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface ILanguageRepository :IRepository<LanguageEntity>
    {
        Task<LanguageEntity> GetLanguageByName(string languageName);
    }
}
