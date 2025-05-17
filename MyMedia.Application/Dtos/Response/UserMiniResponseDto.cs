namespace MyMedia.Application.Dtos.Response
{
    public class UserMiniResponseDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public UserAvatarMediaDto Avatar { get; set; }
    }
}
