namespace MyMedia.Application.Dtos.Response
{
    public class PostResponseDto
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<PostMediaDto> MediaItems { get; set; }
        public List<CommentResponseDto> Comments { get; set; }
        public List<Guid> LikedByUserIds { get; set; } 
        public List<Guid> SavedByUserIds { get; set; }  
        public CreatedByUserDto CreatedBy { get; set; }
        public bool IsEdited { get; set; }
    }

    public class CreatedByUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public UserAvatarMediaDto Avatar { get; set; }
    }
}
