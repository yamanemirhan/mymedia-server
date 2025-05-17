namespace MyMedia.Application.Dtos.Response
{
    public class FollowingResponseDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string? Description { get; set; }
        public UserAvatarMediaDto Avatar { get; set; }
        public bool IsFollowedByCurrentUser { get; set; }
    }
}
