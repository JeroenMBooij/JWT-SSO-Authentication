

using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface IEmailService
    {
        public Task VerifyTenantEmail(string code);
        public Task VerifyUserEmail(string code);
    }
}
