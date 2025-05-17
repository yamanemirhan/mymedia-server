namespace MyMedia.Application.Dtos.Response
{
    public class FollowRequestResponseDto
    {
        public Guid RequestId { get; set; }
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserMiniResponseDto User { get; set; }
    }
}
