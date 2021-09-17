

using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Logic.Managers
{
    public interface IEmailService
    {
        Task SendVerificationEmail(string recipient, Guid code);
        Task SendRecoverPasswordEmail(string recipient, string passwordRecoverToken);
    }
}
