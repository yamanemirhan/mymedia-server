using MyMedia.Domain.Enums;

namespace MyMedia.Domain.Entities
{
    public class PostMedia
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string MediaUrl { get; set; } = string.Empty;
        public MediaTypeEnum MediaType { get; set; }
        public int Order { get; set; } = 0;

        public Guid PostId { get; set; }

        public Post Post { get; set; }
    }
}
