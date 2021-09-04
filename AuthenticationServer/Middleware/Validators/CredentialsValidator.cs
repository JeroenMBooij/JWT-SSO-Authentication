using AuthenticationServer.Common.Models.ContractModels;
using FluentValidation;

namespace AuthenticationServer.Web.Middleware.Validators
{
    public class CredentialsValidator : AbstractValidator<Credentials>
    {
        public CredentialsValidator()
        {
            RuleFor(account => account.Email)
            .EmailAddress()
            .WithMessage("{PropertyValue} is not a valid email address.");
        }
    }
}
