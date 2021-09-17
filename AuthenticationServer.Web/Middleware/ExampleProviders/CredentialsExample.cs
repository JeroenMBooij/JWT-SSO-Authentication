using AuthenticationServer.Common.Models.ContractModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Filters;

namespace AuthenticationServer.Web.Middleware.ExampleProviders
{
    public class CredentialsExample : IExamplesProvider<Credentials>
    {
        private readonly IWebHostEnvironment _env;


        public CredentialsExample(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Credentials GetExamples()
        {
            if (_env.IsDevelopment())
            {
                return new Credentials() { Email = "jmbooij.a@gmail.com", Password = "string" };
            }
            else
            {
                return new Credentials() { Email = "string", Password = "string" };
            }
        }
    }
}
