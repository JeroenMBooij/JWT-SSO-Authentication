using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Models.ContractModels;
using FluentValidation;
using FluentValidation.Validators;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Middleware.Validators
{
    public class UserValidator : AccountValidator<User>
    {
        private readonly IUserRepository _userRepository;

        public UserValidator(IUserRepository userRepository, ILanguageRepository languageRepository)
            : base(languageRepository)
        {
            _userRepository = userRepository;

            RuleFor(user => new { user.DataModel, user.Email })
                .CustomAsync(async (x, context, cancellation) => await ValidateDataModel(context, x.DataModel, x.Email));
        }


        private async Task ValidateDataModel(CustomContext context, JToken dataModel, string email)
        {
            JSchema datamodelSchema = JSchema.Parse(await _userRepository.GetDatamodelSchemaFromEmail(email));

            dataModel.IsValid(datamodelSchema, out IList<string> messages);

            if (messages.Count > 0)
            {
                context.AddFailure("The User Datamodel contains errors.");
                foreach (string message in messages)
                    context.AddFailure(message);

                context.AddFailure("Double check your JSON model for mistakes or update your user schema to accomodate this JSON model");
            }

        }




    }
}
