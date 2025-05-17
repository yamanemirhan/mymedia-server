namespace MyMedia.Application.Dtos.Request.User
{
    public class UpdateUserRequest
    {
        public string Username { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public string? Description { get; set; }
        public bool IsPrivate { get; set; }
    }
}
