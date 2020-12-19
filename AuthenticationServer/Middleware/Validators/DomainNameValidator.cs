using AuthenticationServer.Common.Models.ContractModels;
using FluentValidation;
using System;

namespace AuthenticationServer.Web.Middleware.Validators
{
    public class DomainNameValidator : AbstractValidator<DomainName>
    {
        public DomainNameValidator()
        {
            RuleFor(domain => domain.Url)
                .Must(url => ValidateUrl(url)).WithMessage("{PropertyValue} is not a valid URL.");

            RuleFor(domain => domain.Logo)
                .Must(logo => ValidateBase64String(logo)).WithMessage("Invalid Base64 string provided for Logo.");
        }

        private bool ValidateUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private bool ValidateBase64String(string base64Logo)
        {
            if (base64Logo != null)
            {
                //https://stackoverflow.com/questions/6309379/how-to-check-for-a-valid-base64-encoded-string
                Span<byte> buffer = new Span<byte>(new byte[base64Logo.Length]);
                return Convert.TryFromBase64String(base64Logo, buffer, out int _);
            }

            return true;
        }
    }
}
