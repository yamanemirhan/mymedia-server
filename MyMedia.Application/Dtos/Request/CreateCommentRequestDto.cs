namespace MyMedia.Application.Dtos.Request
{
    public class CreateCommentRequestDto
    {
        public string Text { get; set; } = string.Empty;
        public Guid PostId { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
