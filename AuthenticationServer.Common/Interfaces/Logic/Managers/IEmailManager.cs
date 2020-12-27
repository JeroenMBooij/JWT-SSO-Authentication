

using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Logic.Managers
{
    public interface IEmailManager
    {
        Task SendVerificationEmail(string recipient, Guid code);
    }
}
