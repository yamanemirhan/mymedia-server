using MyMedia.Application.Dtos.Response;
using MyMedia.Domain.Entities;

namespace MyMedia.Application.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> CreateCommentAsync(Guid userId, string text, Guid postId, Guid? parentCommentId, CancellationToken cancellationToken);
        Task<List<Comment>> GetCommentsByPostIdAsync(Guid postId, CancellationToken cancellationToken);
        Task ToggleLikeCommentAsync(Guid commentId, Guid userId, CancellationToken cancellationToken);
        Task<Comment?> GetCommentByIdAsync(Guid commentId);
        Task DeleteCommentByIdAsync(Guid commentId);
        Task UpdateAsync(Comment comment);
        Task<List<Comment>> GetAllCommentsAsync(CancellationToken cancellationToken);
    }
}
