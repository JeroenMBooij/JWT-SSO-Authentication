using AuthenticationServer.Common.Constants;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IStorageWorker _storageService;
        private readonly IApplicationManager _applicationService;

        public StorageController(IStorageWorker storageService, IApplicationManager applicationService)
        {
            _storageService = storageService;
            _applicationService = applicationService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<Guid> UploadFile([FromForm] IFormFile file)
        {
            return await _storageService.UploadFile(file);
        }

        /// <remarks>
        /// Get an image as a file stream
        /// </remarks>
        /// <param name="multimediaUUID">Guid of the file you want to download</param>
        /// <response code="200"></response>
        [HttpGet]
        [Route("{multimediaUUID}")]
        public async Task<IActionResult> DownloadFile(Guid multimediaUUID)
        {
            DownloadInfo downloadInfo = await _storageService.DownloadFile(multimediaUUID);

            return File(downloadInfo.Content, downloadInfo.MimeType, downloadInfo.FileName);
        }

        [HttpGet]
        [Route("application/{applicationId}/icon")]
        public async Task<Guid> GetApplicationIconUUID(Guid applicationId)
        {
            return await _applicationService.GetApplicationIconUUID(applicationId);
        }
    }
}
