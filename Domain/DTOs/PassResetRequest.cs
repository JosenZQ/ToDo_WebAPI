namespace Domain.DTOs
{
    public class PassResetRequest
    {
        public string UserCode { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string PassConfirm { get; set; }
        public string UserPhone { get; set; }
    }
}
