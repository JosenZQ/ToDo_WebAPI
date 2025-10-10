namespace Domain.DTOs
{
    public class UserWithVerificationCode
    {
        public string UserCode { get; set; }
        public bool Verified { get; set; }
    }
}
