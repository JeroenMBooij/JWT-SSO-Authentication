using AuthenticationServer.Common.Models.ContractModels;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface IStorageService
    {
        Task<Guid> UploadFile(IFormFile file);
        Task<DownloadInfo> DownloadFile(Guid multimediaUUID);
    }
}