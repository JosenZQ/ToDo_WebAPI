namespace Domain.DTOs
{
    public class UserActionFormat
    {
        public string UserCode { get; set; } = null!;
        public string ActionCode { get; set; } = null!;
        public string ActionDescr { get; set; } = null!;
        public DateTime ActionDate { get; set; }
    }
}
