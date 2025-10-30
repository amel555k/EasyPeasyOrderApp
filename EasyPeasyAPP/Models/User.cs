namespace EasyPeasyAPP.Models
{
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}