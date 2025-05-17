using Microsoft.EntityFrameworkCore;
using MyMedia.Application.Interfaces;
using MyMedia.Domain.Entities;
using MyMedia.Infrastructure.Data;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _context;

        public PostService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreatePostAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.MediaItems)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Likes)
                .Include(p => p.LikedByUsers)
                .Include(p => p.SavedByUsers)
                .Include(p => p.User)
                    .ThenInclude(u => u.Avatar)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Post?> GetPostByIdAsync(Guid id)
        {
            return await _context.Posts
                .Include(p => p.User)
                    .ThenInclude(u => u.Avatar)
                .Include(p => p.MediaItems)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Likes)
                .Include(p => p.LikedByUsers)
                .Include(p => p.SavedByUsers)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Post>> GetPostsByUserIdAsync(Guid userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                    .ThenInclude(u => u.Avatar)
                .Include(p => p.MediaItems)
                .Include(p => p.Comments)
                   .ThenInclude(c => c.Likes)
                .Include(p => p.LikedByUsers)
                .Include(p => p.SavedByUsers)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Post>> GetSavedPostsByUserIdAsync(Guid userId)
        {
            return await _context.Posts
                .Where(p => p.SavedByUsers.Any(u => u.Id == userId))
                 .Include(p => p.User)
                    .ThenInclude(u => u.Avatar)
                .Include(p => p.MediaItems)
                .Include(p => p.Comments)
                      .ThenInclude(c => c.Likes)
                .Include(p => p.LikedByUsers)
                .Include(p => p.SavedByUsers)
                .ToListAsync();
        }

        public async Task DeletePostByIdAsync(Guid postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post is null)
                throw new CustomException("Post not found.", 404);

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}
