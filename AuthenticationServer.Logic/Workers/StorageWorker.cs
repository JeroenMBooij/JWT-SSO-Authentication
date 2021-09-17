using AuthenticationServer.Common.Constants;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Workers
{
    public class StorageWorker : IStorageWorker
    {

        public async Task<Guid> UploadFile(IFormFile file)
        {
            Guid multimediaUUID = Guid.NewGuid();

            Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/UploadedFiles/{multimediaUUID}");

            using (var stream = new FileStream($"{Directory.GetCurrentDirectory()}/UploadedFiles/{multimediaUUID}/{file.FileName}".Replace("\\", "/"), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return multimediaUUID;
        }

        public async Task<DownloadInfo> DownloadFile(Guid multimediaUUID)
        {
            string[] filePaths = Directory.GetFiles($"{Directory.GetCurrentDirectory()}/UploadedFiles/{multimediaUUID}".Replace("\\", "/"));

            var downloadInfo = new DownloadInfo();
            using (var stream = new FileStream(filePaths[0], FileMode.Open))
            {
                downloadInfo.FileName = Path.GetFileName(stream.Name);
                downloadInfo.MimeType = FileConstant.GetMimeType(Path.GetExtension(downloadInfo.FileName).ToLowerInvariant());

                await stream.CopyToAsync(downloadInfo.Content);
                downloadInfo.Content.Position = 0;
            }

            return downloadInfo;
        }
    }
}
