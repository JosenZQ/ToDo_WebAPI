namespace Domain.DTOs
{
    public class PassResetRequest
    {
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PassConfirm { get; set; }
    }
}
