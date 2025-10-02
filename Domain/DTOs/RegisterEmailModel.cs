namespace Domain.DTOs
{
    public class RegisterEmailModel
    {
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string RegisterDate { get; set; }
        public string TimeZoneName { get; set; }
        public string TimeZoneCurrentTime { get; set; }

    }
}
