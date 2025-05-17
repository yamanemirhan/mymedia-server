namespace MyMedia.Application.Dtos.Request
{
    public class CreatePostRequestDto
    {
        public string? Content { get; set; }
        public List<PostMediaDto> MediaItems { get; set; } = new();
    }
}
