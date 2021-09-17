namespace AuthenticationServer.Common.Models.ContractModels
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
    }
}
