

namespace AuthenticationServer.Common.Models.ContractModels
{
    public class NewCredentials
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
