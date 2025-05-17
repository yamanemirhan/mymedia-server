namespace MyMedia.Application.Dtos.Response
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserAvatarMediaDto Avatar { get; set; }

        // Eklenen Alanlar
        public DateTime? UpdatedAt { get; set; }

        public int PostCount { get; set; }
        public int LikedPostCount { get; set; }
        public int SavedPostCount { get; set; }
        public int CommentCount { get; set; }
        public int CommentLikeCount { get; set; }

        public string? Description { get; set; }

        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }

        public bool IsFollowedByCurrentUser { get; set; }
        public bool HasPendingFollowRequestFromCurrentUser { get; set; }
        public bool HasSentFollowRequestToCurrentUser { get; set; }
        public bool IsFollowingCurrentUser { get; set; }       
    }
}