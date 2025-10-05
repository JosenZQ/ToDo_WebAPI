namespace Domain.DTOs
{
    public class VerificationCodeEmailModel
    {
        public string UserName { get; set; }
        public string VerificationCode { get; set; }
        public string MinutesLifeTime { get; set; }
    }
}
