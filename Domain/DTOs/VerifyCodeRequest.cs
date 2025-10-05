namespace Domain.DTOs
{
    public class VerifyCodeRequest
    {
        public string UserCode { get; set; }
        public string VerificationCode { get; set; }
    }
}
