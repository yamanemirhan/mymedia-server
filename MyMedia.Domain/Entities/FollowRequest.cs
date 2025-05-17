namespace MyMedia.Domain.Entities
{
    public class FollowRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid RequesterId { get; set; }
        public User Requester { get; set; }

        public Guid ReceiverId { get; set; }
        public User Receiver { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
