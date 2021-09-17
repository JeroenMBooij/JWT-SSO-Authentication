using System.IO;

namespace AuthenticationServer.Common.Models.ContractModels
{
    public class DownloadInfo
    {
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public MemoryStream Content { get; set; } = new MemoryStream();
    }
}
