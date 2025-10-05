namespace Domain.DTOs
{
    public class UserLocalVerificationCode
    {
        public string UserCode { get; set; }
        public string VerificationCode { get; set; }
        public DateTime GenerationDateTime { get; set; }
        public int MinutesLifeTime { get; set; }
    }
}
