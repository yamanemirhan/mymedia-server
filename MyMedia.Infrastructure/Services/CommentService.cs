using Microsoft.EntityFrameworkCore;
using MyMedia.Application.Interfaces;
using MyMedia.Domain.Entities;
using MyMedia.Infrastructure.Data;
using MyMedia.Shared.GlobalErrorHandling;

namespace MyMedia.Infrastructure.Services
{
    public class CommentService : ICommentService
    {

        private readonly AppDbContext _context;

        public CommentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateCommentAsync(Guid userId, string text, Guid postId, Guid? parentCommentId, CancellationToken cancellationToken)
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Text = text,
                PostId = postId,
                ParentCommentId = parentCommentId,
                UserId = userId,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync(cancellationToken);

            return comment;
        }

        public async Task<List<Comment>> GetCommentsByPostIdAsync(Guid postId, CancellationToken cancellationToken)
        {
            var comments = await _context.Comments
                .Include(c => c.Likes)
                .Include(c => c.User)
                    .ThenInclude(u => u.Avatar)
                .Where(c => c.PostId == postId && c.ParentCommentId == null)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync(cancellationToken);

            foreach (var comment in comments)
            {
                await LoadRepliesRecursive(comment, cancellationToken);
            }

            return comments;
        }

        private async Task LoadRepliesRecursive(Comment comment, CancellationToken cancellationToken)
        {
            await _context.Entry(comment)
                .Collection(c => c.Replies)
                .Query()
                .Include(r => r.User)
                    .ThenInclude(u => u.Avatar)
                .LoadAsync(cancellationToken);

            foreach (var reply in comment.Replies)
            {
                await LoadRepliesRecursive(reply, cancellationToken);
            }
        }

        public async Task ToggleLikeCommentAsync(Guid commentId, Guid userId, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments
                .Include(c => c.Likes)
                .FirstOrDefaultAsync(c => c.Id == commentId, cancellationToken)
                ?? throw new CustomException("Comment not found.", 404);

            var existingLike = comment.Likes.FirstOrDefault(like => like.UserId == userId);

            if (existingLike != null)
            {
                comment.Likes.Remove(existingLike);
            }
            else
            {
                comment.Likes.Add(new CommentLike
                {
                    CommentId = commentId,
                    UserId = userId
                });
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Comment?> GetCommentByIdAsync(Guid commentId)
        {
            return await _context.Comments.FindAsync(commentId);
        }

        public async Task DeleteCommentByIdAsync(Guid commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment is null) return;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetAllCommentsAsync(CancellationToken cancellationToken)
        {
            return await _context.Comments.ToListAsync(cancellationToken);
        }
    }
}
