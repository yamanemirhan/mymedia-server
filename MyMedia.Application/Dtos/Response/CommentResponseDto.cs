namespace MyMedia.Application.Dtos.Response
{
    public class CommentResponseDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public UserResponseDto User { get; set; }
        public List<CommentResponseDto>? Replies { get; set; }
        public List<CommentLikeResponseDto> Likes { get; set; } = new();
        public bool IsEdited { get; set; }
    }
}
