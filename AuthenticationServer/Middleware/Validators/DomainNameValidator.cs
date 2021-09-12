using AuthenticationServer.Common.Models.ContractModels.Applications;
using FluentValidation;
using System;

namespace AuthenticationServer.Web.Middleware.Validators
{
    public class DomainNameValidator : AbstractValidator<Application>
    {
        public DomainNameValidator()
        {
            RuleForEach(application => application.Domains)
               .NotNull().WithMessage("{PropertyName} cannot be empty")
               .Must(domain => ValidateUrl(domain.Url))
               .WithMessage("{PropertyValue} is not a valid URL.");
        }

        private bool ValidateUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

    }
}
