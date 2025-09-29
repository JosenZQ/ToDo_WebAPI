namespace Domain.DTOs
{
    public class UserTaskFormat
    {
        public string UserCode { get; set; } = null!;

        public string CategoryCode { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime Deadline { get; set; }

        public string State { get; set; } = null!;
    }
}
