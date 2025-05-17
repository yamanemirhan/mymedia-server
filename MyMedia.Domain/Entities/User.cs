using System.ComponentModel.DataAnnotations;

namespace MyMedia.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)] 
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public UserAvatarMedia Avatar { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsPrivate { get; set; } = false;

        // Kullanıcının kendi postları
        public ICollection<Post> Posts { get; set; } = new List<Post>();

        // Beğendiği postlar
        public ICollection<Post> LikedPosts { get; set; } = new List<Post>();

        // Kaydettiği postlar
        public ICollection<Post> SavedPosts { get; set; } = new List<Post>();

        // Yorumları
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<CommentLike> CommentLikes { get; set; }
        public List<User> Followers { get; set; } = new();
        public List<User> Followings { get; set; } = new();

        // Takip istekleri
        public ICollection<FollowRequest> IncomingFollowRequests { get; set; } = new List<FollowRequest>();
        public ICollection<FollowRequest> OutgoingFollowRequests { get; set; } = new List<FollowRequest>();
    }
}
