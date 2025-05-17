using Microsoft.EntityFrameworkCore;
using MyMedia.Domain.Entities;
using MyMedia.Domain.Enums;

namespace MyMedia.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostMedia> PostMedias { get; set; }
        public DbSet<UserAvatarMedia> UserAvatarMedias { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<FollowRequest> FollowRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Post - User
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Post - Likes
            modelBuilder.Entity<Post>()
                .HasMany(p => p.LikedByUsers)
                .WithMany(u => u.LikedPosts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostLikes",
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("LikedByUsersId")
                        .OnDelete(DeleteBehavior.Restrict), 
                    j => j
                        .HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("LikedPostsId")
                        .OnDelete(DeleteBehavior.Cascade)
                );

            // Post - Saves
            modelBuilder.Entity<Post>()
                .HasMany(p => p.SavedByUsers)
                .WithMany(u => u.SavedPosts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostSaves",
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("SavedByUsersId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("SavedPostsId")
                        .OnDelete(DeleteBehavior.Cascade)
                );


            // Comment - User
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment - Post
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // PostMedia - Post
            modelBuilder.Entity<PostMedia>()
                .HasOne(pm => pm.Post)
                .WithMany(p => p.MediaItems)
                .HasForeignKey(pm => pm.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Enum: MediaType as string
            modelBuilder.Entity<PostMedia>()
                .Property(pm => pm.MediaType)
                .HasConversion(
                    v => v.ToString(),
                    v => (MediaTypeEnum)Enum.Parse(typeof(MediaTypeEnum), v)
                );

            modelBuilder.Entity<User>()
                .HasOne(u => u.Avatar)
                .WithOne(a => a.User)
                .HasForeignKey<UserAvatarMedia>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // CommentLike - User
            modelBuilder.Entity<CommentLike>()
                .HasOne(cl => cl.User)
                .WithMany(u => u.CommentLikes)
                .HasForeignKey(cl => cl.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

            // CommentLike - Comment
            modelBuilder.Entity<CommentLike>()
                .HasOne(cl => cl.Comment)
                .WithMany(c => c.Likes)
                .HasForeignKey(cl => cl.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - Followers / Followings
            modelBuilder.Entity<User>()
                .HasMany(u => u.Followers)
                .WithMany(u => u.Followings)
                .UsingEntity<Dictionary<string, object>>(
                    "UserFollows",
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Cascade)
                );

            // FollowRequest ilişkileri
            modelBuilder.Entity<FollowRequest>()
    .HasOne(fr => fr.Requester)
    .WithMany(u => u.OutgoingFollowRequests)
    .HasForeignKey(fr => fr.RequesterId)
    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FollowRequest>()
                .HasOne(fr => fr.Receiver)
                .WithMany(u => u.IncomingFollowRequests)
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FollowRequest>()
                .HasIndex(fr => new { fr.RequesterId, fr.ReceiverId })
                .IsUnique();
        }
    }
}
