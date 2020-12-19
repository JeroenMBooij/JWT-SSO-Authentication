using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Models.ContractModels;

namespace AuthenticationServer.Web.Validators
{
    public class TenantValidator : AccountValidator<Tenant>
    {
        public TenantValidator(ILanguageRepository languageRepository)
            :base(languageRepository)
        {

        }
    }
}
