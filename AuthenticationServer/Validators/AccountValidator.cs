using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Models.ContractModels.Common;
using AuthenticationServer.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Validators
{
    public class AccountValidator<T> : AbstractValidator<T> where T : AbstractAccount
    {
        private readonly ILanguageRepository _languageRepository;

        public AccountValidator(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;

            RuleForEach(account => account.Languages)
                .NotNull().WithMessage("{PropertyName} cannot be empty")
                .MustAsync(async (language, cancellation) => await ValidateLanguageAsync(language))
                .WithMessage("{CollectionIndex} is not a language we provide yet. Contact your system adminstrator to add this language to the authentication server.");
        }

        private async Task<bool> ValidateLanguageAsync(string languageName)
        {
            LanguageEntity languageEntity = await _languageRepository.GetLanguageByName(languageName);
            if (languageEntity == null)
                return false;
            
            return true;
        }
    }
}
