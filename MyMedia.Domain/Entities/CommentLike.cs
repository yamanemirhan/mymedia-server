namespace MyMedia.Domain.Entities
{
    public class CommentLike
    {
        public Guid Id { get; set; }

        public Guid CommentId { get; set; }
        public Comment Comment { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public DateTime LikedAt { get; set; } = DateTime.Now;
    }

}
