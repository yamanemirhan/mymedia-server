namespace MyMedia.Domain.Entities
{
    public class UserAvatarMedia
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string AvatarUrl { get; set; } = "https://bu4gkqk43wetzryg.public.blob.vercel-storage.com/def-avatar-S4NnHLL63VypWynP8rLED2sUCNNoBi.jpg";

        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
