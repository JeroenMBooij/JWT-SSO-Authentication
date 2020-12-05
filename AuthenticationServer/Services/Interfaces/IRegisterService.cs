using AuthenticationServer.Models;
using AuthenticationServer.Models.ViewModels;

namespace AuthenticationServer.Services.Interfaces
{
    public interface IRegisterService
    {
        string RegisterUser(UserVM user);
    }
}