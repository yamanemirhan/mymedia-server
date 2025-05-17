using System.ComponentModel.DataAnnotations;

namespace MyMedia.Domain.Entities
{   public class Post
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(250)]
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public bool IsEdited { get; set; } = false;
        public Guid UserId { get; set; }

        public User User { get; set; }

        // Beğeniler
        public ICollection<User> LikedByUsers { get; set; } = new List<User>();

        // Kaydeden kullanıcılar
        public ICollection<User> SavedByUsers { get; set; } = new List<User>();

        // Yorumlar
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<PostMedia> MediaItems { get; set; } = new List<PostMedia>();
    }
}
