using AuthenticationServer.Common.Models.ContractModels.Account;

namespace AuthenticationServer.Web.Middleware.Validators
{
    public class TenantValidator : AccountValidator<AccountData>
    {
        public TenantValidator()
            : base()
        {

        }
    }
}
