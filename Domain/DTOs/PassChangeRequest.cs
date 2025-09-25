namespace Domain.DTOs
{
    public class PassChangeRequest
    {
        public string UserCode { get; set; }
        public string Password { get; set; }
        public string PassConfirm { get; set; }
    }
}
