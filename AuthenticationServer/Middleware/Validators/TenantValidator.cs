using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Models.ContractModels.Account;

namespace AuthenticationServer.Web.Middleware.Validators
{
    public class TenantValidator : AccountValidator<AccountRegistration>
    {
        public TenantValidator(ILanguageRepository languageRepository)
            : base(languageRepository)
        {

        }
    }
}
