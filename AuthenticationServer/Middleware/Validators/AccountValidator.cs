using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Domain.Entities;
using FluentValidation;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Middleware.Validators
{
    public class AccountValidator<T> : AbstractValidator<T> where T : AccountData
    {

        public AccountValidator()
        {
            RuleFor(account => account.Email)
            .EmailAddress()
            .WithMessage("A valid email address is required.");
        }
    }
}
