using AuthenticationServer.Common.Models.ContractModels.Applications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface IApplicationManager
    {
        Task<Guid> CreateApplication(string token, Application application);
        Task<ApplicationWithId> GetApplication(string token, Guid id);
        Task<List<ApplicationWithId>> GetApplications(string token);
        Task UpdateApplication(string token, Guid id, Application application);
        Task DeleteApplication(string token, Guid id);
        Task<Guid> GetApplicationIconUUID(Guid applicationId);
    }
}