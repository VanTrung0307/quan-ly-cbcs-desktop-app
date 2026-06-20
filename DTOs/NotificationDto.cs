namespace QuanLyCBCS.DTOs
{
    public class NotificationDto
    {
        public string Type { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime Date { get; set; }
    }
}